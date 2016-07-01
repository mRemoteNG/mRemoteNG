using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol
{
	public enum ProtocolType
	{
        [LocalizedAttributes.LocalizedDescription("strRDP")]
        RDP = 0,
        [LocalizedAttributes.LocalizedDescription("strVnc")]
        VNC = 1,
        [LocalizedAttributes.LocalizedDescription("strSsh1")]
        SSH1 = 2,
        [LocalizedAttributes.LocalizedDescription("strSsh2")]
        SSH2 = 3,
        [LocalizedAttributes.LocalizedDescription("strTelnet")]
        Telnet = 4,
        [LocalizedAttributes.LocalizedDescription("strRlogin")]
        Rlogin = 5,
        [LocalizedAttributes.LocalizedDescription("strRAW")]
        RAW = 6,
        [LocalizedAttributes.LocalizedDescription("strHttp")]
        HTTP = 7,
        [LocalizedAttributes.LocalizedDescription("strHttps")]
        HTTPS = 8,
        [LocalizedAttributes.LocalizedDescription("strICA")]
        ICA = 9,
        [LocalizedAttributes.LocalizedDescription("strExtApp")]
        IntApp = 20
	}
}