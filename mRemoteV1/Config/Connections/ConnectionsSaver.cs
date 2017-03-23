using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
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
			vRDvRE,
			SQL
		}
				
        #region Private Properties
		private XmlTextWriter _xmlTextWriter;
		private SecureString _password = Runtime.EncryptionKey;
        #endregion
				
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
				case Format.vRDvRE:
					SaveToVRE();
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
            var sqlConnector = new SqlDatabaseConnector();
            sqlConnector.Connect();

            if (!VerifyDatabaseVersion(sqlConnector))
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strErrorConnectionListSaveFailed);
				return;
			}

		    var rootTreeNode = Runtime.ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>().First();

		    UpdateRootNodeTable(rootTreeNode, sqlConnector);
		    UpdateConnectionsTable(rootTreeNode, sqlConnector);
		    UpdateUpdatesTable(sqlConnector);

            sqlConnector.Disconnect();
            sqlConnector.Dispose();
		}

        private bool VerifyDatabaseVersion(SqlDatabaseConnector sqlDatabaseConnector)
        {
            var isVerified = false;
            try
            {
                var databaseVersion = GetDatabaseVersion(sqlDatabaseConnector);
                SqlCommand sqlCommand;

                if (databaseVersion.Equals(new Version()))
                {
                    return true;
                }

                if (databaseVersion.CompareTo(new Version(2, 2)) == 0) // 2.2
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Upgrading database from version {databaseVersion} to version 2.3.");
                    sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD EnableFontSmoothing bit NOT NULL DEFAULT 0, EnableDesktopComposition bit NOT NULL DEFAULT 0, InheritEnableFontSmoothing bit NOT NULL DEFAULT 0, InheritEnableDesktopComposition bit NOT NULL DEFAULT 0;", sqlDatabaseConnector.SqlConnection);
                    sqlCommand.ExecuteNonQuery();
                    databaseVersion = new Version(2, 3);
                }

                if (databaseVersion.CompareTo(new Version(2, 3)) == 0) // 2.3
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Upgrading database from version {databaseVersion} to version 2.4.");
                    sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD UseCredSsp bit NOT NULL DEFAULT 1, InheritUseCredSsp bit NOT NULL DEFAULT 0;", sqlDatabaseConnector.SqlConnection);
                    sqlCommand.ExecuteNonQuery();
                    databaseVersion = new Version(2, 4);
                }

                if (databaseVersion.CompareTo(new Version(2, 4)) == 0) // 2.4
                {
                    Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"Upgrading database from version {databaseVersion} to version 2.5.");
                    sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD LoadBalanceInfo varchar (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, AutomaticResize bit NOT NULL DEFAULT 1, InheritLoadBalanceInfo bit NOT NULL DEFAULT 0, InheritAutomaticResize bit NOT NULL DEFAULT 0;", sqlDatabaseConnector.SqlConnection);
                    sqlCommand.ExecuteNonQuery();
                    databaseVersion = new Version(2, 5);
                }

                if (databaseVersion.CompareTo(new Version(2, 5)) == 0) // 2.5
                    isVerified = true;

                if (isVerified == false)
                    Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(Language.strErrorBadDatabaseVersion, databaseVersion, GeneralAppInfo.ProductName));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strErrorVerifyDatabaseVersionFailed, ex.Message));
            }
            return isVerified;
        }

	    private Version GetDatabaseVersion(SqlDatabaseConnector sqlDatabaseConnector)
	    {
	        Version databaseVersion;
            SqlDataReader sqlDataReader = null;
	        try
	        {
	            var sqlCommand = new SqlCommand("SELECT * FROM tblRoot", sqlDatabaseConnector.SqlConnection);
	            sqlDataReader = sqlCommand.ExecuteReader();
	            if (!sqlDataReader.HasRows)
                    return new Version(); // assume new empty database
                else
	            sqlDataReader.Read();
	            databaseVersion = new Version(Convert.ToString(sqlDataReader["confVersion"], CultureInfo.InvariantCulture));
	        }
	        catch (Exception ex)
	        {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Retrieving database version failed. {ex}");
                throw;
	        }
	        finally
	        {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                    sqlDataReader.Close();
	        }
	        return databaseVersion;
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
                var factory = new CryptographyProviderFactory();
                var cryptographyProvider = factory.CreateAeadCryptographyProvider(mRemoteNG.Settings.Default.EncryptionEngine, mRemoteNG.Settings.Default.EncryptionBlockCipherMode);
                cryptographyProvider.KeyDerivationIterations = mRemoteNG.Settings.Default.EncryptionKeyDerivationIterations;
			    var connectionNodeSerializer = new XmlConnectionNodeSerializer27(
                    cryptographyProvider, 
                    ConnectionTreeModel.RootNodes.OfType<RootNodeInfo>().First().PasswordString.ConvertToSecureString(),
                    SaveFilter);
                var xmlConnectionsSerializer = new XmlConnectionsSerializer(cryptographyProvider, connectionNodeSerializer)
				{
                    UseFullEncryption = mRemoteNG.Settings.Default.EncryptCompleteConnectionsFile
				};
			    var xml = xmlConnectionsSerializer.Serialize(ConnectionTreeModel);
						
                var fileDataProvider = new FileDataProviderWithBackup(ConnectionFileName);
                fileDataProvider.Save(xml);
            }
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionStackTrace("SaveToXml failed", ex);
            }
		}
				
		private void SaveToMremotengFormattedCsv()
		{
            var csvConnectionsSerializer = new CsvConnectionsSerializerMremotengFormat(SaveFilter);
		    var dataProvider = new FileDataProvider(ConnectionFileName);
            var csvContent = csvConnectionsSerializer.Serialize(ConnectionTreeModel);
            dataProvider.Save(csvContent);
        }
				
        #region vRD VRE
        // .VRE files are for ASG-Remote Desktop (prevously visionapp Remote Desktop)
		private void SaveToVRE()
		{
			if (Runtime.IsConnectionsFileLoaded == false)
			{
				return;
			}

		    var tN = (TreeNode)RootTreeNode.Clone();

		    var tNC = tN.Nodes;
					
			_xmlTextWriter = new XmlTextWriter(ConnectionFileName, Encoding.UTF8);
			_xmlTextWriter.Formatting = Formatting.Indented;
			_xmlTextWriter.Indentation = 4;
					
			_xmlTextWriter.WriteStartDocument();
					
			_xmlTextWriter.WriteStartElement("vRDConfig");
			_xmlTextWriter.WriteAttributeString("Version", "", "2.0");
					
			_xmlTextWriter.WriteStartElement("Connections");
			SaveNodeVRE(tNC);
			_xmlTextWriter.WriteEndElement();
					
			_xmlTextWriter.WriteEndElement();
			_xmlTextWriter.WriteEndDocument();
			_xmlTextWriter.Close();
		}
				
		private void SaveNodeVRE(TreeNodeCollection tNC)
		{
			foreach (TreeNode node in tNC)
			{
				if (((ConnectionInfo)node.Tag).GetTreeNodeType() == TreeNodeType.Connection)
				{
                    var curConI = (ConnectionInfo)node.Tag;
							
					if (curConI.Protocol == ProtocolType.RDP)
					{
						_xmlTextWriter.WriteStartElement("Connection");
						_xmlTextWriter.WriteAttributeString("Id", "", "");
								
						WriteVREitem(curConI);
								
						_xmlTextWriter.WriteEndElement();
					}
				}
				else
				{
					SaveNodeVRE(node.Nodes);
				}
			}
		}
				
		private void WriteVREitem(ConnectionInfo con)
		{
			//Name
			_xmlTextWriter.WriteStartElement("ConnectionName");
			_xmlTextWriter.WriteValue(con.Name);
			_xmlTextWriter.WriteEndElement();
					
			//Hostname
			_xmlTextWriter.WriteStartElement("ServerName");
			_xmlTextWriter.WriteValue(con.Hostname);
			_xmlTextWriter.WriteEndElement();
					
			//Mac Adress
			_xmlTextWriter.WriteStartElement("MACAddress");
			_xmlTextWriter.WriteValue(con.MacAddress);
			_xmlTextWriter.WriteEndElement();
					
			//Management Board URL
			_xmlTextWriter.WriteStartElement("MgmtBoardUrl");
			_xmlTextWriter.WriteValue("");
			_xmlTextWriter.WriteEndElement();
					
			//Description
			_xmlTextWriter.WriteStartElement("Description");
			_xmlTextWriter.WriteValue(con.Description);
			_xmlTextWriter.WriteEndElement();
					
			//Port
			_xmlTextWriter.WriteStartElement("Port");
			_xmlTextWriter.WriteValue(con.Port);
			_xmlTextWriter.WriteEndElement();
					
			//Console Session
			_xmlTextWriter.WriteStartElement("Console");
			_xmlTextWriter.WriteValue(con.UseConsoleSession);
			_xmlTextWriter.WriteEndElement();
					
			//Redirect Clipboard
			_xmlTextWriter.WriteStartElement("ClipBoard");
			_xmlTextWriter.WriteValue(true);
			_xmlTextWriter.WriteEndElement();
					
			//Redirect Printers
			_xmlTextWriter.WriteStartElement("Printer");
			_xmlTextWriter.WriteValue(con.RedirectPrinters);
			_xmlTextWriter.WriteEndElement();
					
			//Redirect Ports
			_xmlTextWriter.WriteStartElement("Serial");
			_xmlTextWriter.WriteValue(con.RedirectPorts);
			_xmlTextWriter.WriteEndElement();
					
			//Redirect Disks
			_xmlTextWriter.WriteStartElement("LocalDrives");
			_xmlTextWriter.WriteValue(con.RedirectDiskDrives);
			_xmlTextWriter.WriteEndElement();
					
			//Redirect Smartcards
			_xmlTextWriter.WriteStartElement("SmartCard");
			_xmlTextWriter.WriteValue(con.RedirectSmartCards);
			_xmlTextWriter.WriteEndElement();
					
			//Connection Place
			_xmlTextWriter.WriteStartElement("ConnectionPlace");
			_xmlTextWriter.WriteValue("2"); //----------------------------------------------------------
			_xmlTextWriter.WriteEndElement();
					
			//Smart Size
			_xmlTextWriter.WriteStartElement("AutoSize");
			_xmlTextWriter.WriteValue(con.Resolution == ProtocolRDP.RDPResolutions.SmartSize);
			_xmlTextWriter.WriteEndElement();
					
			//SeparateResolutionX
			_xmlTextWriter.WriteStartElement("SeparateResolutionX");
			_xmlTextWriter.WriteValue("1024");
			_xmlTextWriter.WriteEndElement();
					
			//SeparateResolutionY
			_xmlTextWriter.WriteStartElement("SeparateResolutionY");
			_xmlTextWriter.WriteValue("768");
			_xmlTextWriter.WriteEndElement();
					
			var resolution = ProtocolRDP.GetResolutionRectangle(con.Resolution);
			if (resolution.Width == 0)
			{
				resolution.Width = 1024;
			}
			if (resolution.Height == 0)
			{
				resolution.Height = 768;
			}
					
			//TabResolutionX
			_xmlTextWriter.WriteStartElement("TabResolutionX");
			_xmlTextWriter.WriteValue(resolution.Width);
			_xmlTextWriter.WriteEndElement();
					
			//TabResolutionY
			_xmlTextWriter.WriteStartElement("TabResolutionY");
			_xmlTextWriter.WriteValue(resolution.Height);
			_xmlTextWriter.WriteEndElement();
					
			//RDPColorDepth
			_xmlTextWriter.WriteStartElement("RDPColorDepth");
			_xmlTextWriter.WriteValue(con.Colors.ToString().Replace("Colors", "").Replace("Bit", ""));
			_xmlTextWriter.WriteEndElement();
					
			//Bitmap Caching
			_xmlTextWriter.WriteStartElement("BitmapCaching");
			_xmlTextWriter.WriteValue(con.CacheBitmaps);
			_xmlTextWriter.WriteEndElement();
					
			//Themes
			_xmlTextWriter.WriteStartElement("Themes");
			_xmlTextWriter.WriteValue(con.DisplayThemes);
			_xmlTextWriter.WriteEndElement();
					
			//Wallpaper
			_xmlTextWriter.WriteStartElement("Wallpaper");
			_xmlTextWriter.WriteValue(con.DisplayWallpaper);
			_xmlTextWriter.WriteEndElement();
		}
        #endregion
	}
}