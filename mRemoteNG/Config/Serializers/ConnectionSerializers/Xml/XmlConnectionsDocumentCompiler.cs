using System;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionsDocumentCompiler
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private SecureString _encryptionKey;
        private readonly ISerializer<ConnectionInfo, XElement> _connectionNodeSerializer;

        public XmlConnectionsDocumentCompiler(ICryptographyProvider cryptographyProvider,
                                              ISerializer<ConnectionInfo, XElement> connectionNodeSerializer)
        {
            if (cryptographyProvider == null)
                throw new ArgumentNullException(nameof(cryptographyProvider));
            if (connectionNodeSerializer == null)
                throw new ArgumentNullException(nameof(connectionNodeSerializer));

            _cryptographyProvider = cryptographyProvider;
            _connectionNodeSerializer = connectionNodeSerializer;
        }

        public XDocument CompileDocument(ConnectionTreeModel connectionTreeModel, bool fullFileEncryption)
        {
            var rootNodeInfo = GetRootNodeFromConnectionTreeModel(connectionTreeModel);
            return CompileDocument(rootNodeInfo, fullFileEncryption);
        }

        public XDocument CompileDocument(ConnectionInfo serializationTarget, bool fullFileEncryption)
        {
            var rootNodeInfo = GetRootNodeFromConnectionInfo(serializationTarget);
            _encryptionKey = rootNodeInfo.PasswordString.ConvertToSecureString();
            var rootElement = CompileRootNode(rootNodeInfo, fullFileEncryption);

            CompileRecursive(serializationTarget, rootElement);
            var xmlDeclaration = new XDeclaration("1.0", "utf-8", null);
            var xmlDocument = new XDocument(xmlDeclaration, rootElement);
            if (fullFileEncryption)
                xmlDocument =
                    new XmlConnectionsDocumentEncryptor(_cryptographyProvider).EncryptDocument(xmlDocument,
                                                                                               _encryptionKey);
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
            foreach (var child in serializationTargetAsContainer.Children.ToArray())
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

        private XElement CompileRootNode(RootNodeInfo rootNodeInfo, bool fullFileEncryption)
        {
            var rootNodeSerializer = new XmlRootNodeSerializer();
            return rootNodeSerializer.SerializeRootNodeInfo(rootNodeInfo, _cryptographyProvider,
                                                            _connectionNodeSerializer.Version, fullFileEncryption);
        }

        private XElement CompileConnectionInfoNode(ConnectionInfo connectionInfo)
        {
            return _connectionNodeSerializer.Serialize(connectionInfo);
        }
    }
}