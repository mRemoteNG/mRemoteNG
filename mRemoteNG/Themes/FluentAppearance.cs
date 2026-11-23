using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace mRemoteNG.Themes
{
    /// <summary>Opt-in spacing and native-control styling for comfortable themes.</summary>
    internal static class FluentAppearance
    {
        private static readonly ConditionalWeakTable<Control, FontState> Fonts = new();
        private static readonly ConditionalWeakTable<ToolStrip, StripState> Strips = new();
        private static readonly ConditionalWeakTable<PropertyGrid, GridState> Grids = new();

        internal static bool Enabled(ThemeManager manager) => manager.ActiveAndExtended && manager.ActiveTheme.UseComfortableSpacing;

        internal static Font FontFor(Control owner, Font original, bool enabled, bool compact = false)
        {
            FontState state = Fonts.GetValue(owner, control => new FontState(control, original));
            return enabled ? compact ? state.Compact : state.Comfortable : state.Original;
        }

        internal static void ApplyFont(Control control, bool enabled, bool compact = false)
            => control.Font = FontFor(control, control.Font, enabled, compact);

        internal static void ApplyGrid(PropertyGrid grid, ThemeInfo theme, bool enabled)
        {
            GridState state = Grids.GetValue(grid, value => new GridState(value));
            ApplyFont(grid, enabled, compact: true);
            if (!enabled) { state.Restore(grid); return; }
            ExtendedColorPalette p = theme.ExtendedPalette;
            // WinForms paints category rows with LineColor as well as using it
            // for cell separators. A dark line color avoids broad gray bands.
            grid.LineColor = p.getColor("List_Header_Background");
            grid.CategoryForeColor = p.getColor("List_Header_Foreground");
            grid.CategorySplitterColor = p.getColor("List_Item_Border");
            grid.ViewBorderColor = p.getColor("List_Item_Border");
            grid.HelpBorderColor = p.getColor("List_Item_Border");
            grid.HelpBackColor = p.getColor("List_Header_Background");
            grid.DisabledItemForeColor = p.getColor("TextBox_Disabled_Foreground");
            grid.SelectedItemWithFocusBackColor = p.getColor("Treeview_SelectedItem_Active_Background");
            grid.SelectedItemWithFocusForeColor = p.getColor("Treeview_SelectedItem_Active_Foreground");
        }

        internal static void ApplyStrip(ToolStrip strip, ThemeInfo theme, bool enabled,
            IReadOnlyDictionary<ToolStripItem, string>? glyphs = null, bool compact = false)
        {
            StripState state = Strips.GetValue(strip, value => new StripState(value));
            ApplyFont(strip, enabled, compact);
            if (!enabled)
            {
                if (ReferenceEquals(strip.Renderer, state.Renderer)) strip.Renderer = state.Fallback;
                strip.Padding = state.Padding;
                strip.GripStyle = state.Grip;
                foreach (var pair in state.ItemPadding) if (!pair.Key.IsDisposed) pair.Key.Padding = pair.Value;
                return;
            }
            if (!ReferenceEquals(strip.Renderer, state.Renderer)) state.Fallback = strip.Renderer;
            state.Renderer = new FluentRenderer(theme, glyphs);
            strip.Renderer = state.Renderer;
            strip.GripStyle = ToolStripGripStyle.Hidden;
            // PropertyGrid sizes its toolbar itself. Extra vertical padding
            // reduces the image rectangle and can squeeze a 16px icon flat.
            int inset = Math.Max(1, (int)Math.Round((compact ? 2 : 4) * strip.DeviceDpi / 96d));
            Padding padding = compact ? new Padding(inset, 0, inset, 0) : new Padding(inset, 3, inset, 3);
            strip.Padding = padding;
            foreach (ToolStripItem item in strip.Items)
            {
                if (!state.ItemPadding.ContainsKey(item)) state.ItemPadding[item] = item.Padding;
                if (item is ToolStripButton || item is ToolStripDropDownButton || item is ToolStripMenuItem)
                    item.Padding = padding;
            }
        }

        internal static Rectangle FitImageBounds(Size image, Rectangle available)
        {
            if (image.Width <= 0 || image.Height <= 0 || available.Width <= 0 || available.Height <= 0)
                return Rectangle.Empty;
            double scale = Math.Min((double)available.Width / image.Width, (double)available.Height / image.Height);
            int width = Math.Min(available.Width, Math.Max(1, (int)Math.Round(image.Width * scale)));
            int height = Math.Min(available.Height, Math.Max(1, (int)Math.Round(image.Height * scale)));
            return new Rectangle(available.X + (available.Width - width) / 2,
                available.Y + (available.Height - height) / 2, width, height);
        }

        private sealed class FontState
        {
            internal readonly Font Original;
            private Font? _comfortable;
            private Font? _compact;
            internal Font Comfortable => _comfortable ??= new Font("Segoe UI", 9.75f, Original.Style, GraphicsUnit.Point);
            internal Font Compact => _compact ??= new Font("Segoe UI", 8.25f, Original.Style, GraphicsUnit.Point);
            internal FontState(Control owner, Font original)
            {
                Original = original;
                owner.Disposed += (_, _) => { _comfortable?.Dispose(); _compact?.Dispose(); _comfortable = null; _compact = null; };
            }
        }

        private sealed class StripState
        {
            internal readonly Padding Padding;
            internal readonly ToolStripGripStyle Grip;
            internal ToolStripRenderer Fallback;
            internal FluentRenderer? Renderer;
            internal readonly Dictionary<ToolStripItem, Padding> ItemPadding = new();
            internal StripState(ToolStrip strip) { Padding = strip.Padding; Grip = strip.GripStyle; Fallback = strip.Renderer; }
        }

        private sealed class GridState
        {
            private readonly Color _category, _view, _help, _disabled, _selection, _selectedText;
            internal GridState(PropertyGrid grid)
            {
                _category = grid.CategorySplitterColor; _view = grid.ViewBorderColor; _help = grid.HelpBorderColor;
                _disabled = grid.DisabledItemForeColor; _selection = grid.SelectedItemWithFocusBackColor;
                _selectedText = grid.SelectedItemWithFocusForeColor;
            }
            internal void Restore(PropertyGrid grid)
            {
                grid.CategorySplitterColor = _category; grid.ViewBorderColor = _view; grid.HelpBorderColor = _help;
                grid.DisabledItemForeColor = _disabled; grid.SelectedItemWithFocusBackColor = _selection;
                grid.SelectedItemWithFocusForeColor = _selectedText;
            }
        }

        private sealed class FluentRenderer : ToolStripProfessionalRenderer
        {
            private readonly ThemeInfo _theme;
            private readonly IReadOnlyDictionary<ToolStripItem, string>? _glyphs;
            internal FluentRenderer(ThemeInfo theme, IReadOnlyDictionary<ToolStripItem, string>? glyphs)
                : base(new FluentColorTable(theme.ExtendedPalette)) { _theme = theme; _glyphs = glyphs; RoundedEdges = false; }

            protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
            {
                if (_glyphs != null && _glyphs.TryGetValue(e.Item, out string? key))
                {
                    Rectangle bounds = FitImageBounds(new Size(1, 1), e.ImageRectangle);
                    if (bounds.IsEmpty) return;
                    int size = bounds.Width;
                    using Bitmap? image = ThemeIcons.Create(key, _theme, e.Item.Enabled, size);
                    if (image != null) { e.Graphics.DrawImage(image, bounds); return; }
                }
                if (e.Image == null) { base.OnRenderItemImage(e); return; }
                Rectangle imageBounds = FitImageBounds(e.Image.Size, e.ImageRectangle);
                if (!imageBounds.IsEmpty)
                    base.OnRenderItemImage(new ToolStripItemImageRenderEventArgs(e.Graphics, e.Item, e.Image, imageBounds));
            }
            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = _theme.ExtendedPalette.getColor(e.Item.Enabled ? "Dialog_Foreground" : "TextBox_Disabled_Foreground");
                base.OnRenderItemText(e);
            }
        }

        private sealed class FluentColorTable : ProfessionalColorTable
        {
            private readonly Color _background, _selected, _hover, _edge;
            internal FluentColorTable(ExtendedColorPalette palette)
            {
                UseSystemColors = false;
                _background = palette.getColor("Dialog_Background");
                _selected = palette.getColor("Treeview_SelectedItem_Active_Background");
                _hover = palette.getColor("List_Item_Border"); _edge = _hover;
            }
            public override Color ToolStripGradientBegin => _background;
            public override Color ToolStripGradientMiddle => _background;
            public override Color ToolStripGradientEnd => _background;
            public override Color MenuStripGradientBegin => _background;
            public override Color MenuStripGradientEnd => _background;
            public override Color ToolStripDropDownBackground => _background;
            public override Color ImageMarginGradientBegin => _background;
            public override Color ImageMarginGradientMiddle => _background;
            public override Color ImageMarginGradientEnd => _background;
            public override Color ButtonCheckedGradientBegin => _selected;
            public override Color ButtonCheckedGradientMiddle => _selected;
            public override Color ButtonCheckedGradientEnd => _selected;
            public override Color ButtonCheckedHighlight => _selected;
            public override Color ButtonSelectedGradientBegin => _hover;
            public override Color ButtonSelectedGradientMiddle => _hover;
            public override Color ButtonSelectedGradientEnd => _hover;
            public override Color ButtonSelectedHighlight => _hover;
            public override Color ButtonPressedGradientBegin => _selected;
            public override Color ButtonPressedGradientMiddle => _selected;
            public override Color ButtonPressedGradientEnd => _selected;
            public override Color ButtonPressedHighlight => _selected;
            public override Color ButtonSelectedBorder => _edge;
            public override Color ButtonPressedBorder => _edge;
            public override Color ButtonCheckedHighlightBorder => _edge;
            public override Color ButtonSelectedHighlightBorder => _edge;
            public override Color ButtonPressedHighlightBorder => _edge;
            public override Color MenuItemSelected => _selected;
            public override Color MenuItemSelectedGradientBegin => _selected;
            public override Color MenuItemSelectedGradientEnd => _selected;
            public override Color MenuItemPressedGradientBegin => _selected;
            public override Color MenuItemPressedGradientMiddle => _selected;
            public override Color MenuItemPressedGradientEnd => _selected;
            public override Color MenuItemBorder => _edge;
            public override Color CheckBackground => _selected;
            public override Color CheckSelectedBackground => _selected;
            public override Color CheckPressedBackground => _selected;
            public override Color MenuBorder => _edge;
            public override Color ToolStripBorder => _edge;
            public override Color SeparatorDark => _edge;
            public override Color SeparatorLight => _edge;
        }
    }
}
