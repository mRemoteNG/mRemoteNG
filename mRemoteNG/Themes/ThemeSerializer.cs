using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Linq;
using System.Runtime.Versioning;

namespace mRemoteNG.Themes
{
    [SupportedOSPlatform("windows")]
    public static class ThemeSerializer
    {
        /// <summary>
        /// Save the theme to file, name property is used as filename
        /// The baseTheme is used as a template, by copy that file and rewrite the extpalette values
        /// </summary>
        /// <param name="themeToSave"></param>
        /// <param name="baseTheme"></param>
        public static void SaveToXmlFile(ThemeInfo themeToSave, ThemeInfo baseTheme)
        {
            var oldURI = baseTheme.URI;
            var directoryName = Path.GetDirectoryName(oldURI);
            var toSaveURI = directoryName + Path.DirectorySeparatorChar + themeToSave.Name + ".vstheme";
            File.Copy(baseTheme.URI, toSaveURI);
            themeToSave.URI = toSaveURI;
        }

        public static void DeleteFile(ThemeInfo themeToDelete)
        {
            File.Delete(themeToDelete.URI);
        }

        /// <summary>
        /// Takes a theme in memory and update the color values that the user might have changed
        /// </summary>
        /// <param name="themeToUpdate"></param>
        public static void UpdateThemeXMLValues(ThemeInfo themeToUpdate)
        {
            var bytesIn = File.ReadAllBytes(themeToUpdate.URI);
            var manipulator = new MremoteNGPaletteManipulator(bytesIn, themeToUpdate.ExtendedPalette);
            var bytesOut = manipulator.mergePalette(themeToUpdate.ExtendedPalette);
            File.WriteAllBytes(themeToUpdate.URI, bytesOut);
        }

        /// <summary>
        /// Load a theme form an xml file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="defaultTheme"></param>
        /// <returns></returns>
        public static ThemeInfo LoadFromXmlFile(string filename, ThemeInfo defaultTheme = null)
        {
            var bytes = File.ReadAllBytes(filename);
            //Load the dockpanel part
            var themeBaseLoad = new MremoteNGThemeBase(bytes);
            //Load the mremote part
            //Cause we cannot default the theme for the default theme
            var extColorLoader = new MremoteNGPaletteManipulator(bytes, defaultTheme?.ExtendedPalette);
            var loadedTheme = new ThemeInfo(Path.GetFileNameWithoutExtension(filename), themeBaseLoad, filename,
                                            VisualStudioToolStripExtender.VsVersion.Vs2015, extColorLoader.getColors());
            if (new[] {"darcula", "vs2015blue", "vs2015dark", "vs2015light"}.Contains(
                                                                                      Path
                                                                                          .GetFileNameWithoutExtension(filename))
            )
            {
                loadedTheme.IsThemeBase = true;
            }

            loadedTheme.IsExtendable = true;
            return loadedTheme;
        }

        /*
		private static string EncodeColorName(Color color)
		{
            // best/simplest answer to converting to hex: http://stackoverflow.com/questions/12078942/how-to-convert-from-argb-to-hex-aarrggbb
            return color.IsNamedColor ? color.Name : $"{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
		}

	    private static Color DecodeColorName(string name)
	    {
	        var regex = new System.Text.RegularExpressions.Regex("^[0-9a-fA-F]{8}$");
	        return regex.Match(name).Success ? Color.FromArgb(Convert.ToInt32(name, 16)) : Color.FromName(name);
	    }
        */
    }
}