using System.ComponentModel;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPSizingMode
    {
        [Description("None")]
        None,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.SmartSize))]
        SmartSize,

        [LocalizedAttributes.LocalizedDescription("Smart Size (Aspect Ratio)")]
        SmartSizeAspect
    }
}
