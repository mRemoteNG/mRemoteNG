using System.Collections.Generic;

namespace mRemoteNG.PluginContracts;

public interface IConnectionPropertyProviderPlugin : IPlugin
{
    IReadOnlyCollection<ConnectionPropertyDefinition> ConnectionProperties { get; }
}
