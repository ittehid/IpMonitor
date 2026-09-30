using IpMonitor.Models;
using IpMonitor.Services;
using IpMonitor.UI;

namespace IpMonitor;

public partial class QuickAddForm : Form
{
    private readonly AppSettings _settings;
    private readonly HostCheckService _checkService;

    public List<MonitorHost> Hosts { get; } = [];

    internal QuickAddForm(AppSettings settings, HostCheckService checkService)
    {
        InitializeComponent();
        _settings = settings;
        _checkService = checkService;
        UiTheme.Apply(this);
        UiTheme.StylePrimaryButton(btnAdd);
        UiTheme.StyleSecondaryButton(btnCancel);
        UiTheme.StyleSecondaryButton(btnParse);
        UiTheme.StyleSecondaryButton(btnCheck);
        UiTheme.StyleGrid(dgvPreview);
        lblHint.ForeColor = UiTheme.TextMuted;
        pnlBottom.BackColor = UiTheme.Surface;
    }

    private void btnParse_Click(object? sender, EventArgs e) => ParseInput();

    private void ParseInput()
    {
        dgvPreview.Rows.Clear();
        foreach (var raw in txtHosts.Lines)
        {
            var line = raw.Trim();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = SplitLine(line);
            string name;
            string address;
            var group = string.Empty;
            if (parts.Length >= 2)
            {
                name = parts[0].Trim();
                address = parts[1].Trim();
                if (parts.Length >= 3)
                    group = parts[2].Trim();
            }
            else
            {
                address = parts[0].Trim();
                name = address;
            }

            if (string.IsNullOrWhiteSpace(address))
                continue;

            var rowIndex = dgvPreview.Rows.Add(true, string.IsNullOrWhiteSpace(name) ? address : name, address, "Не проверен");
            var row = dgvPreview.Rows[rowIndex];
            row.Tag = group;
            if (!string.IsNullOrWhiteSpace(group))
                row.Cells[colQuickName.Index].ToolTipText = $"Группа: {group}";
        }
        UpdateCount();
    }

    private static string[] SplitLine(string line)
    {
        if (line.Contains('\t'))
            return line.Split('\t', 3, StringSplitOptions.TrimEntries);
        if (line.Contains(';'))
            return line.Split(';', 3, StringSplitOptions.TrimEntries);
        if (line.Contains(','))
            return line.Split(',', 3, StringSplitOptions.TrimEntries);
        return [line];
    }

    private async void btnCheck_Click(object? sender, EventArgs e)
    {
        if (dgvPreview.Rows.Count == 0)
            ParseInput();
        if (dgvPreview.Rows.Count == 0)
            return;

        btnCheck.Enabled = false;
        btnParse.Enabled = false;
        try
        {
            var tasks = dgvPreview.Rows.Cast<DataGridViewRow>().Select(CheckRowAsync).ToArray();
            await Task.WhenAll(tasks);
        }
        finally
        {
            btnCheck.Enabled = true;
            btnParse.Enabled = true;
        }
    }

    private async Task CheckRowAsync(DataGridViewRow row)
    {
        var address = Convert.ToString(row.Cells[colQuickAddress.Index].Value) ?? string.Empty;
        var host = new MonitorHost { Name = address, Address = address };
        var result = await _checkService.CheckAsync(host, _settings.TimeoutMilliseconds, CancellationToken.None);
        if (IsDisposed)
            return;
        BeginInvoke((Action)(() =>
        {
            row.Cells[colQuickResult.Index].Value = result.Success ? $"● {result.LatencyMs ?? 0} ms" : "● Нет ответа";
            row.Cells[colQuickResult.Index].Style.ForeColor = result.Success ? UiTheme.Success : UiTheme.Danger;
        }));
    }

    private void btnAdd_Click(object? sender, EventArgs e)
    {
        dgvPreview.EndEdit();
        Hosts.Clear();
        foreach (DataGridViewRow row in dgvPreview.Rows)
        {
            var include = row.Cells[colInclude.Index].Value is bool value && value;
            if (!include)
                continue;
            var name = Convert.ToString(row.Cells[colQuickName.Index].Value)?.Trim() ?? string.Empty;
            var address = Convert.ToString(row.Cells[colQuickAddress.Index].Value)?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(address))
                continue;
            Hosts.Add(new MonitorHost
            {
                Name = string.IsNullOrWhiteSpace(name) ? address : name,
                Address = address,
                Group = Convert.ToString(row.Tag)?.Trim() ?? string.Empty,
                Enabled = true
            });
        }

        if (Hosts.Count == 0)
        {
            MessageBox.Show(this, "Не выбрано ни одного хоста.", "IP Monitor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void UpdateCount()
    {
        lblCount.Text = $"Найдено: {dgvPreview.Rows.Count}";
        btnAdd.Enabled = dgvPreview.Rows.Count > 0;
        btnAdd.Text = dgvPreview.Rows.Count > 0 ? $"Добавить ({dgvPreview.Rows.Count})" : "Добавить";
    }
}
