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

        protected override RdpVersion RdpProtocolVersion => RdpVersion.Rdc9;

        protected override AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient9NotSafeForScripting();
        }

        protected override void UpdateSessionDisplaySettings(uint width, uint height)
        {
            RdpClient9.UpdateSessionDisplaySettings(width, height, width, height, 0, 1, 1);
        }

    }
}