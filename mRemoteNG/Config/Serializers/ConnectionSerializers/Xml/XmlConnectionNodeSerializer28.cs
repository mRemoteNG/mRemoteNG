using System;
using System.Runtime.Versioning;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Plugins;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    // ReSharper disable once InconsistentNaming
    [SupportedOSPlatform("windows")]
    public class XmlConnectionNodeSerializer28(ICryptographyProvider cryptographyProvider,
                                         SecureString encryptionKey,
                                         SaveFilter saveFilter) : ISerializer<ConnectionInfo, XElement>
    {
        private readonly ICryptographyProvider _cryptographyProvider = cryptographyProvider ?? throw new ArgumentNullException(nameof(cryptographyProvider));
        private readonly SecureString _encryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey));
        private readonly SaveFilter _saveFilter = saveFilter ?? throw new ArgumentNullException(nameof(saveFilter));

        public virtual Version Version { get; } = new Version(2, 8);

        protected bool SaveInheritance => _saveFilter.SaveInheritance;

        public XElement Serialize(ConnectionInfo connectionInfo)
        {
            XElement element = new(XName.Get("Node", ""));
            SetElementAttributes(element, connectionInfo);
            SetInheritanceAttributes(element, connectionInfo);
            return element;
        }

        protected virtual void SetElementAttributes(XContainer element, ConnectionInfo connectionInfo)
        {
            ContainerInfo nodeAsContainer = connectionInfo as ContainerInfo;
            element.Add(new XAttribute("Name", connectionInfo.Name));
            element.Add(new XAttribute("VmId", connectionInfo.VmId));
            element.Add(new XAttribute("UseVmId", connectionInfo.UseVmId));
            element.Add(new XAttribute("UseEnhancedMode", connectionInfo.UseEnhancedMode));
            element.Add(new XAttribute("Type", connectionInfo.GetTreeNodeType().ToString()));
            if (nodeAsContainer != null)
            {
                element.Add(new XAttribute("Expanded", nodeAsContainer.IsExpanded.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("IsRootGroup", nodeAsContainer.IsRootGroup.ToString().ToLowerInvariant()));
            }
            element.Add(new XAttribute("Descr", connectionInfo.Description));
            element.Add(new XAttribute("Icon", connectionInfo.Icon));
            element.Add(new XAttribute("Panel", connectionInfo.Panel));
            element.Add(new XAttribute("TabColor", connectionInfo.TabColor));
            element.Add(new XAttribute("ConnectionFrameColor", connectionInfo.ConnectionFrameColor));
            element.Add(new XAttribute("Id", connectionInfo.ConstantID));

            AddCredentialAttributes(element, connectionInfo);

            element.Add(new XAttribute("Hostname", connectionInfo.Hostname));
            element.Add(new XAttribute("Protocol", connectionInfo.Protocol));
            element.Add(new XAttribute("RdpVersion", connectionInfo.RdpVersion.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("SSHTunnelConnectionName", connectionInfo.SSHTunnelConnectionName));
            element.Add(new XAttribute("OpeningCommand", connectionInfo.OpeningCommand));
            element.Add(new XAttribute("SSHOptions", connectionInfo.SSHOptions));
            element.Add(new XAttribute("PuttySession", connectionInfo.PuttySession));
            element.Add(new XAttribute("Port", connectionInfo.Port));
            element.Add(new XAttribute("ConnectToConsole", connectionInfo.UseConsoleSession.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("UseCredSsp", connectionInfo.UseCredSsp.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RenderingEngine", connectionInfo.RenderingEngine));
            element.Add(new XAttribute("RDPAuthenticationLevel", connectionInfo.RDPAuthenticationLevel));
            element.Add(new XAttribute("RDPMinutesToIdleTimeout", connectionInfo.RDPMinutesToIdleTimeout));
            element.Add(new XAttribute("RDPAlertIdleTimeout", connectionInfo.RDPAlertIdleTimeout.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("LoadBalanceInfo", connectionInfo.LoadBalanceInfo));
            element.Add(new XAttribute("Colors", connectionInfo.Colors));
            element.Add(new XAttribute("Resolution", connectionInfo.Resolution));
            element.Add(new XAttribute("AutomaticResize", connectionInfo.AutomaticResize.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("DisplayWallpaper", connectionInfo.DisplayWallpaper.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("DisplayThemes", connectionInfo.DisplayThemes.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("EnableFontSmoothing", connectionInfo.EnableFontSmoothing.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("EnableDesktopComposition", connectionInfo.EnableDesktopComposition.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("DisableFullWindowDrag", connectionInfo.DisableFullWindowDrag.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("DisableMenuAnimations", connectionInfo.DisableMenuAnimations.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("DisableCursorShadow", connectionInfo.DisableCursorShadow.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("DisableCursorBlinking", connectionInfo.DisableCursorBlinking.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("CacheBitmaps", connectionInfo.CacheBitmaps.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectDiskDrives", connectionInfo.RedirectDiskDrives));
            element.Add(new XAttribute("RedirectDiskDrivesCustom", connectionInfo.RedirectDiskDrivesCustom));
            element.Add(new XAttribute("RedirectPorts", connectionInfo.RedirectPorts.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectPrinters", connectionInfo.RedirectPrinters.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectClipboard", connectionInfo.RedirectClipboard.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectSmartCards", connectionInfo.RedirectSmartCards.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectSound", connectionInfo.RedirectSound.ToString()));
            element.Add(new XAttribute("SoundQuality", connectionInfo.SoundQuality.ToString()));
            element.Add(new XAttribute("RedirectAudioCapture", connectionInfo.RedirectAudioCapture.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectKeys", connectionInfo.RedirectKeys.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("Connected", (connectionInfo.OpenConnections.Count > 0).ToString().ToLowerInvariant()));
            element.Add(new XAttribute("PreExtApp", connectionInfo.PreExtApp));
            element.Add(new XAttribute("PostExtApp", connectionInfo.PostExtApp));
            element.Add(new XAttribute("MacAddress", connectionInfo.MacAddress));
            element.Add(new XAttribute("UserField", connectionInfo.UserField));
            element.Add(new XAttribute("EnvironmentTags", connectionInfo.EnvironmentTags));
            element.Add(new XAttribute("Favorite", connectionInfo.Favorite));
            element.Add(new XAttribute("ExtApp", connectionInfo.ExtApp));
            element.Add(new XAttribute("StartProgram", connectionInfo.RDPStartProgram));
            element.Add(new XAttribute("StartProgramWorkDir", connectionInfo.RDPStartProgramWorkDir));
            element.Add(new XAttribute("VNCCompression", connectionInfo.VNCCompression));
            element.Add(new XAttribute("VNCEncoding", connectionInfo.VNCEncoding));
            element.Add(new XAttribute("VNCAuthMode", connectionInfo.VNCAuthMode));
            element.Add(new XAttribute("VNCProxyType", connectionInfo.VNCProxyType));
            element.Add(new XAttribute("VNCProxyIP", connectionInfo.VNCProxyIP));
            element.Add(new XAttribute("VNCProxyPort", connectionInfo.VNCProxyPort));

            element.Add(_saveFilter.SaveUsername
                ? new XAttribute("VNCProxyUsername", connectionInfo.VNCProxyUsername)
                : new XAttribute("VNCProxyUsername", ""));

            element.Add(_saveFilter.SavePassword
                ? new XAttribute("VNCProxyPassword", _cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword, _encryptionKey))
                : new XAttribute("VNCProxyPassword", ""));

            element.Add(new XAttribute("VNCColors", connectionInfo.VNCColors));
            element.Add(new XAttribute("VNCSmartSizeMode", connectionInfo.VNCSmartSizeMode));
            element.Add(new XAttribute("VNCViewOnly", connectionInfo.VNCViewOnly.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RDGatewayUsageMethod", connectionInfo.RDGatewayUsageMethod));
            element.Add(new XAttribute("RDGatewayHostname", connectionInfo.RDGatewayHostname));
            element.Add(new XAttribute("RDGatewayUseConnectionCredentials", connectionInfo.RDGatewayUseConnectionCredentials));
            element.Add(new XAttribute("RDGatewayExternalCredentialProvider", connectionInfo.RDGatewayExternalCredentialProvider));
            element.Add(new XAttribute("RDGatewayUserViaAPI", connectionInfo.RDGatewayUserViaAPI));

            element.Add(_saveFilter.SaveUsername
                ? new XAttribute("RDGatewayUsername", connectionInfo.RDGatewayUsername)
                : new XAttribute("RDGatewayUsername", ""));

            element.Add(_saveFilter.SavePassword
                ? new XAttribute("RDGatewayPassword", _cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword, _encryptionKey))
                : new XAttribute("RDGatewayPassword", ""));

            element.Add(_saveFilter.SavePassword
                ? new XAttribute("RDGatewayAccessToken", _cryptographyProvider.Encrypt(connectionInfo.RDGatewayAccessToken, _encryptionKey))
                : new XAttribute("RDGatewayAccessToken", ""));

            element.Add(_saveFilter.SaveDomain
                ? new XAttribute("RDGatewayDomain", connectionInfo.RDGatewayDomain)
                : new XAttribute("RDGatewayDomain", ""));

            element.Add(new XAttribute("UseRCG", connectionInfo.UseRCG));
            element.Add(new XAttribute("UseRestrictedAdmin", connectionInfo.UseRestrictedAdmin));
            element.Add(new XAttribute("UseRedirectionServerName", connectionInfo.UseRedirectionServerName));

            element.Add(new XAttribute("UserViaAPI", connectionInfo.UserViaAPI));
            element.Add(new XAttribute("EC2InstanceId", connectionInfo.EC2InstanceId));
            element.Add(new XAttribute("EC2Region", connectionInfo.EC2Region));
            element.Add(new XAttribute("ExternalCredentialProvider", connectionInfo.ExternalCredentialProvider));
            element.Add(new XAttribute("ExternalAddressProvider", connectionInfo.ExternalAddressProvider));

            string serializedPluginData = PluginConnectionDataSerializer.Serialize(connectionInfo.PluginProperties);
            if (!string.IsNullOrEmpty(serializedPluginData))
                element.Add(new XElement("PluginData", serializedPluginData));

            // Vault/OpenBao specific
            element.Add(new XAttribute("VaultOpenbaoMount", connectionInfo.VaultOpenbaoMount ?? string.Empty));
            element.Add(new XAttribute("VaultOpenbaoRole", connectionInfo.VaultOpenbaoRole ?? string.Empty));
            element.Add(new XAttribute("VaultOpenbaoSecretEngine", connectionInfo.VaultOpenbaoSecretEngine));
        }

        private void AddCredentialAttributes(XContainer element, ConnectionInfo connectionInfo)
        {
            if (Runtime.UseCredentialManager)
                return;

            element.Add(_saveFilter.SaveUsername
                ? new XAttribute("Username", connectionInfo.Username)
                : new XAttribute("Username", ""));

            element.Add(_saveFilter.SaveDomain
                ? new XAttribute("Domain", connectionInfo.Domain)
                : new XAttribute("Domain", ""));

            element.Add(_saveFilter.SavePassword && !connectionInfo.Inheritance.Password
                ? new XAttribute("Password", _cryptographyProvider.Encrypt(connectionInfo.Password, _encryptionKey))
                : new XAttribute("Password", ""));
        }

        protected virtual void SetInheritanceAttributes(XContainer element, IInheritable connectionInfo)
        {
            if (!_saveFilter.SaveInheritance) return;

            ConnectionInfoInheritance inheritance = connectionInfo.Inheritance;

            AddInheritanceAttribute(element, inheritance.CacheBitmaps, "InheritCacheBitmaps");
            AddInheritanceAttribute(element, inheritance.Colors, "InheritColors");
            AddInheritanceAttribute(element, inheritance.Description, "InheritDescription");
            AddInheritanceAttribute(element, inheritance.DisplayThemes, "InheritDisplayThemes");
            AddInheritanceAttribute(element, inheritance.DisplayWallpaper, "InheritDisplayWallpaper");
            AddInheritanceAttribute(element, inheritance.EnableFontSmoothing, "InheritEnableFontSmoothing");
            AddInheritanceAttribute(element, inheritance.EnableDesktopComposition, "InheritEnableDesktopComposition");
            AddInheritanceAttribute(element, inheritance.DisableFullWindowDrag, "InheritDisableFullWindowDrag");
            AddInheritanceAttribute(element, inheritance.DisableMenuAnimations, "InheritDisableMenuAnimations");
            AddInheritanceAttribute(element, inheritance.DisableCursorShadow, "InheritDisableCursorShadow");
            AddInheritanceAttribute(element, inheritance.DisableCursorBlinking, "InheritDisableCursorBlinking");
            AddInheritanceAttribute(element, inheritance.Domain, "InheritDomain");
            AddInheritanceAttribute(element, inheritance.Icon, "InheritIcon");
            AddInheritanceAttribute(element, inheritance.Panel, "InheritPanel");
            AddInheritanceAttribute(element, inheritance.TabColor, "InheritTabColor");
            AddInheritanceAttribute(element, inheritance.ConnectionFrameColor, "InheritConnectionFrameColor");
            AddInheritanceAttribute(element, inheritance.Password, "InheritPassword");
            AddInheritanceAttribute(element, inheritance.Port, "InheritPort");
            AddInheritanceAttribute(element, inheritance.Protocol, "InheritProtocol");
            AddInheritanceAttribute(element, inheritance.RdpVersion, "InheritRdpVersion");
            AddInheritanceAttribute(element, inheritance.SSHTunnelConnectionName, "InheritSSHTunnelConnectionName");
            AddInheritanceAttribute(element, inheritance.OpeningCommand, "InheritOpeningCommand");
            AddInheritanceAttribute(element, inheritance.SSHOptions, "InheritSSHOptions");
            AddInheritanceAttribute(element, inheritance.PuttySession, "InheritPuttySession");
            AddInheritanceAttribute(element, inheritance.RedirectDiskDrives, "InheritRedirectDiskDrives");
            AddInheritanceAttribute(element, inheritance.RedirectDiskDrivesCustom, "InheritRedirectDiskDrivesCustom");
            AddInheritanceAttribute(element, inheritance.RedirectKeys, "InheritRedirectKeys");
            AddInheritanceAttribute(element, inheritance.RedirectPorts, "InheritRedirectPorts");
            AddInheritanceAttribute(element, inheritance.RedirectPrinters, "InheritRedirectPrinters");
            AddInheritanceAttribute(element, inheritance.RedirectClipboard, "InheritRedirectClipboard");
            AddInheritanceAttribute(element, inheritance.RedirectSmartCards, "InheritRedirectSmartCards");
            AddInheritanceAttribute(element, inheritance.RedirectSound, "InheritRedirectSound");
            AddInheritanceAttribute(element, inheritance.SoundQuality, "InheritSoundQuality");
            AddInheritanceAttribute(element, inheritance.RedirectAudioCapture, "InheritRedirectAudioCapture");
            AddInheritanceAttribute(element, inheritance.Resolution, "InheritResolution");
            AddInheritanceAttribute(element, inheritance.AutomaticResize, "InheritAutomaticResize");
            AddInheritanceAttribute(element, inheritance.UseConsoleSession, "InheritUseConsoleSession");
            AddInheritanceAttribute(element, inheritance.UseCredSsp, "InheritUseCredSsp");
            AddInheritanceAttribute(element, inheritance.RenderingEngine, "InheritRenderingEngine");
            AddInheritanceAttribute(element, inheritance.Username, "InheritUsername");
            AddInheritanceAttribute(element, inheritance.RDPAuthenticationLevel, "InheritRDPAuthenticationLevel");
            AddInheritanceAttribute(element, inheritance.RDPMinutesToIdleTimeout, "InheritRDPMinutesToIdleTimeout");
            AddInheritanceAttribute(element, inheritance.RDPAlertIdleTimeout, "InheritRDPAlertIdleTimeout");
            AddInheritanceAttribute(element, inheritance.LoadBalanceInfo, "InheritLoadBalanceInfo");
            AddInheritanceAttribute(element, inheritance.PreExtApp, "InheritPreExtApp");
            AddInheritanceAttribute(element, inheritance.PostExtApp, "InheritPostExtApp");
            AddInheritanceAttribute(element, inheritance.MacAddress, "InheritMacAddress");
            AddInheritanceAttribute(element, inheritance.UserField, "InheritUserField");
            AddInheritanceAttribute(element, inheritance.EnvironmentTags, "InheritEnvironmentTags");
            AddInheritanceAttribute(element, inheritance.Favorite, "InheritFavorite");
            AddInheritanceAttribute(element, inheritance.ExtApp, "InheritExtApp");
            AddInheritanceAttribute(element, inheritance.VNCCompression, "InheritVNCCompression");
            AddInheritanceAttribute(element, inheritance.VNCEncoding, "InheritVNCEncoding");
            AddInheritanceAttribute(element, inheritance.VNCAuthMode, "InheritVNCAuthMode");
            AddInheritanceAttribute(element, inheritance.VNCProxyType, "InheritVNCProxyType");
            AddInheritanceAttribute(element, inheritance.VNCProxyIP, "InheritVNCProxyIP");
            AddInheritanceAttribute(element, inheritance.VNCProxyPort, "InheritVNCProxyPort");
            AddInheritanceAttribute(element, inheritance.VNCProxyUsername, "InheritVNCProxyUsername");
            AddInheritanceAttribute(element, inheritance.VNCProxyPassword, "InheritVNCProxyPassword");
            AddInheritanceAttribute(element, inheritance.VNCColors, "InheritVNCColors");
            AddInheritanceAttribute(element, inheritance.VNCSmartSizeMode, "InheritVNCSmartSizeMode");
            AddInheritanceAttribute(element, inheritance.VNCViewOnly, "InheritVNCViewOnly");
            AddInheritanceAttribute(element, inheritance.RDGatewayUsageMethod, "InheritRDGatewayUsageMethod");
            AddInheritanceAttribute(element, inheritance.RDGatewayHostname, "InheritRDGatewayHostname");
            AddInheritanceAttribute(element, inheritance.RDGatewayUseConnectionCredentials, "InheritRDGatewayUseConnectionCredentials");
            AddInheritanceAttribute(element, inheritance.RDGatewayUsername, "InheritRDGatewayUsername");
            AddInheritanceAttribute(element, inheritance.RDGatewayPassword, "InheritRDGatewayPassword");
            AddInheritanceAttribute(element, inheritance.RDGatewayAccessToken, "InheritRDGatewayAccessToken");
            AddInheritanceAttribute(element, inheritance.RDGatewayDomain, "InheritRDGatewayDomain");
            AddInheritanceAttribute(element, inheritance.RDGatewayExternalCredentialProvider, "InheritRDGatewayExternalCredentialProvider");
            AddInheritanceAttribute(element, inheritance.RDGatewayUserViaAPI, "InheritRDGatewayUserViaAPI");

            AddInheritanceAttribute(element, inheritance.VmId, "InheritVmId");
            AddInheritanceAttribute(element, inheritance.UseVmId, "InheritUseVmId");
            AddInheritanceAttribute(element, inheritance.UseEnhancedMode, "InheritUseEnhancedMode");
            AddInheritanceAttribute(element, inheritance.ExternalCredentialProvider, "InheritExternalCredentialProvider");
            AddInheritanceAttribute(element, inheritance.UserViaAPI, "InheritUserViaAPI");
            AddInheritanceAttribute(element, inheritance.UseRCG, "InheritUseRCG");
            AddInheritanceAttribute(element, inheritance.UseRestrictedAdmin, "InheritUseRestrictedAdmin");
            AddInheritanceAttribute(element, inheritance.UseRedirectionServerName, "InheritUseRedirectionServerName");
        }

        private static void AddInheritanceAttribute(XContainer element, bool inherit, string attributeName)
        {
            if (inherit)
                element.Add(new XAttribute(attributeName, inherit.ToString().ToLowerInvariant()));
        }
    }
}