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
        private SecureString? _encryptionKey;
        private readonly ISerializer<ConnectionInfo, XElement> _connectionNodeSerializer;

        public XmlConnectionsDocumentCompiler(ICryptographyProvider cryptographyProvider, ISerializer<ConnectionInfo, XElement> connectionNodeSerializer)
        {
            ArgumentNullException.ThrowIfNull(cryptographyProvider);
            ArgumentNullException.ThrowIfNull(connectionNodeSerializer);
            _cryptographyProvider = cryptographyProvider;
            _connectionNodeSerializer = connectionNodeSerializer;
        }

        public XDocument CompileDocument(ConnectionTreeModel connectionTreeModel, bool fullFileEncryption)
        {
            RootNodeInfo rootNodeInfo = GetRootNodeFromConnectionTreeModel(connectionTreeModel);
            _encryptionKey = rootNodeInfo.PasswordString.ConvertToSecureString();
            XElement rootElement = CompileRootNode(rootNodeInfo, fullFileEncryption);

            // Iterate through all root nodes.
            // If it is the main RootNodeInfo, we flatten its children into the XML root.
            // If it is a sibling ContainerInfo, we add it as a child of XML root with IsRoot="true".
            foreach (ContainerInfo root in connectionTreeModel.RootNodes)
            {
                if (root is RootNodeInfo)
                {
                    CompileRecursive(root, rootElement);
                }
                else
                {
                    CompileRecursive(root, rootElement, true);
                }
            }
            
            XDeclaration xmlDeclaration = new("1.0", "utf-8", null);
            XDocument xmlDocument = new(xmlDeclaration, rootElement);
            if (fullFileEncryption && _encryptionKey is not null)
                xmlDocument = new XmlConnectionsDocumentEncryptor(_cryptographyProvider).EncryptDocument(xmlDocument, _encryptionKey);
            return xmlDocument;
        }

        public XDocument CompileDocument(ConnectionInfo serializationTarget, bool fullFileEncryption)
        {
            RootNodeInfo rootNodeInfo = GetRootNodeFromConnectionInfo(serializationTarget);
            _encryptionKey = rootNodeInfo.PasswordString.ConvertToSecureString();
            XElement rootElement = CompileRootNode(rootNodeInfo, fullFileEncryption);

            CompileRecursive(serializationTarget, rootElement);
            XDeclaration xmlDeclaration = new("1.0", "utf-8", null);
            XDocument xmlDocument = new(xmlDeclaration, rootElement);
            if (fullFileEncryption && _encryptionKey is not null)
                xmlDocument = new XmlConnectionsDocumentEncryptor(_cryptographyProvider).EncryptDocument(xmlDocument, _encryptionKey);
            return xmlDocument;
        }

        private void CompileRecursive(ConnectionInfo serializationTarget, XContainer parentElement, bool isRoot = false)
        {
            XContainer newElement = parentElement;
            if (serializationTarget is not RootNodeInfo)
            {
                XElement element = CompileConnectionInfoNode(serializationTarget);
                if (isRoot)
                    element.SetAttributeValue("IsRoot", true);
                
                newElement = element;
                parentElement.Add(newElement);
            }

            if (serializationTarget is not ContainerInfo serializationTargetAsContainer) return;
            foreach (ConnectionInfo child in serializationTargetAsContainer.Children.ToArray())
                CompileRecursive(child, newElement);
        }

        private static RootNodeInfo GetRootNodeFromConnectionTreeModel(ConnectionTreeModel connectionTreeModel)
        {
            return (RootNodeInfo)connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
        }

        private static RootNodeInfo GetRootNodeFromConnectionInfo(ConnectionInfo connectionInfo)
        {
            while (true)
            {
                if (connectionInfo is RootNodeInfo connectionInfoAsRootNode) return connectionInfoAsRootNode;
                connectionInfo = connectionInfo?.Parent ?? new RootNodeInfo(RootNodeType.Connection);
            }
        }

        private XElement CompileRootNode(RootNodeInfo rootNodeInfo, bool fullFileEncryption)
        {
            return XmlRootNodeSerializer.SerializeRootNodeInfo(rootNodeInfo, _cryptographyProvider, _connectionNodeSerializer.Version, fullFileEncryption);
        }

        private XElement CompileConnectionInfoNode(ConnectionInfo connectionInfo)
        {
            return _connectionNodeSerializer.Serialize(connectionInfo);
        }
    }
}