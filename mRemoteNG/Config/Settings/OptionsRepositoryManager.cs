using System;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Store;
using mRemoteNG.Security.KeyManagement;

namespace mRemoteNG.Config.Settings
{
    /// <summary>
    /// Manages the initialization and lifecycle of the dev-only options repository.
    /// This is only used during development to allow runtime management of options.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class OptionsRepositoryManager : IDisposable
    {
        private ISettingsStore _store;
        private OptionsRepository _repository;
        private bool _disposed;

        public IOptionsRepository Repository => _repository;
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Initializes the options repository for the dev environment.
        /// This should only be called in DEBUG builds or when explicitly enabled.
        /// </summary>
        /// <param name="settingsPath">The path where settings are stored.</param>
        public void Initialize(string settingsPath)
        {
            if (string.IsNullOrEmpty(settingsPath))
                throw new ArgumentNullException(nameof(settingsPath));

            EnsureNotDisposed();

            if (IsInitialized)
                return;

            try
            {
                // Use the main settings database so options are persisted in the same file.
                string optionsDbPath = Path.Combine(settingsPath, SettingsStoreInitializer.SettingsDatabaseFileName);
                string dekHex = TryGetDekHex(settingsPath);

                var store = new SqliteSettingsStore(optionsDbPath, dekHex);
                store.Initialize();
                _store = store;

                _repository = new OptionsRepository(_store);

                IsInitialized = true;
            }
            catch (Exception ex)
            {
                // If initialization fails, log it but don't crash the app
                System.Diagnostics.Debug.WriteLine($"Failed to initialize options repository: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Flushes any pending changes to disk.
        /// </summary>
        public void Flush()
        {
            if (_store != null && _store.IsInitialized)
            {
                _store.Flush();
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            Flush();
            _store?.Dispose();
            _store = null;
            _repository = null;
            IsInitialized = false;
        }

        private static string TryGetDekHex(string settingsPath)
        {
            try
            {
                string keystorePath = Path.Combine(settingsPath, "mremoteng.keystore.json");
                if (!File.Exists(keystorePath))
                    return null;

                string dpapiFilePath = Path.Combine(settingsPath, "mremoteng.dpapi.bin");
                DpapiMasterKeyProvider dpapiProvider = new(dpapiFilePath);
                if (!dpapiProvider.IsAvailable)
                    return null;

                using var masterKey = dpapiProvider.GetKeyAsync().GetAwaiter().GetResult();
                if (masterKey is null || masterKey.Length == 0)
                    return null;

                KeystoreManager keystoreManager = new(keystorePath);
                return keystoreManager.UnwrapDek(masterKey);
            }
            catch
            {
                return null;
            }
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}
