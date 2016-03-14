using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Reflection;

namespace mRemoteNG.Themes
{
    class ThemeXMLReader
    {
        private XmlDocument _xmlDocument;

        public ThemeXMLReader()
        {
            _xmlDocument = new XmlDocument();
        }

        public List<Theme> GetThemesFromXMLFile(string filename)
        {
            _xmlDocument.Load(filename);
            ValidateThemesFile();
            List<Theme> themes = GetDeserializedXmlThemes();
            return themes;
        }

        private void ValidateThemesFile()
        {
            ValidateFileInfoVersionSupported();
            ValidateFileTypeSupported();
            ValidateFileTypeVersionSupported();
        }

        private List<Theme> GetDeserializedXmlThemes()
        {
            List<Theme> themes = new List<Theme>();
            foreach (XmlNode themeXMLNode in GetAllThemeXMLNodes())
            {
                themes.Add(CreateThemeFromXML(themeXMLNode));
            }
            return themes;
        }

        private XmlNodeList GetAllThemeXMLNodes()
        {
            return _xmlDocument.SelectNodes("/mRemoteNG/Theme");
        }

        private Theme CreateThemeFromXML(XmlNode themeXmlNode)
        {
            Theme theme = new Theme();
            theme.Name = themeXmlNode.Attributes["Name"].Value;
            SetThemeColorsFromXML(themeXmlNode, theme);
            return theme;
        }

        private void SetThemeColorsFromXML(XmlNode themeXmlNode, Theme theme)
        {
            foreach (XmlNode colorNode in themeXmlNode.SelectNodes("./Color"))
            {
                SetThemeColorFromXML(colorNode, theme);
            }
        }

        private void SetThemeColorFromXML(XmlNode colorNode, Theme theme)
        {
            string colorName = colorNode.Attributes["Name"].Value;
            string colorValue = colorNode.Attributes["Value"].Value;
            PropertyInfo propertyInfo = typeof(Theme).GetProperty(colorName);
            if (propertyInfo != null || propertyInfo.PropertyType == typeof(Color))
                propertyInfo.SetValue(theme, DecodeColorName(colorValue), null);
        }

        private void ValidateFileInfoVersionSupported()
        {
            Version fileInfoVersion = new Version(GetFileInfoNode().Attributes["Version"].Value);
            if (fileInfoVersion > new Version(1, 0))
            {
                throw (new FileFormatException(string.Format("Unsupported FileInfo version ({0}).", fileInfoVersion)));
            }
        }

        private void ValidateFileTypeSupported()
        {
            XmlNode fileTypeNode = GetFileInfoNode().SelectSingleNode("./FileType");
            string fileType = fileTypeNode.InnerText;
            if (!(fileType == "Theme"))
            {
                throw (new FileFormatException(string.Format("Incorrect FileType ({0}). Expected \"Theme\".", fileType)));
            }
        }

        private void ValidateFileTypeVersionSupported()
        {
            Version fileTypeVersion = new Version(GetFileInfoNode().SelectSingleNode("./FileTypeVersion").InnerText);
            if (fileTypeVersion > new Version(1, 0))
            {
                throw (new FileFormatException(string.Format("Unsupported FileTypeVersion ({0}).", fileTypeVersion)));
            }
        }

        private XmlNode GetFileInfoNode()
        {
            return _xmlDocument.SelectSingleNode("/mRemoteNG/FileInfo");
        }

        private static Color DecodeColorName(string name)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[0-9a-fA-F]{8}$");
            if (regex.Match(name).Success)
                return Color.FromArgb(Convert.ToInt32(name, 16));
            else
                return Color.FromName(name);
        }
    }
}