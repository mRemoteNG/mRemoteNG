
using Microsoft.Win32;
using NUnit.Framework.Internal;
using System.Runtime.Versioning;
using System;
using NUnit.Framework;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNGTests.Tools.Registry.RegistryEntryTest
{
    [SupportedOSPlatform("windows")]
    internal class BaseRegistryEntryTest
    {
        private const string TestRoot = @"Software\mRemoteNGTest";
        private const RegistryHive TestHive = RegistryHive.CurrentUser;
        private const string TestPath = $"{TestRoot}\\EntryTests";

        [Test]
        public void Path_SetValidValue_GetReturnsSameValue()
        {
            string expectedPath = @"SOFTWARE\Microsoft";
            var entry = new WinRegistryEntry<string>
            {
                Path = expectedPath
            };
            Assert.That(expectedPath, Is.EqualTo(entry.Path));
        }

        [Test]
        public void Path_SetNullValue_ThrowsArgumentNullException()
        {
            var entry = new WinRegistryEntry<string>();
            Assert.Throws<ArgumentNullException>(() => entry.Path = null);
        }

        [Test]
        public void Name_SetValidValue_GetReturnsSameValue()
        {
            string expectedName = "Version";
            var entry = new WinRegistryEntry<string>
            {
                Name = expectedName
            };
            Assert.That(expectedName, Is.EqualTo(entry.Name));
        }

        [Test]
        public void Value_SetValidValue_GetReturnsSameValue()
        {
            string expectedValue = "1.0";
            var entry = new WinRegistryEntry<string>
            {
                Value = expectedValue
            };
            Assert.That(expectedValue, Is.EqualTo(entry.Value));
        }

        [Test]
        public void ValueKind_SetInvalidValue_ThrowsArgumentException()
        {
            var entry = new WinRegistryEntry<string>();
            Assert.Throws<ArgumentException>(() => entry.SetValueKind((RegistryValueKind)100));
        }

        [Test]
        public void IsSet_ValueHasBeenSet_NotRead_ReturnsFalse()
        {
            var entry = new WinRegistryEntry<string>
            {
                Value = "Test"
            };
            Assert.That(entry.IsSet, Is.False);
        }

        [Test]
        public void IsKeyReadable_AllPropertiesSet_ReturnsTrue()
        {
            var entry = new WinRegistryEntry<string>
            {
                Hive = RegistryHive.LocalMachine,
                Path = @"SOFTWARE\Microsoft",
                Name = "Version"
            };
            Assert.That(entry.IsReadable, Is.True);
        }

        [Test]
        public void IsKeyWritable_AllPropertiesSet_ReturnsTrue()
        {
            var entry = new WinRegistryEntry<string>
            {
                Hive = RegistryHive.LocalMachine,
                Path = @"SOFTWARE\Microsoft",
                Name = "Version",
                Value = "1.0"
            };
            Assert.That(entry.IsWritable, Is.True);
        }

        [Test]
        public void Read_WhenRegistryKeyIsNull_ThrowsInvalidOperationException()
        {
            var entry = new WinRegistryEntry<string>();
            Assert.Throws<InvalidOperationException>(() => entry.Read());
        }

        [Test]
        public void Write_WhenKeyIsUnwritable_ThrowsInvalidOperationException()
        {
            var entry = new WinRegistryEntry<string>
            {
                Name = "Version"
            };
            Assert.Throws<InvalidOperationException>(() => entry.Write());
        }

        [Test]
        public void WriteDefaultAndReadDefault_Entry_DoesNotThrowAndReadsCorrectly()
        {
            var entry = new WinRegistryEntry<int>(TestHive, TestPath, 0)
            {
                Hive = TestHive,
                Path = TestPath,
                Value = 0,
            };

            var readEntry = new WinRegistryEntry<int>
            {
                Hive = TestHive,
                Path = TestPath,
            };

            Assert.That(() => entry.Write(), Throws.Nothing);
            Assert.That(() => readEntry.Read(), Throws.Nothing);
            Assert.Multiple(() =>
            {
                Assert.That(readEntry.ValueKind, Is.EqualTo(RegistryValueKind.String));
                Assert.That(readEntry.Value, Is.EqualTo(entry.Value));
            });
        }

        [Test]
        public void WriteAndRead_Entry_DoesNotThrowAndReadsCorrectly()
        {
            var entry = new WinRegistryEntry<long>
            {
                Hive = TestHive,
                Path = TestPath,
                Name = "TestRead",
                Value = 200
            };

            var readEntry = new WinRegistryEntry<long>
            {
                Hive = TestHive,
                Path = TestPath,
                Name = "TestRead"
            };

            Assert.That(() => entry.Write(), Throws.Nothing);
            Assert.That(() => readEntry.Read(), Throws.Nothing);
            Assert.Multiple(() =>
            {
                Assert.That(readEntry.ValueKind, Is.EqualTo(entry.ValueKind));
                Assert.That(readEntry.Value, Is.EqualTo(entry.Value));
            });
        }

        [Test]
        public void FluentWriteAndRead_DoesNotThrow_ReturnsStrJustATest()
        {
            Assert.DoesNotThrow(() => WinRegistryEntry<string>.New(TestHive, TestPath, "FluentReadAndWriteTest", "JustATest").Write());
            var entry = WinRegistryEntry<string>.New(TestHive, TestPath, "FluentReadAndWriteTest").Read();
            Assert.That(entry.Value, Is.EqualTo("JustATest"));
        }

        [Test]
        public void FluentWriteReadAndChange_DoesNotThrow_WriteReadsCorrectly()
        {
            var entry = WinRegistryEntry<string>
                .New(TestHive, TestPath, "FluentReadWriteAndChangeTest", "JustASecondTest")
                .Write()
                .Read();
            string result1 = entry.Value;

            string result2 = entry
                .Write("JustAChangeTest")
                .Read()
                .Value;

            Assert.Multiple(() =>
            {
                Assert.That(result1, Is.EqualTo("JustASecondTest"));
                Assert.That(result2, Is.EqualTo("JustAChangeTest"));
            });
        }

        [Test]
        public void Read_LockAfter_IsLocked()
        {
            var entry = WinRegistryEntry<string>
                .New(TestHive, TestPath, "IsLockedAfter", "After")
                .Write()
                .Read()
                .Lock();

            Assert.Multiple(() =>
            {
                Assert.That(entry.IsLocked, Is.EqualTo(true));
            });
        }

        [Test]
        public void Read_LockBevore_IsLocked()
        {
            var entry = WinRegistryEntry<string>
                .New(TestHive, TestPath, "IsLockedBevore", "Bevore")
                .Write()
                .Read()
                .Lock();

            Assert.Multiple(() =>
            {
                Assert.That(entry.IsLocked, Is.EqualTo(true));
            });
        }

        [Test]
        public void Read_Lock_ThrowWhenRead()
        {
            var entry = WinRegistryEntry<string>
                .New(TestHive, TestPath, "ReadLockThrow", "ReadingIsLocked")
                .Write()
                .Read()
                .Lock();

            Assert.Throws<InvalidOperationException>(() => entry.Read());
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
