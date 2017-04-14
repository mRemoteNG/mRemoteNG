using System.Collections;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class XmlConnectionNodeSerializer27Tests
    {
        private XmlConnectionNodeSerializer27 _connectionNodeSerializer;
        private ICryptographyProvider _cryptographyProvider;

        [SetUp]
        public void Setup()
        {
            _cryptographyProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(
                BlockCipherEngines.AES, BlockCipherModes.GCM);
            _connectionNodeSerializer = new XmlConnectionNodeSerializer27(_cryptographyProvider, "myPassword1".ConvertToSecureString(), new SaveFilter());
        }

        [Test]
        public void ReturnsXElement()
        {
            var returnVal = _connectionNodeSerializer.Serialize(new ConnectionInfo());
            Assert.That(returnVal, Is.TypeOf<XElement>());
        }

        [TestCaseSource(typeof(TestCaseDataSource), nameof(TestCaseDataSource.AttributesAndExpectedValues))]
        public void XmlElementContainsSerializedAttribute(string attributeName, string expectedValue, ConnectionInfo connectionInfo)
        {
            var returnVal = _connectionNodeSerializer.Serialize(connectionInfo);
            var targetAttribute = returnVal.Attribute(XName.Get(attributeName));
            Assert.That(targetAttribute?.Value, Is.EqualTo(expectedValue));
        }

        [TestCaseSource(typeof(TestCaseDataSource), nameof(TestCaseDataSource.PasswordAttributes))]
        public void PasswordFieldsAreSerialized(string attributeName, ConnectionInfo connectionInfo)
        {
            var returnVal = _connectionNodeSerializer.Serialize(connectionInfo);
            var targetAttribute = returnVal.Attribute(XName.Get(attributeName));
            Assert.That(targetAttribute?.Value, Is.Not.Empty);
        }

        [TestCaseSource(typeof(TestCaseDataSource), nameof(TestCaseDataSource.SaveFilterTests))]
        public void AttributesNotSerializedWhenFiltered(string attributeName, ConnectionInfo connectionInfo)
        {
            var saveFilter = new SaveFilter(true);
            var cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.AES, BlockCipherModes.GCM);
            _connectionNodeSerializer = new XmlConnectionNodeSerializer27(cryptoProvider, "myPassword1".ConvertToSecureString(), saveFilter);
            var returnVal = _connectionNodeSerializer.Serialize(connectionInfo);
            var targetAttribute = returnVal.Attribute(XName.Get(attributeName));
            Assert.That(targetAttribute?.Value, Is.EqualTo(string.Empty));
        }

        [TestCaseSource(typeof(TestCaseDataSource), nameof(TestCaseDataSource.InheritanceFilterTests))]
        public void InheritanceNotSerialiedWhenFiltered(string attributeName, ConnectionInfo connectionInfo)
        {
            var saveFilter = new SaveFilter(true);
            var cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.AES, BlockCipherModes.GCM);
            _connectionNodeSerializer = new XmlConnectionNodeSerializer27(cryptoProvider, "myPassword1".ConvertToSecureString(), saveFilter);
            var returnVal = _connectionNodeSerializer.Serialize(connectionInfo);
            var targetAttribute = returnVal.Attribute(XName.Get(attributeName));
            Assert.That(targetAttribute?.Value, Is.EqualTo(false.ToString()));
        }

        private class TestCaseDataSource
        {
            private static readonly ConnectionInfo ConnectionInfo = new ConnectionInfo
            {
                Name = "HeresACustomName",
                Description = "myDescription",
                Username = "myuser",
                Domain = "superdomain",
                Password = "pass",
                CredentialRecord = Substitute.For<ICredentialRecord>(),
                Hostname = "somehost",
                ExtApp = "myextapp",
                PreExtApp = "preext1",
                PostExtApp = "postext1",
                MacAddress = "asdf-asdf-asdf-asdf23423",
                LoadBalanceInfo = "loadbalanceinfohere",
                RDGatewayUsername = "gatewayuser",
                RDGatewayDomain = "somegatewaydomain",
                RDGatewayHostname = "somegatewayhost",
                RDGatewayPassword = "gatewaypass",
                UserField = "userfield data here",
                VNCProxyIP = "192.168.1.1",
                VNCProxyUsername = "vncproxyuser",
                VNCProxyPassword = "vncproxypass",
            };

            private static readonly ConnectionInfo ConnectionInfoWithInheritance = new ConnectionInfo
            {
                Inheritance = {EverythingInherited = true}
            };

            private static readonly ContainerInfo ContainerInfo = new ContainerInfo();

            public static IEnumerable SaveFilterTests
            {
                get
                {
                    yield return new TestCaseData("RDGatewayUsername", ConnectionInfo);
                    yield return new TestCaseData("RDGatewayDomain", ConnectionInfo);
                    yield return new TestCaseData("RDGatewayPassword", ConnectionInfo);
                    yield return new TestCaseData("VNCProxyUsername", ConnectionInfo);
                    yield return new TestCaseData("VNCProxyPassword", ConnectionInfo);
                    yield return new TestCaseData("CredentialId", ConnectionInfo);
                }
            }

            public static IEnumerable InheritanceFilterTests
            {
                get
                {
                    yield return new TestCaseData("InheritRDGatewayUsername", ConnectionInfoWithInheritance);
                    yield return new TestCaseData("InheritRDGatewayDomain", ConnectionInfoWithInheritance);
                    yield return new TestCaseData("InheritRDGatewayPassword", ConnectionInfoWithInheritance);
                    yield return new TestCaseData("InheritVNCProxyUsername", ConnectionInfoWithInheritance);
                    yield return new TestCaseData("InheritVNCProxyPassword", ConnectionInfoWithInheritance);
                }
            }

            public static IEnumerable PasswordAttributes
            {
                get
                {
                    yield return new TestCaseData("VNCProxyPassword", ConnectionInfo);
                    yield return new TestCaseData("RDGatewayPassword", ConnectionInfo);
                }
            }

            public static IEnumerable AttributesAndExpectedValues
            {
                get
                {
                    yield return new TestCaseData("Name", ConnectionInfo.Name, ConnectionInfo);
                    yield return new TestCaseData("Type", "Connection", ConnectionInfo);
                    yield return new TestCaseData("Type", "Container", ContainerInfo);
                    yield return new TestCaseData("Expanded", ContainerInfo.IsExpanded.ToString(), ContainerInfo);
                    yield return new TestCaseData("Descr", ConnectionInfo.Description, ConnectionInfo);
                    yield return new TestCaseData("Icon", ConnectionInfo.Icon, ConnectionInfo);
                    yield return new TestCaseData("Panel", "General", ConnectionInfo);
                    yield return new TestCaseData("Hostname", ConnectionInfo.Hostname, ConnectionInfo);
                    yield return new TestCaseData("Protocol", "RDP", ConnectionInfo);
                    yield return new TestCaseData("PuttySession", ConnectionInfo.PuttySession, ConnectionInfo);
                    yield return new TestCaseData("Port", ConnectionInfo.Port.ToString(), ConnectionInfo);
                    yield return new TestCaseData("ConnectToConsole", ConnectionInfo.UseConsoleSession.ToString(), ConnectionInfo);
                    yield return new TestCaseData("UseCredSsp", ConnectionInfo.UseCredSsp.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RenderingEngine", ConnectionInfo.RenderingEngine.ToString(), ConnectionInfo);
                    yield return new TestCaseData("ICAEncryptionStrength", ConnectionInfo.ICAEncryptionStrength.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RDPAuthenticationLevel", ConnectionInfo.RDPAuthenticationLevel.ToString(), ConnectionInfo);
                    yield return new TestCaseData("LoadBalanceInfo", ConnectionInfo.LoadBalanceInfo, ConnectionInfo);
                    yield return new TestCaseData("Colors", ConnectionInfo.Colors.ToString(), ConnectionInfo);
                    yield return new TestCaseData("Resolution", ConnectionInfo.Resolution.ToString(), ConnectionInfo);
                    yield return new TestCaseData("AutomaticResize", ConnectionInfo.AutomaticResize.ToString(), ConnectionInfo);
                    yield return new TestCaseData("DisplayWallpaper", ConnectionInfo.DisplayWallpaper.ToString(), ConnectionInfo);
                    yield return new TestCaseData("DisplayThemes", ConnectionInfo.DisplayThemes.ToString(), ConnectionInfo);
                    yield return new TestCaseData("EnableFontSmoothing", ConnectionInfo.EnableFontSmoothing.ToString(), ConnectionInfo);
                    yield return new TestCaseData("EnableDesktopComposition", ConnectionInfo.EnableDesktopComposition.ToString(), ConnectionInfo);
                    yield return new TestCaseData("CacheBitmaps", ConnectionInfo.CacheBitmaps.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RedirectDiskDrives", ConnectionInfo.RedirectDiskDrives.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RedirectPorts", ConnectionInfo.RedirectPorts.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RedirectPrinters", ConnectionInfo.RedirectPrinters.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RedirectSmartCards", ConnectionInfo.RedirectSmartCards.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RedirectSound", ConnectionInfo.RedirectSound.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RedirectKeys", ConnectionInfo.RedirectKeys.ToString(), ConnectionInfo);
                    yield return new TestCaseData("Connected", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("PreExtApp", ConnectionInfo.PreExtApp, ConnectionInfo);
                    yield return new TestCaseData("PostExtApp", ConnectionInfo.PostExtApp, ConnectionInfo);
                    yield return new TestCaseData("MacAddress", ConnectionInfo.MacAddress, ConnectionInfo);
                    yield return new TestCaseData("UserField", ConnectionInfo.UserField, ConnectionInfo);
                    yield return new TestCaseData("ExtApp", ConnectionInfo.ExtApp, ConnectionInfo);
                    yield return new TestCaseData("VNCCompression", ConnectionInfo.VNCCompression.ToString(), ConnectionInfo);
                    yield return new TestCaseData("VNCEncoding", ConnectionInfo.VNCEncoding.ToString(), ConnectionInfo);
                    yield return new TestCaseData("VNCAuthMode", ConnectionInfo.VNCAuthMode.ToString(), ConnectionInfo);
                    yield return new TestCaseData("VNCProxyType", ConnectionInfo.VNCProxyType.ToString(), ConnectionInfo);
                    yield return new TestCaseData("VNCProxyIP", ConnectionInfo.VNCProxyIP, ConnectionInfo);
                    yield return new TestCaseData("VNCProxyPort", ConnectionInfo.VNCProxyPort.ToString(), ConnectionInfo);
                    yield return new TestCaseData("VNCProxyUsername", ConnectionInfo.VNCProxyUsername, ConnectionInfo);
                    yield return new TestCaseData("VNCColors", ConnectionInfo.VNCColors.ToString(), ConnectionInfo);
                    yield return new TestCaseData("VNCSmartSizeMode", ConnectionInfo.VNCSmartSizeMode.ToString(), ConnectionInfo);
                    yield return new TestCaseData("VNCViewOnly", ConnectionInfo.VNCViewOnly.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RDGatewayUsageMethod", ConnectionInfo.RDGatewayUsageMethod.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RDGatewayHostname", ConnectionInfo.RDGatewayHostname, ConnectionInfo);
                    yield return new TestCaseData("RDGatewayUseConnectionCredentials", ConnectionInfo.RDGatewayUseConnectionCredentials.ToString(), ConnectionInfo);
                    yield return new TestCaseData("RDGatewayUsername", ConnectionInfo.RDGatewayUsername, ConnectionInfo);
                    yield return new TestCaseData("RDGatewayDomain", ConnectionInfo.RDGatewayDomain, ConnectionInfo);
                    yield return new TestCaseData("InheritCacheBitmaps", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritColors", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritDescription", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritDisplayThemes", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritDisplayWallpaper", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritEnableFontSmoothing", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritEnableDesktopComposition", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritIcon", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritPanel", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritPort", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritProtocol", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritPuttySession", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRedirectDiskDrives", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRedirectKeys", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRedirectPorts", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRedirectPrinters", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRedirectSmartCards", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRedirectSound", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritSoundQuality", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritResolution", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritAutomaticResize", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritUseConsoleSession", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritUseCredSsp", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRenderingEngine", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritICAEncryptionStrength", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRDPAuthenticationLevel", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritLoadBalanceInfo", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritPreExtApp", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritPostExtApp", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritMacAddress", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritUserField", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritExtApp", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCCompression", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCEncoding", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCAuthMode", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCProxyType", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCProxyIP", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCProxyPort", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCProxyUsername", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCProxyPassword", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCColors", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCSmartSizeMode", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritVNCViewOnly", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRDGatewayUsageMethod", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRDGatewayHostname", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRDGatewayUseConnectionCredentials", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRDGatewayUsername", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRDGatewayPassword", false.ToString(), ConnectionInfo);
                    yield return new TestCaseData("InheritRDGatewayDomain", false.ToString(), ConnectionInfo);
                }
            }
        }
    }
}