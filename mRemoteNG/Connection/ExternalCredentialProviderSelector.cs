using mRemoteNG.Resources.Language;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection
{
    public enum ExternalCredentialProvider
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.ECPNone))]
        None = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.ECPDelineaSecretServer))]
        DelineaSecretServer = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.ECPClickstudiosPasswordstate))]
        ClickstudiosPasswordState = 2,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.ECPOnePassword))]
        OnePassword = 3,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.VaultOpenbao))]
        VaultOpenbao = 4,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.ECPPasswordSafe))]
        PasswordSafe = 5,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.ECPLAPS))]
        LAPS = 6,
    }
}
