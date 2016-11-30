using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Container;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
    public class ConnectionInfoTests
    {
        private ConnectionInfo _connectionInfo;
        private const string TestDomain = "somedomain";

        [SetUp]
        public void Setup()
        {
            _connectionInfo = new ConnectionInfo();
        }

        [TearDown]
        public void Teardown()
        {
            _connectionInfo = null;
        }

        [Test]
        public void CreatingConnectionInfoWithParentSetsTheParentProperty()
        {
            var container = new ContainerInfo();
            var connectionInfo = new ConnectionInfo(container);
            Assert.That(connectionInfo.Parent, Is.EqualTo(container));
        }

        [Test]
        public void CreatingConnectionInfoWithParentAddsToTheParentsChildList()
        {
            var container = new ContainerInfo();
            var connectionInfo = new ConnectionInfo(container);
            Assert.That(container.Children, Does.Contain(connectionInfo));
        }

        [Test]
        public void CopyCreatesMemberwiseCopy()
        {
            _connectionInfo.Domain = TestDomain;
            var secondConnection = _connectionInfo.Clone();
            Assert.That(secondConnection.Domain, Is.EqualTo(_connectionInfo.Domain));
        }

        [Test]
        public void CopyFromCopiesProperties()
        {
            var secondConnection = new ConnectionInfo {Domain = TestDomain};
            _connectionInfo.CopyFrom(secondConnection);
            Assert.That(_connectionInfo.Domain, Is.EqualTo(secondConnection.Domain));
        }

        [Test]
        public void CopyingAConnectionInfoAlsoCopiesItsInheritance()
        {
            _connectionInfo.Inheritance.Username = true;
            var secondConnection = new ConnectionInfo {Inheritance = {Username = false}};
            secondConnection.CopyFrom(_connectionInfo);
            Assert.That(secondConnection.Inheritance.Username, Is.True);
        }

        [Test]
        public void PropertyChangedEventRaisedWhenOpenConnectionsChanges()
        {
            var eventWasCalled = false;
            _connectionInfo.PropertyChanged += (sender, args) => eventWasCalled = true;
            _connectionInfo.OpenConnections.Add(new ProtocolSSH2());
            Assert.That(eventWasCalled);
        }

        [Test]
        public void PropertyChangedEventArgsAreCorrectWhenOpenConnectionsChanges()
        {
            var nameOfModifiedProperty = "";
            _connectionInfo.PropertyChanged += (sender, args) => nameOfModifiedProperty = args.PropertyName;
            _connectionInfo.OpenConnections.Add(new ProtocolSSH2());
            Assert.That(nameOfModifiedProperty, Is.EqualTo("OpenConnections"));
        }

        [TestCase(ProtocolType.HTTP, ExpectedResult = 80)]
        [TestCase(ProtocolType.HTTPS, ExpectedResult = 443)]
        [TestCase(ProtocolType.ICA, ExpectedResult = 1494)]
        [TestCase(ProtocolType.IntApp, ExpectedResult = 0)]
        [TestCase(ProtocolType.RAW, ExpectedResult = 23)]
        [TestCase(ProtocolType.RDP, ExpectedResult = 3389)]
        [TestCase(ProtocolType.Rlogin, ExpectedResult = 513)]
        [TestCase(ProtocolType.SSH1, ExpectedResult = 22)]
        [TestCase(ProtocolType.SSH2, ExpectedResult = 22)]
        [TestCase(ProtocolType.Telnet, ExpectedResult = 23)]
        [TestCase(ProtocolType.VNC, ExpectedResult = 5900)]
        public int GetDefaultPortReturnsCorrectPortForProtocol(ProtocolType protocolType)
        {
            _connectionInfo.Protocol = protocolType;
            return _connectionInfo.GetDefaultPort();
        }
    }
}