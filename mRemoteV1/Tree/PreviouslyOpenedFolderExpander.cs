using System.Linq;
using mRemoteNG.Container;
using mRemoteNG.UI.Controls;


namespace mRemoteNG.Tree
{
    public class PreviouslyOpenedFolderExpander : IConnectionTreeDelegate
    {
        public void Execute(IConnectionTree connectionTree)
        {
            var containerList = connectionTree.ConnectionTreeModel.GetRecursiveChildList(connectionTree.GetRootConnectionNode()).OfType<ContainerInfo>();
            var previouslyExpandedNodes = containerList.Where(container => container.IsExpanded);
            connectionTree.ExpandedObjects = previouslyExpandedNodes;
            connectionTree.InvokeRebuildAll(true);
        }
    }
}