using System.Security;
using System.Xml.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;


namespace mRemoteNG.Config.Serializers
{
    public class XmlConnectionNodeSerializer27 : IConnectionSerializer<XElement>
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _encryptionKey;
        private readonly SaveFilter _saveFilter;


        public XmlConnectionNodeSerializer27(ICryptographyProvider cryptographyProvider, SecureString encryptionKey, SaveFilter saveFilter)
        {
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
                element.Add(new XAttribute("Expanded", nodeAsContainer.IsExpanded.ToString()));
            element.Add(new XAttribute("Descr", connectionInfo.Description));
            element.Add(new XAttribute("Icon", connectionInfo.Icon));
            element.Add(new XAttribute("Panel", connectionInfo.Panel));
            element.Add(new XAttribute("Id", connectionInfo.ConstantID));

            element.Add(_saveFilter.SaveCredentialId
                ? new XAttribute("CredentialId", connectionInfo.CredentialRecord?.Id.ToString() ?? "")
                : new XAttribute("CredentialId", ""));

            element.Add(new XAttribute("Hostname", connectionInfo.Hostname));
            element.Add(new XAttribute("Protocol", connectionInfo.Protocol));
            element.Add(new XAttribute("PuttySession", connectionInfo.PuttySession));
            element.Add(new XAttribute("Port", connectionInfo.Port));
            element.Add(new XAttribute("ConnectToConsole", connectionInfo.UseConsoleSession.ToString()));
            element.Add(new XAttribute("UseCredSsp", connectionInfo.UseCredSsp.ToString()));
            element.Add(new XAttribute("RenderingEngine", connectionInfo.RenderingEngine));
            element.Add(new XAttribute("ICAEncryptionStrength", connectionInfo.ICAEncryptionStrength));
            element.Add(new XAttribute("RDPAuthenticationLevel", connectionInfo.RDPAuthenticationLevel));
            element.Add(new XAttribute("RDPMinutesToIdleTimeout", connectionInfo.RDPMinutesToIdleTimeout));
            element.Add(new XAttribute("LoadBalanceInfo", connectionInfo.LoadBalanceInfo));
            element.Add(new XAttribute("Colors", connectionInfo.Colors));
            element.Add(new XAttribute("Resolution", connectionInfo.Resolution));
            element.Add(new XAttribute("AutomaticResize", connectionInfo.AutomaticResize.ToString()));
            element.Add(new XAttribute("DisplayWallpaper", connectionInfo.DisplayWallpaper.ToString()));
            element.Add(new XAttribute("DisplayThemes", connectionInfo.DisplayThemes.ToString()));
            element.Add(new XAttribute("EnableFontSmoothing", connectionInfo.EnableFontSmoothing.ToString()));
            element.Add(new XAttribute("EnableDesktopComposition", connectionInfo.EnableDesktopComposition.ToString()));
            element.Add(new XAttribute("CacheBitmaps", connectionInfo.CacheBitmaps.ToString()));
            element.Add(new XAttribute("RedirectDiskDrives", connectionInfo.RedirectDiskDrives.ToString()));
            element.Add(new XAttribute("RedirectPorts", connectionInfo.RedirectPorts.ToString()));
            element.Add(new XAttribute("RedirectPrinters", connectionInfo.RedirectPrinters.ToString()));
            element.Add(new XAttribute("RedirectSmartCards", connectionInfo.RedirectSmartCards.ToString()));
            element.Add(new XAttribute("RedirectSound", connectionInfo.RedirectSound.ToString()));
            element.Add(new XAttribute("SoundQuality", connectionInfo.SoundQuality.ToString()));
            element.Add(new XAttribute("RedirectKeys", connectionInfo.RedirectKeys.ToString()));
            element.Add(new XAttribute("Connected", (connectionInfo.OpenConnections.Count > 0).ToString()));
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
            element.Add(new XAttribute("VNCViewOnly", connectionInfo.VNCViewOnly.ToString()));
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
                element.Add(new XAttribute("InheritCredentialRecord", connectionInfo.Inheritance.CredentialRecord));
                element.Add(new XAttribute("InheritCacheBitmaps", connectionInfo.Inheritance.CacheBitmaps.ToString()));
                element.Add(new XAttribute("InheritColors", connectionInfo.Inheritance.Colors.ToString()));
                element.Add(new XAttribute("InheritDescription", connectionInfo.Inheritance.Description.ToString()));
                element.Add(new XAttribute("InheritDisplayThemes", connectionInfo.Inheritance.DisplayThemes.ToString()));
                element.Add(new XAttribute("InheritDisplayWallpaper", connectionInfo.Inheritance.DisplayWallpaper.ToString()));
                element.Add(new XAttribute("InheritEnableFontSmoothing", connectionInfo.Inheritance.EnableFontSmoothing.ToString()));
                element.Add(new XAttribute("InheritEnableDesktopComposition", connectionInfo.Inheritance.EnableDesktopComposition.ToString()));
                element.Add(new XAttribute("InheritIcon", connectionInfo.Inheritance.Icon.ToString()));
                element.Add(new XAttribute("InheritPanel", connectionInfo.Inheritance.Panel.ToString()));
                element.Add(new XAttribute("InheritPort", connectionInfo.Inheritance.Port.ToString()));
                element.Add(new XAttribute("InheritProtocol", connectionInfo.Inheritance.Protocol.ToString()));
                element.Add(new XAttribute("InheritPuttySession", connectionInfo.Inheritance.PuttySession.ToString()));
                element.Add(new XAttribute("InheritRedirectDiskDrives", connectionInfo.Inheritance.RedirectDiskDrives.ToString()));
                element.Add(new XAttribute("InheritRedirectKeys", connectionInfo.Inheritance.RedirectKeys.ToString()));
                element.Add(new XAttribute("InheritRedirectPorts", connectionInfo.Inheritance.RedirectPorts.ToString()));
                element.Add(new XAttribute("InheritRedirectPrinters", connectionInfo.Inheritance.RedirectPrinters.ToString()));
                element.Add(new XAttribute("InheritRedirectSmartCards", connectionInfo.Inheritance.RedirectSmartCards.ToString()));
                element.Add(new XAttribute("InheritRedirectSound", connectionInfo.Inheritance.RedirectSound.ToString()));
                element.Add(new XAttribute("InheritSoundQuality", connectionInfo.Inheritance.SoundQuality.ToString()));
                element.Add(new XAttribute("InheritResolution", connectionInfo.Inheritance.Resolution.ToString()));
                element.Add(new XAttribute("InheritAutomaticResize", connectionInfo.Inheritance.AutomaticResize.ToString()));
                element.Add(new XAttribute("InheritUseConsoleSession", connectionInfo.Inheritance.UseConsoleSession.ToString()));
                element.Add(new XAttribute("InheritUseCredSsp", connectionInfo.Inheritance.UseCredSsp.ToString()));
                element.Add(new XAttribute("InheritRenderingEngine", connectionInfo.Inheritance.RenderingEngine.ToString()));
                element.Add(new XAttribute("InheritICAEncryptionStrength", connectionInfo.Inheritance.ICAEncryptionStrength.ToString()));
                element.Add(new XAttribute("InheritRDPAuthenticationLevel", connectionInfo.Inheritance.RDPAuthenticationLevel.ToString()));
                element.Add(new XAttribute("InheritRDPMinutesToIdleTimeout", connectionInfo.Inheritance.RDPMinutesToIdleTimeout.ToString()));
                element.Add(new XAttribute("InheritLoadBalanceInfo", connectionInfo.Inheritance.LoadBalanceInfo.ToString()));
                element.Add(new XAttribute("InheritPreExtApp", connectionInfo.Inheritance.PreExtApp.ToString()));
                element.Add(new XAttribute("InheritPostExtApp", connectionInfo.Inheritance.PostExtApp.ToString()));
                element.Add(new XAttribute("InheritMacAddress", connectionInfo.Inheritance.MacAddress.ToString()));
                element.Add(new XAttribute("InheritUserField", connectionInfo.Inheritance.UserField.ToString()));
                element.Add(new XAttribute("InheritExtApp", connectionInfo.Inheritance.ExtApp.ToString()));
                element.Add(new XAttribute("InheritVNCCompression", connectionInfo.Inheritance.VNCCompression.ToString()));
                element.Add(new XAttribute("InheritVNCEncoding", connectionInfo.Inheritance.VNCEncoding.ToString()));
                element.Add(new XAttribute("InheritVNCAuthMode", connectionInfo.Inheritance.VNCAuthMode.ToString()));
                element.Add(new XAttribute("InheritVNCProxyType", connectionInfo.Inheritance.VNCProxyType.ToString()));
                element.Add(new XAttribute("InheritVNCProxyIP", connectionInfo.Inheritance.VNCProxyIP.ToString()));
                element.Add(new XAttribute("InheritVNCProxyPort", connectionInfo.Inheritance.VNCProxyPort.ToString()));
                element.Add(new XAttribute("InheritVNCProxyUsername", connectionInfo.Inheritance.VNCProxyUsername.ToString()));
                element.Add(new XAttribute("InheritVNCProxyPassword", connectionInfo.Inheritance.VNCProxyPassword.ToString()));
                element.Add(new XAttribute("InheritVNCColors", connectionInfo.Inheritance.VNCColors.ToString()));
                element.Add(new XAttribute("InheritVNCSmartSizeMode", connectionInfo.Inheritance.VNCSmartSizeMode.ToString()));
                element.Add(new XAttribute("InheritVNCViewOnly", connectionInfo.Inheritance.VNCViewOnly.ToString()));
                element.Add(new XAttribute("InheritRDGatewayUsageMethod", connectionInfo.Inheritance.RDGatewayUsageMethod.ToString()));
                element.Add(new XAttribute("InheritRDGatewayHostname", connectionInfo.Inheritance.RDGatewayHostname.ToString()));
                element.Add(new XAttribute("InheritRDGatewayUseConnectionCredentials", connectionInfo.Inheritance.RDGatewayUseConnectionCredentials.ToString()));
                element.Add(new XAttribute("InheritRDGatewayUsername", connectionInfo.Inheritance.RDGatewayUsername.ToString()));
                element.Add(new XAttribute("InheritRDGatewayPassword", connectionInfo.Inheritance.RDGatewayPassword.ToString()));
                element.Add(new XAttribute("InheritRDGatewayDomain", connectionInfo.Inheritance.RDGatewayDomain.ToString()));
            }
            else
            {
                element.Add(new XAttribute("InheritCredentialRecord", false.ToString()));
                element.Add(new XAttribute("InheritCacheBitmaps", false.ToString()));
                element.Add(new XAttribute("InheritColors", false.ToString()));
                element.Add(new XAttribute("InheritDescription", false.ToString()));
                element.Add(new XAttribute("InheritDisplayThemes", false.ToString()));
                element.Add(new XAttribute("InheritDisplayWallpaper", false.ToString()));
                element.Add(new XAttribute("InheritEnableFontSmoothing", false.ToString()));
                element.Add(new XAttribute("InheritEnableDesktopComposition", false.ToString()));
                element.Add(new XAttribute("InheritIcon", false.ToString()));
                element.Add(new XAttribute("InheritPanel", false.ToString()));
                element.Add(new XAttribute("InheritPort", false.ToString()));
                element.Add(new XAttribute("InheritProtocol", false.ToString()));
                element.Add(new XAttribute("InheritPuttySession", false.ToString()));
                element.Add(new XAttribute("InheritRedirectDiskDrives", false.ToString()));
                element.Add(new XAttribute("InheritRedirectKeys", false.ToString()));
                element.Add(new XAttribute("InheritRedirectPorts", false.ToString()));
                element.Add(new XAttribute("InheritRedirectPrinters", false.ToString()));
                element.Add(new XAttribute("InheritRedirectSmartCards", false.ToString()));
                element.Add(new XAttribute("InheritRedirectSound", false.ToString()));
                element.Add(new XAttribute("InheritSoundQuality", false.ToString()));
                element.Add(new XAttribute("InheritResolution", false.ToString()));
                element.Add(new XAttribute("InheritAutomaticResize", false.ToString()));
                element.Add(new XAttribute("InheritUseConsoleSession", false.ToString()));
                element.Add(new XAttribute("InheritUseCredSsp", false.ToString()));
                element.Add(new XAttribute("InheritRenderingEngine", false.ToString()));
                element.Add(new XAttribute("InheritICAEncryptionStrength", false.ToString()));
                element.Add(new XAttribute("InheritRDPAuthenticationLevel", false.ToString()));
                element.Add(new XAttribute("InheritRDPMinutesToIdleTimeout", false.ToString()));
                element.Add(new XAttribute("InheritLoadBalanceInfo", false.ToString()));
                element.Add(new XAttribute("InheritPreExtApp", false.ToString()));
                element.Add(new XAttribute("InheritPostExtApp", false.ToString()));
                element.Add(new XAttribute("InheritMacAddress", false.ToString()));
                element.Add(new XAttribute("InheritUserField", false.ToString()));
                element.Add(new XAttribute("InheritExtApp", false.ToString()));
                element.Add(new XAttribute("InheritVNCCompression", false.ToString()));
                element.Add(new XAttribute("InheritVNCEncoding", false.ToString()));
                element.Add(new XAttribute("InheritVNCAuthMode", false.ToString()));
                element.Add(new XAttribute("InheritVNCProxyType", false.ToString()));
                element.Add(new XAttribute("InheritVNCProxyIP", false.ToString()));
                element.Add(new XAttribute("InheritVNCProxyPort", false.ToString()));
                element.Add(new XAttribute("InheritVNCProxyUsername", false.ToString()));
                element.Add(new XAttribute("InheritVNCProxyPassword", false.ToString()));
                element.Add(new XAttribute("InheritVNCColors", false.ToString()));
                element.Add(new XAttribute("InheritVNCSmartSizeMode", false.ToString()));
                element.Add(new XAttribute("InheritVNCViewOnly", false.ToString()));
                element.Add(new XAttribute("InheritRDGatewayUsageMethod", false.ToString()));
                element.Add(new XAttribute("InheritRDGatewayHostname", false.ToString()));
                element.Add(new XAttribute("InheritRDGatewayUseConnectionCredentials", false.ToString()));
                element.Add(new XAttribute("InheritRDGatewayUsername", false.ToString()));
                element.Add(new XAttribute("InheritRDGatewayPassword", false.ToString()));
                element.Add(new XAttribute("InheritRDGatewayDomain", false.ToString()));
            }
        }
    }
}