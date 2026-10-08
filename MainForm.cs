using System.Diagnostics;
using System.Net;
using IpMonitor.Models;
using IpMonitor.Services;
using IpMonitor.Storage;
using IpMonitor.UI;

namespace IpMonitor;

public partial class MainForm : Form
{
    private readonly HostRepository _hostRepository = new();
    private readonly SettingsRepository _settingsRepository = new();
    private readonly EventRepository _eventRepository = new();
    private readonly HostCheckService _hostCheckService = new();
    private MonitoringService? _monitoringService;
    private NotificationService? _notificationService;
    private readonly Dictionary<Guid, DataGridViewRow> _rows = [];
    private readonly System.Windows.Forms.Timer _uiTimer;
    private List<MonitorHost> _hosts = [];
    private AppSettings _settings = new();
    private bool _allowExit;
    private DateTimeOffset _activityVisibleUntil = DateTimeOffset.MinValue;
    private Guid? _dragHostId;
    private Point _dragStartPoint;
    private bool _adjustingWindowHeight;

    public MainForm()
    {
        InitializeComponent();
        _settings = _settingsRepository.Load();
        UiTheme.Apply(this);
        ApplyModernStyle();

        if (Icon is not null)
            notifyIcon.Icon = (Icon)Icon.Clone();

        _hosts = _hostRepository.Load();
        ApplyStoredSortMode();
        TopMost = _settings.AlwaysOnTop;

        if (_settings.AutoDeleteOldEvents)
            _eventRepository.Prune(_settings.EventRetentionDays);
        try { BackupService.Cleanup(_settings); } catch { }

        _monitoringService = new MonitoringService(_hostCheckService, _eventRepository);
        _monitoringService.HostUpdated += MonitoringService_HostUpdated;
        _monitoringService.EventRaised += MonitoringService_EventRaised;
        _notificationService = new NotificationService(notifyIcon);

        // Небольшой интервал нужен только для UI-индикации ping и счётчика простоя.
        // Сами проверки выполняются MonitoringService и от этого таймера не зависят.
        _uiTimer = new System.Windows.Forms.Timer { Interval = 250 };
        _uiTimer.Tick += (_, _) => RefreshDynamicValues();
        _uiTimer.Start();

        LoadGrid();
        UpdateSummary();
        UpdateMonitoringUi();
        UpdateSortUi();

        Shown += (_, _) =>
        {
            ApplyDpiAwareGridMetrics();
            LoadGrid();
            UpdateSummary();
            AdjustWindowHeightToContent();

            if (_settings.StartMonitoringOnLaunch && _hosts.Any(x => x.Enabled))
                StartMonitoring();
        };
    }

    private void ApplyModernStyle()
    {
        pnlToolbar.BackColor = UiTheme.Surface;
        pnlSummary.BackColor = UiTheme.Surface;
        lblTitle.ForeColor = UiTheme.TextPrimary;
        lblSubtitle.ForeColor = UiTheme.TextMuted;

        UiTheme.StyleSecondaryButton(btnMonitoring);
        UiTheme.StyleSecondaryButton(btnAddHost);
        UiTheme.StyleGhostButton(btnEvents);
        UiTheme.StyleGhostButton(btnSettings);
        UiTheme.StyleGhostButton(btnMore);
        UiTheme.StylePrimaryButton(btnEmptyAdd);
        UiTheme.StyleSecondaryButton(btnEmptyQuickAdd);

        lblEmptyIcon.ForeColor = UiTheme.Accent;
        lblEmptyTitle.ForeColor = UiTheme.TextPrimary;
        lblEmptyHint.ForeColor = UiTheme.TextMuted;

        UiTheme.StyleBadge(lblOnline, UiTheme.SuccessSoft, UiTheme.Success);
        UiTheme.StyleBadge(lblWarning, UiTheme.WarningSoft, UiTheme.Warning);
        UiTheme.StyleBadge(lblOffline, UiTheme.DangerSoft, UiTheme.Danger);
        UiTheme.StyleBadge(lblDisabled, UiTheme.SurfaceAlt, UiTheme.TextMuted);

        UiTheme.StyleGrid(dgvHosts);
        UiTheme.StyleContextMenu(cmsHost);
        UiTheme.StyleContextMenu(cmsAdd);
        UiTheme.StyleContextMenu(cmsMore);
        UiTheme.StyleContextMenu(cmsTray);

        // Строгая таблица без карточек, скруглений и декоративных заливок.
        // Геометрия строк и колонок задаётся отдельно с учётом текущего DPI.
        EnableDoubleBuffering(dgvHosts);
        dgvHosts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        dgvHosts.DefaultCellStyle.Padding = new Padding(4, 1, 4, 1);
        dgvHosts.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 5, 0);
        dgvHosts.DefaultCellStyle.Font = new Font("Segoe UI", 8.25F);
        dgvHosts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.25F);
        dgvHosts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        dgvHosts.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        dgvHosts.GridColor = UiTheme.Border;

        colStatus.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colStatus.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
        colResponse.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colResponse.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 7.35F);
        colName.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F);
        colAddress.Visible = false;
        ApplyDpiAwareGridMetrics();

        statusMain.BackColor = UiTheme.Surface;
        statusMain.ForeColor = UiTheme.TextMuted;
        tslActivity.ForeColor = UiTheme.Accent;
    }

    private static void EnableDoubleBuffering(DataGridView grid)
    {
        try
        {
            var property = typeof(DataGridView).GetProperty(
                "DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            property?.SetValue(grid, true);
        }
        catch
        {
            // Это только визуальная оптимизация; без неё таблица продолжит работать.
        }
    }

    private int ScaleForDpi(int logicalPixels)
    {
        var dpi = IsHandleCreated ? DeviceDpi : 96;
        return Math.Max(1, (int)Math.Round(logicalPixels * dpi / 96f));
    }

    private int HostRowHeight => ScaleForDpi(24);

    private void ApplyDpiAwareGridMetrics()
    {
        // DataGridViewColumn.Width и Row.Height не всегда корректно масштабируются
        // WinForms при PerMonitor DPI, поэтому задаём их в фактических пикселях.
        dgvHosts.ColumnHeadersHeight = ScaleForDpi(25);
        dgvHosts.RowTemplate.Height = HostRowHeight;
        colStatus.Width = ScaleForDpi(22);
        colResponse.Width = ScaleForDpi(76);

        foreach (DataGridViewRow row in dgvHosts.Rows)
            row.Height = HostRowHeight;

        dgvHosts.Invalidate();
    }

    private void AdjustWindowHeightToContent()
    {
        if (_adjustingWindowHeight || !IsHandleCreated || WindowState != FormWindowState.Normal)
            return;

        try
        {
            _adjustingWindowHeight = true;

            var workingArea = Screen.FromControl(this).WorkingArea;
            var nonClientHeight = Math.Max(0, Height - ClientSize.Height);
            var outerMargin = ScaleForDpi(10);
            var maxClientHeight = Math.Max(ScaleForDpi(210), workingArea.Height - nonClientHeight - outerMargin);

            int contentHeight;
            if (_hosts.Count == 0)
            {
                contentHeight = ScaleForDpi(205);
            }
            else
            {
                var rowsHeight = dgvHosts.Rows.Cast<DataGridViewRow>().Sum(row => row.Height);
                contentHeight = dgvHosts.ColumnHeadersHeight + rowsHeight + ScaleForDpi(2);
            }

            var desiredClientHeight = pnlToolbar.Height + pnlSummary.Height + statusMain.Height + contentHeight;
            var minClientHeight = _hosts.Count == 0 ? ScaleForDpi(285) : ScaleForDpi(150);
            desiredClientHeight = Math.Clamp(desiredClientHeight, minClientHeight, maxClientHeight);

            if (Math.Abs(ClientSize.Height - desiredClientHeight) > ScaleForDpi(2))
                ClientSize = new Size(ClientSize.Width, desiredClientHeight);

            // При увеличении списка не позволяем окну уйти ниже рабочей области.
            // Ширину и горизонтальную позицию не трогаем — монитор по-прежнему
            // удобно держать у правого/левого края рабочего стола.
            var maxBottom = workingArea.Bottom - outerMargin;
            if (Bottom > maxBottom)
                Top = Math.Max(workingArea.Top + outerMargin, maxBottom - Height);
        }
        finally
        {
            _adjustingWindowHeight = false;
        }
    }

    private void MainForm_DpiChanged(object? sender, DpiChangedEventArgs e)
    {
        BeginInvoke((Action)(() =>
        {
            ApplyDpiAwareGridMetrics();
            AdjustWindowHeightToContent();
        }));
    }

    private void LoadGrid()
    {
        dgvHosts.Rows.Clear();
        _rows.Clear();

        var ordered = _hosts.OrderBy(x => x.SortOrder).ToList();
        foreach (var host in ordered)
            AddOrUpdateRow(host);

        UpdateSortUi();
        ApplyDpiAwareGridMetrics();
        if (IsHandleCreated)
            BeginInvoke((Action)AdjustWindowHeightToContent);
    }

    private void AddOrUpdateRow(MonitorHost host)
    {
        if (!_rows.TryGetValue(host.Id, out var row))
        {
            var index = dgvHosts.Rows.Add();
            row = dgvHosts.Rows[index];
            row.Tag = host.Id;
            _rows[host.Id] = row;
        }

        row.Height = HostRowHeight;
        row.Cells[colStatus.Index].Value = StatusGlyph(host);
        row.Cells[colName.Index].Value = host.Name;
        row.Cells[colAddress.Index].Value = host.Address;
        row.Cells[colResponse.Index].Value = BuildResponseText(host);
        row.DefaultCellStyle.ForeColor = host.State == HostState.Disabled ? UiTheme.TextMuted : UiTheme.TextPrimary;
        ApplyHostTooltip(row, host);
    }

    private void ApplyHostTooltip(DataGridViewRow row, MonitorHost host)
    {
        var tooltipText = BuildHostTooltip(host);

        // DataGridViewRow не имеет свойства ToolTipText. Подсказка задаётся
        // непосредственно ячейкам строки, поэтому IP/DNS остаётся скрытым
        // в таблице, но доступен при наведении на любую видимую ячейку хоста.
        foreach (DataGridViewCell cell in row.Cells)
            cell.ToolTipText = tooltipText;
    }

    private void RefreshDynamicValues()
    {
        foreach (var host in _hosts)
        {
            if (!_rows.TryGetValue(host.Id, out var row))
                continue;
            row.Cells[colStatus.Index].Value = StatusGlyph(host);
            row.Cells[colResponse.Index].Value = BuildResponseText(host);
            ApplyHostTooltip(row, host);
        }

        UpdatePingActivity();
        UpdateSummary();
    }

    private string StatusGlyph(MonitorHost host)
    {
        if (_monitoringService?.IsRunning != true)
            return host.Enabled ? "•" : "○";
        if (host.IsChecking)
            return "◌";
        if (host.IsInMaintenance)
            return "◆";

        return host.State switch
        {
            HostState.Online => "●",
            HostState.Warning => "▲",
            HostState.Offline => "●",
            HostState.Disabled => "○",
            _ => "•"
        };
    }

    private string BuildResponseText(MonitorHost host)
    {
        if (_monitoringService?.IsRunning != true)
            return host.Enabled ? "—" : "Отключен";
        if (host.IsChecking)
            return host.CheckType == CheckType.Ping ? "ping…" : $"tcp:{host.TcpPort}…";

        if (host.IsInMaintenance)
        {
            var suffix = host.State switch
            {
                HostState.Online when host.LastLatencyMs is long ms => $"{ms} ms",
                HostState.Warning when host.IsLatencyWarning && host.LastLatencyMs is long ms => $"{ms} ms",
                HostState.Offline => "Offline",
                HostState.Disabled => "Выкл",
                _ => "..."
            };
            return $"M · {suffix}";
        }

        return host.State switch
        {
            HostState.Online => host.LastLatencyMs is long ms ? $"{ms} ms" : "Online",
            HostState.Warning when host.IsLatencyWarning && host.LastLatencyMs is long latency => $"{latency} ms ↑",
            HostState.Warning => host.IsConfirmedOffline
                ? "Восстановление…"
                : $"Warn {host.ConsecutiveFailures}/{GetOfflineThreshold(host)}",
            HostState.Offline => "Offline",
            HostState.Disabled => "Отключен",
            _ => "Ожидание…"
        };
    }

    private int GetOfflineThreshold(MonitorHost host) => host.UseGlobalMonitoringSettings
        ? _settings.OfflineAfterFailures
        : host.OfflineAfterFailures;

    private int GetCheckInterval(MonitorHost host) => host.UseGlobalMonitoringSettings
        ? _settings.CheckIntervalSeconds
        : host.CheckIntervalSeconds;

    private int GetTimeout(MonitorHost host) => host.UseGlobalMonitoringSettings
        ? _settings.TimeoutMilliseconds
        : host.TimeoutMilliseconds;

    private bool GetLatencyWarningEnabled(MonitorHost host) => host.UseGlobalMonitoringSettings
        ? _settings.LatencyWarningEnabled
        : host.LatencyWarningEnabled;

    private int GetLatencyThreshold(MonitorHost host) => host.UseGlobalMonitoringSettings
        ? _settings.LatencyWarningThresholdMs
        : host.LatencyWarningThresholdMs;

    private int GetLatencyWarningAfter(MonitorHost host) => host.UseGlobalMonitoringSettings
        ? _settings.LatencyWarningAfterChecks
        : host.LatencyWarningAfterChecks;

    private static string FormatCompactDuration(TimeSpan value)
    {
        if (value.TotalDays >= 1)
            return $"{(int)value.TotalDays}д {value.Hours:00}:{value.Minutes:00}";
        if (value.TotalHours >= 1)
            return $"{(int)value.TotalHours:00}:{value.Minutes:00}:{value.Seconds:00}";
        return $"{value.Minutes:00}:{value.Seconds:00}";
    }

    private void UpdateSummary()
    {
        var activeHosts = _hosts.Where(x => x.Enabled && !x.IsInMaintenance).ToList();
        var maintenance = _hosts.Count(x => x.Enabled && x.IsInMaintenance);
        var disabled = _hosts.Count(x => !x.Enabled || x.State == HostState.Disabled);
        var paused = maintenance + disabled;

        lblOnline.Text = $"● Online {activeHosts.Count(x => x.State == HostState.Online)}";
        lblWarning.Text = $"▲ Warn {activeHosts.Count(x => x.State == HostState.Warning)}";
        lblOffline.Text = $"● Offline {activeHosts.Count(x => x.State == HostState.Offline)}";
        lblDisabled.Text = maintenance > 0 ? $"◆ Пауза {paused}" : $"○ Пауза {paused}";
        lblDisabled.BackColor = UiTheme.Surface;
        lblDisabled.ForeColor = maintenance > 0 ? UiTheme.Maintenance : UiTheme.TextMuted;
        toolTip.SetToolTip(lblDisabled, $"Отключено: {disabled} · Обслуживание: {maintenance}");
        tslHosts.Text = $"Хостов: {_hosts.Count}";
        UpdateEmptyState();

        if (_hosts.Count == 0)
            notifyIcon.Text = "IP Monitor — нет хостов";
        else if (activeHosts.Any(x => x.State == HostState.Offline))
            notifyIcon.Text = TrimTrayText($"IP Monitor — Offline: {activeHosts.Count(x => x.State == HostState.Offline)}");
        else if (activeHosts.Any(x => x.State == HostState.Warning))
            notifyIcon.Text = TrimTrayText("IP Monitor — есть предупреждения");
        else if (_monitoringService?.IsRunning == true && activeHosts.Any(x => x.State == HostState.Unknown))
            notifyIcon.Text = "IP Monitor — идёт проверка";
        else if (_monitoringService?.IsRunning == true && maintenance > 0)
            notifyIcon.Text = TrimTrayText($"IP Monitor — всё доступно · обслуживание: {maintenance}");
        else
            notifyIcon.Text = TrimTrayText(_monitoringService?.IsRunning == true ? "IP Monitor — всё доступно" : "IP Monitor — остановлен");
    }

    private static string TrimTrayText(string text) => text.Length <= 63 ? text : text[..63];

    private void TryAutomaticBackup()
    {
        try { BackupService.CreateBackupIfNeeded(_settings); }
        catch { /* Мониторинг и сохранение настроек важнее фонового бэкапа. */ }
    }

    private void UpdateEmptyState()
    {
        var empty = _hosts.Count == 0;
        pnlEmpty.Visible = empty;
        dgvHosts.Visible = !empty;
        if (empty)
            pnlEmpty.BringToFront();
    }

    private void UpdatePingActivity()
    {
        var active = _monitoringService?.IsRunning == true && DateTimeOffset.Now <= _activityVisibleUntil;
        tslActivity.Text = active ? "◌ ping" : string.Empty;
        tslActivity.ToolTipText = active ? "Выполняется проверка доступности" : string.Empty;
    }

    private void StartMonitoring()
    {
        _monitoringService?.Start(_hosts, _settings);
        UpdateMonitoringUi();
    }

    private void StopMonitoring()
    {
        _monitoringService?.Stop();

        // После ручной остановки мониторинга не оставляем на экране
        // последний Online / Warning / Offline. Для включённых хостов
        // возвращаем нейтральное состояние до следующего запуска.
        foreach (var host in _hosts)
        {
            host.State = host.Enabled ? HostState.Unknown : HostState.Disabled;
            host.IsChecking = false;
            host.FailureStartedAt = null;
            host.OfflineSince = null;
            host.ConsecutiveFailures = 0;
            host.ConsecutiveSuccesses = 0;
            host.ConsecutiveHighLatency = 0;
            host.IsLatencyWarning = false;
            host.IsConfirmedOffline = false;
            host.OfflineNotificationSuppressedByMaintenance = false;
            host.HasSuccessfulCheckThisRun = false;
            AddOrUpdateRow(host);
        }

        _activityVisibleUntil = DateTimeOffset.MinValue;
        UpdateMonitoringUi();
    }

    private void RestartMonitoringIfNeeded()
    {
        if (_monitoringService?.IsRunning == true)
            _monitoringService.Restart(_hosts, _settings);
        else
        {
            foreach (var host in _hosts)
            {
                host.State = host.Enabled ? HostState.Unknown : HostState.Disabled;
                host.IsChecking = false;
                AddOrUpdateRow(host);
            }
        }
        UpdateMonitoringUi();
    }

    private void UpdateMonitoringUi()
    {
        var running = _monitoringService?.IsRunning == true;
        btnMonitoring.Text = running ? "■ Стоп" : "▶ Старт";
        if (running)
            UiTheme.StyleDangerButton(btnMonitoring);
        else
            UiTheme.StylePrimaryButton(btnMonitoring);
        miTrayMonitoring.Text = running ? "Остановить мониторинг" : "Запустить мониторинг";
        tslState.Text = running ? "● Мониторинг" : "Мониторинг остановлен";
        tslState.ForeColor = running ? UiTheme.Success : UiTheme.TextMuted;
        if (!running)
            tslActivity.Text = string.Empty;
        UpdateSummary();
    }

    private void MonitoringService_HostUpdated(MonitorHost host)
    {
        if (IsDisposed || !IsHandleCreated)
            return;
        BeginInvoke((Action)(() =>
        {
            if (host.IsChecking)
                _activityVisibleUntil = DateTimeOffset.Now.AddMilliseconds(700);
            AddOrUpdateRow(host);
            UpdatePingActivity();
            UpdateSummary();
        }));
    }

    private void MonitoringService_EventRaised(MonitorEvent item)
    {
        if (IsDisposed || !IsHandleCreated)
            return;
        BeginInvoke((Action)(() => _notificationService?.Handle(item, _settings)));
    }

    private MonitorHost? SelectedHost
    {
        get
        {
            if (dgvHosts.SelectedRows.Count == 0)
                return null;
            if (dgvHosts.SelectedRows[0].Tag is not Guid id)
                return null;
            return _hosts.FirstOrDefault(x => x.Id == id);
        }
    }

    private void btnMonitoring_Click(object? sender, EventArgs e)
    {
        if (_monitoringService?.IsRunning == true)
            StopMonitoring();
        else
            StartMonitoring();
    }

    private void btnAddHost_Click(object? sender, EventArgs e)
    {
        cmsAdd.Show(btnAddHost, new Point(0, btnAddHost.Height + 2));
    }

    private void miAddSingle_Click(object? sender, EventArgs e) => AddHost();
    private void btnEmptyAdd_Click(object? sender, EventArgs e) => AddHost();
    private void btnEmptyQuickAdd_Click(object? sender, EventArgs e) => miAddMultiple_Click(sender, e);

    private void AddHost()
    {
        using var form = new HostForm(null, _settings, _hostCheckService);
        if (form.ShowDialog(this) != DialogResult.OK || form.Host is null)
            return;

        form.Host.SortOrder = _hosts.Count;
        _hosts.Add(form.Host);
        SaveHostsRespectingSort(form.Host.Id);
        RestartMonitoringIfNeeded();
    }

    private void miAddMultiple_Click(object? sender, EventArgs e)
    {
        using var form = new QuickAddForm(_settings, _hostCheckService);
        if (form.ShowDialog(this) != DialogResult.OK || form.Hosts.Count == 0)
            return;

        foreach (var host in form.Hosts)
        {
            host.SortOrder = _hosts.Count;
            _hosts.Add(host);
        }
        SaveHostsRespectingSort();
        RestartMonitoringIfNeeded();
    }

    private void btnMore_Click(object? sender, EventArgs e)
    {
        UpdateSortUi();
        cmsMore.Show(btnMore, new Point(0, btnMore.Height + 2));
    }
private void miAbout_Click(object? sender, EventArgs e)
    {
        using var form = new AboutForm();
        form.ShowDialog(this);
    }

    private void miExit_Click(object? sender, EventArgs e)
    {
        _allowExit = true;
        Close();
    }

    private void btnEvents_Click(object? sender, EventArgs e)
    {
        using var form = new EventsForm(_eventRepository, _hosts);
        form.ShowDialog(this);
    }

    private void btnSettings_Click(object? sender, EventArgs e)
    {
        using var form = new SettingsForm(_settings);
        if (form.ShowDialog(this) != DialogResult.OK || form.Settings is null)
            return;

        TryAutomaticBackup();
        _settings = form.Settings;
        _settingsRepository.Save(_settings);
        TryAutomaticBackup();
        try { BackupService.Cleanup(_settings); } catch { }
        TopMost = _settings.AlwaysOnTop;
        try { StartupService.SetEnabled(_settings.StartWithWindows); }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Не удалось изменить автозапуск:\n{ex.Message}", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        if (_settings.AutoDeleteOldEvents)
            _eventRepository.Prune(_settings.EventRetentionDays);
        RestartMonitoringIfNeeded();
    }

    // ------------------------ Импорт / экспорт ------------------------

    private void miImportHosts_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Импорт хостов",
            Filter = "IP Monitor / JSON / старый XML|*.ipmhosts.json;*.json;*.xml|IP Monitor JSON|*.ipmhosts.json;*.json|Старый IP Monitor XML|*.xml|Все файлы|*.*"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            var imported = HostTransferService.Import(dialog.FileName);
            if (imported.Count == 0)
            {
                MessageBox.Show(this, "В файле не найдено хостов.", "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var replace = true;
            if (_hosts.Count > 0)
            {
                var choice = MessageBox.Show(this,
                    $"Найдено хостов: {imported.Count}\n\nДа — заменить текущий список.\nНет — добавить к текущему списку.\nОтмена — ничего не менять.",
                    "Импорт хостов", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (choice == DialogResult.Cancel)
                    return;
                replace = choice == DialogResult.Yes;
            }

            var skipped = 0;
            if (replace)
            {
                _hosts = imported;
            }
            else
            {
                var existingKeys = new HashSet<string>(_hosts.Select(HostIdentityKey), StringComparer.OrdinalIgnoreCase);
                var existingIds = new HashSet<Guid>(_hosts.Select(x => x.Id));
                foreach (var host in imported)
                {
                    if (!existingKeys.Add(HostIdentityKey(host)))
                    {
                        skipped++;
                        continue;
                    }
                    if (host.Id == Guid.Empty || !existingIds.Add(host.Id))
                    {
                        host.Id = Guid.NewGuid();
                        existingIds.Add(host.Id);
                    }
                    host.SortOrder = _hosts.Count;
                    _hosts.Add(host);
                }
            }

            SaveHostsRespectingSort();
            RestartMonitoringIfNeeded();
            MessageBox.Show(this,
                skipped > 0 ? $"Импорт завершён. Пропущено дубликатов: {skipped}." : $"Импорт завершён. Хостов: {_hosts.Count}.",
                "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Не удалось импортировать хосты:\n{ex.Message}", "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void miExportHosts_Click(object? sender, EventArgs e)
    {
        if (_hosts.Count == 0)
        {
            MessageBox.Show(this, "Список хостов пуст.", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Title = "Экспорт хостов",
            Filter = "IP Monitor JSON (*.ipmhosts.json)|*.ipmhosts.json|Старый IP Monitor XML (*.xml)|*.xml",
            FileName = $"IpMonitor_hosts_{DateTime.Now:yyyy-MM-dd}.ipmhosts.json",
            DefaultExt = "ipmhosts.json",
            AddExtension = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        if (dialog.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
        {
            var choice = MessageBox.Show(this,
                "Старый XML хранит только название и IP/адрес. Обслуживание, Web/RDP, TCP и индивидуальные настройки в XML не попадут.\n\nПродолжить?",
                "Экспорт в старый формат", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (choice != DialogResult.Yes)
                return;
        }

        try
        {
            HostTransferService.Export(dialog.FileName, _hosts);
            MessageBox.Show(this, "Список хостов экспортирован.", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Не удалось экспортировать хосты:\n{ex.Message}", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void miOpenDataFolder_Click(object? sender, EventArgs e)
    {
        AppPaths.EnsureCreated();
        try { Process.Start(new ProcessStartInfo(AppPaths.DataDirectory) { UseShellExecute = true }); }
        catch (Exception ex) { ShowOpenError("папку данных", ex); }
    }

    private static string HostIdentityKey(MonitorHost host) => $"{host.Name.Trim()}\u001f{host.Address.Trim()}";

    // ------------------------ Сортировка ------------------------

    private void ApplyStoredSortMode()
    {
        SortHostsInMemory(_settings.HostSortMode);
        NormalizeSortOrder();
    }

    private void SaveHostsRespectingSort(Guid? selectedId = null)
    {
        SortHostsInMemory(_settings.HostSortMode);
        NormalizeSortOrder();
        _hostRepository.Save(_hosts, _settings);
        LoadGrid();
        RestoreSelection(selectedId);
    }

    private void SortAndPersist(HostSortMode mode)
    {
        TryAutomaticBackup();
        _settings.HostSortMode = mode;
        _settingsRepository.Save(_settings);
        SortHostsInMemory(mode);
        NormalizeSortOrder();
        _hostRepository.Save(_hosts, _settings);
        LoadGrid();
    }

    private void SwitchToManualOrderAndSave(Guid? selectedId = null)
    {
        TryAutomaticBackup();
        _settings.HostSortMode = HostSortMode.Manual;
        _settingsRepository.Save(_settings);
        NormalizeSortOrder();
        _hostRepository.Save(_hosts, _settings);
        LoadGrid();
        RestoreSelection(selectedId);
    }

    private void SortHostsInMemory(HostSortMode mode)
    {
        var nameComparer = StringComparer.CurrentCultureIgnoreCase;
        var addressComparer = Comparer<string>.Create(CompareAddresses);

        switch (mode)
        {
            case HostSortMode.NameAscending:
                _hosts = _hosts.OrderBy(x => x.Name, nameComparer).ToList();
                break;
            case HostSortMode.NameDescending:
                _hosts = _hosts.OrderByDescending(x => x.Name, nameComparer).ToList();
                break;
            case HostSortMode.AddressAscending:
                _hosts = _hosts.OrderBy(x => x.Address ?? string.Empty, addressComparer).ToList();
                break;
            case HostSortMode.AddressDescending:
                _hosts = _hosts.OrderByDescending(x => x.Address ?? string.Empty, addressComparer).ToList();
                break;
            case HostSortMode.Manual:
            default:
                _hosts = _hosts.OrderBy(x => x.SortOrder).ToList();
                break;
        }
    }

    private void NormalizeSortOrder()
    {
        for (var i = 0; i < _hosts.Count; i++)
            _hosts[i].SortOrder = i;
    }

    private static int CompareAddresses(string? left, string? right)
    {
        left ??= string.Empty;
        right ??= string.Empty;
        if (IPAddress.TryParse(left, out var leftIp) && IPAddress.TryParse(right, out var rightIp))
        {
            var lb = leftIp.GetAddressBytes();
            var rb = rightIp.GetAddressBytes();
            if (lb.Length != rb.Length)
                return lb.Length.CompareTo(rb.Length);
            for (var i = 0; i < lb.Length; i++)
            {
                var cmp = lb[i].CompareTo(rb[i]);
                if (cmp != 0)
                    return cmp;
            }
            return 0;
        }
        return StringComparer.CurrentCultureIgnoreCase.Compare(left, right);
    }

    private void UpdateSortUi()
    {
        miSortManual.Checked = _settings.HostSortMode == HostSortMode.Manual;
        miSortNameAsc.Checked = _settings.HostSortMode == HostSortMode.NameAscending;
        miSortNameDesc.Checked = _settings.HostSortMode == HostSortMode.NameDescending;
        miSortAddressAsc.Checked = _settings.HostSortMode == HostSortMode.AddressAscending;
        miSortAddressDesc.Checked = _settings.HostSortMode == HostSortMode.AddressDescending;

        colName.HeaderCell.SortGlyphDirection = _settings.HostSortMode switch
        {
            HostSortMode.NameAscending or HostSortMode.AddressAscending => SortOrder.Ascending,
            HostSortMode.NameDescending or HostSortMode.AddressDescending => SortOrder.Descending,
            _ => SortOrder.None
        };
        colAddress.HeaderCell.SortGlyphDirection = _settings.HostSortMode switch
        {
            HostSortMode.AddressAscending => SortOrder.Ascending,
            HostSortMode.AddressDescending => SortOrder.Descending,
            _ => SortOrder.None
        };
    }

    private void miSortManual_Click(object? sender, EventArgs e) => SortAndPersist(HostSortMode.Manual);
    private void miSortNameAsc_Click(object? sender, EventArgs e) => SortAndPersist(HostSortMode.NameAscending);
    private void miSortNameDesc_Click(object? sender, EventArgs e) => SortAndPersist(HostSortMode.NameDescending);
    private void miSortAddressAsc_Click(object? sender, EventArgs e) => SortAndPersist(HostSortMode.AddressAscending);
    private void miSortAddressDesc_Click(object? sender, EventArgs e) => SortAndPersist(HostSortMode.AddressDescending);

    private void dgvHosts_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.ColumnIndex == colName.Index)
        {
            SortAndPersist(_settings.HostSortMode == HostSortMode.NameAscending
                ? HostSortMode.NameDescending
                : HostSortMode.NameAscending);
        }
        else if (e.ColumnIndex == colAddress.Index)
        {
            SortAndPersist(_settings.HostSortMode == HostSortMode.AddressAscending
                ? HostSortMode.AddressDescending
                : HostSortMode.AddressAscending);
        }
    }

    private void MoveSelectedHost(int offset)
    {
        var host = SelectedHost;
        if (host is null)
            return;

        var position = _hosts.FindIndex(x => x.Id == host.Id);
        var targetPosition = position + offset;
        if (position < 0 || targetPosition < 0 || targetPosition >= _hosts.Count)
            return;

        (_hosts[position], _hosts[targetPosition]) = (_hosts[targetPosition], _hosts[position]);
        SwitchToManualOrderAndSave(host.Id);
    }

    private void miMoveUp_Click(object? sender, EventArgs e) => MoveSelectedHost(-1);
    private void miMoveDown_Click(object? sender, EventArgs e) => MoveSelectedHost(1);

    private void dgvHosts_MouseDown(object? sender, MouseEventArgs e)
    {
        var hit = dgvHosts.HitTest(e.X, e.Y);

        if (e.Button == MouseButtons.Left)
        {
            _dragStartPoint = e.Location;
            _dragHostId = hit.RowIndex >= 0 && dgvHosts.Rows[hit.RowIndex].Tag is Guid id ? id : null;
            return;
        }

        if (e.Button != MouseButtons.Right || hit.RowIndex < 0)
            return;

        dgvHosts.ClearSelection();
        dgvHosts.Rows[hit.RowIndex].Selected = true;
        dgvHosts.CurrentCell = dgvHosts.Rows[hit.RowIndex].Cells[Math.Max(0, hit.ColumnIndex)];
    }

    private void dgvHosts_MouseMove(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left || _dragHostId is null)
            return;

        var dragSize = SystemInformation.DragSize;
        var dragRect = new Rectangle(
            _dragStartPoint.X - dragSize.Width / 2,
            _dragStartPoint.Y - dragSize.Height / 2,
            dragSize.Width,
            dragSize.Height);

        if (dragRect.Contains(e.Location))
            return;

        var id = _dragHostId.Value;
        dgvHosts.DoDragDrop(id.ToString("D"), DragDropEffects.Move);
        _dragHostId = null;
    }

    private void dgvHosts_DragOver(object? sender, DragEventArgs e)
    {
        e.Effect = _dragHostId is not null ? DragDropEffects.Move : DragDropEffects.None;
    }

    private void dgvHosts_DragDrop(object? sender, DragEventArgs e)
    {
        if (_dragHostId is not Guid sourceId)
            return;

        var sourceIndex = _hosts.FindIndex(x => x.Id == sourceId);
        if (sourceIndex < 0)
            return;

        var host = _hosts[sourceIndex];
        var client = dgvHosts.PointToClient(new Point(e.X, e.Y));
        var hit = dgvHosts.HitTest(client.X, client.Y);
        var targetIndex = _hosts.Count;

        if (hit.RowIndex >= 0 && dgvHosts.Rows[hit.RowIndex].Tag is Guid targetId)
        {
            targetIndex = _hosts.FindIndex(x => x.Id == targetId);
            if (targetIndex < 0)
                targetIndex = _hosts.Count;
            else
            {
                var rowRect = dgvHosts.GetRowDisplayRectangle(hit.RowIndex, false);
                if (client.Y > rowRect.Top + rowRect.Height / 2)
                    targetIndex++;
            }
        }

        _hosts.RemoveAt(sourceIndex);
        if (targetIndex > sourceIndex)
            targetIndex--;
        targetIndex = Math.Clamp(targetIndex, 0, _hosts.Count);
        _hosts.Insert(targetIndex, host);
        _dragHostId = null;
        SwitchToManualOrderAndSave(host.Id);
    }

    private void RestoreSelection(Guid? hostId)
    {
        if (hostId is null || !_rows.TryGetValue(hostId.Value, out var row))
            return;
        row.Selected = true;
        if (row.Cells.Count > 0)
            dgvHosts.CurrentCell = row.Cells[Math.Min(colName.Index, row.Cells.Count - 1)];
    }

    // ------------------------ Tooltip ------------------------

    private void dgvHosts_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.RowIndex < 0 || dgvHosts.Rows[e.RowIndex].Tag is not Guid id)
            return;
        var host = _hosts.FirstOrDefault(x => x.Id == id);
        if (host is not null)
            e.ToolTipText = BuildHostTooltip(host);
    }

    private string BuildHostTooltip(MonitorHost host)
    {
        var state = host.IsChecking ? "Проверяется…" : host.State switch
        {
            HostState.Online => "Online",
            HostState.Warning when host.IsLatencyWarning => "Warning — высокая задержка",
            HostState.Warning => "Warning",
            HostState.Offline => "Offline",
            HostState.Disabled => "Отключен",
            _ => "Ожидание проверки"
        };
        var check = host.CheckType == CheckType.Ping ? "Ping (ICMP)" : $"TCP {host.TcpPort}";
        var lastCheck = host.LastCheckAt?.ToString("dd.MM.yyyy HH:mm:ss") ?? "ещё не выполнялась";
        var latency = host.LastLatencyMs is long ms ? $"{ms} ms" : "—";

        var lines = new List<string>
        {
            host.Name,
            $"Адрес: {host.Address}"
        };


        if (host.IsInMaintenance)
        {
            var maintenance = host.MaintenanceIndefinite
                ? "бессрочно"
                : host.MaintenanceUntil is DateTimeOffset until
                    ? $"до {until:dd.MM.yyyy HH:mm}"
                    : "активно";
            lines.Add($"◆ Обслуживание: {maintenance}");
        }

        lines.Add($"Состояние: {state}");
        lines.Add($"Проверка: {check} · каждые {GetCheckInterval(host)} сек · timeout {GetTimeout(host)} мс");
        if (GetLatencyWarningEnabled(host))
            lines.Add($"Warning задержки: ≥ {GetLatencyThreshold(host)} мс × {GetLatencyWarningAfter(host)} проверок");
        lines.Add($"Последняя проверка: {lastCheck}");
        lines.Add($"Отклик: {latency}");

        if (host.OfflineSince is DateTimeOffset offlineSince)
            lines.Add($"Недоступен с: {offlineSince:dd.MM.yyyy HH:mm:ss}");
        if (!string.IsNullOrWhiteSpace(host.LastError))
            lines.Add($"Последняя ошибка: {host.LastError}");
        if (!string.IsNullOrWhiteSpace(host.WebUrl))
            lines.Add("Web: настроен");
        if (host.RdpEnabled)
            lines.Add($"RDP: {host.Address}:{host.RdpPort}");

        lines.Add("Перетащите строку мышью для ручной сортировки.");
        return string.Join(Environment.NewLine, lines);
    }

    private void dgvHosts_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            EditSelectedHost();
    }

    private void dgvHosts_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0)
            return;

        var row = dgvHosts.Rows[e.RowIndex];

        if (row.Tag is not Guid id)
            return;

        var host = _hosts.FirstOrDefault(x => x.Id == id);
        if (host is null)
            return;

        if (e.ColumnIndex == colStatus.Index)
        {
            e.PaintBackground(e.CellBounds, true);
            var color = HostAccentColor(host);
            var glyph = StatusGlyph(host);
            using (var accentBrush = new SolidBrush(color))
            {
                var accentHeight = Math.Max(10, e.CellBounds.Height - 12);
                e.Graphics.FillRectangle(accentBrush, e.CellBounds.X, e.CellBounds.Y + 6, 3, accentHeight);
            }
            using var glyphFont = new Font("Segoe UI Symbol", 10F, FontStyle.Regular, GraphicsUnit.Point);
            TextRenderer.DrawText(
                e.Graphics,
                glyph,
                glyphFont,
                e.CellBounds,
                color,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding);
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
            e.Handled = true;
            return;
        }

        if (e.ColumnIndex == colName.Index)
        {
            e.PaintBackground(e.CellBounds, true);

            var textColor = host.State == HostState.Disabled ? UiTheme.TextMuted : UiTheme.TextPrimary;
            using var nameFont = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            var horizontalPadding = Math.Max(4, (int)Math.Round(5 * e.Graphics.DpiX / 96f));
            var nameBounds = new Rectangle(
                e.CellBounds.X + horizontalPadding,
                e.CellBounds.Y,
                Math.Max(4, e.CellBounds.Width - horizontalPadding * 2),
                e.CellBounds.Height);

            TextRenderer.DrawText(
                e.Graphics,
                host.Name,
                nameFont,
                nameBounds,
                textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

            e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
            e.Handled = true;
            return;
        }

        if (e.ColumnIndex == colResponse.Index)
        {
            e.PaintBackground(e.CellBounds, true);

            var fore = HostAccentColor(host);
            var textBounds = Rectangle.Inflate(e.CellBounds, -4, 0);
            using var statusFont = new Font("Segoe UI Semibold", 7.35F, FontStyle.Regular, GraphicsUnit.Point);
            TextRenderer.DrawText(
                e.Graphics,
                BuildResponseText(host),
                statusFont,
                textBounds,
                fore,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

            e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
            e.Handled = true;
        }
    }

    private static Color HostAccentColor(MonitorHost host)
    {
        if (host.IsChecking)
            return UiTheme.Accent;
        if (host.IsInMaintenance)
            return UiTheme.Maintenance;

        return host.State switch
        {
            HostState.Online => UiTheme.Success,
            HostState.Warning => UiTheme.Warning,
            HostState.Offline => UiTheme.Danger,
            _ => UiTheme.TextMuted
        };
    }

    private void dgvHosts_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvHosts.Rows[e.RowIndex].Tag is not Guid id)
            return;
        var host = _hosts.FirstOrDefault(x => x.Id == id);
        if (host is null)
            return;

        if (e.ColumnIndex == colStatus.Index || e.ColumnIndex == colResponse.Index)
        {
            e.CellStyle.ForeColor = host.IsChecking ? UiTheme.Accent : host.IsInMaintenance ? UiTheme.Maintenance : host.State switch
            {
                HostState.Online => UiTheme.Success,
                HostState.Warning => UiTheme.Warning,
                HostState.Offline => UiTheme.Danger,
                _ => UiTheme.TextMuted
            };
        }
    }

    private void cmsHost_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        var host = SelectedHost;
        if (host is null)
        {
            e.Cancel = true;
            return;
        }
        miOpenWeb.Enabled = !string.IsNullOrWhiteSpace(host.Address);
        miOpenRdp.Enabled = host.RdpEnabled && !string.IsNullOrWhiteSpace(host.Address);
        miDisable.Text = host.Enabled ? "Отключить мониторинг" : "Включить мониторинг";
        miMaintenance.Enabled = host.Enabled;
        miMaintenanceEnd.Enabled = host.IsInMaintenance;
        miMaintenance.Text = host.IsInMaintenance ? "◆ Режим обслуживания" : "Режим обслуживания";

        var hostIndex = _hosts.FindIndex(x => x.Id == host.Id);
        miMoveUp.Enabled = hostIndex > 0;
        miMoveDown.Enabled = hostIndex >= 0 && hostIndex < _hosts.Count - 1;
    }

    private void miMaintenance15m_Click(object? sender, EventArgs e) => SetMaintenance(TimeSpan.FromMinutes(15));
    private void miMaintenance1h_Click(object? sender, EventArgs e) => SetMaintenance(TimeSpan.FromHours(1));
    private void miMaintenance4h_Click(object? sender, EventArgs e) => SetMaintenance(TimeSpan.FromHours(4));

    private void miMaintenanceTomorrow_Click(object? sender, EventArgs e)
    {
        var local = DateTime.Today.AddDays(1).AddHours(9);
        SetMaintenanceUntil(new DateTimeOffset(local), indefinite: false);
    }

    private void miMaintenanceIndefinite_Click(object? sender, EventArgs e) =>
        SetMaintenanceUntil(null, indefinite: true);

    private void miMaintenanceEnd_Click(object? sender, EventArgs e)
    {
        var host = SelectedHost;
        if (host is null)
            return;
        host.ClearMaintenance();
        _hostRepository.Save(_hosts, _settings);
        AddOrUpdateRow(host);
        UpdateSummary();
    }

    private void SetMaintenance(TimeSpan duration) =>
        SetMaintenanceUntil(DateTimeOffset.Now.Add(duration), indefinite: false);

    private void SetMaintenanceUntil(DateTimeOffset? until, bool indefinite)
    {
        var host = SelectedHost;
        if (host is null)
            return;

        host.MaintenanceIndefinite = indefinite;
        host.MaintenanceUntil = indefinite ? null : until;
        _hostRepository.Save(_hosts, _settings);
        AddOrUpdateRow(host);
        UpdateSummary();
    }

    private async void miCheckNow_Click(object? sender, EventArgs e)
    {
        var host = SelectedHost;
        if (host is null)
            return;
        tslState.Text = $"Проверка {host.Name}…";
        try
        {
            await _monitoringService!.CheckNowAsync(host, _settings);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка проверки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            UpdateMonitoringUi();
        }
    }

    private void miOpenWeb_Click(object? sender, EventArgs e)
    {
        var host = SelectedHost;
        if (host is null)
            return;
        try { RemoteAccessService.OpenWeb(host.WebUrl, host.Address); }
        catch (Exception ex) { ShowOpenError("Web", ex); }
    }

    private void miOpenRdp_Click(object? sender, EventArgs e)
    {
        var host = SelectedHost;
        if (host is null)
            return;
        try { RemoteAccessService.OpenRdp(host.Address, host.RdpPort); }
        catch (Exception ex) { ShowOpenError("RDP", ex); }
    }

    private void miCopyAddress_Click(object? sender, EventArgs e)
    {
        var host = SelectedHost;
        if (host is not null && !string.IsNullOrWhiteSpace(host.Address))
            Clipboard.SetText(host.Address);
    }

    private void miEvents_Click(object? sender, EventArgs e)
    {
        var host = SelectedHost;
        if (host is null)
            return;
        using var form = new EventsForm(_eventRepository, _hosts, host.Id);
        form.ShowDialog(this);
    }

    private void miEdit_Click(object? sender, EventArgs e) => EditSelectedHost();

    private void EditSelectedHost()
    {
        var host = SelectedHost;
        if (host is null)
            return;

        using var form = new HostForm(host, _settings, _hostCheckService);
        if (form.ShowDialog(this) != DialogResult.OK || form.Host is null)
            return;

        var index = _hosts.FindIndex(x => x.Id == host.Id);
        if (index >= 0)
        {
            form.Host.SortOrder = host.SortOrder;
            _hosts[index] = form.Host;
        }
        SaveHostsRespectingSort(form.Host.Id);
        RestartMonitoringIfNeeded();
    }

    private void miDisable_Click(object? sender, EventArgs e)
    {
        var host = SelectedHost;
        if (host is null)
            return;
        host.Enabled = !host.Enabled;
        host.State = host.Enabled ? HostState.Unknown : HostState.Disabled;
        _hostRepository.Save(_hosts, _settings);
        RestartMonitoringIfNeeded();
    }

    private void miDelete_Click(object? sender, EventArgs e)
    {
        var host = SelectedHost;
        if (host is null)
            return;
        if (MessageBox.Show(this, $"Удалить хост «{host.Name}»?", "IP Monitor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _hosts.Remove(host);
        SaveHostsRespectingSort();
        RestartMonitoringIfNeeded();
    }

    private void ShowOpenError(string kind, Exception ex) =>
        MessageBox.Show(this, $"Не удалось открыть {kind}:\n{ex.Message}", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    private void MainForm_Resize(object? sender, EventArgs e)
    {
        if (_adjustingWindowHeight)
            return;

        if (WindowState == FormWindowState.Minimized && _settings.MinimizeToTray)
            Hide();
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (!_allowExit && e.CloseReason == CloseReason.UserClosing && _settings.MinimizeToTray)
        {
            e.Cancel = true;
            Hide();
            notifyIcon.ShowBalloonTip(1500, "IP Monitor", "Программа продолжает работать в области уведомлений.", ToolTipIcon.Info);
            return;
        }

        _uiTimer.Stop();
        _uiTimer.Dispose();
        notifyIcon.Visible = false;
    }

    private void notifyIcon_DoubleClick(object? sender, EventArgs e) => RestoreWindow();
    private void miTrayOpen_Click(object? sender, EventArgs e) => RestoreWindow();

    private void RestoreWindow()
    {
        Show();
        WindowState = FormWindowState.Normal;
        Activate();
    }

    private void miTrayMonitoring_Click(object? sender, EventArgs e) => btnMonitoring_Click(sender, e);

    private void miTrayExit_Click(object? sender, EventArgs e)
    {
        _allowExit = true;
        Close();
    }
}
