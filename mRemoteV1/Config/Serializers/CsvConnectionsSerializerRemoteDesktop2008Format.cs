using System;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers
{
    public class CsvConnectionsSerializerRemoteDesktop2008Format : ISerializer<string>
    {
        private string _csv = "";

        public string Serialize(ConnectionTreeModel connectionTreeModel)
        {
            var rootNode = (RootNodeInfo)connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
            return SerializeToCsv(rootNode);
        }

        private string SerializeToCsv(RootNodeInfo rootNodeInfo)
        {
            if (Runtime.IsConnectionsFileLoaded == false)
                return "";

            _csv = "";
            SerializeNodesRecursive(rootNodeInfo);
            return _csv;
        }

        private void SerializeNodesRecursive(ContainerInfo containerInfo)
        {
            foreach (var child in containerInfo.Children)
            {
                if (child is ContainerInfo)
                    SerializeNodesRecursive((ContainerInfo)child);
                else if (child.Protocol == ProtocolType.RDP)
                    SerializeConnectionInfo(child);
            }
        }

        private void SerializeConnectionInfo(ConnectionInfo con)
        {
            var nodePath = con.TreeNode.FullPath;

            var firstSlash = nodePath.IndexOf("\\", StringComparison.Ordinal);
            nodePath = nodePath.Remove(0, firstSlash + 1);
            var lastSlash = nodePath.LastIndexOf("\\", StringComparison.Ordinal);

            nodePath = lastSlash > 0 ? nodePath.Remove(lastSlash) : "";

            _csv += con.Name + ";" + con.Hostname + ";" + con.MacAddress + ";;" + con.Port + ";" + con.UseConsoleSession + ";" + nodePath + Environment.NewLine;
        }
    }
}