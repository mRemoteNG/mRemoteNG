using System.Security;
using mRemoteNG.Security;
using NUnit.Framework;


namespace mRemoteNGTests.Security
{
    public class AeadCryptographyProviderTests
    {
        private ICryptographyProvider _cryptographyProvider;
        private SecureString _encryptionKey;
        private string _plainText;

        [SetUp]
        public void Setup()
        {
            _cryptographyProvider = new AeadCryptographyProvider();
            _encryptionKey = "mypassword111111".ConvertToSecureString();
            _plainText = "MySecret!";
        }

        [TearDown]
        public void TearDown()
        {
            _cryptographyProvider = null;
        }

        [Test]
        public void GetBlockSizeReturnsProperValueForAes()
        {
            Assert.That(_cryptographyProvider.BlockSizeInBytes, Is.EqualTo(16));
        }

        [Test]
        public void EncryptionOutputsBase64String()
        {
            var cipherText = _cryptographyProvider.Encrypt(_plainText, _encryptionKey);
            Assert.That(cipherText.IsBase64String, Is.True);
        }

        [Test]
        public void DecryptedTextIsEqualToOriginalPlainText()
        {
            var cipherText = _cryptographyProvider.Encrypt(_plainText, _encryptionKey);
            var decryptedCipherText = _cryptographyProvider.Decrypt(cipherText, _encryptionKey);
            Assert.That(decryptedCipherText, Is.EqualTo(_plainText));
        }

        [Test]
        public void EncryptingTheSameValueReturnsNewCipherTextEachTime()
        {
            var cipherText1 = _cryptographyProvider.Encrypt(_plainText, _encryptionKey);
            var cipherText2 = _cryptographyProvider.Encrypt(_plainText, _encryptionKey);
            Assert.That(cipherText1, Is.Not.EqualTo(cipherText2));
        }
    }
}