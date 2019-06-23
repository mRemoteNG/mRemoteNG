using System;
using System.Windows.Forms;
using AxMSTSCLib;
using mRemoteNG.App;
using MSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpProtocol9 : RdpProtocol8
    {
        protected override RdpVersion RdpProtocolVersion => RdpVersion.Rdc9;

        protected override AxHost CreateRdpClientControl()
        {
            return new AxMsRdpClient9NotSafeForScripting();
        }
    }
}
