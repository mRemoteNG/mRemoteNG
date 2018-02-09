using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
	public partial class RdpProtocol
	{
		public enum RDGatewayUsageMethod
		{
            [LocalizedAttributes.LocalizedDescription("strNever")]
            Never = 0, // TSC_PROXY_MODE_NONE_DIRECT
            [LocalizedAttributes.LocalizedDescription("strAlways")]
            Always = 1, // TSC_PROXY_MODE_DIRECT
            [LocalizedAttributes.LocalizedDescription("strDetect")]
            Detect = 2 // TSC_PROXY_MODE_DETECT
		}
	}
}
