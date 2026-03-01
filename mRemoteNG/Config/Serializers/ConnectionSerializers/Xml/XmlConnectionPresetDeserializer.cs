using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Xml.Linq;
using mRemoteNG.Connection;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionPresetDeserializer : IDeserializer<string, IReadOnlyList<ConnectionPreset>>
    {
        public IReadOnlyList<ConnectionPreset> Deserialize(string serializedData)
        {
            if (string.IsNullOrWhiteSpace(serializedData))
                return Array.Empty<ConnectionPreset>();

            XDocument document = XDocument.Parse(serializedData);
            XElement? rootElement = document.Root;
            if (rootElement == null || !string.Equals(rootElement.Name.LocalName, "ConnectionPresets", StringComparison.OrdinalIgnoreCase))
                return Array.Empty<ConnectionPreset>();

            Dictionary<string, PropertyInfo> connectionProperties =
                ConnectionPreset.ConfigurableConnectionProperties
                    .ToDictionary(property => property.Name, StringComparer.Ordinal);

            Dictionary<string, PropertyInfo> inheritanceProperties =
                ConnectionPreset.ConfigurableInheritanceProperties
                    .ToDictionary(property => property.Name, StringComparer.Ordinal);

            List<ConnectionPreset> presets = new();
            foreach (XElement presetElement in rootElement.Elements("Preset"))
            {
                string presetName = presetElement.Attribute("Name")?.Value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(presetName))
                    continue;

                ConnectionPreset preset = new(presetName);

                DeserializeProperties(
                    presetElement.Element("Connection"),
                    preset.ConnectionInfo,
                    connectionProperties);

                DeserializeProperties(
                    presetElement.Element("Inheritance"),
                    preset.Inheritance,
                    inheritanceProperties);

                presets.Add(preset);
            }

            return presets;
        }

        private static void DeserializeProperties(
            XElement? propertiesElement,
            object destination,
            IReadOnlyDictionary<string, PropertyInfo> propertyMap)
        {
            if (propertiesElement == null)
                return;

            foreach (XElement propertyElement in propertiesElement.Elements("Property"))
            {
                string propertyName = propertyElement.Attribute("Name")?.Value ?? string.Empty;
                if (string.IsNullOrWhiteSpace(propertyName))
                    continue;

                if (!propertyMap.TryGetValue(propertyName, out PropertyInfo? property) || !property.CanWrite)
                    continue;

                string serializedValue = propertyElement.Attribute("Value")?.Value ?? string.Empty;
                if (!TryParsePropertyValue(serializedValue, property.PropertyType, out object? parsedValue))
                    continue;

                property.SetValue(destination, parsedValue, null);
            }
        }

        private static bool TryParsePropertyValue(string value, Type targetType, out object? parsedValue)
        {
            if (targetType == typeof(string))
            {
                parsedValue = value;
                return true;
            }

            if (string.IsNullOrEmpty(value))
            {
                parsedValue = targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
                return true;
            }

            if (targetType == typeof(bool))
            {
                if (bool.TryParse(value, out bool boolValue))
                {
                    parsedValue = boolValue;
                    return true;
                }

                parsedValue = null;
                return false;
            }

            if (targetType.IsEnum)
            {
                if (Enum.TryParse(targetType, value, true, out object? enumValue))
                {
                    parsedValue = enumValue;
                    return true;
                }

                parsedValue = null;
                return false;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(targetType);
            if (converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    parsedValue = converter.ConvertFromInvariantString(value);
                    return true;
                }
                catch
                {
                    // fall through to Convert.ChangeType
                }
            }

            try
            {
                parsedValue = Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                parsedValue = null;
                return false;
            }
        }
    }
}
