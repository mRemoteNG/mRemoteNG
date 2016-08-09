using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.Config.Connections
{
    public class SqlConnectionsLoader
    {
        private SqlConnection _sqlConnection;
        private SqlCommand _sqlQuery;
        private SqlDataReader _sqlDataReader;
        private TreeNode _selectedTreeNode;
        private double _confVersion;
        private SecureString _pW = GeneralAppInfo.EncryptionKey;


        public string DatabaseHost { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUsername { get; set; }
        public string DatabasePassword { get; set; }
        public bool DatabaseUpdate { get; set; }
        public string PreviousSelected { get; set; }
        public TreeNode RootTreeNode { get; set; }
        public ConnectionList ConnectionList { get; set; }
        public ContainerList ContainerList { get; set; }
        public ConnectionList PreviousConnectionList { get; set; }
        public ContainerList PreviousContainerList { get; set; }


        private delegate void LoadFromSqlDelegate();
        public void LoadFromSql()
        {
            if (Windows.treeForm == null || Windows.treeForm.tvConnections == null)
                return;
            if (Windows.treeForm.tvConnections.InvokeRequired)
            {
                Windows.treeForm.tvConnections.Invoke(new LoadFromSqlDelegate(LoadFromSql));
                return;
            }

            try
            {
                Runtime.IsConnectionsFileLoaded = false;
                _sqlConnection = !string.IsNullOrEmpty(DatabaseUsername) ? new SqlConnection("Data Source=" + DatabaseHost + ";Initial Catalog=" + DatabaseName + ";User Id=" + DatabaseUsername + ";Password=" + DatabasePassword) : new SqlConnection("Data Source=" + DatabaseHost + ";Initial Catalog=" + DatabaseName + ";Integrated Security=True");

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
                        string.Format("Schema Version: {1}{0}Highest Supported Version: {2}", Environment.NewLine, _confVersion, maxSupportedSchemaVersion),
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

        private void AddNodesFromSql(TreeNode baseNode)
        {
            try
            {
                _sqlConnection.Open();
                _sqlQuery = new SqlCommand("SELECT * FROM tblCons ORDER BY PositionID ASC", _sqlConnection);
                _sqlDataReader = _sqlQuery.ExecuteReader(CommandBehavior.CloseConnection);

                if (_sqlDataReader.HasRows == false)
                    return;

                while (_sqlDataReader.Read())
                {
                    var tNode = new TreeNode(Convert.ToString(_sqlDataReader["Name"]));
                    var nodeType = ConnectionTreeNode.GetNodeTypeFromString(Convert.ToString(_sqlDataReader["Type"]));

                    if (nodeType == TreeNodeType.Connection)
                        AddConnectionToList(tNode);
                    else if (nodeType == TreeNodeType.Container)
                        AddContainerToList(tNode);

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
                                    ((ConnectionInfo) tNode.Tag).Parent = (ContainerInfo)pNode.Tag;
                                    break;
                                case TreeNodeType.Container:
                                    ((ContainerInfo) tNode.Tag).Parent = (ContainerInfo)pNode.Tag;
                                    break;
                            }
                        }
                        else
                        {
                            baseNode.Nodes.Add(tNode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strAddNodesFromSqlFailed + Environment.NewLine + ex.Message, true);
            }
        }

        private void AddConnectionToList(TreeNode tNode)
        {
            var conI = GetConnectionInfoFromSql();
            conI.TreeNode = tNode;
            ConnectionList.Add(conI);
            tNode.Tag = conI;

            if (DatabaseUpdate)
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
                        tNode.ImageIndex = (int) TreeImageType.ConnectionOpen;
                        tNode.SelectedImageIndex = (int) TreeImageType.ConnectionOpen;
                    }
                    else
                    {
                        tNode.ImageIndex = (int) TreeImageType.ConnectionClosed;
                        tNode.SelectedImageIndex = (int) TreeImageType.ConnectionClosed;
                    }
                }
                else
                {
                    tNode.ImageIndex = (int) TreeImageType.ConnectionClosed;
                    tNode.SelectedImageIndex = (int) TreeImageType.ConnectionClosed;
                }

                if (conI.ConstantID == PreviousSelected)
                    _selectedTreeNode = tNode;
            }
            else
            {
                tNode.ImageIndex = (int) TreeImageType.ConnectionClosed;
                tNode.SelectedImageIndex = (int) TreeImageType.ConnectionClosed;
            }
        }

        private void AddContainerToList(TreeNode tNode)
        {
            var contI = new ContainerInfo
            {
                TreeNode = tNode,
                Name = Convert.ToString(_sqlDataReader["Name"])
            };

            var conI = GetConnectionInfoFromSql();
            conI.Parent = contI;
            conI.IsContainer = true;
            contI.ConnectionInfo = conI;

            if (DatabaseUpdate)
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
                connectionInfo.Icon = Convert.ToString(_sqlDataReader["Icon"]);
                connectionInfo.Panel = Convert.ToString(_sqlDataReader["Panel"]);
                connectionInfo.Inheritance = new ConnectionInfoInheritance(connectionInfo)
                {
                    CacheBitmaps = Convert.ToBoolean(_sqlDataReader["InheritCacheBitmaps"]),
                    Colors = Convert.ToBoolean(_sqlDataReader["InheritColors"]),
                    Description = Convert.ToBoolean(_sqlDataReader["InheritDescription"]),
                    DisplayThemes = Convert.ToBoolean(_sqlDataReader["InheritDisplayThemes"]),
                    DisplayWallpaper = Convert.ToBoolean(_sqlDataReader["InheritDisplayWallpaper"]),
                    Domain = Convert.ToBoolean(_sqlDataReader["InheritDomain"]),
                    Icon = Convert.ToBoolean(_sqlDataReader["InheritIcon"]),
                    Panel = Convert.ToBoolean(_sqlDataReader["InheritPanel"]),
                    Password = Convert.ToBoolean(_sqlDataReader["InheritPassword"]),
                    Port = Convert.ToBoolean(_sqlDataReader["InheritPort"]),
                    Protocol = Convert.ToBoolean(_sqlDataReader["InheritProtocol"]),
                    PuttySession = Convert.ToBoolean(_sqlDataReader["InheritPuttySession"]),
                    RedirectDiskDrives = Convert.ToBoolean(_sqlDataReader["InheritRedirectDiskDrives"]),
                    RedirectKeys = Convert.ToBoolean(_sqlDataReader["InheritRedirectKeys"]),
                    RedirectPorts = Convert.ToBoolean(_sqlDataReader["InheritRedirectPorts"]),
                    RedirectPrinters = Convert.ToBoolean(_sqlDataReader["InheritRedirectPrinters"]),
                    RedirectSmartCards = Convert.ToBoolean(_sqlDataReader["InheritRedirectSmartCards"]),
                    RedirectSound = Convert.ToBoolean(_sqlDataReader["InheritRedirectSound"]),
                    Resolution = Convert.ToBoolean(_sqlDataReader["InheritResolution"]),
                    UseConsoleSession = Convert.ToBoolean(_sqlDataReader["InheritUseConsoleSession"]),
                    Username = Convert.ToBoolean(_sqlDataReader["InheritUsername"])
                };

                if (_confVersion > 1.5) //1.6
                {
                    connectionInfo.ICAEncryption = (ProtocolICA.EncryptionStrength)Tools.MiscTools.StringToEnum(typeof(ProtocolICA.EncryptionStrength), Convert.ToString(_sqlDataReader["ICAEncryptionStrength"]));
                    connectionInfo.Inheritance.ICAEncryptionStrength = Convert.ToBoolean(_sqlDataReader["InheritICAEncryptionStrength"]);
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

                if (DatabaseUpdate)
                    connectionInfo.PleaseConnect = Convert.ToBoolean(_sqlDataReader["Connected"]);

                return connectionInfo;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strGetConnectionInfoFromSqlFailed + Environment.NewLine + ex.Message, true);
            }

            return null;
        }

        private delegate void SetSelectedNodeDelegate(TreeNode treeNode);
        private static void SetSelectedNode(TreeNode treeNode)
        {
            if (ConnectionTree.TreeView != null && ConnectionTree.TreeView.InvokeRequired)
            {
                Windows.treeForm.Invoke(new SetSelectedNodeDelegate(SetSelectedNode), treeNode);
                return;
            }
            Windows.treeForm.tvConnections.SelectedNode = treeNode;
        }

        private bool Authenticate(string value, bool compareToOriginalValue, RootNodeInfo rootInfo = null)
        {
            var passwordName = "";
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            passwordName = Language.strSQLServer.TrimEnd(':');


            if (compareToOriginalValue)
            {
                while (cryptographyProvider.Decrypt(value, _pW) == value)
                {
                    _pW = Tools.MiscTools.PasswordDialog(passwordName, false);
                    if (_pW.Length == 0)
                        return false;
                }
            }
            else
            {
                while (cryptographyProvider.Decrypt(value, _pW) != "ThisIsProtected")
                {
                    _pW = Tools.MiscTools.PasswordDialog(passwordName, false);
                    if (_pW.Length == 0)
                        return false;
                }

                if (rootInfo == null) return true;
                rootInfo.Password = true;
                rootInfo.PasswordString = _pW.ConvertToUnsecureString();
            }

            return true;
        }

    }
}