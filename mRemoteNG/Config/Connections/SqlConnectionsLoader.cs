using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.Config.DatabaseConnectors;
using System.Data;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
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
        private readonly IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>> _localConnectionPropertiesDeserializer;
        private readonly IDataProvider<string> _localPropertiesDataProvider;
        private readonly IDatabaseConnector _databaseConnector;
        private readonly IDataProvider<DataTable> _sqlDataProvider;
        private readonly ISqlDatabaseMetaDataRetriever _sqlMetaDataRetriever;
        private readonly ISqlDatabaseVersionVerifier _sqlDatabaseVersionVerifier;
        private readonly ICryptographyProvider _cryptographyProvider;

        private Func<string, Optional<SecureString>> AuthenticationRequestor { get; }

        public SqlConnectionsLoader(
            IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>> localConnectionPropertiesDeserializer,
            IDataProvider<string> localPropertiesDataProvider,
            IDatabaseConnector databaseConnector,
            IDataProvider<DataTable> sqlDataProvider,
            ISqlDatabaseMetaDataRetriever sqlMetaDataRetriever,
            ISqlDatabaseVersionVerifier sqlDatabaseVersionVerifier,
            ICryptographyProvider cryptographyProvider,
            Func<string, Optional<SecureString>>? authenticationRequestor = null)
        {
            ArgumentNullException.ThrowIfNull(localConnectionPropertiesDeserializer);
            ArgumentNullException.ThrowIfNull(localPropertiesDataProvider);
            ArgumentNullException.ThrowIfNull(databaseConnector);
            ArgumentNullException.ThrowIfNull(sqlDataProvider);
            ArgumentNullException.ThrowIfNull(sqlMetaDataRetriever);
            ArgumentNullException.ThrowIfNull(sqlDatabaseVersionVerifier);
            ArgumentNullException.ThrowIfNull(cryptographyProvider);
            _localConnectionPropertiesDeserializer = localConnectionPropertiesDeserializer;
            _localPropertiesDataProvider = localPropertiesDataProvider;
            _databaseConnector = databaseConnector;
            _sqlDataProvider = sqlDataProvider;
            _sqlMetaDataRetriever = sqlMetaDataRetriever;
            _sqlDatabaseVersionVerifier = sqlDatabaseVersionVerifier;
            _cryptographyProvider = cryptographyProvider;
            AuthenticationRequestor = authenticationRequestor ?? ((filename) => MiscTools.PasswordDialog(filename, false));
        }

        public ConnectionTreeModel Load()
        {
            SqlConnectionListMetaData metaData = _sqlMetaDataRetriever.GetDatabaseMetaData(_databaseConnector) ?? HandleFirstRun(_sqlMetaDataRetriever, _databaseConnector);
            Optional<SecureString> decryptionKey = GetDecryptionKey(metaData);

            if (!decryptionKey.Any())
                throw new InvalidOperationException("Could not load SQL connections");

            _sqlDatabaseVersionVerifier.VerifyDatabaseVersion(metaData.ConfVersion);
            System.Data.DataTable dataTable = _sqlDataProvider.Load();
            DataTableDeserializer deserializer = new(_cryptographyProvider, decryptionKey.First());
            ConnectionTreeModel connectionTree = deserializer.Deserialize(dataTable);
            ContainerInfo? rootNode = connectionTree.RootNodes.FirstOrDefault(i => i is RootNodeInfo);
            if (rootNode != null)
                ApplyLocalConnectionProperties(rootNode);
            return connectionTree;
        }

        private Optional<SecureString> GetDecryptionKey(SqlConnectionListMetaData metaData)
        {
            string cipherText = metaData.Protected;

            // If Protected is empty, the database has no master password set.
            // Return the default password directly without authentication.
            if (string.IsNullOrEmpty(cipherText))
                return new RootNodeInfo(RootNodeType.Connection).DefaultPassword.ConvertToSecureString();

            PasswordAuthenticator authenticator = new(_cryptographyProvider, cipherText, () => AuthenticationRequestor(""));
            bool authenticated = authenticator.Authenticate(new RootNodeInfo(RootNodeType.Connection).DefaultPassword.ConvertToSecureString());

            return authenticated && authenticator.LastAuthenticatedPassword is { } password
                ? password
                : Optional<SecureString>.Empty;
        }

        private void ApplyLocalConnectionProperties(ContainerInfo rootNode)
        {
            string localPropertiesXml = _localPropertiesDataProvider.Load();
            IEnumerable<LocalConnectionPropertiesModel> localConnectionProperties = _localConnectionPropertiesDeserializer.Deserialize(localPropertiesXml);

            rootNode
                .GetRecursiveChildList()
                .Join(localConnectionProperties,
                      con => con.ConstantID,
                      locals => locals.ConnectionId,
                      (con, locals) => new {Connection = con, LocalProperties = locals},
                      StringComparer.Ordinal)
                .ForEach(x =>
                {
                    x.Connection.PleaseConnect = x.LocalProperties.Connected;
                    x.Connection.Favorite = x.LocalProperties.Favorite;
                    if (x.Connection is ContainerInfo container)
                        container.IsExpanded = x.LocalProperties.Expanded;
                });
        }

        private static SqlConnectionListMetaData HandleFirstRun(ISqlDatabaseMetaDataRetriever metaDataRetriever, IDatabaseConnector connector)
        {
	        metaDataRetriever.WriteDatabaseMetaData(new RootNodeInfo(RootNodeType.Connection), connector);
	        return metaDataRetriever.GetDatabaseMetaData(connector)!;
		}
    }
}