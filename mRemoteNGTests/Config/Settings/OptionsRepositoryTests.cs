using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using mRemoteNG.Config.Settings;
using mRemoteNG.Config.Settings.Store;

namespace mRemoteNGTests.Config.Settings
{
    [TestFixture]
    public class OptionsRepositoryTests
    {
        private string _testDbPath;
        private OptionsStore _store;
        private OptionsRepository _repository;

        [SetUp]
        public void Setup()
        {
            _testDbPath = Path.Combine(Path.GetTempPath(), $"test_options_{Guid.NewGuid()}.db");
            _store = new OptionsStore(_testDbPath);
            _store.Initialize();
            _repository = new OptionsRepository(_store);
        }

        [TearDown]
        public void Teardown()
        {
            _store?.Dispose();
            if (File.Exists(_testDbPath))
            {
                try
                {
                    File.Delete(_testDbPath);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }

        #region AddOptionAsync Tests

        [Test]
        public async Task AddOptionAsync_ValidOption_ReturnsOptionWithId()
        {
            // Arrange
            var option = new OptionInfo
            {
                Key = "TestKey",
                Value = "TestValue",
                Category = "TestCategory"
            };

            // Act
            var result = await _repository.AddOptionAsync(option);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.GreaterThan(0));
            Assert.That(result.Key, Is.EqualTo("TestKey"));
            Assert.That(result.Value, Is.EqualTo("TestValue"));
        }

        [Test]
        public void AddOptionAsync_NullOption_ThrowsException()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.AddOptionAsync(null));
            Assert.That(ex.ParamName, Is.EqualTo("option"));
        }

        [Test]
        public void AddOptionAsync_EmptyKey_ThrowsException()
        {
            // Arrange
            var option = new OptionInfo { Key = "" };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _repository.AddOptionAsync(option));
        }

        [Test]
        public async Task AddOptionAsync_DuplicateKey_ThrowsException()
        {
            // Arrange
            var option1 = new OptionInfo { Key = "DuplicateKey", Value = "Value1" };
            var option2 = new OptionInfo { Key = "DuplicateKey", Value = "Value2" };

            await _repository.AddOptionAsync(option1);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _repository.AddOptionAsync(option2));
            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        #endregion

        #region GetOptionByKeyAsync Tests

        [Test]
        public async Task GetOptionByKeyAsync_ExistingKey_ReturnsOption()
        {
            // Arrange
            var option = new OptionInfo { Key = "ExistingKey", Value = "Value" };
            await _repository.AddOptionAsync(option);

            // Act
            var result = await _repository.GetOptionByKeyAsync("ExistingKey");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Key, Is.EqualTo("ExistingKey"));
            Assert.That(result.Value, Is.EqualTo("Value"));
        }

        [Test]
        public async Task GetOptionByKeyAsync_NonExistingKey_ReturnsNull()
        {
            // Act
            var result = await _repository.GetOptionByKeyAsync("NonExistingKey");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetOptionByKeyAsync_EmptyKey_ThrowsException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _repository.GetOptionByKeyAsync(""));
        }

        #endregion

        #region GetOptionByIdAsync Tests

        [Test]
        public async Task GetOptionByIdAsync_ExistingId_ReturnsOption()
        {
            // Arrange
            var option = new OptionInfo { Key = "TestKey", Value = "TestValue" };
            var added = await _repository.AddOptionAsync(option);

            // Act
            var result = await _repository.GetOptionByIdAsync(added.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(added.Id));
            Assert.That(result.Key, Is.EqualTo("TestKey"));
        }

        [Test]
        public async Task GetOptionByIdAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetOptionByIdAsync(9999);

            // Assert
            Assert.That(result, Is.Null);
        }

        #endregion

        #region UpdateOptionAsync Tests

        [Test]
        public async Task UpdateOptionAsync_ExistingOption_ReturnsTrue()
        {
            // Arrange
            var option = new OptionInfo { Key = "UpdateKey", Value = "OldValue" };
            var added = await _repository.AddOptionAsync(option);
            added.Value = "NewValue";

            // Act
            var result = await _repository.UpdateOptionAsync(added);

            // Assert
            Assert.That(result, Is.True);
            var updated = await _repository.GetOptionByIdAsync(added.Id);
            Assert.That(updated.Value, Is.EqualTo("NewValue"));
        }

        [Test]
        public async Task UpdateOptionAsync_NonExistingOption_ReturnsFalse()
        {
            // Arrange
            var option = new OptionInfo { Id = 9999, Key = "NonExisting", Value = "Value" };

            // Act
            var result = await _repository.UpdateOptionAsync(option);

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion

        #region DeleteOptionAsync Tests

        [Test]
        public async Task DeleteOptionAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            var option = new OptionInfo { Key = "DeleteKey", Value = "Value" };
            var added = await _repository.AddOptionAsync(option);

            // Act
            var result = await _repository.DeleteOptionAsync(added.Id);

            // Assert
            Assert.That(result, Is.True);
            var deleted = await _repository.GetOptionByIdAsync(added.Id);
            Assert.That(deleted, Is.Null);
        }

        [Test]
        public async Task DeleteOptionAsync_NonExistingId_ReturnsFalse()
        {
            // Act
            var result = await _repository.DeleteOptionAsync(9999);

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion

        #region DeleteOptionByKeyAsync Tests

        [Test]
        public async Task DeleteOptionByKeyAsync_ExistingKey_ReturnsTrue()
        {
            // Arrange
            var option = new OptionInfo { Key = "DeleteByKeyTest", Value = "Value" };
            await _repository.AddOptionAsync(option);

            // Act
            var result = await _repository.DeleteOptionByKeyAsync("DeleteByKeyTest");

            // Assert
            Assert.That(result, Is.True);
            var deleted = await _repository.GetOptionByKeyAsync("DeleteByKeyTest");
            Assert.That(deleted, Is.Null);
        }

        [Test]
        public async Task DeleteOptionByKeyAsync_NonExistingKey_ReturnsFalse()
        {
            // Act
            var result = await _repository.DeleteOptionByKeyAsync("NonExisting");

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion

        #region GetAllOptionsAsync Tests

        [Test]
        public async Task GetAllOptionsAsync_WhenEmpty_ReturnsEmptyCollection()
        {
            // Act
            var result = await _repository.GetAllOptionsAsync();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllOptionsAsync_MultipleOptions_ReturnsAll()
        {
            // Arrange
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key1", Value = "Value1" });
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key2", Value = "Value2" });
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key3", Value = "Value3" });

            // Act
            var result = await _repository.GetAllOptionsAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(3));
        }

        #endregion

        #region GetOptionsByCategoryAsync Tests

        [Test]
        public async Task GetOptionsByCategoryAsync_MatchingCategory_ReturnsFiltered()
        {
            // Arrange
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key1", Category = "CategoryA" });
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key2", Category = "CategoryB" });
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key3", Category = "CategoryA" });

            // Act
            var result = await _repository.GetOptionsByCategoryAsync("CategoryA");

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.All(o => o.Category == "CategoryA"), Is.True);
        }

        #endregion

        #region OptionExistsAsync Tests

        [Test]
        public async Task OptionExistsAsync_ExistingKey_ReturnsTrue()
        {
            // Arrange
            await _repository.AddOptionAsync(new OptionInfo { Key = "ExistKey" });

            // Act
            var result = await _repository.OptionExistsAsync("ExistKey");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task OptionExistsAsync_NonExistingKey_ReturnsFalse()
        {
            // Act
            var result = await _repository.OptionExistsAsync("NonExist");

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion

        #region GetOptionCountAsync Tests

        [Test]
        public async Task GetOptionCountAsync_WhenEmpty_ReturnsZero()
        {
            // Act
            var count = await _repository.GetOptionCountAsync();

            // Assert
            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetOptionCountAsync_MultipleOptions_ReturnsCorrectCount()
        {
            // Arrange
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key1" });
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key2" });
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key3" });

            // Act
            var count = await _repository.GetOptionCountAsync();

            // Assert
            Assert.That(count, Is.EqualTo(3));
        }

        #endregion

        #region ClearAllOptionsAsync Tests

        [Test]
        public async Task ClearAllOptionsAsync_WithOptions_RemovesAll()
        {
            // Arrange
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key1" });
            await _repository.AddOptionAsync(new OptionInfo { Key = "Key2" });

            // Act
            await _repository.ClearAllOptionsAsync();

            // Assert
            var count = await _repository.GetOptionCountAsync();
            Assert.That(count, Is.EqualTo(0));
        }

        #endregion

        #region Integration Tests

        [Test]
        public async Task Integration_AddUpdateDelete_Workflow()
        {
            // Add
            var option = new OptionInfo
            {
                Key = "IntegrationTest",
                Value = "InitialValue",
                Category = "Test",
                Description = "Test option"
            };
            var added = await _repository.AddOptionAsync(option);
            Assert.That(added.Id, Is.GreaterThan(0));

            // Verify Add
            var retrieved = await _repository.GetOptionByKeyAsync("IntegrationTest");
            Assert.That(retrieved, Is.Not.Null);
            Assert.That(retrieved.Value, Is.EqualTo("InitialValue"));

            // Update
            retrieved.Value = "UpdatedValue";
            var updateResult = await _repository.UpdateOptionAsync(retrieved);
            Assert.That(updateResult, Is.True);

            // Verify Update
            var updated = await _repository.GetOptionByKeyAsync("IntegrationTest");
            Assert.That(updated.Value, Is.EqualTo("UpdatedValue"));

            // Delete
            var deleteResult = await _repository.DeleteOptionAsync(added.Id);
            Assert.That(deleteResult, Is.True);

            // Verify Delete
            var deleted = await _repository.GetOptionByKeyAsync("IntegrationTest");
            Assert.That(deleted, Is.Null);
        }

        #endregion
    }
}
