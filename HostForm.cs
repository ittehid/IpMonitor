using IpMonitor.Models;
using IpMonitor.Services;
using IpMonitor.UI;

namespace IpMonitor;

public partial class HostForm : Form
{
    private readonly AppSettings _settings;
    private readonly HostCheckService _checkService;
    private readonly Guid _originalId;
    private readonly int _originalSortOrder;
    private readonly DateTimeOffset? _originalMaintenanceUntil;
    private readonly bool _originalMaintenanceIndefinite;
    private bool _advancedVisible;

    public MonitorHost? Host { get; private set; }

    // Parameterless constructor is intentionally kept for the WinForms Designer.
    // Runtime code uses the constructor with host/settings/check service below.
    public HostForm()
    {
        InitializeComponent();
        _settings = new AppSettings();
        _checkService = new HostCheckService();
        _originalId = Guid.Empty;
        _originalSortOrder = 0;
        _originalMaintenanceUntil = null;
        _originalMaintenanceIndefinite = false;
    }

    internal HostForm(
        MonitorHost? host,
        AppSettings settings,
        HostCheckService checkService,
        IEnumerable<string>? knownGroups = null)
    {
        InitializeComponent();
        UiTheme.Apply(this);
        UiTheme.StylePrimaryButton(btnSave);
        UiTheme.StyleSecondaryButton(btnCancel);
        UiTheme.StyleSecondaryButton(btnTest);
        UiTheme.StyleSecondaryButton(btnOpenWeb);
        UiTheme.StyleGhostButton(btnAdvanced);
        lblHeaderHint.ForeColor = UiTheme.TextMuted;
        lblTestResult.ForeColor = UiTheme.TextMuted;
        pnlBottom.BackColor = UiTheme.Surface;

        _settings = settings;
        _checkService = checkService;
        _originalId = host?.Id ?? Guid.NewGuid();
        _originalSortOrder = host?.SortOrder ?? 0;
        _originalMaintenanceUntil = host?.MaintenanceUntil;
        _originalMaintenanceIndefinite = host?.MaintenanceIndefinite ?? false;

        foreach (var group in (knownGroups ?? []).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.CurrentCultureIgnoreCase).OrderBy(x => x))
            cboGroup.Items.Add(group.Trim());

        if (host is null)
        {
            lblHeader.Text = "Добавление хоста";
            cboCheckType.SelectedIndex = 0;
            nudInterval.Value = Clamp(settings.CheckIntervalSeconds, nudInterval.Minimum, nudInterval.Maximum);
            nudTimeout.Value = Clamp(settings.TimeoutMilliseconds, nudTimeout.Minimum, nudTimeout.Maximum);
            nudFailures.Value = Clamp(settings.OfflineAfterFailures, nudFailures.Minimum, nudFailures.Maximum);
            nudSuccesses.Value = Clamp(settings.OnlineAfterSuccesses, nudSuccesses.Minimum, nudSuccesses.Maximum);
            chkLatencyWarning.Checked = settings.LatencyWarningEnabled;
            nudLatencyThreshold.Value = Clamp(settings.LatencyWarningThresholdMs, nudLatencyThreshold.Minimum, nudLatencyThreshold.Maximum);
            nudLatencyChecks.Value = Clamp(settings.LatencyWarningAfterChecks, nudLatencyChecks.Minimum, nudLatencyChecks.Maximum);
        }
        else
        {
            lblHeader.Text = "Редактирование хоста";
            txtName.Text = host.Name;
            txtAddress.Text = host.Address;
            cboGroup.Text = host.Group;
            chkEnabled.Checked = host.Enabled;
            txtWebUrl.Text = host.WebUrl;
            chkRdpEnabled.Checked = host.RdpEnabled;
            nudRdpPort.Value = Clamp(host.RdpPort, nudRdpPort.Minimum, nudRdpPort.Maximum);
            cboCheckType.SelectedIndex = host.CheckType == CheckType.Tcp ? 1 : 0;
            nudTcpPort.Value = Clamp(host.TcpPort, nudTcpPort.Minimum, nudTcpPort.Maximum);
            chkUseGlobal.Checked = host.UseGlobalMonitoringSettings;
            nudInterval.Value = Clamp(host.CheckIntervalSeconds, nudInterval.Minimum, nudInterval.Maximum);
            nudTimeout.Value = Clamp(host.TimeoutMilliseconds, nudTimeout.Minimum, nudTimeout.Maximum);
            nudFailures.Value = Clamp(host.OfflineAfterFailures, nudFailures.Minimum, nudFailures.Maximum);
            nudSuccesses.Value = Clamp(host.OnlineAfterSuccesses, nudSuccesses.Minimum, nudSuccesses.Maximum);
            chkLatencyWarning.Checked = host.LatencyWarningEnabled;
            nudLatencyThreshold.Value = Clamp(host.LatencyWarningThresholdMs, nudLatencyThreshold.Minimum, nudLatencyThreshold.Maximum);
            nudLatencyChecks.Value = Clamp(host.LatencyWarningAfterChecks, nudLatencyChecks.Minimum, nudLatencyChecks.Maximum);
        }

        SetAdvancedVisible(false);
        UpdateRdpControls();
        UpdateCheckControls();
        UpdateOverrideControls();
        UpdateLatencyControls();
    }

    private static decimal Clamp(int value, decimal min, decimal max) => Math.Min(max, Math.Max(min, value));

    private MonitorHost BuildHost() => new()
    {
        Id = _originalId,
        SortOrder = _originalSortOrder,
        Name = txtName.Text.Trim(),
        Address = txtAddress.Text.Trim(),
        Group = cboGroup.Text.Trim(),
        Enabled = chkEnabled.Checked,
        WebUrl = txtWebUrl.Text.Trim(),
        RdpEnabled = chkRdpEnabled.Checked,
        RdpPort = (int)nudRdpPort.Value,
        CheckType = cboCheckType.SelectedIndex == 1 ? CheckType.Tcp : CheckType.Ping,
        TcpPort = (int)nudTcpPort.Value,
        UseGlobalMonitoringSettings = chkUseGlobal.Checked,
        CheckIntervalSeconds = (int)nudInterval.Value,
        TimeoutMilliseconds = (int)nudTimeout.Value,
        OfflineAfterFailures = (int)nudFailures.Value,
        OnlineAfterSuccesses = (int)nudSuccesses.Value,
        LatencyWarningEnabled = chkLatencyWarning.Checked,
        LatencyWarningThresholdMs = (int)nudLatencyThreshold.Value,
        LatencyWarningAfterChecks = (int)nudLatencyChecks.Value,
        MaintenanceUntil = _originalMaintenanceUntil,
        MaintenanceIndefinite = _originalMaintenanceIndefinite
    };

    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show(this, "Укажите название хоста.", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtName.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(txtAddress.Text))
        {
            MessageBox.Show(this, "Укажите IP-адрес или DNS-имя.", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtAddress.Focus();
            return;
        }

        Host = BuildHost();
        DialogResult = DialogResult.OK;
        Close();
    }

    private async void btnTest_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtAddress.Text))
        {
            lblTestResult.Text = "Укажите адрес для проверки";
            lblTestResult.ForeColor = UiTheme.Warning;
            return;
        }

        btnTest.Enabled = false;
        lblTestResult.Text = "Проверка...";
        lblTestResult.ForeColor = UiTheme.TextMuted;
        try
        {
            var host = BuildHost();
            var timeout = host.UseGlobalMonitoringSettings ? _settings.TimeoutMilliseconds : host.TimeoutMilliseconds;
            var result = await _checkService.CheckAsync(host, timeout, CancellationToken.None);
            if (result.Success)
            {
                lblTestResult.Text = $"● Доступен — {result.LatencyMs ?? 0} ms";
                lblTestResult.ForeColor = UiTheme.Success;
            }
            else
            {
                lblTestResult.Text = "● Не отвечает";
                lblTestResult.ForeColor = UiTheme.Danger;
            }
        }
        catch (Exception ex)
        {
            lblTestResult.Text = ex.Message;
            lblTestResult.ForeColor = UiTheme.Danger;
        }
        finally
        {
            btnTest.Enabled = true;
        }
    }

    private void btnOpenWeb_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtAddress.Text) && string.IsNullOrWhiteSpace(txtWebUrl.Text))
            return;
        try { RemoteAccessService.OpenWeb(txtWebUrl.Text, txtAddress.Text); }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Не удалось открыть Web", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnAdvanced_Click(object? sender, EventArgs e) => SetAdvancedVisible(!_advancedVisible);

    private void SetAdvancedVisible(bool visible)
    {
        _advancedVisible = visible;
        grpAdvanced.Visible = visible;
        btnAdvanced.Text = visible ? "▾ Дополнительные настройки" : "▸ Дополнительные настройки";
        // Не меняем ClientSize после DPI/Font scaling: это ломало высоту формы на масштабировании Windows.
        // Расширенный блок находится в прокручиваемой области и безопасно появляется/скрывается.
    }

    private void chkRdpEnabled_CheckedChanged(object? sender, EventArgs e) => UpdateRdpControls();
    private void cboCheckType_SelectedIndexChanged(object? sender, EventArgs e) => UpdateCheckControls();
    private void chkUseGlobal_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateOverrideControls();
        UpdateLatencyControls();
    }
    private void chkLatencyWarning_CheckedChanged(object? sender, EventArgs e) => UpdateLatencyControls();

    private void UpdateRdpControls()
    {
        lblRdpPort.Enabled = chkRdpEnabled.Checked;
        nudRdpPort.Enabled = chkRdpEnabled.Checked;
    }

    private void UpdateCheckControls()
    {
        var tcp = cboCheckType.SelectedIndex == 1;
        lblTcpPort.Enabled = tcp;
        nudTcpPort.Enabled = tcp;
    }

    private void UpdateOverrideControls()
    {
        var enabled = !chkUseGlobal.Checked;
        lblInterval.Enabled = enabled;
        nudInterval.Enabled = enabled;
        lblIntervalUnit.Enabled = enabled;
        lblTimeout.Enabled = enabled;
        nudTimeout.Enabled = enabled;
        lblTimeoutUnit.Enabled = enabled;
        lblFailures.Enabled = enabled;
        nudFailures.Enabled = enabled;
        lblSuccesses.Enabled = enabled;
        nudSuccesses.Enabled = enabled;
        chkLatencyWarning.Enabled = enabled;
    }

    private void UpdateLatencyControls()
    {
        var enabled = !chkUseGlobal.Checked && chkLatencyWarning.Checked;
        lblLatencyThreshold.Enabled = enabled;
        nudLatencyThreshold.Enabled = enabled;
        lblLatencyMs.Enabled = enabled;
        lblLatencyChecks.Enabled = enabled;
        nudLatencyChecks.Enabled = enabled;
    }
}
