using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.Versioning;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Forms;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace mRemoteNG.Config.Connections
{
	public class ConnectionsSaver
	{
		public enum Format
		{
			None,
			mRXML,
			mRCSV,
			SQL
		}
				
		private SecureString _password = Runtime.EncryptionKey;
				
        #region Public Properties
		public string SQLHost {get; set;}
		public string SQLDatabaseName {get; set;}
		public string SQLUsername {get; set;}
		public string SQLPassword {get; set;}
		
		public string ConnectionFileName {get; set;}
		public TreeNode RootTreeNode {get; set;}
		public Format SaveFormat {get; set;}
		public SaveFilter SaveFilter {get; set;}
        public ConnectionTreeModel ConnectionTreeModel { get; set; }
        #endregion
				
        #region Public Methods
		public void SaveConnections()
		{
			switch (SaveFormat)
			{
				case Format.SQL:
					SaveToSql();
					break;
				case Format.mRCSV:
			        SaveToMremotengFormattedCsv();
                    break;
				default:
					SaveToXml();
					FrmMain.Default.ConnectionsFileName = ConnectionFileName;
					break;
			}
			FrmMain.Default.AreWeUsingSqlServerForSavingConnections = SaveFormat == Format.SQL;
		}
        #endregion
				
        #region SQL
		private void SaveToSql()
		{
            var sqlConnector = DatabaseConnectorFactory.SqlDatabaseConnectorFromSettings();
            sqlConnector.Connect();
            var databaseVersionVerifier = new SqlDatabaseVersionVerifier(sqlConnector);

            if (!databaseVersionVerifier.VerifyDatabaseVersion())
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strErrorConnectionListSaveFailed);
				return;
			}

		    var rootTreeNode = Runtime.ConnectionsService.ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();

		    UpdateRootNodeTable(rootTreeNode, sqlConnector);
		    UpdateConnectionsTable(rootTreeNode, sqlConnector);
		    UpdateUpdatesTable(sqlConnector);

            sqlConnector.Disconnect();
            sqlConnector.Dispose();
		}

        private void UpdateRootNodeTable(RootNodeInfo rootTreeNode, SqlDatabaseConnector sqlDatabaseConnector)
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
            var serializer = new DataTableSerializer(SaveFilter);
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
        #endregion
				
		private void SaveToXml()
		{
			try
			{
                var cryptographyProvider = new CryptoProviderFactoryFromSettings().Build();
			    var connectionNodeSerializer = new XmlConnectionNodeSerializer26(
                    cryptographyProvider, 
                    ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>().First().PasswordString.ConvertToSecureString(),
                    SaveFilter);
                var xmlConnectionsSerializer = new XmlConnectionsSerializer(cryptographyProvider, connectionNodeSerializer)
				{
                    UseFullEncryption = mRemoteNG.Settings.Default.EncryptCompleteConnectionsFile
				};
			    var xml = xmlConnectionsSerializer.Serialize(ConnectionTreeModel);
						
                var fileDataProvider = new FileDataProviderWithRollingBackup(ConnectionFileName);
                fileDataProvider.Save(xml);
            }
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("SaveToXml failed", ex);
            }
		}
				
		private void SaveToMremotengFormattedCsv()
		{
            var csvConnectionsSerializer = new CsvConnectionsSerializerMremotengFormat(SaveFilter, Runtime.CredentialProviderCatalog);
		    var dataProvider = new FileDataProvider(ConnectionFileName);
            var csvContent = csvConnectionsSerializer.Serialize(ConnectionTreeModel);
            dataProvider.Save(csvContent);
        }
	}
}