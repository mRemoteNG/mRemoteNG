using System.Runtime.Versioning;
using System.Windows.Forms;
using AxMSTSCLib;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocol11 : RdpProtocol10
    {
        private MsRdpClient11NotSafeForScripting RdpClient11 => (MsRdpClient11NotSafeForScripting)((AxHost)Control).GetOcx();

        protected override RdpVersion RdpProtocolVersion => RdpVersion.Rdc11;

        protected override AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient11NotSafeForScripting();
        }

        protected override void UpdateSessionDisplaySettings(uint width, uint height)
        {
            RdpClient11.UpdateSessionDisplaySettings(width, height, width, height, 0, 1, 1);
        }

    }
}