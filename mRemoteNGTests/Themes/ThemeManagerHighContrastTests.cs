using System.Reflection;
using System.Windows.Forms;
using mRemoteNG.Themes;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.Themes
{
    [TestFixture]
    public class ThemeManagerHighContrastTests
    {
        [Test]
        public void HighContrastTheme_IsConfiguredCorrectly()
        {
            var manager = ThemeManager.getInstance();
            var highContrastTheme = manager.HighContrastTheme;

            Assert.That(highContrastTheme, Is.Not.Null);
            Assert.That(highContrastTheme.Name, Is.EqualTo("HighContrast"));
            Assert.That(highContrastTheme.Theme, Is.InstanceOf<VS2005Theme>());
            Assert.That(highContrastTheme.IsExtendable, Is.False); // Should be false as we didn't extend it
        }

        [Test]
        public void ActiveTheme_ReturnsHighContrast_WhenHighContrastActive()
        {
            var manager = ThemeManager.getInstance();
            
            // Use reflection to set _highContrastActive to true
            var field = typeof(ThemeManager).GetField("_highContrastActive", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null, "Could not find _highContrastActive field");

            bool originalValue = (bool)field.GetValue(manager);
            try
            {
                field.SetValue(manager, true);
                
                var activeTheme = manager.ActiveTheme;
                Assert.That(activeTheme.Name, Is.EqualTo("HighContrast"));
                Assert.That(activeTheme.Theme, Is.InstanceOf<VS2005Theme>());
            }
            finally
            {
                field.SetValue(manager, originalValue);
            }
        }
        
        [Test]
        public void ActiveTheme_ReturnsDefaultOrUser_WhenHighContrastInactive()
        {
            var manager = ThemeManager.getInstance();
            
            // Use reflection to set _highContrastActive to false
            var field = typeof(ThemeManager).GetField("_highContrastActive", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null, "Could not find _highContrastActive field");

            bool originalValue = (bool)field.GetValue(manager);
            try
            {
                field.SetValue(manager, false);
                
                // Assuming default behavior (might depend on user settings, but definitely not HighContrast theme unless explicitly selected, which shouldn't happen via this logic)
                var activeTheme = manager.ActiveTheme;
                Assert.That(activeTheme.Name, Is.Not.EqualTo("HighContrast"));
            }
            finally
            {
                field.SetValue(manager, originalValue);
            }
        }
    }
}
