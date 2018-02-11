using System;
using System.Collections.Generic;

namespace mRemoteNG.Connection.Protocol.RDP
{
	public static class RdpVersion
	{
		public static readonly Version RDC60 = new Version(6, 0, 6000);
		public static readonly Version RDC61 = new Version(6, 0, 6001);
		public static readonly Version RDC70 = new Version(6, 1, 7600);
		public static readonly Version RDC80 = new Version(6, 2, 9200);
		public static readonly Version RDC81 = new Version(6, 3, 9600);
	}

    /// <summary>
    /// Represents a version of the RDP Client
    /// </summary>
	public enum RdpVersionEnum
	{
		Rdc6,
        Rdc7,
		Rdc8,
        Rdc9,
        Rdc10
	}

    public static class RdpVersionEnumExtensions
    {
        public static IEnumerable<RdpVersionEnum> GetAll(this RdpVersionEnum versionEnum)
        {
            return new[]
            {
                RdpVersionEnum.Rdc6,
                RdpVersionEnum.Rdc7,
                RdpVersionEnum.Rdc8,
                RdpVersionEnum.Rdc9,
                RdpVersionEnum.Rdc10
            };
        }
    }
}
