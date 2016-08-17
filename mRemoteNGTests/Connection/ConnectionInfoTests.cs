using mRemoteNG.Connection;
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
            var secondConnection = _connectionInfo.Copy();
            Assert.That(secondConnection.Domain, Is.EqualTo(_connectionInfo.Domain));
        }

        [Test]
        public void CopyFromCopiesProperties()
        {
            var secondConnection = new ConnectionInfo {Domain = TestDomain};
            _connectionInfo.CopyFrom(secondConnection);
            Assert.That(_connectionInfo.Domain, Is.EqualTo(secondConnection.Domain));
        }
    }
}