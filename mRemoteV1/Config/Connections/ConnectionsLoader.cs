using mRemoteNG.App;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Connection.Protocol.RDP;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using mRemoteNG.App.Info;
using mRemoteNG.Tree;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.UI.Forms;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.Config.Connections
{
	public class ConnectionsLoader
	{
		private XmlDocument _xmlDocument;
		private double _confVersion;
		private SecureString _pW = GeneralAppInfo.EncryptionKey;
		private SqlConnection _sqlConnection;
		private SqlCommand _sqlQuery;
		private SqlDataReader _sqlDataReader;
		private TreeNode _selectedTreeNode;

				
        #region Public Properties
        public bool UseSql { get; set; }

	    public string SqlHost { get; set; }

	    public string SqlDatabaseName { get; set; }

	    public string SqlUsername { get; set; }

	    public string SqlPassword { get; set; }

	    public bool SqlUpdate { get; set; }

	    public string PreviousSelected { get; set; }

	    public string ConnectionFileName { get; set; }

	    public TreeNode RootTreeNode { get; set; }
		
		public ConnectionList ConnectionList { get; set; }
		
        public ContainerList ContainerList { get; set; }

	    public ConnectionList PreviousConnectionList { get; set; }

	    public ContainerList PreviousContainerList { get; set; }

	    #endregion
				
        #region Public Methods
		public void LoadConnections(bool import)
		{
			if (UseSql)
			{
				LoadFromSql();
			}
			else
			{
				var connections = DecryptCompleteFile();
				LoadFromXml(connections, import);
			}
					
			frmMain.Default.AreWeUsingSqlServerForSavingConnections = UseSql;
			frmMain.Default.ConnectionsFileName = ConnectionFileName;
					
			if (!import)
			{
				Putty.Sessions.AddSessionsToTree();
			}
		}
        #endregion
				
        #region SQL
		private delegate void LoadFromSqlDelegate();
		private void LoadFromSql()
		{
            if (Windows.treeForm == null || Windows.treeForm.tvConnections == null)
				return ;
            if (Windows.treeForm.tvConnections.InvokeRequired)
			{
                Windows.treeForm.tvConnections.Invoke(new LoadFromSqlDelegate(LoadFromSql));
				return ;
			}
					
			try
			{
                Runtime.IsConnectionsFileLoaded = false;
				_sqlConnection = !string.IsNullOrEmpty(SqlUsername) ? new SqlConnection("Data Source=" + SqlHost + ";Initial Catalog=" + SqlDatabaseName + ";User Id=" + SqlUsername + ";Password=" + SqlPassword) : new SqlConnection("Data Source=" + SqlHost + ";Initial Catalog=" + SqlDatabaseName + ";Integrated Security=True");
						
				_sqlConnection.Open();
				_sqlQuery = new SqlCommand("SELECT * FROM tblRoot", _sqlConnection);
				_sqlDataReader = _sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
				_sqlDataReader.Read();
						
				if (_sqlDataReader.HasRows == false)
				{
                    Runtime.SaveConnections();
					_sqlQuery = new SqlCommand("SELECT * FROM tblRoot", _sqlConnection);
					_sqlDataReader = _sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
					_sqlDataReader.Read();
				}
						
				_confVersion = Convert.ToDouble(_sqlDataReader["confVersion"], CultureInfo.InvariantCulture);
				const double maxSupportedSchemaVersion = 2.5;
				if (_confVersion > maxSupportedSchemaVersion)
				{
                    CTaskDialog.ShowTaskDialogBox(
                        frmMain.Default, 
                        Application.ProductName, 
                        "Incompatible database schema",
                        $"The database schema on the server is not supported. Please upgrade to a newer version of {Application.ProductName}.", 
                        string.Format("Schema Version: {1}{0}Highest Supported Version: {2}", Environment.NewLine, _confVersion.ToString(), maxSupportedSchemaVersion.ToString()), 
                        "", 
                        "", 
                        "", 
                        "", 
                        ETaskDialogButtons.Ok, 
                        ESysIcons.Error, 
                        ESysIcons.Error
                    );
					throw (new Exception($"Incompatible database schema (schema version {_confVersion})."));
				}
						
				RootTreeNode.Name = Convert.ToString(_sqlDataReader["Name"]);

			    var rootInfo = new RootNodeInfo(RootNodeType.Connection)
			    {
			        Name = RootTreeNode.Name,
			        TreeNode = RootTreeNode
			    };

			    RootTreeNode.Tag = rootInfo;
				RootTreeNode.ImageIndex = (int)TreeImageType.Root;
                RootTreeNode.SelectedImageIndex = (int)TreeImageType.Root;

                var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                if (cryptographyProvider.Decrypt(Convert.ToString(_sqlDataReader["Protected"]), _pW) != "ThisIsNotProtected")
				{
					if (Authenticate(Convert.ToString(_sqlDataReader["Protected"]), false, rootInfo) == false)
					{
						mRemoteNG.Settings.Default.LoadConsFromCustomLocation = false;
                        mRemoteNG.Settings.Default.CustomConsPath = "";
						RootTreeNode.Remove();
						return;
					}
				}
						
				_sqlDataReader.Close();
                Windows.treeForm.tvConnections.BeginUpdate();
						
				// SECTION 3. Populate the TreeView with the DOM nodes.
				AddNodesFromSql(RootTreeNode);
				RootTreeNode.Expand();
						
				//expand containers
				foreach (ContainerInfo contI in ContainerList)
				{
					if (contI.IsExpanded)
						contI.TreeNode.Expand();
				}

                Windows.treeForm.tvConnections.EndUpdate();
						
				//open connections from last mremote session
				if (mRemoteNG.Settings.Default.OpenConsFromLastSession && !mRemoteNG.Settings.Default.NoReconnect)
				{
					foreach (ConnectionInfo conI in ConnectionList)
					{
						if (conI.PleaseConnect)
                            Runtime.OpenConnection(conI);
					}
				}

                Runtime.IsConnectionsFileLoaded = true;
                Windows.treeForm.InitialRefresh();
				SetSelectedNode(_selectedTreeNode);
			}
			finally
			{
			    _sqlConnection?.Close();
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
				
		private void AddNodesFromSql(TreeNode baseNode)
		{
			try
			{
				_sqlConnection.Open();
				_sqlQuery = new SqlCommand("SELECT * FROM tblCons ORDER BY PositionID ASC", _sqlConnection);
				_sqlDataReader = _sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);
						
				if (_sqlDataReader.HasRows == false)
					return;
						
				var tNode = default(TreeNode);
						
				while (_sqlDataReader.Read())
				{
					tNode = new TreeNode(Convert.ToString(_sqlDataReader["Name"]));
					//baseNode.Nodes.Add(tNode)
							
					switch (ConnectionTreeNode.GetNodeTypeFromString(Convert.ToString(_sqlDataReader["Type"])))
					{
					    case TreeNodeType.Connection:
					    {
					        var conI = GetConnectionInfoFromSql();
					        conI.TreeNode = tNode;
					        //conI.Parent = _previousContainer 'NEW
								
					        ConnectionList.Add(conI);
								
					        tNode.Tag = conI;
								
					        if (SqlUpdate)
					        {
					            var prevCon = PreviousConnectionList.FindByConstantID(conI.ConstantID);
									
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
									
					            if (conI.ConstantID == PreviousSelected)
					                _selectedTreeNode = tNode;
					        }
					        else
					        {
					            tNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
					            tNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
					        }
					    }
					        break;
					    case TreeNodeType.Container:
					    {
					        var contI = new ContainerInfo
					        {
					            TreeNode = tNode,
					            Name = Convert.ToString(_sqlDataReader["Name"])
					        };

					        var conI = default(ConnectionInfo);
					        conI = GetConnectionInfoFromSql();
					        conI.Parent = contI;
					        conI.IsContainer = true;
					        contI.ConnectionInfo = conI;
								
					        if (SqlUpdate)
					        {
					            var prevCont = PreviousContainerList.FindByConstantID(conI.ConstantID);
					            if (prevCont != null)
					                contI.IsExpanded = prevCont.IsExpanded;
									
					            if (conI.ConstantID == PreviousSelected)
					                _selectedTreeNode = tNode;
					        }
					        else
					        {
					            contI.IsExpanded = Convert.ToBoolean(_sqlDataReader["Expanded"]);
					        }
								
					        ContainerList.Add(contI);
					        ConnectionList.Add(conI);
					        tNode.Tag = contI;
					        tNode.ImageIndex = (int)TreeImageType.Container;
					        tNode.SelectedImageIndex = (int)TreeImageType.Container;
					    }
					        break;
					}

                    var parentId = Convert.ToString(_sqlDataReader["ParentID"].ToString().Trim());
					if (string.IsNullOrEmpty(parentId) || parentId == "0")
					{
						baseNode.Nodes.Add(tNode);
					}
					else
					{
                        var pNode = ConnectionTreeNode.GetNodeFromConstantID(Convert.ToString(_sqlDataReader["ParentID"]));
								
						if (pNode != null)
						{
						    pNode.Nodes.Add(tNode);

						    switch (ConnectionTreeNode.GetNodeType(tNode))
						    {
						        case TreeNodeType.Connection:
						            (tNode.Tag as ConnectionInfo).Parent = (ContainerInfo)pNode.Tag;
						            break;
						        case TreeNodeType.Container:
						            (tNode.Tag as ContainerInfo).Parent = (ContainerInfo)pNode.Tag;
						            break;
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strAddNodesFromSqlFailed + Environment.NewLine + ex.Message, true);
			}
		}
				
		private ConnectionInfo GetConnectionInfoFromSql()
		{
			try
			{
			    var connectionInfo = new ConnectionInfo
			    {
			        PositionID = Convert.ToInt32(_sqlDataReader["PositionID"]),
			        ConstantID = Convert.ToString(_sqlDataReader["ConstantID"]),
			        Name = Convert.ToString(_sqlDataReader["Name"]),
			        Description = Convert.ToString(_sqlDataReader["Description"]),
			        Hostname = Convert.ToString(_sqlDataReader["Hostname"]),
			        Username = Convert.ToString(_sqlDataReader["Username"])
			    };

			    var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                connectionInfo.Password = cryptographyProvider.Decrypt(Convert.ToString(_sqlDataReader["Password"]), _pW);
				connectionInfo.Domain = Convert.ToString(_sqlDataReader["DomainName"]);
				connectionInfo.DisplayWallpaper = Convert.ToBoolean(_sqlDataReader["DisplayWallpaper"]);
				connectionInfo.DisplayThemes = Convert.ToBoolean(_sqlDataReader["DisplayThemes"]);
				connectionInfo.CacheBitmaps = Convert.ToBoolean(_sqlDataReader["CacheBitmaps"]);
				connectionInfo.UseConsoleSession = Convert.ToBoolean(_sqlDataReader["ConnectToConsole"]);
				connectionInfo.RedirectDiskDrives = Convert.ToBoolean(_sqlDataReader["RedirectDiskDrives"]);
				connectionInfo.RedirectPrinters = Convert.ToBoolean(_sqlDataReader["RedirectPrinters"]);
				connectionInfo.RedirectPorts = Convert.ToBoolean(_sqlDataReader["RedirectPorts"]);
				connectionInfo.RedirectSmartCards = Convert.ToBoolean(_sqlDataReader["RedirectSmartCards"]);
				connectionInfo.RedirectKeys = Convert.ToBoolean(_sqlDataReader["RedirectKeys"]);
                connectionInfo.RedirectSound = (ProtocolRDP.RDPSounds)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPSounds), Convert.ToString(_sqlDataReader["RedirectSound"]));
                connectionInfo.Protocol = (ProtocolType)Tools.MiscTools.StringToEnum(typeof(ProtocolType), Convert.ToString(_sqlDataReader["Protocol"]));
				connectionInfo.Port = Convert.ToInt32(_sqlDataReader["Port"]);
				connectionInfo.PuttySession = Convert.ToString(_sqlDataReader["PuttySession"]);
                connectionInfo.Colors = (ProtocolRDP.RDPColors)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPColors), Convert.ToString(_sqlDataReader["Colors"]));
                connectionInfo.Resolution = (ProtocolRDP.RDPResolutions)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDPResolutions), Convert.ToString(_sqlDataReader["Resolution"]));
				connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo);
				connectionInfo.Inheritance.CacheBitmaps = Convert.ToBoolean(_sqlDataReader["InheritCacheBitmaps"]);
				connectionInfo.Inheritance.Colors = Convert.ToBoolean(_sqlDataReader["InheritColors"]);
				connectionInfo.Inheritance.Description = Convert.ToBoolean(_sqlDataReader["InheritDescription"]);
				connectionInfo.Inheritance.DisplayThemes = Convert.ToBoolean(_sqlDataReader["InheritDisplayThemes"]);
				connectionInfo.Inheritance.DisplayWallpaper = Convert.ToBoolean(_sqlDataReader["InheritDisplayWallpaper"]);
				connectionInfo.Inheritance.Domain = Convert.ToBoolean(_sqlDataReader["InheritDomain"]);
				connectionInfo.Inheritance.Icon = Convert.ToBoolean(_sqlDataReader["InheritIcon"]);
				connectionInfo.Inheritance.Panel = Convert.ToBoolean(_sqlDataReader["InheritPanel"]);
				connectionInfo.Inheritance.Password = Convert.ToBoolean(_sqlDataReader["InheritPassword"]);
				connectionInfo.Inheritance.Port = Convert.ToBoolean(_sqlDataReader["InheritPort"]);
				connectionInfo.Inheritance.Protocol = Convert.ToBoolean(_sqlDataReader["InheritProtocol"]);
				connectionInfo.Inheritance.PuttySession = Convert.ToBoolean(_sqlDataReader["InheritPuttySession"]);
				connectionInfo.Inheritance.RedirectDiskDrives = Convert.ToBoolean(_sqlDataReader["InheritRedirectDiskDrives"]);
				connectionInfo.Inheritance.RedirectKeys = Convert.ToBoolean(_sqlDataReader["InheritRedirectKeys"]);
				connectionInfo.Inheritance.RedirectPorts = Convert.ToBoolean(_sqlDataReader["InheritRedirectPorts"]);
				connectionInfo.Inheritance.RedirectPrinters = Convert.ToBoolean(_sqlDataReader["InheritRedirectPrinters"]);
				connectionInfo.Inheritance.RedirectSmartCards = Convert.ToBoolean(_sqlDataReader["InheritRedirectSmartCards"]);
				connectionInfo.Inheritance.RedirectSound = Convert.ToBoolean(_sqlDataReader["InheritRedirectSound"]);
				connectionInfo.Inheritance.Resolution = Convert.ToBoolean(_sqlDataReader["InheritResolution"]);
				connectionInfo.Inheritance.UseConsoleSession = Convert.ToBoolean(_sqlDataReader["InheritUseConsoleSession"]);
				connectionInfo.Inheritance.Username = Convert.ToBoolean(_sqlDataReader["InheritUsername"]);
				connectionInfo.Icon = Convert.ToString(_sqlDataReader["Icon"]);
				connectionInfo.Panel = Convert.ToString(_sqlDataReader["Panel"]);
						
				if (_confVersion > 1.5) //1.6
				{
                    connectionInfo.ICAEncryption = (ProtocolICA.EncryptionStrength)Tools.MiscTools.StringToEnum(typeof(ProtocolICA.EncryptionStrength), Convert.ToString(_sqlDataReader["ICAEncryptionStrength"]));
					connectionInfo.Inheritance.ICAEncryption = Convert.ToBoolean(_sqlDataReader["InheritICAEncryptionStrength"]);
					connectionInfo.PreExtApp = Convert.ToString(_sqlDataReader["PreExtApp"]);
					connectionInfo.PostExtApp = Convert.ToString(_sqlDataReader["PostExtApp"]);
					connectionInfo.Inheritance.PreExtApp = Convert.ToBoolean(_sqlDataReader["InheritPreExtApp"]);
					connectionInfo.Inheritance.PostExtApp = Convert.ToBoolean(_sqlDataReader["InheritPostExtApp"]);
				}
						
				if (_confVersion > 1.6) //1.7
				{
                    connectionInfo.VNCCompression = (ProtocolVNC.Compression)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Compression), Convert.ToString(_sqlDataReader["VNCCompression"]));
                    connectionInfo.VNCEncoding = (ProtocolVNC.Encoding)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Encoding), Convert.ToString(_sqlDataReader["VNCEncoding"]));
                    connectionInfo.VNCAuthMode = (ProtocolVNC.AuthMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.AuthMode), Convert.ToString(_sqlDataReader["VNCAuthMode"]));
                    connectionInfo.VNCProxyType = (ProtocolVNC.ProxyType)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.ProxyType), Convert.ToString(_sqlDataReader["VNCProxyType"]));
					connectionInfo.VNCProxyIP = Convert.ToString(_sqlDataReader["VNCProxyIP"]);
					connectionInfo.VNCProxyPort = Convert.ToInt32(_sqlDataReader["VNCProxyPort"]);
					connectionInfo.VNCProxyUsername = Convert.ToString(_sqlDataReader["VNCProxyUsername"]);
                    connectionInfo.VNCProxyPassword = cryptographyProvider.Decrypt(Convert.ToString(_sqlDataReader["VNCProxyPassword"]), _pW);
                    connectionInfo.VNCColors = (ProtocolVNC.Colors)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Colors), Convert.ToString(_sqlDataReader["VNCColors"]));
                    connectionInfo.VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.SmartSizeMode), Convert.ToString(_sqlDataReader["VNCSmartSizeMode"]));
					connectionInfo.VNCViewOnly = Convert.ToBoolean(_sqlDataReader["VNCViewOnly"]);
					connectionInfo.Inheritance.VNCCompression = Convert.ToBoolean(_sqlDataReader["InheritVNCCompression"]);
					connectionInfo.Inheritance.VNCEncoding = Convert.ToBoolean(_sqlDataReader["InheritVNCEncoding"]);
					connectionInfo.Inheritance.VNCAuthMode = Convert.ToBoolean(_sqlDataReader["InheritVNCAuthMode"]);
					connectionInfo.Inheritance.VNCProxyType = Convert.ToBoolean(_sqlDataReader["InheritVNCProxyType"]);
					connectionInfo.Inheritance.VNCProxyIP = Convert.ToBoolean(_sqlDataReader["InheritVNCProxyIP"]);
					connectionInfo.Inheritance.VNCProxyPort = Convert.ToBoolean(_sqlDataReader["InheritVNCProxyPort"]);
					connectionInfo.Inheritance.VNCProxyUsername = Convert.ToBoolean(_sqlDataReader["InheritVNCProxyUsername"]);
					connectionInfo.Inheritance.VNCProxyPassword = Convert.ToBoolean(_sqlDataReader["InheritVNCProxyPassword"]);
					connectionInfo.Inheritance.VNCColors = Convert.ToBoolean(_sqlDataReader["InheritVNCColors"]);
					connectionInfo.Inheritance.VNCSmartSizeMode = Convert.ToBoolean(_sqlDataReader["InheritVNCSmartSizeMode"]);
					connectionInfo.Inheritance.VNCViewOnly = Convert.ToBoolean(_sqlDataReader["InheritVNCViewOnly"]);
				}
				
				if (_confVersion > 1.7) //1.8
				{
                    connectionInfo.RDPAuthenticationLevel = (ProtocolRDP.AuthenticationLevel)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.AuthenticationLevel), Convert.ToString(_sqlDataReader["RDPAuthenticationLevel"]));
					connectionInfo.Inheritance.RDPAuthenticationLevel = Convert.ToBoolean(_sqlDataReader["InheritRDPAuthenticationLevel"]);
				}
				
				if (_confVersion > 1.8) //1.9
				{
                    connectionInfo.RenderingEngine = (HTTPBase.RenderingEngine)Tools.MiscTools.StringToEnum(typeof(HTTPBase.RenderingEngine), Convert.ToString(_sqlDataReader["RenderingEngine"]));
					connectionInfo.MacAddress = Convert.ToString(_sqlDataReader["MacAddress"]);
					connectionInfo.Inheritance.RenderingEngine = Convert.ToBoolean(_sqlDataReader["InheritRenderingEngine"]);
					connectionInfo.Inheritance.MacAddress = Convert.ToBoolean(_sqlDataReader["InheritMacAddress"]);
				}
				
				if (_confVersion > 1.9) //2.0
				{
					connectionInfo.UserField = Convert.ToString(_sqlDataReader["UserField"]);
					connectionInfo.Inheritance.UserField = Convert.ToBoolean(_sqlDataReader["InheritUserField"]);
				}
				
				if (_confVersion > 2.0) //2.1
				{
					connectionInfo.ExtApp = Convert.ToString(_sqlDataReader["ExtApp"]);
					connectionInfo.Inheritance.ExtApp = Convert.ToBoolean(_sqlDataReader["InheritExtApp"]);
				}
						
				if (_confVersion >= 2.2)
				{
                    connectionInfo.RDGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUsageMethod), Convert.ToString(_sqlDataReader["RDGatewayUsageMethod"]));
					connectionInfo.RDGatewayHostname = Convert.ToString(_sqlDataReader["RDGatewayHostname"]);
                    connectionInfo.RDGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), Convert.ToString(_sqlDataReader["RDGatewayUseConnectionCredentials"]));
					connectionInfo.RDGatewayUsername = Convert.ToString(_sqlDataReader["RDGatewayUsername"]);
					connectionInfo.RDGatewayPassword = cryptographyProvider.Decrypt(Convert.ToString(_sqlDataReader["RDGatewayPassword"]), _pW);
					connectionInfo.RDGatewayDomain = Convert.ToString(_sqlDataReader["RDGatewayDomain"]);
					connectionInfo.Inheritance.RDGatewayUsageMethod = Convert.ToBoolean(_sqlDataReader["InheritRDGatewayUsageMethod"]);
					connectionInfo.Inheritance.RDGatewayHostname = Convert.ToBoolean(_sqlDataReader["InheritRDGatewayHostname"]);
					connectionInfo.Inheritance.RDGatewayUsername = Convert.ToBoolean(_sqlDataReader["InheritRDGatewayUsername"]);
					connectionInfo.Inheritance.RDGatewayPassword = Convert.ToBoolean(_sqlDataReader["InheritRDGatewayPassword"]);
					connectionInfo.Inheritance.RDGatewayDomain = Convert.ToBoolean(_sqlDataReader["InheritRDGatewayDomain"]);
				}
						
				if (_confVersion >= 2.3)
				{
					connectionInfo.EnableFontSmoothing = Convert.ToBoolean(_sqlDataReader["EnableFontSmoothing"]);
					connectionInfo.EnableDesktopComposition = Convert.ToBoolean(_sqlDataReader["EnableDesktopComposition"]);
					connectionInfo.Inheritance.EnableFontSmoothing = Convert.ToBoolean(_sqlDataReader["InheritEnableFontSmoothing"]);
					connectionInfo.Inheritance.EnableDesktopComposition = Convert.ToBoolean(_sqlDataReader["InheritEnableDesktopComposition"]);
				}
						
				if (_confVersion >= 2.4)
				{
					connectionInfo.UseCredSsp = Convert.ToBoolean(_sqlDataReader["UseCredSsp"]);
					connectionInfo.Inheritance.UseCredSsp = Convert.ToBoolean(_sqlDataReader["InheritUseCredSsp"]);
				}
						
				if (_confVersion >= 2.5)
				{
					connectionInfo.LoadBalanceInfo = Convert.ToString(_sqlDataReader["LoadBalanceInfo"]);
					connectionInfo.AutomaticResize = Convert.ToBoolean(_sqlDataReader["AutomaticResize"]);
					connectionInfo.Inheritance.LoadBalanceInfo = Convert.ToBoolean(_sqlDataReader["InheritLoadBalanceInfo"]);
					connectionInfo.Inheritance.AutomaticResize = Convert.ToBoolean(_sqlDataReader["InheritAutomaticResize"]);
				}
						
				if (SqlUpdate)
				{
					connectionInfo.PleaseConnect = Convert.ToBoolean(_sqlDataReader["Connected"]);
				}
						
				return connectionInfo;
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strGetConnectionInfoFromSqlFailed + Environment.NewLine + ex.Message, true);
			}
					
			return null;
		}
        #endregion
				
        #region XML
		private string DecryptCompleteFile()
		{
			var sRd = new StreamReader(ConnectionFileName);
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            var strCons = "";
			strCons = sRd.ReadToEnd();
			sRd.Close();

		    if (string.IsNullOrEmpty(strCons)) return "";
		    var strDecr = "";
		    var notDecr = true;
						
		    if (strCons.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>"))
		    {
		        strDecr = strCons;
		        return strDecr;
		    }
						
		    try
		    {
		        strDecr = cryptographyProvider.Decrypt(strCons, _pW);
		        notDecr = strDecr == strCons;
		    }
		    catch (Exception)
		    {
		        notDecr = true;
		    }
						
		    if (notDecr)
		    {
		        if (Authenticate(strCons, true))
		        {
		            strDecr = cryptographyProvider.Decrypt(strCons, _pW);
		            notDecr = false;
		        }
		        else
		        {
		            notDecr = true;
		        }
							
		        if (notDecr == false)
		            return strDecr;
		    }
		    else
		    {
		        return strDecr;
		    }

		    return "";
		}
				
		private void LoadFromXml(string cons, bool import)
		{
			try
			{
				if (!import)
					Runtime.IsConnectionsFileLoaded = false;

                var cryptographyProvider = new LegacyRijndaelCryptographyProvider();

                // SECTION 1. Create a DOM Document and load the XML data into it.
                _xmlDocument = new XmlDocument();
				if (cons != "")
					_xmlDocument.LoadXml(cons);
				else
					_xmlDocument.Load(ConnectionFileName);
						
				if (_xmlDocument.DocumentElement.HasAttribute("ConfVersion"))
					_confVersion = Convert.ToDouble(_xmlDocument.DocumentElement.Attributes["ConfVersion"].Value.Replace(",", "."), CultureInfo.InvariantCulture);
				else
					Runtime.MessageCollector.AddMessage(MessageClass.WarningMsg, Language.strOldConffile);
						
				const double maxSupportedConfVersion = 2.5;
				if (_confVersion > maxSupportedConfVersion)
				{
                    CTaskDialog.ShowTaskDialogBox(
                        frmMain.Default,
                        Application.ProductName, 
                        "Incompatible connection file format", 
                        string.Format("The format of this connection file is not supported. Please upgrade to a newer version of {0}.", Application.ProductName), 
                        string.Format("{1}{0}File Format Version: {2}{0}Highest Supported Version: {3}", Environment.NewLine, ConnectionFileName, _confVersion.ToString(), maxSupportedConfVersion.ToString()),
                        "", 
                        "", 
                        "", 
                        "", 
                        ETaskDialogButtons.Ok, 
                        ESysIcons.Error,
                        ESysIcons.Error
                    );
					throw (new Exception(string.Format("Incompatible connection file format (file format version {0}).", _confVersion)));
				}

                // SECTION 2. Initialize the treeview control.
                var rootInfo = default(RootNodeInfo);
				if (import)
				{
					rootInfo = null;
				}
				else
				{
                    var rootNodeName = "";
					if (_xmlDocument.DocumentElement.HasAttribute("Name"))
						rootNodeName = Convert.ToString(_xmlDocument.DocumentElement.Attributes["Name"].Value.Trim());
					RootTreeNode.Name = !string.IsNullOrEmpty(rootNodeName) ? rootNodeName : _xmlDocument.DocumentElement.Name;
					RootTreeNode.Text = RootTreeNode.Name;

				    rootInfo = new RootNodeInfo(RootNodeType.Connection)
				    {
				        Name = RootTreeNode.Name,
				        TreeNode = RootTreeNode
				    };
				    RootTreeNode.Tag = rootInfo;
				}
						
				if (_confVersion > 1.3) //1.4
				{
					if (cryptographyProvider.Decrypt(Convert.ToString(_xmlDocument.DocumentElement.Attributes["Protected"].Value), _pW) != "ThisIsNotProtected")
					{
						if (Authenticate(Convert.ToString(_xmlDocument.DocumentElement.Attributes["Protected"].Value), false, rootInfo) == false)
						{
                            mRemoteNG.Settings.Default.LoadConsFromCustomLocation = false;
                            mRemoteNG.Settings.Default.CustomConsPath = "";
							RootTreeNode.Remove();
							return;
						}
					}
				}

                var isExportFile = false;
				if (_confVersion >= 1.0)
				{
					if (Convert.ToBoolean(_xmlDocument.DocumentElement.Attributes["Export"].Value) == true)
					{
						isExportFile = true;
					}
				}
						
				if (import && !isExportFile)
				{
					Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, Language.strCannotImportNormalSessionFile);
					return ;
				}
						
				if (!isExportFile)
				{
                    RootTreeNode.ImageIndex = (int)TreeImageType.Root;
                    RootTreeNode.SelectedImageIndex = (int)TreeImageType.Root;
				}

                Windows.treeForm.tvConnections.BeginUpdate();
						
				// SECTION 3. Populate the TreeView with the DOM nodes.
				AddNodeFromXml(_xmlDocument.DocumentElement, RootTreeNode);
				RootTreeNode.Expand();
						
				//expand containers
				foreach (ContainerInfo contI in ContainerList)
				{
					if (contI.IsExpanded)
						contI.TreeNode.Expand();
				}

                Windows.treeForm.tvConnections.EndUpdate();
						
				//open connections from last mremote session
				if (mRemoteNG.Settings.Default.OpenConsFromLastSession && !mRemoteNG.Settings.Default.NoReconnect)
				{
					foreach (ConnectionInfo conI in ConnectionList)
					{
						if (conI.PleaseConnect)
							Runtime.OpenConnection(conI);
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
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strLoadFromXmlFailed + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, true);
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
                        var treeNode = new TreeNode(xmlNode.Attributes["Name"].Value);
						parentTreeNode.Nodes.Add(treeNode);
								
						switch (ConnectionTreeNode.GetNodeTypeFromString(xmlNode.Attributes["Type"].Value))
						{
						    case TreeNodeType.Connection:
						    {
						        var connectionInfo = GetConnectionInfoFromXml(xmlNode);
						        connectionInfo.TreeNode = treeNode;
						        connectionInfo.Parent = _previousContainer; //NEW
						        ConnectionList.Add(connectionInfo);
						        treeNode.Tag = connectionInfo;
						        treeNode.ImageIndex = (int)TreeImageType.ConnectionClosed;
						        treeNode.SelectedImageIndex = (int)TreeImageType.ConnectionClosed;
						    }
						        break;
						    case TreeNodeType.Container:
						    {
						        var containerInfo = new ContainerInfo();
						        if (treeNode.Parent != null)
						        {
						            if (ConnectionTreeNode.GetNodeType(treeNode.Parent) == TreeNodeType.Container)
						                containerInfo.Parent = (ContainerInfo)treeNode.Parent.Tag;
						        }
						        _previousContainer = containerInfo; //NEW
						        containerInfo.TreeNode = treeNode;
						        containerInfo.Name = xmlNode.Attributes["Name"].Value;
									
						        if (_confVersion >= 0.8)
						        {
						            containerInfo.IsExpanded = xmlNode.Attributes["Expanded"].Value == "True";
						        }

						        var connectionInfo = default(ConnectionInfo);
						        connectionInfo = _confVersion >= 0.9 ? GetConnectionInfoFromXml(xmlNode) : new ConnectionInfo();
									
						        connectionInfo.Parent = containerInfo;
						        connectionInfo.IsContainer = true;
						        containerInfo.ConnectionInfo = connectionInfo;
									
						        ContainerList.Add(containerInfo);
									
						        treeNode.Tag = containerInfo;
						        treeNode.ImageIndex = (int)TreeImageType.Container;
						        treeNode.SelectedImageIndex = (int)TreeImageType.Container;
						    }
						        break;
						}
								
						AddNodeFromXml(xmlNode, treeNode);
					}
				}
				else
				{
                    var nodeName = "";
                    var nameAttribute = parentXmlNode.Attributes["Name"];
					if (nameAttribute != null)
					{
						nodeName = nameAttribute.Value.Trim();
					}
					parentTreeNode.Text = !string.IsNullOrEmpty(nodeName) ? nodeName : parentXmlNode.Name;
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strAddNodeFromXmlFailed + Environment.NewLine + ex.Message + ex.StackTrace, true);
				throw;
			}
		}
				
		private ConnectionInfo GetConnectionInfoFromXml(XmlNode xxNode)
		{
            var connectionInfo = new ConnectionInfo();
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            try
			{
				var xmlnode = xxNode;
				if (_confVersion > 0.1) //0.2
				{
					connectionInfo.Name = xmlnode.Attributes["Name"].Value;
					connectionInfo.Description = xmlnode.Attributes["Descr"].Value;
					connectionInfo.Hostname = xmlnode.Attributes["Hostname"].Value;
					connectionInfo.Username = xmlnode.Attributes["Username"].Value;
					connectionInfo.Password = cryptographyProvider.Decrypt(xmlnode.Attributes["Password"].Value, _pW);
					connectionInfo.Domain = xmlnode.Attributes["Domain"].Value;
					connectionInfo.DisplayWallpaper = bool.Parse(xmlnode.Attributes["DisplayWallpaper"].Value);
					connectionInfo.DisplayThemes = bool.Parse(xmlnode.Attributes["DisplayThemes"].Value);
					connectionInfo.CacheBitmaps = bool.Parse(xmlnode.Attributes["CacheBitmaps"].Value);
							
					if (_confVersion < 1.1) //1.0 - 0.1
					{
						connectionInfo.Resolution = Convert.ToBoolean(xmlnode.Attributes["Fullscreen"].Value) == true ? ProtocolRDP.RDPResolutions.Fullscreen : ProtocolRDP.RDPResolutions.FitToWindow;
					}
				}
						
				if (_confVersion > 0.2) //0.3
				{
					if (_confVersion < 0.7)
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
						
				if (_confVersion > 0.3) //0.4
				{
					if (_confVersion < 0.7)
					{
						if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value))
                            connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["VNCPort"].Value);
						else
                            connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["RDPPort"].Value);
					}
							
					connectionInfo.UseConsoleSession = bool.Parse(xmlnode.Attributes["ConnectToConsole"].Value);
				}
				else
				{
					if (_confVersion < 0.7)
					{
						if (Convert.ToBoolean(xmlnode.Attributes["UseVNC"].Value))
							connectionInfo.Port = (int)ProtocolVNC.Defaults.Port;
						else
							connectionInfo.Port = (int)ProtocolRDP.Defaults.Port;
					}
					connectionInfo.UseConsoleSession = false;
				}
				
				if (_confVersion > 0.4) //0.5 and 0.6
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
				
				if (_confVersion > 0.6) //0.7
				{
                    connectionInfo.Protocol = (ProtocolType)Tools.MiscTools.StringToEnum(typeof(ProtocolType), xmlnode.Attributes["Protocol"].Value);
                    connectionInfo.Port = Convert.ToInt32(xmlnode.Attributes["Port"].Value);
				}
				
				if (_confVersion > 0.9) //1.0
				{
					connectionInfo.RedirectKeys = bool.Parse(xmlnode.Attributes["RedirectKeys"].Value);
				}
				
				if (_confVersion > 1.1) //1.2
				{
					connectionInfo.PuttySession = xmlnode.Attributes["PuttySession"].Value;
				}
				
				if (_confVersion > 1.2) //1.3
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
				
				if (_confVersion > 1.2) //1.3
				{
				    connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo)
				    {
				        CacheBitmaps = bool.Parse(xmlnode.Attributes["InheritCacheBitmaps"].Value),
				        Colors = bool.Parse(xmlnode.Attributes["InheritColors"].Value),
				        Description = bool.Parse(xmlnode.Attributes["InheritDescription"].Value),
				        DisplayThemes = bool.Parse(xmlnode.Attributes["InheritDisplayThemes"].Value),
				        DisplayWallpaper = bool.Parse(xmlnode.Attributes["InheritDisplayWallpaper"].Value),
				        Domain = bool.Parse(xmlnode.Attributes["InheritDomain"].Value),
				        Icon = bool.Parse(xmlnode.Attributes["InheritIcon"].Value),
				        Panel = bool.Parse(xmlnode.Attributes["InheritPanel"].Value),
				        Password = bool.Parse(xmlnode.Attributes["InheritPassword"].Value),
				        Port = bool.Parse(xmlnode.Attributes["InheritPort"].Value),
				        Protocol = bool.Parse(xmlnode.Attributes["InheritProtocol"].Value),
				        PuttySession = bool.Parse(xmlnode.Attributes["InheritPuttySession"].Value),
				        RedirectDiskDrives = bool.Parse(xmlnode.Attributes["InheritRedirectDiskDrives"].Value),
				        RedirectKeys = bool.Parse(xmlnode.Attributes["InheritRedirectKeys"].Value),
				        RedirectPorts = bool.Parse(xmlnode.Attributes["InheritRedirectPorts"].Value),
				        RedirectPrinters = bool.Parse(xmlnode.Attributes["InheritRedirectPrinters"].Value),
				        RedirectSmartCards = bool.Parse(xmlnode.Attributes["InheritRedirectSmartCards"].Value),
				        RedirectSound = bool.Parse(xmlnode.Attributes["InheritRedirectSound"].Value),
				        Resolution = bool.Parse(xmlnode.Attributes["InheritResolution"].Value),
				        UseConsoleSession = bool.Parse(xmlnode.Attributes["InheritUseConsoleSession"].Value),
				        Username = bool.Parse(xmlnode.Attributes["InheritUsername"].Value)
				    };
				    connectionInfo.Icon = xmlnode.Attributes["Icon"].Value;
					connectionInfo.Panel = xmlnode.Attributes["Panel"].Value;
				}
				else
				{
                    connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo, Convert.ToBoolean(xmlnode.Attributes["Inherit"].Value));
					connectionInfo.Icon = Convert.ToString(xmlnode.Attributes["Icon"].Value.Replace(".ico", ""));
					connectionInfo.Panel = Language.strGeneral;
				}
				
				if (_confVersion > 1.4) //1.5
				{
					connectionInfo.PleaseConnect = bool.Parse(xmlnode.Attributes["Connected"].Value);
				}
				
				if (_confVersion > 1.5) //1.6
				{
                    connectionInfo.ICAEncryption = (ProtocolICA.EncryptionStrength)Tools.MiscTools.StringToEnum(typeof(ProtocolICA.EncryptionStrength), xmlnode.Attributes["ICAEncryptionStrength"].Value);
					connectionInfo.Inheritance.ICAEncryption = bool.Parse(xmlnode.Attributes["InheritICAEncryptionStrength"].Value);
					connectionInfo.PreExtApp = xmlnode.Attributes["PreExtApp"].Value;
					connectionInfo.PostExtApp = xmlnode.Attributes["PostExtApp"].Value;
					connectionInfo.Inheritance.PreExtApp = bool.Parse(xmlnode.Attributes["InheritPreExtApp"].Value);
					connectionInfo.Inheritance.PostExtApp = bool.Parse(xmlnode.Attributes["InheritPostExtApp"].Value);
				}
				
				if (_confVersion > 1.6) //1.7
				{
                    connectionInfo.VNCCompression = (ProtocolVNC.Compression)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Compression), xmlnode.Attributes["VNCCompression"].Value);
                    connectionInfo.VNCEncoding = (ProtocolVNC.Encoding)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.Encoding), Convert.ToString(xmlnode.Attributes["VNCEncoding"].Value));
                    connectionInfo.VNCAuthMode = (ProtocolVNC.AuthMode)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.AuthMode), xmlnode.Attributes["VNCAuthMode"].Value);
                    connectionInfo.VNCProxyType = (ProtocolVNC.ProxyType)Tools.MiscTools.StringToEnum(typeof(ProtocolVNC.ProxyType), xmlnode.Attributes["VNCProxyType"].Value);
					connectionInfo.VNCProxyIP = xmlnode.Attributes["VNCProxyIP"].Value;
                    connectionInfo.VNCProxyPort = Convert.ToInt32(xmlnode.Attributes["VNCProxyPort"].Value);
					connectionInfo.VNCProxyUsername = xmlnode.Attributes["VNCProxyUsername"].Value;
                    connectionInfo.VNCProxyPassword = cryptographyProvider.Decrypt(xmlnode.Attributes["VNCProxyPassword"].Value, _pW);
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
				
				if (_confVersion > 1.7) //1.8
				{
                    connectionInfo.RDPAuthenticationLevel = (ProtocolRDP.AuthenticationLevel)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.AuthenticationLevel), xmlnode.Attributes["RDPAuthenticationLevel"].Value);
					connectionInfo.Inheritance.RDPAuthenticationLevel = bool.Parse(xmlnode.Attributes["InheritRDPAuthenticationLevel"].Value);
				}
				
				if (_confVersion > 1.8) //1.9
				{
                    connectionInfo.RenderingEngine = (HTTPBase.RenderingEngine)Tools.MiscTools.StringToEnum(typeof(HTTPBase.RenderingEngine), xmlnode.Attributes["RenderingEngine"].Value);
					connectionInfo.MacAddress = xmlnode.Attributes["MacAddress"].Value;
					connectionInfo.Inheritance.RenderingEngine = bool.Parse(xmlnode.Attributes["InheritRenderingEngine"].Value);
					connectionInfo.Inheritance.MacAddress = bool.Parse(xmlnode.Attributes["InheritMacAddress"].Value);
				}
				
				if (_confVersion > 1.9) //2.0
				{
					connectionInfo.UserField = xmlnode.Attributes["UserField"].Value;
					connectionInfo.Inheritance.UserField = bool.Parse(xmlnode.Attributes["InheritUserField"].Value);
				}
				
				if (_confVersion > 2.0) //2.1
				{
					connectionInfo.ExtApp = xmlnode.Attributes["ExtApp"].Value;
					connectionInfo.Inheritance.ExtApp = bool.Parse(xmlnode.Attributes["InheritExtApp"].Value);
				}
				
				if (_confVersion > 2.1) //2.2
				{
					// Get settings
                    connectionInfo.RDGatewayUsageMethod = (ProtocolRDP.RDGatewayUsageMethod)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUsageMethod), Convert.ToString(xmlnode.Attributes["RDGatewayUsageMethod"].Value));
					connectionInfo.RDGatewayHostname = xmlnode.Attributes["RDGatewayHostname"].Value;
                    connectionInfo.RDGatewayUseConnectionCredentials = (ProtocolRDP.RDGatewayUseConnectionCredentials)Tools.MiscTools.StringToEnum(typeof(ProtocolRDP.RDGatewayUseConnectionCredentials), Convert.ToString(xmlnode.Attributes["RDGatewayUseConnectionCredentials"].Value));
					connectionInfo.RDGatewayUsername = xmlnode.Attributes["RDGatewayUsername"].Value;
					connectionInfo.RDGatewayPassword = cryptographyProvider.Decrypt(Convert.ToString(xmlnode.Attributes["RDGatewayPassword"].Value), _pW);
					connectionInfo.RDGatewayDomain = xmlnode.Attributes["RDGatewayDomain"].Value;
							
					// Get inheritance settings
					connectionInfo.Inheritance.RDGatewayUsageMethod = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsageMethod"].Value);
					connectionInfo.Inheritance.RDGatewayHostname = bool.Parse(xmlnode.Attributes["InheritRDGatewayHostname"].Value);
					connectionInfo.Inheritance.RDGatewayUseConnectionCredentials = bool.Parse(xmlnode.Attributes["InheritRDGatewayUseConnectionCredentials"].Value);
					connectionInfo.Inheritance.RDGatewayUsername = bool.Parse(xmlnode.Attributes["InheritRDGatewayUsername"].Value);
					connectionInfo.Inheritance.RDGatewayPassword = bool.Parse(xmlnode.Attributes["InheritRDGatewayPassword"].Value);
					connectionInfo.Inheritance.RDGatewayDomain = bool.Parse(xmlnode.Attributes["InheritRDGatewayDomain"].Value);
				}
				
				if (_confVersion > 2.2) //2.3
				{
					// Get settings
					connectionInfo.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["EnableFontSmoothing"].Value);
					connectionInfo.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["EnableDesktopComposition"].Value);
							
					// Get inheritance settings
					connectionInfo.Inheritance.EnableFontSmoothing = bool.Parse(xmlnode.Attributes["InheritEnableFontSmoothing"].Value);
					connectionInfo.Inheritance.EnableDesktopComposition = bool.Parse(xmlnode.Attributes["InheritEnableDesktopComposition"].Value);
				}
				
				if (_confVersion >= 2.4)
				{
					connectionInfo.UseCredSsp = bool.Parse(xmlnode.Attributes["UseCredSsp"].Value);
					connectionInfo.Inheritance.UseCredSsp = bool.Parse(xmlnode.Attributes["InheritUseCredSsp"].Value);
				}
				
				if (_confVersion >= 2.5)
				{
					connectionInfo.LoadBalanceInfo = xmlnode.Attributes["LoadBalanceInfo"].Value;
					connectionInfo.AutomaticResize = bool.Parse(xmlnode.Attributes["AutomaticResize"].Value);
					connectionInfo.Inheritance.LoadBalanceInfo = bool.Parse(xmlnode.Attributes["InheritLoadBalanceInfo"].Value);
					connectionInfo.Inheritance.AutomaticResize = bool.Parse(xmlnode.Attributes["InheritAutomaticResize"].Value);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, string.Format(Language.strGetConnectionInfoFromXmlFailed, connectionInfo.Name, ConnectionFileName, ex.Message), false);
			}
			return connectionInfo;
		}
				
		private bool Authenticate(string Value, bool CompareToOriginalValue, RootNodeInfo rootInfo = null)
		{
            var passwordName = "";
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            if (UseSql)
			{
				passwordName = Language.strSQLServer.TrimEnd(':');
			}
			else
			{
				passwordName = Path.GetFileName(ConnectionFileName);
			}
					
			if (CompareToOriginalValue)
			{
				while (cryptographyProvider.Decrypt(Value, _pW) == Value)
				{
					_pW = Tools.MiscTools.PasswordDialog(passwordName, false);
							
					if (_pW.Length == 0)
					{
						return false;
					}
				}
			}
			else
			{
				while (cryptographyProvider.Decrypt(Value, _pW) != "ThisIsProtected")
				{
					_pW = Tools.MiscTools.PasswordDialog(passwordName, false);
							
					if (_pW.Length == 0)
					{
						return false;
					}
				}

			    if (rootInfo == null) return true;
			    rootInfo.Password = true;
			    rootInfo.PasswordString = _pW.ConvertToUnsecureString();
			}
					
			return true;
		}
        #endregion
	}
}