using System.Runtime.Versioning;
using System.Windows.Forms;
using AxMSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocol11 : RdpProtocol10
    {
        protected override RdpVersion RdpProtocolVersion => RDP.RdpVersion.Rdc11;

        protected override AxHost CreateActiveXRdpClientControl()
        {
            return new AxMsRdpClient11NotSafeForScripting();
        }
        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            if (RdpVersion < Versions.RDC100) return false; // minimum dll version checked, loaded MSTSCLIB dll version is not capable

            return true;
        }

    }
}