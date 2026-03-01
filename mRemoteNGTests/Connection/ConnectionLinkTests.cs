using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    [TestFixture]
    public class ConnectionLinkTests
    {
        private ConnectionTreeModel _treeModel;
        private ContainerInfo _rootNode;

        [SetUp]
        public void Setup()
        {
            _treeModel = new ConnectionTreeModel();
            _rootNode = new RootNodeInfo(RootNodeType.Connection) { Name = "Root" };
            _treeModel.AddRootNode(_rootNode);
            
            // Set Runtime.ConnectionsService.ConnectionTreeModel using reflection because the setter is private
            var connectionsService = Runtime.ConnectionsService;
            if (connectionsService != null)
            {
                var prop = connectionsService.GetType().GetProperty("ConnectionTreeModel", BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(connectionsService, _treeModel, null);
                }
            }
        }

        [Test]
        public void TestCreateLink()
        {
            // 1. Create source connection
            var sourceConnection = new ConnectionInfo
            {
                Name = "SourceConnection",
                Hostname = "source.example.com",
                Username = "user1"
            };
            sourceConnection.SetParent(_rootNode);
            //_rootNode.AddChild(sourceConnection); // SetParent calls AddChild

            // 2. Create link (simulate ConnectionTree.CreateLinkToSelectedNode)
            var linkedConnection = sourceConnection.Clone();
            linkedConnection.Name = "LinkedConnection"; // Give it a different name to distinguish
            // ResolveLinkedConnection returns null if it's not a link, so we use the node itself
            var sourceNode = _treeModel.ResolveLinkedConnection(sourceConnection) ?? sourceConnection;
            linkedConnection.LinkedConnectionId = sourceNode.ConstantID;
            
            linkedConnection.SetParent(_rootNode);
            //_rootNode.AddChild(linkedConnection);

            // 3. Verify link properties
            Assert.That(linkedConnection.IsLinkedConnection, Is.True, "IsLinkedConnection should be true");
            Assert.That(linkedConnection.LinkedConnectionId, Is.EqualTo(sourceConnection.ConstantID), "LinkedConnectionId should match source ID");

            // 4. Verify property delegation
            Assert.That(linkedConnection.Hostname, Is.EqualTo("source.example.com"), "Hostname should be inherited from source");
            Assert.That(linkedConnection.Username, Is.EqualTo("user1"), "Username should be inherited from source");

            // 5. Change source property and verify link updates
            sourceConnection.Hostname = "updated.example.com";
            Assert.That(linkedConnection.Hostname, Is.EqualTo("updated.example.com"), "Link should reflect updated source Hostname");
        }

        [Test]
        public void TestLinkSerialization()
        {
            // 1. Setup
            var sourceConnection = new ConnectionInfo
            {
                Name = "SourceConnection",
                Hostname = "192.168.1.1"
            };
            sourceConnection.SetParent(_rootNode);

            var linkedConnection = new ConnectionInfo
            {
                Name = "LinkToSource",
                LinkedConnectionId = sourceConnection.ConstantID
            };
            linkedConnection.SetParent(_rootNode);

            // 2. Serialize
            var cryptoProvider = new AeadCryptographyProvider();
            cryptoProvider.KeyDerivationIterations = 1000;
            var nodeSerializer = new XmlConnectionNodeSerializer28(cryptoProvider, new SecureString(), new SaveFilter());
            var serializer = new XmlConnectionsSerializer(cryptoProvider, nodeSerializer);
            
            string xml = serializer.Serialize(_treeModel);

            // 3. Verify XML contains LinkedConnectionId
            Assert.That(xml.Contains($"LinkedConnectionId=\"{sourceConnection.ConstantID}\""), Is.True, "XML should contain LinkedConnectionId attribute");

            // 4. Deserialize
            var deserializer = new XmlConnectionsDeserializer();
            var newTreeModel = deserializer.Deserialize(xml);
            
            // Set the new model to runtime so property resolution works
            var connectionsService = Runtime.ConnectionsService;
            if (connectionsService != null)
            {
                var prop = connectionsService.GetType().GetProperty("ConnectionTreeModel", BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(connectionsService, newTreeModel, null);
                }
            }

            var newRoot = newTreeModel.RootNodes[0];
            var newSource = newRoot.Children.FirstOrDefault(c => string.Equals(c.Name, "SourceConnection", StringComparison.Ordinal)) as ConnectionInfo;
            var newLink = newRoot.Children.FirstOrDefault(c => string.Equals(c.Name, "LinkToSource", StringComparison.Ordinal)) as ConnectionInfo;

            Assert.That(newSource, Is.Not.Null, "Source connection should exist after deserialization");
            Assert.That(newLink, Is.Not.Null, "Linked connection should exist after deserialization");

            // 5. Verify properties
            Assert.That(newLink.LinkedConnectionId, Is.EqualTo(newSource.ConstantID), "Linked ID should match source ID");
            Assert.That(newLink.IsLinkedConnection, Is.True, "Should be identified as linked connection");
            
            // Verify delegation works after deserialization
            Assert.That(newLink.Hostname, Is.EqualTo("192.168.1.1"), "Hostname should be resolved from source");
        }
    }
}
