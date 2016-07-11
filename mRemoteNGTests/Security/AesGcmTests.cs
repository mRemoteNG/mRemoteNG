using System.Security;
using mRemoteNG.Security;
using NUnit.Framework;


namespace mRemoteNGTests.Security
{
    public class AesGcmTests
    {
        private SecureString _encryptionKey;
        private string _plainText;

        [SetUp]
        public void Setup()
        {
            _encryptionKey = "mypassword111111".ConvertToSecureString();
            _plainText = "MySecret!";
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void GetBlockSizeReturnsProperValueForAes()
        {
            Assert.That(AESGCM.BlockSizeInBytes, Is.EqualTo(16));
        }

        [Test]
        public void EncryptionOutputsBase64String()
        {
            var cipherText = AESGCM.Encrypt(_plainText, _encryptionKey);
            Assert.That(cipherText.IsBase64String, Is.True);
        }

        [Test]
        public void DecryptedTextIsEqualToOriginalPlainText()
        {
            var cipherText = AESGCM.Encrypt(_plainText, _encryptionKey);
            var decryptedCipherText = AESGCM.Decrypt(cipherText, _encryptionKey);
            Assert.That(decryptedCipherText, Is.EqualTo(_plainText));
        }

        [Test]
        public void EncryptingTheSameValueReturnsNewCipherTextEachTime()
        {
            var cipherText1 = AESGCM.Encrypt(_plainText, _encryptionKey);
            var cipherText2 = AESGCM.Encrypt(_plainText, _encryptionKey);
            Assert.That(cipherText1, Is.Not.EqualTo(cipherText2));
        }
    }
}