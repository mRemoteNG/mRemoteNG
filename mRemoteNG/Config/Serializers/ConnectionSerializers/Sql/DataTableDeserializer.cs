using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Windows.Media.TextFormatting;
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
    public class DataTableDeserializer(ICryptographyProvider cryptographyProvider, SecureString decryptionKey) : IDeserializer<DataTable, ConnectionTreeModel>
    {
        private readonly ICryptographyProvider _cryptographyProvider = cryptographyProvider.ThrowIfNull(nameof(cryptographyProvider));
        private readonly SecureString _decryptionKey = decryptionKey.ThrowIfNull(nameof(decryptionKey));

        public ConnectionTreeModel Deserialize(DataTable table)
        {
            List<ConnectionInfo> connectionList = CreateNodesFromTable(table);
            ConnectionTreeModel connectionTreeModel = CreateNodeHierarchy(connectionList, table);
            Runtime.ConnectionsService.IsConnectionsFileLoaded = true;
            return connectionTreeModel;
        }

        private List<ConnectionInfo> CreateNodesFromTable(DataTable table)
        {
            List<ConnectionInfo> nodeList = new();
            foreach (DataRow row in table.Rows)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (row["Type"] as string)
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
            string connectionId = row["ConstantID"] as string ?? Guid.NewGuid().ToString();
            ConnectionInfo connectionInfo = new(connectionId);
            PopulateConnectionInfoFromDatarow(row, connectionInfo);
            return connectionInfo;
        }

        private ContainerInfo DeserializeContainerInfo(DataRow row)
        {
            string containerId = row["ConstantID"] as string ?? Guid.NewGuid().ToString();
            ContainerInfo containerInfo = new(containerId);
            PopulateConnectionInfoFromDatarow(row, containerInfo);
            return containerInfo;
        }

        private void PopulateConnectionInfoFromDatarow(DataRow dataRow, ConnectionInfo connectionInfo)
        {
            connectionInfo.Name = dataRow["Name"] as string ?? "";

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
            if (dataRow.Table.Columns.Contains("AutomaticResize"))
                connectionInfo.AutomaticResize = MiscTools.GetBooleanValue(dataRow["AutomaticResize"]);
            connectionInfo.CacheBitmaps = MiscTools.GetBooleanValue(dataRow["CacheBitmaps"]);
            if (!dataRow.IsNull("Colors"))
                connectionInfo.Colors = Enum.Parse<RDPColors>((string)dataRow["Colors"]);
            if (dataRow.Table.Columns.Contains("ConnectionFrameColor") && !dataRow.IsNull("ConnectionFrameColor"))
                if (Enum.TryParse((string)dataRow["ConnectionFrameColor"], true, out ConnectionFrameColor frameColor))
                    connectionInfo.ConnectionFrameColor = frameColor;
            connectionInfo.Description = dataRow["Description"] as string ?? "";
            connectionInfo.DisableCursorBlinking = MiscTools.GetBooleanValue(dataRow["DisableCursorBlinking"]);
            connectionInfo.DisableCursorShadow = MiscTools.GetBooleanValue(dataRow["DisableCursorShadow"]);
            connectionInfo.DisableFullWindowDrag = MiscTools.GetBooleanValue(dataRow["DisableFullWindowDrag"]);
            connectionInfo.DisableMenuAnimations = MiscTools.GetBooleanValue(dataRow["DisableMenuAnimations"]);
            connectionInfo.DisplayThemes = MiscTools.GetBooleanValue(dataRow["DisplayThemes"]);
            connectionInfo.DisplayWallpaper = MiscTools.GetBooleanValue(dataRow["DisplayWallpaper"]);
            connectionInfo.Domain = dataRow.Table.Columns.Contains("Domain")
                ? dataRow["Domain"] as string ?? ""
                : "";
            if (dataRow.Table.Columns.Contains("EnableDesktopComposition"))
                connectionInfo.EnableDesktopComposition = MiscTools.GetBooleanValue(dataRow["EnableDesktopComposition"]);
            if (dataRow.Table.Columns.Contains("EnableFontSmoothing"))
                connectionInfo.EnableFontSmoothing = MiscTools.GetBooleanValue(dataRow["EnableFontSmoothing"]);
            connectionInfo.ExtApp = dataRow["ExtApp"] as string ?? "";
            connectionInfo.Hostname = dataRow["Hostname"] as string ?? "";
            connectionInfo.Icon = dataRow["Icon"] as string ?? "";
            if (dataRow.Table.Columns.Contains("IsTemplate"))
                connectionInfo.IsTemplate = MiscTools.GetBooleanValue(dataRow["IsTemplate"]);
            if (dataRow.Table.Columns.Contains("LoadBalanceInfo"))
                connectionInfo.LoadBalanceInfo = dataRow["LoadBalanceInfo"] as string ?? "";
            connectionInfo.MacAddress = dataRow["MacAddress"] as string ?? "";
            connectionInfo.OpeningCommand = dataRow["OpeningCommand"] as string ?? "";
            connectionInfo.OpeningCommand = dataRow["OpeningCommand"] as string ?? "";
            connectionInfo.Panel = dataRow["Panel"] as string ?? "";
            var pw = dataRow["Password"] as string;
            //connectionInfo.Password = DecryptValue(pw ?? "").ConvertToSecureString();
            connectionInfo.Password = DecryptValue(pw ?? "");
            if (!dataRow.IsNull("Port"))
                connectionInfo.Port = (int)dataRow["Port"];
            connectionInfo.PostExtApp = dataRow["PostExtApp"] as string ?? "";
            connectionInfo.PreExtApp = dataRow["PreExtApp"] as string ?? "";
            if (!dataRow.IsNull("Protocol"))
                connectionInfo.Protocol = Enum.Parse<ProtocolType>((string)dataRow["Protocol"]);
            connectionInfo.PuttySession = dataRow["PuttySession"] as string ?? "";
            connectionInfo.RDGatewayDomain = dataRow["RDGatewayDomain"] as string ?? "";
            connectionInfo.RDGatewayHostname = dataRow["RDGatewayHostname"] as string ?? "";
            connectionInfo.RDGatewayPassword = DecryptValue(dataRow["RDGatewayPassword"] as string ?? "");
            if (!dataRow.IsNull("RDGatewayUsageMethod"))
                connectionInfo.RDGatewayUsageMethod = Enum.Parse<RDGatewayUsageMethod>((string)dataRow["RDGatewayUsageMethod"]);
            if (!dataRow.IsNull("RDGatewayUseConnectionCredentials"))
                connectionInfo.RDGatewayUseConnectionCredentials = Enum.Parse<RDGatewayUseConnectionCredentials>((string)dataRow["RDGatewayUseConnectionCredentials"]);
            connectionInfo.RDGatewayUsername = dataRow["RDGatewayUsername"] as string ?? "";
            if (dataRow.Table.Columns.Contains("RDPAlertIdleTimeout"))
                connectionInfo.RDPAlertIdleTimeout = MiscTools.GetBooleanValue(dataRow["RDPAlertIdleTimeout"]);
            if (!dataRow.IsNull("RDPAuthenticationLevel"))
                connectionInfo.RDPAuthenticationLevel = Enum.Parse<AuthenticationLevel>((string)dataRow["RDPAuthenticationLevel"]);
            if (dataRow.Table.Columns.Contains("RDPMinutesToIdleTimeout") && !dataRow.IsNull("RDPMinutesToIdleTimeout"))
                connectionInfo.RDPMinutesToIdleTimeout = (int)dataRow["RDPMinutesToIdleTimeout"];
            connectionInfo.RDPStartProgram = dataRow["StartProgram"] as string ?? "";
            connectionInfo.RDPStartProgramWorkDir = dataRow["StartProgramWorkDir"] as string ?? "";
            connectionInfo.RedirectAudioCapture = MiscTools.GetBooleanValue(dataRow["RedirectAudioCapture"]);
            if (dataRow.Table.Columns.Contains("RedirectClipboard"))
                connectionInfo.RedirectClipboard = MiscTools.GetBooleanValue(dataRow["RedirectClipboard"]);
            if (!dataRow.IsNull("RedirectDiskDrives"))
                connectionInfo.RedirectDiskDrives = Enum.Parse<RDPDiskDrives>((string)dataRow["RedirectDiskDrives"]);
            if (dataRow.Table.Columns.Contains("RedirectDiskDrivesCustom"))
                connectionInfo.RedirectDiskDrivesCustom = dataRow["RedirectDiskDrivesCustom"] as string ?? "";
            connectionInfo.RedirectKeys = MiscTools.GetBooleanValue(dataRow["RedirectKeys"]);
            connectionInfo.RedirectPorts = MiscTools.GetBooleanValue(dataRow["RedirectPorts"]);
            connectionInfo.RedirectPrinters = MiscTools.GetBooleanValue(dataRow["RedirectPrinters"]);
            connectionInfo.RedirectSmartCards = MiscTools.GetBooleanValue(dataRow["RedirectSmartCards"]);
            if (!dataRow.IsNull("RedirectSound"))
                connectionInfo.RedirectSound = Enum.Parse<RDPSounds>((string)dataRow["RedirectSound"]);
            if (!dataRow.IsNull("RenderingEngine"))
                connectionInfo.RenderingEngine = Enum.Parse<HTTPBase.RenderingEngine>((string)dataRow["RenderingEngine"]);
            if (!dataRow.IsNull("Resolution"))
                connectionInfo.Resolution = Enum.Parse<RDPResolutions>((string)dataRow["Resolution"]);
            if (dataRow.Table.Columns.Contains("SoundQuality") && !dataRow.IsNull("SoundQuality"))
                connectionInfo.SoundQuality = Enum.Parse<RDPSoundQuality>((string)dataRow["SoundQuality"]);
            if (dataRow.Table.Columns.Contains("SSHOptions"))
                connectionInfo.SSHOptions = dataRow["SSHOptions"] as string ?? "";
            if (dataRow.Table.Columns.Contains("SSHTunnelConnectionName"))
                connectionInfo.SSHTunnelConnectionName = dataRow["SSHTunnelConnectionName"] as string ?? "";
            connectionInfo.UseConsoleSession = MiscTools.GetBooleanValue(dataRow["ConnectToConsole"]);
            if (dataRow.Table.Columns.Contains("UseCredSsp"))
                connectionInfo.UseCredSsp = MiscTools.GetBooleanValue(dataRow["UseCredSsp"]);
            if (dataRow.Table.Columns.Contains("UseEnhancedMode"))
                connectionInfo.UseEnhancedMode = MiscTools.GetBooleanValue(dataRow["UseEnhancedMode"]);
            if (dataRow.Table.Columns.Contains("UseRCG"))
                connectionInfo.UseRCG = MiscTools.GetBooleanValue(dataRow["UseRCG"]);
            if (dataRow.Table.Columns.Contains("UseRestrictedAdmin"))
                connectionInfo.UseRestrictedAdmin = MiscTools.GetBooleanValue(dataRow["UseRestrictedAdmin"]);
            connectionInfo.UserField = dataRow["UserField"] as string ?? "";
            connectionInfo.EnvironmentTags = dataRow.Table.Columns.Contains("EnvironmentTags") ? (dataRow["EnvironmentTags"] as string ?? "") : "";
            connectionInfo.Username = dataRow["Username"] as string ?? "";
            if (dataRow.Table.Columns.Contains("UseVmId"))
                connectionInfo.UseVmId = MiscTools.GetBooleanValue(dataRow["UseVmId"]);
            if (dataRow.Table.Columns.Contains("VmId"))
                connectionInfo.VmId = dataRow["VmId"] as string ?? "";
            if (!dataRow.IsNull("VNCAuthMode"))
                connectionInfo.VNCAuthMode = Enum.Parse<ProtocolVNC.AuthMode>((string)dataRow["VNCAuthMode"]);
            if (!dataRow.IsNull("VNCColors"))
                connectionInfo.VNCColors = Enum.Parse<ProtocolVNC.Colors>((string)dataRow["VNCColors"]);
            if (!dataRow.IsNull("VNCCompression"))
                connectionInfo.VNCCompression = Enum.Parse<ProtocolVNC.Compression>((string)dataRow["VNCCompression"]);
            if (!dataRow.IsNull("VNCEncoding"))
                connectionInfo.VNCEncoding = Enum.Parse<ProtocolVNC.Encoding>((string)dataRow["VNCEncoding"]);
            connectionInfo.VNCProxyIP = dataRow["VNCProxyIP"] as string ?? "";
            connectionInfo.VNCProxyPassword = DecryptValue(dataRow["VNCProxyPassword"] as string ?? "");
            if (!dataRow.IsNull("VNCProxyPort"))
                connectionInfo.VNCProxyPort = (int)dataRow["VNCProxyPort"];
            if (!dataRow.IsNull("VNCProxyType"))
                connectionInfo.VNCProxyType = Enum.Parse<ProtocolVNC.ProxyType>((string)dataRow["VNCProxyType"]);
            connectionInfo.VNCProxyUsername = dataRow["VNCProxyUsername"] as string ?? "";
            if (!dataRow.IsNull("VNCSmartSizeMode"))
                connectionInfo.VNCSmartSizeMode = Enum.Parse<ProtocolVNC.SmartSizeMode>((string)dataRow["VNCSmartSizeMode"]);
            connectionInfo.VNCViewOnly = MiscTools.GetBooleanValue(dataRow["VNCViewOnly"]);
            connectionInfo.VNCClipboardRedirect = dataRow.Table.Columns.Contains("VNCClipboardRedirect")
                ? MiscTools.GetBooleanValue(dataRow["VNCClipboardRedirect"])
                : true;

            if (!dataRow.IsNull("RdpVersion")) // table allows null values which must be handled
                if (Enum.TryParse((string)dataRow["RdpVersion"], true, out RdpVersion rdpVersion))
                    connectionInfo.RdpVersion = rdpVersion;

            //connectionInfo.Inheritance.ExternalCredentialProvider = MiscTools.GetBooleanValue(dataRow["InheritExternalCredentialProvider"]);
            //connectionInfo.Inheritance.RDGatewayExternalCredentialProvider = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayExternalCredentialProvider"]);
            //connectionInfo.Inheritance.RDGatewayUserViaAPI = MiscTools.GetBooleanValue(dataRow["InheritRDGatewayUserViaAPI"]);
            //connectionInfo.Inheritance.UserViaAPI = MiscTools.GetBooleanValue(dataRow["InheritUserViaAPI"]);
            if (dataRow.Table.Columns.Contains("InheritAutomaticResize"))
                connectionInfo.Inheritance.AutomaticResize = MiscTools.GetBooleanValue(dataRow["InheritAutomaticResize"]);
            connectionInfo.Inheritance.CacheBitmaps = MiscTools.GetBooleanValue(dataRow["InheritCacheBitmaps"]);
            connectionInfo.Inheritance.Colors = MiscTools.GetBooleanValue(dataRow["InheritColors"]);
            if (dataRow.Table.Columns.Contains("InheritConnectionFrameColor"))
                connectionInfo.Inheritance.ConnectionFrameColor = MiscTools.GetBooleanValue(dataRow["InheritConnectionFrameColor"]);
            connectionInfo.Inheritance.Description = MiscTools.GetBooleanValue(dataRow["InheritDescription"]);
            connectionInfo.Inheritance.DisableCursorBlinking = MiscTools.GetBooleanValue(dataRow["InheritDisableCursorBlinking"]);
            connectionInfo.Inheritance.DisableCursorShadow = MiscTools.GetBooleanValue(dataRow["InheritDisableCursorShadow"]);
            connectionInfo.Inheritance.DisableFullWindowDrag = MiscTools.GetBooleanValue(dataRow["InheritDisableFullWindowDrag"]);
            connectionInfo.Inheritance.DisableMenuAnimations = MiscTools.GetBooleanValue(dataRow["InheritDisableMenuAnimations"]);
            connectionInfo.Inheritance.DisplayThemes = MiscTools.GetBooleanValue(dataRow["InheritDisplayThemes"]);
            connectionInfo.Inheritance.DisplayWallpaper = MiscTools.GetBooleanValue(dataRow["InheritDisplayWallpaper"]);
            connectionInfo.Inheritance.Domain = MiscTools.GetBooleanValue(dataRow["InheritDomain"]);
            if (dataRow.Table.Columns.Contains("InheritEnableDesktopComposition"))
                connectionInfo.Inheritance.EnableDesktopComposition = MiscTools.GetBooleanValue(dataRow["InheritEnableDesktopComposition"]);
            if (dataRow.Table.Columns.Contains("InheritEnableFontSmoothing"))
                connectionInfo.Inheritance.EnableFontSmoothing = MiscTools.GetBooleanValue(dataRow["InheritEnableFontSmoothing"]);
            connectionInfo.Inheritance.ExtApp = MiscTools.GetBooleanValue(dataRow["InheritExtApp"]);
            connectionInfo.Inheritance.Icon = MiscTools.GetBooleanValue(dataRow["InheritIcon"]);
            if (dataRow.Table.Columns.Contains("InheritLoadBalanceInfo"))
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
            if (dataRow.Table.Columns.Contains("InheritRDPAlertIdleTimeout"))
                connectionInfo.Inheritance.RDPAlertIdleTimeout = MiscTools.GetBooleanValue(dataRow["InheritRDPAlertIdleTimeout"]);
            connectionInfo.Inheritance.RDPAuthenticationLevel = MiscTools.GetBooleanValue(dataRow["InheritRDPAuthenticationLevel"]);
            if (dataRow.Table.Columns.Contains("InheritRDPMinutesToIdleTimeout"))
                connectionInfo.Inheritance.RDPMinutesToIdleTimeout = MiscTools.GetBooleanValue(dataRow["InheritRDPMinutesToIdleTimeout"]);
            connectionInfo.Inheritance.RdpVersion = MiscTools.GetBooleanValue(dataRow["InheritRdpVersion"]);
            connectionInfo.Inheritance.RedirectAudioCapture = MiscTools.GetBooleanValue(dataRow["InheritRedirectAudioCapture"]);
            if (dataRow.Table.Columns.Contains("InheritRedirectClipboard"))
                connectionInfo.Inheritance.RedirectClipboard = MiscTools.GetBooleanValue(dataRow["InheritRedirectClipboard"]);
            connectionInfo.Inheritance.RedirectDiskDrives = MiscTools.GetBooleanValue(dataRow["InheritRedirectDiskDrives"]);
            if (dataRow.Table.Columns.Contains("InheritRedirectDiskDrivesCustom"))
                connectionInfo.Inheritance.RedirectDiskDrivesCustom = MiscTools.GetBooleanValue(dataRow["InheritRedirectDiskDrivesCustom"]);
            connectionInfo.Inheritance.RedirectKeys = MiscTools.GetBooleanValue(dataRow["InheritRedirectKeys"]);
            connectionInfo.Inheritance.RedirectPorts = MiscTools.GetBooleanValue(dataRow["InheritRedirectPorts"]);
            connectionInfo.Inheritance.RedirectPrinters = MiscTools.GetBooleanValue(dataRow["InheritRedirectPrinters"]);
            connectionInfo.Inheritance.RedirectSmartCards = MiscTools.GetBooleanValue(dataRow["InheritRedirectSmartCards"]);
            connectionInfo.Inheritance.RedirectSound = MiscTools.GetBooleanValue(dataRow["InheritRedirectSound"]);
            connectionInfo.Inheritance.RenderingEngine = MiscTools.GetBooleanValue(dataRow["InheritRenderingEngine"]);
            connectionInfo.Inheritance.Resolution = MiscTools.GetBooleanValue(dataRow["InheritResolution"]);
            if (dataRow.Table.Columns.Contains("InheritSoundQuality"))
                connectionInfo.Inheritance.SoundQuality = MiscTools.GetBooleanValue(dataRow["InheritSoundQuality"]);
            connectionInfo.Inheritance.SSHOptions = MiscTools.GetBooleanValue(dataRow["InheritSSHOptions"]);
            connectionInfo.Inheritance.SSHTunnelConnectionName = MiscTools.GetBooleanValue(dataRow["InheritSSHTunnelConnectionName"]);
            connectionInfo.Inheritance.UseConsoleSession = MiscTools.GetBooleanValue(dataRow["InheritUseConsoleSession"]);
            connectionInfo.Inheritance.UseCredSsp = MiscTools.GetBooleanValue(dataRow["InheritUseCredSsp"]);
            connectionInfo.Inheritance.UseEnhancedMode = MiscTools.GetBooleanValue(dataRow["InheritUseEnhancedMode"]);
            connectionInfo.Inheritance.UseRCG = MiscTools.GetBooleanValue(dataRow["InheritUseRCG"]);
            connectionInfo.Inheritance.UseRestrictedAdmin = MiscTools.GetBooleanValue(dataRow["InheritUseRestrictedAdmin"]);
            connectionInfo.Inheritance.UserField = MiscTools.GetBooleanValue(dataRow["InheritUserField"]);
            if (dataRow.Table.Columns.Contains("InheritEnvironmentTags"))
                connectionInfo.Inheritance.EnvironmentTags = MiscTools.GetBooleanValue(dataRow["InheritEnvironmentTags"]);
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
            if (dataRow.Table.Columns.Contains("InheritVNCClipboardRedirect"))
                connectionInfo.Inheritance.VNCClipboardRedirect = MiscTools.GetBooleanValue(dataRow["InheritVNCClipboardRedirect"]);
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
            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo rootNode = new(RootNodeType.Connection, "0")
            {
                PasswordString = _decryptionKey.ConvertToUnsecureString()
            };
            connectionTreeModel.AddRootNode(rootNode);

            Dictionary<string, ConnectionInfo> nodeById = new(connectionList.Count, StringComparer.Ordinal);
            foreach (ConnectionInfo node in connectionList)
                nodeById[node.ConstantID] = node;

            foreach (DataRow row in dataTable.Rows)
            {
                string id = row["ConstantID"] as string ?? "";
                if (string.IsNullOrEmpty(id))
                    continue;

                // Track every connection ID from the database so we can distinguish
                // user-deleted connections from connections added by other users (#1424).
                connectionTreeModel.TrackLoadedConnectionId(id);

                if (!nodeById.TryGetValue(id, out ConnectionInfo? connectionInfo))
                    continue;
                string parentId = row["ParentID"] as string ?? "0";
                if (string.Equals(parentId, "0", StringComparison.Ordinal) || !nodeById.TryGetValue(parentId, out ConnectionInfo? parentNode))
                    rootNode.AddChild(connectionInfo);
                else
                    (parentNode as ContainerInfo)?.AddChild(connectionInfo);
            }

            return connectionTreeModel;
        }
    }
}
