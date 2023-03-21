using System.Runtime.Versioning;
using System.Windows.Forms;
using AxMSTSCLib;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocol10 : RdpProtocol9
    {
        protected override RdpVersion RdpProtocolVersion => RDP.RdpVersion.Rdc10;
        
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