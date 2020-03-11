using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol
{
    public enum ProtocolType
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDP))]
        RDP = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strVnc))]
        VNC = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strSsh1))]
        SSH1 = 2,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strSsh2))]
        SSH2 = 3,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strTelnet))]
        Telnet = 4,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRlogin))]
        Rlogin = 5,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRAW))]
        RAW = 6,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strHttp))]
        HTTP = 7,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strHttps))]
        HTTPS = 8,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strICA))]
        ICA = 9,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strPowerShell))]
        PowerShell = 10,
        
        [LocalizedAttributes.LocalizedDescription(nameof(Language.strSerial))]
        Serial = 11,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strExtApp))]
        IntApp = 20
    }
}
