using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Themes;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.Themes
{
    [TestFixture, Apartment(ApartmentState.STA), NonParallelizable]
    public class ThemeIconsTests
    {
        private ThemeManager _manager;
        private ThemeInfo _original;
        private bool _enabled;

        [SetUp]
        public void SetUp()
        {
            _manager = ThemeManager.getInstance();
            _enabled = _manager.ThemingActive;
            _manager.ThemingActive = true;
            _original = _manager.ActiveTheme;
            _manager.ActiveTheme = MakeTheme();
            _manager.ThemingActive = true;
        }

        [TearDown]
        public void TearDown() { _manager.ActiveTheme = _original; _manager.ThemingActive = _enabled; }

        private static ThemeInfo MakeTheme() => new("Fluent icon test", new VS2015LightTheme(), "",
            VisualStudioToolStripExtender.VsVersion.Vs2015, new ExtendedColorPalette())
        {
            UseFluentIcons = true, IconColor = Color.FromArgb(203, 166, 247),
            DisabledIconColor = Color.FromArgb(108, 112, 134)
        };

        private static Color OpaquePixel(Bitmap image)
        {
            Color best = Color.Transparent;
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                    if (image.GetPixel(x, y).A > best.A) best = image.GetPixel(x, y);
            Assert.That(best.A, Is.GreaterThan(127), "Icon must contain a visible glyph.");
            return Color.FromArgb(best.R, best.G, best.B);
        }

        private static void AssertTint(Bitmap image, Color expected)
        {
            Color actual = OpaquePixel(image);
            // GDI+ premultiplied-alpha rounding can shift antialiased channels by one.
            Assert.That(actual.R, Is.EqualTo(expected.R).Within(1));
            Assert.That(actual.G, Is.EqualTo(expected.G).Within(1));
            Assert.That(actual.B, Is.EqualTo(expected.B).Within(1));
        }

        [TestCase(16), TestCase(20), TestCase(24), TestCase(32), TestCase(48)]
        public void Render_UsesRequestedDpiSizeAndThemeColor(int size)
        {
            using Bitmap icon = ThemeIcons.Create("FolderClosed_16x", _manager.ActiveTheme, true, size);
            Assert.That(icon.Size, Is.EqualTo(new Size(size, size)));
            AssertTint(icon, _manager.ActiveTheme.IconColor);
            Assert.That(icon.GetPixel(0, 0).A, Is.Zero);
        }

        [Test]
        public void Render_DisabledUsesSeparateTint()
        {
            using Bitmap icon = ThemeIcons.Create("Run_16x", _manager.ActiveTheme, false, 32);
            AssertTint(icon, _manager.ActiveTheme.DisabledIconColor);
        }

        [Test]
        public void Render_ClassicThemeAndUnknownResourcesFallBack()
        {
            ThemeInfo classic = MakeTheme(); classic.UseFluentIcons = false;
            Assert.That(ThemeIcons.Create("Run_16x", classic, true, 16), Is.Null);
            Assert.That(ThemeIcons.Create("UserSuppliedIcon", _manager.ActiveTheme, true, 16), Is.Null);
        }

        [TestCase(0), TestCase(257)]
        public void Render_InvalidSizeIsRejected(int size) =>
            Assert.Throws<ArgumentOutOfRangeException>(() => ThemeIcons.Create("Run_16x", _manager.ActiveTheme, true, size));

        [Test]
        public void Bind_UpdatesOnThemeAndEnabledChangesAndRestoresStockResource()
        {
            using ToolStrip strip = new(); using ToolStripButton item = new(); strip.Items.Add(item);
            _manager.ThemingActive = false;
            ThemeIcons.Bind(item, "Run_16x");
            Bitmap fallback = (Bitmap)item.Image;
            Bitmap stock = mRemoteNG.Properties.Resources.Run_16x;
            Assert.That(fallback.Size, Is.EqualTo(stock.Size));
            for (int y = 0; y < stock.Height; y++)
                for (int x = 0; x < stock.Width; x++)
                    Assert.That(fallback.GetPixel(x, y).ToArgb(), Is.EqualTo(stock.GetPixel(x, y).ToArgb()));
            _manager.ThemingActive = true;
            AssertTint((Bitmap)item.Image, _manager.ActiveTheme.IconColor);
            item.Enabled = false;
            AssertTint((Bitmap)item.Image, _manager.ActiveTheme.DisabledIconColor);
            item.Enabled = true;
            ThemeInfo next = MakeTheme(); next.IconColor = Color.DarkBlue; _manager.ActiveTheme = next;
            AssertTint((Bitmap)item.Image, Color.DarkBlue);
            _manager.ThemingActive = false;
            // ResourceManager can deserialize a fresh bitmap on each lookup. The
            // binding must restore its own original stock image, still usable.
            Assert.That(item.Image, Is.SameAs(fallback));
            Assert.That(fallback.Width, Is.EqualTo(stock.Width));
        }

        [Test]
        public void Bind_ReplacingOrDisposingBindingDoesNotDisposeStockResource()
        {
            using PictureBox picture = new();
            _manager.ThemingActive = false;
            ThemeIcons.Bind(picture, "Copy_16x");
            Image stock = picture.Image;
            _manager.ThemingActive = true;
            Image first = picture.Image;
            ThemeIcons.Bind(picture, "Copy_16x");
            Assert.That(picture.Image, Is.Not.SameAs(first));
            Assert.Throws<ArgumentException>(() => { _ = first.Width; });
            picture.Dispose();
            Assert.That(stock.Width, Is.GreaterThan(0));
            Assert.DoesNotThrow(() => _manager.refreshUI());
        }

        [Test]
        public void Bind_EachControlOwnsItsImage()
        {
            using PictureBox first = new(); using Button second = new();
            ThemeIcons.Bind(first, "Copy_16x"); ThemeIcons.Bind(second, "Copy_16x");
            Assert.That(first.Image, Is.Not.SameAs(second.Image));
            first.Dispose();
            Assert.That(second.Image.Width, Is.GreaterThan(0));
        }

        [Test]
        public void ConnectionTab_CustomIconIsNotReplacedOnThemeChange()
        {
            using Form form = new();
            mRemoteNG.Connection.ConnectionInfo connection = new() { Icon = "MyCustomIcon" };
            ThemeIcons.BindConnectionTab(form, connection);
            Icon initial = form.Icon;
            _manager.ActiveTheme = MakeTheme();
            Assert.That(form.Icon, Is.SameAs(initial));
            form.Dispose();
            Assert.DoesNotThrow(() => _manager.refreshUI());
        }

        [Test]
        public void Render_AllMappedUiGlyphsExist()
        {
            string[] keys = { "Add_16x", "AddItem_16x", "AddFolder_16x", "FolderClosed_16x", "ASPWebSite_16x",
                "Run_16x", "Stop_16x", "Search_16x", "Save_16x", "Settings_16x", "Refresh_16x", "Favorite_16x",
                "Copy_16x", "Close_16x", "DefaultConnection", "Rename_16x", "SortAscending_16x", "SortDescending_16x",
                "Schema_16x", "Property_16x", "ExtendedProperty_16x", "ViewDownBySchema_16x",
                "Panel_16x", "InsertPanel_16x", "FullScreen_16x", "F1Help_16x", "UIAboutBox_16x", "RunUpdate_16x", "Console_16x",
                "Monitor_16x", "Tab_16x", "LogError_16x", "Key_16x", "SQLDatabase_16x", "Lock_16x",
                "AppearanceEditor_16x", "OptionsPlugins", "DocumentsFolder_16x" };
            foreach (string key in keys) { using Bitmap icon = ThemeIcons.Create(key, _manager.ActiveTheme, true, 32); Assert.That(icon, Is.Not.Null, key); }
        }
    }
}
