using System.Collections;
using mRemoteNG.Connection;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.UI.Controls
{
    public interface IConnectionTree
    {
        ConnectionTreeModel ConnectionTreeModel { get; set; }

        ConnectionInfo SelectedNode { get; }

        IEnumerable ExpandedObjects { get; set; }

        RootNodeInfo GetRootConnectionNode();

        void InvokeExpand(object model);

        void InvokeRebuildAll(bool preserveState);

        void ToggleExpansion(object model);
    }
}