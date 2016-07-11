using System.Security;
using mRemoteNG.Security;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;


namespace mRemoteNGTests.Security
{
    public class AesCryptographyProviderTests
    {
        private ICryptographyProvider _aesCryptographyProvider;
        private SecureString _encryptionKey;
        private string _plainText;

        [SetUp]
        public void Setup()
        {
            _aesCryptographyProvider = new Encryptor<AesEngine, Sha256Digest>();
            _encryptionKey = "mypassword111111".ConvertToSecureString();
            _plainText = "MySecret!";
        }

        [TearDown]
        public void TearDown()
        {
            _aesCryptographyProvider = null;
        }

        [Test]
        public void GetBlockSizeReturnsProperValueForAes()
        {
            Assert.That(_aesCryptographyProvider.BlockSizeInBytes, Is.EqualTo(16));
        }

        [Test]
        public void EncryptionOutputsBase64String()
        {
            var cipherText = _aesCryptographyProvider.Encrypt(_plainText, _encryptionKey);
            Assert.That(cipherText.IsBase64String, Is.True);
        }

        [Test]
        public void DecryptedTextIsEqualToOriginalPlainText()
        {
            var cipherText = _aesCryptographyProvider.Encrypt(_plainText, _encryptionKey);
            var decryptedCipherText = _aesCryptographyProvider.Decrypt(cipherText, _encryptionKey);
            Assert.That(decryptedCipherText, Is.EqualTo(_plainText));
        }

        [Test]
        public void EncryptingTheSameValueReturnsNewCipherTextEachTime()
        {
            var cipherText1 = _aesCryptographyProvider.Encrypt(_plainText, _encryptionKey);
            var cipherText2 = _aesCryptographyProvider.Encrypt(_plainText, _encryptionKey);
            Assert.That(cipherText1, Is.Not.EqualTo(cipherText2));
        }
    }
}