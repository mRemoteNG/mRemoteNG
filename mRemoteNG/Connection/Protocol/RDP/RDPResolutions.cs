using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPResolutions
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.SmartSize))]
        SmartSize,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.FitToPanel))]
        FitToWindow,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Fullscreen))]
        Fullscreen
    }
}