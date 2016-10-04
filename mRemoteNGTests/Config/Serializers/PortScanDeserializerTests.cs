using System.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class PortScanDeserializerTests
    {
        private PortScanDeserializer _deserializer;
        private ConnectionInfo _importedConnectionInfo;
        private const string ExpectedHostName = "server1.domain.com";
        private const string ExpectedDisplayName = "server1";
        private const ProtocolType ExpectedProtocolType = ProtocolType.SSH2;



        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            var host = new ScanHost("10.20.30.40")
            {
                HostName = "server1.domain.com",
                SSH = true
            };
            _deserializer = new PortScanDeserializer(new [] {host}, ProtocolType.SSH2);
            _deserializer.Deserialize();
            var connectionTreeModel = _deserializer.Deserialize();
            var root = connectionTreeModel.RootNodes.First();
            _importedConnectionInfo = root.Children.First();
        }

        [OneTimeTearDown]
        public void OnetimeTeardown()
        {
            _deserializer = null;
            _importedConnectionInfo = null;
        }

        [Test]
        public void DisplayNameImported()
        {
            Assert.That(_importedConnectionInfo.Name, Is.EqualTo(ExpectedDisplayName));
        }

        [Test]
        public void HostNameImported()
        {
            Assert.That(_importedConnectionInfo.Hostname, Is.EqualTo(ExpectedHostName));
        }

        [Test]
        public void ProtocolImported()
        {
            Assert.That(_importedConnectionInfo.Protocol, Is.EqualTo(ExpectedProtocolType));
        }
    }
}