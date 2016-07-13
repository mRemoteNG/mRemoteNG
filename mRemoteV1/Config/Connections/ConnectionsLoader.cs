using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.My;
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
using mRemoteNG.UI.Forms;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.TaskDialog;

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
			get { return _ConnectionFileName; }
			set { _ConnectionFileName = value; }
		}
		
		public TreeNode RootTreeNode {get; set;}
		
		public ConnectionList ConnectionList {get; set;}
		
        public ContainerList ContainerList
		{
			get { return _ContainerList; }
			set { _ContainerList = value; }
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
                    CTaskDialog.ShowTaskDialogBox(
                        frmMain.Default, 
                        Application.ProductName, 
                        "Incompatible database schema",
                        $"The database schema on the server is not supported. Please upgrade to a newer version of {Application.ProductName}.", 
                        string.Format("Schema Version: {1}{0}Highest Supported Version: {2}", Environment.NewLine, confVersion.ToString(), maxSupportedSchemaVersion.ToString()), 
                        "", 
                        "", 
                        "", 
                        "", 
                        ETaskDialogButtons.Ok, 
                        ESysIcons.Error, 
                        ESysIcons.Error
                    );
					throw (new Exception($"Incompatible database schema (schema version {confVersion})."));
				}
						
				RootTreeNode.Name = Convert.ToString(sqlRd["Name"]);
						
				var rootInfo = new RootNodeInfo(RootNodeType.Connection);
				rootInfo.Name = RootTreeNode.Name;
				rootInfo.TreeNode = RootTreeNode;
						
				RootTreeNode.Tag = rootInfo;
				RootTreeNode.ImageIndex = (int)TreeImageType.Root;
                RootTreeNode.SelectedImageIndex = (int)TreeImageType.Root;
						
				if (Security.LegacyRijndaelCryptographyProvider.Decrypt(Convert.ToString(sqlRd["Protected"]), pW) != "ThisIsNotProtected")
				{
					if (Authenticate(Convert.ToString(sqlRd["Protected"]), false, rootInfo) == false)
					{
						mRemoteNG.Settings.Default.LoadConsFromCustomLocation = false;
                        mRemoteNG.Settings.Default.CustomConsPath = "";
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
				foreach (ContainerInfo contI in _ContainerList)
				{
					if (contI.IsExpanded == true)
					{
						contI.TreeNode.Expand();
					}
				}

                Windows.treeForm.tvConnections.EndUpdate();
						
				//open connections from last mremote session
				if (mRemoteNG.Settings.Default.OpenConsFromLastSession && !mRemoteNG.Settings.Default.NoReconnect)
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
							
					if (ConnectionTreeNode.GetNodeTypeFromString(Convert.ToString(sqlRd["Type"])) == TreeNodeType.Connection)
					{
                        ConnectionInfo conI = GetConnectionInfoFromSQL();
						conI.TreeNode = tNode;
						//conI.Parent = _previousContainer 'NEW
								
						ConnectionList.Add(conI);
								
						tNode.Tag = conI;
								
						if (SQLUpdate == true)
						{
                            ConnectionInfo prevCon = PreviousConnectionList.FindByConstantID(conI.ConstantID);
									
							if (prevCon != null)
							{
								foreach (ProtocolBase prot in prevCon.OpenConnections)
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
					else if (ConnectionTreeNode.GetNodeTypeFromString(Convert.ToString(sqlRd["Type"])) == TreeNodeType.Container)
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
								
						_ContainerList.Add(contI);
						ConnectionList.Add(conI);
								
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
						TreeNode pNode = ConnectionTreeNode.GetNodeFromConstantID(Convert.ToString(sqlRd["ParentID"]));
								
						if (pNode != null)
						{
							pNode.Nodes.Add(tNode);
									
							if (ConnectionTreeNode.GetNodeType(tNode) == TreeNodeType.Connection)
							{
								(tNode.Tag as ConnectionInfo).Parent = (ContainerInfo)pNode.Tag;
							}
							else if (ConnectionTreeNode.GetNodeType(tNode) == TreeNodeType.Container)
							{
								(tNode.Tag as ContainerInfo).Parent = (ContainerInfo)pNode.Tag;
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
				connectionInfo.Password = Security.LegacyRijndaelCryptographyProvider.Decrypt(Convert.ToString(sqlRd["Password"]), pW);
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
				connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo);
				connectionInfo.Inheritance.CacheBitmaps = Convert.ToBoolean(sqlRd["InheritCacheBitmaps"]);
				connectionInfo.Inheritance.Colors = Convert.ToBoolean(sqlRd["InheritColors"]);
				connectionInfo.Inheritance.Description = Convert.ToBoolean(sqlRd["InheritDescription"]);
				connectionInfo.Inheritance.DisplayThemes = Convert.ToBoolean(sqlRd["InheritDisplayThemes"]);
				connectionInfo.Inheritance.DisplayWallpaper = Convert.ToBoolean(sqlRd["InheritDisplayWallpaper"]);
				connectionInfo.Inheritance.Domain = Convert.ToBoolean(sqlRd["InheritDomain"]);
				connectionInfo.Inheritance.Icon = Convert.ToBoolean(sqlRd["InheritIcon"]);
				connectionInfo.Inheritance.Panel = Convert.ToBoolean(sqlRd["InheritPanel"]);
				connectionInfo.Inheritance.Password = Convert.ToBoolean(sqlRd["InheritPassword"]);
				connectionInfo.Inheritance.Port = Convert.ToBoolean(sqlRd["InheritPort"]);
				connectionInfo.Inheritance.Protocol = Convert.ToBoolean(sqlRd["InheritProtocol"]);
				connectionInfo.Inheritance.PuttySession = Convert.ToBoolean(sqlRd["InheritPuttySession"]);
				connectionInfo.Inheritance.RedirectDiskDrives = Convert.ToBoolean(sqlRd["InheritRedirectDiskDrives"]);
				connectionInfo.Inheritance.RedirectKeys = Convert.ToBoolean(sqlRd["InheritRedirectKeys"]);
				connectionInfo.Inheritance.RedirectPorts = Convert.ToBoolean(sqlRd["InheritRedirectPorts"]);
				connectionInfo.Inheritance.RedirectPrinters = Convert.ToBoolean(sqlRd["InheritRedirectPrinters"]);
				connectionInfo.Inheritance.RedirectSmartCards = Convert.ToBoolean(sqlRd["InheritRedirectSmartCards"]);
				connectionInfo.Inheritance.RedirectSound = Convert.ToBoolean(sqlRd["InheritRedirectSound"]);
				connectionInfo.Inheritance.Resolution = Convert.ToBoolean(sqlRd["InheritResolution"]);
				connectionInfo.Inheritance.UseConsoleSession = Convert.ToBoolean(sqlRd["InheritUseConsoleSession"]);
				connectionInfo.Inheritance.Username = Convert.ToBoolean(sqlRd["InheritUsername"]);
				connectionInfo.Icon = Convert.ToString(sqlRd["Icon"]);
				connectionInfo.Panel = Convert.ToString(sqlRd["Panel"]);
						
				if (confVersion > 1.5) //1.6
				{
                    connectionInfo.ICAEncryption = (ProtocolICA.EncryptionStrength)Tools.MiscTools.StringToEnum(typeof(ProtocolICA.EncryptionStrength), Convert.ToString(sqlRd["ICAEncryptionStrength"]));
					connectionInfo.Inheritance.ICAEncryption = Convert.ToBoolean(sqlRd["InheritICAEncryptionStrength"]);
					connectionInfo.PreExtApp = Convert.ToString(sqlRd["PreExtApp"]);
					connectionInfo.PostExtApp = Convert.ToString(sqlRd["PostExtApp"]);
					connectionInfo.Inheritance.PreExtApp = Convert.ToBoolean(sqlRd["InheritPreExtApp"]);
					connectionInfo.Inheritance.PostExtApp = Convert.ToBoolean(sqlRd["InheritPostExtApp"]);
				}
						
				if (confVersion > 1.6) //1.7
				{
                    connectionInfo.VNCCompression = (ProtocolVNC.Compression)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Compression), Convert.ToString(sqlRd["VNCCompression"]));
                    connectionInfo.VNCEncoding = (ProtocolVNC.Encoding)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Encoding), Convert.ToString(sqlRd["VNCEncoding"]));
                    connectionInfo.VNCAuthMode = (ProtocolVNC.AuthMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.AuthMode), Convert.ToString(sqlRd["VNCAuthMode"]));
                    connectionInfo.VNCProxyType = (ProtocolVNC.ProxyType)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.ProxyType), Convert.ToString(sqlRd["VNCProxyType"]));
					connectionInfo.VNCProxyIP = Convert.ToString(sqlRd["VNCProxyIP"]);
					connectionInfo.VNCProxyPort = Convert.ToInt32(sqlRd["VNCProxyPort"]);
					connectionInfo.VNCProxyUsername = Convert.ToString(sqlRd["VNCProxyUsername"]);
					connectionInfo.VNCProxyPassword = Security.LegacyRijndaelCryptographyProvider.Decrypt(Convert.ToString(sqlRd["VNCProxyPassword"]), pW);
                    connectionInfo.VNCColors = (ProtocolVNC.Colors)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Colors), Convert.ToString(sqlRd["VNCColors"]));
                    connectionInfo.VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.SmartSizeMode), Convert.ToString(sqlRd["VNCSmartSizeMode"]));
					connectionInfo.VNCViewOnly = Convert.ToBoolean(sqlRd["VNCViewOnly"]);
					connectionInfo.Inheritance.VNCCompression = Convert.ToBoolean(sqlRd["InheritVNCCompression"]);
					connectionInfo.Inheritance.VNCEncoding = Convert.ToBoolean(sqlRd["InheritVNCEncoding"]);
					connectionInfo.Inheritance.VNCAuthMode = Convert.ToBoolean(sqlRd["InheritVNCAuthMode"]);
					connectionInfo.Inheritance.VNCProxyType = Convert.ToBoolean(sqlRd["InheritVNCProxyType"]);
					connectionInfo.Inheritance.VNCProxyIP = Convert.ToBoolean(sqlRd["InheritVNCProxyIP"]);
					connectionInfo.Inheritance.VNCProxyPort = Convert.ToBoolean(sqlRd["InheritVNCProxyPort"]);
					connectionInfo.Inheritance.VNCProxyUsername = Convert.ToBoolean(sqlRd["InheritVNCProxyUsername"]);
					connectionInfo.Inheritance.VNCProxyPassword = Convert.ToBoolean(sqlRd["InheritVNCProxyPassword"]);
					connectionInfo.Inheritance.VNCColors = Convert.ToBoolean(sqlRd["InheritVNCColors"]);
					connectionInfo.Inheritance.VNCSmartSizeMode = Convert.ToBoolean(sqlRd["InheritVNCSmartSizeMode"]);
					connectionInfo.Inheritance.VNCViewOnly = Convert.ToBoolean(sqlRd["InheritVNCViewOnly"]);
				}
				
				if (confVersion > 1.7) //1.8
				{
                    connectionInfo.RDPAuthenticationLevel = (ProtocolRDP.AuthenticationLevel)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.AuthenticationLevel), Convert.ToString(sqlRd["RDPAuthenticationLevel"]));
					connectionInfo.Inheritance.RDPAuthenticationLevel = Convert.ToBoolean(sqlRd["InheritRDPAuthenticationLevel"]);
				}
				
				if (confVersion > 1.8) //1.9
				{
                    connectionInfo.RenderingEngine = (HTTPBase.RenderingEngine)Tools.MiscTools.StringToEnum(typeof(HTTPBase.RenderingEngine), Convert.ToString(sqlRd["RenderingEngine"]));
					connectionInfo.MacAddress = Convert.ToString(sqlRd["MacAddress"]);
					connectionInfo.Inheritance.RenderingEngine = Convert.ToBoolean(sqlRd["InheritRenderingEngine"]);
					connectionInfo.Inheritance.MacAddress = Convert.ToBoolean(sqlRd["InheritMacAddress"]);
				}
				
				if (confVersion > 1.9) //2.0
				{
					connectionInfo.UserField = Convert.ToString(sqlRd["UserField"]);
					connectionInfo.Inheritance.UserField = Convert.ToBoolean(sqlRd["InheritUserField"]);
				}
				
				if (confVersion > 2.0) //2.1
				{
					connectionInfo.ExtApp = Convert.ToString(sqlRd["ExtApp"]);
					connectionInfo.Inheritance.ExtApp = Convert.ToBoolean(sqlRd["InheritExtApp"]);
				}
						
				if (confVersion >= 2.2)
				{
                    connectionInfo.RDGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUsageMethod), Convert.ToString(sqlRd["RDGatewayUsageMethod"]));
					connectionInfo.RDGatewayHostname = Convert.ToString(sqlRd["RDGatewayHostname"]);
                    connectionInfo.RDGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), Convert.ToString(sqlRd["RDGatewayUseConnectionCredentials"]));
					connectionInfo.RDGatewayUsername = Convert.ToString(sqlRd["RDGatewayUsername"]);
					connectionInfo.RDGatewayPassword = Security.LegacyRijndaelCryptographyProvider.Decrypt(Convert.ToString(sqlRd["RDGatewayPassword"]), pW);
					connectionInfo.RDGatewayDomain = Convert.ToString(sqlRd["RDGatewayDomain"]);
					connectionInfo.Inheritance.RDGatewayUsageMethod = Convert.ToBoolean(sqlRd["InheritRDGatewayUsageMethod"]);
					connectionInfo.Inheritance.RDGatewayHostname = Convert.ToBoolean(sqlRd["InheritRDGatewayHostname"]);
					connectionInfo.Inheritance.RDGatewayUsername = Convert.ToBoolean(sqlRd["InheritRDGatewayUsername"]);
					connectionInfo.Inheritance.RDGatewayPassword = Convert.ToBoolean(sqlRd["InheritRDGatewayPassword"]);
					connectionInfo.Inheritance.RDGatewayDomain = Convert.ToBoolean(sqlRd["InheritRDGatewayDomain"]);
				}
						
				if (confVersion >= 2.3)
				{
					connectionInfo.EnableFontSmoothing = Convert.ToBoolean(sqlRd["EnableFontSmoothing"]);
					connectionInfo.EnableDesktopComposition = Convert.ToBoolean(sqlRd["EnableDesktopComposition"]);
					connectionInfo.Inheritance.EnableFontSmoothing = Convert.ToBoolean(sqlRd["InheritEnableFontSmoothing"]);
					connectionInfo.Inheritance.EnableDesktopComposition = Convert.ToBoolean(sqlRd["InheritEnableDesktopComposition"]);
				}
						
				if (confVersion >= 2.4)
				{
					connectionInfo.UseCredSsp = Convert.ToBoolean(sqlRd["UseCredSsp"]);
					connectionInfo.Inheritance.UseCredSsp = Convert.ToBoolean(sqlRd["InheritUseCredSsp"]);
				}
						
				if (confVersion >= 2.5)
				{
					connectionInfo.LoadBalanceInfo = Convert.ToString(sqlRd["LoadBalanceInfo"]);
					connectionInfo.AutomaticResize = Convert.ToBoolean(sqlRd["AutomaticResize"]);
					connectionInfo.Inheritance.LoadBalanceInfo = Convert.ToBoolean(sqlRd["InheritLoadBalanceInfo"]);
					connectionInfo.Inheritance.AutomaticResize = Convert.ToBoolean(sqlRd["InheritAutomaticResize"]);
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
			StreamReader sRd = new StreamReader(_ConnectionFileName);
					
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
					strDecr = Security.LegacyRijndaelCryptographyProvider.Decrypt(strCons, pW);
							
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
						strDecr = Security.LegacyRijndaelCryptographyProvider.Decrypt(strCons, pW);
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
				xDom = new XmlDocument();
				if (cons != "")
				{
					xDom.LoadXml(cons);
				}
				else
				{
					xDom.Load(_ConnectionFileName);
				}
						
				if (xDom.DocumentElement.HasAttribute("ConfVersion"))
				{
					confVersion = Convert.ToDouble(xDom.DocumentElement.Attributes["ConfVersion"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
				}
				else
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, Language.strOldConffile);
				}
						
				const double maxSupportedConfVersion = 2.5;
				if (confVersion > maxSupportedConfVersion)
				{
                    CTaskDialog.ShowTaskDialogBox(
                        frmMain.Default,
                        Application.ProductName, 
                        "Incompatible connection file format", 
                        string.Format("The format of this connection file is not supported. Please upgrade to a newer version of {0}.", Application.ProductName), 
                        string.Format("{1}{0}File Format Version: {2}{0}Highest Supported Version: {3}", Environment.NewLine, ConnectionFileName, confVersion.ToString(), maxSupportedConfVersion.ToString()),
                        "", 
                        "", 
                        "", 
                        "", 
                        ETaskDialogButtons.Ok, 
                        ESysIcons.Error,
                        ESysIcons.Error
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
						
				if (confVersion > 1.3) //1.4
				{
					if (Security.LegacyRijndaelCryptographyProvider.Decrypt(Convert.ToString(xDom.DocumentElement.Attributes["Protected"].Value), pW) != "ThisIsNotProtected")
					{
						if (Authenticate(Convert.ToString(xDom.DocumentElement.Attributes["Protected"].Value), false, rootInfo) == false)
						{
                            mRemoteNG.Settings.Default.LoadConsFromCustomLocation = false;
                            mRemoteNG.Settings.Default.CustomConsPath = "";
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
				foreach (ContainerInfo contI in _ContainerList)
				{
					if (contI.IsExpanded == true)
					{
						contI.TreeNode.Expand();
					}
				}

                Windows.treeForm.tvConnections.EndUpdate();
						
				//open connections from last mremote session
				if (mRemoteNG.Settings.Default.OpenConsFromLastSession && !mRemoteNG.Settings.Default.NoReconnect)
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
				Runtime.IsConnectionsFileLoaded = false;
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
								
						if (ConnectionTreeNode.GetNodeTypeFromString(xmlNode.Attributes["Type"].Value) == TreeNodeType.Connection) //connection info
						{
							ConnectionInfo connectionInfo = GetConnectionInfoFromXml(xmlNode);
							connectionInfo.TreeNode = treeNode;
							connectionInfo.Parent = _previousContainer; //NEW
									
							ConnectionList.Add(connectionInfo);
									
							treeNode.Tag = connectionInfo;
                            treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
                            treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
						}
						else if (ConnectionTreeNode.GetNodeTypeFromString(xmlNode.Attributes["Type"].Value) == TreeNodeType.Container) //container info
						{
							ContainerInfo containerInfo = new ContainerInfo();
							if (treeNode.Parent != null)
							{
								if (ConnectionTreeNode.GetNodeType(treeNode.Parent) == TreeNodeType.Container)
								{
									containerInfo.Parent = (ContainerInfo)treeNode.Parent.Tag;
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
				if (confVersion > 0.1) //0.2
				{
					connectionInfo.Name = xmlnode.Attributes["Name"].Value;
					connectionInfo.Description = xmlnode.Attributes["Descr"].Value;
					connectionInfo.Hostname = xmlnode.Attributes["Hostname"].Value;
					connectionInfo.Username = xmlnode.Attributes["Username"].Value;
					connectionInfo.Password = Security.LegacyRijndaelCryptographyProvider.Decrypt(xmlnode.Attributes["Password"].Value, pW);
					connectionInfo.Domain = xmlnode.Attributes["Domain"].Value;
					connectionInfo.DisplayWallpaper = bool.Parse(xmlnode.Attributes["DisplayWallpaper"].Value);
					connectionInfo.DisplayThemes = bool.Parse(xmlnode.Attributes["DisplayThemes"].Value);
					connectionInfo.CacheBitmaps = bool.Parse(xmlnode.Attributes["CacheBitmaps"].Value);
							
					if (confVersion < 1.1) //1.0 - 0.1
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
						
				if (confVersion > 0.2) //0.3
				{
					if (confVersion < 0.7)
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
						
				if (confVersion > 0.3) //0.4
				{
					if (confVersion < 0.7)
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
					if (confVersion < 0.7)
					{
						if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value) == true)
							connectionInfo.Port = (int)ProtocolVNC.Defaults.Port;
						else
							connectionInfo.Port = (int)ProtocolRDP.Defaults.Port;
					}
					connectionInfo.UseConsoleSession = false;
				}
				
				if (confVersion > 0.4) //0.5 and 0.6
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
				
				if (confVersion > 0.6) //0.7
				{
                    connectionInfo.Protocol = (ProtocolType)Tools.MiscTools.StringToEnum(typeof(ProtocolType), xmlnode.Attributes["Protocol"].Value);
                    connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["Port"].Value);
				}
				
				if (confVersion > 0.9) //1.0
				{
					connectionInfo.RedirectKeys = bool.Parse(xmlnode.Attributes["RedirectKeys"].Value);
				}
				
				if (confVersion > 1.1) //1.2
				{
					connectionInfo.PuttySession = xmlnode.Attributes["PuttySession"].Value;
				}
				
				if (confVersion > 1.2) //1.3
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
				
				if (confVersion > 1.2) //1.3
				{
					connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo);
					connectionInfo.Inheritance.CacheBitmaps = bool.Parse(xmlnode.Attributes["InheritCacheBitmaps"].Value);
					connectionInfo.Inheritance.Colors = bool.Parse(xmlnode.Attributes["InheritColors"].Value);
					connectionInfo.Inheritance.Description = bool.Parse(xmlnode.Attributes["InheritDescription"].Value);
					connectionInfo.Inheritance.DisplayThemes = bool.Parse(xmlnode.Attributes["InheritDisplayThemes"].Value);
					connectionInfo.Inheritance.DisplayWallpaper = bool.Parse(xmlnode.Attributes["InheritDisplayWallpaper"].Value);
					connectionInfo.Inheritance.Domain = bool.Parse(xmlnode.Attributes["InheritDomain"].Value);
					connectionInfo.Inheritance.Icon = bool.Parse(xmlnode.Attributes["InheritIcon"].Value);
					connectionInfo.Inheritance.Panel = bool.Parse(xmlnode.Attributes["InheritPanel"].Value);
					connectionInfo.Inheritance.Password = bool.Parse(xmlnode.Attributes["InheritPassword"].Value);
					connectionInfo.Inheritance.Port = bool.Parse(xmlnode.Attributes["InheritPort"].Value);
					connectionInfo.Inheritance.Protocol = bool.Parse(xmlnode.Attributes["InheritProtocol"].Value);
					connectionInfo.Inheritance.PuttySession = bool.Parse(xmlnode.Attributes["InheritPuttySession"].Value);
					connectionInfo.Inheritance.RedirectDiskDrives = bool.Parse(xmlnode.Attributes["InheritRedirectDiskDrives"].Value);
					connectionInfo.Inheritance.RedirectKeys = bool.Parse(xmlnode.Attributes["InheritRedirectKeys"].Value);
					connectionInfo.Inheritance.RedirectPorts = bool.Parse(xmlnode.Attributes["InheritRedirectPorts"].Value);
					connectionInfo.Inheritance.RedirectPrinters = bool.Parse(xmlnode.Attributes["InheritRedirectPrinters"].Value);
					connectionInfo.Inheritance.RedirectSmartCards = bool.Parse(xmlnode.Attributes["InheritRedirectSmartCards"].Value);
					connectionInfo.Inheritance.RedirectSound = bool.Parse(xmlnode.Attributes["InheritRedirectSound"].Value);
					connectionInfo.Inheritance.Resolution = bool.Parse(xmlnode.Attributes["InheritResolution"].Value);
					connectionInfo.Inheritance.UseConsoleSession = bool.Parse(xmlnode.Attributes["InheritUseConsoleSession"].Value);
					connectionInfo.Inheritance.Username = bool.Parse(xmlnode.Attributes["InheritUsername"].Value);
					connectionInfo.Icon = xmlnode.Attributes["Icon"].Value;
					connectionInfo.Panel = xmlnode.Attributes["Panel"].Value;
				}
				else
				{
                    connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo, Convert.ToBoolean(xmlnode.Attributes["Inherit"].Value));
					connectionInfo.Icon = Convert.ToString(xmlnode.Attributes["Icon"].Value.Replace(".ico", ""));
					connectionInfo.Panel = Language.strGeneral;
				}
				
				if (confVersion > 1.4) //1.5
				{
					connectionInfo.PleaseConnect = bool.Parse(xmlnode.Attributes["Connected"].Value);
				}
				
				if (confVersion > 1.5) //1.6
				{
                    connectionInfo.ICAEncryption = (ProtocolICA.EncryptionStrength)Tools.MiscTools.StringToEnum(typeof(ProtocolICA.EncryptionStrength), xmlnode.Attributes["ICAEncryptionStrength"].Value);
					connectionInfo.Inheritance.ICAEncryption = bool.Parse(xmlnode.Attributes["InheritICAEncryptionStrength"].Value);
					connectionInfo.PreExtApp = xmlnode.Attributes["PreExtApp"].Value;
					connectionInfo.PostExtApp = xmlnode.Attributes["PostExtApp"].Value;
					connectionInfo.Inheritance.PreExtApp = bool.Parse(xmlnode.Attributes["InheritPreExtApp"].Value);
					connectionInfo.Inheritance.PostExtApp = bool.Parse(xmlnode.Attributes["InheritPostExtApp"].Value);
				}
				
				if (confVersion > 1.6) //1.7
				{
                    connectionInfo.VNCCompression = (ProtocolVNC.Compression)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Compression), xmlnode.Attributes["VNCCompression"].Value);
                    connectionInfo.VNCEncoding = (ProtocolVNC.Encoding)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Encoding), Convert.ToString(xmlnode.Attributes["VNCEncoding"].Value));
                    connectionInfo.VNCAuthMode = (ProtocolVNC.AuthMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.AuthMode), xmlnode.Attributes["VNCAuthMode"].Value);
                    connectionInfo.VNCProxyType = (ProtocolVNC.ProxyType)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.ProxyType), xmlnode.Attributes["VNCProxyType"].Value);
					connectionInfo.VNCProxyIP = xmlnode.Attributes["VNCProxyIP"].Value;
                    connectionInfo.VNCProxyPort = Convert.ToInt32(xmlnode.Attributes["VNCProxyPort"].Value);
					connectionInfo.VNCProxyUsername = xmlnode.Attributes["VNCProxyUsername"].Value;
					connectionInfo.VNCProxyPassword = Security.LegacyRijndaelCryptographyProvider.Decrypt(xmlnode.Attributes["VNCProxyPassword"].Value, pW);
                    connectionInfo.VNCColors = (ProtocolVNC.Colors)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Colors), xmlnode.Attributes["VNCColors"].Value);
                    connectionInfo.VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.SmartSizeMode), xmlnode.Attributes["VNCSmartSizeMode"].Value);
					connectionInfo.VNCViewOnly = bool.Parse(xmlnode.Attributes["VNCViewOnly"].Value);
					connectionInfo.Inheritance.VNCCompression = bool.Parse(xmlnode.Attributes["InheritVNCCompression"].Value);
					connectionInfo.Inheritance.VNCEncoding = bool.Parse(xmlnode.Attributes["InheritVNCEncoding"].Value);
					connectionInfo.Inheritance.VNCAuthMode = bool.Parse(xmlnode.Attributes["InheritVNCAuthMode"].Value);
					connectionInfo.Inheritance.VNCProxyType = bool.Parse(xmlnode.Attributes["InheritVNCProxyType"].Value);
					connectionInfo.Inheritance.VNCProxyIP = bool.Parse(xmlnode.Attributes["InheritVNCProxyIP"].Value);
					connectionInfo.Inheritance.VNCProxyPort = bool.Parse(xmlnode.Attributes["InheritVNCProxyPort"].Value);
					connectionInfo.Inheritance.VNCProxyUsername = bool.Parse(xmlnode.Attributes["InheritVNCProxyUsername"].Value);
					connectionInfo.Inheritance.VNCProxyPassword = bool.Parse(xmlnode.Attributes["InheritVNCProxyPassword"].Value);
					connectionInfo.Inheritance.VNCColors = bool.Parse(xmlnode.Attributes["InheritVNCColors"].Value);
					connectionInfo.Inheritance.VNCSmartSizeMode = bool.Parse(xmlnode.Attributes["InheritVNCSmartSizeMode"].Value);
					connectionInfo.Inheritance.VNCViewOnly = bool.Parse(xmlnode.Attributes["InheritVNCViewOnly"].Value);
				}
				
				if (confVersion > 1.7) //1.8
				{
                    connectionInfo.RDPAuthenticationLevel = (ProtocolRDP.AuthenticationLevel)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.AuthenticationLevel), xmlnode.Attributes["RDPAuthenticationLevel"].Value);
					connectionInfo.Inheritance.RDPAuthenticationLevel = bool.Parse(xmlnode.Attributes["InheritRDPAuthenticationLevel"].Value);
				}
				
				if (confVersion > 1.8) //1.9
				{
                    connectionInfo.RenderingEngine = (HTTPBase.RenderingEngine)Tools.MiscTools.StringToEnum(typeof(HTTPBase.RenderingEngine), xmlnode.Attributes["RenderingEngine"].Value);
					connectionInfo.MacAddress = xmlnode.Attributes["MacAddress"].Value;
					connectionInfo.Inheritance.RenderingEngine = bool.Parse(xmlnode.Attributes["InheritRenderingEngine"].Value);
					connectionInfo.Inheritance.MacAddress = bool.Parse(xmlnode.Attributes["InheritMacAddress"].Value);
				}
				
				if (confVersion > 1.9) //2.0
				{
					connectionInfo.UserField = xmlnode.Attributes["UserField"].Value;
					connectionInfo.Inheritance.UserField = bool.Parse(xmlnode.Attributes["InheritUserField"].Value);
				}
				
				if (confVersion > 2.0) //2.1
				{
					connectionInfo.ExtApp = xmlnode.Attributes["ExtApp"].Value;
					connectionInfo.Inheritance.ExtApp = bool.Parse(xmlnode.Attributes["InheritExtApp"].Value);
				}
				
				if (confVersion > 2.1) //2.2
				{
					// Get settings
                    connectionInfo.RDGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUsageMethod), Convert.ToString(xmlnode.Attributes["RDGatewayUsageMethod"].Value));
					connectionInfo.RDGatewayHostname = xmlnode.Attributes["RDGatewayHostname"].Value;
                    connectionInfo.RDGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), Convert.ToString(xmlnode.Attributes["RDGatewayUseConnectionCredentials"].Value));
					connectionInfo.RDGatewayUsername = xmlnode.Attributes["RDGatewayUsername"].Value;
					connectionInfo.RDGatewayPassword = Security.LegacyRijndaelCryptographyProvider.Decrypt(Convert.ToString(xmlnode.Attributes["RDGatewayPassword"].Value), pW);
					connectionInfo.RDGatewayDomain = xmlnode.Attributes["RDGatewayDomain"].Value;
							
					// Get inheritance settings
					connectionInfo.Inheritance.RDGatewayUsageMethod = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsageMethod"].Value);
					connectionInfo.Inheritance.RDGatewayHostname = bool.Parse(xmlnode.Attributes["InheritRDGatewayHostname"].Value);
					connectionInfo.Inheritance.RDGatewayUseConnectionCredentials = bool.Parse(xmlnode.Attributes["InheritRDGatewayUseConnectionCredentials"].Value);
					connectionInfo.Inheritance.RDGatewayUsername = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsername"].Value);
					connectionInfo.Inheritance.RDGatewayPassword = bool.Parse(xmlnode.Attributes["InheritRDGatewayPassword"].Value);
					connectionInfo.Inheritance.RDGatewayDomain = bool.Parse(xmlnode.Attributes["InheritRDGatewayDomain"].Value);
				}
				
				if (confVersion > 2.2) //2.3
				{
					// Get settings
					connectionInfo.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["EnableFontSmoothing"].Value);
					connectionInfo.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["EnableDesktopComposition"].Value);
							
					// Get inheritance settings
					connectionInfo.Inheritance.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["InheritEnableFontSmoothing"].Value);
					connectionInfo.Inheritance.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["InheritEnableDesktopComposition"].Value);
				}
				
				if (confVersion >= 2.4)
				{
					connectionInfo.UseCredSsp = bool.Parse(xmlnode.Attributes["UseCredSsp"].Value);
					connectionInfo.Inheritance.UseCredSsp = bool.Parse(xmlnode.Attributes["InheritUseCredSsp"].Value);
				}
				
				if (confVersion >= 2.5)
				{
					connectionInfo.LoadBalanceInfo = xmlnode.Attributes["LoadBalanceInfo"].Value;
					connectionInfo.AutomaticResize = bool.Parse(xmlnode.Attributes["AutomaticResize"].Value);
					connectionInfo.Inheritance.LoadBalanceInfo = bool.Parse(xmlnode.Attributes["InheritLoadBalanceInfo"].Value);
					connectionInfo.Inheritance.AutomaticResize = bool.Parse(xmlnode.Attributes["InheritAutomaticResize"].Value);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(Language.strGetConnectionInfoFromXmlFailed, connectionInfo.Name, ConnectionFileName, ex.Message), false);
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
				while (!(Security.LegacyRijndaelCryptographyProvider.Decrypt(Value, pW) != Value))
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
				while (!(Security.LegacyRijndaelCryptographyProvider.Decrypt(Value, pW) == "ThisIsProtected"))
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