using System.Collections.Generic;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
	public class DefaultConnectionInfoTests
	{
		private ConnectionInfo _randomizedConnectionInfo;

        [SetUp]
        public void Setup()
        {
	        _randomizedConnectionInfo = ConnectionInfoHelpers.GetRandomizedConnectionInfo();
        }

        [TestCaseSource(nameof(GetConnectionInfoProperties))]
        public void LoadingDefaultInfoUpdatesAllProperties(PropertyInfo property)
        {
            DefaultConnectionInfo.Instance.LoadFrom(_randomizedConnectionInfo);
	        var valueInDestination = property.GetValue(DefaultConnectionInfo.Instance);
	        var valueInSource = property.GetValue(_randomizedConnectionInfo);
            Assert.That(valueInDestination, Is.EqualTo(valueInSource));
        }

		[TestCaseSource(nameof(GetConnectionInfoProperties))]
		public void SavingDefaultConnectionInfoExportsAllProperties(PropertyInfo property)
        {
            var saveTarget = new ConnectionInfo();
	        var randomizedValue = property.GetValue(_randomizedConnectionInfo);
			property.SetValue(DefaultConnectionInfo.Instance, randomizedValue);
            DefaultConnectionInfo.Instance.SaveTo(saveTarget);
	        var valueInDestination = property.GetValue(saveTarget);
	        var valueInSource = property.GetValue(DefaultConnectionInfo.Instance);
			Assert.That(valueInDestination, Is.EqualTo(valueInSource));
        }

		[TestCaseSource(nameof(GetConnectionInfoProperties))]
		public void CanSaveDefaultConnectionToModelWithAllStringProperties(PropertyInfo property)
		{
            var saveTarget = new SerializableConnectionInfoAllPropertiesOfType<string>();

			// randomize default connnection values to ensure we dont get false passing tests
			var randomizedValue = property.GetValue(_randomizedConnectionInfo);
			property.SetValue(DefaultConnectionInfo.Instance, randomizedValue);

            DefaultConnectionInfo.Instance.SaveTo(saveTarget);

			var valueInSource = property.GetValue(DefaultConnectionInfo.Instance).ToString();
			var valueInDestination = saveTarget.GetType().GetProperty(property.Name).GetValue(saveTarget).ToString();
            Assert.That(valueInDestination, Is.EqualTo(valueInSource));
        }

		private static IEnumerable<PropertyInfo> GetConnectionInfoProperties()
	    {
			return new ConnectionInfo().GetSerializableProperties();
	    }
    }
}