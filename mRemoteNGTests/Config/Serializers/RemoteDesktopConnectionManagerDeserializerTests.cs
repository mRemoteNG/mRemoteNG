using System.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNGTests.Properties;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class RemoteDesktopConnectionManagerDeserializerTests
    {
        private string _connectionFileContents;
        private RemoteDesktopConnectionManagerDeserializer _deserializer;
        private ConnectionTreeModel _connectionTreeModel;


        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _connectionFileContents = Resources.test_rdcman_v2_2_schema1;
            _deserializer = new RemoteDesktopConnectionManagerDeserializer(_connectionFileContents);
            _connectionTreeModel = _deserializer.Deserialize();
        }

        [OneTimeTearDown]
        public void OnetimeTeardown()
        {
            _deserializer = null;
        }

        [Test]
        public void ConnectionTreeModelHasARootNode()
        {
            var numberOfRootNodes = _connectionTreeModel.RootNodes.Count;
            Assert.That(numberOfRootNodes, Is.GreaterThan(0));
        }

        [Test]
        public void RootNodeHasContents()
        {
            var rootNodeContents = _connectionTreeModel.RootNodes.First().Children;
            Assert.That(rootNodeContents, Is.Not.Empty);
        }

        [Test]
        public void AllSubRootFoldersImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var rootNodeContents = importedRdcmanRootNode.Children.Count(node => node.Name == "Group1" || node.Name == "Group2");
            Assert.That(rootNodeContents, Is.EqualTo(2));
        }
    }
}