using System.Diagnostics;

namespace IpMonitor.Services;

internal static class RemoteAccessService
{
    public static void OpenWeb(string url, string address)
    {
        var target = string.IsNullOrWhiteSpace(url) ? $"http://{address}" : url.Trim();
        if (!Uri.TryCreate(target, UriKind.Absolute, out var uri))
            target = $"http://{target}";

        Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
    }

    public static void OpenRdp(string address, int port)
    {
        var target = port == 3389 ? address : $"{address}:{port}";
        Process.Start(new ProcessStartInfo("mstsc.exe", $"/v:\"{target}\"")
        {
            UseShellExecute = true
        });
    }
}
