using AxMSTSCLib;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpProtocol10 : RdpProtocol9
    {
        public RdpProtocol10()
        {
            Control = new AxMsRdpClient10NotSafeForScripting();
        }

        protected override MsRdpClient6NotSafeForScripting CreateRdpClientControl()
        {
            return (MsRdpClient6NotSafeForScripting)((AxMsRdpClient10NotSafeForScripting)Control).GetOcx();
        }
    }
}
