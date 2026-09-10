using System.Collections.Generic;
using System.Linq;
using System.Threading;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tools.Clipboard;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Controls.ConnectionTree;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.UI.Controls
{
	public class ConnectionTreeTests
	{
		private ConnectionTreeSearchTextFilter _filter;
		private mRemoteNG.UI.Controls.ConnectionTree.ConnectionTree _connectionTree;

		[SetUp]
		public void Setup()
		{
			_filter = new ConnectionTreeSearchTextFilter();
			_connectionTree = new mRemoteNG.UI.Controls.ConnectionTree.ConnectionTree
			{
				UseFiltering = true
			};
			mRemoteNG.Properties.OptionsAppearancePage.Default.EnableConnectionTreeAnimations = true;
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
	        _connectionTree.ExpandAll();

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
	        _connectionTree.ExpandAll();

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
	        _connectionTree.ExpandAll();

			_connectionTree.SelectedObject = root;
            _connectionTree.DuplicateSelectedNode();

	        Assert.That(connectionTreeModel.RootNodes, Has.One.Items);
	    }

	    [Test]
	    [Apartment(ApartmentState.STA)]
	    public void CanDuplicateConnectionNode()
	    {
		    var connectionTreeModel = new ConnectionTreeModel();
		    var root = new RootNodeInfo(RootNodeType.Connection);
			var con1 = new ConnectionInfo();
			root.AddChild(con1);
		    connectionTreeModel.AddRootNode(root);
		    _connectionTree.ConnectionTreeModel = connectionTreeModel;
		    _connectionTree.ExpandAll();

			_connectionTree.SelectedObject = con1;
		    _connectionTree.DuplicateSelectedNode();

		    Assert.That(root.Children, Has.Exactly(2).Items);
	    }

		[Test]
	    [Apartment(ApartmentState.STA)]
	    public void CannotDuplicateRootPuttyNode()
	    {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var puttyRoot = new RootNodeInfo(RootNodeType.PuttySessions);
	        connectionTreeModel.AddRootNode(puttyRoot);
	        _connectionTree.ConnectionTreeModel = connectionTreeModel;
	        _connectionTree.ExpandAll();

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
	        _connectionTree.ExpandAll();

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
	        _connectionTree.ExpandAll();
			_connectionTree.SelectedObject = null;

	        Assert.DoesNotThrow(() => _connectionTree.RenameSelectedNode());
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void CopyHostnameCopiesTheHostnameOfTheSelectedConnection()
        {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var root = new RootNodeInfo(RootNodeType.Connection);
			var con1 = new ConnectionInfo {Hostname = "MyHost"};
			root.AddChild(con1);
			connectionTreeModel.AddRootNode(root);

	        _connectionTree.ConnectionTreeModel = connectionTreeModel;
			_connectionTree.ExpandAll();
	        _connectionTree.SelectedObject = con1;

	        var clipboard = Substitute.For<IClipboard>();
			_connectionTree.CopyHostnameSelectedNode(clipboard);
			clipboard.Received(1).SetText(con1.Hostname);
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void CopyHostnameCopiesTheNodeNameOfTheSelectedContainer()
        {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var root = new RootNodeInfo(RootNodeType.Connection);
	        var container = new ContainerInfo { Name = "MyFolder" };
	        root.AddChild(container);
	        connectionTreeModel.AddRootNode(root);

	        _connectionTree.ConnectionTreeModel = connectionTreeModel;
	        _connectionTree.ExpandAll();
			_connectionTree.SelectedObject = container;

	        var clipboard = Substitute.For<IClipboard>();
			_connectionTree.CopyHostnameSelectedNode(clipboard);
			clipboard.Received(1).SetText(container.Name);
		}

        [Test]
        [Apartment(ApartmentState.STA)]
        public void CopyHostnameDoesNotCopyAnythingIfNoNodeSelected()
        {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var root = new RootNodeInfo(RootNodeType.Connection);
	        var con1 = new ConnectionInfo { Hostname = "MyHost" };
	        root.AddChild(con1);
	        connectionTreeModel.AddRootNode(root);

	        _connectionTree.ConnectionTreeModel = connectionTreeModel;
	        _connectionTree.ExpandAll();
			_connectionTree.SelectedObject = null;

			var clipboard = Substitute.For<IClipboard>();
			_connectionTree.CopyHostnameSelectedNode(clipboard);
			clipboard.DidNotReceiveWithAnyArgs().SetText("");
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void CopyHostnameDoesNotCopyAnythingIfHostnameOfSelectedConnectionIsEmpty()
        {
	        var connectionTreeModel = new ConnectionTreeModel();
	        var root = new RootNodeInfo(RootNodeType.Connection);
	        var con1 = new ConnectionInfo { Hostname = string.Empty };
	        root.AddChild(con1);
	        connectionTreeModel.AddRootNode(root);

	        _connectionTree.ConnectionTreeModel = connectionTreeModel;
	        _connectionTree.ExpandAll();
			_connectionTree.SelectedObject = con1;

	        var clipboard = Substitute.For<IClipboard>();
			_connectionTree.CopyHostnameSelectedNode(clipboard);
			clipboard.DidNotReceiveWithAnyArgs().SetText("");
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void CopyHostnameDoesNotCopyAnythingIfNameOfSelectedContainerIsEmpty()
		{
			var connectionTreeModel = new ConnectionTreeModel();
			var root = new RootNodeInfo(RootNodeType.Connection);
			var con1 = new ContainerInfo { Name = string.Empty};
			root.AddChild(con1);
			connectionTreeModel.AddRootNode(root);

			_connectionTree.ConnectionTreeModel = connectionTreeModel;
			_connectionTree.ExpandAll();
			_connectionTree.SelectedObject = con1;

			var clipboard = Substitute.For<IClipboard>();
			_connectionTree.CopyHostnameSelectedNode(clipboard);
			clipboard.DidNotReceiveWithAnyArgs().SetText("");
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void ExpandingLargeContainerCompletesAndShowsAllChildren()
		{
			var (root, parent, children) = CreateTreeWithChildren(150);

			Assert.That(_connectionTree.IsExpanded(parent), Is.False);

			_connectionTree.Expand(parent);
			WaitUntil(() => _connectionTree.IsExpanded(parent) && _connectionTree.GetChildren(parent).Count == children.Count);

			Assert.That(_connectionTree.GetChildren(parent), Has.Count.EqualTo(children.Count));
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void CollapseRequestDuringExpandCompletesCollapseWithoutStaleState()
		{
			var (root, parent, children) = CreateTreeWithChildren(120);

			_connectionTree.Expand(parent);
			WaitUntil(() => _connectionTree.GetChildren(parent).Count > 0);

			_connectionTree.Collapse(parent);
			WaitUntil(() => !_connectionTree.IsExpanded(parent));

			Assert.That(_connectionTree.IsExpanded(parent), Is.False);
			Assert.That(_connectionTree.GetChildren(parent), Is.Empty);

			_connectionTree.Expand(parent);
			WaitUntil(() => _connectionTree.IsExpanded(parent) && _connectionTree.GetChildren(parent).Count == children.Count);
			Assert.That(_connectionTree.GetChildren(parent), Has.Count.EqualTo(children.Count));
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void DisabledAnimationsExpandAndCollapseImmediately()
		{
			mRemoteNG.Properties.OptionsAppearancePage.Default.EnableConnectionTreeAnimations = false;
			var (root, parent, children) = CreateTreeWithChildren(40);

			_connectionTree.Expand(parent);

			Assert.That(_connectionTree.IsExpanded(parent), Is.True);
			Assert.That(_connectionTree.GetChildren(parent), Has.Count.EqualTo(children.Count));

			_connectionTree.Collapse(parent);

			Assert.That(_connectionTree.IsExpanded(parent), Is.False);
			Assert.That(_connectionTree.GetChildren(parent), Is.Empty);
		}

		[Test]
		[Apartment(ApartmentState.STA)]
		public void ProgrammaticExpandWithoutAnimationPreservesNewNodeSelectionAndVisibility()
		{
			var connectionTreeModel = new ConnectionTreeModel();
			var root = new RootNodeInfo(RootNodeType.Connection);
			var parent = new ContainerInfo { Name = "parent" };
			root.AddChild(parent);
			connectionTreeModel.AddRootNode(root);

			_connectionTree.ConnectionTreeModel = connectionTreeModel;
			_connectionTree.ExpandAll();
			_connectionTree.SelectedObject = parent;

			_connectionTree.AddConnection();

			Assert.That(parent.Children, Has.Count.EqualTo(1));
			Assert.That(_connectionTree.SelectedNode, Is.EqualTo(parent.Children[0]));
			Assert.That(_connectionTree.GetChildren(parent), Has.Count.EqualTo(1));
		}

		private (RootNodeInfo root, ContainerInfo parent, List<ConnectionInfo> children) CreateTreeWithChildren(int childCount)
		{
			var connectionTreeModel = new ConnectionTreeModel();
			var root = new RootNodeInfo(RootNodeType.Connection);
			var parent = new ContainerInfo { Name = "parent" };
			root.AddChild(parent);

			var children = new List<ConnectionInfo>();
			for (int i = 0; i < childCount; i++)
			{
				var child = new ConnectionInfo { Name = $"child-{i}" };
				parent.AddChild(child);
				children.Add(child);
			}

			connectionTreeModel.AddRootNode(root);
			_connectionTree.ConnectionTreeModel = connectionTreeModel;

			return (root, parent, children);
		}

		private static void WaitUntil(System.Func<bool> condition, int timeoutMs = 2000)
		{
			int elapsed = 0;
			while (!condition() && elapsed < timeoutMs)
			{
				Application.DoEvents();
				Thread.Sleep(10);
				elapsed += 10;
			}

			Assert.That(condition(), Is.True, "Timed out waiting for expected UI state.");
		}
	}
}
