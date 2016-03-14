using System.Collections.Generic;
using System;
using System.Drawing;
using Microsoft.VisualBasic;
using System.IO;
using System.Xml;
using System.Reflection;


namespace mRemoteNG.Themes
{
	public class ThemeSerializer
	{
		public static List<Theme> LoadFromXmlFile(string filename)
		{
            ThemeXMLReader xmlReader = new ThemeXMLReader();
            List<Theme> themes = xmlReader.GetThemesFromXMLFile(filename);
            return themes;
		}

        public static void SaveToXmlFile(Theme themeInfo, string filename)
        {
            List<Theme> themeList = new List<Theme>();
            themeList.Add(themeInfo);
            SaveToXmlFile(themeList, filename);
        }

        public static void SaveToXmlFile(List<Theme> themes, string filename)
        {
            ThemeXMLWriter themeXmlWriter = new ThemeXMLWriter(filename);
            themeXmlWriter.SaveToXmlFile(themes);
        }
	}
}