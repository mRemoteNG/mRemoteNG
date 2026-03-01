using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    [TestFixture]
    public class CredentialInheritanceTests
    {
        private RootNodeInfo _rootNode;

        [SetUp]
        public void Setup()
        {
            _rootNode = new RootNodeInfo(RootNodeType.Connection);
        }

        [Test]
        public void InheritFromGrandparent_WhenParentHasEmptyCredsAndInheritanceDisabled()
        {
            // Grandparent has credentials
            var grandParent = new ContainerInfo
            {
                Username = "GP_User",
                Password = "GP_Password",
                Domain = "GP_Domain"
            };

            // Parent has NO credentials and Inheritance is OFF (by default for these properties?)
            // Let's explicitly ensure inheritance is OFF for credentials on Parent
            var parent = new ContainerInfo();
            parent.Inheritance.Username = false;
            parent.Inheritance.Password = false;
            parent.Inheritance.Domain = false;
            
            // Explicitly set empty values (default)
            parent.Username = "";
            parent.Password = "";
            parent.Domain = "";

            // Child has Inheritance ON
            var child = new ConnectionInfo();
            child.Inheritance.Username = true;
            child.Inheritance.Password = true;
            child.Inheritance.Domain = true;

            _rootNode.AddChild(grandParent);
            grandParent.AddChild(parent);
            parent.AddChild(child);

            // Expectation: Child should inherit from Grandparent, skipping Parent's empty values
            // This is the "Fix" behavior.
            Assert.Multiple(() =>
            {
                Assert.That(child.Username, Is.EqualTo("GP_User"), "Username should inherit from Grandparent");
                Assert.That(child.Password, Is.EqualTo("GP_Password"), "Password should inherit from Grandparent");
                Assert.That(child.Domain, Is.EqualTo("GP_Domain"), "Domain should inherit from Grandparent");
            });
        }

        [Test]
        public void StandardInheritance_ParentHasCreds()
        {
            var parent = new ContainerInfo
            {
                Username = "Parent_User"
            };

            var child = new ConnectionInfo();
            child.Inheritance.Username = true;

            _rootNode.AddChild(parent);
            parent.AddChild(child);

            Assert.That(child.Username, Is.EqualTo("Parent_User"));
        }
    }
}
