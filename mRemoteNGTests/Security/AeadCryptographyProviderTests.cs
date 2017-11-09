using System;
using System.Collections;
using System.Security;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;
using NUnit.Framework.Constraints;


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

        [TestCaseSource(nameof(GetAllBlockCipherEngineAndModeCombinations))]
        public void DecryptedTextIsEqualToOriginalPlainText(BlockCipherEngines engine, BlockCipherModes mode)
        {
            var cryptoProvider = new CryptoProviderFactory(engine, mode).Build();
            var cipherText = cryptoProvider.Encrypt(_plainText, _encryptionKey);
            var decryptedCipherText = cryptoProvider.Decrypt(cipherText, _encryptionKey);
            Assert.That(decryptedCipherText, Is.EqualTo(_plainText));
        }

        [Test]
        public void EncryptingTheSameValueReturnsNewCipherTextEachTime()
        {
            var cipherText1 = _cryptographyProvider.Encrypt(_plainText, _encryptionKey);
            var cipherText2 = _cryptographyProvider.Encrypt(_plainText, _encryptionKey);
            Assert.That(cipherText1, Is.Not.EqualTo(cipherText2));
        }

        private static IEnumerable GetAllBlockCipherEngineAndModeCombinations()
        {
            var combinationList = new ArrayList();
            var engineChoices = Enum.GetValues(typeof(BlockCipherEngines));
            var modeChoices = Enum.GetValues(typeof(BlockCipherModes));
            foreach (var engine in engineChoices)
            {
                foreach (var mode in modeChoices)
                {
                    combinationList.Add(new[] { engine, mode });
                }
            }
            return combinationList;
        }

        [Test]
        public void DecryptionFailureThrowsException()
        {
            var cipherText = _cryptographyProvider.Encrypt(_plainText, _encryptionKey);
            ActualValueDelegate<string> decryptMethod = () => _cryptographyProvider.Decrypt(cipherText, "wrongKey".ConvertToSecureString());
            Assert.That(decryptMethod, Throws.TypeOf<EncryptionException>());
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.AllEngineAndModeCombos))]
        public void GetCipherEngine(BlockCipherEngines engine, BlockCipherModes mode)
        {
            var cryptoProvider = new CryptoProviderFactory(engine, mode).Build();
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo(engine));
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.AllEngineAndModeCombos))]
        public void GetCipherMode(BlockCipherEngines engine, BlockCipherModes mode)
        {
            var cryptoProvider = new CryptoProviderFactory(engine, mode).Build();
            Assert.That(cryptoProvider.CipherMode, Is.EqualTo(mode));
        }


        private class TestCaseSources
        {
            public static IEnumerable AllEngineAndModeCombos
            {
                get
                {
                    foreach (var engine in Enum.GetValues(typeof(BlockCipherEngines)))
                    {
                        foreach (var mode in Enum.GetValues(typeof(BlockCipherModes)))
                        {
                            yield return new TestCaseData(engine, mode);
                        }
                    }
                }
            }
        }
    }
}