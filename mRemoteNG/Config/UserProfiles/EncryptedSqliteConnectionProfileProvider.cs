using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using mRemoteNG.Security;

namespace mRemoteNG.Config.UserProfiles
{
    [SupportedOSPlatform("windows")]
    public sealed class EncryptedSqliteConnectionProfileProvider : IConnectionProfileDataProvider, IDisposable
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly SecureString _encryptionKey;
        private readonly ProfileAccessLevel _accessLevel;
        private readonly SqliteConnection _connection;

        public EncryptedSqliteConnectionProfileProvider(
            string databasePath,
            ICryptographyProvider cryptographyProvider,
            SecureString encryptionKey,
            ProfileAccessLevel accessLevel)
        {
            if (string.IsNullOrWhiteSpace(databasePath))
                throw new ArgumentException("A profile database path is required.", nameof(databasePath));
            _cryptographyProvider = cryptographyProvider ?? throw new ArgumentNullException(nameof(cryptographyProvider));
            if (encryptionKey is null || encryptionKey.Length == 0)
                throw new ArgumentException("An encryption key is required.", nameof(encryptionKey));
            if (accessLevel < ProfileAccessLevel.ReadOnly)
                throw new UnauthorizedAccessException("Read access to the connection profile is required.");

            string directory = Path.GetDirectoryName(databasePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            _encryptionKey = encryptionKey.Copy();
            _encryptionKey.MakeReadOnly();
            _accessLevel = accessLevel;
            _connection = new SqliteConnection(new SqliteConnectionStringBuilder
            {
                DataSource = databasePath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Pooling = false
            }.ToString());
            _connection.Open();
            Initialize();
        }

        public string Load()
        {
            using SqliteCommand command = _connection.CreateCommand();
            command.CommandText = "SELECT encrypted_data FROM connection_data WHERE id = 1;";
            object result = command.ExecuteScalar();
            if (result is not string encryptedData)
                return null;

            try
            {
                return _cryptographyProvider.Decrypt(encryptedData, _encryptionKey);
            }
            catch (Org.BouncyCastle.Crypto.InvalidCipherTextException ex)
            {
                throw new CryptographicException("The connection profile could not be decrypted.", ex);
            }
        }

        public void Save(string serializedConnections)
        {
            if (_accessLevel < ProfileAccessLevel.Write)
                throw new UnauthorizedAccessException("Write access to the connection profile is required.");
            if (serializedConnections is null)
                throw new ArgumentNullException(nameof(serializedConnections));

            string encryptedData = _cryptographyProvider.Encrypt(serializedConnections, _encryptionKey);
            using SqliteCommand command = _connection.CreateCommand();
            command.CommandText = """
                INSERT INTO connection_data (id, encrypted_data, updated_at)
                VALUES (1, @data, @updatedAt)
                ON CONFLICT(id) DO UPDATE SET
                    encrypted_data = excluded.encrypted_data,
                    updated_at = excluded.updated_at;
                """;
            command.Parameters.AddWithValue("@data", encryptedData);
            command.Parameters.AddWithValue("@updatedAt", DateTime.UtcNow.ToString("O"));
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _connection.Dispose();
            _encryptionKey.Dispose();
        }

        private void Initialize()
        {
            using SqliteCommand command = _connection.CreateCommand();
            command.CommandText = """
                PRAGMA busy_timeout = 5000;
                PRAGMA journal_mode = WAL;
                CREATE TABLE IF NOT EXISTS connection_data (
                    id INTEGER PRIMARY KEY CHECK (id = 1),
                    encrypted_data TEXT NOT NULL,
                    updated_at TEXT NOT NULL
                );
                """;
            command.ExecuteNonQuery();
        }
    }
}
