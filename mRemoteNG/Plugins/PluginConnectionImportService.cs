using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.PluginContracts;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Plugins;

internal sealed class PluginConnectionImportService : IConnectionImportService
{
    public void ImportConnections(IEnumerable<PluginConnectionRequest> connections)
    {
        ArgumentNullException.ThrowIfNull(connections);

        PluginConnectionRequest[] requests = connections
            .Where(request => request is not null)
            .Where(request => !string.IsNullOrWhiteSpace(request.Hostname))
            .ToArray();

        if (requests.Length == 0)
        {
            return;
        }

        ContainerInfo destinationContainer = GetDestinationContainer();
        using (App.Runtime.ConnectionsService.BatchedSavingContext())
        {
            foreach (PluginConnectionRequest request in requests)
            {
                if (!PluginProtocolMapper.TryToCoreProtocolType(request.ProtocolId, out var protocolType))
                {
                    throw new ArgumentException($"Unsupported plugin protocol '{request.ProtocolId}'.", nameof(connections));
                }

                ConnectionInfo connectionInfo = new()
                {
                    Name = string.IsNullOrWhiteSpace(request.Name) ? request.Hostname : request.Name,
                    Hostname = request.Hostname,
                    Protocol = protocolType,
                };

                connectionInfo.SetDefaultPort();
                if (request.Port.HasValue)
                {
                    connectionInfo.Port = request.Port.Value;
                }

                destinationContainer.AddChild(connectionInfo);
            }
        }
    }

    private static ContainerInfo GetDestinationContainer()
    {
        ConnectionInfo selectedNode = App.AppWindows.TreeForm.SelectedNode ??
                                      App.AppWindows.TreeForm.ConnectionTree.ConnectionTreeModel.RootNodes
                                          .OfType<RootNodeInfo>()
                                          .First();

        if (selectedNode is RootPuttySessionsNodeInfo or PuttySessionInfo)
        {
            selectedNode = App.AppWindows.TreeForm.ConnectionTree.ConnectionTreeModel.RootNodes
                .OfType<RootNodeInfo>()
                .First();
        }

        return selectedNode as ContainerInfo ?? selectedNode.Parent;
    }
}
