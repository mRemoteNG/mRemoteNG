using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
	public partial class RdpProtocol
	{
		public enum AuthenticationLevel
		{
            [LocalizedAttributes.LocalizedDescription("strAlwaysConnectEvenIfAuthFails")]
            NoAuth = 0,
            [LocalizedAttributes.LocalizedDescription("strDontConnectWhenAuthFails")]
            AuthRequired = 1,
            [LocalizedAttributes.LocalizedDescription("strWarnIfAuthFails")]
            WarnOnFailedAuth = 2
		}
	}
}
