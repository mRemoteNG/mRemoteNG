using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;
using NUnit.Framework;


namespace mRemoteNGTests.UI
{
    public class ConnectionTreeViewBuilderTests
    {
        private ConnectionTreeModel _connectionTreeModel;
        private ConnectionTreeViewBuilder _connectionTreeViewBuilder;

        [SetUp]
        public void Setup()
        {
            _connectionTreeModel = new ConnectionTreeModel();
            _connectionTreeViewBuilder = new ConnectionTreeViewBuilder(_connectionTreeModel);
        }

        [TearDown]
        public void Teardown()
        {
            _connectionTreeModel = null;
            _connectionTreeViewBuilder = null;
        }

        [Test]
        public void RootNodeGetsAddedToTreeView()
        {
            CreateRootNode();
            _connectionTreeViewBuilder.Build();
            var rootTreeNode = _connectionTreeViewBuilder.RootNode;
            Assert.That(rootTreeNode, Is.Not.Null);
        }

        [Test]
        public void BuildTreeWithSingleConnection()
        {
            /**
             * Root
             * -- con1
             */
            var root = CreateRootNode();
            var con1 = new ConnectionInfo();
            root.AddChild(con1);
            _connectionTreeViewBuilder.Build();
            var rootTreeNode = _connectionTreeViewBuilder.RootNode;
            Assert.That(rootTreeNode.Nodes[0].Tag, Is.EqualTo(con1));
        }

        [Test]
        public void BuildTreeWithSingleFolder()
        {
            /**
             * Root
             * -- folder1
             */
            var root = CreateRootNode();
            var folder1 = new ContainerInfo();
            root.AddChild(folder1);
            _connectionTreeViewBuilder.Build();
            var rootTreeNode = _connectionTreeViewBuilder.RootNode;
            Assert.That(rootTreeNode.Nodes[0].Tag, Is.EqualTo(folder1));
        }

        [Test]
        public void BuildTreeWithTwoConnectionsUnderRoot()
        {
            /**
             * Root
             * -- con1
             * -- con2
             */
            var root = CreateRootNode();
            var con1 = new ConnectionInfo();
            var con2 = new ConnectionInfo();
            root.AddChildRange(new []{con1, con2});
            _connectionTreeViewBuilder.Build();
            var rootTreeNode = _connectionTreeViewBuilder.RootNode;
            Assert.That(GetTreeNodeTags(rootTreeNode.Nodes), Is.EquivalentTo(new[] { con1, con2 }));
        }

        [Test]
        public void BuildTreeWithConnectionInFolder()
        {
            /*
             * Root
             * -- folder1
             * ---- con1
             */
            var root = CreateRootNode();
            var folder1 = new ContainerInfo();
            var con1 = new ConnectionInfo();
            root.AddChild(folder1);
            folder1.AddChild(con1);
            _connectionTreeViewBuilder.Build();
            var rootTreeNode = _connectionTreeViewBuilder.RootNode;
            Assert.That(rootTreeNode.Nodes[0].Tag, Is.EqualTo(folder1));
            Assert.That(rootTreeNode.Nodes[0].Nodes[0].Tag, Is.EqualTo(con1));
        }

        [Test]
        public void BuildTreeWithTwoFoldersAndFolderOneHasAConnection()
        {
            /**
             * Root
             * -- folder1
             * ---- con1
             * -- folder2
             */
            var root = CreateRootNode();
            var folder1 = new ContainerInfo();
            var con1 = new ConnectionInfo();
            var folder2 = new ContainerInfo();
            root.AddChild(folder1);
            folder1.AddChild(con1);
            root.AddChild(folder2);
            _connectionTreeViewBuilder.Build();
            var rootTreeNode = _connectionTreeViewBuilder.RootNode;
            Assert.That(rootTreeNode.Nodes[0].Tag, Is.EqualTo(folder1));
            Assert.That(rootTreeNode.Nodes[0].Nodes[0].Tag, Is.EqualTo(con1));
            Assert.That(rootTreeNode.Nodes[1].Tag, Is.EqualTo(folder2));
        }

        [Test]
        public void BuildTreeWithConnectionNestedTwoLevelsDeep()
        {
            /**
             * Root
             * -- folder1
             * ---- folder2
             * ------ con1
             */
            var root = CreateRootNode();
            var folder1 = new ContainerInfo();
            var folder2 = new ContainerInfo();
            var con1 = new ConnectionInfo();
            root.AddChild(folder1);
            folder1.AddChild(folder2);
            folder2.AddChild(con1);
            _connectionTreeViewBuilder.Build();
            var rootTreeNode = _connectionTreeViewBuilder.RootNode;
            Assert.That(rootTreeNode.Nodes[0].Nodes[0].Nodes[0].Tag, Is.EqualTo(con1));
        }

        [Test]
        public void BuildTreeWithMultipleDeepRecursion()
        {
            /**
             * Root
             * -- folder1
             * ---- folder2
             * ------ con1
             * -- folder3
             * ---- folder4
             * ------ con2
             */
            var root = CreateRootNode();
            var folder1 = new ContainerInfo();
            var folder2 = new ContainerInfo();
            var con1 = new ConnectionInfo();
            var folder3 = new ContainerInfo();
            var folder4 = new ContainerInfo();
            var con2 = new ConnectionInfo();
            root.AddChild(folder1);
            folder1.AddChild(folder2);
            folder2.AddChild(con1);
            root.AddChild(folder3);
            folder3.AddChild(folder4);
            folder4.AddChild(con2);
            _connectionTreeViewBuilder.Build();
            var rootTreeNode = _connectionTreeViewBuilder.RootNode;
            Assert.That(rootTreeNode.Nodes[0].Nodes[0].Nodes[0].Tag, Is.EqualTo(con1));
            Assert.That(rootTreeNode.Nodes[1].Nodes[0].Nodes[0].Tag, Is.EqualTo(con2));
        }

        private IEnumerable<ConnectionInfo> GetTreeNodeTags(TreeNodeCollection treeNodes)
        {
            return (from TreeNode node in treeNodes select (ConnectionInfo) node.Tag).ToList();
        }

        private RootNodeInfo CreateRootNode()
        {
             var root = new RootNodeInfo(RootNodeType.Connection);
            _connectionTreeModel.AddRootNode(root);
            return root;
        }
    }
}