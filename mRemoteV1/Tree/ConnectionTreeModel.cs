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

        public IEnumerable<ConnectionInfo> GetChildList(ContainerInfo container)
        {
            var childList = new List<ConnectionInfo>();
            foreach (var child in container.Children)
            {
                childList.Add(child);
                var childContainer = child as ContainerInfo;
                if (childContainer != null)
                    childList.AddRange(GetChildList(childContainer));
            }
            return childList;
        }
    }
}