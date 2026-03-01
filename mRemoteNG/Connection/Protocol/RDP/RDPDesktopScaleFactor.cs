using System.ComponentModel;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPDesktopScaleFactor
    {
        [Description("Auto")]
        Auto,
        [Description("100 %")]
        Scale100,
        [Description("125 %")]
        Scale125,
        [Description("150 %")]
        Scale150,
        [Description("200 %")]
        Scale200
    }
}
