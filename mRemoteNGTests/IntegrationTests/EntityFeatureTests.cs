using System;
using System.IO;
using System.Linq;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.IntegrationTests
{
    [TestFixture]
    public class EntityFeatureTests
    {
        [Test]
        public void CreateEntity_ShouldHaveCorrectTypeAndProperty()
        {
            var entity = new ContainerInfo { IsEntity = true, Name = "MyEntity" };
            Assert.That(entity.IsEntity, Is.True);
            Assert.That(entity.GetTreeNodeType(), Is.EqualTo(TreeNodeType.Entity));
            Assert.That(entity.IsContainer, Is.True);
        }

        [Test]
        public void SerializeAndDeserialize_Entity_ShouldPersistIsEntityFlag()
        {
            // Arrange
            var cryptoFactory = new CryptoProviderFactory(BlockCipherEngines.AES, BlockCipherModes.GCM);
            var cryptoProvider = cryptoFactory.Build();
            
            var entity = new ContainerInfo { IsEntity = true, Name = "MyEntity" };
            var childConnection = new mRemoteNG.Connection.ConnectionInfo { Name = "ChildConnection", Hostname = "example.com" };
            entity.AddChild(childConnection);
            
            var root = new RootNodeInfo(RootNodeType.Connection);
            root.AddChild(entity);

            var model = new ConnectionTreeModel();
            model.AddRootNode(root);

            var nodeSerializer = new XmlConnectionNodeSerializer28(
                cryptoProvider, 
                new System.Security.SecureString(), // Empty password
                new mRemoteNG.Security.SaveFilter());
                
            var serializer = new XmlConnectionsSerializer(cryptoProvider, nodeSerializer);

            // Act: Serialize
            string fullXml = serializer.Serialize(model);

            // Verify XML contains Type="Entity"
            Assert.That(fullXml, Does.Contain("Type=\"Entity\""));

            // Act: Deserialize
            var deserializer = new XmlConnectionsDeserializer();
            var loadedModel = deserializer.Deserialize(fullXml);

            // Assert
            var loadedRoot = loadedModel.RootNodes.First();
            var loadedEntity = loadedRoot.Children.FirstOrDefault(c => string.Equals(c.Name, "MyEntity", StringComparison.Ordinal));

            Assert.That(loadedEntity, Is.Not.Null);
            Assert.That(loadedEntity, Is.InstanceOf<ContainerInfo>());
            var loadedContainer = (ContainerInfo)loadedEntity;
            
            Assert.That(loadedContainer.IsEntity, Is.True, "IsEntity should be true after deserialization");
            Assert.That(loadedContainer.GetTreeNodeType(), Is.EqualTo(TreeNodeType.Entity));
            Assert.That(loadedContainer.Children.Count, Is.EqualTo(1));
            Assert.That(loadedContainer.Children[0].Name, Is.EqualTo("ChildConnection"));
        }
    }
}
