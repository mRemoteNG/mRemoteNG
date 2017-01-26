using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
    public class AbstractConnectionInfoDataTests
    {
#pragma warning disable 618
        private class TestAbstractConnectionInfoData : AbstractLegacyConnectionRecord {}
#pragma warning restore 618
        private TestAbstractConnectionInfoData _testAbstractConnectionInfoData;

        [SetUp]
        public void Setup()
        {
            _testAbstractConnectionInfoData = new TestAbstractConnectionInfoData();
        }

        [TearDown]
        public void Teardown()
        {
            _testAbstractConnectionInfoData = null;
        }


        [Test]
        public void NameNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Name = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void DescriptionNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Description = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void IconNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Icon = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void PanelNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Panel = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void HostnameNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Hostname = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void UsernameNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Username = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void PasswordNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Password = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void DomainNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Domain = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void ProtocolNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Protocol = ProtocolType.HTTP;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void ExtAppNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.ExtApp = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void PortNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Port = 9999;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void PuttySessionNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.PuttySession = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void IcaEncryptionNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.ICAEncryptionStrength = ProtocolICA.EncryptionStrength.Encr128BitLogonOnly;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void UseConsoleSessionNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.UseConsoleSession = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RdpAuthenticationLevelNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RDPAuthenticationLevel = ProtocolRDP.AuthenticationLevel.AuthRequired;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void LoadBalanceInfoNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.LoadBalanceInfo = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RenderingEngineNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RenderingEngine = HTTPBase.RenderingEngine.Gecko;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void UseCredSspNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.UseCredSsp = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RdGatewayUsageMethodNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RDGatewayUsageMethod = ProtocolRDP.RDGatewayUsageMethod.Always;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RdGatewayHostnameNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RDGatewayHostname = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RdGatewayUseConnectionCredentialsNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RDGatewayUseConnectionCredentials = ProtocolRDP.RDGatewayUseConnectionCredentials.SmartCard;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RdGatewayUsernameNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RDGatewayUsername = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RdGatewayPasswordNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RDGatewayPassword = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RdGatewayDomainNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RDGatewayDomain = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void ResolutionNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Resolution = ProtocolRDP.RDPResolutions.Res1366x768;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void AutomaticResizeNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.AutomaticResize = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void ColorsNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.Colors = ProtocolRDP.RDPColors.Colors16Bit;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void CacheBitmapsNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.CacheBitmaps = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void DisplayWallpaperNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.DisplayWallpaper = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void DisplayThemesNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.DisplayThemes = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void EnableFontSmoothingNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.EnableFontSmoothing = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void EnableDesktopCompositionNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.EnableDesktopComposition = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RedirectKeysNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RedirectKeys = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RedirectDiskDrivesNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RedirectDiskDrives = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RedirectPrintersNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RedirectPrinters = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RedirectPortsNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RedirectPorts = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RedirectSmartCardsNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RedirectSmartCards = true;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void RedirectSoundNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.RedirectSound = ProtocolRDP.RDPSounds.DoNotPlay;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void PreExtAppNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.PreExtApp = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void PostExtAppNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.PostExtApp = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void MacAddressNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.MacAddress = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void UserFieldNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.UserField = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncCompressionNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCCompression = ProtocolVNC.Compression.Comp5;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncEncodingNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCEncoding = ProtocolVNC.Encoding.EncTight;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncAuthModeNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCAuthMode = ProtocolVNC.AuthMode.AuthWin;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncProxyTypeNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCProxyType = ProtocolVNC.ProxyType.ProxyUltra;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncProxyIpNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCProxyIP = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncProxyPortNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCProxyPort = 9999;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncProxyUsernameNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCProxyUsername = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncProxyPasswordNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCProxyPassword = "a";
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncColorsNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCColors = ProtocolVNC.Colors.Col8Bit;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncSmartSizeModeNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCSmartSizeMode = ProtocolVNC.SmartSizeMode.SmartSFree;
            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void VncViewOnlyNotifiesOnValueChange()
        {
            var wasCalled = false;
            _testAbstractConnectionInfoData.PropertyChanged += (sender, args) => wasCalled = true;
            _testAbstractConnectionInfoData.VNCViewOnly = true;
            Assert.That(wasCalled, Is.True);
        }
    }
}