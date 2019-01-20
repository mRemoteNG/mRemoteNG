using System.Collections.Generic;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNGTests.Properties;
using NUnit.Framework;
using System.IO;
using System.Linq;
using mRemoteNGTests.Tools;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    [TestFixtureSource(nameof(TestFixtureData))]
    public class RemoteDesktopConnectionManagerDeserializerTests
    {
        private readonly RemoteDesktopConnectionManagerDeserializer _deserializer;
        private readonly SerializationResult _serializationResult;
        private readonly ContainerInfo _rootContainerInfo;
        private const string ExpectedName = "server1_displayname";
        private const string ExpectedHostname = "server1";
        private const string ExpectedDescription = "Comment text here";
        private const string ExpectedUsername = "myusername1";
        private const string ExpectedDomain = "mydomain";
        private const string ExpectedPassword = "passwordHere!";
        private const bool ExpectedUseConsoleSession = true;
        private const int ExpectedPort = 9933;
        private const RdpProtocol.RDGatewayUsageMethod ExpectedGatewayUsageMethod = RdpProtocol.RDGatewayUsageMethod.Always;
        private const string ExpectedGatewayHostname = "gatewayserverhost.innerdomain.net";
        private const string ExpectedGatewayUsername = "gatewayusername";
        private const string ExpectedGatewayDomain = "innerdomain";
        private const string ExpectedGatewayPassword = "gatewayPassword123";
        private const RdpProtocol.RDPResolutions ExpectedRdpResolution = RdpProtocol.RDPResolutions.FitToWindow;
        private const RdpProtocol.RDPColors ExpectedRdpColorDepth = RdpProtocol.RDPColors.Colors24Bit;
        private const RdpProtocol.RDPSounds ExpectedAudioRedirection = RdpProtocol.RDPSounds.DoNotPlay;
        private const bool ExpectedKeyRedirection = true;
        private const bool ExpectedSmartcardRedirection = true;
        private const bool ExpectedDriveRedirection = true;
        private const bool ExpectedPortRedirection = true;
        private const bool ExpectedPrinterRedirection = true;
        private const RdpProtocol.AuthenticationLevel ExpectedAuthLevel = RdpProtocol.AuthenticationLevel.WarnOnFailedAuth;

        private static IEnumerable<TestFixtureData> TestFixtureData()
        {
            return new[]
            {
                new TestFixtureData(Resources.test_rdcman_v2_2_schema1).SetName("RDCM_v2_2"),
                new TestFixtureData(Resources.test_rdcman_v2_7_schema3).SetName("RDCM_v2_7"),
            };
        }

        public RemoteDesktopConnectionManagerDeserializerTests(string rdcmFileContents)
        {
            _deserializer = new RemoteDesktopConnectionManagerDeserializer();
            _serializationResult = _deserializer.Deserialize(rdcmFileContents);
            _rootContainerInfo = _serializationResult.ConnectionRecords.OfType<ContainerInfo>().First();
        }

        [Test]
        public void AllSubRootFoldersImported()
        {
            var rootNodeContents = _rootContainerInfo.Children.Count(node => node.Name == "Group1" || node.Name == "Group2");
            Assert.That(rootNodeContents, Is.EqualTo(2));
        }

        [Test]
        public void ConnectionDisplayNameImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Name, Is.EqualTo(ExpectedName));
        }

        [Test]
        public void ConnectionHostnameImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Hostname, Is.EqualTo(ExpectedHostname));
        }

        [Test]
        public void ConnectionDescriptionImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Description, Is.EqualTo(ExpectedDescription));
        }

        [Test]
        public void CredentialIdCorrectlySet()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            var cred = _serializationResult.ConnectionToCredentialMap.DistinctCredentialRecords.First();
            Assert.That(connection.CredentialRecordId.FirstOrDefault(), Is.EqualTo(cred.Id));
        }

        [Test]
        public void ConnectionUsernameImported()
        {
            var cred = _serializationResult.ConnectionToCredentialMap.DistinctCredentialRecords.First();
            Assert.That(cred.Username, Is.EqualTo(ExpectedUsername));
        }

        [Test]
        public void ConnectionDomainImported()
        {
            var cred = _serializationResult.ConnectionToCredentialMap.DistinctCredentialRecords.First();
            Assert.That(cred.Domain, Is.EqualTo(ExpectedDomain));
        }

        // Since password is encrypted with a machine key, cant test decryption on another machine
        //[Test]
        //public void ConnectionPasswordImported()
        //{
        //    var rootNode = _connectionTreeModel.RootNodes.First();
        //    var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
        //    var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
        //    var connection = group1.Children.First();
        //    Assert.That(connection.Password, Is.EqualTo(ExpectedPassword));
        //}

        [Test]
        public void ConnectionProtocolSetToRdp()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Protocol, Is.EqualTo(ProtocolType.RDP));
        }

        [Test]
        public void ConnectionUseConsoleSessionImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.UseConsoleSession, Is.EqualTo(ExpectedUseConsoleSession));
        }

        [Test]
        public void ConnectionPortImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Port, Is.EqualTo(ExpectedPort));
        }

        [Test]
        public void ConnectionGatewayUsageMethodImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RDGatewayUsageMethod, Is.EqualTo(ExpectedGatewayUsageMethod));
        }

        [Test]
        public void ConnectionGatewayHostnameImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RDGatewayHostname, Is.EqualTo(ExpectedGatewayHostname));
        }

        [Test]
        public void CredentialIdSetCorrectly()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            var cred = _serializationResult.ConnectionToCredentialMap.DistinctCredentialRecords.FirstOrDefault();
            Assert.That(connection.CredentialRecordId.FirstOrDefault(), Is.EqualTo(cred?.Id));
        }

        [Test]
        public void ConnectionGatewayUsernameImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RDGatewayUsername, Is.EqualTo(ExpectedGatewayUsername));
        }

        // Since password is encrypted with a machine key, cant test decryption on another machine
        //[Test]
        //public void ConnectionGatewayPasswordImported()
        //{
        //    var rootNode = _connectionTreeModel.RootNodes.First();
        //    var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
        //    var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
        //    var connection = group1.Children.First();
        //    Assert.That(connection.RDGatewayPassword, Is.EqualTo(ExpectedGatewayPassword));
        //}

        [Test]
        public void ConnectionGatewayDomainImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RDGatewayDomain, Is.EqualTo(ExpectedGatewayDomain));
        }

        [Test]
        public void ConnectionResolutionImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Resolution, Is.EqualTo(ExpectedRdpResolution));
        }

        [Test]
        public void ConnectionColorDepthImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Colors, Is.EqualTo(ExpectedRdpColorDepth));
        }

        [Test]
        public void ConnectionAudioRedirectionImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectSound, Is.EqualTo(ExpectedAudioRedirection));
        }

        [Test]
        public void ConnectionKeyRedirectionImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectKeys, Is.EqualTo(ExpectedKeyRedirection));
        }

        [Test]
        public void ConnectionDriveRedirectionImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectDiskDrives, Is.EqualTo(ExpectedDriveRedirection));
        }

        [Test]
        public void ConnectionPortRedirectionImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectPorts, Is.EqualTo(ExpectedPortRedirection));
        }

        [Test]
        public void ConnectionPrinterRedirectionImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectPrinters, Is.EqualTo(ExpectedPrinterRedirection));
        }

        [Test]
        public void ConnectionSmartcardRedirectionImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectSmartCards, Is.EqualTo(ExpectedSmartcardRedirection));
        }

        [Test]
        public void ConnectionAuthenticationLevelImported()
        {
            var group1 = _rootContainerInfo.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RDPAuthenticationLevel, Is.EqualTo(ExpectedAuthLevel));
        }

        [Test]
        public void ExceptionThrownOnBadSchemaVersion()
        {
            var badFileContents = Resources.test_rdcman_v2_2_badschemaversion;
            Assert.That(() => _deserializer.Deserialize(badFileContents), Throws.TypeOf<FileFormatException>());
        }

        [Test]
        public void ExceptionThrownOnUnsupportedVersion()
        {
            var badFileContents = Resources.test_rdcman_badVersionNumber;
            Assert.That(() => _deserializer.Deserialize(badFileContents), Throws.TypeOf<FileFormatException>());
        }

        [Test]
        public void ExceptionThrownOnNoVersion()
        {
            var badFileContents = Resources.test_rdcman_noversion;
            Assert.That(() => _deserializer.Deserialize(badFileContents), Throws.TypeOf<FileFormatException>());
        }
    }
}