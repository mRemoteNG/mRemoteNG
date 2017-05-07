using System;
using System.Collections.Generic;
using System.Security;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Credential;
using mRemoteNG.Tools;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Connections
{
    public class XmlConnectionsLoader
    {
        private readonly string _connectionFilePath;
        private readonly IEnumerable<ICredentialRecord> _credentialRecords;

        public XmlConnectionsLoader(string connectionFilePath, IEnumerable<ICredentialRecord> credentialRecords)
        {
            if (string.IsNullOrEmpty(connectionFilePath))
                throw new ArgumentException($"{nameof(connectionFilePath)} cannot be null or empty");
            if (credentialRecords == null)
                throw new ArgumentNullException(nameof(credentialRecords));

            _connectionFilePath = connectionFilePath;
            _credentialRecords = credentialRecords;
        }

        public ConnectionTreeModel Load()
        {
            var dataProvider = new FileDataProvider(_connectionFilePath);
            var xmlString = dataProvider.Load();
            var deserializer = new XmlConnectionsDeserializer(_credentialRecords, PromptForPassword);
            return deserializer.Deserialize(xmlString);
        }

        private SecureString PromptForPassword()
        {
            var password = MiscTools.PasswordDialog("", false);
            return password;
        }
    }
}
