using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum AuthenticationLevel
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.strAlwaysConnectEvenIfAuthFails))]
        NoAuth = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strDontConnectWhenAuthFails))]
        AuthRequired = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strWarnIfAuthFails))]
        WarnOnFailedAuth = 2
    }
}