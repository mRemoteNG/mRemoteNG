using System;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Security.AsymmetricEncryption
{
    /// <summary>
    /// Provides hybrid RSA+AES-GCM encryption using an X.509 certificate.
    /// The certificate's public RSA key (OAEP-SHA256) wraps a randomly generated
    /// AES-256 session key; the payload is encrypted with AES-256-GCM.
    /// Decryption requires the certificate's private key to be present in the
    /// Windows Certificate Store (CurrentUser\My or LocalMachine\My).
    /// </summary>
    /// <remarks>
    /// Wire format of the Base64-encoded ciphertext:
    /// <code>
    ///   [4 bytes LE: RSA-ciphertext length]
    ///   [N bytes:    RSA-encrypted AES-256 session key]
    ///   [12 bytes:   AES-GCM nonce]
    ///   [16 bytes:   AES-GCM authentication tag]
    ///   [M bytes:    AES-GCM ciphertext]
    /// </code>
    /// The <see cref="ICryptographyProvider.Encrypt"/> and
    /// <see cref="ICryptographyProvider.Decrypt"/> <c>SecureString</c> parameters
    /// are ignored — the certificate thumbprint supplied at construction time
    /// identifies all key material.
    /// </remarks>
    [SupportedOSPlatform("windows")]
    public class CertificateCryptographyProvider : ICryptographyProvider
    {
        private const int AesKeyBytes = 32;    // 256-bit session key
        private const int NonceSizeBytes = 12; // Standard 96-bit GCM nonce
        private const int TagSizeBytes = 16;   // 128-bit GCM authentication tag

        /// <summary>The hex thumbprint of the X.509 certificate used for key wrapping.</summary>
        public string Thumbprint { get; }

        // ICryptographyProvider members — AES-256-GCM is used internally.
        public int BlockSizeInBytes => 16;
        public BlockCipherEngines CipherEngine => BlockCipherEngines.AES;
        public BlockCipherModes CipherMode => BlockCipherModes.GCM;
        public int KeyDerivationIterations { get => 0; set { } }

        public CertificateCryptographyProvider(string thumbprint)
        {
            if (string.IsNullOrWhiteSpace(thumbprint))
                throw new ArgumentNullException(nameof(thumbprint));
            Thumbprint = thumbprint.Trim();
        }

        /// <inheritdoc/>
        public string Encrypt(string plainText, SecureString encryptionKey)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            using X509Certificate2 cert = FindCertificate(Thumbprint);
            using RSA? rsa = cert.GetRSAPublicKey();
            if (rsa == null)
                throw new EncryptionException("Certificate does not contain an RSA public key.");

            // Generate a random AES-256 session key and wrap it with the certificate's public key.
            byte[] aesKey = RandomNumberGenerator.GetBytes(AesKeyBytes);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            try
            {
                byte[] encryptedAesKey = rsa.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);

                // Encrypt the plaintext with AES-256-GCM.
                byte[] nonce = RandomNumberGenerator.GetBytes(NonceSizeBytes);
                byte[] cipherBytes = new byte[plainBytes.Length];
                byte[] tag = new byte[TagSizeBytes];

                using (AesGcm aesGcm = new(aesKey, TagSizeBytes))
                    aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);

                // Assemble the wire format.
                byte[] output = new byte[4 + encryptedAesKey.Length + NonceSizeBytes + TagSizeBytes + cipherBytes.Length];
                int pos = 0;
                BitConverter.TryWriteBytes(output.AsSpan(pos, 4), encryptedAesKey.Length); pos += 4;
                encryptedAesKey.CopyTo(output, pos); pos += encryptedAesKey.Length;
                nonce.CopyTo(output, pos);           pos += NonceSizeBytes;
                tag.CopyTo(output, pos);             pos += TagSizeBytes;
                cipherBytes.CopyTo(output, pos);

                return Convert.ToBase64String(output);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(aesKey);
                CryptographicOperations.ZeroMemory(plainBytes);
            }
        }

        /// <inheritdoc/>
        public string Decrypt(string cipherText, SecureString decryptionKey)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";

            using X509Certificate2 cert = FindCertificate(Thumbprint);
            using RSA? rsa = cert.GetRSAPrivateKey();
            if (rsa == null)
                throw new EncryptionException(
                    "Certificate does not contain an RSA private key. Decryption is not possible.");

            byte[] data;
            try { data = Convert.FromBase64String(cipherText); }
            catch (FormatException ex) { throw new EncryptionException(Language.ErrorDecryptionFailed, ex); }

            // Minimum: 4 (key length) + 1 (key) + NonceSizeBytes + TagSizeBytes
            const int minimumDataLength = 4 + 1 + NonceSizeBytes + TagSizeBytes;
            if (data.Length < minimumDataLength)
                throw new EncryptionException(Language.ErrorDecryptionFailed);

            int pos = 0;
            int encryptedKeyLen = BitConverter.ToInt32(data, pos); pos += 4;

            // Validate the embedded key length against what the buffer actually contains
            const int maxReasonableKeyLen = 4096; // RSA-4096 max
            if (encryptedKeyLen <= 0 || encryptedKeyLen > maxReasonableKeyLen ||
                encryptedKeyLen > data.Length - 4 - NonceSizeBytes - TagSizeBytes)
                throw new EncryptionException(Language.ErrorDecryptionFailed);

            byte[] encryptedAesKey = new byte[encryptedKeyLen];
            Buffer.BlockCopy(data, pos, encryptedAesKey, 0, encryptedKeyLen); pos += encryptedKeyLen;

            byte[] nonce = new byte[NonceSizeBytes];
            Buffer.BlockCopy(data, pos, nonce, 0, NonceSizeBytes); pos += NonceSizeBytes;

            byte[] tag = new byte[TagSizeBytes];
            Buffer.BlockCopy(data, pos, tag, 0, TagSizeBytes); pos += TagSizeBytes;

            byte[] cipherBytes = new byte[data.Length - pos];
            Buffer.BlockCopy(data, pos, cipherBytes, 0, cipherBytes.Length);

            byte[] aesKey;
            try { aesKey = rsa.Decrypt(encryptedAesKey, RSAEncryptionPadding.OaepSHA256); }
            catch (CryptographicException ex) { throw new EncryptionException(Language.ErrorDecryptionFailed, ex); }

            byte[] plainBytes = new byte[cipherBytes.Length];
            try
            {
                try
                {
                    using AesGcm aesGcm = new(aesKey, TagSizeBytes);
                    aesGcm.Decrypt(nonce, cipherBytes, tag, plainBytes);
                }
                catch (CryptographicException ex) { throw new EncryptionException(Language.ErrorDecryptionFailed, ex); }

                return Encoding.UTF8.GetString(plainBytes);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(aesKey);
                CryptographicOperations.ZeroMemory(plainBytes);
            }
        }

        /// <summary>
        /// Searches CurrentUser\My then LocalMachine\My for a certificate matching
        /// <paramref name="thumbprint"/>. Returns the first match.
        /// </summary>
        /// <exception cref="EncryptionException">Thrown when no matching certificate is found.</exception>
        private static X509Certificate2 FindCertificate(string thumbprint)
        {
            foreach (StoreLocation location in new[] { StoreLocation.CurrentUser, StoreLocation.LocalMachine })
            {
                using X509Store store = new(StoreName.My, location);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection matches = store.Certificates.Find(
                    X509FindType.FindByThumbprint, thumbprint, validOnly: false);
                if (matches.Count > 0)
                    return matches[0];
            }

            throw new EncryptionException(
                $"Certificate with thumbprint '{thumbprint}' was not found in the Windows Certificate Store.");
        }
    }
}
