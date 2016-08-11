using mRemoteNG.Connection;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
    public class DefaultConnectionInfoTests
    {
        private string _testDomain = "somedomain";

        [SetUp]
        public void Setup()
        {
            DefaultConnectionInfo.Instance.Domain = "";
        }

        [Test]
        public void LoadingDefaultInfoUpdatesAllProperties()
        {
            var connectionInfoSource = new ConnectionInfo { Domain = _testDomain };
            DefaultConnectionInfo.Instance.LoadFrom(connectionInfoSource);
            Assert.That(DefaultConnectionInfo.Instance.Domain, Is.EqualTo(_testDomain));
        }

        [Test]
        public void SavingDefaultConnectionInfoExportsAllProperties()
        {
            var connectionInfoDestination = new ConnectionInfo();
            DefaultConnectionInfo.Instance.Domain = _testDomain;
            DefaultConnectionInfo.Instance.SaveTo(connectionInfoDestination);
            Assert.That(connectionInfoDestination.Domain, Is.EqualTo(_testDomain));
        }

        [Test]
        public void NewConnectionInfoInstancesCreatedWithDefaultConnectionInfoValues()
        {
            DefaultConnectionInfo.Instance.Domain = _testDomain;
            var connectionInstance = new ConnectionInfo();
            Assert.That(connectionInstance.Domain, Is.EqualTo(_testDomain));
        }
    }
}