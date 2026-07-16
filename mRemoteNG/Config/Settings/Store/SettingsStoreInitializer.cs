using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;
using System.Threading.Tasks;
using mRemoteNG.Security.KeyManagement;

namespace mRemoteNG.Config.Settings.Store
{
    /// <summary>
    /// Orchestrates the settings store initialisation workflow:
    /// <list type="number">
    ///   <item>First run: prompt for password → create keystore → create DB with schema</item>
    ///   <item>Subsequent runs: attempt DPAPI unlock → fall back to password prompt → open DB</item>
    /// </list>
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class SettingsStoreInitializer
    {
        private readonly string _settingsDbPath;
        private readonly string _keystorePath;
        private readonly string _dpapiFilePath;
        private readonly IReadOnlyList<IMasterKeyProvider> _keyProviders;

        /// <param name="settingsPath">Directory where settings files are stored.</param>
        /// <param name="keyProviders">Ordered list of key providers to try (e.g. DPAPI first, then password prompt).</param>
        public SettingsStoreInitializer(string settingsPath, IReadOnlyList<IMasterKeyProvider> keyProviders)
        {
            if (string.IsNullOrEmpty(settingsPath))
                throw new ArgumentNullException(nameof(settingsPath));

            _keyProviders = keyProviders ?? throw new ArgumentNullException(nameof(keyProviders));

            _settingsDbPath = Path.Combine(settingsPath, "mremoteng.settings.db");
            _keystorePath = Path.Combine(settingsPath, "mremoteng.keystore.json");
            _dpapiFilePath = Path.Combine(settingsPath, "mremoteng.dpapi.bin");
        }

        /// <summary>
        /// Returns paths to the files managed by this initializer.
        /// </summary>
        public string SettingsDbPath => _settingsDbPath;
        public string KeystorePath => _keystorePath;

        /// <summary>
        /// Performs the full initialisation.
        /// Returns an open, ready-to-use <see cref="SqliteSettingsStore"/> or null if the user cancels.
        /// </summary>
        public async Task<SqliteSettingsStore> InitializeAsync()
        {
            KeystoreManager keystoreManager = new(_keystorePath);

            if (!keystoreManager.Exists)
            {
                // === FIRST RUN ===
                return await FirstRunSetupAsync(keystoreManager);
            }

            // === SUBSEQUENT RUNS ===
            return await UnlockExistingAsync(keystoreManager);
        }

        private async Task<SqliteSettingsStore> FirstRunSetupAsync(KeystoreManager keystoreManager)
        {
            // Find a provider that can give us a new password
            IMasterKeyProvider passwordProvider = _keyProviders.FirstOrDefault(p => p.ProviderId == "password" && p.IsAvailable);
            if (passwordProvider is null)
                return null;

            SecureString masterPassword = await passwordProvider.GetKeyAsync();
            if (masterPassword is null || masterPassword.Length == 0)
                return null;

            // Ensure directory exists
            string directory = Path.GetDirectoryName(_settingsDbPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Create keystore with random DEK wrapped by the master password
            string dekHex = keystoreManager.CreateKeystore(masterPassword);

            // Optionally store with DPAPI for "remember me"
            DpapiMasterKeyProvider dpapiProvider = _keyProviders.OfType<DpapiMasterKeyProvider>().FirstOrDefault();
            if (dpapiProvider is not null)
            {
                dpapiProvider.StoreKey(masterPassword);
                keystoreManager.AddWrappedKey(dekHex, masterPassword, dpapiProvider.ProviderId);
            }

            // Create and initialize the settings database
            SqliteSettingsStore store = new(_settingsDbPath, dekHex);
            store.Initialize();

            // Verify schema
            SettingsSchemaVersionVerifier verifier = new(currentSupportedVersion: 1);
            verifier.VerifyAndUpgrade(store);

            return store;
        }

        private async Task<SqliteSettingsStore> UnlockExistingAsync(KeystoreManager keystoreManager)
        {
            // Try each provider in order (DPAPI first for seamless unlock, then password prompt)
            foreach (IMasterKeyProvider provider in _keyProviders.Where(p => p.IsAvailable))
            {
                SecureString masterKey = await provider.GetKeyAsync();
                if (masterKey is null || masterKey.Length == 0)
                    continue;

                string dekHex = keystoreManager.UnwrapDek(masterKey);
                if (dekHex is not null)
                {
                    return OpenStore(dekHex);
                }
            }

            return null; // All providers failed or user cancelled
        }

        private SqliteSettingsStore OpenStore(string dekHex)
        {
            SqliteSettingsStore store = new(_settingsDbPath, dekHex);
            store.Initialize();

            SettingsSchemaVersionVerifier verifier = new(currentSupportedVersion: 1);
            verifier.VerifyAndUpgrade(store);

            return store;
        }
    }
}
