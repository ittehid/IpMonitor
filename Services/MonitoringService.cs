using System.Collections.Concurrent;
using IpMonitor.Models;
using IpMonitor.Storage;

namespace IpMonitor.Services;

internal sealed class MonitoringService : IDisposable
{
    private readonly HostCheckService _checkService;
    private readonly EventRepository _events;
    private CancellationTokenSource? _cts;
    private List<Task> _workers = [];
    private AppSettings _settings = new();
    private ConcurrentDictionary<Guid, OutageRecord> _openOutages = new();

    public bool IsRunning => _cts is not null;

    public event Action<MonitorHost>? HostUpdated;
    public event Action<MonitorEvent>? EventRaised;

    public MonitoringService(HostCheckService checkService, EventRepository events)
    {
        _checkService = checkService;
        _events = events;
    }

    public void Start(IEnumerable<MonitorHost> hosts, AppSettings settings) =>
        StartCore(hosts, settings, resetRuntime: true);

    public void Restart(IEnumerable<MonitorHost> hosts, AppSettings settings) =>
        StartCore(hosts, settings, resetRuntime: false);

    private void StartCore(IEnumerable<MonitorHost> hosts, AppSettings settings, bool resetRuntime)
    {
        Stop();
        _settings = settings;
        _openOutages = new ConcurrentDictionary<Guid, OutageRecord>(
            _events.LoadOutages().Where(x => x.IsOngoing).ToDictionary(x => x.HostId));

        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        foreach (var host in hosts)
        {
            if (resetRuntime)
                ResetRuntime(host);

            if (!host.Enabled)
            {
                ResetRuntime(host);
                host.State = HostState.Disabled;
                HostUpdated?.Invoke(host);
                continue;
            }

            if (host.State == HostState.Disabled)
                ResetRuntime(host);

            _workers.Add(Task.Run(() => MonitorHostLoopAsync(host, token), token));
        }
    }

    public void Stop()
    {
        if (_cts is null)
            return;

        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
        _workers = [];
    }

    public async Task CheckNowAsync(MonitorHost host, AppSettings settings, CancellationToken cancellationToken = default)
    {
        if (!host.Enabled)
        {
            host.State = HostState.Disabled;
            HostUpdated?.Invoke(host);
            return;
        }

        await PerformCheckAsync(host, settings, cancellationToken);
    }

    private async Task MonitorHostLoopAsync(MonitorHost host, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await PerformCheckAsync(host, _settings, cancellationToken);
                var interval = host.UseGlobalMonitoringSettings
                    ? _settings.CheckIntervalSeconds
                    : host.CheckIntervalSeconds;
                await Task.Delay(TimeSpan.FromSeconds(Math.Max(1, interval)), cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch
            {
                try { await Task.Delay(1000, cancellationToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    private async Task PerformCheckAsync(MonitorHost host, AppSettings settings, CancellationToken cancellationToken)
    {
        var timeout = host.UseGlobalMonitoringSettings ? settings.TimeoutMilliseconds : host.TimeoutMilliseconds;
        var offlineAfter = host.UseGlobalMonitoringSettings ? settings.OfflineAfterFailures : host.OfflineAfterFailures;
        var onlineAfter = host.UseGlobalMonitoringSettings ? settings.OnlineAfterSuccesses : host.OnlineAfterSuccesses;
        var latencyWarningEnabled = host.UseGlobalMonitoringSettings ? settings.LatencyWarningEnabled : host.LatencyWarningEnabled;
        var latencyWarningThreshold = host.UseGlobalMonitoringSettings ? settings.LatencyWarningThresholdMs : host.LatencyWarningThresholdMs;
        var latencyWarningAfter = host.UseGlobalMonitoringSettings ? settings.LatencyWarningAfterChecks : host.LatencyWarningAfterChecks;

        offlineAfter = Math.Max(1, offlineAfter);
        onlineAfter = Math.Max(1, onlineAfter);
        latencyWarningThreshold = Math.Max(1, latencyWarningThreshold);
        latencyWarningAfter = Math.Max(1, latencyWarningAfter);

        var checkStartedAt = DateTimeOffset.Now;
        host.IsChecking = true;
        HostUpdated?.Invoke(host);

        CheckResult result;
        try
        {
            result = await _checkService.CheckAsync(host, Math.Max(100, timeout), cancellationToken);
        }
        catch
        {
            host.IsChecking = false;
            HostUpdated?.Invoke(host);
            throw;
        }

        var now = DateTimeOffset.Now;
        host.IsChecking = false;
        host.LastCheckAt = now;
        host.LastLatencyMs = result.Success ? result.LatencyMs : null;
        host.LastError = result.Success ? null : result.Error;

        if (result.Success)
        {
            host.ConsecutiveFailures = 0;
            host.FailureStartedAt = null;
            host.ConsecutiveSuccesses++;
            host.HasSuccessfulCheckThisRun = true;

            UpdateLatencyWarning(host, latencyWarningEnabled, latencyWarningThreshold, latencyWarningAfter);

            // Если приложение было закрыто во время Offline, незавершённый период остаётся
            // в журнале. После первого подтверждённого успешного ответа закрываем именно его,
            // а не создаём новый период.
            if (!host.IsConfirmedOffline && _openOutages.TryGetValue(host.Id, out var persistedOutage))
            {
                host.OfflineSince = persistedOutage.LostAt;

                if (host.ConsecutiveSuccesses >= onlineAfter)
                    CloseOutage(host, now, persistedOutage.LostAt);
                else
                {
                    host.State = HostState.Warning;
                    host.IsLatencyWarning = false;
                }
            }
            else if (host.IsConfirmedOffline)
            {
                if (host.ConsecutiveSuccesses >= onlineAfter)
                {
                    var downSince = host.OfflineSince ?? checkStartedAt;
                    CloseOutage(host, now, downSince);
                }
                else
                {
                    host.State = HostState.Warning;
                    host.IsLatencyWarning = false;
                }
            }
            else
            {
                host.State = host.IsLatencyWarning ? HostState.Warning : HostState.Online;
            }
        }
        else
        {
            host.ConsecutiveSuccesses = 0;
            host.ConsecutiveHighLatency = 0;
            host.IsLatencyWarning = false;
            host.ConsecutiveFailures++;
            host.FailureStartedAt ??= checkStartedAt;

            if (host.IsConfirmedOffline)
            {
                host.State = HostState.Offline;
            }
            else if (host.ConsecutiveFailures >= offlineAfter)
            {
                host.State = HostState.Offline;
                host.IsConfirmedOffline = true;

                // Если в журнале уже есть незавершённый Offline этого хоста (например,
                // приложение перезапустили, пока связь не восстановилась), продолжаем его.
                if (_openOutages.TryGetValue(host.Id, out var persistedOutage))
                {
                    host.OfflineSince = persistedOutage.LostAt;
                    if (host.IsInMaintenance)
                        host.OfflineNotificationSuppressedByMaintenance = true;
                }
                else
                {
                    host.OfflineSince = host.FailureStartedAt ?? checkStartedAt;
                    var initialDetection = !host.HasSuccessfulCheckThisRun;
                    var evt = CreateEvent(host, MonitorEventType.Offline, host.OfflineSince.Value, null, initialDetection);
                    _events.Append(evt);

                    _openOutages[host.Id] = new OutageRecord
                    {
                        HostId = host.Id,
                        HostName = host.Name,
                        Address = host.Address,
                        LostAt = host.OfflineSince.Value,
                        InitialDetection = initialDetection
                    };

                    if (host.IsInMaintenance)
                        host.OfflineNotificationSuppressedByMaintenance = true;
                    else
                        EventRaised?.Invoke(evt);
                }
            }
            else
            {
                host.State = HostState.Warning;
            }
        }

        // Если хост упал во время обслуживания и обслуживание уже закончилось,
        // один раз подаём обычное Offline-уведомление. В журнал второй дубль не пишется.
        if (!host.IsInMaintenance && host.IsConfirmedOffline && host.OfflineNotificationSuppressedByMaintenance)
        {
            host.OfflineNotificationSuppressedByMaintenance = false;
            EventRaised?.Invoke(CreateEvent(host, MonitorEventType.Offline, host.OfflineSince ?? now, null));
        }

        HostUpdated?.Invoke(host);
    }

    private void CloseOutage(MonitorHost host, DateTimeOffset now, DateTimeOffset downSince)
    {
        host.IsConfirmedOffline = false;
        var evt = CreateEvent(host, MonitorEventType.Online, now, now - downSince);
        host.OfflineSince = null;
        host.OfflineNotificationSuppressedByMaintenance = false;
        host.State = host.IsLatencyWarning ? HostState.Warning : HostState.Online;
        _events.Append(evt);
        _openOutages.TryRemove(host.Id, out _);

        if (!host.IsInMaintenance)
            EventRaised?.Invoke(evt);
    }

    private static void UpdateLatencyWarning(MonitorHost host, bool enabled, int thresholdMs, int afterChecks)
    {
        if (!enabled || host.LastLatencyMs is not long latency || latency < thresholdMs)
        {
            host.ConsecutiveHighLatency = 0;
            host.IsLatencyWarning = false;
            return;
        }

        host.ConsecutiveHighLatency++;
        host.IsLatencyWarning = host.ConsecutiveHighLatency >= afterChecks;
    }

    private static MonitorEvent CreateEvent(
        MonitorHost host,
        MonitorEventType type,
        DateTimeOffset timestamp,
        TimeSpan? downtime,
        bool initialDetection = false) => new()
    {
        HostId = host.Id,
        HostName = host.Name,
        Address = host.Address,
        Type = type,
        Timestamp = timestamp,
        Downtime = downtime,
        InitialDetection = initialDetection
    };

    private static void ResetRuntime(MonitorHost host)
    {
        host.State = host.Enabled ? HostState.Unknown : HostState.Disabled;
        host.LastLatencyMs = null;
        host.LastCheckAt = null;
        host.FailureStartedAt = null;
        host.OfflineSince = null;
        host.ConsecutiveFailures = 0;
        host.ConsecutiveSuccesses = 0;
        host.ConsecutiveHighLatency = 0;
        host.IsLatencyWarning = false;
        host.IsConfirmedOffline = false;
        host.IsChecking = false;
        host.LastError = null;
        host.OfflineNotificationSuppressedByMaintenance = false;
        host.HasSuccessfulCheckThisRun = false;
    }

    public void Dispose() => Stop();
}
