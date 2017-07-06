using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Themes
{
	public class ThemeSerializer
	{
		public static void SaveToXmlFile(ThemeInfo themeInfo, string filename)
		{
		}
			
		public static void SaveToXmlFile(List<ThemeInfo> themes, string filename)
		{
		
		}
			
		public static ThemeInfo LoadFromXmlFile(string filename, ThemeInfo defaultTheme=null)
		{
            byte[] bytes = File.ReadAllBytes(filename);
            //Load the dockpanel part
            MremoteNGThemeBase themeBaseLoad= new MremoteNGThemeBase(bytes);
            //Load the mremote part
            MremoteNGPaletteLoader extColorLoader;
            //Cause we cannot default the theme for the default theme
            extColorLoader = new MremoteNGPaletteLoader(bytes, defaultTheme ==null ? null:defaultTheme.ExtendedPalette); 
            ThemeInfo loadedTheme = new ThemeInfo(Path.GetFileNameWithoutExtension(filename), themeBaseLoad, filename, VisualStudioToolStripExtender.VsVersion.Vs2015, extColorLoader.getColors());

            return loadedTheme;
		}
			
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
	}
}
