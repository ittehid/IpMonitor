using System.ComponentModel;

namespace IpMonitor;

partial class HostForm
{
    private IContainer? components = null;
    private TableLayoutPanel tlpRoot;
    private Panel pnlContent;
    private TableLayoutPanel tlpMain;
    private Label lblHeader;
    private Label lblHeaderHint;
    private Label lblName;
    private TextBox txtName;
    private Label lblAddress;
    private TableLayoutPanel tlpAddress;
    private TextBox txtAddress;
    private FlowLayoutPanel flpTest;
    private Button btnTest;
    private Label lblTestResult;
    private TableLayoutPanel tlpState;
    private CheckBox chkEnabled;
    private Label lblGroup;
    private ComboBox cboGroup;
    private GroupBox grpAccess;
    private TableLayoutPanel tlpAccess;
    private Label lblWeb;
    private TableLayoutPanel tlpWeb;
    private TextBox txtWebUrl;
    private Button btnOpenWeb;
    private FlowLayoutPanel flpRdp;
    private CheckBox chkRdpEnabled;
    private Label lblRdpPort;
    private NumericUpDown nudRdpPort;
    private Button btnAdvanced;
    private GroupBox grpAdvanced;
    private Label lblCheckType;
    private ComboBox cboCheckType;
    private Label lblTcpPort;
    private NumericUpDown nudTcpPort;
    private CheckBox chkUseGlobal;
    private Label lblInterval;
    private NumericUpDown nudInterval;
    private Label lblIntervalUnit;
    private Label lblTimeout;
    private NumericUpDown nudTimeout;
    private Label lblTimeoutUnit;
    private Label lblFailures;
    private NumericUpDown nudFailures;
    private Label lblSuccesses;
    private NumericUpDown nudSuccesses;
    private CheckBox chkLatencyWarning;
    private Label lblLatencyThreshold;
    private NumericUpDown nudLatencyThreshold;
    private Label lblLatencyMs;
    private Label lblLatencyChecks;
    private NumericUpDown nudLatencyChecks;
    private Label lblLatencyChecksUnit;
    private Panel pnlBottom;
    private Button btnCancel;
    private Button btnSave;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        tlpRoot = new TableLayoutPanel();
        pnlContent = new Panel();
        tlpMain = new TableLayoutPanel();
        lblHeader = new Label();
        lblHeaderHint = new Label();
        lblName = new Label();
        txtName = new TextBox();
        lblAddress = new Label();
        tlpAddress = new TableLayoutPanel();
        txtAddress = new TextBox();
        flpTest = new FlowLayoutPanel();
        btnTest = new Button();
        lblTestResult = new Label();
        tlpState = new TableLayoutPanel();
        chkEnabled = new CheckBox();
        lblGroup = new Label();
        cboGroup = new ComboBox();
        grpAccess = new GroupBox();
        tlpAccess = new TableLayoutPanel();
        lblWeb = new Label();
        tlpWeb = new TableLayoutPanel();
        txtWebUrl = new TextBox();
        btnOpenWeb = new Button();
        flpRdp = new FlowLayoutPanel();
        chkRdpEnabled = new CheckBox();
        lblRdpPort = new Label();
        nudRdpPort = new NumericUpDown();
        btnAdvanced = new Button();
        grpAdvanced = new GroupBox();
        lblCheckType = new Label();
        cboCheckType = new ComboBox();
        lblTcpPort = new Label();
        nudTcpPort = new NumericUpDown();
        chkUseGlobal = new CheckBox();
        lblInterval = new Label();
        nudInterval = new NumericUpDown();
        lblIntervalUnit = new Label();
        lblTimeout = new Label();
        nudTimeout = new NumericUpDown();
        lblTimeoutUnit = new Label();
        lblFailures = new Label();
        nudFailures = new NumericUpDown();
        lblSuccesses = new Label();
        nudSuccesses = new NumericUpDown();
        chkLatencyWarning = new CheckBox();
        lblLatencyThreshold = new Label();
        nudLatencyThreshold = new NumericUpDown();
        lblLatencyMs = new Label();
        lblLatencyChecks = new Label();
        nudLatencyChecks = new NumericUpDown();
        lblLatencyChecksUnit = new Label();
        pnlBottom = new Panel();
        btnCancel = new Button();
        btnSave = new Button();
        tlpRoot.SuspendLayout();
        pnlContent.SuspendLayout();
        tlpMain.SuspendLayout();
        tlpAddress.SuspendLayout();
        flpTest.SuspendLayout();
        tlpState.SuspendLayout();
        grpAccess.SuspendLayout();
        tlpAccess.SuspendLayout();
        tlpWeb.SuspendLayout();
        flpRdp.SuspendLayout();
        ((ISupportInitialize)nudRdpPort).BeginInit();
        grpAdvanced.SuspendLayout();
        ((ISupportInitialize)nudTcpPort).BeginInit();
        ((ISupportInitialize)nudInterval).BeginInit();
        ((ISupportInitialize)nudTimeout).BeginInit();
        ((ISupportInitialize)nudFailures).BeginInit();
        ((ISupportInitialize)nudSuccesses).BeginInit();
        ((ISupportInitialize)nudLatencyThreshold).BeginInit();
        ((ISupportInitialize)nudLatencyChecks).BeginInit();
        pnlBottom.SuspendLayout();
        SuspendLayout();
        // 
        // tlpRoot
        // 
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.Controls.Add(pnlContent, 0, 0);
        tlpRoot.Controls.Add(pnlBottom, 0, 1);
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.Location = new Point(0, 0);
        tlpRoot.Margin = new Padding(0);
        tlpRoot.Name = "tlpRoot";
        tlpRoot.RowCount = 2;
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
        tlpRoot.Size = new Size(858, 875);
        tlpRoot.TabIndex = 0;
        // 
        // pnlContent
        // 
        pnlContent.AutoScroll = true;
        pnlContent.BackColor = Color.FromArgb(243, 245, 247);
        pnlContent.Controls.Add(tlpMain);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(0, 0);
        pnlContent.Margin = new Padding(0);
        pnlContent.Name = "pnlContent";
        pnlContent.Size = new Size(858, 791);
        pnlContent.TabIndex = 0;
        // 
        // tlpMain
        // 
        tlpMain.AutoSize = true;
        tlpMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpMain.ColumnCount = 1;
        tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpMain.Controls.Add(lblHeader, 0, 0);
        tlpMain.Controls.Add(lblHeaderHint, 0, 1);
        tlpMain.Controls.Add(lblName, 0, 2);
        tlpMain.Controls.Add(txtName, 0, 3);
        tlpMain.Controls.Add(lblAddress, 0, 4);
        tlpMain.Controls.Add(tlpAddress, 0, 5);
        tlpMain.Controls.Add(lblTestResult, 0, 6);
        tlpMain.Controls.Add(tlpState, 0, 7);
        tlpMain.Controls.Add(grpAccess, 0, 8);
        tlpMain.Controls.Add(btnAdvanced, 0, 9);
        tlpMain.Controls.Add(grpAdvanced, 0, 10);
        tlpMain.Dock = DockStyle.Top;
        tlpMain.Location = new Point(0, 0);
        tlpMain.Margin = new Padding(0);
        tlpMain.Name = "tlpMain";
        tlpMain.Padding = new Padding(24, 18, 24, 21);
        tlpMain.RowCount = 11;
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.RowStyles.Add(new RowStyle());
        tlpMain.Size = new Size(828, 1143);
        tlpMain.TabIndex = 0;
        // 
        // lblHeader
        // 
        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 11F);
        lblHeader.Location = new Point(24, 18);
        lblHeader.Margin = new Padding(0, 0, 0, 4);
        lblHeader.Name = "lblHeader";
        lblHeader.Size = new Size(240, 36);
        lblHeader.TabIndex = 0;
        lblHeader.Text = "Добавление хоста";
        // 
        // lblHeaderHint
        // 
        lblHeaderHint.AutoSize = true;
        lblHeaderHint.Font = new Font("Segoe UI", 7.4F);
        lblHeaderHint.Location = new Point(24, 58);
        lblHeaderHint.Margin = new Padding(0, 0, 0, 16);
        lblHeaderHint.Name = "lblHeaderHint";
        lblHeaderHint.Size = new Size(339, 25);
        lblHeaderHint.TabIndex = 1;
        lblHeaderHint.Text = "Достаточно названия и IP / DNS-имени.";
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(24, 99);
        lblName.Margin = new Padding(0, 0, 0, 5);
        lblName.Name = "lblName";
        lblName.Size = new Size(105, 30);
        lblName.TabIndex = 2;
        lblName.Text = "Название";
        // 
        // txtName
        // 
        txtName.Dock = DockStyle.Fill;
        txtName.Location = new Point(24, 134);
        txtName.Margin = new Padding(0, 0, 0, 16);
        txtName.Name = "txtName";
        txtName.Size = new Size(780, 35);
        txtName.TabIndex = 3;
        // 
        // lblAddress
        // 
        lblAddress.AutoSize = true;
        lblAddress.Location = new Point(24, 185);
        lblAddress.Margin = new Padding(0, 0, 0, 5);
        lblAddress.Name = "lblAddress";
        lblAddress.Size = new Size(229, 30);
        lblAddress.TabIndex = 4;
        lblAddress.Text = "IP-адрес или DNS-имя";
        // 
        // tlpAddress
        // 
        tlpAddress.AutoSize = true;
        tlpAddress.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpAddress.ColumnCount = 1;
        tlpAddress.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpAddress.Controls.Add(txtAddress, 0, 0);
        tlpAddress.Controls.Add(flpTest, 0, 1);
        tlpAddress.Dock = DockStyle.Fill;
        tlpAddress.Location = new Point(24, 220);
        tlpAddress.Margin = new Padding(0, 0, 0, 7);
        tlpAddress.Name = "tlpAddress";
        tlpAddress.RowCount = 2;
        tlpAddress.RowStyles.Add(new RowStyle());
        tlpAddress.RowStyles.Add(new RowStyle());
        tlpAddress.Size = new Size(780, 96);
        tlpAddress.TabIndex = 5;
        // 
        // txtAddress
        // 
        txtAddress.Dock = DockStyle.Fill;
        txtAddress.Location = new Point(0, 0);
        txtAddress.Margin = new Padding(0, 0, 0, 10);
        txtAddress.Name = "txtAddress";
        txtAddress.Size = new Size(780, 35);
        txtAddress.TabIndex = 0;
        txtAddress.TextChanged += txtAddress_TextChanged;
        // 
        // flpTest
        // 
        flpTest.AutoSize = true;
        flpTest.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpTest.Controls.Add(btnTest);
        flpTest.Dock = DockStyle.Fill;
        flpTest.Location = new Point(0, 45);
        flpTest.Margin = new Padding(0);
        flpTest.Name = "flpTest";
        flpTest.Size = new Size(780, 51);
        flpTest.TabIndex = 1;
        flpTest.WrapContents = false;
        // 
        // btnTest
        // 
        btnTest.Location = new Point(0, 0);
        btnTest.Margin = new Padding(0);
        btnTest.Name = "btnTest";
        btnTest.Size = new Size(182, 51);
        btnTest.TabIndex = 0;
        btnTest.Text = "Проверить";
        btnTest.UseVisualStyleBackColor = true;
        btnTest.Click += btnTest_Click;
        // 
        // lblTestResult
        // 
        lblTestResult.AutoSize = true;
        lblTestResult.Font = new Font("Segoe UI", 7.4F);
        lblTestResult.Location = new Point(24, 323);
        lblTestResult.Margin = new Padding(0, 0, 0, 14);
        lblTestResult.Name = "lblTestResult";
        lblTestResult.Size = new Size(269, 25);
        lblTestResult.TabIndex = 6;
        lblTestResult.Text = "Проверка ещё не выполнялась";
        // 
        // tlpState
        // 
        tlpState.AutoSize = true;
        tlpState.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpState.ColumnCount = 3;
        tlpState.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        tlpState.ColumnStyles.Add(new ColumnStyle());
        tlpState.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
        tlpState.Controls.Add(chkEnabled, 0, 0);
        tlpState.Controls.Add(lblGroup, 1, 0);
        tlpState.Controls.Add(cboGroup, 2, 0);
        tlpState.Dock = DockStyle.Fill;
        tlpState.Location = new Point(24, 362);
        tlpState.Margin = new Padding(0, 0, 0, 14);
        tlpState.Name = "tlpState";
        tlpState.RowCount = 1;
        tlpState.RowStyles.Add(new RowStyle());
        tlpState.Size = new Size(780, 38);
        tlpState.TabIndex = 7;
        // 
        // chkEnabled
        // 
        chkEnabled.AutoSize = true;
        chkEnabled.Checked = true;
        chkEnabled.CheckState = CheckState.Checked;
        chkEnabled.Dock = DockStyle.Fill;
        chkEnabled.Location = new Point(0, 0);
        chkEnabled.Margin = new Padding(0);
        chkEnabled.Name = "chkEnabled";
        chkEnabled.Size = new Size(283, 38);
        chkEnabled.TabIndex = 0;
        chkEnabled.Text = "Мониторинг";
        chkEnabled.UseVisualStyleBackColor = true;
        // 
        // lblGroup
        // 
        lblGroup.AutoSize = true;
        lblGroup.Dock = DockStyle.Fill;
        lblGroup.Location = new Point(297, 0);
        lblGroup.Margin = new Padding(14, 0, 7, 0);
        lblGroup.Name = "lblGroup";
        lblGroup.Size = new Size(85, 38);
        lblGroup.TabIndex = 1;
        lblGroup.Text = "Группа:";
        lblGroup.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboGroup
        // 
        cboGroup.Dock = DockStyle.Fill;
        cboGroup.FlatStyle = FlatStyle.Standard;
        cboGroup.FormattingEnabled = true;
        cboGroup.Location = new Point(389, 0);
        cboGroup.Margin = new Padding(0);
        cboGroup.MaxLength = 60;
        cboGroup.Name = "cboGroup";
        cboGroup.Size = new Size(391, 38);
        cboGroup.TabIndex = 2;
        // 
        // grpAccess
        // 
        grpAccess.AutoSize = true;
        grpAccess.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpAccess.Controls.Add(tlpAccess);
        grpAccess.Dock = DockStyle.Fill;
        grpAccess.Location = new Point(24, 414);
        grpAccess.Margin = new Padding(0, 0, 0, 14);
        grpAccess.Name = "grpAccess";
        grpAccess.Padding = new Padding(14, 10, 14, 14);
        grpAccess.Size = new Size(780, 194);
        grpAccess.TabIndex = 8;
        grpAccess.TabStop = false;
        grpAccess.Text = "Быстрый доступ";
        // 
        // tlpAccess
        // 
        tlpAccess.AutoSize = true;
        tlpAccess.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpAccess.ColumnCount = 1;
        tlpAccess.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpAccess.Controls.Add(lblWeb, 0, 0);
        tlpAccess.Controls.Add(tlpWeb, 0, 1);
        tlpAccess.Controls.Add(flpRdp, 0, 2);
        tlpAccess.Dock = DockStyle.Top;
        tlpAccess.Location = new Point(14, 38);
        tlpAccess.Margin = new Padding(0);
        tlpAccess.Name = "tlpAccess";
        tlpAccess.RowCount = 3;
        tlpAccess.RowStyles.Add(new RowStyle());
        tlpAccess.RowStyles.Add(new RowStyle());
        tlpAccess.RowStyles.Add(new RowStyle());
        tlpAccess.Size = new Size(752, 142);
        tlpAccess.TabIndex = 0;
        // 
        // lblWeb
        // 
        lblWeb.AutoSize = true;
        lblWeb.Font = new Font("Segoe UI", 7.3F);
        lblWeb.Location = new Point(0, 0);
        lblWeb.Margin = new Padding(0, 0, 0, 5);
        lblWeb.Name = "lblWeb";
        lblWeb.Size = new Size(215, 25);
        lblWeb.TabIndex = 0;
        lblWeb.Text = "Web (пусто = http://хост)";
        // 
        // tlpWeb
        // 
        tlpWeb.AutoSize = true;
        tlpWeb.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tlpWeb.ColumnCount = 2;
        tlpWeb.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpWeb.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 144F));
        tlpWeb.Controls.Add(txtWebUrl, 0, 0);
        tlpWeb.Controls.Add(btnOpenWeb, 1, 0);
        tlpWeb.Dock = DockStyle.Fill;
        tlpWeb.Location = new Point(0, 30);
        tlpWeb.Margin = new Padding(0, 0, 0, 10);
        tlpWeb.Name = "tlpWeb";
        tlpWeb.RowCount = 1;
        tlpWeb.RowStyles.Add(new RowStyle());
        tlpWeb.Size = new Size(752, 61);
        tlpWeb.TabIndex = 1;
        // 
        // txtWebUrl
        // 
        txtWebUrl.Dock = DockStyle.Fill;
        txtWebUrl.Location = new Point(0, 0);
        txtWebUrl.Margin = new Padding(0, 0, 12, 0);
        txtWebUrl.Name = "txtWebUrl";
        txtWebUrl.PlaceholderText = "http://192.168.1.1";
        txtWebUrl.Size = new Size(596, 35);
        txtWebUrl.TabIndex = 0;
        txtWebUrl.TextChanged += txtWebUrl_TextChanged;
        // 
        // btnOpenWeb
        // 
        btnOpenWeb.Dock = DockStyle.Fill;
        btnOpenWeb.Location = new Point(608, 0);
        btnOpenWeb.Margin = new Padding(0);
        btnOpenWeb.MinimumSize = new Size(0, 47);
        btnOpenWeb.Name = "btnOpenWeb";
        btnOpenWeb.Size = new Size(144, 61);
        btnOpenWeb.TabIndex = 1;
        btnOpenWeb.Text = "Открыть";
        btnOpenWeb.UseVisualStyleBackColor = true;
        btnOpenWeb.Click += btnOpenWeb_Click;
        // 
        // flpRdp
        // 
        flpRdp.AutoSize = true;
        flpRdp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpRdp.Controls.Add(chkRdpEnabled);
        flpRdp.Controls.Add(lblRdpPort);
        flpRdp.Controls.Add(nudRdpPort);
        flpRdp.Dock = DockStyle.Fill;
        flpRdp.Location = new Point(0, 101);
        flpRdp.Margin = new Padding(0);
        flpRdp.Name = "flpRdp";
        flpRdp.Size = new Size(752, 41);
        flpRdp.TabIndex = 2;
        flpRdp.WrapContents = false;
        // 
        // chkRdpEnabled
        // 
        chkRdpEnabled.AutoSize = true;
        chkRdpEnabled.Location = new Point(0, 7);
        chkRdpEnabled.Margin = new Padding(0, 7, 24, 0);
        chkRdpEnabled.Name = "chkRdpEnabled";
        chkRdpEnabled.Size = new Size(79, 34);
        chkRdpEnabled.TabIndex = 0;
        chkRdpEnabled.Text = "RDP";
        chkRdpEnabled.UseVisualStyleBackColor = true;
        chkRdpEnabled.CheckedChanged += chkRdpEnabled_CheckedChanged;
        // 
        // lblRdpPort
        // 
        lblRdpPort.AutoSize = true;
        lblRdpPort.Location = new Point(103, 10);
        lblRdpPort.Margin = new Padding(0, 10, 9, 0);
        lblRdpPort.Name = "lblRdpPort";
        lblRdpPort.Size = new Size(66, 30);
        lblRdpPort.TabIndex = 1;
        lblRdpPort.Text = "Порт:";
        // 
        // nudRdpPort
        // 
        nudRdpPort.Location = new Point(178, 0);
        nudRdpPort.Margin = new Padding(0);
        nudRdpPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudRdpPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudRdpPort.Name = "nudRdpPort";
        nudRdpPort.Size = new Size(126, 35);
        nudRdpPort.TabIndex = 2;
        nudRdpPort.Value = new decimal(new int[] { 3389, 0, 0, 0 });
        // 
        // btnAdvanced
        // 
        btnAdvanced.Dock = DockStyle.Fill;
        btnAdvanced.Location = new Point(24, 622);
        btnAdvanced.Margin = new Padding(0, 0, 0, 12);
        btnAdvanced.MinimumSize = new Size(0, 47);
        btnAdvanced.Name = "btnAdvanced";
        btnAdvanced.Size = new Size(780, 47);
        btnAdvanced.TabIndex = 9;
        btnAdvanced.Text = "▸ Дополнительные настройки";
        btnAdvanced.TextAlign = ContentAlignment.MiddleLeft;
        btnAdvanced.UseVisualStyleBackColor = true;
        btnAdvanced.Click += btnAdvanced_Click;
        // 
        // grpAdvanced
        // 
        grpAdvanced.Controls.Add(lblCheckType);
        grpAdvanced.Controls.Add(cboCheckType);
        grpAdvanced.Controls.Add(lblTcpPort);
        grpAdvanced.Controls.Add(nudTcpPort);
        grpAdvanced.Controls.Add(chkUseGlobal);
        grpAdvanced.Controls.Add(lblInterval);
        grpAdvanced.Controls.Add(nudInterval);
        grpAdvanced.Controls.Add(lblIntervalUnit);
        grpAdvanced.Controls.Add(lblTimeout);
        grpAdvanced.Controls.Add(nudTimeout);
        grpAdvanced.Controls.Add(lblTimeoutUnit);
        grpAdvanced.Controls.Add(lblFailures);
        grpAdvanced.Controls.Add(nudFailures);
        grpAdvanced.Controls.Add(lblSuccesses);
        grpAdvanced.Controls.Add(nudSuccesses);
        grpAdvanced.Controls.Add(chkLatencyWarning);
        grpAdvanced.Controls.Add(lblLatencyThreshold);
        grpAdvanced.Controls.Add(nudLatencyThreshold);
        grpAdvanced.Controls.Add(lblLatencyMs);
        grpAdvanced.Controls.Add(lblLatencyChecks);
        grpAdvanced.Controls.Add(nudLatencyChecks);
        grpAdvanced.Controls.Add(lblLatencyChecksUnit);
        grpAdvanced.Dock = DockStyle.Top;
        grpAdvanced.Location = new Point(24, 681);
        grpAdvanced.Margin = new Padding(0);
        grpAdvanced.Name = "grpAdvanced";
        grpAdvanced.Padding = new Padding(16, 12, 16, 16);
        grpAdvanced.Size = new Size(780, 441);
        grpAdvanced.TabIndex = 10;
        grpAdvanced.TabStop = false;
        grpAdvanced.Text = "Параметры проверки";
        // 
        // lblCheckType
        // 
        lblCheckType.AutoSize = true;
        lblCheckType.Location = new Point(32, 56);
        lblCheckType.Margin = new Padding(5, 0, 5, 0);
        lblCheckType.Name = "lblCheckType";
        lblCheckType.Size = new Size(80, 30);
        lblCheckType.TabIndex = 0;
        lblCheckType.Text = "Метод:";
        // 
        // cboCheckType
        // 
        cboCheckType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCheckType.FlatStyle = FlatStyle.Standard;
        cboCheckType.FormattingEnabled = true;
        cboCheckType.Items.AddRange(new object[] { "Ping (ICMP)", "TCP-порт" });
        cboCheckType.Location = new Point(122, 53);
        cboCheckType.Margin = new Padding(5, 5, 5, 5);
        cboCheckType.Name = "cboCheckType";
        cboCheckType.Size = new Size(224, 38);
        cboCheckType.TabIndex = 1;
        cboCheckType.SelectedIndexChanged += cboCheckType_SelectedIndexChanged;
        // 
        // lblTcpPort
        // 
        lblTcpPort.AutoSize = true;
        lblTcpPort.Location = new Point(389, 56);
        lblTcpPort.Margin = new Padding(5, 0, 5, 0);
        lblTcpPort.Name = "lblTcpPort";
        lblTcpPort.Size = new Size(106, 30);
        lblTcpPort.TabIndex = 2;
        lblTcpPort.Text = "TCP-порт:";
        // 
        // nudTcpPort
        // 
        nudTcpPort.Location = new Point(505, 54);
        nudTcpPort.Margin = new Padding(5, 5, 5, 5);
        nudTcpPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudTcpPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudTcpPort.Name = "nudTcpPort";
        nudTcpPort.Size = new Size(144, 35);
        nudTcpPort.TabIndex = 3;
        nudTcpPort.Value = new decimal(new int[] { 443, 0, 0, 0 });
        // 
        // chkUseGlobal
        // 
        chkUseGlobal.AutoSize = true;
        chkUseGlobal.Checked = true;
        chkUseGlobal.CheckState = CheckState.Checked;
        chkUseGlobal.Location = new Point(32, 119);
        chkUseGlobal.Margin = new Padding(5, 5, 5, 5);
        chkUseGlobal.Name = "chkUseGlobal";
        chkUseGlobal.Size = new Size(358, 34);
        chkUseGlobal.TabIndex = 4;
        chkUseGlobal.Text = "Использовать общие параметры";
        chkUseGlobal.UseVisualStyleBackColor = true;
        chkUseGlobal.CheckedChanged += chkUseGlobal_CheckedChanged;
        // 
        // lblInterval
        // 
        lblInterval.AutoSize = true;
        lblInterval.Location = new Point(32, 189);
        lblInterval.Margin = new Padding(5, 0, 5, 0);
        lblInterval.Name = "lblInterval";
        lblInterval.Size = new Size(111, 30);
        lblInterval.TabIndex = 5;
        lblInterval.Text = "Интервал:";
        // 
        // nudInterval
        // 
        nudInterval.Location = new Point(210, 182);
        nudInterval.Margin = new Padding(5, 5, 5, 5);
        nudInterval.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
        nudInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudInterval.Name = "nudInterval";
        nudInterval.Size = new Size(122, 35);
        nudInterval.TabIndex = 6;
        nudInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
        // 
        // lblIntervalUnit
        // 
        lblIntervalUnit.AutoSize = true;
        lblIntervalUnit.Location = new Point(342, 187);
        lblIntervalUnit.Margin = new Padding(5, 0, 5, 0);
        lblIntervalUnit.Name = "lblIntervalUnit";
        lblIntervalUnit.Size = new Size(23, 30);
        lblIntervalUnit.TabIndex = 7;
        lblIntervalUnit.Text = "с";
        // 
        // lblTimeout
        // 
        lblTimeout.AutoSize = true;
        lblTimeout.Location = new Point(410, 187);
        lblTimeout.Margin = new Padding(5, 0, 5, 0);
        lblTimeout.Name = "lblTimeout";
        lblTimeout.Size = new Size(94, 30);
        lblTimeout.TabIndex = 8;
        lblTimeout.Text = "Timeout:";
        // 
        // nudTimeout
        // 
        nudTimeout.Increment = new decimal(new int[] { 100, 0, 0, 0 });
        nudTimeout.Location = new Point(547, 187);
        nudTimeout.Margin = new Padding(5, 5, 5, 5);
        nudTimeout.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
        nudTimeout.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
        nudTimeout.Name = "nudTimeout";
        nudTimeout.Size = new Size(144, 35);
        nudTimeout.TabIndex = 9;
        nudTimeout.Value = new decimal(new int[] { 1000, 0, 0, 0 });
        // 
        // lblTimeoutUnit
        // 
        lblTimeoutUnit.AutoSize = true;
        lblTimeoutUnit.Location = new Point(701, 189);
        lblTimeoutUnit.Margin = new Padding(5, 0, 5, 0);
        lblTimeoutUnit.Name = "lblTimeoutUnit";
        lblTimeoutUnit.Size = new Size(38, 30);
        lblTimeoutUnit.TabIndex = 10;
        lblTimeoutUnit.Text = "мс";
        // 
        // lblFailures
        // 
        lblFailures.AutoSize = true;
        lblFailures.Location = new Point(32, 254);
        lblFailures.Margin = new Padding(5, 0, 5, 0);
        lblFailures.Name = "lblFailures";
        lblFailures.Size = new Size(143, 30);
        lblFailures.TabIndex = 11;
        lblFailures.Text = "Offline после:";
        // 
        // nudFailures
        // 
        nudFailures.Location = new Point(210, 247);
        nudFailures.Margin = new Padding(5, 5, 5, 5);
        nudFailures.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        nudFailures.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudFailures.Name = "nudFailures";
        nudFailures.Size = new Size(122, 35);
        nudFailures.TabIndex = 12;
        nudFailures.Value = new decimal(new int[] { 3, 0, 0, 0 });
        // 
        // lblSuccesses
        // 
        lblSuccesses.AutoSize = true;
        lblSuccesses.Location = new Point(410, 249);
        lblSuccesses.Margin = new Padding(5, 0, 5, 0);
        lblSuccesses.Name = "lblSuccesses";
        lblSuccesses.Size = new Size(141, 30);
        lblSuccesses.TabIndex = 13;
        lblSuccesses.Text = "Online после:";
        // 
        // nudSuccesses
        // 
        nudSuccesses.Location = new Point(561, 247);
        nudSuccesses.Margin = new Padding(5, 5, 5, 5);
        nudSuccesses.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        nudSuccesses.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudSuccesses.Name = "nudSuccesses";
        nudSuccesses.Size = new Size(122, 35);
        nudSuccesses.TabIndex = 14;
        nudSuccesses.Value = new decimal(new int[] { 2, 0, 0, 0 });
        // 
        // chkLatencyWarning
        // 
        chkLatencyWarning.AutoSize = true;
        chkLatencyWarning.Checked = true;
        chkLatencyWarning.CheckState = CheckState.Checked;
        chkLatencyWarning.Location = new Point(32, 312);
        chkLatencyWarning.Margin = new Padding(5, 5, 5, 5);
        chkLatencyWarning.Name = "chkLatencyWarning";
        chkLatencyWarning.Size = new Size(332, 34);
        chkLatencyWarning.TabIndex = 15;
        chkLatencyWarning.Text = "Warning по высокой задержке";
        chkLatencyWarning.UseVisualStyleBackColor = true;
        chkLatencyWarning.CheckedChanged += chkLatencyWarning_CheckedChanged;
        // 
        // lblLatencyThreshold
        // 
        lblLatencyThreshold.AutoSize = true;
        lblLatencyThreshold.Location = new Point(32, 382);
        lblLatencyThreshold.Margin = new Padding(5, 0, 5, 0);
        lblLatencyThreshold.Name = "lblLatencyThreshold";
        lblLatencyThreshold.Size = new Size(77, 30);
        lblLatencyThreshold.TabIndex = 16;
        lblLatencyThreshold.Text = "Порог:";
        // 
        // nudLatencyThreshold
        // 
        nudLatencyThreshold.Location = new Point(119, 377);
        nudLatencyThreshold.Margin = new Padding(5, 5, 5, 5);
        nudLatencyThreshold.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
        nudLatencyThreshold.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudLatencyThreshold.Name = "nudLatencyThreshold";
        nudLatencyThreshold.Size = new Size(122, 35);
        nudLatencyThreshold.TabIndex = 17;
        nudLatencyThreshold.Value = new decimal(new int[] { 100, 0, 0, 0 });
        // 
        // lblLatencyMs
        // 
        lblLatencyMs.AutoSize = true;
        lblLatencyMs.Location = new Point(251, 379);
        lblLatencyMs.Margin = new Padding(5, 0, 5, 0);
        lblLatencyMs.Name = "lblLatencyMs";
        lblLatencyMs.Size = new Size(38, 30);
        lblLatencyMs.TabIndex = 18;
        lblLatencyMs.Text = "мс";
        // 
        // lblLatencyChecks
        // 
        lblLatencyChecks.AutoSize = true;
        lblLatencyChecks.Location = new Point(410, 379);
        lblLatencyChecks.Margin = new Padding(5, 0, 5, 0);
        lblLatencyChecks.Name = "lblLatencyChecks";
        lblLatencyChecks.Size = new Size(77, 30);
        lblLatencyChecks.TabIndex = 19;
        lblLatencyChecks.Text = "После:";
        // 
        // nudLatencyChecks
        // 
        nudLatencyChecks.Location = new Point(496, 377);
        nudLatencyChecks.Margin = new Padding(5, 5, 5, 5);
        nudLatencyChecks.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        nudLatencyChecks.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudLatencyChecks.Name = "nudLatencyChecks";
        nudLatencyChecks.Size = new Size(96, 35);
        nudLatencyChecks.TabIndex = 20;
        nudLatencyChecks.Value = new decimal(new int[] { 2, 0, 0, 0 });
        // 
        // lblLatencyChecksUnit
        // 
        lblLatencyChecksUnit.AutoSize = true;
        lblLatencyChecksUnit.Location = new Point(602, 379);
        lblLatencyChecksUnit.Margin = new Padding(5, 0, 5, 0);
        lblLatencyChecksUnit.Name = "lblLatencyChecksUnit";
        lblLatencyChecksUnit.Size = new Size(105, 30);
        lblLatencyChecksUnit.TabIndex = 21;
        lblLatencyChecksUnit.Text = "проверок";
        // 
        // pnlBottom
        // 
        pnlBottom.BackColor = Color.White;
        pnlBottom.Controls.Add(btnCancel);
        pnlBottom.Controls.Add(btnSave);
        pnlBottom.Dock = DockStyle.Fill;
        pnlBottom.Location = new Point(0, 791);
        pnlBottom.Margin = new Padding(0);
        pnlBottom.Name = "pnlBottom";
        pnlBottom.Size = new Size(858, 84);
        pnlBottom.TabIndex = 1;
        // 
        // btnCancel
        // 
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(448, 16);
        btnCancel.Margin = new Padding(5, 5, 5, 5);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(168, 52);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Отмена";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(634, 16);
        btnSave.Margin = new Padding(5, 5, 5, 5);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(203, 52);
        btnSave.TabIndex = 0;
        btnSave.Text = "Сохранить";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // HostForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(168F, 168F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 245, 247);
        CancelButton = btnCancel;
        ClientSize = new Size(858, 875);
        Controls.Add(tlpRoot);
        ForeColor = Color.FromArgb(31, 41, 55);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Margin = new Padding(5, 5, 5, 5);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "HostForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Хост — IP Monitor";
        tlpRoot.ResumeLayout(false);
        pnlContent.ResumeLayout(false);
        pnlContent.PerformLayout();
        tlpMain.ResumeLayout(false);
        tlpMain.PerformLayout();
        tlpAddress.ResumeLayout(false);
        tlpAddress.PerformLayout();
        flpTest.ResumeLayout(false);
        tlpState.ResumeLayout(false);
        tlpState.PerformLayout();
        grpAccess.ResumeLayout(false);
        grpAccess.PerformLayout();
        tlpAccess.ResumeLayout(false);
        tlpAccess.PerformLayout();
        tlpWeb.ResumeLayout(false);
        tlpWeb.PerformLayout();
        flpRdp.ResumeLayout(false);
        flpRdp.PerformLayout();
        ((ISupportInitialize)nudRdpPort).EndInit();
        grpAdvanced.ResumeLayout(false);
        grpAdvanced.PerformLayout();
        ((ISupportInitialize)nudTcpPort).EndInit();
        ((ISupportInitialize)nudInterval).EndInit();
        ((ISupportInitialize)nudTimeout).EndInit();
        ((ISupportInitialize)nudFailures).EndInit();
        ((ISupportInitialize)nudSuccesses).EndInit();
        ((ISupportInitialize)nudLatencyThreshold).EndInit();
        ((ISupportInitialize)nudLatencyChecks).EndInit();
        pnlBottom.ResumeLayout(false);
        ResumeLayout(false);
    }
}
