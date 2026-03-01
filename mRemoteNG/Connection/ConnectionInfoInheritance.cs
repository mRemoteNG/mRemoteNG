using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.Connection
{
    [SupportedOSPlatform("windows")]
    public class ConnectionInfoInheritance
    {
        private ConnectionInfoInheritance? _tempInheritanceStorage;
        private bool _autoEverythingInheritedRequested;

        #region Public Properties

        #region General

        [LocalizedAttributes.LocalizedCategory(nameof(Language.General)),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.All)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAll)),
         TypeConverter(typeof(MiscTools.YesNoAutoTypeConverter))]
        public bool EverythingInherited
        {
            get => EverythingIsInherited();
            set
            {
                if (_autoEverythingInheritedRequested)
                {
                    _autoEverythingInheritedRequested = false;
                    ApplyAutomaticInheritanceFromParent();
                    return;
                }

                SetAllValues(value);
            }
        }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.General)),
         DisplayName("Inherit Automatic Sort"),
         Description("Inherit the Automatic Sort setting from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutoSort { get; set; }

        #endregion

        #region Display

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Description)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDescription)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Description { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Icon)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionIcon)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Icon { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Panel)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionPanel)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Panel { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Color)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionColor)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Color { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.TabColor)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionTabColor)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool TabColor { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Display), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ConnectionFrameColor)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionConnectionFrameColor)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ConnectionFrameColor { get; set; }

        #endregion

        #region Connection

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ExternalCredentialProvider)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionExternalCredentialProvider)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool ExternalCredentialProvider { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.UserViaAPI)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUserViaAPI)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool UserViaAPI { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Username)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUsername)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Username { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.VmId)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVmId)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool VmId { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Password)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionPassword)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Password { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Domain)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDomain)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Domain { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.HostnameIp)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionHostnameIp)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Hostname { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         DisplayName("Inherit IP Address"),
         Description("Inherit the IP Address property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool IPAddress { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         DisplayName("Inherit Primary Address"),
         Description("Inherit the Primary Address setting from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ConnectionAddressPrimary { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         DisplayName("Inherit Alternative Hostname/IP"),
         Description("Inherit the Alternative Hostname/IP property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AlternativeAddress { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Port)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionPort)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Port { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInheritAttribute(nameof(Language.SshTunnel)),
         LocalizedAttributes.LocalizedDescriptionInheritAttribute(nameof(Language.PropertyDescriptionSshTunnel)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool SSHTunnelConnectionName { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInheritAttribute(nameof(Language.OpeningCommand)),
         LocalizedAttributes.LocalizedDescriptionInheritAttribute(nameof(Language.PropertyDescriptionOpeningCommand)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool OpeningCommand { get; set; }

        

        #endregion

        #region Protocol

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Protocol)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionProtocol)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Protocol { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RdpVersion)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRdpVersion)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RdpVersion { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ExternalTool)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionExternalTool)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PuttySession)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionPuttySession)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PuttySession { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.SshOptions)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionSshOptions)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool SSHOptions { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         DisplayName("Inherit Private Key File"),
         Description("Inherit the Private Key File path from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PrivateKeyPath { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.AuthenticationLevel)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAuthenticationLevel)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPAuthenticationLevel { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.MinutesToIdleTimeout)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDPMinutesToIdleTimeout)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPMinutesToIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.MinutesToIdleTimeout)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDPAlertIdleTimeout)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPAlertIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.LoadBalanceInfo)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionLoadBalanceInfo)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool LoadBalanceInfo { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         DisplayName("Inherit RDP Sign Scope"),
         Description("Inherit the RDP Sign Scope setting from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPSignScope { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         DisplayName("Inherit RDP Signature"),
         Description("Inherit the RDP Signature setting from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPSignature { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RenderingEngine)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRenderingEngine)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RenderingEngine { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         DisplayName("Inherit Suppress Script Errors"),
         Description("Inherit the Suppress Script Errors setting from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ScriptErrorsSuppressed { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.UseConsoleSession)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseConsoleSession)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseConsoleSession { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.UseCredSsp)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseCredSsp)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseCredSsp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.UseRestrictedAdmin)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseRestrictedAdmin)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseRestrictedAdmin { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.UseRCG)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseRCG)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseRCG { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.UseVmId)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseVmId)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseVmId { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.UseEnhancedMode)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseEnhancedMode)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseEnhancedMode { get; set; }

        #endregion

        #region RD Gateway

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RdpGatewayUsageMethod)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRdpGatewayUsageMethod)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsageMethod { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RdpGatewayHostname)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayHostname)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayHostname { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RdpGatewayUseConnectionCredentials)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayUseConnectionCredentials)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUseConnectionCredentials { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RdpGatewayUsername)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayUsername)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsername { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RdpGatewayPassword)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRdpGatewayPassword)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayPassword { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RdpGatewayDomain)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayDomain)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayDomain { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ExternalCredentialProvider)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionExternalCredentialProvider)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayExternalCredentialProvider { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.RDPGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.UserViaAPI)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUserViaAPI)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUserViaAPI { get; set; }


        #endregion

        #region Appearance

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Resolution)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionResolution)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Resolution { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         DisplayName("Inherit Sizing Mode"),
         Description("Inherit the RDP sizing mode from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPSizingMode { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         DisplayName("Inherit Resolution Width"),
         Description("Inherit the custom resolution width from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ResolutionWidth { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         DisplayName("Inherit Resolution Height"),
         Description("Inherit the custom resolution height from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ResolutionHeight { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         DisplayName("Inherit Zoom Level (Desktop Scale Factor)"),
         Description("Inherit the RDP zoom level (Desktop Scale Factor) setting from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DesktopScaleFactor { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.AutomaticResize)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAutomaticResize)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutomaticResize { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         DisplayName("Inherit Use Multiple Monitors"),
         Description("Inherit the Use Multiple Monitors setting from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPUseMultimon { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Colors)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionColors)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Colors { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.CacheBitmaps)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionCacheBitmaps)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool CacheBitmaps { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.DisplayWallpaper)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisplayWallpaper)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayWallpaper { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.DisplayThemes)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisplayThemes)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayThemes { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.FontSmoothing)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionEnableFontSmoothing)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableFontSmoothing { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.EnableDesktopComposition)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionEnableDesktopComposition)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableDesktopComposition { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.DisableFullWindowDrag)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisableFullWindowDrag)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisableFullWindowDrag { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.DisableMenuAnimations)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisableMenuAnimations)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisableMenuAnimations { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.DisableCursorShadow)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisableCursorShadow)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisableCursorShadow { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.DisableCursorBlinking)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisableCursorBlinking)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisableCursorBlinking { get; set; }

        #endregion

        #region Redirect

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RedirectKeys)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectKeys)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectKeys { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.DiskDrives)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectDrives)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectDiskDrives { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RedirectDiskDrivesCustom)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectDiskDrivesCustom)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectDiskDrivesCustom { get; set; }
        
        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Printers)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectPrinters)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPrinters { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Clipboard)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectClipboard)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectClipboard { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Redirect)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectPorts)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPorts { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Redirect)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectSmartCards)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSmartCards { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Sounds)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectSounds)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSound { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.SoundQuality)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionSoundQuality)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool SoundQuality { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.AudioCapture)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectAudioCapture)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectAudioCapture { get; set; }

        #endregion

        #region Misc

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ExternalToolBefore)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionExternalToolBefore)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PreExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ExternalToolAfter)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionExternalToolAfter)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PostExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.MacAddress)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionMACAddress)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool MacAddress { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.UserField)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUser1)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 1"),
         Description("Inherit the User Field 1 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField1 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 2"),
         Description("Inherit the User Field 2 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField2 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 3"),
         Description("Inherit the User Field 3 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField3 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 4"),
         Description("Inherit the User Field 4 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField4 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 5"),
         Description("Inherit the User Field 5 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField5 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 6"),
         Description("Inherit the User Field 6 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField6 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 7"),
         Description("Inherit the User Field 7 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField7 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 8"),
         Description("Inherit the User Field 8 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField8 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 9"),
         Description("Inherit the User Field 9 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField9 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit User Field 10"),
         Description("Inherit the User Field 10 property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField10 { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit Notes"),
         Description("Inherit the Notes property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Notes { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.EnvironmentTags)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionEnvironmentTags)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnvironmentTags { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
        LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Favorite)),
        LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionFavorite)),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Favorite { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit Retry On First Connect"),
         Description("Inherit the Retry On First Connect property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RetryOnFirstConnect { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit Wait For IP Availability"),
         Description("Inherit the Wait For IP Availability property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool WaitForIPAvailability { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Miscellaneous), 8),
         DisplayName("Inherit Wait For IP Timeout"),
         Description("Inherit the Wait For IP Timeout property from the parent."),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool WaitForIPTimeout { get; set; }
        #endregion

        #region VNC
        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Compression)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionCompression)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCCompression {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Encoding)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionEncoding)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCEncoding {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.AuthenticationMode)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAuthenticationMode)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCAuthMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ProxyType)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyType)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyType {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ProxyAddress)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyAddress)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyIP {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ProxyPort)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyPort)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPort {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ProxyUsername)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyUsername)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyUsername {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Proxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ProxyPassword)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyPassword)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPassword {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Colors)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionColors)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCColors {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.SmartSizeMode)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionSmartSizeMode)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCSmartSizeMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ViewOnly)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionViewOnly)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCViewOnly {get; set;}

		[LocalizedAttributes.LocalizedCategory(nameof(Language.Redirect), 9),
		DisplayName("Inherit VNC Clipboard Redirect"),
		Description("Inherit the VNC Clipboard Redirect setting from the parent."),
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCClipboardRedirect {get; set;}
        #endregion

        [Browsable(false)]
        public ConnectionInfo? Parent { get; private set; }

        /// <summary>
        /// Indicates whether this inheritance object is enabled.
        /// When false, users of this object should not respect inheritance
        /// settings for individual properties.
        /// </summary>
        [Browsable(false)]
        public bool InheritanceActive => !(Parent is RootNodeInfo || Parent?.Parent is RootNodeInfo);

        #endregion


        public ConnectionInfoInheritance(ConnectionInfo? parent, bool ignoreDefaultInheritance = false)
        {
            Parent = parent;
            if (!ignoreDefaultInheritance)
                SetAllValues(DefaultConnectionInheritance.Instance);
        }


        public ConnectionInfoInheritance Clone(ConnectionInfo parent)
        {
            ConnectionInfoInheritance newInheritance = (ConnectionInfoInheritance)MemberwiseClone();
            newInheritance.Parent = parent;
            return newInheritance;
        }

        public void EnableInheritance()
        {
            if (_tempInheritanceStorage != null)
                UnstashInheritanceData();
        }

        private void UnstashInheritanceData()
        {
            if (_tempInheritanceStorage == null) return;
            SetAllValues(_tempInheritanceStorage);
            _tempInheritanceStorage = null;
        }

        public void DisableInheritance()
        {
            StashInheritanceData();
            TurnOffInheritanceCompletely();
        }

        private void StashInheritanceData()
        {
            _tempInheritanceStorage = Clone(Parent!);
        }

        public void TurnOnInheritanceCompletely()
        {
            SetAllValues(true);
        }

        public void TurnOffInheritanceCompletely()
        {
            SetAllValues(false);
        }

        internal void RequestAutomaticEverythingInheritanceEvaluation()
        {
            _autoEverythingInheritedRequested = true;
        }

        public void ApplyAutomaticInheritanceFromParent()
        {
            ConnectionInfo? childConnection = Parent;
            ConnectionInfo? parentConnection = childConnection?.Parent;
            if (childConnection == null || parentConnection == null)
                return;

            foreach (PropertyInfo inheritanceProperty in s_cachedBoolProperties)
            {
                bool shouldInherit = PropertyValuesMatch(childConnection, parentConnection, inheritanceProperty.Name);
                inheritanceProperty.SetValue(this, shouldInherit);
            }
        }

        private bool EverythingIsInherited()
        {
            IEnumerable<PropertyInfo> inheritanceProperties = GetProperties();
            bool everythingInherited = inheritanceProperties.All((p) => p.GetValue(this, null) is true);
            return everythingInherited;
        }

        private static bool PropertyValuesMatch(ConnectionInfo childConnection, ConnectionInfo parentConnection, string propertyName)
        {
            PropertyInfo? childProperty = childConnection.GetType().GetProperty(propertyName);
            PropertyInfo? parentProperty = parentConnection.GetType().GetProperty(propertyName);
            if (childProperty == null || parentProperty == null)
                return false;

            object? childValue = GetConnectionPropertyValueWithoutInheritance(childConnection, childProperty);
            object? parentValue = GetConnectionPropertyValueWithoutInheritance(parentConnection, parentProperty);
            return Equals(childValue, parentValue);
        }

        private static object? GetConnectionPropertyValueWithoutInheritance(ConnectionInfo connection, PropertyInfo connectionProperty)
        {
            PropertyInfo? inheritanceProperty = typeof(ConnectionInfoInheritance).GetProperty(connectionProperty.Name);
            bool inheritanceWasEnabled = inheritanceProperty?.PropertyType == typeof(bool) &&
                                         inheritanceProperty.GetValue(connection.Inheritance) is true;

            if (inheritanceWasEnabled)
                inheritanceProperty!.SetValue(connection.Inheritance, false);

            try
            {
                return connectionProperty.GetValue(connection);
            }
            finally
            {
                if (inheritanceWasEnabled)
                    inheritanceProperty!.SetValue(connection.Inheritance, true);
            }
        }

        private static readonly HashSet<string> s_exclusions = new(StringComparer.Ordinal)
        {
            nameof(EverythingInherited),
            nameof(Parent),
            nameof(InheritanceActive)
        };

        private static readonly PropertyInfo[] s_cachedProperties =
            typeof(ConnectionInfoInheritance).GetProperties()
                .Where(p => !s_exclusions.Contains(p.Name))
                .ToArray();

        private static readonly PropertyInfo[] s_cachedBoolProperties =
            s_cachedProperties.Where(p => p.PropertyType == typeof(bool)).ToArray();

        public static IEnumerable<PropertyInfo> GetProperties() => s_cachedProperties;

        /// <summary>
        /// Gets the name of all properties where inheritance is turned on
        /// (set to True).
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetEnabledInheritanceProperties()
        {
            return InheritanceActive
                ? GetProperties()
                    .Where(property => property.GetValue(this) is true)
                    .Select(property => property.Name)
                    .ToList()
                : Enumerable.Empty<string>();
        }

        private static bool FilterProperty(PropertyInfo propertyInfo)
        {
            return !s_exclusions.Contains(propertyInfo.Name);
        }

        private void SetAllValues(bool value)
        {
            foreach (PropertyInfo property in s_cachedBoolProperties)
            {
                property.SetValue(this, value, null);
            }
        }

        private void SetAllValues(ConnectionInfoInheritance otherInheritanceObject)
        {
            foreach (PropertyInfo property in s_cachedProperties)
            {
                object? newPropertyValue = property.GetValue(otherInheritanceObject, null);
                property.SetValue(this, newPropertyValue, null);
            }
        }
    }
}