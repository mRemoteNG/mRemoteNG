using System.ComponentModel;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPResolutions
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.FitToPanel))]
        FitToWindow,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.Fullscreen))]
        Fullscreen,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.SmartSize))]
        SmartSize,
        [Description("800x600")] Res800x600,
        [Description("1024x768")] Res1024x768,
        [Description("1152x864")] Res1152x864,
        [Description("1280x800")] Res1280x800,
        [Description("1280x1024")] Res1280x1024,
        [Description("1366x768")] Res1366x768,
        [Description("1440x900")] Res1440x900,
        [Description("1600x900")] Res1600x900,
        [Description("1600x1200")] Res1600x1200,
        [Description("1680x1050")] Res1680x1050,
        [Description("1920x1080")] Res1920x1080,
        [Description("1920x1200")] Res1920x1200,
        [Description("2048x1536")] Res2048x1536,
        [Description("2560x1440")] Res2560x1440,
        [Description("2560x1600")] Res2560x1600,
        [Description("2560x2048")] Res2560x2048,
        [Description("3840x2160")] Res3840x2160
    }
}