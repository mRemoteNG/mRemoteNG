using System.Xml.XPath;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class XmlConnectionsDocumentCompilerTests
    {
        private XmlConnectionsDocumentCompiler _documentCompiler;
        private ConnectionTreeModel _connectionTreeModel;
        private ICryptographyProvider _cryptographyProvider;
        private ContainerInfo _folder1;
        private ContainerInfo _folder2;
        private ContainerInfo _folder3;
        private ConnectionInfo _con0;
        private ConnectionInfo _con1;
        private ConnectionInfo _con2;
        private ConnectionInfo _con3;
        private ConnectionInfo _con4;

        [SetUp]
        public void Setup()
        {
            _cryptographyProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.AES, BlockCipherModes.GCM);
            _documentCompiler = new XmlConnectionsDocumentCompiler(_cryptographyProvider);
            _connectionTreeModel = SetupConnectionTreeModel();
        }

        [Test]
        public void XDocumentHasXmlDeclaration()
        {
            var xdoc = _documentCompiler.CompileDocument(_connectionTreeModel, false, false);
            Assert.That(xdoc.Declaration, Is.Not.Null);
        }

        [Test]
        public void DocumentHasRootConnectionElement()
        {
            var xdoc =_documentCompiler.CompileDocument(_connectionTreeModel, false, false);
            var rootElementName = xdoc.Root?.Name.LocalName;
            Assert.That(rootElementName, Is.EqualTo("Connections"));
        }

        [Test]
        public void ConnectionNodesSerializedRecursively()
        {
            var xdoc = _documentCompiler.CompileDocument(_connectionTreeModel, false, false);
            var con4 = xdoc.Root?.XPathSelectElement("Node[@Name='folder2']/Node[@Name='folder3']/Node[@Name='con4']");
            Assert.That(con4, Is.Not.Null);
        }

        [Test]
        public void XmlContentEncryptedWhenFullFileEncryptionTurnedOn()
        {
            var xdoc = _documentCompiler.CompileDocument(_connectionTreeModel, true, false);
            var rootElementValue = xdoc.Root?.Value;
            Assert.That(rootElementValue, Is.Not.EqualTo(string.Empty));
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
            BuildTreeNodes();
            var connectionTreeModel = new ConnectionTreeModel();
            var rootNode = new RootNodeInfo(RootNodeType.Connection);
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

        private void BuildTreeNodes()
        {
            _folder1 = new ContainerInfo { Name = "folder1" };
            _folder2 = new ContainerInfo { Name = "folder2" };
            _folder3 = new ContainerInfo { Name = "folder3" };
            _con0 = new ConnectionInfo { Name = "con0" };
            _con1 = new ConnectionInfo { Name = "con1" };
            _con2 = new ConnectionInfo { Name = "con2" };
            _con3 = new ConnectionInfo { Name = "con3" };
            _con4 = new ConnectionInfo { Name = "con4" };
        }
    }
}