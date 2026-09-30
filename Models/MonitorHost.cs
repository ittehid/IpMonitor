using System.Text.Json.Serialization;

namespace IpMonitor.Models;

public sealed class MonitorHost
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "Новый хост";
    public string Address { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public string WebUrl { get; set; } = string.Empty;
    public bool RdpEnabled { get; set; }
    public int RdpPort { get; set; } = 3389;
    public CheckType CheckType { get; set; } = CheckType.Ping;
    public int TcpPort { get; set; } = 443;
    public bool UseGlobalMonitoringSettings { get; set; } = true;
    public int CheckIntervalSeconds { get; set; } = 5;
    public int TimeoutMilliseconds { get; set; } = 1000;
    public int OfflineAfterFailures { get; set; } = 3;
    public int OnlineAfterSuccesses { get; set; } = 2;
    public bool LatencyWarningEnabled { get; set; } = true;
    public int LatencyWarningThresholdMs { get; set; } = 100;
    public int LatencyWarningAfterChecks { get; set; } = 2;
    public int SortOrder { get; set; }

    // Режим обслуживания сохраняется между перезапусками.
    public DateTimeOffset? MaintenanceUntil { get; set; }
    public bool MaintenanceIndefinite { get; set; }

    [JsonIgnore] public HostState State { get; set; } = HostState.Unknown;
    [JsonIgnore] public long? LastLatencyMs { get; set; }
    [JsonIgnore] public DateTimeOffset? LastCheckAt { get; set; }
    [JsonIgnore] public DateTimeOffset? FailureStartedAt { get; set; }
    [JsonIgnore] public DateTimeOffset? OfflineSince { get; set; }
    [JsonIgnore] public int ConsecutiveFailures { get; set; }
    [JsonIgnore] public int ConsecutiveSuccesses { get; set; }
    [JsonIgnore] public int ConsecutiveHighLatency { get; set; }
    [JsonIgnore] public bool IsLatencyWarning { get; set; }
    [JsonIgnore] public bool IsConfirmedOffline { get; set; }
    [JsonIgnore] public bool IsChecking { get; set; }
    [JsonIgnore] public string? LastError { get; set; }
    [JsonIgnore] public bool OfflineNotificationSuppressedByMaintenance { get; set; }
    [JsonIgnore] public bool HasSuccessfulCheckThisRun { get; set; }

    [JsonIgnore]
    public bool IsInMaintenance => MaintenanceIndefinite ||
        (MaintenanceUntil is DateTimeOffset until && until > DateTimeOffset.Now);

    public void ClearMaintenance()
    {
        MaintenanceIndefinite = false;
        MaintenanceUntil = null;
        OfflineNotificationSuppressedByMaintenance = false;
    }

    public MonitorHost Clone() => (MonitorHost)MemberwiseClone();
}
