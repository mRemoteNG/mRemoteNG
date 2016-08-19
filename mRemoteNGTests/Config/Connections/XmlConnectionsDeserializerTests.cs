using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNGTests.Properties;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Connections
{
    public class XmlConnectionsDeserializerTests
    {
        private XmlConnectionsDeserializer _xmlConnectionsDeserializer;
        private ConnectionTreeModel _connectionTreeModel;

        [SetUp]
        public void Setup()
        {
            _xmlConnectionsDeserializer = new XmlConnectionsDeserializer(Resources.TestConfCons);
            _connectionTreeModel = _xmlConnectionsDeserializer.Deserialize();
        }

        [TearDown]
        public void Teardown()
        {
            _xmlConnectionsDeserializer = null;
            _connectionTreeModel = null;
        }

        [Test]
        public void DeserializingCreatesRootNode()
        {
            Assert.That(_connectionTreeModel.RootNodes, Is.Not.Empty);
        }

        [Test]
        public void RootNodeHasThreeChildren()
        {
            var connectionRoot = _connectionTreeModel.RootNodes[0];
            Assert.That(connectionRoot.Children.Count, Is.EqualTo(3));
        }

        [Test]
        public void RootContainsFolder1()
        {
            var connectionRoot = _connectionTreeModel.RootNodes[0];
            Assert.That(ContainsNodeNamed("Folder1", connectionRoot.Children), Is.True);
        }

        [Test]
        public void Folder1ContainsThreeConnections()
        {
            var connectionRoot = _connectionTreeModel.RootNodes[0];
            var folder1 = GetFolderNamed("Folder1", connectionRoot.Children);
            var folder1ConnectionCount = folder1?.Children.Count(node => !(node is ContainerInfo));
            Assert.That(folder1ConnectionCount, Is.EqualTo(3));
        }

        [Test]
        public void Folder2ContainsThreeNodes()
        {
            var connectionRoot = _connectionTreeModel.RootNodes[0];
            var folder2 = GetFolderNamed("Folder2", connectionRoot.Children);
            var folder1Count = folder2?.Children.Count();
            Assert.That(folder1Count, Is.EqualTo(3));
        }

        [Test]
        public void Folder21HasTwoNodes()
        {
            var connectionRoot = _connectionTreeModel.RootNodes[0];
            var folder2 = GetFolderNamed("Folder2", connectionRoot.Children);
            var folder21 = GetFolderNamed("Folder2.1", folder2.Children);
            Assert.That(folder21.Children.Count, Is.EqualTo(2));
        }

        [Test]
        public void Folder211HasOneConnection()
        {
            var connectionRoot = _connectionTreeModel.RootNodes[0];
            var folder2 = GetFolderNamed("Folder2", connectionRoot.Children);
            var folder21 = GetFolderNamed("Folder2.1", folder2.Children);
            var folder211 = GetFolderNamed("Folder2.1.1", folder21.Children);
            var connectionCount = folder211.Children.Count(node => !(node is ContainerInfo));
            Assert.That(connectionCount, Is.EqualTo(1));
        }

        private bool ContainsNodeNamed(string name, IEnumerable<ConnectionInfo> list)
        {
            return list.Any(node => node.Name == name);
        }

        private ContainerInfo GetFolderNamed(string name, IEnumerable<ConnectionInfo> list)
        {
            var folder = list.First(node => (node is ContainerInfo && node.Name == name)) as ContainerInfo;
            return folder;
        }
    }
}