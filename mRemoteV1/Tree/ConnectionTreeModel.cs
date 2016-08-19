using System.Collections.Generic;
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
    }
}