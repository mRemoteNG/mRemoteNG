using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
	public partial class RdpProtocol
	{
		public enum RdpSoundQuality
	    {
            [LocalizedAttributes.LocalizedDescription("strRDPSoundQualityDynamic")]
            Dynamic = 0,
            [LocalizedAttributes.LocalizedDescription("strRDPSoundQualityMedium")]
            Medium = 1,
            [LocalizedAttributes.LocalizedDescription("strRDPSoundQualityHigh")]
            High = 2
        }
	}
}
