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
            UserProfile user = AddUser("person");
            _store.SetPassword(
                UserProfile.BuiltInAdministratorId,
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
            UserProfile owner = AddUser("owner");
            UserProfile reader = AddUser("reader");
            _store.AddProfile(owner.Id, "Shared", Path.Combine(_settingsPath, "shared.db"), true);

            ConnectionProfile profile = _store.GetProfiles(reader.Id).Single();

            Assert.That(profile.AccessLevel, Is.EqualTo(ProfileAccessLevel.ReadOnly));
            Assert.Throws<UnauthorizedAccessException>(() => _store.RenameProfile(reader.Id, profile.Id, "Renamed"));
        }

        [Test]
        public void Administrator_CanGrantWriteAccess()
        {
            UserProfile owner = AddUser("owner");
            UserProfile writer = AddUser("writer");
            ConnectionProfile profile = _store.AddProfile(owner.Id, "Private", Path.Combine(_settingsPath, "private.db"));

            _store.SetAccess(UserProfile.BuiltInAdministratorId, profile.Id, writer.Id, ProfileAccessLevel.Write);
            _store.RenameProfile(writer.Id, profile.Id, "Writable");

            Assert.That(_store.GetProfiles(writer.Id).Single().Name, Is.EqualTo("Writable"));
        }

        [Test]
        public void OnlyAdministrator_CanDeleteProfile()
        {
            UserProfile owner = AddUser("owner");
            ConnectionProfile profile = _store.AddProfile(owner.Id, "Private", Path.Combine(_settingsPath, "private.db"));

            Assert.Throws<UnauthorizedAccessException>(() => _store.DeleteProfile(owner.Id, profile.Id));

            _store.DeleteProfile(UserProfile.BuiltInAdministratorId, profile.Id);
            Assert.That(_store.GetProfiles(owner.Id), Is.Empty);
        }

        [Test]
        public void LoadOnStartup_IsStoredPerUser()
        {
            UserProfile owner = AddUser("owner");
            UserProfile reader = AddUser("reader");
            ConnectionProfile profile = _store.AddProfile(owner.Id, "Shared", Path.Combine(_settingsPath, "shared.db"), true);

            _store.SetLoadOnStartup(reader.Id, profile.Id, true);

            Assert.That(_store.GetProfiles(reader.Id).Single().LoadOnStartup, Is.True);
            Assert.That(_store.GetProfiles(owner.Id).Single().LoadOnStartup, Is.False);
        }

        [Test]
        public void ProfileCatalog_PersistsAcrossInstances()
        {
            UserProfile owner = AddUser("owner");
            _store.AddProfile(owner.Id, "Persistent", Path.Combine(_settingsPath, "persistent.db"));
            _store.Dispose();

            _store = new SqliteUserProfileStore(_settingsPath);

            Assert.That(_store.GetProfiles(owner.Id).Single().Name, Is.EqualTo("Persistent"));
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
            UserProfile first = AddUser("first");
            UserProfile second = AddUser("second");

            Assert.Throws<UnauthorizedAccessException>(() => _store.SetPassword(
                first.Id,
                second.Id,
                null,
                "attacker password".ConvertToSecureString()));
        }

        private UserProfile AddUser(string userName)
        {
            return _store.AddUser(UserProfile.BuiltInAdministratorId, userName);
        }
    }
}
