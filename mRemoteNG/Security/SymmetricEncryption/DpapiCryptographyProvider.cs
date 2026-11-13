using System;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace mRemoteNG.Security.SymmetricEncryption
{
    /// <summary>
    /// Encrypts connection passwords using Windows DPAPI (<see cref="ProtectedData"/>), bound to the
    /// current Windows user account on the current machine (<see cref="DataProtectionScope.CurrentUser"/>).
    /// No master password is required or used: the encryption key parameters on
    /// <see cref="Encrypt"/>/<see cref="Decrypt"/> exist only to satisfy <see cref="ICryptographyProvider"/>
    /// and are ignored.
    ///
    /// Threat model: this replaces mRemoteNG's "no master password" default, which encrypts
    /// confCons.xml with a fixed, publicly-known key (anyone with a copy of the file can decrypt it
    /// in seconds with public tools). DPAPI ties the key to this Windows account + this machine
    /// instead — a copied file is unreadable anywhere else. It does NOT protect against someone who
    /// is logged into this account on this machine (same as any "remember me" mechanism).
    ///
    /// Optional entropy is intentionally not used: DPAPI's own account/machine binding is the whole
    /// point of this provider (no password to use as entropy without reintroducing what we're trying
    /// to avoid).
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class DpapiCryptographyProvider : ICryptographyProvider
    {
        private static readonly Encoding TextEncoding = Encoding.UTF8;

        public int BlockSizeInBytes => 0;

        // Placeholder values only — confCons.xml's EncryptionEngine/BlockCipherMode attributes are
        // meaningless for DPAPI and are never read back for this provider (see KeyProtection="Dpapi"
        // branch in XmlConnectionsDeserializer.CreateDecryptor). Kept here only because
        // ICryptographyProvider requires them.
        public BlockCipherEngines CipherEngine => BlockCipherEngines.AES;
        public BlockCipherModes CipherMode => BlockCipherModes.GCM;

        // Not used by DPAPI (no password-based key derivation happens here).
        public int KeyDerivationIterations { get; set; }

        public string Encrypt(string plainText, SecureString encryptionKey)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            byte[] plainBytes = TextEncoding.GetBytes(plainText);
            try
            {
                byte[] protectedBytes = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(protectedBytes);
            }
            catch (CryptographicException ex)
            {
                throw new EncryptionException(
                    "Failed to protect data with Windows DPAPI for the current user account.", ex);
            }
            finally
            {
                Array.Clear(plainBytes, 0, plainBytes.Length);
            }
        }

        public string Decrypt(string cipherText, SecureString decryptionKey)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";

            byte[] protectedBytes;
            try
            {
                protectedBytes = Convert.FromBase64String(cipherText);
            }
            catch (FormatException ex)
            {
                throw new EncryptionException("DPAPI-protected value is not valid base64.", ex);
            }

            try
            {
                byte[] plainBytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
                try
                {
                    return TextEncoding.GetString(plainBytes);
                }
                finally
                {
                    Array.Clear(plainBytes, 0, plainBytes.Length);
                }
            }
            catch (CryptographicException ex)
            {
                // The usual cause: this file was DPAPI-protected under a different Windows account,
                // or on a different machine, or the account's DPAPI master key changed/was removed.
                throw new EncryptionException(
                    "This connections file was protected with Windows DPAPI under a different Windows " +
                    "user account or on a different computer, and cannot be decrypted here.", ex);
            }
        }
    }
}
