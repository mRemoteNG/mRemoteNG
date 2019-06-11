using System;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public class RdpProtocolFactory
    {
        public RdpProtocol6 Build(RdpVersion rdpVersion)
        {
            switch (rdpVersion)
            {
                case RdpVersion.Rdc6:
                    return new RdpProtocol6();
                case RdpVersion.Rdc7:
                case RdpVersion.Rdc8:
                case RdpVersion.Rdc9:
                case RdpVersion.Rdc10:
                    return new RdpProtocol8();
                default:
                    throw new ArgumentOutOfRangeException(nameof(rdpVersion), rdpVersion, null);
            }
        }
    }
}
