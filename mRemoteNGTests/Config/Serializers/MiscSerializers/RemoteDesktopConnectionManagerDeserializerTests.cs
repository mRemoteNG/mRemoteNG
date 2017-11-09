using System.IO;
using System.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNGTests.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    public class RemoteDesktopConnectionManagerDeserializerTests
    {
        private string _connectionFileContents;
        private RemoteDesktopConnectionManagerDeserializer _deserializer;
        private ConnectionTreeModel _connectionTreeModel;
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
        private const RdpProtocol.AuthenticationLevel ExpectedAuthLevel = RdpProtocol.AuthenticationLevel.AuthRequired;


        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _connectionFileContents = Resources.test_rdcman_v2_2_schema1;
            _deserializer = new RemoteDesktopConnectionManagerDeserializer();
            _connectionTreeModel = _deserializer.Deserialize(_connectionFileContents);
        }

        [Test]
        public void ConnectionTreeModelHasARootNode()
        {
            var numberOfRootNodes = _connectionTreeModel.RootNodes.Count;
            Assert.That(numberOfRootNodes, Is.GreaterThan(0));
        }

        [Test]
        public void RootNodeHasContents()
        {
            var rootNodeContents = _connectionTreeModel.RootNodes.First().Children;
            Assert.That(rootNodeContents, Is.Not.Empty);
        }

        [Test]
        public void AllSubRootFoldersImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var rootNodeContents = importedRdcmanRootNode.Children.Count(node => node.Name == "Group1" || node.Name == "Group2");
            Assert.That(rootNodeContents, Is.EqualTo(2));
        }

        [Test]
        public void ConnectionDisplayNameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Name, Is.EqualTo(ExpectedName));
        }

        [Test]
        public void ConnectionHostnameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Hostname, Is.EqualTo(ExpectedHostname));
        }

        [Test]
        public void ConnectionDescriptionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Description, Is.EqualTo(ExpectedDescription));
        }

        [Test]
        public void ConnectionUsernameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Username, Is.EqualTo(ExpectedUsername));
        }

        [Test]
        public void ConnectionDomainImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Domain, Is.EqualTo(ExpectedDomain));
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
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Protocol, Is.EqualTo(ProtocolType.RDP));
        }

        [Test]
        public void ConnectionUseConsoleSessionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.UseConsoleSession, Is.EqualTo(ExpectedUseConsoleSession));
        }

        [Test]
        public void ConnectionPortImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Port, Is.EqualTo(ExpectedPort));
        }

        [Test]
        public void ConnectionGatewayUsageMethodImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RDGatewayUsageMethod, Is.EqualTo(ExpectedGatewayUsageMethod));
        }

        [Test]
        public void ConnectionGatewayHostnameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RDGatewayHostname, Is.EqualTo(ExpectedGatewayHostname));
        }

        [Test]
        public void ConnectionGatewayUsernameImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
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
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RDGatewayDomain, Is.EqualTo(ExpectedGatewayDomain));
        }

        [Test]
        public void ConnectionResolutionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Resolution, Is.EqualTo(ExpectedRdpResolution));
        }

        [Test]
        public void ConnectionColorDepthImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.Colors, Is.EqualTo(ExpectedRdpColorDepth));
        }

        [Test]
        public void ConnectionAudioRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectSound, Is.EqualTo(ExpectedAudioRedirection));
        }

        [Test]
        public void ConnectionKeyRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectKeys, Is.EqualTo(ExpectedKeyRedirection));
        }

        [Test]
        public void ConnectionDriveRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectDiskDrives, Is.EqualTo(ExpectedDriveRedirection));
        }

        [Test]
        public void ConnectionPortRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectPorts, Is.EqualTo(ExpectedPortRedirection));
        }

        [Test]
        public void ConnectionPrinterRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectPrinters, Is.EqualTo(ExpectedPrinterRedirection));
        }

        [Test]
        public void ConnectionSmartcardRedirectionImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
            var connection = group1.Children.First();
            Assert.That(connection.RedirectSmartCards, Is.EqualTo(ExpectedSmartcardRedirection));
        }

        [Test]
        public void ConnectionauthenticationLevelImported()
        {
            var rootNode = _connectionTreeModel.RootNodes.First();
            var importedRdcmanRootNode = rootNode.Children.OfType<ContainerInfo>().First();
            var group1 = importedRdcmanRootNode.Children.OfType<ContainerInfo>().First(node => node.Name == "Group1");
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