using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    public class RemoteDesktopManagerDeserializer : IDeserializer<string, ConnectionTreeModel>
    {
        public ConnectionTreeModel Deserialize(string serializedData)
        {
            var connectionTreeModel = new ConnectionTreeModel();
            var root = new RootNodeInfo(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);

            var xdoc = XDocument.Parse(serializedData);
            var descendants = xdoc.Root.Descendants("Connection");
            foreach (var descendant in descendants)
            {
                var connection = CreateConnectionFromXml(descendant);
                root.AddChild(connection);
            }

            return connectionTreeModel;
        }

        private ConnectionInfo CreateConnectionFromXml(XElement xml)
        {
            var protocol = ParseProtocol(xml);
            var protocolSpecificNode = GetProtocolSpecificNode(xml, protocol);
            var metaInfoNode = xml.Element("MetaInformation");
            var connectionId = xml.GetChildElementAsString("ID", Guid.NewGuid().ToString());
            

            var connection = new ConnectionInfo(connectionId)
            {
                Name = xml.GetChildElementAsString("Name"),
                Protocol = protocol,
                Description = xml.GetChildElementAsString("Description"),
                Username = protocolSpecificNode.GetChildElementAsString("UserName",
                    protocolSpecificNode.GetChildElementAsString("Username",
                        protocolSpecificNode.GetChildElementAsString("MsUser"))),
                Domain = protocolSpecificNode.GetChildElementAsString("Domain",
                    protocolSpecificNode.GetChildElementAsString("MsDomain")),
                Password = protocolSpecificNode.GetChildElementAsString("SafePassword"),
                Resolution = ParseResolution(xml),
                AutomaticResize = protocolSpecificNode.GetChildElementAsString("ScreenSizingMode") == "AutoScale",
                RedirectSound = ParseSoundRedirection(xml),
                Colors = ParseScreenColors(xml),
                CacheBitmaps = !xml.GetChildElementAsBool("DisableBitmapCache"),
                DisplayThemes = !xml.GetChildElementAsBool("DisableThemes"),
                DisplayWallpaper = !xml.GetChildElementAsBool("DisableWallpaper"),
                EnableDesktopComposition = xml.GetChildElementAsBool("DesktopComposition"),
                EnableFontSmoothing = xml.GetChildElementAsBool("FontSmoothing"),
                RedirectAudioCapture = protocolSpecificNode.GetChildElementAsBool("AudioCaptureRedirectionMode"),
                RedirectClipboard = xml.GetChildElementAsBool("UsesClipboard", DefaultConnectionInfo.Instance.RedirectClipboard),
                RedirectDiskDrives = xml.GetChildElementAsBool("UsesHardDrives", DefaultConnectionInfo.Instance.RedirectDiskDrives),
                RedirectPrinters = xml.GetChildElementAsBool("UsesPrinters", DefaultConnectionInfo.Instance.RedirectPrinters),
                RedirectPorts = xml.GetChildElementAsBool("UsesSerialPorts", DefaultConnectionInfo.Instance.RedirectPorts),
                RedirectSmartCards = xml.GetChildElementAsBool("UsesSmartDevices", DefaultConnectionInfo.Instance.RedirectSmartCards),
                RDPMinutesToIdleTimeout = xml.GetChildElementAsInt("AutomaticallyCloseInterval"),
                MacAddress = metaInfoNode.GetChildElementAsString("MAC"),
                RdpVersion = ParseRdpVersion(protocolSpecificNode),
                RedirectKeys = xml.GetChildElementAsString("KeyboardHook") == "OnTheRemoteComputer",
                UseConsoleSession = xml.GetChildElementAsBool("Console"),
                RDPAuthenticationLevel = ParseAuthenticationLevel(xml),
                UseCredSsp = protocolSpecificNode.GetChildElementAsBool("EnableCredSSPSupport"),

                RDGatewayUsageMethod = ParseRdpGatewayUsageMethod(protocolSpecificNode),
                RDGatewayHostname = protocolSpecificNode.GetChildElementAsString("GatewayHostname"),
                UseEnhancedMode = protocolSpecificNode.GetChildElementAsBool("UseEnhancedSessionMode"),
                UseVmId = protocolSpecificNode.GetChildElementAsString("RDPType") == "HyperV",
                VmId = protocolSpecificNode.GetChildElementAsString("HyperVInstanceID"),

                VNCEncoding = ParseVncEncoding(protocolSpecificNode),
                VNCCompression = ParseVncCompressionLevel(protocolSpecificNode),
                VNCAuthMode = DetermineVncAuthMode(protocolSpecificNode),
                VNCViewOnly = protocolSpecificNode.GetChildElementAsBool("ViewOnly"),
                RenderingEngine = ParseWebRenderingEngine(xml),
            };

            SetHostnameAndPort(connection, xml, protocolSpecificNode);
            SetRdpGatewayCredentials(connection, protocolSpecificNode);
            SetVncProxyHostAndPort(connection, protocolSpecificNode);

            return connection;
        }

        private ProtocolType ParseProtocol(XElement xml)
        {
            switch (xml.GetChildElementAsString("ConnectionType"))
            {
                case "RDPConfigured":
                    return ProtocolType.RDP;
                case "SSHShell":
                    return ProtocolType.SSH2;
                case "VNC":
                    return ProtocolType.VNC;
                case "Telnet":
                    return ProtocolType.Telnet;
                case "WebBrowser":
                    return xml.GetChildElementAsString("WebBrowserUrl").StartsWith("http://")
                        ? ProtocolType.HTTP
                        : ProtocolType.HTTPS;
                default:
                    throw new Exception();
            }
        }

        private XElement GetProtocolSpecificNode(XElement xml, ProtocolType protocol)
        {
            switch (protocol)
            {
                case ProtocolType.RDP:
                    return xml.Element("RDP");
                case ProtocolType.SSH1:
                case ProtocolType.SSH2:
                case ProtocolType.Telnet:
                    return xml.Element("Terminal");
                case ProtocolType.VNC:
                    return xml.Element("VNC");
                case ProtocolType.HTTP:
                case ProtocolType.HTTPS:
                    return xml.Element("Web");
                default:
                    throw new Exception();
            }
        }

        private void SetHostnameAndPort(ConnectionInfo connection, XElement xml, XElement protocolSpecificNode)
        {
            var urlElement = xml.Element("Url") ?? xml.Element("WebBrowserUrl");
            if (urlElement != null)
            {
                var urlRegex = new Regex(@"((?<protocol>https?)://)?(?<host>[\w\.]+):?(?<port>\d*)");
                var urlMatch = urlRegex.Match(urlElement.Value);
                
                connection.Hostname = urlMatch.Groups["host"]?.Value ?? string.Empty;
                if (int.TryParse(urlMatch.Groups["port"]?.Value, out var port))
                {
                    connection.Port = port;
                }
            }
            else
            {
                connection.Hostname = protocolSpecificNode.GetChildElementAsString("Host");
                if (protocolSpecificNode.Element("HostPort") != null)
                {
                    connection.Port = protocolSpecificNode.GetChildElementAsInt("HostPort");
                }
                if (protocolSpecificNode.Element("Port") != null)
                {
                    connection.Port = protocolSpecificNode.GetChildElementAsInt("Port");
                }
            }
        }

        private RDPResolutions ParseResolution(XElement xml)
        {
            var resolution = xml.GetChildElementAsString("ScreenSize");

            // attempt to convert "R800x600" style descriptors
            // to our "Res800x600" style enum
            if (resolution.StartsWith("R"))
            {
                var fixedResolution = resolution.Replace("R", "Res");
                if (Enum.TryParse<RDPResolutions>(fixedResolution, out var convertedRes))
                {
                    return convertedRes;
                }
            }

            switch (resolution)
            {
                case "FullScreen":
                    return RDPResolutions.Fullscreen;
                case "CurrentWorkAreaSize":
                case "CurrentScreenSize":
                    return RDPResolutions.FitToWindow;
            }

            switch (xml.GetChildElementAsString("ScreenSizingMode"))
            {
                // This is confusing, but what they show as "Smart sizing" in the UI
                // gets serialized to FitToWindow in the xml.
                case "FitToWindow":
                    return RDPResolutions.SmartSize;
            }

            return DefaultConnectionInfo.Instance.Resolution;
        }

        private RDPSounds ParseSoundRedirection(XElement xml)
        {
            switch (xml.GetChildElementAsString("SoundHook"))
            {
                case "LeaveAtRemoteComputer":
                    return RDPSounds.LeaveAtRemoteComputer;
                case "DoNotPlay":
                    return RDPSounds.DoNotPlay;
                default: // there is no xml element when set to BringToThisComputer
                    return RDPSounds.BringToThisComputer;
            }
        }

        private RDPColors ParseScreenColors(XElement xml)
        {
            switch (xml.GetChildElementAsString("ScreenColor"))
            {
                case "C15Bits":
                    return RDPColors.Colors15Bit;
                case "C16Bits":
                    return RDPColors.Colors16Bit;
                case "C24Bits":
                    return RDPColors.Colors24Bit;
                case "C32Bits":
                    return RDPColors.Colors32Bit;
                case "C256":
                    return RDPColors.Colors256;
                default:
                    return DefaultConnectionInfo.Instance.Colors;
            }
        }

        private RdpVersion ParseRdpVersion(XElement xml)
        {
            switch (xml.GetChildElementAsString("Version"))
            {
                case "RDP50":
                case "RDP60":
                case "RDP61":
                    return RdpVersion.Rdc6;
                case "RDP70":
                    return RdpVersion.Rdc7;
                case "RDP80":
                case "RDP81":
                    return RdpVersion.Rdc8;
                default: // no xml node = use latest
                    return RdpVersion.Highest;
            }
        }

        private AuthenticationLevel ParseAuthenticationLevel(XElement xml)
        {
            switch (xml.GetChildElementAsString("AuthentificationLevel"))
            {
                case "WarnMe":
                    return AuthenticationLevel.WarnOnFailedAuth;
                case "DontConnect":
                    return AuthenticationLevel.AuthRequired;
                default: // "Connect and don't warn me"
                    return AuthenticationLevel.NoAuth;
            }
        }

        private RDGatewayUsageMethod ParseRdpGatewayUsageMethod(XElement xml)
        {
            switch (xml.GetChildElementAsString("GatewayUsageMethod"))
            {
                case "ModeDirect":
                    return RDGatewayUsageMethod.Always;
                case "NoneDetect":
                    return RDGatewayUsageMethod.Never;
                default: // no xml node = detect
                    return RDGatewayUsageMethod.Detect;
            }
        }

        private void SetRdpGatewayCredentials(ConnectionInfo connection, XElement xml)
        {
            if (xml.GetChildElementAsString("GatewayCredentialsSource") == "Smartcard")
            {
                connection.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.SmartCard;
            }
            else
            {
                connection.RDGatewayUseConnectionCredentials =
                    xml.GetChildElementAsBool("PromptCredentialOnce")
                        ? RDGatewayUseConnectionCredentials.Yes
                        : RDGatewayUseConnectionCredentials.No;
            }

            connection.RDGatewayUsername = xml.GetChildElementAsString("GatewayUserName");
            connection.RDGatewayDomain = xml.GetChildElementAsString("GatewayDomain");
            connection.RDGatewayPassword = xml.GetChildElementAsString("GatewaySafePassword");
        }

        private ProtocolVNC.Encoding ParseVncEncoding(XElement xml)
        {
            switch (xml.GetChildElementAsString("PreferredEncoding"))
            {
                case "CoRRE":
                    return ProtocolVNC.Encoding.EncCorre;
                case "Hextile":
                    return ProtocolVNC.Encoding.EncHextile;
                case "Raw":
                    return ProtocolVNC.Encoding.EncRaw;
                case "RRE":
                    return ProtocolVNC.Encoding.EncRRE;
                case "Tight":
                    return ProtocolVNC.Encoding.EncTight;
                case "Zlib":
                    return ProtocolVNC.Encoding.EncZlib;
                case "ZlibHEX":
                    return ProtocolVNC.Encoding.EncZLibHex;
                case "ZRLE":
                    return ProtocolVNC.Encoding.EncZRLE;
                default:
                    return DefaultConnectionInfo.Instance.VNCEncoding;
            }
        }

        private ProtocolVNC.Compression ParseVncCompressionLevel(XElement xml)
        {
            var compressionXml = xml.GetChildElementAsString("CustomCompressionLevel");
            return Enum.TryParse<ProtocolVNC.Compression>("Comp" + compressionXml, out var compression)
                ? compression
                : DefaultConnectionInfo.Instance.VNCCompression;
        }

        private ProtocolVNC.AuthMode DetermineVncAuthMode(XElement xml)
        {
            return xml.Element("MsUser") != null
                ? ProtocolVNC.AuthMode.AuthWin
                : ProtocolVNC.AuthMode.AuthVNC;
        }

        private void SetVncProxyHostAndPort(ConnectionInfo connection, XElement xml)
        {
            var proxy = xml.GetChildElementAsString("ProxyHost").Split(':');

            if (proxy.Length >= 1)
            {
                connection.VNCProxyIP = proxy[0];
            }

            if (proxy.Length >= 2 && int.TryParse(proxy[1], out var port))
            {
                connection.VNCProxyPort = port;
            }
        }

        private HTTPBase.RenderingEngine ParseWebRenderingEngine(XElement xml)
        {
            if (xml.GetChildElementAsString("ConnectionType") != "WebBrowser")
                return DefaultConnectionInfo.Instance.RenderingEngine;

            switch (xml.GetChildElementAsString("ConnectionSubType"))
            {
                case "IE":
                    return HTTPBase.RenderingEngine.IE;
                case "FireFox":
                    return HTTPBase.RenderingEngine.Gecko;
                default:
                    return DefaultConnectionInfo.Instance.RenderingEngine;
            }
        }
    }
}
