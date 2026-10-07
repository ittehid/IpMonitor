using System.ComponentModel;

namespace IpMonitor;

partial class QuickAddForm
{
    private IContainer? components = null;
    private TableLayoutPanel tlpRoot;
    private Label lblHeader;
    private Label lblHint;
    private TextBox txtHosts;
    private FlowLayoutPanel flpActions;
    private Button btnParse;
    private Button btnCheck;
    private DataGridView dgvPreview;
    private DataGridViewCheckBoxColumn colInclude;
    private DataGridViewTextBoxColumn colQuickName;
    private DataGridViewTextBoxColumn colQuickAddress;
    private DataGridViewTextBoxColumn colQuickResult;
    private Panel pnlBottom;
    private Label lblCount;
    private Button btnCancel;
    private Button btnAdd;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        tlpRoot = new TableLayoutPanel();
        lblHeader = new Label();
        lblHint = new Label();
        txtHosts = new TextBox();
        flpActions = new FlowLayoutPanel();
        btnParse = new Button();
        btnCheck = new Button();
        dgvPreview = new DataGridView();
        colInclude = new DataGridViewCheckBoxColumn();
        colQuickName = new DataGridViewTextBoxColumn();
        colQuickAddress = new DataGridViewTextBoxColumn();
        colQuickResult = new DataGridViewTextBoxColumn();
        pnlBottom = new Panel();
        lblCount = new Label();
        btnCancel = new Button();
        btnAdd = new Button();

        tlpRoot.SuspendLayout();
        flpActions.SuspendLayout();
        ((ISupportInitialize)dgvPreview).BeginInit();
        pnlBottom.SuspendLayout();
        SuspendLayout();

        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.Margin = Padding.Empty;
        tlpRoot.Name = "tlpRoot";
        tlpRoot.Padding = new Padding(12, 9, 12, 0);
        tlpRoot.RowCount = 6;
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 78F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

        lblHeader.AutoSize = true;
        lblHeader.Font = new Font("Segoe UI Semibold", 11F);
        lblHeader.Margin = new Padding(0, 0, 0, 1);
        lblHeader.Name = "lblHeader";
        lblHeader.Text = "Быстрое добавление";

        lblHint.AutoSize = true;
        lblHint.Font = new Font("Segoe UI", 7.35F);
        lblHint.Margin = new Padding(0, 0, 0, 7);
        lblHint.Name = "lblHint";
        lblHint.Text = "Формат: Название;Адрес;Группа. Группа необязательна.";

        txtHosts.AcceptsReturn = true;
        txtHosts.Dock = DockStyle.Fill;
        txtHosts.Font = new Font("Segoe UI", 8F);
        txtHosts.Margin = new Padding(0, 0, 0, 7);
        txtHosts.Multiline = true;
        txtHosts.Name = "txtHosts";
        txtHosts.PlaceholderText = "MikroTik CORE;10.0.99.2;СЕТЬ\r\nMail Server;10.0.8.122;СЕРВЕРЫ";
        txtHosts.ScrollBars = ScrollBars.Vertical;

        flpActions.AutoSize = true;
        flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpActions.Controls.Add(btnParse);
        flpActions.Controls.Add(btnCheck);
        flpActions.Dock = DockStyle.Fill;
        flpActions.FlowDirection = FlowDirection.LeftToRight;
        flpActions.Margin = new Padding(0, 0, 0, 7);
        flpActions.Name = "flpActions";
        flpActions.WrapContents = false;

        btnParse.Margin = new Padding(0, 0, 8, 0);
        btnParse.Name = "btnParse";
        btnParse.Size = new Size(100, 27);
        btnParse.Text = "Разобрать";
        btnParse.Click += btnParse_Click;

        btnCheck.Margin = Padding.Empty;
        btnCheck.Name = "btnCheck";
        btnCheck.Size = new Size(120, 27);
        btnCheck.Text = "Проверить все";
        btnCheck.Click += btnCheck_Click;

        dgvPreview.AllowUserToAddRows = false;
        dgvPreview.AllowUserToDeleteRows = false;
        dgvPreview.AutoGenerateColumns = false;
        dgvPreview.Columns.AddRange(new DataGridViewColumn[] { colInclude, colQuickName, colQuickAddress, colQuickResult });
        dgvPreview.Dock = DockStyle.Fill;
        dgvPreview.Margin = Padding.Empty;
        dgvPreview.MultiSelect = false;
        dgvPreview.Name = "dgvPreview";
        dgvPreview.RowHeadersVisible = false;
        dgvPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        colInclude.HeaderText = "";
        colInclude.Name = "colInclude";
        colInclude.Width = 28;
        colQuickName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colQuickName.FillWeight = 44F;
        colQuickName.HeaderText = "Название";
        colQuickName.Name = "colQuickName";
        colQuickName.ReadOnly = true;
        colQuickAddress.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colQuickAddress.FillWeight = 34F;
        colQuickAddress.HeaderText = "Адрес";
        colQuickAddress.Name = "colQuickAddress";
        colQuickAddress.ReadOnly = true;
        colQuickResult.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colQuickResult.FillWeight = 28F;
        colQuickResult.HeaderText = "Проверка";
        colQuickResult.Name = "colQuickResult";
        colQuickResult.ReadOnly = true;

        pnlBottom.BackColor = Color.White;
        pnlBottom.Dock = DockStyle.Fill;
        pnlBottom.Location = new Point(12, 316);
        pnlBottom.Margin = Padding.Empty;
        pnlBottom.Name = "pnlBottom";
        pnlBottom.Size = new Size(386, 44);
        pnlBottom.TabIndex = 5;

        lblCount.AutoSize = true;
        lblCount.Location = new Point(12, 14);
        lblCount.Name = "lblCount";
        lblCount.Text = "Найдено: 0";

        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.Enabled = false;
        btnAdd.Location = new Point(282, 8);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(92, 29);
        btnAdd.TabIndex = 2;
        btnAdd.Text = "Добавить";
        btnAdd.Click += btnAdd_Click;

        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(178, 8);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(94, 29);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Отмена";

        pnlBottom.Controls.Add(lblCount);
        pnlBottom.Controls.Add(btnCancel);
        pnlBottom.Controls.Add(btnAdd);

        tlpRoot.Controls.Add(lblHeader, 0, 0);
        tlpRoot.Controls.Add(lblHint, 0, 1);
        tlpRoot.Controls.Add(txtHosts, 0, 2);
        tlpRoot.Controls.Add(flpActions, 0, 3);
        tlpRoot.Controls.Add(dgvPreview, 0, 4);
        tlpRoot.Controls.Add(pnlBottom, 0, 5);

        AcceptButton = btnAdd;
        CancelButton = btnCancel;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(243, 245, 247);
        ForeColor = Color.FromArgb(31, 41, 55);
        ClientSize = new Size(410, 360);
        Controls.Add(tlpRoot);
        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        MinimumSize = new Size(390, 330);
        Name = "QuickAddForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Быстрое добавление — IP Monitor";

        tlpRoot.ResumeLayout(false);
        tlpRoot.PerformLayout();
        flpActions.ResumeLayout(false);
        flpActions.PerformLayout();
        ((ISupportInitialize)dgvPreview).EndInit();
        pnlBottom.ResumeLayout(false);
        pnlBottom.PerformLayout();
        ResumeLayout(false);
    }
}
