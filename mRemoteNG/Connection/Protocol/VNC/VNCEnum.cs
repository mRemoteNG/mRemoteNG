using System.ComponentModel;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

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
        [LocalizedAttributes.LocalizedDescription(nameof(Language.NoCompression))]
        CompNone = 99,
        [Description("0")] Comp0 = 0,
        [Description("1")] Comp1 = 1,
        [Description("2")] Comp2 = 2,
        [Description("3")] Comp3 = 3,
        [Description("4")] Comp4 = 4,
        [Description("5")] Comp5 = 5,
        [Description("6")] Comp6 = 6,
        [Description("7")] Comp7 = 7,
        [Description("8")] Comp8 = 8,
        [Description("9")] Comp9 = 9
    }

    public enum Encoding
    {
        [Description("Raw")] EncRaw,
        [Description("RRE")] EncRRE,
        [Description("CoRRE")] EncCorre,
        [Description("Hextile")] EncHextile,
        [Description("Zlib")] EncZlib,
        [Description("Tight")] EncTight,
        [Description("ZlibHex")] EncZLibHex,
        [Description("ZRLE")] EncZRLE
    }

    public enum AuthMode
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.Vnc))]
        AuthVNC,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Windows))]
        AuthWin
    }

    public enum ProxyType
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.None))]
        ProxyNone,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Http))]
        ProxyHTTP,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Socks5))]
        ProxySocks5,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.UltraVncRepeater))]
        ProxyUltra
    }

    public enum Colors
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.Normal))]
        ColNormal,
        [Description("8-bit")] Col8Bit
    }

    public enum SmartSizeMode
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.NoSmartSize))]
        SmartSNo,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Free))]
        SmartSFree,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Aspect))]
        SmartSAspect
    }
}