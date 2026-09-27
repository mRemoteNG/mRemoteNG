using System.Threading;
using System.Threading.Tasks;

namespace mRemoteNG.PluginContracts;

public interface IConnectionAddressResolverPlugin : IPlugin
{
    bool CanResolve(IPluginConnection connection);

    Task ResolveAsync(IPluginConnection connection, CancellationToken cancellationToken);
}
