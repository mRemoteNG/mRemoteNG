using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.MsSql;
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
        private readonly SaveFilter _saveFilter;
        private readonly ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> _localPropertiesSerializer;
        private readonly IDataProvider<string> _dataProvider;

        public SqlConnectionsSaver(
            SaveFilter saveFilter, 
            ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> localPropertieSerializer,
            IDataProvider<string> localPropertiesDataProvider)
        {
            if (saveFilter == null)
                throw new ArgumentNullException(nameof(saveFilter));
            _saveFilter = saveFilter;
            _localPropertiesSerializer = localPropertieSerializer.ThrowIfNull(nameof(localPropertieSerializer));
            _dataProvider = localPropertiesDataProvider.ThrowIfNull(nameof(localPropertiesDataProvider));
        }

        public void Save(ConnectionTreeModel connectionTreeModel, string propertyNameTrigger = "")
        {
            var rootTreeNode = connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();

            UpdateLocalConnectionProperties(rootTreeNode);

            if (PropertyIsLocalOnly(propertyNameTrigger))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, 
                    $"Property {propertyNameTrigger} is local only. Not saving to database.");
                return;
            }

            if (SqlUserIsReadOnly())
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Trying to save connection tree but the SQL read only checkbox is checked, aborting!");
                return;
            }

            using (var sqlConnector = DatabaseConnectorFactory.SqlDatabaseConnectorFromSettings())
            {
                sqlConnector.Connect();
                var databaseVersionVerifier = new SqlDatabaseVersionVerifier(sqlConnector);
                var metaDataRetriever = new SqlDatabaseMetaDataRetriever();
                var metaData = metaDataRetriever.GetDatabaseMetaData(sqlConnector);

                if (!databaseVersionVerifier.VerifyDatabaseVersion(metaData.ConfVersion))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strErrorConnectionListSaveFailed);
                    return;
                }

                metaDataRetriever.WriteDatabaseMetaData(rootTreeNode, sqlConnector);
                UpdateConnectionsTable(rootTreeNode, sqlConnector);
                UpdateUpdatesTable(sqlConnector);
            }

            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Saved connections to database");
        }

        /// <summary>
        /// Determines if a given property name should be only saved
        /// locally.
        /// </summary>
        /// <param name="property">
        /// The name of the property that triggered the save event
        /// </param>
        /// <returns></returns>
        private bool PropertyIsLocalOnly(string property)
        {
            return property == nameof(ConnectionInfo.OpenConnections) ||
                   property == nameof(ContainerInfo.IsExpanded);
        }

        private void UpdateLocalConnectionProperties(ContainerInfo rootNode)
        {
            var a = rootNode.GetRecursiveChildList().Select(info => new LocalConnectionPropertiesModel
            {
                ConnectionId = info.ConstantID,
                Connected = info.OpenConnections.Count > 0,
                Expanded = info is ContainerInfo c && c.IsExpanded
            });

            var serializedProperties = _localPropertiesSerializer.Serialize(a);
            _dataProvider.Save(serializedProperties);
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Saved local connection properties");
        }

        private void UpdateRootNodeTable(RootNodeInfo rootTreeNode, SqlDatabaseConnector sqlDatabaseConnector)
        {
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            string strProtected;
            if (rootTreeNode != null)
            {
                if (rootTreeNode.Password)
                {
                    var password = rootTreeNode.PasswordString.ConvertToSecureString();
                    strProtected = cryptographyProvider.Encrypt("ThisIsProtected", password);
                }
                else
                {
                    strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", Runtime.EncryptionKey);
                }
            }
            else
            {
                strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", Runtime.EncryptionKey);
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

        private void UpdateConnectionsTable(RootNodeInfo rootTreeNode, SqlDatabaseConnector sqlDatabaseConnector)
        {
            var cryptoProvider = new LegacyRijndaelCryptographyProvider();
            var serializer = new DataTableSerializer(_saveFilter, cryptoProvider, rootTreeNode.PasswordString.ConvertToSecureString());
            var dataTable = serializer.Serialize(rootTreeNode);
            var dataProvider = new SqlDataProvider(sqlDatabaseConnector);

            var sqlQuery = new SqlCommand("DELETE FROM tblCons", sqlDatabaseConnector.SqlConnection);
            sqlQuery.ExecuteNonQuery();
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
