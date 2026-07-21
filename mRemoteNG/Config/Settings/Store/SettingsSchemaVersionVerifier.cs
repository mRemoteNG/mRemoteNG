using System;
using System.Collections.Generic;
using System.Linq;

namespace mRemoteNG.Config.Settings.Store
{
    /// <summary>
    /// Verifies and applies schema migrations for the SQLite settings store.
    /// </summary>
    public class SettingsSchemaVersionVerifier
    {
        private readonly int _currentSupportedVersion;
        private readonly ISettingsSchemaUpgrader[] _upgraders;

        /// <param name="currentSupportedVersion">The latest schema version the application supports.</param>
        /// <param name="upgraders">Ordered list of schema upgraders.</param>
        public SettingsSchemaVersionVerifier(int currentSupportedVersion, ISettingsSchemaUpgrader[] upgraders = null)
        {
            _currentSupportedVersion = currentSupportedVersion;
            _upgraders = upgraders ?? [];
        }

        /// <summary>
        /// Checks the store's current schema version and applies any needed migrations.
        /// </summary>
        /// <returns>True if the schema is at the expected version after any upgrades.</returns>
        public bool VerifyAndUpgrade(SqliteSettingsStore store)
        {
            if (store is null) throw new ArgumentNullException(nameof(store));

            int currentVersion = store.SchemaVersion;

            if (currentVersion == _currentSupportedVersion)
                return true;

            if (currentVersion > _currentSupportedVersion)
                return false; // Database is from a newer version — don't downgrade

            foreach (ISettingsSchemaUpgrader upgrader in _upgraders.OrderBy(u => u.FromVersion))
            {
                if (upgrader.FromVersion == currentVersion)
                {
                    upgrader.Upgrade(store);
                    currentVersion = upgrader.ToVersion;
                }
            }

            return currentVersion == _currentSupportedVersion;
        }
    }
}
