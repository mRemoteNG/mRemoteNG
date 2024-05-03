using mRemoteNG.UI.Controls.ConnectionTree;


namespace mRemoteNG.Tree
{
    public class RootNodeExpander : IConnectionTreeDelegate
    {
        public void Execute(IConnectionTree connectionTree)
        {
            Root.RootNodeInfo rootConnectionNode = connectionTree.GetRootConnectionNode();
            connectionTree.InvokeExpand(rootConnectionNode);
        }
    }
}