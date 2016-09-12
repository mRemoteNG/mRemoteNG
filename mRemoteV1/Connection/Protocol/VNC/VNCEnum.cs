using System.ComponentModel;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.VNC
{
    public enum Defaults
		{
			Port = 5900
		}
				
		public enum SpecialKeys
		{
			CtrlAltDel,
			CtrlEsc
		}
				
		public enum Compression
		{
            [LocalizedAttributes.LocalizedDescription("strNoCompression")]CompNone = 99,
			[Description("0")]Comp0 = 0,
			[Description("1")]Comp1 = 1,
			[Description("2")]Comp2 = 2,
			[Description("3")]Comp3 = 3,
			[Description("4")]Comp4 = 4,
			[Description("5")]Comp5 = 5,
			[Description("6")]Comp6 = 6,
			[Description("7")]Comp7 = 7,
			[Description("8")]Comp8 = 8,
			[Description("9")]Comp9 = 9
		}
				
		public enum Encoding
		{
			[Description("Raw")]EncRaw,
			[Description("RRE")]EncRRE,
			[Description("CoRRE")]EncCorre,
			[Description("Hextile")]EncHextile,
			[Description("Zlib")]EncZlib,
			[Description("Tight")]EncTight,
			[Description("ZlibHex")]EncZLibHex,
			[Description("ZRLE")]EncZRLE
		}
				
		public enum AuthMode
		{
            [LocalizedAttributes.LocalizedDescription("VNC")]
            AuthVNC,
            [LocalizedAttributes.LocalizedDescription("Windows")]
            AuthWin
		}
				
		public enum ProxyType
		{
            [LocalizedAttributes.LocalizedDescription("strNone")]
            ProxyNone,
            [LocalizedAttributes.LocalizedDescription("strHttp")]
            ProxyHTTP,
            [LocalizedAttributes.LocalizedDescription("strSocks5")]
            ProxySocks5,
            [LocalizedAttributes.LocalizedDescription("strUltraVncRepeater")]
            ProxyUltra
		}
				
		public enum Colors
		{
            [LocalizedAttributes.LocalizedDescription("strNormal")]
            ColNormal,
			[Description("8-bit")]Col8Bit
		}
				
		public enum SmartSizeMode
		{
            [LocalizedAttributes.LocalizedDescription("strNoSmartSize")]
            SmartSNo,
            [LocalizedAttributes.LocalizedDescription("strFree")]
            SmartSFree,
            [LocalizedAttributes.LocalizedDescription("strAspect")]
            SmartSAspect
		}
}
