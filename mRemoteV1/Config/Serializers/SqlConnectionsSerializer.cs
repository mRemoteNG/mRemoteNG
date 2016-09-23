using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Security;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers
{
    public class SqlConnectionsSerializer : ISerializer
    {
        private SqlConnection _sqlConnection;
        private SqlCommand _sqlQuery;
        private SecureString _password = GeneralAppInfo.EncryptionKey;
        private int _currentNodeIndex;
        private string _parentConstantId = Convert.ToString(0);

        public string SqlHost { get; set; }
        public string SqlDatabaseName { get; set; }
        public string SqlUsername { get; set; }
        public string SqlPassword { get; set; }
        public TreeNode RootTreeNode { get; set; }
        public Save SaveSecurity { get; set; }
        public ConnectionList ConnectionList { get; set; }
        public ContainerList ContainerList { get; set; }
        public ConnectionTreeModel ConnectionTreeModel { get; set; }

        public string Serialize(ConnectionTreeModel connectionTreeModel)
        {
            throw new NotImplementedException();
        }

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
            if (SqlUsername != "")
            {
                _sqlConnection = new SqlConnection("Data Source=" + SqlHost + ";Initial Catalog=" + SqlDatabaseName + ";User Id=" + SqlUsername + ";Password=" + SqlPassword);
            }
            else
            {
                _sqlConnection = new SqlConnection("Data Source=" + SqlHost + ";Initial Catalog=" + SqlDatabaseName + ";Integrated Security=True");
            }
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();

            _sqlConnection.Open();

            if (!VerifyDatabaseVersion(_sqlConnection))
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strErrorConnectionListSaveFailed);
                return;
            }

            var tN = (TreeNode)RootTreeNode.Clone();

            string strProtected;
            if (tN.Tag != null)
            {
                if (((RootNodeInfo)tN.Tag).Password)
                {
                    _password = Convert.ToString(((RootNodeInfo)tN.Tag).PasswordString).ConvertToSecureString();
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
                var sqlInsertStatement =
                    "INSERT INTO tblCons (Name, Type, Expanded, Description, Icon, Panel, Username, " +
                    "DomainName, Password, Hostname, Protocol, PuttySession, " +
                    "Port, ConnectToConsole, RenderingEngine, ICAEncryptionStrength, RDPAuthenticationLevel, LoadBalanceInfo, Colors, Resolution, AutomaticResize, DisplayWallpaper, " +
                    "DisplayThemes, EnableFontSmoothing, EnableDesktopComposition, CacheBitmaps, RedirectDiskDrives, RedirectPorts, " +
                    "RedirectPrinters, RedirectSmartCards, RedirectSound, RedirectKeys, " +
                    "Connected, PreExtApp, PostExtApp, MacAddress, UserField, ExtApp, VNCCompression, VNCEncoding, VNCAuthMode, " +
                    "VNCProxyType, VNCProxyIP, VNCProxyPort, VNCProxyUsername, VNCProxyPassword, " +
                    "VNCColors, VNCSmartSizeMode, VNCViewOnly, " +
                    "RDGatewayUsageMethod, RDGatewayHostname, RDGatewayUseConnectionCredentials, RDGatewayUsername, RDGatewayPassword, RDGatewayDomain, " +
                    "UseCredSsp, " + "InheritCacheBitmaps, InheritColors, " +
                    "InheritDescription, InheritDisplayThemes, InheritDisplayWallpaper, InheritEnableFontSmoothing, InheritEnableDesktopComposition, InheritDomain, " +
                    "InheritIcon, InheritPanel, InheritPassword, InheritPort, " +
                    "InheritProtocol, InheritPuttySession, InheritRedirectDiskDrives, " +
                    "InheritRedirectKeys, InheritRedirectPorts, InheritRedirectPrinters, " +
                    "InheritRedirectSmartCards, InheritRedirectSound, InheritResolution, InheritAutomaticResize, " +
                    "InheritUseConsoleSession, InheritRenderingEngine, InheritUsername, InheritICAEncryptionStrength, InheritRDPAuthenticationLevel, InheritLoadBalanceInfo, " +
                    "InheritPreExtApp, InheritPostExtApp, InheritMacAddress, InheritUserField, InheritExtApp, InheritVNCCompression, InheritVNCEncoding, " +
                    "InheritVNCAuthMode, InheritVNCProxyType, InheritVNCProxyIP, InheritVNCProxyPort, " +
                    "InheritVNCProxyUsername, InheritVNCProxyPassword, InheritVNCColors, " +
                    "InheritVNCSmartSizeMode, InheritVNCViewOnly, " +
                    "InheritRDGatewayUsageMethod, InheritRDGatewayHostname, InheritRDGatewayUseConnectionCredentials, InheritRDGatewayUsername, InheritRDGatewayPassword, InheritRDGatewayDomain, "
                    + "InheritUseCredSsp, " + "PositionID, ParentID, ConstantID, LastChange)" + "VALUES (";
                _sqlQuery = new SqlCommand(sqlInsertStatement, _sqlConnection);

                if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Connection | ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Container)
                {
                    _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(node.Text) + "\',"; //Name
                    _sqlQuery.CommandText += "\'" + ConnectionTreeNode.GetNodeType(node) + "\',"; //Type
                }

                if (ConnectionTreeNode.GetNodeType(node) == TreeNodeType.Container) //container
                {
                    _sqlQuery.CommandText += "\'" + ContainerList[node.Tag].IsExpanded + "\',"; //Expanded
                    curConI = ContainerList[node.Tag];
                    SerializeConnectionInfo(curConI);

                    _sqlQuery.CommandText = MiscTools.PrepareForDB(_sqlQuery.CommandText);
                    _sqlQuery.ExecuteNonQuery();
                    //_parentConstantId = _currentNodeIndex
                    SaveNodesSQL(node.Nodes);
                }

                if (ConnectionTreeNode.GetNodeType(node) != TreeNodeType.Connection) continue;
                _sqlQuery.CommandText += "\'" + Convert.ToString(false) + "\',";
                curConI = ConnectionList[node.Tag];
                SerializeConnectionInfo(curConI);
                _sqlQuery.CommandText = MiscTools.PrepareForDB(_sqlQuery.CommandText);
                _sqlQuery.ExecuteNonQuery();
                //_parentConstantId = 0
            }
        }

        private void SerializeConnectionInfo(ConnectionInfo connectionInfo)
        {
            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(connectionInfo.Description) + "\',";
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(connectionInfo.Icon) + "\',";
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(connectionInfo.Panel) + "\',";

            if (SaveSecurity.Username)
                _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(connectionInfo.Username) + "\',";
            else
                _sqlQuery.CommandText += "\'" + "" + "\',";

            if (SaveSecurity.Domain)
                _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(connectionInfo.Domain) + "\',";
            else
                _sqlQuery.CommandText += "\'" + "" + "\',";

            if (SaveSecurity.Password)
                _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(cryptographyProvider.Encrypt(connectionInfo.Password, _password)) + "\',";
            else
                _sqlQuery.CommandText += "\'" + "" + "\',";

            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(connectionInfo.Hostname) + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.Protocol + "\',";
            _sqlQuery.CommandText += "\'" + MiscTools.PrepareValueForDB(connectionInfo.PuttySession) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.Port) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.UseConsoleSession) + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.RenderingEngine + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.ICAEncryptionStrength + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.RDPAuthenticationLevel + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.LoadBalanceInfo + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.Colors + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.Resolution + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.AutomaticResize) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.DisplayWallpaper) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.DisplayThemes) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.EnableFontSmoothing) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.EnableDesktopComposition) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.CacheBitmaps) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.RedirectDiskDrives) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.RedirectPorts) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.RedirectPrinters) + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.RedirectSmartCards) + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.RedirectSound + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.RedirectKeys) + "\',";

            if (connectionInfo.OpenConnections.Count > 0)
                _sqlQuery.CommandText += 1 + ",";
            else
                _sqlQuery.CommandText += 0 + ",";

            _sqlQuery.CommandText += "\'" + connectionInfo.PreExtApp + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.PostExtApp + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.MacAddress + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.UserField + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.ExtApp + "\',";

            _sqlQuery.CommandText += "\'" + connectionInfo.VNCCompression + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.VNCEncoding + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.VNCAuthMode + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.VNCProxyType + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.VNCProxyIP + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.VNCProxyPort) + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.VNCProxyUsername + "\',";
            _sqlQuery.CommandText += "\'" + cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword, _password) + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.VNCColors + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.VNCSmartSizeMode + "\',";
            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.VNCViewOnly) + "\',";

            _sqlQuery.CommandText += "\'" + connectionInfo.RDGatewayUsageMethod + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.RDGatewayHostname + "\',";
            _sqlQuery.CommandText += "\'" + connectionInfo.RDGatewayUseConnectionCredentials + "\',";

            if (SaveSecurity.Username)
                _sqlQuery.CommandText += "\'" + connectionInfo.RDGatewayUsername + "\',";
            else
                _sqlQuery.CommandText += "\'" + "" + "\',";

            if (SaveSecurity.Password)
                _sqlQuery.CommandText += "\'" + cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword, _password) + "\',";
            else
                _sqlQuery.CommandText += "\'" + "" + "\',";

            if (SaveSecurity.Domain)
                _sqlQuery.CommandText += "\'" + connectionInfo.RDGatewayDomain + "\',";
            else
                _sqlQuery.CommandText += "\'" + "" + "\',";

            _sqlQuery.CommandText += "\'" + Convert.ToString(connectionInfo.UseCredSsp) + "\',";

            if (SaveSecurity.Inheritance)
            {
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.CacheBitmaps + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Colors + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Description + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.DisplayThemes + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.DisplayWallpaper + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.EnableFontSmoothing + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.EnableDesktopComposition + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Domain + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Icon + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Panel + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Password + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Port + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Protocol + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.PuttySession + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RedirectDiskDrives + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RedirectKeys + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RedirectPorts + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RedirectPrinters + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RedirectSmartCards + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RedirectSound + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Resolution + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.AutomaticResize + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.UseConsoleSession + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RenderingEngine + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.Username + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.ICAEncryptionStrength + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RDPAuthenticationLevel + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.LoadBalanceInfo + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.PreExtApp + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.PostExtApp + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.MacAddress + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.UserField + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.ExtApp + "\',";

                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCCompression + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCEncoding + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCAuthMode + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCProxyType + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCProxyIP + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCProxyPort + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCProxyUsername + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCProxyPassword + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCColors + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCSmartSizeMode + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.VNCViewOnly + "\',";

                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RDGatewayUsageMethod + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RDGatewayHostname + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RDGatewayUseConnectionCredentials + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RDGatewayUsername + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RDGatewayPassword + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.RDGatewayDomain + "\',";
                _sqlQuery.CommandText += "\'" + connectionInfo.Inheritance.UseCredSsp + "\',";
            }
            else
            {
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',"; // .AutomaticResize
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',"; // .LoadBalanceInfo
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";

                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";
                _sqlQuery.CommandText += "\'" + false + "\',";

                _sqlQuery.CommandText += "\'" + false + "\',"; // .RDGatewayUsageMethod
                _sqlQuery.CommandText += "\'" + false + "\',"; // .RDGatewayHostname
                _sqlQuery.CommandText += "\'" + false + "\',"; // .RDGatewayUseConnectionCredentials
                _sqlQuery.CommandText += "\'" + false + "\',"; // .RDGatewayUsername
                _sqlQuery.CommandText += "\'" + false + "\',"; // .RDGatewayPassword
                _sqlQuery.CommandText += "\'" + false + "\',"; // .RDGatewayDomain
                _sqlQuery.CommandText += "\'" + false + "\',"; // .UseCredSsp
            }

            connectionInfo.PositionID = _currentNodeIndex;

            if (connectionInfo.IsContainer == false)
            {
                _parentConstantId = connectionInfo.Parent != null ? connectionInfo.Parent.ConstantID : "0";
            }
            else
            {
                _parentConstantId = connectionInfo.Parent.Parent != null ? connectionInfo.Parent.Parent.ConstantID : "0";
            }

            _sqlQuery.CommandText += _currentNodeIndex + ",\'" + _parentConstantId + "\',\'" + connectionInfo.ConstantID + "\',\'" + MiscTools.DBDate(DateTime.Now) + "\')";
        }
    }
}