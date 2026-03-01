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

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Serial))]
        Serial = 9,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.PowerShell))]
        PowerShell = 10,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Ard))]
        ARD = 11,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Terminal))]
        Terminal = 12,
        
        [LocalizedAttributes.LocalizedDescription(nameof(Language.Wsl))]
        WSL = 13,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.AnyDesk))]
        AnyDesk = 14,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Vmrc))]
        VMRC = 15,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Msra))]
        MSRA = 16,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.ExternalTool))]
        IntApp = 20,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.OpenSsh))]
        OpenSSH = 22,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Winbox))]
        Winbox = 21
    }

    public static class ProtocolFeature
    {
        public static bool SupportBlankHostname(ProtocolType protocolType)
        {
            return (protocolType == ProtocolType.IntApp || protocolType == ProtocolType.PowerShell || protocolType == ProtocolType.WSL || protocolType == ProtocolType.Terminal);
        }
    }
}