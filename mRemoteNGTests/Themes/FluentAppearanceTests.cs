using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Themes;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.Themes
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class FluentAppearanceTests
    {
        private static ThemeInfo Theme()
        {
            ExtendedColorPalette p = new();
            foreach (var pair in new Dictionary<string, Color>
            {
                ["Dialog_Background"] = Color.FromArgb(30,30,46), ["Dialog_Foreground"] = Color.FromArgb(205,214,244),
                ["List_Header_Background"] = Color.FromArgb(24,24,37), ["List_Header_Foreground"] = Color.FromArgb(186,194,222),
                ["List_Item_Border"] = Color.FromArgb(69,71,90), ["TextBox_Disabled_Foreground"] = Color.FromArgb(166,173,200),
                ["Treeview_SelectedItem_Active_Background"] = Color.FromArgb(49,50,68),
                ["Treeview_SelectedItem_Active_Foreground"] = Color.FromArgb(205,214,244)
            }) p.addColor(pair.Key, pair.Value);
            return new ThemeInfo("Appearance test", new VS2015LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015, p)
            { UseFluentIcons = true, UseComfortableSpacing = true, IconColor = Color.FromArgb(203,166,247) };
        }

        [Test]
        public void FontSwitch_RestoresTheOriginalFont()
        {
            using Button button = new(); Font original = button.Font;
            FluentAppearance.ApplyFont(button, true);
            Assert.That(button.Font.SizeInPoints, Is.EqualTo(9.75f));
            FluentAppearance.ApplyFont(button, false);
            Assert.That(button.Font, Is.SameAs(original));
            Assert.That(original.Height, Is.GreaterThan(0));
        }

        [Test]
        public void GridStyle_UsesDarkCategoriesAndRestoresNativeSelection()
        {
            using PropertyGrid grid = new();
            Font original = grid.Font;
            Color selected = grid.SelectedItemWithFocusBackColor, splitter = grid.CategorySplitterColor;
            ThemeInfo theme = Theme();
            FluentAppearance.ApplyGrid(grid, theme, true);
            Assert.That(grid.Font.SizeInPoints, Is.EqualTo(8.25f));
            Assert.That(grid.LineColor, Is.EqualTo(theme.ExtendedPalette.getColor("List_Header_Background")));
            Assert.That(grid.SelectedItemWithFocusBackColor, Is.EqualTo(Color.FromArgb(49,50,68)));
            FluentAppearance.ApplyGrid(grid, theme, false);
            Assert.That(grid.Font, Is.SameAs(original));
            Assert.That(grid.SelectedItemWithFocusBackColor, Is.EqualTo(selected));
            Assert.That(grid.CategorySplitterColor, Is.EqualTo(splitter));
        }

        [Test]
        public void ToolbarStyle_RestoresRendererAndButtonPaddingWithoutReplacingImages()
        {
            using ToolStrip strip = new(); using Bitmap stock = new(16,16);
            ToolStripButton button = new() { Image = stock, Padding = new Padding(1) }; strip.Items.Add(button);
            ToolStripRenderer original = strip.Renderer; Padding padding = strip.Padding;
            ThemeInfo theme = Theme();
            FluentAppearance.ApplyStrip(strip, theme, true, new Dictionary<ToolStripItem,string> { [button] = "Properties_16x" });
            Assert.That(strip.Renderer, Is.Not.SameAs(original));
            Assert.That(button.Image, Is.SameAs(stock));
            FluentAppearance.ApplyStrip(strip, theme, false);
            Assert.That(strip.Renderer, Is.SameAs(original));
            Assert.That(strip.Padding, Is.EqualTo(padding));
            Assert.That(button.Padding, Is.EqualTo(new Padding(1)));
            Assert.That(stock.Width, Is.EqualTo(16));
        }

        [Test]
        public void ToolbarStyle_DoesNotOverrideAReplacementThemeRendererWhenRestoring()
        {
            using ToolStrip strip = new(); ThemeInfo theme = Theme();
            FluentAppearance.ApplyStrip(strip, theme, true);
            ToolStripRenderer replacement = new ToolStripSystemRenderer(); strip.Renderer = replacement;
            FluentAppearance.ApplyStrip(strip, theme, false);
            Assert.That(strip.Renderer, Is.SameAs(replacement));
        }

        [Test]
        public void CompactToolbar_LeavesRoomForNativeIconsAndRestoresOriginalSpacing()
        {
            using ToolStrip strip = new() { AutoSize = false, Height = 25 };
            using Bitmap stock = new(16, 16);
            ToolStripButton button = new() { Image = stock, Padding = new Padding(1) };
            strip.Items.Add(button);
            Font originalFont = strip.Font; Padding originalPadding = strip.Padding;
            Size originalImageSize = strip.ImageScalingSize;
            FluentAppearance.ApplyStrip(strip, Theme(), true, compact: true);
            Assert.That(strip.Font.SizeInPoints, Is.EqualTo(8.25f));
            Assert.That(strip.Padding.Vertical + button.Padding.Vertical, Is.Zero);
            Assert.That(strip.Height, Is.EqualTo(25));
            Assert.That(strip.ImageScalingSize, Is.EqualTo(originalImageSize));
            Assert.That(button.Image, Is.SameAs(stock));
            FluentAppearance.ApplyStrip(strip, Theme(), false, compact: true);
            Assert.That(strip.Font, Is.SameAs(originalFont));
            Assert.That(strip.Padding, Is.EqualTo(originalPadding));
            Assert.That(button.Padding, Is.EqualTo(new Padding(1)));
        }

        [TestCase(16, 16, 16, 10, 10, 10)]
        [TestCase(24, 24, 24, 15, 15, 15)]
        [TestCase(32, 32, 32, 20, 20, 20)]
        [TestCase(32, 16, 16, 16, 16, 8)]
        public void IconBounds_PreserveAspectAndCenterInsideAvailableSpace(int sourceWidth, int sourceHeight,
            int width, int height, int expectedWidth, int expectedHeight)
        {
            Rectangle available = new(5, 7, width, height);
            Rectangle bounds = FluentAppearance.FitImageBounds(new Size(sourceWidth, sourceHeight), available);
            Assert.That(bounds.Size, Is.EqualTo(new Size(expectedWidth, expectedHeight)));
            Assert.That(available.Contains(bounds), Is.True);
            Assert.That(bounds.X, Is.EqualTo(available.X + (width - expectedWidth) / 2));
            Assert.That(bounds.Y, Is.EqualTo(available.Y + (height - expectedHeight) / 2));
        }
    }
}
