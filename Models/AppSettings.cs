namespace IpMonitor.Models;

public sealed class AppSettings
{
    public bool StartMonitoringOnLaunch { get; set; } = true;
    public bool MinimizeToTray { get; set; } = true;
    public bool StartWithWindows { get; set; }
    public bool AlwaysOnTop { get; set; }
    public HostSortMode HostSortMode { get; set; } = HostSortMode.Manual;

    public int CheckIntervalSeconds { get; set; } = 5;
    public int TimeoutMilliseconds { get; set; } = 1000;
    public int OfflineAfterFailures { get; set; } = 3;
    public int OnlineAfterSuccesses { get; set; } = 2;

    // Warning по высокой задержке. Применяется только к успешным Ping/TCP-проверкам.
    public bool LatencyWarningEnabled { get; set; } = true;
    public int LatencyWarningThresholdMs { get; set; } = 100;
    public int LatencyWarningAfterChecks { get; set; } = 2;

    public bool SoundOnOffline { get; set; } = true;
    public bool SoundOnOnline { get; set; } = false;
    public bool TrayNotifications { get; set; } = true;

    public int EventRetentionDays { get; set; } = 30;
    public bool AutoDeleteOldEvents { get; set; } = true;

    // Автоматические резервные копии hosts.json + settings.json.
    public bool AutomaticBackupsEnabled { get; set; } = true;
    public int BackupMinimumIntervalMinutes { get; set; } = 15;
    public int BackupMaxCopies { get; set; } = 20;
    public int BackupRetentionDays { get; set; } = 30;
}
