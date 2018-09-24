using System;
using System.Linq;
using mRemoteNG.Tools.WindowsRegistry;
using NUnit.Framework;

namespace mRemoteNGTests.Tools.Registry
{
    public class WindowsRegistryTests
    {
        private WindowsRegistry _registry;

        [SetUp]
        public void Setup()
        {
            _registry = new WindowsRegistry();
        }

        [Test]
        public void CanGetSubkeyNames()
        {
            var subKeyNames = _registry.GetSubKeyNames(RegistryHive.CurrentUser, "Software");
            Assert.That(subKeyNames, Does.Contain("Microsoft"));
        }

        [Test]
        public void GetSubkeyNamesThrowsIfGivenNullKeyPath()
        {
            Assert.Throws<ArgumentNullException>(() => _registry.GetSubKeyNames(RegistryHive.CurrentUser, null));
        }

        [Test]
        public void CanGetKeyValue()
        {
            var keyValue = _registry.GetKeyValue(RegistryHive.ClassesRoot, @".dll\PersistentHandler", "");
            Assert.That(keyValue.FirstOrDefault(), Is.EqualTo("{098f2470-bae0-11cd-b579-08002b30bfeb}"));
        }

        [Test]
        public void GetKeyValueThrowsIfGivenNullKeyPath()
        {
            Assert.Throws<ArgumentNullException>(() => _registry.GetKeyValue(RegistryHive.CurrentUser, null, ""));
        }

        [Test]
        public void GetKeyValueThrowsIfGivenNullPropertyName()
        {
            Assert.Throws<ArgumentNullException>(() => _registry.GetKeyValue(RegistryHive.CurrentUser, "", null));
        }
    }
}
