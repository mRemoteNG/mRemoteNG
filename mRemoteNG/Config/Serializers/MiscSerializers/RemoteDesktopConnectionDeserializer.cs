using System;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using System.Collections.Generic;
using mRemoteNG.Credential;

namespace mRemoteNG.Config.Serializers.MiscSerializers
{
    public class RemoteDesktopConnectionDeserializer
    {
        // .rdp file schema: https://technet.microsoft.com/en-us/library/ff393699(v=ws.10).aspx

        public SerializationResult Deserialize(string rdcFileContent)
        {
            var connectionInfo = new ConnectionInfo();
            var username = "";
            var domain = "";

            foreach (var line in rdcFileContent.Split(Environment.NewLine.ToCharArray()))
            {
                var parts = line.Split(new[] { ':' }, 3);
                if (parts.Length < 3)
                {
                    continue;
                }

                var propertyName = parts[0].Trim().ToLowerInvariant();
                var value = parts[2].Trim();

                SetConnectionInfoParameter(connectionInfo, propertyName, value);

                
                if (propertyName.Equals("username"))
                    username = value;
                if (propertyName.Equals("domain"))
                    domain = value;
            }

            var serializationResult = new SerializationResult(new List<ConnectionInfo>(), new ConnectionToCredentialMap());
            serializationResult.ConnectionRecords.Add(connectionInfo);

            if (username.Length > 0 || domain.Length > 0)
            {
                var cred = new CredentialRecord
                {
                    Title = domain.Length > 0 ? $"{domain}\\" : "" + username,
                    Domain = domain,
                    Username = username
                };

                serializationResult.ConnectionToCredentialMap.Add(Guid.Parse(connectionInfo.ConstantID), cred);
                connectionInfo.CredentialRecordId = cred.Id;
            }


            return serializationResult;
        }


        private void SetConnectionInfoParameter(ConnectionInfo connectionInfo, string key, string value)
        {
            switch (key)
            {
                case "full address":
                    var uri = new Uri("dummyscheme" + Uri.SchemeDelimiter + value);
                    if (!string.IsNullOrEmpty(uri.Host))
                        connectionInfo.Hostname = uri.Host;
                    if (uri.Port != -1)
                        connectionInfo.Port = uri.Port;
                    break;
                case "server port":
                    connectionInfo.Port = Convert.ToInt32(value);
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
                    connectionInfo.CacheBitmaps = value == "1";
                    break;
                case "screen mode id":
                    connectionInfo.Resolution = value == "2"
                        ? RDPResolutions.Fullscreen
                        : RDPResolutions.FitToWindow;
                    break;
                case "connect to console":
                    connectionInfo.UseConsoleSession = value == "1";
                    break;
                case "disable wallpaper":
                    connectionInfo.DisplayWallpaper = value == "1";
                    break;
                case "disable themes":
                    connectionInfo.DisplayThemes = value == "1";
                    break;
                case "allow font smoothing":
                    connectionInfo.EnableFontSmoothing = value == "1";
                    break;
                case "allow desktop composition":
                    connectionInfo.EnableDesktopComposition = value == "1";
                    break;
                case "redirectsmartcards":
                    connectionInfo.RedirectSmartCards = value == "1";
                    break;
                case "redirectdrives":
                    connectionInfo.RedirectDiskDrives = value == "1";
                    break;
                case "redirectcomports":
                    connectionInfo.RedirectPorts = value == "1";
                    break;
                case "redirectprinters":
                    connectionInfo.RedirectPrinters = value == "1";
                    break;
                case "redirectclipboard":
                    connectionInfo.RedirectClipboard = value == "1";
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
                    connectionInfo.RedirectAudioCapture = value == "1";
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
                case "alternate shell":
                    connectionInfo.StartProgram = value;
                    break;
            }
        }
    }
}