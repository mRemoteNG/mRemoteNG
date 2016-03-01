using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using mRemoteNG.App.Runtime;
using System.Data.SqlClient;
using System.IO;
using mRemoteNG.My;
using PSTaskDialog;

namespace mRemoteNG.Config
{
	namespace Connections
	{
		public class Load
		{
			#region "Private Properties"
			private XmlDocument xDom;
			private double confVersion;

			private string pW = mRemoteNG.App.Info.General.EncryptionKey;
			private SqlConnection sqlCon;
			private SqlCommand sqlQuery;

			private SqlDataReader sqlRd;
				#endregion
			private TreeNode _selectedTreeNode;

			#region "Public Properties"
			private bool _UseSQL;
			public bool UseSQL {
				get { return _UseSQL; }
				set { _UseSQL = value; }
			}

			private string _SQLHost;
			public string SQLHost {
				get { return _SQLHost; }
				set { _SQLHost = value; }
			}

			private string _SQLDatabaseName;
			public string SQLDatabaseName {
				get { return _SQLDatabaseName; }
				set { _SQLDatabaseName = value; }
			}

			private string _SQLUsername;
			public string SQLUsername {
				get { return _SQLUsername; }
				set { _SQLUsername = value; }
			}

			private string _SQLPassword;
			public string SQLPassword {
				get { return _SQLPassword; }
				set { _SQLPassword = value; }
			}

			private bool _SQLUpdate;
			public bool SQLUpdate {
				get { return _SQLUpdate; }
				set { _SQLUpdate = value; }
			}

			private string _PreviousSelected;
			public string PreviousSelected {
				get { return _PreviousSelected; }
				set { _PreviousSelected = value; }
			}

			private string _ConnectionFileName;
			public string ConnectionFileName {
				get { return this._ConnectionFileName; }
				set { this._ConnectionFileName = value; }
			}

			public TreeNode RootTreeNode { get; set; }

			public Connection.List ConnectionList { get; set; }

			private Container.List _ContainerList;
			public Container.List ContainerList {
				get { return this._ContainerList; }
				set { this._ContainerList = value; }
			}

			private Connection.List _PreviousConnectionList;
			public Connection.List PreviousConnectionList {
				get { return _PreviousConnectionList; }
				set { _PreviousConnectionList = value; }
			}

			private Container.List _PreviousContainerList;
			public Container.List PreviousContainerList {
				get { return _PreviousContainerList; }
				set { _PreviousContainerList = value; }
			}
			#endregion

			#region "Public Methods"
			public void Load(bool import)
			{
				if (UseSQL) {
					LoadFromSQL();
				} else {
					string connections = DecryptCompleteFile();
					LoadFromXML(connections, import);
				}

				My.MyProject.Forms.frmMain.UsingSqlServer = UseSQL;
				My.MyProject.Forms.frmMain.ConnectionsFileName = ConnectionFileName;

				if (!import)
					mRemoteNG.Config.Putty.Sessions.AddSessionsToTree();
			}
			#endregion

			#region "SQL"
			private delegate void LoadFromSqlDelegate();
			private void LoadFromSQL()
			{
				if (Windows.treeForm == null || Windows.treeForm.tvConnections == null)
					return;
				if (Windows.treeForm.tvConnections.InvokeRequired) {
					Windows.treeForm.tvConnections.Invoke(new LoadFromSqlDelegate(LoadFromSQL));
					return;
				}

				try {
					mRemoteNG.App.Runtime.IsConnectionsFileLoaded = false;

					if (!string.IsNullOrEmpty(_SQLUsername)) {
						sqlCon = new SqlConnection("Data Source=" + _SQLHost + ";Initial Catalog=" + _SQLDatabaseName + ";User Id=" + _SQLUsername + ";Password=" + _SQLPassword);
					} else {
						sqlCon = new SqlConnection("Data Source=" + _SQLHost + ";Initial Catalog=" + _SQLDatabaseName + ";Integrated Security=True");
					}

					sqlCon.Open();

					sqlQuery = new SqlCommand("SELECT * FROM tblRoot", sqlCon);
					sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);

					sqlRd.Read();

					if (sqlRd.HasRows == false) {
						mRemoteNG.App.Runtime.SaveConnections();

						sqlQuery = new SqlCommand("SELECT * FROM tblRoot", sqlCon);
						sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);

						sqlRd.Read();
					}

					confVersion = Convert.ToDouble(sqlRd["confVersion"], CultureInfo.InvariantCulture);
					const double maxSupportedSchemaVersion = 2.5;
					if (confVersion > maxSupportedSchemaVersion) {
						cTaskDialog.ShowTaskDialogBox(frmMain, Application.ProductName, "Incompatible database schema", string.Format("The database schema on the server is not supported. Please upgrade to a newer version of {0}.", Application.ProductName), string.Format("Schema Version: {1}{0}Highest Supported Version: {2}", Constants.vbNewLine, confVersion.ToString(), maxSupportedSchemaVersion.ToString()), "", "", "", "", eTaskDialogButtons.OK,
						eSysIcons.Error, null);
						throw new Exception(string.Format("Incompatible database schema (schema version {0}).", confVersion));
					}

					RootTreeNode.Name = sqlRd["Name"];

					Root.Info rootInfo = new Root.Info(mRemoteNG.Root.Info.RootType.Connection);
					rootInfo.Name = RootTreeNode.Name;
					rootInfo.TreeNode = RootTreeNode;

					RootTreeNode.Tag = rootInfo;
					RootTreeNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.Root;
					RootTreeNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.Root;

					if (mRemoteNG.Security.Crypt.Decrypt(sqlRd["Protected"], pW) != "ThisIsNotProtected") {
						if (Authenticate(sqlRd["Protected"], false, rootInfo) == false) {
							mRemoteNG.My.Settings.LoadConsFromCustomLocation = false;
							mRemoteNG.My.Settings.CustomConsPath = "";
							RootTreeNode.Remove();
							return;
						}
					}

					sqlRd.Close();

					Windows.treeForm.tvConnections.BeginUpdate();

					// SECTION 3. Populate the TreeView with the DOM nodes.
					AddNodesFromSQL(RootTreeNode);

					RootTreeNode.Expand();

					//expand containers
					foreach (Container.Info contI in this._ContainerList) {
						if (contI.IsExpanded == true) {
							contI.TreeNode.Expand();
						}
					}

					Windows.treeForm.tvConnections.EndUpdate();

					//open connections from last mremote session
					if (mRemoteNG.My.Settings.OpenConsFromLastSession == true & mRemoteNG.My.Settings.NoReconnect == false) {
						foreach (Connection.Info conI in ConnectionList) {
							if (conI.PleaseConnect == true) {
								mRemoteNG.App.Runtime.OpenConnection(conI);
							}
						}
					}

					mRemoteNG.App.Runtime.IsConnectionsFileLoaded = true;
					Windows.treeForm.InitialRefresh();
					SetSelectedNode(_selectedTreeNode);
				} catch (Exception ex) {
					throw;
				} finally {
					if (sqlCon != null) {
						sqlCon.Close();
					}
				}
			}

			private delegate void SetSelectedNodeDelegate(TreeNode treeNode);
			private static void SetSelectedNode(TreeNode treeNode)
			{
				if (mRemoteNG.Tree.Node.TreeView != null && mRemoteNG.Tree.Node.TreeView.InvokeRequired) {
					Windows.treeForm.Invoke(new SetSelectedNodeDelegate(SetSelectedNode), new object[] { treeNode });
					return;
				}
				Windows.treeForm.tvConnections.SelectedNode = treeNode;
			}

			private void AddNodesFromSQL(TreeNode baseNode)
			{
				try {
					sqlCon.Open();
					sqlQuery = new SqlCommand("SELECT * FROM tblCons ORDER BY PositionID ASC", sqlCon);
					sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);

					if (sqlRd.HasRows == false) {
						return;
					}

					TreeNode tNode = null;

					while (sqlRd.Read()) {
						tNode = new TreeNode(sqlRd["Name"]);
						//baseNode.Nodes.Add(tNode)

						if (mRemoteNG.Tree.Node.GetNodeTypeFromString(sqlRd["Type"]) == mRemoteNG.Tree.Node.Type.Connection) {
							Connection.Info conI = GetConnectionInfoFromSQL();
							conI.TreeNode = tNode;
							//conI.Parent = _previousContainer 'NEW

							this._ConnectionList.Add(conI);

							tNode.Tag = conI;

							if (SQLUpdate == true) {
								Connection.Info prevCon = PreviousConnectionList.FindByConstantID(conI.ConstantID);

								if (prevCon != null) {
									foreach (Connection.Protocol.Base prot in prevCon.OpenConnections) {
										prot.InterfaceControl.Info = conI;
										conI.OpenConnections.Add(prot);
									}

									if (conI.OpenConnections.Count > 0) {
										tNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionOpen;
										tNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionOpen;
									} else {
										tNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;
										tNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;
									}
								} else {
									tNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;
									tNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;
								}

								if (conI.ConstantID == _PreviousSelected) {
									_selectedTreeNode = tNode;
								}
							} else {
								tNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;
								tNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;
							}
						} else if (mRemoteNG.Tree.Node.GetNodeTypeFromString(sqlRd["Type"]) == mRemoteNG.Tree.Node.Type.Container) {
							Container.Info contI = new Container.Info();
							//If tNode.Parent IsNot Nothing Then
							//    If Tree.Node.GetNodeType(tNode.Parent) = Tree.Node.Type.Container Then
							//        contI.Parent = tNode.Parent.Tag
							//    End If
							//End If
							//_previousContainer = contI 'NEW
							contI.TreeNode = tNode;

							contI.Name = sqlRd["Name"];

							Connection.Info conI = null;

							conI = GetConnectionInfoFromSQL();

							conI.Parent = contI;
							conI.IsContainer = true;
							contI.ConnectionInfo = conI;

							if (SQLUpdate == true) {
								Container.Info prevCont = PreviousContainerList.FindByConstantID(conI.ConstantID);
								if (prevCont != null) {
									contI.IsExpanded = prevCont.IsExpanded;
								}

								if (conI.ConstantID == _PreviousSelected) {
									_selectedTreeNode = tNode;
								}
							} else {
								if (sqlRd["Expanded"] == true) {
									contI.IsExpanded = true;
								} else {
									contI.IsExpanded = false;
								}
							}

							this._ContainerList.Add(contI);
							this._ConnectionList.Add(conI);

							tNode.Tag = contI;
							tNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.Container;
							tNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.Container;
						}

						string parentId = sqlRd["ParentID"].ToString().Trim();
						if (string.IsNullOrEmpty(parentId) | parentId == "0") {
							baseNode.Nodes.Add(tNode);
						} else {
							TreeNode pNode = mRemoteNG.Tree.Node.GetNodeFromConstantID(sqlRd["ParentID"]);

							if (pNode != null) {
								pNode.Nodes.Add(tNode);

								if (mRemoteNG.Tree.Node.GetNodeType(tNode) == mRemoteNG.Tree.Node.Type.Connection) {
									(tNode.Tag as Connection.Info).Parent = pNode.Tag;
								} else if (mRemoteNG.Tree.Node.GetNodeType(tNode) == mRemoteNG.Tree.Node.Type.Container) {
									(tNode.Tag as Container.Info).Parent = pNode.Tag;
								}
							} else {
								baseNode.Nodes.Add(tNode);
							}
						}

						//AddNodesFromSQL(tNode)
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strAddNodesFromSqlFailed + Constants.vbNewLine + ex.Message, true);
				}
			}

			private Connection.Info GetConnectionInfoFromSQL()
			{
				try {
					Connection.Info conI = new Connection.Info();

					var _with1 = sqlRd;
					conI.PositionID = _with1.Item("PositionID");
					conI.ConstantID = _with1.Item("ConstantID");
					conI.Name = _with1.Item("Name");
					conI.Description = _with1.Item("Description");
					conI.Hostname = _with1.Item("Hostname");
					conI.Username = _with1.Item("Username");
					conI.Password = mRemoteNG.Security.Crypt.Decrypt(_with1.Item("Password"), pW);
					conI.Domain = _with1.Item("DomainName");
					conI.DisplayWallpaper = _with1.Item("DisplayWallpaper");
					conI.DisplayThemes = _with1.Item("DisplayThemes");
					conI.CacheBitmaps = _with1.Item("CacheBitmaps");
					conI.UseConsoleSession = _with1.Item("ConnectToConsole");

					conI.RedirectDiskDrives = _with1.Item("RedirectDiskDrives");
					conI.RedirectPrinters = _with1.Item("RedirectPrinters");
					conI.RedirectPorts = _with1.Item("RedirectPorts");
					conI.RedirectSmartCards = _with1.Item("RedirectSmartCards");
					conI.RedirectKeys = _with1.Item("RedirectKeys");
					conI.RedirectSound = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPSounds), _with1.Item("RedirectSound"));

					conI.Protocol = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.Protocols), _with1.Item("Protocol"));
					conI.Port = _with1.Item("Port");
					conI.PuttySession = _with1.Item("PuttySession");

					conI.Colors = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPColors), _with1.Item("Colors"));
					conI.Resolution = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPResolutions), _with1.Item("Resolution"));

					conI.Inherit = new Connection.Info.Inheritance(conI);
					conI.Inherit.CacheBitmaps = _with1.Item("InheritCacheBitmaps");
					conI.Inherit.Colors = _with1.Item("InheritColors");
					conI.Inherit.Description = _with1.Item("InheritDescription");
					conI.Inherit.DisplayThemes = _with1.Item("InheritDisplayThemes");
					conI.Inherit.DisplayWallpaper = _with1.Item("InheritDisplayWallpaper");
					conI.Inherit.Domain = _with1.Item("InheritDomain");
					conI.Inherit.Icon = _with1.Item("InheritIcon");
					conI.Inherit.Panel = _with1.Item("InheritPanel");
					conI.Inherit.Password = _with1.Item("InheritPassword");
					conI.Inherit.Port = _with1.Item("InheritPort");
					conI.Inherit.Protocol = _with1.Item("InheritProtocol");
					conI.Inherit.PuttySession = _with1.Item("InheritPuttySession");
					conI.Inherit.RedirectDiskDrives = _with1.Item("InheritRedirectDiskDrives");
					conI.Inherit.RedirectKeys = _with1.Item("InheritRedirectKeys");
					conI.Inherit.RedirectPorts = _with1.Item("InheritRedirectPorts");
					conI.Inherit.RedirectPrinters = _with1.Item("InheritRedirectPrinters");
					conI.Inherit.RedirectSmartCards = _with1.Item("InheritRedirectSmartCards");
					conI.Inherit.RedirectSound = _with1.Item("InheritRedirectSound");
					conI.Inherit.Resolution = _with1.Item("InheritResolution");
					conI.Inherit.UseConsoleSession = _with1.Item("InheritUseConsoleSession");
					conI.Inherit.Username = _with1.Item("InheritUsername");

					conI.Icon = _with1.Item("Icon");
					conI.Panel = _with1.Item("Panel");

					//1.6
					if (this.confVersion > 1.5) {
						conI.ICAEncryption = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.ICA.EncryptionStrength), _with1.Item("ICAEncryptionStrength"));
						conI.Inherit.ICAEncryption = _with1.Item("InheritICAEncryptionStrength");

						conI.PreExtApp = _with1.Item("PreExtApp");
						conI.PostExtApp = _with1.Item("PostExtApp");
						conI.Inherit.PreExtApp = _with1.Item("InheritPreExtApp");
						conI.Inherit.PostExtApp = _with1.Item("InheritPostExtApp");
					}

					//1.7
					if (this.confVersion > 1.6) {
						conI.VNCCompression = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.Compression), _with1.Item("VNCCompression"));
						conI.VNCEncoding = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.Encoding), _with1.Item("VNCEncoding"));
						conI.VNCAuthMode = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.AuthMode), _with1.Item("VNCAuthMode"));
						conI.VNCProxyType = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.ProxyType), _with1.Item("VNCProxyType"));
						conI.VNCProxyIP = _with1.Item("VNCProxyIP");
						conI.VNCProxyPort = _with1.Item("VNCProxyPort");
						conI.VNCProxyUsername = _with1.Item("VNCProxyUsername");
						conI.VNCProxyPassword = mRemoteNG.Security.Crypt.Decrypt(_with1.Item("VNCProxyPassword"), pW);
						conI.VNCColors = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.Colors), _with1.Item("VNCColors"));
						conI.VNCSmartSizeMode = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.SmartSizeMode), _with1.Item("VNCSmartSizeMode"));
						conI.VNCViewOnly = _with1.Item("VNCViewOnly");

						conI.Inherit.VNCCompression = _with1.Item("InheritVNCCompression");
						conI.Inherit.VNCEncoding = _with1.Item("InheritVNCEncoding");
						conI.Inherit.VNCAuthMode = _with1.Item("InheritVNCAuthMode");
						conI.Inherit.VNCProxyType = _with1.Item("InheritVNCProxyType");
						conI.Inherit.VNCProxyIP = _with1.Item("InheritVNCProxyIP");
						conI.Inherit.VNCProxyPort = _with1.Item("InheritVNCProxyPort");
						conI.Inherit.VNCProxyUsername = _with1.Item("InheritVNCProxyUsername");
						conI.Inherit.VNCProxyPassword = _with1.Item("InheritVNCProxyPassword");
						conI.Inherit.VNCColors = _with1.Item("InheritVNCColors");
						conI.Inherit.VNCSmartSizeMode = _with1.Item("InheritVNCSmartSizeMode");
						conI.Inherit.VNCViewOnly = _with1.Item("InheritVNCViewOnly");
					}

					//1.8
					if (this.confVersion > 1.7) {
						conI.RDPAuthenticationLevel = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.AuthenticationLevel), _with1.Item("RDPAuthenticationLevel"));

						conI.Inherit.RDPAuthenticationLevel = _with1.Item("InheritRDPAuthenticationLevel");
					}

					//1.9
					if (this.confVersion > 1.8) {
						conI.RenderingEngine = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.HTTPBase.RenderingEngine), _with1.Item("RenderingEngine"));
						conI.MacAddress = _with1.Item("MacAddress");

						conI.Inherit.RenderingEngine = _with1.Item("InheritRenderingEngine");
						conI.Inherit.MacAddress = _with1.Item("InheritMacAddress");
					}

					//2.0
					if (this.confVersion > 1.9) {
						conI.UserField = _with1.Item("UserField");

						conI.Inherit.UserField = _with1.Item("InheritUserField");
					}

					//2.1
					if (this.confVersion > 2.0) {
						conI.ExtApp = _with1.Item("ExtApp");

						conI.Inherit.ExtApp = _with1.Item("InheritExtApp");
					}

					if (this.confVersion >= 2.2) {
						conI.RDGatewayUsageMethod = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod), _with1.Item("RDGatewayUsageMethod"));
						conI.RDGatewayHostname = _with1.Item("RDGatewayHostname");
						conI.RDGatewayUseConnectionCredentials = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials), _with1.Item("RDGatewayUseConnectionCredentials"));
						conI.RDGatewayUsername = _with1.Item("RDGatewayUsername");
						conI.RDGatewayPassword = mRemoteNG.Security.Crypt.Decrypt(_with1.Item("RDGatewayPassword"), pW);
						conI.RDGatewayDomain = _with1.Item("RDGatewayDomain");
						conI.Inherit.RDGatewayUsageMethod = _with1.Item("InheritRDGatewayUsageMethod");
						conI.Inherit.RDGatewayHostname = _with1.Item("InheritRDGatewayHostname");
						conI.Inherit.RDGatewayUsername = _with1.Item("InheritRDGatewayUsername");
						conI.Inherit.RDGatewayPassword = _with1.Item("InheritRDGatewayPassword");
						conI.Inherit.RDGatewayDomain = _with1.Item("InheritRDGatewayDomain");
					}

					if (this.confVersion >= 2.3) {
						conI.EnableFontSmoothing = _with1.Item("EnableFontSmoothing");
						conI.EnableDesktopComposition = _with1.Item("EnableDesktopComposition");
						conI.Inherit.EnableFontSmoothing = _with1.Item("InheritEnableFontSmoothing");
						conI.Inherit.EnableDesktopComposition = _with1.Item("InheritEnableDesktopComposition");
					}

					if (confVersion >= 2.4) {
						conI.UseCredSsp = _with1.Item("UseCredSsp");
						conI.Inherit.UseCredSsp = _with1.Item("InheritUseCredSsp");
					}

					if (confVersion >= 2.5) {
						conI.LoadBalanceInfo = _with1.Item("LoadBalanceInfo");
						conI.AutomaticResize = _with1.Item("AutomaticResize");
						conI.Inherit.LoadBalanceInfo = _with1.Item("InheritLoadBalanceInfo");
						conI.Inherit.AutomaticResize = _with1.Item("InheritAutomaticResize");
					}

					if (SQLUpdate == true) {
						conI.PleaseConnect = _with1.Item("Connected");
					}

					return conI;
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strGetConnectionInfoFromSqlFailed + Constants.vbNewLine + ex.Message, true);
				}

				return null;
			}
			#endregion

			#region "XML"
			private string DecryptCompleteFile()
			{
				StreamReader sRd = new StreamReader(this._ConnectionFileName);

				string strCons = null;
				strCons = sRd.ReadToEnd();
				sRd.Close();

				if (!string.IsNullOrEmpty(strCons)) {
					string strDecr = "";
					bool notDecr = true;

					if (strCons.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>")) {
						strDecr = strCons;
						return strDecr;
					}

					try {
						strDecr = mRemoteNG.Security.Crypt.Decrypt(strCons, pW);

						if (strDecr != strCons) {
							notDecr = false;
						} else {
							notDecr = true;
						}
					} catch (Exception ex) {
						notDecr = true;
					}

					if (notDecr) {
						if (Authenticate(strCons, true) == true) {
							strDecr = mRemoteNG.Security.Crypt.Decrypt(strCons, pW);
							notDecr = false;
						} else {
							notDecr = true;
						}

						if (notDecr == false) {
							return strDecr;
						}
					} else {
						return strDecr;
					}
				}

				return "";
			}

			private void LoadFromXML(string cons, bool import)
			{
				try {
					if (!import)
						mRemoteNG.App.Runtime.IsConnectionsFileLoaded = false;

					// SECTION 1. Create a DOM Document and load the XML data into it.
					this.xDom = new XmlDocument();
					if (!string.IsNullOrEmpty(cons)) {
						xDom.LoadXml(cons);
					} else {
						xDom.Load(this._ConnectionFileName);
					}

					if (xDom.DocumentElement.HasAttribute("ConfVersion")) {
						this.confVersion = Convert.ToDouble(xDom.DocumentElement.Attributes["ConfVersion"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
					} else {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.WarningMsg, mRemoteNG.My.Language.strOldConffile);
					}

					const double maxSupportedConfVersion = 2.5;
					if (confVersion > maxSupportedConfVersion) {
						cTaskDialog.ShowTaskDialogBox(frmMain, Application.ProductName, "Incompatible connection file format", string.Format("The format of this connection file is not supported. Please upgrade to a newer version of {0}.", Application.ProductName), string.Format("{1}{0}File Format Version: {2}{0}Highest Supported Version: {3}", Constants.vbNewLine, ConnectionFileName, confVersion.ToString(), maxSupportedConfVersion.ToString()), "", "", "", "", eTaskDialogButtons.OK,
						eSysIcons.Error, null);
						throw new Exception(string.Format("Incompatible connection file format (file format version {0}).", confVersion));
					}

					// SECTION 2. Initialize the treeview control.
					Root.Info rootInfo = null;
					if (import) {
						rootInfo = null;
					} else {
						string rootNodeName = "";
						if (xDom.DocumentElement.HasAttribute("Name"))
							rootNodeName = xDom.DocumentElement.Attributes["Name"].Value.Trim();
						if (!string.IsNullOrEmpty(rootNodeName)) {
							RootTreeNode.Name = rootNodeName;
						} else {
							RootTreeNode.Name = xDom.DocumentElement.Name;
						}
						RootTreeNode.Text = RootTreeNode.Name;

						rootInfo = new Root.Info(mRemoteNG.Root.Info.RootType.Connection);
						rootInfo.Name = RootTreeNode.Name;
						rootInfo.TreeNode = RootTreeNode;

						RootTreeNode.Tag = rootInfo;
					}

					//1.4
					if (this.confVersion > 1.3) {
						if (mRemoteNG.Security.Crypt.Decrypt(xDom.DocumentElement.Attributes["Protected"].Value, pW) != "ThisIsNotProtected") {
							if (Authenticate(xDom.DocumentElement.Attributes["Protected"].Value, false, rootInfo) == false) {
								mRemoteNG.My.Settings.LoadConsFromCustomLocation = false;
								mRemoteNG.My.Settings.CustomConsPath = "";
								RootTreeNode.Remove();
								return;
							}
						}
					}

					bool isExportFile = false;
					if (confVersion >= 1.0) {
						if (xDom.DocumentElement.Attributes["Export"].Value == true) {
							isExportFile = true;
						}
					}

					if (import & !isExportFile) {
						mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.InformationMsg, mRemoteNG.My.Language.strCannotImportNormalSessionFile);
						return;
					}

					if (!isExportFile) {
						RootTreeNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.Root;
						RootTreeNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.Root;
					}

					Windows.treeForm.tvConnections.BeginUpdate();

					// SECTION 3. Populate the TreeView with the DOM nodes.
					AddNodeFromXml(ref xDom.DocumentElement, ref RootTreeNode);

					RootTreeNode.Expand();

					//expand containers
					foreach (Container.Info contI in this._ContainerList) {
						if (contI.IsExpanded == true) {
							contI.TreeNode.Expand();
						}
					}

					Windows.treeForm.tvConnections.EndUpdate();

					//open connections from last mremote session
					if (mRemoteNG.My.Settings.OpenConsFromLastSession == true & mRemoteNG.My.Settings.NoReconnect == false) {
						foreach (Connection.Info conI in _ConnectionList) {
							if (conI.PleaseConnect == true) {
								mRemoteNG.App.Runtime.OpenConnection(conI);
							}
						}
					}

					RootTreeNode.EnsureVisible();

					if (!import)
						mRemoteNG.App.Runtime.IsConnectionsFileLoaded = true;
					Windows.treeForm.InitialRefresh();
					SetSelectedNode(RootTreeNode);
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.IsConnectionsFileLoaded = false;
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strLoadFromXmlFailed + Constants.vbNewLine + ex.Message + Constants.vbNewLine + ex.StackTrace, true);
					throw;
				}
			}

			private Container.Info _previousContainer;
			private void AddNodeFromXml(ref XmlNode parentXmlNode, ref TreeNode parentTreeNode)
			{
				try {
					// Loop through the XML nodes until the leaf is reached.
					// Add the nodes to the TreeView during the looping process.
					if (parentXmlNode.HasChildNodes()) {
						foreach (XmlNode xmlNode in parentXmlNode.ChildNodes) {
							TreeNode treeNode = new TreeNode(xmlNode.Attributes["Name"].Value);
							parentTreeNode.Nodes.Add(treeNode);

							//connection info
							if (mRemoteNG.Tree.Node.GetNodeTypeFromString(xmlNode.Attributes["Type"].Value) == mRemoteNG.Tree.Node.Type.Connection) {
								Connection.Info connectionInfo = GetConnectionInfoFromXml(xmlNode);
								connectionInfo.TreeNode = treeNode;
								connectionInfo.Parent = _previousContainer;
								//NEW

								ConnectionList.Add(connectionInfo);

								treeNode.Tag = connectionInfo;
								treeNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;
								treeNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.ConnectionClosed;
							//container info
							} else if (mRemoteNG.Tree.Node.GetNodeTypeFromString(xmlNode.Attributes["Type"].Value) == mRemoteNG.Tree.Node.Type.Container) {
								Container.Info containerInfo = new Container.Info();
								if (treeNode.Parent != null) {
									if (mRemoteNG.Tree.Node.GetNodeType(treeNode.Parent) == mRemoteNG.Tree.Node.Type.Container) {
										containerInfo.Parent = treeNode.Parent.Tag;
									}
								}
								_previousContainer = containerInfo;
								//NEW
								containerInfo.TreeNode = treeNode;

								containerInfo.Name = xmlNode.Attributes["Name"].Value;

								if (confVersion >= 0.8) {
									if (xmlNode.Attributes["Expanded"].Value == "True") {
										containerInfo.IsExpanded = true;
									} else {
										containerInfo.IsExpanded = false;
									}
								}

								Connection.Info connectionInfo = null;
								if (confVersion >= 0.9) {
									connectionInfo = GetConnectionInfoFromXml(xmlNode);
								} else {
									connectionInfo = new Connection.Info();
								}

								connectionInfo.Parent = containerInfo;
								connectionInfo.IsContainer = true;
								containerInfo.ConnectionInfo = connectionInfo;

								ContainerList.Add(containerInfo);

								treeNode.Tag = containerInfo;
								treeNode.ImageIndex = mRemoteNG.Images.Enums.TreeImage.Container;
								treeNode.SelectedImageIndex = mRemoteNG.Images.Enums.TreeImage.Container;
							}

							AddNodeFromXml(ref xmlNode, ref treeNode);
						}
					} else {
						string nodeName = "";
						XmlAttribute nameAttribute = parentXmlNode.Attributes["Name"];
						if ((nameAttribute != null))
							nodeName = nameAttribute.Value.Trim();
						if (!string.IsNullOrEmpty(nodeName)) {
							parentTreeNode.Text = nodeName;
						} else {
							parentTreeNode.Text = parentXmlNode.Name;
						}
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, mRemoteNG.My.Language.strAddNodeFromXmlFailed + Constants.vbNewLine + ex.Message + ex.StackTrace, true);
					throw;
				}
			}

			private Connection.Info GetConnectionInfoFromXml(XmlNode xxNode)
			{
				Connection.Info conI = new Connection.Info();

				try {
					var _with2 = xxNode;
					//0.2
					if (this.confVersion > 0.1) {
						conI.Name = _with2.Attributes("Name").Value;
						conI.Description = _with2.Attributes("Descr").Value;
						conI.Hostname = _with2.Attributes("Hostname").Value;
						conI.Username = _with2.Attributes("Username").Value;
						conI.Password = mRemoteNG.Security.Crypt.Decrypt(_with2.Attributes("Password").Value, pW);
						conI.Domain = _with2.Attributes("Domain").Value;
						conI.DisplayWallpaper = _with2.Attributes("DisplayWallpaper").Value;
						conI.DisplayThemes = _with2.Attributes("DisplayThemes").Value;
						conI.CacheBitmaps = _with2.Attributes("CacheBitmaps").Value;

						//1.0 - 0.1
						if (this.confVersion < 1.1) {
							if (_with2.Attributes("Fullscreen").Value == true) {
								conI.Resolution = mRemoteNG.Connection.Protocol.RDP.RDPResolutions.Fullscreen;
							} else {
								conI.Resolution = mRemoteNG.Connection.Protocol.RDP.RDPResolutions.FitToWindow;
							}
						}
					}

					//0.3
					if (this.confVersion > 0.2) {
						if (this.confVersion < 0.7) {
							if (Convert.ToBoolean(_with2.Attributes("UseVNC").Value) == true) {
								conI.Protocol = mRemoteNG.Connection.Protocol.Protocols.VNC;
								conI.Port = _with2.Attributes("VNCPort").Value;
							} else {
								conI.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP;
							}
						}
					} else {
						conI.Port = mRemoteNG.Connection.Protocol.RDP.Defaults.Port;
						conI.Protocol = mRemoteNG.Connection.Protocol.Protocols.RDP;
					}

					//0.4
					if (this.confVersion > 0.3) {
						if (this.confVersion < 0.7) {
							if (Convert.ToBoolean(_with2.Attributes("UseVNC").Value) == true) {
								conI.Port = _with2.Attributes("VNCPort").Value;
							} else {
								conI.Port = _with2.Attributes("RDPPort").Value;
							}
						}

						conI.UseConsoleSession = _with2.Attributes("ConnectToConsole").Value;
					} else {
						if (this.confVersion < 0.7) {
							if (Convert.ToBoolean(_with2.Attributes("UseVNC").Value) == true) {
								conI.Port = mRemoteNG.Connection.Protocol.VNC.Defaults.Port;
							} else {
								conI.Port = mRemoteNG.Connection.Protocol.RDP.Defaults.Port;
							}
						}
						conI.UseConsoleSession = false;
					}

					//0.5 and 0.6
					if (this.confVersion > 0.4) {
						conI.RedirectDiskDrives = _with2.Attributes("RedirectDiskDrives").Value;
						conI.RedirectPrinters = _with2.Attributes("RedirectPrinters").Value;
						conI.RedirectPorts = _with2.Attributes("RedirectPorts").Value;
						conI.RedirectSmartCards = _with2.Attributes("RedirectSmartCards").Value;
					} else {
						conI.RedirectDiskDrives = false;
						conI.RedirectPrinters = false;
						conI.RedirectPorts = false;
						conI.RedirectSmartCards = false;
					}

					//0.7
					if (this.confVersion > 0.6) {
						conI.Protocol = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.Protocols), _with2.Attributes("Protocol").Value);
						conI.Port = _with2.Attributes("Port").Value;
					}

					//1.0
					if (this.confVersion > 0.9) {
						conI.RedirectKeys = _with2.Attributes("RedirectKeys").Value;
					}

					//1.2
					if (this.confVersion > 1.1) {
						conI.PuttySession = _with2.Attributes("PuttySession").Value;
					}

					//1.3
					if (this.confVersion > 1.2) {
						conI.Colors = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPColors), _with2.Attributes("Colors").Value);
						conI.Resolution = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPResolutions), _with2.Attributes("Resolution").Value);
						conI.RedirectSound = mRemoteNG.Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPSounds), _with2.Attributes("RedirectSound").Value);
					} else {
						switch (_with2.Attributes("Colors").Value) {
							case 0:
								conI.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors256;
								break;
							case 1:
								conI.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors16Bit;
								break;
							case 2:
								conI.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors24Bit;
								break;
							case 3:
								conI.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors32Bit;
								break;
							case 4:
								conI.Colors = mRemoteNG.Connection.Protocol.RDP.RDPColors.Colors15Bit;
								break;
						}

						conI.RedirectSound = _with2.Attributes("RedirectSound").Value;
					}

					//1.3
					if (this.confVersion > 1.2) {
						conI.Inherit = new Connection.Info.Inheritance(conI);
						conI.Inherit.CacheBitmaps = _with2.Attributes("InheritCacheBitmaps").Value;
						conI.Inherit.Colors = _with2.Attributes("InheritColors").Value;
						conI.Inherit.Description = _with2.Attributes("InheritDescription").Value;
						conI.Inherit.DisplayThemes = _with2.Attributes("InheritDisplayThemes").Value;
						conI.Inherit.DisplayWallpaper = _with2.Attributes("InheritDisplayWallpaper").Value;
						conI.Inherit.Domain = _with2.Attributes("InheritDomain").Value;
						conI.Inherit.Icon = _with2.Attributes("InheritIcon").Value;
						conI.Inherit.Panel = _with2.Attributes("InheritPanel").Value;
						conI.Inherit.Password = _with2.Attributes("InheritPassword").Value;
						conI.Inherit.Port = _with2.Attributes("InheritPort").Value;
						conI.Inherit.Protocol = _with2.Attributes("InheritProtocol").Value;
						conI.Inherit.PuttySession = _with2.Attributes("InheritPuttySession").Value;
						conI.Inherit.RedirectDiskDrives = _with2.Attributes("InheritRedirectDiskDrives").Value;
						conI.Inherit.RedirectKeys = _with2.Attributes("InheritRedirectKeys").Value;
						conI.Inherit.RedirectPorts = _with2.Attributes("InheritRedirectPorts").Value;
						conI.Inherit.RedirectPrinters = _with2.Attributes("InheritRedirectPrinters").Value;
						conI.Inherit.RedirectSmartCards = _with2.Attributes("InheritRedirectSmartCards").Value;
						conI.Inherit.RedirectSound = _with2.Attributes("InheritRedirectSound").Value;
						conI.Inherit.Resolution = _with2.Attributes("InheritResolution").Value;
						conI.Inherit.UseConsoleSession = _with2.Attributes("InheritUseConsoleSession").Value;
						conI.Inherit.Username = _with2.Attributes("InheritUsername").Value;

						conI.Icon = _with2.Attributes("Icon").Value;
						conI.Panel = _with2.Attributes("Panel").Value;
					} else {
						conI.Inherit = new Connection.Info.Inheritance(conI, _with2.Attributes("Inherit").Value);

						conI.Icon = _with2.Attributes("Icon").Value.Replace(".ico", "");
						conI.Panel = mRemoteNG.My.Language.strGeneral;
					}

					//1.5
					if (this.confVersion > 1.4) {
						conI.PleaseConnect = _with2.Attributes("Connected").Value;
					}

					//1.6
					if (this.confVersion > 1.5) {
						conI.ICAEncryption = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.ICA.EncryptionStrength), _with2.Attributes("ICAEncryptionStrength").Value);
						conI.Inherit.ICAEncryption = _with2.Attributes("InheritICAEncryptionStrength").Value;

						conI.PreExtApp = _with2.Attributes("PreExtApp").Value;
						conI.PostExtApp = _with2.Attributes("PostExtApp").Value;
						conI.Inherit.PreExtApp = _with2.Attributes("InheritPreExtApp").Value;
						conI.Inherit.PostExtApp = _with2.Attributes("InheritPostExtApp").Value;
					}

					//1.7
					if (this.confVersion > 1.6) {
						conI.VNCCompression = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.Compression), _with2.Attributes("VNCCompression").Value);
						conI.VNCEncoding = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.Encoding), _with2.Attributes("VNCEncoding").Value);
						conI.VNCAuthMode = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.AuthMode), _with2.Attributes("VNCAuthMode").Value);
						conI.VNCProxyType = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.ProxyType), _with2.Attributes("VNCProxyType").Value);
						conI.VNCProxyIP = _with2.Attributes("VNCProxyIP").Value;
						conI.VNCProxyPort = _with2.Attributes("VNCProxyPort").Value;
						conI.VNCProxyUsername = _with2.Attributes("VNCProxyUsername").Value;
						conI.VNCProxyPassword = mRemoteNG.Security.Crypt.Decrypt(_with2.Attributes("VNCProxyPassword").Value, pW);
						conI.VNCColors = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.Colors), _with2.Attributes("VNCColors").Value);
						conI.VNCSmartSizeMode = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.SmartSizeMode), _with2.Attributes("VNCSmartSizeMode").Value);
						conI.VNCViewOnly = _with2.Attributes("VNCViewOnly").Value;

						conI.Inherit.VNCCompression = _with2.Attributes("InheritVNCCompression").Value;
						conI.Inherit.VNCEncoding = _with2.Attributes("InheritVNCEncoding").Value;
						conI.Inherit.VNCAuthMode = _with2.Attributes("InheritVNCAuthMode").Value;
						conI.Inherit.VNCProxyType = _with2.Attributes("InheritVNCProxyType").Value;
						conI.Inherit.VNCProxyIP = _with2.Attributes("InheritVNCProxyIP").Value;
						conI.Inherit.VNCProxyPort = _with2.Attributes("InheritVNCProxyPort").Value;
						conI.Inherit.VNCProxyUsername = _with2.Attributes("InheritVNCProxyUsername").Value;
						conI.Inherit.VNCProxyPassword = _with2.Attributes("InheritVNCProxyPassword").Value;
						conI.Inherit.VNCColors = _with2.Attributes("InheritVNCColors").Value;
						conI.Inherit.VNCSmartSizeMode = _with2.Attributes("InheritVNCSmartSizeMode").Value;
						conI.Inherit.VNCViewOnly = _with2.Attributes("InheritVNCViewOnly").Value;
					}

					//1.8
					if (this.confVersion > 1.7) {
						conI.RDPAuthenticationLevel = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.AuthenticationLevel), _with2.Attributes("RDPAuthenticationLevel").Value);

						conI.Inherit.RDPAuthenticationLevel = _with2.Attributes("InheritRDPAuthenticationLevel").Value;
					}

					//1.9
					if (this.confVersion > 1.8) {
						conI.RenderingEngine = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.HTTPBase.RenderingEngine), _with2.Attributes("RenderingEngine").Value);
						conI.MacAddress = _with2.Attributes("MacAddress").Value;

						conI.Inherit.RenderingEngine = _with2.Attributes("InheritRenderingEngine").Value;
						conI.Inherit.MacAddress = _with2.Attributes("InheritMacAddress").Value;
					}

					//2.0
					if (this.confVersion > 1.9) {
						conI.UserField = _with2.Attributes("UserField").Value;
						conI.Inherit.UserField = _with2.Attributes("InheritUserField").Value;
					}

					//2.1
					if (this.confVersion > 2.0) {
						conI.ExtApp = _with2.Attributes("ExtApp").Value;
						conI.Inherit.ExtApp = _with2.Attributes("InheritExtApp").Value;
					}

					//2.2
					if (this.confVersion > 2.1) {
						// Get settings
						conI.RDGatewayUsageMethod = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod), _with2.Attributes("RDGatewayUsageMethod").Value);
						conI.RDGatewayHostname = _with2.Attributes("RDGatewayHostname").Value;
						conI.RDGatewayUseConnectionCredentials = mRemoteNG.Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials), _with2.Attributes("RDGatewayUseConnectionCredentials").Value);
						conI.RDGatewayUsername = _with2.Attributes("RDGatewayUsername").Value;
						conI.RDGatewayPassword = mRemoteNG.Security.Crypt.Decrypt(_with2.Attributes("RDGatewayPassword").Value, pW);
						conI.RDGatewayDomain = _with2.Attributes("RDGatewayDomain").Value;

						// Get inheritance settings
						conI.Inherit.RDGatewayUsageMethod = _with2.Attributes("InheritRDGatewayUsageMethod").Value;
						conI.Inherit.RDGatewayHostname = _with2.Attributes("InheritRDGatewayHostname").Value;
						conI.Inherit.RDGatewayUseConnectionCredentials = _with2.Attributes("InheritRDGatewayUseConnectionCredentials").Value;
						conI.Inherit.RDGatewayUsername = _with2.Attributes("InheritRDGatewayUsername").Value;
						conI.Inherit.RDGatewayPassword = _with2.Attributes("InheritRDGatewayPassword").Value;
						conI.Inherit.RDGatewayDomain = _with2.Attributes("InheritRDGatewayDomain").Value;
					}

					//2.3
					if (this.confVersion > 2.2) {
						// Get settings
						conI.EnableFontSmoothing = _with2.Attributes("EnableFontSmoothing").Value;
						conI.EnableDesktopComposition = _with2.Attributes("EnableDesktopComposition").Value;

						// Get inheritance settings
						conI.Inherit.EnableFontSmoothing = _with2.Attributes("InheritEnableFontSmoothing").Value;
						conI.Inherit.EnableDesktopComposition = _with2.Attributes("InheritEnableDesktopComposition").Value;
					}

					if (confVersion >= 2.4) {
						conI.UseCredSsp = _with2.Attributes("UseCredSsp").Value;
						conI.Inherit.UseCredSsp = _with2.Attributes("InheritUseCredSsp").Value;
					}

					if (confVersion >= 2.5) {
						conI.LoadBalanceInfo = _with2.Attributes("LoadBalanceInfo").Value;
						conI.AutomaticResize = _with2.Attributes("AutomaticResize").Value;
						conI.Inherit.LoadBalanceInfo = _with2.Attributes("InheritLoadBalanceInfo").Value;
						conI.Inherit.AutomaticResize = _with2.Attributes("InheritAutomaticResize").Value;
					}
				} catch (Exception ex) {
					mRemoteNG.App.Runtime.MessageCollector.AddMessage(mRemoteNG.Messages.MessageClass.ErrorMsg, string.Format(mRemoteNG.My.Language.strGetConnectionInfoFromXmlFailed, conI.Name, this.ConnectionFileName, ex.Message), false);
				}
				return conI;
			}

			private bool Authenticate(string Value, bool CompareToOriginalValue, Root.Info rootInfo = null)
			{
				string passwordName = null;
				if (UseSQL) {
					passwordName = Language.strSQLServer.TrimEnd(":");
				} else {
					passwordName = Path.GetFileName(ConnectionFileName);
				}

				if (CompareToOriginalValue) {
					while (!(mRemoteNG.Security.Crypt.Decrypt(Value, pW) != Value)) {
						pW = mRemoteNG.Tools.Misc.PasswordDialog(passwordName, false);

						if (string.IsNullOrEmpty(pW)) {
							return false;
						}
					}
				} else {
					while (!(mRemoteNG.Security.Crypt.Decrypt(Value, pW) == "ThisIsProtected")) {
						pW = mRemoteNG.Tools.Misc.PasswordDialog(passwordName, false);

						if (string.IsNullOrEmpty(pW)) {
							return false;
						}
					}

					if (rootInfo != null) {
						rootInfo.Password = true;
						rootInfo.PasswordString = pW;
					}
				}

				return true;
			}
			#endregion
		}
	}
}
