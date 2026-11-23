using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using mRemoteNG.Themes;
using mRemoteNG.UI.Forms.OptionsPages;
using NUnit.Framework;

namespace mRemoteNGTests.Themes
{
    [TestFixture]
    public class FluentThemeMetadataTests
    {
        private static string ThemePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Themes", "Catppuccin Mocha.vstheme");

        [Test]
        public void PaletteLabels_UseJapaneseOnlyForJapaneseUi()
        {
            const string key = "Dialog_Foreground";
            Assert.That(ThemePage.ColorKeyDisplayName(key, CultureInfo.GetCultureInfo("en-US")), Is.EqualTo("Dialog: Foreground"));
            Assert.That(ThemePage.ColorKeyDisplayName(key, CultureInfo.GetCultureInfo("sv-SE")), Is.EqualTo("Dialog: Foreground"));
            Assert.That(ThemePage.ColorKeyDisplayName(key, CultureInfo.GetCultureInfo("ja-JP")), Is.EqualTo("ダイアログ: 文字"));
            Assert.That(ThemePage.ColorKeyDisplayName("Custom_Unknown_Key", CultureInfo.GetCultureInfo("en-US")), Is.EqualTo("Custom_Unknown_Key"));
        }

        [Test]
        public void BundledProfile_LoadsFluentStyleAndExactMochaTints()
        {
            ThemeInfo theme = ThemeSerializer.LoadFromXmlFile(ThemePath);
            Assert.That(theme.Name, Is.EqualTo("Catppuccin Mocha"));
            Assert.That(theme.UseFluentIcons, Is.True);
            Assert.That(theme.UseComfortableSpacing, Is.True);
            Assert.That(theme.IconColor.ToArgb(), Is.EqualTo(Color.FromArgb(203, 166, 247).ToArgb()));
            Assert.That(theme.DisabledIconColor.ToArgb(), Is.EqualTo(Color.FromArgb(108, 112, 134).ToArgb()));
            Assert.That(theme.ExtendedPalette.getColor("TextBox_Border").ToArgb(), Is.EqualTo(Color.FromArgb(108, 112, 134).ToArgb()));
        }

        [Test]
        public void BundledProfile_CaptionAndSelectedTabTextHasReadableContrast()
        {
            var colors = ThemeSerializer.LoadFromXmlFile(ThemePath).Theme.ColorPalette;
            AssertContrast(colors.ToolWindowCaptionActive.Text, colors.ToolWindowCaptionActive.Background);
            AssertContrast(colors.ToolWindowCaptionInactive.Text, colors.ToolWindowCaptionInactive.Background);
            AssertContrast(colors.TabSelectedActive.Text, colors.TabSelectedActive.Background);
            AssertContrast(colors.TabSelectedInactive.Text, colors.TabSelectedInactive.Background);
        }

        private static void AssertContrast(Color text, Color background)
        {
            static double Channel(byte value)
            {
                double channel = value / 255d;
                return channel <= 0.04045 ? channel / 12.92 : Math.Pow((channel + 0.055) / 1.055, 2.4);
            }
            static double Light(Color color) => 0.2126 * Channel(color.R) + 0.7152 * Channel(color.G) + 0.0722 * Channel(color.B);
            double a = Light(text), b = Light(background);
            Assert.That((Math.Max(a, b) + 0.05) / (Math.Min(a, b) + 0.05), Is.GreaterThanOrEqualTo(4.5));
        }

        [Test]
        public void Clone_PreservesStyleAndColors()
        {
            ThemeInfo theme = ThemeSerializer.LoadFromXmlFile(ThemePath);
            ThemeInfo clone = (ThemeInfo)theme.Clone();
            Assert.That(clone.UseFluentIcons, Is.True);
            Assert.That(clone.UseComfortableSpacing, Is.True);
            Assert.That(clone.IconColor, Is.EqualTo(theme.IconColor));
            Assert.That(clone.DisabledIconColor, Is.EqualTo(theme.DisabledIconColor));
        }

        [Test]
        public void PaletteUpdate_RoundTripsIconMetadata()
        {
            string folder = Path.Combine(Path.GetTempPath(), "mRemoteNG-fluent-tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(folder);
            try
            {
                string path = Path.Combine(folder, "preview.vstheme"); File.Copy(ThemePath, path);
                ThemeInfo theme = ThemeSerializer.LoadFromXmlFile(path); theme.IconColor = Color.DarkMagenta;
                ThemeSerializer.UpdateThemeXMLValues(theme);
                ThemeInfo reloaded = ThemeSerializer.LoadFromXmlFile(path);
                Assert.That(reloaded.UseFluentIcons, Is.True);
                Assert.That(reloaded.UseComfortableSpacing, Is.True);
                Assert.That(reloaded.IconColor.ToArgb(), Is.EqualTo(Color.DarkMagenta.ToArgb()));
                theme.UseFluentIcons = false; theme.UseComfortableSpacing = false; ThemeSerializer.UpdateThemeXMLValues(theme);
                Assert.That(ThemeSerializer.LoadFromXmlFile(path).UseFluentIcons, Is.False);
                Assert.That(ThemeSerializer.LoadFromXmlFile(path).UseComfortableSpacing, Is.False);
            }
            finally { Directory.Delete(folder, true); }
        }
    }
}
