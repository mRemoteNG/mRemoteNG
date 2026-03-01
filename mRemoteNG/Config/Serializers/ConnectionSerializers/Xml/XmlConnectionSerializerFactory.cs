using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    [SupportedOSPlatform("windows")]
    public static class XmlConnectionSerializerFactory
    {
        public static ISerializer<ConnectionInfo, string> Build(
            ICryptographyProvider cryptographyProvider,
            ConnectionTreeModel connectionTreeModel,
            SaveFilter? saveFilter = null,
            bool useFullEncryption = false)
        {
            System.Security.SecureString encryptionKey = connectionTreeModel
                .RootNodes.OfType<RootNodeInfo>()
                .First().PasswordString
                .ConvertToSecureString();

            XmlConnectionNodeSerializer28 connectionNodeSerializer = new(
                cryptographyProvider,
                encryptionKey,
                saveFilter ?? new SaveFilter());

            return new XmlConnectionsSerializer(cryptographyProvider, connectionNodeSerializer)
            {
                UseFullEncryption = useFullEncryption
            };
        }
    }
}
