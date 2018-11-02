using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.MsSql;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNG.Config.Connections
{
    public class SqlConnectionsLoader : IConnectionsLoader
    {
        private readonly IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>> _localConnectionPropertiesDeserializer;
        private readonly IDataProvider<string> _dataProvider;

        public SqlConnectionsLoader(
            IDeserializer<string, IEnumerable<LocalConnectionPropertiesModel>> localConnectionPropertiesDeserializer, 
            IDataProvider<string> dataProvider)
        {
            _localConnectionPropertiesDeserializer = localConnectionPropertiesDeserializer.ThrowIfNull(nameof(localConnectionPropertiesDeserializer));
            _dataProvider = dataProvider.ThrowIfNull(nameof(dataProvider));
        }

        public ConnectionTreeModel Load()
        {
            var connector = DatabaseConnectorFactory.SqlDatabaseConnectorFromSettings();
            var dataProvider = new SqlDataProvider(connector);
            var databaseVersionVerifier = new SqlDatabaseVersionVerifier(connector);
            databaseVersionVerifier.VerifyDatabaseVersion();
            var dataTable = dataProvider.Load();
            var deserializer = new DataTableDeserializer();
            var connectionTree = deserializer.Deserialize(dataTable);
            ApplyLocalConnectionProperties(connectionTree.RootNodes.First(i => i is RootNodeInfo));
            return connectionTree;
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
                    if (x.Connection is ContainerInfo container)
                        container.IsExpanded = x.LocalProperties.Expanded;
                });
        }
    }
}