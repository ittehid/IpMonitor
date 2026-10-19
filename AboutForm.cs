using System.Diagnostics;
using IpMonitor.UI;

namespace IpMonitor;

public partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
        UiTheme.Apply(this);
        UiTheme.StyleSecondaryButton(btnClose);

        lblVersion.ForeColor = UiTheme.TextMuted;
        lblDescription.ForeColor = UiTheme.TextSecondary;
        lblAuthor.ForeColor = UiTheme.TextSecondary;
        lblThanks.ForeColor = UiTheme.TextSecondary;
        linkEmail.LinkColor = UiTheme.Accent;
        linkEmail.ActiveLinkColor = UiTheme.AccentDark;
        linkEmail.VisitedLinkColor = UiTheme.Accent;
        linkGitHub.LinkColor = UiTheme.Accent;
        linkGitHub.ActiveLinkColor = UiTheme.AccentDark;
        linkGitHub.VisitedLinkColor = UiTheme.Accent;

        var version = typeof(AboutForm).Assembly.GetName().Version;
        if (version is not null)
            lblVersion.Text = $"Версия {version.Major}.{version.Minor}.{Math.Max(0, version.Build)}";

        if (Icon is not null)
            picIcon.Image = Icon.ToBitmap();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // CenterParent применяется WinForms только к модальным диалогам.
        // Учитываем рабочую область монитора, когда главное окно стоит у края.
        var workingArea = Screen.FromControl(Owner ?? this).WorkingArea;
        var parentBounds = Owner?.Bounds ?? workingArea;
        Location = new Point(
            Math.Clamp(parentBounds.Left + (parentBounds.Width - Width) / 2,
                workingArea.Left, Math.Max(workingArea.Left, workingArea.Right - Width)),
            Math.Clamp(parentBounds.Top + (parentBounds.Height - Height) / 2,
                workingArea.Top, Math.Max(workingArea.Top, workingArea.Bottom - Height)));
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    private void linkEmail_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        => OpenLink("mailto:ittehid@gmail.com", "Не удалось открыть почтовую программу");

    private void linkGitHub_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        => OpenLink("https://github.com/ittehid/IpMonitor", "Не удалось открыть GitHub");

    private void OpenLink(string target, string title)
    {
        try
        {
            Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
