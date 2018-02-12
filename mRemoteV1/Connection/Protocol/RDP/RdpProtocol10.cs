using AxMSTSCLib;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpProtocol10 : RdpProtocol9
    {
        public RdpProtocol10(ConnectionInfo connectionInfo)
            : base(connectionInfo)
        {
            Control = new AxMsRdpClient10NotSafeForScripting();
            RdpVersionEnum = RdpVersionEnum.Rdc10;
        }

        protected override MsRdpClient6NotSafeForScripting CreateRdpClientControl()
        {
            return (MsRdpClient6NotSafeForScripting)((AxMsRdpClient10NotSafeForScripting)Control).GetOcx();
        }
    }
}
