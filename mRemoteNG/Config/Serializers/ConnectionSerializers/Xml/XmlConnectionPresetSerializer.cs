using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using mRemoteNG.Connection;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionPresetSerializer : ISerializer<IEnumerable<ConnectionPreset>, string>
    {
        public Version Version { get; } = new(1, 0);

        public string Serialize(IEnumerable<ConnectionPreset> presets)
        {
            ArgumentNullException.ThrowIfNull(presets);

            XElement rootElement = new(
                "ConnectionPresets",
                new XAttribute("Version", Version.ToString(2)));

            foreach (ConnectionPreset preset in presets)
            {
                if (preset == null)
                    continue;

                rootElement.Add(SerializePreset(preset));
            }

            XDocument document = new(new XDeclaration("1.0", "utf-8", null), rootElement);
            return WriteXmlToString(document);
        }

        private static XElement SerializePreset(ConnectionPreset preset)
        {
            XElement presetElement = new(
                "Preset",
                new XAttribute("Name", preset.Name));

            presetElement.Add(
                SerializePropertyValues(
                    "Connection",
                    preset.ConnectionInfo,
                    ConnectionPreset.ConfigurableConnectionProperties));

            presetElement.Add(
                SerializePropertyValues(
                    "Inheritance",
                    preset.Inheritance,
                    ConnectionPreset.ConfigurableInheritanceProperties));

            return presetElement;
        }

        private static XElement SerializePropertyValues(
            string elementName,
            object source,
            IEnumerable<PropertyInfo> properties)
        {
            XElement element = new(elementName);
            foreach (PropertyInfo property in properties)
            {
                if (!property.CanRead)
                    continue;

                object? value = property.GetValue(source, null);
                string serializedValue = SerializePropertyValue(value, property.PropertyType);

                element.Add(
                    new XElement(
                        "Property",
                        new XAttribute("Name", property.Name),
                        new XAttribute("Value", serializedValue)));
            }

            return element;
        }

        private static string SerializePropertyValue(object? value, Type propertyType)
        {
            if (value == null)
                return string.Empty;

            if (propertyType == typeof(bool))
                return ((bool)value).ToString().ToLowerInvariant();

            if (propertyType.IsEnum)
                return value.ToString() ?? string.Empty;

            if (value is IFormattable formattable)
                return formattable.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty;

            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static string WriteXmlToString(XNode xmlDocument)
        {
            XmlWriterSettings xmlWriterSettings = new()
            {
                Indent = true,
                IndentChars = "    ",
                Encoding = Encoding.UTF8
            };

            using MemoryStream memoryStream = new();
            using (XmlWriter xmlTextWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
            {
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            using StreamReader streamReader = new(memoryStream, Encoding.UTF8, true);
            return streamReader.ReadToEnd();
        }
    }
}
