using System;
using System.Collections;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;


namespace mRemoteNGTests.Security
{
    [TestFixture]
    public class CryptographyProviderFactoryTests
    {
        private CryptographyProviderFactory _cryptographyProviderFactory;

        [SetUp]
        public void SetUp()
        {
            _cryptographyProviderFactory = new CryptographyProviderFactory();
        }

        [TearDown]
        public void TearDown()
        {
            _cryptographyProviderFactory = null;
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.AllEngineAndModeCombos))]
        public void CanCreateAeadProvidersWithCorrectEngine(BlockCipherEngines engine, BlockCipherModes mode)
        {
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo(engine));
        }

        [TestCaseSource(typeof(TestCaseSources), nameof(TestCaseSources.AllEngineAndModeCombos))]
        public void CanCreateAeadProvidersWithCorrectMode(BlockCipherEngines engine, BlockCipherModes mode)
        {
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherMode, Is.EqualTo(mode));
        }

        [Test]
        public void CanCreateLegacyRijndael()
        {
            var cryptoProvider = _cryptographyProviderFactory.CreateLegacyRijndaelCryptographyProvider();
            Assert.That(cryptoProvider, Is.TypeOf<LegacyRijndaelCryptographyProvider>());
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