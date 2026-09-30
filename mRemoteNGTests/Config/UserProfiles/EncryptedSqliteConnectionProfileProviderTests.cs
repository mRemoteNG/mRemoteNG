using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using mRemoteNG.Config.UserProfiles;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Config.UserProfiles
{
    [SupportedOSPlatform("windows")]
    public class EncryptedSqliteConnectionProfileProviderTests
    {
        private string _databasePath;

        [SetUp]
        public void SetUp()
        {
            _databasePath = Path.Combine(Path.GetTempPath(), $"mremoteng_connection_profile_{Guid.NewGuid()}.db");
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_databasePath))
                File.Delete(_databasePath);
        }

        [Test]
        public void SaveAndLoad_RoundTripsConnectionsWithoutStoringPlaintext()
        {
            const string connections = "<Connections><Node Name=\"Sensitive server\" /></Connections>";
            using (EncryptedSqliteConnectionProfileProvider provider = CreateProvider(ProfileAccessLevel.Write))
            {
                provider.Save(connections);
                Assert.That(provider.Load(), Is.EqualTo(connections));
            }

            byte[] databaseBytes = File.ReadAllBytes(_databasePath);
            string databaseText = System.Text.Encoding.UTF8.GetString(databaseBytes);
            Assert.That(databaseText, Does.Not.Contain("Sensitive server"));
        }

        [Test]
        public void Save_WithReadOnlyAccess_IsRejected()
        {
            using EncryptedSqliteConnectionProfileProvider provider = CreateProvider(ProfileAccessLevel.ReadOnly);

            Assert.Throws<UnauthorizedAccessException>(() => provider.Save("<Connections />"));
        }

        [Test]
        public void Load_WithWrongPassword_FailsAuthentication()
        {
            using (EncryptedSqliteConnectionProfileProvider provider = CreateProvider(ProfileAccessLevel.Write))
                provider.Save("<Connections />");

            AeadCryptographyProvider cryptographyProvider = new() { KeyDerivationIterations = 10_000 };
            using EncryptedSqliteConnectionProfileProvider wrongPasswordProvider = new(
                _databasePath,
                cryptographyProvider,
                "wrong password".ConvertToSecureString(),
                ProfileAccessLevel.ReadOnly);

            Assert.Throws<CryptographicException>(() => wrongPasswordProvider.Load());
        }

        private EncryptedSqliteConnectionProfileProvider CreateProvider(ProfileAccessLevel accessLevel)
        {
            AeadCryptographyProvider cryptographyProvider = new() { KeyDerivationIterations = 10_000 };
            return new EncryptedSqliteConnectionProfileProvider(
                _databasePath,
                cryptographyProvider,
                "profile password".ConvertToSecureString(),
                accessLevel);
        }
    }
}
