using System.Collections.Generic;
using System.Linq;
using BrightIdeasSoftware;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.UI.Controls
{
    public partial class ConnectionTree : TreeListView
    {
        public ConnectionInfo SelectedNode => (ConnectionInfo)SelectedObject;


        public ConnectionTree()
        {
            InitializeComponent();
        }

        public RootNodeInfo GetRootConnectionNode()
        {
            return (RootNodeInfo)Roots.Cast<ConnectionInfo>().First(item => item is RootNodeInfo);
        }

        public IEnumerable<RootPuttySessionsNodeInfo> GetRootPuttyNodes()
        {
            return Objects.OfType<RootPuttySessionsNodeInfo>();
        }

        public void DuplicateSelectedNode()
        {
            var newNode = SelectedNode.Clone();
            newNode.Parent.SetChildBelow(newNode, SelectedNode);
            Runtime.SaveConnectionsAsync();
        }

        public void RenameSelectedNode()
        {
            SelectedItem.BeginEdit();
            Runtime.SaveConnectionsAsync();
        }
    }
}