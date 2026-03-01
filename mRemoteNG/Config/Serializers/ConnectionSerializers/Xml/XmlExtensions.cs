using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Xml;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    public static class XmlExtensions
    {
        private static readonly ConcurrentDictionary<(Type, string), object?> s_enumCache = new();

        public static string GetAttributeAsString(this XmlNode xmlNode, string attribute, string defaultValue = "")
        {
            string? value = xmlNode?.Attributes?[attribute]?.Value;
            return value ?? defaultValue;
        }

        public static bool GetAttributeAsBool(this XmlNode xmlNode, string attribute, bool defaultValue = false)
        {
            string? value = xmlNode?.Attributes?[attribute]?.Value;
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return bool.TryParse(value, out bool valueAsBool)
                ? valueAsBool
                : defaultValue;
        }

        public static int GetAttributeAsInt(this XmlNode xmlNode, string attribute, int defaultValue = 0)
        {
            string? value = xmlNode?.Attributes?[attribute]?.Value;
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int valueAsBool)
                ? valueAsBool
                : defaultValue;
        }

        public static T GetAttributeAsEnum<T>(this XmlNode xmlNode, string attribute, T defaultValue = default)
            where T : struct
        {
            string? value = xmlNode?.Attributes?[attribute]?.Value;
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            var cacheKey = (typeof(T), value);
            if (s_enumCache.TryGetValue(cacheKey, out object? cached))
                return cached is T t ? t : defaultValue;

            bool parsed = Enum.TryParse<T>(value, true, out T result);
            s_enumCache[cacheKey] = parsed ? result : null;
            return parsed ? result : defaultValue;
        }
    }
}
