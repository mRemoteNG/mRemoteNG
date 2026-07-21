using System;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Store;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Settings.Store
{
    [SupportedOSPlatform("windows")]
    public class SqliteSettingsStoreTests
    {
        private string _dbPath;
        private SqliteSettingsStore _store;

        [SetUp]
        public void Setup()
        {
            _dbPath = Path.Combine(Path.GetTempPath(), $"mremoteng_test_{Guid.NewGuid()}.db");
            _store = new SqliteSettingsStore(_dbPath);
            _store.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _store?.Dispose();
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }

        [Test]
        public void Initialize_CreatesDatabase()
        {
            Assert.That(_store.IsInitialized, Is.True);
            Assert.That(File.Exists(_dbPath), Is.True);
        }

        [Test]
        public void SchemaVersion_IsOne_AfterInitialize()
        {
            Assert.That(_store.SchemaVersion, Is.EqualTo(1));
        }

        [Test]
        public void Set_And_Get_StringValue()
        {
            _store.Set("App", "TestKey", "TestValue");
            var result = _store.Get<string>("App", "TestKey");
            Assert.That(result, Is.EqualTo("TestValue"));
        }

        [Test]
        public void Set_And_Get_IntValue()
        {
            _store.Set("App", "Port", 3389);
            var result = _store.Get<int>("App", "Port");
            Assert.That(result, Is.EqualTo(3389));
        }

        [Test]
        public void Set_And_Get_BoolValue()
        {
            _store.Set("App", "Enabled", true);
            var result = _store.Get<bool>("App", "Enabled");
            Assert.That(result, Is.True);
        }

        [Test]
        public void Set_And_Get_PointValue()
        {
            var point = new System.Drawing.Point(100, 200);
            _store.Set("App", "Location", point);
            var result = _store.Get<System.Drawing.Point>("App", "Location");
            Assert.That(result, Is.EqualTo(point));
        }

        [Test]
        public void Set_And_Get_SizeValue()
        {
            var size = new System.Drawing.Size(800, 600);
            _store.Set("App", "Size", size);
            var result = _store.Get<System.Drawing.Size>("App", "Size");
            Assert.That(result, Is.EqualTo(size));
        }

        [Test]
        public void Get_ReturnsDefault_WhenKeyNotFound()
        {
            var result = _store.Get("App", "NonExistent", "fallback");
            Assert.That(result, Is.EqualTo("fallback"));
        }

        [Test]
        public void Set_OverwritesExistingValue()
        {
            _store.Set("App", "Key", "First");
            _store.Set("App", "Key", "Second");
            var result = _store.Get<string>("App", "Key");
            Assert.That(result, Is.EqualTo("Second"));
        }

        [Test]
        public void Exists_ReturnsTrue_WhenKeyExists()
        {
            _store.Set("App", "Key", "Value");
            Assert.That(_store.Exists("App", "Key"), Is.True);
        }

        [Test]
        public void Exists_ReturnsFalse_WhenKeyDoesNotExist()
        {
            Assert.That(_store.Exists("App", "Missing"), Is.False);
        }

        [Test]
        public void Remove_DeletesSetting()
        {
            _store.Set("App", "Key", "Value");
            var removed = _store.Remove("App", "Key");
            Assert.That(removed, Is.True);
            Assert.That(_store.Exists("App", "Key"), Is.False);
        }

        [Test]
        public void Remove_ReturnsFalse_WhenKeyNotFound()
        {
            var result = _store.Remove("App", "Missing");
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetAll_ReturnsAllKeysInCategory()
        {
            _store.Set("Cat1", "Key1", "Val1");
            _store.Set("Cat1", "Key2", "Val2");
            _store.Set("Cat2", "Key3", "Val3");

            var results = _store.GetAll("Cat1");
            Assert.That(results.Count, Is.EqualTo(2));
            Assert.That(results["Key1"], Is.EqualTo("Val1"));
            Assert.That(results["Key2"], Is.EqualTo("Val2"));
        }

        [Test]
        public void GetCategories_ReturnsDistinctCategories()
        {
            _store.Set("Alpha", "K", "V");
            _store.Set("Beta", "K", "V");
            _store.Set("Alpha", "K2", "V2");

            var cats = _store.GetCategories();
            Assert.That(cats.Count, Is.EqualTo(2));
            Assert.That(cats, Does.Contain("Alpha"));
            Assert.That(cats, Does.Contain("Beta"));
        }

        [Test]
        public void Set_And_Get_EnumValue()
        {
            _store.Set("App", "State", System.Windows.Forms.FormWindowState.Maximized);
            var result = _store.Get<System.Windows.Forms.FormWindowState>("App", "State");
            Assert.That(result, Is.EqualTo(System.Windows.Forms.FormWindowState.Maximized));
        }

        [Test]
        public void DataPersists_AcrossInstances()
        {
            _store.Set("App", "Persistent", "yes");
            _store.Flush();
            _store.Dispose();

            using var store2 = new SqliteSettingsStore(_dbPath);
            store2.Initialize();
            var result = store2.Get<string>("App", "Persistent");
            Assert.That(result, Is.EqualTo("yes"));
        }
    }
}
