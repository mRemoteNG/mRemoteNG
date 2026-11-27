using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Common;
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
    public class SqlConnectionsSaver(SaveFilter saveFilter, ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> localPropertieSerializer, IDataProvider<string> localPropertiesDataProvider,
        Func<IDatabaseConnector> connectorFactory = null) : ISaver<ConnectionTreeModel>
    {
        private readonly SaveFilter _saveFilter = saveFilter ?? throw new ArgumentNullException(nameof(saveFilter));
        private readonly ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> _localPropertiesSerializer = localPropertieSerializer.ThrowIfNull(nameof(localPropertieSerializer));
        private readonly IDataProvider<string> _dataProvider = localPropertiesDataProvider.ThrowIfNull(nameof(localPropertiesDataProvider));
        private readonly Func<IDatabaseConnector> _connectorFactory = connectorFactory ?? DatabaseConnectorFactory.DatabaseConnectorFromSettings;

        public void Save(ConnectionTreeModel connectionTreeModel, string propertyNameTrigger = "")
        {
            RootNodeInfo rootTreeNode = connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();

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

            using (IDatabaseConnector dbConnector = _connectorFactory())
            {
                dbConnector.Connect();
                SqlDatabaseVersionVerifier databaseVersionVerifier = new(dbConnector);
                SqlDatabaseMetaDataRetriever metaDataRetriever = new();
                SqlConnectionListMetaData metaData = metaDataRetriever.GetDatabaseMetaData(dbConnector);

                // A database that has no root row yet (a brand new one) reports no metadata.
                // Write it before the version check instead of dereferencing null.
                if (metaData == null)
                {
                    metaDataRetriever.WriteDatabaseMetaData(rootTreeNode, dbConnector);
                    metaData = metaDataRetriever.GetDatabaseMetaData(dbConnector);
                }

                if (metaData == null)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ErrorConnectionListSaveFailed);
                    return;
                }

                if (!databaseVersionVerifier.VerifyDatabaseVersion(metaData.ConfVersion))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ErrorConnectionListSaveFailed);
                    return;
                }

                if (connectionTreeModel.SqlTreeRevision == null)
                    throw new InvalidOperationException("SQL tree revision is unavailable; reload the shared tree before saving.");
                using DbTransaction transaction = dbConnector.DbConnection().BeginTransaction(System.Data.IsolationLevel.Serializable);
                SqlTreeRevisionStore revisionStore = new();
                long currentRevision = revisionStore.ReadRevision(dbConnector, transaction, lockForWrite: true);
                if (currentRevision != connectionTreeModel.SqlTreeRevision.Value)
                {
                    connectionTreeModel.HasSqlSaveConflict = true;
                    throw new InvalidOperationException("Shared SQL tree changed since it was loaded. The draft was retained.");
                }
                metaDataRetriever.WriteDatabaseMetaData(rootTreeNode, dbConnector, transaction);
                UpdateConnectionsTable(rootTreeNode, dbConnector, transaction);
                UpdateUpdatesTable(dbConnector, transaction);
                revisionStore.AdvanceRevision(dbConnector, transaction, currentRevision);
                transaction.Commit();
                connectionTreeModel.SqlTreeRevision = checked(currentRevision + 1);
                connectionTreeModel.HasSqlSaveConflict = false;

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
            IEnumerable<LocalConnectionPropertiesModel> a = rootNode.GetRecursiveChildList().Select(info => new LocalConnectionPropertiesModel
            {
                ConnectionId = info.ConstantID,
                Connected = info.OpenConnections.Count > 0,
                Expanded = info is ContainerInfo c && c.IsExpanded,
                Favorite = info.Favorite,
            });

            string serializedProperties = _localPropertiesSerializer.Serialize(a);
            _dataProvider.Save(serializedProperties);
            Runtime.MessageCollector.AddMessage(MessageClass.DebugMsg, "Saved local connection properties");
        }

        private void UpdateRootNodeTable(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector)
        {
            // TODO: use transaction, but method not used at all?
            LegacyRijndaelCryptographyProvider cryptographyProvider = new();
            string strProtected;
            if (rootTreeNode != null)
            {
                if (rootTreeNode.Password)
                {
                    System.Security.SecureString password = rootTreeNode.PasswordString.ConvertToSecureString();
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

            System.Data.Common.DbCommand dbQuery = databaseConnector.DbCommand("DELETE FROM tblRoot");
            dbQuery.ExecuteNonQuery();

            if (rootTreeNode != null)
            {
                dbQuery = databaseConnector.DbCommand(
                    "INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES(@Name, 0, @Protected, @Version)");
                DbParameter nameParam = dbQuery.CreateParameter();
                nameParam.ParameterName = "@Name";
                nameParam.Value = rootTreeNode.Name;
                DbParameter protectedParam = dbQuery.CreateParameter();
                protectedParam.ParameterName = "@Protected";
                protectedParam.Value = strProtected;
                DbParameter versionParam = dbQuery.CreateParameter();
                versionParam.ParameterName = "@Version";
                versionParam.Value = ConnectionsFileInfo.ConnectionFileVersion;
                dbQuery.Parameters.Add(nameParam);
                dbQuery.Parameters.Add(protectedParam);
                dbQuery.Parameters.Add(versionParam);
                dbQuery.ExecuteNonQuery();
            }
            else
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"UpdateRootNodeTable: rootTreeNode was null. Could not insert!");
            }
        }

        private void UpdateConnectionsTable(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector, DbTransaction transaction)
        {
            SqlDataProvider dataProvider = new(databaseConnector);
            DataTable currentDataTable = dataProvider.Load(transaction);

            LegacyRijndaelCryptographyProvider cryptoProvider = new();
            DataTableSerializer serializer = new(_saveFilter, cryptoProvider, rootTreeNode.PasswordString.ConvertToSecureString());
            serializer.SetSourceDataTable(currentDataTable);

            DataTable dataTable = serializer.Serialize(rootTreeNode);
            
            dataProvider.Save(dataTable, transaction);
        }

        private void UpdateUpdatesTable(IDatabaseConnector databaseConnector, DbTransaction transaction)
        {
            System.Data.Common.DbCommand dbQuery = databaseConnector.DbCommand("DELETE FROM tblUpdate");
            dbQuery.Transaction = transaction;
            dbQuery.ExecuteNonQuery();
            dbQuery = databaseConnector.DbCommand("INSERT INTO tblUpdate (LastUpdate) VALUES(@LastUpdate)");
            dbQuery.Transaction = transaction;
            
            DbParameter lastUpdateParam = dbQuery.CreateParameter();
            lastUpdateParam.ParameterName = "@LastUpdate";
            lastUpdateParam.Value = MiscTools.DBTimeStampNow();
            dbQuery.Parameters.Add(lastUpdateParam);
            
            dbQuery.ExecuteNonQuery();
        }

        private bool SqlUserIsReadOnly()
        {
            return Properties.OptionsDBsPage.Default.SQLReadOnly;
        }
    }
}
