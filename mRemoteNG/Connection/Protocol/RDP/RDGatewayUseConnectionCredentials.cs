using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDGatewayUseConnectionCredentials
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.UseDifferentUsernameAndPassword))]
        No = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.UseSameUsernameAndPassword))]
        Yes = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.UseSmartCard))]
        SmartCard = 2,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.UseExternalCredentialProvider))]
        ExternalCredentialProvider = 3,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.UseAccessToken))]
        AccessToken = 4
    }
}