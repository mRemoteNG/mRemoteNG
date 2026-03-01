using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using NUnit.Framework;


namespace mRemoteNGTests.Tree
{
    public class ConnectionTreeModelTests
    {
        private ConnectionTreeModel _connectionTreeModel;

        [SetUp]
        public void Setup()
        {
            _connectionTreeModel = new ConnectionTreeModel();
        }

        [TearDown]
        public void Teardown()
        {
            _connectionTreeModel = null;
        }

        [Test]
        public void GetChildListProvidesAllChildren()
        {
            var root = new ContainerInfo();
            var folder1 = new ContainerInfo();
            var folder2 = new ContainerInfo();
            var con1 = new ConnectionInfo();
            root.AddChild(folder1);
            folder1.AddChild(folder2);
            root.AddChild(con1);
            _connectionTreeModel.AddRootNode(root);
            var connectionList = ConnectionTreeModel.GetRecursiveChildList(root);
            Assert.That(connectionList, Is.EquivalentTo(new[] {folder1,folder2,con1}));
        }

        [Test]
        public void FindConnectionByIdReturnsChildNode()
        {
            var root = new ContainerInfo();
            var child = new ConnectionInfo();
            root.AddChild(child);
            _connectionTreeModel.AddRootNode(root);

            ConnectionInfo? result = _connectionTreeModel.FindConnectionById(child.ConstantID);

            Assert.That(result, Is.SameAs(child));
        }

        [Test]
        public void ResolveLinkedConnectionReturnsSourceNode()
        {
            var root = new ContainerInfo();
            var source = new ConnectionInfo { Hostname = "source-host" };
            var link = source.Clone();
            link.LinkedConnectionId = source.ConstantID;
            root.AddChild(source);
            root.AddChild(link);
            _connectionTreeModel.AddRootNode(root);

            ConnectionInfo? result = _connectionTreeModel.ResolveLinkedConnection(link);

            Assert.That(result, Is.SameAs(source));
        }

        [Test]
        public void ResolveLinkedConnectionReturnsNullWhenLinksAreCircular()
        {
            var root = new ContainerInfo();
            var firstLink = new ConnectionInfo();
            var secondLink = new ConnectionInfo();
            firstLink.LinkedConnectionId = secondLink.ConstantID;
            secondLink.LinkedConnectionId = firstLink.ConstantID;
            root.AddChild(firstLink);
            root.AddChild(secondLink);
            _connectionTreeModel.AddRootNode(root);

            ConnectionInfo? result = _connectionTreeModel.ResolveLinkedConnection(firstLink);

            Assert.That(result, Is.Null);
        }
    }
}
