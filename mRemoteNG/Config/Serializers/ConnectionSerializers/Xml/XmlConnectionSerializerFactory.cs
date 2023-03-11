using System.Linq;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionSerializerFactory
    {
        public ISerializer<ConnectionInfo, string> Build(
            ICryptographyProvider cryptographyProvider,
            ConnectionTreeModel connectionTreeModel,
            SaveFilter saveFilter = null,
            bool useFullEncryption = false)
        {
            var encryptionKey = connectionTreeModel
                .RootNodes.OfType<RootNodeInfo>()
                .First().PasswordString
                .ConvertToSecureString();

            var connectionNodeSerializer = new XmlConnectionNodeSerializer27(
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
