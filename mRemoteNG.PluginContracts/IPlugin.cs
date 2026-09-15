using System;

namespace mRemoteNG.PluginContracts;

public interface IPlugin
{
    string Id { get; }

    string DisplayName { get; }

    Version Version { get; }

    Version MinimumHostVersion { get; }

    void Initialize(IPluginContext context);
}
