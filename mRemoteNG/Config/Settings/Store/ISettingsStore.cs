using System;
using System.Collections.Generic;

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
    }
}
