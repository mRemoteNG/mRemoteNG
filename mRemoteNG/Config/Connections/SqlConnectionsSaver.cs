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
    public class SqlConnectionsSaver : ISaver<ConnectionTreeModel>
    {
        private readonly SaveFilter _saveFilter;
        private readonly ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> _localPropertiesSerializer;
        private readonly IDataProvider<string> _dataProvider;

        public SqlConnectionsSaver(SaveFilter saveFilter, ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> localPropertieSerializer, IDataProvider<string> localPropertiesDataProvider)
        {
            ArgumentNullException.ThrowIfNull(saveFilter);
            ArgumentNullException.ThrowIfNull(localPropertieSerializer);
            ArgumentNullException.ThrowIfNull(localPropertiesDataProvider);
            _saveFilter = saveFilter;
            _localPropertiesSerializer = localPropertieSerializer;
            _dataProvider = localPropertiesDataProvider;
        }

        public void Save(ConnectionTreeModel connectionTreeModel, string propertyNameTrigger = "")
        {
            RootNodeInfo? rootTreeNode = connectionTreeModel.RootNodes.OfType<RootNodeInfo>().FirstOrDefault();
            if (rootTreeNode == null)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SQL save aborted: connection tree has no root node.");
                return;
            }

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

            using (IDatabaseConnector dbConnector = DatabaseConnectorFactory.DatabaseConnectorFromSettings())
            {
                dbConnector.Connect();
                SqlDatabaseVersionVerifier databaseVersionVerifier = new(dbConnector);
                SqlDatabaseMetaDataRetriever metaDataRetriever = new();
                SqlConnectionListMetaData? metaData = metaDataRetriever.GetDatabaseMetaData(dbConnector);

                // metaData == null means a brand-new database whose schema was just
                // initialized (tblRoot exists but has no rows yet). In that case skip
                // the version check — WriteDatabaseMetaData will insert the tblRoot row
                // with the current version during this save. (#1883)
                if (metaData != null && !databaseVersionVerifier.VerifyDatabaseVersion(metaData.ConfVersion))
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ErrorConnectionListSaveFailed);
                    throw new InvalidOperationException(Language.ErrorConnectionListSaveFailed);
                }

                // Safety check: prevent truncating a non-empty database when the in-memory
                // tree is empty — this indicates a failed or incomplete load (#1351)
                int connectionCount = rootTreeNode.GetRecursiveChildList().Count();
                if (connectionCount == 0)
                {
                    SqlDataProvider checkProvider = new(dbConnector);
                    DataTable existingData = checkProvider.Load();
                    if (existingData.Rows.Count > 0)
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg,
                            $"SQL save aborted: in-memory connection tree is empty but database contains " +
                            $"{existingData.Rows.Count} connection(s). This may indicate the connection tree " +
                            "was not loaded properly. Database data has been preserved. (See issue #1351)");
                        return;
                    }
                }

                using DbTransaction transaction = dbConnector.DbConnection().BeginTransaction();
                try
                {
                    metaDataRetriever.WriteDatabaseMetaData(rootTreeNode, dbConnector, transaction);
                    UpdateConnectionsTable(rootTreeNode, dbConnector, transaction);
                    UpdateUpdatesTable(dbConnector, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Runtime.MessageCollector.AddExceptionStackTrace(Language.ErrorConnectionListSaveFailed, ex);
                    throw;
                }
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
        private static bool PropertyIsLocalOnly(string property)
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

        private static void UpdateRootNodeTable(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector)
        {
            UpdateRootNodeTable(rootTreeNode, databaseConnector, null);
        }

        private static void UpdateRootNodeTable(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector, DbTransaction? transaction)
        {
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

            bool mustDisposeTransaction = false;
            if (transaction == null)
            {
                transaction = databaseConnector.DbConnection().BeginTransaction();
                mustDisposeTransaction = true;
            }

            try
            {
                DbCommand dbQuery = databaseConnector.DbCommand("DELETE FROM tblRoot");
                dbQuery.Transaction = transaction;
                dbQuery.ExecuteNonQuery();

                if (rootTreeNode != null)
                {
                    dbQuery = databaseConnector.DbCommand(
                        "INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES(@Name, 0, @Protected, @Version)");
                    dbQuery.Transaction = transaction;
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

                if (mustDisposeTransaction)
                {
                    transaction.Commit();
                }
            }
            catch
            {
                if (mustDisposeTransaction)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (mustDisposeTransaction)
                {
                    transaction.Dispose();
                }
            }
        }

        private void UpdateConnectionsTable(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector, DbTransaction? transaction = null)
        {
            SqlDataProvider dataProvider = new(databaseConnector);
            DataTable currentDataTable = dataProvider.Load();

            LegacyRijndaelCryptographyProvider cryptoProvider = new();
            DataTableSerializer serializer = new(_saveFilter, cryptoProvider, rootTreeNode.PasswordString.ConvertToSecureString());
            serializer.SetSourceDataTable(currentDataTable);

            DataTable dataTable = serializer.Serialize(rootTreeNode);
            
            dataProvider.Save(dataTable, transaction);
        }

        private static void UpdateUpdatesTable(IDatabaseConnector databaseConnector, DbTransaction? transaction = null)
        {
            bool mustDisposeTransaction = false;
            if (transaction == null)
            {
                transaction = databaseConnector.DbConnection().BeginTransaction();
                mustDisposeTransaction = true;
            }

            try
            {
                DbCommand dbQuery = databaseConnector.DbCommand("DELETE FROM tblUpdate");
                dbQuery.Transaction = transaction;
                dbQuery.ExecuteNonQuery();

                dbQuery = databaseConnector.DbCommand("INSERT INTO tblUpdate (LastUpdate) VALUES(@LastUpdate)");
                dbQuery.Transaction = transaction;

                DbParameter lastUpdateParam = dbQuery.CreateParameter();
                lastUpdateParam.ParameterName = "@LastUpdate";
                lastUpdateParam.Value = MiscTools.DBTimeStampNow();
                dbQuery.Parameters.Add(lastUpdateParam);

                dbQuery.ExecuteNonQuery();
                
                if (mustDisposeTransaction)
                {
                    transaction.Commit();
                }
            }
            catch
            {
                if (mustDisposeTransaction)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (mustDisposeTransaction)
                {
                    transaction.Dispose();
                }
            }
        }

        private static bool SqlUserIsReadOnly()
        {
            return Properties.OptionsDBsPage.Default.SQLReadOnly;
        }
    }
}