using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Properties;
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

			// randomize default connection values to ensure we don't get false passing tests
			var randomizedValue = property.GetValue(_randomizedConnectionInfo);
			property.SetValue(DefaultConnectionInfo.Instance, randomizedValue);

            DefaultConnectionInfo.Instance.SaveTo(saveTarget);

			var valueInSource = property.GetValue(DefaultConnectionInfo.Instance)?.ToString();
			var valueInDestination = saveTarget.GetType().GetProperty(property.Name)?.GetValue(saveTarget)?.ToString();
            try
            {
                Assert.That(valueInDestination, Is.EqualTo(valueInSource));
            }
            catch (AssertionException)
            {
                Console.WriteLine($"Assertion Failed: Parameter {property.Name}");                
            }
            Assert.That(valueInDestination, Is.EqualTo(valueInSource));
        }

		[TestCaseSource(nameof(GetConnectionInfoProperties))]
		public void EveryPropertyHasASettingForItsDefault(PropertyInfo property)
		{
			Assert.That(typeof(Settings).GetProperty("ConDefault" + property.Name), Is.Not.Null);
		}

		// A new installation starts from this value until the default connection is saved.
		[Test]
		public void ANewInstallationWarnsWhenTheRdpServerCannotBeAuthenticated()
		{
			string builtIn = typeof(Settings).GetProperty(nameof(Settings.ConDefaultRDPAuthenticationLevel))
				?.GetCustomAttribute<DefaultSettingValueAttribute>()?.Value;
			Assert.That(builtIn, Is.EqualTo(nameof(AuthenticationLevel.WarnOnFailedAuth)));
		}

		// The application's own config file, where present, overrides the built-in value.
		[Test]
		public void TheApplicationConfigAgreesOnTheRdpAuthenticationDefault()
		{
			string config = typeof(Settings).Assembly.Location + ".config";
			Assume.That(File.Exists(config), $"{config} is not beside the tests");
			string shipped = XDocument.Load(config).Descendants("setting")
				.FirstOrDefault(s => (string)s.Attribute("name") == nameof(Settings.ConDefaultRDPAuthenticationLevel))
				?.Element("value")?.Value;
			Assert.That(shipped, Is.EqualTo(nameof(AuthenticationLevel.WarnOnFailedAuth)));
		}

		// A property that cannot be set, such as PluginProperties, has no default.
		private static IEnumerable<PropertyInfo> GetConnectionInfoProperties()
	    {
			return new ConnectionInfo().GetSerializableProperties().Where(property => property.CanWrite);
	    }
    }
}