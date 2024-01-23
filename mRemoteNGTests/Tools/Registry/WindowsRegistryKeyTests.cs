using Microsoft.Win32;
using mRemoteNG.Tools.WindowsRegistry;
using NUnit.Framework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace mRemoteNGTests.Tools.Registry
{
    internal class WindowsRegistryKeyTests
    {
        private WindowsRegistryKey CompleteRegistryKey { get; set; }
        private WindowsRegistryKey PartialRegistryKey { get; set; }

        [SetUp]
        public void Setup()
        {
            CompleteRegistryKey = new WindowsRegistryKey()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.String,
                Name = "Test",
                Path = @"SOFTWARE\TEST\TEST\Test",
                Value = "CompleteRegistryKey"
            };

            PartialRegistryKey = new WindowsRegistryKey()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
            };
        }

        #region WindowsRegistryKey() tests
        [Test]
        public void WindowsRegistryKeyReadable()
        {
            Assert.That(CompleteRegistryKey.IsKeyReadable, Is.EqualTo(true));
        }

        [Test]
        public void WindowsRegistryKeyNotReadable()
        {
            Assert.That(PartialRegistryKey.IsKeyReadable, Is.EqualTo(false));
        }

        [Test]
        public void WindowsRegistryKeyWriteable()
        {
            Assert.That(CompleteRegistryKey.IsKeyWritable, Is.EqualTo(true));
        }

        [Test]
        public void WindowsRegistryKeyNotWriteable()
        {
            Assert.That(PartialRegistryKey.IsKeyWritable, Is.EqualTo(false));
        }

        [Test]
        public void WindowsRegistryKeyProvided()
        {
            Assert.That(CompleteRegistryKey.IsKeyPresent, Is.EqualTo(true));
        }

        [Test]
        public void WindowsRegistryKeyNotProvided()
        {
            Assert.That(PartialRegistryKey.IsKeyPresent, Is.EqualTo(false));
        }
        #endregion

        #region WindowsRegistryKeyBoolean tests
        [Test]
        public void WindowsRegistryKeyBoolean_FromStringTrue()
        {

            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.String,
                Name = "TestBoolString",
                Path = @"SOFTWARE\Test",
                Value = "true"
            };

            WindowsRegistryKeyBoolean boolKey = new ();
            boolKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(boolKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(boolKey.Value, Is.EqualTo(true)); 
        }

        [Test]
        public void WindowsRegistryKeyBoolean_FromStringFalse()
        {

            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.String,
                Name = "TestBoolString",
                Path = @"SOFTWARE\Test",
                Value = "false"
            };

            WindowsRegistryKeyBoolean boolKey = new();
            boolKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(boolKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(boolKey.Value, Is.EqualTo(false));
        }

        [Test]
        public void WindowsRegistryKeyBoolean_FromDwordTrue()
        {

            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestBoolString",
                Path = @"SOFTWARE\Test",
                Value = "1"
            };

            WindowsRegistryKeyBoolean boolKey = new();
            boolKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(boolKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(boolKey.Value, Is.EqualTo(true));
        }

        [Test]
        public void WindowsRegistryKeyBoolean_FromDwordFalse()
        {

            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestBoolString",
                Path = @"SOFTWARE\Test",
                Value = "0"
            };

            WindowsRegistryKeyBoolean boolKey = new();
            boolKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(boolKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(boolKey.Value, Is.EqualTo(false));
        }

        [Test]
        public void WindowsRegistryKeyBoolean_ReturnDefault()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestBoolString",
                Path = @"SOFTWARE\Test",
                Value = null
            };

            WindowsRegistryKeyBoolean boolKey = new();
            boolKey.ConvertFromWindowsRegistryKey(TestKey, true);

            Assert.That(boolKey.IsKeyPresent, Is.EqualTo(false));
            Assert.That(boolKey.Value, Is.EqualTo(true));
        }
        #endregion

        #region WindowsRegistryKeyInteger tests
        [Test]
        public void WindowsRegistryKeyInteger()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestIntigerString",
                Path = @"SOFTWARE\Test",
                Value = "4711"
            };

            WindowsRegistryKeyInteger IntKey = new();
            IntKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(IntKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(IntKey.Value, Is.EqualTo(4711));
        }
        [Test]
        public void WindowsRegistryKeyInteger_ReturnDefault()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestIntigerString",
                Path = @"SOFTWARE\Test",
                Value = null
            };

            WindowsRegistryKeyInteger IntKey = new();
            IntKey.ConvertFromWindowsRegistryKey(TestKey, 2096);

            Assert.That(IntKey.IsKeyPresent, Is.EqualTo(false));
            Assert.That(IntKey.Value, Is.EqualTo(2096));
        }
        #endregion

        #region WindowsRegistryKeyString tests
        [Test]
        public void WindowsRegistryKeyString()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestRegString",
                Path = @"SOFTWARE\Test",
                Value = "The Big Bang Theory"
            };

            WindowsRegistryKeyString StrKey = new();
            StrKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(StrKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(StrKey.Value, Is.EqualTo("The Big Bang Theory"));
        }
        [Test]
        public void WindowsRegistryKeyString_ReturnDefault()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestRegString",
                Path = @"SOFTWARE\Test",
                Value = null
            };

            WindowsRegistryKeyString StrKey = new();
            StrKey.ConvertFromWindowsRegistryKey(TestKey, "South Park");

            Assert.That(StrKey.IsKeyPresent, Is.EqualTo(false));
            Assert.That(StrKey.Value, Is.EqualTo("South Park"));
        }

        [Test]
        public void WindowsRegistryKeyString_ValidateSuccess()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestRegString",
                Path = @"SOFTWARE\Test",
                Value = "Big Bang"
            };

            WindowsRegistryKeyString StrKey = new();
            StrKey.AllowedValues = new[] { "Big Bang", "Big Bang Theory", "The Big Bang Theory" };
            StrKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(StrKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(StrKey.IsValid, Is.EqualTo(true));
        }

        [Test]
        public void WindowsRegistryKeyString_ValidateNotSuccess()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestRegString",
                Path = @"SOFTWARE\Test",
                Value = "ig ang"
            };

            WindowsRegistryKeyString StrKey = new();
            StrKey.AllowedValues = new[] { "Big Bang", "Big Bang Theory", "The Big Bang Theory" };
            StrKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(StrKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(StrKey.IsValid, Is.EqualTo(false));
        }

        [Test]
        public void WindowsRegistryKeyString_ValidateSuccessCase()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestRegString",
                Path = @"SOFTWARE\Test",
                Value = "BiG BAng"
            };

            WindowsRegistryKeyString StrKey = new();
            StrKey.AllowedValues = new[] { "BiG BAng", "Big Bang Theory", "The Big Bang Theory" };
            StrKey.IsCaseSensitiveValidation = true;
            StrKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(StrKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(StrKey.IsValid, Is.EqualTo(true));
        }

        [Test]
        public void WindowsRegistryKeyString_ValidateNotSuccessCase()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestRegString",
                Path = @"SOFTWARE\Test",
                Value = "BiG BAng"
            };

            WindowsRegistryKeyString StrKey = new();
            StrKey.AllowedValues = new[] { "Big Bang", "Big Bang Theory", "The Big Bang Theory" };
            StrKey.IsCaseSensitiveValidation = true;
            StrKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(StrKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(StrKey.IsValid, Is.EqualTo(false));
        }

        [Test]
        public void WindowsRegistryKeyString_ValidateNotSuccessReturnNull()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestRegString",
                Path = @"SOFTWARE\Test",
                Value = "ig ang"
            };

            WindowsRegistryKeyString StrKey = new();
            StrKey.AllowedValues = new[] { "Big Bang", "Big Bang Theory", "The Big Bang Theory" };
            StrKey.ConvertFromWindowsRegistryKey(TestKey);

            Assert.That(StrKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(StrKey.IsValid, Is.EqualTo(false));
            Assert.That(StrKey.Value, Is.EqualTo(null));
        }

        [Test]
        public void WindowsRegistryKeyString_ValidateNotSuccessValidValue()
        {
            WindowsRegistryKey TestKey = new()
            {
                Hive = RegistryHive.CurrentUser,
                ValueKind = RegistryValueKind.DWord,
                Name = "TestRegString",
                Path = @"SOFTWARE\Test",
                Value = "ig ang"
            };

            WindowsRegistryKeyString StrKey = new();
            StrKey.AllowedValues = new[] { "Big Bang", "Big Bang Theory", "The Big Bang Theory" };
            StrKey.ConvertFromWindowsRegistryKey(TestKey, "Big Bang Theory");

            Assert.That(StrKey.IsKeyPresent, Is.EqualTo(true));
            Assert.That(StrKey.IsValid, Is.EqualTo(false));
            Assert.That(StrKey.Value, Is.EqualTo("Big Bang Theory"));
        }

        #endregion
    }
}
