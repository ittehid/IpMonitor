using IpMonitor.Models;
using IpMonitor.Storage;
using IpMonitor.UI;

namespace IpMonitor;

public partial class EventsForm : Form
{
    private EventRepository _repository = new();
    private List<MonitorHost> _hosts = [];
    private Guid? _initialHostId;
    private List<OutageRecord> _allOutages = [];
    private List<OutageRecord> _visibleOutages = [];

    private sealed record HostFilter(Guid? Id, string Name)
    {
        public override string ToString() => Name;
    }

    // Пустой конструктор нужен Visual Studio Designer.
    // На форме находятся все визуальные элементы; здесь нет создания UI-контролов.
    public EventsForm()
    {
        InitializeComponent();
    }

    internal EventsForm(EventRepository repository, IEnumerable<MonitorHost> hosts, Guid? initialHostId = null)
        : this()
    {
        _repository = repository;
        _hosts = hosts.OrderBy(x => x.Name).ToList();
        _initialHostId = initialHostId;

        UiTheme.Apply(this);
        UiTheme.StyleGrid(dgvEvents);
        UiTheme.StyleSecondaryButton(btnRefresh);
        UiTheme.StyleSecondaryButton(btnExport);
        UiTheme.StyleDangerButton(btnDelete);
        UiTheme.StyleDangerButton(btnClear);
        UiTheme.StyleSecondaryButton(btnClose);
        lblHeaderHint.ForeColor = UiTheme.TextMuted;
        pnlFilters.BackColor = UiTheme.Surface;
        pnlBottom.BackColor = UiTheme.Surface;

        cboHost.Items.Add(new HostFilter(null, "Все хосты"));
        foreach (var host in _hosts)
            cboHost.Items.Add(new HostFilter(host.Id, host.Name));

        cboPeriod.SelectedIndex = 1;
        cboType.SelectedIndex = 0;
        cboHost.SelectedIndex = initialHostId is null
            ? 0
            : Math.Max(0, cboHost.Items.Cast<HostFilter>().ToList().FindIndex(x => x.Id == initialHostId));

        ReloadEvents();
        Resize += (_, _) => ResizeEventColumns();
        Shown += (_, _) => ResizeEventColumns();
        durationTimer.Start();
        UpdateDeleteButton();
    }

    private void ReloadEvents()
    {
        _allOutages = _repository.LoadOutages();
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        IEnumerable<OutageRecord> query = _allOutages;

        if (cboHost.SelectedItem is HostFilter hostFilter && hostFilter.Id is Guid hostId)
            query = query.Where(x => x.HostId == hostId);

        var since = cboPeriod.SelectedIndex switch
        {
            0 => DateTimeOffset.Now.AddHours(-24),
            1 => DateTimeOffset.Now.AddDays(-7),
            2 => DateTimeOffset.Now.AddDays(-30),
            _ => DateTimeOffset.MinValue
        };

        // Текущее падение всегда должно быть видно. Для завершённых периодов показываем
        // запись, если начало или восстановление попадает в выбранный период.
        query = query.Where(x =>
            x.IsOngoing ||
            x.LostAt >= since ||
            (x.RecoveredAt is DateTimeOffset recoveredAt && recoveredAt >= since));

        if (cboType.SelectedIndex == 1)
            query = query.Where(x => x.IsOngoing);
        else if (cboType.SelectedIndex == 2)
            query = query.Where(x => !x.IsOngoing);

        _visibleOutages = query.OrderByDescending(x => x.LostAt).ToList();
        dgvEvents.Rows.Clear();

        var now = DateTimeOffset.Now;
        foreach (var item in _visibleOutages)
        {
            var recovered = item.RecoveredAt is DateTimeOffset recoveredAt
                ? recoveredAt.LocalDateTime.ToString("dd.MM.yyyy HH:mm:ss")
                : "Нет связи";

            var index = dgvEvents.Rows.Add(
                item.HostName,
                item.Address,
                item.LostAt.LocalDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                recovered,
                FormatDuration(item.GetDowntime(now)));

            var row = dgvEvents.Rows[index];
            row.Tag = item;

            if (item.InitialDetection)
            {
                row.Cells[colLostAt.Index].ToolTipText =
                    "Хост уже был недоступен при запуске мониторинга. " +
                    "Указано время обнаружения; более ранний момент потери связи неизвестен.";
            }
        }

        UpdateCount();
        UpdateDeleteButton();
        ResizeEventColumns();
    }

    private void ResizeEventColumns()
    {
        if (dgvEvents.Columns.Count == 0 || dgvEvents.ClientSize.Width <= 0)
            return;

        // Порядок колонок ориентирован на быстрое чтение: сначала устройство,
        // затем его адрес и уже после этого временная шкала недоступности.
        // Все колонки заполняют ширину окна без горизонтальной прокрутки.
        dgvEvents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        colEventHost.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colEventHost.FillWeight = 23F;
        colEventHost.MinimumWidth = 110;

        colEventAddress.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colEventAddress.FillWeight = 18F;
        colEventAddress.MinimumWidth = 110;

        colLostAt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colLostAt.FillWeight = 22F;
        colLostAt.MinimumWidth = 145;

        colRecoveredAt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colRecoveredAt.FillWeight = 24F;
        colRecoveredAt.MinimumWidth = 150;

        colDowntime.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colDowntime.FillWeight = 13F;
        colDowntime.MinimumWidth = 95;
    }

    private void UpdateCount()
    {
        var ongoing = _visibleOutages.Count(x => x.IsOngoing);
        lblCount.Text = ongoing > 0
            ? $"Периодов: {_visibleOutages.Count}   Сейчас без связи: {ongoing}"
            : $"Периодов: {_visibleOutages.Count}";
    }

    private static string FormatDuration(TimeSpan value)
    {
        if (value.TotalDays >= 1)
            return $"{(int)value.TotalDays}д {value:hh\\:mm\\:ss}";
        return value.ToString("hh\\:mm\\:ss");
    }

    private void FilterChanged(object? sender, EventArgs e)
    {
        if (IsHandleCreated)
            ApplyFilter();
    }

    private void btnRefresh_Click(object? sender, EventArgs e) => ReloadEvents();

    private void durationTimer_Tick(object? sender, EventArgs e)
    {
        if (dgvEvents.Rows.Count == 0)
            return;

        var now = DateTimeOffset.Now;
        foreach (DataGridViewRow row in dgvEvents.Rows)
        {
            if (row.Tag is not OutageRecord item || !item.IsOngoing)
                continue;

            row.Cells[colDowntime.Index].Value = FormatDuration(item.GetDowntime(now));
        }
    }

    private void dgvEvents_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvEvents.Rows[e.RowIndex].Tag is not OutageRecord item)
            return;

        if (item.IsOngoing)
        {
            // В строгой теме строка не заливается цветом целиком — статус выделяется только текстом.
            if (e.ColumnIndex == colRecoveredAt.Index || e.ColumnIndex == colDowntime.Index)
            {
                e.CellStyle.ForeColor = UiTheme.Danger;
                e.CellStyle.Font = new Font("Segoe UI Semibold", 7.75F);
            }
        }
        else if (e.ColumnIndex == colRecoveredAt.Index)
        {
            e.CellStyle.ForeColor = UiTheme.Success;
        }
    }

    private void btnExport_Click(object? sender, EventArgs e)
    {
        if (_visibleOutages.Count == 0)
        {
            MessageBox.Show(this, "В текущем фильтре нет периодов недоступности для экспорта.", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "CSV (*.csv)|*.csv",
            FileName = $"IpMonitor-outages-{DateTime.Now:yyyy-MM-dd}.csv"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            _repository.ExportCsv(dialog.FileName, _visibleOutages);
            MessageBox.Show(this, "Журнал экспортирован.", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка экспорта", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }


    private void dgvEvents_SelectionChanged(object? sender, EventArgs e) => UpdateDeleteButton();

    private void UpdateDeleteButton()
    {
        if (dgvEvents.SelectedRows.Count == 0 || dgvEvents.SelectedRows[0].Tag is not OutageRecord item)
        {
            btnDelete.Enabled = false;
            return;
        }

        // Текущий активный Offline не удаляем: иначе потеряется точное время начала падения.
        btnDelete.Enabled = !item.IsOngoing;
    }

    private void btnDelete_Click(object? sender, EventArgs e)
    {
        if (dgvEvents.SelectedRows.Count == 0 || dgvEvents.SelectedRows[0].Tag is not OutageRecord item)
        {
            MessageBox.Show(this, "Выберите завершённую запись в журнале.", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (item.IsOngoing)
        {
            MessageBox.Show(this,
                "Текущий период без связи удалить нельзя. Он исчезнет из активных после восстановления, а затем его можно будет удалить.",
                "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var recovered = item.RecoveredAt?.LocalDateTime.ToString("dd.MM.yyyy HH:mm:ss") ?? "—";
        var message = $"Удалить выбранный период из журнала?\n\n" +
                      $"{item.HostName} ({item.Address})\n" +
                      $"Потеря связи: {item.LostAt.LocalDateTime:dd.MM.yyyy HH:mm:ss}\n" +
                      $"Восстановление: {recovered}";

        if (MessageBox.Show(this, message, "IP Monitor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        if (!_repository.DeleteOutage(item))
        {
            MessageBox.Show(this, "Не удалось удалить выбранную запись.", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ReloadEvents();
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        var message = "Очистить ВСЮ завершённую историю недоступности?\n\n" +
                      "Текущие хосты без связи останутся в журнале, чтобы не потерять время начала падения.";

        if (MessageBox.Show(this, message, "IP Monitor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        _repository.Clear();
        ReloadEvents();
    }
}
