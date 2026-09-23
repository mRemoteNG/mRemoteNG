using System.Drawing;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins.SshTransfer;

internal sealed class Resources(IPluginContext pluginContext)
{
    private readonly IPluginContext _pluginContext = pluginContext;

    public string GetString(string resourceName, string fallback)
    {
        return _pluginContext.Resources.GetString(resourceName, fallback);
    }

    public Image? GetImage(string resourceName)
    {
        return _pluginContext.Resources.GetImage(resourceName) as Image;
    }
}
