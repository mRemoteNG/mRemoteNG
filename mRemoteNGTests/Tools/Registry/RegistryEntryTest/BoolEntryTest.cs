
using Microsoft.Win32;
using NUnit.Framework.Internal;
using System.Runtime.Versioning;
using NUnit.Framework;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNGTests.Tools.Registry.RegistryEntryTest
{
    [SupportedOSPlatform("windows")]
    internal class BoolEntryTest
    {
        private const string TestRoot = @"Software\mRemoteNGTest";
        private const RegistryHive TestHive = RegistryHive.CurrentUser;
        private const string TestPath = $"{TestRoot}\\BoolEntryTest";

        [Test]
        public void StringTrue_SuccessfulToBool_ReturnsTrue()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<bool>.New(TestHive, TestPath, "IsTrueString", true).Write());
            var entry = WinRegistryEntry<bool>.New(TestHive, TestPath, "IsTrueString").Read();
            Assert.That(entry.Value, Is.True);
        }

        [Test]
        public void StringFalse_SuccessfulToBool_ReturnsFalse()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<bool>.New(TestHive, TestPath, "IsFalseString", false).Write());

            var entry = WinRegistryEntry<bool>.New(TestHive, TestPath, "IsFalseString");
            entry.Value = true; // change Value to true to ensure a value was readed
            entry.Read();

            Assert.That(entry.Value, Is.False);
        }

        [Test]
        public void DWordTrue_SuccessfulToBool_ReturnsTrue()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<bool>.New(TestHive, TestPath, "IsTrueDword", true).SetValueKind(RegistryValueKind.DWord).Write());
            var entry = WinRegistryEntry<bool>.New(TestHive, TestPath, "IsTrueDword").Read();
            Assert.That(entry.Value, Is.True);
        }

        [Test]
        public void DWordFalse_SuccessfulToBool_ReturnsFalse()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<bool>.New(TestHive, TestPath, "IsFalseDword", false).SetValueKind(RegistryValueKind.DWord).Write());
            var entry = WinRegistryEntry<bool>.New(TestHive, TestPath, "IsFalseDword");
            entry.Value = true; // change Value to true to ensure a value was readed
            entry.Read();
            Assert.That(entry.Value, Is.False);
        }

        [Test]
        public void FluentWrite_And_Read_DoesNotThrow_ReturnsBoolFalse()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<bool>.New(TestHive, TestPath, "FluentReadAndWriteTest", false).SetValueKind(RegistryValueKind.DWord).Write());
            var entry = WinRegistryEntry<bool>.New(TestHive, TestPath, "FluentReadAndWriteTest", true).Read();
            Assert.That(entry.Value, Is.False);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            // Delete all created tests
            WinRegistry winReg = new();
            winReg.DeleteTree(TestHive, TestRoot);
        }
    }
}
