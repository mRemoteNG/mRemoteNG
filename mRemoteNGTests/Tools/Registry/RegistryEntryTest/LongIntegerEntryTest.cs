
using Microsoft.Win32;
using NUnit.Framework.Internal;
using System;
using System.Runtime.Versioning;
using NUnit.Framework;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNGTests.Tools.Registry.RegistryEntryTest
{
    [SupportedOSPlatform("windows")]
    internal class LongIntegerEntryTest
    {
        private const string TestRoot = @"Software\mRemoteNGTest";
        private const RegistryHive TestHive = RegistryHive.CurrentUser;
        private const string TestPath = $"{TestRoot}\\LongIntegerEntryTest";

        [Test]
        public void IsValid_NoValidationSet_EntryComplete_ReturnsTrue()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid", 3047483647).Write());
            var entry = WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid").Read();
            Assert.That(entry.IsValid, Is.True);
        }

        [Test]
        public void IsValid_AllowedValuesSet_ValueInAllowedValues_ReturnsTrue()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid", 2147483649).Write());
            var entry = WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid").SetValidation(new long[] { 2147483648, 2147483649, 2147483650 }).Read();
            Assert.That(entry.IsValid, Is.True);
        }
        
        [Test]
        public void IsValid_AllowedValuesSet_ValueNotInAllowedValues_ReturnsFalse()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid", 4).Write());
            var entry = WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid").SetValidation(new long[] { 2147483648, 2147483649, 2147483650 }).Read();
            Assert.That(entry.IsValid, Is.False);
        }

        [Test]
        public void IsValid_RangeSet_ValueInRange_ReturnsTrue()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid", 2147483652).Write());
            var entry = WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid").SetValidation(2147483647, 2200000000).Read();
            Assert.That(entry.IsValid, Is.True);
        }

        [Test]
        public void IsValid_RangeSet_ValueOutOfRange_ReturnsFalse()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid", 20).Write());
            var entry = WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid").SetValidation(2147483647, 2200000000).Read();
            Assert.That(entry.IsValid, Is.False);
        }

        [Test]
        public void IsValid_InvalidValueKind_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid", 5).SetValueKind(RegistryValueKind.Unknown));
        }

        [Test]
        public void Value_SetToNegativeNumber_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => WinRegistryEntry<long>.New(TestHive, TestPath, "IsValid", -100));
        }

        [Test]
        public void FluentWrite_And_Read_DoesNotThrow_ReturnsLong15()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<long>.New(TestHive, TestPath, "FluentReadAndWriteTest", 15).Write());
            var entry = WinRegistryEntry<long>.New(TestHive, TestPath, "FluentReadAndWriteTest", 42).Read();
            Assert.That(entry.Value, Is.EqualTo(15));
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
