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

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGeneral)),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameAll)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionAll)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EverythingInherited
        {
            get => EverythingIsInherited();
            set => SetAllValues(value);
        }

        #endregion

        #region Display

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryDisplay), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameDescription)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionDescription)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Description { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryDisplay), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameIcon)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionIcon)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Icon { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryDisplay), 2),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNamePanel)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionPanel)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Panel { get; set; }

        #endregion

        #region Connection

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameUsername)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionUsername)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Username { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameVmId)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionVmId)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool VmId { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNamePassword)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionPassword)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Password { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameDomain)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionDomain)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Domain { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 3),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNamePort)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionPort)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Port { get; set; }
        
        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 3),
         LocalizedAttributes.LocalizedDisplayNameInheritAttribute(nameof(Language.strPropertyNameSSHTunnelConnection)),
         LocalizedAttributes.LocalizedDescriptionInheritAttribute(nameof(Language.strPropertyDescriptionSSHTunnelConnection)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool SSHTunnelConnectionName { get; set; }

        #endregion

        #region Protocol

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameProtocol)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionProtocol)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Protocol { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRdpVersion)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRdpVersion)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RdpVersion { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameExternalTool)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionExternalTool)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNamePuttySession)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionPuttySession)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PuttySession { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameSSHOptions)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionSSHOptions)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool SSHOptions { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameEncryptionStrength)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionEncryptionStrength)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ICAEncryptionStrength { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameAuthenticationLevel)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionAuthenticationLevel)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPAuthenticationLevel { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRDPMinutesToIdleTimeout)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRDPMinutesToIdleTimeout)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPMinutesToIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRDPAlertIdleTimeout)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRDPAlertIdleTimeout)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPAlertIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameLoadBalanceInfo)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionLoadBalanceInfo)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool LoadBalanceInfo { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameUseConsoleSession)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionUseConsoleSession)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseConsoleSession { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameUseCredSsp)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionUseCredSsp)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseCredSsp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameUseVmId)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionUseVmId)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseVmId { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProtocol), 4),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameUseEnhancedMode)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionUseEnhancedMode)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseEnhancedMode { get; set; }

        #endregion

        #region RD Gateway

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRDGatewayUsageMethod)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRDGatewayUsageMethod)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsageMethod { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRDGatewayHostname)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRDGatewayHostname)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayHostname { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRDGatewayUseConnectionCredentials)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRDGatewayUseConnectionCredentials)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUseConnectionCredentials { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRDGatewayUsername)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRDGatewayUsername)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsername { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRDGatewayPassword)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRDGatewayPassword)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayPassword { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryGateway), 5),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRDGatewayDomain)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRDGatewayDomain)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayDomain { get; set; }

        #endregion

        #region Appearance

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameResolution)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionResolution)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Resolution { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameAutomaticResize)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionAutomaticResize)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutomaticResize { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameColors)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionColors)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Colors { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameCacheBitmaps)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionCacheBitmaps)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool CacheBitmaps { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameDisplayWallpaper)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionDisplayWallpaper)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayWallpaper { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameDisplayThemes)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionDisplayThemes)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayThemes { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameEnableFontSmoothing)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionEnableFontSmoothing)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableFontSmoothing { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 6),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameEnableDesktopComposition)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionEnableDesktopComposition)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableDesktopComposition { get; set; }

        #endregion

        #region Redirect

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRedirectKeys)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRedirectKeys)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectKeys { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRedirectDrives)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRedirectDrives)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectDiskDrives { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRedirectPrinters)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRedirectPrinters)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPrinters { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRedirectClipboard)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRedirectClipboard)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectClipboard { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRedirectPorts)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRedirectPorts)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPorts { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRedirectSmartCards)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRedirectSmartCards)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSmartCards { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRedirectSounds)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRedirectSounds)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSound { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameSoundQuality)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionSoundQuality)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool SoundQuality { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryRedirect), 7),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameRedirectAudioCapture)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionRedirectAudioCapture)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectAudioCapture { get; set; }

        #endregion

        #region Misc

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameExternalToolBefore)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionExternalToolBefore)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PreExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameExternalToolAfter)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionExternalToolAfter)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PostExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameMACAddress)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionMACAddress)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool MacAddress { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 8),
         LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameUser1)),
         LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionUser1)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField { get; set; }

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous), 8),
        LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameFavorite)),
        LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionFavorite)),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Favorite { get; set; }
        #endregion

        #region VNC
        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameCompression)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionCompression)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCCompression {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameEncoding)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionEncoding)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCEncoding {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameAuthenticationMode)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionAuthenticationMode)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCAuthMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameVNCProxyType)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionVNCProxyType)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyType {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameVNCProxyAddress)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionVNCProxyAddress)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyIP {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameVNCProxyPort)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionVNCProxyPort)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPort {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameVNCProxyUsername)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionVNCProxyUsername)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyUsername {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryProxy), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameVNCProxyPassword)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionVNCProxyPassword)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPassword {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameColors)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionColors)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCColors {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameSmartSizeMode)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionSmartSizeMode)), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCSmartSizeMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryAppearance), 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strPropertyNameViewOnly)), 
		LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionViewOnly)), 
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