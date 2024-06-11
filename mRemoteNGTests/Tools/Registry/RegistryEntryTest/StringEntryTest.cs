
using Microsoft.Win32;
using NUnit.Framework.Internal;
using System;
using System.Runtime.Versioning;
using NUnit.Framework;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNGTests.Tools.Registry.RegistryEntryTest
{
    [SupportedOSPlatform("windows")]
    internal class StringEntryTest
    {
        private const string TestRoot = @"Software\mRemoteNGTest";
        private const RegistryHive TestHive = RegistryHive.CurrentUser;
        private const string TestPath = $"{TestRoot}\\StringEntryTest";

        public enum TestEnum
        {
            First = 1,
            Second = 2,
            Third = 3
        }

        [Test]
        public void IsValid_NoValidationSet_EntryComplete_ReturnsTrue()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<string>.New(TestHive, TestPath, "IsValid", "IsValid").Write());
            var entry = WinRegistryEntry<string>.New(TestHive, TestPath, "IsValid").Read();
            Assert.That(entry.IsValid, Is.True);
        }

        [Test]
        public void IsValid_AllowedValuesSet_ValueInAllowedValues_ReturnsTrue()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<string>.New(TestHive, TestPath, "ArrayIsValid", "Banana").Write());
            var entry = WinRegistryEntry<string>.New(TestHive, TestPath, "ArrayIsValid").SetValidation(new string[] { "Banana", "Strawberry", "Apple" }).Read();
            Assert.That(entry.IsValid, Is.True);
        }
        
        [Test]
        public void IsValid_AllowedValuesSet_ValueNotInAllowedValues_ReturnsFalse()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<string>.New(TestHive, TestPath, "ArrayIsInValid", "Cheese").Write());
            var entry = WinRegistryEntry<string>.New(TestHive, TestPath, "ArrayIsInValid").SetValidation(new string[] { "Banana", "Strawberry", "Apple" }).Read();
            Assert.That(entry.IsValid, Is.False);
        }

        [Test]
        public void IsValid_AllowedValuesSet_CorrectsValueSpellingAndValidatesSuccessfully()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<string>.New(TestHive, TestPath, "ArrayCorrectsSpellingIsValid", "StrawBerry").Write());
            var entry = WinRegistryEntry<string>.New(TestHive, TestPath, "ArrayCorrectsSpellingIsValid").SetValidation(new string[] { "Banana", "Strawberry", "Apple" }).Read();

            Assert.Multiple(() =>
            {
                Assert.That(entry.IsValid, Is.True);
                Assert.That(entry.Value, Is.EqualTo("Strawberry"));
            });
        }

        [Test]
        public void IsValid_EnumSet_ValueInEnum_ReturnsTrue()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<string>.New(TestHive, TestPath, "IsValid", "Second").Write());
            var entry = WinRegistryEntry<string>.New(TestHive, TestPath, "IsValid").SetValidation<TestEnum>().Read();
            Assert.That(entry.IsValid, Is.True);
        }

        [Test]
        public void IsValid_EnumSet_ValueNotInEnum_ReturnsFalse()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<string>.New(TestHive, TestPath, "IsInValid", "Fourth").Write());
            var entry = WinRegistryEntry<string>.New(TestHive, TestPath, "IsInValid").SetValidation<TestEnum>().Read();
            Assert.That(entry.IsValid, Is.False);
        }

        [Test]
        public void IsValid_EnumSet_CorrectsValueSpellingAndValidatesSuccessfully()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<string>.New(TestHive, TestPath, "IsValidCorrectsSpelling", "SecOND").Write());
            var entry = WinRegistryEntry<string>.New(TestHive, TestPath, "IsValidCorrectsSpelling").SetValidation<TestEnum>().Read();
            Assert.Multiple(() =>
            {
                Assert.That(entry.IsValid, Is.True);
                Assert.That(entry.Value, Is.EqualTo("Second"));
            });
        }

        [Test]
        public void IsValid_InvalidValueKind_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => WinRegistryEntry<string>.New(TestHive, TestPath, "IsValid", "Windows").SetValueKind(RegistryValueKind.Unknown));
        }

        [Test]
        public void FluentWrite_And_Read_DoesNotThrow_ReturnsStrJustATest()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<string>.New(TestHive, TestPath, "FluentReadAndWriteTest", "JustATest").Write());
            var entry = WinRegistryEntry<string>.New(TestHive, TestPath, "FluentReadAndWriteTest", "TestFailed").Read();
            Assert.That(entry.Value, Is.EqualTo("JustATest"));
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
