using System.Text.Json;
using System.Xml.Linq;
using IpMonitor.Models;

namespace IpMonitor.Storage;

internal static class HostTransferService
{
    public static List<MonitorHost> Import(string path)
    {
        if (path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            return LegacyHostImporter.ImportFile(path);

        var json = File.ReadAllText(path);
        using var parsed = JsonDocument.Parse(json);
        List<MonitorHost>? hosts;

        if (parsed.RootElement.ValueKind == JsonValueKind.Array)
        {
            // Совместимость с ранними hosts.json из ветки 2.0.
            hosts = JsonSerializer.Deserialize<List<MonitorHost>>(json, JsonOptions.Default);
        }
        else
        {
            hosts = JsonSerializer.Deserialize<HostTransferDocument>(json, JsonOptions.Default)?.Hosts;
        }

        hosts ??= [];
        ResetRuntime(hosts);
        return hosts.OrderBy(x => x.SortOrder).ToList();
    }

    public static void Export(string path, IEnumerable<MonitorHost> source)
    {
        var hosts = source.Select(x => x.Clone()).ToList();
        for (var i = 0; i < hosts.Count; i++)
            hosts[i].SortOrder = i;

        if (path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
        {
            ExportLegacyXml(path, hosts);
            return;
        }

        var document = new HostTransferDocument
        {
            Format = "IpMonitor.Hosts",
            SchemaVersion = 3,
            ExportedAt = DateTimeOffset.Now,
            Hosts = hosts
        };
        File.WriteAllText(path, JsonSerializer.Serialize(document, JsonOptions.Default));
    }

    private static void ExportLegacyXml(string path, IEnumerable<MonitorHost> hosts)
    {
        var document = new XDocument(
            new XElement("ArrayOfDataItem",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                hosts.Select(host => new XElement("DataItem",
                    new XElement("HostName", host.Name),
                    new XElement("IpAddress", host.Address)))));
        document.Save(path);
    }

    private static void ResetRuntime(IEnumerable<MonitorHost> hosts)
    {
        var ids = new HashSet<Guid>();
        var index = 0;
        foreach (var host in hosts)
        {
            if (host.Id == Guid.Empty || !ids.Add(host.Id))
            {
                host.Id = Guid.NewGuid();
                ids.Add(host.Id);
            }
            host.SortOrder = index++;
            if (!host.MaintenanceIndefinite && host.MaintenanceUntil is DateTimeOffset until && until <= DateTimeOffset.Now)
                host.MaintenanceUntil = null;
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
        }
    }

    private sealed class HostTransferDocument
    {
        public string Format { get; set; } = "IpMonitor.Hosts";
        public int SchemaVersion { get; set; } = 3;
        public DateTimeOffset? ExportedAt { get; set; }
        public List<MonitorHost> Hosts { get; set; } = [];
    }
}
