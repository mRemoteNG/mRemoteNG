using System;
using System.Data;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Connections
{
    public class DataTableSerializer
    {
        private DataTable _dataTable;
        private const string TableName = "tblCons";
        private readonly Save _saveSecurity;
        private int _currentNodeIndex;

        public DataTableSerializer(Save saveSecurity)
        {
            _saveSecurity = saveSecurity;
        }


        public DataTable Serialize(ConnectionTreeModel connectionTreeModel)
        {
            var rootNode = (RootNodeInfo)connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
            SerializeToDataTable(rootNode);
            return _dataTable;
        }

        private void SerializeToDataTable(RootNodeInfo rootNodeInfo)
        {
            _dataTable = new DataTable(TableName);
            CreateSchema();
            SetPrimaryKey();
            _currentNodeIndex = 0;
            SerializeNodesRecursive(rootNodeInfo);
        }

        private void CreateSchema()
        {
            _dataTable.Columns.Add("Name");
            _dataTable.Columns.Add("Type");
            _dataTable.Columns.Add("ConstantID");
            _dataTable.Columns.Add("ParentID");
            _dataTable.Columns.Add("PositionID");
            _dataTable.Columns.Add("LastChange");
            _dataTable.Columns.Add("Expanded");
            _dataTable.Columns.Add("Description");
            _dataTable.Columns.Add("Icon");
            _dataTable.Columns.Add("Panel");
            _dataTable.Columns.Add("Username");
            _dataTable.Columns.Add("Domain");
            _dataTable.Columns.Add("Password");
            _dataTable.Columns.Add("Hostname");
            _dataTable.Columns.Add("Protocol");
            _dataTable.Columns.Add("PuttySession");
            _dataTable.Columns.Add("Port");
            _dataTable.Columns.Add("ConnectToConsole");
            _dataTable.Columns.Add("UseCredSsp");
            _dataTable.Columns.Add("RenderingEngine");
            _dataTable.Columns.Add("ICAEncryptionStrength");
            _dataTable.Columns.Add("RDPAuthenticationLevel");
            _dataTable.Columns.Add("LoadBalanceInfo");
            _dataTable.Columns.Add("Colors");
            _dataTable.Columns.Add("Resolution");
            _dataTable.Columns.Add("AutomaticResize");
            _dataTable.Columns.Add("DisplayWallpaper");
            _dataTable.Columns.Add("DisplayThemes");
            _dataTable.Columns.Add("EnableFontSmoothing");
            _dataTable.Columns.Add("EnableDesktopComposition");
            _dataTable.Columns.Add("CacheBitmaps");
            _dataTable.Columns.Add("RedirectDiskDrives");
            _dataTable.Columns.Add("RedirectPorts");
            _dataTable.Columns.Add("RedirectPrinters");
            _dataTable.Columns.Add("RedirectSmartCards");
            _dataTable.Columns.Add("RedirectSound");
            _dataTable.Columns.Add("RedirectKeys");
            _dataTable.Columns.Add("Connected");
            _dataTable.Columns.Add("PreExtApp");
            _dataTable.Columns.Add("PostExtApp");
            _dataTable.Columns.Add("MacAddress");
            _dataTable.Columns.Add("UserField");
            _dataTable.Columns.Add("ExtApp");
            _dataTable.Columns.Add("VNCCompression");
            _dataTable.Columns.Add("VNCEncoding");
            _dataTable.Columns.Add("VNCAuthMode");
            _dataTable.Columns.Add("VNCProxyType");
            _dataTable.Columns.Add("VNCProxyIP");
            _dataTable.Columns.Add("VNCProxyPort");
            _dataTable.Columns.Add("VNCProxyUsername");
            _dataTable.Columns.Add("VNCProxyPassword");
            _dataTable.Columns.Add("VNCColors");
            _dataTable.Columns.Add("VNCSmartSizeMode");
            _dataTable.Columns.Add("VNCViewOnly");
            _dataTable.Columns.Add("RDGatewayUsageMethod");
            _dataTable.Columns.Add("RDGatewayHostname");
            _dataTable.Columns.Add("RDGatewayUseConnectionCredentials");
            _dataTable.Columns.Add("RDGatewayUsername");
            _dataTable.Columns.Add("RDGatewayPassword");
            _dataTable.Columns.Add("RDGatewayDomain");
            _dataTable.Columns.Add("InheritCacheBitmaps");
            _dataTable.Columns.Add("InheritColors");
            _dataTable.Columns.Add("InheritDescription");
            _dataTable.Columns.Add("InheritDisplayThemes");
            _dataTable.Columns.Add("InheritDisplayWallpaper");
            _dataTable.Columns.Add("InheritEnableFontSmoothing");
            _dataTable.Columns.Add("InheritEnableDesktopComposition");
            _dataTable.Columns.Add("InheritDomain");
            _dataTable.Columns.Add("InheritIcon");
            _dataTable.Columns.Add("InheritPanel");
            _dataTable.Columns.Add("InheritPassword");
            _dataTable.Columns.Add("InheritPort");
            _dataTable.Columns.Add("InheritProtocol");
            _dataTable.Columns.Add("InheritPuttySession");
            _dataTable.Columns.Add("InheritRedirectDiskDrives");
            _dataTable.Columns.Add("InheritRedirectKeys");
            _dataTable.Columns.Add("InheritRedirectPorts");
            _dataTable.Columns.Add("InheritRedirectPrinters");
            _dataTable.Columns.Add("InheritRedirectSmartCards");
            _dataTable.Columns.Add("InheritRedirectSound");
            _dataTable.Columns.Add("InheritResolution");
            _dataTable.Columns.Add("InheritAutomaticResize");
            _dataTable.Columns.Add("InheritUseConsoleSession");
            _dataTable.Columns.Add("InheritUseCredSsp");
            _dataTable.Columns.Add("InheritRenderingEngine");
            _dataTable.Columns.Add("InheritUsername");
            _dataTable.Columns.Add("InheritICAEncryptionStrength");
            _dataTable.Columns.Add("InheritRDPAuthenticationLevel");
            _dataTable.Columns.Add("InheritLoadBalanceInfo");
            _dataTable.Columns.Add("InheritPreExtApp");
            _dataTable.Columns.Add("InheritPostExtApp");
            _dataTable.Columns.Add("InheritMacAddress");
            _dataTable.Columns.Add("InheritUserField");
            _dataTable.Columns.Add("InheritExtApp");
            _dataTable.Columns.Add("InheritVNCCompression");
            _dataTable.Columns.Add("InheritVNCEncoding");
            _dataTable.Columns.Add("InheritVNCAuthMode");
            _dataTable.Columns.Add("InheritVNCProxyType");
            _dataTable.Columns.Add("InheritVNCProxyIP");
            _dataTable.Columns.Add("InheritVNCProxyPort");
            _dataTable.Columns.Add("InheritVNCProxyUsername");
            _dataTable.Columns.Add("InheritVNCProxyPassword");
            _dataTable.Columns.Add("InheritVNCColors");
            _dataTable.Columns.Add("InheritVNCSmartSizeMode");
            _dataTable.Columns.Add("InheritVNCViewOnly");
            _dataTable.Columns.Add("InheritRDGatewayUsageMethod");
            _dataTable.Columns.Add("InheritRDGatewayHostname");
            _dataTable.Columns.Add("InheritRDGatewayUseConnectionCredentials");
            _dataTable.Columns.Add("InheritRDGatewayUsername");
            _dataTable.Columns.Add("InheritRDGatewayPassword");
            _dataTable.Columns.Add("InheritRDGatewayDomain");
        }

        private void SetPrimaryKey()
        {
            _dataTable.PrimaryKey = new[] { _dataTable.Columns["ConstantID"] };
        }

        private void SerializeNodesRecursive(ConnectionInfo connectionInfo)
        {
            if (!(connectionInfo is RootNodeInfo))
                SerializeConnectionInfo(connectionInfo);
            var containerInfo = connectionInfo as ContainerInfo;
            if (containerInfo == null) return;
            foreach (var child in containerInfo.Children)
                SerializeNodesRecursive(child);
        }

        private void SerializeConnectionInfo(ConnectionInfo connectionInfo)
        {
            _currentNodeIndex++;
            var dataRow = _dataTable.NewRow();
            dataRow["Name"] = connectionInfo.Name;
            dataRow["Type"] = ConnectionTreeNode.GetNodeType(connectionInfo.TreeNode).ToString();
            dataRow["ConstantID"] = connectionInfo.ConstantID;
            dataRow["ParentID"] = connectionInfo.Parent.ConstantID;
            dataRow["PositionID"] = _currentNodeIndex;
            dataRow["LastChange"] = FormateDateForDatabase(DateTime.Now);
            dataRow["Expanded"] = connectionInfo is ContainerInfo ? ((ContainerInfo)connectionInfo).IsExpanded.ToString() : "";
            dataRow["Description"] = connectionInfo.Description;
            dataRow["Icon"] = connectionInfo.Icon;
            dataRow["Panel"] = connectionInfo.Panel;
            dataRow["Username"] = _saveSecurity.Username ? connectionInfo.Username : "";
            dataRow["Domain"] = _saveSecurity.Domain ? connectionInfo.Domain : "";
            dataRow["Password"] = _saveSecurity.Password ? connectionInfo.Password : "";
            dataRow["Hostname"] = connectionInfo.Hostname;
            dataRow["Protocol"] = connectionInfo.Protocol;
            dataRow["PuttySession"] = connectionInfo.PuttySession;
            dataRow["Port"] = connectionInfo.Port;
            dataRow["ConnectToConsole"] = connectionInfo.UseConsoleSession;
            dataRow["UseCredSsp"] = connectionInfo.UseCredSsp;
            dataRow["RenderingEngine"] = connectionInfo.RenderingEngine;
            dataRow["ICAEncryptionStrength"] = connectionInfo.ICAEncryptionStrength;
            dataRow["RDPAuthenticationLevel"] = connectionInfo.RDPAuthenticationLevel;
            dataRow["LoadBalanceInfo"] = connectionInfo.LoadBalanceInfo;
            dataRow["Colors"] = connectionInfo.Colors;
            dataRow["Resolution"] = connectionInfo.Resolution;
            dataRow["AutomaticResize"] = connectionInfo.AutomaticResize;
            dataRow["DisplayWallpaper"] = connectionInfo.DisplayWallpaper;
            dataRow["DisplayThemes"] = connectionInfo.DisplayThemes;
            dataRow["EnableFontSmoothing"] = connectionInfo.EnableFontSmoothing;
            dataRow["EnableDesktopComposition"] = connectionInfo.EnableDesktopComposition;
            dataRow["CacheBitmaps"] = connectionInfo.CacheBitmaps;
            dataRow["RedirectDiskDrives"] = connectionInfo.RedirectDiskDrives;
            dataRow["RedirectPorts"] = connectionInfo.RedirectPorts;
            dataRow["RedirectPrinters"] = connectionInfo.RedirectPrinters;
            dataRow["RedirectSmartCards"] = connectionInfo.RedirectSmartCards;
            dataRow["RedirectSound"] = connectionInfo.RedirectSound;
            dataRow["RedirectKeys"] = connectionInfo.RedirectKeys;
            dataRow["Connected"] = connectionInfo.OpenConnections.Count > 0 ? "true" : "false";
            dataRow["PreExtApp"] = connectionInfo.PreExtApp;
            dataRow["PostExtApp"] = connectionInfo.PostExtApp;
            dataRow["MacAddress"] = connectionInfo.MacAddress;
            dataRow["UserField"] = connectionInfo.UserField;
            dataRow["ExtApp"] = connectionInfo.ExtApp;
            dataRow["VNCCompression"] = connectionInfo.VNCCompression;
            dataRow["VNCEncoding"] = connectionInfo.VNCEncoding;
            dataRow["VNCAuthMode"] = connectionInfo.VNCAuthMode;
            dataRow["VNCProxyType"] = connectionInfo.VNCProxyType;
            dataRow["VNCProxyIP"] = connectionInfo.VNCProxyIP;
            dataRow["VNCProxyPort"] = connectionInfo.VNCProxyPort;
            dataRow["VNCProxyUsername"] = connectionInfo.VNCProxyUsername;
            dataRow["VNCProxyPassword"] = connectionInfo.VNCProxyPassword;
            dataRow["VNCColors"] = connectionInfo.VNCColors;
            dataRow["VNCSmartSizeMode"] = connectionInfo.VNCSmartSizeMode;
            dataRow["VNCViewOnly"] = connectionInfo.VNCViewOnly;
            dataRow["RDGatewayUsageMethod"] = connectionInfo.RDGatewayUsageMethod;
            dataRow["RDGatewayHostname"] = connectionInfo.RDGatewayHostname;
            dataRow["RDGatewayUseConnectionCredentials"] = connectionInfo.RDGatewayUseConnectionCredentials;
            dataRow["RDGatewayUsername"] = connectionInfo.RDGatewayUsername;
            dataRow["RDGatewayPassword"] = connectionInfo.RDGatewayPassword;
            dataRow["RDGatewayDomain"] = connectionInfo.RDGatewayDomain;
            if (_saveSecurity.Inheritance)
            {
                dataRow["InheritCacheBitmaps"] = connectionInfo.Inheritance.CacheBitmaps;
                dataRow["InheritColors"] = connectionInfo.Inheritance.Colors;
                dataRow["InheritDescription"] = connectionInfo.Inheritance.Description;
                dataRow["InheritDisplayThemes"] = connectionInfo.Inheritance.DisplayThemes;
                dataRow["InheritDisplayWallpaper"] = connectionInfo.Inheritance.DisplayWallpaper;
                dataRow["InheritEnableFontSmoothing"] = connectionInfo.Inheritance.EnableFontSmoothing;
                dataRow["InheritEnableDesktopComposition"] = connectionInfo.Inheritance.EnableDesktopComposition;
                dataRow["InheritDomain"] = connectionInfo.Inheritance.Domain;
                dataRow["InheritIcon"] = connectionInfo.Inheritance.Icon;
                dataRow["InheritPanel"] = connectionInfo.Inheritance.Panel;
                dataRow["InheritPassword"] = connectionInfo.Inheritance.Password;
                dataRow["InheritPort"] = connectionInfo.Inheritance.Port;
                dataRow["InheritProtocol"] = connectionInfo.Inheritance.Protocol;
                dataRow["InheritPuttySession"] = connectionInfo.Inheritance.PuttySession;
                dataRow["InheritRedirectDiskDrives"] = connectionInfo.Inheritance.RedirectDiskDrives;
                dataRow["InheritRedirectKeys"] = connectionInfo.Inheritance.RedirectKeys;
                dataRow["InheritRedirectPorts"] = connectionInfo.Inheritance.RedirectPorts;
                dataRow["InheritRedirectPrinters"] = connectionInfo.Inheritance.RedirectPrinters;
                dataRow["InheritRedirectSmartCards"] = connectionInfo.Inheritance.RedirectSmartCards;
                dataRow["InheritRedirectSound"] = connectionInfo.Inheritance.RedirectSound;
                dataRow["InheritResolution"] = connectionInfo.Inheritance.Resolution;
                dataRow["InheritAutomaticResize"] = connectionInfo.Inheritance.AutomaticResize;
                dataRow["InheritUseConsoleSession"] = connectionInfo.Inheritance.UseConsoleSession;
                dataRow["InheritUseCredSsp"] = connectionInfo.Inheritance.UseCredSsp;
                dataRow["InheritRenderingEngine"] = connectionInfo.Inheritance.RenderingEngine;
                dataRow["InheritUsername"] = connectionInfo.Inheritance.Username;
                dataRow["InheritICAEncryptionStrength"] = connectionInfo.Inheritance.ICAEncryptionStrength;
                dataRow["InheritRDPAuthenticationLevel"] = connectionInfo.Inheritance.RDPAuthenticationLevel;
                dataRow["InheritLoadBalanceInfo"] = connectionInfo.Inheritance.LoadBalanceInfo;
                dataRow["InheritPreExtApp"] = connectionInfo.Inheritance.PreExtApp;
                dataRow["InheritPostExtApp"] = connectionInfo.Inheritance.PostExtApp;
                dataRow["InheritMacAddress"] = connectionInfo.Inheritance.MacAddress;
                dataRow["InheritUserField"] = connectionInfo.Inheritance.UserField;
                dataRow["InheritExtApp"] = connectionInfo.Inheritance.ExtApp;
                dataRow["InheritVNCCompression"] = connectionInfo.Inheritance.VNCCompression;
                dataRow["InheritVNCEncoding"] = connectionInfo.Inheritance.VNCEncoding;
                dataRow["InheritVNCAuthMode"] = connectionInfo.Inheritance.VNCAuthMode;
                dataRow["InheritVNCProxyType"] = connectionInfo.Inheritance.VNCProxyType;
                dataRow["InheritVNCProxyIP"] = connectionInfo.Inheritance.VNCProxyIP;
                dataRow["InheritVNCProxyPort"] = connectionInfo.Inheritance.VNCProxyPort;
                dataRow["InheritVNCProxyUsername"] = connectionInfo.Inheritance.VNCProxyUsername;
                dataRow["InheritVNCProxyPassword"] = connectionInfo.Inheritance.VNCProxyPassword;
                dataRow["InheritVNCColors"] = connectionInfo.Inheritance.VNCColors;
                dataRow["InheritVNCSmartSizeMode"] = connectionInfo.Inheritance.VNCSmartSizeMode;
                dataRow["InheritVNCViewOnly"] = connectionInfo.Inheritance.VNCViewOnly;
                dataRow["InheritRDGatewayUsageMethod"] = connectionInfo.Inheritance.RDGatewayUsageMethod;
                dataRow["InheritRDGatewayHostname"] = connectionInfo.Inheritance.RDGatewayHostname;
                dataRow["InheritRDGatewayUseConnectionCredentials"] = connectionInfo.Inheritance.RDGatewayUseConnectionCredentials;
                dataRow["InheritRDGatewayUsername"] = connectionInfo.Inheritance.RDGatewayUsername;
                dataRow["InheritRDGatewayPassword"] = connectionInfo.Inheritance.RDGatewayPassword;
                dataRow["InheritRDGatewayDomain"] = connectionInfo.Inheritance.RDGatewayDomain;
            }
            else
            {
                dataRow["InheritCacheBitmaps"] = "";
                dataRow["InheritColors"] = "";
                dataRow["InheritDescription"] = "";
                dataRow["InheritDisplayThemes"] = "";
                dataRow["InheritDisplayWallpaper"] = "";
                dataRow["InheritEnableFontSmoothing"] = "";
                dataRow["InheritEnableDesktopComposition"] = "";
                dataRow["InheritDomain"] = "";
                dataRow["InheritIcon"] = "";
                dataRow["InheritPanel"] = "";
                dataRow["InheritPassword"] = "";
                dataRow["InheritPort"] = "";
                dataRow["InheritProtocol"] = "";
                dataRow["InheritPuttySession"] = "";
                dataRow["InheritRedirectDiskDrives"] = "";
                dataRow["InheritRedirectKeys"] = "";
                dataRow["InheritRedirectPorts"] = "";
                dataRow["InheritRedirectPrinters"] = "";
                dataRow["InheritRedirectSmartCards"] = "";
                dataRow["InheritRedirectSound"] = "";
                dataRow["InheritResolution"] = "";
                dataRow["InheritAutomaticResize"] = "";
                dataRow["InheritUseConsoleSession"] = "";
                dataRow["InheritUseCredSsp"] = "";
                dataRow["InheritRenderingEngine"] = "";
                dataRow["InheritUsername"] = "";
                dataRow["InheritICAEncryptionStrength"] = "";
                dataRow["InheritRDPAuthenticationLevel"] = "";
                dataRow["InheritLoadBalanceInfo"] = "";
                dataRow["InheritPreExtApp"] = "";
                dataRow["InheritPostExtApp"] = "";
                dataRow["InheritMacAddress"] = "";
                dataRow["InheritUserField"] = "";
                dataRow["InheritExtApp"] = "";
                dataRow["InheritVNCCompression"] = "";
                dataRow["InheritVNCEncoding"] = "";
                dataRow["InheritVNCAuthMode"] = "";
                dataRow["InheritVNCProxyType"] = "";
                dataRow["InheritVNCProxyIP"] = "";
                dataRow["InheritVNCProxyPort"] = "";
                dataRow["InheritVNCProxyUsername"] = "";
                dataRow["InheritVNCProxyPassword"] = "";
                dataRow["InheritVNCColors"] = "";
                dataRow["InheritVNCSmartSizeMode"] = "";
                dataRow["InheritVNCViewOnly"] = "";
                dataRow["InheritRDGatewayUsageMethod"] = "";
                dataRow["InheritRDGatewayHostname"] = "";
                dataRow["InheritRDGatewayUseConnectionCredentials"] = "";
                dataRow["InheritRDGatewayUsername"] = "";
                dataRow["InheritRDGatewayPassword"] = "";
                dataRow["InheritRDGatewayDomain"] = "";
            }
            _dataTable.Rows.Add(dataRow);
        }

        private string FormateDateForDatabase(DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd HH:mm:ss}";
        }
    }
}