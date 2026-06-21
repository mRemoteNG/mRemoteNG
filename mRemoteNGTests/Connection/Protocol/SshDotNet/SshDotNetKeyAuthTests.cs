using System.IO;
using System.Linq;
using mRemoteNG.Connection.Protocol.SshDotNet;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;
using Renci.SshNet;

namespace mRemoteNGTests.Connection.Protocol.SshDotNet
{
    /// <summary>
    /// Key-based authentication tests using throwaway keys generated at test time (no committed key
    /// material). Covers the auth matrix: unencrypted / passphrase-protected / wrong passphrase /
    /// missing passphrase / key+password ordering.
    /// </summary>
    [TestFixture]
    [Category("Unit")]
    public class SshDotNetKeyAuthTests
    {
        private const string Passphrase = "test-passphrase-42";
        private string _unencryptedKeyPath;
        private string _encryptedKeyPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unencryptedKeyPath = TestKeyFactory.WriteToTempFile(TestKeyFactory.GenerateRsaPrivateKeyPem());
            _encryptedKeyPath = TestKeyFactory.WriteToTempFile(TestKeyFactory.GenerateRsaPrivateKeyPem(Passphrase));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (_unencryptedKeyPath != null && File.Exists(_unencryptedKeyPath)) File.Delete(_unencryptedKeyPath);
            if (_encryptedKeyPath != null && File.Exists(_encryptedKeyPath)) File.Delete(_encryptedKeyPath);
        }

        // --- format sanity ---

        [Test]
        public void GeneratedUnencryptedKey_LoadsThroughSshNet()
        {
            Assert.DoesNotThrow(() => { using var pk = new PrivateKeyFile(_unencryptedKeyPath); });
        }

        [Test]
        public void GeneratedEncryptedKey_LoadsThroughSshNet_WithPassphrase()
        {
            Assert.DoesNotThrow(() => { using var pk = new PrivateKeyFile(_encryptedKeyPath, Passphrase); });
        }

        // --- CreatePrivateKeyAuth matrix ---

        [Test]
        public void CreatePrivateKeyAuth_LoadsUnencryptedKey()
        {
            var auth = SshAuthenticationProvider.CreatePrivateKeyAuth("user", _unencryptedKeyPath);
            Assert.That(auth, Is.TypeOf<PrivateKeyAuthenticationMethod>());
            Assert.That(auth.Username, Is.EqualTo("user"));
        }

        [Test]
        public void CreatePrivateKeyAuth_LoadsEncryptedKey_WithCorrectPassphrase()
        {
            var auth = SshAuthenticationProvider.CreatePrivateKeyAuth("user", _encryptedKeyPath, Passphrase);
            Assert.That(auth, Is.TypeOf<PrivateKeyAuthenticationMethod>());
        }

        [Test]
        public void CreatePrivateKeyAuth_Throws_WithWrongPassphrase()
        {
            Assert.That(() => SshAuthenticationProvider.CreatePrivateKeyAuth("user", _encryptedKeyPath, "wrong-passphrase"),
                Throws.Exception);
        }

        [Test]
        public void CreatePrivateKeyAuth_Throws_WhenPassphraseMissingForEncryptedKey()
        {
            Assert.That(() => SshAuthenticationProvider.CreatePrivateKeyAuth("user", _encryptedKeyPath),
                Throws.Exception);
        }

        // --- GetAuthenticationMethods wiring/ordering ---

        [Test]
        public void GetAuthenticationMethods_AddsKeyAuthFirst_WhenKeyAndPasswordConfigured()
        {
            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo { SshDotNetPrivateKeyFile = _unencryptedKeyPath };

            var methods = SshAuthenticationProvider.GetAuthenticationMethods("user", "pass", connectionInfo);

            Assert.That(methods[0], Is.TypeOf<PrivateKeyAuthenticationMethod>(), "Key auth should be tried first (OpenSSH behaviour)");
            Assert.That(methods.Any(m => m is PasswordAuthenticationMethod), Is.True, "Password method should also be present");
        }

        [Test]
        public void GetAuthenticationMethods_AddsKeyAuth_WithEncryptedKeyAndPassphrase()
        {
            var connectionInfo = new mRemoteNG.Connection.ConnectionInfo
            {
                SshDotNetPrivateKeyFile = _encryptedKeyPath,
                SshDotNetPrivateKeyPassphrase = Passphrase
            };

            var methods = SshAuthenticationProvider.GetAuthenticationMethods("user", null, connectionInfo);

            Assert.That(methods.Any(m => m is PrivateKeyAuthenticationMethod), Is.True);
            Assert.That(methods.Any(m => m is PasswordAuthenticationMethod), Is.False, "No password configured");
        }
    }
}
