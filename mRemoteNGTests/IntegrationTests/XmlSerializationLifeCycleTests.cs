using System;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.IntegrationTests
{
	public class XmlSerializationLifeCycleTests
    {
        private XmlConnectionsSerializer _serializer;
        private XmlConnectionsDeserializer _deserializer;
        private ConnectionTreeModel _originalModel;
        private readonly ICryptoProviderFactory _cryptoFactory = new CryptoProviderFactory(BlockCipherEngines.AES , BlockCipherModes.GCM);

        [SetUp]
        public void Setup()
        {
            _originalModel = SetupConnectionTreeModel();
            var cryptoProvider = _cryptoFactory.Build();
            var nodeSerializer = new XmlConnectionNodeSerializer26(
                cryptoProvider, 
                _originalModel.RootNodes.OfType<RootNodeInfo>().First().PasswordString.ConvertToSecureString(),
                new SaveFilter());
            _serializer = new XmlConnectionsSerializer(cryptoProvider, nodeSerializer);
            _deserializer = new XmlConnectionsDeserializer();
        }

        [TearDown]
        public void Teardown()
        {
            _serializer = null;
        }

        [Test]
        public void SerializeThenDeserialize()
        {
            var serializedContent = _serializer.Serialize(_originalModel);
            var deserializedModel = _deserializer.Deserialize(serializedContent);
            var nodeNamesFromDeserializedModel = deserializedModel.GetRecursiveChildList().Select(node => node.Name);
            var nodeNamesFromOriginalModel = _originalModel.GetRecursiveChildList().Select(node => node.Name);
            Assert.That(nodeNamesFromDeserializedModel, Is.EquivalentTo(nodeNamesFromOriginalModel));
        }

        [Test]
        public void SerializeThenDeserializeWithFullEncryption()
        {
            _serializer.UseFullEncryption = true;
            var serializedContent = _serializer.Serialize(_originalModel);
            var deserializedModel = _deserializer.Deserialize(serializedContent);
            var nodeNamesFromDeserializedModel = deserializedModel.GetRecursiveChildList().Select(node => node.Name);
            var nodeNamesFromOriginalModel = _originalModel.GetRecursiveChildList().Select(node => node.Name);
            Assert.That(nodeNamesFromDeserializedModel, Is.EquivalentTo(nodeNamesFromOriginalModel));
        }

        [Test]
        public void SerializeAndDeserializePropertiesWithInternationalCharacters()
        {
            var originalConnectionInfo = new ConnectionInfo {Name = "con1", Description = "£°úg¶┬ä" };
            var serializedContent = _serializer.Serialize(originalConnectionInfo);
            var deserializedModel = _deserializer.Deserialize(serializedContent);
            var deserializedConnectionInfo = deserializedModel.GetRecursiveChildList().First(node => node.Name == originalConnectionInfo.Name);
            Assert.That(deserializedConnectionInfo.Description, Is.EqualTo(originalConnectionInfo.Description));
        }


        [Test]
        public void SerializeAndDeserializeWithCustomKdfIterationsValue()
        {
            var cryptoProvider = _cryptoFactory.Build();
            cryptoProvider.KeyDerivationIterations = 5000;
            var nodeSerializer = new XmlConnectionNodeSerializer26(
                cryptoProvider, 
                _originalModel.RootNodes.OfType<RootNodeInfo>().First().PasswordString.ConvertToSecureString(),
                new SaveFilter());
            _serializer = new XmlConnectionsSerializer(cryptoProvider, nodeSerializer);
            var serializedContent = _serializer.Serialize(_originalModel);
            var deserializedModel = _deserializer.Deserialize(serializedContent);
            var nodeNamesFromDeserializedModel = deserializedModel.GetRecursiveChildList().Select(node => node.Name);
            var nodeNamesFromOriginalModel = _originalModel.GetRecursiveChildList().Select(node => node.Name);
            Assert.That(nodeNamesFromDeserializedModel, Is.EquivalentTo(nodeNamesFromOriginalModel));
        }

        [Test]
        public void GuidCreatedIfNonExistedInXml()
        {
            var originalConnectionInfo = new ConnectionInfo { Name = "con1" };
            var serializedContent = _serializer.Serialize(originalConnectionInfo);

            // remove GUID from connection xml
            serializedContent = serializedContent.Replace(originalConnectionInfo.ConstantID, "");

            var deserializedModel = _deserializer.Deserialize(serializedContent);
            var deserializedConnectionInfo = deserializedModel.GetRecursiveChildList().First(node => node.Name == originalConnectionInfo.Name);
            Assert.That(Guid.TryParse(deserializedConnectionInfo.ConstantID, out var guid));
        }


        private ConnectionTreeModel SetupConnectionTreeModel()
        {
            /*
             * Root
             * |--- con0
             * |--- folder1
             * |    L--- con1
             * L--- folder2
             *      |--- con2
             *      L--- folder3
             *           |--- con3
             *           L--- con4
             */
            var connectionTreeModel = new ConnectionTreeModel();
            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            var folder1 = new ContainerInfo { Name = "folder1" };
            var folder2 = new ContainerInfo { Name = "folder2" };
            var folder3 = new ContainerInfo { Name = "folder3" };
            var con0 = new ConnectionInfo { Name = "con0" };
            var con1 = new ConnectionInfo { Name = "con1" };
            var con2 = new ConnectionInfo { Name = "con2" };
            var con3 = new ConnectionInfo { Name = "con3" };
            var con4 = new ConnectionInfo { Name = "con4" };
            rootNode.AddChild(folder1);
            rootNode.AddChild(folder2);
            rootNode.AddChild(con0);
            folder1.AddChild(con1);
            folder2.AddChild(con2);
            folder2.AddChild(folder3);
            folder3.AddChild(con3);
            folder3.AddChild(con4);
            connectionTreeModel.AddRootNode(rootNode);
            return connectionTreeModel;
        }
    }
}