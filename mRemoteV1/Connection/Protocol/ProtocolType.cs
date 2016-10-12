using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol
{
    public enum ProtocolType
    {
        [LocalizedAttributes.LocalizedDescriptionAttribute("strRDP")] RDP = 0,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strVnc")] VNC = 1,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strSsh1")] SSH1 = 2,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strSsh2")] SSH2 = 3,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strTelnet")] Telnet = 4,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strRlogin")] Rlogin = 5,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strRAW")] RAW = 6,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strHttp")] HTTP = 7,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strHttps")] HTTPS = 8,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strICA")] ICA = 9,
        [LocalizedAttributes.LocalizedDescriptionAttribute("strExtApp")] IntApp = 20
    }
}