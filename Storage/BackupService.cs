using System.IO.Compression;
using IpMonitor.Models;

namespace IpMonitor.Storage;

internal static class BackupService
{
    private const string Prefix = "IpMonitor_backup_";

    public static string? CreateBackupIfNeeded(AppSettings settings, bool force = false)
    {
        AppPaths.EnsureCreated();
        Cleanup(settings);

        if (!force && !settings.AutomaticBackupsEnabled)
            return null;
        if (!File.Exists(AppPaths.HostsFile) && !File.Exists(AppPaths.SettingsFile))
            return null;

        if (!force)
        {
            var newest = EnumerateBackups().OrderByDescending(File.GetLastWriteTimeUtc).FirstOrDefault();
            if (newest is not null)
            {
                var age = DateTime.UtcNow - File.GetLastWriteTimeUtc(newest);
                if (age < TimeSpan.FromMinutes(Math.Max(1, settings.BackupMinimumIntervalMinutes)))
                    return null;
            }
        }

        var path = Path.Combine(AppPaths.BackupsDirectory,
            $"{Prefix}{DateTime.Now:yyyyMMdd_HHmmss_fff}.zip");

        using (var archive = ZipFile.Open(path, ZipArchiveMode.Create))
        {
            AddIfExists(archive, AppPaths.HostsFile, "hosts.json");
            AddIfExists(archive, AppPaths.SettingsFile, "settings.json");
        }

        Cleanup(settings);
        return path;
    }

    public static void Cleanup(AppSettings settings)
    {
        AppPaths.EnsureCreated();
        var files = EnumerateBackups()
            .OrderByDescending(File.GetLastWriteTimeUtc)
            .ToList();

        var cutoff = DateTime.UtcNow.AddDays(-Math.Max(1, settings.BackupRetentionDays));
        foreach (var file in files.Where(x => File.GetLastWriteTimeUtc(x) < cutoff).ToList())
        {
            TryDelete(file);
            files.Remove(file);
        }

        var maxCopies = Math.Max(1, settings.BackupMaxCopies);
        foreach (var file in files.Skip(maxCopies))
            TryDelete(file);
    }

    private static IEnumerable<string> EnumerateBackups()
    {
        if (!Directory.Exists(AppPaths.BackupsDirectory))
            return [];
        return Directory.EnumerateFiles(AppPaths.BackupsDirectory, $"{Prefix}*.zip", SearchOption.TopDirectoryOnly);
    }

    private static void AddIfExists(ZipArchive archive, string path, string entryName)
    {
        if (File.Exists(path))
            archive.CreateEntryFromFile(path, entryName, CompressionLevel.Optimal);
    }

    private static void TryDelete(string file)
    {
        try { File.Delete(file); }
        catch { /* Очистка старых копий не должна мешать работе мониторинга. */ }
    }
}
