using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mRemoteNG.Config.Settings.Store;

namespace mRemoteNG.Config.Settings
{
    /// <summary>
    /// Business logic layer for managing development-only options.
    /// Provides CRUD operations with validation and consistency checks.
    /// </summary>
    public class OptionsRepository : IOptionsRepository
    {
        private readonly OptionsStore _store;

        public OptionsRepository(OptionsStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public Task<IEnumerable<OptionInfo>> GetAllOptionsAsync()
        {
            return _store.GetAllOptionsAsync();
        }

        public Task<OptionInfo> GetOptionByKeyAsync(string key)
        {
            ValidateKey(key);
            return _store.GetOptionByKeyAsync(key);
        }

        public Task<OptionInfo> GetOptionByIdAsync(int id)
        {
            ValidateId(id);
            return _store.GetOptionByIdAsync(id);
        }

        public Task<IEnumerable<OptionInfo>> GetOptionsByCategoryAsync(string category)
        {
            ValidateCategory(category);
            return _store.GetOptionsByCategoryAsync(category);
        }

        public async Task<OptionInfo> AddOptionAsync(OptionInfo option)
        {
            ValidateOption(option);

            // Check if option already exists
            bool exists = await _store.OptionExistsAsync(option.Key);
            if (exists)
            {
                throw new InvalidOperationException($"An option with key '{option.Key}' already exists.");
            }

            return await _store.AddOptionAsync(option);
        }

        public async Task<bool> UpdateOptionAsync(OptionInfo option)
        {
            ValidateOption(option);
            ValidateId(option.Id);

            return await _store.UpdateOptionAsync(option);
        }

        public async Task<bool> DeleteOptionAsync(int id)
        {
            ValidateId(id);
            return await _store.DeleteOptionAsync(id);
        }

        public async Task<bool> DeleteOptionByKeyAsync(string key)
        {
            ValidateKey(key);
            return await _store.DeleteOptionByKeyAsync(key);
        }

        public Task<bool> OptionExistsAsync(string key)
        {
            ValidateKey(key);
            return _store.OptionExistsAsync(key);
        }

        public Task<int> GetOptionCountAsync()
        {
            return _store.GetOptionCountAsync();
        }

        public Task<string> ExportSchemaAsync()
        {
            return _store.ExportSchemaAsync();
        }

        public Task ImportSchemaAsync(string schemaSql)
        {
            return _store.ImportSchemaAsync(schemaSql);
        }

        public Task ClearAllOptionsAsync()
        {
            return _store.ClearAllOptionsAsync();
        }

        #region Validation

        private static void ValidateOption(OptionInfo option)
        {
            if (option == null)
                throw new ArgumentNullException(nameof(option));

            ValidateKey(option.Key);
        }

        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Option key cannot be null or whitespace.", nameof(key));

            if (key.Length > 255)
                throw new ArgumentException("Option key cannot exceed 255 characters.", nameof(key));
        }

        private static void ValidateCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be null or whitespace.", nameof(category));

            if (category.Length > 255)
                throw new ArgumentException("Category cannot exceed 255 characters.", nameof(category));
        }

        private static void ValidateId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be greater than 0.", nameof(id));
        }

        #endregion
    }
}
