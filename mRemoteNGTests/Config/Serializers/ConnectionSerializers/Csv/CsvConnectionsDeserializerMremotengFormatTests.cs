using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Csv;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
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
        public void MinimalCsvWithFewColumns_DeserializesCorrectly()
        {
            // A user-created CSV with only Name, Hostname, Protocol columns
            // (much fewer than the full mRemoteNG format) should import cleanly.
            const string csv = "Name;Hostname;Protocol\r\nMyServer;192.168.1.1;RDP";
            var tree = _deserializer.Deserialize(csv);
            var connection = tree.GetRecursiveChildList().FirstOrDefault();
            Assert.That(connection, Is.Not.Null);
            Assert.That(connection!.Name, Is.EqualTo("MyServer"));
            Assert.That(connection.Hostname, Is.EqualTo("192.168.1.1"));
        }

        [Test]
        public void CsvWithShortDataRow_DoesNotThrow()
        {
            // A data row with fewer columns than the header must not throw
            // IndexOutOfRangeException — missing fields should default to empty.
            const string csv = "Name;Hostname;Protocol;Port\r\nMyServer;192.168.1.1";
            Assert.That(() => _deserializer.Deserialize(csv), Throws.Nothing);
            var tree = _deserializer.Deserialize(csv);
            var connection = tree.GetRecursiveChildList().FirstOrDefault();
            Assert.That(connection, Is.Not.Null);
            Assert.That(connection!.Name, Is.EqualTo("MyServer"));
        }

        [Test]
        public void EmptyCsvContent_ReturnsEmptyTree()
        {
            var tree = _deserializer.Deserialize(string.Empty);
            Assert.That(tree.GetRecursiveChildList(), Is.Empty);
        }

        [Test]
        public void TreeStructureDeserializedCorrectly()
        {
            //Root
            // |- folder1
            // |   |- Con1
            // |- Con2
            var treeModel = ConnectionTreeModelBuilder.Build();
            var csv = _serializer.Serialize(treeModel);
            var deserializedConnections = _deserializer.Deserialize(csv);
            var con1 = deserializedConnections.GetRecursiveChildList().First(info => string.Equals(info.Name, "Con1", StringComparison.Ordinal));
            var folder1 = deserializedConnections.GetRecursiveChildList().First(info => string.Equals(info.Name, "folder1", StringComparison.Ordinal));
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
                RDPAuthenticationLevel = AuthenticationLevel.WarnOnFailedAuth,
                Colors = RDPColors.Colors16Bit,
                Resolution = RDPResolutions.Res1366x768,
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
                    if (string.Equals(property.Name, "Password", StringComparison.Ordinal))
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