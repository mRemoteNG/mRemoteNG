using mRemoteNG.Connection;
using mRemoteNG.Container;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    [TestFixture]
    public class ConnectionInfoCloneTests
    {
        [Test]
        public void CloneWithNoCredentials_ShouldHaveEmptyCredentials()
        {
            // Arrange
            var parent = new ContainerInfo { Name = "Parent" };
            parent.Username = "ParentUser";
            parent.Password = "ParentPass";
            parent.Domain = "ParentDomain";

            var original = new ConnectionInfo { Name = "Original" };
            original.SetParent(parent);
            // Ensure inheritance is on (default is usually true, but let's be explicit if possible)
            original.Inheritance.Username = true;
            original.Inheritance.Password = true;
            original.Inheritance.Domain = true;

            // Verify original inherits
            Assert.That(original.Username, Is.EqualTo("ParentUser"), "Original should inherit username");

            // Act - Mimic ConnectionInitiator logic
            var clone = original.Clone();
            
            // Logic from ConnectionInitiator.cs
            clone.SetParent((ContainerInfo)original.Parent!);
            clone.Inheritance.Username = false;
            clone.Inheritance.Password = false;
            clone.Inheritance.Domain = false;
            clone.Username = string.Empty;
            clone.Password = string.Empty;
            clone.Domain = string.Empty;

            // Assert
            Assert.That(clone.Username, Is.EqualTo(string.Empty), "Clone Username should be empty");
            Assert.That(clone.Password, Is.EqualTo(string.Empty), "Clone Password should be empty");
            Assert.That(clone.Domain, Is.EqualTo(string.Empty), "Clone Domain should be empty");
        }
    }
}
