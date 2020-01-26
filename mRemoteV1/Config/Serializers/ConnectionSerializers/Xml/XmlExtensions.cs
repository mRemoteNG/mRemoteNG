using System;
using System.Xml;
using System.Xml.Linq;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    public static class XmlExtensions
    {
        public static string GetAttributeAsString(this XmlNode xmlNode, string attribute, string defaultValue = "")
        {
            var value = xmlNode?.Attributes?[attribute]?.Value;
            return value ?? defaultValue;
        }

        public static bool GetAttributeAsBool(this XmlNode xmlNode, string attribute, bool defaultValue = false)
        {
            var value = xmlNode?.Attributes?[attribute]?.Value;
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return bool.TryParse(value, out var valueAsBool)
                ? valueAsBool
                : defaultValue;
        }

        public static int GetAttributeAsInt(this XmlNode xmlNode, string attribute, int defaultValue = 0)
        {
            var value = xmlNode?.Attributes?[attribute]?.Value;
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return int.TryParse(value, out var valueAsBool)
                ? valueAsBool
                : defaultValue;
        }

        public static T GetAttributeAsEnum<T>(this XmlNode xmlNode, string attribute, T defaultValue = default(T))
            where T : struct
        {
            var value = xmlNode?.Attributes?[attribute]?.Value;
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return Enum.TryParse<T>(value, true, out var valueAsEnum)
                ? valueAsEnum
                : defaultValue;
        }

        public static string GetChildElementAsString(this XElement xmlNode, string elementName, string defaultValue = "")
        {
            var value = xmlNode?.Element(elementName)?.Value;
            return value ?? defaultValue;
        }

        public static bool GetChildElementAsBool(this XElement xmlNode, string elementName, bool defaultValue = false)
        {
            var value = xmlNode?.GetChildElementAsString(elementName);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return bool.TryParse(value, out var valueAsBool)
                ? valueAsBool
                : defaultValue;
        }

        public static int GetChildElementAsInt(this XElement xmlNode, string elementName, int defaultValue = 0)
        {
            var value = xmlNode?.GetChildElementAsString(elementName);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return int.TryParse(value, out var valueAsBool)
                ? valueAsBool
                : defaultValue;
        }

        public static T GetChildElementAsEnum<T>(this XElement xmlNode, string elementName, T defaultValue = default(T))
            where T : struct
        {
            var value = xmlNode?.GetChildElementAsString(elementName);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return Enum.TryParse<T>(value, true, out var valueAsEnum)
                ? valueAsEnum
                : defaultValue;
        }
    }
}