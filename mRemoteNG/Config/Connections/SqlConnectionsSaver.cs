using System;
using System.Collections.Generic;
using System.Data;
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
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Sql;

namespace mRemoteNG.Config.Connections
{
    [SupportedOSPlatform("windows")]
    public class SqlConnectionsSaver : ISaver<ConnectionTreeModel>
    {
        private readonly SaveFilter _saveFilter;
        private readonly ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> _localPropertiesSerializer;
        private readonly IDataProvider<string> _dataProvider;

        public SqlConnectionsSaver(SaveFilter saveFilter, ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> localPropertieSerializer, IDataProvider<string> localPropertiesDataProvider)
        {
            _saveFilter = saveFilter ?? throw new ArgumentNullException(nameof(saveFilter));
            _localPropertiesSerializer = localPropertieSerializer.ThrowIfNull(nameof(localPropertieSerializer));
            _dataProvider = localPropertiesDataProvider.ThrowIfNull(nameof(localPropertiesDataProvider));
        }

        public void Save(ConnectionTreeModel connectionTreeModel, string propertyNameTrigger = "")
        {
            var rootTreeNode = connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();

            UpdateLocalConnectionProperties(rootTreeNode);

            if (PropertyIsLocalOnly(propertyNameTrigger))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, $"Property {propertyNameTrigger} is local only. Not saving to database.");
                return;
            }

            if (SqlUserIsReadOnly())
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Trying to save connection tree but the SQL read only checkbox is checked, aborting!");
                return;
            }

            using (var dbConnector = DatabaseConnectorFactory.DatabaseConnectorFromSettings())
            {
                dbConnector.Connect();
                var databaseVersionVerifier = new SqlDatabaseVersionVerifier(dbConnector);
                var metaDataRetriever = new SqlDatabaseMetaDataRetriever();
                var metaData = metaDataRetriever.GetDatabaseMetaData(dbConnector);

                if (!databaseVersionVerifier.VerifyDatabaseVersion(metaData.ConfVersion))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ErrorConnectionListSaveFailed);
                    return;
                }

                metaDataRetriever.WriteDatabaseMetaData(rootTreeNode, dbConnector);
                UpdateConnectionsTable(rootTreeNode, dbConnector);
                UpdateUpdatesTable(dbConnector);

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
                   property == nameof(ContainerInfo.IsExpanded) ||
                   property == nameof(ContainerInfo.Favorite);
        }

        private void UpdateLocalConnectionProperties(ContainerInfo rootNode)
        {
            var a = rootNode.GetRecursiveChildList().Select(info => new LocalConnectionPropertiesModel
            {
                ConnectionId = info.ConstantID,
                Connected = info.OpenConnections.Count > 0,
                Expanded = info is ContainerInfo c && c.IsExpanded,
                Favorite = info.Favorite,
            });

            var serializedProperties = _localPropertiesSerializer.Serialize(a);
            _dataProvider.Save(serializedProperties);
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Saved local connection properties");
        }

        private void UpdateRootNodeTable(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector)
        {
            // TODO: use transaction, but method not used at all?
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

            var dbQuery = databaseConnector.DbCommand("TRUNCATE TABLE tblRoot");
            dbQuery.ExecuteNonQuery();

            if (rootTreeNode != null)
            {
                dbQuery =
                    databaseConnector.DbCommand(
                        "INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES('" +
                        MiscTools.PrepareValueForDB(rootTreeNode.Name) + "', 0, '" + strProtected + "','" +
                        ConnectionsFileInfo.ConnectionFileVersion + "')");
                dbQuery.ExecuteNonQuery();
            }
            else
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"UpdateRootNodeTable: rootTreeNode was null. Could not insert!");
            }
        }

        private void UpdateConnectionsTable(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector)
        {
            SqlDataProvider dataProvider = new SqlDataProvider(databaseConnector);
            DataTable currentDataTable = dataProvider.Load();

            LegacyRijndaelCryptographyProvider cryptoProvider = new LegacyRijndaelCryptographyProvider();
            DataTableSerializer serializer = new DataTableSerializer(_saveFilter, cryptoProvider, rootTreeNode.PasswordString.ConvertToSecureString());
            serializer.SetSourceDataTable(currentDataTable);

            DataTable dataTable = serializer.Serialize(rootTreeNode);
            
            dataProvider.Save(dataTable);
        }

        private void UpdateUpdatesTable(IDatabaseConnector databaseConnector)
        {
            // TODO: use transaction
            var dbQuery = databaseConnector.DbCommand("TRUNCATE TABLE tblUpdate");
            dbQuery.ExecuteNonQuery();
            dbQuery = databaseConnector.DbCommand("INSERT INTO tblUpdate (LastUpdate) VALUES('" + MiscTools.DBDate(DateTime.Now.ToUniversalTime()) + "')");
            dbQuery.ExecuteNonQuery();
        }

        private bool SqlUserIsReadOnly()
        {
            return Properties.OptionsDBsPage.Default.SQLReadOnly;
        }
    }
}