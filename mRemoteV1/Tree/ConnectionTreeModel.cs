using System.Collections.Generic;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Tree
{
    public class ConnectionTreeModel
    {
        public List<ContainerInfo> RootNodes { get; } = new List<ContainerInfo>();

        public void AddRootNode(ContainerInfo rootNode)
        {
            RootNodes.Add(rootNode);
        }

        public IEnumerable<ConnectionInfo> GetRecursiveChildList()
        {
            var list = new List<ConnectionInfo>();
            foreach (var rootNode in RootNodes)
            {
                list.AddRange(GetRecursiveChildList(rootNode));
            }
            return list;
        }

        public IEnumerable<ConnectionInfo> GetRecursiveChildList(ContainerInfo container)
        {
            return container.GetRecursiveChildList();
        }

        public void RenameNode(ConnectionInfo connectionInfo, string newName)
        {
            if (newName == null || newName.Length <= 0)
                return;

            connectionInfo.Name = newName;
            if (Settings.Default.SetHostnameLikeDisplayName)
                connectionInfo.Hostname = newName;
        }

        public void DeleteNode(ConnectionInfo connectionInfo)
        {
            if (connectionInfo is RootNodeInfo)
                return;
            
            connectionInfo?.Dispose();
        }

        public void CloneNode(ConnectionInfo connectionInfo)
        {
            connectionInfo.Clone();
        }
    }
}