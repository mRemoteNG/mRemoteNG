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

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Sql
{
    [SupportedOSPlatform("windows")]
    public class DataTableSerializer : ISerializer<ConnectionInfo, DataTable>
    {
        private const int DELETE = 0;
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _encryptionKey;
        private DataTable _dataTable;
        private DataTable _sourceDataTable;
        private readonly Dictionary<string, int> _sourcePrimaryKeyDict = new Dictionary<string, int>();
        private const string TABLE_NAME = "tblCons";
        private readonly SaveFilter _saveFilter;
        private int _currentNodeIndex;

        public Version Version { get; } = new Version(3, 0);

        public DataTableSerializer(SaveFilter saveFilter, ICryptographyProvider cryptographyProvider, SecureString encryptionKey)
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

                ContainerInfo rootNode = connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);

                return Serialize(rootNode);
            }
            catch (Exception)
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

            List<string> entryToDelete = _sourcePrimaryKeyDict.Keys.ToList();

            foreach (var entry in entryToDelete)
            {
                _dataTable.Rows.Find(entry)?.Delete();
            }

            return _dataTable;
        }

        private DataTable BuildTable()
        {
            DataTable dataTable = _sourceDataTable ?? new DataTable(TABLE_NAME);

            if (dataTable.Columns.Count == 0) CreateSchema(dataTable);

            if (dataTable.PrimaryKey.Length == 0) SetPrimaryKey(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                _sourcePrimaryKeyDict.Add((string)row["ConstantID"], DELETE);
            }

            return dataTable;
        }

        private void CreateSchema(DataTable dataTable)
        {
            dataTable.Columns.Add("AutomaticResize", typeof(bool));
            dataTable.Columns.Add("CacheBitmaps", typeof(bool));
            dataTable.Columns.Add("Colors", typeof(string));
            dataTable.Columns.Add("ConnectToConsole", typeof(bool));
            dataTable.Columns.Add("Connected", typeof(bool));
            dataTable.Columns.Add("ConstantID", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("DisableCursorBlinking", typeof(bool));
            dataTable.Columns.Add("DisableCursorShadow", typeof(bool));
            dataTable.Columns.Add("DisableFullWindowDrag", typeof(bool));
            dataTable.Columns.Add("DisableMenuAnimations", typeof(bool));
            dataTable.Columns.Add("DisplayThemes", typeof(bool));
            dataTable.Columns.Add("DisplayWallpaper", typeof(bool));
            dataTable.Columns.Add("Domain", typeof(string));
            dataTable.Columns.Add("EC2InstanceId", typeof(string));
            dataTable.Columns.Add("EC2Region", typeof(string));
            dataTable.Columns.Add("EnableDesktopComposition", typeof(bool));
            dataTable.Columns.Add("EnableFontSmoothing", typeof(bool));
            dataTable.Columns.Add("EnhancedMode", typeof(bool));
            dataTable.Columns.Add("Expanded", typeof(bool));
            dataTable.Columns.Add("ExtApp", typeof(string));
            dataTable.Columns.Add("ExternalAddressProvider", typeof(string));
            dataTable.Columns.Add("ExternalCredentialProvider", typeof(string));
            dataTable.Columns.Add("Favorite", typeof(bool));
            dataTable.Columns.Add("Hostname", typeof(string));
            dataTable.Columns.Add("ICAEncryptionStrength", typeof(string));
            dataTable.Columns.Add("Icon", typeof(string));
            dataTable.Columns.Add("InheritAutomaticResize", typeof(bool));
            dataTable.Columns.Add("InheritCacheBitmaps", typeof(bool));
            dataTable.Columns.Add("InheritColors", typeof(bool));
            dataTable.Columns.Add("InheritDescription", typeof(bool));
            dataTable.Columns.Add("InheritDisableCursorBlinking", typeof(bool));
            dataTable.Columns.Add("InheritDisableCursorShadow", typeof(bool));
            dataTable.Columns.Add("InheritDisableFullWindowDrag", typeof(bool));
            dataTable.Columns.Add("InheritDisableMenuAnimations", typeof(bool));
            dataTable.Columns.Add("InheritDisplayThemes", typeof(bool));
            dataTable.Columns.Add("InheritDisplayWallpaper", typeof(bool));
            dataTable.Columns.Add("InheritDomain", typeof(bool));
            dataTable.Columns.Add("InheritEnableDesktopComposition", typeof(bool));
            dataTable.Columns.Add("InheritEnableFontSmoothing", typeof(bool));
            dataTable.Columns.Add("InheritEnhancedMode", typeof(bool));
            dataTable.Columns.Add("InheritExtApp", typeof(bool));
            dataTable.Columns.Add("InheritExternalCredentialProvider", typeof(bool));
            dataTable.Columns.Add("InheritFavorite", typeof(bool));
            dataTable.Columns.Add("InheritICAEncryptionStrength", typeof(bool));
            dataTable.Columns.Add("InheritIcon", typeof(bool));
            dataTable.Columns.Add("InheritLoadBalanceInfo", typeof(bool));
            dataTable.Columns.Add("InheritMacAddress", typeof(bool));
            dataTable.Columns.Add("InheritOpeningCommand", typeof(bool));
            dataTable.Columns.Add("InheritPanel", typeof(bool));
            dataTable.Columns.Add("InheritPassword", typeof(bool));
            dataTable.Columns.Add("InheritPort", typeof(bool));
            dataTable.Columns.Add("InheritPostExtApp", typeof(bool));
            dataTable.Columns.Add("InheritPreExtApp", typeof(bool));
            dataTable.Columns.Add("InheritProtocol", typeof(bool));
            dataTable.Columns.Add("InheritPuttySession", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayDomain", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayExternalCredentialProvider", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayHostname", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayPassword", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayUsageMethod", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayUseConnectionCredentials", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayUserViaAPI", typeof(bool));
            dataTable.Columns.Add("InheritRDGatewayUsername", typeof(bool));
            dataTable.Columns.Add("InheritRDPAlertIdleTimeout", typeof(bool));
            dataTable.Columns.Add("InheritRDPAuthenticationLevel", typeof(bool));
            dataTable.Columns.Add("InheritRDPMinutesToIdleTimeout", typeof(bool));
            dataTable.Columns.Add("InheritRdpVersion", typeof(bool));
            dataTable.Columns.Add("InheritRedirectAudioCapture", typeof(bool));
            dataTable.Columns.Add("InheritRedirectClipboard", typeof(bool));
            dataTable.Columns.Add("InheritRedirectDiskDrives", typeof(bool));
            dataTable.Columns.Add("InheritRedirectDiskDrivesCustom", typeof(bool));
            dataTable.Columns.Add("InheritRedirectKeys", typeof(bool));
            dataTable.Columns.Add("InheritRedirectPorts", typeof(bool));
            dataTable.Columns.Add("InheritRedirectPrinters", typeof(bool));
            dataTable.Columns.Add("InheritRedirectSmartCards", typeof(bool));
            dataTable.Columns.Add("InheritRedirectSound", typeof(bool));
            dataTable.Columns.Add("InheritRenderingEngine", typeof(bool));
            dataTable.Columns.Add("InheritResolution", typeof(bool));
            dataTable.Columns.Add("InheritSSHOptions", typeof(bool));
            dataTable.Columns.Add("InheritSSHTunnelConnectionName", typeof(bool));
            dataTable.Columns.Add("InheritSoundQuality", typeof(bool));
            dataTable.Columns.Add("InheritUseConsoleSession", typeof(bool));
            dataTable.Columns.Add("InheritUseCredSsp", typeof(bool));
            dataTable.Columns.Add("InheritUseEnhancedMode", typeof(bool));
            dataTable.Columns.Add("InheritUseRCG", typeof(bool));
            dataTable.Columns.Add("InheritUseRestrictedAdmin", typeof(bool));
            dataTable.Columns.Add("InheritUseVmId", typeof(bool));
            dataTable.Columns.Add("InheritUserField", typeof(bool));
            dataTable.Columns.Add("InheritUserViaAPI", typeof(bool));
            dataTable.Columns.Add("InheritUsername", typeof(bool));
            dataTable.Columns.Add("InheritVNCAuthMode", typeof(bool));
            dataTable.Columns.Add("InheritVNCColors", typeof(bool));
            dataTable.Columns.Add("InheritVNCCompression", typeof(bool));
            dataTable.Columns.Add("InheritVNCEncoding", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyIP", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyPassword", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyPort", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyType", typeof(bool));
            dataTable.Columns.Add("InheritVNCProxyUsername", typeof(bool));
            dataTable.Columns.Add("InheritVNCSmartSizeMode", typeof(bool));
            dataTable.Columns.Add("InheritVNCViewOnly", typeof(bool));
            dataTable.Columns.Add("InheritVmId", typeof(bool));
            dataTable.Columns.Add("LastChange", MiscTools.DBTimeStampType());
            dataTable.Columns.Add("LoadBalanceInfo", typeof(string));
            dataTable.Columns.Add("MacAddress", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("OpeningCommand", typeof(string));
            dataTable.Columns.Add("Panel", typeof(string));
            dataTable.Columns.Add("ParentID", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("Port", typeof(int));
            dataTable.Columns.Add("PositionID", typeof(int));
            dataTable.Columns.Add("PostExtApp", typeof(string));
            dataTable.Columns.Add("PreExtApp", typeof(string));
            dataTable.Columns.Add("Protocol", typeof(string));
            dataTable.Columns.Add("PuttySession", typeof(string));
            dataTable.Columns.Add("RDGatewayDomain", typeof(string));
            dataTable.Columns.Add("RDGatewayExternalCredentialProvider", typeof(string));
            dataTable.Columns.Add("RDGatewayHostname", typeof(string));
            dataTable.Columns.Add("RDGatewayPassword", typeof(string));
            dataTable.Columns.Add("RDGatewayUsageMethod", typeof(string));
            dataTable.Columns.Add("RDGatewayUseConnectionCredentials", typeof(string));
            dataTable.Columns.Add("RDGatewayUserViaAPI", typeof(string));
            dataTable.Columns.Add("RDGatewayUsername", typeof(string));
            dataTable.Columns.Add("RDPAlertIdleTimeout", typeof(bool));
            dataTable.Columns.Add("RDPAuthenticationLevel", typeof(string));
            dataTable.Columns.Add("RDPMinutesToIdleTimeout", typeof(int));
            dataTable.Columns.Add("RdpVersion", typeof(string));
            dataTable.Columns.Add("RedirectAudioCapture", typeof(bool));
            dataTable.Columns.Add("RedirectClipboard", typeof(bool));
            dataTable.Columns.Add("RedirectDiskDrives", typeof(string));
            dataTable.Columns.Add("RedirectDiskDrivesCustom", typeof(string));
            dataTable.Columns.Add("RedirectKeys", typeof(bool));
            dataTable.Columns.Add("RedirectPorts", typeof(bool));
            dataTable.Columns.Add("RedirectPrinters", typeof(bool));
            dataTable.Columns.Add("RedirectSmartCards", typeof(bool));
            dataTable.Columns.Add("RedirectSound", typeof(string));
            dataTable.Columns.Add("RenderingEngine", typeof(string));
            dataTable.Columns.Add("Resolution", typeof(string));
            dataTable.Columns.Add("SSHOptions", typeof(string));
            dataTable.Columns.Add("SSHTunnelConnectionName", typeof(string));
            dataTable.Columns.Add("SoundQuality", typeof(string));
            dataTable.Columns.Add("StartProgram", typeof(string));
            dataTable.Columns.Add("StartProgramWorkDir", typeof(string));
            dataTable.Columns.Add("Type", typeof(string));
            dataTable.Columns.Add("UseCredSsp", typeof(bool));
            dataTable.Columns.Add("UseEnhancedMode", typeof(bool));
            dataTable.Columns.Add("UseRCG", typeof(bool));
            dataTable.Columns.Add("UseRestrictedAdmin", typeof(bool));
            dataTable.Columns.Add("UseVmId", typeof(bool));
            dataTable.Columns.Add("UserField", typeof(string));
            dataTable.Columns.Add("UserViaAPI", typeof(string));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("VNCAuthMode", typeof(string));
            dataTable.Columns.Add("VNCColors", typeof(string));
            dataTable.Columns.Add("VNCCompression", typeof(string));
            dataTable.Columns.Add("VNCEncoding", typeof(string));
            dataTable.Columns.Add("VNCProxyIP", typeof(string));
            dataTable.Columns.Add("VNCProxyPassword", typeof(string));
            dataTable.Columns.Add("VNCProxyPort", typeof(int));
            dataTable.Columns.Add("VNCProxyType", typeof(string));
            dataTable.Columns.Add("VNCProxyUsername", typeof(string));
            dataTable.Columns.Add("VNCSmartSizeMode", typeof(string));
            dataTable.Columns.Add("VNCViewOnly", typeof(bool));
            dataTable.Columns.Add("VmId", typeof(string));
            dataTable.Columns[0].AutoIncrement = true;
            dataTable.Columns.Add("ID", typeof(int));
        }

        private void SetPrimaryKey(DataTable dataTable)
        {
            dataTable.PrimaryKey = new[] { dataTable.Columns["ConstantID"] };
        }

        private void SerializeNodesRecursive(ConnectionInfo connectionInfo)
        {
            if (connectionInfo is not RootNodeInfo)
            {
                SerializeConnectionInfo(connectionInfo);
            }

            var containerInfo = connectionInfo as ContainerInfo;
            if (containerInfo == null) return;

            foreach (ConnectionInfo child in containerInfo.Children)
            {
                SerializeNodesRecursive(child);
            }
        }

        private bool IsRowUpdated(ConnectionInfo connectionInfo, DataRow dataRow)
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

            isFieldNotChange = isFieldNotChange && dataRow["AutomaticResize"].Equals(connectionInfo.AutomaticResize);
            isFieldNotChange = isFieldNotChange && dataRow["CacheBitmaps"].Equals(connectionInfo.CacheBitmaps);
            isFieldNotChange = isFieldNotChange && dataRow["Colors"].Equals(connectionInfo.Colors.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["ConnectToConsole"].Equals(connectionInfo.UseConsoleSession);
            isFieldNotChange = isFieldNotChange && dataRow["Connected"].Equals(false); // TODO: this column can eventually be removed. we now save this property locally
            isFieldNotChange = isFieldNotChange && dataRow["DisableCursorBlinking"].Equals(connectionInfo.DisableCursorBlinking);
            isFieldNotChange = isFieldNotChange && dataRow["DisableCursorShadow"].Equals(connectionInfo.DisableCursorShadow);
            isFieldNotChange = isFieldNotChange && dataRow["DisableFullWindowDrag"].Equals(connectionInfo.DisableFullWindowDrag);
            isFieldNotChange = isFieldNotChange && dataRow["DisableMenuAnimations"].Equals(connectionInfo.DisableMenuAnimations);
            isFieldNotChange = isFieldNotChange && dataRow["DisplayThemes"].Equals(connectionInfo.DisplayThemes);
            isFieldNotChange = isFieldNotChange && dataRow["DisplayWallpaper"].Equals(connectionInfo.DisplayWallpaper);
            isFieldNotChange = isFieldNotChange && dataRow["EC2InstanceId"].Equals(connectionInfo.EC2InstanceId);
            isFieldNotChange = isFieldNotChange && dataRow["EC2Region"].Equals(connectionInfo.EC2Region);
            isFieldNotChange = isFieldNotChange && dataRow["EnableDesktopComposition"].Equals(connectionInfo.EnableDesktopComposition);
            isFieldNotChange = isFieldNotChange && dataRow["EnableFontSmoothing"].Equals(connectionInfo.EnableFontSmoothing);
            isFieldNotChange = isFieldNotChange && dataRow["ExtApp"].Equals(connectionInfo.ExtApp);
            isFieldNotChange = isFieldNotChange && dataRow["ExternalAddressProvider"].Equals(connectionInfo.ExternalAddressProvider);
            isFieldNotChange = isFieldNotChange && dataRow["ExternalCredentialProvider"].Equals(connectionInfo.ExternalCredentialProvider);
            isFieldNotChange = isFieldNotChange && dataRow["Hostname"].Equals(connectionInfo.Hostname);
            isFieldNotChange = isFieldNotChange && dataRow["LoadBalanceInfo"].Equals(connectionInfo.LoadBalanceInfo);
            isFieldNotChange = isFieldNotChange && dataRow["MacAddress"].Equals(connectionInfo.MacAddress);
            isFieldNotChange = isFieldNotChange && dataRow["OpeningCommand"].Equals(connectionInfo.OpeningCommand);
            isFieldNotChange = isFieldNotChange && dataRow["Port"].Equals(connectionInfo.Port);
            isFieldNotChange = isFieldNotChange && dataRow["PostExtApp"].Equals(connectionInfo.PostExtApp);
            isFieldNotChange = isFieldNotChange && dataRow["PreExtApp"].Equals(connectionInfo.PreExtApp);
            isFieldNotChange = isFieldNotChange && dataRow["Protocol"].Equals(connectionInfo.Protocol.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["PuttySession"].Equals(connectionInfo.PuttySession);
            isFieldNotChange = isFieldNotChange && dataRow["RDGatewayDomain"].Equals(connectionInfo.RDGatewayDomain);
            isFieldNotChange = isFieldNotChange && dataRow["RDGatewayExternalCredentialProvider"].Equals(connectionInfo.RDGatewayExternalCredentialProvider);
            isFieldNotChange = isFieldNotChange && dataRow["RDGatewayHostname"].Equals(connectionInfo.RDGatewayHostname);
            isFieldNotChange = isFieldNotChange && dataRow["RDGatewayUsageMethod"].Equals(connectionInfo.RDGatewayUsageMethod.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["RDGatewayUseConnectionCredentials"].Equals(connectionInfo.RDGatewayUseConnectionCredentials.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["RDGatewayUserViaAPI"].Equals(connectionInfo.RDGatewayUserViaAPI);
            isFieldNotChange = isFieldNotChange && dataRow["RDGatewayUsername"].Equals(connectionInfo.RDGatewayUsername);
            isFieldNotChange = isFieldNotChange && dataRow["RDPAlertIdleTimeout"].Equals(connectionInfo.RDPAlertIdleTimeout);
            isFieldNotChange = isFieldNotChange && dataRow["RDPAuthenticationLevel"].Equals(connectionInfo.RDPAuthenticationLevel.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["RDPMinutesToIdleTimeout"].Equals(connectionInfo.RDPMinutesToIdleTimeout);
            isFieldNotChange = isFieldNotChange && dataRow["RdpVersion"].Equals(connectionInfo.RdpVersion.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["RedirectAudioCapture"].Equals(connectionInfo.RedirectAudioCapture);
            isFieldNotChange = isFieldNotChange && dataRow["RedirectClipboard"].Equals(connectionInfo.RedirectClipboard);
            isFieldNotChange = isFieldNotChange && dataRow["RedirectDiskDrives"].Equals(connectionInfo.RedirectDiskDrives.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["RedirectDiskDrivesCustom"].Equals(connectionInfo.RedirectDiskDrivesCustom);
            isFieldNotChange = isFieldNotChange && dataRow["RedirectKeys"].Equals(connectionInfo.RedirectKeys);
            isFieldNotChange = isFieldNotChange && dataRow["RedirectPorts"].Equals(connectionInfo.RedirectPorts);
            isFieldNotChange = isFieldNotChange && dataRow["RedirectPrinters"].Equals(connectionInfo.RedirectPrinters);
            isFieldNotChange = isFieldNotChange && dataRow["RedirectSmartCards"].Equals(connectionInfo.RedirectSmartCards);
            isFieldNotChange = isFieldNotChange && dataRow["RedirectSound"].Equals(connectionInfo.RedirectSound.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["RenderingEngine"].Equals(connectionInfo.RenderingEngine.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["Resolution"].Equals(connectionInfo.Resolution.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["SoundQuality"].Equals(connectionInfo.SoundQuality.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["StartProgram"].Equals(connectionInfo.RDPStartProgram);
            isFieldNotChange = isFieldNotChange && dataRow["StartProgramWorkDir"].Equals(connectionInfo.RDPStartProgramWorkDir);
            isFieldNotChange = isFieldNotChange && dataRow["UseCredSsp"].Equals(connectionInfo.UseCredSsp);
            isFieldNotChange = isFieldNotChange && dataRow["UseEnhancedMode"].Equals(connectionInfo.UseEnhancedMode);
            isFieldNotChange = isFieldNotChange && dataRow["UseRCG"].Equals(connectionInfo.UseRCG);
            isFieldNotChange = isFieldNotChange && dataRow["UseRestrictedAdmin"].Equals(connectionInfo.UseRestrictedAdmin);
            isFieldNotChange = isFieldNotChange && dataRow["UseVmId"].Equals(connectionInfo.UseVmId);
            isFieldNotChange = isFieldNotChange && dataRow["UserField"].Equals(connectionInfo.UserField);
            isFieldNotChange = isFieldNotChange && dataRow["UserViaAPI"].Equals(connectionInfo.UserViaAPI);
            isFieldNotChange = isFieldNotChange && dataRow["VNCAuthMode"].Equals(connectionInfo.VNCAuthMode.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["VNCColors"].Equals(connectionInfo.VNCColors.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["VNCCompression"].Equals(connectionInfo.VNCCompression.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["VNCEncoding"].Equals(connectionInfo.VNCEncoding.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["VNCProxyIP"].Equals(connectionInfo.VNCProxyIP);
            isFieldNotChange = isFieldNotChange && dataRow["VNCProxyPort"].Equals(connectionInfo.VNCProxyPort);
            isFieldNotChange = isFieldNotChange && dataRow["VNCProxyType"].Equals(connectionInfo.VNCProxyType.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["VNCProxyUsername"].Equals(connectionInfo.VNCProxyUsername);
            isFieldNotChange = isFieldNotChange && dataRow["VNCSmartSizeMode"].Equals(connectionInfo.VNCSmartSizeMode.ToString());
            isFieldNotChange = isFieldNotChange && dataRow["VNCViewOnly"].Equals(connectionInfo.VNCViewOnly);
            isFieldNotChange = isFieldNotChange && dataRow["VmId"].Equals(connectionInfo.VmId);

            var isInheritanceFieldNotChange = false;
            if (_saveFilter.SaveInheritance)
            {
                isInheritanceFieldNotChange =
                    dataRow["InheritAutomaticResize"].Equals(connectionInfo.Inheritance.AutomaticResize) &&
                    dataRow["InheritCacheBitmaps"].Equals(connectionInfo.Inheritance.CacheBitmaps) &&
                    dataRow["InheritColors"].Equals(connectionInfo.Inheritance.Colors) &&
                    dataRow["InheritDescription"].Equals(connectionInfo.Inheritance.Description) &&
                    dataRow["InheritDisableCursorBlinking"].Equals(connectionInfo.Inheritance.DisableCursorBlinking) &&
                    dataRow["InheritDisableCursorShadow"].Equals(connectionInfo.Inheritance.DisableCursorShadow) &&
                    dataRow["InheritDisableFullWindowDrag"].Equals(connectionInfo.Inheritance.DisableFullWindowDrag) &&
                    dataRow["InheritDisableMenuAnimations"].Equals(connectionInfo.Inheritance.DisableMenuAnimations) &&
                    dataRow["InheritDisplayThemes"].Equals(connectionInfo.Inheritance.DisplayThemes) &&
                    dataRow["InheritDisplayWallpaper"].Equals(connectionInfo.Inheritance.DisplayWallpaper) &&
                    dataRow["InheritDomain"].Equals(connectionInfo.Inheritance.Domain) &&
                    dataRow["InheritEnableDesktopComposition"].Equals(connectionInfo.Inheritance.EnableDesktopComposition) &&
                    dataRow["InheritEnableFontSmoothing"].Equals(connectionInfo.Inheritance.EnableFontSmoothing) &&
                    dataRow["InheritExtApp"].Equals(connectionInfo.Inheritance.ExtApp) &&
                    dataRow["InheritExternalCredentialProvider"].Equals(connectionInfo.Inheritance.ExternalCredentialProvider) &&
                    dataRow["InheritIcon"].Equals(connectionInfo.Inheritance.Icon) &&
                    dataRow["InheritLoadBalanceInfo"].Equals(connectionInfo.Inheritance.LoadBalanceInfo) &&
                    dataRow["InheritMacAddress"].Equals(connectionInfo.Inheritance.MacAddress) &&
                    dataRow["InheritOpeningCommand"].Equals(connectionInfo.Inheritance.OpeningCommand) &&
                    dataRow["InheritPanel"].Equals(connectionInfo.Inheritance.Panel) &&
                    dataRow["InheritPassword"].Equals(connectionInfo.Inheritance.Password) &&
                    dataRow["InheritPort"].Equals(connectionInfo.Inheritance.Port) &&
                    dataRow["InheritPostExtApp"].Equals(connectionInfo.Inheritance.PostExtApp) &&
                    dataRow["InheritPreExtApp"].Equals(connectionInfo.Inheritance.PreExtApp) &&
                    dataRow["InheritProtocol"].Equals(connectionInfo.Inheritance.Protocol) &&
                    dataRow["InheritPuttySession"].Equals(connectionInfo.Inheritance.PuttySession) &&
                    dataRow["InheritRDGatewayDomain"].Equals(connectionInfo.Inheritance.RDGatewayDomain) &&
                    dataRow["InheritRDGatewayExternalCredentialProvider"].Equals(connectionInfo.Inheritance.RDGatewayExternalCredentialProvider) &&
                    dataRow["InheritRDGatewayHostname"].Equals(connectionInfo.Inheritance.RDGatewayHostname) &&
                    dataRow["InheritRDGatewayPassword"].Equals(connectionInfo.Inheritance.RDGatewayPassword) &&
                    dataRow["InheritRDGatewayUsageMethod"].Equals(connectionInfo.Inheritance.RDGatewayUsageMethod) &&
                    dataRow["InheritRDGatewayUseConnectionCredentials"].Equals(connectionInfo.Inheritance.RDGatewayUseConnectionCredentials) &&
                    dataRow["InheritRDGatewayUsername"].Equals(connectionInfo.Inheritance.RDGatewayUsername) &&
                    dataRow["InheritRDGatewayUserViaAPI"].Equals(connectionInfo.Inheritance.RDGatewayUserViaAPI) &&
                    dataRow["InheritRDPAlertIdleTimeout"].Equals(connectionInfo.Inheritance.RDPAlertIdleTimeout) &&
                    dataRow["InheritRDPAuthenticationLevel"].Equals(connectionInfo.Inheritance.RDPAuthenticationLevel) &&
                    dataRow["InheritRDPMinutesToIdleTimeout"].Equals(connectionInfo.Inheritance.RDPMinutesToIdleTimeout) &&
                    dataRow["InheritRdpVersion"].Equals(connectionInfo.Inheritance.RdpVersion) &&
                    dataRow["InheritRedirectAudioCapture"].Equals(connectionInfo.Inheritance.RedirectAudioCapture) &&
                    dataRow["InheritRedirectClipboard"].Equals(connectionInfo.Inheritance.RedirectClipboard) &&
                    dataRow["InheritRedirectDiskDrives"].Equals(connectionInfo.Inheritance.RedirectDiskDrives) &&
                    dataRow["InheritRedirectDiskDrivesCustom"].Equals(connectionInfo.Inheritance.RedirectDiskDrivesCustom) &&
                    dataRow["InheritRedirectKeys"].Equals(connectionInfo.Inheritance.RedirectKeys) &&
                    dataRow["InheritRedirectPorts"].Equals(connectionInfo.Inheritance.RedirectPorts) &&
                    dataRow["InheritRedirectPrinters"].Equals(connectionInfo.Inheritance.RedirectPrinters) &&
                    dataRow["InheritRedirectSmartCards"].Equals(connectionInfo.Inheritance.RedirectSmartCards) &&
                    dataRow["InheritRedirectSound"].Equals(connectionInfo.Inheritance.RedirectSound) &&
                    dataRow["InheritRenderingEngine"].Equals(connectionInfo.Inheritance.RenderingEngine) &&
                    dataRow["InheritResolution"].Equals(connectionInfo.Inheritance.Resolution) &&
                    dataRow["InheritSoundQuality"].Equals(connectionInfo.Inheritance.SoundQuality) &&
                    dataRow["InheritUseConsoleSession"].Equals(connectionInfo.Inheritance.UseConsoleSession) &&
                    dataRow["InheritUseCredSsp"].Equals(connectionInfo.Inheritance.UseCredSsp) &&
                    dataRow["InheritUseEnhancedMode"].Equals(connectionInfo.Inheritance.UseEnhancedMode) &&
                    dataRow["InheritUseRCG"].Equals(connectionInfo.Inheritance.UseRCG) &&
                    dataRow["InheritUseRestrictedAdmin"].Equals(connectionInfo.Inheritance.UseRestrictedAdmin) &&
                    dataRow["InheritUserField"].Equals(connectionInfo.Inheritance.UserField) &&
                    dataRow["InheritUsername"].Equals(connectionInfo.Inheritance.Username) &&
                    dataRow["InheritUserViaAPI"].Equals(connectionInfo.Inheritance.UserViaAPI) &&
                    dataRow["InheritUseVmId"].Equals(connectionInfo.Inheritance.UseVmId) &&
                    dataRow["InheritVmId"].Equals(connectionInfo.Inheritance.VmId) &&
                    dataRow["InheritVNCAuthMode"].Equals(connectionInfo.Inheritance.VNCAuthMode) &&
                    dataRow["InheritVNCColors"].Equals(connectionInfo.Inheritance.VNCColors) &&
                    dataRow["InheritVNCCompression"].Equals(connectionInfo.Inheritance.VNCCompression) &&
                    dataRow["InheritVNCEncoding"].Equals(connectionInfo.Inheritance.VNCEncoding) &&
                    dataRow["InheritVNCProxyIP"].Equals(connectionInfo.Inheritance.VNCProxyIP) &&
                    dataRow["InheritVNCProxyPassword"].Equals(connectionInfo.Inheritance.VNCProxyPassword) &&
                    dataRow["InheritVNCProxyPort"].Equals(connectionInfo.Inheritance.VNCProxyPort) &&
                    dataRow["InheritVNCProxyType"].Equals(connectionInfo.Inheritance.VNCProxyType) &&
                    dataRow["InheritVNCProxyUsername"].Equals(connectionInfo.Inheritance.VNCProxyUsername) &&
                    dataRow["InheritVNCSmartSizeMode"].Equals(connectionInfo.Inheritance.VNCSmartSizeMode) &&
                    dataRow["InheritVNCViewOnly"].Equals(connectionInfo.Inheritance.VNCViewOnly);
            }
            else
            {
                isInheritanceFieldNotChange =
                    dataRow["InheritAutomaticResize"].Equals(false) &&
                    dataRow["InheritCacheBitmaps"].Equals(false) &&
                    dataRow["InheritColors"].Equals(false) &&
                    dataRow["InheritDescription"].Equals(false) &&
                    dataRow["InheritDisableCursorBlinking"].Equals(false) &&
                    dataRow["InheritDisableCursorShadow"].Equals(false) &&
                    dataRow["InheritDisableFullWindowDrag"].Equals(false) &&
                    dataRow["InheritDisableMenuAnimations"].Equals(false) &&
                    dataRow["InheritDisplayThemes"].Equals(false) &&
                    dataRow["InheritDisplayWallpaper"].Equals(false) &&
                    dataRow["InheritDomain"].Equals(false) &&
                    dataRow["InheritEnableDesktopComposition"].Equals(false) &&
                    dataRow["InheritEnableFontSmoothing"].Equals(false) &&
                    dataRow["InheritExtApp"].Equals(false) &&
                    dataRow["InheritExternalCredentialProvider"].Equals(false) &&
                    dataRow["InheritIcon"].Equals(false) &&
                    dataRow["InheritLoadBalanceInfo"].Equals(false) &&
                    dataRow["InheritMacAddress"].Equals(false) &&
                    dataRow["InheritOpeningCommand"].Equals(false) &&
                    dataRow["InheritPanel"].Equals(false) &&
                    dataRow["InheritPassword"].Equals(false) &&
                    dataRow["InheritPort"].Equals(false) &&
                    dataRow["InheritPostExtApp"].Equals(false) &&
                    dataRow["InheritPreExtApp"].Equals(false) &&
                    dataRow["InheritProtocol"].Equals(false) &&
                    dataRow["InheritPuttySession"].Equals(false) &&
                    dataRow["InheritRDGatewayDomain"].Equals(false) &&
                    dataRow["InheritRDGatewayExternalCredentialProvider"].Equals(connectionInfo.Inheritance.RDGatewayExternalCredentialProvider) &&
                    dataRow["InheritRDGatewayHostname"].Equals(false) &&
                    dataRow["InheritRDGatewayPassword"].Equals(false) &&
                    dataRow["InheritRDGatewayUsageMethod"].Equals(false) &&
                    dataRow["InheritRDGatewayUseConnectionCredentials"].Equals(false) &&
                    dataRow["InheritRDGatewayUsername"].Equals(false) &&
                    dataRow["InheritRDGatewayUserViaAPI"].Equals(false) &&
                    dataRow["InheritRDPAlertIdleTimeout"].Equals(false) &&
                    dataRow["InheritRDPAuthenticationLevel"].Equals(false) &&
                    dataRow["InheritRDPMinutesToIdleTimeout"].Equals(false) &&
                    dataRow["InheritRdpVersion"].Equals(false) &&
                    dataRow["InheritRedirectAudioCapture"].Equals(false) &&
                    dataRow["InheritRedirectClipboard"].Equals(false) &&
                    dataRow["InheritRedirectDiskDrives"].Equals(false) &&
                    dataRow["InheritRedirectDiskDrivesCustom"].Equals(false) &&
                    dataRow["InheritRedirectKeys"].Equals(false) &&
                    dataRow["InheritRedirectPorts"].Equals(false) &&
                    dataRow["InheritRedirectPrinters"].Equals(false) &&
                    dataRow["InheritRedirectSmartCards"].Equals(false) &&
                    dataRow["InheritRedirectSound"].Equals(false) &&
                    dataRow["InheritRenderingEngine"].Equals(false) &&
                    dataRow["InheritResolution"].Equals(false) &&
                    dataRow["InheritSoundQuality"].Equals(false) &&
                    dataRow["InheritUseConsoleSession"].Equals(false) &&
                    dataRow["InheritUseCredSsp"].Equals(false) &&
                    dataRow["InheritUseRCG"].Equals(false) &&
                    dataRow["InheritUseRestrictedAdmin"].Equals(false) &&
                    dataRow["InheritUserField"].Equals(false) &&
                    dataRow["InheritUsername"].Equals(false) &&
                    dataRow["InheritUserViaAPI"].Equals(false) &&
                    dataRow["InheritVNCAuthMode"].Equals(false) &&
                    dataRow["InheritVNCColors"].Equals(false) &&
                    dataRow["InheritVNCCompression"].Equals(false) &&
                    dataRow["InheritVNCEncoding"].Equals(false) &&
                    dataRow["InheritVNCProxyIP"].Equals(false) &&
                    dataRow["InheritVNCProxyPassword"].Equals(false) &&
                    dataRow["InheritVNCProxyPort"].Equals(false) &&
                    dataRow["InheritVNCProxyType"].Equals(false) &&
                    dataRow["InheritVNCProxyUsername"].Equals(false) &&
                    dataRow["InheritVNCSmartSizeMode"].Equals(false) &&
                    dataRow["InheritVNCViewOnly"].Equals(false);
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
                _sourcePrimaryKeyDict.Remove(connectionInfo.ConstantID);
            }
            var tmp = IsRowUpdated(connectionInfo, dataRow);
            if (!tmp)
            {
                return;
            }

            dataRow["AutomaticResize"] = connectionInfo.AutomaticResize;
            dataRow["CacheBitmaps"] = connectionInfo.CacheBitmaps;
            dataRow["Colors"] = connectionInfo.Colors;
            dataRow["ConnectToConsole"] = connectionInfo.UseConsoleSession;
            dataRow["Connected"] = false;
            dataRow["Description"] = connectionInfo.Description; // TODO: this column can eventually be removed. we now save this property locally
            dataRow["DisableCursorBlinking"] = connectionInfo.DisableCursorBlinking;
            dataRow["DisableCursorShadow"] = connectionInfo.DisableCursorShadow;
            dataRow["DisableFullWindowDrag"] = connectionInfo.DisableFullWindowDrag;
            dataRow["DisableMenuAnimations"] = connectionInfo.DisableMenuAnimations;
            dataRow["DisplayThemes"] = connectionInfo.DisplayThemes;
            dataRow["DisplayWallpaper"] = connectionInfo.DisplayWallpaper;
            dataRow["Domain"] = _saveFilter.SaveDomain ? connectionInfo.Domain : "";
            dataRow["EnableDesktopComposition"] = connectionInfo.EnableDesktopComposition;
            dataRow["EnableFontSmoothing"] = connectionInfo.EnableFontSmoothing;
            dataRow["Expanded"] = false;
            dataRow["ExtApp"] = connectionInfo.ExtApp;
            dataRow["Favorite"] = connectionInfo.Favorite;
            dataRow["Hostname"] = connectionInfo.Hostname;
            dataRow["ICAEncryptionStrength"] = string.Empty;
            dataRow["Icon"] = connectionInfo.Icon;
            dataRow["LastChange"] = MiscTools.DBTimeStampNow();
            dataRow["LoadBalanceInfo"] = connectionInfo.LoadBalanceInfo;
            dataRow["MacAddress"] = connectionInfo.MacAddress;
            dataRow["Name"] = connectionInfo.Name;
            dataRow["OpeningCommand"] = connectionInfo.OpeningCommand;
            dataRow["OpeningCommand"] = connectionInfo.OpeningCommand;
            dataRow["Panel"] = connectionInfo.Panel;
            dataRow["ParentID"] = connectionInfo.Parent?.ConstantID ?? "";
            dataRow["Password"] = _saveFilter.SavePassword ? _cryptographyProvider.Encrypt(connectionInfo.Password, _encryptionKey) : "";
            dataRow["Port"] = connectionInfo.Port;
            dataRow["PositionID"] = _currentNodeIndex;
            dataRow["PostExtApp"] = connectionInfo.PostExtApp;
            dataRow["PreExtApp"] = connectionInfo.PreExtApp;
            dataRow["Protocol"] = connectionInfo.Protocol;
            dataRow["PuttySession"] = connectionInfo.PuttySession;
            dataRow["RDGatewayDomain"] = connectionInfo.RDGatewayDomain;
            dataRow["RDGatewayHostname"] = connectionInfo.RDGatewayHostname;
            dataRow["RDGatewayPassword"] = _cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword, _encryptionKey);
            dataRow["RDGatewayUsageMethod"] = connectionInfo.RDGatewayUsageMethod;
            dataRow["RDGatewayUseConnectionCredentials"] = connectionInfo.RDGatewayUseConnectionCredentials;
            dataRow["RDGatewayUsername"] = connectionInfo.RDGatewayUsername;
            dataRow["RDPAlertIdleTimeout"] = connectionInfo.RDPAlertIdleTimeout;
            dataRow["RDPAuthenticationLevel"] = connectionInfo.RDPAuthenticationLevel;
            dataRow["RDPMinutesToIdleTimeout"] = connectionInfo.RDPMinutesToIdleTimeout;
            dataRow["RdpVersion"] = connectionInfo.RdpVersion;
            dataRow["RedirectAudioCapture"] = connectionInfo.RedirectAudioCapture;
            dataRow["RedirectClipboard"] = connectionInfo.RedirectClipboard;
            dataRow["RedirectDiskDrives"] = connectionInfo.RedirectDiskDrives;
            dataRow["RedirectDiskDrivesCustom"] = connectionInfo.RedirectDiskDrivesCustom;
            dataRow["RedirectKeys"] = connectionInfo.RedirectKeys;
            dataRow["RedirectPorts"] = connectionInfo.RedirectPorts;
            dataRow["RedirectPrinters"] = connectionInfo.RedirectPrinters;
            dataRow["RedirectSmartCards"] = connectionInfo.RedirectSmartCards;
            dataRow["RedirectSound"] = connectionInfo.RedirectSound;
            dataRow["RenderingEngine"] = connectionInfo.RenderingEngine;
            dataRow["Resolution"] = connectionInfo.Resolution;
            dataRow["SSHOptions"] = connectionInfo.SSHOptions;
            dataRow["SSHTunnelConnectionName"] = connectionInfo.SSHTunnelConnectionName;
            dataRow["SoundQuality"] = connectionInfo.SoundQuality;
            dataRow["StartProgram"] = connectionInfo.RDPStartProgram;
            dataRow["StartProgramWorkDir"] = connectionInfo.RDPStartProgramWorkDir;
            dataRow["Type"] = connectionInfo.GetTreeNodeType().ToString();
            dataRow["UseCredSsp"] = connectionInfo.UseCredSsp;
            dataRow["UseEnhancedMode"] = connectionInfo.UseEnhancedMode;
            dataRow["UseRCG"] = connectionInfo.UseRCG;
            dataRow["UseRestrictedAdmin"] = connectionInfo.UseRestrictedAdmin;
            dataRow["UseVmId"] = connectionInfo.UseVmId;
            dataRow["UserField"] = connectionInfo.UserField;
            dataRow["Username"] = _saveFilter.SaveUsername ? connectionInfo.Username : "";
            dataRow["VNCAuthMode"] = connectionInfo.VNCAuthMode;
            dataRow["VNCColors"] = connectionInfo.VNCColors;
            dataRow["VNCCompression"] = connectionInfo.VNCCompression;
            dataRow["VNCEncoding"] = connectionInfo.VNCEncoding;
            dataRow["VNCProxyIP"] = connectionInfo.VNCProxyIP;
            dataRow["VNCProxyPassword"] = _cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword, _encryptionKey);
            dataRow["VNCProxyPort"] = connectionInfo.VNCProxyPort;
            dataRow["VNCProxyType"] = connectionInfo.VNCProxyType;
            dataRow["VNCProxyUsername"] = connectionInfo.VNCProxyUsername;
            dataRow["VNCSmartSizeMode"] = connectionInfo.VNCSmartSizeMode;
            dataRow["VNCViewOnly"] = connectionInfo.VNCViewOnly; // TODO: this column can eventually be removed. we now save this property locally
            dataRow["VmId"] = connectionInfo.VmId;

            if (_saveFilter.SaveInheritance)
            {
                dataRow["InheritAutomaticResize"] = connectionInfo.Inheritance.AutomaticResize;
                dataRow["InheritColors"] = connectionInfo.Inheritance.Colors;
                dataRow["InheritDescription"] = connectionInfo.Inheritance.Description;
                dataRow["InheritDisableCursorBlinking"] = connectionInfo.Inheritance.DisableCursorBlinking;
                dataRow["InheritDisableCursorShadow"] = connectionInfo.Inheritance.DisableCursorShadow;
                dataRow["InheritDisableFullWindowDrag"] = connectionInfo.Inheritance.DisableFullWindowDrag;
                dataRow["InheritDisableMenuAnimations"] = connectionInfo.Inheritance.DisableMenuAnimations;
                dataRow["InheritDisplayThemes"] = connectionInfo.Inheritance.DisplayThemes;
                dataRow["InheritDisplayWallpaper"] = connectionInfo.Inheritance.DisplayWallpaper;
                dataRow["InheritDomain"] = connectionInfo.Inheritance.Domain;
                dataRow["InheritEnableDesktopComposition"] = connectionInfo.Inheritance.EnableDesktopComposition;
                dataRow["InheritEnableFontSmoothing"] = connectionInfo.Inheritance.EnableFontSmoothing;
                dataRow["InheritExtApp"] = connectionInfo.Inheritance.ExtApp;
                dataRow["InheritExternalCredentialProvider"] = connectionInfo.Inheritance.ExternalCredentialProvider;
                dataRow["InheritFavorite"] = connectionInfo.Inheritance.Favorite;
                dataRow["InheritICAEncryptionStrength"] = false;
                dataRow["InheritIcon"] = connectionInfo.Inheritance.Icon;
                dataRow["InheritLoadBalanceInfo"] = connectionInfo.Inheritance.LoadBalanceInfo;
                dataRow["InheritMacAddress"] = connectionInfo.Inheritance.MacAddress;
                dataRow["InheritOpeningCommand"] = connectionInfo.Inheritance.OpeningCommand;
                dataRow["InheritOpeningCommand"] = connectionInfo.Inheritance.OpeningCommand;
                dataRow["InheritPanel"] = connectionInfo.Inheritance.Panel;
                dataRow["InheritPassword"] = connectionInfo.Inheritance.Password;
                dataRow["InheritPort"] = connectionInfo.Inheritance.Port;
                dataRow["InheritPostExtApp"] = connectionInfo.Inheritance.PostExtApp;
                dataRow["InheritPreExtApp"] = connectionInfo.Inheritance.PreExtApp;
                dataRow["InheritProtocol"] = connectionInfo.Inheritance.Protocol;
                dataRow["InheritPuttySession"] = connectionInfo.Inheritance.PuttySession;
                dataRow["InheritRDGatewayDomain"] = connectionInfo.Inheritance.RDGatewayDomain;
                dataRow["InheritRDGatewayExternalCredentialProvider"] = connectionInfo.Inheritance.RDGatewayExternalCredentialProvider;
                dataRow["InheritRDGatewayHostname"] = connectionInfo.Inheritance.RDGatewayHostname;
                dataRow["InheritRDGatewayPassword"] = connectionInfo.Inheritance.RDGatewayPassword;
                dataRow["InheritRDGatewayUsageMethod"] = connectionInfo.Inheritance.RDGatewayUsageMethod;
                dataRow["InheritRDGatewayUseConnectionCredentials"] = connectionInfo.Inheritance.RDGatewayUseConnectionCredentials;
                dataRow["InheritRDGatewayUserViaAPI"] = connectionInfo.Inheritance.RDGatewayUserViaAPI;
                dataRow["InheritRDGatewayUsername"] = connectionInfo.Inheritance.RDGatewayUsername;
                dataRow["InheritRDPAlertIdleTimeout"] = connectionInfo.Inheritance.RDPAlertIdleTimeout;
                dataRow["InheritRDPAuthenticationLevel"] = connectionInfo.Inheritance.RDPAuthenticationLevel;
                dataRow["InheritRDPMinutesToIdleTimeout"] = connectionInfo.Inheritance.RDPMinutesToIdleTimeout;
                dataRow["InheritRdpVersion"] = connectionInfo.Inheritance.RdpVersion;
                dataRow["InheritRedirectAudioCapture"] = connectionInfo.Inheritance.RedirectAudioCapture;
                dataRow["InheritRedirectClipboard"] = connectionInfo.Inheritance.RedirectClipboard;
                dataRow["InheritRedirectDiskDrives"] = connectionInfo.Inheritance.RedirectDiskDrives;
                dataRow["InheritRedirectDiskDrivesCustom"] = connectionInfo.Inheritance.RedirectDiskDrivesCustom;
                dataRow["InheritRedirectKeys"] = connectionInfo.Inheritance.RedirectKeys;
                dataRow["InheritRedirectPorts"] = connectionInfo.Inheritance.RedirectPorts;
                dataRow["InheritRedirectPrinters"] = connectionInfo.Inheritance.RedirectPrinters;
                dataRow["InheritRedirectSmartCards"] = connectionInfo.Inheritance.RedirectSmartCards;
                dataRow["InheritRedirectSound"] = connectionInfo.Inheritance.RedirectSound;
                dataRow["InheritRenderingEngine"] = connectionInfo.Inheritance.RenderingEngine;
                dataRow["InheritResolution"] = connectionInfo.Inheritance.Resolution;
                dataRow["InheritSSHOptions"] = connectionInfo.Inheritance.SSHOptions;
                dataRow["InheritSSHTunnelConnectionName"] = connectionInfo.Inheritance.SSHTunnelConnectionName;
                dataRow["InheritSoundQuality"] = connectionInfo.Inheritance.SoundQuality;
                dataRow["InheritUseConsoleSession"] = connectionInfo.Inheritance.UseConsoleSession;
                dataRow["InheritUseCredSsp"] = connectionInfo.Inheritance.UseCredSsp;
                dataRow["InheritUseEnhancedMode"] = connectionInfo.Inheritance.UseEnhancedMode;
                dataRow["InheritUseRCG"] = connectionInfo.Inheritance.UseRCG;
                dataRow["InheritUseRestrictedAdmin"] = connectionInfo.Inheritance.UseRestrictedAdmin;
                dataRow["InheritUseVmId"] = connectionInfo.Inheritance.UseVmId;
                dataRow["InheritUserField"] = connectionInfo.Inheritance.UserField;
                dataRow["InheritUserViaAPI"] = connectionInfo.Inheritance.UserViaAPI;
                dataRow["InheritUsername"] = connectionInfo.Inheritance.Username;
                dataRow["InheritVNCAuthMode"] = connectionInfo.Inheritance.VNCAuthMode;
                dataRow["InheritVNCColors"] = connectionInfo.Inheritance.VNCColors;
                dataRow["InheritVNCCompression"] = connectionInfo.Inheritance.VNCCompression;
                dataRow["InheritVNCEncoding"] = connectionInfo.Inheritance.VNCEncoding;
                dataRow["InheritVNCProxyIP"] = connectionInfo.Inheritance.VNCProxyIP;
                dataRow["InheritVNCProxyPassword"] = connectionInfo.Inheritance.VNCProxyPassword;
                dataRow["InheritVNCProxyPort"] = connectionInfo.Inheritance.VNCProxyPort;
                dataRow["InheritVNCProxyType"] = connectionInfo.Inheritance.VNCProxyType;
                dataRow["InheritVNCProxyUsername"] = connectionInfo.Inheritance.VNCProxyUsername;
                dataRow["InheritVNCSmartSizeMode"] = connectionInfo.Inheritance.VNCSmartSizeMode;
                dataRow["InheritVNCViewOnly"] = connectionInfo.Inheritance.VNCViewOnly;
                dataRow["InheritVmId"] = connectionInfo.Inheritance.VmId;
                dataRow["InheritCacheBitmaps"] = connectionInfo.Inheritance.CacheBitmaps;
            }
            else
            {
                dataRow["InheritAutomaticResize"] = false;
                dataRow["InheritColors"] = false;
                dataRow["InheritDescription"] = false;
                dataRow["InheritDisableCursorBlinking"] = false;
                dataRow["InheritDisableCursorShadow"] = false;
                dataRow["InheritDisableFullWindowDrag"] = false;
                dataRow["InheritDisableMenuAnimations"] = false;
                dataRow["InheritDisplayThemes"] = false;
                dataRow["InheritDisplayWallpaper"] = false;
                dataRow["InheritDomain"] = false;
                dataRow["InheritEnableDesktopComposition"] = false;
                dataRow["InheritEnableFontSmoothing"] = false;
                dataRow["InheritExtApp"] = false;
                dataRow["InheritExternalCredentialProvider"] = false;
                dataRow["InheritFavorite"] = false;
                dataRow["InheritICAEncryptionStrength"] = false;
                dataRow["InheritIcon"] = false;
                dataRow["InheritLoadBalanceInfo"] = false;
                dataRow["InheritMacAddress"] = false;
                dataRow["InheritOpeningCommand"] = false;
                dataRow["InheritPanel"] = false;
                dataRow["InheritPassword"] = false;
                dataRow["InheritPort"] = false;
                dataRow["InheritPostExtApp"] = false;
                dataRow["InheritPreExtApp"] = false;
                dataRow["InheritProtocol"] = false;
                dataRow["InheritPuttySession"] = false;
                dataRow["InheritRDGatewayDomain"] = false;
                dataRow["InheritRDGatewayExternalCredentialProvider"] = false;
                dataRow["InheritRDGatewayHostname"] = false;
                dataRow["InheritRDGatewayPassword"] = false;
                dataRow["InheritRDGatewayUsageMethod"] = false;
                dataRow["InheritRDGatewayUseConnectionCredentials"] = false;
                dataRow["InheritRDGatewayUserViaAPI"] = false;
                dataRow["InheritRDGatewayUsername"] = false;
                dataRow["InheritRDPAlertIdleTimeout"] = false;
                dataRow["InheritRDPAuthenticationLevel"] = false;
                dataRow["InheritRDPMinutesToIdleTimeout"] = false;
                dataRow["InheritRdpVersion"] = false;
                dataRow["InheritRedirectAudioCapture"] = false;
                dataRow["InheritRedirectClipboard"] = false;
                dataRow["InheritRedirectDiskDrives"] = false;
                dataRow["InheritRedirectDiskDrivesCustom"] = false;
                dataRow["InheritRedirectKeys"] = false;
                dataRow["InheritRedirectPorts"] = false;
                dataRow["InheritRedirectPrinters"] = false;
                dataRow["InheritRedirectSmartCards"] = false;
                dataRow["InheritRedirectSound"] = false;
                dataRow["InheritRenderingEngine"] = false;
                dataRow["InheritResolution"] = false;
                dataRow["InheritSSHOptions"] = false;
                dataRow["InheritSSHTunnelConnectionName"] = false;
                dataRow["InheritSoundQuality"] = false;
                dataRow["InheritUseConsoleSession"] = false;
                dataRow["InheritUseCredSsp"] = false;
                dataRow["InheritUseRCG"] = false;
                dataRow["InheritUseRestrictedAdmin"] = false;
                dataRow["InheritUserField"] = false;
                dataRow["InheritUserViaAPI"] = false;
                dataRow["InheritUsername"] = false;
                dataRow["InheritVNCAuthMode"] = false;
                dataRow["InheritVNCColors"] = false;
                dataRow["InheritVNCCompression"] = false;
                dataRow["InheritVNCEncoding"] = false;
                dataRow["InheritVNCProxyIP"] = false;
                dataRow["InheritVNCProxyPassword"] = false;
                dataRow["InheritVNCProxyPort"] = false;
                dataRow["InheritVNCProxyType"] = false;
                dataRow["InheritVNCProxyUsername"] = false;
                dataRow["InheritVNCSmartSizeMode"] = false;
                dataRow["InheritVNCViewOnly"] = false;
                dataRow["InheritCacheBitmaps"] = false;
            }

            if (isNewRow) _dataTable.Rows.Add(dataRow);
        }
    }
}