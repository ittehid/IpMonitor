using System.ComponentModel;

namespace IpMonitor;

partial class MainForm
{
    private IContainer? components = null;
    private Panel pnlToolbar;
    private Label lblTitle;
    private Label lblSubtitle;
    private FlowLayoutPanel flpToolbar;
    private Button btnMonitoring;
    private Button btnAddHost;
    private Button btnEvents;
    private Button btnSettings;
    private Button btnMore;
    private Panel pnlSummary;
    private TableLayoutPanel tlpSummary;
    private Label lblOnline;
    private Label lblWarning;
    private Label lblOffline;
    private Label lblDisabled;
    private Panel pnlContent;
    private DataGridView dgvHosts;
    private Panel pnlEmpty;
    private TableLayoutPanel tlpEmpty;
    private Label lblEmptyIcon;
    private Label lblEmptyTitle;
    private Label lblEmptyHint;
    private FlowLayoutPanel flpEmptyButtons;
    private Button btnEmptyAdd;
    private Button btnEmptyQuickAdd;
    private DataGridViewTextBoxColumn colStatus;
    private DataGridViewTextBoxColumn colName;
    private DataGridViewTextBoxColumn colAddress;
    private DataGridViewTextBoxColumn colResponse;
    private ContextMenuStrip cmsHost;
    private ToolStripMenuItem miCheckNow;
    private ToolStripSeparator sep1;
    private ToolStripMenuItem miOpenWeb;
    private ToolStripMenuItem miOpenRdp;
    private ToolStripSeparator sep2;
    private ToolStripMenuItem miCopyAddress;
    private ToolStripSeparator sep3;
    private ToolStripMenuItem miEvents;
    private ToolStripMenuItem miEdit;
    private ToolStripMenuItem miMoveUp;
    private ToolStripMenuItem miMoveDown;
    private ToolStripSeparator sep4;
    private ToolStripMenuItem miMaintenance;
    private ToolStripMenuItem miMaintenance15m;
    private ToolStripMenuItem miMaintenance1h;
    private ToolStripMenuItem miMaintenance4h;
    private ToolStripMenuItem miMaintenanceTomorrow;
    private ToolStripMenuItem miMaintenanceIndefinite;
    private ToolStripSeparator maintenanceSep;
    private ToolStripMenuItem miMaintenanceEnd;
    private ToolStripMenuItem miDisable;
    private ToolStripMenuItem miDelete;
    private ContextMenuStrip cmsAdd;
    private ToolStripMenuItem miAddSingle;
    private ToolStripMenuItem miAddMultiple;
    private ContextMenuStrip cmsMore;
    private ToolStripMenuItem miData;
    private ToolStripMenuItem miImportHosts;
    private ToolStripMenuItem miExportHosts;
    private ToolStripSeparator dataSep;
    private ToolStripMenuItem miOpenDataFolder;
    private ToolStripMenuItem miSort;
    private ToolStripMenuItem miSortManual;
    private ToolStripSeparator sortSep;
    private ToolStripMenuItem miSortNameAsc;
    private ToolStripMenuItem miSortNameDesc;
    private ToolStripMenuItem miSortAddressAsc;
    private ToolStripMenuItem miSortAddressDesc;
    private ToolStripMenuItem miAbout;
    private ToolStripSeparator moreSep;
    private ToolStripMenuItem miExit;
    private StatusStrip statusMain;
    private ToolStripStatusLabel tslState;
    private ToolStripStatusLabel tslActivity;
    private ToolStripStatusLabel tslSpring;
    private ToolStripStatusLabel tslHosts;
    private NotifyIcon notifyIcon;
    private ContextMenuStrip cmsTray;
    private ToolStripMenuItem miTrayOpen;
    private ToolStripMenuItem miTrayMonitoring;
    private ToolStripMenuItem miTrayAbout;
    private ToolStripSeparator traySep;
    private ToolStripMenuItem miTrayExit;
    private ToolTip toolTip;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            _monitoringService?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        pnlToolbar = new Panel();
        lblTitle = new Label();
        lblSubtitle = new Label();
        flpToolbar = new FlowLayoutPanel();
        btnMonitoring = new Button();
        btnAddHost = new Button();
        btnEvents = new Button();
        btnSettings = new Button();
        btnMore = new Button();
        pnlSummary = new Panel();
        tlpSummary = new TableLayoutPanel();
        lblOnline = new Label();
        lblWarning = new Label();
        lblOffline = new Label();
        lblDisabled = new Label();
        pnlContent = new Panel();
        dgvHosts = new DataGridView();
        pnlEmpty = new Panel();
        tlpEmpty = new TableLayoutPanel();
        lblEmptyIcon = new Label();
        lblEmptyTitle = new Label();
        lblEmptyHint = new Label();
        flpEmptyButtons = new FlowLayoutPanel();
        btnEmptyAdd = new Button();
        btnEmptyQuickAdd = new Button();
        colStatus = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colAddress = new DataGridViewTextBoxColumn();
        colResponse = new DataGridViewTextBoxColumn();
        cmsHost = new ContextMenuStrip(components);
        miCheckNow = new ToolStripMenuItem();
        sep1 = new ToolStripSeparator();
        miOpenWeb = new ToolStripMenuItem();
        miOpenRdp = new ToolStripMenuItem();
        sep2 = new ToolStripSeparator();
        miCopyAddress = new ToolStripMenuItem();
        sep3 = new ToolStripSeparator();
        miEvents = new ToolStripMenuItem();
        miEdit = new ToolStripMenuItem();
        miMoveUp = new ToolStripMenuItem();
        miMoveDown = new ToolStripMenuItem();
        sep4 = new ToolStripSeparator();
        miMaintenance = new ToolStripMenuItem();
        miMaintenance15m = new ToolStripMenuItem();
        miMaintenance1h = new ToolStripMenuItem();
        miMaintenance4h = new ToolStripMenuItem();
        miMaintenanceTomorrow = new ToolStripMenuItem();
        miMaintenanceIndefinite = new ToolStripMenuItem();
        maintenanceSep = new ToolStripSeparator();
        miMaintenanceEnd = new ToolStripMenuItem();
        miDisable = new ToolStripMenuItem();
        miDelete = new ToolStripMenuItem();
        cmsAdd = new ContextMenuStrip(components);
        miAddSingle = new ToolStripMenuItem();
        miAddMultiple = new ToolStripMenuItem();
        cmsMore = new ContextMenuStrip(components);
        miData = new ToolStripMenuItem();
        miImportHosts = new ToolStripMenuItem();
        miExportHosts = new ToolStripMenuItem();
        dataSep = new ToolStripSeparator();
        miOpenDataFolder = new ToolStripMenuItem();
        miSort = new ToolStripMenuItem();
        miSortManual = new ToolStripMenuItem();
        sortSep = new ToolStripSeparator();
        miSortNameAsc = new ToolStripMenuItem();
        miSortNameDesc = new ToolStripMenuItem();
        miSortAddressAsc = new ToolStripMenuItem();
        miSortAddressDesc = new ToolStripMenuItem();
        miAbout = new ToolStripMenuItem();
        moreSep = new ToolStripSeparator();
        miExit = new ToolStripMenuItem();
        statusMain = new StatusStrip();
        tslState = new ToolStripStatusLabel();
        tslActivity = new ToolStripStatusLabel();
        tslSpring = new ToolStripStatusLabel();
        tslHosts = new ToolStripStatusLabel();
        notifyIcon = new NotifyIcon(components);
        cmsTray = new ContextMenuStrip(components);
        miTrayOpen = new ToolStripMenuItem();
        miTrayMonitoring = new ToolStripMenuItem();
        miTrayAbout = new ToolStripMenuItem();
        traySep = new ToolStripSeparator();
        miTrayExit = new ToolStripMenuItem();
        toolTip = new ToolTip(components);

        pnlToolbar.SuspendLayout();
        flpToolbar.SuspendLayout();
        pnlSummary.SuspendLayout();
        tlpSummary.SuspendLayout();
        pnlContent.SuspendLayout();
        ((ISupportInitialize)dgvHosts).BeginInit();
        pnlEmpty.SuspendLayout();
        tlpEmpty.SuspendLayout();
        flpEmptyButtons.SuspendLayout();
        cmsHost.SuspendLayout();
        cmsAdd.SuspendLayout();
        cmsMore.SuspendLayout();
        statusMain.SuspendLayout();
        cmsTray.SuspendLayout();
        SuspendLayout();

        // pnlToolbar
        pnlToolbar.BackColor = Color.White;
        pnlToolbar.Controls.Add(lblTitle);
        pnlToolbar.Controls.Add(lblSubtitle);
        pnlToolbar.Controls.Add(flpToolbar);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Height = 38;
        pnlToolbar.Name = "pnlToolbar";

        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI Semibold", 11.25F);
        lblTitle.Location = new Point(10, 7);
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "IP Monitor";
        lblTitle.Visible = false;

        // lblSubtitle
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new Font("Segoe UI", 7.25F);
        lblSubtitle.Location = new Point(11, 29);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Text = "Локальный мониторинг устройств";
        lblSubtitle.Visible = false;

        // flpToolbar
        flpToolbar.Controls.Add(btnMonitoring);
        flpToolbar.Controls.Add(btnAddHost);
        flpToolbar.Controls.Add(btnEvents);
        flpToolbar.Controls.Add(btnSettings);
        flpToolbar.Controls.Add(btnMore);
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.FlowDirection = FlowDirection.LeftToRight;
        flpToolbar.Height = 38;
        flpToolbar.Location = new Point(0, 0);
        flpToolbar.Margin = new Padding(0);
        flpToolbar.Name = "flpToolbar";
        flpToolbar.Padding = new Padding(8, 4, 4, 4);
        flpToolbar.WrapContents = false;

        // toolbar buttons
        btnMonitoring.Margin = new Padding(0, 0, 5, 0);
        btnMonitoring.Name = "btnMonitoring";
        btnMonitoring.Size = new Size(64, 26);
        btnMonitoring.Text = "▶ Старт";
        btnMonitoring.Click += btnMonitoring_Click;

        btnAddHost.Margin = new Padding(0, 0, 5, 0);
        btnAddHost.Name = "btnAddHost";
        btnAddHost.Size = new Size(68, 26);
        btnAddHost.Text = "＋ Хост ▼";
        btnAddHost.Click += btnAddHost_Click;

        btnEvents.Margin = new Padding(0, 0, 5, 0);
        btnEvents.Name = "btnEvents";
        btnEvents.Size = new Size(64, 26);
        btnEvents.Text = "Журнал";
        btnEvents.Click += btnEvents_Click;

        btnSettings.Margin = new Padding(0, 0, 5, 0);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(27, 26);
        btnSettings.Text = "⚙";
        toolTip.SetToolTip(btnSettings, "Настройки");
        btnSettings.Click += btnSettings_Click;

        btnMore.Margin = new Padding(0);
        btnMore.Name = "btnMore";
        btnMore.Size = new Size(27, 26);
        btnMore.Text = "⋯";
        toolTip.SetToolTip(btnMore, "Ещё");
        btnMore.Click += btnMore_Click;

        // pnlSummary / tlpSummary
        pnlSummary.BackColor = Color.White;
        pnlSummary.Controls.Add(tlpSummary);
        pnlSummary.Dock = DockStyle.Top;
        pnlSummary.Height = 34;
        pnlSummary.Name = "pnlSummary";

        tlpSummary.ColumnCount = 4;
        tlpSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpSummary.Controls.Add(lblOnline, 0, 0);
        tlpSummary.Controls.Add(lblWarning, 1, 0);
        tlpSummary.Controls.Add(lblOffline, 2, 0);
        tlpSummary.Controls.Add(lblDisabled, 3, 0);
        tlpSummary.Dock = DockStyle.Fill;
        tlpSummary.Name = "tlpSummary";
        tlpSummary.Padding = new Padding(6, 3, 6, 3);
        tlpSummary.RowCount = 1;
        tlpSummary.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        lblOnline.Name = "lblOnline";
        lblWarning.Name = "lblWarning";
        lblOffline.Name = "lblOffline";
        lblDisabled.Name = "lblDisabled";
        ConfigureBadge(lblOnline, "● Online 0");
        ConfigureBadge(lblWarning, "▲ Warn 0");
        ConfigureBadge(lblOffline, "● Offline 0");
        ConfigureBadge(lblDisabled, "○ Выкл 0");

        // pnlContent
        pnlContent.BackColor = Color.White;
        pnlContent.Controls.Add(dgvHosts);
        pnlContent.Controls.Add(pnlEmpty);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Name = "pnlContent";

        // dgvHosts
        dgvHosts.AllowUserToAddRows = false;
        dgvHosts.AllowUserToDeleteRows = false;
        dgvHosts.AllowUserToResizeRows = false;
        dgvHosts.AllowDrop = true;
        dgvHosts.AutoGenerateColumns = false;
        dgvHosts.BackgroundColor = Color.White;
        dgvHosts.BorderStyle = BorderStyle.None;
        dgvHosts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvHosts.Columns.AddRange(new DataGridViewColumn[] { colStatus, colName, colAddress, colResponse });
        dgvHosts.ContextMenuStrip = cmsHost;
        dgvHosts.Dock = DockStyle.Fill;
        dgvHosts.MultiSelect = false;
        dgvHosts.Name = "dgvHosts";
        dgvHosts.ReadOnly = true;
        dgvHosts.RowHeadersVisible = false;
        dgvHosts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvHosts.ShowCellToolTips = true;
        dgvHosts.CellDoubleClick += dgvHosts_CellDoubleClick;
        dgvHosts.CellFormatting += dgvHosts_CellFormatting;
        dgvHosts.CellPainting += dgvHosts_CellPainting;
        dgvHosts.CellToolTipTextNeeded += dgvHosts_CellToolTipTextNeeded;
        dgvHosts.ColumnHeaderMouseClick += dgvHosts_ColumnHeaderMouseClick;
        dgvHosts.MouseDown += dgvHosts_MouseDown;
        dgvHosts.MouseMove += dgvHosts_MouseMove;
        dgvHosts.DragOver += dgvHosts_DragOver;
        dgvHosts.DragDrop += dgvHosts_DragDrop;

        // pnlEmpty
        pnlEmpty.BackColor = Color.White;
        pnlEmpty.Controls.Add(tlpEmpty);
        pnlEmpty.Dock = DockStyle.Fill;
        pnlEmpty.Name = "pnlEmpty";
        pnlEmpty.Visible = false;

        // tlpEmpty - DPI-safe centered empty state
        tlpEmpty.ColumnCount = 1;
        tlpEmpty.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpEmpty.Controls.Add(lblEmptyIcon, 0, 1);
        tlpEmpty.Controls.Add(lblEmptyTitle, 0, 2);
        tlpEmpty.Controls.Add(lblEmptyHint, 0, 3);
        tlpEmpty.Controls.Add(flpEmptyButtons, 0, 4);
        tlpEmpty.Dock = DockStyle.Fill;
        tlpEmpty.Name = "tlpEmpty";
        tlpEmpty.RowCount = 6;
        tlpEmpty.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
        tlpEmpty.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpEmpty.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpEmpty.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpEmpty.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpEmpty.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));

        lblEmptyIcon.Anchor = AnchorStyles.None;
        lblEmptyIcon.AutoSize = true;
        lblEmptyIcon.Font = new Font("Segoe UI", 17F);
        lblEmptyIcon.Margin = new Padding(0, 0, 0, 2);
        lblEmptyIcon.Name = "lblEmptyIcon";
        lblEmptyIcon.Text = "◎";
        lblEmptyIcon.TextAlign = ContentAlignment.MiddleCenter;

        lblEmptyTitle.Anchor = AnchorStyles.None;
        lblEmptyTitle.AutoSize = true;
        lblEmptyTitle.Font = new Font("Segoe UI Semibold", 9.5F);
        lblEmptyTitle.Margin = new Padding(0, 0, 0, 3);
        lblEmptyTitle.Name = "lblEmptyTitle";
        lblEmptyTitle.Text = "Хостов пока нет";

        lblEmptyHint.Anchor = AnchorStyles.None;
        lblEmptyHint.Font = new Font("Segoe UI", 7.25F);
        lblEmptyHint.Margin = new Padding(0, 0, 0, 8);
        lblEmptyHint.Name = "lblEmptyHint";
        lblEmptyHint.Size = new Size(250, 30);
        lblEmptyHint.Text = "Добавьте устройство или сервер,\r\nчтобы начать мониторинг.";
        lblEmptyHint.TextAlign = ContentAlignment.MiddleCenter;

        flpEmptyButtons.Anchor = AnchorStyles.None;
        flpEmptyButtons.AutoSize = true;
        flpEmptyButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpEmptyButtons.Controls.Add(btnEmptyAdd);
        flpEmptyButtons.Controls.Add(btnEmptyQuickAdd);
        flpEmptyButtons.FlowDirection = FlowDirection.LeftToRight;
        flpEmptyButtons.Margin = new Padding(0);
        flpEmptyButtons.Name = "flpEmptyButtons";
        flpEmptyButtons.WrapContents = false;

        btnEmptyAdd.Margin = new Padding(0, 0, 5, 0);
        btnEmptyAdd.Name = "btnEmptyAdd";
        btnEmptyAdd.Size = new Size(84, 27);
        btnEmptyAdd.Text = "＋ Хост";
        btnEmptyAdd.Click += btnEmptyAdd_Click;

        btnEmptyQuickAdd.Margin = new Padding(0);
        btnEmptyQuickAdd.Name = "btnEmptyQuickAdd";
        btnEmptyQuickAdd.Size = new Size(75, 27);
        btnEmptyQuickAdd.Text = "Списком";
        btnEmptyQuickAdd.Click += btnEmptyQuickAdd_Click;

        // columns
        colStatus.HeaderText = "";
        colStatus.Name = "colStatus";
        colStatus.Width = 30;
        colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
        colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colName.HeaderText = "Хост / адрес";
        colName.SortMode = DataGridViewColumnSortMode.Programmatic;
        colName.Name = "colName";
        colName.MinimumWidth = 115;
        colAddress.HeaderText = "Адрес";
        colAddress.Name = "colAddress";
        colAddress.SortMode = DataGridViewColumnSortMode.Programmatic;
        colAddress.Visible = false;
        colAddress.Width = 88;
        colResponse.HeaderText = "Состояние";
        colResponse.Name = "colResponse";
        colResponse.Width = 92;
        colResponse.SortMode = DataGridViewColumnSortMode.NotSortable;

        // cmsHost
        cmsHost.Items.AddRange(new ToolStripItem[] { miCheckNow, sep1, miOpenWeb, miOpenRdp, sep2, miCopyAddress, sep3, miEvents, miEdit, miMoveUp, miMoveDown, sep4, miMaintenance, miDisable, miDelete });
        cmsHost.Name = "cmsHost";
        cmsHost.Opening += cmsHost_Opening;
        miCheckNow.Text = "Проверить сейчас";
        miCheckNow.Click += miCheckNow_Click;
        miOpenWeb.Text = "Открыть Web";
        miOpenWeb.Click += miOpenWeb_Click;
        miOpenRdp.Text = "Подключиться по RDP";
        miOpenRdp.Click += miOpenRdp_Click;
        miCopyAddress.Text = "Копировать IP / адрес";
        miCopyAddress.Click += miCopyAddress_Click;
        miEvents.Text = "Журнал событий";
        miEvents.Click += miEvents_Click;
        miEdit.Text = "Изменить";
        miEdit.Click += miEdit_Click;
        miMoveUp.Text = "Переместить выше";
        miMoveUp.Click += miMoveUp_Click;
        miMoveDown.Text = "Переместить ниже";
        miMoveDown.Click += miMoveDown_Click;
        miMaintenance.Text = "Режим обслуживания";
        miMaintenance.DropDownItems.AddRange(new ToolStripItem[] { miMaintenance15m, miMaintenance1h, miMaintenance4h, miMaintenanceTomorrow, miMaintenanceIndefinite, maintenanceSep, miMaintenanceEnd });
        miMaintenance15m.Text = "На 15 минут";
        miMaintenance15m.Click += miMaintenance15m_Click;
        miMaintenance1h.Text = "На 1 час";
        miMaintenance1h.Click += miMaintenance1h_Click;
        miMaintenance4h.Text = "На 4 часа";
        miMaintenance4h.Click += miMaintenance4h_Click;
        miMaintenanceTomorrow.Text = "До завтра 09:00";
        miMaintenanceTomorrow.Click += miMaintenanceTomorrow_Click;
        miMaintenanceIndefinite.Text = "Бессрочно";
        miMaintenanceIndefinite.Click += miMaintenanceIndefinite_Click;
        miMaintenanceEnd.Text = "Завершить обслуживание";
        miMaintenanceEnd.Click += miMaintenanceEnd_Click;
        miDisable.Text = "Отключить мониторинг";
        miDisable.Click += miDisable_Click;
        miDelete.Text = "Удалить";
        miDelete.Click += miDelete_Click;

        // cmsAdd
        cmsAdd.Items.AddRange(new ToolStripItem[] { miAddSingle, miAddMultiple });
        miAddSingle.Text = "Добавить один хост...";
        miAddSingle.Click += miAddSingle_Click;
        miAddMultiple.Text = "Добавить несколько...";
        miAddMultiple.Click += miAddMultiple_Click;

        // cmsMore
        cmsMore.Items.AddRange(new ToolStripItem[] { miData, miSort, moreSep, miAbout, miExit });

        miData.Text = "Импорт / экспорт";
        miData.DropDownItems.AddRange(new ToolStripItem[] { miImportHosts, miExportHosts, dataSep, miOpenDataFolder });
        miImportHosts.Text = "Импорт хостов...";
        miImportHosts.Click += miImportHosts_Click;
        miExportHosts.Text = "Экспорт хостов...";
        miExportHosts.Click += miExportHosts_Click;
        miOpenDataFolder.Text = "Открыть папку данных";
        miOpenDataFolder.Click += miOpenDataFolder_Click;

        miSort.Text = "Сортировка";
        miSort.DropDownItems.AddRange(new ToolStripItem[] { miSortManual, sortSep, miSortNameAsc, miSortNameDesc, miSortAddressAsc, miSortAddressDesc });
        miSortManual.Text = "Ручной порядок";
        miSortManual.Click += miSortManual_Click;
        miSortNameAsc.Text = "По имени А–Я";
        miSortNameAsc.Click += miSortNameAsc_Click;
        miSortNameDesc.Text = "По имени Я–А";
        miSortNameDesc.Click += miSortNameDesc_Click;
        miSortAddressAsc.Text = "По адресу ↑";
        miSortAddressAsc.Click += miSortAddressAsc_Click;
        miSortAddressDesc.Text = "По адресу ↓";
        miSortAddressDesc.Click += miSortAddressDesc_Click;

        miAbout.Text = "О программе...";
        miAbout.Click += miAbout_Click;
        miExit.Text = "Выход";
        miExit.Click += miExit_Click;

        // statusMain
        statusMain.Items.AddRange(new ToolStripItem[] { tslState, tslActivity, tslSpring, tslHosts });
        statusMain.Name = "statusMain";
        statusMain.SizingGrip = false;
        tslState.Text = "Мониторинг остановлен";
        tslActivity.Text = "";
        tslActivity.Margin = new Padding(4, 3, 0, 2);
        tslSpring.Spring = true;
        tslHosts.Text = "Хостов: 0";

        // tray
        cmsTray.Items.AddRange(new ToolStripItem[] { miTrayOpen, miTrayMonitoring, miTrayAbout, traySep, miTrayExit });
        miTrayOpen.Text = "Открыть IP Monitor";
        miTrayOpen.Font = new Font("Segoe UI Semibold", 8.25F);
        miTrayOpen.Click += miTrayOpen_Click;
        miTrayMonitoring.Text = "Запустить мониторинг";
        miTrayMonitoring.Click += miTrayMonitoring_Click;
        miTrayAbout.Text = "О программе...";
        miTrayAbout.Click += miAbout_Click;
        miTrayExit.Text = "Выход";
        miTrayExit.Click += miTrayExit_Click;
        notifyIcon.ContextMenuStrip = cmsTray;
        notifyIcon.Icon = SystemIcons.Application;
        notifyIcon.Text = "IP Monitor";
        notifyIcon.Visible = true;
        notifyIcon.DoubleClick += notifyIcon_DoubleClick;

        // MainForm
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(320, 330);
        Controls.Add(pnlContent);
        Controls.Add(pnlSummary);
        Controls.Add(pnlToolbar);
        Controls.Add(statusMain);
        MinimumSize = new Size(305, 210);
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "IP Monitor";
        FormClosing += MainForm_FormClosing;
        Resize += MainForm_Resize;
        DpiChanged += MainForm_DpiChanged;

        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        flpToolbar.ResumeLayout(false);
        pnlSummary.ResumeLayout(false);
        tlpSummary.ResumeLayout(false);
        pnlContent.ResumeLayout(false);
        ((ISupportInitialize)dgvHosts).EndInit();
        pnlEmpty.ResumeLayout(false);
        tlpEmpty.ResumeLayout(false);
        tlpEmpty.PerformLayout();
        flpEmptyButtons.ResumeLayout(false);
        cmsHost.ResumeLayout(false);
        cmsAdd.ResumeLayout(false);
        cmsMore.ResumeLayout(false);
        statusMain.ResumeLayout(false);
        statusMain.PerformLayout();
        cmsTray.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private static void ConfigureBadge(Label label, string text)
    {
        label.Dock = DockStyle.Fill;
        label.Margin = new Padding(2, 0, 2, 0);
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleCenter;
    }
}
