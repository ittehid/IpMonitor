namespace IpMonitor.Models;

/// <summary>
/// Один период недоступности хоста. В журнале одна такая запись отображается одной строкой.
/// </summary>
public sealed class OutageRecord
{
    // Идентификаторы исходных технических событий нужны только для точного удаления
    // выбранного завершённого периода из журнала.
    public Guid OfflineEventId { get; set; }
    public Guid RecoveryEventId { get; set; }
    public Guid HostId { get; set; }
    public string HostName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTimeOffset LostAt { get; set; }
    public DateTimeOffset? RecoveredAt { get; set; }

    /// <summary>
    /// True, если хост уже был недоступен, когда мониторинг начал его проверять.
    /// В этом случае LostAt — момент обнаружения, а не гарантированно реальный момент падения.
    /// </summary>
    public bool InitialDetection { get; set; }

    public bool IsOngoing => RecoveredAt is null;

    public TimeSpan GetDowntime(DateTimeOffset now)
    {
        var end = RecoveredAt ?? now;
        var value = end - LostAt;
        return value < TimeSpan.Zero ? TimeSpan.Zero : value;
    }
}
