using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using IpMonitor.Models;

namespace IpMonitor.Services;

internal sealed class HostCheckService
{
    public async Task<CheckResult> CheckAsync(MonitorHost host, int timeoutMs, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(host.Address))
            return new CheckResult(false, null, "Не указан адрес");

        return host.CheckType == CheckType.Tcp
            ? await CheckTcpAsync(host.Address.Trim(), host.TcpPort, timeoutMs, cancellationToken)
            : await CheckPingAsync(host.Address.Trim(), timeoutMs, cancellationToken);
    }

    private static async Task<CheckResult> CheckPingAsync(string address, int timeoutMs, CancellationToken cancellationToken)
    {
        try
        {
            using var ping = new Ping();
            var stopwatch = Stopwatch.StartNew();
            var reply = await ping.SendPingAsync(address, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken: cancellationToken);
            stopwatch.Stop();
            return reply.Status == IPStatus.Success
                ? new CheckResult(true, reply.RoundtripTime >= 0 ? reply.RoundtripTime : stopwatch.ElapsedMilliseconds)
                : new CheckResult(false, null, reply.Status.ToString());
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            return new CheckResult(false, null, ex.Message);
        }
    }

    private static async Task<CheckResult> CheckTcpAsync(string address, int port, int timeoutMs, CancellationToken cancellationToken)
    {
        try
        {
            using var client = new TcpClient();
            var stopwatch = Stopwatch.StartNew();
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(timeoutMs);
            await client.ConnectAsync(address, port, timeoutCts.Token);
            stopwatch.Stop();
            return new CheckResult(true, stopwatch.ElapsedMilliseconds);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            return new CheckResult(false, null, ex.Message);
        }
    }
}
