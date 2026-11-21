using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Security;
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
            FileDataProvider dataProvider = new(_connectionFilePath);
            string xmlString = dataProvider.Load();
            XmlConnectionsDeserializer deserializer = new(PromptForPassword);
            ConnectionTreeModel connectionTree = deserializer.Deserialize(xmlString);
            DefaultKeyWarning.ForConnectionsFile(connectionTree, deserializer.IsDpapiProtected, Runtime.MessageCollector);
            return connectionTree;
        }

        private Optional<SecureString> PromptForPassword()
        {
            Optional<SecureString> password = MiscTools.PasswordDialog(Path.GetFileName(_connectionFilePath), false);
            return password;
        }
    }
}