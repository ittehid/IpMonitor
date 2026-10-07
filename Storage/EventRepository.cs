using System.Globalization;
using System.Text;
using System.Text.Json;
using IpMonitor.Models;

namespace IpMonitor.Storage;

internal sealed class EventRepository
{
    private readonly object _sync = new();

    public void Append(MonitorEvent item)
    {
        AppPaths.EnsureCreated();
        lock (_sync)
        {
            File.AppendAllText(AppPaths.EventsFile,
                JsonSerializer.Serialize(item, JsonOptions.Compact) + Environment.NewLine,
                Encoding.UTF8);
        }
    }

    public List<MonitorEvent> LoadAll()
    {
        AppPaths.EnsureCreated();
        if (!File.Exists(AppPaths.EventsFile))
            return [];

        var result = new List<MonitorEvent>();
        lock (_sync)
        {
            foreach (var line in File.ReadLines(AppPaths.EventsFile, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var item = JsonSerializer.Deserialize<MonitorEvent>(line, JsonOptions.Compact);
                    if (item is not null)
                        result.Add(item);
                }
                catch
                {
                    // Повреждённая строка не должна ломать весь журнал.
                }
            }
        }

        return result.OrderByDescending(x => x.Timestamp).ToList();
    }

    /// <summary>
    /// Преобразует технические события Offline/Online в понятные пользователю периоды недоступности.
    /// Одна строка журнала = одно падение от момента потери связи до восстановления.
    /// </summary>
    public List<OutageRecord> LoadOutages()
    {
        var events = LoadAll().OrderBy(x => x.Timestamp).ToList();
        var result = new List<OutageRecord>();
        var open = new Dictionary<Guid, OutageRecord>();

        foreach (var item in events)
        {
            if (item.Type == MonitorEventType.Offline)
            {
                // Защита от старых/случайных дублей Offline: пока период не закрыт,
                // второй Offline не создаёт второй период.
                if (open.ContainsKey(item.HostId))
                    continue;

                var outage = new OutageRecord
                {
                    OfflineEventId = item.Id,
                    HostId = item.HostId,
                    HostName = item.HostName,
                    Address = item.Address,
                    LostAt = item.Timestamp,
                    InitialDetection = item.InitialDetection
                };

                open[item.HostId] = outage;
                result.Add(outage);
                continue;
            }

            if (open.TryGetValue(item.HostId, out var current))
            {
                current.RecoveredAt = item.Timestamp;
                current.RecoveryEventId = item.Id;
                // Имя/адрес могли быть изменены во время падения — в журнале оставляем
                // актуальные значения на момент восстановления, если они присутствуют.
                if (!string.IsNullOrWhiteSpace(item.HostName))
                    current.HostName = item.HostName;
                if (!string.IsNullOrWhiteSpace(item.Address))
                    current.Address = item.Address;
                open.Remove(item.HostId);
            }
            else if (item.Downtime is TimeSpan downtime && downtime > TimeSpan.Zero)
            {
                // Совместимость с очищенными/урезанными журналами: если сохранилось
                // только событие Online, точное начало можно восстановить из Downtime.
                result.Add(new OutageRecord
                {
                    RecoveryEventId = item.Id,
                    HostId = item.HostId,
                    HostName = item.HostName,
                    Address = item.Address,
                    LostAt = item.Timestamp - downtime,
                    RecoveredAt = item.Timestamp
                });
            }
        }

        return result.OrderByDescending(x => x.LostAt).ToList();
    }


    /// <summary>
    /// Удаляет один выбранный завершённый период недоступности.
    /// Активный период удалять нельзя: его начало нужно сохранить до восстановления связи.
    /// </summary>
    public bool DeleteOutage(OutageRecord outage)
    {
        if (outage.IsOngoing)
            return false;

        var ids = new HashSet<Guid>();
        if (outage.OfflineEventId != Guid.Empty)
            ids.Add(outage.OfflineEventId);
        if (outage.RecoveryEventId != Guid.Empty)
            ids.Add(outage.RecoveryEventId);

        if (ids.Count == 0)
            return false;

        var all = LoadAll().OrderBy(x => x.Timestamp).ToList();
        var keep = all.Where(x => !ids.Contains(x.Id)).OrderBy(x => x.Timestamp).ToList();
        if (keep.Count == all.Count)
            return false;

        Rewrite(keep);
        return true;
    }

    /// <summary>
    /// Очищает завершённую историю, но не удаляет текущие незавершённые падения.
    /// </summary>
    public void Clear()
    {
        var all = LoadAll().OrderBy(x => x.Timestamp).ToList();
        var openOffline = FindOpenOfflineEvents(all);
        Rewrite(openOffline.Values.OrderBy(x => x.Timestamp));
    }

    public void Prune(int retentionDays)
    {
        if (retentionDays <= 0)
            return;

        var border = DateTimeOffset.Now.AddDays(-retentionDays);
        var all = LoadAll().OrderBy(x => x.Timestamp).ToList();
        var openOffline = FindOpenOfflineEvents(all);
        var openIds = openOffline.Values.Select(x => x.Id).ToHashSet();

        // Старые завершённые события удаляются, но начало любого текущего Offline
        // сохраняется независимо от возраста, иначе потеряется точное время падения.
        var keep = all
            .Where(x => x.Timestamp >= border || openIds.Contains(x.Id))
            .OrderBy(x => x.Timestamp)
            .ToList();

        Rewrite(keep);
    }

    public void ExportCsv(string path, IEnumerable<OutageRecord> outages)
    {
        static string Esc(string value) => "\"" + value.Replace("\"", "\"\"") + "\"";

        var now = DateTimeOffset.Now;
        using var writer = new StreamWriter(path, false, new UTF8Encoding(true));
        writer.WriteLine("Хост;Адрес;Потеря связи;Восстановление;Простой;Примечание");

        foreach (var item in outages.OrderByDescending(x => x.LostAt))
        {
            var recovered = item.RecoveredAt is DateTimeOffset recoveredAt
                ? recoveredAt.LocalDateTime.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                : "Нет связи";
            var note = item.InitialDetection
                ? "Недоступность обнаружена при запуске мониторинга; более ранний момент неизвестен"
                : string.Empty;

            writer.WriteLine(string.Join(';',
                Esc(item.HostName),
                Esc(item.Address),
                Esc(item.LostAt.LocalDateTime.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture)),
                Esc(recovered),
                Esc(FormatDuration(item.GetDowntime(now))),
                Esc(note)));
        }
    }

    private static Dictionary<Guid, MonitorEvent> FindOpenOfflineEvents(IEnumerable<MonitorEvent> source)
    {
        var open = new Dictionary<Guid, MonitorEvent>();
        foreach (var item in source.OrderBy(x => x.Timestamp))
        {
            if (item.Type == MonitorEventType.Offline)
            {
                if (!open.ContainsKey(item.HostId))
                    open[item.HostId] = item;
            }
            else
            {
                open.Remove(item.HostId);
            }
        }
        return open;
    }

    private void Rewrite(IEnumerable<MonitorEvent> events)
    {
        AppPaths.EnsureCreated();
        lock (_sync)
        {
            using var writer = new StreamWriter(AppPaths.EventsFile, false, Encoding.UTF8);
            foreach (var item in events)
                writer.WriteLine(JsonSerializer.Serialize(item, JsonOptions.Compact));
        }
    }

    private static string FormatDuration(TimeSpan value)
    {
        if (value.TotalDays >= 1)
            return $"{(int)value.TotalDays}д {value:hh\\:mm\\:ss}";
        return value.ToString("hh\\:mm\\:ss");
    }
}
