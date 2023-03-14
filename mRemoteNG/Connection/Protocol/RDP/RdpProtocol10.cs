using System.Runtime.Versioning;
using System.Windows.Forms;
using AxMSTSCLib;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocol10 : RdpProtocol9
    {
        private MsRdpClient10NotSafeForScripting RdpClient10 => (MsRdpClient10NotSafeForScripting)((AxHost)Control).GetOcx();

        protected override RdpVersion RdpProtocolVersion => RdpVersion.Rdc10;

        protected override AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient10NotSafeForScripting();
        }

        protected override void UpdateSessionDisplaySettings(uint width, uint height)
        {
            RdpClient10.UpdateSessionDisplaySettings(width, height, width, height, 0, 1, 1);
        }

    }
}