using Microsoft.Win32;
using mRemoteNG.Tools.WindowsRegistry;
using NUnit.Framework;

namespace mRemoteNGTests.Tools.Registry
{
    internal class WindowsRegistryAdvancedTests : WindowsRegistryAdvanced
    {
        private const string _TestRootKey = @"Software\mRemoteNGTest";
        private const RegistryHive _TestHive = RegistryHive.CurrentUser;

        [SetUp]
        public void Setup() 
        {
            // GetBoolean && GetBoolValue (GetBoolValue -> Not Advanced but not tested jet)
            SetRegistryValue(_TestHive, _TestRootKey, "TestBoolAsString", "true", RegistryValueKind.String);
            SetRegistryValue(_TestHive, _TestRootKey, "TestBoolAsDWord", 0, RegistryValueKind.DWord);

            // GetInteger Tests
            SetRegistryValue(_TestHive, _TestRootKey, "TestInteger", "4711", RegistryValueKind.DWord);

            // GetString Tests
            SetRegistryValue(_TestHive, _TestRootKey, "TestString1", "Banane", RegistryValueKind.String);
            SetRegistryValue(_TestHive, _TestRootKey, "TestString2", "Hund", RegistryValueKind.String);
        }

        [TearDown]
        public void Cleanup()
        {
            // Delete the registry keys here
            DeleteRegistryKey(_TestHive, _TestRootKey, true);
        }

        #region GetBoolean() Tests
        // Non object returns
        [Test]
        public void GetBooleanFromString_ReturnsTrue()
        {
            var key = GetBoolean(_TestHive, _TestRootKey, "TestBoolAsString");
            Assert.That(key.Value, Is.EqualTo(true));
        }

        
        [Test]
        public void GetBooleanFromDword_ReturnsFalse()
        {
            var key = GetBoolean(_TestHive, _TestRootKey, "TestBoolAsDWord");
            Assert.That(key.Value, Is.EqualTo(false));
        }

        [Test]
        public void GetBooleanNotProvided_ReturnsDefaultTrue()
        {
            var key = GetBoolean(_TestHive, _TestRootKey, "TestBoolNotProvided", true);
            Assert.Multiple(() =>
            {
                Assert.That(key.Value, Is.EqualTo(true), "Value should be the default (true)");
                Assert.That(key.IsKeyPresent, Is.EqualTo(false), "IsProvided should be false");
            });
        }
        #endregion

        #region GetBoolValue()___No Object, just bool value returns
        [Test]
        public void GetBoolValueFromString_ReturnsTrue()
        {
            var key = GetBoolValue(_TestHive, _TestRootKey, "TestBoolAsString");
            Assert.That(key, Is.EqualTo(true));
        }
        [Test]
        public void GetBoolValueFromDword_ReturnsFalse()
        {
            var key = GetBoolValue(_TestHive, _TestRootKey, "TestBoolAsDWord", true);
            Assert.That(key, Is.EqualTo(false));
        }
        [Test]
        public void GetBoolValue_ReturnsDefaultTrue()
        {
            var key = GetBoolValue(_TestHive, _TestRootKey, "TestBoolNotProvided", true);
            Assert.That(key, Is.EqualTo(true));
        }
        #endregion

        #region GetInteger()
        [Test]
        public void GetInteger()
        {
            var key = GetInteger(_TestHive, _TestRootKey, "TestInteger");
            Assert.Multiple(() =>
            {
                Assert.That(key.Value, Is.EqualTo(4711));
                Assert.That(key.IsKeyPresent, Is.EqualTo(true));
            });
        }
        [Test]
        public void GetInteger_returnObjectDefault()
        {
            var key = GetInteger(_TestHive, _TestRootKey, "TestIntegerNotProvided");
            Assert.Multiple(() =>
            {
                Assert.That(key.Value, Is.EqualTo(-1), "Value should be the default (-1)");
                Assert.That(key.IsKeyPresent, Is.EqualTo(false));
            });
        }

        [Test]
        public void GetInteger_returnSpecifiedDefault()
        {
            var key = GetInteger(_TestHive, _TestRootKey, "TestIntegerNotProvided", 2096);
            Assert.Multiple(() =>
            {
                Assert.That(key.Value, Is.EqualTo(-1), "Value should be the default (-1)");
                Assert.That(key.IsKeyPresent, Is.EqualTo(false));
            });
        }

        [Test]
        public void GetDwordValue_returnIntegerValue()
        {
            int value = GetDwordValue(_TestHive, _TestRootKey, "TestInteger");
            Assert.That(value, Is.EqualTo(4711));
        }
        #endregion

        #region GetString()
        [Test]
        public void GetString()
        {
            var key = GetString(_TestHive, _TestRootKey, "TestString1");
            Assert.Multiple(() =>
            {
                Assert.That(key.Value, Is.EqualTo("Banane"));
                Assert.That(key.IsKeyPresent, Is.EqualTo(true));
            });
        }

        [Test]
        public void GetString_ReturnsDefault()
        {
            var key = GetString(_TestHive, _TestRootKey, "TestStringNotProvided", "Banane");
            Assert.Multiple(() =>
            {
                Assert.That(key.Value, Is.EqualTo("Banane"));
                Assert.That(key.IsKeyPresent, Is.EqualTo(false));
            });
        }

        [Test]
        public void GetStringValidated_Valid()
        {
            string[] fruits = { "Banane", "Erdbeere", "Apfel" };
            var key = GetStringValidated(_TestHive, _TestRootKey, "TestString1", fruits);
            Assert.Multiple(() =>
            {
                Assert.That(key.Value, Is.EqualTo("Banane"));
                Assert.That(key.IsKeyPresent, Is.EqualTo(true));
                Assert.That(key.IsValid, Is.EqualTo(true));
            });
        }

        [Test]
        public void GetStringValidated_NotValidNull()
        {
            string[] fruits = { "Banane", "Erdbeere", "Apfel" };
            var key = GetStringValidated(_TestHive, _TestRootKey, "TestString2", fruits);
            Assert.Multiple(() =>
            {
                Assert.That(key.Value, Is.EqualTo(null));
                Assert.That(key.IsKeyPresent, Is.EqualTo(true));
                Assert.That(key.IsValid, Is.EqualTo(false));
            });
        }

        [Test]
        public void GetStringValidated_NotValidDefault()
        {
            string[] fruits = { "Banane", "Erdbeere", "Apfel" };
            var key = GetStringValidated(_TestHive, _TestRootKey, "TestString2", fruits, false, "Banane");
            Assert.Multiple(() =>
            {
                Assert.That(key.Value, Is.EqualTo("Banane"));
                Assert.That(key.IsKeyPresent, Is.EqualTo(true));
                Assert.That(key.IsValid, Is.EqualTo(false));
            });
        }
        #endregion
    }
}
