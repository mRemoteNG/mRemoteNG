using System.Collections.Generic;
using System.Reflection;
using mRemoteNG.Connection;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
	public class DefaultConnectionInheritanceTests
    {
	    [TestCaseSource(nameof(GetInheritanceProperties))]
		public void LoadingDefaultInheritanceUpdatesAllProperties(PropertyInfo property)
        {
			var inheritanceSource = new ConnectionInfoInheritance(new object(), true);
            inheritanceSource.TurnOnInheritanceCompletely();
	        DefaultConnectionInheritance.Instance.TurnOffInheritanceCompletely();

            DefaultConnectionInheritance.Instance.LoadFrom(inheritanceSource);

	        var valueInDestination = property.GetValue(DefaultConnectionInheritance.Instance);
	        var valueInSource = property.GetValue(inheritanceSource);
	        Assert.That(valueInDestination, Is.EqualTo(valueInSource));
		}

		[TestCaseSource(nameof(GetInheritanceProperties))]
		public void SavingDefaultInheritanceExportsAllProperties(PropertyInfo property)
        {
			var saveTarget = new ConnectionInfoInheritance(new object(), true);
	        saveTarget.TurnOffInheritanceCompletely();
	        DefaultConnectionInheritance.Instance.TurnOnInheritanceCompletely();

	        DefaultConnectionInheritance.Instance.SaveTo(saveTarget);

	        var valueInDestination = property.GetValue(saveTarget);
	        var valueInSource = property.GetValue(DefaultConnectionInheritance.Instance);
	        Assert.That(valueInDestination, Is.EqualTo(valueInSource));
		}

        [Test]
        public void NewInheritanceInstancesCreatedWithDefaultInheritanceValues()
        {
            DefaultConnectionInheritance.Instance.Domain = true;
            var inheritanceInstance = new ConnectionInfoInheritance(new object());
            Assert.That(inheritanceInstance.Domain, Is.True);
        }

		[TestCaseSource(nameof(GetInheritanceProperties))]
		public void NewInheritanceInstancesCreatedWithAllDefaultInheritanceValues(PropertyInfo property)
        {
            DefaultConnectionInheritance.Instance.TurnOnInheritanceCompletely();
            var inheritanceInstance = new ConnectionInfoInheritance(new object());

			var valueInDestination = property.GetValue(inheritanceInstance);
	        var valueInSource = property.GetValue(DefaultConnectionInheritance.Instance);
	        Assert.That(valueInDestination, Is.EqualTo(valueInSource));
		}

	    private static IEnumerable<PropertyInfo> GetInheritanceProperties()
	    {
		    return new ConnectionInfoInheritance(new object(), true).GetProperties();
	    }
	}
}