using mRemoteNG.App;
using System.Windows.Forms;


namespace mRemoteNG.Tree
{
    public static class ConnectionTree
    {
        private static TreeNode treeNodeToBeSelected;

        public static TreeView TreeView { get; set; }

        //TODO Fix for TreeListView
        private delegate void ResetTreeDelegate();
        public static void ResetTree()
        {
            if (TreeView.InvokeRequired)
            {
                ResetTreeDelegate resetTreeDelegate = ResetTree;
                Windows.treeForm.Invoke(resetTreeDelegate);
            }
            else
            {
                TreeView.BeginUpdate();
                TreeView.Nodes.Clear();
                TreeView.Nodes.Add(Language.strConnections);
                TreeView.EndUpdate();
            }
        }
    }
}