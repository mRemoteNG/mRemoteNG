namespace mRemoteNG.PluginContracts;

public interface IPluginContext
{
    IConnectionImportService Connections { get; }

    IMessageWriter Messages { get; }

    IPluginResources Resources { get; }
}
