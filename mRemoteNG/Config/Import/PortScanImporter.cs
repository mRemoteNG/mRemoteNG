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
    public class PortScanImporter : IConnectionImporter<IEnumerable<ScanHost>>
    {
        private readonly ProtocolType _targetProtocolType;

        public PortScanImporter(ProtocolType targetProtocolType)
        {
            _targetProtocolType = targetProtocolType;
        }

        public void Import(IEnumerable<ScanHost> hosts, ContainerInfo destinationContainer)
        {
            PortScanDeserializer deserializer = new(_targetProtocolType);
            Tree.ConnectionTreeModel connectionTreeModel = deserializer.Deserialize(hosts);

            ContainerInfo importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            Connection.ConnectionInfo[] childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildRange(childrenToAdd);
        }
    }
}