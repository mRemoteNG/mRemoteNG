

using System.Reflection;

namespace mRemoteNG.Connection
{
    public class DefaultConnectionInheritance : ConnectionInfoInheritance
    {
        private static readonly DefaultConnectionInheritance _singletonInstance = new DefaultConnectionInheritance();
        private const string SettingNamePrefix = "InhDefault";


        public static DefaultConnectionInheritance Instance { get { return _singletonInstance; } }

        private DefaultConnectionInheritance() : base(null)
        {
        }

        static DefaultConnectionInheritance()
        { }


        public void LoadFromSettings()
        {
            var inheritanceProperties = GetProperties();
            foreach (var property in inheritanceProperties)
            {
                var propertyFromSettings = typeof(Settings).GetProperty($"{SettingNamePrefix}{property.Name}");
                var valueFromSettings = propertyFromSettings.GetValue(Settings.Default, null);
                property.SetValue(Instance, valueFromSettings, null);
            }
        }

        public void SaveToSettings()
        {
            var inheritanceProperties = GetProperties();
            foreach (var property in inheritanceProperties)
            {
                var propertyFromSettings = typeof(Settings).GetProperty($"{SettingNamePrefix}{property.Name}");
                var localValue = property.GetValue(Instance, null);
                propertyFromSettings.SetValue(Settings.Default, localValue, null);
            }

            //Settings.Default.InhDefaultDescription = Description;
            //Settings.Default.InhDefaultIcon = Icon;
            //Settings.Default.InhDefaultPanel = Panel;
            //Settings.Default.InhDefaultUsername = Username;
            //Settings.Default.InhDefaultPassword = Password;
            //Settings.Default.InhDefaultDomain = Domain;
            //Settings.Default.InhDefaultProtocol = Protocol;
            //Settings.Default.InhDefaultPort = Port;
            //Settings.Default.InhDefaultPuttySession = PuttySession;
            //Settings.Default.InhDefaultUseConsoleSession = UseConsoleSession;
            //Settings.Default.InhDefaultUseCredSsp = UseCredSsp;
            //Settings.Default.InhDefaultRenderingEngine = RenderingEngine;
            //Settings.Default.InhDefaultICAEncryptionStrength = ICAEncryptionStrength;
            //Settings.Default.InhDefaultRDPAuthenticationLevel = RDPAuthenticationLevel;
            //Settings.Default.InhDefaultLoadBalanceInfo = LoadBalanceInfo;
            //Settings.Default.InhDefaultResolution = Resolution;
            //Settings.Default.InhDefaultAutomaticResize = AutomaticResize;
            //Settings.Default.InhDefaultColors = Colors;
            //Settings.Default.InhDefaultCacheBitmaps = CacheBitmaps;
            //Settings.Default.InhDefaultDisplayWallpaper = DisplayWallpaper;
            //Settings.Default.InhDefaultDisplayThemes = DisplayThemes;
            //Settings.Default.InhDefaultEnableFontSmoothing = EnableFontSmoothing;
            //Settings.Default.InhDefaultEnableDesktopComposition = EnableDesktopComposition;
            //// 
            //Settings.Default.InhDefaultRedirectKeys = RedirectKeys;
            //Settings.Default.InhDefaultRedirectDiskDrives = RedirectDiskDrives;
            //Settings.Default.InhDefaultRedirectPrinters = RedirectPrinters;
            //Settings.Default.InhDefaultRedirectPorts = RedirectPorts;
            //Settings.Default.InhDefaultRedirectSmartCards = RedirectSmartCards;
            //Settings.Default.InhDefaultRedirectSound = RedirectSound;
            ////
            //Settings.Default.InhDefaultPreExtApp = PreExtApp;
            //Settings.Default.InhDefaultPostExtApp = PostExtApp;
            //Settings.Default.InhDefaultMacAddress = MacAddress;
            //Settings.Default.InhDefaultUserField = UserField;
            //// VNC inheritance
            //Settings.Default.InhDefaultVNCAuthMode = VNCAuthMode;
            //Settings.Default.InhDefaultVNCColors = VNCColors;
            //Settings.Default.InhDefaultVNCCompression = VNCCompression;
            //Settings.Default.InhDefaultVNCEncoding = VNCEncoding;
            //Settings.Default.InhDefaultVNCProxyIP = VNCProxyIP;
            //Settings.Default.InhDefaultVNCProxyPassword = VNCProxyPassword;
            //Settings.Default.InhDefaultVNCProxyPort = VNCProxyPort;
            //Settings.Default.InhDefaultVNCProxyType = VNCProxyType;
            //Settings.Default.InhDefaultVNCProxyUsername = VNCProxyUsername;
            //Settings.Default.InhDefaultVNCSmartSizeMode = VNCSmartSizeMode;
            //Settings.Default.InhDefaultVNCViewOnly = VNCViewOnly;
            //// Ext. App inheritance
            //Settings.Default.InhDefaultExtApp = ExtApp;
            //// RDP gateway inheritance
            //Settings.Default.InhDefaultRDGatewayUsageMethod = RDGatewayUsageMethod;
            //Settings.Default.InhDefaultRDGatewayHostname = RDGatewayHostname;
            //Settings.Default.InhDefaultRDGatewayUsername = RDGatewayUsername;
            //Settings.Default.InhDefaultRDGatewayPassword = RDGatewayPassword;
            //Settings.Default.InhDefaultRDGatewayDomain = RDGatewayDomain;
            //Settings.Default.InhDefaultRDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials;
        }
    }
}