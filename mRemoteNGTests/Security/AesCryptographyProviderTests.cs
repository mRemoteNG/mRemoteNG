using System;
using System.Security;
using mRemoteNG.Security;
using NUnit.Framework;


namespace mRemoteNGTests.Security
{
    public class AesCryptographyProviderTests
    {
        private AesCryptographyProvider _aesCryptographyProvider;
        private SecureString _encryptionKey;

        [SetUp]
        public void Setup()
        {
            _aesCryptographyProvider = new AesCryptographyProvider();
            _encryptionKey = "mypassword111111".ConvertToSecureString();
        }

        [TearDown]
        public void TearDown()
        {
            _aesCryptographyProvider = null;
        }

        [Test]
        public void GetBlockSizeReturnsProperValueForAes()
        {
            Assert.That(_aesCryptographyProvider.BlockSize, Is.EqualTo(16));
        }

        [Test]
        public void EncryptionOutputsBase64String()
        {
            var plainText = "MySecret!";
            var cipherText = _aesCryptographyProvider.Encrypt(plainText, _encryptionKey);
            Assert.That(cipherText.IsBase64String, Is.True);
        }

        [Test]
        public void DecryptedTextIsEqualToOriginalPlainText()
        {
            var plainText = "MySecret!";
            var cipherText = _aesCryptographyProvider.Encrypt(plainText, _encryptionKey);
            var decryptedCipherText = _aesCryptographyProvider.Decrypt(cipherText, _encryptionKey);
            Assert.That(decryptedCipherText, Is.EqualTo(plainText));
        }

        [Test]
        public void EncryptingTheSameValueReturnsNewCipherTextEachTime()
        {
            var plainText = "MySecret!";
            var cipherText1 = _aesCryptographyProvider.Encrypt(plainText, _encryptionKey);
            var cipherText2 = _aesCryptographyProvider.Encrypt(plainText, _encryptionKey);
            Assert.That(cipherText1, Is.Not.EqualTo(cipherText2));
        }
    }
}