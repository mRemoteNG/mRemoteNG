using System.Drawing;
using System.Reflection;
using mRemoteNG.PluginContracts;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Plugins;

internal sealed class PluginResources : IPluginResources
{
    public Image? GetImage(string resourceName)
    {
        PropertyInfo? property = typeof(Properties.Resources).GetProperty(resourceName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        return property?.GetValue(null) as Image;
    }

    public string GetString(string resourceName, string fallback)
    {
        PropertyInfo? property = typeof(Language).GetProperty(resourceName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        return property?.GetValue(null) as string ?? fallback;
    }
}
