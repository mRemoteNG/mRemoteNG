using System.Collections.Generic;
using mRemoteNG.Connection;
using mRemoteNG.Container;


namespace mRemoteNG.Tree
{
    public class ConnectionTreeModel
    {
        public List<ContainerInfo> RootNodes { get; } = new List<ContainerInfo>();

        public void AddRootNode(ContainerInfo rootNode)
        {
            RootNodes.Add(rootNode);
        }

        public IEnumerable<ConnectionInfo> GetRecursiveChildList(ContainerInfo container)
        {
            return container.GetRecursiveChildList();
        }
    }
}