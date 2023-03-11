using mRemoteNG.Config.DataProviders;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using System;
using System.IO;
using System.Security;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Connections
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionsLoader : IConnectionsLoader
    {
        private readonly string _connectionFilePath;

        public XmlConnectionsLoader(string connectionFilePath)
        {
            if (string.IsNullOrEmpty(connectionFilePath))
                throw new ArgumentException($"{nameof(connectionFilePath)} cannot be null or empty");

            if (!File.Exists(connectionFilePath))
                throw new FileNotFoundException($"{connectionFilePath} does not exist");

            _connectionFilePath = connectionFilePath;
        }

        public ConnectionTreeModel Load()
        {
            var dataProvider = new FileDataProvider(_connectionFilePath);
            var xmlString = dataProvider.Load();
            var deserializer = new XmlConnectionsDeserializer(PromptForPassword);
            return deserializer.Deserialize(xmlString);
        }

        private Optional<SecureString> PromptForPassword()
        {
            var password = MiscTools.PasswordDialog(Path.GetFileName(_connectionFilePath), false);
            return password;
        }
    }
}