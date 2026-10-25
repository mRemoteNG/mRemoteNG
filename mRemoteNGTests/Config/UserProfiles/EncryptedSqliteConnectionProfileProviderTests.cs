using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using mRemoteNG.Config.UserProfiles;
using mRemoteNG.Security.KeyDerivation;
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
        public void Load_UsesPersistedKeyDerivationIterations()
        {
            using (EncryptedSqliteConnectionProfileProvider provider = CreateProvider(ProfileAccessLevel.Write))
                provider.Save("<Connections />");

            AeadCryptographyProvider cryptographyProvider = new() { KeyDerivationIterations = 20_000 };
            using EncryptedSqliteConnectionProfileProvider reopenedProvider = new(
                _databasePath,
                cryptographyProvider,
                "profile password".ConvertToSecureString(),
                ProfileAccessLevel.ReadOnly);

            Assert.That(reopenedProvider.Load(), Is.EqualTo("<Connections />"));
            Assert.That(cryptographyProvider.KeyDerivationIterations, Is.EqualTo(10_000));
        }

        [Test]
        public void ProfileWithoutKdfAlgorithmColumn_LoadsAsLegacyAndUpgradesOnSave()
        {
            const string connections = "<Connections><Node Name=\"Old\" /></Connections>";
            string legacyCipherText = new AeadCryptographyProvider { KeyDerivationIterations = 10_000 }
                .Encrypt(connections, "profile password".ConvertToSecureString());
            using (SqliteConnection connection = new($"Data Source={_databasePath};Pooling=False"))
            {
                connection.Open();
                using SqliteCommand command = connection.CreateCommand();
                // Schema as it was before kdf_algorithm existed
                command.CommandText = """
                    CREATE TABLE connection_data (
                        id INTEGER PRIMARY KEY CHECK (id = 1),
                        encrypted_data TEXT NOT NULL,
                        cipher_engine TEXT NOT NULL,
                        cipher_mode TEXT NOT NULL,
                        kdf_iterations INTEGER NOT NULL,
                        updated_at TEXT NOT NULL
                    );
                    INSERT INTO connection_data VALUES (1, @data, 'AES', 'GCM', 10000, '2020-01-01');
                    """;
                command.Parameters.AddWithValue("@data", legacyCipherText);
                command.ExecuteNonQuery();
            }

            AeadCryptographyProvider upgraded = new()
            {
                KeyDerivationIterations = 600_000,
                KeyDerivationAlgorithm = KeyDerivationAlgorithms.Pbkdf2Sha256
            };
            using (EncryptedSqliteConnectionProfileProvider provider = new(
                       _databasePath, upgraded, "profile password".ConvertToSecureString(), ProfileAccessLevel.Write))
            {
                Assert.That(provider.Load(), Is.EqualTo(connections));
                provider.Save(connections);
            }

            using (SqliteConnection connection = new($"Data Source={_databasePath};Pooling=False"))
            {
                connection.Open();
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT kdf_algorithm, kdf_iterations FROM connection_data WHERE id = 1;";
                using SqliteDataReader reader = command.ExecuteReader();
                Assert.That(reader.Read(), Is.True);
                Assert.That(reader.GetString(0), Is.EqualTo("Pbkdf2Sha256"));
                Assert.That(reader.GetInt32(1), Is.EqualTo(600_000));
            }

            AeadCryptographyProvider reopenedCrypto = new();
            using EncryptedSqliteConnectionProfileProvider reopened = new(
                _databasePath, reopenedCrypto, "profile password".ConvertToSecureString(), ProfileAccessLevel.ReadOnly);
            Assert.That(reopened.Load(), Is.EqualTo(connections));
            Assert.That(reopenedCrypto.KeyDerivationAlgorithm, Is.EqualTo(KeyDerivationAlgorithms.Pbkdf2Sha256));
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
