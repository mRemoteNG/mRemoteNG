using AxMSTSCLib;
using mRemoteNG.App;
using MSTSCLib;
using System;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpProtocol7 : RdpProtocol6
    {
        private new MsRdpClient7NotSafeForScripting _rdpClient;

        public override bool Initialize()
        {
            base.Initialize();
            try
            {
                _rdpClient.AdvancedSettings8.AudioQualityMode = (uint)connectionInfo.SoundQuality;
                _rdpClient.AdvancedSettings8.AudioCaptureRedirectionMode = connectionInfo.RedirectAudioCapture;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace(Language.strRdpSetPropsFailed, ex);
                return false;
            }

            return true;
        }

        protected override object CreateRdpClientControl()
        {
            _rdpClient = (MsRdpClient7NotSafeForScripting)((AxMsRdpClient7NotSafeForScripting)Control).GetOcx();
            return _rdpClient;
        }
    }
}
