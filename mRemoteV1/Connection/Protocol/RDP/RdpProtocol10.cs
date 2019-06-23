using System.Windows.Forms;
using AxMSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpProtocol10 : RdpProtocol9
    {
        protected override RdpVersion RdpProtocolVersion => RdpVersion.Rdc10;

        protected override AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient10NotSafeForScripting();
        }
    }
}
