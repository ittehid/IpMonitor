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
    private FlowLayoutPanel flpBottomButtons;
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
        flpBottomButtons = new FlowLayoutPanel();
        btnCancel = new Button();
        btnAdd = new Button();

        tlpRoot.SuspendLayout();
        flpActions.SuspendLayout();
        ((ISupportInitialize)dgvPreview).BeginInit();
        pnlBottom.SuspendLayout();
        flpBottomButtons.SuspendLayout();
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

        btnParse.AutoSize = true;
        btnParse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnParse.Margin = new Padding(0, 0, 6, 0);
        btnParse.MinimumSize = new Size(84, 27);
        btnParse.Name = "btnParse";
        btnParse.Padding = new Padding(5, 0, 5, 0);
        btnParse.Text = "Разобрать";
        btnParse.Click += btnParse_Click;

        btnCheck.AutoSize = true;
        btnCheck.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnCheck.Margin = Padding.Empty;
        btnCheck.MinimumSize = new Size(92, 27);
        btnCheck.Name = "btnCheck";
        btnCheck.Padding = new Padding(5, 0, 5, 0);
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
        pnlBottom.Margin = Padding.Empty;
        pnlBottom.Name = "pnlBottom";

        lblCount.AutoSize = true;
        lblCount.Location = new Point(12, 14);
        lblCount.Name = "lblCount";
        lblCount.Text = "Найдено: 0";

        flpBottomButtons.AutoSize = true;
        flpBottomButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        flpBottomButtons.Controls.Add(btnAdd);
        flpBottomButtons.Controls.Add(btnCancel);
        flpBottomButtons.Dock = DockStyle.Right;
        flpBottomButtons.FlowDirection = FlowDirection.RightToLeft;
        flpBottomButtons.Margin = Padding.Empty;
        flpBottomButtons.Name = "flpBottomButtons";
        flpBottomButtons.Padding = new Padding(0, 8, 10, 0);
        flpBottomButtons.WrapContents = false;

        btnAdd.AutoSize = true;
        btnAdd.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnAdd.Enabled = false;
        btnAdd.Margin = new Padding(0, 0, 6, 0);
        btnAdd.MinimumSize = new Size(82, 27);
        btnAdd.Name = "btnAdd";
        btnAdd.Padding = new Padding(6, 0, 6, 0);
        btnAdd.Text = "Добавить";
        btnAdd.Click += btnAdd_Click;

        btnCancel.AutoSize = true;
        btnCancel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Margin = Padding.Empty;
        btnCancel.MinimumSize = new Size(68, 27);
        btnCancel.Name = "btnCancel";
        btnCancel.Padding = new Padding(6, 0, 6, 0);
        btnCancel.Text = "Отмена";

        pnlBottom.Controls.Add(lblCount);
        pnlBottom.Controls.Add(flpBottomButtons);

        tlpRoot.Controls.Add(lblHeader, 0, 0);
        tlpRoot.Controls.Add(lblHint, 0, 1);
        tlpRoot.Controls.Add(txtHosts, 0, 2);
        tlpRoot.Controls.Add(flpActions, 0, 3);
        tlpRoot.Controls.Add(dgvPreview, 0, 4);
        tlpRoot.Controls.Add(pnlBottom, 0, 5);

        AcceptButton = btnAdd;
        CancelButton = btnCancel;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
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
        flpBottomButtons.ResumeLayout(false);
        flpBottomButtons.PerformLayout();
        ResumeLayout(false);
    }
}
