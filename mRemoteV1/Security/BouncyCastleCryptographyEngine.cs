using System;
using System.Security;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace mRemoteNG.Security
{
    internal class BouncyCastleCryptographyEngine
    {
        private readonly Encoding _encoding;
        private readonly IBlockCipher _blockCipher;
        private IBlockCipherPadding _padding;
        private const bool ActionEncrypt = true;
        private const bool ActionDecrypt = false;

        internal IBlockCipherPadding Padding
        {
            get { return _padding; }
            set {
                if (value != null)
                    _padding = value;
            }
        }

        internal BouncyCastleCryptographyEngine(IBlockCipher cipherEngine, Encoding encoding)
        {
            _blockCipher = new CbcBlockCipher(cipherEngine);
            _encoding = encoding;
        }

        internal string Encrypt(string plain, SecureString key)
        {
            var result = BouncyCastleCrypto(ActionEncrypt, plain, key);
            return Convert.ToBase64String(result);
        }

        internal string Decrypt(string cipher, SecureString key)
        {
            var result = BouncyCastleCrypto(ActionDecrypt, cipher, key);
            return _encoding.GetString(result);
        }


        private byte[] BouncyCastleCrypto(bool forEncrypt, string input, SecureString key)
        {
            try
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var iv = BuildInitializationVector(); //for the sake of demo

                //Set up
                var cipher = new PaddedBufferedBlockCipher(_blockCipher);
                var keyParam = new KeyParameter(Encoding.Default.GetBytes(key.ConvertToUnsecureString()));
                var keyParamWithIv = new ParametersWithIV(keyParam, iv, 0, 16);

                // Encrypt/Decrypt
                cipher.Init(forEncrypt, keyParamWithIv);
                var outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
                var length = cipher.ProcessBytes(inputBytes, outputBytes, 0);
                cipher.DoFinal(outputBytes, length); //Do the final block
                return outputBytes;
            }
            catch (CryptoException ex)
            {
                throw new CryptoException("", ex);
            }
        }

        private byte[] BuildInitializationVector()
        {
            var numberOfBytes = _blockCipher.GetBlockSize();
            var iv = new byte[numberOfBytes];
            var randomNumberGenerator = new SecureRandom();
            randomNumberGenerator.NextBytes(iv, 0, numberOfBytes);
            return iv;
        }
    }
}