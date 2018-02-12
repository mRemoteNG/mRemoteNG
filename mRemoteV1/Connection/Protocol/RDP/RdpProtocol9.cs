using AxMSTSCLib;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpProtocol9 : RdpProtocol8
    {
        public RdpProtocol9(ConnectionInfo connectionInfo)
            : base(connectionInfo)
        {
            Control = new AxMsRdpClient9NotSafeForScripting();
            RdpVersionEnum = RdpVersionEnum.Rdc9;
        }

        protected override MsRdpClient6NotSafeForScripting CreateRdpClientControl()
        {
            return (MsRdpClient6NotSafeForScripting)((AxMsRdpClient9NotSafeForScripting)Control).GetOcx();
        }
    }
}
