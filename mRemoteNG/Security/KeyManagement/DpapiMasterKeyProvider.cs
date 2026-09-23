using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace mRemoteNG.Security.KeyManagement
{
    /// <summary>
    /// Uses Windows DPAPI (user-scoped) to store and retrieve a master key
    /// without prompting the user. Acts as a "remember me on this machine" mechanism.
    /// The wrapped key is tied to the current Windows user account.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class DpapiMasterKeyProvider : IMasterKeyProvider
    {
        private readonly string _protectedFilePath;

        public string ProviderId => "dpapi";
        public string DisplayName => "Windows User Account (DPAPI)";
        public bool IsAvailable => OperatingSystem.IsWindows();
        public bool CanPersistWrappedKey => true;

        /// <param name="protectedFilePath">
        /// Path to the DPAPI-protected file that stores the master key.
        /// Defaults to <c>{SettingsPath}\mremoteng.dpapi.bin</c>.
        /// </param>
        public DpapiMasterKeyProvider(string protectedFilePath)
        {
            _protectedFilePath = protectedFilePath ?? throw new ArgumentNullException(nameof(protectedFilePath));
        }

        public Task<SecureString> GetKeyAsync()
        {
            if (!File.Exists(_protectedFilePath))
                return Task.FromResult<SecureString>(null);

            try
            {
                byte[] protectedBytes = File.ReadAllBytes(_protectedFilePath);
                byte[] plainBytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
                string password = Encoding.UTF8.GetString(plainBytes);
                SecureString secure = password.ConvertToSecureString();

                // Zero out the plain text buffer
                Array.Clear(plainBytes, 0, plainBytes.Length);

                return Task.FromResult(secure);
            }
            catch (CryptographicException)
            {
                // DPAPI data was created by a different user or is corrupted — remove it
                TryDeleteProtectedFile();
                return Task.FromResult<SecureString>(null);
            }
        }

        /// <summary>
        /// Protects and stores a master key password using DPAPI (CurrentUser scope).
        /// </summary>
        public void StoreKey(SecureString key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            string plain = key.ConvertToUnsecureString();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

            try
            {
                byte[] protectedBytes = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);

                string directory = Path.GetDirectoryName(_protectedFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllBytes(_protectedFilePath, protectedBytes);
            }
            finally
            {
                Array.Clear(plainBytes, 0, plainBytes.Length);
            }
        }

        /// <summary>
        /// Removes the stored DPAPI-protected key.
        /// </summary>
        public void ClearStoredKey()
        {
            TryDeleteProtectedFile();
        }

        /// <summary>
        /// Returns true if a DPAPI-protected key file exists.
        /// </summary>
        public bool HasStoredKey => File.Exists(_protectedFilePath);

        private void TryDeleteProtectedFile()
        {
            try
            {
                if (File.Exists(_protectedFilePath))
                    File.Delete(_protectedFilePath);
            }
            catch
            {
                // Best-effort cleanup
            }
        }
    }
}
