using AxMSTSCLib;
using mRemoteNG.App;
using MSTSCLib;
using System;
using System.Windows.Forms;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocol7 : RdpProtocol
    {
        private MsRdpClient7NotSafeForScripting RdpClient7 => (MsRdpClient7NotSafeForScripting)((AxHost)Control).GetOcx();
        protected override RdpVersion RdpProtocolVersion => RDP.RdpVersion.Rdc7;

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            try
            {
                if (RdpVersion < Versions.RDC70) return false; // loaded MSTSCLIB dll version is not capable

                RdpClient7.AdvancedSettings8.AudioQualityMode = (uint)connectionInfo.SoundQuality;
                RdpClient7.AdvancedSettings8.AudioCaptureRedirectionMode = connectionInfo.RedirectAudioCapture;
                RdpClient7.AdvancedSettings8.NetworkConnectionType = (int)RdpNetworkConnectionType.Modem;

                if (connectionInfo.UseVmId)
                {
                    SetExtendedProperty("DisableCredentialsDelegation", true);
                    RdpClient7.AdvancedSettings7.AuthenticationServiceClass = "Microsoft Virtual Console Service";
                    RdpClient7.AdvancedSettings8.EnableCredSspSupport = true;
                    RdpClient7.AdvancedSettings8.NegotiateSecurityLayer = false;
                    RdpClient7.AdvancedSettings7.PCB = $"{connectionInfo.VmId}";
                    if (connectionInfo.UseEnhancedMode)
                        RdpClient7.AdvancedSettings7.PCB += ";EnhancedMode=1";
                }

                if (connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.AccessToken)
                {
                    var authToken = connectionInfo.RDGatewayAccessToken;
                    var encryptedAuthToken = RdGatewayAccessTokenHelper.EncryptAuthCookieString(authToken);
                    RdpClient7.TransportSettings3.GatewayEncryptedAuthCookie = encryptedAuthToken;  
                    RdpClient7.TransportSettings3.GatewayEncryptedAuthCookieSize = (uint)encryptedAuthToken.Length;
                    RdpClient7.TransportSettings3.GatewayCredsSource = 5;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.RdpSetPropsFailed, ex);
                return false;
            }

            return true;
        }

        protected override AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient11NotSafeForScripting();
        }
        
    }
}
