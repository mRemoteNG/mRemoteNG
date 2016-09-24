using System;
using System.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Tree;
using mRemoteNGTests.Properties;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class RemoteDesktopConnectionDeserializerTests
    {
        // .rdp file schema: https://technet.microsoft.com/en-us/library/ff393699(v=ws.10).aspx
        private string[] _connectionFileContents;
        private RemoteDesktopConnectionDeserializer _deserializer;
        private ConnectionTreeModel _connectionTreeModel;
        private const string ExpectedHostname = "testhostname.domain.com";
        private const string ExpectedUserName = "myusernamehere";
        private const string ExpectedDomain = "myspecialdomain";
        private const string ExpectedGatewayHostname = "gatewayhostname.domain.com";
        private const int ExpectedPort = 9933;
        private const ProtocolRDP.RDPColors ExpectedColors = ProtocolRDP.RDPColors.Colors24Bit;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _connectionFileContents = Resources.test_remotedesktopconnection_rdp.Split(Environment.NewLine.ToCharArray());
            _deserializer = new RemoteDesktopConnectionDeserializer(_connectionFileContents);
            _connectionTreeModel = _deserializer.Deserialize();
        }

        [OneTimeTearDown]
        public void OnetimeTeardown()
        {
            _deserializer = null;
        }

        [Test]
        public void ConnectionTreeModelHasARootNode()
        {
            var numberOfRootNodes = _connectionTreeModel.RootNodes.Count;
            Assert.That(numberOfRootNodes, Is.GreaterThan(0));
        }

        [Test]
        public void RootNodeHasConnectionInfo()
        {
            var rootNodeContents = _connectionTreeModel.RootNodes.First().Children.OfType<ConnectionInfo>();
            Assert.That(rootNodeContents, Is.Not.Empty);
        }

        [Test]
        public void HostnameImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Hostname, Is.EqualTo(ExpectedHostname));
        }

        [Test]
        public void PortImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Port, Is.EqualTo(ExpectedPort));
        }

        [Test]
        public void UsernameImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Username, Is.EqualTo(ExpectedUserName));
        }

        [Test]
        public void DomainImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Domain, Is.EqualTo(ExpectedDomain));
        }

        [Test]
        public void RdpColorsImportedCorrectly()
        {
            var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
            Assert.That(connectionInfo.Colors, Is.EqualTo(ExpectedColors));
        }

        //[Test]
        //public void GatewayHostnameImportedCorrectly()
        //{
        //    var connectionInfo = _connectionTreeModel.RootNodes.First().Children.First();
        //    Assert.That(connectionInfo.RDGatewayHostname, Is.EqualTo(_expectedGatewayHostname));
        //}
    }
}