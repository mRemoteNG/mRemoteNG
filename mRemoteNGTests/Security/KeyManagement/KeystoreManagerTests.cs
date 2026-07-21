using System;
using System.IO;
using System.Security;
using mRemoteNG.Security;
using mRemoteNG.Security.KeyManagement;
using NUnit.Framework;

namespace mRemoteNGTests.Security.KeyManagement
{
    public class KeystoreManagerTests
    {
        private string _keystorePath;
        private KeystoreManager _manager;
        private SecureString _password;

        [SetUp]
        public void Setup()
        {
            _keystorePath = Path.Combine(Path.GetTempPath(), $"mremoteng_keystore_test_{Guid.NewGuid()}.json");
            _manager = new KeystoreManager(_keystorePath);
            _password = "TestPassword123!".ConvertToSecureString();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_keystorePath))
                File.Delete(_keystorePath);
        }

        [Test]
        public void Exists_ReturnsFalse_WhenNoKeystoreFile()
        {
            Assert.That(_manager.Exists, Is.False);
        }

        [Test]
        public void CreateKeystore_CreatesFileAndReturnsDek()
        {
            var dekHex = _manager.CreateKeystore(_password);
            Assert.That(_manager.Exists, Is.True);
            Assert.That(dekHex, Is.Not.Null.And.Not.Empty);
            Assert.That(dekHex.Length, Is.EqualTo(64)); // 32 bytes = 64 hex chars
        }

        [Test]
        public void UnwrapDek_ReturnsCorrectDek_WithCorrectPassword()
        {
            var originalDek = _manager.CreateKeystore(_password);
            var unwrappedDek = _manager.UnwrapDek(_password);
            Assert.That(unwrappedDek, Is.EqualTo(originalDek));
        }

        [Test]
        public void UnwrapDek_ReturnsNull_WithWrongPassword()
        {
            _manager.CreateKeystore(_password);
            var wrongPassword = "WrongPassword!".ConvertToSecureString();
            var result = _manager.UnwrapDek(wrongPassword);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UnwrapDek_ReturnsNull_WhenNoKeystoreExists()
        {
            var result = _manager.UnwrapDek(_password);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ChangePassword_AllowsUnwrapWithNewPassword()
        {
            var dekHex = _manager.CreateKeystore(_password);
            var newPassword = "NewPassword456!".ConvertToSecureString();

            _manager.ChangePassword(dekHex, newPassword);

            var unwrapped = _manager.UnwrapDek(newPassword);
            Assert.That(unwrapped, Is.EqualTo(dekHex));
        }

        [Test]
        public void ChangePassword_OldPasswordNoLongerWorks()
        {
            var dekHex = _manager.CreateKeystore(_password);
            var newPassword = "NewPassword456!".ConvertToSecureString();

            _manager.ChangePassword(dekHex, newPassword);

            var result = _manager.UnwrapDek(_password);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void AddWrappedKey_AllowsMultipleProviders()
        {
            var dekHex = _manager.CreateKeystore(_password);
            var secondKey = "SecondProvider!".ConvertToSecureString();

            _manager.AddWrappedKey(dekHex, secondKey, "dpapi");

            // Both should work
            var fromPassword = _manager.UnwrapDek(_password);
            var fromDpapi = _manager.UnwrapDek(secondKey);

            Assert.That(fromPassword, Is.EqualTo(dekHex));
            Assert.That(fromDpapi, Is.EqualTo(dekHex));
        }

        [Test]
        public void RemoveProvider_RemovesSpecificEntry()
        {
            var dekHex = _manager.CreateKeystore(_password);
            var secondKey = "SecondKey!".ConvertToSecureString();
            _manager.AddWrappedKey(dekHex, secondKey, "dpapi");

            _manager.RemoveProvider("dpapi");

            // Password should still work
            var fromPassword = _manager.UnwrapDek(_password);
            Assert.That(fromPassword, Is.EqualTo(dekHex));

            // dpapi should not work
            var fromDpapi = _manager.UnwrapDek(secondKey);
            Assert.That(fromDpapi, Is.Null);
        }

        [Test]
        public void CreateKeystore_ThrowsOnNullPassword()
        {
            Assert.Throws<ArgumentException>(() => _manager.CreateKeystore(null));
        }

        [Test]
        public void CreateKeystore_ThrowsOnEmptyPassword()
        {
            var empty = new SecureString();
            Assert.Throws<ArgumentException>(() => _manager.CreateKeystore(empty));
        }

        [Test]
        public void DekIsConsistent_AcrossMultipleUnwraps()
        {
            var dekHex = _manager.CreateKeystore(_password);
            var unwrap1 = _manager.UnwrapDek(_password);
            var unwrap2 = _manager.UnwrapDek(_password);
            Assert.That(unwrap1, Is.EqualTo(dekHex));
            Assert.That(unwrap2, Is.EqualTo(dekHex));
        }
    }
}
