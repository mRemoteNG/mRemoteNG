using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Sql
{
    [SupportedOSPlatform("windows")]
    public class DataTableDeserializer : IDeserializer<DataTable, ConnectionTreeModel>
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _decryptionKey;

        public DataTableDeserializer(ICryptographyProvider cryptographyProvider, SecureString decryptionKey)
        {
            _cryptographyProvider = cryptographyProvider.ThrowIfNull(nameof(cryptographyProvider));
            _decryptionKey = decryptionKey.ThrowIfNull(nameof(decryptionKey));
        }

        public ConnectionTreeModel Deserialize(DataTable table)
        {
            var connectionList = CreateNodesFromTable(table);
            var connectionTreeModel = CreateNodeHierarchy(connectionList, table);
            Runtime.ConnectionsService.IsConnectionsFileLoaded = true;
            return connectionTreeModel;
        }

        private List<ConnectionInfo> CreateNodesFromTable(DataTable table)
        {
            var nodeList = new List<ConnectionInfo>();
            foreach (DataRow row in table.Rows)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch ((string)row["Type"])
                {
                    case "Connection":
                        nodeList.Add(DeserializeConnectionInfo(row));
                        break;
                    case "Container":
                        nodeList.Add(DeserializeContainerInfo(row));
                        break;
                }
            }

            return nodeList;
        }
        
        private ConnectionInfo DeserializeConnectionInfo(DataRow row)
        {
            var connectionId = row["ConstantID"] as string ?? Guid.NewGuid().ToString();
            var connectionInfo = new ConnectionInfo(connectionId);
            PopulateConnectionInfoFromDatarow(row, connectionInfo);
            return connectionInfo;
        }

        private ContainerInfo DeserializeContainerInfo(DataRow row)
        {
            var containerId = row["ConstantID"] as string ?? Guid.NewGuid().ToString();
            var containerInfo = new ContainerInfo(containerId);
            PopulateConnectionInfoFromDatarow(row, containerInfo);
            return containerInfo;
        }

        private void PopulateConnectionInfoFromDatarow(DataRow dataRow, ConnectionInfo connectionInfo)
        {
            connectionInfo.Name = (string)dataRow["Name"];

            // This throws a NPE - Parent is a connectionInfo object which will be null at this point.
            // The Parent object is linked properly later in CreateNodeHierarchy()
            //connectionInfo.Parent.ConstantID = (string)dataRow["ParentID"];

            //connectionInfo.EC2InstanceId = (string)dataRow["EC2InstanceId"];
            //connectionInfo.EC2Region = (string)dataRow["EC2Region"];
            //connectionInfo.ExternalAddressProvider = (ExternalAddressProvider)Enum.Parse(typeof(ExternalAddressProvider), (string)dataRow["ExternalAddressProvider"]);
            //connectionInfo.ExternalCredentialProvider = (ExternalCredentialProvider)Enum.Parse(typeof(ExternalCredentialProvider), (string)dataRow["ExternalCredentialProvider"]);
            //connectionInfo.RDGatewayExternalCredentialProvider = (ExternalCredentialProvider)Enum.Parse(typeof(ExternalCredentialProvider), (string)dataRow["RDGatewayExternalCredentialProvider"]);
            //connectionInfo.RDGatewayUserViaAPI = (string)dataRow["RDGatewayUserViaAPI"];
            //connectionInfo.UserViaAPI = (string)dataRow["UserViaAPI"];
            connectionInfo.AutomaticResize = MiscTools.GetBooleanValue(dataRow["AutomaticResize"]);
            connectionInfo.CacheBitmaps = MiscTools.GetBooleanValue(dataRow["CacheBitmaps"]);
            connectionInfo.Colors = (RDPColors)Enum.Parse(typeof(RDPColors), (string)dataRow["Colors"]);
            connectionInfo.Description = (string)dataRow["Description"];
            connectionInfo.DisableCursorBlinking = MiscTools.GetBooleanValue(dataRow["DisableCursorBlinking"]);
            connectionInfo.DisableCursorShadow = MiscTools.GetBooleanValue(dataRow["DisableCursorShadow"]);
            connectionInfo.DisableFullWindowDrag = MiscTools.GetBooleanValue(dataRow["DisableFullWindowDrag"]);
            connectionInfo.DisableMenuAnimations = MiscTools.GetBooleanValue(dataRow["DisableMenuAnimations"]);
            connectionInfo.DisplayThemes = MiscTools.GetBooleanValue(dataRow["DisplayThemes"]);
            connectionInfo.DisplayWallpaper = MiscTools.GetBooleanValue(dataRow["DisplayWallpaper"]);
            connectionInfo.Domain = (string)dataRow["Domain"];
            connectionInfo.EnableDesktopComposition = MiscTools.GetBooleanValue(dataRow["EnableDesktopComposition"]);
            connectionInfo.EnableFontSmoothing = MiscTools.GetBooleanValue(dataRow["EnableFontSmoothing"]);
            connectionInfo.ExtApp = (string)dataRow["ExtApp"];
            connectionInfo.Hostname = (string)dataRow["Hostname"];
            connectionInfo.Icon = (string)dataRow["Icon"];
            connectionInfo.LoadBalanceInfo = (string)dataRow["LoadBalanceInfo"];
            connectionInfo.MacAddress = (string)dataRow["MacAddress"];
            connectionInfo.OpeningCommand = (string)dataRow["OpeningCommand"];
            connectionInfo.OpeningCommand = (string)dataRow["OpeningCommand"];
            connectionInfo.Panel = (string)dataRow["Panel"];
            connectionInfo.Password = DecryptValue((string)dataRow["Password"]);
            connectionInfo.Port = (int)dataRow["Port"];
            connectionInfo.PostExtApp = (string)dataRow["PostExtApp"];
            connectionInfo.PreExtApp = (string)dataRow["PreExtApp"];
            connectionInfo.Protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), (string)dataRow["Protocol"]);
            connectionInfo.PuttySession = (string)dataRow["PuttySession"];
            connectionInfo.RDGatewayDomain = (string)dataRow["RDGatewayDomain"];
            connectionInfo.RDGatewayHostname = (string)dataRow["RDGatewayHostname"];
            connectionInfo.RDGatewayPassword = DecryptValue((string)dataRow["RDGatewayPassword"]);
            connectionInfo.RDGatewayUsageMethod = (RDGatewayUsageMethod)Enum.Parse(typeof(RDGatewayUsageMethod), (string)dataRow["RDGatewayUsageMethod"]);
            connectionInfo.RDGatewayUseConnectionCredentials = (RDGatewayUseConnectionCredentials)Enum.Parse(typeof(RDGatewayUseConnectionCredentials), (string)dataRow["RDGatewayUseConnectionCredentials"]);
            connectionInfo.RDGatewayUsername = (string)dataRow["RDGatewayUsername"];
            connectionInfo.RDPAlertIdleTimeout = MiscTools.GetBooleanValue(dataRow["RDPAlertIdleTimeout"]);
            connectionInfo.RDPAuthenticationLevel = (AuthenticationLevel)Enum.Parse(typeof(AuthenticationLevel), (string)dataRow["RDPAuthenticationLevel"]);
            connectionInfo.RDPMinutesToIdleTimeout = (int)dataRow["RDPMinutesToIdleTimeout"];
            connectionInfo.RDPStartProgram = (string)dataRow["StartProgram"];
            connectionInfo.RDPStartProgramWorkDir = (string)dataRow["StartProgramWorkDir"];
            connectionInfo.RedirectAudioCapture = MiscTools.GetBooleanValue(dataRow["RedirectAudioCapture"]);
            connectionInfo.RedirectClipboard = MiscTools.GetBooleanValue(dataRow["RedirectClipboard"]);
            connectionInfo.RedirectDiskDrives = (RDPDiskDrives)Enum.Parse(typeof(RDPDiskDrives), (string)dataRow["RedirectDiskDrives"]);
            connectionInfo.RedirectDiskDrivesCustom = (string)dataRow["RedirectDiskDrivesCustom"];
            connectionInfo.RedirectKeys = MiscTools.GetBooleanValue(dataRow["RedirectKeys"]);
            connectionInfo.RedirectPorts = MiscTools.GetBooleanValue(dataRow["RedirectPorts"]);
            connectionInfo.RedirectPrinters = MiscTools.GetBooleanValue(dataRow["RedirectPrinters"]);
            connectionInfo.RedirectSmartCards = MiscTools.GetBooleanValue(dataRow["RedirectSmartCards"]);
            connectionInfo.RedirectSound = (RDPSounds)Enum.Parse(typeof(RDPSounds), (string)dataRow["RedirectSound"]);
            connectionInfo.RenderingEngine = (HTTPBase.RenderingEngine)Enum.Parse(typeof(HTTPBase.RenderingEngine), (string)dataRow["RenderingEngine"]);
            connectionInfo.Resolution = (RDPResolutions)Enum.Parse(typeof(RDPResolutions), (string)dataRow["Resolution"]);
            connectionInfo.SoundQuality = (RDPSoundQuality)Enum.Parse(typeof(RDPSoundQuality), (string)dataRow["SoundQuality"]);
            connectionInfo.SSHOptions = (string)dataRow["SSHOptions"];
            connectionInfo.SSHTunnelConnectionName = (string)dataRow["SSHTunnelConnectionName"];
            connectionInfo.UseConsoleSession = MiscTools.GetBooleanValue(dataRow["ConnectToConsole"]);
            connectionInfo.UseCredSsp = MiscTools.GetBooleanValue(dataRow["UseCredSsp"]);
            connectionInfo.UseEnhancedMode = MiscTools.GetBooleanValue(dataRow["UseEnhancedMode"]);
            connectionInfo.UseRCG = MiscTools.GetBooleanValue(dataRow["UseRCG"]);
            connectionInfo.UseRestrictedAdmin = MiscTools.GetBooleanValue(dataRow["UseRestrictedAdmin"]);
            connectionInfo.UserField = (string)dataRow["UserField"];
            connectionInfo.Username = (string)dataRow["Username"];
            connectionInfo.UseVmId = MiscTools.GetBooleanValue(dataRow["UseVmId"]);
            connectionInfo.VmId = (string)dataRow["VmId"];
            connectionInfo.VNCAuthMode = (ProtocolVNC.AuthMode)Enum.Parse(typeof(ProtocolVNC.AuthMode), (string)dataRow["VNCAuthMode"]);
            connectionInfo.VNCColors = (ProtocolVNC.Colors)Enum.Parse(typeof(ProtocolVNC.Colors), (string)dataRow["VNCColors"]);
            connectionInfo.VNCCompression = (ProtocolVNC.Compression)Enum.Parse(typeof(ProtocolVNC.Compression), (string)dataRow["VNCCompression"]);
            connectionInfo.VNCEncoding = (ProtocolVNC.Encoding)Enum.Parse(typeof(ProtocolVNC.Encoding), (string)dataRow["VNCEncoding"]);
            connectionInfo.VNCProxyIP = (string)dataRow["VNCProxyIP"];
            connectionInfo.VNCProxyPassword = DecryptValue((string)dataRow["VNCProxyPassword"]);
            connectionInfo.VNCProxyPort = (int)dataRow["VNCProxyPort"];
            connectionInfo.VNCProxyType = (ProtocolVNC.ProxyType)Enum.Parse(typeof(ProtocolVNC.ProxyType), (string)dataRow["VNCProxyType"]);
            connectionInfo.VNCProxyUsername = (string)dataRow["VNCProxyUsername"];
            connectionInfo.VNCSmartSizeMode = (ProtocolVNC.SmartSizeMode)Enum.Parse(typeof(ProtocolVNC.SmartSizeMode), (string)dataRow["VNCSmartSizeMode"]);
            connectionInfo.VNCViewOnly = MiscTools.GetBooleanValue(dataRow["VNCViewOnly"]);

            if (!dataRow.IsNull("RdpVersion")) // table allows null values which must be handled
                if (Enum.TryParse((string)dataRow["RdpVersion"], true, out RdpVersion rdpVersion))
                    connectionInfo.RdpVersion = rdpVersion;

            //connectionInfo.Inheritance.ExternalCredentialProvider = MiscTools.GetBooleanValue(dataRow["InheritExternalCredentialProvider"]);
            //connectionInfo.Inheritance.RDGatewayExternalCredentialProvider = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayExternalCredentialProvider"]);
            //connectionInfo.Inheritance.RDGatewayUserViaAPI = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayUserViaAPI"]);
            //connectionInfo.Inheritance.UserViaAPI = MiscTools.GetBooleanValue(dataRow["InheritUserViaAPI"]);
            connectionInfo.Inheritance.AutomaticResize = MiscTools.GetBooleanValue(dataRow["InheritAutomaticResize"]);
            connectionInfo.Inheritance.CacheBitmaps = MiscTools.GetBooleanValue(dataRow["InheritCacheBitmaps"]);
            connectionInfo.Inheritance.Colors = MiscTools.GetBooleanValue(dataRow["InheritColors"]);
            connectionInfo.Inheritance.Description = MiscTools.GetBooleanValue(dataRow["InheritDescription"]);
            connectionInfo.Inheritance.DisableCursorBlinking = MiscTools.GetBooleanValue(dataRow["InheritDisableCursorBlinking"]);
            connectionInfo.Inheritance.DisableCursorShadow = MiscTools.GetBooleanValue(dataRow["InheritDisableCursorShadow"]);
            connectionInfo.Inheritance.DisableFullWindowDrag = MiscTools.GetBooleanValue(dataRow["InheritDisableFullWindowDrag"]);
            connectionInfo.Inheritance.DisableMenuAnimations = MiscTools.GetBooleanValue(dataRow["InheritDisableMenuAnimations"]);
            connectionInfo.Inheritance.DisplayThemes = MiscTools.GetBooleanValue(dataRow["InheritDisplayThemes"]);
            connectionInfo.Inheritance.DisplayWallpaper = MiscTools.GetBooleanValue(dataRow["InheritDisplayWallpaper"]);
            connectionInfo.Inheritance.Domain = MiscTools.GetBooleanValue(dataRow["InheritDomain"]);
            connectionInfo.Inheritance.EnableDesktopComposition = MiscTools.GetBooleanValue(dataRow["InheritEnableDesktopComposition"]);
            connectionInfo.Inheritance.EnableFontSmoothing = MiscTools.GetBooleanValue(dataRow["InheritEnableFontSmoothing"]);
            connectionInfo.Inheritance.ExtApp = MiscTools.GetBooleanValue(dataRow["InheritExtApp"]);
            connectionInfo.Inheritance.Icon = MiscTools.GetBooleanValue(dataRow["InheritIcon"]);
            connectionInfo.Inheritance.LoadBalanceInfo = MiscTools.GetBooleanValue(dataRow["InheritLoadBalanceInfo"]);
            connectionInfo.Inheritance.MacAddress = MiscTools.GetBooleanValue(dataRow["InheritMacAddress"]);
            connectionInfo.Inheritance.OpeningCommand = MiscTools.GetBooleanValue(dataRow["InheritOpeningCommand"]);
            connectionInfo.Inheritance.OpeningCommand = MiscTools.GetBooleanValue(dataRow["InheritOpeningCommand"]);
            connectionInfo.Inheritance.Panel = MiscTools.GetBooleanValue(dataRow["InheritPanel"]);
            connectionInfo.Inheritance.Password = MiscTools.GetBooleanValue(dataRow["InheritPassword"]);
            connectionInfo.Inheritance.Port = MiscTools.GetBooleanValue(dataRow["InheritPort"]);
            connectionInfo.Inheritance.PostExtApp = MiscTools.GetBooleanValue(dataRow["InheritPostExtApp"]);
            connectionInfo.Inheritance.PreExtApp = MiscTools.GetBooleanValue(dataRow["InheritPreExtApp"]);
            connectionInfo.Inheritance.Protocol = MiscTools.GetBooleanValue(dataRow["InheritProtocol"]);
            connectionInfo.Inheritance.PuttySession = MiscTools.GetBooleanValue(dataRow["InheritPuttySession"]);
            connectionInfo.Inheritance.RDGatewayDomain = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayDomain"]);
            connectionInfo.Inheritance.RDGatewayHostname = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayHostname"]);
            connectionInfo.Inheritance.RDGatewayPassword = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayPassword"]);
            connectionInfo.Inheritance.RDGatewayUsageMethod = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayUsageMethod"]);
            connectionInfo.Inheritance.RDGatewayUseConnectionCredentials = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayUseConnectionCredentials"]);
            connectionInfo.Inheritance.RDGatewayUsername = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayUsername"]);
            connectionInfo.Inheritance.RDPAlertIdleTimeout = MiscTools.GetBooleanValue(dataRow["InheritRDPAlertIdleTimeout"]);
            connectionInfo.Inheritance.RDPAuthenticationLevel = MiscTools.GetBooleanValue(dataRow["InheritRDPAuthenticationLevel"]);
            connectionInfo.Inheritance.RDPMinutesToIdleTimeout = MiscTools.GetBooleanValue(dataRow["InheritRDPMinutesToIdleTimeout"]);
            connectionInfo.Inheritance.RdpVersion = MiscTools.GetBooleanValue(dataRow["InheritRdpVersion"]);
            connectionInfo.Inheritance.RedirectAudioCapture = MiscTools.GetBooleanValue(dataRow["InheritRedirectAudioCapture"]);
            connectionInfo.Inheritance.RedirectClipboard = MiscTools.GetBooleanValue(dataRow["InheritRedirectClipboard"]);
            connectionInfo.Inheritance.RedirectDiskDrives = MiscTools.GetBooleanValue(dataRow["InheritRedirectDiskDrives"]);
            connectionInfo.Inheritance.RedirectDiskDrivesCustom = MiscTools.GetBooleanValue(dataRow["InheritRedirectDiskDrivesCustom"]);
            connectionInfo.Inheritance.RedirectKeys = MiscTools.GetBooleanValue(dataRow["InheritRedirectKeys"]);
            connectionInfo.Inheritance.RedirectPorts = MiscTools.GetBooleanValue(dataRow["InheritRedirectPorts"]);
            connectionInfo.Inheritance.RedirectPrinters = MiscTools.GetBooleanValue(dataRow["InheritRedirectPrinters"]);
            connectionInfo.Inheritance.RedirectSmartCards = MiscTools.GetBooleanValue(dataRow["InheritRedirectSmartCards"]);
            connectionInfo.Inheritance.RedirectSound = MiscTools.GetBooleanValue(dataRow["InheritRedirectSound"]);
            connectionInfo.Inheritance.RenderingEngine = MiscTools.GetBooleanValue(dataRow["InheritRenderingEngine"]);
            connectionInfo.Inheritance.Resolution = MiscTools.GetBooleanValue(dataRow["InheritResolution"]);
            connectionInfo.Inheritance.SoundQuality = MiscTools.GetBooleanValue(dataRow["InheritSoundQuality"]);
            connectionInfo.Inheritance.SSHOptions = MiscTools.GetBooleanValue(dataRow["InheritSSHOptions"]);
            connectionInfo.Inheritance.SSHTunnelConnectionName = MiscTools.GetBooleanValue(dataRow["InheritSSHTunnelConnectionName"]);
            connectionInfo.Inheritance.UseConsoleSession = MiscTools.GetBooleanValue(dataRow["InheritUseConsoleSession"]);
            connectionInfo.Inheritance.UseCredSsp = MiscTools.GetBooleanValue(dataRow["InheritUseCredSsp"]);
            connectionInfo.Inheritance.UseEnhancedMode = MiscTools.GetBooleanValue(dataRow["InheritUseEnhancedMode"]);
            connectionInfo.Inheritance.UseRCG = MiscTools.GetBooleanValue(dataRow["InheritUseRCG"]);
            connectionInfo.Inheritance.UseRestrictedAdmin = MiscTools.GetBooleanValue(dataRow["InheritUseRestrictedAdmin"]);
            connectionInfo.Inheritance.UserField = MiscTools.GetBooleanValue(dataRow["InheritUserField"]);
            connectionInfo.Inheritance.Username = MiscTools.GetBooleanValue(dataRow["InheritUsername"]);
            connectionInfo.Inheritance.UseVmId = MiscTools.GetBooleanValue(dataRow["InheritUseVmId"]);
            connectionInfo.Inheritance.VmId = MiscTools.GetBooleanValue(dataRow["InheritVmId"]);
            connectionInfo.Inheritance.VNCAuthMode = MiscTools.GetBooleanValue(dataRow["InheritVNCAuthMode"]);
            connectionInfo.Inheritance.VNCColors = MiscTools.GetBooleanValue(dataRow["InheritVNCColors"]);
            connectionInfo.Inheritance.VNCCompression = MiscTools.GetBooleanValue(dataRow["InheritVNCCompression"]);
            connectionInfo.Inheritance.VNCEncoding = MiscTools.GetBooleanValue(dataRow["InheritVNCEncoding"]);
            connectionInfo.Inheritance.VNCProxyIP = MiscTools.GetBooleanValue(dataRow["InheritVNCProxyIP"]);
            connectionInfo.Inheritance.VNCProxyPassword = MiscTools.GetBooleanValue(dataRow["InheritVNCProxyPassword"]);
            connectionInfo.Inheritance.VNCProxyPort = MiscTools.GetBooleanValue(dataRow["InheritVNCProxyPort"]);
            connectionInfo.Inheritance.VNCProxyType = MiscTools.GetBooleanValue(dataRow["InheritVNCProxyType"]);
            connectionInfo.Inheritance.VNCProxyUsername = MiscTools.GetBooleanValue(dataRow["InheritVNCProxyUsername"]);
            connectionInfo.Inheritance.VNCSmartSizeMode = MiscTools.GetBooleanValue(dataRow["InheritVNCSmartSizeMode"]);
            connectionInfo.Inheritance.VNCViewOnly = MiscTools.GetBooleanValue(dataRow["InheritVNCViewOnly"]);
        }

        private string DecryptValue(string cipherText)
        {
            try
            {
                return _cryptographyProvider.Decrypt(cipherText, _decryptionKey);
            }
            catch (EncryptionException)
            {
                // value may not be encrypted
                return cipherText;
            }
        }

        private ConnectionTreeModel CreateNodeHierarchy(List<ConnectionInfo> connectionList, DataTable dataTable)
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var rootNode = new RootNodeInfo(RootNodeType.Connection, "0")
            {
                PasswordString = _decryptionKey.ConvertToUnsecureString()
            };
            connectionTreeModel.AddRootNode(rootNode);

            foreach (DataRow row in dataTable.Rows)
            {
                var id = (string)row["ConstantID"];
                var connectionInfo = connectionList.First(node => node.ConstantID == id);
                var parentId = (string)row["ParentID"];
                if (parentId == "0" || connectionList.All(node => node.ConstantID != parentId))
                    rootNode.AddChild(connectionInfo);
                else
                    (connectionList.First(node => node.ConstantID == parentId) as ContainerInfo)?.AddChild(connectionInfo);
            }

            return connectionTreeModel;
        }
    }
}