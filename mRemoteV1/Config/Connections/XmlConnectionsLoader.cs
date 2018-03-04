using System.Security;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Connection;

namespace mRemoteNG.Config.Connections
{
	public class XmlConnectionsLoader
	{
	    private readonly ConnectionsService _connectionsService;
        private readonly string _connectionFilePath;

        public XmlConnectionsLoader(string connectionFilePath, ConnectionsService connectionsService)
        {
            _connectionFilePath = connectionFilePath.ThrowIfNullOrEmpty(nameof(connectionFilePath));
            _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
        }

        public ConnectionTreeModel Load()
        {
            var dataProvider = new FileDataProvider(_connectionFilePath);
            var xmlString = dataProvider.Load();
            var deserializer = new XmlConnectionsDeserializer(_connectionsService, PromptForPassword);
            return deserializer.Deserialize(xmlString);
        }

        private SecureString PromptForPassword()
        {
            var password = MiscTools.PasswordDialog("", false);
            return password;
        }
    }
}
