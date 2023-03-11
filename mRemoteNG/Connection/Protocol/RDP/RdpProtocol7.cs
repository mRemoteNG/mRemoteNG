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
    public class RdpProtocol7 : RdpProtocol6
    {
        protected override RdpVersion RdpProtocolVersion => RdpVersion.Rdc7;

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            try
            {
                var rdpClient7 = (MsRdpClient7NotSafeForScripting)((AxHost) Control).GetOcx();
                rdpClient7.AdvancedSettings8.AudioQualityMode = (uint)connectionInfo.SoundQuality;
                rdpClient7.AdvancedSettings8.AudioCaptureRedirectionMode = connectionInfo.RedirectAudioCapture;
                rdpClient7.AdvancedSettings8.NetworkConnectionType = (int)RdpNetworkConnectionType.Modem;

                if (connectionInfo.UseVmId)
                {
                    SetExtendedProperty("DisableCredentialsDelegation", true);
                    rdpClient7.AdvancedSettings7.AuthenticationServiceClass = "Microsoft Virtual Console Service";
                    rdpClient7.AdvancedSettings8.EnableCredSspSupport = true;
                    rdpClient7.AdvancedSettings8.NegotiateSecurityLayer = false;
                    rdpClient7.AdvancedSettings7.PCB = $"{connectionInfo.VmId}";
                    if (connectionInfo.UseEnhancedMode)
                        rdpClient7.AdvancedSettings7.PCB += ";EnhancedMode=1";
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
            return new AxMsRdpClient7NotSafeForScripting();
        }
    }
}
