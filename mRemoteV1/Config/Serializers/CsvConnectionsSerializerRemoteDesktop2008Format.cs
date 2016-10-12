using System;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers
{
    public class CsvConnectionsSerializerRemoteDesktop2008Format : ISerializer<string>
    {
        private string _csv = "";
        public SaveFilter SaveFilter { get; set; }

        public string Serialize(ConnectionTreeModel connectionTreeModel)
        {
            var rootNode = connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
            return Serialize(rootNode);
        }

        public string Serialize(ConnectionInfo serializationTarget)
        {
            _csv = "";
            SerializeNodesRecursive(serializationTarget);
            return _csv;
        }

        private void SerializeNodesRecursive(ConnectionInfo node)
        {
            var nodeAsContainer = node as ContainerInfo;
            if (nodeAsContainer != null)
            {
                foreach (var child in nodeAsContainer.Children)
                {
                    if (child is ContainerInfo)
                        SerializeNodesRecursive((ContainerInfo)child);
                    else if (child.Protocol == ProtocolType.RDP)
                        SerializeConnectionInfo(child);
                }
            }
            else if (node.Protocol == ProtocolType.RDP)
                SerializeConnectionInfo(node);
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