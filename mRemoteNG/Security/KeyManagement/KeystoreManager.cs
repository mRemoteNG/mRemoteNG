using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace mRemoteNG.Security.KeyManagement
{
    /// <summary>
    /// Manages the Data Encryption Key (DEK) envelope.
    /// The DEK is a random 256-bit key used to encrypt database files.
    /// It is wrapped (encrypted) using a master key derived from the user's password.
    /// This allows password changes without re-encrypting the entire database.
    /// </summary>
    public sealed class KeystoreManager
    {
        private const int DekSizeBytes = 32;  // 256-bit
        private const int SaltSizeBytes = 16; // 128-bit
        private const int NonceSizeBytes = 12; // 96-bit for AES-GCM
        private const int TagSizeBytes = 16;  // 128-bit
        private const int KdfIterations = 600_000; // OWASP 2023 recommendation for PBKDF2-SHA256

        private readonly string _keystorePath;

        public KeystoreManager(string keystorePath)
        {
            _keystorePath = keystorePath ?? throw new ArgumentNullException(nameof(keystorePath));
        }

        /// <summary>
        /// Returns true if the keystore file exists.
        /// </summary>
        public bool Exists => File.Exists(_keystorePath);

        /// <summary>
        /// Creates a brand-new keystore with a random DEK, wrapped by the given master password.
        /// </summary>
        /// <returns>The hex-encoded DEK for use as a database encryption key.</returns>
        public string CreateKeystore(SecureString masterPassword)
        {
            if (masterPassword is null || masterPassword.Length == 0)
                throw new ArgumentException("Master password is required.", nameof(masterPassword));

            // Generate a random DEK
            byte[] dek = RandomNumberGenerator.GetBytes(DekSizeBytes);

            try
            {
                // Wrap the DEK with the master password
                WrappedKeyEntry wrappedEntry = WrapDek(dek, masterPassword);

                KeystoreData keystore = new()
                {
                    Version = 1,
                    KdfAlgorithm = "PBKDF2-SHA256",
                    KdfIterations = KdfIterations,
                    WrappedKeys = [wrappedEntry]
                };

                SaveKeystore(keystore);

                return Convert.ToHexString(dek);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(dek);
            }
        }

        /// <summary>
        /// Unwraps the DEK using the given master password.
        /// </summary>
        /// <returns>The hex-encoded DEK, or null if unwrapping fails.</returns>
        public string UnwrapDek(SecureString masterPassword)
        {
            if (!Exists)
                return null;

            KeystoreData keystore = LoadKeystore();

            foreach (WrappedKeyEntry entry in keystore.WrappedKeys)
            {
                byte[] dek = TryUnwrapEntry(entry, masterPassword, keystore.KdfIterations);
                if (dek is not null)
                {
                    try
                    {
                        return Convert.ToHexString(dek);
                    }
                    finally
                    {
                        CryptographicOperations.ZeroMemory(dek);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Adds an additional wrapped key entry for a new provider (e.g. adding DPAPI alongside password).
        /// Requires the current DEK (already unwrapped) and the new master password to wrap with.
        /// </summary>
        public void AddWrappedKey(string dekHex, SecureString newMasterKey, string providerId)
        {
            if (string.IsNullOrEmpty(dekHex)) throw new ArgumentNullException(nameof(dekHex));
            if (newMasterKey is null) throw new ArgumentNullException(nameof(newMasterKey));

            KeystoreData keystore = LoadKeystore();
            byte[] dek = Convert.FromHexString(dekHex);

            try
            {
                WrappedKeyEntry newEntry = WrapDek(dek, newMasterKey);
                newEntry.ProviderId = providerId ?? "password";

                keystore.WrappedKeys.Add(newEntry);
                SaveKeystore(keystore);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(dek);
            }
        }

        /// <summary>
        /// Re-wraps the DEK with a new master password, replacing the entry for the specified provider.
        /// Use this when the user changes their password.
        /// </summary>
        public void ChangePassword(string dekHex, SecureString newPassword, string providerId = "password")
        {
            if (string.IsNullOrEmpty(dekHex)) throw new ArgumentNullException(nameof(dekHex));
            if (newPassword is null) throw new ArgumentNullException(nameof(newPassword));

            KeystoreData keystore = LoadKeystore();
            byte[] dek = Convert.FromHexString(dekHex);

            try
            {
                // Remove existing entries for this provider
                keystore.WrappedKeys.RemoveAll(e =>
                    string.Equals(e.ProviderId, providerId, StringComparison.OrdinalIgnoreCase));

                // Add new wrapped entry
                WrappedKeyEntry newEntry = WrapDek(dek, newPassword);
                newEntry.ProviderId = providerId;

                keystore.WrappedKeys.Add(newEntry);
                SaveKeystore(keystore);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(dek);
            }
        }

        /// <summary>
        /// Removes all wrapped key entries for a given provider.
        /// </summary>
        public void RemoveProvider(string providerId)
        {
            KeystoreData keystore = LoadKeystore();
            keystore.WrappedKeys.RemoveAll(e =>
                string.Equals(e.ProviderId, providerId, StringComparison.OrdinalIgnoreCase));
            SaveKeystore(keystore);
        }

        #region Wrap / Unwrap

        private WrappedKeyEntry WrapDek(byte[] dek, SecureString password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);
            byte[] derivedKey = DeriveKey(password, salt, KdfIterations);
            byte[] nonce = RandomNumberGenerator.GetBytes(NonceSizeBytes);
            byte[] cipherText = new byte[dek.Length];
            byte[] tag = new byte[TagSizeBytes];

            try
            {
                using AesGcm aes = new(derivedKey, TagSizeBytes);
                aes.Encrypt(nonce, dek, cipherText, tag);

                return new WrappedKeyEntry
                {
                    ProviderId = "password",
                    Salt = Convert.ToBase64String(salt),
                    Nonce = Convert.ToBase64String(nonce),
                    WrappedDek = Convert.ToBase64String(cipherText),
                    Tag = Convert.ToBase64String(tag)
                };
            }
            finally
            {
                CryptographicOperations.ZeroMemory(derivedKey);
            }
        }

        private static byte[] TryUnwrapEntry(WrappedKeyEntry entry, SecureString password, int kdfIterations)
        {
            try
            {
                byte[] salt = Convert.FromBase64String(entry.Salt);
                byte[] nonce = Convert.FromBase64String(entry.Nonce);
                byte[] cipherText = Convert.FromBase64String(entry.WrappedDek);
                byte[] tag = Convert.FromBase64String(entry.Tag);
                byte[] derivedKey = DeriveKey(password, salt, kdfIterations);
                byte[] dek = new byte[cipherText.Length];

                try
                {
                    using AesGcm aes = new(derivedKey, TagSizeBytes);
                    aes.Decrypt(nonce, cipherText, tag, dek);
                    return dek;
                }
                finally
                {
                    CryptographicOperations.ZeroMemory(derivedKey);
                }
            }
            catch (CryptographicException)
            {
                return null; // Wrong password or corrupted entry
            }
        }

        private static byte[] DeriveKey(SecureString password, byte[] salt, int iterations)
        {
            string plain = password.ConvertToUnsecureString();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(plain);

            try
            {
                using Rfc2898DeriveBytes kdf = new(passwordBytes, salt, iterations, HashAlgorithmName.SHA256);
                return kdf.GetBytes(32); // 256-bit key
            }
            finally
            {
                CryptographicOperations.ZeroMemory(passwordBytes);
            }
        }

        #endregion

        #region Persistence

        private void SaveKeystore(KeystoreData keystore)
        {
            string directory = Path.GetDirectoryName(_keystorePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            JsonSerializerOptions options = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(keystore, options);
            File.WriteAllText(_keystorePath, json, Encoding.UTF8);
        }

        private KeystoreData LoadKeystore()
        {
            string json = File.ReadAllText(_keystorePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<KeystoreData>(json)
                   ?? throw new InvalidOperationException("Keystore file is empty or corrupt.");
        }

        #endregion
    }
}
