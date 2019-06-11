using AxMSTSCLib;
using mRemoteNG.App;
using MSTSCLib;
using System;
using System.Windows.Forms;

namespace mRemoteNG.Connection.Protocol.RDP
{
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
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetPropsFailed, ex);
                return false;
            }

            return true;
        }

        protected override AxHost CreateRdpClientControl()
        {
            return new AxMsRdpClient7NotSafeForScripting();
        }
    }
}
