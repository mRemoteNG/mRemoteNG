using System.Collections.Generic;
using mRemoteNG.Tools;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace mRemoteNG.Connection
{
    public class ConnectionInfoInheritance
	{
        private ConnectionInfoInheritance _tempInheritanceStorage;

        #region Public Properties
        #region General
        [LocalizedAttributes.LocalizedCategory("strCategoryGeneral"),
            LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameAll"),
            LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionAll"), 
            TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool EverythingInherited
		{
			get { return EverythingIsInherited(); }
            set { SetAllValues(value); }
		}
        #endregion
        #region Display
		[LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 2), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameDescription"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionDescription"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool Description {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 2), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameIcon"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionIcon"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool Icon {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryDisplay", 2), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNamePanel"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionPanel"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool Panel {get; set;}
        #endregion
        #region Connection
        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryConnection), 3),
        LocalizedAttributes.LocalizedDisplayNameInherit(nameof(Language.strCategoryCredentials)),
        LocalizedAttributes.LocalizedDescriptionInherit(nameof(Language.strPropertyDescriptionCredential)),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool CredentialRecord { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
        LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameUsername"),
        LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionUsername"),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(false)]
        public bool Username { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
        LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNamePassword"),
        LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionPassword"),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(false)]
        public bool Password { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryConnection", 3),
        LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameDomain"),
        LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionDomain"),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        [Browsable(false)]
        public bool Domain { get; set; }
        #endregion
        #region Protocol
        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameProtocol"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionProtocol"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool Protocol {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameExternalTool"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalTool"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool ExtApp {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNamePort"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionPort"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool Port {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNamePuttySession"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionPuttySession"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool PuttySession {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameEncryptionStrength"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionEncryptionStrength"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool ICAEncryptionStrength {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameAuthenticationLevel"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionAuthenticationLevel"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool RDPAuthenticationLevel {get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
        LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRDPMinutesToIdleTimeout"),
        LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDPMinutesToIdleTimeout"),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool RDPMinutesToIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4),
        LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRDPAlertIdleTimeout"),
        LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDPAlertIdleTimeout"),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))] public bool RDPAlertIdleTimeout { get; set; }

        [LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameLoadBalanceInfo"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionLoadBalanceInfo"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool LoadBalanceInfo {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRenderingEngine"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRenderingEngine"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool RenderingEngine {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameUseConsoleSession"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionUseConsoleSession"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool UseConsoleSession {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryProtocol", 4), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameUseCredSsp"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionUseCredSsp"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool UseCredSsp {get; set;}
        #endregion
        #region RD Gateway
		[LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5),
            LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUsageMethod"),
            LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUsageMethod"), 
			TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsageMethod {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5), 
			LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayHostname"), 
			LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayHostname"), 
			TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayHostname {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5), 
			LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUseConnectionCredentials"), 
			LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUseConnectionCredentials"), 
			TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUseConnectionCredentials {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5), 
			LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayUsername"), 
			LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayUsername"), 
			TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayUsername {get; set;}

        [LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5), 
			LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayPassword"), 
			LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayPassword"), 
			TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayPassword {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryGateway", 5), 
			LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRDGatewayDomain"), 
			LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRDGatewayDomain"), 
			TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool RDGatewayDomain {get; set;}
        #endregion
        #region Appearance
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameResolution"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionResolution"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool Resolution {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameAutomaticResize"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionAutomaticResize"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool AutomaticResize {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameColors"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionColors"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool Colors {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameCacheBitmaps"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionCacheBitmaps"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool CacheBitmaps {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameDisplayWallpaper"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionDisplayWallpaper"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool DisplayWallpaper {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameDisplayThemes"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionDisplayThemes"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool DisplayThemes {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameEnableFontSmoothing"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionEnableFontSmoothing"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool EnableFontSmoothing {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 6), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameEnableDesktopComposition"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionEnableEnableDesktopComposition"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool EnableDesktopComposition {get; set;}

        #endregion
        #region Redirect
        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectKeys"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectKeys"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool RedirectKeys {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectDrives"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectDrives"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool RedirectDiskDrives {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectPrinters"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectPrinters"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool RedirectPrinters {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectPorts"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectPorts"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool RedirectPorts {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectSmartCards"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectSmartCards"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool RedirectSmartCards {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameRedirectSounds"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionRedirectSounds"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool RedirectSound {get; set;}

        [LocalizedAttributes.LocalizedCategory("strCategoryRedirect", 7),
        LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameSoundQuality"),
        LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionSoundQuality"),
        TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public bool SoundQuality { get; set; }
        #endregion
        #region Misc
        [LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameExternalToolBefore"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalToolBefore"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool PreExtApp {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameExternalToolAfter"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionExternalToolAfter"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool PostExtApp {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameMACAddress"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionMACAddress"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool MacAddress {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 8), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameUser1"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionUser1"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool UserField {get; set;}
        #endregion
        #region VNC
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameCompression"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionCompression"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCCompression {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameEncoding"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionEncoding"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCEncoding {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryConnection", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameAuthenticationMode"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionAuthenticationMode"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCAuthMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyType"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyType"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyType {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyAddress"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyAddress"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyIP {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyPort"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyPort"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPort {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyUsername"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyUsername"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyUsername {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryMiscellaneous", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameVNCProxyPassword"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionVNCProxyPassword"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCProxyPassword {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameColors"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionColors"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCColors {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameSmartSizeMode"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionSmartSizeMode"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCSmartSizeMode {get; set;}
				
		[LocalizedAttributes.LocalizedCategory("strCategoryAppearance", 9), 
		LocalizedAttributes.LocalizedDisplayNameInheritAttribute("strPropertyNameViewOnly"), 
		LocalizedAttributes.LocalizedDescriptionInheritAttribute("strPropertyDescriptionViewOnly"), 
		TypeConverter(typeof(MiscTools.YesNoTypeConverter))]public bool VNCViewOnly {get; set;}
        #endregion
		
		[Browsable(false)]
        public object Parent {get; set;}
        #endregion
		

		public ConnectionInfoInheritance(object parent, bool ignoreDefaultInheritance = false)
		{
            Parent = parent;
			if (!ignoreDefaultInheritance)
			    SetAllValues(DefaultConnectionInheritance.Instance);
		}


		public ConnectionInfoInheritance Clone()
		{
		    var newInheritance = (ConnectionInfoInheritance) MemberwiseClone();
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
            _tempInheritanceStorage = Clone();
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

        private bool FilterProperty(PropertyInfo propertyInfo)
        {
            var exclusions = new[] { "EverythingInherited", "Parent" };
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