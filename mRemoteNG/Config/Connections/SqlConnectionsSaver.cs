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
    public class SqlConnectionsSaver(SaveFilter saveFilter, ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> localPropertieSerializer, IDataProvider<string> localPropertiesDataProvider) : ISaver<ConnectionTreeModel>
    {
        private readonly SaveFilter _saveFilter = saveFilter ?? throw new ArgumentNullException(nameof(saveFilter));
        private readonly ISerializer<IEnumerable<LocalConnectionPropertiesModel>, string> _localPropertiesSerializer = localPropertieSerializer.ThrowIfNull(nameof(localPropertieSerializer));
        private readonly IDataProvider<string> _dataProvider = localPropertiesDataProvider.ThrowIfNull(nameof(localPropertiesDataProvider));

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

            using (IDatabaseConnector dbConnector = DatabaseConnectorFactory.DatabaseConnectorFromSettings())
            {
                dbConnector.Connect();
                SqlDatabaseVersionVerifier databaseVersionVerifier = new(dbConnector, rootTreeNode.PasswordString.ConvertToSecureString());
                SqlDatabaseMetaDataRetriever metaDataRetriever = new();
                SqlConnectionListMetaData metaData = metaDataRetriever.GetDatabaseMetaData(dbConnector);

                // A database that has no root row yet (a brand new one) reports no metadata.
                // Write it before the version check instead of dereferencing null.
                if (metaData == null)
                {
                    SqlDatabaseMetaDataRetriever.WriteDatabaseMetaData(rootTreeNode, dbConnector, DerivedKeyCryptographyProvider.WithNewSalt());
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

                // The upgrade to 3.4, when it just ran, gave the database its salt.
                if (metaData.KdfSalt == null)
                    metaData = metaDataRetriever.GetDatabaseMetaData(dbConnector);

                if (metaData?.CryptographyProvider() is not DerivedKeyCryptographyProvider cryptographyProvider)
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.ErrorConnectionListSaveFailed);
                    return;
                }

                SqlDatabaseMetaDataRetriever.WriteDatabaseMetaData(rootTreeNode, dbConnector, cryptographyProvider);
                UpdateConnectionsTable(rootTreeNode, dbConnector, cryptographyProvider);
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

        private void UpdateConnectionsTable(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector, ICryptographyProvider cryptoProvider)
        {
            SqlDataProvider dataProvider = new(databaseConnector);
            DataTable currentDataTable = dataProvider.Load();

            DataTableSerializer serializer = new(_saveFilter, cryptoProvider, rootTreeNode.PasswordString.ConvertToSecureString());
            serializer.SetSourceDataTable(currentDataTable);

            DataTable dataTable = serializer.Serialize(rootTreeNode);
            
            dataProvider.Save(dataTable);
        }

        private void UpdateUpdatesTable(IDatabaseConnector databaseConnector)
        {
            // TODO: use transaction
            System.Data.Common.DbCommand dbQuery = databaseConnector.DbCommand("TRUNCATE TABLE tblUpdate");
            dbQuery.ExecuteNonQuery();
            dbQuery = databaseConnector.DbCommand("INSERT INTO tblUpdate (LastUpdate) VALUES(@LastUpdate)");
            
            DbParameter lastUpdateParam = dbQuery.CreateParameter();
            lastUpdateParam.ParameterName = "@LastUpdate";
            // Use DBTimeStampNow() instead of DBDate() - the column is datetime type, not string
            // DBTimeStampNow() returns the database-specific .NET type: DateTime for MSSQL, MySqlDateTime for MySQL
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