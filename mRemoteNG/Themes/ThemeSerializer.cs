using System;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Linq;
using System.Runtime.Versioning;
using System.Drawing;
using System.Globalization;
using System.Xml;
using mRemoteNG.Security;

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
            if (string.IsNullOrWhiteSpace(baseTheme.URI) || baseTheme.URI.Contains("../") || baseTheme.URI.Contains(@"..\"))
                throw new ArgumentException("Invalid file path");
            if (themeToSave.Name == null || themeToSave.Name.Contains("../") || themeToSave.Name.Contains(@"..\"))
                throw new ArgumentException("Invalid file path");
            string oldURI = baseTheme.URI;
            string directoryName = Path.GetDirectoryName(oldURI);
            if (string.IsNullOrEmpty(directoryName))
                throw new ArgumentException("Invalid file path");
            string toSaveURI = Path.Combine(directoryName, $"{themeToSave.Name}.vstheme");
            File.Copy(baseTheme.URI, toSaveURI);
            themeToSave.URI = toSaveURI;
        }

        public static void DeleteFile(ThemeInfo themeToDelete)
        {
            if (themeToDelete.URI == null || themeToDelete.URI.Contains("../") || themeToDelete.URI.Contains(@"..\"))
                throw new ArgumentException("Invalid file path");
            File.Delete(themeToDelete.URI);
        }

        /// <summary>
        /// Takes a theme in memory and update the color values that the user might have changed
        /// </summary>
        /// <param name="themeToUpdate"></param>
        public static void UpdateThemeXMLValues(ThemeInfo themeToUpdate)
        {
            if (themeToUpdate.URI == null || themeToUpdate.URI.Contains("../") || themeToUpdate.URI.Contains(@"..\"))
                throw new ArgumentException("Invalid file path");
            byte[] bytesIn = File.ReadAllBytes(themeToUpdate.URI);
            MremoteNGPaletteManipulator manipulator = new(bytesIn, themeToUpdate.ExtendedPalette);
            byte[] bytesOut = manipulator.mergePalette(themeToUpdate.ExtendedPalette);
            XmlDocument metadata = ReadMetadata(bytesOut);
            WriteIconMetadata((XmlElement)metadata.SelectSingleNode("/Themes/Theme")!, themeToUpdate);
            using (MemoryStream output = new()) { metadata.Save(output); bytesOut = output.ToArray(); }
            File.WriteAllBytes(themeToUpdate.URI, bytesOut);
        }

        /// <summary>
        /// Load a theme form an xml file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="defaultTheme"></param>
        /// <returns></returns>
        public static ThemeInfo LoadFromXmlFile(string filename, ThemeInfo? defaultTheme = null)
        {
            if (filename == null || filename.Contains("../") || filename.Contains(@"..\"))
                throw new ArgumentException("Invalid file path");
            byte[] bytes = File.ReadAllBytes(filename);
            //Load the dockpanel part
            MremoteNGThemeBase themeBaseLoad = new(bytes);
            //Load the mremote part
            //Cause we cannot default the theme for the default theme
            MremoteNGPaletteManipulator extColorLoader = new(bytes, defaultTheme?.ExtendedPalette);
            ThemeInfo loadedTheme = new(Path.GetFileNameWithoutExtension(filename), themeBaseLoad, filename,
                                            VisualStudioToolStripExtender.VsVersion.Vs2015, extColorLoader.getColors());
            XmlDocument metadata = ReadMetadata(bytes);
            XmlElement? root = metadata.SelectSingleNode("/Themes/Theme") as XmlElement;
            loadedTheme.UseFluentIcons = root?.GetAttribute("IconStyle").Equals("Fluent", StringComparison.OrdinalIgnoreCase) == true;
            loadedTheme.UseComfortableSpacing = root?.GetAttribute("Density").Equals("Comfortable", StringComparison.OrdinalIgnoreCase) == true;
            loadedTheme.IconColor = ParseIconColor(root?.GetAttribute("IconColor"));
            loadedTheme.DisabledIconColor = ParseIconColor(root?.GetAttribute("IconDisabledColor"));
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

        private static Color ParseIconColor(string? value) => value?.Length == 8
            && uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint argb)
            && (argb >> 24) != 0 ? Color.FromArgb(unchecked((int)argb)) : Color.Empty;

        private static XmlDocument ReadMetadata(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);
            using StreamReader reader = new(stream);
            return SecureXmlHelper.LoadXmlFromString(reader.ReadToEnd());
        }

        private static void WriteIconMetadata(XmlElement root, ThemeInfo theme)
        {
            if (theme.UseComfortableSpacing) root.SetAttribute("Density", "Comfortable"); else root.RemoveAttribute("Density");
            if (theme.UseFluentIcons) root.SetAttribute("IconStyle", "Fluent"); else root.RemoveAttribute("IconStyle");
            if (!theme.IconColor.IsEmpty) root.SetAttribute("IconColor", unchecked((uint)theme.IconColor.ToArgb()).ToString("X8")); else root.RemoveAttribute("IconColor");
            if (!theme.DisabledIconColor.IsEmpty) root.SetAttribute("IconDisabledColor", unchecked((uint)theme.DisabledIconColor.ToArgb()).ToString("X8")); else root.RemoveAttribute("IconDisabledColor");
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
