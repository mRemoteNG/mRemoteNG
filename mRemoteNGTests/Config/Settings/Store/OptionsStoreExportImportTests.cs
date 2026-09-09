using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using mRemoteNG.Config.Settings;
using mRemoteNG.Config.Settings.Store;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Settings.Store
{
    /// <summary>
    /// Tests for OptionsStore export/import roundtrip functionality.
    /// Verifies that exported schema and data can be imported to recreate the database.
    /// </summary>
    [TestFixture]
    public class OptionsStoreExportImportTests
    {
        private string _testDbPath;

        [SetUp]
        public void Setup()
        {
            _testDbPath = Path.Combine(Path.GetTempPath(), $"test_options_{Guid.NewGuid()}.db");
            if (File.Exists(_testDbPath))
                File.Delete(_testDbPath);
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(_testDbPath))
                File.Delete(_testDbPath);
        }

        [Test]
        public async Task ExportImportRoundtrip_WithMultipleOptions_RestoresAllData()
        {
            // Arrange: Create initial store with data
            var store1 = new OptionsStore(_testDbPath);
            store1.Initialize();

            var options = new List<OptionInfo>
            {
                new() { Key = "key1", Value = "value1", Category = "cat1", OptionType = "string" },
                new() { Key = "key2", Value = "123", Category = "cat2", OptionType = "int" },
                new() { Key = "key3", Value = "true", Category = "cat1", OptionType = "bool" },
                new() { Key = "key4", Value = null, Category = "cat3", OptionType = "string" }
            };

            foreach (var option in options)
            {
                await store1.AddOptionAsync(option);
            }
            store1.Flush();

            // Export the schema and data
            string exported = await store1.ExportSchemaAsync();
            store1.Dispose();

            // Act: Create a new store and import
            var store2 = new OptionsStore(_testDbPath);
            store2.Initialize();
            await store2.ImportSchemaAsync(exported);
            store2.Flush();

            // Assert: Verify all data is restored
            var importedOptions = await store2.GetAllOptionsAsync();
            var importedList = new List<OptionInfo>(importedOptions);

            Assert.That(importedList, Has.Count.EqualTo(4), "Should have imported all 4 options");

            var key1Option = importedList.Find(o => o.Key == "key1");
            Assert.That(key1Option, Is.Not.Null);
            Assert.That(key1Option.Value, Is.EqualTo("value1"));
            Assert.That(key1Option.Category, Is.EqualTo("cat1"));
            Assert.That(key1Option.OptionType, Is.EqualTo("string"));

            var key3Option = importedList.Find(o => o.Key == "key3");
            Assert.That(key3Option, Is.Not.Null);
            Assert.That(key3Option.Value, Is.EqualTo("true"));
            Assert.That(key3Option.Category, Is.EqualTo("cat1"));

            var key4Option = importedList.Find(o => o.Key == "key4");
            Assert.That(key4Option, Is.Not.Null);
            Assert.That(key4Option.Value, Is.Null);
            Assert.That(key4Option.Category, Is.EqualTo("cat3"));

            store2.Dispose();
        }

        [Test]
        public async Task ExportImportRoundtrip_WithSpecialCharacters_PreservesEscaping()
        {
            // Arrange
            var store1 = new OptionsStore(_testDbPath);
            store1.Initialize();

            var optionWithQuotes = new OptionInfo
            {
                Key = "test'key",
                Value = "value'with'quotes",
                Category = "cat'with'quotes",
                OptionType = "string"
            };

            await store1.AddOptionAsync(optionWithQuotes);
            store1.Flush();

            string exported = await store1.ExportSchemaAsync();
            store1.Dispose();

            // Act
            var store2 = new OptionsStore(_testDbPath);
            store2.Initialize();
            await store2.ImportSchemaAsync(exported);
            store2.Flush();

            // Assert
            var importedOptions = await store2.GetAllOptionsAsync();
            var importedList = new List<OptionInfo>(importedOptions);
            var found = importedList.Find(o => o.Key == "test'key");

            Assert.That(found, Is.Not.Null);
            Assert.That(found.Value, Is.EqualTo("value'with'quotes"));
            Assert.That(found.Category, Is.EqualTo("cat'with'quotes"));

            store2.Dispose();
        }

        [Test]
        public async Task ExportSchemaAsync_WithEmptyDatabase_ExportsValidSql()
        {
            // Arrange
            var store = new OptionsStore(_testDbPath);
            store.Initialize();

            // Act
            string exported = await store.ExportSchemaAsync();

            // Assert
            Assert.That(exported, Is.Not.Null);
            Assert.That(exported, Does.Contain("CREATE TABLE"));
            Assert.That(exported, Does.Contain("options"));
            store.Dispose();
        }

        [Test]
        public async Task ExportSchemaAsync_WithData_IncludesInsertStatements()
        {
            // Arrange
            var store = new OptionsStore(_testDbPath);
            store.Initialize();

            await store.AddOptionAsync(new OptionInfo { Key = "testKey", Value = "testValue", Category = "testCat", OptionType = "string" });
            store.Flush();

            // Act
            string exported = await store.ExportSchemaAsync();

            // Assert
            Assert.That(exported, Does.Contain("INSERT INTO options"));
            Assert.That(exported, Does.Contain("testKey"));
            Assert.That(exported, Does.Contain("testValue"));
            store.Dispose();
        }
    }
}
