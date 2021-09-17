using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol
{
    public enum ProtocolType
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.Rdp))]
        RDP = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Vnc))]
        VNC = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.SshV1))]
        SSH1 = 2,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.SshV2))]
        SSH2 = 3,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Telnet))]
        Telnet = 4,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Rlogin))]
        Rlogin = 5,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Raw))]
        RAW = 6,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Http))]
        HTTP = 7,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Https))]
        HTTPS = 8,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.PowerShell))]
        PowerShell = 10,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.ExternalTool))]
        IntApp = 20
    }
}