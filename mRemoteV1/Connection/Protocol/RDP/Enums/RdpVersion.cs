using System;

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

	public enum RdpVersionEnum
	{
		Rdc6,
		Rdc8
	}
}
