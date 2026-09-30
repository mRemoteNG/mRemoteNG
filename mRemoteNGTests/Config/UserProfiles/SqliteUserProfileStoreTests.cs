using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.Config.UserProfiles;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Config.UserProfiles
{
    [SupportedOSPlatform("windows")]
    public class SqliteUserProfileStoreTests
    {
        private string _settingsPath;
        private SqliteUserProfileStore _store;

        [SetUp]
        public void SetUp()
        {
            _settingsPath = Path.Combine(Path.GetTempPath(), $"mremoteng_profiles_{Guid.NewGuid()}");
            _store = new SqliteUserProfileStore(_settingsPath, "admin password".ConvertToSecureString());
        }

        [TearDown]
        public void TearDown()
        {
            _store.Dispose();
            Directory.Delete(_settingsPath, true);
        }

        [Test]
        public void Constructor_CreatesSeparateProfilesDatabaseAndAdministrator()
        {
            Assert.That(File.Exists(Path.Combine(_settingsPath, SqliteUserProfileStore.DatabaseFileName)), Is.True);
            UserProfile administrator = _store.GetUser(UserProfile.BuiltInAdministratorId);
            Assert.That(administrator.UserName, Is.EqualTo("Admin"));
            Assert.That(administrator.IsAdministrator, Is.True);
        }

        [Test]
        public void EnsureFirstRunUsers_AddsCurrentWindowsUserOnlyOnce()
        {
            UserProfile first = _store.EnsureFirstRunUsers("DOMAIN\\person");
            UserProfile second = _store.EnsureFirstRunUsers("domain\\PERSON");

            Assert.That(second.Id, Is.EqualTo(first.Id));
            Assert.That(_store.GetUsers(), Has.Count.EqualTo(2));
        }

        [Test]
        public void PasswordProvider_StoresAndVerifiesPassword()
        {
            UserProfile user = _store.AddUser(Administrator, "person");
            _store.SetPassword(
                Administrator,
                user.Id,
                null,
                "correct horse battery staple".ConvertToSecureString());

            Assert.That(_store.VerifyPassword(user.Id, "correct horse battery staple".ConvertToSecureString()), Is.True);
            Assert.That(_store.VerifyPassword(user.Id, "wrong".ConvertToSecureString()), Is.False);
            Assert.That(_store.GetUser(user.Id).HasPassword, Is.True);
        }

        [Test]
        public void SharedProfile_IsReadOnlyForOtherUsers()
        {
            UserProfileSession owner = AddUser("owner");
            UserProfileSession reader = AddUser("reader");
            _store.AddProfile(owner, "Shared", Path.Combine(_settingsPath, "shared.db"), true);

            ConnectionProfile profile = _store.GetProfiles(reader).Single();

            Assert.That(profile.AccessLevel, Is.EqualTo(ProfileAccessLevel.ReadOnly));
            Assert.Throws<UnauthorizedAccessException>(() => _store.RenameProfile(reader, profile.Id, "Renamed"));
        }

        [Test]
        public void Administrator_CanGrantWriteAccess()
        {
            UserProfileSession owner = AddUser("owner");
            UserProfileSession writer = AddUser("writer");
            ConnectionProfile profile = _store.AddProfile(owner, "Private", Path.Combine(_settingsPath, "private.db"));

            _store.SetAccess(Administrator, profile.Id, writer.User.Id, ProfileAccessLevel.Write);
            _store.RenameProfile(writer, profile.Id, "Writable");

            Assert.That(_store.GetProfiles(writer).Single().Name, Is.EqualTo("Writable"));
        }

        [Test]
        public void OnlyAdministrator_CanDeleteProfile()
        {
            UserProfileSession owner = AddUser("owner");
            ConnectionProfile profile = _store.AddProfile(owner, "Private", Path.Combine(_settingsPath, "private.db"));

            Assert.Throws<UnauthorizedAccessException>(() => _store.DeleteProfile(owner, profile.Id));

            _store.DeleteProfile(Administrator, profile.Id);
            Assert.That(_store.GetProfiles(owner), Is.Empty);
        }

        [Test]
        public void LoadOnStartup_IsStoredPerUser()
        {
            UserProfileSession owner = AddUser("owner");
            UserProfileSession reader = AddUser("reader");
            ConnectionProfile profile = _store.AddProfile(owner, "Shared", Path.Combine(_settingsPath, "shared.db"), true);

            _store.SetLoadOnStartup(reader, profile.Id, true);

            Assert.That(_store.GetProfiles(reader).Single().LoadOnStartup, Is.True);
            Assert.That(_store.GetProfiles(owner).Single().LoadOnStartup, Is.False);

            _store.SetShared(owner, profile.Id, false);
            Assert.That(_store.GetProfiles(reader), Is.Empty);
        }

        [Test]
        public void ProfileCatalog_PersistsAcrossInstances()
        {
            UserProfileSession owner = AddUser("owner");
            _store.AddProfile(owner, "Persistent", Path.Combine(_settingsPath, "persistent.db"));
            _store.Dispose();

            _store = new SqliteUserProfileStore(_settingsPath);
            owner = _store.Authenticate("owner", "owner password".ConvertToSecureString());

            Assert.That(_store.GetProfiles(owner).Single().Name, Is.EqualTo("Persistent"));
        }

        [Test]
        public void Administrator_RequiresPasswordOnFirstRun()
        {
            string path = Path.Combine(Path.GetTempPath(), $"mremoteng_profiles_{Guid.NewGuid()}");
            try
            {
                Assert.Throws<InvalidOperationException>(() => new SqliteUserProfileStore(path));
            }
            finally
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
        }

        [Test]
        public void User_CannotResetAnotherUsersPassword()
        {
            UserProfileSession first = AddUser("first");
            UserProfileSession second = AddUser("second");

            Assert.Throws<UnauthorizedAccessException>(() => _store.SetPassword(
                first,
                second.User.Id,
                "first password".ConvertToSecureString(),
                "attacker password".ConvertToSecureString()));
        }

        [Test]
        public void AddProfile_RejectsExistingDatabaseFile()
        {
            UserProfileSession owner = AddUser("owner");
            string existingPath = Path.Combine(_settingsPath, "existing.db");
            File.WriteAllText(existingPath, "existing data");

            Assert.Throws<IOException>(() => _store.AddProfile(owner, "Alias", existingPath));
            Assert.That(File.ReadAllText(existingPath), Is.EqualTo("existing data"));
        }

        private UserProfileSession Administrator =>
            _store.Authenticate("Admin", "admin password".ConvertToSecureString());

        private UserProfileSession AddUser(string userName)
        {
            UserProfile user = _store.AddUser(Administrator, userName);
            _store.SetPassword(
                Administrator,
                user.Id,
                null,
                $"{userName} password".ConvertToSecureString());
            return _store.Authenticate(userName, $"{userName} password".ConvertToSecureString());
        }
    }
}
