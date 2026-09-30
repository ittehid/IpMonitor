namespace IpMonitor.Storage;

internal static class AppPaths
{
    public static readonly string DataDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "IpMonitor");

    public static readonly string HostsFile = Path.Combine(DataDirectory, "hosts.json");
    public static readonly string SettingsFile = Path.Combine(DataDirectory, "settings.json");
    public static readonly string EventsFile = Path.Combine(DataDirectory, "events.jsonl");
    public static readonly string BackupsDirectory = Path.Combine(DataDirectory, "Backups");

    public static void EnsureCreated()
    {
        Directory.CreateDirectory(DataDirectory);
        Directory.CreateDirectory(BackupsDirectory);
    }
}
