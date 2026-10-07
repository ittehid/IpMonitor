using System.ComponentModel;

namespace IpMonitor;

partial class AboutForm
{
    private IContainer? components = null;
    private PictureBox picIcon;
    private Label lblTitle;
    private Label lblVersion;
    private Label lblDescription;
    private Label lblAuthor;
    private LinkLabel linkEmail;
    private LinkLabel linkGitHub;
    private Button btnClose;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        picIcon = new PictureBox();
        lblTitle = new Label();
        lblVersion = new Label();
        lblDescription = new Label();
        lblAuthor = new Label();
        linkEmail = new LinkLabel();
        linkGitHub = new LinkLabel();
        btnClose = new Button();
        ((ISupportInitialize)picIcon).BeginInit();
        SuspendLayout();

        picIcon.Location = new Point(16, 18);
        picIcon.Name = "picIcon";
        picIcon.Size = new Size(46, 46);
        picIcon.SizeMode = PictureBoxSizeMode.Zoom;
        picIcon.TabStop = false;

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI Semibold", 12.5F);
        lblTitle.Location = new Point(75, 18);
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "IP Monitor";

        lblVersion.AutoSize = true;
        lblVersion.Font = new Font("Segoe UI", 7.75F);
        lblVersion.Location = new Point(77, 45);
        lblVersion.Name = "lblVersion";
        lblVersion.Text = "Версия 2.0";

        lblDescription.Location = new Point(16, 78);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(300, 38);
        lblDescription.Text = "Компактный локальный мониторинг доступности\r\nсерверов, ПК и сетевых устройств.";

        lblAuthor.AutoSize = true;
        lblAuthor.Location = new Point(16, 124);
        lblAuthor.Name = "lblAuthor";
        lblAuthor.Text = "Разработчик: Alexandr Gedz";

        linkEmail.AutoSize = true;
        linkEmail.Location = new Point(16, 147);
        linkEmail.Name = "linkEmail";
        linkEmail.Text = "ittehid@gmail.com";
        linkEmail.LinkClicked += linkEmail_LinkClicked;

        linkGitHub.AutoSize = true;
        linkGitHub.Location = new Point(16, 169);
        linkGitHub.Name = "linkGitHub";
        linkGitHub.Text = "github.com/ittehid/IpMonitor";
        linkGitHub.LinkClicked += linkGitHub_LinkClicked;

        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Location = new Point(220, 200);
                btnClose.Name = "btnClose";
        btnClose.Size = new Size(96, 27);
        btnClose.Text = "Закрыть";

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        CancelButton = btnClose;
        BackColor = Color.FromArgb(243, 245, 247);
        ForeColor = Color.FromArgb(31, 41, 55);
        ClientSize = new Size(330, 240);
        Controls.Add(picIcon);
        Controls.Add(lblTitle);
        Controls.Add(lblVersion);
        Controls.Add(lblDescription);
        Controls.Add(lblAuthor);
        Controls.Add(linkEmail);
        Controls.Add(linkGitHub);
        Controls.Add(btnClose);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AboutForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "О программе — IP Monitor";
        ((ISupportInitialize)picIcon).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
