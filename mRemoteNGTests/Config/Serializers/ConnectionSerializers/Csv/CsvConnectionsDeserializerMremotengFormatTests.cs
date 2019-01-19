using System;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.Csv;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.ICA;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNGTests.TestHelpers;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Csv
{
    public class CsvConnectionsDeserializerMremotengFormatTests
    {
        private CsvConnectionsDeserializerMremotengFormat _deserializer;
        private CsvConnectionsSerializerMremotengFormat _serializer;
        private static readonly ICredentialRecord CredentialRecord = new CredentialRecord
        {
            Title = "MyTestCredential",
            Username = "myuser",
            Domain = "somedomain",
            Password = "helloworld123".ConvertToSecureString()
        };

        [SetUp]
        public void Setup()
        {
            var credRepo = Substitute.For<ICredentialRepository>();
            credRepo.CredentialRecords.Returns(info => new List<ICredentialRecord> { CredentialRecord });

            Runtime.CredentialService.RepositoryList.ForEach(r => Runtime.CredentialService.RemoveRepository(r));
            Runtime.CredentialService.AddRepository(credRepo);

            _deserializer = new CsvConnectionsDeserializerMremotengFormat();
            _serializer = new CsvConnectionsSerializerMremotengFormat(new SaveFilter(), Runtime.CredentialService.RepositoryList);
        }

        [TestCaseSource(typeof(DeserializationTestSource), nameof(DeserializationTestSource.ConnectionPropertyTestCases))]
        public object ConnectionPropertiesDeserializedCorrectly(string propertyToCheck)
        {
            var csv = _serializer.Serialize(GetTestConnection());
            var deserializedConnections = _deserializer.Deserialize(csv);

            var connection = deserializedConnections.ConnectionRecords.FirstOrDefault();
            var propertyValue = typeof(ConnectionInfo).GetProperty(propertyToCheck)?.GetValue(connection);
            return propertyValue;
        }

        [TestCaseSource(typeof(DeserializationTestSource), nameof(DeserializationTestSource.InheritanceTestCases))]
        public object InheritancePropertiesDeserializedCorrectly(string propertyToCheck)
        {
            var csv = _serializer.Serialize(GetTestConnectionWithAllInherited());
            var deserializedConnections = _deserializer.Deserialize(csv);

            var connection = deserializedConnections.ConnectionRecords.FirstOrDefault();

            connection?.RemoveParent();
            var propertyValue = typeof(ConnectionInfoInheritance).GetProperty(propertyToCheck)?.GetValue(connection?.Inheritance);
            return propertyValue;
        }

        [Test]
        public void CredentialsHarvestedWhenDeserialized()
        {
            var cred = new CredentialRecord
            {
                Username = Randomizer.RandomString(),
                Domain = Randomizer.RandomString(),
                Password = Randomizer.RandomString().ConvertToSecureString()
            };

            var csv = "Id;Username;Domain;Password\n" +
                    $"{Guid.NewGuid()};{cred.Username};{cred.Domain};{cred.Password.ConvertToUnsecureString()}";
            
            var deserializedConnections = _deserializer.Deserialize(csv);
            var harvestedCredential = deserializedConnections.CredentialRecords.FirstOrDefault();

            Assert.That(harvestedCredential, Is.EqualTo(cred)
                .Using(new CredentialDomainUserPasswordComparer()));
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

            var con1 = deserializedConnections.ConnectionRecords.FlattenConnectionTree().First(info => info.Name == "Con1");
            var folder1 = deserializedConnections.ConnectionRecords.FlattenConnectionTree().First(info => info.Name == "folder1");
            Assert.That(con1.Parent, Is.EqualTo(folder1));
        }

        internal static ConnectionInfo GetTestConnection()
        {
            return new ConnectionInfo
            {
                Name = "SomeName",
                Description = "SomeDescription",
                Icon = "SomeIcon",
                Panel = "SomePanel",
                CredentialRecordId = CredentialRecord.Id,
                Hostname = "SomeHostname",
                PuttySession = "SomePuttySession",
                LoadBalanceInfo = "SomeLoadBalanceInfo",
                PreExtApp = "SomePreExtApp",
                PostExtApp = "SomePostExtApp",
                MacAddress = "SomeMacAddress",
                UserField = "SomeUserField",
                ExtApp = "SomeExtApp",
                VNCProxyUsername = "SomeVNCProxyUsername",
                VNCProxyPassword = "SomeVNCProxyPassword",
                RDGatewayUsername = "SomeRDGatewayUsername",
                RDGatewayPassword = "SomeRDGatewayPassword",
                RDGatewayDomain = "SomeRDGatewayDomain",
                VNCProxyIP = "SomeVNCProxyIP",
                RDGatewayHostname = "SomeRDGatewayHostname",
                Protocol = ProtocolType.ICA,
                Port = 999,
                UseConsoleSession = true,
                UseCredSsp = true,
                RenderingEngine = HTTPBase.RenderingEngine.Gecko,
                ICAEncryptionStrength = IcaProtocol.EncryptionStrength.Encr40Bit,
                RDPAuthenticationLevel = RdpProtocol.AuthenticationLevel.WarnOnFailedAuth,
                Colors = RdpProtocol.RDPColors.Colors16Bit,
                Resolution = RdpProtocol.RDPResolutions.Res1366x768,
                AutomaticResize = true,
                DisplayWallpaper = true,
                DisplayThemes = true,
                EnableFontSmoothing = true,
                EnableDesktopComposition = true,
                CacheBitmaps = true,
                RedirectDiskDrives = true,
                RedirectPorts = true,
                RedirectPrinters = true,
                RedirectSmartCards = true,
                RedirectSound = RdpProtocol.RDPSounds.LeaveAtRemoteComputer,
                RedirectKeys = true,
                VNCCompression = ProtocolVNC.Compression.Comp4,
                VNCEncoding = ProtocolVNC.Encoding.EncRRE,
                VNCAuthMode = ProtocolVNC.AuthMode.AuthVNC,
                VNCProxyType = ProtocolVNC.ProxyType.ProxySocks5,
                VNCProxyPort = 123,
                VNCColors = ProtocolVNC.Colors.Col8Bit,
                VNCSmartSizeMode = ProtocolVNC.SmartSizeMode.SmartSAspect,
                VNCViewOnly = true,
                RDGatewayUsageMethod = RdpProtocol.RDGatewayUsageMethod.Detect,
                RDGatewayUseConnectionCredentials = RdpProtocol.RDGatewayUseConnectionCredentials.SmartCard
            };
        }

        internal static ConnectionInfo GetTestConnectionWithAllInherited()
        {
            var connectionInfo = new ConnectionInfo();
            connectionInfo.Inheritance.TurnOnInheritanceCompletely();
            return connectionInfo;
        }

        private class DeserializationTestSource
        {
            public static IEnumerable ConnectionPropertyTestCases()
            {
                var properties = new ConnectionInfo().GetSerializableProperties();
                var testCases = new List<TestCaseData>();
                var testConnectionInfo = GetTestConnection();

                foreach (var property in properties)
                {
                    testCases.Add(
                        new TestCaseData(property.Name)
                        .Returns(property.GetValue(testConnectionInfo)));
                }

                return testCases;
            }

            public static IEnumerable InheritanceTestCases()
            {
                var properties = new ConnectionInfoInheritance(new object()).GetProperties();
                var testCases = new List<TestCaseData>();
                var testInheritance = GetTestConnectionWithAllInherited().Inheritance;

                foreach (var property in properties)
                {
                    testCases.Add(
                        new TestCaseData(property.Name)
                            .Returns(property.GetValue(testInheritance)));
                }

                return testCases;
            }
        }
    }
}
