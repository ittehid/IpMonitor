using System.ComponentModel;

namespace IpMonitor;

partial class EventsForm
{
    private IContainer? components = null;
    private Label lblHeader;
    private Label lblHeaderHint;
    private Panel pnlFilters;
    private Label lblHostFilter;
    private ComboBox cboHost;
    private Label lblPeriod;
    private ComboBox cboPeriod;
    private Label lblType;
    private ComboBox cboType;
    private Button btnRefresh;
    private DataGridView dgvEvents;
    private DataGridViewTextBoxColumn colLostAt;
    private DataGridViewTextBoxColumn colEventHost;
    private DataGridViewTextBoxColumn colEventAddress;
    private DataGridViewTextBoxColumn colRecoveredAt;
    private DataGridViewTextBoxColumn colDowntime;
    private Panel pnlBottom;
    private Label lblCount;
    private Button btnExport;
    private Button btnDelete;
    private Button btnClear;
    private Button btnClose;
    private System.Windows.Forms.Timer durationTimer;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        lblHeader = new Label();
        lblHeaderHint = new Label();
        pnlFilters = new Panel();
        lblHostFilter = new Label();
        cboHost = new ComboBox();
        lblPeriod = new Label();
        cboPeriod = new ComboBox();
        lblType = new Label();
        cboType = new ComboBox();
        btnRefresh = new Button();
        dgvEvents = new DataGridView();
        colLostAt = new DataGridViewTextBoxColumn();
        colEventHost = new DataGridViewTextBoxColumn();
        colEventAddress = new DataGridViewTextBoxColumn();
        colRecoveredAt = new DataGridViewTextBoxColumn();
        colDowntime = new DataGridViewTextBoxColumn();
        pnlBottom = new Panel();
        lblCount = new Label();
        btnExport = new Button();
        btnDelete = new Button();
        btnClear = new Button();
        btnClose = new Button();
        durationTimer = new System.Windows.Forms.Timer(components);

        pnlFilters.SuspendLayout();
        ((ISupportInitialize)dgvEvents).BeginInit();
        pnlBottom.SuspendLayout();
        SuspendLayout();

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 11.5F);
        lblHeader.Location = new Point(14, 10);
        lblHeader.Text = "Журнал недоступности";

        lblHeaderHint.AutoSize = true;
        lblHeaderHint.Font = new Font("Segoe UI", 7.25F);
        lblHeaderHint.Location = new Point(15, 34);
        lblHeaderHint.Text = "Одна строка — один период без связи. Текущий простой обновляется автоматически.";

        pnlFilters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlFilters.BackColor = Color.White;
        pnlFilters.Controls.Add(lblHostFilter);
        pnlFilters.Controls.Add(cboHost);
        pnlFilters.Controls.Add(lblPeriod);
        pnlFilters.Controls.Add(cboPeriod);
        pnlFilters.Controls.Add(lblType);
        pnlFilters.Controls.Add(cboType);
        pnlFilters.Controls.Add(btnRefresh);
        pnlFilters.Location = new Point(14, 57);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Size = new Size(732, 56);

        lblHostFilter.AutoSize = true;
        lblHostFilter.Font = new Font("Segoe UI", 7F);
        lblHostFilter.Location = new Point(9, 6);
        lblHostFilter.Text = "Хост";

        cboHost.FlatStyle = FlatStyle.Standard;
        cboHost.DropDownStyle = ComboBoxStyle.DropDownList;
        cboHost.Location = new Point(9, 22);
        cboHost.Size = new Size(124, 23);
        cboHost.SelectedIndexChanged += FilterChanged;

        lblPeriod.AutoSize = true;
        lblPeriod.Font = new Font("Segoe UI", 7F);
        lblPeriod.Location = new Point(141, 6);
        lblPeriod.Text = "Период";

        cboPeriod.FlatStyle = FlatStyle.Standard;
        cboPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPeriod.Items.AddRange(new object[] { "24 часа", "7 дней", "30 дней", "Всё время" });
        cboPeriod.Location = new Point(141, 22);
        cboPeriod.Size = new Size(78, 23);
        cboPeriod.SelectedIndexChanged += FilterChanged;

        lblType.AutoSize = true;
        lblType.Font = new Font("Segoe UI", 7F);
        lblType.Location = new Point(227, 6);
        lblType.Text = "Статус";

        cboType.FlatStyle = FlatStyle.Standard;
        cboType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboType.Items.AddRange(new object[] { "Все записи", "Нет связи сейчас", "Восстановлено" });
        cboType.Location = new Point(227, 22);
        cboType.Size = new Size(106, 23);
        cboType.SelectedIndexChanged += FilterChanged;

        btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRefresh.Location = new Point(633, 19);
        btnRefresh.Size = new Size(100, 27);
        btnRefresh.Text = "Обновить";
        btnRefresh.Click += btnRefresh_Click;

        dgvEvents.AllowUserToAddRows = false;
        dgvEvents.AllowUserToDeleteRows = false;
        dgvEvents.AllowUserToResizeRows = false;
        dgvEvents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvEvents.AutoGenerateColumns = false;
        dgvEvents.Columns.AddRange(new DataGridViewColumn[] { colEventHost, colEventAddress, colLostAt, colRecoveredAt, colDowntime });
        dgvEvents.Location = new Point(14, 122);
        dgvEvents.MultiSelect = false;
        dgvEvents.ReadOnly = true;
        dgvEvents.RowHeadersVisible = false;
        dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEvents.ShowCellToolTips = true;
        dgvEvents.Size = new Size(732, 254);
        dgvEvents.CellFormatting += dgvEvents_CellFormatting;
        dgvEvents.SelectionChanged += dgvEvents_SelectionChanged;

        colLostAt.HeaderText = "Потеря связи";
        colLostAt.Name = "colLostAt";
        colLostAt.MinimumWidth = 135;
        colLostAt.Width = 160;

        colEventHost.HeaderText = "Хост";
        colEventHost.Name = "colEventHost";
        colEventHost.MinimumWidth = 105;
        colEventHost.Width = 150;

        colEventAddress.HeaderText = "Адрес";
        colEventAddress.Name = "colEventAddress";
        colEventAddress.MinimumWidth = 105;
        colEventAddress.Width = 135;

        colRecoveredAt.HeaderText = "Восстановление";
        colRecoveredAt.Name = "colRecoveredAt";
        colRecoveredAt.MinimumWidth = 145;
        colRecoveredAt.Width = 165;

        colDowntime.HeaderText = "Простой";
        colDowntime.Name = "colDowntime";
        colDowntime.MinimumWidth = 90;
        colDowntime.Width = 110;

        pnlBottom.BackColor = Color.White;
        pnlBottom.Controls.Add(lblCount);
        pnlBottom.Controls.Add(btnExport);
        pnlBottom.Controls.Add(btnDelete);
        pnlBottom.Controls.Add(btnClear);
        pnlBottom.Controls.Add(btnClose);
        pnlBottom.Dock = DockStyle.Bottom;
        pnlBottom.Location = new Point(0, 382);
        pnlBottom.Name = "pnlBottom";
        pnlBottom.Size = new Size(760, 48);
        pnlBottom.TabIndex = 4;

        lblCount.AutoSize = true;
        lblCount.Location = new Point(14, 16);
        lblCount.Text = "Периодов: 0";

        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Location = new Point(642, 10);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(104, 29);
        btnClose.TabIndex = 4;
        btnClose.Text = "Закрыть";

        btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClear.Location = new Point(520, 10);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(112, 29);
        btnClear.TabIndex = 3;
        btnClear.Text = "Очистить всё";
        btnClear.Click += btnClear_Click;

        btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnDelete.Enabled = false;
        btnDelete.Location = new Point(392, 10);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(118, 29);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "Удалить";
        btnDelete.Click += btnDelete_Click;

        btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnExport.Location = new Point(278, 10);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(104, 29);
        btnExport.TabIndex = 1;
        btnExport.Text = "Экспорт";
        btnExport.Click += btnExport_Click;

        durationTimer.Interval = 1000;
        durationTimer.Tick += durationTimer_Tick;

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        CancelButton = btnClose;
        BackColor = Color.FromArgb(243, 245, 247);
        ForeColor = Color.FromArgb(31, 41, 55);
        ClientSize = new Size(760, 430);
        Controls.Add(lblHeader);
        Controls.Add(lblHeaderHint);
        Controls.Add(pnlFilters);
        Controls.Add(dgvEvents);
        Controls.Add(pnlBottom);
        MinimumSize = new Size(680, 360);
        Name = "EventsForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Журнал недоступности — IP Monitor";

        pnlFilters.ResumeLayout(false);
        pnlFilters.PerformLayout();
        ((ISupportInitialize)dgvEvents).EndInit();
        pnlBottom.ResumeLayout(false);
        pnlBottom.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
