using System;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tools;

namespace mRemoteNG.Config.Serializers.Xml
{
    // ReSharper disable once InconsistentNaming
    public class XmlConnectionNodeSerializer27 : ISerializer<ConnectionInfo, XElement>
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _encryptionKey;
        private readonly SaveFilter _saveFilter;

        public Version Version { get; } = new Version(2, 7);

        public XmlConnectionNodeSerializer27(ICryptographyProvider cryptographyProvider,
                                             SecureString encryptionKey,
                                             SaveFilter saveFilter)
        {
            _cryptographyProvider = cryptographyProvider.ThrowIfNull(nameof(cryptographyProvider));
            _encryptionKey = encryptionKey.ThrowIfNull(nameof(encryptionKey));
            _saveFilter = saveFilter.ThrowIfNull(nameof(saveFilter));
        }

        public XElement Serialize(ConnectionInfo connectionInfo)
        {
            var element = new XElement(XName.Get("Node", ""));
            SetElementAttributes(element, connectionInfo);
            SetInheritanceAttributes(element, connectionInfo.Inheritance);
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
            element.Add(new XAttribute("CredentialId", connectionInfo.CredentialRecordId));

            element.Add(new XAttribute("Hostname", connectionInfo.Hostname));
            element.Add(new XAttribute("Protocol", connectionInfo.Protocol));
            element.Add(new XAttribute("PuttySession", connectionInfo.PuttySession));
            element.Add(new XAttribute("Port", connectionInfo.Port));
            element.Add(new XAttribute("ConnectToConsole",
                                       connectionInfo.UseConsoleSession.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("UseCredSsp", connectionInfo.UseCredSsp.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RenderingEngine", connectionInfo.RenderingEngine));
            element.Add(new XAttribute("ICAEncryptionStrength", connectionInfo.ICAEncryptionStrength));
            element.Add(new XAttribute("RDPAuthenticationLevel", connectionInfo.RDPAuthenticationLevel));
            element.Add(new XAttribute("RDPMinutesToIdleTimeout", connectionInfo.RDPMinutesToIdleTimeout));
            element.Add(new XAttribute("RDPAlertIdleTimeout",
                                       connectionInfo.RDPAlertIdleTimeout.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("LoadBalanceInfo", connectionInfo.LoadBalanceInfo));
            element.Add(new XAttribute("Colors", connectionInfo.Colors));
            element.Add(new XAttribute("Resolution", connectionInfo.Resolution));
            element.Add(new XAttribute("AutomaticResize",
                                       connectionInfo.AutomaticResize.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("DisplayWallpaper",
                                       connectionInfo.DisplayWallpaper.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("DisplayThemes", connectionInfo.DisplayThemes.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("EnableFontSmoothing",
                                       connectionInfo.EnableFontSmoothing.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("EnableDesktopComposition",
                                       connectionInfo.EnableDesktopComposition.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("CacheBitmaps", connectionInfo.CacheBitmaps.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectDiskDrives",
                                       connectionInfo.RedirectDiskDrives.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectPorts", connectionInfo.RedirectPorts.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectPrinters",
                                       connectionInfo.RedirectPrinters.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectClipboard",
                                       connectionInfo.RedirectClipboard.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectSmartCards",
                                       connectionInfo.RedirectSmartCards.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectSound", connectionInfo.RedirectSound.ToString()));
            element.Add(new XAttribute("SoundQuality", connectionInfo.SoundQuality.ToString()));
            element.Add(new XAttribute("RedirectKeys", connectionInfo.RedirectKeys.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("Connected",
                                       (connectionInfo.OpenConnections.Count > 0).ToString().ToLowerInvariant()));
            element.Add(new XAttribute("PreExtApp", connectionInfo.PreExtApp));
            element.Add(new XAttribute("PostExtApp", connectionInfo.PostExtApp));
            element.Add(new XAttribute("MacAddress", connectionInfo.MacAddress));
            element.Add(new XAttribute("UserField", connectionInfo.UserField));
            element.Add(new XAttribute("Favorite", connectionInfo.Favorite));
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
                                             _cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword,
                                                                           _encryptionKey))
                            : new XAttribute("VNCProxyPassword", ""));

            element.Add(new XAttribute("VNCColors", connectionInfo.VNCColors));
            element.Add(new XAttribute("VNCSmartSizeMode", connectionInfo.VNCSmartSizeMode));
            element.Add(new XAttribute("VNCViewOnly", connectionInfo.VNCViewOnly.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RDGatewayUsageMethod", connectionInfo.RDGatewayUsageMethod));
            element.Add(new XAttribute("RDGatewayHostname", connectionInfo.RDGatewayHostname));
            element.Add(new XAttribute("RDGatewayUseConnectionCredentials",
                                       connectionInfo.RDGatewayUseConnectionCredentials));

            element.Add(_saveFilter.SaveUsername
                            ? new XAttribute("RDGatewayUsername", connectionInfo.RDGatewayUsername)
                            : new XAttribute("RDGatewayUsername", ""));

            element.Add(_saveFilter.SavePassword
                            ? new XAttribute("RDGatewayPassword",
                                             _cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword,
                                                                           _encryptionKey))
                            : new XAttribute("RDGatewayPassword", ""));

            element.Add(_saveFilter.SaveDomain
                            ? new XAttribute("RDGatewayDomain", connectionInfo.RDGatewayDomain)
                            : new XAttribute("RDGatewayDomain", ""));
        }

        private void SetInheritanceAttributes(XContainer element, ConnectionInfoInheritance inheritance)
        {
            element.Add(
                new XAttribute("InheritCacheBitmaps", FormatInheritance(inheritance.CacheBitmaps)),
                new XAttribute("InheritColors", FormatInheritance(inheritance.Colors)),
                new XAttribute("InheritDescription", FormatInheritance(inheritance.Description)),
                new XAttribute("InheritDisplayThemes", FormatInheritance(inheritance.DisplayThemes)),
                new XAttribute("InheritDisplayWallpaper", FormatInheritance(inheritance.DisplayWallpaper)),
                new XAttribute("InheritEnableFontSmoothing", FormatInheritance(inheritance.EnableFontSmoothing)),
                new XAttribute("InheritEnableDesktopComposition", FormatInheritance(inheritance.EnableDesktopComposition)),
                new XAttribute("InheritCredentialId", FormatInheritance(inheritance.CredentialId)),
                new XAttribute("InheritIcon", FormatInheritance(inheritance.Icon)),
                new XAttribute("InheritPanel", FormatInheritance(inheritance.Panel)),
                new XAttribute("InheritPort", FormatInheritance(inheritance.Port)),
                new XAttribute("InheritProtocol", FormatInheritance(inheritance.Protocol)),
                new XAttribute("InheritPuttySession", FormatInheritance(inheritance.PuttySession)),
                new XAttribute("InheritRedirectDiskDrives", FormatInheritance(inheritance.RedirectDiskDrives)),
                new XAttribute("InheritRedirectKeys", FormatInheritance(inheritance.RedirectKeys)),
                new XAttribute("InheritRedirectPorts", FormatInheritance(inheritance.RedirectPorts)),
                new XAttribute("InheritRedirectPrinters", FormatInheritance(inheritance.RedirectPrinters)),
                new XAttribute("InheritRedirectClipboard", FormatInheritance(inheritance.RedirectClipboard)),
                new XAttribute("InheritRedirectSmartCards", FormatInheritance(inheritance.RedirectSmartCards)),
                new XAttribute("InheritRedirectSound", FormatInheritance(inheritance.RedirectSound)),
                new XAttribute("InheritSoundQuality", FormatInheritance(inheritance.SoundQuality)),
                new XAttribute("InheritResolution", FormatInheritance(inheritance.Resolution)),
                new XAttribute("InheritAutomaticResize", FormatInheritance(inheritance.AutomaticResize)),
                new XAttribute("InheritUseConsoleSession", FormatInheritance(inheritance.UseConsoleSession)),
                new XAttribute("InheritUseCredSsp", FormatInheritance(inheritance.UseCredSsp)),
                new XAttribute("InheritRenderingEngine", FormatInheritance(inheritance.RenderingEngine)),
                new XAttribute("InheritICAEncryptionStrength", FormatInheritance(inheritance.ICAEncryptionStrength)),
                new XAttribute("InheritRDPAuthenticationLevel", FormatInheritance(inheritance.RDPAuthenticationLevel)),
                new XAttribute("InheritRDPMinutesToIdleTimeout", FormatInheritance(inheritance.RDPMinutesToIdleTimeout)),
                new XAttribute("InheritRDPAlertIdleTimeout", FormatInheritance(inheritance.RDPAlertIdleTimeout)),
                new XAttribute("InheritLoadBalanceInfo", FormatInheritance(inheritance.LoadBalanceInfo)),
                new XAttribute("InheritPreExtApp", FormatInheritance(inheritance.PreExtApp)),
                new XAttribute("InheritPostExtApp", FormatInheritance(inheritance.PostExtApp)),
                new XAttribute("InheritMacAddress", FormatInheritance(inheritance.MacAddress)),
                new XAttribute("InheritUserField", FormatInheritance(inheritance.UserField)),
                new XAttribute("InheritFavorite", FormatInheritance(inheritance.Favorite)),
                new XAttribute("InheritExtApp", FormatInheritance(inheritance.ExtApp)),
                new XAttribute("InheritVNCCompression", FormatInheritance(inheritance.VNCCompression)),
                new XAttribute("InheritVNCEncoding", FormatInheritance(inheritance.VNCEncoding)),
                new XAttribute("InheritVNCAuthMode", FormatInheritance(inheritance.VNCAuthMode)),
                new XAttribute("InheritVNCProxyType", FormatInheritance(inheritance.VNCProxyType)),
                new XAttribute("InheritVNCProxyIP", FormatInheritance(inheritance.VNCProxyIP)),
                new XAttribute("InheritVNCProxyPort", FormatInheritance(inheritance.VNCProxyPort)),
                new XAttribute("InheritVNCProxyUsername", FormatInheritance(inheritance.VNCProxyUsername)),
                new XAttribute("InheritVNCProxyPassword", FormatInheritance(inheritance.VNCProxyPassword)),
                new XAttribute("InheritVNCColors", FormatInheritance(inheritance.VNCColors)),
                new XAttribute("InheritVNCSmartSizeMode", FormatInheritance(inheritance.VNCSmartSizeMode)),
                new XAttribute("InheritVNCViewOnly", FormatInheritance(inheritance.VNCViewOnly)),
                new XAttribute("InheritRDGatewayUsageMethod", FormatInheritance(inheritance.RDGatewayUsageMethod)),
                new XAttribute("InheritRDGatewayHostname", FormatInheritance(inheritance.RDGatewayHostname)),
                new XAttribute("InheritRDGatewayUseConnectionCredentials", FormatInheritance(inheritance.RDGatewayUseConnectionCredentials)),
                new XAttribute("InheritRDGatewayUsername", FormatInheritance(inheritance.RDGatewayUsername)),
                new XAttribute("InheritRDGatewayPassword", FormatInheritance(inheritance.RDGatewayPassword)),
                new XAttribute("InheritRDGatewayDomain", FormatInheritance(inheritance.RDGatewayDomain)));
        }

        private string FormatInheritance(bool value)
        {
            // this is easier to understand than the alternative && expression
            // ReSharper disable once SimplifyConditionalTernaryExpression
            return (_saveFilter.SaveInheritance ? value : false)
                .ToString()
                .ToLowerInvariant();
        }
    }
}