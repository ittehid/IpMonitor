namespace IpMonitor.Models;

public readonly record struct CheckResult(bool Success, long? LatencyMs, string? Error = null);
