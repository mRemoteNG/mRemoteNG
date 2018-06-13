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
	}
}
