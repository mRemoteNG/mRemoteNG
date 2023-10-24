using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using mRemoteNG.Tools.WindowsRegistry;
using NUnit.Framework;

namespace mRemoteNGTests.Tools.Registry
{
    public class WindowsRegistryTests
    {
        private IRegistry _registry;
        private IRegistryRead _registryReader;
        private IRegistryWrite _registryWriter;

        [SetUp]
        public void Setup()
        {
            _registry = new WindowsRegistry();
            _registryReader = new WindowsRegistry();
            _registryWriter = new WindowsRegistry();
        }

        #region GetSubKeyNames() tests
        [Test]
        public void CanGetSubkeyNames()
        {
            var subKeyNames = _registryReader.GetSubKeyNames(RegistryHive.CurrentUser, "Software");
            Assert.That(subKeyNames, Does.Contain("Microsoft"));
        }
        [Test]
        public void GetSubkeyNamesThrowsIfGivenNullKeyPath()
        {
            Assert.Throws<ArgumentNullException>(() => _registryReader.GetSubKeyNames(RegistryHive.CurrentUser, null));
        }
        [Test]
        public void GetSubkeyNamesThrowsIfGivenUnknownHive()
        {
            Assert.Throws<ArgumentException>(() => _registryReader.GetSubKeyNames(new RegistryHive(), "Software"));
        }
        #endregion

        #region GetPropertyValue() tests
        [Test]
        public void CanGetPropertyValue()
        {
            var keyValue = _registryReader.GetPropertyValue(RegistryHive.ClassesRoot, @".dll\PersistentHandler", "");
            Assert.That(keyValue.FirstOrDefault(), Is.EqualTo("{098f2470-bae0-11cd-b579-08002b30bfeb}"));
        }

        [Test]
        public void CanGetPropertyValueByRegistryKeyObject()
        {
            WindowsRegistryKey key = new WindowsRegistryKey
            {
                Hive = RegistryHive.ClassesRoot,
                Path = @".dll\PersistentHandler",
                Name = ""
            };

            var keyValue = _registryReader.GetPropertyValue(key);
            Assert.That(keyValue.FirstOrDefault(), Is.EqualTo("{098f2470-bae0-11cd-b579-08002b30bfeb}"));
        }
        [Test]
        public void GetPropertyValueThrowsIfGivenNullKeyPath()
        {
            Assert.Throws<ArgumentNullException>(() => _registryReader.GetPropertyValue(RegistryHive.CurrentUser, null, ""));
        }

        [Test]
        public void GetPropertyValueThrowsIfGivenNullPropertyName()
        {
            Assert.Throws<ArgumentNullException>(() => _registryReader.GetPropertyValue(RegistryHive.CurrentUser, "", null));
        }
        #endregion

        #region GetWindowsRegistryKey() tests
        [Test]
        public void CanGetWindowsRegistryKey()
        {
            WindowsRegistryKey keyValue = _registryReader.GetWindowsRegistryKey(RegistryHive.ClassesRoot, @".dll\PersistentHandler", "");
            Assert.That(keyValue.Value, Is.EqualTo("{098f2470-bae0-11cd-b579-08002b30bfeb}"));
        }

        [Test]
        public void CanGetWindowsRegistryKeyByObject()
        {
            WindowsRegistryKey key = new WindowsRegistryKey
            {
                Hive = RegistryHive.ClassesRoot,
                Path = @".dll\PersistentHandler",
                Name = ""
            };

            WindowsRegistryKey keyValue = _registryReader.GetWindowsRegistryKey(key);
            Assert.That(keyValue.Value, Is.EqualTo("{098f2470-bae0-11cd-b579-08002b30bfeb}"));
        }

        [Test]
        public void CanGetWindowsRegistryKeyForKeyNotExists()
        {
            // No exception. Only null value
            WindowsRegistryKey keyValue = _registryReader.GetWindowsRegistryKey(RegistryHive.LocalMachine, @"Software\Testabcdefg", "abcdefg");
            Assert.That(keyValue.Value, Is.EqualTo(null));
        }

        [Test]
        public void GetWindowsRegistryThrowNotReadable()
        {
            WindowsRegistryKey key = new WindowsRegistryKey
            {
                Hive = RegistryHive.ClassesRoot,
            };

            Assert.Throws<InvalidOperationException>(() => _registryReader.GetWindowsRegistryKey(key));
        }
        #endregion

        #region GetRegistryEntries() + Recurse tests
        [Test]
        public void CanGetRegistryEntries()
        {
            List<WindowsRegistryKey> keys = _registryReader.GetRegistryEntries(RegistryHive.LocalMachine, @"HARDWARE\DESCRIPTION\System\BIOS");
            Assert.That(keys.Count, Is.Not.EqualTo(0));
        }

        [Test]
        public void CanGetRegistryEntriesRecurse()
        {
            List<WindowsRegistryKey> keys = _registryReader.GetRegistryEntryiesRecursive(RegistryHive.LocalMachine, @"SOFTWARE\Microsoft\Windows\Windows Search");
            Assert.That(keys.Count, Is.Not.EqualTo(0));
        }
        #endregion

        #region new WindowsRegistryKey() tests
        [Test]
        public void IsWindowsRegistryKeyValid()
        {
            // Tests property rules of WindowsRegistryKey
            WindowsRegistryKey key = new WindowsRegistryKey();

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => key.Hive = RegistryHive.CurrentUser);
                Assert.DoesNotThrow(() => key.ValueKind = RegistryValueKind.String);
                Assert.DoesNotThrow(() => key.Path = "Software");
                Assert.DoesNotThrow(() => key.Name = "NotThereButOK");
                //Assert.DoesNotThrow(() => key.Value = "Equal", "");
            });

        }
        [Test]
        public void WindowsRegistryKeyThrowHiveNullException()
        {
            WindowsRegistryKey key = new WindowsRegistryKey();
            Assert.Throws<ArgumentNullException>(() => key.Hive = 0, "Expected IsHiveValid to throw ArgumentNullException");
        }

        [Test]
        public void WindowsRegistryKeyThrowValueKindNullException()
        {
            WindowsRegistryKey key = new WindowsRegistryKey();
            Assert.Throws<ArgumentNullException>(() => key.ValueKind = 0, "Expected IsValueKindValid to throw ArgumentNullException");
        }
        [Test]
        public void WindowsRegistryKeyThrowPathNullException()
        {
            WindowsRegistryKey key = new WindowsRegistryKey();
            Assert.Throws<ArgumentNullException>(() => key.Path = null, "Expected IsPathValid to throw ArgumentNullException");
        }
        [Test]
        public void WindowsRegistryKeyThrowNameNullException()
        {
            WindowsRegistryKey key = new WindowsRegistryKey();
            Assert.Throws<ArgumentNullException>(() => key.Name = null, "Expected IsNameValid to throw ArgumentNullException");
        }
        #endregion

        #region SetRegistryValue() tests
        [Test]
         public void CanSetRegistryValue()
         {
            Assert.DoesNotThrow(() => _registryWriter.SetRegistryValue(RegistryHive.CurrentUser, @"SOFTWARE\mRemoteNGTest", "TestKey", "A value string", RegistryValueKind.String));
         }

        [Test]
        public void SetRegistryValueThrowAccessDenied()
        {
            Assert.Throws<UnauthorizedAccessException>(() => _registryWriter.SetRegistryValue(RegistryHive.LocalMachine, @"SOFTWARE\mRemoteNGTest", "TestKey", "A value string", RegistryValueKind.String));
        }
        #endregion
    }
}
