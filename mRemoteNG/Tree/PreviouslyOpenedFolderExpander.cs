using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Container;
using mRemoteNG.UI.Controls.ConnectionTree;


namespace mRemoteNG.Tree
{
    [SupportedOSPlatform("windows")]
    public class PreviouslyOpenedFolderExpander : IConnectionTreeDelegate
    {
        public void Execute(IConnectionTree connectionTree)
        {
            Root.RootNodeInfo rootNode = connectionTree.GetRootConnectionNode();
            System.Collections.Generic.IEnumerable<ContainerInfo> containerList = connectionTree.ConnectionTreeModel.GetRecursiveChildList(rootNode)
                                              .OfType<ContainerInfo>();
            System.Collections.Generic.IEnumerable<ContainerInfo> previouslyExpandedNodes = containerList.Where(container => container.IsExpanded);
            connectionTree.ExpandedObjects = previouslyExpandedNodes;
            connectionTree.InvokeRebuildAll(true);
        }
    }
}