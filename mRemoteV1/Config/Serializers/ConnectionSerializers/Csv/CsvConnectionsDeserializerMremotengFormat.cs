using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Tools;
using mRemoteNG.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.Csv
{
    public class CsvConnectionsDeserializerMremotengFormat
    {
        public SerializationResult Deserialize(string serializedData)
        {
            var lines = serializedData.Split(new []{"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            var csvHeaders = new List<string>();
            // used to map a connectioninfo to it's parent's GUID
            var parentMapping = new Dictionary<ConnectionInfo, string>();

            for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var line = lines[lineNumber].Split(';');
                if (lineNumber == 0)
                    csvHeaders = line.ToList();
                else
                {
                    var connectionInfo = ParseConnectionInfo(csvHeaders, line);

                    var parentFieldIndex = csvHeaders.IndexOf("Parent");
                    if (parentFieldIndex > 0)
                        parentMapping.Add(connectionInfo, line[parentFieldIndex]);
                }
            }

            var root = CreateTreeStructure(parentMapping);
            var harvestedCredentials = new CredentialHarvester()
                .Harvest(new HarvestConfig<string[]>
                {
                    ItemEnumerator = () => lines.Skip(1).Select(s => s.Split(';')),
                    ConnectionGuidSelector = line => csvHeaders.Contains("Id") ? Guid.Parse(line[csvHeaders.IndexOf("Id")]) : Guid.NewGuid(),
                    TitleSelector = line => "",
                    UsernameSelector = line => csvHeaders.Contains("Username") ? line[csvHeaders.IndexOf("Username")] : "",
                    DomainSelector = line => csvHeaders.Contains("Domain") ? line[csvHeaders.IndexOf("Domain")] : "",
                    PasswordSelector = line => csvHeaders.Contains("Password") ? line[csvHeaders.IndexOf("Password")].ConvertToSecureString() : new SecureString()
                });

            var result = new SerializationResult(root, harvestedCredentials.DistinctCredentialRecords.ToList(), harvestedCredentials);
            return result;
        }

        private List<ConnectionInfo> CreateTreeStructure(Dictionary<ConnectionInfo, string> parentMapping)
        {
            var root = new List<ConnectionInfo>();

            foreach (var node in parentMapping)
            {
                // no parent mapped, add to root
                if (string.IsNullOrEmpty(node.Value))
                {
                    root.Add(node.Key);
                    continue;
                }

                // search for parent in the list by GUID
                var parent = parentMapping
                    .Keys
                    .OfType<ContainerInfo>()
                    .FirstOrDefault(info => info.ConstantID == node.Value);

                if (parent != null)
                {
                    parent.AddChild(node.Key);
                }
                else
                {
                    root.Add(node.Key);
                }
            }

            return root;
        }

        private ConnectionInfo ParseConnectionInfo(IList<string> headers, string[] connectionCsv)
        {
            var nodeType = headers.Contains("NodeType")
                ? (TreeNodeType)Enum.Parse(typeof(TreeNodeType), connectionCsv[headers.IndexOf("NodeType")], true) 
                : TreeNodeType.Connection;

            var nodeId = headers.Contains("Id")
                ? connectionCsv[headers.IndexOf("Id")]
                : Guid.NewGuid().ToString();

            var connectionRecord = nodeType == TreeNodeType.Connection
                ? new ConnectionInfo(nodeId)
                : new ContainerInfo(nodeId);

            connectionRecord.Name = headers.Contains("Name") ? connectionCsv[headers.IndexOf("Name")] : "";
            connectionRecord.Description = headers.Contains("Description") ? connectionCsv[headers.IndexOf("Description")] : "";
            connectionRecord.Icon = headers.Contains("Icon") ? connectionCsv[headers.IndexOf("Icon")] : "";
            connectionRecord.Panel = headers.Contains("Panel") ? connectionCsv[headers.IndexOf("Panel")] : "";

            var hasCredRecordId = Guid.TryParse(
                headers.Contains("CredentialRecordId") ? connectionCsv[headers.IndexOf("CredentialRecordId")] : "",
                out var credRecordId);
            connectionRecord.CredentialRecordId = hasCredRecordId ? credRecordId : Optional<Guid>.Empty;

            // TODO: harvest
            connectionRecord.Username = headers.Contains("Username") ? connectionCsv[headers.IndexOf("Username")] : "";
            connectionRecord.Password = headers.Contains("Password") ? connectionCsv[headers.IndexOf("Password")] : "";
            connectionRecord.Domain = headers.Contains("Domain") ? connectionCsv[headers.IndexOf("Domain")] : "";

            connectionRecord.Hostname = headers.Contains("Hostname") ? connectionCsv[headers.IndexOf("Hostname")] : "";
            connectionRecord.PuttySession = headers.Contains("PuttySession") ? connectionCsv[headers.IndexOf("PuttySession")] : "";
            connectionRecord.LoadBalanceInfo = headers.Contains("LoadBalanceInfo") ? connectionCsv[headers.IndexOf("LoadBalanceInfo")] : "";
            connectionRecord.PreExtApp = headers.Contains("PreExtApp") ? connectionCsv[headers.IndexOf("PreExtApp")] : "";
            connectionRecord.PostExtApp = headers.Contains("PostExtApp") ? connectionCsv[headers.IndexOf("PostExtApp")] : "";
            connectionRecord.MacAddress = headers.Contains("MacAddress") ? connectionCsv[headers.IndexOf("MacAddress")] : "";
            connectionRecord.UserField = headers.Contains("UserField") ? connectionCsv[headers.IndexOf("UserField")] : "";
            connectionRecord.ExtApp = headers.Contains("ExtApp") ? connectionCsv[headers.IndexOf("ExtApp")] : "";
            connectionRecord.VNCProxyUsername = headers.Contains("VNCProxyUsername") ? connectionCsv[headers.IndexOf("VNCProxyUsername")] : "";
            connectionRecord.VNCProxyPassword = headers.Contains("VNCProxyPassword") ? connectionCsv[headers.IndexOf("VNCProxyPassword")] : "";
            connectionRecord.RDGatewayUsername = headers.Contains("RDGatewayUsername") ? connectionCsv[headers.IndexOf("RDGatewayUsername")] : "";
            connectionRecord.RDGatewayPassword = headers.Contains("RDGatewayPassword") ? connectionCsv[headers.IndexOf("RDGatewayPassword")] : "";
            connectionRecord.RDGatewayDomain = headers.Contains("RDGatewayDomain") ? connectionCsv[headers.IndexOf("RDGatewayDomain")] : "";
            connectionRecord.VNCProxyIP = headers.Contains("VNCProxyIP") ? connectionCsv[headers.IndexOf("VNCProxyIP")] : "";
            connectionRecord.RDGatewayHostname = headers.Contains("RDGatewayHostname") ? connectionCsv[headers.IndexOf("RDGatewayHostname")] : "";

            if (headers.Contains("CredentialId"))
            {
                if (Guid.TryParse(connectionCsv[headers.IndexOf("CredentialId")], out var credId))
                    connectionRecord.CredentialRecordId = credId;
            }

            if (headers.Contains("Protocol"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Protocol")], out ProtocolType protocolType))
                    connectionRecord.Protocol = protocolType;
            }

            if (headers.Contains("Port"))
            {
                if (int.TryParse(connectionCsv[headers.IndexOf("Port")], out int port))
                    connectionRecord.Port = port;
            }

            if (headers.Contains("ConnectToConsole"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("ConnectToConsole")], out bool useConsoleSession))
                    connectionRecord.UseConsoleSession = useConsoleSession;
            }

            if (headers.Contains("UseCredSsp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseCredSsp")], out bool value))
                    connectionRecord.UseCredSsp = value;
            }

            if (headers.Contains("RenderingEngine"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RenderingEngine")], out HTTPBase.RenderingEngine value))
                    connectionRecord.RenderingEngine = value;
            }

            if (headers.Contains("ICAEncryptionStrength"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("ICAEncryptionStrength")], out IcaProtocol.EncryptionStrength value))
                    connectionRecord.ICAEncryptionStrength = value;
            }

            if (headers.Contains("RDPAuthenticationLevel"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDPAuthenticationLevel")], out RdpProtocol.AuthenticationLevel value))
                    connectionRecord.RDPAuthenticationLevel = value;
            }

            if (headers.Contains("Colors"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Colors")], out RdpProtocol.RDPColors value))
                    connectionRecord.Colors = value;
            }

            if (headers.Contains("Resolution"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Resolution")], out RdpProtocol.RDPResolutions value))
                    connectionRecord.Resolution = value;
            }

            if (headers.Contains("AutomaticResize"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("AutomaticResize")], out bool value))
                    connectionRecord.AutomaticResize = value;
            }

            if (headers.Contains("DisplayWallpaper"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisplayWallpaper")], out bool value))
                    connectionRecord.DisplayWallpaper = value;
            }

            if (headers.Contains("DisplayThemes"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisplayThemes")], out bool value))
                    connectionRecord.DisplayThemes = value;
            }

            if (headers.Contains("EnableFontSmoothing"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("EnableFontSmoothing")], out bool value))
                    connectionRecord.EnableFontSmoothing = value;
            }

            if (headers.Contains("EnableDesktopComposition"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("EnableDesktopComposition")], out bool value))
                    connectionRecord.EnableDesktopComposition = value;
            }

            if (headers.Contains("CacheBitmaps"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("CacheBitmaps")], out bool value))
                    connectionRecord.CacheBitmaps = value;
            }

            if (headers.Contains("RedirectDiskDrives"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectDiskDrives")], out bool value))
                    connectionRecord.RedirectDiskDrives = value;
            }

            if (headers.Contains("RedirectPorts"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectPorts")], out bool value))
                    connectionRecord.RedirectPorts = value;
            }

            if (headers.Contains("RedirectPrinters"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectPrinters")], out bool value))
                    connectionRecord.RedirectPrinters = value;
            }
            if (headers.Contains("RedirectClipboard"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectClipboard")], out bool value))
                    connectionRecord.RedirectClipboard = value;
            }

            if (headers.Contains("RedirectSmartCards"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectSmartCards")], out bool value))
                    connectionRecord.RedirectSmartCards = value;
            }

            if (headers.Contains("RedirectSound"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RedirectSound")], out RdpProtocol.RDPSounds value))
                    connectionRecord.RedirectSound = value;
            }

            if (headers.Contains("RedirectKeys"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectKeys")], out bool value))
                    connectionRecord.RedirectKeys = value;
            }

            if (headers.Contains("VNCCompression"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCCompression")], out ProtocolVNC.Compression value))
                    connectionRecord.VNCCompression = value;
            }

            if (headers.Contains("VNCEncoding"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCEncoding")], out ProtocolVNC.Encoding value))
                    connectionRecord.VNCEncoding = value;
            }

            if (headers.Contains("VNCAuthMode"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCAuthMode")], out ProtocolVNC.AuthMode value))
                    connectionRecord.VNCAuthMode = value;
            }

            if (headers.Contains("VNCProxyType"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCProxyType")], out ProtocolVNC.ProxyType value))
                    connectionRecord.VNCProxyType = value;
            }

            if (headers.Contains("VNCProxyPort"))
            {
                if (int.TryParse(connectionCsv[headers.IndexOf("VNCProxyPort")], out int value))
                    connectionRecord.VNCProxyPort = value;
            }

            if (headers.Contains("VNCColors"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCColors")], out ProtocolVNC.Colors value))
                    connectionRecord.VNCColors = value;
            }

            if (headers.Contains("VNCSmartSizeMode"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCSmartSizeMode")], out ProtocolVNC.SmartSizeMode value))
                    connectionRecord.VNCSmartSizeMode = value;
            }

            if (headers.Contains("VNCViewOnly"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("VNCViewOnly")], out bool value))
                    connectionRecord.VNCViewOnly = value;
            }

            if (headers.Contains("RDGatewayUsageMethod"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDGatewayUsageMethod")], out RdpProtocol.RDGatewayUsageMethod value))
                    connectionRecord.RDGatewayUsageMethod = value;
            }

            if (headers.Contains("RDGatewayUseConnectionCredentials"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDGatewayUseConnectionCredentials")], out RdpProtocol.RDGatewayUseConnectionCredentials value))
                    connectionRecord.RDGatewayUseConnectionCredentials = value;
            }

            #region Inheritance
            if (headers.Contains("InheritCacheBitmaps"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritCacheBitmaps")], out bool value))
                    connectionRecord.Inheritance.CacheBitmaps = value;
            }

            if (headers.Contains("InheritColors"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritColors")], out bool value))
                    connectionRecord.Inheritance.Colors = value;
            }

            if (headers.Contains("InheritDescription"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDescription")], out bool value))
                    connectionRecord.Inheritance.Description = value;
            }

            if (headers.Contains("InheritDisplayThemes"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisplayThemes")], out bool value))
                    connectionRecord.Inheritance.DisplayThemes = value;
            }

            if (headers.Contains("InheritDisplayWallpaper"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisplayWallpaper")], out bool value))
                    connectionRecord.Inheritance.DisplayWallpaper = value;
            }

            if (headers.Contains("InheritEnableFontSmoothing"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritEnableFontSmoothing")], out bool value))
                    connectionRecord.Inheritance.EnableFontSmoothing = value;
            }

            if (headers.Contains("InheritEnableDesktopComposition"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritEnableDesktopComposition")], out bool value))
                    connectionRecord.Inheritance.EnableDesktopComposition = value;
            }

            if (headers.Contains("InheritDomain"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDomain")], out bool value))
                    connectionRecord.Inheritance.Domain = value;
            }

            if (headers.Contains("InheritIcon"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritIcon")], out bool value))
                    connectionRecord.Inheritance.Icon = value;
            }

            if (headers.Contains("InheritPanel"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPanel")], out bool value))
                    connectionRecord.Inheritance.Panel = value;
            }

            if (headers.Contains("InheritPassword"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPassword")], out bool value))
                    connectionRecord.Inheritance.Password = value;
            }

            if (headers.Contains("InheritPort"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPort")], out bool value))
                    connectionRecord.Inheritance.Port = value;
            }

            if (headers.Contains("InheritProtocol"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritProtocol")], out bool value))
                    connectionRecord.Inheritance.Protocol = value;
            }

            if (headers.Contains("InheritPuttySession"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPuttySession")], out bool value))
                    connectionRecord.Inheritance.PuttySession = value;
            }

            if (headers.Contains("InheritRedirectDiskDrives"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectDiskDrives")], out bool value))
                    connectionRecord.Inheritance.RedirectDiskDrives = value;
            }

            if (headers.Contains("InheritRedirectKeys"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectKeys")], out bool value))
                    connectionRecord.Inheritance.RedirectKeys = value;
            }

            if (headers.Contains("InheritRedirectPorts"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectPorts")], out bool value))
                    connectionRecord.Inheritance.RedirectPorts = value;
            }

            if (headers.Contains("InheritRedirectPrinters"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectPrinters")], out bool value))
                    connectionRecord.Inheritance.RedirectPrinters = value;
            }

            if (headers.Contains("InheritRedirectClipboard"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectClipboard")], out bool value))
                    connectionRecord.Inheritance.RedirectClipboard = value;
            }

            if (headers.Contains("InheritRedirectSmartCards"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectSmartCards")], out bool value))
                    connectionRecord.Inheritance.RedirectSmartCards = value;
            }

            if (headers.Contains("InheritRedirectSound"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectSound")], out bool value))
                    connectionRecord.Inheritance.RedirectSound = value;
            }

            if (headers.Contains("InheritResolution"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritResolution")], out bool value))
                    connectionRecord.Inheritance.Resolution = value;
            }

            if (headers.Contains("InheritAutomaticResize"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritAutomaticResize")], out bool value))
                    connectionRecord.Inheritance.AutomaticResize = value;
            }

            if (headers.Contains("InheritUseConsoleSession"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseConsoleSession")], out bool value))
                    connectionRecord.Inheritance.UseConsoleSession = value;
            }

            if (headers.Contains("InheritUseCredSsp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseCredSsp")], out bool value))
                    connectionRecord.Inheritance.UseCredSsp = value;
            }

            if (headers.Contains("InheritRenderingEngine"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRenderingEngine")], out bool value))
                    connectionRecord.Inheritance.RenderingEngine = value;
            }

            if (headers.Contains("InheritUsername"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUsername")], out bool value))
                    connectionRecord.Inheritance.Username = value;
            }

            if (headers.Contains("InheritICAEncryptionStrength"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritICAEncryptionStrength")], out bool value))
                    connectionRecord.Inheritance.ICAEncryptionStrength = value;
            }

            if (headers.Contains("InheritRDPAuthenticationLevel"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPAuthenticationLevel")], out bool value))
                    connectionRecord.Inheritance.RDPAuthenticationLevel = value;
            }

            if (headers.Contains("InheritLoadBalanceInfo"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritLoadBalanceInfo")], out bool value))
                    connectionRecord.Inheritance.LoadBalanceInfo = value;
            }

            if (headers.Contains("InheritPreExtApp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPreExtApp")], out bool value))
                    connectionRecord.Inheritance.PreExtApp = value;
            }

            if (headers.Contains("InheritPostExtApp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPostExtApp")], out bool value))
                    connectionRecord.Inheritance.PostExtApp = value;
            }

            if (headers.Contains("InheritMacAddress"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritMacAddress")], out bool value))
                    connectionRecord.Inheritance.MacAddress = value;
            }

            if (headers.Contains("InheritUserField"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUserField")], out bool value))
                    connectionRecord.Inheritance.UserField = value;
            }

            if (headers.Contains("InheritExtApp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritExtApp")], out bool value))
                    connectionRecord.Inheritance.ExtApp = value;
            }

            if (headers.Contains("InheritVNCCompression"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCCompression")], out bool value))
                    connectionRecord.Inheritance.VNCCompression = value;
            }

            if (headers.Contains("InheritVNCEncoding"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCEncoding")], out bool value))
                    connectionRecord.Inheritance.VNCEncoding = value;
            }

            if (headers.Contains("InheritVNCAuthMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCAuthMode")], out bool value))
                    connectionRecord.Inheritance.VNCAuthMode = value;
            }

            if (headers.Contains("InheritVNCProxyType"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyType")], out bool value))
                    connectionRecord.Inheritance.VNCProxyType = value;
            }

            if (headers.Contains("InheritVNCProxyIP"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyIP")], out bool value))
                    connectionRecord.Inheritance.VNCProxyIP = value;
            }

            if (headers.Contains("InheritVNCProxyPort"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyPort")], out bool value))
                    connectionRecord.Inheritance.VNCProxyPort = value;
            }

            if (headers.Contains("InheritVNCProxyUsername"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyUsername")], out bool value))
                    connectionRecord.Inheritance.VNCProxyUsername = value;
            }

            if (headers.Contains("InheritVNCProxyPassword"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyPassword")], out bool value))
                    connectionRecord.Inheritance.VNCProxyPassword = value;
            }

            if (headers.Contains("InheritVNCColors"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCColors")], out bool value))
                    connectionRecord.Inheritance.VNCColors = value;
            }

            if (headers.Contains("InheritVNCSmartSizeMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCSmartSizeMode")], out bool value))
                    connectionRecord.Inheritance.VNCSmartSizeMode = value;
            }

            if (headers.Contains("InheritVNCViewOnly"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCViewOnly")], out bool value))
                    connectionRecord.Inheritance.VNCViewOnly = value;
            }

            if (headers.Contains("InheritRDGatewayUsageMethod"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUsageMethod")], out bool value))
                    connectionRecord.Inheritance.RDGatewayUsageMethod = value;
            }

            if (headers.Contains("InheritRDGatewayHostname"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayHostname")], out bool value))
                    connectionRecord.Inheritance.RDGatewayHostname = value;
            }

            if (headers.Contains("InheritRDGatewayUseConnectionCredentials"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUseConnectionCredentials")], out bool value))
                    connectionRecord.Inheritance.RDGatewayUseConnectionCredentials = value;
            }

            if (headers.Contains("InheritRDGatewayUsername"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUsername")], out bool value))
                    connectionRecord.Inheritance.RDGatewayUsername = value;
            }

            if (headers.Contains("InheritRDGatewayPassword"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayPassword")], out bool value))
                    connectionRecord.Inheritance.RDGatewayPassword = value;
            }

            if (headers.Contains("InheritRDGatewayDomain"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayDomain")], out bool value))
                    connectionRecord.Inheritance.RDGatewayDomain = value;
            }

            if (headers.Contains("InheritRDPAlertIdleTimeout"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPAlertIdleTimeout")], out bool value))
                    connectionRecord.Inheritance.RDPAlertIdleTimeout = value;
            }

            if (headers.Contains("InheritRDPMinutesToIdleTimeout"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPMinutesToIdleTimeout")], out bool value))
                    connectionRecord.Inheritance.RDPMinutesToIdleTimeout = value;
            }

            if (headers.Contains("InheritSoundQuality"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritSoundQuality")], out bool value))
                    connectionRecord.Inheritance.SoundQuality = value;
            }

            if (headers.Contains("InheritCredentialRecord"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritCredentialRecord")], out bool value))
                    connectionRecord.Inheritance.CredentialId = value;
            }
            #endregion

            return connectionRecord;
        }
    }
}
