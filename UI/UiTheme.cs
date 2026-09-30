using System.Drawing.Drawing2D;

namespace IpMonitor.UI;

internal static class UiTheme
{
    public static readonly Color Background = Color.FromArgb(244, 245, 249);      // #F4F5F9
    public static readonly Color Surface = Color.White;                           // #FFFFFF
    public static readonly Color SurfaceAlt = Color.FromArgb(249, 249, 252);
    public static readonly Color Sidebar = SurfaceAlt;      // #F9F9FC
    public static readonly Color Border = Color.FromArgb(231, 233, 239);           // #E7E9EF
    public static readonly Color TextPrimary = Color.FromArgb(42, 44, 52);         // #2A2C34
    public static readonly Color TextSecondary = Color.FromArgb(101, 105, 116);    // #656974
    public static readonly Color TextMuted = Color.FromArgb(151, 156, 168);        // #979CA8
    public static readonly Color Accent = Color.FromArgb(104, 87, 217);            // #6857D9
    public static readonly Color AccentDark = Color.FromArgb(87, 70, 196);         // #5746C4
    public static readonly Color AccentSoft = Color.FromArgb(239, 236, 253);       // #EFECFD
    public static readonly Color Success = Color.FromArgb(112, 190, 87);           // #70BE57
    public static readonly Color SuccessSoft = Color.FromArgb(239, 248, 235);      // #EFF8EB
    public static readonly Color Info = Color.FromArgb(47, 177, 210);              // #2FB1D2
    public static readonly Color Warning = Color.FromArgb(229, 171, 66);           // #E5AB42
    public static readonly Color WarningSoft = Color.FromArgb(255, 248, 231);      // #FFF8E7
    public static readonly Color Danger = Color.FromArgb(225, 83, 101);            // #E15365
    public static readonly Color DangerSoft = Color.FromArgb(253, 239, 242);        // #FDEFF2

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

    public static void StylePrimaryButton(Button button) => StyleButton(button, Accent, Color.White, Accent, 0);
    public static void StyleSuccessButton(Button button) => StyleButton(button, Success, Color.White, Success, 0);
    public static void StyleDangerButton(Button button) => StyleButton(button, Danger, Color.White, Danger, 0);
    public static void StyleSecondaryButton(Button button) => StyleButton(button, Surface, TextSecondary, Border, 1);
    public static void StyleGhostButton(Button button) => StyleButton(button, Color.Transparent, TextSecondary, Color.Transparent, 0);

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Surface;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.GridColor = Border;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.ColumnHeadersHeight = 30;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersDefaultCellStyle.BackColor = SurfaceAlt;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = TextSecondary;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SurfaceAlt;
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextSecondary;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.25F);
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
        grid.DefaultCellStyle.BackColor = Surface;
        grid.DefaultCellStyle.ForeColor = TextPrimary;
        grid.DefaultCellStyle.SelectionBackColor = AccentSoft;
        grid.DefaultCellStyle.SelectionForeColor = TextPrimary;
        grid.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 252, 254);
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
        menu.Renderer = new ToolStripProfessionalRenderer(new ModernColorTable());
        menu.ShowImageMargin = false;
        menu.Padding = new Padding(2);
    }

    public static void StyleBadge(Label label, Color background, Color foreground)
    {
        label.BackColor = background;
        label.ForeColor = foreground;
        label.Font = new Font("Segoe UI Semibold", 7.4F);
        label.TextAlign = ContentAlignment.MiddleCenter;
        label.Padding = new Padding(3, 0, 3, 0);

        // Не используем Region для статистики. Region, созданный до финального DPI-layout,
        // не масштабируется вместе с Label и на 150–200% DPI обрезал текст, оставляя
        // только первый символ (● / ▲ / ○). Обычный фон здесь DPI-safe.
        label.Region?.Dispose();
        label.Region = null;
    }

    public static void RoundControl(Control control, int radius = 8)
    {
        if (control.Width <= 1 || control.Height <= 1)
            return;
        using var path = CreateRoundedRectangle(new Rectangle(0, 0, control.Width, control.Height), radius);
        control.Region?.Dispose();
        control.Region = new Region(path);
    }

    private static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
    {
        var diameter = Math.Max(2, radius * 2);
        var rect = new Rectangle(bounds.X, bounds.Y, diameter, diameter);
        var path = new GraphicsPath();
        path.AddArc(rect, 180, 90);
        rect.X = bounds.Right - diameter;
        path.AddArc(rect, 270, 90);
        rect.Y = bounds.Bottom - diameter;
        path.AddArc(rect, 0, 90);
        rect.X = bounds.Left;
        path.AddArc(rect, 90, 90);
        path.CloseFigure();
        return path;
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
                    textBox.BackColor = Surface;
                    textBox.ForeColor = TextPrimary;
                    break;
                case RichTextBox richTextBox:
                    richTextBox.BorderStyle = BorderStyle.FixedSingle;
                    richTextBox.BackColor = Surface;
                    richTextBox.ForeColor = TextPrimary;
                    break;
                case ComboBox combo:
                    combo.FlatStyle = FlatStyle.Flat;
                    combo.BackColor = Surface;
                    combo.ForeColor = TextPrimary;
                    break;
                case NumericUpDown numeric:
                    numeric.BorderStyle = BorderStyle.FixedSingle;
                    numeric.BackColor = Surface;
                    numeric.ForeColor = TextPrimary;
                    break;
                case CheckBox checkBox:
                    checkBox.ForeColor = TextSecondary;
                    break;
                case GroupBox groupBox:
                    groupBox.BackColor = Surface;
                    groupBox.ForeColor = TextSecondary;
                    groupBox.Font = new Font("Segoe UI Semibold", 8.25F);
                    break;
                case StatusStrip statusStrip:
                    statusStrip.BackColor = Surface;
                    statusStrip.ForeColor = TextMuted;
                    statusStrip.Font = new Font("Segoe UI", 7.5F);
                    break;
                case TabControl tabControl:
                    tabControl.Font = new Font("Segoe UI Semibold", 8.25F);
                    break;
            }

            if (control.HasChildren)
                ApplyControlTree(control);
        }
    }

    private static void StyleButton(Button button, Color background, Color foreground, Color border, int borderSize)
    {
        // ButtonBase in WinForms does not accept Color.Transparent for FlatAppearance.BorderColor.
        // Ghost buttons therefore inherit an opaque background from their parent and simply use no border.
        var isGhost = background.A == 0;
        var effectiveBackground = isGhost
            ? (button.Parent?.BackColor ?? Background)
            : background;

        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = borderSize;

        if (borderSize > 0)
            button.FlatAppearance.BorderColor = border.A == 0 ? effectiveBackground : border;

        button.FlatAppearance.MouseOverBackColor = isGhost
            ? AccentSoft
            : Blend(effectiveBackground, Color.White, 0.12);
        button.FlatAppearance.MouseDownBackColor = isGhost
            ? Blend(AccentSoft, Accent, 0.08)
            : Blend(effectiveBackground, Color.Black, 0.07);
        button.BackColor = effectiveBackground;
        button.ForeColor = foreground;
        button.Cursor = Cursors.Hand;
        button.Font = new Font("Segoe UI Semibold", 8F);
        button.Padding = new Padding(3, 0, 3, 0);
        button.UseVisualStyleBackColor = false;
        button.Resize -= ButtonRoundedResize;
        button.Resize += ButtonRoundedResize;
        RoundControl(button, 6);
    }

    private static void ButtonRoundedResize(object? sender, EventArgs e)
    {
        if (sender is Button button)
            RoundControl(button, 6);
    }

    private sealed class ModernColorTable : ProfessionalColorTable
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
        public override Color MenuItemBorder => AccentSoft;
        public override Color SeparatorDark => Border;
        public override Color SeparatorLight => Border;
    }
}
