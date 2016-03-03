using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using mRemoteNG.App;
using System.Data.SqlClient;
using System.IO;
using mRemoteNG.My;
using PSTaskDialog;


namespace mRemoteNG.Config.Connections
{
	public class Load
	{
        #region Private Properties
		private XmlDocument xDom;
		private double confVersion;
		private string pW = "mR3m";
		private SqlConnection sqlCon;
		private SqlCommand sqlQuery;
		private SqlDataReader sqlRd;
		private TreeNode _selectedTreeNode;
        #endregion
				
        #region Public Properties
		private bool _UseSQL;
        public bool UseSQL
		{
			get { return _UseSQL; }
			set { _UseSQL = value; }
		}
				
		private string _SQLHost;
        public string SQLHost
		{
			get { return _SQLHost; }
			set { _SQLHost = value; }
		}
				
		private string _SQLDatabaseName;
        public string SQLDatabaseName
		{
			get { return _SQLDatabaseName; }
			set { _SQLDatabaseName = value; }
		}
				
		private string _SQLUsername;
        public string SQLUsername
		{
			get { return _SQLUsername; }
			set { _SQLUsername = value; }
		}
				
		private string _SQLPassword;
        public string SQLPassword
		{
			get { return _SQLPassword; }
			set { _SQLPassword = value; }
		}
				
		private bool _SQLUpdate;
        public bool SQLUpdate
		{
			get { return _SQLUpdate; }
			set { _SQLUpdate = value; }
		}
				
		private string _PreviousSelected;
        public string PreviousSelected
		{
			get { return _PreviousSelected; }
			set { _PreviousSelected = value; }
		}
				
		private string _ConnectionFileName;
        public string ConnectionFileName
		{
			get { return this._ConnectionFileName; }
			set { this._ConnectionFileName = value; }
		}
				
		public TreeNode RootTreeNode {get; set;}
				
		public Connection.List ConnectionList {get; set;}
				
		private Container.List _ContainerList;
        public Container.List ContainerList
		{
			get { return this._ContainerList; }
			set { this._ContainerList = value; }
		}
				
		private Connection.List _PreviousConnectionList;
        public Connection.List PreviousConnectionList
		{
			get { return _PreviousConnectionList; }
			set { _PreviousConnectionList = value; }
		}
				
		private Container.List _PreviousContainerList;
        public Container.List PreviousContainerList
		{
			get { return _PreviousContainerList; }
			set { _PreviousContainerList = value; }
		}
        #endregion
				
        #region Public Methods
		public void Load_Renamed(bool import)
		{
			if (UseSQL)
			{
				LoadFromSQL();
			}
			else
			{
				string connections = DecryptCompleteFile();
				LoadFromXML(connections, import);
			}
					
			frmMain.Default.UsingSqlServer = UseSQL;
			frmMain.Default.ConnectionsFileName = ConnectionFileName;
					
			if (!import)
			{
				Putty.Sessions.AddSessionsToTree();
			}
		}
        #endregion
				
        #region SQL
		private delegate void LoadFromSqlDelegate();
		private void LoadFromSQL()
		{
            if (Runtime.Windows.treeForm == null || Runtime.Windows.treeForm.tvConnections == null)
			{
				return ;
			}
            if (Runtime.Windows.treeForm.tvConnections.InvokeRequired)
			{
                Runtime.Windows.treeForm.tvConnections.Invoke(new LoadFromSqlDelegate(LoadFromSQL));
				return ;
			}
					
			try
			{
                Runtime.IsConnectionsFileLoaded = false;
						
				if (!string.IsNullOrEmpty(_SQLUsername))
				{
					sqlCon = new SqlConnection("Data Source=" + _SQLHost + ";Initial Catalog=" + _SQLDatabaseName + ";User Id=" + _SQLUsername + ";Password=" + _SQLPassword);
				}
				else
				{
					sqlCon = new SqlConnection("Data Source=" + _SQLHost + ";Initial Catalog=" + _SQLDatabaseName + ";Integrated Security=True");
				}
						
				sqlCon.Open();
						
				sqlQuery = new SqlCommand("SELECT * FROM tblRoot", sqlCon);
				sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
						
				sqlRd.Read();
						
				if (sqlRd.HasRows == false)
				{
                    Runtime.SaveConnections();
							
					sqlQuery = new SqlCommand("SELECT * FROM tblRoot", sqlCon);
					sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
							
					sqlRd.Read();
				}
						
				confVersion = Convert.ToDouble(sqlRd["confVersion"], CultureInfo.InvariantCulture);
				const double maxSupportedSchemaVersion = 2.5;
				if (confVersion > maxSupportedSchemaVersion)
				{
					cTaskDialog.ShowTaskDialogBox(frmMain, System.Windows.Forms.Application.ProductName, "Incompatible database schema", string.Format("The database schema on the server is not supported. Please upgrade to a newer version of {0}.", System.Windows.Forms.Application.ProductName), string.Format("Schema Version: {1}{0}Highest Supported Version: {2}", Constants.vbNewLine, confVersion.ToString(), maxSupportedSchemaVersion.ToString()), "", "", "", "", eTaskDialogButtons.OK, eSysIcons.Error, null);
					throw (new Exception(string.Format("Incompatible database schema (schema version {0}).", confVersion)));
				}
						
				RootTreeNode.Name = System.Convert.ToString(sqlRd["Name"]);
						
				Root.Info rootInfo = new Root.Info(Root.Info.RootType.Connection);
				rootInfo.Name = RootTreeNode.Name;
				rootInfo.TreeNode = RootTreeNode;
						
				RootTreeNode.Tag = rootInfo;
				RootTreeNode.ImageIndex = Images.Enums.TreeImage.Root;
				RootTreeNode.SelectedImageIndex = Images.Enums.TreeImage.Root;
						
				if (Security.Crypt.Decrypt(System.Convert.ToString(sqlRd["Protected"]), pW) != "ThisIsNotProtected")
				{
					if (Authenticate(System.Convert.ToString(sqlRd["Protected"]), false, rootInfo) == false)
					{
						My.Settings.Default.LoadConsFromCustomLocation = false;
						My.Settings.Default.CustomConsPath = "";
						RootTreeNode.Remove();
						return;
					}
				}
						
				sqlRd.Close();

                Runtime.Windows.treeForm.tvConnections.BeginUpdate();
						
				// SECTION 3. Populate the TreeView with the DOM nodes.
				AddNodesFromSQL(RootTreeNode);
						
				RootTreeNode.Expand();
						
				//expand containers
				foreach (Container.Info contI in this._ContainerList)
				{
					if (contI.IsExpanded == true)
					{
						contI.TreeNode.Expand();
					}
				}

                Runtime.Windows.treeForm.tvConnections.EndUpdate();
						
				//open connections from last mremote session
				if (My.Settings.Default.OpenConsFromLastSession == true && My.Settings.Default.NoReconnect == false)
				{
					foreach (Connection.Info conI in ConnectionList)
					{
						if (conI.PleaseConnect == true)
						{
                            Runtime.OpenConnection(conI);
						}
					}
				}

                Runtime.IsConnectionsFileLoaded = true;
                Runtime.Windows.treeForm.InitialRefresh();
				SetSelectedNode(_selectedTreeNode);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (sqlCon != null)
				{
					sqlCon.Close();
				}
			}
		}
				
		private delegate void SetSelectedNodeDelegate(TreeNode treeNode);
		private static void SetSelectedNode(TreeNode treeNode)
		{
			if (Tree.Node.TreeView != null && Tree.Node.TreeView.InvokeRequired)
			{
                Runtime.Windows.treeForm.Invoke(new SetSelectedNodeDelegate(SetSelectedNode), new object[] { treeNode });
				return ;
			}
            Runtime.Windows.treeForm.tvConnections.SelectedNode = treeNode;
		}
				
		private void AddNodesFromSQL(TreeNode baseNode)
		{
			try
			{
				sqlCon.Open();
				sqlQuery = new SqlCommand("SELECT * FROM tblCons ORDER BY PositionID ASC", sqlCon);
				sqlRd = sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
						
				if (sqlRd.HasRows == false)
				{
					return;
				}
						
				TreeNode tNode = default(TreeNode);
						
				while (sqlRd.Read())
				{
					tNode = new TreeNode(System.Convert.ToString(sqlRd["Name"]));
					//baseNode.Nodes.Add(tNode)
							
					if (Tree.Node.GetNodeTypeFromString(System.Convert.ToString(sqlRd["Type"])) == Tree.Node.Type.Connection)
					{
						Connection.Info conI = GetConnectionInfoFromSQL();
						conI.TreeNode = tNode;
						//conI.Parent = _previousContainer 'NEW
								
						this._ConnectionList.Add(conI);
								
						tNode.Tag = conI;
								
						if (SQLUpdate == true)
						{
							Connection.Info prevCon = PreviousConnectionList.FindByConstantID(conI.ConstantID);
									
							if (prevCon != null)
							{
								foreach (Connection.Protocol.Base prot in prevCon.OpenConnections)
								{
									prot.InterfaceControl.Info = conI;
									conI.OpenConnections.Add(prot);
								}
										
								if (conI.OpenConnections.Count > 0)
								{
									tNode.ImageIndex = Images.Enums.TreeImage.ConnectionOpen;
									tNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionOpen;
								}
								else
								{
									tNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed;
									tNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed;
								}
							}
							else
							{
								tNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed;
								tNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed;
							}
									
							if (conI.ConstantID == _PreviousSelected)
							{
								_selectedTreeNode = tNode;
							}
						}
						else
						{
							tNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed;
							tNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed;
						}
					}
					else if (Tree.Node.GetNodeTypeFromString(System.Convert.ToString(sqlRd["Type"])) == Tree.Node.Type.Container)
					{
						Container.Info contI = new Container.Info();
						//If tNode.Parent IsNot Nothing Then
						//    If Tree.Node.GetNodeType(tNode.Parent) = Tree.Node.Type.Container Then
						//        contI.Parent = tNode.Parent.Tag
						//    End If
						//End If
						//_previousContainer = contI 'NEW
						contI.TreeNode = tNode;
								
						contI.Name = System.Convert.ToString(sqlRd["Name"]);
								
						Connection.Info conI = default(Connection.Info);
								
						conI = GetConnectionInfoFromSQL();
								
						conI.Parent = contI;
						conI.IsContainer = true;
						contI.ConnectionInfo = conI;
								
						if (SQLUpdate == true)
						{
							Container.Info prevCont = PreviousContainerList.FindByConstantID(conI.ConstantID);
							if (prevCont != null)
							{
								contI.IsExpanded = prevCont.IsExpanded;
							}
									
							if (conI.ConstantID == _PreviousSelected)
							{
								_selectedTreeNode = tNode;
							}
						}
						else
						{
							if (sqlRd["Expanded"] == true)
							{
								contI.IsExpanded = true;
							}
							else
							{
								contI.IsExpanded = false;
							}
						}
								
						this._ContainerList.Add(contI);
						this._ConnectionList.Add(conI);
								
						tNode.Tag = contI;
						tNode.ImageIndex = Images.Enums.TreeImage.Container;
						tNode.SelectedImageIndex = Images.Enums.TreeImage.Container;
					}
							
					string parentId = System.Convert.ToString(sqlRd["ParentID"].ToString().Trim());
					if (string.IsNullOrEmpty(parentId) || parentId == "0")
					{
						baseNode.Nodes.Add(tNode);
					}
					else
					{
						TreeNode pNode = Tree.Node.GetNodeFromConstantID(System.Convert.ToString(sqlRd["ParentID"]));
								
						if (pNode != null)
						{
							pNode.Nodes.Add(tNode);
									
							if (Tree.Node.GetNodeType(tNode) == Tree.Node.Type.Connection)
							{
								(tNode.Tag as Connection.Info).Parent = pNode.Tag;
							}
							else if (Tree.Node.GetNodeType(tNode) == Tree.Node.Type.Container)
							{
								(tNode.Tag as Container.Info).Parent = pNode.Tag;
							}
						}
						else
						{
							baseNode.Nodes.Add(tNode);
						}
					}
							
					//AddNodesFromSQL(tNode)
				}
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strAddNodesFromSqlFailed + Constants.vbNewLine + ex.Message, true);
			}
		}
				
		private Connection.Info GetConnectionInfoFromSQL()
		{
			try
			{
				Connection.Info conI = new Connection.Info();
						
				conI.PositionID = System.Convert.ToInt32(sqlRd["PositionID"]);
				conI.ConstantID = System.Convert.ToString(sqlRd["ConstantID"]);
				conI.Name = System.Convert.ToString(sqlRd["Name"]);
				conI.Description = System.Convert.ToString(sqlRd["Description"]);
				conI.Hostname = System.Convert.ToString(sqlRd["Hostname"]);
				conI.Username = System.Convert.ToString(sqlRd["Username"]);
				conI.Password = Security.Crypt.Decrypt(System.Convert.ToString(sqlRd["Password"]), pW);
				conI.Domain = System.Convert.ToString(sqlRd["DomainName"]);
				conI.DisplayWallpaper = System.Convert.ToBoolean(sqlRd["DisplayWallpaper"]);
				conI.DisplayThemes = System.Convert.ToBoolean(sqlRd["DisplayThemes"]);
				conI.CacheBitmaps = System.Convert.ToBoolean(sqlRd["CacheBitmaps"]);
				conI.UseConsoleSession = System.Convert.ToBoolean(sqlRd["ConnectToConsole"]);
						
				conI.RedirectDiskDrives = System.Convert.ToBoolean(sqlRd["RedirectDiskDrives"]);
				conI.RedirectPrinters = System.Convert.ToBoolean(sqlRd["RedirectPrinters"]);
				conI.RedirectPorts = System.Convert.ToBoolean(sqlRd["RedirectPorts"]);
				conI.RedirectSmartCards = System.Convert.ToBoolean(sqlRd["RedirectSmartCards"]);
				conI.RedirectKeys = System.Convert.ToBoolean(sqlRd["RedirectKeys"]);
				conI.RedirectSound = Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPSounds), System.Convert.ToString(sqlRd["RedirectSound"]));
						
				conI.Protocol = Tools.Misc.StringToEnum(typeof(Connection.Protocol.Protocols), System.Convert.ToString(sqlRd["Protocol"]));
				conI.Port = System.Convert.ToInt32(sqlRd["Port"]);
				conI.PuttySession = System.Convert.ToString(sqlRd["PuttySession"]);
						
				conI.Colors = Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPColors), System.Convert.ToString(sqlRd["Colors"]));
				conI.Resolution = Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPResolutions), System.Convert.ToString(sqlRd["Resolution"]));
						
				conI.Inherit = new Connection.Info.Inheritance(conI);
				conI.Inherit.CacheBitmaps = System.Convert.ToBoolean(sqlRd["InheritCacheBitmaps"]);
				conI.Inherit.Colors = System.Convert.ToBoolean(sqlRd["InheritColors"]);
				conI.Inherit.Description = System.Convert.ToBoolean(sqlRd["InheritDescription"]);
				conI.Inherit.DisplayThemes = System.Convert.ToBoolean(sqlRd["InheritDisplayThemes"]);
				conI.Inherit.DisplayWallpaper = System.Convert.ToBoolean(sqlRd["InheritDisplayWallpaper"]);
				conI.Inherit.Domain = System.Convert.ToBoolean(sqlRd["InheritDomain"]);
				conI.Inherit.Icon = System.Convert.ToBoolean(sqlRd["InheritIcon"]);
				conI.Inherit.Panel = System.Convert.ToBoolean(sqlRd["InheritPanel"]);
				conI.Inherit.Password = System.Convert.ToBoolean(sqlRd["InheritPassword"]);
				conI.Inherit.Port = System.Convert.ToBoolean(sqlRd["InheritPort"]);
				conI.Inherit.Protocol = System.Convert.ToBoolean(sqlRd["InheritProtocol"]);
				conI.Inherit.PuttySession = System.Convert.ToBoolean(sqlRd["InheritPuttySession"]);
				conI.Inherit.RedirectDiskDrives = System.Convert.ToBoolean(sqlRd["InheritRedirectDiskDrives"]);
				conI.Inherit.RedirectKeys = System.Convert.ToBoolean(sqlRd["InheritRedirectKeys"]);
				conI.Inherit.RedirectPorts = System.Convert.ToBoolean(sqlRd["InheritRedirectPorts"]);
				conI.Inherit.RedirectPrinters = System.Convert.ToBoolean(sqlRd["InheritRedirectPrinters"]);
				conI.Inherit.RedirectSmartCards = System.Convert.ToBoolean(sqlRd["InheritRedirectSmartCards"]);
				conI.Inherit.RedirectSound = System.Convert.ToBoolean(sqlRd["InheritRedirectSound"]);
				conI.Inherit.Resolution = System.Convert.ToBoolean(sqlRd["InheritResolution"]);
				conI.Inherit.UseConsoleSession = System.Convert.ToBoolean(sqlRd["InheritUseConsoleSession"]);
				conI.Inherit.Username = System.Convert.ToBoolean(sqlRd["InheritUsername"]);
						
				conI.Icon = System.Convert.ToString(sqlRd["Icon"]);
				conI.Panel = System.Convert.ToString(sqlRd["Panel"]);
						
				if (this.confVersion > 1.5) //1.6
				{
					conI.ICAEncryption = Tools.Misc.StringToEnum(typeof(Connection.Protocol.ICA.EncryptionStrength), System.Convert.ToString(sqlRd["ICAEncryptionStrength"]));
					conI.Inherit.ICAEncryption = System.Convert.ToBoolean(sqlRd["InheritICAEncryptionStrength"]);
							
					conI.PreExtApp = System.Convert.ToString(sqlRd["PreExtApp"]);
					conI.PostExtApp = System.Convert.ToString(sqlRd["PostExtApp"]);
					conI.Inherit.PreExtApp = System.Convert.ToBoolean(sqlRd["InheritPreExtApp"]);
					conI.Inherit.PostExtApp = System.Convert.ToBoolean(sqlRd["InheritPostExtApp"]);
				}
						
				if (this.confVersion > 1.6) //1.7
				{
					conI.VNCCompression = Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.Compression), System.Convert.ToString(sqlRd["VNCCompression"]));
					conI.VNCEncoding = Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.Encoding), System.Convert.ToString(sqlRd["VNCEncoding"]));
					conI.VNCAuthMode = Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.AuthMode), System.Convert.ToString(sqlRd["VNCAuthMode"]));
					conI.VNCProxyType = Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.ProxyType), System.Convert.ToString(sqlRd["VNCProxyType"]));
					conI.VNCProxyIP = System.Convert.ToString(sqlRd["VNCProxyIP"]);
					conI.VNCProxyPort = System.Convert.ToInt32(sqlRd["VNCProxyPort"]);
					conI.VNCProxyUsername = System.Convert.ToString(sqlRd["VNCProxyUsername"]);
					conI.VNCProxyPassword = Security.Crypt.Decrypt(System.Convert.ToString(sqlRd["VNCProxyPassword"]), pW);
					conI.VNCColors = Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.Colors), System.Convert.ToString(sqlRd["VNCColors"]));
					conI.VNCSmartSizeMode = Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.SmartSizeMode), System.Convert.ToString(sqlRd["VNCSmartSizeMode"]));
					conI.VNCViewOnly = System.Convert.ToBoolean(sqlRd["VNCViewOnly"]);
							
					conI.Inherit.VNCCompression = System.Convert.ToBoolean(sqlRd["InheritVNCCompression"]);
					conI.Inherit.VNCEncoding = System.Convert.ToBoolean(sqlRd["InheritVNCEncoding"]);
					conI.Inherit.VNCAuthMode = System.Convert.ToBoolean(sqlRd["InheritVNCAuthMode"]);
					conI.Inherit.VNCProxyType = System.Convert.ToBoolean(sqlRd["InheritVNCProxyType"]);
					conI.Inherit.VNCProxyIP = System.Convert.ToBoolean(sqlRd["InheritVNCProxyIP"]);
					conI.Inherit.VNCProxyPort = System.Convert.ToBoolean(sqlRd["InheritVNCProxyPort"]);
					conI.Inherit.VNCProxyUsername = System.Convert.ToBoolean(sqlRd["InheritVNCProxyUsername"]);
					conI.Inherit.VNCProxyPassword = System.Convert.ToBoolean(sqlRd["InheritVNCProxyPassword"]);
					conI.Inherit.VNCColors = System.Convert.ToBoolean(sqlRd["InheritVNCColors"]);
					conI.Inherit.VNCSmartSizeMode = System.Convert.ToBoolean(sqlRd["InheritVNCSmartSizeMode"]);
					conI.Inherit.VNCViewOnly = System.Convert.ToBoolean(sqlRd["InheritVNCViewOnly"]);
				}
						
				if (this.confVersion > 1.7) //1.8
				{
					conI.RDPAuthenticationLevel = Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.AuthenticationLevel), System.Convert.ToString(sqlRd["RDPAuthenticationLevel"]));
							
					conI.Inherit.RDPAuthenticationLevel = System.Convert.ToBoolean(sqlRd["InheritRDPAuthenticationLevel"]);
				}
						
				if (this.confVersion > 1.8) //1.9
				{
					conI.RenderingEngine = Tools.Misc.StringToEnum(typeof(Connection.Protocol.HTTPBase.RenderingEngine), System.Convert.ToString(sqlRd["RenderingEngine"]));
					conI.MacAddress = System.Convert.ToString(sqlRd["MacAddress"]);
							
					conI.Inherit.RenderingEngine = System.Convert.ToBoolean(sqlRd["InheritRenderingEngine"]);
					conI.Inherit.MacAddress = System.Convert.ToBoolean(sqlRd["InheritMacAddress"]);
				}
						
				if (this.confVersion > 1.9) //2.0
				{
					conI.UserField = System.Convert.ToString(sqlRd["UserField"]);
							
					conI.Inherit.UserField = System.Convert.ToBoolean(sqlRd["InheritUserField"]);
				}
						
				if (this.confVersion > 2.0) //2.1
				{
					conI.ExtApp = System.Convert.ToString(sqlRd["ExtApp"]);
							
					conI.Inherit.ExtApp = System.Convert.ToBoolean(sqlRd["InheritExtApp"]);
				}
						
				if (this.confVersion >= 2.2)
				{
					conI.RDGatewayUsageMethod = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod), System.Convert.ToString(sqlRd["RDGatewayUsageMethod"]));
					conI.RDGatewayHostname = System.Convert.ToString(sqlRd["RDGatewayHostname"]);
					conI.RDGatewayUseConnectionCredentials = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials), System.Convert.ToString(sqlRd["RDGatewayUseConnectionCredentials"]));
					conI.RDGatewayUsername = System.Convert.ToString(sqlRd["RDGatewayUsername"]);
					conI.RDGatewayPassword = Security.Crypt.Decrypt(System.Convert.ToString(sqlRd["RDGatewayPassword"]), pW);
					conI.RDGatewayDomain = System.Convert.ToString(sqlRd["RDGatewayDomain"]);
					conI.Inherit.RDGatewayUsageMethod = System.Convert.ToBoolean(sqlRd["InheritRDGatewayUsageMethod"]);
					conI.Inherit.RDGatewayHostname = System.Convert.ToBoolean(sqlRd["InheritRDGatewayHostname"]);
					conI.Inherit.RDGatewayUsername = System.Convert.ToBoolean(sqlRd["InheritRDGatewayUsername"]);
					conI.Inherit.RDGatewayPassword = System.Convert.ToBoolean(sqlRd["InheritRDGatewayPassword"]);
					conI.Inherit.RDGatewayDomain = System.Convert.ToBoolean(sqlRd["InheritRDGatewayDomain"]);
				}
						
				if (this.confVersion >= 2.3)
				{
					conI.EnableFontSmoothing = System.Convert.ToBoolean(sqlRd["EnableFontSmoothing"]);
					conI.EnableDesktopComposition = System.Convert.ToBoolean(sqlRd["EnableDesktopComposition"]);
					conI.Inherit.EnableFontSmoothing = System.Convert.ToBoolean(sqlRd["InheritEnableFontSmoothing"]);
					conI.Inherit.EnableDesktopComposition = System.Convert.ToBoolean(sqlRd["InheritEnableDesktopComposition"]);
				}
						
				if (confVersion >= 2.4)
				{
					conI.UseCredSsp = System.Convert.ToBoolean(sqlRd["UseCredSsp"]);
					conI.Inherit.UseCredSsp = System.Convert.ToBoolean(sqlRd["InheritUseCredSsp"]);
				}
						
				if (confVersion >= 2.5)
				{
					conI.LoadBalanceInfo = System.Convert.ToString(sqlRd["LoadBalanceInfo"]);
					conI.AutomaticResize = System.Convert.ToBoolean(sqlRd["AutomaticResize"]);
					conI.Inherit.LoadBalanceInfo = System.Convert.ToBoolean(sqlRd["InheritLoadBalanceInfo"]);
					conI.Inherit.AutomaticResize = System.Convert.ToBoolean(sqlRd["InheritAutomaticResize"]);
				}
						
				if (SQLUpdate == true)
				{
					conI.PleaseConnect = System.Convert.ToBoolean(sqlRd["Connected"]);
				}
						
				return conI;
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strGetConnectionInfoFromSqlFailed + Constants.vbNewLine + ex.Message, true);
			}
					
			return null;
		}
        #endregion
				
        #region XML
		private string DecryptCompleteFile()
		{
			StreamReader sRd = new StreamReader(this._ConnectionFileName);
					
			string strCons = "";
			strCons = sRd.ReadToEnd();
			sRd.Close();
					
			if (!string.IsNullOrEmpty(strCons))
			{
				string strDecr = "";
				bool notDecr = true;
						
				if (strCons.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"))
				{
					strDecr = strCons;
					return strDecr;
				}
						
				try
				{
					strDecr = Security.Crypt.Decrypt(strCons, pW);
							
					if (strDecr != strCons)
					{
						notDecr = false;
					}
					else
					{
						notDecr = true;
					}
				}
				catch (Exception)
				{
					notDecr = true;
				}
						
				if (notDecr)
				{
					if (Authenticate(strCons, true) == true)
					{
						strDecr = Security.Crypt.Decrypt(strCons, pW);
						notDecr = false;
					}
					else
					{
						notDecr = true;
					}
							
					if (notDecr == false)
					{
						return strDecr;
					}
				}
				else
				{
					return strDecr;
				}
			}
					
			return "";
		}
				
		private void LoadFromXML(string cons, bool import)
		{
			try
			{
				if (!import)
				{
					IsConnectionsFileLoaded = false;
				}
						
				// SECTION 1. Create a DOM Document and load the XML data into it.
				this.xDom = new XmlDocument();
				if (cons != "")
				{
					xDom.LoadXml(cons);
				}
				else
				{
					xDom.Load(this._ConnectionFileName);
				}
						
				if (xDom.DocumentElement.HasAttribute("ConfVersion"))
				{
					this.confVersion = Convert.ToDouble(xDom.DocumentElement.Attributes["ConfVersion"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
				}
				else
				{
					MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strOldConffile);
				}
						
				const double maxSupportedConfVersion = 2.5;
				if (confVersion > maxSupportedConfVersion)
				{
					cTaskDialog.ShowTaskDialogBox(frmMain, System.Windows.Forms.Application.ProductName, "Incompatible connection file format", string.Format("The format of this connection file is not supported. Please upgrade to a newer version of {0}.", System.Windows.Forms.Application.ProductName), string.Format("{1}{0}File Format Version: {2}{0}Highest Supported Version: {3}", Constants.vbNewLine, ConnectionFileName, confVersion.ToString(), maxSupportedConfVersion.ToString()), "", "", "", "", eTaskDialogButtons.OK, eSysIcons.Error, null);
					throw (new Exception(string.Format("Incompatible connection file format (file format version {0}).", confVersion)));
				}
						
				// SECTION 2. Initialize the treeview control.
				Root.Info rootInfo = default(Root.Info);
				if (import)
				{
					rootInfo = null;
				}
				else
				{
					string rootNodeName = "";
					if (xDom.DocumentElement.HasAttribute("Name"))
					{
						rootNodeName = System.Convert.ToString(xDom.DocumentElement.Attributes["Name"].Value.Trim());
					}
					if (!string.IsNullOrEmpty(rootNodeName))
					{
						RootTreeNode.Name = rootNodeName;
					}
					else
					{
						RootTreeNode.Name = xDom.DocumentElement.Name;
					}
					RootTreeNode.Text = RootTreeNode.Name;
							
					rootInfo = new Root.Info(Root.Info.RootType.Connection);
					rootInfo.Name = RootTreeNode.Name;
					rootInfo.TreeNode = RootTreeNode;
							
					RootTreeNode.Tag = rootInfo;
				}
						
				if (this.confVersion > 1.3) //1.4
				{
					if (Security.Crypt.Decrypt(System.Convert.ToString(xDom.DocumentElement.Attributes["Protected"].Value), pW) != "ThisIsNotProtected")
					{
						if (Authenticate(System.Convert.ToString(xDom.DocumentElement.Attributes["Protected"].Value), false, rootInfo) == false)
						{
							My.Settings.Default.LoadConsFromCustomLocation = false;
							My.Settings.Default.CustomConsPath = "";
							RootTreeNode.Remove();
							return;
						}
					}
				}
						
				bool isExportFile = false;
				if (confVersion >= 1.0)
				{
					if (xDom.DocumentElement.Attributes["Export"].Value == true)
					{
						isExportFile = true;
					}
				}
						
				if (import && !isExportFile)
				{
					MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strCannotImportNormalSessionFile);
					return ;
				}
						
				if (!isExportFile)
				{
					RootTreeNode.ImageIndex = Images.Enums.TreeImage.Root;
					RootTreeNode.SelectedImageIndex = Images.Enums.TreeImage.Root;
				}
						
				Windows.treeForm.tvConnections.BeginUpdate();
						
				// SECTION 3. Populate the TreeView with the DOM nodes.
				AddNodeFromXml(xDom.DocumentElement, RootTreeNode);
						
				RootTreeNode.Expand();
						
				//expand containers
				foreach (Container.Info contI in this._ContainerList)
				{
					if (contI.IsExpanded == true)
					{
						contI.TreeNode.Expand();
					}
				}
						
				Windows.treeForm.tvConnections.EndUpdate();
						
				//open connections from last mremote session
				if (My.Settings.Default.OpenConsFromLastSession == true && My.Settings.Default.NoReconnect == false)
				{
					foreach (Connection.Info conI in ConnectionList)
					{
						if (conI.PleaseConnect == true)
						{
							OpenConnection(conI);
						}
					}
				}
						
				RootTreeNode.EnsureVisible();
						
				if (!import)
				{
					IsConnectionsFileLoaded = true;
				}
				Windows.treeForm.InitialRefresh();
				SetSelectedNode(RootTreeNode);
			}
			catch (Exception ex)
			{
				App.Runtime.IsConnectionsFileLoaded = false;
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strLoadFromXmlFailed + Constants.vbNewLine + ex.Message + Constants.vbNewLine + ex.StackTrace, true);
				throw;
			}
		}
				
		private Container.Info _previousContainer;
		private void AddNodeFromXml(XmlNode parentXmlNode, TreeNode parentTreeNode)
		{
			try
			{
				// Loop through the XML nodes until the leaf is reached.
				// Add the nodes to the TreeView during the looping process.
				if (parentXmlNode.HasChildNodes)
				{
					foreach (XmlNode xmlNode in parentXmlNode.ChildNodes)
					{
						TreeNode treeNode = new TreeNode(xmlNode.Attributes["Name"].Value);
						parentTreeNode.Nodes.Add(treeNode);
								
						if (Tree.Node.GetNodeTypeFromString(xmlNode.Attributes["Type"].Value) == Tree.Node.Type.Connection) //connection info
						{
							Connection.Info connectionInfo = GetConnectionInfoFromXml(xmlNode);
							connectionInfo.TreeNode = treeNode;
							connectionInfo.Parent = _previousContainer; //NEW
									
							ConnectionList.Add(connectionInfo);
									
							treeNode.Tag = connectionInfo;
							treeNode.ImageIndex = Images.Enums.TreeImage.ConnectionClosed;
							treeNode.SelectedImageIndex = Images.Enums.TreeImage.ConnectionClosed;
						}
						else if (Tree.Node.GetNodeTypeFromString(xmlNode.Attributes["Type"].Value) == Tree.Node.Type.Container) //container info
						{
							Container.Info containerInfo = new Container.Info();
							if (treeNode.Parent != null)
							{
								if (Tree.Node.GetNodeType(treeNode.Parent) == Tree.Node.Type.Container)
								{
									containerInfo.Parent = treeNode.Parent.Tag;
								}
							}
							_previousContainer = containerInfo; //NEW
							containerInfo.TreeNode = treeNode;
									
							containerInfo.Name = xmlNode.Attributes["Name"].Value;
									
							if (confVersion >= 0.8)
							{
								if (xmlNode.Attributes["Expanded"].Value == "True")
								{
									containerInfo.IsExpanded = true;
								}
								else
								{
									containerInfo.IsExpanded = false;
								}
							}
									
							Connection.Info connectionInfo = default(Connection.Info);
							if (confVersion >= 0.9)
							{
								connectionInfo = GetConnectionInfoFromXml(xmlNode);
							}
							else
							{
								connectionInfo = new Connection.Info();
							}
									
							connectionInfo.Parent = containerInfo;
							connectionInfo.IsContainer = true;
							containerInfo.ConnectionInfo = connectionInfo;
									
							ContainerList.Add(containerInfo);
									
							treeNode.Tag = containerInfo;
							treeNode.ImageIndex = Images.Enums.TreeImage.Container;
							treeNode.SelectedImageIndex = Images.Enums.TreeImage.Container;
						}
								
						AddNodeFromXml(xmlNode, treeNode);
					}
				}
				else
				{
					string nodeName = "";
					XmlAttribute nameAttribute = parentXmlNode.Attributes["Name"];
					if (!(nameAttribute == null))
					{
						nodeName = nameAttribute.Value.Trim();
					}
					if (!string.IsNullOrEmpty(nodeName))
					{
						parentTreeNode.Text = nodeName;
					}
					else
					{
						parentTreeNode.Text = parentXmlNode.Name;
					}
				}
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strAddNodeFromXmlFailed + Constants.vbNewLine + ex.Message + ex.StackTrace, true);
				throw;
			}
		}
				
		private Connection.Info GetConnectionInfoFromXml(XmlNode xxNode)
		{
			Connection.Info conI = new Connection.Info();
					
			try
			{
				XmlNode with_1 = xxNode;
				if (this.confVersion > 0.1) //0.2
				{
					conI.Name = with_1.Attributes["Name"].Value;
					conI.Description = with_1.Attributes["Descr"].Value;
					conI.Hostname = with_1.Attributes["Hostname"].Value;
					conI.Username = with_1.Attributes["Username"].Value;
					conI.Password = Security.Crypt.Decrypt(with_1.Attributes["Password"].Value, pW);
					conI.Domain = with_1.Attributes["Domain"].Value;
					conI.DisplayWallpaper = bool.Parse(with_1.Attributes["DisplayWallpaper"].Value);
					conI.DisplayThemes = bool.Parse(with_1.Attributes["DisplayThemes"].Value);
					conI.CacheBitmaps = bool.Parse(with_1.Attributes["CacheBitmaps"].Value);
							
					if (this.confVersion < 1.1) //1.0 - 0.1
					{
						if (with_1.Attributes["Fullscreen"].Value == true)
						{
							conI.Resolution = Connection.Protocol.RDP.RDPResolutions.Fullscreen;
						}
						else
						{
							conI.Resolution = Connection.Protocol.RDP.RDPResolutions.FitToWindow;
						}
					}
				}
						
				if (this.confVersion > 0.2) //0.3
				{
					if (this.confVersion < 0.7)
					{
						if (System.Convert.ToBoolean(with_1.Attributes["UseVNC"].Value) == true)
						{
							conI.Protocol = Connection.Protocol.Protocols.VNC;
							conI.Port = (int) (with_1.Attributes["VNCPort"].Value);
						}
						else
						{
							conI.Protocol = Connection.Protocol.Protocols.RDP;
						}
					}
				}
				else
				{
					conI.Port = Connection.Protocol.RDP.Defaults.Port;
					conI.Protocol = Connection.Protocol.Protocols.RDP;
				}
						
				if (this.confVersion > 0.3) //0.4
				{
					if (this.confVersion < 0.7)
					{
						if (System.Convert.ToBoolean(with_1.Attributes["UseVNC"].Value) == true)
						{
							conI.Port = (int) (with_1.Attributes["VNCPort"].Value);
						}
						else
						{
							conI.Port = (int) (with_1.Attributes["RDPPort"].Value);
						}
					}
							
					conI.UseConsoleSession = bool.Parse(with_1.Attributes["ConnectToConsole"].Value);
				}
				else
				{
					if (this.confVersion < 0.7)
					{
						if (System.Convert.ToBoolean(with_1.Attributes["UseVNC"].Value) == true)
						{
							conI.Port = Connection.Protocol.VNC.Defaults.Port;
						}
						else
						{
							conI.Port = Connection.Protocol.RDP.Defaults.Port;
						}
					}
					conI.UseConsoleSession = false;
				}
						
				if (this.confVersion > 0.4) //0.5 and 0.6
				{
					conI.RedirectDiskDrives = bool.Parse(with_1.Attributes["RedirectDiskDrives"].Value);
					conI.RedirectPrinters = bool.Parse(with_1.Attributes["RedirectPrinters"].Value);
					conI.RedirectPorts = bool.Parse(with_1.Attributes["RedirectPorts"].Value);
					conI.RedirectSmartCards = bool.Parse(with_1.Attributes["RedirectSmartCards"].Value);
				}
				else
				{
					conI.RedirectDiskDrives = false;
					conI.RedirectPrinters = false;
					conI.RedirectPorts = false;
					conI.RedirectSmartCards = false;
				}
						
				if (this.confVersion > 0.6) //0.7
				{
					conI.Protocol = Tools.Misc.StringToEnum(typeof(Connection.Protocol.Protocols), with_1.Attributes["Protocol"].Value);
					conI.Port = (int) (with_1.Attributes["Port"].Value);
				}
						
				if (this.confVersion > 0.9) //1.0
				{
					conI.RedirectKeys = bool.Parse(with_1.Attributes["RedirectKeys"].Value);
				}
						
				if (this.confVersion > 1.1) //1.2
				{
					conI.PuttySession = with_1.Attributes["PuttySession"].Value;
				}
						
				if (this.confVersion > 1.2) //1.3
				{
					conI.Colors = Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPColors), with_1.Attributes["Colors"].Value);
					conI.Resolution = Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPResolutions), System.Convert.ToString(with_1.Attributes["Resolution"].Value));
					conI.RedirectSound = Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPSounds), System.Convert.ToString(with_1.Attributes["RedirectSound"].Value));
				}
				else
				{
					switch (with_1.Attributes["Colors"].Value)
					{
						case 0:
							conI.Colors = Connection.Protocol.RDP.RDPColors.Colors256;
							break;
						case 1:
							conI.Colors = Connection.Protocol.RDP.RDPColors.Colors16Bit;
							break;
						case 2:
							conI.Colors = Connection.Protocol.RDP.RDPColors.Colors24Bit;
							break;
						case 3:
							conI.Colors = Connection.Protocol.RDP.RDPColors.Colors32Bit;
							break;
						case 4:
							conI.Colors = Connection.Protocol.RDP.RDPColors.Colors15Bit;
							break;
					}
							
					conI.RedirectSound = with_1.Attributes["RedirectSound"].Value;
				}
						
				if (this.confVersion > 1.2) //1.3
				{
					conI.Inherit = new Connection.Info.Inheritance(conI);
					conI.Inherit.CacheBitmaps = bool.Parse(with_1.Attributes["InheritCacheBitmaps"].Value);
					conI.Inherit.Colors = bool.Parse(with_1.Attributes["InheritColors"].Value);
					conI.Inherit.Description = bool.Parse(with_1.Attributes["InheritDescription"].Value);
					conI.Inherit.DisplayThemes = bool.Parse(with_1.Attributes["InheritDisplayThemes"].Value);
					conI.Inherit.DisplayWallpaper = bool.Parse(with_1.Attributes["InheritDisplayWallpaper"].Value);
					conI.Inherit.Domain = bool.Parse(with_1.Attributes["InheritDomain"].Value);
					conI.Inherit.Icon = bool.Parse(with_1.Attributes["InheritIcon"].Value);
					conI.Inherit.Panel = bool.Parse(with_1.Attributes["InheritPanel"].Value);
					conI.Inherit.Password = bool.Parse(with_1.Attributes["InheritPassword"].Value);
					conI.Inherit.Port = bool.Parse(with_1.Attributes["InheritPort"].Value);
					conI.Inherit.Protocol = bool.Parse(with_1.Attributes["InheritProtocol"].Value);
					conI.Inherit.PuttySession = bool.Parse(with_1.Attributes["InheritPuttySession"].Value);
					conI.Inherit.RedirectDiskDrives = bool.Parse(with_1.Attributes["InheritRedirectDiskDrives"].Value);
					conI.Inherit.RedirectKeys = bool.Parse(with_1.Attributes["InheritRedirectKeys"].Value);
					conI.Inherit.RedirectPorts = bool.Parse(with_1.Attributes["InheritRedirectPorts"].Value);
					conI.Inherit.RedirectPrinters = bool.Parse(with_1.Attributes["InheritRedirectPrinters"].Value);
					conI.Inherit.RedirectSmartCards = bool.Parse(with_1.Attributes["InheritRedirectSmartCards"].Value);
					conI.Inherit.RedirectSound = bool.Parse(with_1.Attributes["InheritRedirectSound"].Value);
					conI.Inherit.Resolution = bool.Parse(with_1.Attributes["InheritResolution"].Value);
					conI.Inherit.UseConsoleSession = bool.Parse(with_1.Attributes["InheritUseConsoleSession"].Value);
					conI.Inherit.Username = bool.Parse(with_1.Attributes["InheritUsername"].Value);
							
					conI.Icon = with_1.Attributes["Icon"].Value;
					conI.Panel = with_1.Attributes["Panel"].Value;
				}
				else
				{
					conI.Inherit = new Connection.Info.Inheritance(conI, with_1.Attributes["Inherit"].Value);
							
					conI.Icon = System.Convert.ToString(with_1.Attributes["Icon"].Value.Replace(".ico", ""));
					conI.Panel = My.Language.strGeneral;
				}
						
				if (this.confVersion > 1.4) //1.5
				{
					conI.PleaseConnect = bool.Parse(with_1.Attributes["Connected"].Value);
				}
						
				if (this.confVersion > 1.5) //1.6
				{
					conI.ICAEncryption = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.ICA.EncryptionStrength), with_1.Attributes["ICAEncryptionStrength"].Value);
					conI.Inherit.ICAEncryption = bool.Parse(with_1.Attributes["InheritICAEncryptionStrength"].Value);
							
					conI.PreExtApp = with_1.Attributes["PreExtApp"].Value;
					conI.PostExtApp = with_1.Attributes["PostExtApp"].Value;
					conI.Inherit.PreExtApp = bool.Parse(with_1.Attributes["InheritPreExtApp"].Value);
					conI.Inherit.PostExtApp = bool.Parse(with_1.Attributes["InheritPostExtApp"].Value);
				}
						
				if (this.confVersion > 1.6) //1.7
				{
					conI.VNCCompression = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.Compression), with_1.Attributes["VNCCompression"].Value);
					conI.VNCEncoding = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.Encoding), System.Convert.ToString(with_1.Attributes["VNCEncoding"].Value));
					conI.VNCAuthMode = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.AuthMode), with_1.Attributes["VNCAuthMode"].Value);
					conI.VNCProxyType = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.ProxyType), with_1.Attributes["VNCProxyType"].Value);
					conI.VNCProxyIP = with_1.Attributes["VNCProxyIP"].Value;
					conI.VNCProxyPort = (int) (with_1.Attributes["VNCProxyPort"].Value);
					conI.VNCProxyUsername = with_1.Attributes["VNCProxyUsername"].Value;
					conI.VNCProxyPassword = Security.Crypt.Decrypt(with_1.Attributes["VNCProxyPassword"].Value, pW);
					conI.VNCColors = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.Colors), with_1.Attributes["VNCColors"].Value);
					conI.VNCSmartSizeMode = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.SmartSizeMode), with_1.Attributes["VNCSmartSizeMode"].Value);
					conI.VNCViewOnly = bool.Parse(with_1.Attributes["VNCViewOnly"].Value);
							
					conI.Inherit.VNCCompression = bool.Parse(with_1.Attributes["InheritVNCCompression"].Value);
					conI.Inherit.VNCEncoding = bool.Parse(with_1.Attributes["InheritVNCEncoding"].Value);
					conI.Inherit.VNCAuthMode = bool.Parse(with_1.Attributes["InheritVNCAuthMode"].Value);
					conI.Inherit.VNCProxyType = bool.Parse(with_1.Attributes["InheritVNCProxyType"].Value);
					conI.Inherit.VNCProxyIP = bool.Parse(with_1.Attributes["InheritVNCProxyIP"].Value);
					conI.Inherit.VNCProxyPort = bool.Parse(with_1.Attributes["InheritVNCProxyPort"].Value);
					conI.Inherit.VNCProxyUsername = bool.Parse(with_1.Attributes["InheritVNCProxyUsername"].Value);
					conI.Inherit.VNCProxyPassword = bool.Parse(with_1.Attributes["InheritVNCProxyPassword"].Value);
					conI.Inherit.VNCColors = bool.Parse(with_1.Attributes["InheritVNCColors"].Value);
					conI.Inherit.VNCSmartSizeMode = bool.Parse(with_1.Attributes["InheritVNCSmartSizeMode"].Value);
					conI.Inherit.VNCViewOnly = bool.Parse(with_1.Attributes["InheritVNCViewOnly"].Value);
				}
						
				if (this.confVersion > 1.7) //1.8
				{
					conI.RDPAuthenticationLevel = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.AuthenticationLevel), with_1.Attributes["RDPAuthenticationLevel"].Value);
							
					conI.Inherit.RDPAuthenticationLevel = bool.Parse(with_1.Attributes["InheritRDPAuthenticationLevel"].Value);
				}
						
				if (this.confVersion > 1.8) //1.9
				{
					conI.RenderingEngine = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.HTTPBase.RenderingEngine), with_1.Attributes["RenderingEngine"].Value);
					conI.MacAddress = with_1.Attributes["MacAddress"].Value;
							
					conI.Inherit.RenderingEngine = bool.Parse(with_1.Attributes["InheritRenderingEngine"].Value);
					conI.Inherit.MacAddress = bool.Parse(with_1.Attributes["InheritMacAddress"].Value);
				}
						
				if (this.confVersion > 1.9) //2.0
				{
					conI.UserField = with_1.Attributes["UserField"].Value;
					conI.Inherit.UserField = bool.Parse(with_1.Attributes["InheritUserField"].Value);
				}
						
				if (this.confVersion > 2.0) //2.1
				{
					conI.ExtApp = with_1.Attributes["ExtApp"].Value;
					conI.Inherit.ExtApp = bool.Parse(with_1.Attributes["InheritExtApp"].Value);
				}
						
				if (this.confVersion > 2.1) //2.2
				{
					// Get settings
					conI.RDGatewayUsageMethod = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod), System.Convert.ToString(with_1.Attributes["RDGatewayUsageMethod"].Value));
					conI.RDGatewayHostname = with_1.Attributes["RDGatewayHostname"].Value;
					conI.RDGatewayUseConnectionCredentials = Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials), System.Convert.ToString(with_1.Attributes["RDGatewayUseConnectionCredentials"].Value));
					conI.RDGatewayUsername = with_1.Attributes["RDGatewayUsername"].Value;
					conI.RDGatewayPassword = Security.Crypt.Decrypt(System.Convert.ToString(with_1.Attributes["RDGatewayPassword"].Value), pW);
					conI.RDGatewayDomain = with_1.Attributes["RDGatewayDomain"].Value;
							
					// Get inheritance settings
					conI.Inherit.RDGatewayUsageMethod = bool.Parse(with_1.Attributes["InheritRDGatewayUsageMethod"].Value);
					conI.Inherit.RDGatewayHostname = bool.Parse(with_1.Attributes["InheritRDGatewayHostname"].Value);
					conI.Inherit.RDGatewayUseConnectionCredentials = bool.Parse(with_1.Attributes["InheritRDGatewayUseConnectionCredentials"].Value);
					conI.Inherit.RDGatewayUsername = bool.Parse(with_1.Attributes["InheritRDGatewayUsername"].Value);
					conI.Inherit.RDGatewayPassword = bool.Parse(with_1.Attributes["InheritRDGatewayPassword"].Value);
					conI.Inherit.RDGatewayDomain = bool.Parse(with_1.Attributes["InheritRDGatewayDomain"].Value);
				}
						
				if (this.confVersion > 2.2) //2.3
				{
					// Get settings
					conI.EnableFontSmoothing = bool.Parse(with_1.Attributes["EnableFontSmoothing"].Value);
					conI.EnableDesktopComposition = bool.Parse(with_1.Attributes["EnableDesktopComposition"].Value);
							
					// Get inheritance settings
					conI.Inherit.EnableFontSmoothing = bool.Parse(with_1.Attributes["InheritEnableFontSmoothing"].Value);
					conI.Inherit.EnableDesktopComposition = bool.Parse(with_1.Attributes["InheritEnableDesktopComposition"].Value);
				}
						
				if (confVersion >= 2.4)
				{
					conI.UseCredSsp = bool.Parse(with_1.Attributes["UseCredSsp"].Value);
					conI.Inherit.UseCredSsp = bool.Parse(with_1.Attributes["InheritUseCredSsp"].Value);
				}
						
				if (confVersion >= 2.5)
				{
					conI.LoadBalanceInfo = with_1.Attributes["LoadBalanceInfo"].Value;
					conI.AutomaticResize = bool.Parse(with_1.Attributes["AutomaticResize"].Value);
					conI.Inherit.LoadBalanceInfo = bool.Parse(with_1.Attributes["InheritLoadBalanceInfo"].Value);
					conI.Inherit.AutomaticResize = bool.Parse(with_1.Attributes["InheritAutomaticResize"].Value);
				}
			}
			catch (Exception ex)
			{
				MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(My.Language.strGetConnectionInfoFromXmlFailed, conI.Name, this.ConnectionFileName, ex.Message), false);
			}
			return conI;
		}
				
		private bool Authenticate(string Value, bool CompareToOriginalValue, Root.Info rootInfo = null)
		{
			string passwordName = "";
			if (UseSQL)
			{
				passwordName = Language.strSQLServer.TrimEnd(':');
			}
			else
			{
				passwordName = Path.GetFileName(ConnectionFileName);
			}
					
			if (CompareToOriginalValue)
			{
				while (!(Security.Crypt.Decrypt(Value, pW) != Value))
				{
					pW = Tools.Misc.PasswordDialog(passwordName, false);
							
					if (string.IsNullOrEmpty(pW))
					{
						return false;
					}
				}
			}
			else
			{
				while (!(Security.Crypt.Decrypt(Value, pW) == "ThisIsProtected"))
				{
					pW = Tools.Misc.PasswordDialog(passwordName, false);
							
					if (string.IsNullOrEmpty(pW))
					{
						return false;
					}
				}
						
				if (rootInfo != null)
				{
					rootInfo.Password = true;
					rootInfo.PasswordString = pW;
				}
			}
					
			return true;
		}
        #endregion
	}
}
