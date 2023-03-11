using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.MsSql
{
    [SupportedOSPlatform("windows")]
    public class DataTableSerializer : ISerializer<ConnectionInfo, DataTable>
    {
        public readonly int DELETE = 0;
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _encryptionKey;
        private DataTable _dataTable;
        private DataTable _sourceDataTable;
        private Dictionary<string, int> sourcePrimaryKeyDict = new Dictionary<string, int>();
        private const string TableName = "tblCons";
        private readonly SaveFilter _saveFilter;
        private int _currentNodeIndex;

        public Version Version { get; } = new Version(2, 8);

        public DataTableSerializer(SaveFilter saveFilter,
                                   ICryptographyProvider cryptographyProvider,
                                   SecureString encryptionKey)
        {
            _saveFilter = saveFilter.ThrowIfNull(nameof(saveFilter));
            _cryptographyProvider = cryptographyProvider.ThrowIfNull(nameof(cryptographyProvider));
            _encryptionKey = encryptionKey.ThrowIfNull(nameof(encryptionKey));
        }

        public void SetSourceDataTable(DataTable sourceDataTable)
        {
            _sourceDataTable = sourceDataTable;
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
            // Register add or update row
            SerializeNodesRecursive(serializationTarget);
            var entryToDelete = sourcePrimaryKeyDict.Keys.ToList();
            foreach( var entry in entryToDelete)
            {
                _dataTable.Rows.Find(entry).Delete();
            }
            return _dataTable;
        }

        private DataTable BuildTable()
        {
            DataTable dataTable;
            if (_sourceDataTable != null)
            {
                dataTable = _sourceDataTable;
            }else
            {
                dataTable = new DataTable(TableName);

            }
            if (dataTable.Columns.Count == 0) CreateSchema(dataTable);
            if (dataTable.PrimaryKey.Length == 0 ) SetPrimaryKey(dataTable);
            foreach(DataRow row in dataTable.Rows)
            {
                sourcePrimaryKeyDict.Add((string)row["ConstantID"], DELETE);
            }
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
            dataTable.Columns.Add("LastChange", MiscTools.DBTimeStampType());
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Type", typeof(string));
            dataTable.Columns.Add("Expanded", typeof(bool));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Icon", typeof(string));
            dataTable.Columns.Add("Panel", typeof(string));
            dataTable.Columns.Add("ExternalCredentialProvider", typeof(string));
            dataTable.Columns.Add("UserViaAPI", typeof(string));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("Domain", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("Hostname", typeof(string));
            dataTable.Columns.Add("ExternalAddressProvider", typeof(string));
            dataTable.Columns.Add("EC2Region", typeof(string));
            dataTable.Columns.Add("EC2InstanceId", typeof(string));
            dataTable.Columns.Add("Port", typeof(int));
            dataTable.Columns.Add("Protocol", typeof(string));
            dataTable.Columns.Add("SSHTunnelConnectionName", typeof(string));
            dataTable.Columns.Add("SSHOptions", typeof(string));
            dataTable.Columns.Add("PuttySession", typeof(string));
            dataTable.Columns.Add("ConnectToConsole", typeof(bool));
            dataTable.Columns.Add("UseCredSsp", typeof(bool));
            dataTable.Columns.Add("UseRestrictedAdmin", typeof(bool));
            dataTable.Columns.Add("UseRCG", typeof(bool));
            dataTable.Columns.Add("RenderingEngine", typeof(string));
            dataTable.Columns.Add("RDPAuthenticationLevel", typeof(string));
            dataTable.Columns.Add("Colors", typeof(string));
            dataTable.Columns.Add("Resolution", typeof(string));
            dataTable.Columns.Add("DisplayWallpaper", typeof(bool));
            dataTable.Columns.Add("DisplayThemes", typeof(bool));
            dataTable.Columns.Add("EnableFontSmoothing", typeof(bool));
            dataTable.Columns.Add("EnableDesktopComposition", typeof(bool));
            dataTable.Columns.Add("DisableFullWindowDrag", typeof(bool));
            dataTable.Columns.Add("DisableMenuAnimations", typeof(bool));
            dataTable.Columns.Add("DisableCursorShadow", typeof(bool));
            dataTable.Columns.Add("DisableCursorBlinking", typeof(bool));
            dataTable.Columns.Add("CacheBitmaps", typeof(bool));
            dataTable.Columns.Add("RedirectDiskDrives", typeof(bool));
            dataTable.Columns.Add("RedirectPorts", typeof(bool));
            dataTable.Columns.Add("RedirectPrinters", typeof(bool));
            dataTable.Columns.Add("RedirectClipboard", typeof(bool));
            dataTable.Columns.Add("RedirectSmartCards", typeof(bool));
            dataTable.Columns.Add("RedirectSound", typeof(string));
            dataTable.Columns.Add("RedirectAudioCapture", typeof(bool));
            dataTable.Columns.Add("RedirectKeys", typeof(bool));
            dataTable.Columns.Add("Connected", typeof(bool));
            dataTable.Columns.Add("OpeningCommand", typeof(string));
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
            dataTable.Columns.Add("RDGatewayExternalCredentialProvider", typeof(string));
            dataTable.Columns.Add("RDGatewayUserViaAPI", typeof(string));
            dataTable.Columns.Add("InheritCacheBitmaps", typeof(bool));
            dataTable.Columns.Add("InheritColors", typeof(bool));
            dataTable.Columns.Add("InheritDescription", typeof(bool));
            dataTable.Columns.Add("InheritDisplayThemes", typeof(bool));
            dataTable.Columns.Add("InheritDisplayWallpaper", typeof(bool));
            dataTable.Columns.Add("InheritEnableFontSmoothing", typeof(bool));
            dataTable.Columns.Add("InheritEnableDesktopComposition", typeof(bool));
            dataTable.Columns.Add("InheritDisableFullWindowDrag", typeof(bool));
            dataTable.Columns.Add("InheritDisableMenuAnimations", typeof(bool));
            dataTable.Columns.Add("InheritDisableCursorShadow", typeof(bool));
            dataTable.Columns.Add("InheritDisableCursorBlinking", typeof(bool));
            dataTable.Columns.Add("InheritDomain", typeof(bool));
            dataTable.Columns.Add("InheritIcon", typeof(bool));
            dataTable.Columns.Add("InheritPanel", typeof(bool));
            dataTable.Columns.Add("InheritPassword", typeof(bool));
            dataTable.Columns.Add("InheritPort", typeof(bool));
            dataTable.Columns.Add("InheritProtocol", typeof(bool));
            dataTable.Columns.Add("InheritExternalCredentialProvider", typeof(bool));
            dataTable.Columns.Add("InheritUserViaAPI", typeof(bool));
            dataTable.Columns.Add("InheritSSHTunnelConnectionName", typeof(bool));
            dataTable.Columns.Add("InheritSSHOptions", typeof(bool));
            dataTable.Columns.Add("InheritPuttySession", typeof(bool));
            dataTable.Columns.Add("InheritRedirectDiskDrives", typeof(bool));
            dataTable.Columns.Add("InheritRedirectKeys", typeof(bool));
            dataTable.Columns.Add("InheritRedirectPorts", typeof(bool));
            dataTable.Columns.Add("InheritRedirectPrinters", typeof(bool));
            dataTable.Columns.Add("InheritRedirectClipboard", typeof(bool));
            dataTable.Columns.Add("InheritRedirectSmartCards", typeof(bool));
            dataTable.Columns.Add("InheritRedirectSound", typeof(bool));
            dataTable.Columns.Add("InheritRedirectAudioCapture", typeof(bool));
            dataTable.Columns.Add("InheritResolution", typeof(bool));
            dataTable.Columns.Add("InheritUseConsoleSession", typeof(bool));
            dataTable.Columns.Add("InheritUseCredSsp", typeof(bool));
            dataTable.Columns.Add("InheritUseRestrictedAdmin", typeof(bool));
            dataTable.Columns.Add("InheritUseRCG", typeof(bool));
            dataTable.Columns.Add("InheritRenderingEngine", typeof(bool));
            dataTable.Columns.Add("InheritRDPAuthenticationLevel", typeof(bool));
            dataTable.Columns.Add("InheritUsername", typeof(bool));
            dataTable.Columns.Add("InheritOpeningCommand", typeof(bool));
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
            dataTable.Columns.Add("InheritRDGatewayExternalCredentialProvider", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayUserViaAPI", typeof(bool));
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
            dataTable.Columns.Add("VmId", typeof(string));
            dataTable.Columns.Add("UseVmId", typeof(bool));
            dataTable.Columns.Add("UseEnhancedMode", typeof(bool));
            dataTable.Columns.Add("InheritVmId", typeof(bool));
            dataTable.Columns.Add("InheritUseVmId", typeof(bool));
            dataTable.Columns.Add("InheritUseEnhancedMode", typeof(bool));
            dataTable.Columns.Add("RdpVersion", typeof(string));
            dataTable.Columns.Add("InheritRdpVersion", typeof(bool));
            dataTable.Columns.Add("EnhancedMode", typeof(bool));
            dataTable.Columns.Add("InheritEnhancedMode", typeof(bool));
            dataTable.Columns.Add("Favorite", typeof(bool));
            dataTable.Columns.Add("InheritFavorite", typeof(bool));
            dataTable.Columns.Add("ICAEncryptionStrength", typeof(string));
            dataTable.Columns.Add("InheritICAEncryptionStrength", typeof(bool));
            dataTable.Columns.Add("StartProgram", typeof(string));
            dataTable.Columns.Add("StartProgramWorkDir", typeof(string));
        }

        private void SetPrimaryKey(DataTable dataTable)
        {
            dataTable.PrimaryKey = new[] {dataTable.Columns["ConstantID"]};
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

        public bool isRowUpdated(ConnectionInfo connectionInfo, DataRow dataRow)
        {
            var isFieldNotChange = dataRow["Name"].Equals(connectionInfo.Name) &&
            dataRow["Type"].Equals(connectionInfo.GetTreeNodeType().ToString()) &&
            dataRow["ParentID"].Equals(connectionInfo.Parent?.ConstantID ?? "") &&
            dataRow["PositionID"].Equals(_currentNodeIndex) &&
            dataRow["Expanded"].Equals(false) &&
            dataRow["Description"].Equals(connectionInfo.Description) &&
            dataRow["Icon"].Equals(connectionInfo.Icon) &&
            dataRow["Panel"].Equals(connectionInfo.Panel) &&
            dataRow["Username"].Equals(_saveFilter.SaveUsername ? connectionInfo.Username : "") &&
            dataRow["Domain"].Equals(_saveFilter.SaveDomain ? connectionInfo.Domain : "");

            isFieldNotChange = isFieldNotChange && dataRow["Hostname"].Equals(connectionInfo.Hostname);
            isFieldNotChange = isFieldNotChange && dataRow["EC2Region"].Equals(connectionInfo.EC2Region);
            isFieldNotChange = isFieldNotChange && dataRow["EC2InstanceId"].Equals(connectionInfo.EC2InstanceId);
            isFieldNotChange = isFieldNotChange && dataRow["ExternalAddressProvider"].Equals(connectionInfo.ExternalAddressProvider);
            isFieldNotChange = isFieldNotChange && dataRow["ExternalCredentialProvider"].Equals(connectionInfo.ExternalCredentialProvider);
            isFieldNotChange = isFieldNotChange && dataRow["UserViaAPI"].Equals(connectionInfo.UserViaAPI);
            isFieldNotChange = isFieldNotChange && dataRow["VmId"].Equals(connectionInfo.VmId);
            isFieldNotChange = isFieldNotChange && dataRow["Protocol"].Equals(connectionInfo.Protocol.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["PuttySession"].Equals(connectionInfo.PuttySession);
            isFieldNotChange = isFieldNotChange &&
            dataRow["Port"].Equals(connectionInfo.Port);
            isFieldNotChange = isFieldNotChange &&
            dataRow["ConnectToConsole"].Equals(connectionInfo.UseConsoleSession);
            isFieldNotChange = isFieldNotChange &&
            dataRow["UseCredSsp"].Equals(connectionInfo.UseCredSsp);
            isFieldNotChange = isFieldNotChange &&
            dataRow["UseRestrictedAdmin"].Equals(connectionInfo.UseRestrictedAdmin);
            isFieldNotChange = isFieldNotChange &&
            dataRow["UseRCG"].Equals(connectionInfo.UseRCG);
            isFieldNotChange = isFieldNotChange &&
            dataRow["UseVmId"].Equals(connectionInfo.UseVmId);
            isFieldNotChange = isFieldNotChange &&
            dataRow["UseEnhancedMode"].Equals(connectionInfo.UseEnhancedMode);
            isFieldNotChange = isFieldNotChange &&
            dataRow["RenderingEngine"].Equals(connectionInfo.RenderingEngine.ToString());
            isFieldNotChange = isFieldNotChange &&
            dataRow["RDPAuthenticationLevel"].Equals(connectionInfo.RDPAuthenticationLevel.ToString());
            isFieldNotChange = isFieldNotChange &&
            dataRow["RDPMinutesToIdleTimeout"].Equals(connectionInfo.RDPMinutesToIdleTimeout);
            isFieldNotChange = isFieldNotChange &&
            dataRow["RDPAlertIdleTimeout"].Equals(connectionInfo.RDPAlertIdleTimeout);
            isFieldNotChange = isFieldNotChange &&
            dataRow["LoadBalanceInfo"].Equals(connectionInfo.LoadBalanceInfo);
            isFieldNotChange = isFieldNotChange &&
            dataRow["Colors"].Equals(connectionInfo.Colors.ToString());
            isFieldNotChange = isFieldNotChange &&
            dataRow["Resolution"].Equals(connectionInfo.Resolution.ToString());
            isFieldNotChange = isFieldNotChange &&
            dataRow["AutomaticResize"].Equals(connectionInfo.AutomaticResize);
            isFieldNotChange = isFieldNotChange &&
            dataRow["DisplayWallpaper"].Equals(connectionInfo.DisplayWallpaper) &&
             dataRow["DisplayThemes"].Equals(connectionInfo.DisplayThemes) &&
             dataRow["EnableFontSmoothing"].Equals(connectionInfo.EnableFontSmoothing) &&
             dataRow["EnableDesktopComposition"].Equals(connectionInfo.EnableDesktopComposition) &&
             dataRow["DisableFullWindowDrag"].Equals(connectionInfo.DisableFullWindowDrag) &&
             dataRow["DisableMenuAnimations"].Equals(connectionInfo.DisableMenuAnimations) &&
             dataRow["DisableCursorShadow"].Equals(connectionInfo.DisableCursorShadow) &&
             dataRow["DisableCursorBlinking"].Equals(connectionInfo.DisableCursorBlinking) &&
             dataRow["CacheBitmaps"].Equals(connectionInfo.CacheBitmaps) &&
             dataRow["RedirectDiskDrives"].Equals(connectionInfo.RedirectDiskDrives) &&
             dataRow["RedirectPorts"].Equals(connectionInfo.RedirectPorts) &&
             dataRow["RedirectPrinters"].Equals(connectionInfo.RedirectPrinters) &&
             dataRow["RedirectClipboard"].Equals(connectionInfo.RedirectClipboard) &&
             dataRow["RedirectSmartCards"].Equals(connectionInfo.RedirectSmartCards) &&
             dataRow["RedirectSound"].Equals(connectionInfo.RedirectSound.ToString()) &&
             dataRow["SoundQuality"].Equals(connectionInfo.SoundQuality.ToString()) &&
             dataRow["RedirectAudioCapture"].Equals(connectionInfo.RedirectAudioCapture) &&
             dataRow["RedirectKeys"].Equals(connectionInfo.RedirectKeys) &&
             dataRow["StartProgram"].Equals(connectionInfo.RDPStartProgram) &&
             dataRow["StartProgramWorkDir"].Equals(connectionInfo.RDPStartProgramWorkDir);

            isFieldNotChange = isFieldNotChange &&
             dataRow["Connected"].Equals(false) && // TODO: this column can eventually be removed. we now save this property locally
             dataRow["OpeningCommand"].Equals(connectionInfo.OpeningCommand) && 
             dataRow["PreExtApp"].Equals(connectionInfo.PreExtApp) &&
             dataRow["PostExtApp"].Equals(connectionInfo.PostExtApp) &&
             dataRow["MacAddress"].Equals(connectionInfo.MacAddress) &&
             dataRow["UserField"].Equals(connectionInfo.UserField) &&
             dataRow["ExtApp"].Equals(connectionInfo.ExtApp) &&
             dataRow["VNCCompression"].Equals(connectionInfo.VNCCompression.ToString()) &&
             dataRow["VNCEncoding"].Equals(connectionInfo.VNCEncoding.ToString()) &&
             dataRow["VNCAuthMode"].Equals(connectionInfo.VNCAuthMode.ToString()) &&
             dataRow["VNCProxyType"].Equals(connectionInfo.VNCProxyType.ToString()) &&
             dataRow["VNCProxyIP"].Equals(connectionInfo.VNCProxyIP) &&
             dataRow["VNCProxyPort"].Equals(connectionInfo.VNCProxyPort) &&
             dataRow["VNCProxyUsername"].Equals(connectionInfo.VNCProxyUsername) &&
             dataRow["VNCColors"].Equals(connectionInfo.VNCColors.ToString()) &&
             dataRow["VNCSmartSizeMode"].Equals(connectionInfo.VNCSmartSizeMode.ToString()) &&
             dataRow["VNCViewOnly"].Equals(connectionInfo.VNCViewOnly) &&
             dataRow["RDGatewayUsageMethod"].Equals(connectionInfo.RDGatewayUsageMethod.ToString()) &&
             dataRow["RDGatewayHostname"].Equals(connectionInfo.RDGatewayHostname) &&
             dataRow["RDGatewayUseConnectionCredentials"].Equals(connectionInfo.RDGatewayUseConnectionCredentials.ToString()) &&
             dataRow["RDGatewayExternalCredentialProvider"].Equals(connectionInfo.RDGatewayExternalCredentialProvider) &&
             dataRow["RDGatewayUsername"].Equals(connectionInfo.RDGatewayUsername) &&
             dataRow["RDGatewayDomain"].Equals(connectionInfo.RDGatewayDomain) &&
             dataRow["RDGatewayUserViaAPI"].Equals(connectionInfo.RDGatewayUserViaAPI) &&
             dataRow["RdpVersion"].Equals(connectionInfo.RdpVersion.ToString());

            var isInheritanceFieldNotChange = false;
            if (_saveFilter.SaveInheritance)
            {
                isInheritanceFieldNotChange = (dataRow["InheritCacheBitmaps"].Equals(connectionInfo.Inheritance.CacheBitmaps) &&
                dataRow["InheritColors"].Equals(connectionInfo.Inheritance.Colors) &&
                dataRow["InheritDescription"].Equals(connectionInfo.Inheritance.Description) &&
                dataRow["InheritDisplayThemes"].Equals(connectionInfo.Inheritance.DisplayThemes) &&
                dataRow["InheritDisplayWallpaper"].Equals(connectionInfo.Inheritance.DisplayWallpaper) &&
                dataRow["InheritEnableFontSmoothing"].Equals(connectionInfo.Inheritance.EnableFontSmoothing) &&
                dataRow["InheritEnableDesktopComposition"].Equals(connectionInfo.Inheritance.EnableDesktopComposition) &&
                dataRow["InheritDisableFullWindowDrag"].Equals(connectionInfo.Inheritance.DisableFullWindowDrag) &&
                dataRow["InheritDisableMenuAnimations"].Equals(connectionInfo.Inheritance.DisableMenuAnimations) &&
                dataRow["InheritDisableCursorShadow"].Equals(connectionInfo.Inheritance.DisableCursorShadow) &&
                dataRow["InheritDisableCursorBlinking"].Equals(connectionInfo.Inheritance.DisableCursorBlinking) &&
                dataRow["InheritDomain"].Equals(connectionInfo.Inheritance.Domain) &&
                dataRow["InheritIcon"].Equals(connectionInfo.Inheritance.Icon) &&
                dataRow["InheritPanel"].Equals(connectionInfo.Inheritance.Panel) &&
                dataRow["InheritPassword"].Equals(connectionInfo.Inheritance.Password) &&
                dataRow["InheritPort"].Equals(connectionInfo.Inheritance.Port) &&
                dataRow["InheritProtocol"].Equals(connectionInfo.Inheritance.Protocol) &&
                dataRow["InheritExternalCredentialProvider"].Equals(connectionInfo.Inheritance.ExternalCredentialProvider) &&
                dataRow["InheritUserViaAPI"].Equals(connectionInfo.Inheritance.UserViaAPI) &&
                dataRow["InheritPuttySession"].Equals(connectionInfo.Inheritance.PuttySession) &&
                dataRow["InheritRedirectDiskDrives"].Equals(connectionInfo.Inheritance.RedirectDiskDrives) &&
                dataRow["InheritRedirectKeys"].Equals(connectionInfo.Inheritance.RedirectKeys) &&
                dataRow["InheritRedirectPorts"].Equals(connectionInfo.Inheritance.RedirectPorts) &&
                dataRow["InheritRedirectPrinters"].Equals(connectionInfo.Inheritance.RedirectPrinters) &&
                dataRow["InheritRedirectClipboard"].Equals(connectionInfo.Inheritance.RedirectClipboard) &&
                dataRow["InheritRedirectSmartCards"].Equals(connectionInfo.Inheritance.RedirectSmartCards) &&
                dataRow["InheritRedirectSound"].Equals(connectionInfo.Inheritance.RedirectSound) &&
                dataRow["InheritSoundQuality"].Equals(connectionInfo.Inheritance.SoundQuality) &&
                dataRow["InheritRedirectAudioCapture"].Equals(connectionInfo.Inheritance.RedirectAudioCapture) &&
                dataRow["InheritResolution"].Equals(connectionInfo.Inheritance.Resolution) &&
                dataRow["InheritAutomaticResize"].Equals(connectionInfo.Inheritance.AutomaticResize) &&
                dataRow["InheritUseConsoleSession"].Equals(connectionInfo.Inheritance.UseConsoleSession) &&
                dataRow["InheritUseCredSsp"].Equals(connectionInfo.Inheritance.UseCredSsp) &&
                dataRow["InheritUseRestrictedAdmin"].Equals(connectionInfo.Inheritance.UseRestrictedAdmin) &&
                dataRow["InheritUseRCG"].Equals(connectionInfo.Inheritance.UseRCG) &&
                dataRow["InheritRenderingEngine"].Equals(connectionInfo.Inheritance.RenderingEngine) &&
                dataRow["InheritUsername"].Equals(connectionInfo.Inheritance.Username) &&
                dataRow["InheritVmId"].Equals(connectionInfo.Inheritance.VmId) &&
                dataRow["InheritUseVmId"].Equals(connectionInfo.Inheritance.UseVmId) &&
                dataRow["InheritUseEnhancedMode"].Equals(connectionInfo.Inheritance.UseEnhancedMode) &&
                dataRow["InheritRDPAuthenticationLevel"].Equals(connectionInfo.Inheritance.RDPAuthenticationLevel) &&
                dataRow["InheritRDPMinutesToIdleTimeout"].Equals(connectionInfo.Inheritance.RDPMinutesToIdleTimeout) &&
                dataRow["InheritRDPAlertIdleTimeout"].Equals(connectionInfo.Inheritance.RDPAlertIdleTimeout) &&
                dataRow["InheritLoadBalanceInfo"].Equals(connectionInfo.Inheritance.LoadBalanceInfo) &&
                dataRow["InheritOpeningCommand"].Equals(connectionInfo.Inheritance.OpeningCommand) &&
                dataRow["InheritPreExtApp"].Equals(connectionInfo.Inheritance.PreExtApp) &&
                dataRow["InheritPostExtApp"].Equals(connectionInfo.Inheritance.PostExtApp) &&
                dataRow["InheritMacAddress"].Equals(connectionInfo.Inheritance.MacAddress) &&
                dataRow["InheritUserField"].Equals(connectionInfo.Inheritance.UserField) &&
                dataRow["InheritExtApp"].Equals(connectionInfo.Inheritance.ExtApp) &&
                dataRow["InheritVNCCompression"].Equals(connectionInfo.Inheritance.VNCCompression) &&
                dataRow["InheritVNCEncoding"].Equals(connectionInfo.Inheritance.VNCEncoding) &&
                dataRow["InheritVNCAuthMode"].Equals(connectionInfo.Inheritance.VNCAuthMode) &&
                dataRow["InheritVNCProxyType"].Equals(connectionInfo.Inheritance.VNCProxyType) &&
                dataRow["InheritVNCProxyIP"].Equals(connectionInfo.Inheritance.VNCProxyIP) &&
                dataRow["InheritVNCProxyPort"].Equals(connectionInfo.Inheritance.VNCProxyPort) &&
                dataRow["InheritVNCProxyUsername"].Equals(connectionInfo.Inheritance.VNCProxyUsername) &&
                dataRow["InheritVNCProxyPassword"].Equals(connectionInfo.Inheritance.VNCProxyPassword) &&
                dataRow["InheritVNCColors"].Equals(connectionInfo.Inheritance.VNCColors) &&
                dataRow["InheritVNCSmartSizeMode"].Equals(connectionInfo.Inheritance.VNCSmartSizeMode) &&
                dataRow["InheritVNCViewOnly"].Equals(connectionInfo.Inheritance.VNCViewOnly) &&
                dataRow["InheritRDGatewayUsageMethod"].Equals(connectionInfo.Inheritance.RDGatewayUsageMethod) &&
                dataRow["InheritRDGatewayHostname"].Equals(connectionInfo.Inheritance.RDGatewayHostname) &&
                dataRow["InheritRDGatewayUseConnectionCredentials"].Equals(connectionInfo.Inheritance.RDGatewayUseConnectionCredentials) &&
                dataRow["InheritRDGatewayExternalCredentialProvider"].Equals(connectionInfo.Inheritance.RDGatewayExternalCredentialProvider) &&
                dataRow["InheritRDGatewayUsername"].Equals(connectionInfo.Inheritance.RDGatewayUsername) &&
                dataRow["InheritRDGatewayPassword"].Equals(connectionInfo.Inheritance.RDGatewayPassword) &&
                dataRow["InheritRDGatewayDomain"].Equals(connectionInfo.Inheritance.RDGatewayDomain) &&
                dataRow["InheritRDGatewayUserViaAPI"].Equals(connectionInfo.Inheritance.RDGatewayUserViaAPI) &&
                dataRow["InheritRdpVersion"].Equals(connectionInfo.Inheritance.RdpVersion));
            }
            else
            {
                isInheritanceFieldNotChange = (dataRow["InheritCacheBitmaps"].Equals(false) &&
                dataRow["InheritColors"].Equals(false) &&
                dataRow["InheritDescription"].Equals(false) &&
                dataRow["InheritDisplayThemes"].Equals(false) &&
                dataRow["InheritDisplayWallpaper"].Equals(false) &&
                dataRow["InheritEnableFontSmoothing"].Equals(false) &&
                dataRow["InheritEnableDesktopComposition"].Equals(false) &&
                dataRow["InheritDisableFullWindowDrag"].Equals(false) &&
                dataRow["InheritDisableMenuAnimations"].Equals(false) &&
                dataRow["InheritDisableCursorShadow"].Equals(false) &&
                dataRow["InheritDisableCursorBlinking"].Equals(false) &&
                dataRow["InheritDomain"].Equals(false) &&
                dataRow["InheritIcon"].Equals(false) &&
                dataRow["InheritPanel"].Equals(false) &&
                dataRow["InheritPassword"].Equals(false) &&
                dataRow["InheritPort"].Equals(false) &&
                dataRow["InheritProtocol"].Equals(false) &&
                dataRow["InheritExternalCredentialProvider"].Equals(false) &&
                dataRow["InheritUserViaAPI"].Equals(false) &&
                dataRow["InheritPuttySession"].Equals(false) &&
                dataRow["InheritRedirectDiskDrives"].Equals(false) &&
                dataRow["InheritRedirectKeys"].Equals(false) &&
                dataRow["InheritRedirectPorts"].Equals(false) &&
                dataRow["InheritRedirectPrinters"].Equals(false) &&
                dataRow["InheritRedirectClipboard"].Equals(false) &&
                dataRow["InheritRedirectSmartCards"].Equals(false) &&
                dataRow["InheritRedirectSound"].Equals(false) &&
                dataRow["InheritSoundQuality"].Equals(false) &&
                dataRow["InheritRedirectAudioCapture"].Equals(false) &&
                dataRow["InheritResolution"].Equals(false) &&
                dataRow["InheritAutomaticResize"].Equals(false) &&
                dataRow["InheritUseConsoleSession"].Equals(false) &&
                dataRow["InheritUseCredSsp"].Equals(false) &&
                dataRow["InheritUseRestrictedAdmin"].Equals(false) &&
                dataRow["InheritUseRCG"].Equals(false) &&
                dataRow["InheritRenderingEngine"].Equals(false) &&
                dataRow["InheritUsername"].Equals(false) &&
                dataRow["InheritRDPAuthenticationLevel"].Equals(false) &&
                dataRow["InheritRDPMinutesToIdleTimeout"].Equals(false) &&
                dataRow["InheritRDPAlertIdleTimeout"].Equals(false) &&
                dataRow["InheritLoadBalanceInfo"].Equals(false) &&
                dataRow["InheritOpeningCommand"].Equals(false) &&
                dataRow["InheritPreExtApp"].Equals(false) &&
                dataRow["InheritPostExtApp"].Equals(false) &&
                dataRow["InheritMacAddress"].Equals(false) &&
                dataRow["InheritUserField"].Equals(false) &&
                dataRow["InheritExtApp"].Equals(false) &&
                dataRow["InheritVNCCompression"].Equals(false) &&
                dataRow["InheritVNCEncoding"].Equals(false) &&
                dataRow["InheritVNCAuthMode"].Equals(false) &&
                dataRow["InheritVNCProxyType"].Equals(false) &&
                dataRow["InheritVNCProxyIP"].Equals(false) &&
                dataRow["InheritVNCProxyPort"].Equals(false) &&
                dataRow["InheritVNCProxyUsername"].Equals(false) &&
                dataRow["InheritVNCProxyPassword"].Equals(false) &&
                dataRow["InheritVNCColors"].Equals(false) &&
                dataRow["InheritVNCSmartSizeMode"].Equals(false) &&
                dataRow["InheritVNCViewOnly"].Equals(false) &&
                dataRow["InheritRDGatewayUsageMethod"].Equals(false) &&
                dataRow["InheritRDGatewayHostname"].Equals(false) &&
                dataRow["InheritRDGatewayUseConnectionCredentials"].Equals(false) &&
                dataRow["InheritRDGatewayExternalCredentialProvider"].Equals(connectionInfo.Inheritance.RDGatewayExternalCredentialProvider) &&
                dataRow["InheritRDGatewayUsername"].Equals(false) &&
                dataRow["InheritRDGatewayPassword"].Equals(false) &&
                dataRow["InheritRDGatewayDomain"].Equals(false) &&
                dataRow["InheritRDGatewayUserViaAPI"].Equals(false) &&
                dataRow["InheritRdpVersion"].Equals(false));
            }

            var pwd = dataRow["Password"].Equals(_saveFilter.SavePassword ? _cryptographyProvider.Encrypt(connectionInfo.Password, _encryptionKey) : "") &&
                      dataRow["VNCProxyPassword"].Equals(_cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword, _encryptionKey)) &&
                      dataRow["RDGatewayPassword"].Equals(_cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword, _encryptionKey));
            return !(pwd && isFieldNotChange && isInheritanceFieldNotChange);
        }

        private void SerializeConnectionInfo(ConnectionInfo connectionInfo)
        {
            _currentNodeIndex++;
            var isNewRow = false;
            DataRow dataRow = _dataTable.Rows.Find(connectionInfo.ConstantID);
            if (dataRow == null)
            {
                dataRow = _dataTable.NewRow();
                dataRow["ConstantID"] = connectionInfo.ConstantID;
                isNewRow = true;
            }
            else
            {
                sourcePrimaryKeyDict.Remove(connectionInfo.ConstantID);
            }
            var tmp = isRowUpdated(connectionInfo, dataRow);
            if (!tmp){
                return;
            } 
            dataRow["Name"] = connectionInfo.Name;
            dataRow["Type"] = connectionInfo.GetTreeNodeType().ToString();
            dataRow["ParentID"] = connectionInfo.Parent?.ConstantID ?? "";
            dataRow["PositionID"] = _currentNodeIndex;
            dataRow["LastChange"] = MiscTools.DBTimeStampNow();
            dataRow["Expanded"] =
                false; // TODO: this column can eventually be removed. we now save this property locally
            dataRow["Description"] = connectionInfo.Description;
            dataRow["Icon"] = connectionInfo.Icon;
            dataRow["Panel"] = connectionInfo.Panel;
            dataRow["Username"] = _saveFilter.SaveUsername ? connectionInfo.Username : "";
            dataRow["Domain"] = _saveFilter.SaveDomain ? connectionInfo.Domain : "";
            dataRow["Password"] = _saveFilter.SavePassword
                ? _cryptographyProvider.Encrypt(connectionInfo.Password, _encryptionKey)
                : "";
            dataRow["Hostname"] = connectionInfo.Hostname;
            dataRow["VmId"] = connectionInfo.VmId;
            dataRow["Protocol"] = connectionInfo.Protocol;
            dataRow["SSHTunnelConnectionName"] = connectionInfo.SSHTunnelConnectionName;
            dataRow["OpeningCommand"] = connectionInfo.OpeningCommand;
            dataRow["SSHOptions"] = connectionInfo.SSHOptions;
            dataRow["PuttySession"] = connectionInfo.PuttySession;
            dataRow["Port"] = connectionInfo.Port;
            dataRow["ConnectToConsole"] = connectionInfo.UseConsoleSession;
            dataRow["UseCredSsp"] = connectionInfo.UseCredSsp;
            dataRow["UseRestrictedAdmin"] = connectionInfo.UseRestrictedAdmin;
            dataRow["UseRCG"] = connectionInfo.UseRCG;
            dataRow["UseVmId"] = connectionInfo.UseVmId;
            dataRow["UseEnhancedMode"] = connectionInfo.UseEnhancedMode;
            dataRow["RenderingEngine"] = connectionInfo.RenderingEngine;
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
            dataRow["DisableFullWindowDrag"] = connectionInfo.DisableFullWindowDrag;
            dataRow["DisableMenuAnimations"] = connectionInfo.DisableMenuAnimations;
            dataRow["DisableCursorShadow"] = connectionInfo.DisableCursorShadow;
            dataRow["DisableCursorBlinking"] = connectionInfo.DisableCursorBlinking;
            dataRow["CacheBitmaps"] = connectionInfo.CacheBitmaps;
            dataRow["RedirectDiskDrives"] = connectionInfo.RedirectDiskDrives;
            dataRow["RedirectPorts"] = connectionInfo.RedirectPorts;
            dataRow["RedirectPrinters"] = connectionInfo.RedirectPrinters;
            dataRow["RedirectClipboard"] = connectionInfo.RedirectClipboard;
            dataRow["RedirectSmartCards"] = connectionInfo.RedirectSmartCards;
            dataRow["RedirectSound"] = connectionInfo.RedirectSound;
            dataRow["SoundQuality"] = connectionInfo.SoundQuality;
            dataRow["RedirectAudioCapture"] = connectionInfo.RedirectAudioCapture;
            dataRow["RedirectKeys"] = connectionInfo.RedirectKeys;
            dataRow["Connected"] = false; // TODO: this column can eventually be removed. we now save this property locally
            dataRow["OpeningCommand"] = connectionInfo.OpeningCommand;
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
            dataRow["VNCProxyPassword"] =
                _cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword, _encryptionKey);
            dataRow["VNCColors"] = connectionInfo.VNCColors;
            dataRow["VNCSmartSizeMode"] = connectionInfo.VNCSmartSizeMode;
            dataRow["VNCViewOnly"] = connectionInfo.VNCViewOnly;
            dataRow["RDGatewayUsageMethod"] = connectionInfo.RDGatewayUsageMethod;
            dataRow["RDGatewayHostname"] = connectionInfo.RDGatewayHostname;
            dataRow["RDGatewayUseConnectionCredentials"] = connectionInfo.RDGatewayUseConnectionCredentials;
            dataRow["RDGatewayUsername"] = connectionInfo.RDGatewayUsername;
            dataRow["RDGatewayPassword"] = _cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword, _encryptionKey);
            dataRow["RDGatewayDomain"] = connectionInfo.RDGatewayDomain;
            dataRow["RdpVersion"] = connectionInfo.RdpVersion;
            dataRow["Favorite"] = connectionInfo.Favorite;
            dataRow["ICAEncryptionStrength"] = string.Empty;
            dataRow["StartProgram"] = connectionInfo.RDPStartProgram;
            dataRow["StartProgramWorkDir"] = connectionInfo.RDPStartProgramWorkDir;

            if (_saveFilter.SaveInheritance)
            {
                dataRow["InheritCacheBitmaps"] = connectionInfo.Inheritance.CacheBitmaps;
                dataRow["InheritColors"] = connectionInfo.Inheritance.Colors;
                dataRow["InheritDescription"] = connectionInfo.Inheritance.Description;
                dataRow["InheritDisplayThemes"] = connectionInfo.Inheritance.DisplayThemes;
                dataRow["InheritDisplayWallpaper"] = connectionInfo.Inheritance.DisplayWallpaper;
                dataRow["InheritEnableFontSmoothing"] = connectionInfo.Inheritance.EnableFontSmoothing;
                dataRow["InheritEnableDesktopComposition"] = connectionInfo.Inheritance.EnableDesktopComposition;
                dataRow["InheritDisableFullWindowDrag"] = connectionInfo.Inheritance.DisableFullWindowDrag;
                dataRow["InheritDisableMenuAnimations"] = connectionInfo.Inheritance.DisableMenuAnimations;
                dataRow["InheritDisableCursorShadow"] = connectionInfo.Inheritance.DisableCursorShadow;
                dataRow["InheritDisableCursorBlinking"] = connectionInfo.Inheritance.DisableCursorBlinking;
                dataRow["InheritDomain"] = connectionInfo.Inheritance.Domain;
                dataRow["InheritIcon"] = connectionInfo.Inheritance.Icon;
                dataRow["InheritPanel"] = connectionInfo.Inheritance.Panel;
                dataRow["InheritPassword"] = connectionInfo.Inheritance.Password;
                dataRow["InheritPort"] = connectionInfo.Inheritance.Port;
                dataRow["InheritProtocol"] = connectionInfo.Inheritance.Protocol;
                dataRow["InheritExternalCredentialProvider"] = connectionInfo.Inheritance.ExternalCredentialProvider;
                dataRow["InheritUserViaAPI"] = connectionInfo.Inheritance.UserViaAPI;
                dataRow["InheritSSHTunnelConnectionName"] = connectionInfo.Inheritance.SSHTunnelConnectionName;
                dataRow["InheritOpeningCommand"] = connectionInfo.Inheritance.OpeningCommand;
                dataRow["InheritSSHOptions"] = connectionInfo.Inheritance.SSHOptions;
                dataRow["InheritPuttySession"] = connectionInfo.Inheritance.PuttySession;
                dataRow["InheritRedirectDiskDrives"] = connectionInfo.Inheritance.RedirectDiskDrives;
                dataRow["InheritRedirectKeys"] = connectionInfo.Inheritance.RedirectKeys;
                dataRow["InheritRedirectPorts"] = connectionInfo.Inheritance.RedirectPorts;
                dataRow["InheritRedirectPrinters"] = connectionInfo.Inheritance.RedirectPrinters;
                dataRow["InheritRedirectClipboard"] = connectionInfo.Inheritance.RedirectClipboard;
                dataRow["InheritRedirectSmartCards"] = connectionInfo.Inheritance.RedirectSmartCards;
                dataRow["InheritRedirectSound"] = connectionInfo.Inheritance.RedirectSound;
                dataRow["InheritSoundQuality"] = connectionInfo.Inheritance.SoundQuality;
                dataRow["InheritRedirectAudioCapture"] = connectionInfo.Inheritance.RedirectAudioCapture;
                dataRow["InheritResolution"] = connectionInfo.Inheritance.Resolution;
                dataRow["InheritAutomaticResize"] = connectionInfo.Inheritance.AutomaticResize;
                dataRow["InheritUseConsoleSession"] = connectionInfo.Inheritance.UseConsoleSession;
                dataRow["InheritUseCredSsp"] = connectionInfo.Inheritance.UseCredSsp;
                dataRow["InheritUseRestrictedAdmin"] = connectionInfo.Inheritance.UseRestrictedAdmin;
                dataRow["InheritUseRCG"] = connectionInfo.Inheritance.UseRCG;
                dataRow["InheritRenderingEngine"] = connectionInfo.Inheritance.RenderingEngine;
                dataRow["InheritUsername"] = connectionInfo.Inheritance.Username;
                dataRow["InheritVmId"] = connectionInfo.Inheritance.VmId;
                dataRow["InheritUseVmId"] = connectionInfo.Inheritance.UseVmId;
                dataRow["InheritUseEnhancedMode"] = connectionInfo.Inheritance.UseEnhancedMode;
                dataRow["InheritRDPAuthenticationLevel"] = connectionInfo.Inheritance.RDPAuthenticationLevel;
                dataRow["InheritRDPMinutesToIdleTimeout"] = connectionInfo.Inheritance.RDPMinutesToIdleTimeout;
                dataRow["InheritRDPAlertIdleTimeout"] = connectionInfo.Inheritance.RDPAlertIdleTimeout;
                dataRow["InheritLoadBalanceInfo"] = connectionInfo.Inheritance.LoadBalanceInfo;
                dataRow["InheritOpeningCommand"] = connectionInfo.Inheritance.OpeningCommand;
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
                dataRow["InheritRDGatewayExternalCredentialProvider"] = connectionInfo.Inheritance.RDGatewayExternalCredentialProvider;
                dataRow["InheritRDGatewayUsername"] = connectionInfo.Inheritance.RDGatewayUsername;
                dataRow["InheritRDGatewayPassword"] = connectionInfo.Inheritance.RDGatewayPassword;
                dataRow["InheritRDGatewayDomain"] = connectionInfo.Inheritance.RDGatewayDomain;
                dataRow["InheritRDGatewayUserViaAPI"] = connectionInfo.Inheritance.RDGatewayUserViaAPI;
                dataRow["InheritRdpVersion"] = connectionInfo.Inheritance.RdpVersion;
                dataRow["InheritFavorite"] = connectionInfo.Inheritance.Favorite;
                dataRow["InheritICAEncryptionStrength"] = false;
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
                dataRow["InheritDisableFullWindowDrag"] = false;
                dataRow["InheritDisableMenuAnimations"] = false;
                dataRow["InheritDisableCursorShadow"] = false;
                dataRow["InheritDisableCursorBlinking"] = false;
                dataRow["InheritDomain"] = false;
                dataRow["InheritIcon"] = false;
                dataRow["InheritPanel"] = false;
                dataRow["InheritPassword"] = false;
                dataRow["InheritPort"] = false;
                dataRow["InheritProtocol"] = false;
                dataRow["InheritExternalCredentialProvider"] = false;
                dataRow["InheritUserViaAPI"] = false;
                dataRow["InheritSSHTunnelConnectionName"] = false;
                dataRow["InheritSSHOptions"] = false;
                dataRow["InheritPuttySession"] = false;
                dataRow["InheritRedirectDiskDrives"] = false;
                dataRow["InheritRedirectKeys"] = false;
                dataRow["InheritRedirectPorts"] = false;
                dataRow["InheritRedirectPrinters"] = false;
                dataRow["InheritRedirectClipboard"] = false;
                dataRow["InheritRedirectSmartCards"] = false;
                dataRow["InheritRedirectSound"] = false;
                dataRow["InheritSoundQuality"] = false;
                dataRow["InheritRedirectAudioCapture"] = false;
                dataRow["InheritResolution"] = false;
                dataRow["InheritAutomaticResize"] = false;
                dataRow["InheritUseConsoleSession"] = false;
                dataRow["InheritUseCredSsp"] = false;
                dataRow["InheritUseRestrictedAdmin"] = false;
                dataRow["InheritUseRCG"] = false;
                dataRow["InheritRenderingEngine"] = false;
                dataRow["InheritUsername"] = false;
                dataRow["InheritRDPAuthenticationLevel"] = false;
                dataRow["InheritRDPMinutesToIdleTimeout"] = false;
                dataRow["InheritRDPAlertIdleTimeout"] = false;
                dataRow["InheritLoadBalanceInfo"] = false;
                dataRow["InheritOpeningCommand"] = false;
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
                dataRow["InheritRDGatewayExternalCredentialProvider"] = false;
                dataRow["InheritRDGatewayUsername"] = false;
                dataRow["InheritRDGatewayPassword"] = false;
                dataRow["InheritRDGatewayDomain"] = false;
                dataRow["InheritRDGatewayUserViaAPI"] = false;
                dataRow["InheritRdpVersion"] = false;
                dataRow["InheritFavorite"] = false;
                dataRow["InheritICAEncryptionStrength"] = false;
            }
            if (isNewRow)_dataTable.Rows.Add(dataRow);
        }
    }
}