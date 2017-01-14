using System;
using System.Data;
using System.Data.SqlTypes;
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
        private readonly SaveFilter _saveFilter;
        private int _currentNodeIndex;

        public DataTableSerializer(SaveFilter saveFilter)
        {
            _saveFilter = saveFilter;
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
            _dataTable.Columns.Add("ID", typeof(int));
            _dataTable.Columns[0].AutoIncrement = true;
            _dataTable.Columns.Add("ConstantID", typeof(string));
            _dataTable.Columns.Add("PositionID", typeof(int));
            _dataTable.Columns.Add("ParentID", typeof(string));
            _dataTable.Columns.Add("LastChange", typeof(SqlDateTime));
            _dataTable.Columns.Add("Name", typeof(string));
            _dataTable.Columns.Add("Type", typeof(string));
            _dataTable.Columns.Add("Expanded", typeof(bool));
            _dataTable.Columns.Add("Description", typeof(string));
            _dataTable.Columns.Add("Icon", typeof(string));
            _dataTable.Columns.Add("Panel", typeof(string));
            _dataTable.Columns.Add("Username", typeof(string));
            _dataTable.Columns.Add("DomainName", typeof(string));
            _dataTable.Columns.Add("Password", typeof(string));
            _dataTable.Columns.Add("Hostname", typeof(string));
            _dataTable.Columns.Add("Protocol", typeof(string));
            _dataTable.Columns.Add("PuttySession", typeof(string));
            _dataTable.Columns.Add("Port", typeof(int));
            _dataTable.Columns.Add("ConnectToConsole", typeof(bool));
            _dataTable.Columns.Add("UseCredSsp", typeof(bool));
            _dataTable.Columns.Add("RenderingEngine", typeof(string));
            _dataTable.Columns.Add("ICAEncryptionStrength", typeof(string));
            _dataTable.Columns.Add("RDPAuthenticationLevel", typeof(string));
            _dataTable.Columns.Add("RDPMinutesToIdleTimeout", typeof(int));
            _dataTable.Columns.Add("Colors", typeof(string));
            _dataTable.Columns.Add("Resolution", typeof(string));
            _dataTable.Columns.Add("DisplayWallpaper", typeof(bool));
            _dataTable.Columns.Add("DisplayThemes", typeof(bool));
            _dataTable.Columns.Add("EnableFontSmoothing", typeof(bool));
            _dataTable.Columns.Add("EnableDesktopComposition", typeof(bool));
            _dataTable.Columns.Add("CacheBitmaps", typeof(bool));
            _dataTable.Columns.Add("RedirectDiskDrives", typeof(bool));
            _dataTable.Columns.Add("RedirectPorts", typeof(bool));
            _dataTable.Columns.Add("RedirectPrinters", typeof(bool));
            _dataTable.Columns.Add("RedirectSmartCards", typeof(bool));
            _dataTable.Columns.Add("RedirectSound", typeof(string));
            _dataTable.Columns.Add("RedirectKeys", typeof(bool));
            _dataTable.Columns.Add("Connected", typeof(bool));
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
            _dataTable.Columns.Add("VNCViewOnly", typeof(bool));
            _dataTable.Columns.Add("RDGatewayUsageMethod", typeof(string));
            _dataTable.Columns.Add("RDGatewayHostname", typeof(string));
            _dataTable.Columns.Add("RDGatewayUseConnectionCredentials", typeof(string));
            _dataTable.Columns.Add("RDGatewayUsername", typeof(string));
            _dataTable.Columns.Add("RDGatewayPassword", typeof(string));
            _dataTable.Columns.Add("RDGatewayDomain", typeof(string));
            _dataTable.Columns.Add("InheritCacheBitmaps", typeof(bool));
            _dataTable.Columns.Add("InheritColors", typeof(bool));
            _dataTable.Columns.Add("InheritDescription", typeof(bool));
            _dataTable.Columns.Add("InheritDisplayThemes", typeof(bool));
            _dataTable.Columns.Add("InheritDisplayWallpaper", typeof(bool));
            _dataTable.Columns.Add("InheritEnableFontSmoothing", typeof(bool));
            _dataTable.Columns.Add("InheritEnableDesktopComposition", typeof(bool));
            _dataTable.Columns.Add("InheritDomain", typeof(bool));
            _dataTable.Columns.Add("InheritIcon", typeof(bool));
            _dataTable.Columns.Add("InheritPanel", typeof(bool));
            _dataTable.Columns.Add("InheritPassword", typeof(bool));
            _dataTable.Columns.Add("InheritPort", typeof(bool));
            _dataTable.Columns.Add("InheritProtocol", typeof(bool));
            _dataTable.Columns.Add("InheritPuttySession", typeof(bool));
            _dataTable.Columns.Add("InheritRedirectDiskDrives", typeof(bool));
            _dataTable.Columns.Add("InheritRedirectKeys", typeof(bool));
            _dataTable.Columns.Add("InheritRedirectPorts", typeof(bool));
            _dataTable.Columns.Add("InheritRedirectPrinters", typeof(bool));
            _dataTable.Columns.Add("InheritRedirectSmartCards", typeof(bool));
            _dataTable.Columns.Add("InheritRedirectSound", typeof(bool));
            _dataTable.Columns.Add("InheritSoundQuality", typeof(bool));
            _dataTable.Columns.Add("InheritResolution", typeof(bool));
            _dataTable.Columns.Add("InheritUseConsoleSession", typeof(bool));
            _dataTable.Columns.Add("InheritUseCredSsp", typeof(bool));
            _dataTable.Columns.Add("InheritRenderingEngine", typeof(bool));
            _dataTable.Columns.Add("InheritICAEncryptionStrength", typeof(bool));
            _dataTable.Columns.Add("InheritRDPAuthenticationLevel", typeof(bool));
            _dataTable.Columns.Add("InheritRDPMinutesToIdleTimeout", typeof(bool));
            _dataTable.Columns.Add("InheritUsername", typeof(bool));
            _dataTable.Columns.Add("InheritPreExtApp", typeof(bool));
            _dataTable.Columns.Add("InheritPostExtApp", typeof(bool));
            _dataTable.Columns.Add("InheritMacAddress", typeof(bool));
            _dataTable.Columns.Add("InheritUserField", typeof(bool));
            _dataTable.Columns.Add("InheritExtApp", typeof(bool));
            _dataTable.Columns.Add("InheritVNCCompression", typeof(bool));
            _dataTable.Columns.Add("InheritVNCEncoding", typeof(bool));
            _dataTable.Columns.Add("InheritVNCAuthMode", typeof(bool));
            _dataTable.Columns.Add("InheritVNCProxyType", typeof(bool));
            _dataTable.Columns.Add("InheritVNCProxyIP", typeof(bool));
            _dataTable.Columns.Add("InheritVNCProxyPort", typeof(bool));
            _dataTable.Columns.Add("InheritVNCProxyUsername", typeof(bool));
            _dataTable.Columns.Add("InheritVNCProxyPassword", typeof(bool));
            _dataTable.Columns.Add("InheritVNCColors", typeof(bool));
            _dataTable.Columns.Add("InheritVNCSmartSizeMode", typeof(bool));
            _dataTable.Columns.Add("InheritVNCViewOnly", typeof(bool));
            _dataTable.Columns.Add("InheritRDGatewayUsageMethod", typeof(bool));
            _dataTable.Columns.Add("InheritRDGatewayHostname", typeof(bool));
            _dataTable.Columns.Add("InheritRDGatewayUseConnectionCredentials", typeof(bool));
            _dataTable.Columns.Add("InheritRDGatewayUsername", typeof(bool));
            _dataTable.Columns.Add("InheritRDGatewayPassword", typeof(bool));
            _dataTable.Columns.Add("InheritRDGatewayDomain", typeof(bool));
            _dataTable.Columns.Add("LoadBalanceInfo", typeof(string));
            _dataTable.Columns.Add("AutomaticResize", typeof(bool));
            _dataTable.Columns.Add("InheritLoadBalanceInfo", typeof(bool));
            _dataTable.Columns.Add("InheritAutomaticResize", typeof(bool));
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
            dataRow["ID"] = DBNull.Value;
            dataRow["Name"] = connectionInfo.Name;
            dataRow["Type"] = connectionInfo.GetTreeNodeType().ToString();
            dataRow["ConstantID"] = connectionInfo.ConstantID;
            dataRow["ParentID"] = connectionInfo.Parent.ConstantID;
            dataRow["PositionID"] = _currentNodeIndex;
            dataRow["LastChange"] = (SqlDateTime)DateTime.Now;
            var info = connectionInfo as ContainerInfo;
            dataRow["Expanded"] = info != null && info.IsExpanded;
            dataRow["Description"] = connectionInfo.Description;
            dataRow["Icon"] = connectionInfo.Icon;
            dataRow["Panel"] = connectionInfo.Panel;
            dataRow["Username"] = _saveFilter.SaveUsername ? connectionInfo.Username : "";
            dataRow["DomainName"] = _saveFilter.SaveDomain ? connectionInfo.Domain : "";
            dataRow["Password"] = _saveFilter.SavePassword ? connectionInfo.Password : "";
            dataRow["Hostname"] = connectionInfo.Hostname;
            dataRow["Protocol"] = connectionInfo.Protocol;
            dataRow["PuttySession"] = connectionInfo.PuttySession;
            dataRow["Port"] = connectionInfo.Port;
            dataRow["ConnectToConsole"] = connectionInfo.UseConsoleSession;
            dataRow["UseCredSsp"] = connectionInfo.UseCredSsp;
            dataRow["RenderingEngine"] = connectionInfo.RenderingEngine;
            dataRow["ICAEncryptionStrength"] = connectionInfo.ICAEncryptionStrength;
            dataRow["RDPAuthenticationLevel"] = connectionInfo.RDPAuthenticationLevel;
            //dataRow["RDPMinutesToIdleTimeout"] = connectionInfo.RDPMinutesToIdleTimeout;
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
            //dataRow["SoundQuality"] = connectionInfo.SoundQuality;
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
            if (_saveFilter.SaveInheritance)
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
                //dataRow["InheritSoundQuality"] = connectionInfo.Inheritance.SoundQuality;
                dataRow["InheritResolution"] = connectionInfo.Inheritance.Resolution;
                dataRow["InheritAutomaticResize"] = connectionInfo.Inheritance.AutomaticResize;
                dataRow["InheritUseConsoleSession"] = connectionInfo.Inheritance.UseConsoleSession;
                dataRow["InheritUseCredSsp"] = connectionInfo.Inheritance.UseCredSsp;
                dataRow["InheritRenderingEngine"] = connectionInfo.Inheritance.RenderingEngine;
                dataRow["InheritUsername"] = connectionInfo.Inheritance.Username;
                dataRow["InheritICAEncryptionStrength"] = connectionInfo.Inheritance.ICAEncryptionStrength;
                dataRow["InheritRDPAuthenticationLevel"] = connectionInfo.Inheritance.RDPAuthenticationLevel;
                //dataRow["InheritRDPMinutesToIdleTimeout"] = connectionInfo.Inheritance.RDPMinutesToIdleTimeout;
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
                //dataRow["InheritSoundQuality"] = false;
                dataRow["InheritResolution"] = false;
                dataRow["InheritAutomaticResize"] = false;
                dataRow["InheritUseConsoleSession"] = false;
                dataRow["InheritUseCredSsp"] = false;
                dataRow["InheritRenderingEngine"] = false;
                dataRow["InheritUsername"] = false;
                dataRow["InheritICAEncryptionStrength"] = false;
                dataRow["InheritRDPAuthenticationLevel"] = false;
                //dataRow["InheritRDPMinutesToIdleTimeout"] = false;
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
    }
}