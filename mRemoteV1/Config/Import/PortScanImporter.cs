using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tools;


namespace mRemoteNG.Config.Import
{
	public class PortScanImporter : IConnectionImporter<IEnumerable<ScanHost>>
	{
	    private readonly ProtocolType _targetProtocolType;

	    public PortScanImporter(ProtocolType targetProtocolType)
	    {
	        _targetProtocolType = targetProtocolType;
	    }

        public void Import(IEnumerable<ScanHost> hosts, ContainerInfo destinationContainer)
		{
			var deserializer = new PortScanDeserializer(_targetProtocolType);
            var connectionTreeModel = deserializer.Deserialize(hosts);

            var importedRootNode = connectionTreeModel.RootNodes.First();
            if (importedRootNode == null) return;
            var childrenToAdd = importedRootNode.Children.ToArray();
            destinationContainer.AddChildRange(childrenToAdd);
        }
	}
}