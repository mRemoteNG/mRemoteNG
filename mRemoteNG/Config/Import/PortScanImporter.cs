using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;


namespace mRemoteNG.Config.Import
{
    [SupportedOSPlatform("windows")]
    public class PortScanImporter(ProtocolType targetProtocolType) : IConnectionImporter<IEnumerable<ScanHost>>
    {
        private readonly ProtocolType _targetProtocolType = targetProtocolType;

        public void Import(IEnumerable<ScanHost> hosts, ContainerInfo destinationContainer)
        {
            if (hosts == null) throw new System.ArgumentNullException(nameof(hosts));
            ScanHost[] boundedHosts = hosts.Take(ImportBudget.MaximumNodes + 1).ToArray();
            if (boundedHosts.Length > ImportBudget.MaximumNodes)
                throw new System.IO.InvalidDataException($"Import exceeds the {ImportBudget.MaximumNodes} node limit.");
            PortScanDeserializer deserializer = new(_targetProtocolType);
            Tree.ConnectionTreeModel connectionTreeModel = deserializer.Deserialize(boundedHosts);
            ImportBudget.ValidateTree(connectionTreeModel);

            ContainerInfo importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            Connection.ConnectionInfo[] childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildrenAtomically(childrenToAdd);
        }
    }
}
