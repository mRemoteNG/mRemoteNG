using mRemoteNG.Security;
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

        [Test]
        public void CanCreateAesGcm()
        {
            var engine = BlockCipherEngines.AES;
            var mode = BlockCipherModes.GCM;
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo($"{engine}/{mode}"));
        }

        [Test]
        public void CanCreateAesCcm()
        {
            var engine = BlockCipherEngines.AES;
            var mode = BlockCipherModes.CCM;
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo($"{engine}/{mode}"));
        }

        [Test]
        public void CanCreateAesEax()
        {
            var engine = BlockCipherEngines.AES;
            var mode = BlockCipherModes.EAX;
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo($"{engine}/{mode}"));
        }

        [Test]
        public void CanCreateSerpentGcm()
        {
            var engine = BlockCipherEngines.Serpent;
            var mode = BlockCipherModes.GCM;
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo($"{engine}/{mode}"));
        }

        [Test]
        public void CanCreateSerpentCcm()
        {
            var engine = BlockCipherEngines.Serpent;
            var mode = BlockCipherModes.CCM;
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo($"{engine}/{mode}"));
        }

        [Test]
        public void CanCreateSerpentEax()
        {
            var engine = BlockCipherEngines.Serpent;
            var mode = BlockCipherModes.EAX;
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo($"{engine}/{mode}"));
        }

        [Test]
        public void CanCreateTwofishCcm()
        {
            var engine = BlockCipherEngines.Twofish;
            var mode = BlockCipherModes.CCM;
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo($"{engine}/{mode}"));
        }

        [Test]
        public void CanCreateTwofishEax()
        {
            var engine = BlockCipherEngines.Twofish;
            var mode = BlockCipherModes.EAX;
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo($"{engine}/{mode}"));
        }

        [Test]
        public void CanCreateTwofishGcm()
        {
            var engine = BlockCipherEngines.Twofish;
            var mode = BlockCipherModes.GCM;
            var cryptoProvider = _cryptographyProviderFactory.CreateAeadCryptographyProvider(engine, mode);
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo($"{engine}/{mode}"));
        }

        [Test]
        public void CanCreateLegacyRijndael()
        {
            var cryptoProvider = _cryptographyProviderFactory.CreateLegacyRijndaelCryptographyProvider();
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo("Rijndael"));
        }
    }
}