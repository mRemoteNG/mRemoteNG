using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection.Protocol.RDP
{
    [SupportedOSPlatform("windows")]
    public class RdpProtocolFactory
    {
        public RdpProtocol6 Build(RdpVersion rdpVersion)
        {
            switch (rdpVersion)
            {
                case RdpVersion.Highest:
                    return BuildHighestSupportedVersion();
                case RdpVersion.Rdc6:
                    return new RdpProtocol6();
                case RdpVersion.Rdc7:
                    return new RdpProtocol7();
                case RdpVersion.Rdc8:
                    return new RdpProtocol8();
                case RdpVersion.Rdc9:
                    return new RdpProtocol9();
                default:
                    throw new ArgumentOutOfRangeException(nameof(rdpVersion), rdpVersion, null);
            }
        }

        private RdpProtocol6 BuildHighestSupportedVersion()
        {
            var versions = Enum.GetValues(typeof(RdpVersion))
                .OfType<RdpVersion>()
                .Except(new[] { RdpVersion.Highest })
                .Reverse();

            foreach (var version in versions)
            {
                var rdp = Build(version);
                if (rdp.RdpVersionSupported())
                    return rdp;
            }

            throw new ArgumentOutOfRangeException();
        }

        public List<RdpVersion> GetSupportedVersions()
        {
            var versions = Enum.GetValues(typeof(RdpVersion))
                .OfType<RdpVersion>()
                .Except(new[] {RdpVersion.Highest});

            var supportedVersions = new List<RdpVersion>();
            foreach (var version in versions)
            {
                if (Build(version).RdpVersionSupported())
                    supportedVersions.Add(version);
            }

            return supportedVersions;
        }
    }
}
