using System;
using System.Collections.Generic;
using System.Text;
using mRemoteNG.Tools;
using System.ComponentModel;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum Defaults
    {
        Colors = RDPColors.Colors16Bit,
        Sounds = RDPSounds.DoNotPlay,
        Resolution = RDPResolutions.FitToWindow,
        Port = 3389
    }

    public enum RDPColors
    {
        [LocalizedAttributes.LocalizedDescription("strRDP256Colors")]
        Colors256 = 8,
        [LocalizedAttributes.LocalizedDescription("strRDP32768Colors")]
        Colors15Bit = 15,
        [LocalizedAttributes.LocalizedDescription("strRDP65536Colors")]
        Colors16Bit = 16,
        [LocalizedAttributes.LocalizedDescription("strRDP16777216Colors")]
        Colors24Bit = 24,
        [LocalizedAttributes.LocalizedDescription("strRDP4294967296Colors")]
        Colors32Bit = 32
    }

    public enum RDPSounds
    {
        [LocalizedAttributes.LocalizedDescription("strRDPSoundBringToThisComputer")]
        BringToThisComputer = 0,
        [LocalizedAttributes.LocalizedDescription("strRDPSoundLeaveAtRemoteComputer")]
        LeaveAtRemoteComputer = 1,
        [LocalizedAttributes.LocalizedDescription("strRDPSoundDoNotPlay")]
        DoNotPlay = 2
    }

    private enum RDPPerformanceFlags
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
        EnableDesktopComposition = 0x100
    }

    public enum RDPResolutions
    {
        [LocalizedAttributes.LocalizedDescription("strRDPFitToPanel")]
        FitToWindow,
        [LocalizedAttributes.LocalizedDescription("strFullscreen")]
        Fullscreen,
        [LocalizedAttributes.LocalizedDescription("strRDPSmartSize")]
        SmartSize,
        [Description("640x480")]
        Res640x480,
        [Description("800x600")]
        Res800x600,
        [Description("1024x768")]
        Res1024x768,
        [Description("1152x864")]
        Res1152x864,
        [Description("1280x800")]
        Res1280x800,
        [Description("1280x1024")]
        Res1280x1024,
        [Description("1400x1050")]
        Res1400x1050,
        [Description("1440x900")]
        Res1440x900,
        [Description("1600x1024")]
        Res1600x1024,
        [Description("1600x1200")]
        Res1600x1200,
        [Description("1600x1280")]
        Res1600x1280,
        [Description("1680x1050")]
        Res1680x1050,
        [Description("1900x1200")]
        Res1900x1200,
        [Description("1920x1200")]
        Res1920x1200,
        [Description("2048x1536")]
        Res2048x1536,
        [Description("2560x2048")]
        Res2560x2048,
        [Description("3200x2400")]
        Res3200x2400,
        [Description("3840x2400")]
        Res3840x2400
    }

    public enum AuthenticationLevel
    {
        [LocalizedAttributes.LocalizedDescription("strAlwaysConnectEvenIfAuthFails")]
        NoAuth = 0,
        [LocalizedAttributes.LocalizedDescription("strDontConnectWhenAuthFails")]
        AuthRequired = 1,
        [LocalizedAttributes.LocalizedDescription("strWarnIfAuthFails")]
        WarnOnFailedAuth = 2
    }

    public enum RDGatewayUsageMethod
    {
        [LocalizedAttributes.LocalizedDescription("strNever")]
        Never = 0, // TSC_PROXY_MODE_NONE_DIRECT
        [LocalizedAttributes.LocalizedDescription("strAlways")]
        Always = 1, // TSC_PROXY_MODE_DIRECT
        [LocalizedAttributes.LocalizedDescription("strDetect")]
        Detect = 2 // TSC_PROXY_MODE_DETECT
    }

    public enum RDGatewayUseConnectionCredentials
    {
        [LocalizedAttributes.LocalizedDescription("strUseDifferentUsernameAndPassword")]
        No = 0,
        [LocalizedAttributes.LocalizedDescription("strUseSameUsernameAndPassword")]
        Yes = 1,
        [LocalizedAttributes.LocalizedDescription("strUseSmartCard")]
        SmartCard = 2
    }
}