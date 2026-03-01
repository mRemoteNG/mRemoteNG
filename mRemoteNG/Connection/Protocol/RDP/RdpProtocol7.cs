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
        private MsRdpClient7NotSafeForScripting? RdpClient7 => (Control as AxHost)?.GetOcx() as MsRdpClient7NotSafeForScripting;
        protected override RdpVersion RdpProtocolVersion => RDP.RdpVersion.Rdc7;

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            return PostInitialize();
        }

        public override async System.Threading.Tasks.Task<bool> InitializeAsync()
        {
            if (!await base.InitializeAsync())
                return false;

            return PostInitialize();
        }

        private bool PostInitialize()
        {
            try
            {
                if (RdpVersion < Versions.RDC70) return false; // loaded MSTSCLIB dll version is not capable

                var rdpClient = RdpClient7;
                if (rdpClient == null) return false;

                rdpClient.AdvancedSettings8.AudioQualityMode = (uint)connectionInfo.SoundQuality;
                rdpClient.AdvancedSettings8.AudioCaptureRedirectionMode = connectionInfo.RedirectAudioCapture;
                rdpClient.AdvancedSettings8.NetworkConnectionType = (int)RdpNetworkConnectionType.Modem;

                if (connectionInfo.UseVmId)
                {
                    SetExtendedProperty("DisableCredentialsDelegation", true);
                    rdpClient.AdvancedSettings7.AuthenticationServiceClass = "Microsoft Virtual Console Service";
                    rdpClient.AdvancedSettings8.EnableCredSspSupport = true;
                    rdpClient.AdvancedSettings7.PCB = $"{connectionInfo.VmId}";
                    if (connectionInfo.UseEnhancedMode)
                        rdpClient.AdvancedSettings7.PCB += ";EnhancedMode=1";
                }

                if (connectionInfo.RDGatewayUseConnectionCredentials == RDGatewayUseConnectionCredentials.AccessToken)
                {
                    string authToken = connectionInfo.RDGatewayAccessToken;
                    string encryptedAuthToken = RdGatewayAccessTokenHelper.EncryptAuthCookieString(authToken);
                    rdpClient.TransportSettings3.GatewayEncryptedAuthCookie = encryptedAuthToken;
                    rdpClient.TransportSettings3.GatewayEncryptedAuthCookieSize = (uint)encryptedAuthToken.Length;
                    rdpClient.TransportSettings3.GatewayCredsSource = 5;
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
