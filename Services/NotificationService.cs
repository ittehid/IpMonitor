using System.Media;
using IpMonitor.Models;

namespace IpMonitor.Services;

internal sealed class NotificationService
{
    private readonly NotifyIcon _notifyIcon;

    public NotificationService(NotifyIcon notifyIcon) => _notifyIcon = notifyIcon;

    public void Handle(MonitorEvent item, AppSettings settings)
    {
        if (item.Type == MonitorEventType.Offline && settings.SoundOnOffline)
            SystemSounds.Exclamation.Play();
        else if (item.Type == MonitorEventType.Online && settings.SoundOnOnline)
            SystemSounds.Asterisk.Play();

        if (!settings.TrayNotifications)
            return;

        var isOffline = item.Type == MonitorEventType.Offline;
        _notifyIcon.BalloonTipTitle = isOffline ? "Хост недоступен" : "Связь восстановлена";
        _notifyIcon.BalloonTipText = isOffline
            ? $"{item.HostName} ({item.Address})"
            : $"{item.HostName} ({item.Address}) — простой {FormatDuration(item.Downtime)}";
        _notifyIcon.BalloonTipIcon = isOffline ? ToolTipIcon.Warning : ToolTipIcon.Info;
        _notifyIcon.ShowBalloonTip(3500);
    }

    private static string FormatDuration(TimeSpan? value)
    {
        if (value is null)
            return "—";
        return value.Value.TotalDays >= 1
            ? $"{(int)value.Value.TotalDays}д {value.Value:hh\\:mm\\:ss}"
            : value.Value.ToString("hh\\:mm\\:ss");
    }
}
