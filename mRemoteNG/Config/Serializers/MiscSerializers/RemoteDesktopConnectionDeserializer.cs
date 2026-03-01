using System;
using System.Globalization;
using System.Runtime.Versioning;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    [SupportedOSPlatform("windows")]
    public class RemoteDesktopConnectionDeserializer : IDeserializer<string, ConnectionTreeModel>
    {
        // .rdp file schema: https://technet.microsoft.com/en-us/library/ff393699(v=ws.10).aspx

        public ConnectionTreeModel Deserialize(string rdcFileContent)
        {
            ConnectionTreeModel connectionTreeModel = new();
            RootNodeInfo root = new(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(root);
            ConnectionInfo connectionInfo = new();
            foreach (string line in rdcFileContent.Split(Environment.NewLine.ToCharArray()))
            {
                string[] parts = line.Split(new[] { ':' }, 3);
                if (parts.Length < 3)
                {
                    continue;
                }

                string key = parts[0].Trim();
                string value = parts[2].Trim();

                SetConnectionInfoParameter(connectionInfo, key, value);
            }

            root.AddChild(connectionInfo);

            return connectionTreeModel;
        }


        private static void SetConnectionInfoParameter(ConnectionInfo connectionInfo, string key, string value)
        {
            switch (key.ToLowerInvariant())
            {
                case "full address":
                    Uri uri = new("dummyscheme" + Uri.SchemeDelimiter + value);
                    if (!string.IsNullOrEmpty(uri.Host))
                        connectionInfo.Hostname = uri.Host;
                    if (uri.Port != -1)
                        connectionInfo.Port = uri.Port;
                    break;
                case "server port":
                    connectionInfo.Port = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    break;
                case "username":
                    connectionInfo.Username = value;
                    break;
                case "domain":
                    connectionInfo.Domain = value;
                    break;
                case "session bpp":
                    switch (value)
                    {
                        case "8":
                            connectionInfo.Colors = RDPColors.Colors256;
                            break;
                        case "15":
                            connectionInfo.Colors = RDPColors.Colors15Bit;
                            break;
                        case "16":
                            connectionInfo.Colors = RDPColors.Colors16Bit;
                            break;
                        case "24":
                            connectionInfo.Colors = RDPColors.Colors24Bit;
                            break;
                        case "32":
                            connectionInfo.Colors = RDPColors.Colors32Bit;
                            break;
                    }
                    break;
                case "bitmapcachepersistenable":
                    connectionInfo.CacheBitmaps = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "screen mode id":
                    connectionInfo.Resolution = string.Equals(value, "2", StringComparison.Ordinal)
                        ? RDPResolutions.Fullscreen
                        : RDPResolutions.FitToWindow;
                    break;
                case "connect to console":
                    connectionInfo.UseConsoleSession = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "disable wallpaper":
                    connectionInfo.DisplayWallpaper = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "disable themes":
                    connectionInfo.DisplayThemes = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "allow font smoothing":
                    connectionInfo.EnableFontSmoothing = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "allow desktop composition":
                    connectionInfo.EnableDesktopComposition = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "keyboardhook":
                    connectionInfo.RedirectKeys = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "redirectsmartcards":
                    connectionInfo.RedirectSmartCards = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "redirectdrives":
                    connectionInfo.RedirectDiskDrives = (string.Equals(value, "1", StringComparison.Ordinal) ? RDPDiskDrives.Local : RDPDiskDrives.None);
                    break;
                case "redirectdrivescustom":
                    connectionInfo.RedirectDiskDrivesCustom = value;
                    break;
                case "redirectcomports":
                    connectionInfo.RedirectPorts = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "redirectprinters":
                    connectionInfo.RedirectPrinters = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "redirectclipboard":
                    connectionInfo.RedirectClipboard = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "audiomode":
                    switch (value)
                    {
                        case "0":
                            connectionInfo.RedirectSound = RDPSounds.BringToThisComputer;
                            break;
                        case "1":
                            connectionInfo.RedirectSound = RDPSounds.LeaveAtRemoteComputer;
                            break;
                        case "2":
                            connectionInfo.RedirectSound = RDPSounds.DoNotPlay;
                            break;
                    }
                    break;
                case "redirectaudiocapture":
                    connectionInfo.RedirectAudioCapture = string.Equals(value, "1", StringComparison.Ordinal);
                    break;
                case "loadbalanceinfo":
                    connectionInfo.LoadBalanceInfo = value;
                    break;
                case "gatewayusagemethod":
                    switch (value)
                    {
                        case "0":
                            connectionInfo.RDGatewayUsageMethod = RDGatewayUsageMethod.Never;
                            break;
                        case "1":
                            connectionInfo.RDGatewayUsageMethod = RDGatewayUsageMethod.Always;
                            break;
                        case "2":
                            connectionInfo.RDGatewayUsageMethod = RDGatewayUsageMethod.Detect;
                            break;
                    }
                    break;
                case "gatewayhostname":
                    connectionInfo.RDGatewayHostname = value;
                    break;
                case "gatewaycredentialssource":
                    switch(value)
                    {
                        case "0":
                            connectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.ExternalCredentialProvider;
                            break;
                        case "1":
                            connectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.SmartCard;
                            break;
                        case "2":
                            connectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.Yes;
                            break;
                        case "3":
                            // Both 3 and 4 require that the user enter gateway credentials manually
                            connectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.No;
                            break;
                        case "4":
                            // Both 3 and 4 require that the user enter gateway credentials manually
                            connectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.No;
                            break;
                        case "5":
                            connectionInfo.RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.AccessToken;
                            break;
                    }
                    break;
                case "gatewayaccesstoken":
                    connectionInfo.RDGatewayAccessToken = value;
                    break;
                case "alternate shell":
                    connectionInfo.RDPStartProgram = value;
                    break;
            }
        }
    }
}