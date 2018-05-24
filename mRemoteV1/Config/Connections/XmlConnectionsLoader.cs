using System.Security;
using System.Windows.Forms;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Connection;

namespace mRemoteNG.Config.Connections
{
	public class XmlConnectionsLoader
	{
	    private readonly IConnectionsService _connectionsService;
        private readonly string _connectionFilePath;
	    private readonly IWin32Window _dialogWindowParent;

        public XmlConnectionsLoader(string connectionFilePath, IConnectionsService connectionsService, IWin32Window dialogWindowParent)
        {
            _dialogWindowParent = dialogWindowParent;
            _connectionFilePath = connectionFilePath.ThrowIfNullOrEmpty(nameof(connectionFilePath));
            _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
        }

        public ConnectionTreeModel Load()
        {
            var dataProvider = new FileDataProvider(_connectionFilePath);
            var xmlString = dataProvider.Load();
            var deserializer = new XmlConnectionsDeserializer(_connectionsService, _dialogWindowParent, PromptForPassword);
            return deserializer.Deserialize(xmlString);
        }

        private SecureString PromptForPassword()
        {
            return MiscTools.PasswordDialog("", false);
        }
    }
}
