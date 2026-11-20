using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using mRemoteNG.Resources.Language;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace mRemoteNG.Security.SymmetricEncryption
{
    /// <summary>
    /// Encrypts each value with an AEAD cipher under one key, derived from the password with
    /// PBKDF2-HMAC-SHA256 and a salt that belongs to the whole connection list: the database, or
    /// the connections file. <see cref="AeadCryptographyProvider"/> derives a key for every value
    /// instead, so its iteration count has to stay low enough for a list with many passwords to
    /// load; deriving once lets it follow current guidance (CVE-2026-87868). A value is the nonce
    /// followed by the cipher text and its tag, in base64.
    /// </summary>
    /// <remarks>
    /// A derived key is kept in memory for the rest of the session, as the password it comes from
    /// already is, so that saving the connections again and reloading them when another client
    /// changes the database do not pay for the derivation each time. Only keys that have encrypted
    /// or authenticated a value are kept, never the key of a wrong password.
    /// </remarks>
    public class DerivedKeyCryptographyProvider : ICryptographyProvider
    {
        /// <summary>The OWASP Password Storage Cheat Sheet value for PBKDF2-HMAC-SHA256.</summary>
        public const int DefaultIterations = 600_000;

        public const int SaltSizeInBytes = 16;

        private const int KeySizeInBytes = 32;
        private const int MacSizeInBits = 128;

        private readonly IAeadBlockCipher _aeadBlockCipher;
        private readonly byte[] _salt;
        private readonly int _nonceSizeInBytes;

        public DerivedKeyCryptographyProvider(byte[] salt, int iterations)
            : this(new GcmBlockCipher(new AesEngine()), salt, iterations)
        {
        }

        public DerivedKeyCryptographyProvider(IAeadBlockCipher aeadBlockCipher, byte[] salt, int iterations)
        {
            _aeadBlockCipher = aeadBlockCipher ?? throw new ArgumentNullException(nameof(aeadBlockCipher));
            if (salt == null)
                throw new ArgumentNullException(nameof(salt));
            if (salt.Length < SaltSizeInBytes)
                throw new ArgumentException($"The salt must be at least {SaltSizeInBytes} bytes long.", nameof(salt));
            if (iterations < 1)
                throw new ArgumentOutOfRangeException(nameof(iterations), iterations, "At least one iteration is needed.");

            _salt = (byte[])salt.Clone();
            KeyDerivationIterations = iterations;
            // CCM takes a nonce of 7 to 13 bytes, and the length left for the message shrinks as it grows.
            _nonceSizeInBytes = aeadBlockCipher is CcmBlockCipher ? 11 : 12;
        }

        /// <summary>A new random salt, for a connection list that has none yet.</summary>
        public static byte[] NewSalt() => RandomNumberGenerator.GetBytes(SaltSizeInBytes);

        public byte[] Salt => (byte[])_salt.Clone();

        public int BlockSizeInBytes => _aeadBlockCipher.GetBlockSize();

        public BlockCipherEngines CipherEngine =>
            Enum.Parse<BlockCipherEngines>(_aeadBlockCipher.AlgorithmName.Split('/')[0]);

        public BlockCipherModes CipherMode =>
            Enum.Parse<BlockCipherModes>(_aeadBlockCipher.AlgorithmName.Split('/')[1]);

        public int KeyDerivationIterations { get; set; }

        public string Encrypt(string plainText, SecureString encryptionKey)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            string password = PasswordOf(encryptionKey);
            byte[] key = DerivedKeys.Find(password, _salt, KeyDerivationIterations) ?? DeriveKey(password, _salt, KeyDerivationIterations);

            byte[] nonce = RandomNumberGenerator.GetBytes(_nonceSizeInBytes);
            byte[] message = Encoding.UTF8.GetBytes(plainText);
            byte[] output;
            lock (_aeadBlockCipher)
            {
                _aeadBlockCipher.Init(true, new AeadParameters(new KeyParameter(key), MacSizeInBits, nonce));
                byte[] cipherText = new byte[_aeadBlockCipher.GetOutputSize(message.Length)];
                int length = _aeadBlockCipher.ProcessBytes(message, 0, message.Length, cipherText, 0);
                _aeadBlockCipher.DoFinal(cipherText, length);

                output = new byte[nonce.Length + cipherText.Length];
                Buffer.BlockCopy(nonce, 0, output, 0, nonce.Length);
                Buffer.BlockCopy(cipherText, 0, output, nonce.Length, cipherText.Length);
            }

            DerivedKeys.Remember(password, _salt, KeyDerivationIterations, key);
            return Convert.ToBase64String(output);
        }

        public string Decrypt(string cipherText, SecureString decryptionKey)
        {
            if (string.IsNullOrEmpty(cipherText))
                return "";

            byte[] input;
            try
            {
                input = Convert.FromBase64String(cipherText);
            }
            catch (FormatException ex)
            {
                throw new EncryptionException(Language.ErrorDecryptionFailed, ex);
            }

            if (input.Length < _nonceSizeInBytes + MacSizeInBits / 8)
                throw new EncryptionException(Language.ErrorDecryptionFailed);

            string password = PasswordOf(decryptionKey);
            byte[] key = DerivedKeys.Find(password, _salt, KeyDerivationIterations) ?? DeriveKey(password, _salt, KeyDerivationIterations);

            byte[] plainText;
            lock (_aeadBlockCipher)
            {
                byte[] nonce = new byte[_nonceSizeInBytes];
                Buffer.BlockCopy(input, 0, nonce, 0, nonce.Length);
                _aeadBlockCipher.Init(false, new AeadParameters(new KeyParameter(key), MacSizeInBits, nonce));

                int cipherTextLength = input.Length - nonce.Length;
                plainText = new byte[_aeadBlockCipher.GetOutputSize(cipherTextLength)];
                try
                {
                    int length = _aeadBlockCipher.ProcessBytes(input, nonce.Length, cipherTextLength, plainText, 0);
                    _aeadBlockCipher.DoFinal(plainText, length);
                }
                catch (InvalidCipherTextException ex)
                {
                    // A wrong password, a value made under another salt, or a value that was changed.
                    throw new EncryptionException(Language.ErrorDecryptionFailed, ex);
                }
            }

            DerivedKeys.Remember(password, _salt, KeyDerivationIterations, key);
            return Encoding.UTF8.GetString(plainText);
        }

        private static string PasswordOf(SecureString key)
        {
            string password = key?.ConvertToUnsecureString();
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("A password is needed to derive the key.", nameof(key));
            return password;
        }

        internal static byte[] DeriveKey(string password, byte[] salt, int iterations)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            try
            {
                return Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, iterations, HashAlgorithmName.SHA256, KeySizeInBytes);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(passwordBytes);
            }
        }

        /// <summary>
        /// The keys derived in this session, found by the iteration count and a hash of the salt
        /// and the password, so the password itself is not kept here a second time.
        /// </summary>
        private static class DerivedKeys
        {
            private const int Capacity = 16;
            private static readonly Dictionary<string, byte[]> Keys = new(StringComparer.Ordinal);

            public static byte[] Find(string password, byte[] salt, int iterations)
            {
                string id = IdOf(password, salt, iterations);
                lock (Keys)
                    return Keys.TryGetValue(id, out byte[] key) ? key : null;
            }

            public static void Remember(string password, byte[] salt, int iterations, byte[] key)
            {
                string id = IdOf(password, salt, iterations);
                lock (Keys)
                {
                    if (Keys.ContainsKey(id)) return;
                    if (Keys.Count >= Capacity) Keys.Clear();
                    Keys[id] = key;
                }
            }

            private static string IdOf(string password, byte[] salt, int iterations)
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] input = new byte[salt.Length + passwordBytes.Length];
                Buffer.BlockCopy(salt, 0, input, 0, salt.Length);
                Buffer.BlockCopy(passwordBytes, 0, input, salt.Length, passwordBytes.Length);
                try
                {
                    return iterations + ":" + Convert.ToBase64String(SHA256.HashData(input));
                }
                finally
                {
                    CryptographicOperations.ZeroMemory(passwordBytes);
                    CryptographicOperations.ZeroMemory(input);
                }
            }
        }
    }
}
