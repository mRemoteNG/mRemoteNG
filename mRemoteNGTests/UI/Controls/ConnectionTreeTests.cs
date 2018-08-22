using System.Linq;
using System.Threading;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
    public class ConnectionTreeTests
	{
		private ConnectionTreeSearchTextFilter _filter;
		private ConnectionTree _connectionTree;

		[SetUp]
		public void Setup()
		{
			_filter = new ConnectionTreeSearchTextFilter();
			_connectionTree = new ConnectionTree
			{
				UseFiltering = true
			};
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void FilteringIsRetainedAndUpdatedWhenNodeDeleted()
		{
			// root
			// |- folder1
			// |	|- con1
			// |	|- dontshowme
			// |- folder2
			//		|- con2
			var connectionTreeModel = new ConnectionTreeModel();
			var root = new RootNodeInfo(RootNodeType.Connection);
			var folder1 = new ContainerInfo {Name = "folder1"};
			var folder2 = new ContainerInfo {Name = "folder2"};
			var con1 = new ConnectionInfo {Name = "con1"};
			var con2 = new ConnectionInfo {Name = "con2"};
			var conDontShow = new ConnectionInfo {Name = "dontshowme" };
			root.AddChildRange(new []{folder1, folder2});
			folder1.AddChildRange(new []{con1, conDontShow});
			folder2.AddChild(con2);
			connectionTreeModel.AddRootNode(root);

			_connectionTree.ConnectionTreeModel = connectionTreeModel;
			// ensure all folders expanded
			_connectionTree.ExpandAll();

			// apply filtering on the tree
			_filter.FilterText = "con";
			_connectionTree.ModelFilter = _filter;

			connectionTreeModel.DeleteNode(con1);

			Assert.That(_connectionTree.IsFiltering, Is.True);
			Assert.That(_connectionTree.FilteredObjects, Does.Not.Contain(con1));
			Assert.That(_connectionTree.FilteredObjects, Does.Not.Contain(conDontShow));
			Assert.That(_connectionTree.FilteredObjects, Does.Contain(con2));
		}

	    [Test]
	    [Apartment(ApartmentState.STA)]
	    public void CannotAddConnectionToPuttySessionNode()
	    {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var root = new RootNodeInfo(RootNodeType.Connection);
            var puttyRoot = new RootNodeInfo(RootNodeType.PuttySessions);
	        connectionTreeModel.AddRootNode(root);
	        connectionTreeModel.AddRootNode(puttyRoot);

	        _connectionTree.ConnectionTreeModel = connectionTreeModel;

	        _connectionTree.SelectedObject = puttyRoot;
	        _connectionTree.AddConnection();

	        Assert.That(puttyRoot.Children, Is.Empty);
	    }

	    [Test]
	    [Apartment(ApartmentState.STA)]
	    public void CannotAddFolderToPuttySessionNode()
	    {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var root = new RootNodeInfo(RootNodeType.Connection);
	        var puttyRoot = new RootNodeInfo(RootNodeType.PuttySessions);
	        connectionTreeModel.AddRootNode(root);
	        connectionTreeModel.AddRootNode(puttyRoot);

	        _connectionTree.ConnectionTreeModel = connectionTreeModel;

	        _connectionTree.SelectedObject = puttyRoot;
	        _connectionTree.AddFolder();

	        Assert.That(puttyRoot.Children, Is.Empty);
	    }

	    [Test]
	    [Apartment(ApartmentState.STA)]
	    public void CannotDuplicateRootConnectionNode()
	    {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var root = new RootNodeInfo(RootNodeType.Connection);
	        connectionTreeModel.AddRootNode(root);
	        _connectionTree.ConnectionTreeModel = connectionTreeModel;

	        _connectionTree.SelectedObject = root;
            _connectionTree.DuplicateSelectedNode();

	        Assert.That(connectionTreeModel.RootNodes, Has.One.Items);
	    }

	    [Test]
	    [Apartment(ApartmentState.STA)]
	    public void CannotDuplicateRootPuttyNode()
	    {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var puttyRoot = new RootNodeInfo(RootNodeType.PuttySessions);
	        connectionTreeModel.AddRootNode(puttyRoot);
	        _connectionTree.ConnectionTreeModel = connectionTreeModel;

	        _connectionTree.SelectedObject = puttyRoot;
	        _connectionTree.DuplicateSelectedNode();

	        Assert.That(connectionTreeModel.RootNodes, Has.One.Items);
	    }

	    [Test]
	    [Apartment(ApartmentState.STA)]
	    public void CannotDuplicatePuttyConnectionNode()
	    {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var puttyRoot = new RootNodeInfo(RootNodeType.PuttySessions);
            var puttyConnection = new PuttySessionInfo();
            puttyRoot.AddChild(puttyConnection);
	        connectionTreeModel.AddRootNode(puttyRoot);
	        _connectionTree.ConnectionTreeModel = connectionTreeModel;
	        _connectionTree.ExpandAll();

            _connectionTree.SelectedObject = puttyConnection;
	        _connectionTree.DuplicateSelectedNode();

	        Assert.That(puttyRoot.Children, Has.One.Items);
	    }

	    [Test]
	    [Apartment(ApartmentState.STA)]
	    public void DuplicatingWithNoNodeSelectedDoesNothing()
	    {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var puttyRoot = new RootNodeInfo(RootNodeType.PuttySessions);
	        connectionTreeModel.AddRootNode(puttyRoot);
	        _connectionTree.ConnectionTreeModel = connectionTreeModel;

            _connectionTree.SelectedObject = null;
	        _connectionTree.DuplicateSelectedNode();

	        Assert.That(connectionTreeModel.RootNodes, Has.One.Items);
	    }

	    [Test]
	    [Apartment(ApartmentState.STA)]
	    public void ExpandingAllItemsUpdatesColumnWidthAppropriately()
	    {
            var connectionTreeModel = new ConnectionTreeModel();
	        var root = new RootNodeInfo(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);
	        ContainerInfo parent = root;
	        foreach (var i in Enumerable.Repeat("", 8))
	        {
                var newContainer = new ContainerInfo {IsExpanded = false};
                parent.AddChild(newContainer);
	            parent = newContainer;
	        }

	        _connectionTree.ConnectionTreeModel = connectionTreeModel;

	        var widthBefore = _connectionTree.Columns[0].Width;
	        _connectionTree.ExpandAll();
            var widthAfter = _connectionTree.Columns[0].Width;

            Assert.That(widthAfter, Is.GreaterThan(widthBefore));
	    }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void RenamingNodeWithNothingSelectedDoesNothing()
	    {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var root = new RootNodeInfo(RootNodeType.Connection);
	        connectionTreeModel.AddRootNode(root);

	        _connectionTree.ConnectionTreeModel = connectionTreeModel;
	        _connectionTree.SelectedObject = null;

	        Assert.DoesNotThrow(() => _connectionTree.RenameSelectedNode());
        }
    }
}
