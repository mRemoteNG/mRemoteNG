using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.Csv
{
    public class CsvConnectionsDeserializerMremotengFormat : IDeserializer<string, ConnectionTreeModel>
    {
        public ConnectionTreeModel Deserialize(string serializedData)
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
                    parentMapping.Add(connectionInfo, line[csvHeaders.IndexOf("Parent")]);
                }
            }

            var root = CreateTreeStructure(parentMapping);
            var connectionTreeModel = new ConnectionTreeModel();
            connectionTreeModel.AddRootNode(root);
            return connectionTreeModel;
        }

        private RootNodeInfo CreateTreeStructure(Dictionary<ConnectionInfo, string> parentMapping)
        {
            var root = new RootNodeInfo(RootNodeType.Connection);

            foreach (var node in parentMapping)
            {
                // no parent mapped, add to root
                if (string.IsNullOrEmpty(node.Value))
                {
                    root.AddChild(node.Key);
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
                    root.AddChild(node.Key);
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

            if (headers.Contains("Protocol"))
            {
                ProtocolType protocolType;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Protocol")], out protocolType))
                    connectionRecord.Protocol = protocolType;
            }

            if (headers.Contains("Port"))
            {
                int port;
                if (int.TryParse(connectionCsv[headers.IndexOf("Port")], out port))
                    connectionRecord.Port = port;
            }

            if (headers.Contains("ConnectToConsole"))
            {
                bool useConsoleSession;
                if (bool.TryParse(connectionCsv[headers.IndexOf("ConnectToConsole")], out useConsoleSession))
                    connectionRecord.UseConsoleSession = useConsoleSession;
            }

            if (headers.Contains("UseCredSsp"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseCredSsp")], out value))
                    connectionRecord.UseCredSsp = value;
            }

            if (headers.Contains("RenderingEngine"))
            {
                HTTPBase.RenderingEngine value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RenderingEngine")], out value))
                    connectionRecord.RenderingEngine = value;
            }

            if (headers.Contains("ICAEncryptionStrength"))
            {
                IcaProtocol.EncryptionStrength value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("ICAEncryptionStrength")], out value))
                    connectionRecord.ICAEncryptionStrength = value;
            }

            if (headers.Contains("RDPAuthenticationLevel"))
            {
                RdpProtocol.AuthenticationLevel value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDPAuthenticationLevel")], out value))
                    connectionRecord.RDPAuthenticationLevel = value;
            }

            if (headers.Contains("Colors"))
            {
                RdpProtocol.RDPColors value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Colors")], out value))
                    connectionRecord.Colors = value;
            }

            if (headers.Contains("Resolution"))
            {
                RdpProtocol.RDPResolutions value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Resolution")], out value))
                    connectionRecord.Resolution = value;
            }

            if (headers.Contains("AutomaticResize"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("AutomaticResize")], out value))
                    connectionRecord.AutomaticResize = value;
            }

            if (headers.Contains("DisplayWallpaper"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisplayWallpaper")], out value))
                    connectionRecord.DisplayWallpaper = value;
            }

            if (headers.Contains("DisplayThemes"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisplayThemes")], out value))
                    connectionRecord.DisplayThemes = value;
            }

            if (headers.Contains("EnableFontSmoothing"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("EnableFontSmoothing")], out value))
                    connectionRecord.EnableFontSmoothing = value;
            }

            if (headers.Contains("EnableDesktopComposition"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("EnableDesktopComposition")], out value))
                    connectionRecord.EnableDesktopComposition = value;
            }

            if (headers.Contains("CacheBitmaps"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("CacheBitmaps")], out value))
                    connectionRecord.CacheBitmaps = value;
            }

            if (headers.Contains("RedirectDiskDrives"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectDiskDrives")], out value))
                    connectionRecord.RedirectDiskDrives = value;
            }

            if (headers.Contains("RedirectPorts"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectPorts")], out value))
                    connectionRecord.RedirectPorts = value;
            }

            if (headers.Contains("RedirectPrinters"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectPrinters")], out value))
                    connectionRecord.RedirectPrinters = value;
            }

            if (headers.Contains("RedirectSmartCards"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectSmartCards")], out value))
                    connectionRecord.RedirectSmartCards = value;
            }

            if (headers.Contains("RedirectSound"))
            {
                RdpProtocol.RDPSounds value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RedirectSound")], out value))
                    connectionRecord.RedirectSound = value;
            }

            if (headers.Contains("RedirectKeys"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectKeys")], out value))
                    connectionRecord.RedirectKeys = value;
            }

            if (headers.Contains("VNCCompression"))
            {
                ProtocolVNC.Compression value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCCompression")], out value))
                    connectionRecord.VNCCompression = value;
            }

            if (headers.Contains("VNCEncoding"))
            {
                ProtocolVNC.Encoding value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCEncoding")], out value))
                    connectionRecord.VNCEncoding = value;
            }

            if (headers.Contains("VNCAuthMode"))
            {
                ProtocolVNC.AuthMode value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCAuthMode")], out value))
                    connectionRecord.VNCAuthMode = value;
            }

            if (headers.Contains("VNCProxyType"))
            {
                ProtocolVNC.ProxyType value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCProxyType")], out value))
                    connectionRecord.VNCProxyType = value;
            }

            if (headers.Contains("VNCProxyPort"))
            {
                int value;
                if (int.TryParse(connectionCsv[headers.IndexOf("VNCProxyPort")], out value))
                    connectionRecord.VNCProxyPort = value;
            }

            if (headers.Contains("VNCColors"))
            {
                ProtocolVNC.Colors value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCColors")], out value))
                    connectionRecord.VNCColors = value;
            }

            if (headers.Contains("VNCSmartSizeMode"))
            {
                ProtocolVNC.SmartSizeMode value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCSmartSizeMode")], out value))
                    connectionRecord.VNCSmartSizeMode = value;
            }

            if (headers.Contains("VNCViewOnly"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("VNCViewOnly")], out value))
                    connectionRecord.VNCViewOnly = value;
            }

            if (headers.Contains("RDGatewayUsageMethod"))
            {
                RdpProtocol.RDGatewayUsageMethod value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDGatewayUsageMethod")], out value))
                    connectionRecord.RDGatewayUsageMethod = value;
            }

            if (headers.Contains("RDGatewayUseConnectionCredentials"))
            {
                RdpProtocol.RDGatewayUseConnectionCredentials value;
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDGatewayUseConnectionCredentials")], out value))
                    connectionRecord.RDGatewayUseConnectionCredentials = value;
            }

            #region Inheritance
            if (headers.Contains("InheritCacheBitmaps"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritCacheBitmaps")], out value))
                    connectionRecord.Inheritance.CacheBitmaps = value;
            }

            if (headers.Contains("InheritColors"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritColors")], out value))
                    connectionRecord.Inheritance.Colors = value;
            }

            if (headers.Contains("InheritDescription"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDescription")], out value))
                    connectionRecord.Inheritance.Description = value;
            }

            if (headers.Contains("InheritDisplayThemes"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisplayThemes")], out value))
                    connectionRecord.Inheritance.DisplayThemes = value;
            }

            if (headers.Contains("InheritDisplayWallpaper"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisplayWallpaper")], out value))
                    connectionRecord.Inheritance.DisplayWallpaper = value;
            }

            if (headers.Contains("InheritEnableFontSmoothing"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritEnableFontSmoothing")], out value))
                    connectionRecord.Inheritance.EnableFontSmoothing = value;
            }

            if (headers.Contains("InheritEnableDesktopComposition"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritEnableDesktopComposition")], out value))
                    connectionRecord.Inheritance.EnableDesktopComposition = value;
            }

            if (headers.Contains("InheritDomain"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDomain")], out value))
                    connectionRecord.Inheritance.Domain = value;
            }

            if (headers.Contains("InheritIcon"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritIcon")], out value))
                    connectionRecord.Inheritance.Icon = value;
            }

            if (headers.Contains("InheritPanel"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPanel")], out value))
                    connectionRecord.Inheritance.Panel = value;
            }

            if (headers.Contains("InheritPassword"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPassword")], out value))
                    connectionRecord.Inheritance.Password = value;
            }

            if (headers.Contains("InheritPort"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPort")], out value))
                    connectionRecord.Inheritance.Port = value;
            }

            if (headers.Contains("InheritProtocol"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritProtocol")], out value))
                    connectionRecord.Inheritance.Protocol = value;
            }

            if (headers.Contains("InheritPuttySession"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPuttySession")], out value))
                    connectionRecord.Inheritance.PuttySession = value;
            }

            if (headers.Contains("InheritRedirectDiskDrives"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectDiskDrives")], out value))
                    connectionRecord.Inheritance.RedirectDiskDrives = value;
            }

            if (headers.Contains("InheritRedirectKeys"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectKeys")], out value))
                    connectionRecord.Inheritance.RedirectKeys = value;
            }

            if (headers.Contains("InheritRedirectPorts"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectPorts")], out value))
                    connectionRecord.Inheritance.RedirectPorts = value;
            }

            if (headers.Contains("InheritRedirectPrinters"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectPrinters")], out value))
                    connectionRecord.Inheritance.RedirectPrinters = value;
            }

            if (headers.Contains("InheritRedirectSmartCards"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectSmartCards")], out value))
                    connectionRecord.Inheritance.RedirectSmartCards = value;
            }

            if (headers.Contains("InheritRedirectSound"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectSound")], out value))
                    connectionRecord.Inheritance.RedirectSound = value;
            }

            if (headers.Contains("InheritResolution"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritResolution")], out value))
                    connectionRecord.Inheritance.Resolution = value;
            }

            if (headers.Contains("InheritAutomaticResize"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritAutomaticResize")], out value))
                    connectionRecord.Inheritance.AutomaticResize = value;
            }

            if (headers.Contains("InheritUseConsoleSession"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseConsoleSession")], out value))
                    connectionRecord.Inheritance.UseConsoleSession = value;
            }

            if (headers.Contains("InheritUseCredSsp"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseCredSsp")], out value))
                    connectionRecord.Inheritance.UseCredSsp = value;
            }

            if (headers.Contains("InheritRenderingEngine"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRenderingEngine")], out value))
                    connectionRecord.Inheritance.RenderingEngine = value;
            }

            if (headers.Contains("InheritUsername"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUsername")], out value))
                    connectionRecord.Inheritance.Username = value;
            }

            if (headers.Contains("InheritICAEncryptionStrength"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritICAEncryptionStrength")], out value))
                    connectionRecord.Inheritance.ICAEncryptionStrength = value;
            }

            if (headers.Contains("InheritRDPAuthenticationLevel"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPAuthenticationLevel")], out value))
                    connectionRecord.Inheritance.RDPAuthenticationLevel = value;
            }

            if (headers.Contains("InheritLoadBalanceInfo"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritLoadBalanceInfo")], out value))
                    connectionRecord.Inheritance.LoadBalanceInfo = value;
            }

            if (headers.Contains("InheritPreExtApp"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPreExtApp")], out value))
                    connectionRecord.Inheritance.PreExtApp = value;
            }

            if (headers.Contains("InheritPostExtApp"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPostExtApp")], out value))
                    connectionRecord.Inheritance.PostExtApp = value;
            }

            if (headers.Contains("InheritMacAddress"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritMacAddress")], out value))
                    connectionRecord.Inheritance.MacAddress = value;
            }

            if (headers.Contains("InheritUserField"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUserField")], out value))
                    connectionRecord.Inheritance.UserField = value;
            }

            if (headers.Contains("InheritExtApp"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritExtApp")], out value))
                    connectionRecord.Inheritance.ExtApp = value;
            }

            if (headers.Contains("InheritVNCCompression"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCCompression")], out value))
                    connectionRecord.Inheritance.VNCCompression = value;
            }

            if (headers.Contains("InheritVNCEncoding"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCEncoding")], out value))
                    connectionRecord.Inheritance.VNCEncoding = value;
            }

            if (headers.Contains("InheritVNCAuthMode"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCAuthMode")], out value))
                    connectionRecord.Inheritance.VNCAuthMode = value;
            }

            if (headers.Contains("InheritVNCProxyType"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyType")], out value))
                    connectionRecord.Inheritance.VNCProxyType = value;
            }

            if (headers.Contains("InheritVNCProxyIP"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyIP")], out value))
                    connectionRecord.Inheritance.VNCProxyIP = value;
            }

            if (headers.Contains("InheritVNCProxyPort"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyPort")], out value))
                    connectionRecord.Inheritance.VNCProxyPort = value;
            }

            if (headers.Contains("InheritVNCProxyUsername"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyUsername")], out value))
                    connectionRecord.Inheritance.VNCProxyUsername = value;
            }

            if (headers.Contains("InheritVNCProxyPassword"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyPassword")], out value))
                    connectionRecord.Inheritance.VNCProxyPassword = value;
            }

            if (headers.Contains("InheritVNCColors"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCColors")], out value))
                    connectionRecord.Inheritance.VNCColors = value;
            }

            if (headers.Contains("InheritVNCSmartSizeMode"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCSmartSizeMode")], out value))
                    connectionRecord.Inheritance.VNCSmartSizeMode = value;
            }

            if (headers.Contains("InheritVNCViewOnly"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCViewOnly")], out value))
                    connectionRecord.Inheritance.VNCViewOnly = value;
            }

            if (headers.Contains("InheritRDGatewayUsageMethod"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUsageMethod")], out value))
                    connectionRecord.Inheritance.RDGatewayUsageMethod = value;
            }

            if (headers.Contains("InheritRDGatewayHostname"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayHostname")], out value))
                    connectionRecord.Inheritance.RDGatewayHostname = value;
            }

            if (headers.Contains("InheritRDGatewayUseConnectionCredentials"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUseConnectionCredentials")], out value))
                    connectionRecord.Inheritance.RDGatewayUseConnectionCredentials = value;
            }

            if (headers.Contains("InheritRDGatewayUsername"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUsername")], out value))
                    connectionRecord.Inheritance.RDGatewayUsername = value;
            }

            if (headers.Contains("InheritRDGatewayPassword"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayPassword")], out value))
                    connectionRecord.Inheritance.RDGatewayPassword = value;
            }

            if (headers.Contains("InheritRDGatewayDomain"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayDomain")], out value))
                    connectionRecord.Inheritance.RDGatewayDomain = value;
            }

            if (headers.Contains("InheritRDPAlertIdleTimeout"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPAlertIdleTimeout")], out value))
                    connectionRecord.Inheritance.RDPAlertIdleTimeout = value;
            }

            if (headers.Contains("InheritRDPMinutesToIdleTimeout"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPMinutesToIdleTimeout")], out value))
                    connectionRecord.Inheritance.RDPMinutesToIdleTimeout = value;
            }

            if (headers.Contains("InheritSoundQuality"))
            {
                bool value;
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritSoundQuality")], out value))
                    connectionRecord.Inheritance.SoundQuality = value;
            }
            #endregion

            return connectionRecord;
        }
    }
}
