using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.App.Info;
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

namespace mRemoteNG.Config.Connections
{
	public class ConnectionsSaver
	{
        #region Public Enums
		public enum Format
		{
			None,
			mRXML,
			mRCSV,
			vRDvRE,
			vRDCSV,
			SQL
		}
        #endregion
				
        #region Private Properties
		private XmlTextWriter _xmlTextWriter;
		private SecureString _password = GeneralAppInfo.EncryptionKey;
				
		private SqlConnection _sqlConnection;
		private SqlCommand _sqlQuery;
				
		private int _currentNodeIndex;
		private string _parentConstantId = Convert.ToString(0);
        #endregion
				
        #region Public Properties
		public string SQLHost {get; set;}
		public string SQLDatabaseName {get; set;}
		public string SQLUsername {get; set;}
		public string SQLPassword {get; set;}
		
		public string ConnectionFileName {get; set;}
		public TreeNode RootTreeNode {get; set;}
		public bool Export {get; set;}
		public Format SaveFormat {get; set;}
		public Save SaveSecurity {get; set;}
		public ConnectionList ConnectionList {get; set;}
		public ContainerList ContainerList {get; set;}
        #endregion
				
        #region Public Methods
		public void SaveConnections()
		{
			switch (SaveFormat)
			{
				case Format.SQL:
					SaveToSQL();
					break;
				case Format.mRCSV:
					SaveTomRCSV();
					break;
				case Format.vRDvRE:
					SaveToVRE();
					break;
				case Format.vRDCSV:
					SaveTovRDCSV();
					break;
				default:
					SaveToXml();
					if (mRemoteNG.Settings.Default.EncryptCompleteConnectionsFile)
					{
						EncryptCompleteFile();
					}
					if (!Export)
					{
						frmMain.Default.ConnectionsFileName = ConnectionFileName;
					}
					break;
			}
			frmMain.Default.AreWeUsingSqlServerForSavingConnections = SaveFormat == Format.SQL;
		}
        #endregion
				
        #region SQL
		private bool VerifyDatabaseVersion(SqlConnection sqlConnection)
		{
			bool isVerified = false;
			SqlDataReader sqlDataReader = null;
		    try
			{
				SqlCommand sqlCommand = new SqlCommand("SELECT * FROM tblRoot", sqlConnection);
				sqlDataReader = sqlCommand.ExecuteReader();
				if (!sqlDataReader.HasRows)
				{
					return true; // assume new empty database
				}
				sqlDataReader.Read();
						
				var databaseVersion = new Version(Convert.ToString(sqlDataReader["confVersion"], CultureInfo.InvariantCulture));
						
				sqlDataReader.Close();
						
				if (databaseVersion.CompareTo(new Version(2, 2)) == 0) // 2.2
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format("Upgrading database from version {0} to version {1}.", databaseVersion, "2.3"));
					sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD EnableFontSmoothing bit NOT NULL DEFAULT 0, EnableDesktopComposition bit NOT NULL DEFAULT 0, InheritEnableFontSmoothing bit NOT NULL DEFAULT 0, InheritEnableDesktopComposition bit NOT NULL DEFAULT 0;", sqlConnection);
					sqlCommand.ExecuteNonQuery();
					databaseVersion = new Version(2, 3);
				}
						
				if (databaseVersion.CompareTo(new Version(2, 3)) == 0) // 2.3
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format("Upgrading database from version {0} to version {1}.", databaseVersion, "2.4"));
					sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD UseCredSsp bit NOT NULL DEFAULT 1, InheritUseCredSsp bit NOT NULL DEFAULT 0;", sqlConnection);
					sqlCommand.ExecuteNonQuery();
					databaseVersion = new Version(2, 4);
				}
						
				if (databaseVersion.CompareTo(new Version(2, 4)) == 0) // 2.4
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, string.Format("Upgrading database from version {0} to version {1}.", databaseVersion, "2.5"));
					sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD LoadBalanceInfo varchar (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, AutomaticResize bit NOT NULL DEFAULT 1, InheritLoadBalanceInfo bit NOT NULL DEFAULT 0, InheritAutomaticResize bit NOT NULL DEFAULT 0;", sqlConnection);
					sqlCommand.ExecuteNonQuery();
					databaseVersion = new Version(2, 5);
				}
						
				if (databaseVersion.CompareTo(new Version(2, 5)) == 0) // 2.5
				{
					isVerified = true;
				}
						
				if (isVerified == false)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, string.Format(Language.strErrorBadDatabaseVersion, databaseVersion, GeneralAppInfo.ProdName));
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strErrorVerifyDatabaseVersionFailed, ex.Message));
			}
			finally
			{
				if (sqlDataReader != null)
				{
					if (!sqlDataReader.IsClosed)
					{
						sqlDataReader.Close();
					}
				}
			}
			return isVerified;
		}
				
		private void SaveToSQL()
		{
			if (SQLUsername != "")
			{
				_sqlConnection = new SqlConnection("Data Source=" + SQLHost + ";Initial Catalog=" + SQLDatabaseName + ";User Id=" + SQLUsername + ";Password=" + SQLPassword);
			}
			else
			{
				_sqlConnection = new SqlConnection("Data Source=" + SQLHost + ";Initial Catalog=" + SQLDatabaseName + ";Integrated Security=True");
			}
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();

            _sqlConnection.Open();
					
			if (!VerifyDatabaseVersion(_sqlConnection))
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strErrorConnectionListSaveFailed);
				return ;
			}

		    var tN = (TreeNode)RootTreeNode.Clone();
					
			string strProtected;
			if (tN.Tag != null)
			{
				if (((RootNodeInfo) tN.Tag).Password)
				{
					_password = Convert.ToString(((RootNodeInfo) tN.Tag).PasswordString).ConvertToSecureString();
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
					
			_sqlQuery = new SqlCommand("DELETE FROM tblRoot", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();

            _sqlQuery = new SqlCommand("INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES(\'" + MiscTools.PrepareValueForDB(tN.Text) + "\', 0, \'" + strProtected + "\'," + ConnectionsFileInfo.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture) + ")", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();
					
			_sqlQuery = new SqlCommand("DELETE FROM tblCons", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();
					
			TreeNodeCollection tNC = tN.Nodes;

			SaveNodesSQL(tNC);
					
			_sqlQuery = new SqlCommand("DELETE FROM tblUpdate", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();
			_sqlQuery = new SqlCommand("INSERT INTO tblUpdate (LastUpdate) VALUES(\'" + MiscTools.DBDate(DateTime.Now) + "\')", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();
					
			_sqlConnection.Close();
		}
				
		private void SaveNodesSQL(TreeNodeCollection tnc)
		{
			foreach (TreeNode node in tnc)
			{
				_currentNodeIndex++;
						
				ConnectionInfo curConI;
				_sqlQuery = new SqlCommand("INSERT INTO tblCons (Name, Type, Expanded, Description, Icon, Panel, Username, " + "DomainName, Password, Hostname, Protocol, PuttySession, " + "Port, ConnectToConsole, RenderingEngine, ICAEncryptionStrength, RDPAuthenticationLevel, LoadBalanceInfo, Colors, Resolution, AutomaticResize, DisplayWallpaper, " + "DisplayThemes, EnableFontSmoothing, EnableDesktopComposition, CacheBitmaps, RedirectDiskDrives, RedirectPorts, " + "RedirectPrinters, RedirectSmartCards, RedirectSound, RedirectKeys, " + "Connected, PreExtApp, PostExtApp, MacAddress, UserField, ExtApp, VNCCompression, VNCEncoding, VNCAuthMode, " + "VNCProxyType, VNCProxyIP, VNCProxyPort, VNCProxyUsername, VNCProxyPassword, " + "VNCColors, VNCSmartSizeMode, VNCViewOnly, " + "RDGatewayUsageMethod, RDGatewayHostname, RDGatewayUseConnectionCredentials, RDGatewayUsername, RDGatewayPassword, RDGatewayDomain, " + "UseCredSsp, " + "InheritCacheBitmaps, InheritColors, " + "InheritDescription, InheritDisplayThemes, InheritDisplayWallpaper, InheritEnableFontSmoothing, InheritEnableDesktopComposition, InheritDomain, " + "InheritIcon, InheritPanel, InheritPassword, InheritPort, " + "InheritProtocol, InheritPuttySession, InheritRedirectDiskDrives, " + "InheritRedirectKeys, InheritRedirectPorts, InheritRedirectPrinters, " + "InheritRedirectSmartCards, InheritRedirectSound, InheritResolution, InheritAutomaticResize, " + "InheritUseConsoleSession, InheritRenderingEngine, InheritUsername, InheritICAEncryptionStrength, InheritRDPAuthenticationLevel, InheritLoadBalanceInfo, " + "InheritPreExtApp, InheritPostExtApp, InheritMacAddress, InheritUserField, InheritExtApp, InheritVNCCompression, InheritVNCEncoding, " + "InheritVNCAuthMode, InheritVNCProxyType, InheritVNCProxyIP, InheritVNCProxyPort, " + "InheritVNCProxyUsername, InheritVNCProxyPassword, InheritVNCColors, " + "InheritVNCSmartSizeMode, InheritVNCViewOnly, " + "InheritRDGatewayUsageMethod, InheritRDGatewayHostname, InheritRDGatewayUseConnectionCredentials, InheritRDGatewayUsername, InheritRDGatewayPassword, InheritRDGatewayDomain, "
				+ "InheritUseCredSsp, " + "PositionID, ParentID, ConstantID, LastChange)" + "VALUES (", _sqlConnection
				);
						
				if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Container)
				{
					//_xmlTextWriter.WriteStartElement("Node")
					_sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(node.Text) + "\',"; //Name
					_sqlQuery.CommandText += "\'" + ConnectionTreeNode.GetNodeType(node) + "\',"; //Type
				}
						
				if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Container) //container
				{
					_sqlQuery.CommandText += "\'" + ContainerList[node.Tag].IsExpanded + "\',"; //Expanded
					curConI = ContainerList[node.Tag];
					SaveConnectionFieldsSQL(curConI);
							
					_sqlQuery.CommandText = MiscTools.PrepareForDB(_sqlQuery.CommandText);
					_sqlQuery.ExecuteNonQuery();
					//_parentConstantId = _currentNodeIndex
					SaveNodesSQL(node.Nodes);
					//_xmlTextWriter.WriteEndElement()
				}
						
				if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Connection)
				{
					_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
					curConI = ConnectionList[node.Tag];
					SaveConnectionFieldsSQL(curConI);
					//_xmlTextWriter.WriteEndElement()
					_sqlQuery.CommandText = MiscTools.PrepareForDB(_sqlQuery.CommandText);
					_sqlQuery.ExecuteNonQuery();
				}
						
				//_parentConstantId = 0
			}
		}
				
		private void SaveConnectionFieldsSQL(ConnectionInfo curConI)
		{
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            ConnectionInfo with_1 = curConI;
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Description) + "\',";
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Icon) + "\',";
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Panel) + "\',";
					
			if (SaveSecurity.Username)
			{
                _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Username) + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			if (SaveSecurity.Domain)
			{
                _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Domain) + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			if (SaveSecurity.Password)
			{
                _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(cryptographyProvider.Encrypt(with_1.Password, _password)) + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}

            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Hostname) + "\',";
			_sqlQuery.CommandText += "\'" + with_1.Protocol + "\',";
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.PuttySession) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Port) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.UseConsoleSession) + "\',";
			_sqlQuery.CommandText += "\'" + with_1.RenderingEngine + "\',";
			_sqlQuery.CommandText += "\'" + with_1.ICAEncryptionStrength + "\',";
			_sqlQuery.CommandText += "\'" + with_1.RDPAuthenticationLevel + "\',";
			_sqlQuery.CommandText += "\'" + with_1.LoadBalanceInfo + "\',";
			_sqlQuery.CommandText += "\'" + with_1.Colors + "\',";
			_sqlQuery.CommandText += "\'" + with_1.Resolution + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.AutomaticResize) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.DisplayWallpaper) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.DisplayThemes) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.EnableFontSmoothing) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.EnableDesktopComposition) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.CacheBitmaps) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.RedirectDiskDrives) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.RedirectPorts) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.RedirectPrinters) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.RedirectSmartCards) + "\',";
			_sqlQuery.CommandText += "\'" + with_1.RedirectSound + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.RedirectKeys) + "\',";
					
			if (curConI.OpenConnections.Count > 0)
			{
				_sqlQuery.CommandText += 1 + ",";
			}
			else
			{
				_sqlQuery.CommandText += 0 + ",";
			}
					
			_sqlQuery.CommandText += "\'" + with_1.PreExtApp + "\',";
			_sqlQuery.CommandText += "\'" + with_1.PostExtApp + "\',";
			_sqlQuery.CommandText += "\'" + with_1.MacAddress + "\',";
			_sqlQuery.CommandText += "\'" + with_1.UserField + "\',";
			_sqlQuery.CommandText += "\'" + with_1.ExtApp + "\',";
					
			_sqlQuery.CommandText += "\'" + with_1.VNCCompression + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCEncoding + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCAuthMode + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCProxyType + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCProxyIP + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.VNCProxyPort) + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCProxyUsername + "\',";
			_sqlQuery.CommandText += "\'" + cryptographyProvider.Encrypt(with_1.VNCProxyPassword, _password) + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCColors + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCSmartSizeMode + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.VNCViewOnly) + "\',";
					
			_sqlQuery.CommandText += "\'" + with_1.RDGatewayUsageMethod + "\',";
			_sqlQuery.CommandText += "\'" + with_1.RDGatewayHostname + "\',";
			_sqlQuery.CommandText += "\'" + with_1.RDGatewayUseConnectionCredentials + "\',";
					
			if (SaveSecurity.Username)
			{
				_sqlQuery.CommandText += "\'" + with_1.RDGatewayUsername + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			if (SaveSecurity.Password)
			{
				_sqlQuery.CommandText += "\'" + cryptographyProvider.Encrypt(with_1.RDGatewayPassword, _password) + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			if (SaveSecurity.Domain)
			{
				_sqlQuery.CommandText += "\'" + with_1.RDGatewayDomain + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.UseCredSsp) + "\',";
					
			if (SaveSecurity.Inheritance)
			{
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.CacheBitmaps) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Colors) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Description) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.DisplayThemes) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.DisplayWallpaper) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.EnableFontSmoothing) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.EnableDesktopComposition) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Domain) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Icon) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Panel) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Password) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Port) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Protocol) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.PuttySession) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RedirectDiskDrives) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RedirectKeys) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RedirectPorts) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RedirectPrinters) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RedirectSmartCards) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RedirectSound) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Resolution) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.AutomaticResize) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.UseConsoleSession) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RenderingEngine) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.Username) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.ICAEncryptionStrength) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RDPAuthenticationLevel) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.LoadBalanceInfo) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.PreExtApp) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.PostExtApp) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.MacAddress) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.UserField) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.ExtApp) + "\',";
						
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCCompression) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCEncoding) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCAuthMode) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCProxyType) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCProxyIP) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCProxyPort) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCProxyUsername) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCProxyPassword) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCColors) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCSmartSizeMode) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.VNCViewOnly) + "\',";
						
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RDGatewayUsageMethod) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RDGatewayHostname) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RDGatewayUseConnectionCredentials) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RDGatewayUsername) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RDGatewayPassword) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.RDGatewayDomain) + "\',";
						
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inheritance.UseCredSsp) + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',"; // .AutomaticResize
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',"; // .LoadBalanceInfo
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
						
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
						
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',"; // .RDGatewayUsageMethod
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',"; // .RDGatewayHostname
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',"; // .RDGatewayUseConnectionCredentials
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',"; // .RDGatewayUsername
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',"; // .RDGatewayPassword
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',"; // .RDGatewayDomain
						
				_sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',"; // .UseCredSsp
			}
					
			with_1.PositionID = _currentNodeIndex;
					
			if (with_1.IsContainer == false)
			{
				if (with_1.Parent != null)
				{
					_parentConstantId = Convert.ToString(with_1.Parent.ConstantID);
				}
				else
				{
					_parentConstantId = Convert.ToString(0);
				}
			}
			else
			{
				if (with_1.Parent.Parent != null)
				{
					_parentConstantId = Convert.ToString(with_1.Parent.Parent.ConstantID);
				}
				else
				{
					_parentConstantId = Convert.ToString(0);
				}
			}
					
			_sqlQuery.CommandText += _currentNodeIndex + ",\'" + _parentConstantId + "\',\'" + with_1.ConstantID + "\',\'" + MiscTools.DBDate(DateTime.Now) + "\')";
		}
        #endregion
				
        #region XML
		private void EncryptCompleteFile()
		{
			StreamReader streamReader = new StreamReader(ConnectionFileName);
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();

            string fileContents;
			fileContents = streamReader.ReadToEnd();
			streamReader.Close();
					
			if (!string.IsNullOrEmpty(fileContents))
			{
				StreamWriter streamWriter = new StreamWriter(ConnectionFileName);
				streamWriter.Write(cryptographyProvider.Encrypt(fileContents, _password));
				streamWriter.Close();
			}
		}
				
		private void SaveToXml()
		{
			try
			{
				if (!Runtime.IsConnectionsFileLoaded)
				{
					return;
				}
                var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                TreeNode treeNode;
						
				if (ConnectionTreeNode.GetNodeType(RootTreeNode) == TreeNodeType.Root)
				{
					treeNode = (TreeNode)RootTreeNode.Clone();
				}
				else
				{
					treeNode = new TreeNode("mR|Export (" + MiscTools.DBDate(DateTime.Now) + ")");
					treeNode.Nodes.Add(Convert.ToString(RootTreeNode.Clone()));
				}
						
				string tempFileName = Path.GetTempFileName();
				_xmlTextWriter = new XmlTextWriter(tempFileName, Encoding.UTF8);
						
				_xmlTextWriter.Formatting = Formatting.Indented;
				_xmlTextWriter.Indentation = 4;
						
				_xmlTextWriter.WriteStartDocument();
						
				_xmlTextWriter.WriteStartElement("Connections"); // Do not localize
				_xmlTextWriter.WriteAttributeString("Name", "", treeNode.Text);
				_xmlTextWriter.WriteAttributeString("Export", "", Convert.ToString(Export));
						
				if (Export)
				{
					_xmlTextWriter.WriteAttributeString("Protected", "", cryptographyProvider.Encrypt("ThisIsNotProtected", _password));
				}
				else
				{
					if (((RootNodeInfo) treeNode.Tag).Password)
					{
						_password = Convert.ToString(((RootNodeInfo) treeNode.Tag).PasswordString).ConvertToSecureString();
						_xmlTextWriter.WriteAttributeString("Protected", "", cryptographyProvider.Encrypt("ThisIsProtected", _password));
					}
					else
					{
						_xmlTextWriter.WriteAttributeString("Protected", "", cryptographyProvider.Encrypt("ThisIsNotProtected", _password));
					}
				}
						
				_xmlTextWriter.WriteAttributeString("ConfVersion", "", ConnectionsFileInfo.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture));

			    var treeNodeCollection = treeNode.Nodes;
						
				SaveNode(treeNodeCollection);
						
				_xmlTextWriter.WriteEndElement();
				_xmlTextWriter.Close();
						
				if (File.Exists(ConnectionFileName))
				{
					if (Export)
					{
						File.Delete(ConnectionFileName);
					}
					else
					{
						string backupFileName = ConnectionFileName +".backup";
						File.Delete(backupFileName);
						File.Move(ConnectionFileName, backupFileName);
					}
				}
				File.Move(tempFileName, ConnectionFileName);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SaveToXml failed" + Environment.NewLine + ex.Message);
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SaveToXml failed" + Environment.NewLine + ex.StackTrace, true);
            }
		}
				
		private void SaveNode(TreeNodeCollection tNC)
		{
			try
			{
				foreach (TreeNode node in tNC)
				{
                    ConnectionInfo curConI;
							
					if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Container)
					{
						_xmlTextWriter.WriteStartElement("Node");
						_xmlTextWriter.WriteAttributeString("Name", "", node.Text);
						_xmlTextWriter.WriteAttributeString("Type", "", ConnectionTreeNode.GetNodeType(node).ToString());
					}
							
					if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Container) //container
					{
						_xmlTextWriter.WriteAttributeString("Expanded", "", Convert.ToString(ContainerList[node.Tag].TreeNode.IsExpanded));
						curConI = ContainerList[node.Tag];
						SaveConnectionFields(curConI);
						SaveNode(node.Nodes);
						_xmlTextWriter.WriteEndElement();
					}
							
					if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Connection)
					{
						curConI = ConnectionList[node.Tag];
						SaveConnectionFields(curConI);
						_xmlTextWriter.WriteEndElement();
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SaveNode failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SaveConnectionFields(ConnectionInfo curConI)
		{
			try
			{
                var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                _xmlTextWriter.WriteAttributeString("Descr", "", curConI.Description);
						
				_xmlTextWriter.WriteAttributeString("Icon", "", curConI.Icon);
						
				_xmlTextWriter.WriteAttributeString("Panel", "", curConI.Panel);
						
				if (SaveSecurity.Username)
				{
					_xmlTextWriter.WriteAttributeString("Username", "", curConI.Username);
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("Username", "", "");
				}
						
				if (SaveSecurity.Domain)
				{
					_xmlTextWriter.WriteAttributeString("Domain", "", curConI.Domain);
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("Domain", "", "");
				}
						
				if (SaveSecurity.Password)
				{
					_xmlTextWriter.WriteAttributeString("Password", "", cryptographyProvider.Encrypt(curConI.Password, _password));
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("Password", "", "");
				}
						
				_xmlTextWriter.WriteAttributeString("Hostname", "", curConI.Hostname);
						
				_xmlTextWriter.WriteAttributeString("Protocol", "", curConI.Protocol.ToString());
						
				_xmlTextWriter.WriteAttributeString("PuttySession", "", curConI.PuttySession);
						
				_xmlTextWriter.WriteAttributeString("Port", "", Convert.ToString(curConI.Port));
						
				_xmlTextWriter.WriteAttributeString("ConnectToConsole", "", Convert.ToString(curConI.UseConsoleSession));
						
				_xmlTextWriter.WriteAttributeString("UseCredSsp", "", Convert.ToString(curConI.UseCredSsp));
						
				_xmlTextWriter.WriteAttributeString("RenderingEngine", "", curConI.RenderingEngine.ToString());
						
				_xmlTextWriter.WriteAttributeString("ICAEncryptionStrength", "", curConI.ICAEncryptionStrength.ToString());
						
				_xmlTextWriter.WriteAttributeString("RDPAuthenticationLevel", "", curConI.RDPAuthenticationLevel.ToString());
						
				_xmlTextWriter.WriteAttributeString("LoadBalanceInfo", "", curConI.LoadBalanceInfo);
						
				_xmlTextWriter.WriteAttributeString("Colors", "", curConI.Colors.ToString());
						
				_xmlTextWriter.WriteAttributeString("Resolution", "", curConI.Resolution.ToString());
						
				_xmlTextWriter.WriteAttributeString("AutomaticResize", "", Convert.ToString(curConI.AutomaticResize));
						
				_xmlTextWriter.WriteAttributeString("DisplayWallpaper", "", Convert.ToString(curConI.DisplayWallpaper));
						
				_xmlTextWriter.WriteAttributeString("DisplayThemes", "", Convert.ToString(curConI.DisplayThemes));
						
				_xmlTextWriter.WriteAttributeString("EnableFontSmoothing", "", Convert.ToString(curConI.EnableFontSmoothing));
						
				_xmlTextWriter.WriteAttributeString("EnableDesktopComposition", "", Convert.ToString(curConI.EnableDesktopComposition));
						
				_xmlTextWriter.WriteAttributeString("CacheBitmaps", "", Convert.ToString(curConI.CacheBitmaps));
						
				_xmlTextWriter.WriteAttributeString("RedirectDiskDrives", "", Convert.ToString(curConI.RedirectDiskDrives));
						
				_xmlTextWriter.WriteAttributeString("RedirectPorts", "", Convert.ToString(curConI.RedirectPorts));
						
				_xmlTextWriter.WriteAttributeString("RedirectPrinters", "", Convert.ToString(curConI.RedirectPrinters));
						
				_xmlTextWriter.WriteAttributeString("RedirectSmartCards", "", Convert.ToString(curConI.RedirectSmartCards));
						
				_xmlTextWriter.WriteAttributeString("RedirectSound", "", curConI.RedirectSound.ToString());
						
				_xmlTextWriter.WriteAttributeString("RedirectKeys", "", Convert.ToString(curConI.RedirectKeys));
						
				if (curConI.OpenConnections.Count > 0)
				{
					_xmlTextWriter.WriteAttributeString("Connected", "", Convert.ToString(true));
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("Connected", "", Convert.ToString(false));
				}
						
				_xmlTextWriter.WriteAttributeString("PreExtApp", "", curConI.PreExtApp);
				_xmlTextWriter.WriteAttributeString("PostExtApp", "", curConI.PostExtApp);
				_xmlTextWriter.WriteAttributeString("MacAddress", "", curConI.MacAddress);
				_xmlTextWriter.WriteAttributeString("UserField", "", curConI.UserField);
				_xmlTextWriter.WriteAttributeString("ExtApp", "", curConI.ExtApp);
						
				_xmlTextWriter.WriteAttributeString("VNCCompression", "", curConI.VNCCompression.ToString());
				_xmlTextWriter.WriteAttributeString("VNCEncoding", "", curConI.VNCEncoding.ToString());
				_xmlTextWriter.WriteAttributeString("VNCAuthMode", "", curConI.VNCAuthMode.ToString());
				_xmlTextWriter.WriteAttributeString("VNCProxyType", "", curConI.VNCProxyType.ToString());
				_xmlTextWriter.WriteAttributeString("VNCProxyIP", "", curConI.VNCProxyIP);
				_xmlTextWriter.WriteAttributeString("VNCProxyPort", "", Convert.ToString(curConI.VNCProxyPort));
				_xmlTextWriter.WriteAttributeString("VNCProxyUsername", "", curConI.VNCProxyUsername);
				_xmlTextWriter.WriteAttributeString("VNCProxyPassword", "", cryptographyProvider.Encrypt(curConI.VNCProxyPassword, _password));
				_xmlTextWriter.WriteAttributeString("VNCColors", "", curConI.VNCColors.ToString());
				_xmlTextWriter.WriteAttributeString("VNCSmartSizeMode", "", curConI.VNCSmartSizeMode.ToString());
				_xmlTextWriter.WriteAttributeString("VNCViewOnly", "", Convert.ToString(curConI.VNCViewOnly));
						
				_xmlTextWriter.WriteAttributeString("RDGatewayUsageMethod", "", curConI.RDGatewayUsageMethod.ToString());
				_xmlTextWriter.WriteAttributeString("RDGatewayHostname", "", curConI.RDGatewayHostname);
						
				_xmlTextWriter.WriteAttributeString("RDGatewayUseConnectionCredentials", "", curConI.RDGatewayUseConnectionCredentials.ToString());
						
				if (SaveSecurity.Username)
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", curConI.RDGatewayUsername);
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", "");
				}
						
				if (SaveSecurity.Password)
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", cryptographyProvider.Encrypt(curConI.RDGatewayPassword, _password));
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", "");
				}
						
				if (SaveSecurity.Domain)
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", curConI.RDGatewayDomain);
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", "");
				}
						
				if (SaveSecurity.Inheritance)
				{
					_xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", Convert.ToString(curConI.Inheritance.CacheBitmaps));
					_xmlTextWriter.WriteAttributeString("InheritColors", "", Convert.ToString(curConI.Inheritance.Colors));
					_xmlTextWriter.WriteAttributeString("InheritDescription", "", Convert.ToString(curConI.Inheritance.Description));
					_xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", Convert.ToString(curConI.Inheritance.DisplayThemes));
					_xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", Convert.ToString(curConI.Inheritance.DisplayWallpaper));
					_xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", Convert.ToString(curConI.Inheritance.EnableFontSmoothing));
					_xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", Convert.ToString(curConI.Inheritance.EnableDesktopComposition));
					_xmlTextWriter.WriteAttributeString("InheritDomain", "", Convert.ToString(curConI.Inheritance.Domain));
					_xmlTextWriter.WriteAttributeString("InheritIcon", "", Convert.ToString(curConI.Inheritance.Icon));
					_xmlTextWriter.WriteAttributeString("InheritPanel", "", Convert.ToString(curConI.Inheritance.Panel));
					_xmlTextWriter.WriteAttributeString("InheritPassword", "", Convert.ToString(curConI.Inheritance.Password));
					_xmlTextWriter.WriteAttributeString("InheritPort", "", Convert.ToString(curConI.Inheritance.Port));
					_xmlTextWriter.WriteAttributeString("InheritProtocol", "", Convert.ToString(curConI.Inheritance.Protocol));
					_xmlTextWriter.WriteAttributeString("InheritPuttySession", "", Convert.ToString(curConI.Inheritance.PuttySession));
					_xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", Convert.ToString(curConI.Inheritance.RedirectDiskDrives));
					_xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", Convert.ToString(curConI.Inheritance.RedirectKeys));
					_xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", Convert.ToString(curConI.Inheritance.RedirectPorts));
					_xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", Convert.ToString(curConI.Inheritance.RedirectPrinters));
					_xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", Convert.ToString(curConI.Inheritance.RedirectSmartCards));
					_xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", Convert.ToString(curConI.Inheritance.RedirectSound));
					_xmlTextWriter.WriteAttributeString("InheritResolution", "", Convert.ToString(curConI.Inheritance.Resolution));
					_xmlTextWriter.WriteAttributeString("InheritAutomaticResize", "", Convert.ToString(curConI.Inheritance.AutomaticResize));
					_xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", Convert.ToString(curConI.Inheritance.UseConsoleSession));
					_xmlTextWriter.WriteAttributeString("InheritUseCredSsp", "", Convert.ToString(curConI.Inheritance.UseCredSsp));
					_xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", Convert.ToString(curConI.Inheritance.RenderingEngine));
					_xmlTextWriter.WriteAttributeString("InheritUsername", "", Convert.ToString(curConI.Inheritance.Username));
					_xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", Convert.ToString(curConI.Inheritance.ICAEncryptionStrength));
					_xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", Convert.ToString(curConI.Inheritance.RDPAuthenticationLevel));
					_xmlTextWriter.WriteAttributeString("InheritLoadBalanceInfo", "", Convert.ToString(curConI.Inheritance.LoadBalanceInfo));
					_xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", Convert.ToString(curConI.Inheritance.PreExtApp));
					_xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", Convert.ToString(curConI.Inheritance.PostExtApp));
					_xmlTextWriter.WriteAttributeString("InheritMacAddress", "", Convert.ToString(curConI.Inheritance.MacAddress));
					_xmlTextWriter.WriteAttributeString("InheritUserField", "", Convert.ToString(curConI.Inheritance.UserField));
					_xmlTextWriter.WriteAttributeString("InheritExtApp", "", Convert.ToString(curConI.Inheritance.ExtApp));
					_xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", Convert.ToString(curConI.Inheritance.VNCCompression));
					_xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", Convert.ToString(curConI.Inheritance.VNCEncoding));
					_xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", Convert.ToString(curConI.Inheritance.VNCAuthMode));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", Convert.ToString(curConI.Inheritance.VNCProxyType));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", Convert.ToString(curConI.Inheritance.VNCProxyIP));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", Convert.ToString(curConI.Inheritance.VNCProxyPort));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", Convert.ToString(curConI.Inheritance.VNCProxyUsername));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", Convert.ToString(curConI.Inheritance.VNCProxyPassword));
					_xmlTextWriter.WriteAttributeString("InheritVNCColors", "", Convert.ToString(curConI.Inheritance.VNCColors));
					_xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", Convert.ToString(curConI.Inheritance.VNCSmartSizeMode));
					_xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", Convert.ToString(curConI.Inheritance.VNCViewOnly));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUsageMethod", "", Convert.ToString(curConI.Inheritance.RDGatewayUsageMethod));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", Convert.ToString(curConI.Inheritance.RDGatewayHostname));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", Convert.ToString(curConI.Inheritance.RDGatewayUseConnectionCredentials));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", Convert.ToString(curConI.Inheritance.RDGatewayUsername));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", Convert.ToString(curConI.Inheritance.RDGatewayPassword));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", Convert.ToString(curConI.Inheritance.RDGatewayDomain));
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritColors", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritDescription", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritDomain", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritIcon", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPanel", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPassword", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPort", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritProtocol", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPuttySession", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritResolution", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritAutomaticResize", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritUseCredSsp", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritUsername", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritLoadBalanceInfo", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritMacAddress", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritUserField", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritExtApp", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCColors", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", Convert.ToString(false));
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "SaveConnectionFields failed" + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region CSV
		private StreamWriter csvWr;
				
		private void SaveTomRCSV()
		{
			if (Runtime.IsConnectionsFileLoaded == false)
			{
				return;
			}

		    var tN = (TreeNode)RootTreeNode.Clone();

		    var tNC = tN.Nodes;
					
			csvWr = new StreamWriter(ConnectionFileName);
					
					
			string csvLn = string.Empty;
					
			csvLn += "Name;Folder;Description;Icon;Panel;";
					
			if (SaveSecurity.Username)
			{
				csvLn += "Username;";
			}
					
			if (SaveSecurity.Password)
			{
				csvLn += "Password;";
			}
					
			if (SaveSecurity.Domain)
			{
				csvLn += "Domain;";
			}
					
			csvLn += "Hostname;Protocol;PuttySession;Port;ConnectToConsole;UseCredSsp;RenderingEngine;ICAEncryptionStrength;RDPAuthenticationLevel;LoadBalanceInfo;Colors;Resolution;AutomaticResize;DisplayWallpaper;DisplayThemes;EnableFontSmoothing;EnableDesktopComposition;CacheBitmaps;RedirectDiskDrives;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;RedirectKeys;PreExtApp;PostExtApp;MacAddress;UserField;ExtApp;VNCCompression;VNCEncoding;VNCAuthMode;VNCProxyType;VNCProxyIP;VNCProxyPort;VNCProxyUsername;VNCProxyPassword;VNCColors;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;";
					
			if (SaveSecurity.Inheritance)
			{
				csvLn += "InheritCacheBitmaps;InheritColors;InheritDescription;InheritDisplayThemes;InheritDisplayWallpaper;InheritEnableFontSmoothing;InheritEnableDesktopComposition;InheritDomain;InheritIcon;InheritPanel;InheritPassword;InheritPort;InheritProtocol;InheritPuttySession;InheritRedirectDiskDrives;InheritRedirectKeys;InheritRedirectPorts;InheritRedirectPrinters;InheritRedirectSmartCards;InheritRedirectSound;InheritResolution;InheritAutomaticResize;InheritUseConsoleSession;InheritUseCredSsp;InheritRenderingEngine;InheritUsername;InheritICAEncryptionStrength;InheritRDPAuthenticationLevel;InheritLoadBalanceInfo;InheritPreExtApp;InheritPostExtApp;InheritMacAddress;InheritUserField;InheritExtApp;InheritVNCCompression;InheritVNCEncoding;InheritVNCAuthMode;InheritVNCProxyType;InheritVNCProxyIP;InheritVNCProxyPort;InheritVNCProxyUsername;InheritVNCProxyPassword;InheritVNCColors;InheritVNCSmartSizeMode;InheritVNCViewOnly;InheritRDGatewayUsageMethod;InheritRDGatewayHostname;InheritRDGatewayUseConnectionCredentials;InheritRDGatewayUsername;InheritRDGatewayPassword;InheritRDGatewayDomain";
			}
					
			csvWr.WriteLine(csvLn);
					
			SaveNodemRCSV(tNC);
					
			csvWr.Close();
		}
				
		private void SaveNodemRCSV(TreeNodeCollection tNC)
		{
			foreach (TreeNode node in tNC)
			{
				if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Connection)
				{
                    ConnectionInfo curConI = (ConnectionInfo)node.Tag;
							
					WritemRCSVLine(curConI);
				}
				else if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Container)
				{
					SaveNodemRCSV(node.Nodes);
				}
			}
		}
				
		private void WritemRCSVLine(ConnectionInfo con)
		{
			string nodePath = con.TreeNode.FullPath;
					
			int firstSlash = nodePath.IndexOf("\\", StringComparison.Ordinal);
			nodePath = nodePath.Remove(0, firstSlash + 1);
			int lastSlash = nodePath.LastIndexOf("\\", StringComparison.Ordinal);
					
			if (lastSlash > 0)
			{
				nodePath = nodePath.Remove(lastSlash);
			}
			else
			{
				nodePath = "";
			}
					
			string csvLn = string.Empty;
					
			csvLn += con.Name + ";" + nodePath + ";" + con.Description + ";" + con.Icon + ";" + con.Panel + ";";
					
			if (SaveSecurity.Username)
			{
				csvLn += con.Username + ";";
			}
					
			if (SaveSecurity.Password)
			{
				csvLn += con.Password + ";";
			}
					
			if (SaveSecurity.Domain)
			{
				csvLn += con.Domain + ";";
			}
					
			csvLn += con.Hostname + ";" + con.Protocol + ";" + con.PuttySession + ";" + Convert.ToString(con.Port) + ";" + Convert.ToString(con.UseConsoleSession) + ";" + Convert.ToString(con.UseCredSsp) + ";" + con.RenderingEngine + ";" + con.ICAEncryptionStrength + ";" + con.RDPAuthenticationLevel + ";" + con.LoadBalanceInfo + ";" + con.Colors + ";" + con.Resolution + ";" + Convert.ToString(con.AutomaticResize) + ";" + Convert.ToString(con.DisplayWallpaper) + ";" + Convert.ToString(con.DisplayThemes) + ";" + Convert.ToString(con.EnableFontSmoothing) + ";" + Convert.ToString(con.EnableDesktopComposition) + ";" + Convert.ToString(con.CacheBitmaps) + ";" + Convert.ToString(con.RedirectDiskDrives) + ";" + Convert.ToString(con.RedirectPorts) + ";" + Convert.ToString(con.RedirectPrinters) + ";" + Convert.ToString(con.RedirectSmartCards) + ";" + con.RedirectSound + ";" + Convert.ToString(con.RedirectKeys) + ";" + con.PreExtApp + ";" + con.PostExtApp + ";" + con.MacAddress + ";" + con.UserField + ";" + con.ExtApp + ";" + con.VNCCompression + ";" + con.VNCEncoding + ";" + con.VNCAuthMode + ";" + con.VNCProxyType + ";" + con.VNCProxyIP + ";" + Convert.ToString(con.VNCProxyPort) + ";" + con.VNCProxyUsername + ";" + con.VNCProxyPassword + ";" + con.VNCColors + ";" + con.VNCSmartSizeMode + ";" + Convert.ToString(con.VNCViewOnly) + ";";
					
			if (SaveSecurity.Inheritance)
			{
				csvLn += con.Inheritance.CacheBitmaps + ";" + Convert.ToString(con.Inheritance.Colors) + ";" + Convert.ToString(con.Inheritance.Description) + ";" + Convert.ToString(con.Inheritance.DisplayThemes) + ";" + Convert.ToString(con.Inheritance.DisplayWallpaper) + ";" + Convert.ToString(con.Inheritance.EnableFontSmoothing) + ";" + Convert.ToString(con.Inheritance.EnableDesktopComposition) + ";" + Convert.ToString(con.Inheritance.Domain) + ";" + Convert.ToString(con.Inheritance.Icon) + ";" + Convert.ToString(con.Inheritance.Panel) + ";" + Convert.ToString(con.Inheritance.Password) + ";" + Convert.ToString(con.Inheritance.Port) + ";" + Convert.ToString(con.Inheritance.Protocol) + ";" + Convert.ToString(con.Inheritance.PuttySession) + ";" + Convert.ToString(con.Inheritance.RedirectDiskDrives) + ";" + Convert.ToString(con.Inheritance.RedirectKeys) + ";" + Convert.ToString(con.Inheritance.RedirectPorts) + ";" + Convert.ToString(con.Inheritance.RedirectPrinters) + ";" + Convert.ToString(con.Inheritance.RedirectSmartCards) + ";" + Convert.ToString(con.Inheritance.RedirectSound) + ";" + Convert.ToString(con.Inheritance.Resolution) + ";" + Convert.ToString(con.Inheritance.AutomaticResize) + ";" + Convert.ToString(con.Inheritance.UseConsoleSession) + ";" + Convert.ToString(con.Inheritance.UseCredSsp) + ";" + Convert.ToString(con.Inheritance.RenderingEngine) + ";" + Convert.ToString(con.Inheritance.Username) + ";" + Convert.ToString(con.Inheritance.ICAEncryptionStrength) + ";" + Convert.ToString(con.Inheritance.RDPAuthenticationLevel) + ";" + Convert.ToString(con.Inheritance.LoadBalanceInfo) + ";" + Convert.ToString(con.Inheritance.PreExtApp) + ";" + Convert.ToString(con.Inheritance.PostExtApp) + ";" + Convert.ToString(con.Inheritance.MacAddress) + ";" + Convert.ToString(con.Inheritance.UserField) + ";" + Convert.ToString(con.Inheritance.ExtApp) + ";" + Convert.ToString(con.Inheritance.VNCCompression) + ";"
				+ Convert.ToString(con.Inheritance.VNCEncoding) + ";" + Convert.ToString(con.Inheritance.VNCAuthMode) + ";" + Convert.ToString(con.Inheritance.VNCProxyType) + ";" + Convert.ToString(con.Inheritance.VNCProxyIP) + ";" + Convert.ToString(con.Inheritance.VNCProxyPort) + ";" + Convert.ToString(con.Inheritance.VNCProxyUsername) + ";" + Convert.ToString(con.Inheritance.VNCProxyPassword) + ";" + Convert.ToString(con.Inheritance.VNCColors) + ";" + Convert.ToString(con.Inheritance.VNCSmartSizeMode) + ";" + Convert.ToString(con.Inheritance.VNCViewOnly);
			}
					
			csvWr.WriteLine(csvLn);
		}
        #endregion
				
        #region vRD CSV
		private void SaveTovRDCSV()
		{
			if (Runtime.IsConnectionsFileLoaded == false)
			{
				return;
			}

		    var tN = (TreeNode)RootTreeNode.Clone();

		    var tNC = tN.Nodes;
					
			csvWr = new StreamWriter(ConnectionFileName);
					
			SaveNodevRDCSV(tNC);
					
			csvWr.Close();
		}
				
		private void SaveNodevRDCSV(TreeNodeCollection tNC)
		{
			foreach (TreeNode node in tNC)
			{
				if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Connection)
				{
                    ConnectionInfo curConI = (ConnectionInfo)node.Tag;
							
					if (curConI.Protocol == ProtocolType.RDP)
					{
						WritevRDCSVLine(curConI);
					}
				}
				else if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Container)
				{
					SaveNodevRDCSV(node.Nodes);
				}
			}
		}
				
		private void WritevRDCSVLine(ConnectionInfo con)
		{
			string nodePath = con.TreeNode.FullPath;
					
			int firstSlash = nodePath.IndexOf("\\", StringComparison.Ordinal);
			nodePath = nodePath.Remove(0, firstSlash + 1);
			int lastSlash = nodePath.LastIndexOf("\\", StringComparison.Ordinal);
					
			if (lastSlash > 0)
			{
				nodePath = nodePath.Remove(lastSlash);
			}
			else
			{
				nodePath = "";
			}
					
			csvWr.WriteLine(con.Name + ";" + con.Hostname + ";" + con.MacAddress + ";;" + Convert.ToString(con.Port) + ";" + Convert.ToString(con.UseConsoleSession) + ";" + nodePath);
		}
        #endregion
				
        #region vRD VRE
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
				if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Connection)
				{
                    ConnectionInfo curConI = (ConnectionInfo)node.Tag;
							
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
					
			Rectangle resolution = ProtocolRDP.GetResolutionRectangle(con.Resolution);
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