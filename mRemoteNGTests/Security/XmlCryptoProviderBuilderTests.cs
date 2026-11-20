using System;
using System.Xml.Linq;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;


namespace mRemoteNGTests.Security
{
    public class XmlCryptoProviderBuilderTests
    {
        [Test]
        public void BuildsCorrectEncryptionEngine()
        {
            var element = BuildValidElement();
            var builder = new CryptoProviderFactoryFromXml(element);
            var cryptoProvider = builder.Build();
            Assert.That(cryptoProvider.CipherEngine, Is.EqualTo(BlockCipherEngines.Serpent));
        }

        [Test]
        public void BuildsCorrectCipherMode()
        {
            var element = BuildValidElement();
            var builder = new CryptoProviderFactoryFromXml(element);
            var cryptoProvider = builder.Build();
            Assert.That(cryptoProvider.CipherMode, Is.EqualTo(BlockCipherModes.EAX));
        }

        [Test]
        public void BuildsCorrectKdfIterations()
        {
            var element = BuildValidElement();
            var builder = new CryptoProviderFactoryFromXml(element);
            var cryptoProvider = builder.Build();
            Assert.That(cryptoProvider.KeyDerivationIterations, Is.EqualTo(1234));
        }

        [Test]
        public void CantPassNullIntoConstructor()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new CryptoProviderFactoryFromXml(null);
            });
        }

        [Test]
        public void ReturnsOldProviderTypeIfXmlIsntValid()
        {
            var badElement = new XElement("BadElement");
            var builder = new CryptoProviderFactoryFromXml(badElement);
            var cryptoProvider = builder.Build();
            Assert.That(cryptoProvider, Is.TypeOf<LegacyRijndaelCryptographyProvider>());
        }

        [Test]
        public void BuildsKeyDerivedOnceWhenTheFileHasASalt()
        {
            byte[] salt = DerivedKeyCryptographyProvider.NewSalt();
            var element = BuildValidElement();
            element.Add(new XAttribute("KdfSalt", Convert.ToBase64String(salt)));
            var cryptoProvider = new CryptoProviderFactoryFromXml(element).Build() as DerivedKeyCryptographyProvider;
            Assert.That(cryptoProvider, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(cryptoProvider.Salt, Is.EqualTo(salt));
                Assert.That(cryptoProvider.CipherEngine, Is.EqualTo(BlockCipherEngines.Serpent));
                Assert.That(cryptoProvider.CipherMode, Is.EqualTo(BlockCipherModes.EAX));
                Assert.That(cryptoProvider.KeyDerivationIterations, Is.EqualTo(1234));
            });
        }

        [Test]
        public void BuildsKeyDerivedForEachValueWhenTheFileHasNoSalt()
        {
            var cryptoProvider = new CryptoProviderFactoryFromXml(BuildValidElement()).Build();
            Assert.That(cryptoProvider, Is.TypeOf<AeadCryptographyProvider>());
        }

        private static XElement BuildValidElement()
        {
            return new XElement("TestElement",
                new XAttribute("EncryptionEngine", "Serpent"),
                new XAttribute("BlockCipherMode", "EAX"),
                new XAttribute("KdfIterations", "1234"));
        }
    }
}