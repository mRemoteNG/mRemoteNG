using System;
using System.IO;
using mRemoteNG.Themes;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNGTests.Themes;

[TestFixture]
public class ThemeSerializerTests
{
    [Test]
    public void SaveToXmlFile_WithEmptyBaseThemeUri_ThrowsArgumentException()
    {
        ThemeInfo baseTheme = new("baseTheme", new VS2015LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015);
        ThemeInfo themeToSave = new("newTheme", new VS2015LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015);

        Assert.Throws<ArgumentException>(() => ThemeSerializer.SaveToXmlFile(themeToSave, baseTheme));
    }

    [Test]
    public void SaveToXmlFile_WithWhitespaceBaseThemeUri_ThrowsArgumentException()
    {
        ThemeInfo baseTheme = new("baseTheme", new VS2015LightTheme(), "   ", VisualStudioToolStripExtender.VsVersion.Vs2015);
        ThemeInfo themeToSave = new("newTheme", new VS2015LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015);

        Assert.Throws<ArgumentException>(() => ThemeSerializer.SaveToXmlFile(themeToSave, baseTheme));
    }

    [Test]
    public void SaveToXmlFile_WithBaseThemeUriWithoutDirectory_ThrowsArgumentException()
    {
        ThemeInfo baseTheme = new("baseTheme", new VS2015LightTheme(), "base.vstheme", VisualStudioToolStripExtender.VsVersion.Vs2015);
        ThemeInfo themeToSave = new("newTheme", new VS2015LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015);

        Assert.Throws<ArgumentException>(() => ThemeSerializer.SaveToXmlFile(themeToSave, baseTheme));
    }

    [Test]
    public void SaveToXmlFile_WithValidBaseThemeUri_CopiesThemeToSameDirectory()
    {
        string testDirectory = Path.Combine(Path.GetTempPath(), "mRemoteNGTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(testDirectory);
        string baseThemePath = Path.Combine(testDirectory, "base.vstheme");
        string expectedPath = Path.Combine(testDirectory, "newTheme.vstheme");
        File.WriteAllText(baseThemePath, "theme");
        ThemeInfo baseTheme = new("baseTheme", new VS2015LightTheme(), baseThemePath, VisualStudioToolStripExtender.VsVersion.Vs2015);
        ThemeInfo themeToSave = new("newTheme", new VS2015LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015);

        try
        {
            ThemeSerializer.SaveToXmlFile(themeToSave, baseTheme);

            Assert.That(themeToSave.URI, Is.EqualTo(expectedPath));
            Assert.That(File.Exists(expectedPath), Is.True);
        }
        finally
        {
            if (Directory.Exists(testDirectory))
                Directory.Delete(testDirectory, true);
        }
    }
}
