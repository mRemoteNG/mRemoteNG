using System;
using System.Runtime.Versioning;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    [SupportedOSPlatform("windows")]
    // ReSharper disable once InconsistentNaming
    public class XmlConnectionNodeSerializer26 : ISerializer<ConnectionInfo, XElement>
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _encryptionKey;
        private readonly SaveFilter _saveFilter;

        public Version Version { get; } = new Version(2, 6);

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
            element.Add(new XAttribute("CacheBitmaps", connectionInfo.CacheBitmaps.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectDiskDrives", connectionInfo.RedirectDiskDrives.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectPorts", connectionInfo.RedirectPorts.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectPrinters", connectionInfo.RedirectPrinters.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectClipboard", connectionInfo.RedirectClipboard.ToString().ToLowerInvariant()));
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
            element.Add(new XAttribute("StartProgram", connectionInfo.RDPStartProgram));
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

            element.Add(_saveFilter.SaveUsername
                ? new XAttribute("RDGatewayUsername", connectionInfo.RDGatewayUsername)
                : new XAttribute("RDGatewayUsername", ""));

            element.Add(_saveFilter.SavePassword
                ? new XAttribute("RDGatewayPassword", _cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword, _encryptionKey))
                : new XAttribute("RDGatewayPassword", ""));

            element.Add(_saveFilter.SaveDomain
                ? new XAttribute("RDGatewayDomain", connectionInfo.RDGatewayDomain)
                : new XAttribute("RDGatewayDomain", ""));
        }

        private void SetInheritanceAttributes(XContainer element, IInheritable connectionInfo)
        {
            if (_saveFilter.SaveInheritance)
            {
                var inheritance = connectionInfo.Inheritance;

                if (inheritance.CacheBitmaps)
                    element.Add(new XAttribute("InheritCacheBitmaps", inheritance.CacheBitmaps.ToString().ToLowerInvariant()));
                if (inheritance.Colors)
                    element.Add(new XAttribute("InheritColors", inheritance.Colors.ToString().ToLowerInvariant()));
                if (inheritance.Description)
                    element.Add(new XAttribute("InheritDescription", inheritance.Description.ToString().ToLowerInvariant()));
                if (inheritance.DisplayThemes)
                    element.Add(new XAttribute("InheritDisplayThemes", inheritance.DisplayThemes.ToString().ToLowerInvariant()));
                if (inheritance.DisplayWallpaper)
                    element.Add(new XAttribute("InheritDisplayWallpaper", inheritance.DisplayWallpaper.ToString().ToLowerInvariant()));
                if (inheritance.EnableFontSmoothing)
                    element.Add(new XAttribute("InheritEnableFontSmoothing", inheritance.EnableFontSmoothing.ToString().ToLowerInvariant()));
                if (inheritance.EnableDesktopComposition)
                    element.Add(new XAttribute("InheritEnableDesktopComposition", inheritance.EnableDesktopComposition.ToString().ToLowerInvariant()));
                if (inheritance.Domain)
                    element.Add(new XAttribute("InheritDomain", inheritance.Domain.ToString().ToLowerInvariant()));
                if (inheritance.Icon)
                    element.Add(new XAttribute("InheritIcon", inheritance.Icon.ToString().ToLowerInvariant()));
                if (inheritance.Panel)
                    element.Add(new XAttribute("InheritPanel", inheritance.Panel.ToString().ToLowerInvariant()));
                if (inheritance.Password)
                    element.Add(new XAttribute("InheritPassword", inheritance.Password.ToString().ToLowerInvariant()));
                if (inheritance.Port)
                    element.Add(new XAttribute("InheritPort", inheritance.Port.ToString().ToLowerInvariant()));
                if (inheritance.Protocol)
                    element.Add(new XAttribute("InheritProtocol", inheritance.Protocol.ToString().ToLowerInvariant()));
                if (inheritance.SSHTunnelConnectionName)
                    element.Add(new XAttribute("InheritSSHTunnelConnectionName", inheritance.SSHTunnelConnectionName.ToString().ToLowerInvariant()));
                if (inheritance.OpeningCommand)
                    element.Add(new XAttribute("InheritOpeningCommand", inheritance.OpeningCommand.ToString().ToLowerInvariant()));
                if (inheritance.SSHOptions)
                    element.Add(new XAttribute("InheritSSHOptions", inheritance.SSHOptions.ToString().ToLowerInvariant()));
                if (inheritance.PuttySession)
                    element.Add(new XAttribute("InheritPuttySession", inheritance.PuttySession.ToString().ToLowerInvariant()));
                if (inheritance.RedirectDiskDrives)
                    element.Add(new XAttribute("InheritRedirectDiskDrives", inheritance.RedirectDiskDrives.ToString().ToLowerInvariant()));
                if (inheritance.RedirectKeys)
                    element.Add(new XAttribute("InheritRedirectKeys", inheritance.RedirectKeys.ToString().ToLowerInvariant()));
                if (inheritance.RedirectPorts)
                    element.Add(new XAttribute("InheritRedirectPorts", inheritance.RedirectPorts.ToString().ToLowerInvariant()));
                if (inheritance.RedirectPrinters)
                    element.Add(new XAttribute("InheritRedirectPrinters", inheritance.RedirectPrinters.ToString().ToLowerInvariant()));
                if (inheritance.RedirectSmartCards)
                    element.Add(new XAttribute("InheritRedirectSmartCards", inheritance.RedirectSmartCards.ToString().ToLowerInvariant()));
                if (inheritance.RedirectSound)
                    element.Add(new XAttribute("InheritRedirectSound", inheritance.RedirectSound.ToString().ToLowerInvariant()));
                if (inheritance.SoundQuality)
                    element.Add(new XAttribute("InheritSoundQuality", inheritance.SoundQuality.ToString().ToLowerInvariant()));
                if (inheritance.Resolution)
                    element.Add(new XAttribute("InheritResolution", inheritance.Resolution.ToString().ToLowerInvariant()));
                if (inheritance.AutomaticResize)
                    element.Add(new XAttribute("InheritAutomaticResize", inheritance.AutomaticResize.ToString().ToLowerInvariant()));
                if (inheritance.UseConsoleSession)
                    element.Add(new XAttribute("InheritUseConsoleSession", inheritance.UseConsoleSession.ToString().ToLowerInvariant()));
                if (inheritance.UseCredSsp)
                    element.Add(new XAttribute("InheritUseCredSsp", inheritance.UseCredSsp.ToString().ToLowerInvariant()));
                if (inheritance.RenderingEngine)
                    element.Add(new XAttribute("InheritRenderingEngine", inheritance.RenderingEngine.ToString().ToLowerInvariant()));
                if (inheritance.Username)
                    element.Add(new XAttribute("InheritUsername", inheritance.Username.ToString().ToLowerInvariant()));
                if (inheritance.RDPAuthenticationLevel)
                    element.Add(new XAttribute("InheritRDPAuthenticationLevel", inheritance.RDPAuthenticationLevel.ToString().ToLowerInvariant()));
                if (inheritance.RDPMinutesToIdleTimeout)
                    element.Add(new XAttribute("InheritRDPMinutesToIdleTimeout", inheritance.RDPMinutesToIdleTimeout.ToString().ToLowerInvariant()));
                if (inheritance.RDPAlertIdleTimeout)
                    element.Add(new XAttribute("InheritRDPAlertIdleTimeout", inheritance.RDPAlertIdleTimeout.ToString().ToLowerInvariant()));
                if (inheritance.LoadBalanceInfo)
                    element.Add(new XAttribute("InheritLoadBalanceInfo", inheritance.LoadBalanceInfo.ToString().ToLowerInvariant()));
                if (inheritance.PreExtApp)
                    element.Add(new XAttribute("InheritPreExtApp", inheritance.PreExtApp.ToString().ToLowerInvariant()));
                if (inheritance.PostExtApp)
                    element.Add(new XAttribute("InheritPostExtApp", inheritance.PostExtApp.ToString().ToLowerInvariant()));
                if (inheritance.MacAddress)
                    element.Add(new XAttribute("InheritMacAddress", inheritance.MacAddress.ToString().ToLowerInvariant()));
                if (inheritance.UserField)
                    element.Add(new XAttribute("InheritUserField", inheritance.UserField.ToString().ToLowerInvariant()));
                if (inheritance.ExtApp)
                    element.Add(new XAttribute("InheritExtApp", inheritance.ExtApp.ToString().ToLowerInvariant()));
                if (inheritance.VNCCompression)
                    element.Add(new XAttribute("InheritVNCCompression", inheritance.VNCCompression.ToString().ToLowerInvariant()));
                if (inheritance.VNCEncoding)
                    element.Add(new XAttribute("InheritVNCEncoding", inheritance.VNCEncoding.ToString().ToLowerInvariant()));
                if (inheritance.VNCAuthMode)
                    element.Add(new XAttribute("InheritVNCAuthMode", inheritance.VNCAuthMode.ToString().ToLowerInvariant()));
                if (inheritance.VNCProxyType)
                    element.Add(new XAttribute("InheritVNCProxyType", inheritance.VNCProxyType.ToString().ToLowerInvariant()));
                if (inheritance.VNCProxyIP)
                    element.Add(new XAttribute("InheritVNCProxyIP", inheritance.VNCProxyIP.ToString().ToLowerInvariant()));
                if (inheritance.VNCProxyPort)
                    element.Add(new XAttribute("InheritVNCProxyPort", inheritance.VNCProxyPort.ToString().ToLowerInvariant()));
                if (inheritance.VNCProxyUsername)
                    element.Add(new XAttribute("InheritVNCProxyUsername", inheritance.VNCProxyUsername.ToString().ToLowerInvariant()));
                if (inheritance.VNCProxyPassword)
                    element.Add(new XAttribute("InheritVNCProxyPassword", inheritance.VNCProxyPassword.ToString().ToLowerInvariant()));
                if (inheritance.VNCColors)
                    element.Add(new XAttribute("InheritVNCColors", inheritance.VNCColors.ToString().ToLowerInvariant()));
                if (inheritance.VNCSmartSizeMode)
                    element.Add(new XAttribute("InheritVNCSmartSizeMode", inheritance.VNCSmartSizeMode.ToString().ToLowerInvariant()));
                if (inheritance.VNCViewOnly)
                    element.Add(new XAttribute("InheritVNCViewOnly", inheritance.VNCViewOnly.ToString().ToLowerInvariant()));
                if (inheritance.RDGatewayUsageMethod)
                    element.Add(new XAttribute("InheritRDGatewayUsageMethod", inheritance.RDGatewayUsageMethod.ToString().ToLowerInvariant()));
                if (inheritance.RDGatewayHostname)
                    element.Add(new XAttribute("InheritRDGatewayHostname", inheritance.RDGatewayHostname.ToString().ToLowerInvariant()));
                if (inheritance.RDGatewayUseConnectionCredentials)
                    element.Add(new XAttribute("InheritRDGatewayUseConnectionCredentials", inheritance.RDGatewayUseConnectionCredentials.ToString().ToLowerInvariant()));
                if (inheritance.RDGatewayUsername)
                    element.Add(new XAttribute("InheritRDGatewayUsername", inheritance.RDGatewayUsername.ToString().ToLowerInvariant()));
                if (inheritance.RDGatewayPassword)
                    element.Add(new XAttribute("InheritRDGatewayPassword", inheritance.RDGatewayPassword.ToString().ToLowerInvariant()));
                if (inheritance.RDGatewayDomain)
                    element.Add(new XAttribute("InheritRDGatewayDomain", inheritance.RDGatewayDomain.ToString().ToLowerInvariant()));
            }
        }
    }
}