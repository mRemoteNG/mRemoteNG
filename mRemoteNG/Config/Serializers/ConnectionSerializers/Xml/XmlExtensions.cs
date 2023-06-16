using System;
using System.Xml;

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

        public static T GetAttributeAsEnum<T>(this XmlNode xmlNode, string attribute, T defaultValue = default)
            where T : struct
        {
            var value = xmlNode?.Attributes?[attribute]?.Value;
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return Enum.TryParse<T>(value, true, out var valueAsEnum)
                ? valueAsEnum
                : defaultValue;
        }
    }
}