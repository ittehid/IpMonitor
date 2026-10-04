using System.Diagnostics;
using System.Media;
using IpMonitor.Models;
using IpMonitor.Storage;
using IpMonitor.UI;

namespace IpMonitor;

public partial class SettingsForm : Form
{
    public AppSettings? Settings { get; private set; }
    private readonly HostSortMode _sourceSortMode;

    public SettingsForm(AppSettings source)
    {
        InitializeComponent();
        _sourceSortMode = source.HostSortMode;
        ApplyThemeStyles();

        chkStartMonitoring.Checked = source.StartMonitoringOnLaunch;
        chkMinimizeToTray.Checked = source.MinimizeToTray;
        chkStartWithWindows.Checked = source.StartWithWindows;
        chkAlwaysOnTop.Checked = source.AlwaysOnTop;
        nudInterval.Value = Clamp(source.CheckIntervalSeconds, nudInterval.Minimum, nudInterval.Maximum);
        nudTimeout.Value = Clamp(source.TimeoutMilliseconds, nudTimeout.Minimum, nudTimeout.Maximum);
        nudFailures.Value = Clamp(source.OfflineAfterFailures, nudFailures.Minimum, nudFailures.Maximum);
        nudSuccesses.Value = Clamp(source.OnlineAfterSuccesses, nudSuccesses.Minimum, nudSuccesses.Maximum);
        chkLatencyWarning.Checked = source.LatencyWarningEnabled;
        nudLatencyThreshold.Value = Clamp(source.LatencyWarningThresholdMs, nudLatencyThreshold.Minimum, nudLatencyThreshold.Maximum);
        nudLatencyChecks.Value = Clamp(source.LatencyWarningAfterChecks, nudLatencyChecks.Minimum, nudLatencyChecks.Maximum);
        chkSoundOffline.Checked = source.SoundOnOffline;
        chkSoundOnline.Checked = source.SoundOnOnline;
        chkTrayNotifications.Checked = source.TrayNotifications;
        nudRetention.Value = Clamp(source.EventRetentionDays, nudRetention.Minimum, nudRetention.Maximum);
        chkAutoDelete.Checked = source.AutoDeleteOldEvents;
        chkAutoBackup.Checked = source.AutomaticBackupsEnabled;
        nudBackupInterval.Value = Clamp(source.BackupMinimumIntervalMinutes, nudBackupInterval.Minimum, nudBackupInterval.Maximum);
        nudBackupCopies.Value = Clamp(source.BackupMaxCopies, nudBackupCopies.Minimum, nudBackupCopies.Maximum);
        nudBackupRetention.Value = Clamp(source.BackupRetentionDays, nudBackupRetention.Minimum, nudBackupRetention.Maximum);
        lblDataPath.Text = AppPaths.DataDirectory;
        UpdateLatencyControls();
        UpdateBackupControls();
    }

    private static decimal Clamp(int value, decimal min, decimal max) => Math.Min(max, Math.Max(min, value));

    private AppSettings BuildSettings() => new()
    {
        StartMonitoringOnLaunch = chkStartMonitoring.Checked,
        MinimizeToTray = chkMinimizeToTray.Checked,
        StartWithWindows = chkStartWithWindows.Checked,
        AlwaysOnTop = chkAlwaysOnTop.Checked,
        HostSortMode = _sourceSortMode,
        CheckIntervalSeconds = (int)nudInterval.Value,
        TimeoutMilliseconds = (int)nudTimeout.Value,
        OfflineAfterFailures = (int)nudFailures.Value,
        OnlineAfterSuccesses = (int)nudSuccesses.Value,
        LatencyWarningEnabled = chkLatencyWarning.Checked,
        LatencyWarningThresholdMs = (int)nudLatencyThreshold.Value,
        LatencyWarningAfterChecks = (int)nudLatencyChecks.Value,
        SoundOnOffline = chkSoundOffline.Checked,
        SoundOnOnline = chkSoundOnline.Checked,
        TrayNotifications = chkTrayNotifications.Checked,
        EventRetentionDays = (int)nudRetention.Value,
        AutoDeleteOldEvents = chkAutoDelete.Checked,
        AutomaticBackupsEnabled = chkAutoBackup.Checked,
        BackupMinimumIntervalMinutes = (int)nudBackupInterval.Value,
        BackupMaxCopies = (int)nudBackupCopies.Value,
        BackupRetentionDays = (int)nudBackupRetention.Value
    };

    private void ApplyThemeStyles()
    {
        UiTheme.Apply(this);
        UiTheme.StylePrimaryButton(btnSave);
        UiTheme.StyleSecondaryButton(btnCancel);
        UiTheme.StyleSecondaryButton(btnTestOfflineSound);
        UiTheme.StyleSecondaryButton(btnTestOnlineSound);
        UiTheme.StyleSecondaryButton(btnOpenDataFolder);
        UiTheme.StyleSecondaryButton(btnBackupNow);
        UiTheme.StyleSecondaryButton(btnOpenBackupFolder);
        lblHeaderHint.ForeColor = UiTheme.TextMuted;
        lblGeneralHint.ForeColor = UiTheme.TextMuted;
        lblFailuresHint.ForeColor = UiTheme.TextMuted;
        lblSuccessesHint.ForeColor = UiTheme.TextMuted;
        lblLatencyHint.ForeColor = UiTheme.TextMuted;
        lblDataPath.ForeColor = UiTheme.TextMuted;
        lblBackupHint.ForeColor = UiTheme.TextMuted;
        pnlBottom.BackColor = UiTheme.Surface;
        Invalidate(true);
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        Settings = BuildSettings();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void chkLatencyWarning_CheckedChanged(object? sender, EventArgs e) => UpdateLatencyControls();
    private void chkAutoBackup_CheckedChanged(object? sender, EventArgs e) => UpdateBackupControls();

    private void UpdateLatencyControls()
    {
        var enabled = chkLatencyWarning.Checked;
        lblLatencyThreshold.Enabled = enabled;
        nudLatencyThreshold.Enabled = enabled;
        lblLatencyMs.Enabled = enabled;
        lblLatencyChecks.Enabled = enabled;
        nudLatencyChecks.Enabled = enabled;
        lblLatencyChecksUnit.Enabled = enabled;
    }

    private void UpdateBackupControls()
    {
        var enabled = chkAutoBackup.Checked;
        lblBackupInterval.Enabled = enabled;
        nudBackupInterval.Enabled = enabled;
        lblBackupMinutes.Enabled = enabled;
        lblBackupCopies.Enabled = enabled;
        nudBackupCopies.Enabled = enabled;
        lblBackupRetention.Enabled = enabled;
        nudBackupRetention.Enabled = enabled;
        lblBackupDays.Enabled = enabled;
    }

    private void btnTestOfflineSound_Click(object? sender, EventArgs e) => SystemSounds.Exclamation.Play();
    private void btnTestOnlineSound_Click(object? sender, EventArgs e) => SystemSounds.Asterisk.Play();

    private void btnOpenDataFolder_Click(object? sender, EventArgs e) => OpenFolder(AppPaths.DataDirectory);
    private void btnOpenBackupFolder_Click(object? sender, EventArgs e) => OpenFolder(AppPaths.BackupsDirectory);

    private void btnBackupNow_Click(object? sender, EventArgs e)
    {
        try
        {
            var path = BackupService.CreateBackupIfNeeded(BuildSettings(), force: true);
            if (path is null)
            {
                MessageBox.Show(this, "Пока нечего сохранять: файлы данных ещё не созданы.", "Резервная копия", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show(this, $"Резервная копия создана:\n{path}", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Не удалось создать резервную копию:\n{ex.Message}", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void OpenFolder(string path)
    {
        AppPaths.EnsureCreated();
        Directory.CreateDirectory(path);
        try { Process.Start(new ProcessStartInfo(path) { UseShellExecute = true }); }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
