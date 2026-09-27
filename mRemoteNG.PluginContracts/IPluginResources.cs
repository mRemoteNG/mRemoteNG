using System.Drawing;

namespace mRemoteNG.PluginContracts;

public interface IPluginResources
{
    Image? GetImage(string resourceName);

    string GetString(string resourceName, string fallback);
}
