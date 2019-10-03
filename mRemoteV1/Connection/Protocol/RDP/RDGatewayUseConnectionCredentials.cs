using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDGatewayUseConnectionCredentials
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.strUseDifferentUsernameAndPassword))]
        No = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strUseSameUsernameAndPassword))]
        Yes = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strUseSmartCard))]
        SmartCard = 2
    }
}