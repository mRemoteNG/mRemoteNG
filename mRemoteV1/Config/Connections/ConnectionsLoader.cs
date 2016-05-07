using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.My;
using PSTaskDialog;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.Tree;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Images;
using mRemoteNG.UI.Forms;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Connections
{
	public class ConnectionsLoader
	{
        #region Private Properties
		private XmlDocument xDom;
		private double confVersion;
		private string pW = "mR3m";
		private SqlConnection sqlCon;
		private SqlCommand sqlQuery;
		private SqlDataReader sqlRd;
		private TreeNode _selectedTreeNode;
        private bool _UseSQL;
        private string _SQLHost;
        private string _SQLDatabaseName;
        private string _SQLUsername;
        private string _SQLPassword;
        private bool _SQLUpdate;
        private string _PreviousSelected;
        private string _ConnectionFileName;
        private ContainerList _ContainerList;
        private ConnectionList _PreviousConnectionList;
        private ContainerList _PreviousContainerList;
        #endregion
				
        #region Public Properties
        public bool UseSQL
		{
			get { return _UseSQL; }
			set { _UseSQL = value; }
		}
		
        public string SQLHost
		{
			get { return _SQLHost; }
			set { _SQLHost = value; }
		}
		
        public string SQLDatabaseName
		{
			get { return _SQLDatabaseName; }
			set { _SQLDatabaseName = value; }
		}
		
        public string SQLUsername
		{
			get { return _SQLUsername; }
			set { _SQLUsername = value; }
		}
		
        public string SQLPassword
		{
			get { return _SQLPassword; }
			set { _SQLPassword = value; }
		}
		
        public bool SQLUpdate
		{
			get { return _SQLUpdate; }
			set { _SQLUpdate = value; }
		}
		
        public string PreviousSelected
		{
			get { return _PreviousSelected; }
			set { _PreviousSelected = value; }
		}
		
        public string ConnectionFileName
		{
			get { return this._ConnectionFileName; }
			set { this._ConnectionFileName = value; }
		}
		
		public TreeNode RootTreeNode {get; set;}
		
		public ConnectionList ConnectionList {get; set;}
		
        public ContainerList ContainerList
		{
			get { return this._ContainerList; }
			set { this._ContainerList = value; }
		}
		
        public ConnectionList PreviousConnectionList
		{
			get { return _PreviousConnectionList; }
			set { _PreviousConnectionList = value; }
		}
		
        public ContainerList PreviousContainerList
		{
			get { return _PreviousContainerList; }
			set { _PreviousContainerList = value; }
		}
        #endregion
				
        #region Public Methods
		public void LoadConnections(bool import)
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
					
			frmMain.Default.AreWeUsingSqlServerForSavingConnections = UseSQL;
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
            if (Windows.treeForm == null || Windows.treeForm.tvConnections == null)
			{
				return ;
			}
            if (Windows.treeForm.tvConnections.InvokeRequired)
			{
                Windows.treeForm.tvConnections.Invoke(new LoadFromSqlDelegate(LoadFromSQL));
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
                    cTaskDialog.ShowTaskDialogBox(
                        frmMain.Default, 
                        System.Windows.Forms.Application.ProductName, 
                        "Incompatible database schema", 
                        string.Format("The database schema on the server is not supported. Please upgrade to a newer version of {0}.", System.Windows.Forms.Application.ProductName), 
                        string.Format("Schema Version: {1}{0}Highest Supported Version: {2}", Environment.NewLine, confVersion.ToString(), maxSupportedSchemaVersion.ToString()), 
                        "", 
                        "", 
                        "", 
                        "", 
                        eTaskDialogButtons.OK, 
                        eSysIcons.Error, 
                        eSysIcons.Error
                    );
					throw (new Exception(string.Format("Incompatible database schema (schema version {0}).", confVersion)));
				}
						
				RootTreeNode.Name = Convert.ToString(sqlRd["Name"]);
						
				RootNodeInfo rootInfo = new RootNodeInfo(RootNodeType.Connection);
				rootInfo.Name = RootTreeNode.Name;
				rootInfo.TreeNode = RootTreeNode;
						
				RootTreeNode.Tag = rootInfo;
				RootTreeNode.ImageIndex = (int)TreeImageType.Root;
                RootTreeNode.SelectedImageIndex = (int)TreeImageType.Root;
						
				if (Security.Crypt.Decrypt(Convert.ToString(sqlRd["Protected"]), pW) != "ThisIsNotProtected")
				{
					if (Authenticate(Convert.ToString(sqlRd["Protected"]), false, rootInfo) == false)
					{
						My.Settings.Default.LoadConsFromCustomLocation = false;
						My.Settings.Default.CustomConsPath = "";
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
				foreach (ContainerInfo contI in this._ContainerList)
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
					foreach (ConnectionInfo conI in ConnectionList)
					{
						if (conI.PleaseConnect == true)
						{
                            Runtime.OpenConnection(conI);
						}
					}
				}

                Runtime.IsConnectionsFileLoaded = true;
                Windows.treeForm.InitialRefresh();
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
            if (ConnectionTree.TreeView != null && ConnectionTree.TreeView.InvokeRequired)
			{
                Windows.treeForm.Invoke(new SetSelectedNodeDelegate(SetSelectedNode), new object[] { treeNode });
				return ;
			}
            Windows.treeForm.tvConnections.SelectedNode = treeNode;
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
					tNode = new TreeNode(Convert.ToString(sqlRd["Name"]));
					//baseNode.Nodes.Add(tNode)
							
					if (Tree.ConnectionTreeNode.GetNodeTypeFromString(Convert.ToString(sqlRd["Type"])) == Tree.TreeNodeType.Connection)
					{
                        ConnectionInfo conI = GetConnectionInfoFromSQL();
						conI.TreeNode = tNode;
						//conI.Parent = _previousContainer 'NEW
								
						this.ConnectionList.Add(conI);
								
						tNode.Tag = conI;
								
						if (SQLUpdate == true)
						{
                            ConnectionInfo prevCon = PreviousConnectionList.FindByConstantID(conI.ConstantID);
									
							if (prevCon != null)
							{
								foreach (Connection.Protocol.ProtocolBase prot in prevCon.OpenConnections)
								{
									prot.InterfaceControl.Info = conI;
									conI.OpenConnections.Add(prot);
								}
										
								if (conI.OpenConnections.Count > 0)
								{
                                    tNode.ImageIndex = (int)TreeImageType.ConnectionOpen;
                                    tNode.SelectedImageIndex = (int)TreeImageType.ConnectionOpen;
								}
								else
								{
                                    tNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
                                    tNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
								}
							}
							else
							{
                                tNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
                                tNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
							}
									
							if (conI.ConstantID == _PreviousSelected)
							{
								_selectedTreeNode = tNode;
							}
						}
						else
						{
                            tNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
                            tNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
						}
					}
					else if (Tree.ConnectionTreeNode.GetNodeTypeFromString(Convert.ToString(sqlRd["Type"])) == Tree.TreeNodeType.Container)
					{
                        ContainerInfo contI = new ContainerInfo();
						//If tNode.Parent IsNot Nothing Then
						//    If Tree.Node.GetNodeType(tNode.Parent) = Tree.Node.Type.Container Then
						//        contI.Parent = tNode.Parent.Tag
						//    End If
						//End If
						//_previousContainer = contI 'NEW
						contI.TreeNode = tNode;
								
						contI.Name = Convert.ToString(sqlRd["Name"]);

                        ConnectionInfo conI = default(ConnectionInfo);
								
						conI = GetConnectionInfoFromSQL();
								
						conI.Parent = contI;
						conI.IsContainer = true;
						contI.ConnectionInfo = conI;
								
						if (SQLUpdate == true)
						{
                            ContainerInfo prevCont = PreviousContainerList.FindByConstantID(conI.ConstantID);
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
							if (Convert.ToBoolean(sqlRd["Expanded"]) == true)
							{
								contI.IsExpanded = true;
							}
							else
							{
								contI.IsExpanded = false;
							}
						}
								
						this._ContainerList.Add(contI);
						this.ConnectionList.Add(conI);
								
						tNode.Tag = contI;
                        tNode.ImageIndex = (int)TreeImageType.Container;
                        tNode.SelectedImageIndex = (int)TreeImageType.Container;
					}
							
					string parentId = Convert.ToString(sqlRd["ParentID"].ToString().Trim());
					if (string.IsNullOrEmpty(parentId) || parentId == "0")
					{
						baseNode.Nodes.Add(tNode);
					}
					else
					{
						TreeNode pNode = Tree.ConnectionTreeNode.GetNodeFromConstantID(Convert.ToString(sqlRd["ParentID"]));
								
						if (pNode != null)
						{
							pNode.Nodes.Add(tNode);
									
							if (Tree.ConnectionTreeNode.GetNodeType(tNode) == Tree.TreeNodeType.Connection)
							{
								(tNode.Tag as ConnectionInfo).Parent = (ContainerInfo)pNode.Tag;
							}
							else if (Tree.ConnectionTreeNode.GetNodeType(tNode) == Tree.TreeNodeType.Container)
							{
								(tNode.Tag as ContainerInfo).Parent = pNode.Tag;
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strAddNodesFromSqlFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private ConnectionInfo GetConnectionInfoFromSQL()
		{
			try
			{
				ConnectionInfo connectionInfo = new ConnectionInfo();
						
				connectionInfo.PositionID = Convert.ToInt32(sqlRd["PositionID"]);
				connectionInfo.ConstantID = Convert.ToString(sqlRd["ConstantID"]);
				connectionInfo.Name = Convert.ToString(sqlRd["Name"]);
				connectionInfo.Description = Convert.ToString(sqlRd["Description"]);
				connectionInfo.Hostname = Convert.ToString(sqlRd["Hostname"]);
				connectionInfo.Username = Convert.ToString(sqlRd["Username"]);
				connectionInfo.Password = Security.Crypt.Decrypt(Convert.ToString(sqlRd["Password"]), pW);
				connectionInfo.Domain = Convert.ToString(sqlRd["DomainName"]);
				connectionInfo.DisplayWallpaper = Convert.ToBoolean(sqlRd["DisplayWallpaper"]);
				connectionInfo.DisplayThemes = Convert.ToBoolean(sqlRd["DisplayThemes"]);
				connectionInfo.CacheBitmaps = Convert.ToBoolean(sqlRd["CacheBitmaps"]);
				connectionInfo.UseConsoleSession = Convert.ToBoolean(sqlRd["ConnectToConsole"]);
				connectionInfo.RedirectDiskDrives = Convert.ToBoolean(sqlRd["RedirectDiskDrives"]);
				connectionInfo.RedirectPrinters = Convert.ToBoolean(sqlRd["RedirectPrinters"]);
				connectionInfo.RedirectPorts = Convert.ToBoolean(sqlRd["RedirectPorts"]);
				connectionInfo.RedirectSmartCards = Convert.ToBoolean(sqlRd["RedirectSmartCards"]);
				connectionInfo.RedirectKeys = Convert.ToBoolean(sqlRd["RedirectKeys"]);
                connectionInfo.RedirectSound = (ProtocolRDP.RDPSounds)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPSounds), Convert.ToString(sqlRd["RedirectSound"]));
                connectionInfo.Protocol = (ProtocolType)Tools.MiscTools.StringToEnum(typeof(ProtocolType), Convert.ToString(sqlRd["Protocol"]));
				connectionInfo.Port = Convert.ToInt32(sqlRd["Port"]);
				connectionInfo.PuttySession = Convert.ToString(sqlRd["PuttySession"]);
                connectionInfo.Colors = (ProtocolRDP.RDPColors)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPColors), Convert.ToString(sqlRd["Colors"]));
                connectionInfo.Resolution = (ProtocolRDP.RDPResolutions)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPResolutions), Convert.ToString(sqlRd["Resolution"]));
				connectionInfo.Inherit = new ConnectionInfoInheritance(connectionInfo);
				connectionInfo.Inherit.CacheBitmaps = Convert.ToBoolean(sqlRd["InheritCacheBitmaps"]);
				connectionInfo.Inherit.Colors = Convert.ToBoolean(sqlRd["InheritColors"]);
				connectionInfo.Inherit.Description = Convert.ToBoolean(sqlRd["InheritDescription"]);
				connectionInfo.Inherit.DisplayThemes = Convert.ToBoolean(sqlRd["InheritDisplayThemes"]);
				connectionInfo.Inherit.DisplayWallpaper = Convert.ToBoolean(sqlRd["InheritDisplayWallpaper"]);
				connectionInfo.Inherit.Domain = Convert.ToBoolean(sqlRd["InheritDomain"]);
				connectionInfo.Inherit.Icon = Convert.ToBoolean(sqlRd["InheritIcon"]);
				connectionInfo.Inherit.Panel = Convert.ToBoolean(sqlRd["InheritPanel"]);
				connectionInfo.Inherit.Password = Convert.ToBoolean(sqlRd["InheritPassword"]);
				connectionInfo.Inherit.Port = Convert.ToBoolean(sqlRd["InheritPort"]);
				connectionInfo.Inherit.Protocol = Convert.ToBoolean(sqlRd["InheritProtocol"]);
				connectionInfo.Inherit.PuttySession = Convert.ToBoolean(sqlRd["InheritPuttySession"]);
				connectionInfo.Inherit.RedirectDiskDrives = Convert.ToBoolean(sqlRd["InheritRedirectDiskDrives"]);
				connectionInfo.Inherit.RedirectKeys = Convert.ToBoolean(sqlRd["InheritRedirectKeys"]);
				connectionInfo.Inherit.RedirectPorts = Convert.ToBoolean(sqlRd["InheritRedirectPorts"]);
				connectionInfo.Inherit.RedirectPrinters = Convert.ToBoolean(sqlRd["InheritRedirectPrinters"]);
				connectionInfo.Inherit.RedirectSmartCards = Convert.ToBoolean(sqlRd["InheritRedirectSmartCards"]);
				connectionInfo.Inherit.RedirectSound = Convert.ToBoolean(sqlRd["InheritRedirectSound"]);
				connectionInfo.Inherit.Resolution = Convert.ToBoolean(sqlRd["InheritResolution"]);
				connectionInfo.Inherit.UseConsoleSession = Convert.ToBoolean(sqlRd["InheritUseConsoleSession"]);
				connectionInfo.Inherit.Username = Convert.ToBoolean(sqlRd["InheritUsername"]);
				connectionInfo.Icon = Convert.ToString(sqlRd["Icon"]);
				connectionInfo.Panel = Convert.ToString(sqlRd["Panel"]);
						
				if (this.confVersion > 1.5) //1.6
				{
                    connectionInfo.ICAEncryption = (ProtocolICA.EncryptionStrength)Tools.MiscTools.StringToEnum(typeof(ProtocolICA.EncryptionStrength), Convert.ToString(sqlRd["ICAEncryptionStrength"]));
					connectionInfo.Inherit.ICAEncryption = Convert.ToBoolean(sqlRd["InheritICAEncryptionStrength"]);
					connectionInfo.PreExtApp = Convert.ToString(sqlRd["PreExtApp"]);
					connectionInfo.PostExtApp = Convert.ToString(sqlRd["PostExtApp"]);
					connectionInfo.Inherit.PreExtApp = Convert.ToBoolean(sqlRd["InheritPreExtApp"]);
					connectionInfo.Inherit.PostExtApp = Convert.ToBoolean(sqlRd["InheritPostExtApp"]);
				}
						
				if (this.confVersion > 1.6) //1.7
				{
                    connectionInfo.VNCCompression = (ProtocolVNC.Compression)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Compression), Convert.ToString(sqlRd["VNCCompression"]));
                    connectionInfo.VNCEncoding = (ProtocolVNC.Encoding)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Encoding), Convert.ToString(sqlRd["VNCEncoding"]));
                    connectionInfo.VNCAuthMode = (ProtocolVNC.AuthMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.AuthMode), Convert.ToString(sqlRd["VNCAuthMode"]));
                    connectionInfo.VNCProxyType = (ProtocolVNC.ProxyType)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.ProxyType), Convert.ToString(sqlRd["VNCProxyType"]));
					connectionInfo.VNCProxyIP = Convert.ToString(sqlRd["VNCProxyIP"]);
					connectionInfo.VNCProxyPort = Convert.ToInt32(sqlRd["VNCProxyPort"]);
					connectionInfo.VNCProxyUsername = Convert.ToString(sqlRd["VNCProxyUsername"]);
					connectionInfo.VNCProxyPassword = Security.Crypt.Decrypt(Convert.ToString(sqlRd["VNCProxyPassword"]), pW);
                    connectionInfo.VNCColors = (ProtocolVNC.Colors)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Colors), Convert.ToString(sqlRd["VNCColors"]));
                    connectionInfo.VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.SmartSizeMode), Convert.ToString(sqlRd["VNCSmartSizeMode"]));
					connectionInfo.VNCViewOnly = Convert.ToBoolean(sqlRd["VNCViewOnly"]);
					connectionInfo.Inherit.VNCCompression = Convert.ToBoolean(sqlRd["InheritVNCCompression"]);
					connectionInfo.Inherit.VNCEncoding = Convert.ToBoolean(sqlRd["InheritVNCEncoding"]);
					connectionInfo.Inherit.VNCAuthMode = Convert.ToBoolean(sqlRd["InheritVNCAuthMode"]);
					connectionInfo.Inherit.VNCProxyType = Convert.ToBoolean(sqlRd["InheritVNCProxyType"]);
					connectionInfo.Inherit.VNCProxyIP = Convert.ToBoolean(sqlRd["InheritVNCProxyIP"]);
					connectionInfo.Inherit.VNCProxyPort = Convert.ToBoolean(sqlRd["InheritVNCProxyPort"]);
					connectionInfo.Inherit.VNCProxyUsername = Convert.ToBoolean(sqlRd["InheritVNCProxyUsername"]);
					connectionInfo.Inherit.VNCProxyPassword = Convert.ToBoolean(sqlRd["InheritVNCProxyPassword"]);
					connectionInfo.Inherit.VNCColors = Convert.ToBoolean(sqlRd["InheritVNCColors"]);
					connectionInfo.Inherit.VNCSmartSizeMode = Convert.ToBoolean(sqlRd["InheritVNCSmartSizeMode"]);
					connectionInfo.Inherit.VNCViewOnly = Convert.ToBoolean(sqlRd["InheritVNCViewOnly"]);
				}
				
				if (this.confVersion > 1.7) //1.8
				{
                    connectionInfo.RDPAuthenticationLevel = (ProtocolRDP.AuthenticationLevel)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.AuthenticationLevel), Convert.ToString(sqlRd["RDPAuthenticationLevel"]));
					connectionInfo.Inherit.RDPAuthenticationLevel = Convert.ToBoolean(sqlRd["InheritRDPAuthenticationLevel"]);
				}
				
				if (this.confVersion > 1.8) //1.9
				{
                    connectionInfo.RenderingEngine = (HTTPBase.RenderingEngine)Tools.MiscTools.StringToEnum(typeof(HTTPBase.RenderingEngine), Convert.ToString(sqlRd["RenderingEngine"]));
					connectionInfo.MacAddress = Convert.ToString(sqlRd["MacAddress"]);
					connectionInfo.Inherit.RenderingEngine = Convert.ToBoolean(sqlRd["InheritRenderingEngine"]);
					connectionInfo.Inherit.MacAddress = Convert.ToBoolean(sqlRd["InheritMacAddress"]);
				}
				
				if (this.confVersion > 1.9) //2.0
				{
					connectionInfo.UserField = Convert.ToString(sqlRd["UserField"]);
					connectionInfo.Inherit.UserField = Convert.ToBoolean(sqlRd["InheritUserField"]);
				}
				
				if (this.confVersion > 2.0) //2.1
				{
					connectionInfo.ExtApp = Convert.ToString(sqlRd["ExtApp"]);
					connectionInfo.Inherit.ExtApp = Convert.ToBoolean(sqlRd["InheritExtApp"]);
				}
						
				if (this.confVersion >= 2.2)
				{
                    connectionInfo.RDGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUsageMethod), Convert.ToString(sqlRd["RDGatewayUsageMethod"]));
					connectionInfo.RDGatewayHostname = Convert.ToString(sqlRd["RDGatewayHostname"]);
                    connectionInfo.RDGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), Convert.ToString(sqlRd["RDGatewayUseConnectionCredentials"]));
					connectionInfo.RDGatewayUsername = Convert.ToString(sqlRd["RDGatewayUsername"]);
					connectionInfo.RDGatewayPassword = Security.Crypt.Decrypt(Convert.ToString(sqlRd["RDGatewayPassword"]), pW);
					connectionInfo.RDGatewayDomain = Convert.ToString(sqlRd["RDGatewayDomain"]);
					connectionInfo.Inherit.RDGatewayUsageMethod = Convert.ToBoolean(sqlRd["InheritRDGatewayUsageMethod"]);
					connectionInfo.Inherit.RDGatewayHostname = Convert.ToBoolean(sqlRd["InheritRDGatewayHostname"]);
					connectionInfo.Inherit.RDGatewayUsername = Convert.ToBoolean(sqlRd["InheritRDGatewayUsername"]);
					connectionInfo.Inherit.RDGatewayPassword = Convert.ToBoolean(sqlRd["InheritRDGatewayPassword"]);
					connectionInfo.Inherit.RDGatewayDomain = Convert.ToBoolean(sqlRd["InheritRDGatewayDomain"]);
				}
						
				if (this.confVersion >= 2.3)
				{
					connectionInfo.EnableFontSmoothing = Convert.ToBoolean(sqlRd["EnableFontSmoothing"]);
					connectionInfo.EnableDesktopComposition = Convert.ToBoolean(sqlRd["EnableDesktopComposition"]);
					connectionInfo.Inherit.EnableFontSmoothing = Convert.ToBoolean(sqlRd["InheritEnableFontSmoothing"]);
					connectionInfo.Inherit.EnableDesktopComposition = Convert.ToBoolean(sqlRd["InheritEnableDesktopComposition"]);
				}
						
				if (confVersion >= 2.4)
				{
					connectionInfo.UseCredSsp = Convert.ToBoolean(sqlRd["UseCredSsp"]);
					connectionInfo.Inherit.UseCredSsp = Convert.ToBoolean(sqlRd["InheritUseCredSsp"]);
				}
						
				if (confVersion >= 2.5)
				{
					connectionInfo.LoadBalanceInfo = Convert.ToString(sqlRd["LoadBalanceInfo"]);
					connectionInfo.AutomaticResize = Convert.ToBoolean(sqlRd["AutomaticResize"]);
					connectionInfo.Inherit.LoadBalanceInfo = Convert.ToBoolean(sqlRd["InheritLoadBalanceInfo"]);
					connectionInfo.Inherit.AutomaticResize = Convert.ToBoolean(sqlRd["InheritAutomaticResize"]);
				}
						
				if (SQLUpdate == true)
				{
					connectionInfo.PleaseConnect = Convert.ToBoolean(sqlRd["Connected"]);
				}
						
				return connectionInfo;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strGetConnectionInfoFromSqlFailed + Environment.NewLine + ex.Message, true);
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
						notDecr = false;
					else
						notDecr = true;
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
					Runtime.IsConnectionsFileLoaded = false;
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
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, Language.strOldConffile);
				}
						
				const double maxSupportedConfVersion = 2.5;
				if (confVersion > maxSupportedConfVersion)
				{
                    cTaskDialog.ShowTaskDialogBox(
                        frmMain.Default,
                        System.Windows.Forms.Application.ProductName, 
                        "Incompatible connection file format", 
                        string.Format("The format of this connection file is not supported. Please upgrade to a newer version of {0}.", System.Windows.Forms.Application.ProductName), 
                        string.Format("{1}{0}File Format Version: {2}{0}Highest Supported Version: {3}", Environment.NewLine, ConnectionFileName, confVersion.ToString(), maxSupportedConfVersion.ToString()),
                        "", 
                        "", 
                        "", 
                        "", 
                        eTaskDialogButtons.OK, 
                        eSysIcons.Error,
                        eSysIcons.Error
                    );
					throw (new Exception(string.Format("Incompatible connection file format (file format version {0}).", confVersion)));
				}
						
				// SECTION 2. Initialize the treeview control.
				RootNodeInfo rootInfo = default(RootNodeInfo);
				if (import)
				{
					rootInfo = null;
				}
				else
				{
					string rootNodeName = "";
					if (xDom.DocumentElement.HasAttribute("Name"))
					{
						rootNodeName = Convert.ToString(xDom.DocumentElement.Attributes["Name"].Value.Trim());
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
							
					rootInfo = new RootNodeInfo(RootNodeType.Connection);
					rootInfo.Name = RootTreeNode.Name;
					rootInfo.TreeNode = RootTreeNode;
							
					RootTreeNode.Tag = rootInfo;
				}
						
				if (this.confVersion > 1.3) //1.4
				{
					if (Security.Crypt.Decrypt(Convert.ToString(xDom.DocumentElement.Attributes["Protected"].Value), pW) != "ThisIsNotProtected")
					{
						if (Authenticate(Convert.ToString(xDom.DocumentElement.Attributes["Protected"].Value), false, rootInfo) == false)
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
					if (Convert.ToBoolean(xDom.DocumentElement.Attributes["Export"].Value) == true)
					{
						isExportFile = true;
					}
				}
						
				if (import && !isExportFile)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, Language.strCannotImportNormalSessionFile);
					return ;
				}
						
				if (!isExportFile)
				{
                    RootTreeNode.ImageIndex = (int)TreeImageType.Root;
                    RootTreeNode.SelectedImageIndex = (int)TreeImageType.Root;
				}

                Windows.treeForm.tvConnections.BeginUpdate();
						
				// SECTION 3. Populate the TreeView with the DOM nodes.
				AddNodeFromXml(xDom.DocumentElement, RootTreeNode);
						
				RootTreeNode.Expand();
						
				//expand containers
				foreach (ContainerInfo contI in this._ContainerList)
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
					foreach (ConnectionInfo conI in ConnectionList)
					{
						if (conI.PleaseConnect == true)
						{
							Runtime.OpenConnection(conI);
						}
					}
				}
						
				RootTreeNode.EnsureVisible();
						
				if (!import)
				{
                    Runtime.IsConnectionsFileLoaded = true;
				}
                Windows.treeForm.InitialRefresh();
				SetSelectedNode(RootTreeNode);
			}
			catch (Exception ex)
			{
				App.Runtime.IsConnectionsFileLoaded = false;
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strLoadFromXmlFailed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, true);
				throw;
			}
		}
				
		private ContainerInfo _previousContainer;
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
								
						if (Tree.ConnectionTreeNode.GetNodeTypeFromString(xmlNode.Attributes["Type"].Value) == TreeNodeType.Connection) //connection info
						{
							ConnectionInfo connectionInfo = GetConnectionInfoFromXml(xmlNode);
							connectionInfo.TreeNode = treeNode;
							connectionInfo.Parent = _previousContainer; //NEW
									
							ConnectionList.Add(connectionInfo);
									
							treeNode.Tag = connectionInfo;
                            treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
                            treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
						}
						else if (Tree.ConnectionTreeNode.GetNodeTypeFromString(xmlNode.Attributes["Type"].Value) == TreeNodeType.Container) //container info
						{
							ContainerInfo containerInfo = new ContainerInfo();
							if (treeNode.Parent != null)
							{
								if (Tree.ConnectionTreeNode.GetNodeType(treeNode.Parent) == TreeNodeType.Container)
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
									
							ConnectionInfo connectionInfo = default(ConnectionInfo);
							if (confVersion >= 0.9)
							{
								connectionInfo = GetConnectionInfoFromXml(xmlNode);
							}
							else
							{
								connectionInfo = new ConnectionInfo();
							}
									
							connectionInfo.Parent = containerInfo;
							connectionInfo.IsContainer = true;
							containerInfo.ConnectionInfo = connectionInfo;
									
							ContainerList.Add(containerInfo);
									
							treeNode.Tag = containerInfo;
                            treeNode.ImageIndex = (int)TreeImageType.Container;
                            treeNode.SelectedImageIndex = (int)TreeImageType.Container;
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, Language.strAddNodeFromXmlFailed + Environment.NewLine + ex.Message + ex.StackTrace, true);
				throw;
			}
		}
				
		private ConnectionInfo GetConnectionInfoFromXml(XmlNode xxNode)
		{
			ConnectionInfo connectionInfo = new ConnectionInfo();
			try
			{
				XmlNode xmlnode = xxNode;
				if (this.confVersion > 0.1) //0.2
				{
					connectionInfo.Name = xmlnode.Attributes["Name"].Value;
					connectionInfo.Description = xmlnode.Attributes["Descr"].Value;
					connectionInfo.Hostname = xmlnode.Attributes["Hostname"].Value;
					connectionInfo.Username = xmlnode.Attributes["Username"].Value;
					connectionInfo.Password = Security.Crypt.Decrypt(xmlnode.Attributes["Password"].Value, pW);
					connectionInfo.Domain = xmlnode.Attributes["Domain"].Value;
					connectionInfo.DisplayWallpaper = bool.Parse(xmlnode.Attributes["DisplayWallpaper"].Value);
					connectionInfo.DisplayThemes = bool.Parse(xmlnode.Attributes["DisplayThemes"].Value);
					connectionInfo.CacheBitmaps = bool.Parse(xmlnode.Attributes["CacheBitmaps"].Value);
							
					if (this.confVersion < 1.1) //1.0 - 0.1
					{
						if (Convert.ToBoolean(xmlnode.Attributes["Fullscreen"].Value) == true)
						{
							connectionInfo.Resolution = ProtocolRDP.RDPResolutions.Fullscreen;
						}
						else
						{
							connectionInfo.Resolution = ProtocolRDP.RDPResolutions.FitToWindow;
						}
					}
				}
						
				if (this.confVersion > 0.2) //0.3
				{
					if (this.confVersion < 0.7)
					{
						if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value) == true)
						{
							connectionInfo.Protocol = ProtocolType.VNC;
							connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["VNCPort"].Value);
						}
						else
						{
							connectionInfo.Protocol = ProtocolType.RDP;
						}
					}
				}
				else
				{
					connectionInfo.Port = (int)ProtocolRDP.Defaults.Port;
					connectionInfo.Protocol = ProtocolType.RDP;
				}
						
				if (this.confVersion > 0.3) //0.4
				{
					if (this.confVersion < 0.7)
					{
						if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value) == true)
                            connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["VNCPort"].Value);
						else
                            connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["RDPPort"].Value);
					}
							
					connectionInfo.UseConsoleSession = bool.Parse(xmlnode.Attributes["ConnectToConsole"].Value);
				}
				else
				{
					if (this.confVersion < 0.7)
					{
						if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value) == true)
							connectionInfo.Port = (int)ProtocolVNC.Defaults.Port;
						else
							connectionInfo.Port = (int)ProtocolRDP.Defaults.Port;
					}
					connectionInfo.UseConsoleSession = false;
				}
				
				if (this.confVersion > 0.4) //0.5 and 0.6
				{
					connectionInfo.RedirectDiskDrives = bool.Parse(xmlnode.Attributes["RedirectDiskDrives"].Value);
					connectionInfo.RedirectPrinters = bool.Parse(xmlnode.Attributes["RedirectPrinters"].Value);
					connectionInfo.RedirectPorts = bool.Parse(xmlnode.Attributes["RedirectPorts"].Value);
					connectionInfo.RedirectSmartCards = bool.Parse(xmlnode.Attributes["RedirectSmartCards"].Value);
				}
				else
				{
					connectionInfo.RedirectDiskDrives = false;
					connectionInfo.RedirectPrinters = false;
					connectionInfo.RedirectPorts = false;
					connectionInfo.RedirectSmartCards = false;
				}
				
				if (this.confVersion > 0.6) //0.7
				{
                    connectionInfo.Protocol = (ProtocolType)Tools.MiscTools.StringToEnum(typeof(ProtocolType), xmlnode.Attributes["Protocol"].Value);
                    connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["Port"].Value);
				}
				
				if (this.confVersion > 0.9) //1.0
				{
					connectionInfo.RedirectKeys = bool.Parse(xmlnode.Attributes["RedirectKeys"].Value);
				}
				
				if (this.confVersion > 1.1) //1.2
				{
					connectionInfo.PuttySession = xmlnode.Attributes["PuttySession"].Value;
				}
				
				if (this.confVersion > 1.2) //1.3
				{
                    connectionInfo.Colors = (ProtocolRDP.RDPColors)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPColors), xmlnode.Attributes["Colors"].Value);
                    connectionInfo.Resolution = (ProtocolRDP.RDPResolutions)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPResolutions), Convert.ToString(xmlnode.Attributes["Resolution"].Value));
                    connectionInfo.RedirectSound = (ProtocolRDP.RDPSounds)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPSounds), Convert.ToString(xmlnode.Attributes["RedirectSound"].Value));
				}
				else
				{
					switch (Convert.ToInt32(xmlnode.Attributes["Colors"].Value))
					{
						case 0:
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors256;
							break;
						case 1:
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors16Bit;
							break;
						case 2:
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors24Bit;
							break;
						case 3:
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors32Bit;
							break;
						case 4:
							connectionInfo.Colors = ProtocolRDP.RDPColors.Colors15Bit;
							break;
					}
							
					connectionInfo.RedirectSound = (ProtocolRDP.RDPSounds) Convert.ToInt32(xmlnode.Attributes["RedirectSound"].Value);
				}
				
				if (this.confVersion > 1.2) //1.3
				{
					connectionInfo.Inherit = new ConnectionInfoInheritance(connectionInfo);
					connectionInfo.Inherit.CacheBitmaps = bool.Parse(xmlnode.Attributes["InheritCacheBitmaps"].Value);
					connectionInfo.Inherit.Colors = bool.Parse(xmlnode.Attributes["InheritColors"].Value);
					connectionInfo.Inherit.Description = bool.Parse(xmlnode.Attributes["InheritDescription"].Value);
					connectionInfo.Inherit.DisplayThemes = bool.Parse(xmlnode.Attributes["InheritDisplayThemes"].Value);
					connectionInfo.Inherit.DisplayWallpaper = bool.Parse(xmlnode.Attributes["InheritDisplayWallpaper"].Value);
					connectionInfo.Inherit.Domain = bool.Parse(xmlnode.Attributes["InheritDomain"].Value);
					connectionInfo.Inherit.Icon = bool.Parse(xmlnode.Attributes["InheritIcon"].Value);
					connectionInfo.Inherit.Panel = bool.Parse(xmlnode.Attributes["InheritPanel"].Value);
					connectionInfo.Inherit.Password = bool.Parse(xmlnode.Attributes["InheritPassword"].Value);
					connectionInfo.Inherit.Port = bool.Parse(xmlnode.Attributes["InheritPort"].Value);
					connectionInfo.Inherit.Protocol = bool.Parse(xmlnode.Attributes["InheritProtocol"].Value);
					connectionInfo.Inherit.PuttySession = bool.Parse(xmlnode.Attributes["InheritPuttySession"].Value);
					connectionInfo.Inherit.RedirectDiskDrives = bool.Parse(xmlnode.Attributes["InheritRedirectDiskDrives"].Value);
					connectionInfo.Inherit.RedirectKeys = bool.Parse(xmlnode.Attributes["InheritRedirectKeys"].Value);
					connectionInfo.Inherit.RedirectPorts = bool.Parse(xmlnode.Attributes["InheritRedirectPorts"].Value);
					connectionInfo.Inherit.RedirectPrinters = bool.Parse(xmlnode.Attributes["InheritRedirectPrinters"].Value);
					connectionInfo.Inherit.RedirectSmartCards = bool.Parse(xmlnode.Attributes["InheritRedirectSmartCards"].Value);
					connectionInfo.Inherit.RedirectSound = bool.Parse(xmlnode.Attributes["InheritRedirectSound"].Value);
					connectionInfo.Inherit.Resolution = bool.Parse(xmlnode.Attributes["InheritResolution"].Value);
					connectionInfo.Inherit.UseConsoleSession = bool.Parse(xmlnode.Attributes["InheritUseConsoleSession"].Value);
					connectionInfo.Inherit.Username = bool.Parse(xmlnode.Attributes["InheritUsername"].Value);
					connectionInfo.Icon = xmlnode.Attributes["Icon"].Value;
					connectionInfo.Panel = xmlnode.Attributes["Panel"].Value;
				}
				else
				{
                    connectionInfo.Inherit = new ConnectionInfoInheritance(connectionInfo, Convert.ToBoolean(xmlnode.Attributes["Inherit"].Value));
					connectionInfo.Icon = Convert.ToString(xmlnode.Attributes["Icon"].Value.Replace(".ico", ""));
					connectionInfo.Panel = Language.strGeneral;
				}
				
				if (this.confVersion > 1.4) //1.5
				{
					connectionInfo.PleaseConnect = bool.Parse(xmlnode.Attributes["Connected"].Value);
				}
				
				if (this.confVersion > 1.5) //1.6
				{
                    connectionInfo.ICAEncryption = (ProtocolICA.EncryptionStrength)Tools.MiscTools.StringToEnum(typeof(ProtocolICA.EncryptionStrength), xmlnode.Attributes["ICAEncryptionStrength"].Value);
					connectionInfo.Inherit.ICAEncryption = bool.Parse(xmlnode.Attributes["InheritICAEncryptionStrength"].Value);
					connectionInfo.PreExtApp = xmlnode.Attributes["PreExtApp"].Value;
					connectionInfo.PostExtApp = xmlnode.Attributes["PostExtApp"].Value;
					connectionInfo.Inherit.PreExtApp = bool.Parse(xmlnode.Attributes["InheritPreExtApp"].Value);
					connectionInfo.Inherit.PostExtApp = bool.Parse(xmlnode.Attributes["InheritPostExtApp"].Value);
				}
				
				if (this.confVersion > 1.6) //1.7
				{
                    connectionInfo.VNCCompression = (ProtocolVNC.Compression)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Compression), xmlnode.Attributes["VNCCompression"].Value);
                    connectionInfo.VNCEncoding = (ProtocolVNC.Encoding)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Encoding), Convert.ToString(xmlnode.Attributes["VNCEncoding"].Value));
                    connectionInfo.VNCAuthMode = (ProtocolVNC.AuthMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.AuthMode), xmlnode.Attributes["VNCAuthMode"].Value);
                    connectionInfo.VNCProxyType = (ProtocolVNC.ProxyType)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.ProxyType), xmlnode.Attributes["VNCProxyType"].Value);
					connectionInfo.VNCProxyIP = xmlnode.Attributes["VNCProxyIP"].Value;
                    connectionInfo.VNCProxyPort = Convert.ToInt32(xmlnode.Attributes["VNCProxyPort"].Value);
					connectionInfo.VNCProxyUsername = xmlnode.Attributes["VNCProxyUsername"].Value;
					connectionInfo.VNCProxyPassword = Security.Crypt.Decrypt(xmlnode.Attributes["VNCProxyPassword"].Value, pW);
                    connectionInfo.VNCColors = (ProtocolVNC.Colors)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Colors), xmlnode.Attributes["VNCColors"].Value);
                    connectionInfo.VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.SmartSizeMode), xmlnode.Attributes["VNCSmartSizeMode"].Value);
					connectionInfo.VNCViewOnly = bool.Parse(xmlnode.Attributes["VNCViewOnly"].Value);
					connectionInfo.Inherit.VNCCompression = bool.Parse(xmlnode.Attributes["InheritVNCCompression"].Value);
					connectionInfo.Inherit.VNCEncoding = bool.Parse(xmlnode.Attributes["InheritVNCEncoding"].Value);
					connectionInfo.Inherit.VNCAuthMode = bool.Parse(xmlnode.Attributes["InheritVNCAuthMode"].Value);
					connectionInfo.Inherit.VNCProxyType = bool.Parse(xmlnode.Attributes["InheritVNCProxyType"].Value);
					connectionInfo.Inherit.VNCProxyIP = bool.Parse(xmlnode.Attributes["InheritVNCProxyIP"].Value);
					connectionInfo.Inherit.VNCProxyPort = bool.Parse(xmlnode.Attributes["InheritVNCProxyPort"].Value);
					connectionInfo.Inherit.VNCProxyUsername = bool.Parse(xmlnode.Attributes["InheritVNCProxyUsername"].Value);
					connectionInfo.Inherit.VNCProxyPassword = bool.Parse(xmlnode.Attributes["InheritVNCProxyPassword"].Value);
					connectionInfo.Inherit.VNCColors = bool.Parse(xmlnode.Attributes["InheritVNCColors"].Value);
					connectionInfo.Inherit.VNCSmartSizeMode = bool.Parse(xmlnode.Attributes["InheritVNCSmartSizeMode"].Value);
					connectionInfo.Inherit.VNCViewOnly = bool.Parse(xmlnode.Attributes["InheritVNCViewOnly"].Value);
				}
				
				if (this.confVersion > 1.7) //1.8
				{
                    connectionInfo.RDPAuthenticationLevel = (ProtocolRDP.AuthenticationLevel)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.AuthenticationLevel), xmlnode.Attributes["RDPAuthenticationLevel"].Value);
					connectionInfo.Inherit.RDPAuthenticationLevel = bool.Parse(xmlnode.Attributes["InheritRDPAuthenticationLevel"].Value);
				}
				
				if (this.confVersion > 1.8) //1.9
				{
                    connectionInfo.RenderingEngine = (HTTPBase.RenderingEngine)Tools.MiscTools.StringToEnum(typeof(HTTPBase.RenderingEngine), xmlnode.Attributes["RenderingEngine"].Value);
					connectionInfo.MacAddress = xmlnode.Attributes["MacAddress"].Value;
					connectionInfo.Inherit.RenderingEngine = bool.Parse(xmlnode.Attributes["InheritRenderingEngine"].Value);
					connectionInfo.Inherit.MacAddress = bool.Parse(xmlnode.Attributes["InheritMacAddress"].Value);
				}
				
				if (this.confVersion > 1.9) //2.0
				{
					connectionInfo.UserField = xmlnode.Attributes["UserField"].Value;
					connectionInfo.Inherit.UserField = bool.Parse(xmlnode.Attributes["InheritUserField"].Value);
				}
				
				if (this.confVersion > 2.0) //2.1
				{
					connectionInfo.ExtApp = xmlnode.Attributes["ExtApp"].Value;
					connectionInfo.Inherit.ExtApp = bool.Parse(xmlnode.Attributes["InheritExtApp"].Value);
				}
				
				if (this.confVersion > 2.1) //2.2
				{
					// Get settings
                    connectionInfo.RDGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUsageMethod), Convert.ToString(xmlnode.Attributes["RDGatewayUsageMethod"].Value));
					connectionInfo.RDGatewayHostname = xmlnode.Attributes["RDGatewayHostname"].Value;
                    connectionInfo.RDGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), Convert.ToString(xmlnode.Attributes["RDGatewayUseConnectionCredentials"].Value));
					connectionInfo.RDGatewayUsername = xmlnode.Attributes["RDGatewayUsername"].Value;
					connectionInfo.RDGatewayPassword = Security.Crypt.Decrypt(Convert.ToString(xmlnode.Attributes["RDGatewayPassword"].Value), pW);
					connectionInfo.RDGatewayDomain = xmlnode.Attributes["RDGatewayDomain"].Value;
							
					// Get inheritance settings
					connectionInfo.Inherit.RDGatewayUsageMethod = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsageMethod"].Value);
					connectionInfo.Inherit.RDGatewayHostname = bool.Parse(xmlnode.Attributes["InheritRDGatewayHostname"].Value);
					connectionInfo.Inherit.RDGatewayUseConnectionCredentials = bool.Parse(xmlnode.Attributes["InheritRDGatewayUseConnectionCredentials"].Value);
					connectionInfo.Inherit.RDGatewayUsername = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsername"].Value);
					connectionInfo.Inherit.RDGatewayPassword = bool.Parse(xmlnode.Attributes["InheritRDGatewayPassword"].Value);
					connectionInfo.Inherit.RDGatewayDomain = bool.Parse(xmlnode.Attributes["InheritRDGatewayDomain"].Value);
				}
				
				if (this.confVersion > 2.2) //2.3
				{
					// Get settings
					connectionInfo.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["EnableFontSmoothing"].Value);
					connectionInfo.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["EnableDesktopComposition"].Value);
							
					// Get inheritance settings
					connectionInfo.Inherit.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["InheritEnableFontSmoothing"].Value);
					connectionInfo.Inherit.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["InheritEnableDesktopComposition"].Value);
				}
				
				if (confVersion >= 2.4)
				{
					connectionInfo.UseCredSsp = bool.Parse(xmlnode.Attributes["UseCredSsp"].Value);
					connectionInfo.Inherit.UseCredSsp = bool.Parse(xmlnode.Attributes["InheritUseCredSsp"].Value);
				}
				
				if (confVersion >= 2.5)
				{
					connectionInfo.LoadBalanceInfo = xmlnode.Attributes["LoadBalanceInfo"].Value;
					connectionInfo.AutomaticResize = bool.Parse(xmlnode.Attributes["AutomaticResize"].Value);
					connectionInfo.Inherit.LoadBalanceInfo = bool.Parse(xmlnode.Attributes["InheritLoadBalanceInfo"].Value);
					connectionInfo.Inherit.AutomaticResize = bool.Parse(xmlnode.Attributes["InheritAutomaticResize"].Value);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(Language.strGetConnectionInfoFromXmlFailed, connectionInfo.Name, this.ConnectionFileName, ex.Message), false);
			}
			return connectionInfo;
		}
				
		private bool Authenticate(string Value, bool CompareToOriginalValue, RootNodeInfo rootInfo = null)
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
					pW = Tools.MiscTools.PasswordDialog(passwordName, false);
							
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
					pW = Tools.MiscTools.PasswordDialog(passwordName, false);
							
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