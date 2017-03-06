using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class XmlConnectionsDocumentEncryptorTests
    {
        private XmlConnectionsDocumentEncryptor _documentEncryptor;
        private XDocument _originalDocument;

        [SetUp]
        public void Setup()
        {
            var connectionTreeModel = SetupConnectionTreeModel();
            var cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.AES, BlockCipherModes.GCM);
            var connectionNodeSerializer = new XmlConnectionNodeSerializer27(
                cryptoProvider, 
                connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First().PasswordString.ConvertToSecureString(),
                new SaveFilter());
            _originalDocument = new XmlConnectionsDocumentCompiler(cryptoProvider, connectionNodeSerializer).CompileDocument(connectionTreeModel, false);
            _documentEncryptor = new XmlConnectionsDocumentEncryptor(cryptoProvider);
        }

        [Test]
        public void RootNodeValueIsEncrypted()
        {
            var encryptedDocument = _documentEncryptor.EncryptDocument(_originalDocument, "mR3m".ConvertToSecureString());
            var encryptedContent = encryptedDocument.Root?.Value;
            Assert.That(encryptedContent, Is.Not.EqualTo(string.Empty));
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