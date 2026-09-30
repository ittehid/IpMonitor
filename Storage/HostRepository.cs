using System.Text.Json;
using IpMonitor.Models;

namespace IpMonitor.Storage;

internal sealed class HostRepository
{
    private readonly object _sync = new();

    public List<MonitorHost> Load()
    {
        AppPaths.EnsureCreated();
        if (!File.Exists(AppPaths.HostsFile))
        {
            var legacy = LegacyHostImporter.TryImport();
            if (legacy.Count > 0)
            {
                Save(legacy);
                return legacy;
            }
            return [];
        }

        try
        {
            var json = File.ReadAllText(AppPaths.HostsFile);
            using var parsed = JsonDocument.Parse(json);
            List<MonitorHost>? hosts;

            // Совместимость с ранними сборками 2.0, где hosts.json был просто массивом.
            if (parsed.RootElement.ValueKind == JsonValueKind.Array)
            {
                hosts = JsonSerializer.Deserialize<List<MonitorHost>>(json, JsonOptions.Default);
            }
            else
            {
                hosts = JsonSerializer.Deserialize<HostStorageDocument>(json, JsonOptions.Default)?.Hosts;
            }

            return Normalize(hosts ?? []);
        }
        catch
        {
            return [];
        }
    }

    public void Save(IEnumerable<MonitorHost> hosts, AppSettings? settings = null)
    {
        AppPaths.EnsureCreated();
        lock (_sync)
        {
            if (settings is not null)
            {
                try { BackupService.CreateBackupIfNeeded(settings); }
                catch { /* Ошибка автобэкапа не должна блокировать сохранение хостов. */ }
            }

            var ordered = hosts.ToList();
            for (var i = 0; i < ordered.Count; i++)
                ordered[i].SortOrder = i;

            var document = new HostStorageDocument
            {
                SchemaVersion = 2,
                SavedAt = DateTimeOffset.Now,
                Hosts = ordered
            };

            File.WriteAllText(AppPaths.HostsFile, JsonSerializer.Serialize(document, JsonOptions.Default));
        }
    }

    private static List<MonitorHost> Normalize(List<MonitorHost> hosts)
    {
        var seenIds = new HashSet<Guid>();
        foreach (var host in hosts)
        {
            host.Group = host.Group?.Trim() ?? string.Empty;
            if (!host.MaintenanceIndefinite && host.MaintenanceUntil is DateTimeOffset until && until <= DateTimeOffset.Now)
                host.MaintenanceUntil = null;

            if (host.Id == Guid.Empty || !seenIds.Add(host.Id))
            {
                host.Id = Guid.NewGuid();
                seenIds.Add(host.Id);
            }
        }

        return hosts
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    private sealed class HostStorageDocument
    {
        public int SchemaVersion { get; set; } = 2;
        public DateTimeOffset SavedAt { get; set; }
        public List<MonitorHost> Hosts { get; set; } = [];
    }
}
