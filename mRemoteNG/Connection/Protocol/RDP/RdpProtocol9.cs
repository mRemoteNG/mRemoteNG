using System;
using System.Runtime.Versioning;
using System.Windows.Forms;
using AxMSTSCLib;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocol9 : RdpProtocol8
    {
        private MsRdpClient9NotSafeForScripting RdpClient9 => (MsRdpClient9NotSafeForScripting)((AxHost)Control).GetOcx();

        protected override RdpVersion RdpProtocolVersion => RDP.RdpVersion.Rdc9;

        public RdpProtocol9()
        {
            _frmMain.ResizeEnd += ResizeEnd;
        }
        
        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            if (RdpVersion < Versions.RDC81) return false; // minimum dll version checked, loaded MSTSCLIB dll version is not capable

            return true;
        }

        protected override AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient9NotSafeForScripting();
        }

        protected override void UpdateSessionDisplaySettings(uint width, uint height)
        {
            try
            {
                RdpClient9.UpdateSessionDisplaySettings(width, height, width, height, Orientation, DesktopScaleFactor, DeviceScaleFactor);
            }
            catch (Exception)
            {
                // target OS does not support newer method, fallback to an older method
                base.UpdateSessionDisplaySettings(width, height);
            }
        }

    }
}