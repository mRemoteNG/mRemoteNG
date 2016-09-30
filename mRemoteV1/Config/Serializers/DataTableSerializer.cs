using System;
using System.Data;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers
{
    public class DataTableSerializer : ISerializer<DataTable>
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
            return Serialize(rootNode);
        }

        public DataTable Serialize(ConnectionInfo serializationTarget)
        {
            _dataTable = new DataTable(TableName);
            CreateSchema();
            SetPrimaryKey();
            _currentNodeIndex = 0;
            SerializeNodesRecursive(serializationTarget);
            return _dataTable;
        }

        private void CreateSchema()
        {
            _dataTable.Columns.Add("Name", typeof(string));
            _dataTable.Columns.Add("Type", typeof(string));
            _dataTable.Columns.Add("ConstantID", typeof(string));
            _dataTable.Columns.Add("ParentID", typeof(string));
            _dataTable.Columns.Add("PositionID", typeof(int));
            _dataTable.Columns.Add("LastChange", typeof(DateTime));
            _dataTable.Columns.Add("Expanded", typeof(int));
            _dataTable.Columns.Add("Description", typeof(string));
            _dataTable.Columns.Add("Icon", typeof(string));
            _dataTable.Columns.Add("Panel", typeof(string));
            _dataTable.Columns.Add("Username", typeof(string));
            _dataTable.Columns.Add("Domain", typeof(string));
            _dataTable.Columns.Add("Password", typeof(string));
            _dataTable.Columns.Add("Hostname", typeof(string));
            _dataTable.Columns.Add("Protocol", typeof(string));
            _dataTable.Columns.Add("PuttySession", typeof(string));
            _dataTable.Columns.Add("Port", typeof(int));
            _dataTable.Columns.Add("ConnectToConsole", typeof(int));
            _dataTable.Columns.Add("UseCredSsp", typeof(int));
            _dataTable.Columns.Add("RenderingEngine", typeof(string));
            _dataTable.Columns.Add("ICAEncryptionStrength", typeof(string));
            _dataTable.Columns.Add("RDPAuthenticationLevel", typeof(string));
            _dataTable.Columns.Add("LoadBalanceInfo", typeof(string));
            _dataTable.Columns.Add("Colors", typeof(string));
            _dataTable.Columns.Add("Resolution", typeof(string));
            _dataTable.Columns.Add("AutomaticResize", typeof(int));
            _dataTable.Columns.Add("DisplayWallpaper", typeof(int));
            _dataTable.Columns.Add("DisplayThemes", typeof(int));
            _dataTable.Columns.Add("EnableFontSmoothing", typeof(int));
            _dataTable.Columns.Add("EnableDesktopComposition", typeof(int));
            _dataTable.Columns.Add("CacheBitmaps", typeof(int));
            _dataTable.Columns.Add("RedirectDiskDrives", typeof(int));
            _dataTable.Columns.Add("RedirectPorts", typeof(int));
            _dataTable.Columns.Add("RedirectPrinters", typeof(int));
            _dataTable.Columns.Add("RedirectSmartCards", typeof(int));
            _dataTable.Columns.Add("RedirectSound", typeof(int));
            _dataTable.Columns.Add("RedirectKeys", typeof(int));
            _dataTable.Columns.Add("Connected", typeof(int));
            _dataTable.Columns.Add("PreExtApp", typeof(string));
            _dataTable.Columns.Add("PostExtApp", typeof(string));
            _dataTable.Columns.Add("MacAddress", typeof(string));
            _dataTable.Columns.Add("UserField", typeof(string));
            _dataTable.Columns.Add("ExtApp", typeof(string));
            _dataTable.Columns.Add("VNCCompression", typeof(string));
            _dataTable.Columns.Add("VNCEncoding", typeof(string));
            _dataTable.Columns.Add("VNCAuthMode", typeof(string));
            _dataTable.Columns.Add("VNCProxyType", typeof(string));
            _dataTable.Columns.Add("VNCProxyIP", typeof(string));
            _dataTable.Columns.Add("VNCProxyPort", typeof(int));
            _dataTable.Columns.Add("VNCProxyUsername", typeof(string));
            _dataTable.Columns.Add("VNCProxyPassword", typeof(string));
            _dataTable.Columns.Add("VNCColors", typeof(string));
            _dataTable.Columns.Add("VNCSmartSizeMode", typeof(string));
            _dataTable.Columns.Add("VNCViewOnly", typeof(int));
            _dataTable.Columns.Add("RDGatewayUsageMethod", typeof(string));
            _dataTable.Columns.Add("RDGatewayHostname", typeof(string));
            _dataTable.Columns.Add("RDGatewayUseConnectionCredentials");
            _dataTable.Columns.Add("RDGatewayUsername", typeof(string));
            _dataTable.Columns.Add("RDGatewayPassword", typeof(string));
            _dataTable.Columns.Add("RDGatewayDomain", typeof(string));
            _dataTable.Columns.Add("InheritCacheBitmaps", typeof(int));
            _dataTable.Columns.Add("InheritColors", typeof(int));
            _dataTable.Columns.Add("InheritDescription", typeof(int));
            _dataTable.Columns.Add("InheritDisplayThemes", typeof(int));
            _dataTable.Columns.Add("InheritDisplayWallpaper", typeof(int));
            _dataTable.Columns.Add("InheritEnableFontSmoothing", typeof(int));
            _dataTable.Columns.Add("InheritEnableDesktopComposition", typeof(int));
            _dataTable.Columns.Add("InheritDomain", typeof(int));
            _dataTable.Columns.Add("InheritIcon", typeof(int));
            _dataTable.Columns.Add("InheritPanel", typeof(int));
            _dataTable.Columns.Add("InheritPassword", typeof(int));
            _dataTable.Columns.Add("InheritPort", typeof(int));
            _dataTable.Columns.Add("InheritProtocol", typeof(int));
            _dataTable.Columns.Add("InheritPuttySession", typeof(int));
            _dataTable.Columns.Add("InheritRedirectDiskDrives", typeof(int));
            _dataTable.Columns.Add("InheritRedirectKeys", typeof(int));
            _dataTable.Columns.Add("InheritRedirectPorts", typeof(int));
            _dataTable.Columns.Add("InheritRedirectPrinters", typeof(int));
            _dataTable.Columns.Add("InheritRedirectSmartCards", typeof(int));
            _dataTable.Columns.Add("InheritRedirectSound", typeof(int));
            _dataTable.Columns.Add("InheritResolution", typeof(int));
            _dataTable.Columns.Add("InheritAutomaticResize", typeof(int));
            _dataTable.Columns.Add("InheritUseConsoleSession", typeof(int));
            _dataTable.Columns.Add("InheritUseCredSsp", typeof(int));
            _dataTable.Columns.Add("InheritRenderingEngine", typeof(int));
            _dataTable.Columns.Add("InheritUsername", typeof(int));
            _dataTable.Columns.Add("InheritICAEncryptionStrength", typeof(int));
            _dataTable.Columns.Add("InheritRDPAuthenticationLevel", typeof(int));
            _dataTable.Columns.Add("InheritLoadBalanceInfo", typeof(int));
            _dataTable.Columns.Add("InheritPreExtApp", typeof(int));
            _dataTable.Columns.Add("InheritPostExtApp", typeof(int));
            _dataTable.Columns.Add("InheritMacAddress", typeof(int));
            _dataTable.Columns.Add("InheritUserField", typeof(int));
            _dataTable.Columns.Add("InheritExtApp", typeof(int));
            _dataTable.Columns.Add("InheritVNCCompression", typeof(int));
            _dataTable.Columns.Add("InheritVNCEncoding", typeof(int));
            _dataTable.Columns.Add("InheritVNCAuthMode", typeof(int));
            _dataTable.Columns.Add("InheritVNCProxyType", typeof(int));
            _dataTable.Columns.Add("InheritVNCProxyIP", typeof(int));
            _dataTable.Columns.Add("InheritVNCProxyPort", typeof(int));
            _dataTable.Columns.Add("InheritVNCProxyUsername", typeof(int));
            _dataTable.Columns.Add("InheritVNCProxyPassword", typeof(int));
            _dataTable.Columns.Add("InheritVNCColors", typeof(int));
            _dataTable.Columns.Add("InheritVNCSmartSizeMode", typeof(int));
            _dataTable.Columns.Add("InheritVNCViewOnly", typeof(int));
            _dataTable.Columns.Add("InheritRDGatewayUsageMethod", typeof(int));
            _dataTable.Columns.Add("InheritRDGatewayHostname", typeof(int));
            _dataTable.Columns.Add("InheritRDGatewayUseConnectionCredentials", typeof(int));
            _dataTable.Columns.Add("InheritRDGatewayUsername", typeof(int));
            _dataTable.Columns.Add("InheritRDGatewayPassword", typeof(int));
            _dataTable.Columns.Add("InheritRDGatewayDomain", typeof(int));
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
            dataRow["Type"] = connectionInfo.GetTreeNodeType().ToString();
            dataRow["ConstantID"] = connectionInfo.ConstantID;
            dataRow["ParentID"] = connectionInfo.Parent.ConstantID;
            dataRow["PositionID"] = _currentNodeIndex;
            dataRow["LastChange"] = FormateDateForDatabase(DateTime.Now);
            dataRow["Expanded"] = connectionInfo is ContainerInfo ? ((ContainerInfo)connectionInfo).IsExpanded : false;
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
            dataRow["Connected"] = connectionInfo.OpenConnections.Count > 0;
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
                dataRow["InheritCacheBitmaps"] = false;
                dataRow["InheritColors"] = false;
                dataRow["InheritDescription"] = false;
                dataRow["InheritDisplayThemes"] = false;
                dataRow["InheritDisplayWallpaper"] = false;
                dataRow["InheritEnableFontSmoothing"] = false;
                dataRow["InheritEnableDesktopComposition"] = false;
                dataRow["InheritDomain"] = false;
                dataRow["InheritIcon"] = false;
                dataRow["InheritPanel"] = false;
                dataRow["InheritPassword"] = false;
                dataRow["InheritPort"] = false;
                dataRow["InheritProtocol"] = false;
                dataRow["InheritPuttySession"] = false;
                dataRow["InheritRedirectDiskDrives"] = false;
                dataRow["InheritRedirectKeys"] = false;
                dataRow["InheritRedirectPorts"] = false;
                dataRow["InheritRedirectPrinters"] = false;
                dataRow["InheritRedirectSmartCards"] = false;
                dataRow["InheritRedirectSound"] = false;
                dataRow["InheritResolution"] = false;
                dataRow["InheritAutomaticResize"] = false;
                dataRow["InheritUseConsoleSession"] = false;
                dataRow["InheritUseCredSsp"] = false;
                dataRow["InheritRenderingEngine"] = false;
                dataRow["InheritUsername"] = false;
                dataRow["InheritICAEncryptionStrength"] = false;
                dataRow["InheritRDPAuthenticationLevel"] = false;
                dataRow["InheritLoadBalanceInfo"] = false;
                dataRow["InheritPreExtApp"] = false;
                dataRow["InheritPostExtApp"] = false;
                dataRow["InheritMacAddress"] = false;
                dataRow["InheritUserField"] = false;
                dataRow["InheritExtApp"] = false;
                dataRow["InheritVNCCompression"] = false;
                dataRow["InheritVNCEncoding"] = false;
                dataRow["InheritVNCAuthMode"] = false;
                dataRow["InheritVNCProxyType"] = false;
                dataRow["InheritVNCProxyIP"] = false;
                dataRow["InheritVNCProxyPort"] = false;
                dataRow["InheritVNCProxyUsername"] = false;
                dataRow["InheritVNCProxyPassword"] = false;
                dataRow["InheritVNCColors"] = false;
                dataRow["InheritVNCSmartSizeMode"] = false;
                dataRow["InheritVNCViewOnly"] = false;
                dataRow["InheritRDGatewayUsageMethod"] = false;
                dataRow["InheritRDGatewayHostname"] = false;
                dataRow["InheritRDGatewayUseConnectionCredentials"] = false;
                dataRow["InheritRDGatewayUsername"] = false;
                dataRow["InheritRDGatewayPassword"] = false;
                dataRow["InheritRDGatewayDomain"] = false;
            }
            _dataTable.Rows.Add(dataRow);
        }

        private string FormateDateForDatabase(DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd HH:mm:ss}";
        }
    }
}