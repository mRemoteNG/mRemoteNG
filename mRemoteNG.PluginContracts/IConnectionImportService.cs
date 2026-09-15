using System.Collections.Generic;

namespace mRemoteNG.PluginContracts;

public interface IConnectionImportService
{
    void ImportConnections(IEnumerable<PluginConnectionRequest> connections);
}
