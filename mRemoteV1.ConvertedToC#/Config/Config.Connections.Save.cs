using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Globalization;
using mRemoteNG.App.Runtime;
using System.Data.SqlClient;
using mRemoteNG.Tools.Misc;

namespace mRemoteNG.Config
{
	namespace Connections
	{
		public class Save
		{
			#region "Public Enums"
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

			#region "Private Properties"
			private XmlTextWriter _xmlTextWriter;

			private string _password = mRemoteNG.App.Info.General.EncryptionKey;
			private SqlConnection _sqlConnection;

			private SqlCommand _sqlQuery;
			private int _currentNodeIndex = 0;
				#endregion
			private string _parentConstantId = 0;

			#region "Public Properties"
			public string SQLHost { get; set; }
			public string SQLDatabaseName { get; set; }
			public string SQLUsername { get; set; }
			public string SQLPassword { get; set; }

			public string ConnectionFileName { get; set; }
			public TreeNode RootTreeNode { get; set; }
			public bool Export { get; set; }
			public Format SaveFormat { get; set; }
			public Security.Save SaveSecurity { get; set; }
			public Connection.List ConnectionList { get; set; }
			public Container.List ContainerList { get; set; }
			#endregion

			#region "Public Methods"
			public void Save()
			{
				switch (SaveFormat) {
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
						if (mRemoteNG.My.Settings.EncryptCompleteConnectionsFile) {
							EncryptCompleteFile();
						}
						if (!Export)
							My.MyProject.Forms.frmMain.ConnectionsFileName = ConnectionFileName;
						break;
				}
				My.MyProject.Forms.frmMain.UsingSqlServer = (SaveFormat == Format.SQL);
			}
			#endregion

			#region "SQL"
			private bool VerifyDatabaseVersion(SqlConnection sqlConnection)
			{
				bool isVerified = false;
				SqlDataReader sqlDataReader = null;
				System.Version databaseVersion = null;
				try {
					SqlCommand sqlCommand = new SqlCommand("SELECT * FROM tblRoot", sqlConnection);
					sqlDataReader = sqlCommand.ExecuteReader();
					if ((!sqlDataReader.HasRows))
						return true;
					// assume new empty database
					sqlDataReader.Read();

					databaseVersion = new Version(Convert.ToString(sqlDataReader["confVersion"], CultureInfo.InvariantCulture));

					sqlDataReader.Close();

					// 2.2
					if (databaseVersion.CompareTo(new System.Version(2, 2)) == 0) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, string.Format("Upgrading database from version {0} to version {1}.", databaseVersion.ToString(), "2.3"));
						sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD EnableFontSmoothing bit NOT NULL DEFAULT 0, EnableDesktopComposition bit NOT NULL DEFAULT 0, InheritEnableFontSmoothing bit NOT NULL DEFAULT 0, InheritEnableDesktopComposition bit NOT NULL DEFAULT 0;", sqlConnection);
						sqlCommand.ExecuteNonQuery();
						databaseVersion = new System.Version(2, 3);
					}

					// 2.3
					if (databaseVersion.CompareTo(new System.Version(2, 3)) == 0) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, string.Format("Upgrading database from version {0} to version {1}.", databaseVersion.ToString(), "2.4"));
						sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD UseCredSsp bit NOT NULL DEFAULT 1, InheritUseCredSsp bit NOT NULL DEFAULT 0;", sqlConnection);
						sqlCommand.ExecuteNonQuery();
						databaseVersion = new Version(2, 4);
					}

					// 2.4
					if (databaseVersion.CompareTo(new Version(2, 4)) == 0) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, string.Format("Upgrading database from version {0} to version {1}.", databaseVersion.ToString(), "2.5"));
						sqlCommand = new SqlCommand("ALTER TABLE tblCons ADD LoadBalanceInfo varchar (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, AutomaticResize bit NOT NULL DEFAULT 1, InheritLoadBalanceInfo bit NOT NULL DEFAULT 0, InheritAutomaticResize bit NOT NULL DEFAULT 0;", sqlConnection);
						sqlCommand.ExecuteNonQuery();
						databaseVersion = new Version(2, 5);
					}

					// 2.5
					if (databaseVersion.CompareTo(new Version(2, 5)) == 0) {
						isVerified = true;
					}

					if (isVerified == false) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, string.Format(mRemoteNG.My.Language.strErrorBadDatabaseVersion, databaseVersion.ToString(), mRemoteNG.My.MyProject.Application.Info.ProductName));
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, string.Format(mRemoteNG.My.Language.strErrorVerifyDatabaseVersionFailed, ex.Message));
				} finally {
					if (sqlDataReader != null) {
						if (!sqlDataReader.IsClosed)
							sqlDataReader.Close();
					}
				}
				return isVerified;
			}

			private void SaveToSQL()
			{
				if (!string.IsNullOrEmpty(_SQLUsername)) {
					_sqlConnection = new SqlConnection("Data Source=" + _SQLHost + ";Initial Catalog=" + _SQLDatabaseName + ";User Id=" + _SQLUsername + ";Password=" + _SQLPassword);
				} else {
					_sqlConnection = new SqlConnection("Data Source=" + _SQLHost + ";Initial Catalog=" + _SQLDatabaseName + ";Integrated Security=True");
				}

				_sqlConnection.Open();

				if (!VerifyDatabaseVersion(_sqlConnection)) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strErrorConnectionListSaveFailed);
					return;
				}

				TreeNode tN = null;
				tN = RootTreeNode.Clone();

				string strProtected = null;
				if (tN.Tag != null) {
					if ((tN.Tag as mRemoteNG.Root.Info).Password == true) {
						_password = (tN.Tag as mRemoteNG.Root.Info).PasswordString;
						strProtected = mRemoteNG.Security.Crypt.Encrypt("ThisIsProtected", _password);
					} else {
						strProtected = mRemoteNG.Security.Crypt.Encrypt("ThisIsNotProtected", _password);
					}
				} else {
					strProtected = mRemoteNG.Security.Crypt.Encrypt("ThisIsNotProtected", _password);
				}

				_sqlQuery = new SqlCommand("DELETE FROM tblRoot", _sqlConnection);
				_sqlQuery.ExecuteNonQuery();

				_sqlQuery = new SqlCommand("INSERT INTO tblRoot (Name, Export, Protected, ConfVersion) VALUES('" + mRemoteNG.Tools.Misc.PrepareValueForDB(tN.Text) + "', 0, '" + strProtected + "'," + mRemoteNG.App.Info.Connections.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture) + ")", _sqlConnection);
				_sqlQuery.ExecuteNonQuery();

				_sqlQuery = new SqlCommand("DELETE FROM tblCons", _sqlConnection);
				_sqlQuery.ExecuteNonQuery();

				TreeNodeCollection tNC = null;
				tNC = tN.Nodes;

				SaveNodesSQL(tNC);

				_sqlQuery = new SqlCommand("DELETE FROM tblUpdate", _sqlConnection);
				_sqlQuery.ExecuteNonQuery();
				_sqlQuery = new SqlCommand("INSERT INTO tblUpdate (LastUpdate) VALUES('" + mRemoteNG.Tools.Misc.DBDate(DateAndTime.Now) + "')", _sqlConnection);
				_sqlQuery.ExecuteNonQuery();

				_sqlConnection.Close();
			}

			private void SaveNodesSQL(TreeNodeCollection tnc)
			{
				foreach (TreeNode node in tnc) {
					_currentNodeIndex += 1;

					Connection.Info curConI = null;
					_sqlQuery = new SqlCommand("INSERT INTO tblCons (Name, Type, Expanded, Description, Icon, Panel, Username, " + "DomainName, Password, Hostname, Protocol, PuttySession, " + "Port, ConnectToConsole, RenderingEngine, ICAEncryptionStrength, RDPAuthenticationLevel, LoadBalanceInfo, Colors, Resolution, AutomaticResize, DisplayWallpaper, " + "DisplayThemes, EnableFontSmoothing, EnableDesktopComposition, CacheBitmaps, RedirectDiskDrives, RedirectPorts, " + "RedirectPrinters, RedirectSmartCards, RedirectSound, RedirectKeys, " + "Connected, PreExtApp, PostExtApp, MacAddress, UserField, ExtApp, VNCCompression, VNCEncoding, VNCAuthMode, " + "VNCProxyType, VNCProxyIP, VNCProxyPort, VNCProxyUsername, VNCProxyPassword, " + "VNCColors, VNCSmartSizeMode, VNCViewOnly, " + "RDGatewayUsageMethod, RDGatewayHostname, RDGatewayUseConnectionCredentials, RDGatewayUsername, RDGatewayPassword, RDGatewayDomain, " + "UseCredSsp, " + "InheritCacheBitmaps, InheritColors, " + "InheritDescription, InheritDisplayThemes, InheritDisplayWallpaper, InheritEnableFontSmoothing, InheritEnableDesktopComposition, InheritDomain, " + "InheritIcon, InheritPanel, InheritPassword, InheritPort, " + "InheritProtocol, InheritPuttySession, InheritRedirectDiskDrives, " + "InheritRedirectKeys, InheritRedirectPorts, InheritRedirectPrinters, " + "InheritRedirectSmartCards, InheritRedirectSound, InheritResolution, InheritAutomaticResize, " + "InheritUseConsoleSession, InheritRenderingEngine, InheritUsername, InheritICAEncryptionStrength, InheritRDPAuthenticationLevel, InheritLoadBalanceInfo, " + "InheritPreExtApp, InheritPostExtApp, InheritMacAddress, InheritUserField, InheritExtApp, InheritVNCCompression, InheritVNCEncoding, " + "InheritVNCAuthMode, InheritVNCProxyType, InheritVNCProxyIP, InheritVNCProxyPort, " + "InheritVNCProxyUsername, InheritVNCProxyPassword, InheritVNCColors, " + "InheritVNCSmartSizeMode, InheritVNCViewOnly, " + "InheritRDGatewayUsageMethod, InheritRDGatewayHostname, InheritRDGatewayUseConnectionCredentials, InheritRDGatewayUsername, InheritRDGatewayPassword, InheritRDGatewayDomain, " + "InheritUseCredSsp, " + "PositionID, ParentID, ConstantID, LastChange)" + "VALUES (", _sqlConnection);

					if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Connection | mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Container) {
						//_xmlTextWriter.WriteStartElement("Node")
						_sqlQuery.CommandText += "'" + mRemoteNG.Tools.Misc.PrepareValueForDB(node.Text) + "',";
						//Name
						_sqlQuery.CommandText += "'" + mRemoteNG.Tree.Node.GetNodeType(node).ToString() + "',";
						//Type
					}

					//container
					if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Container) {
						_sqlQuery.CommandText += "'" + this._ContainerList(node.Tag).IsExpanded + "',";
						//Expanded
						curConI = this._ContainerList(node.Tag).ConnectionInfo;
						SaveConnectionFieldsSQL(curConI);

						_sqlQuery.CommandText = mRemoteNG.Tools.Misc.PrepareForDB(_sqlQuery.CommandText);
						_sqlQuery.ExecuteNonQuery();
						//_parentConstantId = _currentNodeIndex
						SaveNodesSQL(node.Nodes);
						//_xmlTextWriter.WriteEndElement()
					}

					if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Connection) {
						_sqlQuery.CommandText += "'" + false + "',";
						curConI = this._ConnectionList(node.Tag);
						SaveConnectionFieldsSQL(curConI);
						//_xmlTextWriter.WriteEndElement()
						_sqlQuery.CommandText = mRemoteNG.Tools.Misc.PrepareForDB(_sqlQuery.CommandText);
						_sqlQuery.ExecuteNonQuery();
					}

					//_parentConstantId = 0
				}
			}

			private void SaveConnectionFieldsSQL(Connection.Info curConI)
			{
				var _with1 = curConI;
				_sqlQuery.CommandText += "'" + mRemoteNG.Tools.Misc.PrepareValueForDB(_with1.Description) + "',";
				_sqlQuery.CommandText += "'" + mRemoteNG.Tools.Misc.PrepareValueForDB(_with1.Icon) + "',";
				_sqlQuery.CommandText += "'" + mRemoteNG.Tools.Misc.PrepareValueForDB(_with1.Panel) + "',";

				if (this._SaveSecurity.Username == true) {
					_sqlQuery.CommandText += "'" + mRemoteNG.Tools.Misc.PrepareValueForDB(_with1.Username) + "',";
				} else {
					_sqlQuery.CommandText += "'" + "" + "',";
				}

				if (this._SaveSecurity.Domain == true) {
					_sqlQuery.CommandText += "'" + mRemoteNG.Tools.Misc.PrepareValueForDB(_with1.Domain) + "',";
				} else {
					_sqlQuery.CommandText += "'" + "" + "',";
				}

				if (this._SaveSecurity.Password == true) {
					_sqlQuery.CommandText += "'" + mRemoteNG.Tools.Misc.PrepareValueForDB(mRemoteNG.Security.Crypt.Encrypt(_with1.Password, _password)) + "',";
				} else {
					_sqlQuery.CommandText += "'" + "" + "',";
				}

				_sqlQuery.CommandText += "'" + mRemoteNG.Tools.Misc.PrepareValueForDB(_with1.Hostname) + "',";
				_sqlQuery.CommandText += "'" + _with1.Protocol.ToString + "',";
				_sqlQuery.CommandText += "'" + mRemoteNG.Tools.Misc.PrepareValueForDB(_with1.PuttySession) + "',";
				_sqlQuery.CommandText += "'" + _with1.Port + "',";
				_sqlQuery.CommandText += "'" + _with1.UseConsoleSession + "',";
				_sqlQuery.CommandText += "'" + _with1.RenderingEngine.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.ICAEncryption.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.RDPAuthenticationLevel.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.LoadBalanceInfo + "',";
				_sqlQuery.CommandText += "'" + _with1.Colors.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.Resolution.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.AutomaticResize + "',";
				_sqlQuery.CommandText += "'" + _with1.DisplayWallpaper + "',";
				_sqlQuery.CommandText += "'" + _with1.DisplayThemes + "',";
				_sqlQuery.CommandText += "'" + _with1.EnableFontSmoothing + "',";
				_sqlQuery.CommandText += "'" + _with1.EnableDesktopComposition + "',";
				_sqlQuery.CommandText += "'" + _with1.CacheBitmaps + "',";
				_sqlQuery.CommandText += "'" + _with1.RedirectDiskDrives + "',";
				_sqlQuery.CommandText += "'" + _with1.RedirectPorts + "',";
				_sqlQuery.CommandText += "'" + _with1.RedirectPrinters + "',";
				_sqlQuery.CommandText += "'" + _with1.RedirectSmartCards + "',";
				_sqlQuery.CommandText += "'" + _with1.RedirectSound.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.RedirectKeys + "',";

				if (curConI.OpenConnections.Count > 0) {
					_sqlQuery.CommandText += 1 + ",";
				} else {
					_sqlQuery.CommandText += 0 + ",";
				}

				_sqlQuery.CommandText += "'" + _with1.PreExtApp + "',";
				_sqlQuery.CommandText += "'" + _with1.PostExtApp + "',";
				_sqlQuery.CommandText += "'" + _with1.MacAddress + "',";
				_sqlQuery.CommandText += "'" + _with1.UserField + "',";
				_sqlQuery.CommandText += "'" + _with1.ExtApp + "',";

				_sqlQuery.CommandText += "'" + _with1.VNCCompression.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.VNCEncoding.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.VNCAuthMode.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.VNCProxyType.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.VNCProxyIP + "',";
				_sqlQuery.CommandText += "'" + _with1.VNCProxyPort + "',";
				_sqlQuery.CommandText += "'" + _with1.VNCProxyUsername + "',";
				_sqlQuery.CommandText += "'" + mRemoteNG.Security.Crypt.Encrypt(_with1.VNCProxyPassword, _password) + "',";
				_sqlQuery.CommandText += "'" + _with1.VNCColors.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.VNCSmartSizeMode.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.VNCViewOnly + "',";

				_sqlQuery.CommandText += "'" + _with1.RDGatewayUsageMethod.ToString + "',";
				_sqlQuery.CommandText += "'" + _with1.RDGatewayHostname + "',";
				_sqlQuery.CommandText += "'" + _with1.RDGatewayUseConnectionCredentials.ToString + "',";

				if (this._SaveSecurity.Username == true) {
					_sqlQuery.CommandText += "'" + _with1.RDGatewayUsername + "',";
				} else {
					_sqlQuery.CommandText += "'" + "" + "',";
				}

				if (this._SaveSecurity.Password == true) {
					_sqlQuery.CommandText += "'" + mRemoteNG.Security.Crypt.Encrypt(_with1.RDGatewayPassword, _password) + "',";
				} else {
					_sqlQuery.CommandText += "'" + "" + "',";
				}

				if (this._SaveSecurity.Domain == true) {
					_sqlQuery.CommandText += "'" + _with1.RDGatewayDomain + "',";
				} else {
					_sqlQuery.CommandText += "'" + "" + "',";
				}

				_sqlQuery.CommandText += "'" + _with1.UseCredSsp + "',";

				var _with2 = _with1.Inherit;
				if (this._SaveSecurity.Inheritance == true) {
					_sqlQuery.CommandText += "'" + _with2.CacheBitmaps + "',";
					_sqlQuery.CommandText += "'" + _with2.Colors + "',";
					_sqlQuery.CommandText += "'" + _with2.Description + "',";
					_sqlQuery.CommandText += "'" + _with2.DisplayThemes + "',";
					_sqlQuery.CommandText += "'" + _with2.DisplayWallpaper + "',";
					_sqlQuery.CommandText += "'" + _with2.EnableFontSmoothing + "',";
					_sqlQuery.CommandText += "'" + _with2.EnableDesktopComposition + "',";
					_sqlQuery.CommandText += "'" + _with2.Domain + "',";
					_sqlQuery.CommandText += "'" + _with2.Icon + "',";
					_sqlQuery.CommandText += "'" + _with2.Panel + "',";
					_sqlQuery.CommandText += "'" + _with2.Password + "',";
					_sqlQuery.CommandText += "'" + _with2.Port + "',";
					_sqlQuery.CommandText += "'" + _with2.Protocol + "',";
					_sqlQuery.CommandText += "'" + _with2.PuttySession + "',";
					_sqlQuery.CommandText += "'" + _with2.RedirectDiskDrives + "',";
					_sqlQuery.CommandText += "'" + _with2.RedirectKeys + "',";
					_sqlQuery.CommandText += "'" + _with2.RedirectPorts + "',";
					_sqlQuery.CommandText += "'" + _with2.RedirectPrinters + "',";
					_sqlQuery.CommandText += "'" + _with2.RedirectSmartCards + "',";
					_sqlQuery.CommandText += "'" + _with2.RedirectSound + "',";
					_sqlQuery.CommandText += "'" + _with2.Resolution + "',";
					_sqlQuery.CommandText += "'" + _with2.AutomaticResize + "',";
					_sqlQuery.CommandText += "'" + _with2.UseConsoleSession + "',";
					_sqlQuery.CommandText += "'" + _with2.RenderingEngine + "',";
					_sqlQuery.CommandText += "'" + _with2.Username + "',";
					_sqlQuery.CommandText += "'" + _with2.ICAEncryption + "',";
					_sqlQuery.CommandText += "'" + _with2.RDPAuthenticationLevel + "',";
					_sqlQuery.CommandText += "'" + _with2.LoadBalanceInfo + "',";
					_sqlQuery.CommandText += "'" + _with2.PreExtApp + "',";
					_sqlQuery.CommandText += "'" + _with2.PostExtApp + "',";
					_sqlQuery.CommandText += "'" + _with2.MacAddress + "',";
					_sqlQuery.CommandText += "'" + _with2.UserField + "',";
					_sqlQuery.CommandText += "'" + _with2.ExtApp + "',";

					_sqlQuery.CommandText += "'" + _with2.VNCCompression + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCEncoding + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCAuthMode + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCProxyType + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCProxyIP + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCProxyPort + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCProxyUsername + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCProxyPassword + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCColors + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCSmartSizeMode + "',";
					_sqlQuery.CommandText += "'" + _with2.VNCViewOnly + "',";

					_sqlQuery.CommandText += "'" + _with2.RDGatewayUsageMethod + "',";
					_sqlQuery.CommandText += "'" + _with2.RDGatewayHostname + "',";
					_sqlQuery.CommandText += "'" + _with2.RDGatewayUseConnectionCredentials + "',";
					_sqlQuery.CommandText += "'" + _with2.RDGatewayUsername + "',";
					_sqlQuery.CommandText += "'" + _with2.RDGatewayPassword + "',";
					_sqlQuery.CommandText += "'" + _with2.RDGatewayDomain + "',";

					_sqlQuery.CommandText += "'" + _with2.UseCredSsp + "',";
				} else {
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					// .AutomaticResize
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					// .LoadBalanceInfo
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";

					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";
					_sqlQuery.CommandText += "'" + false + "',";

					_sqlQuery.CommandText += "'" + false + "',";
					// .RDGatewayUsageMethod
					_sqlQuery.CommandText += "'" + false + "',";
					// .RDGatewayHostname
					_sqlQuery.CommandText += "'" + false + "',";
					// .RDGatewayUseConnectionCredentials
					_sqlQuery.CommandText += "'" + false + "',";
					// .RDGatewayUsername
					_sqlQuery.CommandText += "'" + false + "',";
					// .RDGatewayPassword
					_sqlQuery.CommandText += "'" + false + "',";
					// .RDGatewayDomain

					_sqlQuery.CommandText += "'" + false + "',";
					// .UseCredSsp
				}

				_with1.PositionID = _currentNodeIndex;

				if (_with1.IsContainer == false) {
					if (_with1.Parent != null) {
						_parentConstantId = (_with1.Parent as Container.Info).ConnectionInfo.ConstantID;
					} else {
						_parentConstantId = 0;
					}
				} else {
					if ((_with1.Parent as Container.Info).Parent != null) {
						_parentConstantId = ((_with1.Parent as Container.Info).Parent as Container.Info).ConnectionInfo.ConstantID;
					} else {
						_parentConstantId = 0;
					}
				}

				_sqlQuery.CommandText += _currentNodeIndex + ",'" + _parentConstantId + "','" + _with1.ConstantID + "','" + mRemoteNG.Tools.Misc.DBDate(DateAndTime.Now) + "')";
			}
			#endregion

			#region "XML"
			private void EncryptCompleteFile()
			{
				StreamReader streamReader = new StreamReader(ConnectionFileName);

				string fileContents = null;
				fileContents = streamReader.ReadToEnd();
				streamReader.Close();

				if (!string.IsNullOrEmpty(fileContents)) {
					StreamWriter streamWriter = new StreamWriter(ConnectionFileName);
					streamWriter.Write(mRemoteNG.Security.Crypt.Encrypt(fileContents, _password));
					streamWriter.Close();
				}
			}

			private void SaveToXml()
			{
				try {
					if (!mRemoteNG.App.Runtime.IsConnectionsFileLoaded)
						return;

					TreeNode treeNode = null;

					if (mRemoteNG.Tree.Node.GetNodeType(RootTreeNode) == mRemoteNG.Tree.Node.Type.Root) {
						treeNode = RootTreeNode.Clone();
					} else {
						treeNode = new TreeNode("mR|Export (" + mRemoteNG.Tools.Misc.DBDate(DateAndTime.Now) + ")");
						treeNode.Nodes.Add(RootTreeNode.Clone());
					}

					string tempFileName = Path.GetTempFileName();
					_xmlTextWriter = new XmlTextWriter(tempFileName, System.Text.Encoding.UTF8);

					_xmlTextWriter.Formatting = Formatting.Indented;
					_xmlTextWriter.Indentation = 4;

					_xmlTextWriter.WriteStartDocument();

					_xmlTextWriter.WriteStartElement("Connections");
					// Do not localize
					_xmlTextWriter.WriteAttributeString("Name", "", treeNode.Text);
					_xmlTextWriter.WriteAttributeString("Export", "", Export);

					if (Export) {
						_xmlTextWriter.WriteAttributeString("Protected", "", mRemoteNG.Security.Crypt.Encrypt("ThisIsNotProtected", _password));
					} else {
						if ((treeNode.Tag as Root.Info).Password == true) {
							_password = (treeNode.Tag as Root.Info).PasswordString;
							_xmlTextWriter.WriteAttributeString("Protected", "", mRemoteNG.Security.Crypt.Encrypt("ThisIsProtected", _password));
						} else {
							_xmlTextWriter.WriteAttributeString("Protected", "", mRemoteNG.Security.Crypt.Encrypt("ThisIsNotProtected", _password));
						}
					}

					_xmlTextWriter.WriteAttributeString("ConfVersion", "", mRemoteNG.App.Info.Connections.ConnectionFileVersion.ToString(CultureInfo.InvariantCulture));

					TreeNodeCollection treeNodeCollection = null;
					treeNodeCollection = treeNode.Nodes;

					SaveNode(treeNodeCollection);

					_xmlTextWriter.WriteEndElement();
					_xmlTextWriter.Close();

					if (File.Exists(ConnectionFileName)) {
						if (Export) {
							File.Delete(ConnectionFileName);
						} else {
							string backupFileName = ConnectionFileName + ".backup";
							File.Delete(backupFileName);
							File.Move(ConnectionFileName, backupFileName);
						}
					}
					File.Move(tempFileName, ConnectionFileName);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "SaveToXml failed" + Constants.vbNewLine + ex.Message, false);
				}
			}

			private void SaveNode(TreeNodeCollection tNC)
			{
				try {
					foreach (TreeNode node in tNC) {
						Connection.Info curConI = null;

						if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Connection | mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Container) {
							_xmlTextWriter.WriteStartElement("Node");
							_xmlTextWriter.WriteAttributeString("Name", "", node.Text);
							_xmlTextWriter.WriteAttributeString("Type", "", mRemoteNG.Tree.Node.GetNodeType(node).ToString());
						}

						//container
						if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Container) {
							_xmlTextWriter.WriteAttributeString("Expanded", "", this._ContainerList(node.Tag).TreeNode.IsExpanded);
							curConI = this._ContainerList(node.Tag).ConnectionInfo;
							SaveConnectionFields(curConI);
							SaveNode(node.Nodes);
							_xmlTextWriter.WriteEndElement();
						}

						if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Connection) {
							curConI = this._ConnectionList(node.Tag);
							SaveConnectionFields(curConI);
							_xmlTextWriter.WriteEndElement();
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "SaveNode failed" + Constants.vbNewLine + ex.Message, true);
				}
			}

			private void SaveConnectionFields(Connection.Info curConI)
			{
				try {
					_xmlTextWriter.WriteAttributeString("Descr", "", curConI.Description);

					_xmlTextWriter.WriteAttributeString("Icon", "", curConI.Icon);

					_xmlTextWriter.WriteAttributeString("Panel", "", curConI.Panel);

					if (this._SaveSecurity.Username == true) {
						_xmlTextWriter.WriteAttributeString("Username", "", curConI.Username);
					} else {
						_xmlTextWriter.WriteAttributeString("Username", "", "");
					}

					if (this._SaveSecurity.Domain == true) {
						_xmlTextWriter.WriteAttributeString("Domain", "", curConI.Domain);
					} else {
						_xmlTextWriter.WriteAttributeString("Domain", "", "");
					}

					if (this._SaveSecurity.Password == true) {
						_xmlTextWriter.WriteAttributeString("Password", "", mRemoteNG.Security.Crypt.Encrypt(curConI.Password, _password));
					} else {
						_xmlTextWriter.WriteAttributeString("Password", "", "");
					}

					_xmlTextWriter.WriteAttributeString("Hostname", "", curConI.Hostname);

					_xmlTextWriter.WriteAttributeString("Protocol", "", curConI.Protocol.ToString());

					_xmlTextWriter.WriteAttributeString("PuttySession", "", curConI.PuttySession);

					_xmlTextWriter.WriteAttributeString("Port", "", curConI.Port);

					_xmlTextWriter.WriteAttributeString("ConnectToConsole", "", curConI.UseConsoleSession);

					_xmlTextWriter.WriteAttributeString("UseCredSsp", "", curConI.UseCredSsp);

					_xmlTextWriter.WriteAttributeString("RenderingEngine", "", curConI.RenderingEngine.ToString());

					_xmlTextWriter.WriteAttributeString("ICAEncryptionStrength", "", curConI.ICAEncryption.ToString());

					_xmlTextWriter.WriteAttributeString("RDPAuthenticationLevel", "", curConI.RDPAuthenticationLevel.ToString());

					_xmlTextWriter.WriteAttributeString("LoadBalanceInfo", "", curConI.LoadBalanceInfo);

					_xmlTextWriter.WriteAttributeString("Colors", "", curConI.Colors.ToString());

					_xmlTextWriter.WriteAttributeString("Resolution", "", curConI.Resolution.ToString());

					_xmlTextWriter.WriteAttributeString("AutomaticResize", "", curConI.AutomaticResize);

					_xmlTextWriter.WriteAttributeString("DisplayWallpaper", "", curConI.DisplayWallpaper);

					_xmlTextWriter.WriteAttributeString("DisplayThemes", "", curConI.DisplayThemes);

					_xmlTextWriter.WriteAttributeString("EnableFontSmoothing", "", curConI.EnableFontSmoothing);

					_xmlTextWriter.WriteAttributeString("EnableDesktopComposition", "", curConI.EnableDesktopComposition);

					_xmlTextWriter.WriteAttributeString("CacheBitmaps", "", curConI.CacheBitmaps);

					_xmlTextWriter.WriteAttributeString("RedirectDiskDrives", "", curConI.RedirectDiskDrives);

					_xmlTextWriter.WriteAttributeString("RedirectPorts", "", curConI.RedirectPorts);

					_xmlTextWriter.WriteAttributeString("RedirectPrinters", "", curConI.RedirectPrinters);

					_xmlTextWriter.WriteAttributeString("RedirectSmartCards", "", curConI.RedirectSmartCards);

					_xmlTextWriter.WriteAttributeString("RedirectSound", "", curConI.RedirectSound.ToString());

					_xmlTextWriter.WriteAttributeString("RedirectKeys", "", curConI.RedirectKeys);

					if (curConI.OpenConnections.Count > 0) {
						_xmlTextWriter.WriteAttributeString("Connected", "", true);
					} else {
						_xmlTextWriter.WriteAttributeString("Connected", "", false);
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
					_xmlTextWriter.WriteAttributeString("VNCProxyPort", "", curConI.VNCProxyPort);
					_xmlTextWriter.WriteAttributeString("VNCProxyUsername", "", curConI.VNCProxyUsername);
					_xmlTextWriter.WriteAttributeString("VNCProxyPassword", "", mRemoteNG.Security.Crypt.Encrypt(curConI.VNCProxyPassword, _password));
					_xmlTextWriter.WriteAttributeString("VNCColors", "", curConI.VNCColors.ToString());
					_xmlTextWriter.WriteAttributeString("VNCSmartSizeMode", "", curConI.VNCSmartSizeMode.ToString());
					_xmlTextWriter.WriteAttributeString("VNCViewOnly", "", curConI.VNCViewOnly);

					_xmlTextWriter.WriteAttributeString("RDGatewayUsageMethod", "", curConI.RDGatewayUsageMethod.ToString());
					_xmlTextWriter.WriteAttributeString("RDGatewayHostname", "", curConI.RDGatewayHostname);

					_xmlTextWriter.WriteAttributeString("RDGatewayUseConnectionCredentials", "", curConI.RDGatewayUseConnectionCredentials.ToString());

					if (this._SaveSecurity.Username == true) {
						_xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", curConI.RDGatewayUsername);
					} else {
						_xmlTextWriter.WriteAttributeString("RDGatewayUsername", "", "");
					}

					if (this._SaveSecurity.Password == true) {
						_xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", mRemoteNG.Security.Crypt.Encrypt(curConI.RDGatewayPassword, _password));
					} else {
						_xmlTextWriter.WriteAttributeString("RDGatewayPassword", "", "");
					}

					if (this._SaveSecurity.Domain == true) {
						_xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", curConI.RDGatewayDomain);
					} else {
						_xmlTextWriter.WriteAttributeString("RDGatewayDomain", "", "");
					}

					if (this._SaveSecurity.Inheritance == true) {
						_xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", curConI.Inherit.CacheBitmaps);
						_xmlTextWriter.WriteAttributeString("InheritColors", "", curConI.Inherit.Colors);
						_xmlTextWriter.WriteAttributeString("InheritDescription", "", curConI.Inherit.Description);
						_xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", curConI.Inherit.DisplayThemes);
						_xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", curConI.Inherit.DisplayWallpaper);
						_xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", curConI.Inherit.EnableFontSmoothing);
						_xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", curConI.Inherit.EnableDesktopComposition);
						_xmlTextWriter.WriteAttributeString("InheritDomain", "", curConI.Inherit.Domain);
						_xmlTextWriter.WriteAttributeString("InheritIcon", "", curConI.Inherit.Icon);
						_xmlTextWriter.WriteAttributeString("InheritPanel", "", curConI.Inherit.Panel);
						_xmlTextWriter.WriteAttributeString("InheritPassword", "", curConI.Inherit.Password);
						_xmlTextWriter.WriteAttributeString("InheritPort", "", curConI.Inherit.Port);
						_xmlTextWriter.WriteAttributeString("InheritProtocol", "", curConI.Inherit.Protocol);
						_xmlTextWriter.WriteAttributeString("InheritPuttySession", "", curConI.Inherit.PuttySession);
						_xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", curConI.Inherit.RedirectDiskDrives);
						_xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", curConI.Inherit.RedirectKeys);
						_xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", curConI.Inherit.RedirectPorts);
						_xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", curConI.Inherit.RedirectPrinters);
						_xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", curConI.Inherit.RedirectSmartCards);
						_xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", curConI.Inherit.RedirectSound);
						_xmlTextWriter.WriteAttributeString("InheritResolution", "", curConI.Inherit.Resolution);
						_xmlTextWriter.WriteAttributeString("InheritAutomaticResize", "", curConI.Inherit.AutomaticResize);
						_xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", curConI.Inherit.UseConsoleSession);
						_xmlTextWriter.WriteAttributeString("InheritUseCredSsp", "", curConI.Inherit.UseCredSsp);
						_xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", curConI.Inherit.RenderingEngine);
						_xmlTextWriter.WriteAttributeString("InheritUsername", "", curConI.Inherit.Username);
						_xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", curConI.Inherit.ICAEncryption);
						_xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", curConI.Inherit.RDPAuthenticationLevel);
						_xmlTextWriter.WriteAttributeString("InheritLoadBalanceInfo", "", curConI.Inherit.LoadBalanceInfo);
						_xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", curConI.Inherit.PreExtApp);
						_xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", curConI.Inherit.PostExtApp);
						_xmlTextWriter.WriteAttributeString("InheritMacAddress", "", curConI.Inherit.MacAddress);
						_xmlTextWriter.WriteAttributeString("InheritUserField", "", curConI.Inherit.UserField);
						_xmlTextWriter.WriteAttributeString("InheritExtApp", "", curConI.Inherit.ExtApp);
						_xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", curConI.Inherit.VNCCompression);
						_xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", curConI.Inherit.VNCEncoding);
						_xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", curConI.Inherit.VNCAuthMode);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", curConI.Inherit.VNCProxyType);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", curConI.Inherit.VNCProxyIP);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", curConI.Inherit.VNCProxyPort);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", curConI.Inherit.VNCProxyUsername);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", curConI.Inherit.VNCProxyPassword);
						_xmlTextWriter.WriteAttributeString("InheritVNCColors", "", curConI.Inherit.VNCColors);
						_xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", curConI.Inherit.VNCSmartSizeMode);
						_xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", curConI.Inherit.VNCViewOnly);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayUsageMethod", "", curConI.Inherit.RDGatewayUsageMethod);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", curConI.Inherit.RDGatewayHostname);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", curConI.Inherit.RDGatewayUseConnectionCredentials);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", curConI.Inherit.RDGatewayUsername);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", curConI.Inherit.RDGatewayPassword);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", curConI.Inherit.RDGatewayDomain);
					} else {
						_xmlTextWriter.WriteAttributeString("InheritCacheBitmaps", "", false);
						_xmlTextWriter.WriteAttributeString("InheritColors", "", false);
						_xmlTextWriter.WriteAttributeString("InheritDescription", "", false);
						_xmlTextWriter.WriteAttributeString("InheritDisplayThemes", "", false);
						_xmlTextWriter.WriteAttributeString("InheritDisplayWallpaper", "", false);
						_xmlTextWriter.WriteAttributeString("InheritEnableFontSmoothing", "", false);
						_xmlTextWriter.WriteAttributeString("InheritEnableDesktopComposition", "", false);
						_xmlTextWriter.WriteAttributeString("InheritDomain", "", false);
						_xmlTextWriter.WriteAttributeString("InheritIcon", "", false);
						_xmlTextWriter.WriteAttributeString("InheritPanel", "", false);
						_xmlTextWriter.WriteAttributeString("InheritPassword", "", false);
						_xmlTextWriter.WriteAttributeString("InheritPort", "", false);
						_xmlTextWriter.WriteAttributeString("InheritProtocol", "", false);
						_xmlTextWriter.WriteAttributeString("InheritPuttySession", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRedirectDiskDrives", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRedirectKeys", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRedirectPorts", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRedirectPrinters", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRedirectSmartCards", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRedirectSound", "", false);
						_xmlTextWriter.WriteAttributeString("InheritResolution", "", false);
						_xmlTextWriter.WriteAttributeString("InheritAutomaticResize", "", false);
						_xmlTextWriter.WriteAttributeString("InheritUseConsoleSession", "", false);
						_xmlTextWriter.WriteAttributeString("InheritUseCredSsp", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRenderingEngine", "", false);
						_xmlTextWriter.WriteAttributeString("InheritUsername", "", false);
						_xmlTextWriter.WriteAttributeString("InheritICAEncryptionStrength", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRDPAuthenticationLevel", "", false);
						_xmlTextWriter.WriteAttributeString("InheritLoadBalanceInfo", "", false);
						_xmlTextWriter.WriteAttributeString("InheritPreExtApp", "", false);
						_xmlTextWriter.WriteAttributeString("InheritPostExtApp", "", false);
						_xmlTextWriter.WriteAttributeString("InheritMacAddress", "", false);
						_xmlTextWriter.WriteAttributeString("InheritUserField", "", false);
						_xmlTextWriter.WriteAttributeString("InheritExtApp", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCCompression", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCEncoding", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCAuthMode", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyType", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyIP", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyPort", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyUsername", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCProxyPassword", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCColors", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCSmartSizeMode", "", false);
						_xmlTextWriter.WriteAttributeString("InheritVNCViewOnly", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayHostname", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayUseConnectionCredentials", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayUsername", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayPassword", "", false);
						_xmlTextWriter.WriteAttributeString("InheritRDGatewayDomain", "", false);
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, "SaveConnectionFields failed" + Constants.vbNewLine + ex.Message, true);
				}
			}
			#endregion

			#region "CSV"

			private StreamWriter csvWr;
			private void SaveTomRCSV()
			{
				if (mRemoteNG.App.Runtime.IsConnectionsFileLoaded == false) {
					return;
				}

				TreeNode tN = null;
				tN = RootTreeNode.Clone();

				TreeNodeCollection tNC = null;
				tNC = tN.Nodes;

				csvWr = new StreamWriter(ConnectionFileName);


				string csvLn = string.Empty;

				csvLn += "Name;Folder;Description;Icon;Panel;";

				if (SaveSecurity.Username) {
					csvLn += "Username;";
				}

				if (SaveSecurity.Password) {
					csvLn += "Password;";
				}

				if (SaveSecurity.Domain) {
					csvLn += "Domain;";
				}

				csvLn += "Hostname;Protocol;PuttySession;Port;ConnectToConsole;UseCredSsp;RenderingEngine;ICAEncryptionStrength;RDPAuthenticationLevel;LoadBalanceInfo;Colors;Resolution;AutomaticResize;DisplayWallpaper;DisplayThemes;EnableFontSmoothing;EnableDesktopComposition;CacheBitmaps;RedirectDiskDrives;RedirectPorts;RedirectPrinters;RedirectSmartCards;RedirectSound;RedirectKeys;PreExtApp;PostExtApp;MacAddress;UserField;ExtApp;VNCCompression;VNCEncoding;VNCAuthMode;VNCProxyType;VNCProxyIP;VNCProxyPort;VNCProxyUsername;VNCProxyPassword;VNCColors;VNCSmartSizeMode;VNCViewOnly;RDGatewayUsageMethod;RDGatewayHostname;RDGatewayUseConnectionCredentials;RDGatewayUsername;RDGatewayPassword;RDGatewayDomain;";

				if (SaveSecurity.Inheritance) {
					csvLn += "InheritCacheBitmaps;InheritColors;InheritDescription;InheritDisplayThemes;InheritDisplayWallpaper;InheritEnableFontSmoothing;InheritEnableDesktopComposition;InheritDomain;InheritIcon;InheritPanel;InheritPassword;InheritPort;InheritProtocol;InheritPuttySession;InheritRedirectDiskDrives;InheritRedirectKeys;InheritRedirectPorts;InheritRedirectPrinters;InheritRedirectSmartCards;InheritRedirectSound;InheritResolution;InheritAutomaticResize;InheritUseConsoleSession;InheritUseCredSsp;InheritRenderingEngine;InheritUsername;InheritICAEncryptionStrength;InheritRDPAuthenticationLevel;InheritLoadBalanceInfo;InheritPreExtApp;InheritPostExtApp;InheritMacAddress;InheritUserField;InheritExtApp;InheritVNCCompression;InheritVNCEncoding;InheritVNCAuthMode;InheritVNCProxyType;InheritVNCProxyIP;InheritVNCProxyPort;InheritVNCProxyUsername;InheritVNCProxyPassword;InheritVNCColors;InheritVNCSmartSizeMode;InheritVNCViewOnly;InheritRDGatewayUsageMethod;InheritRDGatewayHostname;InheritRDGatewayUseConnectionCredentials;InheritRDGatewayUsername;InheritRDGatewayPassword;InheritRDGatewayDomain";
				}

				csvWr.WriteLine(csvLn);

				SaveNodemRCSV(tNC);

				csvWr.Close();
			}

			private void SaveNodemRCSV(TreeNodeCollection tNC)
			{
				foreach (TreeNode node in tNC) {
					if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Connection) {
						Connection.Info curConI = node.Tag;

						WritemRCSVLine(curConI);
					} else if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Container) {
						SaveNodemRCSV(node.Nodes);
					}
				}
			}

			private void WritemRCSVLine(Connection.Info con)
			{
				string nodePath = con.TreeNode.FullPath;

				int firstSlash = nodePath.IndexOf("\\");
				nodePath = nodePath.Remove(0, firstSlash + 1);
				int lastSlash = nodePath.LastIndexOf("\\");

				if (lastSlash > 0) {
					nodePath = nodePath.Remove(lastSlash);
				} else {
					nodePath = "";
				}

				string csvLn = string.Empty;

				csvLn += con.Name + ";" + nodePath + ";" + con.Description + ";" + con.Icon + ";" + con.Panel + ";";

				if (SaveSecurity.Username) {
					csvLn += con.Username + ";";
				}

				if (SaveSecurity.Password) {
					csvLn += con.Password + ";";
				}

				if (SaveSecurity.Domain) {
					csvLn += con.Domain + ";";
				}

				csvLn += con.Hostname + ";" + con.Protocol.ToString() + ";" + con.PuttySession + ";" + con.Port + ";" + con.UseConsoleSession + ";" + con.UseCredSsp + ";" + con.RenderingEngine.ToString() + ";" + con.ICAEncryption.ToString() + ";" + con.RDPAuthenticationLevel.ToString() + ";" + con.LoadBalanceInfo + ";" + con.Colors.ToString() + ";" + con.Resolution.ToString() + ";" + con.AutomaticResize + ";" + con.DisplayWallpaper + ";" + con.DisplayThemes + ";" + con.EnableFontSmoothing + ";" + con.EnableDesktopComposition + ";" + con.CacheBitmaps + ";" + con.RedirectDiskDrives + ";" + con.RedirectPorts + ";" + con.RedirectPrinters + ";" + con.RedirectSmartCards + ";" + con.RedirectSound.ToString() + ";" + con.RedirectKeys + ";" + con.PreExtApp + ";" + con.PostExtApp + ";" + con.MacAddress + ";" + con.UserField + ";" + con.ExtApp + ";" + con.VNCCompression.ToString() + ";" + con.VNCEncoding.ToString() + ";" + con.VNCAuthMode.ToString() + ";" + con.VNCProxyType.ToString() + ";" + con.VNCProxyIP + ";" + con.VNCProxyPort + ";" + con.VNCProxyUsername + ";" + con.VNCProxyPassword + ";" + con.VNCColors.ToString() + ";" + con.VNCSmartSizeMode.ToString() + ";" + con.VNCViewOnly + ";";

				if (SaveSecurity.Inheritance) {
					csvLn += con.Inherit.CacheBitmaps + ";" + con.Inherit.Colors + ";" + con.Inherit.Description + ";" + con.Inherit.DisplayThemes + ";" + con.Inherit.DisplayWallpaper + ";" + con.Inherit.EnableFontSmoothing + ";" + con.Inherit.EnableDesktopComposition + ";" + con.Inherit.Domain + ";" + con.Inherit.Icon + ";" + con.Inherit.Panel + ";" + con.Inherit.Password + ";" + con.Inherit.Port + ";" + con.Inherit.Protocol + ";" + con.Inherit.PuttySession + ";" + con.Inherit.RedirectDiskDrives + ";" + con.Inherit.RedirectKeys + ";" + con.Inherit.RedirectPorts + ";" + con.Inherit.RedirectPrinters + ";" + con.Inherit.RedirectSmartCards + ";" + con.Inherit.RedirectSound + ";" + con.Inherit.Resolution + ";" + con.Inherit.AutomaticResize + ";" + con.Inherit.UseConsoleSession + ";" + con.Inherit.UseCredSsp + ";" + con.Inherit.RenderingEngine + ";" + con.Inherit.Username + ";" + con.Inherit.ICAEncryption + ";" + con.Inherit.RDPAuthenticationLevel + ";" + con.Inherit.LoadBalanceInfo + ";" + con.Inherit.PreExtApp + ";" + con.Inherit.PostExtApp + ";" + con.Inherit.MacAddress + ";" + con.Inherit.UserField + ";" + con.Inherit.ExtApp + ";" + con.Inherit.VNCCompression + ";" + con.Inherit.VNCEncoding + ";" + con.Inherit.VNCAuthMode + ";" + con.Inherit.VNCProxyType + ";" + con.Inherit.VNCProxyIP + ";" + con.Inherit.VNCProxyPort + ";" + con.Inherit.VNCProxyUsername + ";" + con.Inherit.VNCProxyPassword + ";" + con.Inherit.VNCColors + ";" + con.Inherit.VNCSmartSizeMode + ";" + con.Inherit.VNCViewOnly;
				}

				csvWr.WriteLine(csvLn);
			}
			#endregion

			#region "vRD CSV"
			private void SaveTovRDCSV()
			{
				if (mRemoteNG.App.Runtime.IsConnectionsFileLoaded == false) {
					return;
				}

				TreeNode tN = null;
				tN = RootTreeNode.Clone();

				TreeNodeCollection tNC = null;
				tNC = tN.Nodes;

				csvWr = new StreamWriter(ConnectionFileName);

				SaveNodevRDCSV(tNC);

				csvWr.Close();
			}

			private void SaveNodevRDCSV(TreeNodeCollection tNC)
			{
				foreach (TreeNode node in tNC) {
					if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Connection) {
						Connection.Info curConI = node.Tag;

						if (curConI.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP) {
							WritevRDCSVLine(curConI);
						}
					} else if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Container) {
						SaveNodevRDCSV(node.Nodes);
					}
				}
			}

			private void WritevRDCSVLine(Connection.Info con)
			{
				string nodePath = con.TreeNode.FullPath;

				int firstSlash = nodePath.IndexOf("\\");
				nodePath = nodePath.Remove(0, firstSlash + 1);
				int lastSlash = nodePath.LastIndexOf("\\");

				if (lastSlash > 0) {
					nodePath = nodePath.Remove(lastSlash);
				} else {
					nodePath = "";
				}

				csvWr.WriteLine(con.Name + ";" + con.Hostname + ";" + con.MacAddress + ";;" + con.Port + ";" + con.UseConsoleSession + ";" + nodePath);
			}
			#endregion

			#region "vRD VRE"
			private void SaveToVRE()
			{
				if (mRemoteNG.App.Runtime.IsConnectionsFileLoaded == false) {
					return;
				}

				TreeNode tN = null;
				tN = RootTreeNode.Clone();

				TreeNodeCollection tNC = null;
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
				foreach (TreeNode node in tNC) {
					if (mRemoteNG.Tree.Node.GetNodeType(node) == mRemoteNG.Tree.Node.Type.Connection) {
						Connection.Info curConI = node.Tag;

						if (curConI.Protocol == mRemoteNG.Connection.Protocol.Protocols.RDP) {
							_xmlTextWriter.WriteStartElement("Connection");
							_xmlTextWriter.WriteAttributeString("Id", "", "");

							WriteVREitem(curConI);

							_xmlTextWriter.WriteEndElement();
						}
					} else {
						SaveNodeVRE(node.Nodes);
					}
				}
			}

			private void WriteVREitem(Connection.Info con)
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
				_xmlTextWriter.WriteValue("2");
				//----------------------------------------------------------
				_xmlTextWriter.WriteEndElement();

				//Smart Size
				_xmlTextWriter.WriteStartElement("AutoSize");
				_xmlTextWriter.WriteValue(con.Resolution == mRemoteNG.Connection.Protocol.RDP.RDPResolutions.SmartSize);
				_xmlTextWriter.WriteEndElement();

				//SeparateResolutionX
				_xmlTextWriter.WriteStartElement("SeparateResolutionX");
				_xmlTextWriter.WriteValue("1024");
				_xmlTextWriter.WriteEndElement();

				//SeparateResolutionY
				_xmlTextWriter.WriteStartElement("SeparateResolutionY");
				_xmlTextWriter.WriteValue("768");
				_xmlTextWriter.WriteEndElement();

				Rectangle resolution = mRemoteNG.Connection.Protocol.RDP.GetResolutionRectangle(con.Resolution);
				if (resolution.Width == 0)
					resolution.Width = 1024;
				if (resolution.Height == 0)
					resolution.Height = 768;

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
}
