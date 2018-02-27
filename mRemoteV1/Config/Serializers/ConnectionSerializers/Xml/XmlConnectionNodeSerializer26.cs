using System;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.Xml
{
	// ReSharper disable once InconsistentNaming
	public class XmlConnectionNodeSerializer26 : ISerializer<ConnectionInfo,XElement>
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _encryptionKey;
        private readonly SaveFilter _saveFilter;

        public XmlConnectionNodeSerializer26(ICryptographyProvider cryptographyProvider, SecureString encryptionKey, SaveFilter saveFilter)
        {
            if (cryptographyProvider == null)
                throw new ArgumentNullException(nameof(cryptographyProvider));
            if (encryptionKey == null)
                throw new ArgumentNullException(nameof(encryptionKey));
            if (saveFilter == null)
                throw new ArgumentNullException(nameof(saveFilter));

            _cryptographyProvider = cryptographyProvider;
            _encryptionKey = encryptionKey;
            _saveFilter = saveFilter;
        }

        public XElement Serialize(ConnectionInfo connectionInfo)
        {
            var element = new XElement(XName.Get("Node", ""));
            SetElementAttributes(element, connectionInfo);
            SetInheritanceAttributes(element, connectionInfo);
            return element;
        }

        private void SetElementAttributes(XContainer element, ConnectionInfo connectionInfo)
        {
            var nodeAsContainer = connectionInfo as ContainerInfo;
            element.Add(new XAttribute("Name", connectionInfo.Name));
            element.Add(new XAttribute("Type", connectionInfo.GetTreeNodeType().ToString()));
            if (nodeAsContainer != null)
                element.Add(new XAttribute("Expanded", nodeAsContainer.IsExpanded.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("Descr", connectionInfo.Description));
            element.Add(new XAttribute("Icon", connectionInfo.Icon));
            element.Add(new XAttribute("Panel", connectionInfo.Panel));
            element.Add(new XAttribute("Id", connectionInfo.ConstantID));

            element.Add(_saveFilter.SaveUsername
                ? new XAttribute("Username", connectionInfo.Username)
                : new XAttribute("Username", ""));

            element.Add(_saveFilter.SaveDomain
                ? new XAttribute("Domain", connectionInfo.Domain)
                : new XAttribute("Domain", ""));

            if (_saveFilter.SavePassword && !connectionInfo.Inheritance.Password)
                element.Add(new XAttribute("Password", _cryptographyProvider.Encrypt(connectionInfo.Password, _encryptionKey)));
            else
                element.Add(new XAttribute("Password", ""));

            element.Add(new XAttribute("Hostname", connectionInfo.Hostname));
            element.Add(new XAttribute("Protocol", connectionInfo.Protocol));
            element.Add(new XAttribute("PuttySession", connectionInfo.PuttySession));
            element.Add(new XAttribute("Port", connectionInfo.Port));
            element.Add(new XAttribute("ConnectToConsole", connectionInfo.UseConsoleSession.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("UseCredSsp", connectionInfo.UseCredSsp.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RenderingEngine", connectionInfo.RenderingEngine));
            element.Add(new XAttribute("ICAEncryptionStrength", connectionInfo.ICAEncryptionStrength));
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
            element.Add(new XAttribute("CacheBitmaps", connectionInfo.CacheBitmaps.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectDiskDrives", connectionInfo.RedirectDiskDrives.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectPorts", connectionInfo.RedirectPorts.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectPrinters", connectionInfo.RedirectPrinters.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectSmartCards", connectionInfo.RedirectSmartCards.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectSound", connectionInfo.RedirectSound.ToString()));
            element.Add(new XAttribute("SoundQuality", connectionInfo.SoundQuality.ToString()));
            element.Add(new XAttribute("RedirectKeys", connectionInfo.RedirectKeys.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("Connected", (connectionInfo.OpenConnections.Count > 0).ToString().ToLowerInvariant()));
            element.Add(new XAttribute("PreExtApp", connectionInfo.PreExtApp));
            element.Add(new XAttribute("PostExtApp", connectionInfo.PostExtApp));
            element.Add(new XAttribute("MacAddress", connectionInfo.MacAddress));
            element.Add(new XAttribute("UserField", connectionInfo.UserField));
            element.Add(new XAttribute("ExtApp", connectionInfo.ExtApp));
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
                ? new XAttribute("VNCProxyPassword",
                    _cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword, _encryptionKey))
                : new XAttribute("VNCProxyPassword", ""));

            element.Add(new XAttribute("VNCColors", connectionInfo.VNCColors));
            element.Add(new XAttribute("VNCSmartSizeMode", connectionInfo.VNCSmartSizeMode));
            element.Add(new XAttribute("VNCViewOnly", connectionInfo.VNCViewOnly.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RDGatewayUsageMethod", connectionInfo.RDGatewayUsageMethod));
            element.Add(new XAttribute("RDGatewayHostname", connectionInfo.RDGatewayHostname));
            element.Add(new XAttribute("RDGatewayUseConnectionCredentials", connectionInfo.RDGatewayUseConnectionCredentials));

            element.Add(_saveFilter.SaveUsername
                ? new XAttribute("RDGatewayUsername", connectionInfo.RDGatewayUsername)
                : new XAttribute("RDGatewayUsername", ""));

            element.Add(_saveFilter.SavePassword
                ? new XAttribute("RDGatewayPassword",
                    _cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword, _encryptionKey))
                : new XAttribute("RDGatewayPassword", ""));

            element.Add(_saveFilter.SaveDomain
                ? new XAttribute("RDGatewayDomain", connectionInfo.RDGatewayDomain)
                : new XAttribute("RDGatewayDomain", ""));
        }

        private void SetInheritanceAttributes(XContainer element, IInheritable connectionInfo)
        {
            if (_saveFilter.SaveInheritance)
            {
                element.Add(new XAttribute("InheritCacheBitmaps", connectionInfo.Inheritance.CacheBitmaps.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritColors", connectionInfo.Inheritance.Colors.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritDescription", connectionInfo.Inheritance.Description.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritDisplayThemes", connectionInfo.Inheritance.DisplayThemes.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritDisplayWallpaper", connectionInfo.Inheritance.DisplayWallpaper.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritEnableFontSmoothing", connectionInfo.Inheritance.EnableFontSmoothing.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritEnableDesktopComposition", connectionInfo.Inheritance.EnableDesktopComposition.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritDomain", connectionInfo.Inheritance.Domain.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritIcon", connectionInfo.Inheritance.Icon.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritPanel", connectionInfo.Inheritance.Panel.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritPassword", connectionInfo.Inheritance.Password.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritPort", connectionInfo.Inheritance.Port.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritProtocol", connectionInfo.Inheritance.Protocol.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritPuttySession", connectionInfo.Inheritance.PuttySession.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRedirectDiskDrives", connectionInfo.Inheritance.RedirectDiskDrives.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRedirectKeys", connectionInfo.Inheritance.RedirectKeys.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRedirectPorts", connectionInfo.Inheritance.RedirectPorts.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRedirectPrinters", connectionInfo.Inheritance.RedirectPrinters.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRedirectSmartCards", connectionInfo.Inheritance.RedirectSmartCards.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRedirectSound", connectionInfo.Inheritance.RedirectSound.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritSoundQuality", connectionInfo.Inheritance.SoundQuality.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritResolution", connectionInfo.Inheritance.Resolution.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritAutomaticResize", connectionInfo.Inheritance.AutomaticResize.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritUseConsoleSession", connectionInfo.Inheritance.UseConsoleSession.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritUseCredSsp", connectionInfo.Inheritance.UseCredSsp.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRenderingEngine", connectionInfo.Inheritance.RenderingEngine.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritUsername", connectionInfo.Inheritance.Username.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritICAEncryptionStrength", connectionInfo.Inheritance.ICAEncryptionStrength.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRDPAuthenticationLevel", connectionInfo.Inheritance.RDPAuthenticationLevel.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRDPMinutesToIdleTimeout", connectionInfo.Inheritance.RDPMinutesToIdleTimeout.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRDPAlertIdleTimeout", connectionInfo.Inheritance.RDPAlertIdleTimeout.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritLoadBalanceInfo", connectionInfo.Inheritance.LoadBalanceInfo.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritPreExtApp", connectionInfo.Inheritance.PreExtApp.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritPostExtApp", connectionInfo.Inheritance.PostExtApp.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritMacAddress", connectionInfo.Inheritance.MacAddress.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritUserField", connectionInfo.Inheritance.UserField.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritExtApp", connectionInfo.Inheritance.ExtApp.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCCompression", connectionInfo.Inheritance.VNCCompression.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCEncoding", connectionInfo.Inheritance.VNCEncoding.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCAuthMode", connectionInfo.Inheritance.VNCAuthMode.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCProxyType", connectionInfo.Inheritance.VNCProxyType.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCProxyIP", connectionInfo.Inheritance.VNCProxyIP.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCProxyPort", connectionInfo.Inheritance.VNCProxyPort.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCProxyUsername", connectionInfo.Inheritance.VNCProxyUsername.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCProxyPassword", connectionInfo.Inheritance.VNCProxyPassword.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCColors", connectionInfo.Inheritance.VNCColors.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCSmartSizeMode", connectionInfo.Inheritance.VNCSmartSizeMode.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritVNCViewOnly", connectionInfo.Inheritance.VNCViewOnly.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRDGatewayUsageMethod", connectionInfo.Inheritance.RDGatewayUsageMethod.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRDGatewayHostname", connectionInfo.Inheritance.RDGatewayHostname.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRDGatewayUseConnectionCredentials", connectionInfo.Inheritance.RDGatewayUseConnectionCredentials.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRDGatewayUsername", connectionInfo.Inheritance.RDGatewayUsername.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRDGatewayPassword", connectionInfo.Inheritance.RDGatewayPassword.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("InheritRDGatewayDomain", connectionInfo.Inheritance.RDGatewayDomain.ToString().ToLowerInvariant()));
            }
            else
            {
                var falseString = false.ToString().ToLowerInvariant();
                element.Add(new XAttribute("InheritCacheBitmaps", falseString));
                element.Add(new XAttribute("InheritColors", falseString));
                element.Add(new XAttribute("InheritDescription", falseString));
                element.Add(new XAttribute("InheritDisplayThemes", falseString));
                element.Add(new XAttribute("InheritDisplayWallpaper", falseString));
                element.Add(new XAttribute("InheritEnableFontSmoothing", falseString));
                element.Add(new XAttribute("InheritEnableDesktopComposition", falseString));
                element.Add(new XAttribute("InheritDomain", falseString));
                element.Add(new XAttribute("InheritIcon", falseString));
                element.Add(new XAttribute("InheritPanel", falseString));
                element.Add(new XAttribute("InheritPassword", falseString));
                element.Add(new XAttribute("InheritPort", falseString));
                element.Add(new XAttribute("InheritProtocol", falseString));
                element.Add(new XAttribute("InheritPuttySession", falseString));
                element.Add(new XAttribute("InheritRedirectDiskDrives", falseString));
                element.Add(new XAttribute("InheritRedirectKeys", falseString));
                element.Add(new XAttribute("InheritRedirectPorts", falseString));
                element.Add(new XAttribute("InheritRedirectPrinters", falseString));
                element.Add(new XAttribute("InheritRedirectSmartCards", falseString));
                element.Add(new XAttribute("InheritRedirectSound", falseString));
                element.Add(new XAttribute("InheritSoundQuality", falseString));
                element.Add(new XAttribute("InheritResolution", falseString));
                element.Add(new XAttribute("InheritAutomaticResize", falseString));
                element.Add(new XAttribute("InheritUseConsoleSession", falseString));
                element.Add(new XAttribute("InheritUseCredSsp", falseString));
                element.Add(new XAttribute("InheritRenderingEngine", falseString));
                element.Add(new XAttribute("InheritUsername", falseString));
                element.Add(new XAttribute("InheritICAEncryptionStrength", falseString));
                element.Add(new XAttribute("InheritRDPAuthenticationLevel", falseString));
                element.Add(new XAttribute("InheritRDPMinutesToIdleTimeout", falseString));
                element.Add(new XAttribute("InheritRDPAlertIdleTimeout", falseString));
                element.Add(new XAttribute("InheritLoadBalanceInfo", falseString));
                element.Add(new XAttribute("InheritPreExtApp", falseString));
                element.Add(new XAttribute("InheritPostExtApp", falseString));
                element.Add(new XAttribute("InheritMacAddress", falseString));
                element.Add(new XAttribute("InheritUserField", falseString));
                element.Add(new XAttribute("InheritExtApp", falseString));
                element.Add(new XAttribute("InheritVNCCompression", falseString));
                element.Add(new XAttribute("InheritVNCEncoding", falseString));
                element.Add(new XAttribute("InheritVNCAuthMode", falseString));
                element.Add(new XAttribute("InheritVNCProxyType", falseString));
                element.Add(new XAttribute("InheritVNCProxyIP", falseString));
                element.Add(new XAttribute("InheritVNCProxyPort", falseString));
                element.Add(new XAttribute("InheritVNCProxyUsername", falseString));
                element.Add(new XAttribute("InheritVNCProxyPassword", falseString));
                element.Add(new XAttribute("InheritVNCColors", falseString));
                element.Add(new XAttribute("InheritVNCSmartSizeMode", falseString));
                element.Add(new XAttribute("InheritVNCViewOnly", falseString));
                element.Add(new XAttribute("InheritRDGatewayUsageMethod", falseString));
                element.Add(new XAttribute("InheritRDGatewayHostname", falseString));
                element.Add(new XAttribute("InheritRDGatewayUseConnectionCredentials", falseString));
                element.Add(new XAttribute("InheritRDGatewayUsername", falseString));
                element.Add(new XAttribute("InheritRDGatewayPassword", falseString));
                element.Add(new XAttribute("InheritRDGatewayDomain", falseString));
            }
        }
    }
}