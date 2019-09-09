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

        [LocalizedAttributes.LocalizedCategory("strCategoryGeneral"),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameAll"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionAll"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EverythingInherited
        {
            get => EverythingIsInherited();
            set => SetAllValues(value);
        }

        #endregion

        #region Display

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 2),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameDescription"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionDescription"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Description { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 2),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameIcon"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionIcon"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Icon { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 2),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNamePanel"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionPanel"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Panel { get; set; }

        #endregion

        #region Connection

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUsername"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUsername"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Username { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVmId"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVmId"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool VmId { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNamePassword"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionPassword"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Password { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameDomain"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionDomain"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(true)]
        public bool Domain { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNamePort"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionPort"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Port { get; set; }

        #endregion

        #region Protocol

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameProtocol"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionProtocol"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Protocol { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayName("strPropertyNameRdpVersion"),
         LocalizedAttributes.LocalizedDescription("strPropertyDescriptionRdpVersion"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RdpVersion { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameExternalTool"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionExternalTool"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNamePuttySession"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionPuttySession"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PuttySession { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameEncryptionStrength"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionEncryptionStrength"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool ICAEncryptionStrength { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameAuthenticationLevel"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionAuthenticationLevel"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPAuthenticationLevel { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDPMinutesToIdleTimeout"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDPMinutesToIdleTimeout"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPMinutesToIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDPAlertIdleTimeout"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDPAlertIdleTimeout"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDPAlertIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameLoadBalanceInfo"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionLoadBalanceInfo"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool LoadBalanceInfo { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRenderingEngine"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRenderingEngine"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RenderingEngine { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUseConsoleSession"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUseConsoleSession"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseConsoleSession { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUseCredSsp"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUseCredSsp"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseCredSsp { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUseVmId"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUseVmId"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseVmId { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUseEnhancedMode"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUseEnhancedMode"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UseEnhancedMode { get; set; }

        #endregion

        #region RD Gateway

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayUsageMethod"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayUsageMethod"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsageMethod { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayHostname"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayHostname"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayHostname { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayUseConnectionCredentials"),
         LocalizedAttributes.LocalizedDescriptionInherit(
             "strPropertyDescriptionRDGatewayUseConnectionCredentials"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUseConnectionCredentials { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayUsername"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayUsername"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsername { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayPassword"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayPassword"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayPassword { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRDGatewayDomain"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRDGatewayDomain"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayDomain { get; set; }

        #endregion

        #region Appearance

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameResolution"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionResolution"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Resolution { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameAutomaticResize"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionAutomaticResize"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool AutomaticResize { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameColors"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionColors"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Colors { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameCacheBitmaps"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionCacheBitmaps"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool CacheBitmaps { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameDisplayWallpaper"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionDisplayWallpaper"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayWallpaper { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameDisplayThemes"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionDisplayThemes"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool DisplayThemes { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameEnableFontSmoothing"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionEnableFontSmoothing"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableFontSmoothing { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameEnableDesktopComposition"),
         LocalizedAttributes.LocalizedDescriptionInherit(
             "strPropertyDescriptionEnableEnableDesktopComposition"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EnableDesktopComposition { get; set; }

        #endregion

        #region Redirect

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectKeys"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectKeys"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectKeys { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectDrives"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectDrives"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectDiskDrives { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectPrinters"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectPrinters"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPrinters { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectClipboard"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectClipboard"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectClipboard { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectPorts"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectPorts"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectPorts { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectSmartCards"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectSmartCards"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSmartCards { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectSounds"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectSounds"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectSound { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameSoundQuality"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionSoundQuality"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool SoundQuality { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameRedirectAudioCapture"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionRedirectAudioCapture"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RedirectAudioCapture { get; set; }

        #endregion

        #region Misc

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameExternalToolBefore"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionExternalToolBefore"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PreExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameExternalToolAfter"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionExternalToolAfter"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool PostExtApp { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameMACAddress"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionMACAddress"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool MacAddress { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8),
         LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameUser1"),
         LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionUser1"),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool UserField { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8),
        LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameFavorite"),
        LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionFavorite"),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool Favorite { get; set; }
        #endregion

        #region VNC
        [LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameCompression"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionCompression"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCCompression {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameEncoding"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionEncoding"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCEncoding {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryConnection", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameAuthenticationMode"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionAuthenticationMode"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCAuthMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProxy", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyType"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyType"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyType {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProxy", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyAddress"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyAddress"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyIP {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProxy", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyPort"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyPort"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPort {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProxy", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyUsername"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyUsername"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyUsername {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProxy", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameVNCProxyPassword"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionVNCProxyPassword"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPassword {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameColors"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionColors"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCColors {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameSmartSizeMode"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionSmartSizeMode"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCSmartSizeMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInherit("strPropertyNameViewOnly"), 
		LocalizedAttributes.LocalizedDescriptionInherit("strPropertyDescriptionViewOnly"), 
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