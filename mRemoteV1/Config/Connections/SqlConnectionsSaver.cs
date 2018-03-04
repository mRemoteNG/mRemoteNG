using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Connections
{
    public class SqlConnectionsSaver : ISaver<ConnectionTreeModel>
    {
        private readonly ConnectionsService _connectionsService;
        private readonly SaveFilter _saveFilter;
        private readonly DatabaseConnectorFactory _databaseConnectorFactory;

        public SqlConnectionsSaver(SaveFilter saveFilter, ConnectionsService connectionsService, DatabaseConnectorFactory databaseConnectorFactory)
        {
            _saveFilter = saveFilter.ThrowIfNull(nameof(saveFilter));
            _databaseConnectorFactory = databaseConnectorFactory.ThrowIfNull(nameof(databaseConnectorFactory));
            _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
        }

        public void Save(ConnectionTreeModel connectionTreeModel)
        {
            if (SqlUserIsReadOnly())
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Trying to save connection tree but the SQL read only checkbox is checked, aborting!");
                return;
            }
                

            using (var sqlConnector = _databaseConnectorFactory.SqlDatabaseConnectorFromSettings())
            {
                sqlConnector.Connect();
                var databaseVersionVerifier = new SqlDatabaseVersionVerifier(sqlConnector);

                if (!databaseVersionVerifier.VerifyDatabaseVersion())
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strErrorConnectionListSaveFailed);
                    return;
                }

                var rootTreeNode = connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();

                UpdateRootNodeTable(rootTreeNode, sqlConnector);
                UpdateConnectionsTable(rootTreeNode, sqlConnector);
                UpdateUpdatesTable(sqlConnector);
            }
        }

        private void UpdateRootNodeTable(RootNodeInfo rootTreeNode, SqlDatabaseConnector sqlDatabaseConnector)
        {
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            string strProtected;
            if (rootTreeNode != null)
            {
                if (rootTreeNode.Password)
                {
                    _connectionsService.EncryptionKey = rootTreeNode.PasswordString.ConvertToSecureString();
                    strProtected = cryptographyProvider.Encrypt("ThisIsProtected", _connectionsService.EncryptionKey);
                }
                else
                {
                    strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", _connectionsService.EncryptionKey);
                }
            }
            else
            {
                strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", _connectionsService.EncryptionKey);
            }

            var sqlQuery = new SqlCommand("DELETE FROM tblRoot", sqlDatabaseConnector.SqlConnection);
            sqlQuery.ExecuteNonQuery();

            if (rootTreeNode != null)
            {
                sqlQuery =
                    new SqlCommand(
                        "INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES(\'" +
                        MiscTools.PrepareValueForDB(rootTreeNode.Name) + "\', 0, \'" + strProtected + "\'," +
                        ConnectionsFileInfo.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture) + ")",
                        sqlDatabaseConnector.SqlConnection);
                sqlQuery.ExecuteNonQuery();
            }
            else
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"UpdateRootNodeTable: rootTreeNode was null. Could not insert!");
            }
        }

        private void UpdateConnectionsTable(ContainerInfo rootTreeNode, SqlDatabaseConnector sqlDatabaseConnector)
        {
            var sqlQuery = new SqlCommand("DELETE FROM tblCons", sqlDatabaseConnector.SqlConnection);
            sqlQuery.ExecuteNonQuery();
            var serializer = new DataTableSerializer(_saveFilter);
            var dataTable = serializer.Serialize(rootTreeNode);
            var dataProvider = new SqlDataProvider(sqlDatabaseConnector);
            dataProvider.Save(dataTable);
        }

        private void UpdateUpdatesTable(SqlDatabaseConnector sqlDatabaseConnector)
        {
            var sqlQuery = new SqlCommand("DELETE FROM tblUpdate", sqlDatabaseConnector.SqlConnection);
            sqlQuery.ExecuteNonQuery();
            sqlQuery = new SqlCommand("INSERT INTO tblUpdate (LastUpdate) VALUES(\'" + MiscTools.DBDate(DateTime.Now) + "\')", sqlDatabaseConnector.SqlConnection);
            sqlQuery.ExecuteNonQuery();
        }

        private bool SqlUserIsReadOnly()
        {
            return mRemoteNG.Settings.Default.SQLReadOnly;

        }
    }
}
