using System;
using System.Linq;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.Properties;
using System.Runtime.Versioning;

namespace mRemoteNG.Config.Connections
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionsSaver : ISaver<ConnectionTreeModel>
    {
        private readonly string _connectionFileName;
        private readonly SaveFilter _saveFilter;

        public XmlConnectionsSaver(string connectionFileName, SaveFilter saveFilter)
        {
            if (string.IsNullOrEmpty(connectionFileName))
                throw new ArgumentException($"Argument '{nameof(connectionFileName)}' cannot be null or empty");
            _connectionFileName = connectionFileName;
            _saveFilter = saveFilter ?? throw new ArgumentNullException(nameof(saveFilter));
        }

        public void Save(ConnectionTreeModel connectionTreeModel, string propertyNameTrigger = "")
        {
            ICryptographyProvider cryptographyProvider = new ConnectionsFileCryptoProviderFactory().Build();
            XmlConnectionSerializerFactory serializerFactory = new();

            Serializers.ISerializer<Connection.ConnectionInfo, string> xmlConnectionsSerializer = serializerFactory.Build(cryptographyProvider, connectionTreeModel, _saveFilter, Properties.OptionsSecurityPage.Default.EncryptCompleteConnectionsFile);

            RootNodeInfo rootNode = connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();
            string xml = xmlConnectionsSerializer.Serialize(rootNode);

            // Refuse to overwrite the connection file with an empty document. An empty
            // result indicates a serialization failure; writing it would destroy the file
            // (and rotate the last good backup out) while the save still reports success.
            if (string.IsNullOrWhiteSpace(xml))
                throw new InvalidOperationException($"Connection serialization produced no output; refusing to overwrite {_connectionFileName}");

            FileDataProviderWithRollingBackup fileDataProvider = new(_connectionFileName);
            fileDataProvider.Save(xml);
        }
    }
}