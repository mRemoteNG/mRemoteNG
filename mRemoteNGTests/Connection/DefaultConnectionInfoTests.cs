using System.Collections.Generic;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
	public class DefaultConnectionInfoTests
    {
        [SetUp]
        public void Setup()
        {
            DefaultConnectionInfo.Instance.Domain = "";
        }

        [TestCaseSource(nameof(GetConnectionInfoProperties))]
        public void LoadingDefaultInfoUpdatesAllProperties(PropertyInfo property)
        {
            var connectionInfoSource = ConnectionInfoHelpers.GetRandomizedConnectionInfo();
            DefaultConnectionInfo.Instance.LoadFrom(connectionInfoSource);
	        var valueInDestination = property.GetValue(DefaultConnectionInfo.Instance);
	        var valueInSource = property.GetValue(connectionInfoSource);
            Assert.That(valueInDestination, Is.EqualTo(valueInSource));
        }

		[TestCaseSource(nameof(GetConnectionInfoProperties))]
		public void SavingDefaultConnectionInfoExportsAllProperties(PropertyInfo property)
        {
            var saveTarget = new ConnectionInfo();
	        var randomizedValue = property.GetValue(ConnectionInfoHelpers.GetRandomizedConnectionInfo());
			property.SetValue(DefaultConnectionInfo.Instance, randomizedValue);
            DefaultConnectionInfo.Instance.SaveTo(saveTarget);
	        var valueInDestination = property.GetValue(saveTarget);
	        var valueInSource = property.GetValue(DefaultConnectionInfo.Instance);
			Assert.That(valueInDestination, Is.EqualTo(valueInSource));
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

	    private static IEnumerable<PropertyInfo> GetConnectionInfoProperties()
	    {
			return new ConnectionInfo().GetSerializableProperties();
	    }
    }
}