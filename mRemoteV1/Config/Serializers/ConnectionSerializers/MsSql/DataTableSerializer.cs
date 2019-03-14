using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Security;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Serializers.MsSql
{
    public class DataTableSerializer : ISerializer<ConnectionInfo,DataTable>
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _encryptionKey;
        private DataTable _dataTable;
        private const string TableName = "tblCons";
        private readonly SaveFilter _saveFilter;
        private int _currentNodeIndex;

        public DataTableSerializer(SaveFilter saveFilter, ICryptographyProvider cryptographyProvider, SecureString encryptionKey)
        {
            _saveFilter = saveFilter.ThrowIfNull(nameof(saveFilter));
            _cryptographyProvider = cryptographyProvider.ThrowIfNull(nameof(cryptographyProvider));
            _encryptionKey = encryptionKey.ThrowIfNull(nameof(encryptionKey));
        }


        public DataTable Serialize(ConnectionTreeModel connectionTreeModel)
        {
            try
            {
                _dataTable = BuildTable();
                _currentNodeIndex = 0;
                var rootNode = connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
                return Serialize(rootNode);
            }
            catch
            {
                return _dataTable;
            }
        }

        public DataTable Serialize(ConnectionInfo serializationTarget)
        {
            _dataTable = BuildTable();
            _currentNodeIndex = 0;
            SerializeNodesRecursive(serializationTarget);
            return _dataTable;
        }

        private DataTable BuildTable()
        {
            var dataTable = new DataTable(TableName);
            CreateSchema(dataTable);
            SetPrimaryKey(dataTable);
            return dataTable;
        }

        private void CreateSchema(DataTable dataTable)
        {
            // Note: these columns must be defined in the same order that they exist in the DB
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns[0].AutoIncrement = true;
            dataTable.Columns.Add("ConstantID", typeof(string));
            dataTable.Columns.Add("PositionID", typeof(int));
            dataTable.Columns.Add("ParentID", typeof(string));
            dataTable.Columns.Add("LastChange", typeof(SqlDateTime));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Type", typeof(string));
            dataTable.Columns.Add("Expanded", typeof(bool));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Icon", typeof(string));
            dataTable.Columns.Add("Panel", typeof(string));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("DomainName", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("Hostname", typeof(string));
            dataTable.Columns.Add("Protocol", typeof(string));
            dataTable.Columns.Add("PuttySession", typeof(string));
            dataTable.Columns.Add("Port", typeof(int));
            dataTable.Columns.Add("ConnectToConsole", typeof(bool));
            dataTable.Columns.Add("UseCredSsp", typeof(bool));
            dataTable.Columns.Add("RenderingEngine", typeof(string));
            dataTable.Columns.Add("ICAEncryptionStrength", typeof(string));
            dataTable.Columns.Add("RDPAuthenticationLevel", typeof(string));
            dataTable.Columns.Add("Colors", typeof(string));
            dataTable.Columns.Add("Resolution", typeof(string));
            dataTable.Columns.Add("DisplayWallpaper", typeof(bool));
            dataTable.Columns.Add("DisplayThemes", typeof(bool));
            dataTable.Columns.Add("EnableFontSmoothing", typeof(bool));
            dataTable.Columns.Add("EnableDesktopComposition", typeof(bool));
            dataTable.Columns.Add("CacheBitmaps", typeof(bool));
            dataTable.Columns.Add("RedirectDiskDrives", typeof(bool));
            dataTable.Columns.Add("RedirectPorts", typeof(bool));
            dataTable.Columns.Add("RedirectPrinters", typeof(bool));
            dataTable.Columns.Add("RedirectSmartCards", typeof(bool));
            dataTable.Columns.Add("RedirectSound", typeof(string));
            dataTable.Columns.Add("RedirectKeys", typeof(bool));
            dataTable.Columns.Add("Connected", typeof(bool));
            dataTable.Columns.Add("PreExtApp", typeof(string));
            dataTable.Columns.Add("PostExtApp", typeof(string));
            dataTable.Columns.Add("MacAddress", typeof(string));
            dataTable.Columns.Add("UserField", typeof(string));
            dataTable.Columns.Add("ExtApp", typeof(string));
            dataTable.Columns.Add("VNCCompression", typeof(string));
            dataTable.Columns.Add("VNCEncoding", typeof(string));
            dataTable.Columns.Add("VNCAuthMode", typeof(string));
            dataTable.Columns.Add("VNCProxyType", typeof(string));
            dataTable.Columns.Add("VNCProxyIP", typeof(string));
            dataTable.Columns.Add("VNCProxyPort", typeof(int));
            dataTable.Columns.Add("VNCProxyUsername", typeof(string));
            dataTable.Columns.Add("VNCProxyPassword", typeof(string));
            dataTable.Columns.Add("VNCColors", typeof(string));
            dataTable.Columns.Add("VNCSmartSizeMode", typeof(string));
            dataTable.Columns.Add("VNCViewOnly", typeof(bool));
            dataTable.Columns.Add("RDGatewayUsageMethod", typeof(string));
            dataTable.Columns.Add("RDGatewayHostname", typeof(string));
            dataTable.Columns.Add("RDGatewayUseConnectionCredentials", typeof(string));
            dataTable.Columns.Add("RDGatewayUsername", typeof(string));
            dataTable.Columns.Add("RDGatewayPassword", typeof(string));
            dataTable.Columns.Add("RDGatewayDomain", typeof(string));
            dataTable.Columns.Add("InheritCacheBitmaps", typeof(bool));
            dataTable.Columns.Add("InheritColors", typeof(bool));
            dataTable.Columns.Add("InheritDescription", typeof(bool));
            dataTable.Columns.Add("InheritDisplayThemes", typeof(bool));
            dataTable.Columns.Add("InheritDisplayWallpaper", typeof(bool));
            dataTable.Columns.Add("InheritEnableFontSmoothing", typeof(bool));
            dataTable.Columns.Add("InheritEnableDesktopComposition", typeof(bool));
            dataTable.Columns.Add("InheritDomain", typeof(bool));
            dataTable.Columns.Add("InheritIcon", typeof(bool));
            dataTable.Columns.Add("InheritPanel", typeof(bool));
            dataTable.Columns.Add("InheritPassword", typeof(bool));
            dataTable.Columns.Add("InheritPort", typeof(bool));
            dataTable.Columns.Add("InheritProtocol", typeof(bool));
            dataTable.Columns.Add("InheritPuttySession", typeof(bool));
            dataTable.Columns.Add("InheritRedirectDiskDrives", typeof(bool));
            dataTable.Columns.Add("InheritRedirectKeys", typeof(bool));
            dataTable.Columns.Add("InheritRedirectPorts", typeof(bool));
            dataTable.Columns.Add("InheritRedirectPrinters", typeof(bool));
            dataTable.Columns.Add("InheritRedirectSmartCards", typeof(bool));
            dataTable.Columns.Add("InheritRedirectSound", typeof(bool));
            dataTable.Columns.Add("InheritResolution", typeof(bool));
            dataTable.Columns.Add("InheritUseConsoleSession", typeof(bool));
            dataTable.Columns.Add("InheritUseCredSsp", typeof(bool));
            dataTable.Columns.Add("InheritRenderingEngine", typeof(bool));
            dataTable.Columns.Add("InheritICAEncryptionStrength", typeof(bool));
            dataTable.Columns.Add("InheritRDPAuthenticationLevel", typeof(bool));
            dataTable.Columns.Add("InheritUsername", typeof(bool));
            dataTable.Columns.Add("InheritPreExtApp", typeof(bool));
            dataTable.Columns.Add("InheritPostExtApp", typeof(bool));
            dataTable.Columns.Add("InheritMacAddress", typeof(bool));
            dataTable.Columns.Add("InheritUserField", typeof(bool));
            dataTable.Columns.Add("InheritExtApp", typeof(bool));
            dataTable.Columns.Add("InheritVNCCompression", typeof(bool));
            dataTable.Columns.Add("InheritVNCEncoding", typeof(bool));
            dataTable.Columns.Add("InheritVNCAuthMode", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyType", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyIP", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyPort", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyUsername", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyPassword", typeof(bool));
            dataTable.Columns.Add("InheritVNCColors", typeof(bool));
            dataTable.Columns.Add("InheritVNCSmartSizeMode", typeof(bool));
            dataTable.Columns.Add("InheritVNCViewOnly", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayUsageMethod", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayHostname", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayUseConnectionCredentials", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayUsername", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayPassword", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayDomain", typeof(bool));
            dataTable.Columns.Add("LoadBalanceInfo", typeof(string));
            dataTable.Columns.Add("AutomaticResize", typeof(bool));
            dataTable.Columns.Add("InheritLoadBalanceInfo", typeof(bool));
            dataTable.Columns.Add("InheritAutomaticResize", typeof(bool));
            dataTable.Columns.Add("RDPMinutesToIdleTimeout", typeof(int));
            dataTable.Columns.Add("RDPAlertIdleTimeout", typeof(bool));
            dataTable.Columns.Add("SoundQuality", typeof(string));
            dataTable.Columns.Add("InheritRDPMinutesToIdleTimeout", typeof(bool));
            dataTable.Columns.Add("InheritRDPAlertIdleTimeout", typeof(bool));
            dataTable.Columns.Add("InheritSoundQuality", typeof(bool));
        }

        private void SetPrimaryKey(DataTable dataTable)
        {
            dataTable.PrimaryKey = new[] { dataTable.Columns["ConstantID"] };
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
            dataRow["ParentID"] = connectionInfo.Parent?.ConstantID ?? "";
            dataRow["PositionID"] = _currentNodeIndex;
            dataRow["LastChange"] = (SqlDateTime)DateTime.Now;
            dataRow["Expanded"] = false; // TODO: this column can eventually be removed. we now save this property locally
            dataRow["Description"] = connectionInfo.Description;
            dataRow["Icon"] = connectionInfo.Icon;
            dataRow["Panel"] = connectionInfo.Panel;
            dataRow["Username"] = _saveFilter.SaveUsername ? connectionInfo.Username : "";
            dataRow["DomainName"] = _saveFilter.SaveDomain ? connectionInfo.Domain : "";
            dataRow["Password"] = _saveFilter.SavePassword 
                ? _cryptographyProvider.Encrypt(connectionInfo.Password, _encryptionKey) 
                : "";
            dataRow["Hostname"] = connectionInfo.Hostname;
            dataRow["Protocol"] = connectionInfo.Protocol;
            dataRow["PuttySession"] = connectionInfo.PuttySession;
            dataRow["Port"] = connectionInfo.Port;
            dataRow["ConnectToConsole"] = connectionInfo.UseConsoleSession;
            dataRow["UseCredSsp"] = connectionInfo.UseCredSsp;
            dataRow["RenderingEngine"] = connectionInfo.RenderingEngine;
            dataRow["ICAEncryptionStrength"] = connectionInfo.ICAEncryptionStrength;
            dataRow["RDPAuthenticationLevel"] = connectionInfo.RDPAuthenticationLevel;
            dataRow["RDPMinutesToIdleTimeout"] = connectionInfo.RDPMinutesToIdleTimeout;
            dataRow["RDPAlertIdleTimeout"] = connectionInfo.RDPAlertIdleTimeout;
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
            dataRow["SoundQuality"] = connectionInfo.SoundQuality;
            dataRow["RedirectKeys"] = connectionInfo.RedirectKeys;
            dataRow["Connected"] = false; // TODO: this column can eventually be removed. we now save this property locally
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
            dataRow["VNCProxyPassword"] = _cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword, _encryptionKey);
            dataRow["VNCColors"] = connectionInfo.VNCColors;
            dataRow["VNCSmartSizeMode"] = connectionInfo.VNCSmartSizeMode;
            dataRow["VNCViewOnly"] = connectionInfo.VNCViewOnly;
            dataRow["RDGatewayUsageMethod"] = connectionInfo.RDGatewayUsageMethod;
            dataRow["RDGatewayHostname"] = connectionInfo.RDGatewayHostname;
            dataRow["RDGatewayUseConnectionCredentials"] = connectionInfo.RDGatewayUseConnectionCredentials;
            dataRow["RDGatewayUsername"] = connectionInfo.RDGatewayUsername;
            dataRow["RDGatewayPassword"] = _cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword, _encryptionKey);
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
                dataRow["InheritSoundQuality"] = connectionInfo.Inheritance.SoundQuality;
                dataRow["InheritResolution"] = connectionInfo.Inheritance.Resolution;
                dataRow["InheritAutomaticResize"] = connectionInfo.Inheritance.AutomaticResize;
                dataRow["InheritUseConsoleSession"] = connectionInfo.Inheritance.UseConsoleSession;
                dataRow["InheritUseCredSsp"] = connectionInfo.Inheritance.UseCredSsp;
                dataRow["InheritRenderingEngine"] = connectionInfo.Inheritance.RenderingEngine;
                dataRow["InheritUsername"] = connectionInfo.Inheritance.Username;
                dataRow["InheritICAEncryptionStrength"] = connectionInfo.Inheritance.ICAEncryptionStrength;
                dataRow["InheritRDPAuthenticationLevel"] = connectionInfo.Inheritance.RDPAuthenticationLevel;
                dataRow["InheritRDPMinutesToIdleTimeout"] = connectionInfo.Inheritance.RDPMinutesToIdleTimeout;
                dataRow["InheritRDPAlertIdleTimeout"] = connectionInfo.Inheritance.RDPAlertIdleTimeout;
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
                dataRow["InheritSoundQuality"] = false;
                dataRow["InheritResolution"] = false;
                dataRow["InheritAutomaticResize"] = false;
                dataRow["InheritUseConsoleSession"] = false;
                dataRow["InheritUseCredSsp"] = false;
                dataRow["InheritRenderingEngine"] = false;
                dataRow["InheritUsername"] = false;
                dataRow["InheritICAEncryptionStrength"] = false;
                dataRow["InheritRDPAuthenticationLevel"] = false;
                dataRow["InheritRDPMinutesToIdleTimeout"] = false;
                dataRow["InheritRDPAlertIdleTimeout"] = false;
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