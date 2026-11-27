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
            string xmlString = File.ReadAllText(_connectionFilePath);
            if (string.IsNullOrWhiteSpace(xmlString))
                throw new System.Xml.XmlException("The connection file is empty.");
            XmlConnectionsDeserializer deserializer = new(PromptForPassword);
            return deserializer.Deserialize(xmlString);
        }

        private Optional<SecureString> PromptForPassword()
        {
            Optional<SecureString> password = MiscTools.PasswordDialog(Path.GetFileName(_connectionFilePath), false);
            return password;
        }
    }
}