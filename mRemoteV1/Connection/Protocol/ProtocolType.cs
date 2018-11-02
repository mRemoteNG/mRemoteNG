using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol
{
	public enum ProtocolType
	{
        [LocalizedAttributes.LocalizedDescription("strRDP")]
        RDP = 0,
        [LocalizedAttributes.LocalizedDescription("strRDPonVMBus")]
        RDPonVMBus = 1,
        [LocalizedAttributes.LocalizedDescription("strVnc")]
        VNC = 2,
        [LocalizedAttributes.LocalizedDescription("strSsh1")]
        SSH1 = 3,
        [LocalizedAttributes.LocalizedDescription("strSsh2")]
        SSH2 = 4,
        [LocalizedAttributes.LocalizedDescription("strTelnet")]
        Telnet = 5,
        [LocalizedAttributes.LocalizedDescription("strRlogin")]
        Rlogin = 6,
        [LocalizedAttributes.LocalizedDescription("strRAW")]
        RAW = 7,
        [LocalizedAttributes.LocalizedDescription("strHttp")]
        HTTP = 8,
        [LocalizedAttributes.LocalizedDescription("strHttps")]
        HTTPS = 9,
        [LocalizedAttributes.LocalizedDescription("strICA")]
        ICA = 10,
        [LocalizedAttributes.LocalizedDescription("strExtApp")]
        IntApp = 20
	}
}