using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Rdp
{
    [SupportedOSPlatform("windows")]
    public class RdpConnectionSerializer : ISerializer<ConnectionInfo, string>
    {
        private readonly SaveFilter _saveFilter;

        public Version Version { get; } = new Version(1, 0);

        public RdpConnectionSerializer(SaveFilter saveFilter)
        {
            _saveFilter = saveFilter ?? throw new ArgumentNullException(nameof(saveFilter));
        }

        public string Serialize(ConnectionTreeModel connectionTreeModel)
        {
            ContainerInfo rootNode = connectionTreeModel.RootNodes.First(node => node is RootNodeInfo);
            return Serialize(rootNode);
        }

        public string Serialize(ConnectionInfo serializationTarget)
        {
            // For containers, serialize only the first RDP connection found
            if (serializationTarget is ContainerInfo container)
            {
                ConnectionInfo? firstRdp = FindFirstRdpConnection(container);
                if (firstRdp != null)
                    return SerializeSingleConnection(firstRdp);

                // If no RDP connections, serialize the first connection
                ConnectionInfo? firstConnection = FindFirstConnection(container);
                if (firstConnection != null)
                    return SerializeSingleConnection(firstConnection);

                return string.Empty;
            }

            return SerializeSingleConnection(serializationTarget);
        }

        private static ConnectionInfo? FindFirstRdpConnection(ContainerInfo container)
        {
            foreach (ConnectionInfo child in container.Children)
            {
                if (child is ContainerInfo subContainer)
                {
                    ConnectionInfo? found = FindFirstRdpConnection(subContainer);
                    if (found != null)
                        return found;
                }
                else if (child.Protocol == ProtocolType.RDP)
                {
                    return child;
                }
            }
            return null;
        }

        private static ConnectionInfo? FindFirstConnection(ContainerInfo container)
        {
            foreach (ConnectionInfo child in container.Children)
            {
                if (child is ContainerInfo subContainer)
                {
                    ConnectionInfo? found = FindFirstConnection(subContainer);
                    if (found != null)
                        return found;
                }
                else if (child is not RootNodeInfo)
                {
                    return child;
                }
            }
            return null;
        }

        private string SerializeSingleConnection(ConnectionInfo con)
        {
            StringBuilder sb = new();

            // Connection settings
            sb.AppendLine(CultureInfo.InvariantCulture, $"full address:s:{con.Hostname}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"server port:i:{con.Port}");

            // Credentials (only if save filter allows)
            if (_saveFilter.SaveUsername && !string.IsNullOrEmpty(con.Username))
                sb.AppendLine(CultureInfo.InvariantCulture, $"username:s:{con.Username}");

            if (_saveFilter.SaveDomain && !string.IsNullOrEmpty(con.Domain))
                sb.AppendLine(CultureInfo.InvariantCulture, $"domain:s:{con.Domain}");

            // Resolution
            WriteResolution(sb, con.Resolution);

            // Color depth
            sb.AppendLine(CultureInfo.InvariantCulture, $"session bpp:i:{(int)con.Colors}");

            // Display settings
            sb.AppendLine(CultureInfo.InvariantCulture, $"disable wallpaper:i:{BoolToRdp(!con.DisplayWallpaper)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"disable themes:i:{BoolToRdp(!con.DisplayThemes)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"disable full window drag:i:{BoolToRdp(con.DisableFullWindowDrag)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"disable menu anims:i:{BoolToRdp(con.DisableMenuAnimations)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"disable cursor setting:i:{BoolToRdp(con.DisableCursorShadow)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"allow font smoothing:i:{BoolToRdp(con.EnableFontSmoothing)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"allow desktop composition:i:{BoolToRdp(con.EnableDesktopComposition)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"bitmapcachepersistenable:i:{BoolToRdp(con.CacheBitmaps)}");

            // Redirect settings
            sb.AppendLine(CultureInfo.InvariantCulture, $"redirectclipboard:i:{BoolToRdp(con.RedirectClipboard)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"redirectprinters:i:{BoolToRdp(con.RedirectPrinters)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"redirectcomports:i:{BoolToRdp(con.RedirectPorts)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"redirectsmartcards:i:{BoolToRdp(con.RedirectSmartCards)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"redirectdrives:i:{WriteDriveRedirect(con.RedirectDiskDrives)}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"audiocapturemode:i:{BoolToRdp(con.RedirectAudioCapture)}");

            // Sound
            sb.AppendLine(CultureInfo.InvariantCulture, $"audiomode:i:{(int)con.RedirectSound}");

            // Keyboard redirect
            sb.AppendLine(CultureInfo.InvariantCulture, $"keyboardhook:i:{(con.RedirectKeys ? 1 : 0)}");

            // Console session
            sb.AppendLine(CultureInfo.InvariantCulture, $"connect to console:i:{BoolToRdp(con.UseConsoleSession)}");

            // Authentication
            sb.AppendLine(CultureInfo.InvariantCulture, $"authentication level:i:{(int)con.RDPAuthenticationLevel}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"enablecredsspsupport:i:{BoolToRdp(con.UseCredSsp)}");

            // Load balance info
            if (!string.IsNullOrEmpty(con.LoadBalanceInfo))
                sb.AppendLine(CultureInfo.InvariantCulture, $"loadbalanceinfo:s:{con.LoadBalanceInfo}");

            // RD Gateway
            WriteGatewaySettings(sb, con);

            // Start program
            if (!string.IsNullOrEmpty(con.RDPStartProgram))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"alternate shell:s:{con.RDPStartProgram}");
                if (!string.IsNullOrEmpty(con.RDPStartProgramWorkDir))
                    sb.AppendLine(CultureInfo.InvariantCulture, $"shell working directory:s:{con.RDPStartProgramWorkDir}");
            }

            return sb.ToString();
        }

        private static void WriteResolution(StringBuilder sb, RDPResolutions resolution)
        {
            switch (resolution)
            {
                case RDPResolutions.Fullscreen:
                    sb.AppendLine("screen mode id:i:2");
                    break;
                case RDPResolutions.FitToWindow:
                case RDPResolutions.SmartSize:
                case RDPResolutions.SmartSizeAspect:
                    sb.AppendLine("screen mode id:i:1");
                    sb.AppendLine("smart sizing:i:1");
                    break;
                default:
                    sb.AppendLine("screen mode id:i:1");
                    (int w, int h) = GetResolutionDimensions(resolution);
                    if (w > 0 && h > 0)
                    {
                        sb.AppendLine(CultureInfo.InvariantCulture, $"desktopwidth:i:{w}");
                        sb.AppendLine(CultureInfo.InvariantCulture, $"desktopheight:i:{h}");
                    }
                    break;
            }
        }

        private static (int width, int height) GetResolutionDimensions(RDPResolutions resolution)
        {
            return resolution switch
            {
                RDPResolutions.Res800x600 => (800, 600),
                RDPResolutions.Res1024x768 => (1024, 768),
                RDPResolutions.Res1152x864 => (1152, 864),
                RDPResolutions.Res1280x800 => (1280, 800),
                RDPResolutions.Res1280x1024 => (1280, 1024),
                RDPResolutions.Res1366x768 => (1366, 768),
                RDPResolutions.Res1440x900 => (1440, 900),
                RDPResolutions.Res1600x900 => (1600, 900),
                RDPResolutions.Res1600x1200 => (1600, 1200),
                RDPResolutions.Res1680x1050 => (1680, 1050),
                RDPResolutions.Res1920x1080 => (1920, 1080),
                RDPResolutions.Res1920x1200 => (1920, 1200),
                RDPResolutions.Res2048x1536 => (2048, 1536),
                RDPResolutions.Res2560x1440 => (2560, 1440),
                RDPResolutions.Res2560x1600 => (2560, 1600),
                RDPResolutions.Res2560x2048 => (2560, 2048),
                RDPResolutions.Res3840x2160 => (3840, 2160),
                _ => (0, 0)
            };
        }

        private static void WriteGatewaySettings(StringBuilder sb, ConnectionInfo con)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"gatewayusagemethod:i:{(int)con.RDGatewayUsageMethod}");

            if (con.RDGatewayUsageMethod != RDGatewayUsageMethod.Never)
            {
                if (!string.IsNullOrEmpty(con.RDGatewayHostname))
                    sb.AppendLine(CultureInfo.InvariantCulture, $"gatewayhostname:s:{con.RDGatewayHostname}");

                sb.AppendLine(CultureInfo.InvariantCulture, $"gatewaycredentialssource:i:{MapGatewayCredentialSource(con.RDGatewayUseConnectionCredentials)}");

                // Profile method: 1 = NTLM
                sb.AppendLine("gatewayprofileusagemethod:i:1");
            }
        }

        private static int MapGatewayCredentialSource(RDGatewayUseConnectionCredentials credSource)
        {
            return credSource switch
            {
                RDGatewayUseConnectionCredentials.Yes => 0,        // Use same credentials
                RDGatewayUseConnectionCredentials.No => 1,         // Ask for credentials
                RDGatewayUseConnectionCredentials.SmartCard => 2,  // Smart card
                _ => 4                                              // Ask user
            };
        }

        private static int WriteDriveRedirect(RDPDiskDrives drives)
        {
            return drives switch
            {
                RDPDiskDrives.None => 0,
                _ => 1
            };
        }

        private static int BoolToRdp(bool value) => value ? 1 : 0;
    }
}
