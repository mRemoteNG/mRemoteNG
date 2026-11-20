using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Connections
{
    [SupportedOSPlatform("windows")]
    public class SqlConnectionsLoader(
        IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>> localConnectionPropertiesDeserializer,
        IDataProvider<string> dataProvider) : IConnectionsLoader
    {
        private readonly IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>> _localConnectionPropertiesDeserializer = localConnectionPropertiesDeserializer.ThrowIfNull(nameof(localConnectionPropertiesDeserializer));

        private readonly IDataProvider<string> _dataProvider = dataProvider.ThrowIfNull(nameof(dataProvider));

        private Func<Optional<SecureString>> AuthenticationRequestor { get; set; } = () => MiscTools.PasswordDialog("", false);

        public ConnectionTreeModel Load()
        {
            IDatabaseConnector connector = DatabaseConnectorFactory.DatabaseConnectorFromSettings();
            SqlDataProvider dataProvider = new(connector);
            SqlDatabaseMetaDataRetriever metaDataRetriever = new();
            SqlConnectionListMetaData metaData = metaDataRetriever.GetDatabaseMetaData(connector) ?? HandleFirstRun(metaDataRetriever, connector);

            // Without this the caller sees a bare NullReferenceException from GetDecryptionKey.
            if (metaData == null)
                throw new InvalidOperationException("Could not read the connection list metadata from the database");

            // A newer version may store the passwords in a form this one cannot read, and reading
            // them anyway would show them wrong.
            if (IsNewerThanSupported(metaData.ConfVersion))
            {
                string message = string.Format(Language.ErrorNewerDatabaseVersion, metaData.ConfVersion, SqlDatabaseVersionVerifier.SupportedVersion);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, message);
                throw new InvalidOperationException(message);
            }

            Optional<SecureString> decryptionKey = GetDecryptionKey(metaData);

            if (!decryptionKey.Any())
                throw new Exception("Could not load SQL connections");

            // Upgrading to 3.4 moves the stored passwords to a key derived with a new salt.
            SqlDatabaseVersionVerifier databaseVersionVerifier = new(connector, decryptionKey.First());
            if (databaseVersionVerifier.VerifyDatabaseVersion(metaData.ConfVersion) && metaData.KdfSalt == null)
                metaData = metaDataRetriever.GetDatabaseMetaData(connector) ?? metaData;

            System.Data.DataTable dataTable = dataProvider.Load();
            DataTableDeserializer deserializer = new(metaData.CryptographyProvider(), decryptionKey.First());
            ConnectionTreeModel connectionTree = deserializer.Deserialize(dataTable);
            ApplyLocalConnectionProperties(connectionTree.RootNodes.First(i => i is RootNodeInfo));
            return connectionTree;
        }

        internal static bool IsNewerThanSupported(Version databaseVersion) =>
            databaseVersion > SqlDatabaseVersionVerifier.SupportedVersion;

        private Optional<SecureString> GetDecryptionKey(SqlConnectionListMetaData metaData)
        {
            ICryptographyProvider cryptographyProvider = metaData.CryptographyProvider();
            string cipherText = metaData.Protected;
            PasswordAuthenticator authenticator = new(cryptographyProvider, cipherText, AuthenticationRequestor);
            bool authenticated = authenticator.Authenticate(new RootNodeInfo(RootNodeType.Connection).DefaultPassword.ConvertToSecureString());

            return authenticated ? authenticator.LastAuthenticatedPassword : Optional<SecureString>.Empty;
        }

        private void ApplyLocalConnectionProperties(ContainerInfo rootNode)
        {
            string localPropertiesXml = _dataProvider.Load();
            IEnumerable<LocalConnectionPropertiesModel> localConnectionProperties = _localConnectionPropertiesDeserializer.Deserialize(localPropertiesXml);

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
	        metaDataRetriever.WriteDatabaseMetaData(new RootNodeInfo(RootNodeType.Connection), connector, DerivedKeyCryptographyProvider.WithNewSalt());
	        return metaDataRetriever.GetDatabaseMetaData(connector);
		}
    }
}