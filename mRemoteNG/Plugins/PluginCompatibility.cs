using System;
using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins;

public static class PluginCompatibility
{
    public static bool IsCompatible(IPlugin plugin, Version hostVersion)
    {
        ArgumentNullException.ThrowIfNull(plugin);
        ArgumentNullException.ThrowIfNull(hostVersion);

        return plugin.MinimumHostVersion <= hostVersion;
    }
}
