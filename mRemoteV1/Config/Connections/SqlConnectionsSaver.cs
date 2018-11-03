using System;
using System.Globalization;
using System.Linq;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Versioning;
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
        private SecureString _password = Runtime.EncryptionKey;
        private readonly SaveFilter _saveFilter;

        public SqlConnectionsSaver(SaveFilter saveFilter)
        {
            if (saveFilter == null)
                throw new ArgumentNullException(nameof(saveFilter));
            _saveFilter = saveFilter;
        }

        public void Save(ConnectionTreeModel connectionTreeModel)
        {
            if (SqlUserIsReadOnly())
            {
                Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, "Trying to save connection tree but the SQL read only checkbox is checked, aborting!");
                return;
            }
                

            using (var dbConnector = DatabaseConnectorFactory.DatabaseConnectorFromSettings())
            {
                dbConnector.Connect();
                var databaseVersionVerifier = new SqlDatabaseVersionVerifier(dbConnector);

                if (!databaseVersionVerifier.VerifyDatabaseVersion())
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strErrorConnectionListSaveFailed);
                    return;
                }

                var rootTreeNode = connectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();

                UpdateRootNodeTable(rootTreeNode, dbConnector);
                UpdateConnectionsTable(rootTreeNode, dbConnector);
                UpdateUpdatesTable(dbConnector);
            }
        }

        private void UpdateRootNodeTable(RootNodeInfo rootTreeNode, IDatabaseConnector databaseConnector)
        {
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            string strProtected;
            if (rootTreeNode != null)
            {
                if (rootTreeNode.Password)
                {
                    _password = rootTreeNode.PasswordString.ConvertToSecureString();
                    strProtected = cryptographyProvider.Encrypt("ThisIsProtected", _password);
                }
                else
                {
                    strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", _password);
                }
            }
            else
            {
                strProtected = cryptographyProvider.Encrypt("ThisIsNotProtected", _password);
            }

            var dbQuery = databaseConnector.DbCommand("DELETE FROM tblRoot");
            dbQuery.ExecuteNonQuery();

            if (rootTreeNode != null)
            {
                dbQuery =
                    databaseConnector.DbCommand(
                        "INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES(\'" +
                        MiscTools.PrepareValueForDB(rootTreeNode.Name) + "\', 0, \'" + strProtected + "\'," +
                        ConnectionsFileInfo.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture) + ")");
                dbQuery.ExecuteNonQuery();
            }
            else
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"UpdateRootNodeTable: rootTreeNode was null. Could not insert!");
            }
        }

        private void UpdateConnectionsTable(ContainerInfo rootTreeNode, IDatabaseConnector databaseConnector)
        {
            var dbQuery = databaseConnector.DbCommand("DELETE FROM tblCons");
            dbQuery.ExecuteNonQuery();
            var serializer = new DataTableSerializer(_saveFilter);
            var dataTable = serializer.Serialize(rootTreeNode);
            var dataProvider = new SqlDataProvider(databaseConnector);
            dataProvider.Save(dataTable);
        }

        private void UpdateUpdatesTable(IDatabaseConnector databaseConnector)
        {
            var dbQuery = databaseConnector.DbCommand("DELETE FROM tblUpdate");
            dbQuery.ExecuteNonQuery();
            dbQuery = databaseConnector.DbCommand("INSERT INTO tblUpdate (LastUpdate) VALUES(\'" + MiscTools.DBDate(DateTime.Now) + "\')");
            dbQuery.ExecuteNonQuery();
        }

        private bool SqlUserIsReadOnly()
        {
            return mRemoteNG.Settings.Default.SQLReadOnly;

        }
    }
}
