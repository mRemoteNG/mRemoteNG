using mRemoteNG.PluginContracts;

namespace mRemoteNG.Plugins;

internal sealed class PluginContext : IPluginContext
{
    public PluginContext(IConnectionImportService connections, IMessageWriter messages, IPluginResources resources)
    {
        Connections = connections;
        Messages = messages;
        Resources = resources;
    }

    public IConnectionImportService Connections { get; }

    public IMessageWriter Messages { get; }

    public IPluginResources Resources { get; }
}
