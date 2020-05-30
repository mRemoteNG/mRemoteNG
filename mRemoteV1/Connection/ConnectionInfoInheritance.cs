using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using mRemoteNG.Tools;

namespace mRemoteNG.Connection
{
    public class ConnectionInfoInheritance
    {
        private ConnectionInfoInheritance _tempInheritanceStorage;

        #region Public Properties

        #region General

        [LocalizedAttributes.LocalizedCategory(nameof(Language.General)),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameAll)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAll)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EverythingInherited
        {
            get => EverythingIsInherited();
            set => SetAllValues(value);
        }

        #endregion

        #region Display

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryDisplay), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameDescription)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDescription)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Description { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryDisplay), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Icon)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionIcon)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Icon { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryDisplay), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNamePanel)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionPanel)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Panel { get; set; }

        #endregion

        #region Connection

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Username)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUsername)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Username { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameVmId)),
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
         LocalizedAttributes.LocalizedDisplayNameInheritAttribute(nameof(Language.PropertyNameSSHTunnelConnection)),
         LocalizedAttributes.LocalizedDescriptionInheritAttribute(nameof(Language.PropertyDescriptionSSHTunnelConnection)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool SSHTunnelConnectionName { get; set; }

        #endregion

        #region Protocol

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.Protocol)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionProtocol)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Protocol { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRdpVersion)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRdpVersion)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RdpVersion { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameExternalTool)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionExternalTool)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNamePuttySession)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionPuttySession)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PuttySession { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameSSHOptions)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionSSHOptions)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool SSHOptions { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameEncryptionStrength)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionEncryptionStrength)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ICAEncryptionStrength { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameAuthenticationLevel)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAuthenticationLevel)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPAuthenticationLevel { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRDPMinutesToIdleTimeout)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDPMinutesToIdleTimeout)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPMinutesToIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRDPAlertIdleTimeout)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDPAlertIdleTimeout)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPAlertIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameLoadBalanceInfo)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionLoadBalanceInfo)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool LoadBalanceInfo { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRenderingEngine)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRenderingEngine)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RenderingEngine { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameUseConsoleSession)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseConsoleSession)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseConsoleSession { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameUseCredSsp)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseCredSsp)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseCredSsp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameUseVmId)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseVmId)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseVmId { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Protocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameUseEnhancedMode)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUseEnhancedMode)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseEnhancedMode { get; set; }

        #endregion

        #region RD Gateway

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRDGatewayUsageMethod)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayUsageMethod)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsageMethod { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRDGatewayHostname)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayHostname)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayHostname { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRDGatewayUseConnectionCredentials)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayUseConnectionCredentials)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUseConnectionCredentials { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRDGatewayUsername)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayUsername)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsername { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRDGatewayPassword)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayPassword)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayPassword { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRDGatewayDomain)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRDGatewayDomain)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayDomain { get; set; }

        #endregion

        #region Appearance

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameResolution)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionResolution)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Resolution { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameAutomaticResize)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAutomaticResize)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutomaticResize { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameColors)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionColors)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Colors { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameCacheBitmaps)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionCacheBitmaps)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool CacheBitmaps { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameDisplayWallpaper)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisplayWallpaper)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayWallpaper { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameDisplayThemes)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisplayThemes)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayThemes { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameEnableFontSmoothing)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionEnableFontSmoothing)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableFontSmoothing { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameEnableDesktopComposition)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionEnableDesktopComposition)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableDesktopComposition { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameDisableFullWindowDrag)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisableFullWindowDrag)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisableFullWindowDrag { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameDisableMenuAnimations)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisableMenuAnimations)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisableMenuAnimations { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameDisableCursorShadow)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisableCursorShadow)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisableCursorShadow { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameDisableCursorBlinking)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionDisableCursorBlinking)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisableCursorBlinking { get; set; }

        #endregion

        #region Redirect

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRedirectKeys)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectKeys)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectKeys { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRedirectDrives)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectDrives)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectDiskDrives { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRedirectPrinters)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectPrinters)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPrinters { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRedirectClipboard)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectClipboard)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectClipboard { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRedirectPorts)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectPorts)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPorts { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRedirectSmartCards)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectSmartCards)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSmartCards { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRedirectSounds)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectSounds)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSound { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameSoundQuality)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionSoundQuality)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool SoundQuality { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameRedirectAudioCapture)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionRedirectAudioCapture)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectAudioCapture { get; set; }

        #endregion

        #region Misc

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryMiscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameExternalToolBefore)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionExternalToolBefore)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PreExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryMiscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameExternalToolAfter)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionExternalToolAfter)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PostExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryMiscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameMACAddress)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionMACAddress)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool MacAddress { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryMiscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameUser1)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionUser1)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryMiscellaneous), 8),
        LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameFavorite)),
        LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionFavorite)),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Favorite { get; set; }
        #endregion

        #region VNC
        [LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameCompression)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionCompression)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCCompression {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameEncoding)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionEncoding)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCEncoding {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Connection), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameAuthenticationMode)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionAuthenticationMode)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCAuthMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameVNCProxyType)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyType)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyType {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameVNCProxyAddress)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyAddress)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyIP {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameVNCProxyPort)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyPort)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPort {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameVNCProxyUsername)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyUsername)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyUsername {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.CategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameVNCProxyPassword)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionVNCProxyPassword)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPassword {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameColors)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionColors)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCColors {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.PropertyNameSmartSizeMode)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionSmartSizeMode)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCSmartSizeMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.Appearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.ViewOnly)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.PropertyDescriptionViewOnly)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCViewOnly {get; set;}
        #endregion

        [Browsable(false)] public ConnectionInfo Parent { get; private set; }

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
            newInheritance._tempInheritanceStorage = null;
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
            return GetProperties()
                .Where(property => (bool)property.GetValue(this))
                .Select(property => property.Name)
                .ToList();
        }

        private bool FilterProperty(PropertyInfo propertyInfo)
        {
            var exclusions = new[] {"EverythingInherited", "Parent"};
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