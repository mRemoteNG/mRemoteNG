using System.Linq;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Serializers
{
    public class XmlConnectionsDocumentCompiler
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private SecureString _encryptionKey;

        public XmlConnectionsDocumentCompiler(ICryptographyProvider cryptographyProvider)
        {
            _cryptographyProvider = cryptographyProvider;
        }

        public XDocument CompileDocument(ConnectionTreeModel connectionTreeModel, bool fullFileEncryption, bool export)
        {
            var rootNodeInfo = GetRootNodeFromConnectionTreeModel(connectionTreeModel);
            return CompileDocument(rootNodeInfo, fullFileEncryption, export);
        }

        public XDocument CompileDocument(ConnectionInfo serializationTarget, bool fullFileEncryption, bool export)
        {
            var rootNodeInfo = GetRootNodeFromConnectionInfo(serializationTarget);
            _encryptionKey = rootNodeInfo.PasswordString.ConvertToSecureString();
            var rootElement = CompileRootNode(rootNodeInfo, fullFileEncryption, export);

            CompileRecursive(serializationTarget, rootElement);
            var xmlDeclaration = new XDeclaration("1.0", "utf-8", null);
            var xmlDocument = new XDocument(xmlDeclaration, rootElement);
            if (fullFileEncryption)
                xmlDocument = new XmlConnectionsDocumentEncryptor(_cryptographyProvider).EncryptDocument(xmlDocument, _encryptionKey);
            return xmlDocument;
        }

        private void CompileRecursive(ConnectionInfo serializationTarget, XContainer parentElement)
        {
            var newElement = parentElement;
            if (!(serializationTarget is RootNodeInfo))
            {
                newElement = CompileConnectionInfoNode(serializationTarget);
                parentElement.Add(newElement);
            }
            var serializationTargetAsContainer = serializationTarget as ContainerInfo;
            if (serializationTargetAsContainer == null) return;
            foreach (var child in serializationTargetAsContainer.Children)
                CompileRecursive(child, newElement);
        }

        private RootNodeInfo GetRootNodeFromConnectionTreeModel(ConnectionTreeModel connectionTreeModel)
        {
            return (RootNodeInfo)connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
        }

        private RootNodeInfo GetRootNodeFromConnectionInfo(ConnectionInfo connectionInfo)
        {
            while (true)
            {
                var connectionInfoAsRootNode = connectionInfo as RootNodeInfo;
                if (connectionInfoAsRootNode != null) return connectionInfoAsRootNode;
                connectionInfo = connectionInfo?.Parent ?? new RootNodeInfo(RootNodeType.Connection);
            }
        }

        private XElement CompileRootNode(RootNodeInfo rootNodeInfo, bool fullFileEncryption, bool export)
        {
            var rootNodeSerializer = new XmlRootNodeSerializer();
            return rootNodeSerializer.SerializeRootNodeInfo(rootNodeInfo, _cryptographyProvider, fullFileEncryption, export);
        }

        private XElement CompileConnectionInfoNode(ConnectionInfo connectionInfo)
        {
            var connectionSerializer = new XmlConnectionNodeSerializer(_cryptographyProvider, _encryptionKey);
            return connectionSerializer.SerializeConnectionInfo(connectionInfo);
        }
    }
}