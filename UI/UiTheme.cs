namespace IpMonitor.UI;

internal static class UiTheme
{
    // IP Monitor 2.0.19: единая строгая светлая тема.
    // Тёмная и системная темы намеренно удалены из приложения.
    public static Color Background => Color.FromArgb(245, 245, 245);
    public static Color Surface => Color.White;
    public static Color SurfaceAlt => Color.FromArgb(245, 245, 245);
    public static Color InputBackground => Color.White;
    public static Color Sidebar => Color.FromArgb(248, 248, 248);
    public static Color Border => Color.FromArgb(207, 207, 207);
    public static Color TextPrimary => Color.FromArgb(31, 31, 31);
    public static Color TextSecondary => Color.FromArgb(82, 82, 82);
    public static Color TextMuted => Color.FromArgb(104, 104, 104);
    public static Color Accent => Color.FromArgb(37, 99, 235);
    public static Color AccentDark => Color.FromArgb(29, 78, 216);
    public static Color AccentSoft => Color.FromArgb(232, 240, 254);
    public static Color Success => Color.FromArgb(22, 128, 60);
    public static Color SuccessSoft => Color.FromArgb(235, 247, 238);
    public static Color Warning => Color.FromArgb(183, 121, 0);
    public static Color WarningSoft => Color.FromArgb(255, 247, 226);
    public static Color Danger => Color.FromArgb(196, 43, 28);
    public static Color DangerSoft => Color.FromArgb(253, 237, 235);
    public static Color Maintenance => Color.FromArgb(101, 84, 192);
    public static Color MaintenanceSoft => Color.FromArgb(241, 238, 255);

    public static void Apply(Form form)
    {
        form.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
        form.BackColor = Background;
        form.ForeColor = TextPrimary;

        try
        {
            using var appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (appIcon is not null)
                form.Icon = (Icon)appIcon.Clone();
        }
        catch
        {
            // В дизайнере Visual Studio или при необычном host-процессе иконка может быть недоступна.
        }

        ApplyControlTree(form);
    }

    public static void StylePrimaryButton(Button button) => StyleButton(button, Accent, Color.White, Accent, 1);
    public static void StyleSuccessButton(Button button) => StyleButton(button, Surface, Success, Success, 1);
    public static void StyleDangerButton(Button button) => StyleButton(button, Surface, Danger, Danger, 1);
    public static void StyleSecondaryButton(Button button) => StyleButton(button, Surface, TextPrimary, Border, 1);
    public static void StyleGhostButton(Button button) => StyleButton(button, Surface, TextPrimary, Border, 1);

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Surface;
        grid.BorderStyle = BorderStyle.FixedSingle;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        grid.GridColor = Border;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        grid.ColumnHeadersHeight = 30;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersDefaultCellStyle.BackColor = SurfaceAlt;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SurfaceAlt;
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextPrimary;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.25F);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        grid.DefaultCellStyle.BackColor = Surface;
        grid.DefaultCellStyle.ForeColor = TextPrimary;
        grid.DefaultCellStyle.SelectionBackColor = AccentSoft;
        grid.DefaultCellStyle.SelectionForeColor = TextPrimary;
        grid.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = AccentSoft;
        grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextPrimary;
        grid.RowTemplate.Height = 30;
        grid.RowHeadersVisible = false;
    }

    public static void StyleContextMenu(ContextMenuStrip menu)
    {
        menu.BackColor = Surface;
        menu.ForeColor = TextPrimary;
        menu.Font = new Font("Segoe UI", 8.25F);
        menu.RenderMode = ToolStripRenderMode.Professional;
        menu.Renderer = new ToolStripProfessionalRenderer(new StrictColorTable());
        menu.ShowImageMargin = false;
        menu.Padding = new Padding(1);
    }

    public static void StyleBadge(Label label, Color background, Color foreground)
    {
        // Строгий интерфейс: никаких "таблеток" и скруглений. Цвет передаёт только текст/индикатор.
        label.Region?.Dispose();
        label.Region = null;
        label.BackColor = Surface;
        label.ForeColor = foreground;
        label.Font = new Font("Segoe UI Semibold", 7.6F);
        label.TextAlign = ContentAlignment.MiddleCenter;
        label.Padding = new Padding(2, 0, 2, 0);
    }

    public static Color Blend(Color baseColor, Color overlay, double amount)
    {
        amount = Math.Clamp(amount, 0, 1);
        var inverse = 1 - amount;
        return Color.FromArgb(
            (int)(baseColor.R * inverse + overlay.R * amount),
            (int)(baseColor.G * inverse + overlay.G * amount),
            (int)(baseColor.B * inverse + overlay.B * amount));
    }

    private static void ApplyControlTree(Control root)
    {
        foreach (Control control in root.Controls)
        {
            switch (control)
            {
                case DataGridView grid:
                    StyleGrid(grid);
                    break;
                case Button button:
                    StyleSecondaryButton(button);
                    break;
                case TextBox textBox:
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = InputBackground;
                    textBox.ForeColor = TextPrimary;
                    break;
                case RichTextBox richTextBox:
                    richTextBox.BorderStyle = BorderStyle.FixedSingle;
                    richTextBox.BackColor = InputBackground;
                    richTextBox.ForeColor = TextPrimary;
                    break;
                case ComboBox combo:
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.BackColor = InputBackground;
                    combo.ForeColor = TextPrimary;
                    break;
                case NumericUpDown numeric:
                    numeric.BorderStyle = BorderStyle.FixedSingle;
                    numeric.BackColor = InputBackground;
                    numeric.ForeColor = TextPrimary;
                    break;
                case CheckBox checkBox:
                    checkBox.BackColor = Color.Transparent;
                    checkBox.ForeColor = TextPrimary;
                    break;
                case RadioButton radioButton:
                    radioButton.BackColor = Color.Transparent;
                    radioButton.ForeColor = TextPrimary;
                    break;
                case GroupBox groupBox:
                    groupBox.BackColor = Surface;
                    groupBox.ForeColor = TextSecondary;
                    groupBox.Font = new Font("Segoe UI Semibold", 8.25F);
                    break;
                case TabPage tabPage:
                    tabPage.BackColor = Surface;
                    tabPage.ForeColor = TextPrimary;
                    break;
                case TabControl tabControl:
                    StyleTabControl(tabControl);
                    break;
                case StatusStrip statusStrip:
                    statusStrip.BackColor = Surface;
                    statusStrip.ForeColor = TextMuted;
                    statusStrip.Font = new Font("Segoe UI", 7.5F);
                    statusStrip.Renderer = new ToolStripProfessionalRenderer(new StrictColorTable());
                    break;
                case FlowLayoutPanel flow:
                    flow.BackColor = Surface;
                    break;
                case TableLayoutPanel table:
                    table.BackColor = Surface;
                    break;
                case Panel panel:
                    panel.BackColor = Surface;
                    break;
                case LinkLabel link:
                    link.BackColor = Color.Transparent;
                    link.ForeColor = TextPrimary;
                    link.LinkColor = Accent;
                    link.ActiveLinkColor = AccentDark;
                    link.VisitedLinkColor = Accent;
                    break;
                case Label label:
                    label.ForeColor = TextPrimary;
                    if (label.BackColor != Color.Transparent)
                        label.BackColor = Surface;
                    break;
            }

            if (control.HasChildren)
                ApplyControlTree(control);
        }
    }

    private static void StyleButton(Button button, Color background, Color foreground, Color border, int borderSize)
    {
        button.Region?.Dispose();
        button.Region = null;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = borderSize;
        button.FlatAppearance.BorderColor = border;
        button.FlatAppearance.MouseOverBackColor = background == Surface
            ? SurfaceAlt
            : Blend(background, Color.White, 0.12);
        button.FlatAppearance.MouseDownBackColor = background == Surface
            ? Blend(SurfaceAlt, Color.Black, 0.04)
            : Blend(background, Color.Black, 0.10);
        button.BackColor = background;
        button.ForeColor = foreground;
        button.Cursor = Cursors.Hand;
        button.Font = new Font("Segoe UI Semibold", 8F);
        button.Padding = new Padding(3, 0, 3, 0);
        button.UseVisualStyleBackColor = false;
    }

    private static void StyleTabControl(TabControl tabControl)
    {
        tabControl.Font = new Font("Segoe UI", 8.25F);
        tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
        tabControl.DrawItem -= TabControl_DrawItem;
        tabControl.DrawItem += TabControl_DrawItem;
        tabControl.Invalidate();
    }

    private static void TabControl_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not TabControl tabs || e.Index < 0 || e.Index >= tabs.TabPages.Count)
            return;

        var selected = (e.State & DrawItemState.Selected) != 0;
        var bounds = e.Bounds;
        using var back = new SolidBrush(selected ? Surface : SurfaceAlt);
        using var border = new Pen(Border);
        e.Graphics.FillRectangle(back, bounds);
        e.Graphics.DrawRectangle(border, bounds.X, bounds.Y, Math.Max(0, bounds.Width - 1), Math.Max(0, bounds.Height - 1));
        TextRenderer.DrawText(
            e.Graphics,
            tabs.TabPages[e.Index].Text,
            tabs.Font,
            bounds,
            selected ? TextPrimary : TextSecondary,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private sealed class StrictColorTable : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => Surface;
        public override Color ImageMarginGradientBegin => Surface;
        public override Color ImageMarginGradientMiddle => Surface;
        public override Color ImageMarginGradientEnd => Surface;
        public override Color MenuItemSelected => AccentSoft;
        public override Color MenuItemSelectedGradientBegin => AccentSoft;
        public override Color MenuItemSelectedGradientEnd => AccentSoft;
        public override Color MenuItemPressedGradientBegin => AccentSoft;
        public override Color MenuItemPressedGradientEnd => AccentSoft;
        public override Color MenuBorder => Border;
        public override Color MenuItemBorder => Border;
        public override Color SeparatorDark => Border;
        public override Color SeparatorLight => Border;
        public override Color ToolStripBorder => Border;
        public override Color StatusStripGradientBegin => Surface;
        public override Color StatusStripGradientEnd => Surface;
    }
}
