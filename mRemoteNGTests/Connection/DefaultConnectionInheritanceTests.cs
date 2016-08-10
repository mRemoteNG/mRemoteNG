using mRemoteNG.Connection;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
    public class DefaultConnectionInheritanceTests
    {
        [SetUp]
        public void Setup()
        {
            DefaultConnectionInheritance.Instance.TurnOffInheritanceCompletely();
        }

        [Test]
        public void LoadingDefaultInheritanceUpdatesAllProperties()
        {
            var inheritanceSource = new ConnectionInfoInheritance(new object(), true);
            inheritanceSource.TurnOnInheritanceCompletely();
            DefaultConnectionInheritance.Instance.LoadFrom(inheritanceSource);
            Assert.That(DefaultConnectionInheritance.Instance.EverythingInherited, Is.True);
        }

        [Test]
        public void SavingDefaultInheritanceExportsAllProperties()
        {
            var inheritanceDestination = new ConnectionInfoInheritance(new object(), true);
            DefaultConnectionInheritance.Instance.AutomaticResize = true;
            DefaultConnectionInheritance.Instance.SaveTo(inheritanceDestination);
            Assert.That(inheritanceDestination.AutomaticResize, Is.True);
        }

        [Test]
        public void NewInheritanceInstancesCreatedWithDefaultInheritanceValues()
        {
            DefaultConnectionInheritance.Instance.Domain = true;
            var inheritanceInstance = new ConnectionInfoInheritance(new object());
            Assert.That(inheritanceInstance.Domain, Is.True);
        }

        [Test]
        public void NewInheritanceInstancesCreatedWithAllDefaultInheritanceValues()
        {
            DefaultConnectionInheritance.Instance.TurnOnInheritanceCompletely();
            var inheritanceInstance = new ConnectionInfoInheritance(new object());
            Assert.That(inheritanceInstance.EverythingInherited, Is.True);
        }
    }
}