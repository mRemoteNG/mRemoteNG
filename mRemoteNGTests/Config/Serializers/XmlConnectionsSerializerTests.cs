using System.Xml;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
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
        

        [SetUp]
        public void Setup()
        {
            var encryptor = new AeadCryptographyProvider();
            _serializer = new XmlConnectionsSerializer(encryptor);
            _connectionTreeModel = SetupConnectionTreeModel();
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
            var _folder1 = new ContainerInfo { Name = "folder1" };
            var _folder2 = new ContainerInfo { Name = "folder2" };
            var _folder3 = new ContainerInfo { Name = "folder3" };
            var _con0 = new ConnectionInfo { Name = "con0" };
            var _con1 = new ConnectionInfo { Name = "con1" };
            var _con2 = new ConnectionInfo { Name = "con2" };
            var _con3 = new ConnectionInfo { Name = "con3" };
            var _con4 = new ConnectionInfo { Name = "con4" };
            rootNode.AddChild(_folder1);
            rootNode.AddChild(_folder2);
            rootNode.AddChild(_con0);
            _folder1.AddChild(_con1);
            _folder2.AddChild(_con2);
            _folder2.AddChild(_folder3);
            _folder3.AddChild(_con3);
            _folder3.AddChild(_con4);
            connectionTreeModel.AddRootNode(rootNode);
            return connectionTreeModel;
        }
    }
}