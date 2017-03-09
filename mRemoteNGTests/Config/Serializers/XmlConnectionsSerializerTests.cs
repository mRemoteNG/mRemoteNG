using System.Linq;
using System.Xml;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class XmlConnectionsSerializerTests
    {
        private XmlConnectionsSerializer _serializer;
        private ConnectionTreeModel _connectionTreeModel;
        private ICryptographyProvider _cryptographyProvider;
        

        [SetUp]
        public void Setup()
        {
            _connectionTreeModel = SetupConnectionTreeModel();
            _cryptographyProvider = new AeadCryptographyProvider();
            var connectionNodeSerializer = new XmlConnectionNodeSerializer27(
                _cryptographyProvider, 
                _connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First().PasswordString.ConvertToSecureString(),
                new SaveFilter());
            _serializer = new XmlConnectionsSerializer(_cryptographyProvider, connectionNodeSerializer);
        }

        [Test]
        public void ChildNestingSerializedCorrectly()
        {
            var serializedConnections = _serializer.Serialize(_connectionTreeModel);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(serializedConnections);
            var nodeCon4 = xmlDoc.DocumentElement?.SelectSingleNode("Node[@Name='folder2']/Node[@Name='folder3']/Node[@Name='con4']");
            Assert.That(nodeCon4, Is.Not.Null);
        }

        [Test]
        public void SingleConnectionSerializedCorrectly()
        {
            var connectionInfo = new ConnectionInfo {Name = "myConnection"};
            var serializedConnections = _serializer.Serialize(connectionInfo);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(serializedConnections);
            var connectionNode = xmlDoc.DocumentElement?.SelectSingleNode($"Node[@Name='{connectionInfo.Name}']");
            Assert.That(connectionNode, Is.Not.Null);
        }

        [TestCase("CredentialId", "")]
        [TestCase("InheritAutomaticResize", "False")]
        public void SerializerRespectsSaveFilterSettings(string attributeName, string expectedValue)
        {
            var connectionNodeSerializer = new XmlConnectionNodeSerializer27(
                _cryptographyProvider,
                _connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First().PasswordString.ConvertToSecureString(),
                new SaveFilter(true));
            var serializer = new XmlConnectionsSerializer(_cryptographyProvider, connectionNodeSerializer);
            var connectionInfo = new ConnectionInfo
            {
                Name = "myConnection",
                Inheritance = {AutomaticResize = true}
            };
            var serializedConnections = serializer.Serialize(connectionInfo);
            var xdoc = XDocument.Parse(serializedConnections);
            var attributeValue = xdoc.Root?.Element("Node")?.Attribute(attributeName)?.Value;
            Assert.That(attributeValue, Is.EqualTo(expectedValue));
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