using System.Xml.Linq;
using IpMonitor.Models;

namespace IpMonitor.Storage;

internal static class LegacyHostImporter
{
    public static List<MonitorHost> TryImport()
    {
        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "IPMonitor", "data.xml"),
            Path.Combine(Environment.CurrentDirectory, "IPMonitor", "data.xml"),
            Path.Combine(AppContext.BaseDirectory, "data.xml"),
            Path.Combine(Environment.CurrentDirectory, "data.xml")
        }.Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var path in candidates)
        {
            if (!File.Exists(path))
                continue;

            try
            {
                var result = ImportFile(path);
                if (result.Count > 0)
                    return result;
            }
            catch
            {
                // Неудачный автоматический импорт старого файла не мешает запуску.
            }
        }

        return [];
    }

    public static List<MonitorHost> ImportFile(string path)
    {
        var doc = XDocument.Load(path);
        return doc.Descendants("DataItem")
            .Select((node, index) => new MonitorHost
            {
                Name = ((string?)node.Element("HostName") ?? string.Empty).Trim(),
                Address = ((string?)node.Element("IpAddress") ?? string.Empty).Trim(),
                Enabled = true,
                SortOrder = index
            })
            .Where(x => !string.IsNullOrWhiteSpace(x.Address))
            .ToList();
    }
}
