using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
	public enum RdpColors
	{
        [LocalizedAttributes.LocalizedDescription("strRDP256Colors")]
        Colors256 = 8,
        [LocalizedAttributes.LocalizedDescription("strRDP32768Colors")]
        Colors15Bit = 15,
        [LocalizedAttributes.LocalizedDescription("strRDP65536Colors")]
        Colors16Bit = 16,
        [LocalizedAttributes.LocalizedDescription("strRDP16777216Colors")]
        Colors24Bit = 24,
        [LocalizedAttributes.LocalizedDescription("strRDP4294967296Colors")]
        Colors32Bit = 32
	}
}
