using System;
using System.Collections.Generic;
using System.Text;
using mRemoteNG.Tools;
using mRemoteNG.Connection.Protocol;

namespace mRemoteNG.Connection
{
    
    public enum Protocols
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

    /*
    public class Protocols : GenericEnum<Protocols,ConnectionProtocol>
    {
        public static readonly ConnectionProtocol RDP = new RDPConnectionProtocol();
        public static readonly ConnectionProtocol VNC = new VNCConnectionProtocol();
        public static readonly ConnectionProtocol SSH1 = new SSH1ConnectionProtocol();
        public static readonly ConnectionProtocol SSH2 = new SSH2ConnectionProtocol();
        public static readonly ConnectionProtocol Telnet = new TelnetConnectionProtocol();
        public static readonly ConnectionProtocol Rlogin = new RloginConnectionProtocol();
        public static readonly ConnectionProtocol RAW = new RAWConnectionProtocol();
        public static readonly ConnectionProtocol HTTP = new HttpConnectionProtocol();
        public static readonly ConnectionProtocol HTTPS = new HttpsConnectionProtocol();
        public static readonly ConnectionProtocol ICA = new ICAConnectionProtocol();
        public static readonly ConnectionProtocol IntApp = new OtherConnectionProtocol();
    }
    */
}