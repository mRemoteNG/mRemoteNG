using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;

namespace mRemoteNG.Config.UserProfiles
{
    [SupportedOSPlatform("windows")]
    public sealed class SqliteUserProfileStore : IUserProvider, IPasswordProvider, IConnectionProfileProvider, IDisposable
    {
        public const string DatabaseFileName = "mremoteng.profiles.db";
        private const int PasswordIterations = 210_000;
        private const int PasswordHashLength = 32;

        private readonly Guid _storeId = Guid.NewGuid();
        private readonly SqliteConnection _connection;

        public SqliteUserProfileStore(string settingsPath, SecureString initialAdministratorPassword = null)
        {
            if (string.IsNullOrWhiteSpace(settingsPath))
                throw new ArgumentException("A settings path is required.", nameof(settingsPath));

            Directory.CreateDirectory(settingsPath);
            bool databaseExists = File.Exists(Path.Combine(settingsPath, DatabaseFileName));
            SqliteConnectionStringBuilder builder = new()
            {
                DataSource = Path.Combine(settingsPath, DatabaseFileName),
                Mode = SqliteOpenMode.ReadWriteCreate,
                Pooling = false
            };
            _connection = new SqliteConnection(builder.ToString());
            _connection.Open();
            Initialize();
            UserProfile administrator = GetUser(UserProfile.BuiltInAdministratorId);
            if (!administrator.HasPassword)
            {
                if (initialAdministratorPassword is null || initialAdministratorPassword.Length == 0)
                {
                    _connection.Dispose();
                    throw new InvalidOperationException(databaseExists
                        ? "The built-in administrator must be assigned a password."
                        : "An initial administrator password is required.");
                }
                SetPasswordInternal(UserProfile.BuiltInAdministratorId, initialAdministratorPassword);
            }
        }

        public UserProfile EnsureFirstRunUsers(string windowsUserName)
        {
            if (string.IsNullOrWhiteSpace(windowsUserName))
                throw new ArgumentException("A Windows user name is required.", nameof(windowsUserName));

            EnsureBuiltInAdministrator();
            UserProfile user = GetUser(windowsUserName);
            if (user is not null)
                return user;

            return AddUserInternal(windowsUserName);
        }

        public IReadOnlyList<UserProfile> GetUsers()
        {
            using SqliteCommand command = CreateCommand(
                "SELECT id, user_name, is_administrator, password_hash IS NOT NULL FROM users ORDER BY user_name;");
            using SqliteDataReader reader = command.ExecuteReader();
            List<UserProfile> users = [];
            while (reader.Read())
                users.Add(ReadUser(reader));
            return users;
        }

        public UserProfile GetUser(Guid userId)
        {
            using SqliteCommand command = CreateCommand(
                "SELECT id, user_name, is_administrator, password_hash IS NOT NULL FROM users WHERE id = @id;");
            command.Parameters.AddWithValue("@id", userId.ToString());
            using SqliteDataReader reader = command.ExecuteReader();
            return reader.Read() ? ReadUser(reader) : null;
        }

        public UserProfile GetUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            using SqliteCommand command = CreateCommand(
                "SELECT id, user_name, is_administrator, password_hash IS NOT NULL FROM users WHERE user_name = @name COLLATE NOCASE;");
            command.Parameters.AddWithValue("@name", userName.Trim());
            using SqliteDataReader reader = command.ExecuteReader();
            return reader.Read() ? ReadUser(reader) : null;
        }

        public UserProfileSession Authenticate(string userName, SecureString password)
        {
            UserProfile user = GetUser(userName);
            return user is not null && VerifyPassword(user.Id, password)
                ? new UserProfileSession(_storeId, user)
                : null;
        }

        public UserProfile AddUser(UserProfileSession administrator, string userName)
        {
            DemandAdministrator(administrator);
            return AddUserInternal(userName);
        }

        public void RemoveUser(UserProfileSession administrator, Guid userId)
        {
            DemandAdministrator(administrator);
            if (userId == UserProfile.BuiltInAdministratorId)
                throw new InvalidOperationException("The built-in administrator cannot be removed.");

            using SqliteCommand command = CreateCommand("DELETE FROM users WHERE id = @id;");
            command.Parameters.AddWithValue("@id", userId.ToString());
            command.ExecuteNonQuery();
        }

        public void SetPassword(
            UserProfileSession actingUser,
            Guid userId,
            SecureString currentPassword,
            SecureString newPassword)
        {
            Guid actingUserId = DemandAuthenticated(actingUser);
            EnsureUserExists(userId);
            if (actingUserId != userId && !IsAdministrator(actingUserId))
                throw new UnauthorizedAccessException("Only an administrator can reset another user's password.");
            if (actingUserId == userId && !VerifyPassword(userId, currentPassword))
                throw new UnauthorizedAccessException("The current password is incorrect.");

            SetPasswordInternal(userId, newPassword);
        }

        private UserProfile AddUserInternal(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("A user name is required.", nameof(userName));

            Guid id = Guid.NewGuid();
            using SqliteCommand command = CreateCommand(
                "INSERT INTO users (id, user_name, is_administrator) VALUES (@id, @name, 0);");
            command.Parameters.AddWithValue("@id", id.ToString());
            command.Parameters.AddWithValue("@name", userName.Trim());
            command.ExecuteNonQuery();
            return GetUser(id);
        }

        private void SetPasswordInternal(Guid userId, SecureString password)
        {
            if (password is null || password.Length == 0)
                throw new ArgumentException("A password is required.", nameof(password));
            EnsureUserExists(userId);

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = DerivePassword(password, salt);
            try
            {
                using SqliteCommand command = CreateCommand(
                    "UPDATE users SET password_salt = @salt, password_hash = @hash, password_iterations = @iterations WHERE id = @id;");
                command.Parameters.AddWithValue("@salt", salt);
                command.Parameters.AddWithValue("@hash", hash);
                command.Parameters.AddWithValue("@iterations", PasswordIterations);
                command.Parameters.AddWithValue("@id", userId.ToString());
                command.ExecuteNonQuery();
            }
            finally
            {
                CryptographicOperations.ZeroMemory(hash);
            }
        }

        public bool VerifyPassword(Guid userId, SecureString password)
        {
            if (password is null)
                return false;

            using SqliteCommand command = CreateCommand(
                "SELECT password_salt, password_hash, password_iterations FROM users WHERE id = @id;");
            command.Parameters.AddWithValue("@id", userId.ToString());
            using SqliteDataReader reader = command.ExecuteReader();
            if (!reader.Read())
                return false;

            if (reader.IsDBNull(0) || reader.IsDBNull(1))
                return false;

            byte[] salt = (byte[])reader[0];
            byte[] expectedHash = (byte[])reader[1];
            int iterations = reader.GetInt32(2);
            byte[] actualHash = DerivePassword(password, salt, iterations);
            try
            {
                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(actualHash);
            }
        }

        public IReadOnlyList<ConnectionProfile> GetProfiles(UserProfileSession user)
        {
            Guid userId = DemandAuthenticated(user);
            bool isAdministrator = IsAdministrator(userId);
            const string sql = """
                SELECT p.id, p.name, p.database_path, p.is_shared,
                       CASE WHEN @admin = 1 THEN @owner
                            WHEN a.access_level IS NOT NULL THEN a.access_level
                            WHEN p.is_shared = 1 THEN @readOnly
                            ELSE @none END,
                       COALESCE(pref.load_on_startup, 0)
                FROM connection_profiles p
                LEFT JOIN profile_access a ON a.profile_id = p.id AND a.user_id = @userId
                LEFT JOIN profile_preferences pref ON pref.profile_id = p.id AND pref.user_id = @userId
                WHERE @admin = 1
                   OR (a.access_level IS NULL AND p.is_shared = 1)
                   OR a.access_level > 0
                ORDER BY p.name;
                """;
            using SqliteCommand command = CreateCommand(sql);
            command.Parameters.AddWithValue("@admin", isAdministrator ? 1 : 0);
            command.Parameters.AddWithValue("@owner", (int)ProfileAccessLevel.Owner);
            command.Parameters.AddWithValue("@readOnly", (int)ProfileAccessLevel.ReadOnly);
            command.Parameters.AddWithValue("@none", (int)ProfileAccessLevel.None);
            command.Parameters.AddWithValue("@userId", userId.ToString());
            using SqliteDataReader reader = command.ExecuteReader();
            List<ConnectionProfile> profiles = [];
            while (reader.Read())
            {
                profiles.Add(new ConnectionProfile
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    Name = reader.GetString(1),
                    DatabasePath = reader.GetString(2),
                    IsShared = reader.GetBoolean(3),
                    AccessLevel = (ProfileAccessLevel)reader.GetInt32(4),
                    LoadOnStartup = reader.GetBoolean(5)
                });
            }
            return profiles;
        }

        public ConnectionProfile AddProfile(UserProfileSession user, string name, string databasePath, bool isShared = false)
        {
            Guid userId = DemandAuthenticated(user);
            ValidateProfileName(name);
            if (string.IsNullOrWhiteSpace(databasePath))
                throw new ArgumentException("A profile database path is required.", nameof(databasePath));
            string normalizedDatabasePath = Path.GetFullPath(databasePath);
            string profileDirectory = Path.GetDirectoryName(normalizedDatabasePath);
            if (!string.IsNullOrEmpty(profileDirectory))
                Directory.CreateDirectory(profileDirectory);
            using (new FileStream(normalizedDatabasePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
            }

            Guid profileId = Guid.NewGuid();
            try
            {
                using SqliteTransaction transaction = _connection.BeginTransaction();
                using (SqliteCommand command = CreateCommand(
                           "INSERT INTO connection_profiles (id, name, database_path, is_shared) VALUES (@id, @name, @path, @shared);",
                           transaction))
                {
                    command.Parameters.AddWithValue("@id", profileId.ToString());
                    command.Parameters.AddWithValue("@name", name.Trim());
                    command.Parameters.AddWithValue("@path", normalizedDatabasePath);
                    command.Parameters.AddWithValue("@shared", isShared ? 1 : 0);
                    command.ExecuteNonQuery();
                }
                using (SqliteCommand command = CreateCommand(
                           "INSERT INTO profile_access (profile_id, user_id, access_level) VALUES (@profileId, @userId, @access);",
                           transaction))
                {
                    command.Parameters.AddWithValue("@profileId", profileId.ToString());
                    command.Parameters.AddWithValue("@userId", userId.ToString());
                    command.Parameters.AddWithValue("@access", (int)ProfileAccessLevel.Owner);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch
            {
                File.Delete(normalizedDatabasePath);
                throw;
            }
            return FindProfile(user, profileId);
        }

        public void RenameProfile(UserProfileSession user, Guid profileId, string name)
        {
            ValidateProfileName(name);
            DemandAccess(user, profileId, ProfileAccessLevel.Write);
            using SqliteCommand command = CreateCommand("UPDATE connection_profiles SET name = @name WHERE id = @id;");
            command.Parameters.AddWithValue("@name", name.Trim());
            command.Parameters.AddWithValue("@id", profileId.ToString());
            command.ExecuteNonQuery();
        }

        public void DeleteProfile(UserProfileSession user, Guid profileId)
        {
            DemandAdministrator(user);
            using SqliteCommand command = CreateCommand("DELETE FROM connection_profiles WHERE id = @id;");
            command.Parameters.AddWithValue("@id", profileId.ToString());
            command.ExecuteNonQuery();
        }

        public void SetShared(UserProfileSession user, Guid profileId, bool isShared)
        {
            DemandAccess(user, profileId, ProfileAccessLevel.Owner);
            using SqliteCommand command = CreateCommand(
                "UPDATE connection_profiles SET is_shared = @shared WHERE id = @id;");
            command.Parameters.AddWithValue("@shared", isShared ? 1 : 0);
            command.Parameters.AddWithValue("@id", profileId.ToString());
            command.ExecuteNonQuery();
        }

        public void SetAccess(UserProfileSession administrator, Guid profileId, Guid userId, ProfileAccessLevel accessLevel)
        {
            DemandAdministrator(administrator);
            EnsureUserExists(userId);
            if (!Enum.IsDefined(accessLevel))
                throw new ArgumentOutOfRangeException(nameof(accessLevel));

            using SqliteCommand command = CreateCommand("""
                INSERT INTO profile_access (profile_id, user_id, access_level)
                VALUES (@profileId, @userId, @access)
                ON CONFLICT(profile_id, user_id) DO UPDATE SET access_level = excluded.access_level;
                """);
            command.Parameters.AddWithValue("@profileId", profileId.ToString());
            command.Parameters.AddWithValue("@userId", userId.ToString());
            command.Parameters.AddWithValue("@access", (int)accessLevel);
            if (command.ExecuteNonQuery() == 0)
                throw new InvalidOperationException("The connection profile does not exist.");
        }

        public void SetLoadOnStartup(UserProfileSession user, Guid profileId, bool loadOnStartup)
        {
            Guid userId = DemandAuthenticated(user);
            DemandAccess(user, profileId, ProfileAccessLevel.ReadOnly);
            using SqliteCommand command = CreateCommand("""
                INSERT INTO profile_preferences (profile_id, user_id, load_on_startup)
                VALUES (@profileId, @userId, @load)
                ON CONFLICT(profile_id, user_id) DO UPDATE SET load_on_startup = excluded.load_on_startup;
                """);
            command.Parameters.AddWithValue("@profileId", profileId.ToString());
            command.Parameters.AddWithValue("@userId", userId.ToString());
            command.Parameters.AddWithValue("@load", loadOnStartup ? 1 : 0);
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        private void Initialize()
        {
            using SqliteCommand command = CreateCommand("""
                PRAGMA foreign_keys = ON;
                PRAGMA busy_timeout = 5000;
                PRAGMA journal_mode = WAL;
                CREATE TABLE IF NOT EXISTS users (
                    id TEXT PRIMARY KEY,
                    user_name TEXT NOT NULL COLLATE NOCASE UNIQUE,
                    is_administrator INTEGER NOT NULL DEFAULT 0,
                    password_salt BLOB,
                    password_hash BLOB,
                    password_iterations INTEGER
                );
                CREATE TABLE IF NOT EXISTS connection_profiles (
                    id TEXT PRIMARY KEY,
                    name TEXT NOT NULL COLLATE NOCASE,
                    database_path TEXT NOT NULL COLLATE NOCASE UNIQUE,
                    is_shared INTEGER NOT NULL DEFAULT 0
                );
                CREATE TABLE IF NOT EXISTS profile_access (
                    profile_id TEXT NOT NULL REFERENCES connection_profiles(id) ON DELETE CASCADE,
                    user_id TEXT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                    access_level INTEGER NOT NULL,
                    load_on_startup INTEGER NOT NULL DEFAULT 0,
                    PRIMARY KEY (profile_id, user_id)
                );
                CREATE TABLE IF NOT EXISTS profile_preferences (
                    profile_id TEXT NOT NULL REFERENCES connection_profiles(id) ON DELETE CASCADE,
                    user_id TEXT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
                    load_on_startup INTEGER NOT NULL DEFAULT 0,
                    PRIMARY KEY (profile_id, user_id)
                );
                """);
            command.ExecuteNonQuery();
            EnsureBuiltInAdministrator();
        }

        private void EnsureBuiltInAdministrator()
        {
            using SqliteCommand command = CreateCommand("""
                INSERT OR IGNORE INTO users (id, user_name, is_administrator)
                VALUES (@id, 'Admin', 1);
                """);
            command.Parameters.AddWithValue("@id", UserProfile.BuiltInAdministratorId.ToString());
            command.ExecuteNonQuery();
        }

        private ConnectionProfile FindProfile(UserProfileSession user, Guid profileId)
        {
            foreach (ConnectionProfile profile in GetProfiles(user))
            {
                if (profile.Id == profileId)
                    return profile;
            }
            return null;
        }

        private ProfileAccessLevel GetAccess(Guid userId, Guid profileId)
        {
            if (IsAdministrator(userId))
                return ProfileAccessLevel.Owner;

            using SqliteCommand command = CreateCommand("""
                SELECT COALESCE(a.access_level, CASE WHEN p.is_shared = 1 THEN @readOnly ELSE @none END)
                FROM connection_profiles p
                LEFT JOIN profile_access a ON a.profile_id = p.id AND a.user_id = @userId
                WHERE p.id = @profileId;
                """);
            command.Parameters.AddWithValue("@readOnly", (int)ProfileAccessLevel.ReadOnly);
            command.Parameters.AddWithValue("@none", (int)ProfileAccessLevel.None);
            command.Parameters.AddWithValue("@userId", userId.ToString());
            command.Parameters.AddWithValue("@profileId", profileId.ToString());
            object value = command.ExecuteScalar();
            if (value is null)
                throw new InvalidOperationException("The connection profile does not exist.");
            return (ProfileAccessLevel)Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        private void DemandAccess(UserProfileSession user, Guid profileId, ProfileAccessLevel requiredAccess)
        {
            Guid userId = DemandAuthenticated(user);
            if (GetAccess(userId, profileId) < requiredAccess)
                throw new UnauthorizedAccessException("The user does not have permission to perform this action.");
        }

        private void DemandAdministrator(UserProfileSession user)
        {
            Guid userId = DemandAuthenticated(user);
            if (!IsAdministrator(userId))
                throw new UnauthorizedAccessException("Only an administrator can perform this action.");
        }

        private Guid DemandAuthenticated(UserProfileSession user)
        {
            if (user is null || user.StoreId != _storeId)
                throw new UnauthorizedAccessException("An authenticated user session is required.");
            EnsureUserExists(user.User.Id);
            return user.User.Id;
        }

        private bool IsAdministrator(Guid userId)
        {
            using SqliteCommand command = CreateCommand("SELECT is_administrator FROM users WHERE id = @id;");
            command.Parameters.AddWithValue("@id", userId.ToString());
            object value = command.ExecuteScalar();
            return value is long number && number != 0;
        }

        private void EnsureUserExists(Guid userId)
        {
            if (GetUser(userId) is null)
                throw new InvalidOperationException("The user does not exist.");
        }

        private static void ValidateProfileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("A profile name is required.", nameof(name));
        }

        private static byte[] DerivePassword(SecureString password, byte[] salt, int iterations = PasswordIterations)
        {
            IntPtr pointer = IntPtr.Zero;
            byte[] passwordBytes = null;
            try
            {
                pointer = Marshal.SecureStringToGlobalAllocUnicode(password);
                passwordBytes = new byte[password.Length * sizeof(char)];
                Marshal.Copy(pointer, passwordBytes, 0, passwordBytes.Length);
                return Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, iterations, HashAlgorithmName.SHA256, PasswordHashLength);
            }
            finally
            {
                if (passwordBytes is not null)
                    CryptographicOperations.ZeroMemory(passwordBytes);
                if (pointer != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(pointer);
            }
        }

        private static UserProfile ReadUser(SqliteDataReader reader)
        {
            return new UserProfile
            {
                Id = Guid.Parse(reader.GetString(0)),
                UserName = reader.GetString(1),
                IsAdministrator = reader.GetBoolean(2),
                HasPassword = reader.GetBoolean(3)
            };
        }

        private SqliteCommand CreateCommand(string commandText, SqliteTransaction transaction = null)
        {
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = commandText;
            command.Transaction = transaction;
            return command;
        }
    }
}
