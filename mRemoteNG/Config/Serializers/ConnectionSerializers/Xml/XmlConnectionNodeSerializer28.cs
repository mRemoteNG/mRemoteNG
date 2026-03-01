using System;
using System.Runtime.Versioning;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Xml
{
    // ReSharper disable once InconsistentNaming
    [SupportedOSPlatform("windows")]
    public class XmlConnectionNodeSerializer28 : ISerializer<ConnectionInfo, XElement>
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _encryptionKey;
        private readonly SaveFilter _saveFilter;

        public XmlConnectionNodeSerializer28(ICryptographyProvider cryptographyProvider,
                                             SecureString encryptionKey,
                                             SaveFilter saveFilter)
        {
            ArgumentNullException.ThrowIfNull(cryptographyProvider);
            ArgumentNullException.ThrowIfNull(encryptionKey);
            ArgumentNullException.ThrowIfNull(saveFilter);
            _cryptographyProvider = cryptographyProvider;
            _encryptionKey = encryptionKey;
            _saveFilter = saveFilter;
        }

        public Version Version { get; } = new Version(2, 8);

        public XElement Serialize(ConnectionInfo connectionInfo)
        {
            XElement element = new(XName.Get("Node", ""));
            SetElementAttributes(element, connectionInfo);
            SetInheritanceAttributes(element, connectionInfo);
            return element;
        }

        private void SetElementAttributes(XContainer element, ConnectionInfo connectionInfo)
        {
            ContainerInfo? nodeAsContainer = connectionInfo as ContainerInfo;
            element.Add(new XAttribute("Name", connectionInfo.Name));
            element.Add(new XAttribute("VmId", connectionInfo.VmId));
            element.Add(new XAttribute("UseVmId", connectionInfo.UseVmId));
            element.Add(new XAttribute("UseEnhancedMode", connectionInfo.UseVmId));
            element.Add(new XAttribute("IsTemplate", connectionInfo.IsTemplate.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("UsePersistentBrowser", connectionInfo.UsePersistentBrowser.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("ScriptErrorsSuppressed", connectionInfo.ScriptErrorsSuppressed.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("ShowBrowserNavigationBar", connectionInfo.ShowBrowserNavigationBar.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("Type", connectionInfo.GetTreeNodeType().ToString()));
            if (nodeAsContainer != null)
            {
                element.Add(new XAttribute("Expanded", nodeAsContainer.IsExpanded.ToString().ToLowerInvariant()));
                element.Add(new XAttribute("AutoSort", nodeAsContainer.AutoSort.ToString().ToLowerInvariant()));

                if (_saveFilter.SavePassword && !string.IsNullOrEmpty(nodeAsContainer.ContainerPassword))
                    element.Add(new XAttribute("ContainerPassword", _cryptographyProvider.Encrypt(nodeAsContainer.ContainerPassword, _encryptionKey)));

                if (nodeAsContainer.DynamicSource != DynamicSourceType.None)
                {
                    element.Add(new XAttribute("DynamicSource", nodeAsContainer.DynamicSource.ToString()));
                    element.Add(new XAttribute("DynamicSourceValue", nodeAsContainer.DynamicSourceValue));
                    element.Add(new XAttribute("DynamicRefreshInterval", nodeAsContainer.DynamicRefreshInterval));
                }
            }
            element.Add(new XAttribute("Descr", connectionInfo.Description ?? string.Empty));
            element.Add(new XAttribute("Icon", connectionInfo.Icon ?? string.Empty));
            element.Add(new XAttribute("Panel", connectionInfo.Panel ?? string.Empty));
            element.Add(new XAttribute("Color", connectionInfo.Color ?? string.Empty));
            element.Add(new XAttribute("TabColor", connectionInfo.TabColor ?? string.Empty));
            element.Add(new XAttribute("ConnectionFrameColor", connectionInfo.ConnectionFrameColor));
            element.Add(new XAttribute("Id", connectionInfo.ConstantID));
            if (!string.IsNullOrWhiteSpace(connectionInfo.LinkedConnectionId))
                element.Add(new XAttribute("LinkedConnectionId", connectionInfo.LinkedConnectionId));

            if (!Runtime.UseCredentialManager)
            {
                element.Add(_saveFilter.SaveUsername
                    ? new XAttribute("Username", connectionInfo.Username ?? string.Empty)
                    : new XAttribute("Username", ""));

                element.Add(_saveFilter.SaveDomain
                    ? new XAttribute("Domain", connectionInfo.Domain ?? string.Empty)
                    : new XAttribute("Domain", ""));

                if (_saveFilter.SavePassword && !connectionInfo.Inheritance.Password)
                    //element.Add(new XAttribute("Password", _cryptographyProvider.Encrypt(connectionInfo.Password?.ConvertToUnsecureString(), _encryptionKey)));
                    element.Add(new XAttribute("Password", _cryptographyProvider.Encrypt(connectionInfo.Password ?? string.Empty, _encryptionKey)));
                else
                    element.Add(new XAttribute("Password", ""));
            }

            element.Add(new XAttribute("Hostname", connectionInfo.Hostname ?? string.Empty));
            element.Add(new XAttribute("IPAddress", connectionInfo.IPAddress ?? string.Empty));
            element.Add(new XAttribute("ConnectionAddressPrimary", connectionInfo.ConnectionAddressPrimary));
            element.Add(new XAttribute("AlternativeAddress", connectionInfo.AlternativeAddress ?? string.Empty));
            element.Add(new XAttribute("Protocol", connectionInfo.Protocol));
            element.Add(new XAttribute("RdpVersion", connectionInfo.RdpVersion.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("SSHTunnelConnectionName", connectionInfo.SSHTunnelConnectionName ?? string.Empty));
            element.Add(new XAttribute("OpeningCommand", connectionInfo.OpeningCommand ?? string.Empty));
            element.Add(new XAttribute("SSHOptions", connectionInfo.SSHOptions ?? string.Empty));
            element.Add(new XAttribute("PrivateKeyPath", connectionInfo.PrivateKeyPath ?? string.Empty));
            element.Add(new XAttribute("PuttySession", connectionInfo.PuttySession ?? string.Empty));
            element.Add(new XAttribute("Port", connectionInfo.Port));
            element.Add(new XAttribute("ConnectToConsole", connectionInfo.UseConsoleSession.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("UseCredSsp", connectionInfo.UseCredSsp.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RenderingEngine", connectionInfo.RenderingEngine));
            element.Add(new XAttribute("RDPAuthenticationLevel", connectionInfo.RDPAuthenticationLevel));
            element.Add(new XAttribute("RDPMinutesToIdleTimeout", connectionInfo.RDPMinutesToIdleTimeout));
            element.Add(new XAttribute("RDPAlertIdleTimeout", connectionInfo.RDPAlertIdleTimeout.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("LoadBalanceInfo", connectionInfo.LoadBalanceInfo ?? string.Empty));
            element.Add(new XAttribute("RDPSignScope", connectionInfo.RDPSignScope ?? string.Empty));
            element.Add(new XAttribute("RDPSignature", connectionInfo.RDPSignature ?? string.Empty));
            element.Add(new XAttribute("Colors", connectionInfo.Colors));
            element.Add(new XAttribute("Resolution", connectionInfo.Resolution));
            element.Add(new XAttribute("DesktopScaleFactor", connectionInfo.DesktopScaleFactor));
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
            element.Add(new XAttribute("RedirectDiskDrivesCustom", connectionInfo.RedirectDiskDrivesCustom ?? string.Empty));
            element.Add(new XAttribute("RedirectPorts", connectionInfo.RedirectPorts.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectPrinters", connectionInfo.RedirectPrinters.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectClipboard", connectionInfo.RedirectClipboard.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectSmartCards", connectionInfo.RedirectSmartCards.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectSound", connectionInfo.RedirectSound.ToString()));
            element.Add(new XAttribute("SoundQuality", connectionInfo.SoundQuality.ToString()));
            element.Add(new XAttribute("RedirectAudioCapture", connectionInfo.RedirectAudioCapture.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RedirectKeys", connectionInfo.RedirectKeys.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("Connected", (connectionInfo.OpenConnections.Count > 0).ToString().ToLowerInvariant()));
            element.Add(new XAttribute("PreExtApp", connectionInfo.PreExtApp ?? string.Empty));
            element.Add(new XAttribute("PostExtApp", connectionInfo.PostExtApp ?? string.Empty));
            element.Add(new XAttribute("MacAddress", connectionInfo.MacAddress ?? string.Empty));
            element.Add(new XAttribute("UserField", connectionInfo.UserField ?? string.Empty));
            element.Add(new XAttribute("UserField1", connectionInfo.UserField1 ?? string.Empty));
            element.Add(new XAttribute("UserField2", connectionInfo.UserField2 ?? string.Empty));
            element.Add(new XAttribute("UserField3", connectionInfo.UserField3 ?? string.Empty));
            element.Add(new XAttribute("UserField4", connectionInfo.UserField4 ?? string.Empty));
            element.Add(new XAttribute("UserField5", connectionInfo.UserField5 ?? string.Empty));
            element.Add(new XAttribute("UserField6", connectionInfo.UserField6 ?? string.Empty));
            element.Add(new XAttribute("UserField7", connectionInfo.UserField7 ?? string.Empty));
            element.Add(new XAttribute("UserField8", connectionInfo.UserField8 ?? string.Empty));
            element.Add(new XAttribute("UserField9", connectionInfo.UserField9 ?? string.Empty));
            element.Add(new XAttribute("UserField10", connectionInfo.UserField10 ?? string.Empty));
            element.Add(new XAttribute("Notes", connectionInfo.Notes ?? string.Empty));
            element.Add(new XAttribute("EnvironmentTags", connectionInfo.EnvironmentTags ?? string.Empty));
            element.Add(new XAttribute("Favorite", connectionInfo.Favorite));
            element.Add(new XAttribute("RetryOnFirstConnect", connectionInfo.RetryOnFirstConnect.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("WaitForIPAvailability", connectionInfo.WaitForIPAvailability.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("WaitForIPTimeout", connectionInfo.WaitForIPTimeout));
            element.Add(new XAttribute("AlwaysPromptForCredentials", connectionInfo.AlwaysPromptForCredentials.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RDPSizingMode", connectionInfo.RDPSizingMode));
            element.Add(new XAttribute("ResolutionWidth", connectionInfo.ResolutionWidth));
            element.Add(new XAttribute("ResolutionHeight", connectionInfo.ResolutionHeight));
            element.Add(new XAttribute("RDPUseMultimon", connectionInfo.RDPUseMultimon.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("HttpPath", connectionInfo.HttpPath ?? string.Empty));
            element.Add(new XAttribute("ExtApp", connectionInfo.ExtApp ?? string.Empty));
            element.Add(new XAttribute("StartProgram", connectionInfo.RDPStartProgram ?? string.Empty));
            element.Add(new XAttribute("StartProgramWorkDir", connectionInfo.RDPStartProgramWorkDir ?? string.Empty));
            element.Add(new XAttribute("VNCCompression", connectionInfo.VNCCompression));
            element.Add(new XAttribute("VNCEncoding", connectionInfo.VNCEncoding));
            element.Add(new XAttribute("VNCAuthMode", connectionInfo.VNCAuthMode));
            element.Add(new XAttribute("VNCProxyType", connectionInfo.VNCProxyType));
            element.Add(new XAttribute("VNCProxyIP", connectionInfo.VNCProxyIP ?? string.Empty));
            element.Add(new XAttribute("VNCProxyPort", connectionInfo.VNCProxyPort));

            element.Add(_saveFilter.SaveUsername
                ? new XAttribute("VNCProxyUsername", connectionInfo.VNCProxyUsername ?? string.Empty)
                : new XAttribute("VNCProxyUsername", ""));

            element.Add(_saveFilter.SavePassword
                ? new XAttribute("VNCProxyPassword", _cryptographyProvider.Encrypt(connectionInfo.VNCProxyPassword ?? string.Empty, _encryptionKey))
                : new XAttribute("VNCProxyPassword", ""));

            element.Add(new XAttribute("VNCColors", connectionInfo.VNCColors));
            element.Add(new XAttribute("VNCSmartSizeMode", connectionInfo.VNCSmartSizeMode));
            element.Add(new XAttribute("VNCViewOnly", connectionInfo.VNCViewOnly.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("VNCClipboardRedirect", connectionInfo.VNCClipboardRedirect.ToString().ToLowerInvariant()));
            element.Add(new XAttribute("RDGatewayUsageMethod", connectionInfo.RDGatewayUsageMethod));
            element.Add(new XAttribute("RDGatewayHostname", connectionInfo.RDGatewayHostname ?? string.Empty));
            element.Add(new XAttribute("RDGatewayUseConnectionCredentials", connectionInfo.RDGatewayUseConnectionCredentials));
            element.Add(new XAttribute("RDGatewayExternalCredentialProvider", connectionInfo.RDGatewayExternalCredentialProvider));
            element.Add(new XAttribute("RDGatewayUserViaAPI", connectionInfo.RDGatewayUserViaAPI ?? string.Empty));
            element.Add(new XAttribute("RDGatewayAccessToken", connectionInfo.RDGatewayAccessToken ?? string.Empty));

            element.Add(_saveFilter.SaveUsername
                ? new XAttribute("RDGatewayUsername", connectionInfo.RDGatewayUsername ?? string.Empty)
                : new XAttribute("RDGatewayUsername", ""));

            element.Add(_saveFilter.SavePassword
                ? new XAttribute("RDGatewayPassword", _cryptographyProvider.Encrypt(connectionInfo.RDGatewayPassword ?? string.Empty, _encryptionKey))
                : new XAttribute("RDGatewayPassword", ""));

            element.Add(_saveFilter.SaveDomain
                ? new XAttribute("RDGatewayDomain", connectionInfo.RDGatewayDomain ?? string.Empty)
                : new XAttribute("RDGatewayDomain", ""));

            element.Add(new XAttribute("UseRCG", connectionInfo.UseRCG));
            element.Add(new XAttribute("UseRestrictedAdmin", connectionInfo.UseRestrictedAdmin));

            element.Add(new XAttribute("UserViaAPI", connectionInfo.UserViaAPI ?? string.Empty));
            element.Add(new XAttribute("EC2InstanceId", connectionInfo.EC2InstanceId ?? string.Empty));
            element.Add(new XAttribute("EC2Region", connectionInfo.EC2Region ?? string.Empty));
            element.Add(new XAttribute("ExternalCredentialProvider", connectionInfo.ExternalCredentialProvider));
            element.Add(new XAttribute("ExternalAddressProvider", connectionInfo.ExternalAddressProvider));

            // Vault/OpenBao specific
            element.Add(new XAttribute("VaultOpenbaoMount", connectionInfo.VaultOpenbaoMount ?? string.Empty));
            element.Add(new XAttribute("VaultOpenbaoRole", connectionInfo.VaultOpenbaoRole ?? string.Empty));
            element.Add(new XAttribute("VaultOpenbaoSecretEngine", connectionInfo.VaultOpenbaoSecretEngine));

            // Credential record link
            if (_saveFilter.SaveCredentialId)
                element.Add(new XAttribute("CredentialId", connectionInfo.CredentialId ?? string.Empty));
        }

        private void SetInheritanceAttributes(XContainer element, IInheritable connectionInfo)
        {
            if (!_saveFilter.SaveInheritance) return;

            ConnectionInfoInheritance inheritance = connectionInfo.Inheritance;

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
            if (inheritance.DisableFullWindowDrag)
                element.Add(new XAttribute("InheritDisableFullWindowDrag", inheritance.DisableFullWindowDrag.ToString().ToLowerInvariant()));
            if (inheritance.DisableMenuAnimations)
                element.Add(new XAttribute("InheritDisableMenuAnimations", inheritance.DisableMenuAnimations.ToString().ToLowerInvariant()));
            if (inheritance.DisableCursorShadow)
                element.Add(new XAttribute("InheritDisableCursorShadow", inheritance.DisableCursorShadow.ToString().ToLowerInvariant()));
            if (inheritance.DisableCursorBlinking)
                element.Add(new XAttribute("InheritDisableCursorBlinking", inheritance.DisableCursorBlinking.ToString().ToLowerInvariant()));
            if (inheritance.Domain)
                element.Add(new XAttribute("InheritDomain", inheritance.Domain.ToString().ToLowerInvariant()));
            if (inheritance.Icon)
                element.Add(new XAttribute("InheritIcon", inheritance.Icon.ToString().ToLowerInvariant()));
            if (inheritance.Panel)
                element.Add(new XAttribute("InheritPanel", inheritance.Panel.ToString().ToLowerInvariant()));
            if (inheritance.Color)
                element.Add(new XAttribute("InheritColor", inheritance.Color.ToString().ToLowerInvariant()));
            if (inheritance.TabColor)
                element.Add(new XAttribute("InheritTabColor", inheritance.TabColor.ToString().ToLowerInvariant()));
            if (inheritance.ConnectionFrameColor)
                element.Add(new XAttribute("InheritConnectionFrameColor", inheritance.ConnectionFrameColor.ToString().ToLowerInvariant()));
            if (inheritance.Password)
                element.Add(new XAttribute("InheritPassword", inheritance.Password.ToString().ToLowerInvariant()));
            if (inheritance.Port)
                element.Add(new XAttribute("InheritPort", inheritance.Port.ToString().ToLowerInvariant()));
            if (inheritance.Protocol)
                element.Add(new XAttribute("InheritProtocol", inheritance.Protocol.ToString().ToLowerInvariant()));
            if (inheritance.RdpVersion)
                element.Add(new XAttribute("InheritRdpVersion", inheritance.RdpVersion.ToString().ToLowerInvariant()));
            if (inheritance.SSHTunnelConnectionName)
                element.Add(new XAttribute("InheritSSHTunnelConnectionName", inheritance.SSHTunnelConnectionName.ToString().ToLowerInvariant()));
            if (inheritance.OpeningCommand)
                element.Add(new XAttribute("InheritOpeningCommand", inheritance.OpeningCommand.ToString().ToLowerInvariant()));
            if (inheritance.SSHOptions)
                element.Add(new XAttribute("InheritSSHOptions", inheritance.SSHOptions.ToString().ToLowerInvariant()));
            if (inheritance.PrivateKeyPath)
                element.Add(new XAttribute("InheritPrivateKeyPath", inheritance.PrivateKeyPath.ToString().ToLowerInvariant()));
            if (inheritance.PuttySession)
                element.Add(new XAttribute("InheritPuttySession", inheritance.PuttySession.ToString().ToLowerInvariant()));
            if (inheritance.RedirectDiskDrives)
                element.Add(new XAttribute("InheritRedirectDiskDrives", inheritance.RedirectDiskDrives.ToString().ToLowerInvariant()));
            if (inheritance.RedirectDiskDrivesCustom)
                element.Add(new XAttribute("InheritRedirectDiskDrivesCustom", inheritance.RedirectDiskDrivesCustom.ToString().ToLowerInvariant()));
            if (inheritance.RedirectKeys)
                element.Add(new XAttribute("InheritRedirectKeys", inheritance.RedirectKeys.ToString().ToLowerInvariant()));
            if (inheritance.RedirectPorts)
                element.Add(new XAttribute("InheritRedirectPorts", inheritance.RedirectPorts.ToString().ToLowerInvariant()));
            if (inheritance.RedirectPrinters)
                element.Add(new XAttribute("InheritRedirectPrinters", inheritance.RedirectPrinters.ToString().ToLowerInvariant()));
            if (inheritance.RedirectClipboard)
                element.Add(new XAttribute("InheritRedirectClipboard", inheritance.RedirectClipboard.ToString().ToLowerInvariant()));
            if (inheritance.RedirectSmartCards)
                element.Add(new XAttribute("InheritRedirectSmartCards", inheritance.RedirectSmartCards.ToString().ToLowerInvariant()));
            if (inheritance.RedirectSound)
                element.Add(new XAttribute("InheritRedirectSound", inheritance.RedirectSound.ToString().ToLowerInvariant()));
            if (inheritance.SoundQuality)
                element.Add(new XAttribute("InheritSoundQuality", inheritance.SoundQuality.ToString().ToLowerInvariant()));
            if (inheritance.RedirectAudioCapture)
                element.Add(new XAttribute("InheritRedirectAudioCapture", inheritance.RedirectAudioCapture.ToString().ToLowerInvariant()));
            if (inheritance.Resolution)
                element.Add(new XAttribute("InheritResolution", inheritance.Resolution.ToString().ToLowerInvariant()));
            if (inheritance.DesktopScaleFactor)
                element.Add(new XAttribute("InheritDesktopScaleFactor", inheritance.DesktopScaleFactor.ToString().ToLowerInvariant()));
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
            if (inheritance.IPAddress)
                element.Add(new XAttribute("InheritIPAddress", inheritance.IPAddress.ToString().ToLowerInvariant()));
            if (inheritance.ConnectionAddressPrimary)
                element.Add(new XAttribute("InheritConnectionAddressPrimary", inheritance.ConnectionAddressPrimary.ToString().ToLowerInvariant()));
            if (inheritance.RDPSignScope)
                element.Add(new XAttribute("InheritRDPSignScope", inheritance.RDPSignScope.ToString().ToLowerInvariant()));
            if (inheritance.RDPSignature)
                element.Add(new XAttribute("InheritRDPSignature", inheritance.RDPSignature.ToString().ToLowerInvariant()));
            if (inheritance.RDPSizingMode)
                element.Add(new XAttribute("InheritRDPSizingMode", inheritance.RDPSizingMode.ToString().ToLowerInvariant()));
            if (inheritance.ResolutionWidth)
                element.Add(new XAttribute("InheritResolutionWidth", inheritance.ResolutionWidth.ToString().ToLowerInvariant()));
            if (inheritance.ResolutionHeight)
                element.Add(new XAttribute("InheritResolutionHeight", inheritance.ResolutionHeight.ToString().ToLowerInvariant()));
            if (inheritance.RDPUseMultimon)
                element.Add(new XAttribute("InheritRDPUseMultimon", inheritance.RDPUseMultimon.ToString().ToLowerInvariant()));
            if (inheritance.Notes)
                element.Add(new XAttribute("InheritNotes", inheritance.Notes.ToString().ToLowerInvariant()));
            if (inheritance.PreExtApp)
                element.Add(new XAttribute("InheritPreExtApp", inheritance.PreExtApp.ToString().ToLowerInvariant()));
            if (inheritance.PostExtApp)
                element.Add(new XAttribute("InheritPostExtApp", inheritance.PostExtApp.ToString().ToLowerInvariant()));
            if (inheritance.MacAddress)
                element.Add(new XAttribute("InheritMacAddress", inheritance.MacAddress.ToString().ToLowerInvariant()));
            if (inheritance.Hostname)
                element.Add(new XAttribute("InheritHostname", inheritance.Hostname.ToString().ToLowerInvariant()));
            if (inheritance.AlternativeAddress)
                element.Add(new XAttribute("InheritAlternativeAddress", inheritance.AlternativeAddress.ToString().ToLowerInvariant()));
            if (inheritance.UserField)
                element.Add(new XAttribute("InheritUserField", inheritance.UserField.ToString().ToLowerInvariant()));
            if (inheritance.UserField1)
                element.Add(new XAttribute("InheritUserField1", inheritance.UserField1.ToString().ToLowerInvariant()));
            if (inheritance.UserField2)
                element.Add(new XAttribute("InheritUserField2", inheritance.UserField2.ToString().ToLowerInvariant()));
            if (inheritance.UserField3)
                element.Add(new XAttribute("InheritUserField3", inheritance.UserField3.ToString().ToLowerInvariant()));
            if (inheritance.UserField4)
                element.Add(new XAttribute("InheritUserField4", inheritance.UserField4.ToString().ToLowerInvariant()));
            if (inheritance.UserField5)
                element.Add(new XAttribute("InheritUserField5", inheritance.UserField5.ToString().ToLowerInvariant()));
            if (inheritance.UserField6)
                element.Add(new XAttribute("InheritUserField6", inheritance.UserField6.ToString().ToLowerInvariant()));
            if (inheritance.UserField7)
                element.Add(new XAttribute("InheritUserField7", inheritance.UserField7.ToString().ToLowerInvariant()));
            if (inheritance.UserField8)
                element.Add(new XAttribute("InheritUserField8", inheritance.UserField8.ToString().ToLowerInvariant()));
            if (inheritance.UserField9)
                element.Add(new XAttribute("InheritUserField9", inheritance.UserField9.ToString().ToLowerInvariant()));
            if (inheritance.UserField10)
                element.Add(new XAttribute("InheritUserField10", inheritance.UserField10.ToString().ToLowerInvariant()));
            if (inheritance.EnvironmentTags)
                element.Add(new XAttribute("InheritEnvironmentTags", inheritance.EnvironmentTags.ToString().ToLowerInvariant()));
            if (inheritance.Favorite)
                element.Add(new XAttribute("InheritFavorite", inheritance.Favorite.ToString().ToLowerInvariant()));
            if (inheritance.RetryOnFirstConnect)
                element.Add(new XAttribute("InheritRetryOnFirstConnect", inheritance.RetryOnFirstConnect.ToString().ToLowerInvariant()));
            if (inheritance.WaitForIPAvailability)
                element.Add(new XAttribute("InheritWaitForIPAvailability", inheritance.WaitForIPAvailability.ToString().ToLowerInvariant()));
            if (inheritance.WaitForIPTimeout)
                element.Add(new XAttribute("InheritWaitForIPTimeout", inheritance.WaitForIPTimeout.ToString().ToLowerInvariant()));
            if (inheritance.AutoSort)
                element.Add(new XAttribute("InheritAutoSort", inheritance.AutoSort.ToString().ToLowerInvariant()));
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
            if (inheritance.VNCClipboardRedirect)
                element.Add(new XAttribute("InheritVNCClipboardRedirect", inheritance.VNCClipboardRedirect.ToString().ToLowerInvariant()));
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
            if (inheritance.RDGatewayExternalCredentialProvider)
                element.Add(new XAttribute("InheritRDGatewayExternalCredentialProvider", inheritance.RDGatewayExternalCredentialProvider.ToString().ToLowerInvariant()));
            if (inheritance.RDGatewayUserViaAPI)
                element.Add(new XAttribute("InheritRDGatewayUserViaAPI", inheritance.RDGatewayUserViaAPI.ToString().ToLowerInvariant()));

            if (inheritance.VmId)
                element.Add(new XAttribute("InheritVmId", inheritance.VmId.ToString().ToLowerInvariant()));
            if (inheritance.UseVmId)
                element.Add(new XAttribute("InheritUseVmId", inheritance.UseVmId.ToString().ToLowerInvariant()));
            if (inheritance.UseEnhancedMode)
                element.Add(new XAttribute("InheritUseEnhancedMode", inheritance.UseEnhancedMode.ToString().ToLowerInvariant()));
            if (inheritance.ExternalCredentialProvider)
                element.Add(new XAttribute("InheritExternalCredentialProvider", inheritance.ExternalCredentialProvider.ToString().ToLowerInvariant()));
            if (inheritance.UserViaAPI)
                element.Add(new XAttribute("InheritUserViaAPI", inheritance.UserViaAPI.ToString().ToLowerInvariant()));
            if (inheritance.UseRCG)
                element.Add(new XAttribute("InheritUseRCG", inheritance.UseRCG.ToString().ToLowerInvariant()));
            if (inheritance.UseRestrictedAdmin)
                element.Add(new XAttribute("InheritUseRestrictedAdmin", inheritance.UseRestrictedAdmin.ToString().ToLowerInvariant()));
            if (inheritance.ScriptErrorsSuppressed)
                element.Add(new XAttribute("InheritScriptErrorsSuppressed", inheritance.ScriptErrorsSuppressed.ToString().ToLowerInvariant()));
        }
    }
}