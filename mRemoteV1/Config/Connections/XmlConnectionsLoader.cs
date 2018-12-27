using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using System;
using System.IO;
using System.Security;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Credential;
using mRemoteNG.UI.Forms;

namespace mRemoteNG.Config.Connections
{
    public class XmlConnectionsLoader : IConnectionsLoader
    {
        private readonly string _credentialFilePath = Path.Combine(CredentialsFileInfo.CredentialsPath, CredentialsFileInfo.CredentialsFile);
        private readonly string _connectionFilePath;
        private readonly CredentialServiceFacade _credentialService;

        public XmlConnectionsLoader(string connectionFilePath, CredentialServiceFacade credentialService)
        {
            if (string.IsNullOrEmpty(connectionFilePath))
                throw new ArgumentException($"{nameof(connectionFilePath)} cannot be null or empty");

            if (!File.Exists(connectionFilePath))
                throw new FileNotFoundException($"{connectionFilePath} does not exist");

            _connectionFilePath = connectionFilePath;
            _credentialService = credentialService.ThrowIfNull(nameof(credentialService));
        }

        public ConnectionTreeModel Load()
        {
            var dataProvider = new FileDataProvider(_connectionFilePath);
            var xmlString = dataProvider.Load();
            var deserializer = new CredentialManagerUpgradeForm
            {
                ConnectionFilePath = _connectionFilePath,
                NewCredentialRepoPath = _credentialFilePath,
                DecoratedDeserializer = new XmlCredentialManagerUpgrader(
                    _credentialService,
                    _credentialFilePath,
                    new XmlConnectionsDeserializer(PromptForPassword)
                )
            };

            return deserializer.Deserialize(xmlString);
        }

        private Optional<SecureString> PromptForPassword()
        {
            var password = MiscTools.PasswordDialog("", false);
            return password;
        }
    }
}
