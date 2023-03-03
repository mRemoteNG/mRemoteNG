using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection
{
    public enum ExternalCredentialProvider
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.ECPNone))]
        None = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.ECPDelineaSecretServer))]
        DelineaSecretServer = 1
    }
}
