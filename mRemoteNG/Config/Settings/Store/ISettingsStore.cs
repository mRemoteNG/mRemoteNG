using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mRemoteNG.Config.Settings.Store
{
    /// <summary>
    /// Abstraction for reading and writing application settings.
    /// </summary>
    public interface ISettingsStore : IDisposable
    {
        /// <summary>
        /// Gets a setting value, returning the default if not found.
        /// </summary>
        T Get<T>(string category, string key, T defaultValue = default);

        /// <summary>
        /// Sets a setting value.
        /// </summary>
        void Set<T>(string category, string key, T value);

        /// <summary>
        /// Removes a single setting.
        /// </summary>
        bool Remove(string category, string key);

        /// <summary>
        /// Returns all key-value pairs in a category.
        /// </summary>
        IReadOnlyDictionary<string, string> GetAll(string category);

        /// <summary>
        /// Returns all categories present in the store.
        /// </summary>
        IReadOnlyList<string> GetCategories();

        /// <summary>
        /// Returns true if the setting exists.
        /// </summary>
        bool Exists(string category, string key);

        /// <summary>
        /// Persists any pending changes to disk.
        /// </summary>
        void Flush();

        /// <summary>
        /// Returns true if the underlying store file exists and is accessible.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// The schema version of the store.
        /// </summary>
        int SchemaVersion { get; }

        #region Option Management (Development Features)

        /// <summary>
        /// Retrieves all options from the store.
        /// </summary>
        Task<IEnumerable<OptionInfo>> GetAllOptionsAsync();

        /// <summary>
        /// Retrieves a specific option by its key.
        /// </summary>
        Task<OptionInfo> GetOptionByKeyAsync(string key);

        /// <summary>
        /// Retrieves a specific option by its ID.
        /// </summary>
        Task<OptionInfo> GetOptionByIdAsync(int id);

        /// <summary>
        /// Retrieves options filtered by category.
        /// </summary>
        Task<IEnumerable<OptionInfo>> GetOptionsByCategoryAsync(string category);

        /// <summary>
        /// Adds a new option to the store.
        /// </summary>
        Task<OptionInfo> AddOptionAsync(OptionInfo option);

        /// <summary>
        /// Updates an existing option in the store.
        /// </summary>
        Task<bool> UpdateOptionAsync(OptionInfo option);

        /// <summary>
        /// Deletes an option from the store by its ID.
        /// </summary>
        Task<bool> DeleteOptionAsync(int id);

        /// <summary>
        /// Deletes an option from the store by its key.
        /// </summary>
        Task<bool> DeleteOptionByKeyAsync(string key);

        /// <summary>
        /// Checks if an option with the given key exists.
        /// </summary>
        Task<bool> OptionExistsAsync(string key);

        /// <summary>
        /// Gets the count of options in the store.
        /// </summary>
        Task<int> GetOptionCountAsync();

        /// <summary>
        /// Clears all options from the store.
        /// </summary>
        Task ClearAllOptionsAsync();

        /// <summary>
        /// Exports the options schema and data as SQL statements.
        /// </summary>
        Task<string> ExportSchemaAsync();

        /// <summary>
        /// Imports and applies an options schema SQL script.
        /// </summary>
        Task ImportSchemaAsync(string schemaSql);

        #endregion
    }
}
