using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.UI
{
    public class ConnectionTreeViewBuilder
    {
        private ConnectionTreeModel _connectionTreeModel;

        public TreeView TreeView { get; private set; }
        public ConnectionList ConnectionList { get; set; }
        public ContainerList ContainerList { get; set; }

        public ConnectionTreeViewBuilder(ConnectionTreeModel connectionTreeModel)
        {
            _connectionTreeModel = connectionTreeModel;
            TreeView = new TreeView();
            ConnectionList = new ConnectionList();
            ContainerList = new ContainerList();
        }

        public void Build()
        {
            var rootNodeInfo = _connectionTreeModel.RootNodes.First(a => a is RootNodeInfo);
            BuildTreeViewFromConnectionTree(rootNodeInfo, InitRootNode((RootNodeInfo)rootNodeInfo));
        }

        private TreeNode InitRootNode(RootNodeInfo rootNodeInfo)
        {
            var rootNode = new TreeNode(rootNodeInfo.Name);
            TreeView.Nodes.Add(rootNode);
            return rootNode;
        }

        private void BuildTreeViewFromConnectionTree(ContainerInfo parentContainer, TreeNode parentTreeNode)
        {
            if (parentContainer.Children.Any())
            {
                foreach (var child in parentContainer.Children)
                {
                    var treeNode = new TreeNode(child.Name);
                    parentTreeNode.Nodes.Add(treeNode);

                    if (child is ContainerInfo)
                    {
                        AddContainerToList((ContainerInfo) child, treeNode);
                        BuildTreeViewFromConnectionTree((ContainerInfo)child, treeNode);
                    }
                    else
                    {
                        AddConnectionToList(child, treeNode);
                    }
                }
            }
            else
            {

                //parentTreeNode.Text = parentContainer.Name;
            }
        }

        private void AddConnectionToList(ConnectionInfo connectionInfo, TreeNode treeNode)
        {
            connectionInfo.TreeNode = treeNode;
            ConnectionList.Add(connectionInfo);
            treeNode.Tag = connectionInfo;
            treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
            treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
        }

        private void AddContainerToList(ContainerInfo containerInfo, TreeNode treeNode)
        {
            containerInfo.TreeNode = treeNode;
            ContainerList.Add(containerInfo);
            treeNode.Tag = containerInfo;
            treeNode.ImageIndex = (int)TreeImageType.Container;
            treeNode.SelectedImageIndex = (int)TreeImageType.Container;
        }
    }
}