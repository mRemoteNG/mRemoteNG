using AxMSTSCLib;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpProtocol7 : RdpProtocol6
    {
        public RdpProtocol7()
        {
            Control = new AxMsRdpClient7NotSafeForScripting();
            RdpVersionEnum = RdpVersionEnum.Rdc7;
        }

        protected override MsRdpClient6NotSafeForScripting CreateRdpClientControl()
        {
            return (MsRdpClient6NotSafeForScripting)((AxMsRdpClient7NotSafeForScripting)Control).GetOcx();
        }
    }
}
