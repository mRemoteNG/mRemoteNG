using System.Drawing;
using mRemoteNG.Properties;
using mRemoteNG.UI.Tabs;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Tabs
{
    [TestFixture]
    public class ConnectionTabAppearanceSettingsTests
    {
        private bool _originalUseCustomFont;
        private string _originalFontName = string.Empty;
        private float _originalFontSize;
        private bool _originalUseCustomColor;
        private string _originalColor = string.Empty;

        [SetUp]
        public void SetUp()
        {
            _originalUseCustomFont = OptionsTabsPanelsPage.Default.UseCustomConnectionTabFont;
            _originalFontName = OptionsTabsPanelsPage.Default.ConnectionTabFontName;
            _originalFontSize = OptionsTabsPanelsPage.Default.ConnectionTabFontSize;
            _originalUseCustomColor = OptionsTabsPanelsPage.Default.UseCustomConnectionTabColor;
            _originalColor = OptionsTabsPanelsPage.Default.ConnectionTabColor;
            ConnectionTabAppearanceSettings.ResetCache();
        }

        [TearDown]
        public void TearDown()
        {
            OptionsTabsPanelsPage.Default.UseCustomConnectionTabFont = _originalUseCustomFont;
            OptionsTabsPanelsPage.Default.ConnectionTabFontName = _originalFontName;
            OptionsTabsPanelsPage.Default.ConnectionTabFontSize = _originalFontSize;
            OptionsTabsPanelsPage.Default.UseCustomConnectionTabColor = _originalUseCustomColor;
            OptionsTabsPanelsPage.Default.ConnectionTabColor = _originalColor;
            ConnectionTabAppearanceSettings.ResetCache();
        }

        [Test]
        public void GetTabColorOverride_ReturnsConnectionColor_WhenProvided()
        {
            OptionsTabsPanelsPage.Default.UseCustomConnectionTabColor = true;
            OptionsTabsPanelsPage.Default.ConnectionTabColor = "#112233";

            Color? result = ConnectionTabAppearanceSettings.GetTabColorOverride(Color.OrangeRed);

            Assert.That(result, Is.EqualTo(Color.OrangeRed));
        }

        [Test]
        public void GetTabColorOverride_ReturnsConfiguredColor_WhenEnabled()
        {
            OptionsTabsPanelsPage.Default.UseCustomConnectionTabColor = true;
            OptionsTabsPanelsPage.Default.ConnectionTabColor = "#112233";

            Color? result = ConnectionTabAppearanceSettings.GetTabColorOverride(null);

            Assert.That(result, Is.EqualTo(Color.FromArgb(0x11, 0x22, 0x33)));
        }

        [Test]
        public void GetTabColorOverride_ReturnsNull_WhenDisabled()
        {
            OptionsTabsPanelsPage.Default.UseCustomConnectionTabColor = false;
            OptionsTabsPanelsPage.Default.ConnectionTabColor = "#112233";

            Color? result = ConnectionTabAppearanceSettings.GetTabColorOverride(null);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetTabFont_ReturnsThemeFont_WhenDisabled()
        {
            OptionsTabsPanelsPage.Default.UseCustomConnectionTabFont = false;

            using Font themeFont = new("Segoe UI", 9f);
            Font result = ConnectionTabAppearanceSettings.GetTabFont(themeFont);

            Assert.That(result, Is.SameAs(themeFont));
        }

        [Test]
        public void GetTabFont_ReturnsConfiguredFont_WhenEnabled()
        {
            using Font themeFont = new("Segoe UI", 9f);

            OptionsTabsPanelsPage.Default.UseCustomConnectionTabFont = true;
            OptionsTabsPanelsPage.Default.ConnectionTabFontName = themeFont.Name;
            OptionsTabsPanelsPage.Default.ConnectionTabFontSize = 11f;

            Font result = ConnectionTabAppearanceSettings.GetTabFont(themeFont);

            Assert.That(result.Name, Is.EqualTo(themeFont.Name));
            Assert.That(result.SizeInPoints, Is.EqualTo(11f).Within(0.01f));
        }
    }
}
