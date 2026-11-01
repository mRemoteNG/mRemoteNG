using System.Collections;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Csv;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.Terminal;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNGTests.TestHelpers;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Csv
{
    public class CsvConnectionsDeserializerMremotengFormatTests
    {
        private CsvConnectionsDeserializerMremotengFormat _deserializer;
        private CsvConnectionsSerializerMremotengFormat _serializer;

        [SetUp]
        public void Setup()
        {
            _deserializer = new CsvConnectionsDeserializerMremotengFormat();
            var credentialRepositoryList = Substitute.For<ICredentialRepositoryList>();
            _serializer = new CsvConnectionsSerializerMremotengFormat(new SaveFilter(), credentialRepositoryList);
        }

        [TestCaseSource(typeof(DeserializationTestSource), nameof(DeserializationTestSource.ConnectionPropertyTestCases))]
        public object ConnectionPropertiesDeserializedCorrectly(string propertyToCheck)
        {
            var csv = _serializer.Serialize(GetTestConnection());
            var deserializedConnections = _deserializer.Deserialize(csv);
            var connection = deserializedConnections.GetRecursiveChildList().FirstOrDefault();
            var propertyValue = typeof(ConnectionInfo).GetProperty(propertyToCheck)?.GetValue(connection);
            return propertyValue;
        }

        [TestCaseSource(typeof(DeserializationTestSource), nameof(DeserializationTestSource.InheritanceTestCases))]
        public object InheritancePropertiesDeserializedCorrectly(string propertyToCheck)
        {
            var csv = _serializer.Serialize(GetTestConnectionWithAllInherited());
            var deserializedConnections = _deserializer.Deserialize(csv);
            var connection = deserializedConnections.GetRecursiveChildList().FirstOrDefault();
            connection?.RemoveParent();
            var propertyValue = typeof(ConnectionInfoInheritance).GetProperty(propertyToCheck)?.GetValue(connection?.Inheritance);
            return propertyValue;
        }

        [Test]
        public void ColoursSurviveAnExportAndAnImport()
        {
            var original = GetTestConnection();
            original.Color = "#336699";
            original.TabColor = "#AA5500";
            original.ConnectionFrameColor = ConnectionFrameColor.Purple;

            var connection = _deserializer.Deserialize(_serializer.Serialize(original)).GetRecursiveChildList().First();

            Assert.That(connection.Color, Is.EqualTo("#336699"));
            Assert.That(connection.TabColor, Is.EqualTo("#AA5500"));
            Assert.That(connection.ConnectionFrameColor, Is.EqualTo(ConnectionFrameColor.Purple));
        }

        [Test]
        public void FileFromAnExportWithTheMergedRedirectHeaderIsReadInPlace()
        {
            var original = GetTestConnection();
            var csv = _serializer.Serialize(original);
            var olderExport = csv.Replace("RedirectDiskDrivesCustom;RedirectPorts;", "RedirectDiskDrivesCustomRedirectPorts;");
            Assume.That(olderExport, Is.Not.EqualTo(csv));

            var connection = _deserializer.Deserialize(olderExport).GetRecursiveChildList().First();

            Assert.That(connection.RedirectPorts, Is.EqualTo(original.RedirectPorts));
            Assert.That(connection.RedirectSound, Is.EqualTo(original.RedirectSound));
            Assert.That(connection.VNCProxyPort, Is.EqualTo(original.VNCProxyPort));
            Assert.That(connection.EC2Region, Is.EqualTo(original.EC2Region));
        }

        [TestCase("pass;word")]
        [TestCase("say \"hi\"")]
        [TestCase("two\r\nlines")]
        [TestCase("\"starts with a quote")]
        public void ValuesWithSeparatorsQuotesAndLineBreaksSurviveExportAndImport(string value)
        {
            var original = GetTestConnection();
            original.Description = value;
            original.Password = value;
            var csv = _serializer.Serialize(original);

            var connection = _deserializer.Deserialize(csv).GetRecursiveChildList().First();

            Assert.That(connection.Description, Is.EqualTo(value));
            Assert.That(connection.Password, Is.EqualTo(value));
            Assert.That(connection.Hostname, Is.EqualTo(original.Hostname));
            Assert.That(connection.EC2Region, Is.EqualTo(original.EC2Region));
        }

        [Test]
        public void HandWrittenFileWithAQuotedPasswordIsImported()
        {
            var csv = "Name;Id;Parent;NodeType;Password;Hostname\r\n" +
                      "SampleVM;;;Connection;\"pa;ss\"\"word\";10.100.10.111\r\n";

            var connection = _deserializer.Deserialize(csv).GetRecursiveChildList().First();

            Assert.That(connection.Password, Is.EqualTo("pa;ss\"word"));
            Assert.That(connection.Hostname, Is.EqualTo("10.100.10.111"));
        }

        [Test]
        public void TreeStructureDeserializedCorrectly()
        {
            //Root
            // |- folder1
            // |   |- Con1
            // |- Con2
            var treeModel = new ConnectionTreeModelBuilder().Build();
            var csv = _serializer.Serialize(treeModel);
            var deserializedConnections = _deserializer.Deserialize(csv);
            var con1 = deserializedConnections.GetRecursiveChildList().First(info => info.Name == "Con1");
            var folder1 = deserializedConnections.GetRecursiveChildList().First(info => info.Name == "folder1");
            Assert.That(con1.Parent, Is.EqualTo(folder1));
        }

        private static ConnectionInfo GetTestConnection()
        {
            return new ConnectionInfo
            {
                Name = "SomeName",
                Description = "SomeDescription",
                Icon = "SomeIcon",
                Panel = "SomePanel",
                Username = "SomeUsername",
                //Password = "SomePassword".ConvertToSecureString(),
                Password = "SomePassword",
                Domain = "SomeDomain",
                Hostname = "SomeHostname",
                PuttySession = "SomePuttySession",
                LoadBalanceInfo = "SomeLoadBalanceInfo",
                OpeningCommand = "SomeOpeningCommand",
                PreExtApp = "SomePreExtApp",
                PostExtApp = "SomePostExtApp",
                MacAddress = "SomeMacAddress",
                UserField = "SomeUserField",
                VmId = "SomeVmId",
                ExtApp = "SomeExtApp",
                VNCProxyUsername = "SomeVNCProxyUsername",
                VNCProxyPassword = "SomeVNCProxyPassword",
                RDGatewayUsername = "SomeRDGatewayUsername",
                RDGatewayPassword = "SomeRDGatewayPassword",
                RDGatewayDomain = "SomeRDGatewayDomain",
                VNCProxyIP = "SomeVNCProxyIP",
                RDGatewayHostname = "SomeRDGatewayHostname",
                RDGatewayExternalCredentialProvider = ExternalCredentialProvider.None,
                RDGatewayUserViaAPI = "123",
                Protocol = ProtocolType.RDP,
                Port = 999,
                Favorite = true,
                UseConsoleSession = true,
                UseCredSsp = true,
                UseRestrictedAdmin = true,
                UseRCG = true,
                UseVmId = false,
                UseEnhancedMode = false,
                RenderingEngine = HTTPBase.RenderingEngine.EdgeChromium,
                TerminalBackend = TerminalBackend.WindowsTerminal,
                RDPAuthenticationLevel = AuthenticationLevel.WarnOnFailedAuth,
                Colors = RDPColors.Colors16Bit,
                Resolution = RDPResolutions.SmartSize,
                AutomaticResize = true,
                DisplayWallpaper = true,
                DisplayThemes = true,
                EnableFontSmoothing = true,
                EnableDesktopComposition = true,
                DisableFullWindowDrag = false,
                DisableMenuAnimations = false,
                DisableCursorShadow = false,
                DisableCursorBlinking = false,
                CacheBitmaps = true,
                RedirectDiskDrives = RDPDiskDrives.None,
                RedirectDiskDrivesCustom = "",
                RedirectPorts = true,
                RedirectPrinters = true,
                RedirectSmartCards = true,
                RedirectSound = RDPSounds.LeaveAtRemoteComputer,
                RedirectAudioCapture = true,
                RedirectKeys = true,
                VNCCompression = ProtocolVNC.Compression.Comp4,
                VNCEncoding = ProtocolVNC.Encoding.EncRRE,
                VNCAuthMode = ProtocolVNC.AuthMode.AuthVNC,
                VNCProxyType = ProtocolVNC.ProxyType.ProxySocks5,
                VNCProxyPort = 123,
                VNCColors = ProtocolVNC.Colors.Col8Bit,
                VNCSmartSizeMode = ProtocolVNC.SmartSizeMode.SmartSAspect,
                VNCViewOnly = true,
                RDGatewayUsageMethod = RDGatewayUsageMethod.Detect,
                RDGatewayUseConnectionCredentials = RDGatewayUseConnectionCredentials.SmartCard,
                UserViaAPI = "",
                EC2InstanceId = "",
                EC2Region = "eu-central-1",
                ExternalAddressProvider = ExternalAddressProvider.None,
                ExternalCredentialProvider = ExternalCredentialProvider.None
            };
        }

        private static ConnectionInfo GetTestConnectionWithAllInherited()
        {
            var connectionInfo = new ConnectionInfo();
            connectionInfo.Inheritance.TurnOnInheritanceCompletely();
            return connectionInfo;
        }

        private class DeserializationTestSource
        {
            public static IEnumerable ConnectionPropertyTestCases()
            {
                var ignoreProperties = new[]
                {
                    nameof(ConnectionInfo.Inheritance),
                    nameof(ConnectionInfo.ConstantID),
                    nameof(ConnectionInfo.Parent)
                };
                var properties = typeof(ConnectionInfo)
                    .GetProperties()
                    .Where(property => !ignoreProperties.Contains(property.Name));
                var testCases = new List<TestCaseData>();
                var testConnectionInfo = GetTestConnection();

                foreach (var property in properties)
                {
                    if (property.Name == "Password")
                        continue;

                    testCases.Add(
                        new TestCaseData(property.Name)
                        .Returns(property.GetValue(testConnectionInfo)));
                }

                return testCases;
            }

            public static IEnumerable InheritanceTestCases()
            {
                var ignoreProperties = new[]
                {
                    nameof(ConnectionInfoInheritance.EverythingInherited),
                    nameof(ConnectionInfoInheritance.Parent),
                    nameof(ConnectionInfoInheritance.EverythingInherited)
                };
                var properties = typeof(ConnectionInfoInheritance)
                    .GetProperties()
                    .Where(property => !ignoreProperties.Contains(property.Name));
                var testCases = new List<TestCaseData>();
                var testInheritance = GetTestConnectionWithAllInherited().Inheritance;

                return properties
                    .Select(property =>
                        new TestCaseData(property.Name)
                            .Returns(property.GetValue(testInheritance)))
                    .ToList();
            }
        }
    }
}