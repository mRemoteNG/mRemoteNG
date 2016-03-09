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
		public static void SaveToXmlFile(ThemeInfo themeInfo, string filename)
		{
			List<ThemeInfo> themeList = new List<ThemeInfo>();
			themeList.Add(themeInfo);
			SaveToXmlFile(themeList, filename);
		}
			
		public static void SaveToXmlFile(List<ThemeInfo> themes, string filename)
		{
			string tempFileName = Path.GetTempFileName();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(tempFileName, System.Text.Encoding.UTF8);
				
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.Indentation = 4;
				
			xmlTextWriter.WriteStartDocument();
				
			xmlTextWriter.WriteStartElement("mRemoteNG");
				
			xmlTextWriter.WriteStartElement("FileInfo");
			xmlTextWriter.WriteAttributeString("Version", "1.0");
			xmlTextWriter.WriteElementString("FileType", "Theme");
			xmlTextWriter.WriteElementString("FileTypeVersion", "1.0");
			xmlTextWriter.WriteEndElement(); // FileInfo
				
			Type themeType = (new ThemeInfo()).GetType();
			Type colorType = (new Color()).GetType();
			Color color = new Color();
			foreach (ThemeInfo themeInfo in themes)
			{
				xmlTextWriter.WriteStartElement("Theme");
				xmlTextWriter.WriteAttributeString("Name", themeInfo.Name);
					
				foreach (PropertyInfo propertyInfo in themeType.GetProperties())
				{
					if (!(propertyInfo.PropertyType == colorType))
					{
						continue;
					}
					color = (Color)propertyInfo.GetValue(themeInfo, null);
					xmlTextWriter.WriteStartElement("Color");
					xmlTextWriter.WriteAttributeString("Name", propertyInfo.Name);
					xmlTextWriter.WriteAttributeString("Value", EncodeColorName(color));
					xmlTextWriter.WriteEndElement(); // Color
				}
					
				xmlTextWriter.WriteEndElement(); // Theme
			}
				
			xmlTextWriter.WriteEndElement(); // mRemoteNG
				
			xmlTextWriter.Close();
				
			File.Delete(filename);
			File.Move(tempFileName, filename);
		}
			
		public static List<ThemeInfo> LoadFromXmlFile(string filename)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
				
			XmlNode fileInfoNode = xmlDocument.SelectSingleNode("/mRemoteNG/FileInfo");
			Version fileInfoVersion = new Version(fileInfoNode.Attributes["Version"].Value);
			if (fileInfoVersion > new Version(1, 0))
			{
				throw (new FileFormatException(string.Format("Unsupported FileInfo version ({0}).", fileInfoVersion)));
			}
				
			XmlNode fileTypeNode = fileInfoNode.SelectSingleNode("./FileType");
			string fileType = fileTypeNode.InnerText;
			if (!(fileType == "Theme"))
			{
				throw (new FileFormatException(string.Format("Incorrect FileType ({0}). Expected \"Theme\".", fileType)));
			}
				
			Version fileTypeVersion = new Version(fileInfoNode.SelectSingleNode("./FileTypeVersion").InnerText);
			if (fileTypeVersion > new Version(1, 0))
			{
				throw (new FileFormatException(string.Format("Unsupported FileTypeVersion ({0}).", fileTypeVersion)));
			}
				
			XmlNodeList themeNodes = xmlDocument.SelectNodes("/mRemoteNG/Theme");
			List<ThemeInfo> themes = new List<ThemeInfo>();
			ThemeInfo themeInfo = default(ThemeInfo);
			Type themeType = (new ThemeInfo()).GetType();
			Type colorType = (new Color()).GetType();
			string colorName = "";
			string colorValue = "";
			PropertyInfo propertyInfo = default(PropertyInfo);
			foreach (XmlNode themeNode in themeNodes)
			{
				themeInfo = new ThemeInfo();
				themeInfo.Name = themeNode.Attributes["Name"].Value;
				foreach (XmlNode colorNode in themeNode.SelectNodes("./Color"))
				{
					colorName = colorNode.Attributes["Name"].Value;
					colorValue = colorNode.Attributes["Value"].Value;
					propertyInfo = themeType.GetProperty(colorName);
					if (propertyInfo == null || !(propertyInfo.PropertyType == colorType))
					{
						continue;
					}
					propertyInfo.SetValue(themeInfo, DecodeColorName(colorValue), null);
				}
				themes.Add(themeInfo);
			}
				
			return themes;
		}
			
		private static string EncodeColorName(Color color)
		{
			if (color.IsNamedColor)
			{
				return color.Name;
			}
			else
			{
				return Conversion.Hex(color.ToArgb()).PadLeft(8, '0');
			}
		}
			
		private static Color DecodeColorName(string name)
		{
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[0-9a-fA-F]{8}$");
			if (regex.Match(name).Success)
			{
				return Color.FromArgb(Convert.ToInt32(name, 16));
			}
			else
			{
				return Color.FromName(name);
			}
		}
	}
}
