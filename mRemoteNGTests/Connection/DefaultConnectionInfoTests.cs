using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
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
            var saveTarget = new ConnectionInfo();
            DefaultConnectionInfo.Instance.Domain = _testDomain;
            DefaultConnectionInfo.Instance.SaveTo(saveTarget);
            Assert.That(saveTarget.Domain, Is.EqualTo(_testDomain));
        }

        [Test]
        public void CanSaveEnumValuesToString()
        {
            const ProtocolType targetProtocol = ProtocolType.RAW;
            var saveTarget = new AllStringPropertySaveTarget();
            DefaultConnectionInfo.Instance.Protocol = targetProtocol;
            DefaultConnectionInfo.Instance.SaveTo(saveTarget);
            Assert.That(saveTarget.Protocol, Is.EqualTo(targetProtocol.ToString()));
        }

        [Test]
        public void CanSaveIntegerValuesToString()
        {
            const int targetValue = 123;
            var saveTarget = new AllStringPropertySaveTarget();
            DefaultConnectionInfo.Instance.RDPMinutesToIdleTimeout = targetValue;
            DefaultConnectionInfo.Instance.SaveTo(saveTarget);
            Assert.That(saveTarget.RDPMinutesToIdleTimeout, Is.EqualTo(targetValue.ToString()));
        }

        [Test]
        public void CanSaveStringValuesToString()
        {
            const string targetName = "hello";
            var saveTarget = new AllStringPropertySaveTarget();
            DefaultConnectionInfo.Instance.Username = targetName;
            DefaultConnectionInfo.Instance.SaveTo(saveTarget);
            Assert.That(saveTarget.Username, Is.EqualTo(targetName));
        }


        private class AllStringPropertySaveTarget
        {
            public string Username { get; set; }
            public string Protocol { get; set; }
            public string RDPMinutesToIdleTimeout { get; set; }
        }
    }
}