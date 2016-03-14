using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace mRemoteNG.Themes
{
    class ThemeXMLWriter
    {
        private XmlTextWriter _XmlTextWriter;
        private string _FinalFilePath;
        private string _TempFilePath;

        public ThemeXMLWriter(string FileName)
        {
            _FinalFilePath = FileName;
            _TempFilePath = Path.GetTempFileName();
            this._XmlTextWriter = new XmlTextWriter(_TempFilePath, System.Text.Encoding.UTF8);
            this._XmlTextWriter.Formatting = Formatting.Indented;
            this._XmlTextWriter.Indentation = 4;
        }

        public void SaveToXmlFile(List<Theme> themes)
        {
            WriteThemeSettingsToXML(themes);
            ReplaceOldThemeFileWithUpdatedCopy();
        }

        private void ReplaceOldThemeFileWithUpdatedCopy()
        {
            File.Delete(_FinalFilePath);
            File.Move(_TempFilePath, _FinalFilePath);
        }

        private void WriteThemeSettingsToXML(List<Theme> themes)
        {
            WriteOpeningSettingsStatementToXML();
            WriteFileInfoToXML();
            WriteAllThemesToXML(themes);
            WriteEndingSettingsStatementToXML();
        }

        private void WriteOpeningSettingsStatementToXML()
        {
            this._XmlTextWriter.WriteStartDocument();
            this._XmlTextWriter.WriteStartElement("mRemoteNG");
        }

        private void WriteEndingSettingsStatementToXML()
        {
            this._XmlTextWriter.WriteEndElement();
            this._XmlTextWriter.Close();
        }

        private void WriteFileInfoToXML()
        {
            this._XmlTextWriter.WriteStartElement("FileInfo");
            this._XmlTextWriter.WriteAttributeString("Version", "1.0");
            this._XmlTextWriter.WriteElementString("FileType", "Theme");
            this._XmlTextWriter.WriteElementString("FileTypeVersion", "1.0");
            this._XmlTextWriter.WriteEndElement();
        }

        private void WriteAllThemesToXML(List<Theme> themes)
        {
            foreach (Theme theme in themes)
            {
                WriteThemeToXML(theme);
            }
        }

        private void WriteThemeToXML(Theme theme)
        {
            this._XmlTextWriter.WriteStartElement("Theme");
            this._XmlTextWriter.WriteAttributeString("Name", theme.Name);
            WriteAllThemePropertiesToXML(theme);
            this._XmlTextWriter.WriteEndElement();
        }

        private void WriteAllThemePropertiesToXML(Theme theme)
        {
            foreach (PropertyInfo property in typeof(Theme).GetProperties())
            {
                SerializeThemePropertyToXML(theme, property);
            }
        }

        private void SerializeThemePropertyToXML(Theme theme, PropertyInfo property)
        {
            if (property.PropertyType == typeof(Color))
            {
                WriteThemeColorPropertyToXML(theme, property);
            }
        }

        private void WriteThemeColorPropertyToXML(Theme theme, PropertyInfo property)
        {
            Color color = (Color)property.GetValue(theme, null);
            this._XmlTextWriter.WriteStartElement("Color");
            this._XmlTextWriter.WriteAttributeString("Name", property.Name);
            this._XmlTextWriter.WriteAttributeString("Value", EncodeColorName(color));
            this._XmlTextWriter.WriteEndElement();
        }

        private string EncodeColorName(Color color)
        {
            if (color.IsNamedColor)
                return color.Name;
            else
                return EncodeColorAsARGB(color);
        }

        private string EncodeColorAsARGB(Color color)
        {
            int argb = color.ToArgb();
            string hex = String.Format("{0:X}", argb);
            return hex.PadLeft(8, '0');
        }
    }
}