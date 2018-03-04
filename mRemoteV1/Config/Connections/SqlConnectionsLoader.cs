using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Tools;
using mRemoteNG.Tree;

namespace mRemoteNG.Config.Connections
{
    public class SqlConnectionsLoader
    {
        private readonly DatabaseConnectorFactory _databaseConnectorFactory;

        public SqlConnectionsLoader(DatabaseConnectorFactory databaseConnectorFactory)
        {
            _databaseConnectorFactory = databaseConnectorFactory.ThrowIfNull(nameof(databaseConnectorFactory));
        }

        public ConnectionTreeModel Load()
        {
            var connector = _databaseConnectorFactory.SqlDatabaseConnectorFromSettings();
            var dataProvider = new SqlDataProvider(connector);
            var databaseVersionVerifier = new SqlDatabaseVersionVerifier(connector);
            databaseVersionVerifier.VerifyDatabaseVersion();
            var dataTable = dataProvider.Load();
            var deserializer = new DataTableDeserializer();
            return deserializer.Deserialize(dataTable);
        }
    }
}