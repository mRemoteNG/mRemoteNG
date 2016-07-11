using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace mRemoteNG.Security
{
    public sealed class Encryptor<TBlockCipher, TDigest> : ICryptographyProvider
        where TBlockCipher : IBlockCipher, new()
        where TDigest : IDigest, new()
    {
        private IBlockCipher _blockCipher;
        private TDigest _digest;
        private Encoding _encoding;
        private BufferedBlockCipher _cipher;
        private HMac _mac;
        

        public int BlockSizeInBytes => _blockCipher.GetBlockSize();

        public Encryptor()
        {
            _encoding = Encoding.UTF8;
            Init(new Pkcs7Padding());
            _digest = new TDigest();
        }

        public Encryptor(Encoding encoding)
        {
            _encoding = encoding;
            Init(new Pkcs7Padding());
            _digest = new TDigest();
        }

        public Encryptor(Encoding encoding, IBlockCipherPadding padding)
        {
            _encoding = encoding;
            Init(padding);
            _digest = new TDigest();
        }

        private void Init(IBlockCipherPadding padding)
        {
            _blockCipher = new CbcBlockCipher(new TBlockCipher());
            _cipher = new PaddedBufferedBlockCipher(_blockCipher, padding);
        }

        private void InitializeMac(string message, SecureString key)
        {
            var macKey = BuildMacKey(message, key);
            _mac = new HMac(_digest);
            _mac.Init(new KeyParameter(macKey));
        }

        private byte[] BuildMacKey(string message, SecureString key)
        {
            var derivativeKey = GetDerivativeKey(key);
            return derivativeKey;
        }

        private byte[] GetDerivativeKey(SecureString key)
        {
            var kdfParam = new KdfParameters(_encoding.GetBytes(key.ConvertToUnsecureString()), GenerateIv());
            var kdf = new BaseKdfBytesGenerator(0, _digest);
            kdf.Init(kdfParam);
            
            var outputBytes = new byte[_digest.GetByteLength()];
            kdf.GenerateBytes(outputBytes, 0, _digest.GetByteLength());
            return outputBytes;
        }

        public string Encrypt(string plainText, SecureString encryptionKey)
        {
            var encryptedBytes = EncryptBytes(plainText, encryptionKey);
            return Convert.ToBase64String(encryptedBytes);
        }

        public byte[] EncryptBytes(string plainText, SecureString encryptionKey)
        {
            InitializeMac(plainText, encryptionKey);
            var input = _encoding.GetBytes(plainText);
            var iv = GenerateIv();

            var encryptionKeyAsByteArray = _encoding.GetBytes(encryptionKey.ConvertToUnsecureString());
            var keyParam = new KeyParameter(encryptionKeyAsByteArray);
            var keyParamWithIv = new ParametersWithIV(keyParam, iv);
            var cipher = BouncyCastleCrypto(true, input, keyParamWithIv);
            var message = CombineArrays(iv, cipher);

            _mac.Reset();
            _mac.BlockUpdate(message, 0, message.Length);
            var digest = new byte[_mac.GetUnderlyingDigest().GetDigestSize()];
            _mac.DoFinal(digest, 0);

            var result = CombineArrays(digest, message);
            return result;
        }

        public byte[] DecryptBytes(byte[] bytes, SecureString decryptionKey)
        {
            // split the digest into component parts
            var digest = new byte[_mac.GetUnderlyingDigest().GetDigestSize()];
            var message = new byte[bytes.Length - digest.Length];
            var iv = new byte[_blockCipher.GetBlockSize()];
            var cipher = new byte[message.Length - iv.Length];

            Buffer.BlockCopy(bytes, 0, digest, 0, digest.Length);
            Buffer.BlockCopy(bytes, digest.Length, message, 0, message.Length);
            if (!IsValidHMac(digest, message))
            {
                throw new CryptoException();
            }

            Buffer.BlockCopy(message, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(message, iv.Length, cipher, 0, cipher.Length);

            var decryptionKeyAsByteArray = _encoding.GetBytes(decryptionKey.ConvertToUnsecureString());
            var keyParam = new KeyParameter(decryptionKeyAsByteArray);
            var keyParamWithIv = new ParametersWithIV(keyParam, iv);
            var result = BouncyCastleCrypto(false, cipher, keyParamWithIv);
            return result;
        }

        public string Decrypt(string cipher, SecureString decryptionKey)
        {
            var cipherTextAsByteArray = Convert.FromBase64String(cipher);
            var decryptedBytes = DecryptBytes(cipherTextAsByteArray, decryptionKey);
            var decryptedBytesAsEncodedString = _encoding.GetString(decryptedBytes);
            return decryptedBytesAsEncodedString;
        }

        private bool IsValidHMac(byte[] digest, byte[] message)
        {
            _mac.Reset();
            _mac.BlockUpdate(message, 0, message.Length);
            var computed = new byte[_mac.GetUnderlyingDigest().GetDigestSize()];
            _mac.DoFinal(computed, 0);
            return AreEqual(digest, computed);
        }

        private static bool AreEqual(byte[] digest, byte[] computed)
        {
            if (digest.Length != computed.Length)
                return false;

            var result = 0;
            for (var i = 0; i < digest.Length; i++)
            {
                // compute equality of all bytes before returning.
                //   helps prevent timing attacks: 
                //   https://codahale.com/a-lesson-in-timing-attacks/
                result |= digest[i] ^ computed[i];
            }
            return result == 0;
        }

        private byte[] BouncyCastleCrypto(bool forEncrypt, byte[] input, ICipherParameters parameters)
        {
            _cipher.Init(forEncrypt, parameters);
            return _cipher.DoFinal(input);
        }

        private byte[] GenerateIv()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var result = new byte[_blockCipher.GetBlockSize()];
                provider.GetBytes(result);
                return result;
            }
        }

        private static byte[] CombineArrays(byte[] source1, byte[] source2)
        {
            var result = new byte[source1.Length + source2.Length];
            Buffer.BlockCopy(source1, 0, result, 0, source1.Length);
            Buffer.BlockCopy(source2, 0, result, source1.Length, source2.Length);
            return result;
        }
    }
}