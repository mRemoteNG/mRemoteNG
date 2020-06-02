using System.ComponentModel;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPPerformanceFlags
    {
        [Description("strRDPDisableWallpaper")]
        DisableWallpaper = 0x1,

  		[Description("strRDPDisableFullWindowdrag")]
        DisableFullWindowDrag = 0x2,
  
  		[Description("strRDPDisableMenuAnimations")]
        DisableMenuAnimations = 0x4,

        [Description("strRDPDisableThemes")]
        DisableThemes = 0x8,

  		[Description("strRDPDisableCursorShadow")]
        DisableCursorShadow = 0x20,

  		[Description("strRDPDisableCursorblinking")]
        DisableCursorBlinking = 0x40,

        [Description("strRDPEnableFontSmoothing")]
        EnableFontSmoothing = 0x80,

        [Description("strRDPEnableDesktopComposition")]
        EnableDesktopComposition = 0x100,
    }
}