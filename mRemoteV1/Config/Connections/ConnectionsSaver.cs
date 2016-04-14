using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;


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
		private string _password = "mR3m";
				
		private SqlConnection _sqlConnection;
		private SqlCommand _sqlQuery;
				
		private int _currentNodeIndex = 0;
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
		public Security.Save SaveSecurity {get; set;}
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
					if (My.Settings.Default.EncryptCompleteConnectionsFile)
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
			System.Version databaseVersion = null;
			try
			{
				SqlCommand sqlCommand = new SqlCommand("SELECT * FROM tblRoot", sqlConnection);
				sqlDataReader = sqlCommand.ExecuteReader();
				if (!sqlDataReader.HasRows)
				{
					return true; // assume new empty database
				}
				sqlDataReader.Read();
						
				databaseVersion = new Version(Convert.ToString(sqlDataReader["confVersion"], CultureInfo.InvariantCulture));
						
				sqlDataReader.Close();
						
				if (databaseVersion.CompareTo(new System.Version(2, 2)) == 0) // 2.2
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format("Upgrading database from version {0} to version {1}.", databaseVersion.ToString(), "2.3"));
					sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD EnableFontSmoothing bit NOT NULL DEFAULT 0, EnableDesktopComposition bit NOT NULL DEFAULT 0, InheritEnableFontSmoothing bit NOT NULL DEFAULT 0, InheritEnableDesktopComposition bit NOT NULL DEFAULT 0;", sqlConnection);
					sqlCommand.ExecuteNonQuery();
					databaseVersion = new System.Version(2, 3);
				}
						
				if (databaseVersion.CompareTo(new System.Version(2, 3)) == 0) // 2.3
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format("Upgrading database from version {0} to version {1}.", databaseVersion.ToString(), "2.4"));
					sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD UseCredSsp bit NOT NULL DEFAULT 1, InheritUseCredSsp bit NOT NULL DEFAULT 0;", sqlConnection);
					sqlCommand.ExecuteNonQuery();
					databaseVersion = new Version(2, 4);
				}
						
				if (databaseVersion.CompareTo(new Version(2, 4)) == 0) // 2.4
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, string.Format("Upgrading database from version {0} to version {1}.", databaseVersion.ToString(), "2.5"));
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
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, string.Format(My.Language.strErrorBadDatabaseVersion, databaseVersion.ToString(), (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName));
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(My.Language.strErrorVerifyDatabaseVersionFailed, ex.Message));
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
					
			_sqlConnection.Open();
					
			if (!VerifyDatabaseVersion(_sqlConnection))
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strErrorConnectionListSaveFailed);
				return ;
			}
					
			TreeNode tN = default(TreeNode);
			tN = (TreeNode)RootTreeNode.Clone();
					
			string strProtected = "";
			if (tN.Tag != null)
			{
				if ((tN.Tag as mRemoteNG.Root.Info).Password == true)
				{
					_password = Convert.ToString((tN.Tag as mRemoteNG.Root.Info).PasswordString);
					strProtected = Security.Crypt.Encrypt("ThisIsProtected", _password);
				}
				else
				{
					strProtected = Security.Crypt.Encrypt("ThisIsNotProtected", _password);
				}
			}
			else
			{
				strProtected = Security.Crypt.Encrypt("ThisIsNotProtected", _password);
			}
					
			_sqlQuery = new SqlCommand("DELETE FROM tblRoot", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();

            _sqlQuery = new SqlCommand("INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES(\'" + MiscTools.PrepareValueForDB(tN.Text) + "\', 0, \'" + strProtected + "\'," + App.Info.ConnectionsFileInfo.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture) + ")", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();
					
			_sqlQuery = new SqlCommand("DELETE FROM tblCons", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();
					
			TreeNodeCollection tNC = default(TreeNodeCollection);
			tNC = tN.Nodes;
					
			SaveNodesSQL(tNC);
					
			_sqlQuery = new SqlCommand("DELETE FROM tblUpdate", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();
			_sqlQuery = new SqlCommand("INSERT INTO tblUpdate (LastUpdate) VALUES(\'" + Tools.MiscTools.DBDate(DateTime.Now) + "\')", _sqlConnection);
			_sqlQuery.ExecuteNonQuery();
					
			_sqlConnection.Close();
		}
				
		private void SaveNodesSQL(TreeNodeCollection tnc)
		{
			foreach (TreeNode node in tnc)
			{
				_currentNodeIndex++;
						
				ConnectionInfo curConI = default(ConnectionInfo);
				_sqlQuery = new SqlCommand("INSERT INTO tblCons (Name, Type, Expanded, Description, Icon, Panel, Username, " + "DomainName, Password, Hostname, Protocol, PuttySession, " + "Port, ConnectToConsole, RenderingEngine, ICAEncryptionStrength, RDPAuthenticationLevel, LoadBalanceInfo, Colors, Resolution, AutomaticResize, DisplayWallpaper, " + "DisplayThemes, EnableFontSmoothing, EnableDesktopComposition, CacheBitmaps, RedirectDiskDrives, RedirectPorts, " + "RedirectPrinters, RedirectSmartCards, RedirectSound, RedirectKeys, " + "Connected, PreExtApp, PostExtApp, MacAddress, UserField, ExtApp, VNCCompression, VNCEncoding, VNCAuthMode, " + "VNCProxyType, VNCProxyIP, VNCProxyPort, VNCProxyUsername, VNCProxyPassword, " + "VNCColors, VNCSmartSizeMode, VNCViewOnly, " + "RDGatewayUsageMethod, RDGatewayHostname, RDGatewayUseConnectionCredentials, RDGatewayUsername, RDGatewayPassword, RDGatewayDomain, " + "UseCredSsp, " + "InheritCacheBitmaps, InheritColors, " + "InheritDescription, InheritDisplayThemes, InheritDisplayWallpaper, InheritEnableFontSmoothing, InheritEnableDesktopComposition, InheritDomain, " + "InheritIcon, InheritPanel, InheritPassword, InheritPort, " + "InheritProtocol, InheritPuttySession, InheritRedirectDiskDrives, " + "InheritRedirectKeys, InheritRedirectPorts, InheritRedirectPrinters, " + "InheritRedirectSmartCards, InheritRedirectSound, InheritResolution, InheritAutomaticResize, " + "InheritUseConsoleSession, InheritRenderingEngine, InheritUsername, InheritICAEncryptionStrength, InheritRDPAuthenticationLevel, InheritLoadBalanceInfo, " + "InheritPreExtApp, InheritPostExtApp, InheritMacAddress, InheritUserField, InheritExtApp, InheritVNCCompression, InheritVNCEncoding, " + "InheritVNCAuthMode, InheritVNCProxyType, InheritVNCProxyIP, InheritVNCProxyPort, " + "InheritVNCProxyUsername, InheritVNCProxyPassword, InheritVNCColors, " + "InheritVNCSmartSizeMode, InheritVNCViewOnly, " + "InheritRDGatewayUsageMethod, InheritRDGatewayHostname, InheritRDGatewayUseConnectionCredentials, InheritRDGatewayUsername, InheritRDGatewayPassword, InheritRDGatewayDomain, "
				+ "InheritUseCredSsp, " + "PositionID, ParentID, ConstantID, LastChange)" + "VALUES (", _sqlConnection
				);
						
				if (Tree.Node.GetNodeType(node) == TreeNodeType.Connection | Tree.Node.GetNodeType(node) == TreeNodeType.Container)
				{
					//_xmlTextWriter.WriteStartElement("Node")
					_sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(node.Text) + "\',"; //Name
					_sqlQuery.CommandText += "\'" + Tree.Node.GetNodeType(node).ToString() + "\',"; //Type
				}
						
				if (Tree.Node.GetNodeType(node) == TreeNodeType.Container) //container
				{
					_sqlQuery.CommandText += "\'" + this.ContainerList[node.Tag].IsExpanded + "\',"; //Expanded
					curConI = this.ContainerList[node.Tag].ConnectionInfo;
					SaveConnectionFieldsSQL(curConI);
							
					_sqlQuery.CommandText = Tools.MiscTools.PrepareForDB(_sqlQuery.CommandText);
					_sqlQuery.ExecuteNonQuery();
					//_parentConstantId = _currentNodeIndex
					SaveNodesSQL(node.Nodes);
					//_xmlTextWriter.WriteEndElement()
				}
						
				if (Tree.Node.GetNodeType(node) == TreeNodeType.Connection)
				{
					_sqlQuery.CommandText += "\'" + System.Convert.ToString(false) + "\',";
					curConI = this.ConnectionList[node.Tag];
					SaveConnectionFieldsSQL(curConI);
					//_xmlTextWriter.WriteEndElement()
					_sqlQuery.CommandText = Tools.MiscTools.PrepareForDB(_sqlQuery.CommandText);
					_sqlQuery.ExecuteNonQuery();
				}
						
				//_parentConstantId = 0
			}
		}
				
		private void SaveConnectionFieldsSQL(ConnectionInfo curConI)
		{
			ConnectionInfo with_1 = curConI;
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Description) + "\',";
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Icon) + "\',";
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Panel) + "\',";
					
			if (this.SaveSecurity.Username == true)
			{
                _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Username) + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			if (this.SaveSecurity.Domain == true)
			{
                _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Domain) + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			if (this.SaveSecurity.Password == true)
			{
                _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(Security.Crypt.Encrypt(with_1.Password, _password)) + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}

            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.Hostname) + "\',";
			_sqlQuery.CommandText += "\'" + with_1.Protocol.ToString() + "\',";
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(with_1.PuttySession) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Port) + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.UseConsoleSession) + "\',";
			_sqlQuery.CommandText += "\'" + with_1.RenderingEngine.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.ICAEncryption.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.RDPAuthenticationLevel.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.LoadBalanceInfo + "\',";
			_sqlQuery.CommandText += "\'" + with_1.Colors.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.Resolution.ToString() + "\',";
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
			_sqlQuery.CommandText += "\'" + with_1.RedirectSound.ToString() + "\',";
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
					
			_sqlQuery.CommandText += "\'" + with_1.VNCCompression.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCEncoding.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCAuthMode.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCProxyType.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCProxyIP + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.VNCProxyPort) + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCProxyUsername + "\',";
			_sqlQuery.CommandText += "\'" + Security.Crypt.Encrypt(with_1.VNCProxyPassword, _password) + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCColors.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.VNCSmartSizeMode.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.VNCViewOnly) + "\',";
					
			_sqlQuery.CommandText += "\'" + with_1.RDGatewayUsageMethod.ToString() + "\',";
			_sqlQuery.CommandText += "\'" + with_1.RDGatewayHostname + "\',";
			_sqlQuery.CommandText += "\'" + with_1.RDGatewayUseConnectionCredentials.ToString() + "\',";
					
			if (this.SaveSecurity.Username == true)
			{
				_sqlQuery.CommandText += "\'" + with_1.RDGatewayUsername + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			if (this.SaveSecurity.Password == true)
			{
				_sqlQuery.CommandText += "\'" + Security.Crypt.Encrypt(with_1.RDGatewayPassword, _password) + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			if (this.SaveSecurity.Domain == true)
			{
				_sqlQuery.CommandText += "\'" + with_1.RDGatewayDomain + "\',";
			}
			else
			{
				_sqlQuery.CommandText += "\'" + "" + "\',";
			}
					
			_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.UseCredSsp) + "\',";
					
			if (this.SaveSecurity.Inheritance == true)
			{
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.CacheBitmaps) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Colors) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Description) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.DisplayThemes) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.DisplayWallpaper) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.EnableFontSmoothing) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.EnableDesktopComposition) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Domain) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Icon) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Panel) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Password) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Port) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Protocol) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.PuttySession) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RedirectDiskDrives) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RedirectKeys) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RedirectPorts) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RedirectPrinters) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RedirectSmartCards) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RedirectSound) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Resolution) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.AutomaticResize) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.UseConsoleSession) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RenderingEngine) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.Username) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.ICAEncryption) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RDPAuthenticationLevel) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.LoadBalanceInfo) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.PreExtApp) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.PostExtApp) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.MacAddress) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.UserField) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.ExtApp) + "\',";
						
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCCompression) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCEncoding) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCAuthMode) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCProxyType) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCProxyIP) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCProxyPort) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCProxyUsername) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCProxyPassword) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCColors) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCSmartSizeMode) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.VNCViewOnly) + "\',";
						
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RDGatewayUsageMethod) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RDGatewayHostname) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RDGatewayUseConnectionCredentials) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RDGatewayUsername) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RDGatewayPassword) + "\',";
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.RDGatewayDomain) + "\',";
						
				_sqlQuery.CommandText += "\'" + Convert.ToString(with_1.Inherit.UseCredSsp) + "\',";
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
					_parentConstantId = Convert.ToString((with_1.Parent as ContainerInfo).ConnectionInfo.ConstantID);
				}
				else
				{
					_parentConstantId = Convert.ToString(0);
				}
			}
			else
			{
				if ((with_1.Parent as ContainerInfo).Parent != null)
				{
					_parentConstantId = Convert.ToString(((with_1.Parent as ContainerInfo).Parent as ContainerInfo).ConnectionInfo.ConstantID);
				}
				else
				{
					_parentConstantId = Convert.ToString(0);
				}
			}
					
			_sqlQuery.CommandText += _currentNodeIndex + ",\'" + _parentConstantId + "\',\'" + with_1.ConstantID + "\',\'" + Tools.MiscTools.DBDate(DateTime.Now) + "\')";
		}
        #endregion
				
        #region XML
		private void EncryptCompleteFile()
		{
			StreamReader streamReader = new StreamReader(ConnectionFileName);
					
			string fileContents = "";
			fileContents = streamReader.ReadToEnd();
			streamReader.Close();
					
			if (!string.IsNullOrEmpty(fileContents))
			{
				StreamWriter streamWriter = new StreamWriter(ConnectionFileName);
				streamWriter.Write(Security.Crypt.Encrypt(fileContents, _password));
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
						
				TreeNode treeNode = default(TreeNode);
						
				if (Tree.Node.GetNodeType(RootTreeNode) == Tree.TreeNodeType.Root)
				{
					treeNode = (TreeNode)RootTreeNode.Clone();
				}
				else
				{
					treeNode = new TreeNode("mR|Export (" + Tools.MiscTools.DBDate(DateTime.Now) + ")");
					treeNode.Nodes.Add(System.Convert.ToString(RootTreeNode.Clone()));
				}
						
				string tempFileName = Path.GetTempFileName();
				_xmlTextWriter = new XmlTextWriter(tempFileName, System.Text.Encoding.UTF8);
						
				_xmlTextWriter.Formatting = Formatting.Indented;
				_xmlTextWriter.Indentation = 4;
						
				_xmlTextWriter.WriteStartDocument();
						
				_xmlTextWriter.WriteStartElement("Connections"); // Do not localize
				_xmlTextWriter.WriteAttributeString("Name", "", treeNode.Text);
				_xmlTextWriter.WriteAttributeString("Export", "", System.Convert.ToString(Export));
						
				if (Export)
				{
					_xmlTextWriter.WriteAttributeString("Protected", "", Security.Crypt.Encrypt("ThisIsNotProtected", _password));
				}
				else
				{
					if ((treeNode.Tag as Root.Info).Password == true)
					{
						_password = System.Convert.ToString((treeNode.Tag as Root.Info).PasswordString);
						_xmlTextWriter.WriteAttributeString("Protected", "", Security.Crypt.Encrypt("ThisIsProtected", _password));
					}
					else
					{
						_xmlTextWriter.WriteAttributeString("Protected", "", Security.Crypt.Encrypt("ThisIsNotProtected", _password));
					}
				}
						
				_xmlTextWriter.WriteAttributeString("ConfVersion", "", App.Info.ConnectionsFileInfo.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture));
						
				TreeNodeCollection treeNodeCollection = default(TreeNodeCollection);
				treeNodeCollection = treeNode.Nodes;
						
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveToXml failed" + Environment.NewLine + ex.Message, false);
			}
		}
				
		private void SaveNode(TreeNodeCollection tNC)
		{
			try
			{
				foreach (TreeNode node in tNC)
				{
					Connection.ConnectionInfo curConI = default(Connection.ConnectionInfo);
							
					if (Tree.Node.GetNodeType(node) == Tree.TreeNodeType.Connection | Tree.Node.GetNodeType(node) == Tree.TreeNodeType.Container)
					{
						_xmlTextWriter.WriteStartElement("Node");
						_xmlTextWriter.WriteAttributeString("Name", "", node.Text);
						_xmlTextWriter.WriteAttributeString("Type", "", Tree.Node.GetNodeType(node).ToString());
					}
							
					if (Tree.Node.GetNodeType(node) == Tree.TreeNodeType.Container) //container
					{
						_xmlTextWriter.WriteAttributeString("Expanded", "", System.Convert.ToString(this.ContainerList[node.Tag].TreeNode.IsExpanded));
						curConI = this.ContainerList[node.Tag].ConnectionInfo;
						SaveConnectionFields(curConI);
						SaveNode(node.Nodes);
						_xmlTextWriter.WriteEndElement();
					}
							
					if (Tree.Node.GetNodeType(node) == Tree.TreeNodeType.Connection)
					{
						curConI = this.ConnectionList[node.Tag];
						SaveConnectionFields(curConI);
						_xmlTextWriter.WriteEndElement();
					}
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveNode failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void SaveConnectionFields(Connection.ConnectionInfo curConI)
		{
			try
			{
				_xmlTextWriter.WriteAttributeString("Descr", "", curConI.Description);
						
				_xmlTextWriter.WriteAttributeString("Icon", "", curConI.Icon);
						
				_xmlTextWriter.WriteAttributeString("Panel", "", curConI.Panel);
						
				if (this.SaveSecurity.Username == true)
				{
					_xmlTextWriter.WriteAttributeString("Username", "", curConI.Username);
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("Username", "", "");
				}
						
				if (this.SaveSecurity.Domain == true)
				{
					_xmlTextWriter.WriteAttributeString("Domain", "", curConI.Domain);
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("Domain", "", "");
				}
						
				if (this.SaveSecurity.Password == true)
				{
					_xmlTextWriter.WriteAttributeString("Password", "", Security.Crypt.Encrypt(curConI.Password, _password));
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("Password", "", "");
				}
						
				_xmlTextWriter.WriteAttributeString("Hostname", "", curConI.Hostname);
						
				_xmlTextWriter.WriteAttributeString("Protocol", "", curConI.Protocol.ToString());
						
				_xmlTextWriter.WriteAttributeString("PuttySession", "", curConI.PuttySession);
						
				_xmlTextWriter.WriteAttributeString("Port", "", System.Convert.ToString(curConI.Port));
						
				_xmlTextWriter.WriteAttributeString("ConnectToConsole", "", System.Convert.ToString(curConI.UseConsoleSession));
						
				_xmlTextWriter.WriteAttributeString("UseCredSsp", "", System.Convert.ToString(curConI.UseCredSsp));
						
				_xmlTextWriter.WriteAttributeString("RenderingEngine", "", curConI.RenderingEngine.ToString());
						
				_xmlTextWriter.WriteAttributeString("ICAEncryptionStrength", "", curConI.ICAEncryption.ToString());
						
				_xmlTextWriter.WriteAttributeString("RDPAuthenticationLevel", "", curConI.RDPAuthenticationLevel.ToString());
						
				_xmlTextWriter.WriteAttributeString("LoadBalanceInfo", "", curConI.LoadBalanceInfo);
						
				_xmlTextWriter.WriteAttributeString("Colors", "", curConI.Colors.ToString());
						
				_xmlTextWriter.WriteAttributeString("Resolution", "", curConI.Resolution.ToString());
						
				_xmlTextWriter.WriteAttributeString("AutomaticResize", "", System.Convert.ToString(curConI.AutomaticResize));
						
				_xmlTextWriter.WriteAttributeString("DisplayWallpaper", "", System.Convert.ToString(curConI.DisplayWallpaper));
						
				_xmlTextWriter.WriteAttributeString("DisplayThemes", "", System.Convert.ToString(curConI.DisplayThemes));
						
				_xmlTextWriter.WriteAttributeString("EnableFontSmoothing", "", System.Convert.ToString(curConI.EnableFontSmoothing));
						
				_xmlTextWriter.WriteAttributeString("EnableDesktopComposition", "", System.Convert.ToString(curConI.EnableDesktopComposition));
						
				_xmlTextWriter.WriteAttributeString("CacheBitmaps", "", System.Convert.ToString(curConI.CacheBitmaps));
						
				_xmlTextWriter.WriteAttributeString("RedirectDiskDrives", "", System.Convert.ToString(curConI.RedirectDiskDrives));
						
				_xmlTextWriter.WriteAttributeString("RedirectPorts", "", System.Convert.ToString(curConI.RedirectPorts));
						
				_xmlTextWriter.WriteAttributeString("RedirectPrinters", "", System.Convert.ToString(curConI.RedirectPrinters));
						
				_xmlTextWriter.WriteAttributeString("RedirectSmartCards", "", System.Convert.ToString(curConI.RedirectSmartCards));
						
				_xmlTextWriter.WriteAttributeString("RedirectSound", "", curConI.RedirectSound.ToString());
						
				_xmlTextWriter.WriteAttributeString("RedirectKeys", "", System.Convert.ToString(curConI.RedirectKeys));
						
				if (curConI.OpenConnections.Count > 0)
				{
					_xmlTextWriter.WriteAttributeString("Connected", "", System.Convert.ToString(true));
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("Connected", "", System.Convert.ToString(false));
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
				_xmlTextWriter.WriteAttributeString("VNCProxyPort", "", System.Convert.ToString(curConI.VNCProxyPort));
				_xmlTextWriter.WriteAttributeString("VNCProxyUsername", "", curConI.VNCProxyUsername);
				_xmlTextWriter.WriteAttributeString("VNCProxyPassword", "", Security.Crypt.Encrypt(curConI.VNCProxyPassword, _password));
				_xmlTextWriter.WriteAttributeString("VNCColors", "", curConI.VNCColors.ToString());
				_xmlTextWriter.WriteAttributeString("VNCSmartSizeMode", "", curConI.VNCSmartSizeMode.ToString());
				_xmlTextWriter.WriteAttributeString("VNCViewOnly", "", System.Convert.ToString(curConI.VNCViewOnly));
						
				_xmlTextWriter.WriteAttributeString("RDGatewayUsageMethod", "", curConI.RDGatewayUsageMethod.ToString());
				_xmlTextWriter.WriteAttributeString("RDGatewayHostname", "", curConI.RDGatewayHostname);
						
				_xmlTextWriter.WriteAttributeString("RDGatewayUseConnectionCredentials", "", curConI.RDGatewayUseConnectionCredentials.ToString());
						
				if (this.SaveSecurity.Username == true)
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", curConI.RDGatewayUsername);
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", "");
				}
						
				if (this.SaveSecurity.Password == true)
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", Security.Crypt.Encrypt(curConI.RDGatewayPassword, _password));
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", "");
				}
						
				if (this.SaveSecurity.Domain == true)
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", curConI.RDGatewayDomain);
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", "");
				}
						
				if (this.SaveSecurity.Inheritance == true)
				{
					_xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", System.Convert.ToString(curConI.Inherit.CacheBitmaps));
					_xmlTextWriter.WriteAttributeString("InheritColors", "", System.Convert.ToString(curConI.Inherit.Colors));
					_xmlTextWriter.WriteAttributeString("InheritDescription", "", System.Convert.ToString(curConI.Inherit.Description));
					_xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", System.Convert.ToString(curConI.Inherit.DisplayThemes));
					_xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", System.Convert.ToString(curConI.Inherit.DisplayWallpaper));
					_xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", System.Convert.ToString(curConI.Inherit.EnableFontSmoothing));
					_xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", System.Convert.ToString(curConI.Inherit.EnableDesktopComposition));
					_xmlTextWriter.WriteAttributeString("InheritDomain", "", System.Convert.ToString(curConI.Inherit.Domain));
					_xmlTextWriter.WriteAttributeString("InheritIcon", "", System.Convert.ToString(curConI.Inherit.Icon));
					_xmlTextWriter.WriteAttributeString("InheritPanel", "", System.Convert.ToString(curConI.Inherit.Panel));
					_xmlTextWriter.WriteAttributeString("InheritPassword", "", System.Convert.ToString(curConI.Inherit.Password));
					_xmlTextWriter.WriteAttributeString("InheritPort", "", System.Convert.ToString(curConI.Inherit.Port));
					_xmlTextWriter.WriteAttributeString("InheritProtocol", "", System.Convert.ToString(curConI.Inherit.Protocol));
					_xmlTextWriter.WriteAttributeString("InheritPuttySession", "", System.Convert.ToString(curConI.Inherit.PuttySession));
					_xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", System.Convert.ToString(curConI.Inherit.RedirectDiskDrives));
					_xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", System.Convert.ToString(curConI.Inherit.RedirectKeys));
					_xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", System.Convert.ToString(curConI.Inherit.RedirectPorts));
					_xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", System.Convert.ToString(curConI.Inherit.RedirectPrinters));
					_xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", System.Convert.ToString(curConI.Inherit.RedirectSmartCards));
					_xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", System.Convert.ToString(curConI.Inherit.RedirectSound));
					_xmlTextWriter.WriteAttributeString("InheritResolution", "", System.Convert.ToString(curConI.Inherit.Resolution));
					_xmlTextWriter.WriteAttributeString("InheritAutomaticResize", "", System.Convert.ToString(curConI.Inherit.AutomaticResize));
					_xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", System.Convert.ToString(curConI.Inherit.UseConsoleSession));
					_xmlTextWriter.WriteAttributeString("InheritUseCredSsp", "", System.Convert.ToString(curConI.Inherit.UseCredSsp));
					_xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", System.Convert.ToString(curConI.Inherit.RenderingEngine));
					_xmlTextWriter.WriteAttributeString("InheritUsername", "", System.Convert.ToString(curConI.Inherit.Username));
					_xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", System.Convert.ToString(curConI.Inherit.ICAEncryption));
					_xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", System.Convert.ToString(curConI.Inherit.RDPAuthenticationLevel));
					_xmlTextWriter.WriteAttributeString("InheritLoadBalanceInfo", "", System.Convert.ToString(curConI.Inherit.LoadBalanceInfo));
					_xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", System.Convert.ToString(curConI.Inherit.PreExtApp));
					_xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", System.Convert.ToString(curConI.Inherit.PostExtApp));
					_xmlTextWriter.WriteAttributeString("InheritMacAddress", "", System.Convert.ToString(curConI.Inherit.MacAddress));
					_xmlTextWriter.WriteAttributeString("InheritUserField", "", System.Convert.ToString(curConI.Inherit.UserField));
					_xmlTextWriter.WriteAttributeString("InheritExtApp", "", System.Convert.ToString(curConI.Inherit.ExtApp));
					_xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", System.Convert.ToString(curConI.Inherit.VNCCompression));
					_xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", System.Convert.ToString(curConI.Inherit.VNCEncoding));
					_xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", System.Convert.ToString(curConI.Inherit.VNCAuthMode));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", System.Convert.ToString(curConI.Inherit.VNCProxyType));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", System.Convert.ToString(curConI.Inherit.VNCProxyIP));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", System.Convert.ToString(curConI.Inherit.VNCProxyPort));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", System.Convert.ToString(curConI.Inherit.VNCProxyUsername));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", System.Convert.ToString(curConI.Inherit.VNCProxyPassword));
					_xmlTextWriter.WriteAttributeString("InheritVNCColors", "", System.Convert.ToString(curConI.Inherit.VNCColors));
					_xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", System.Convert.ToString(curConI.Inherit.VNCSmartSizeMode));
					_xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", System.Convert.ToString(curConI.Inherit.VNCViewOnly));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUsageMethod", "", System.Convert.ToString(curConI.Inherit.RDGatewayUsageMethod));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", System.Convert.ToString(curConI.Inherit.RDGatewayHostname));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", System.Convert.ToString(curConI.Inherit.RDGatewayUseConnectionCredentials));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", System.Convert.ToString(curConI.Inherit.RDGatewayUsername));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", System.Convert.ToString(curConI.Inherit.RDGatewayPassword));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", System.Convert.ToString(curConI.Inherit.RDGatewayDomain));
				}
				else
				{
					_xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritColors", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritDescription", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritDomain", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritIcon", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPanel", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPassword", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPort", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritProtocol", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPuttySession", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritResolution", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritAutomaticResize", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritUseCredSsp", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritUsername", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritLoadBalanceInfo", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritMacAddress", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritUserField", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritExtApp", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCColors", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", System.Convert.ToString(false));
					_xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", System.Convert.ToString(false));
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "SaveConnectionFields failed" + Environment.NewLine + ex.Message, true);
			}
		}
        #endregion
				
        #region CSV
		private StreamWriter csvWr;
				
		private void SaveTomRCSV()
		{
			if (App.Runtime.IsConnectionsFileLoaded == false)
			{
				return;
			}
					
			TreeNode tN = default(TreeNode);
			tN = (TreeNode)RootTreeNode.Clone();
					
			TreeNodeCollection tNC = default(TreeNodeCollection);
			tNC = tN.Nodes;
					
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
				if (Tree.Node.GetNodeType(node) == Tree.TreeNodeType.Connection)
				{
					Connection.ConnectionInfo curConI = (Connection.ConnectionInfo)node.Tag;
							
					WritemRCSVLine(curConI);
				}
				else if (Tree.Node.GetNodeType(node) == Tree.TreeNodeType.Container)
				{
					SaveNodemRCSV(node.Nodes);
				}
			}
		}
				
		private void WritemRCSVLine(Connection.ConnectionInfo con)
		{
			string nodePath = con.TreeNode.FullPath;
					
			int firstSlash = nodePath.IndexOf("\\");
			nodePath = nodePath.Remove(0, firstSlash + 1);
			int lastSlash = nodePath.LastIndexOf("\\");
					
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
					
			csvLn += con.Hostname + ";" + con.Protocol.ToString() + ";" + con.PuttySession + ";" + System.Convert.ToString(con.Port) + ";" + System.Convert.ToString(con.UseConsoleSession) + ";" + System.Convert.ToString(con.UseCredSsp) + ";" + con.RenderingEngine.ToString() + ";" + con.ICAEncryption.ToString() + ";" + con.RDPAuthenticationLevel.ToString() + ";" + con.LoadBalanceInfo + ";" + con.Colors.ToString() + ";" + con.Resolution.ToString() + ";" + System.Convert.ToString(con.AutomaticResize) + ";" + System.Convert.ToString(con.DisplayWallpaper) + ";" + System.Convert.ToString(con.DisplayThemes) + ";" + System.Convert.ToString(con.EnableFontSmoothing) + ";" + System.Convert.ToString(con.EnableDesktopComposition) + ";" + System.Convert.ToString(con.CacheBitmaps) + ";" + System.Convert.ToString(con.RedirectDiskDrives) + ";" + System.Convert.ToString(con.RedirectPorts) + ";" + System.Convert.ToString(con.RedirectPrinters) + ";" + System.Convert.ToString(con.RedirectSmartCards) + ";" + con.RedirectSound.ToString() + ";" + System.Convert.ToString(con.RedirectKeys) + ";" + con.PreExtApp + ";" + con.PostExtApp + ";" + con.MacAddress + ";" + con.UserField + ";" + con.ExtApp + ";" + con.VNCCompression.ToString() + ";" + con.VNCEncoding.ToString() + ";" + con.VNCAuthMode.ToString() + ";" + con.VNCProxyType.ToString() + ";" + con.VNCProxyIP + ";" + System.Convert.ToString(con.VNCProxyPort) + ";" + con.VNCProxyUsername + ";" + con.VNCProxyPassword + ";" + con.VNCColors.ToString() + ";" + con.VNCSmartSizeMode.ToString() + ";" + System.Convert.ToString(con.VNCViewOnly) + ";";
					
			if (SaveSecurity.Inheritance)
			{
				csvLn += con.Inherit.CacheBitmaps + ";" + System.Convert.ToString(con.Inherit.Colors) + ";" + System.Convert.ToString(con.Inherit.Description) + ";" + System.Convert.ToString(con.Inherit.DisplayThemes) + ";" + System.Convert.ToString(con.Inherit.DisplayWallpaper) + ";" + System.Convert.ToString(con.Inherit.EnableFontSmoothing) + ";" + System.Convert.ToString(con.Inherit.EnableDesktopComposition) + ";" + System.Convert.ToString(con.Inherit.Domain) + ";" + System.Convert.ToString(con.Inherit.Icon) + ";" + System.Convert.ToString(con.Inherit.Panel) + ";" + System.Convert.ToString(con.Inherit.Password) + ";" + System.Convert.ToString(con.Inherit.Port) + ";" + System.Convert.ToString(con.Inherit.Protocol) + ";" + System.Convert.ToString(con.Inherit.PuttySession) + ";" + System.Convert.ToString(con.Inherit.RedirectDiskDrives) + ";" + System.Convert.ToString(con.Inherit.RedirectKeys) + ";" + System.Convert.ToString(con.Inherit.RedirectPorts) + ";" + System.Convert.ToString(con.Inherit.RedirectPrinters) + ";" + System.Convert.ToString(con.Inherit.RedirectSmartCards) + ";" + System.Convert.ToString(con.Inherit.RedirectSound) + ";" + System.Convert.ToString(con.Inherit.Resolution) + ";" + System.Convert.ToString(con.Inherit.AutomaticResize) + ";" + System.Convert.ToString(con.Inherit.UseConsoleSession) + ";" + System.Convert.ToString(con.Inherit.UseCredSsp) + ";" + System.Convert.ToString(con.Inherit.RenderingEngine) + ";" + System.Convert.ToString(con.Inherit.Username) + ";" + System.Convert.ToString(con.Inherit.ICAEncryption) + ";" + System.Convert.ToString(con.Inherit.RDPAuthenticationLevel) + ";" + System.Convert.ToString(con.Inherit.LoadBalanceInfo) + ";" + System.Convert.ToString(con.Inherit.PreExtApp) + ";" + System.Convert.ToString(con.Inherit.PostExtApp) + ";" + System.Convert.ToString(con.Inherit.MacAddress) + ";" + System.Convert.ToString(con.Inherit.UserField) + ";" + System.Convert.ToString(con.Inherit.ExtApp) + ";" + System.Convert.ToString(con.Inherit.VNCCompression) + ";"
				+ System.Convert.ToString(con.Inherit.VNCEncoding) + ";" + System.Convert.ToString(con.Inherit.VNCAuthMode) + ";" + System.Convert.ToString(con.Inherit.VNCProxyType) + ";" + System.Convert.ToString(con.Inherit.VNCProxyIP) + ";" + System.Convert.ToString(con.Inherit.VNCProxyPort) + ";" + System.Convert.ToString(con.Inherit.VNCProxyUsername) + ";" + System.Convert.ToString(con.Inherit.VNCProxyPassword) + ";" + System.Convert.ToString(con.Inherit.VNCColors) + ";" + System.Convert.ToString(con.Inherit.VNCSmartSizeMode) + ";" + System.Convert.ToString(con.Inherit.VNCViewOnly);
			}
					
			csvWr.WriteLine(csvLn);
		}
        #endregion
				
        #region vRD CSV
		private void SaveTovRDCSV()
		{
			if (App.Runtime.IsConnectionsFileLoaded == false)
			{
				return;
			}
					
			TreeNode tN = default(TreeNode);
			tN = (TreeNode)RootTreeNode.Clone();
					
			TreeNodeCollection tNC = default(TreeNodeCollection);
			tNC = tN.Nodes;
					
			csvWr = new StreamWriter(ConnectionFileName);
					
			SaveNodevRDCSV(tNC);
					
			csvWr.Close();
		}
				
		private void SaveNodevRDCSV(TreeNodeCollection tNC)
		{
			foreach (TreeNode node in tNC)
			{
				if (Tree.Node.GetNodeType(node) == Tree.TreeNodeType.Connection)
				{
					Connection.ConnectionInfo curConI = (Connection.ConnectionInfo)node.Tag;
							
					if (curConI.Protocol == Connection.Protocol.ProtocolType.RDP)
					{
						WritevRDCSVLine(curConI);
					}
				}
				else if (Tree.Node.GetNodeType(node) == Tree.TreeNodeType.Container)
				{
					SaveNodevRDCSV(node.Nodes);
				}
			}
		}
				
		private void WritevRDCSVLine(Connection.ConnectionInfo con)
		{
			string nodePath = con.TreeNode.FullPath;
					
			int firstSlash = nodePath.IndexOf("\\");
			nodePath = nodePath.Remove(0, firstSlash + 1);
			int lastSlash = nodePath.LastIndexOf("\\");
					
			if (lastSlash > 0)
			{
				nodePath = nodePath.Remove(lastSlash);
			}
			else
			{
				nodePath = "";
			}
					
			csvWr.WriteLine(con.Name + ";" + con.Hostname + ";" + con.MacAddress + ";;" + System.Convert.ToString(con.Port) + ";" + System.Convert.ToString(con.UseConsoleSession) + ";" + nodePath);
		}
        #endregion
				
        #region vRD VRE
		private void SaveToVRE()
		{
			if (App.Runtime.IsConnectionsFileLoaded == false)
			{
				return;
			}
					
			TreeNode tN = default(TreeNode);
			tN = (TreeNode)RootTreeNode.Clone();
					
			TreeNodeCollection tNC = default(TreeNodeCollection);
			tNC = tN.Nodes;
					
			_xmlTextWriter = new XmlTextWriter(ConnectionFileName, System.Text.Encoding.UTF8);
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
				if (Tree.Node.GetNodeType(node) == Tree.TreeNodeType.Connection)
				{
					Connection.ConnectionInfo curConI = (Connection.ConnectionInfo)node.Tag;
							
					if (curConI.Protocol == Connection.Protocol.ProtocolType.RDP)
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
				
		private void WriteVREitem(Connection.ConnectionInfo con)
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