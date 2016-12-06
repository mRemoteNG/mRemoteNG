using System.Threading;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Window;
using NUnit.Framework;
using WeifenLuo.WinFormsUI.Docking;


namespace mRemoteNGTests.UI.Window
{
    public class ConnectionTreeWindowTests
    {
        private ConnectionTreeWindow _connectionTreeWindow;

        [SetUp]
        public void Setup()
        {
            _connectionTreeWindow = new ConnectionTreeWindow(new DockContent());
        }

        [TearDown]
        public void Teardown()
        {
            _connectionTreeWindow.Close();
        }

        [Test, Apartment(ApartmentState.STA)]
        public void CanSetTheConnectionTreeModel()
        {
            var treeModel = CreateConnectionTreeModel();
            _connectionTreeWindow.ConnectionTreeModel = treeModel;
            _connectionTreeWindow.Show();
            Assert.That(_connectionTreeWindow.ConnectionTreeModel, Is.EqualTo(treeModel));
        }

        [Test, Apartment(ApartmentState.STA)]
        public void CanDeleteLastFolderInTheTree()
        {
            var treeModel = CreateConnectionTreeModel();
            treeModel.RootNodes[0].AddChild(new ContainerInfo());
            _connectionTreeWindow.ConnectionTreeModel = treeModel;
            _connectionTreeWindow.Show();
            Assert.That(_connectionTreeWindow.ConnectionTreeModel, Is.EqualTo(treeModel));
        }

        private ConnectionTreeModel CreateConnectionTreeModel()
        {
            var connectionTreeModel = new ConnectionTreeModel();
            connectionTreeModel.AddRootNode(new RootNodeInfo(RootNodeType.Connection));
            return connectionTreeModel;
        }
    }
}