using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using mRemoteNG.App;
using System.Data.SqlClient;
using System.IO;
using mRemoteNG.My;
using mRemoteNG.Connection;
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
				
		public Connection.ConnectionList ConnectionList {get; set;}
				
		private Container.List _ContainerList;
        public Container.List ContainerList
		{
			get { return this._ContainerList; }
			set { this._ContainerList = value; }
		}
				
		private Connection.ConnectionList _PreviousConnectionList;
        public Connection.ConnectionList PreviousConnectionList
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
                    cTaskDialog.ShowTaskDialogBox(
                        frmMain.Default, 
                        System.Windows.Forms.Application.ProductName, 
                        "Incompatible database schema", 
                        string.Format("The database schema on the server is not supported. Please upgrade to a newer version of {0}.", System.Windows.Forms.Application.ProductName), 
                        string.Format("Schema Version: {1}{0}Highest Supported Version: {2}", System.Environment.NewLine, confVersion.ToString(), maxSupportedSchemaVersion.ToString()), 
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
						
				RootTreeNode.Name = System.Convert.ToString(sqlRd["Name"]);
						
				Root.Info rootInfo = new Root.Info(Root.Info.RootType.Connection);
				rootInfo.Name = RootTreeNode.Name;
				rootInfo.TreeNode = RootTreeNode;
						
				RootTreeNode.Tag = rootInfo;
				RootTreeNode.ImageIndex = (int)Images.Enums.TreeImage.Root;
                RootTreeNode.SelectedImageIndex = (int)Images.Enums.TreeImage.Root;
						
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
					foreach (ConnectionRecordImp connectionRecord in ConnectionList)
					{
						if (connectionRecord.MetaData.PleaseConnect == true)
						{
                            Runtime.OpenConnection(connectionRecord);
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
						ConnectionRecord connectionRecord = GetConnectionInfoFromSQL();
						connectionRecord.TreeNode = tNode;
						//conI.Parent = _previousContainer 'NEW
								
						this.ConnectionList.Add(connectionRecord);
								
						tNode.Tag = connectionRecord;
								
						if (SQLUpdate == true)
						{
							ConnectionRecord prevCon = PreviousConnectionList.FindByConstantID(connectionRecord.ConstantID);
									
							if (prevCon != null)
							{
								foreach (Connection.Protocol.Base prot in ((ConnectionRecordImp)prevCon).OpenConnections)
								{
									prot.InterfaceControl.Info = connectionRecord;
									connectionRecord.OpenConnections.Add(prot);
								}
										
								if (connectionRecord.OpenConnections.Count > 0)
								{
                                    tNode.ImageIndex = (int)Images.Enums.TreeImage.ConnectionOpen;
                                    tNode.SelectedImageIndex = (int)Images.Enums.TreeImage.ConnectionOpen;
								}
								else
								{
                                    tNode.ImageIndex = (int)Images.Enums.TreeImage.ConnectionClosed;
                                    tNode.SelectedImageIndex = (int)Images.Enums.TreeImage.ConnectionClosed;
								}
							}
							else
							{
                                tNode.ImageIndex = (int)Images.Enums.TreeImage.ConnectionClosed;
                                tNode.SelectedImageIndex = (int)Images.Enums.TreeImage.ConnectionClosed;
							}
									
							if (connectionRecord.ConstantID == _PreviousSelected)
							{
								_selectedTreeNode = tNode;
							}
						}
						else
						{
                            tNode.ImageIndex = (int)Images.Enums.TreeImage.ConnectionClosed;
                            tNode.SelectedImageIndex = (int)Images.Enums.TreeImage.ConnectionClosed;
						}
					}
					else if (Tree.Node.GetNodeTypeFromString(System.Convert.ToString(sqlRd["Type"])) == Tree.Node.Type.Container)
					{
						Container.Info containerRecord = new Container.Info();
						//If tNode.Parent IsNot Nothing Then
						//    If Tree.Node.GetNodeType(tNode.Parent) = Tree.Node.Type.Container Then
						//        contI.Parent = tNode.Parent.Tag
						//    End If
						//End If
						//_previousContainer = contI 'NEW
						containerRecord.TreeNode = tNode;
								
						containerRecord.Name = System.Convert.ToString(sqlRd["Name"]);
								
						ConnectionRecord connectionRecord = default(ConnectionRecordImp);
								
						connectionRecord = GetConnectionInfoFromSQL();
								
						connectionRecord.Parent = containerRecord;
						connectionRecord.MetaData.IsContainer = true;
						containerRecord.ConnectionRecord = connectionRecord;
								
						if (SQLUpdate == true)
						{
							Container.Info prevCont = PreviousContainerList.FindByConstantID(connectionRecord.ConstantID);
							if (prevCont != null)
							{
								containerRecord.IsExpanded = prevCont.IsExpanded;
							}
									
							if (connectionRecord.ConstantID == _PreviousSelected)
							{
								_selectedTreeNode = tNode;
							}
						}
						else
						{
							if (System.Convert.ToBoolean(sqlRd["Expanded"]) == true)
								containerRecord.IsExpanded = true;
							else
								containerRecord.IsExpanded = false;
						}
						
						this._ContainerList.Add(containerRecord);
						this.ConnectionList.Add(connectionRecord);
						
						tNode.Tag = containerRecord;
                        tNode.ImageIndex = (int)Images.Enums.TreeImage.Container;
                        tNode.SelectedImageIndex = (int)Images.Enums.TreeImage.Container;
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
								(tNode.Tag as Connection.ConnectionRecordImp).Parent = (mRemoteNG.Container.Info)pNode.Tag;
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strAddNodesFromSqlFailed + System.Environment.NewLine + ex.Message, true);
			}
		}
		
		private ConnectionRecord GetConnectionInfoFromSQL()
		{
			try
			{
				ConnectionRecord connectionRecord = new Connection.ConnectionRecordImp();
				
				connectionRecord.MetaData.PositionID = System.Convert.ToInt32(sqlRd["PositionID"]);
				connectionRecord.ConstantID = System.Convert.ToString(sqlRd["ConstantID"]);
				connectionRecord.Name = System.Convert.ToString(sqlRd["Name"]);
				connectionRecord.Description = System.Convert.ToString(sqlRd["Description"]);
				connectionRecord.Hostname = System.Convert.ToString(sqlRd["Hostname"]);
				connectionRecord.Credential.Username = System.Convert.ToString(sqlRd["Username"]);
				connectionRecord.Credential.Password = Security.Crypt.Decrypt(System.Convert.ToString(sqlRd["Password"]), pW);
				connectionRecord.Credential.Domain = System.Convert.ToString(sqlRd["DomainName"]);
				connectionRecord.DisplayWallpaper = System.Convert.ToBoolean(sqlRd["DisplayWallpaper"]);
				connectionRecord.DisplayThemes = System.Convert.ToBoolean(sqlRd["DisplayThemes"]);
				connectionRecord.CacheBitmaps = System.Convert.ToBoolean(sqlRd["CacheBitmaps"]);
				connectionRecord.UseConsoleSession = System.Convert.ToBoolean(sqlRd["ConnectToConsole"]);
				
				connectionRecord.RedirectDiskDrives = System.Convert.ToBoolean(sqlRd["RedirectDiskDrives"]);
				connectionRecord.RedirectPrinters = System.Convert.ToBoolean(sqlRd["RedirectPrinters"]);
				connectionRecord.RedirectPorts = System.Convert.ToBoolean(sqlRd["RedirectPorts"]);
				connectionRecord.RedirectSmartCards = System.Convert.ToBoolean(sqlRd["RedirectSmartCards"]);
				connectionRecord.RedirectKeys = System.Convert.ToBoolean(sqlRd["RedirectKeys"]);
                connectionRecord.RedirectSound = (Connection.Protocol.RDP.RDPSounds)Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPSounds), System.Convert.ToString(sqlRd["RedirectSound"]));

                connectionRecord.Protocol = (Connection.Protocol.Protocols)Tools.Misc.StringToEnum(typeof(Connection.Protocol.Protocols), System.Convert.ToString(sqlRd["Protocol"]));
				connectionRecord.Port = System.Convert.ToInt32(sqlRd["Port"]);
				connectionRecord.PuttySession = System.Convert.ToString(sqlRd["PuttySession"]);
                
                connectionRecord.Colors = (Connection.Protocol.RDP.RDPColors)Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPColors), System.Convert.ToString(sqlRd["Colors"]));
                connectionRecord.Resolution = (Connection.Protocol.RDP.RDPResolutions)Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPResolutions), System.Convert.ToString(sqlRd["Resolution"]));
				
				connectionRecord.Inherit = new ConnectionRecordInheritanceImp(connectionRecord);
				connectionRecord.Inherit.CacheBitmaps = System.Convert.ToBoolean(sqlRd["InheritCacheBitmaps"]);
				connectionRecord.Inherit.Colors = System.Convert.ToBoolean(sqlRd["InheritColors"]);
				connectionRecord.Inherit.Description = System.Convert.ToBoolean(sqlRd["InheritDescription"]);
				connectionRecord.Inherit.DisplayThemes = System.Convert.ToBoolean(sqlRd["InheritDisplayThemes"]);
				connectionRecord.Inherit.DisplayWallpaper = System.Convert.ToBoolean(sqlRd["InheritDisplayWallpaper"]);
				connectionRecord.Inherit.Domain = System.Convert.ToBoolean(sqlRd["InheritDomain"]);
				connectionRecord.Inherit.Icon = System.Convert.ToBoolean(sqlRd["InheritIcon"]);
				connectionRecord.Inherit.Panel = System.Convert.ToBoolean(sqlRd["InheritPanel"]);
				connectionRecord.Inherit.Password = System.Convert.ToBoolean(sqlRd["InheritPassword"]);
				connectionRecord.Inherit.Port = System.Convert.ToBoolean(sqlRd["InheritPort"]);
				connectionRecord.Inherit.Protocol = System.Convert.ToBoolean(sqlRd["InheritProtocol"]);
				connectionRecord.Inherit.PuttySession = System.Convert.ToBoolean(sqlRd["InheritPuttySession"]);
				connectionRecord.Inherit.RedirectDiskDrives = System.Convert.ToBoolean(sqlRd["InheritRedirectDiskDrives"]);
				connectionRecord.Inherit.RedirectKeys = System.Convert.ToBoolean(sqlRd["InheritRedirectKeys"]);
				connectionRecord.Inherit.RedirectPorts = System.Convert.ToBoolean(sqlRd["InheritRedirectPorts"]);
				connectionRecord.Inherit.RedirectPrinters = System.Convert.ToBoolean(sqlRd["InheritRedirectPrinters"]);
				connectionRecord.Inherit.RedirectSmartCards = System.Convert.ToBoolean(sqlRd["InheritRedirectSmartCards"]);
				connectionRecord.Inherit.RedirectSound = System.Convert.ToBoolean(sqlRd["InheritRedirectSound"]);
				connectionRecord.Inherit.Resolution = System.Convert.ToBoolean(sqlRd["InheritResolution"]);
				connectionRecord.Inherit.UseConsoleSession = System.Convert.ToBoolean(sqlRd["InheritUseConsoleSession"]);
				connectionRecord.Inherit.Username = System.Convert.ToBoolean(sqlRd["InheritUsername"]);
						
				connectionRecord.Icon = System.Convert.ToString(sqlRd["Icon"]);
				connectionRecord.Panel = System.Convert.ToString(sqlRd["Panel"]);
				
				if (this.confVersion > 1.5) //1.6
				{
                    connectionRecord.ICAEncryption = (Connection.Protocol.ICA.EncryptionStrength)Tools.Misc.StringToEnum(typeof(Connection.Protocol.ICA.EncryptionStrength), System.Convert.ToString(sqlRd["ICAEncryptionStrength"]));
					connectionRecord.Inherit.ICAEncryption = System.Convert.ToBoolean(sqlRd["InheritICAEncryptionStrength"]);
							
					connectionRecord.ExternalTool.PreExtApp = System.Convert.ToString(sqlRd["PreExtApp"]);
                    connectionRecord.ExternalTool.PostExtApp = System.Convert.ToString(sqlRd["PostExtApp"]);
					connectionRecord.Inherit.PreExtApp = System.Convert.ToBoolean(sqlRd["InheritPreExtApp"]);
					connectionRecord.Inherit.PostExtApp = System.Convert.ToBoolean(sqlRd["InheritPostExtApp"]);
				}
				
				if (this.confVersion > 1.6) //1.7
				{
                    connectionRecord.VNCCompression = (Connection.Protocol.VNC.Compression)Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.Compression), System.Convert.ToString(sqlRd["VNCCompression"]));
                    connectionRecord.VNCEncoding = (Connection.Protocol.VNC.Encoding)Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.Encoding), System.Convert.ToString(sqlRd["VNCEncoding"]));
                    connectionRecord.VNCAuthMode = (Connection.Protocol.VNC.AuthMode)Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.AuthMode), System.Convert.ToString(sqlRd["VNCAuthMode"]));
                    connectionRecord.VNCProxyType = (Connection.Protocol.VNC.ProxyType)Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.ProxyType), System.Convert.ToString(sqlRd["VNCProxyType"]));
					connectionRecord.VNCProxyIP = System.Convert.ToString(sqlRd["VNCProxyIP"]);
					connectionRecord.VNCProxyPort = System.Convert.ToInt32(sqlRd["VNCProxyPort"]);
					connectionRecord.VNCProxyUsername = System.Convert.ToString(sqlRd["VNCProxyUsername"]);
					connectionRecord.VNCProxyPassword = Security.Crypt.Decrypt(System.Convert.ToString(sqlRd["VNCProxyPassword"]), pW);
                    connectionRecord.VNCColors = (Connection.Protocol.VNC.Colors)Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.Colors), System.Convert.ToString(sqlRd["VNCColors"]));
                    connectionRecord.VNCSmartSizeMode = (Connection.Protocol.VNC.SmartSizeMode)Tools.Misc.StringToEnum(typeof(Connection.Protocol.VNC.SmartSizeMode), System.Convert.ToString(sqlRd["VNCSmartSizeMode"]));
					connectionRecord.VNCViewOnly = System.Convert.ToBoolean(sqlRd["VNCViewOnly"]);
							
					connectionRecord.Inherit.VNCCompression = System.Convert.ToBoolean(sqlRd["InheritVNCCompression"]);
					connectionRecord.Inherit.VNCEncoding = System.Convert.ToBoolean(sqlRd["InheritVNCEncoding"]);
					connectionRecord.Inherit.VNCAuthMode = System.Convert.ToBoolean(sqlRd["InheritVNCAuthMode"]);
					connectionRecord.Inherit.VNCProxyType = System.Convert.ToBoolean(sqlRd["InheritVNCProxyType"]);
					connectionRecord.Inherit.VNCProxyIP = System.Convert.ToBoolean(sqlRd["InheritVNCProxyIP"]);
					connectionRecord.Inherit.VNCProxyPort = System.Convert.ToBoolean(sqlRd["InheritVNCProxyPort"]);
					connectionRecord.Inherit.VNCProxyUsername = System.Convert.ToBoolean(sqlRd["InheritVNCProxyUsername"]);
					connectionRecord.Inherit.VNCProxyPassword = System.Convert.ToBoolean(sqlRd["InheritVNCProxyPassword"]);
					connectionRecord.Inherit.VNCColors = System.Convert.ToBoolean(sqlRd["InheritVNCColors"]);
					connectionRecord.Inherit.VNCSmartSizeMode = System.Convert.ToBoolean(sqlRd["InheritVNCSmartSizeMode"]);
					connectionRecord.Inherit.VNCViewOnly = System.Convert.ToBoolean(sqlRd["InheritVNCViewOnly"]);
				}
				
				if (this.confVersion > 1.7) //1.8
				{
                    connectionRecord.RDPAuthenticationLevel = (Connection.Protocol.RDP.AuthenticationLevel)Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.AuthenticationLevel), System.Convert.ToString(sqlRd["RDPAuthenticationLevel"]));
							
					connectionRecord.Inherit.RDPAuthenticationLevel = System.Convert.ToBoolean(sqlRd["InheritRDPAuthenticationLevel"]);
				}
				
				if (this.confVersion > 1.8) //1.9
				{
                    connectionRecord.RenderingEngine = (Connection.Protocol.HTTPBase.RenderingEngine)Tools.Misc.StringToEnum(typeof(Connection.Protocol.HTTPBase.RenderingEngine), System.Convert.ToString(sqlRd["RenderingEngine"]));
                    connectionRecord.ExternalTool.MacAddress = System.Convert.ToString(sqlRd["MacAddress"]);
							
					connectionRecord.Inherit.RenderingEngine = System.Convert.ToBoolean(sqlRd["InheritRenderingEngine"]);
					connectionRecord.Inherit.MacAddress = System.Convert.ToBoolean(sqlRd["InheritMacAddress"]);
				}
				
				if (this.confVersion > 1.9) //2.0
				{
                    connectionRecord.ExternalTool.UserField = System.Convert.ToString(sqlRd["UserField"]);
							
					connectionRecord.Inherit.UserField = System.Convert.ToBoolean(sqlRd["InheritUserField"]);
				}
				
				if (this.confVersion > 2.0) //2.1
				{
					connectionRecord.ExtApp = System.Convert.ToString(sqlRd["ExtApp"]);
							
					connectionRecord.Inherit.ExtApp = System.Convert.ToBoolean(sqlRd["InheritExtApp"]);
				}
				
				if (this.confVersion >= 2.2)
				{
                    connectionRecord.RDGatewayUsageMethod = (mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod), System.Convert.ToString(sqlRd["RDGatewayUsageMethod"]));
					connectionRecord.RDGatewayHostname = System.Convert.ToString(sqlRd["RDGatewayHostname"]);
                    connectionRecord.RDGatewayUseConnectionCredentials = (mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials), System.Convert.ToString(sqlRd["RDGatewayUseConnectionCredentials"]));
					connectionRecord.RDGatewayUsername = System.Convert.ToString(sqlRd["RDGatewayUsername"]);
					connectionRecord.RDGatewayPassword = Security.Crypt.Decrypt(System.Convert.ToString(sqlRd["RDGatewayPassword"]), pW);
					connectionRecord.RDGatewayDomain = System.Convert.ToString(sqlRd["RDGatewayDomain"]);
					connectionRecord.Inherit.RDGatewayUsageMethod = System.Convert.ToBoolean(sqlRd["InheritRDGatewayUsageMethod"]);
					connectionRecord.Inherit.RDGatewayHostname = System.Convert.ToBoolean(sqlRd["InheritRDGatewayHostname"]);
					connectionRecord.Inherit.RDGatewayUsername = System.Convert.ToBoolean(sqlRd["InheritRDGatewayUsername"]);
					connectionRecord.Inherit.RDGatewayPassword = System.Convert.ToBoolean(sqlRd["InheritRDGatewayPassword"]);
					connectionRecord.Inherit.RDGatewayDomain = System.Convert.ToBoolean(sqlRd["InheritRDGatewayDomain"]);
				}
				
				if (this.confVersion >= 2.3)
				{
					connectionRecord.EnableFontSmoothing = System.Convert.ToBoolean(sqlRd["EnableFontSmoothing"]);
					connectionRecord.EnableDesktopComposition = System.Convert.ToBoolean(sqlRd["EnableDesktopComposition"]);
					connectionRecord.Inherit.EnableFontSmoothing = System.Convert.ToBoolean(sqlRd["InheritEnableFontSmoothing"]);
					connectionRecord.Inherit.EnableDesktopComposition = System.Convert.ToBoolean(sqlRd["InheritEnableDesktopComposition"]);
				}
				
				if (confVersion >= 2.4)
				{
					connectionRecord.UseCredSsp = System.Convert.ToBoolean(sqlRd["UseCredSsp"]);
					connectionRecord.Inherit.UseCredSsp = System.Convert.ToBoolean(sqlRd["InheritUseCredSsp"]);
				}
				
				if (confVersion >= 2.5)
				{
					connectionRecord.LoadBalanceInfo = System.Convert.ToString(sqlRd["LoadBalanceInfo"]);
					connectionRecord.AutomaticResize = System.Convert.ToBoolean(sqlRd["AutomaticResize"]);
					connectionRecord.Inherit.LoadBalanceInfo = System.Convert.ToBoolean(sqlRd["InheritLoadBalanceInfo"]);
					connectionRecord.Inherit.AutomaticResize = System.Convert.ToBoolean(sqlRd["InheritAutomaticResize"]);
				}
				
				if (SQLUpdate == true)
				{
					connectionRecord.PleaseConnect = System.Convert.ToBoolean(sqlRd["Connected"]);
				}
				
				return connectionRecord;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strGetConnectionInfoFromSqlFailed + Constants.vbNewLine + ex.Message, true);
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
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.WarningMsg, My.Language.strOldConffile);
				}
						
				const double maxSupportedConfVersion = 2.5;
				if (confVersion > maxSupportedConfVersion)
				{
                    cTaskDialog.ShowTaskDialogBox(
                        frmMain.Default,
                        System.Windows.Forms.Application.ProductName, 
                        "Incompatible connection file format", 
                        string.Format("The format of this connection file is not supported. Please upgrade to a newer version of {0}.", System.Windows.Forms.Application.ProductName), 
                        string.Format("{1}{0}File Format Version: {2}{0}Highest Supported Version: {3}", Constants.vbNewLine, ConnectionFileName, confVersion.ToString(), maxSupportedConfVersion.ToString()),
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
					if (System.Convert.ToBoolean(xDom.DocumentElement.Attributes["Export"].Value) == true)
					{
						isExportFile = true;
					}
				}
						
				if (import && !isExportFile)
				{
					Runtime.MessageCollector.AddMessage(Messages.MessageClass.InformationMsg, My.Language.strCannotImportNormalSessionFile);
					return ;
				}
						
				if (!isExportFile)
				{
                    RootTreeNode.ImageIndex = (int)Images.Enums.TreeImage.Root;
                    RootTreeNode.SelectedImageIndex = (int)Images.Enums.TreeImage.Root;
				}

                Runtime.Windows.treeForm.tvConnections.BeginUpdate();
						
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

                Runtime.Windows.treeForm.tvConnections.EndUpdate();
						
				//open connections from last mremote session
				if (My.Settings.Default.OpenConsFromLastSession == true && My.Settings.Default.NoReconnect == false)
				{
					foreach (Connection.ConnectionRecordImp conI in ConnectionList)
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
                Runtime.Windows.treeForm.InitialRefresh();
				SetSelectedNode(RootTreeNode);
			}
			catch (Exception ex)
			{
				App.Runtime.IsConnectionsFileLoaded = false;
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strLoadFromXmlFailed + Constants.vbNewLine + ex.Message + Constants.vbNewLine + ex.StackTrace, true);
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
							Connection.ConnectionRecordImp connectionInfo = GetConnectionInfoFromXml(xmlNode);
							connectionInfo.TreeNode = treeNode;
							connectionInfo.Parent = _previousContainer; //NEW
									
							ConnectionList.Add(connectionInfo);
									
							treeNode.Tag = connectionInfo;
                            treeNode.ImageIndex = (int)Images.Enums.TreeImage.ConnectionClosed;
                            treeNode.SelectedImageIndex = (int)Images.Enums.TreeImage.ConnectionClosed;
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
									
							Connection.ConnectionRecordImp connectionInfo = default(Connection.ConnectionRecordImp);
							if (confVersion >= 0.9)
							{
								connectionInfo = GetConnectionInfoFromXml(xmlNode);
							}
							else
							{
								connectionInfo = new Connection.ConnectionRecordImp();
							}
									
							connectionInfo.Parent = containerInfo;
							connectionInfo.IsContainer = true;
							containerInfo.ConnectionRecord = connectionInfo;
									
							ContainerList.Add(containerInfo);
									
							treeNode.Tag = containerInfo;
                            treeNode.ImageIndex = (int)Images.Enums.TreeImage.Container;
                            treeNode.SelectedImageIndex = (int)Images.Enums.TreeImage.Container;
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
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, My.Language.strAddNodeFromXmlFailed + Constants.vbNewLine + ex.Message + ex.StackTrace, true);
				throw;
			}
		}
				
		private Connection.ConnectionRecordImp GetConnectionInfoFromXml(XmlNode xxNode)
		{
			Connection.ConnectionRecordImp conI = new Connection.ConnectionRecordImp();
			try
			{
				XmlNode xmlnode = xxNode;
				if (this.confVersion > 0.1) //0.2
				{
					conI.Name = xmlnode.Attributes["Name"].Value;
					conI.Description = xmlnode.Attributes["Descr"].Value;
					conI.Hostname = xmlnode.Attributes["Hostname"].Value;
					conI.Username = xmlnode.Attributes["Username"].Value;
					conI.Password = Security.Crypt.Decrypt(xmlnode.Attributes["Password"].Value, pW);
					conI.Domain = xmlnode.Attributes["Domain"].Value;
					conI.DisplayWallpaper = bool.Parse(xmlnode.Attributes["DisplayWallpaper"].Value);
					conI.DisplayThemes = bool.Parse(xmlnode.Attributes["DisplayThemes"].Value);
					conI.CacheBitmaps = bool.Parse(xmlnode.Attributes["CacheBitmaps"].Value);
							
					if (this.confVersion < 1.1) //1.0 - 0.1
					{
						if (System.Convert.ToBoolean(xmlnode.Attributes["Fullscreen"].Value) == true)
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
						if (System.Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value) == true)
						{
							conI.Protocol = Connection.Protocol.Protocols.VNC;
							conI.Port = System.Convert.ToInt32(xmlnode.Attributes["VNCPort"].Value);
						}
						else
						{
							conI.Protocol = Connection.Protocol.Protocols.RDP;
						}
					}
				}
				else
				{
					conI.Port = (int)Connection.Protocol.RDP.Defaults.Port;
					conI.Protocol = Connection.Protocol.Protocols.RDP;
				}
						
				if (this.confVersion > 0.3) //0.4
				{
					if (this.confVersion < 0.7)
					{
						if (System.Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value) == true)
						{
                            conI.Port = System.Convert.ToInt32(xmlnode.Attributes["VNCPort"].Value);
						}
						else
						{
                            conI.Port = System.Convert.ToInt32(xmlnode.Attributes["RDPPort"].Value);
						}
					}
							
					conI.UseConsoleSession = bool.Parse(xmlnode.Attributes["ConnectToConsole"].Value);
				}
				else
				{
					if (this.confVersion < 0.7)
					{
						if (System.Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value) == true)
						{
							conI.Port = (int)Connection.Protocol.VNC.Defaults.Port;
						}
						else
						{
							conI.Port = (int)Connection.Protocol.RDP.Defaults.Port;
						}
					}
					conI.UseConsoleSession = false;
				}
						
				if (this.confVersion > 0.4) //0.5 and 0.6
				{
					conI.RedirectDiskDrives = bool.Parse(xmlnode.Attributes["RedirectDiskDrives"].Value);
					conI.RedirectPrinters = bool.Parse(xmlnode.Attributes["RedirectPrinters"].Value);
					conI.RedirectPorts = bool.Parse(xmlnode.Attributes["RedirectPorts"].Value);
					conI.RedirectSmartCards = bool.Parse(xmlnode.Attributes["RedirectSmartCards"].Value);
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
                    conI.Protocol = (Connection.Protocol.Protocols)Tools.Misc.StringToEnum(typeof(Connection.Protocol.Protocols), xmlnode.Attributes["Protocol"].Value);
                    conI.Port = System.Convert.ToInt32(xmlnode.Attributes["Port"].Value);
				}
						
				if (this.confVersion > 0.9) //1.0
				{
					conI.RedirectKeys = bool.Parse(xmlnode.Attributes["RedirectKeys"].Value);
				}
						
				if (this.confVersion > 1.1) //1.2
				{
					conI.PuttySession = xmlnode.Attributes["PuttySession"].Value;
				}
						
				if (this.confVersion > 1.2) //1.3
				{
                    conI.Colors = (Connection.Protocol.RDP.RDPColors)Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPColors), xmlnode.Attributes["Colors"].Value);
                    conI.Resolution = (Connection.Protocol.RDP.RDPResolutions)Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPResolutions), System.Convert.ToString(xmlnode.Attributes["Resolution"].Value));
                    conI.RedirectSound = (Connection.Protocol.RDP.RDPSounds)Tools.Misc.StringToEnum(typeof(Connection.Protocol.RDP.RDPSounds), System.Convert.ToString(xmlnode.Attributes["RedirectSound"].Value));
				}
				else
				{
					switch (System.Convert.ToInt32(xmlnode.Attributes["Colors"].Value))
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
							
					conI.RedirectSound = (Connection.Protocol.RDP.RDPSounds) System.Convert.ToInt32(xmlnode.Attributes["RedirectSound"].Value);
				}
						
				if (this.confVersion > 1.2) //1.3
				{
					conI.Inherit = new Connection.ConnectionRecordImp.ConnectionRecordInheritanceImp(conI);
					conI.Inherit.CacheBitmaps = bool.Parse(xmlnode.Attributes["InheritCacheBitmaps"].Value);
					conI.Inherit.Colors = bool.Parse(xmlnode.Attributes["InheritColors"].Value);
					conI.Inherit.Description = bool.Parse(xmlnode.Attributes["InheritDescription"].Value);
					conI.Inherit.DisplayThemes = bool.Parse(xmlnode.Attributes["InheritDisplayThemes"].Value);
					conI.Inherit.DisplayWallpaper = bool.Parse(xmlnode.Attributes["InheritDisplayWallpaper"].Value);
					conI.Inherit.Domain = bool.Parse(xmlnode.Attributes["InheritDomain"].Value);
					conI.Inherit.Icon = bool.Parse(xmlnode.Attributes["InheritIcon"].Value);
					conI.Inherit.Panel = bool.Parse(xmlnode.Attributes["InheritPanel"].Value);
					conI.Inherit.Password = bool.Parse(xmlnode.Attributes["InheritPassword"].Value);
					conI.Inherit.Port = bool.Parse(xmlnode.Attributes["InheritPort"].Value);
					conI.Inherit.Protocol = bool.Parse(xmlnode.Attributes["InheritProtocol"].Value);
					conI.Inherit.PuttySession = bool.Parse(xmlnode.Attributes["InheritPuttySession"].Value);
					conI.Inherit.RedirectDiskDrives = bool.Parse(xmlnode.Attributes["InheritRedirectDiskDrives"].Value);
					conI.Inherit.RedirectKeys = bool.Parse(xmlnode.Attributes["InheritRedirectKeys"].Value);
					conI.Inherit.RedirectPorts = bool.Parse(xmlnode.Attributes["InheritRedirectPorts"].Value);
					conI.Inherit.RedirectPrinters = bool.Parse(xmlnode.Attributes["InheritRedirectPrinters"].Value);
					conI.Inherit.RedirectSmartCards = bool.Parse(xmlnode.Attributes["InheritRedirectSmartCards"].Value);
					conI.Inherit.RedirectSound = bool.Parse(xmlnode.Attributes["InheritRedirectSound"].Value);
					conI.Inherit.Resolution = bool.Parse(xmlnode.Attributes["InheritResolution"].Value);
					conI.Inherit.UseConsoleSession = bool.Parse(xmlnode.Attributes["InheritUseConsoleSession"].Value);
					conI.Inherit.Username = bool.Parse(xmlnode.Attributes["InheritUsername"].Value);
							
					conI.Icon = xmlnode.Attributes["Icon"].Value;
					conI.Panel = xmlnode.Attributes["Panel"].Value;
				}
				else
				{
                    conI.Inherit = new Connection.ConnectionRecordImp.ConnectionRecordInheritanceImp(conI, System.Convert.ToBoolean(xmlnode.Attributes["Inherit"].Value));
							
					conI.Icon = System.Convert.ToString(xmlnode.Attributes["Icon"].Value.Replace(".ico", ""));
					conI.Panel = My.Language.strGeneral;
				}
						
				if (this.confVersion > 1.4) //1.5
				{
					conI.PleaseConnect = bool.Parse(xmlnode.Attributes["Connected"].Value);
				}
						
				if (this.confVersion > 1.5) //1.6
				{
                    conI.ICAEncryption = (mRemoteNG.Connection.Protocol.ICA.EncryptionStrength)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.ICA.EncryptionStrength), xmlnode.Attributes["ICAEncryptionStrength"].Value);
					conI.Inherit.ICAEncryption = bool.Parse(xmlnode.Attributes["InheritICAEncryptionStrength"].Value);
							
					conI.PreExtApp = xmlnode.Attributes["PreExtApp"].Value;
					conI.PostExtApp = xmlnode.Attributes["PostExtApp"].Value;
					conI.Inherit.PreExtApp = bool.Parse(xmlnode.Attributes["InheritPreExtApp"].Value);
					conI.Inherit.PostExtApp = bool.Parse(xmlnode.Attributes["InheritPostExtApp"].Value);
				}
						
				if (this.confVersion > 1.6) //1.7
				{
                    conI.VNCCompression = (mRemoteNG.Connection.Protocol.VNC.Compression)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.Compression), xmlnode.Attributes["VNCCompression"].Value);
                    conI.VNCEncoding = (mRemoteNG.Connection.Protocol.VNC.Encoding)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.Encoding), System.Convert.ToString(xmlnode.Attributes["VNCEncoding"].Value));
                    conI.VNCAuthMode = (mRemoteNG.Connection.Protocol.VNC.AuthMode)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.AuthMode), xmlnode.Attributes["VNCAuthMode"].Value);
                    conI.VNCProxyType = (mRemoteNG.Connection.Protocol.VNC.ProxyType)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.ProxyType), xmlnode.Attributes["VNCProxyType"].Value);
					conI.VNCProxyIP = xmlnode.Attributes["VNCProxyIP"].Value;
                    conI.VNCProxyPort = System.Convert.ToInt32(xmlnode.Attributes["VNCProxyPort"].Value);
					conI.VNCProxyUsername = xmlnode.Attributes["VNCProxyUsername"].Value;
					conI.VNCProxyPassword = Security.Crypt.Decrypt(xmlnode.Attributes["VNCProxyPassword"].Value, pW);
                    conI.VNCColors = (mRemoteNG.Connection.Protocol.VNC.Colors)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.Colors), xmlnode.Attributes["VNCColors"].Value);
                    conI.VNCSmartSizeMode = (mRemoteNG.Connection.Protocol.VNC.SmartSizeMode)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.VNC.SmartSizeMode), xmlnode.Attributes["VNCSmartSizeMode"].Value);
					conI.VNCViewOnly = bool.Parse(xmlnode.Attributes["VNCViewOnly"].Value);
							
					conI.Inherit.VNCCompression = bool.Parse(xmlnode.Attributes["InheritVNCCompression"].Value);
					conI.Inherit.VNCEncoding = bool.Parse(xmlnode.Attributes["InheritVNCEncoding"].Value);
					conI.Inherit.VNCAuthMode = bool.Parse(xmlnode.Attributes["InheritVNCAuthMode"].Value);
					conI.Inherit.VNCProxyType = bool.Parse(xmlnode.Attributes["InheritVNCProxyType"].Value);
					conI.Inherit.VNCProxyIP = bool.Parse(xmlnode.Attributes["InheritVNCProxyIP"].Value);
					conI.Inherit.VNCProxyPort = bool.Parse(xmlnode.Attributes["InheritVNCProxyPort"].Value);
					conI.Inherit.VNCProxyUsername = bool.Parse(xmlnode.Attributes["InheritVNCProxyUsername"].Value);
					conI.Inherit.VNCProxyPassword = bool.Parse(xmlnode.Attributes["InheritVNCProxyPassword"].Value);
					conI.Inherit.VNCColors = bool.Parse(xmlnode.Attributes["InheritVNCColors"].Value);
					conI.Inherit.VNCSmartSizeMode = bool.Parse(xmlnode.Attributes["InheritVNCSmartSizeMode"].Value);
					conI.Inherit.VNCViewOnly = bool.Parse(xmlnode.Attributes["InheritVNCViewOnly"].Value);
				}
						
				if (this.confVersion > 1.7) //1.8
				{
                    conI.RDPAuthenticationLevel = (mRemoteNG.Connection.Protocol.RDP.AuthenticationLevel)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.AuthenticationLevel), xmlnode.Attributes["RDPAuthenticationLevel"].Value);
							
					conI.Inherit.RDPAuthenticationLevel = bool.Parse(xmlnode.Attributes["InheritRDPAuthenticationLevel"].Value);
				}
						
				if (this.confVersion > 1.8) //1.9
				{
                    conI.RenderingEngine = (mRemoteNG.Connection.Protocol.HTTPBase.RenderingEngine)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.HTTPBase.RenderingEngine), xmlnode.Attributes["RenderingEngine"].Value);
					conI.MacAddress = xmlnode.Attributes["MacAddress"].Value;
							
					conI.Inherit.RenderingEngine = bool.Parse(xmlnode.Attributes["InheritRenderingEngine"].Value);
					conI.Inherit.MacAddress = bool.Parse(xmlnode.Attributes["InheritMacAddress"].Value);
				}
						
				if (this.confVersion > 1.9) //2.0
				{
					conI.UserField = xmlnode.Attributes["UserField"].Value;
					conI.Inherit.UserField = bool.Parse(xmlnode.Attributes["InheritUserField"].Value);
				}
						
				if (this.confVersion > 2.0) //2.1
				{
					conI.ExtApp = xmlnode.Attributes["ExtApp"].Value;
					conI.Inherit.ExtApp = bool.Parse(xmlnode.Attributes["InheritExtApp"].Value);
				}
						
				if (this.confVersion > 2.1) //2.2
				{
					// Get settings
                    conI.RDGatewayUsageMethod = (mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUsageMethod), System.Convert.ToString(xmlnode.Attributes["RDGatewayUsageMethod"].Value));
					conI.RDGatewayHostname = xmlnode.Attributes["RDGatewayHostname"].Value;
                    conI.RDGatewayUseConnectionCredentials = (mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials)Tools.Misc.StringToEnum(typeof(mRemoteNG.Connection.Protocol.RDP.RDGatewayUseConnectionCredentials), System.Convert.ToString(xmlnode.Attributes["RDGatewayUseConnectionCredentials"].Value));
					conI.RDGatewayUsername = xmlnode.Attributes["RDGatewayUsername"].Value;
					conI.RDGatewayPassword = Security.Crypt.Decrypt(System.Convert.ToString(xmlnode.Attributes["RDGatewayPassword"].Value), pW);
					conI.RDGatewayDomain = xmlnode.Attributes["RDGatewayDomain"].Value;
							
					// Get inheritance settings
					conI.Inherit.RDGatewayUsageMethod = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsageMethod"].Value);
					conI.Inherit.RDGatewayHostname = bool.Parse(xmlnode.Attributes["InheritRDGatewayHostname"].Value);
					conI.Inherit.RDGatewayUseConnectionCredentials = bool.Parse(xmlnode.Attributes["InheritRDGatewayUseConnectionCredentials"].Value);
					conI.Inherit.RDGatewayUsername = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsername"].Value);
					conI.Inherit.RDGatewayPassword = bool.Parse(xmlnode.Attributes["InheritRDGatewayPassword"].Value);
					conI.Inherit.RDGatewayDomain = bool.Parse(xmlnode.Attributes["InheritRDGatewayDomain"].Value);
				}
						
				if (this.confVersion > 2.2) //2.3
				{
					// Get settings
					conI.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["EnableFontSmoothing"].Value);
					conI.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["EnableDesktopComposition"].Value);
							
					// Get inheritance settings
					conI.Inherit.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["InheritEnableFontSmoothing"].Value);
					conI.Inherit.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["InheritEnableDesktopComposition"].Value);
				}
						
				if (confVersion >= 2.4)
				{
					conI.UseCredSsp = bool.Parse(xmlnode.Attributes["UseCredSsp"].Value);
					conI.Inherit.UseCredSsp = bool.Parse(xmlnode.Attributes["InheritUseCredSsp"].Value);
				}
						
				if (confVersion >= 2.5)
				{
					conI.LoadBalanceInfo = xmlnode.Attributes["LoadBalanceInfo"].Value;
					conI.AutomaticResize = bool.Parse(xmlnode.Attributes["AutomaticResize"].Value);
					conI.Inherit.LoadBalanceInfo = bool.Parse(xmlnode.Attributes["InheritLoadBalanceInfo"].Value);
					conI.Inherit.AutomaticResize = bool.Parse(xmlnode.Attributes["InheritAutomaticResize"].Value);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, string.Format(My.Language.strGetConnectionInfoFromXmlFailed, conI.Name, this.ConnectionFileName, ex.Message), false);
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
