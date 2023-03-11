using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.ConnectionSerializers.MsSql;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Connections
{
    [SupportedOSPlatform("windows")]
    public class SqlConnectionsLoader : IConnectionsLoader
    {
        private readonly IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>>
            _localConnectionPropertiesDeserializer;

        private readonly IDataProvider<string> _dataProvider;

        public Func<Optional<SecureString>> AuthenticationRequestor { get; set; } =
            () => MiscTools.PasswordDialog("", false);

        public SqlConnectionsLoader(
            IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>> localConnectionPropertiesDeserializer,
            IDataProvider<string> dataProvider)
        {
            _localConnectionPropertiesDeserializer =
                localConnectionPropertiesDeserializer.ThrowIfNull(nameof(localConnectionPropertiesDeserializer));
            _dataProvider = dataProvider.ThrowIfNull(nameof(dataProvider));
        }

        public ConnectionTreeModel Load()
        {
            var connector = DatabaseConnectorFactory.DatabaseConnectorFromSettings();
            var dataProvider = new SqlDataProvider(connector);
            var metaDataRetriever = new SqlDatabaseMetaDataRetriever();
            var databaseVersionVerifier = new SqlDatabaseVersionVerifier(connector);
            var cryptoProvider = new LegacyRijndaelCryptographyProvider();

            var metaData = metaDataRetriever.GetDatabaseMetaData(connector) ?? 
                           HandleFirstRun(metaDataRetriever, connector);
            var decryptionKey = GetDecryptionKey(metaData);

            if (!decryptionKey.Any())
                throw new Exception("Could not load SQL connections");

            databaseVersionVerifier.VerifyDatabaseVersion(metaData.ConfVersion);
            var dataTable = dataProvider.Load();
            var deserializer = new DataTableDeserializer(cryptoProvider, decryptionKey.First());
            var connectionTree = deserializer.Deserialize(dataTable);
            ApplyLocalConnectionProperties(connectionTree.RootNodes.First(i => i is RootNodeInfo));
            return connectionTree;
        }

        private Optional<SecureString> GetDecryptionKey(SqlConnectionListMetaData metaData)
        {
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            var cipherText = metaData.Protected;
            var authenticator = new PasswordAuthenticator(cryptographyProvider, cipherText, AuthenticationRequestor);
            var authenticated =
                authenticator.Authenticate(new RootNodeInfo(RootNodeType.Connection).DefaultPassword
                                                                                    .ConvertToSecureString());

            if (authenticated)
                return authenticator.LastAuthenticatedPassword;
            return Optional<SecureString>.Empty;
        }

        private void ApplyLocalConnectionProperties(ContainerInfo rootNode)
        {
            var localPropertiesXml = _dataProvider.Load();
            var localConnectionProperties = _localConnectionPropertiesDeserializer.Deserialize(localPropertiesXml);

            rootNode
                .GetRecursiveChildList()
                .Join(localConnectionProperties,
                      con => con.ConstantID,
                      locals => locals.ConnectionId,
                      (con, locals) => new {Connection = con, LocalProperties = locals})
                .ForEach(x =>
                {
                    x.Connection.PleaseConnect = x.LocalProperties.Connected;
                    x.Connection.Favorite = x.LocalProperties.Favorite;
                    if (x.Connection is ContainerInfo container)
                        container.IsExpanded = x.LocalProperties.Expanded;
                });
        }

        private SqlConnectionListMetaData HandleFirstRun(SqlDatabaseMetaDataRetriever metaDataRetriever, IDatabaseConnector connector)
        {
	        metaDataRetriever.WriteDatabaseMetaData(new RootNodeInfo(RootNodeType.Connection), connector);
	        return metaDataRetriever.GetDatabaseMetaData(connector);
		}
    }
}