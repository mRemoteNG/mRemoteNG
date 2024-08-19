/*
 * Initial work:
 * This work (Modern Encryption of a String C#, by James Tuley), 
 * identified by James Tuley, is free of known copyright restrictions.
 * https://gist.github.com/4336842
 * http://creativecommons.org/publicdomain/mark/1.0/ 
 */

using System;
using System.IO;
using System.Security;
using System.Text;
using mRemoteNG.Security.KeyDerivation;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using mRemoteNG.Resources.Language;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Security.SymmetricEncryption
{
    public class AeadCryptographyProvider : ICryptographyProvider
    {
        private readonly IAeadBlockCipher _aeadBlockCipher;
        private readonly Encoding _encoding;
        private readonly SecureRandom _random = new();

        //Preconfigured Encryption Parameters
        protected virtual int NonceBitSize { get; set; } = 128;
        protected virtual int MacBitSize { get; set; } = 128;
        protected virtual int KeyBitSize { get; set; } = 256;

        //Preconfigured Password Key Derivation Parameters
        protected virtual int SaltBitSize { get; set; } = 128;
        public virtual int KeyDerivationIterations { get; set; } = 1000;
        protected virtual int MinPasswordLength { get; set; } = 1;


        public int BlockSizeInBytes
        {
            get { return _aeadBlockCipher.GetBlockSize(); }
        }

        public BlockCipherEngines CipherEngine
        {
            get
            {
                string cipherEngine = _aeadBlockCipher.AlgorithmName.Split('/')[0];
                return (BlockCipherEngines)Enum.Parse(typeof(BlockCipherEngines), cipherEngine);
            }
        }

        public BlockCipherModes CipherMode
        {
            get
            {
                string cipherMode = _aeadBlockCipher.AlgorithmName.Split('/')[1];
                return (BlockCipherModes)Enum.Parse(typeof(BlockCipherModes), cipherMode);
            }
        }

        public AeadCryptographyProvider()
        {
            _aeadBlockCipher = new GcmBlockCipher(new AesEngine());
            _encoding = Encoding.UTF8;
        }

        public AeadCryptographyProvider(Encoding encoding)
        {
            _aeadBlockCipher = new GcmBlockCipher(new AesEngine());
            _encoding = encoding;
        }

        public AeadCryptographyProvider(IAeadBlockCipher aeadBlockCipher)
        {
            _aeadBlockCipher = aeadBlockCipher;
            _encoding = Encoding.UTF8;
            SetNonceForCcm();
        }

        public AeadCryptographyProvider(IAeadBlockCipher aeadBlockCipher, Encoding encoding)
        {
            _aeadBlockCipher = aeadBlockCipher;
            _encoding = encoding;
            SetNonceForCcm();
        }

        private void SetNonceForCcm()
        {
            if (_aeadBlockCipher is CcmBlockCipher)
                NonceBitSize = 88;
        }

        public string Encrypt(string plainText, SecureString encryptionKey)
        {
            return Encrypt(plainText.ConvertToSecureString(), encryptionKey);
        }

        public string Encrypt(SecureString plainText, SecureString encryptionKey)
        {
            string encryptedText = SimpleEncryptWithPassword(plainText, encryptionKey.ConvertToUnsecureString());
            return encryptedText;
        }

        private string SimpleEncryptWithPassword(SecureString secretMessage, string password, byte[] nonSecretPayload = null)
        {
            if (secretMessage == null || secretMessage.Length == 0)
                return ""; //throw new ArgumentException(@"Secret Message Required!", nameof(secretMessage));

            byte[] plainText = _encoding.GetBytes(secretMessage.ConvertToUnsecureString());
            byte[] cipherText = SimpleEncryptWithPassword(plainText, password, nonSecretPayload);
            return Convert.ToBase64String(cipherText);
        }

        private byte[] SimpleEncryptWithPassword(byte[] secretMessage, string password, byte[] nonSecretPayload = null)
        {
            nonSecretPayload ??= ""u8.ToArray();

            //User Error Checks
            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                throw new ArgumentException($"Must have a password of at least {MinPasswordLength} characters!",
                                            nameof(password));

            if (secretMessage == null || secretMessage.Length == 0)
                throw new ArgumentException(@"Secret Message Required!", nameof(secretMessage));

            //Use Random Salt to minimize pre-generated weak password attacks.
            byte[] salt = GenerateSalt();

            //Generate Key
            Pkcs5S2KeyGenerator keyDerivationFunction = new(KeyBitSize, KeyDerivationIterations);
            byte[] key = keyDerivationFunction.DeriveKey(password, salt);

            //Create Full Non Secret Payload
            byte[] payload = new byte[salt.Length + nonSecretPayload.Length];
            Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
            Array.Copy(salt, 0, payload, nonSecretPayload.Length, salt.Length);

            return SimpleEncrypt(secretMessage, key, payload);
        }

        private byte[] SimpleEncrypt(byte[] secretMessage, byte[] key, byte[] nonSecretPayload = null)
        {
            //User Error Checks
            if (key == null || key.Length != KeyBitSize / 8)
                throw new ArgumentException($"Key needs to be {KeyBitSize} bit!", nameof(key));

            if (secretMessage == null || secretMessage.Length == 0)
                throw new ArgumentException(@"Secret Message Required!", nameof(secretMessage));

            //Non-secret Payload Optional
            nonSecretPayload ??= ""u8.ToArray();

            //Using random nonce large enough not to repeat
            byte[] nonce = new byte[NonceBitSize / 8];
            _random.NextBytes(nonce, 0, nonce.Length);

            AeadParameters parameters = new(new KeyParameter(key), MacBitSize, nonce, nonSecretPayload);
            _aeadBlockCipher.Init(true, parameters);

            //Generate Cipher Text With Auth Tag
            byte[] cipherText = new byte[_aeadBlockCipher.GetOutputSize(secretMessage.Length)];
            int len = _aeadBlockCipher.ProcessBytes(secretMessage, 0, secretMessage.Length, cipherText, 0);
            _aeadBlockCipher.DoFinal(cipherText, len);

            //Assemble Message
            MemoryStream combinedStream = new();
            using (BinaryWriter binaryWriter = new(combinedStream))
            {
                //Prepend Authenticated Payload
                binaryWriter.Write(nonSecretPayload);
                //Prepend Nonce
                binaryWriter.Write(nonce);
                //Write Cipher Text
                binaryWriter.Write(cipherText);
            }

            return combinedStream.ToArray();
        }


        public string Decrypt(string cipherText, SecureString decryptionKey)
        {
            string decryptedText = SimpleDecryptWithPassword(cipherText, decryptionKey);
            return decryptedText;
        }

        private string SimpleDecryptWithPassword(string encryptedMessage, SecureString decryptionKey, int nonSecretPayloadLength = 0)
        {
            if (string.IsNullOrWhiteSpace(encryptedMessage))
                return ""; //throw new ArgumentException(@"Encrypted Message Required!", nameof(encryptedMessage));

            byte[] cipherText = Convert.FromBase64String(encryptedMessage);
            byte[] plainText = SimpleDecryptWithPassword(cipherText, decryptionKey.ConvertToUnsecureString(), nonSecretPayloadLength);
            return plainText == null ? null : _encoding.GetString(plainText);
        }

        private byte[] SimpleDecryptWithPassword(byte[] encryptedMessage, string password, int nonSecretPayloadLength = 0)
        {
            //User Error Checks
            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                throw new ArgumentException($"Must have a password of at least {MinPasswordLength} characters!", nameof(password));

            if (encryptedMessage == null || encryptedMessage.Length == 0)
                throw new ArgumentException(@"Encrypted Message Required!", nameof(encryptedMessage));

            //Grab Salt from Payload
            byte[] salt = new byte[SaltBitSize / 8];
            Array.Copy(encryptedMessage, nonSecretPayloadLength, salt, 0, salt.Length);

            //Generate Key
            Pkcs5S2KeyGenerator keyDerivationFunction = new(KeyBitSize, KeyDerivationIterations);
            byte[] key = keyDerivationFunction.DeriveKey(password, salt);

            return SimpleDecrypt(encryptedMessage, key, salt.Length + nonSecretPayloadLength);
        }

        private byte[] SimpleDecrypt(byte[] encryptedMessage, byte[] key, int nonSecretPayloadLength = 0)
        {
            //User Error Checks
            if (key == null || key.Length != KeyBitSize / 8)
                throw new ArgumentException($"Key needs to be {KeyBitSize} bit!", nameof(key));

            if (encryptedMessage == null || encryptedMessage.Length == 0)
                throw new ArgumentException(@"Encrypted Message Required!", nameof(encryptedMessage));

            MemoryStream cipherStream = new(encryptedMessage);
            using BinaryReader cipherReader = new(cipherStream);
            //Grab Payload
            byte[] nonSecretPayload = cipherReader.ReadBytes(nonSecretPayloadLength);

            //Grab Nonce
            byte[] nonce = cipherReader.ReadBytes(NonceBitSize / 8);

            AeadParameters parameters = new(new KeyParameter(key), MacBitSize, nonce, nonSecretPayload);
            _aeadBlockCipher.Init(false, parameters);

            //Decrypt Cipher Text
            byte[] cipherText =
                cipherReader.ReadBytes(encryptedMessage.Length - nonSecretPayloadLength - nonce.Length);
            byte[] plainText = new byte[_aeadBlockCipher.GetOutputSize(cipherText.Length)];

            try
            {
                int len = _aeadBlockCipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                _aeadBlockCipher.DoFinal(plainText, len);
            }
            catch (InvalidCipherTextException e)
            {
                throw new EncryptionException(Language.ErrorDecryptionFailed, e);
            }

            return plainText;
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltBitSize / 8];
            _random.NextBytes(salt);
            return salt;
        }

        public SecureString DecryptSecure(string cipherText, SecureString decryptionKey)
        {
            return Decrypt(cipherText, decryptionKey).ConvertToSecureString();
        }


    }
}