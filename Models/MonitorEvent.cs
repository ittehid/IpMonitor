namespace IpMonitor.Models;

public sealed class MonitorEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HostId { get; set; }
    public string HostName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public MonitorEventType Type { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public TimeSpan? Downtime { get; set; }
    public bool InitialDetection { get; set; }
}
