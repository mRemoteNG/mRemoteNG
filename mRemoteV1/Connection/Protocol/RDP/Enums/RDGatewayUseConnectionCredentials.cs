using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
	public enum RDGatewayUseConnectionCredentials
	{
        [LocalizedAttributes.LocalizedDescription("strUseDifferentUsernameAndPassword")]
        No = 0,
        [LocalizedAttributes.LocalizedDescription("strUseSameUsernameAndPassword")]
        Yes = 1,
        [LocalizedAttributes.LocalizedDescription("strUseSmartCard")]
        SmartCard = 2
	}
}
