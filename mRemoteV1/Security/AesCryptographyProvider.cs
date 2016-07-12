using System.Security;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;


namespace mRemoteNG.Security
{
    public class AesCryptographyProvider : ICryptographyProvider
    {
        private AesEngine _aesEngine;
        private readonly Encoding _encoding;

        public int BlockSizeInBytes => _aesEngine.GetBlockSize();

        public string CipherEngine => _aesEngine.AlgorithmName;

        public AesCryptographyProvider()
        {
            _aesEngine = new AesEngine();
            _encoding = Encoding.UTF8;
        }

        public string Encrypt(string plainText, SecureString encryptionKey)
        {
            var bcEngine = new BouncyCastleCryptographyEngine(_aesEngine, _encoding);
            return bcEngine.Encrypt(plainText, encryptionKey);
        }

        public string Decrypt(string cipherText, SecureString decryptionKey)
        {
            var bcEngine = new BouncyCastleCryptographyEngine(_aesEngine, _encoding);
            return bcEngine.Decrypt(cipherText, decryptionKey);
        }
    }
}