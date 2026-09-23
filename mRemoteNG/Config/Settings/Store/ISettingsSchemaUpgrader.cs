using System;

namespace mRemoteNG.Config.Settings.Store
{
    /// <summary>
    /// Represents a single schema migration step for the SQLite settings store.
    /// </summary>
    public interface ISettingsSchemaUpgrader
    {
        /// <summary>
        /// The version this upgrader can migrate FROM.
        /// </summary>
        int FromVersion { get; }

        /// <summary>
        /// The version this upgrader migrates TO.
        /// </summary>
        int ToVersion { get; }

        /// <summary>
        /// Applies the migration.
        /// </summary>
        /// <param name="store">The settings store to migrate.</param>
        void Upgrade(SqliteSettingsStore store);
    }
}
