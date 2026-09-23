using System.Collections.Generic;
using Newtonsoft.Json;

namespace mRemoteNG.Plugins;

internal static class PluginConnectionDataSerializer
{
    public static IReadOnlyDictionary<string, string> Deserialize(string serializedProperties)
    {
        if (string.IsNullOrWhiteSpace(serializedProperties))
        {
            return new Dictionary<string, string>();
        }

        return JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedProperties) ??
               new Dictionary<string, string>();
    }

    public static string Serialize(IReadOnlyDictionary<string, string> properties)
    {
        if (properties.Count == 0)
        {
            return string.Empty;
        }

        return JsonConvert.SerializeObject(properties);
    }
}
