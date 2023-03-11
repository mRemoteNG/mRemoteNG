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
        private ConnectionInfoInheritance _tempInheritanceStorage;

        #region Public Properties

        #region General

        [LocalizedAttributes.LocalizedCategory(nameof(Language.General)),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.All)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAll)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EverythingInherited
        {
            get => EverythingIsInherited();
            set => SetAllValues(value);
        }

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
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.RenderingEngine)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRenderingEngine)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RenderingEngine { get; set; }

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
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.AutomaticResize)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAutomaticResize)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutomaticResize { get; set; }

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
        LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Favorite)),
        LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionFavorite)),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Favorite { get; set; }
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
        #endregion

        [Browsable(false)]
        public ConnectionInfo Parent { get; private set; }

        /// <summary>
        /// Indicates whether this inheritance object is enabled.
        /// When false, users of this object should not respect inheritance
        /// settings for individual properties.
        /// </summary>
        [Browsable(false)]
        public bool InheritanceActive => !(Parent is RootNodeInfo || Parent?.Parent is RootNodeInfo);

        #endregion


        public ConnectionInfoInheritance(ConnectionInfo parent, bool ignoreDefaultInheritance = false)
        {
            Parent = parent;
            if (!ignoreDefaultInheritance)
                SetAllValues(DefaultConnectionInheritance.Instance);
        }


        public ConnectionInfoInheritance Clone(ConnectionInfo parent)
        {
            var newInheritance = (ConnectionInfoInheritance)MemberwiseClone();
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
            _tempInheritanceStorage = Clone(Parent);
        }

        public void TurnOnInheritanceCompletely()
        {
            SetAllValues(true);
        }

        public void TurnOffInheritanceCompletely()
        {
            SetAllValues(false);
        }

        private bool EverythingIsInherited()
        {
            var inheritanceProperties = GetProperties();
            var everythingInherited = inheritanceProperties.All((p) => (bool)p.GetValue(this, null));
            return everythingInherited;
        }

        public IEnumerable<PropertyInfo> GetProperties()
        {
            var properties = typeof(ConnectionInfoInheritance).GetProperties();
            var filteredProperties = properties.Where(FilterProperty);
            return filteredProperties;
        }

        /// <summary>
        /// Gets the name of all properties where inheritance is turned on
        /// (set to True).
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetEnabledInheritanceProperties()
        {
            return InheritanceActive
                ? GetProperties()
                    .Where(property => (bool)property.GetValue(this))
                    .Select(property => property.Name)
                    .ToList()
                : Enumerable.Empty<string>();
        }

        private bool FilterProperty(PropertyInfo propertyInfo)
        {
            var exclusions = new[]
            {
                nameof(EverythingInherited),
                nameof(Parent),
                nameof(InheritanceActive)
            };
            var valueShouldNotBeFiltered = !exclusions.Contains(propertyInfo.Name);
            return valueShouldNotBeFiltered;
        }

        private void SetAllValues(bool value)
        {
            var properties = GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType.Name == typeof(bool).Name)
                    property.SetValue(this, value, null);
            }
        }

        private void SetAllValues(ConnectionInfoInheritance otherInheritanceObject)
        {
            var properties = GetProperties();
            foreach (var property in properties)
            {
                var newPropertyValue = property.GetValue(otherInheritanceObject, null);
                property.SetValue(this, newPropertyValue, null);
            }
        }
    }
}