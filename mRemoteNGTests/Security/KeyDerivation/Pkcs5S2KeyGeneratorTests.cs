using System;
using mRemoteNG.Security.KeyDerivation;
using NUnit.Framework;


namespace mRemoteNGTests.Security.KeyDerivation
{
    public class Pkcs5S2KeyGeneratorTests
    {
        [Test]
        public void DefaultConstructorThrowsNoException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new Pkcs5S2KeyGenerator());
        }

        [Test]
        public void CreatingGeneratorWithLowIterationCountThrowsError()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new Pkcs5S2KeyGenerator(256, 999));
        }

        [Test]
        public void CreatingGeneratorWithNegativeKeyBitSizeThrowsError()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new Pkcs5S2KeyGenerator(-1));
        }

        [Test]
        public void IdenticalParametersProduceIdenticalKeys()
        {
            var keyDerivationFunction = new Pkcs5S2KeyGenerator();
            var key1 = keyDerivationFunction.DeriveKey("", new byte[0]);
            var key2 = keyDerivationFunction.DeriveKey("", new byte[0]);
            Assert.That(key1, Is.EquivalentTo(key2));
        }

        [Test]
        public void DifferingIterationsProduceDifferingKeys()
        {
            var keyDerivationFunction1 = new Pkcs5S2KeyGenerator(256, 1001);
            var keyDerivationFunction2 = new Pkcs5S2KeyGenerator(256, 1002);
            var key1 = keyDerivationFunction1.DeriveKey("", new byte[0]);
            var key2 = keyDerivationFunction2.DeriveKey("", new byte[0]);
            Assert.That(key1, Is.Not.EquivalentTo(key2));
        }

        [Test]
        public void DifferingKeysizeProduceDifferingKeys()
        {
            var keyDerivationFunction1 = new Pkcs5S2KeyGenerator();
            var keyDerivationFunction2 = new Pkcs5S2KeyGenerator(512);
            var key1 = keyDerivationFunction1.DeriveKey("", new byte[0]);
            var key2 = keyDerivationFunction2.DeriveKey("", new byte[0]);
            Assert.That(key1, Is.Not.EquivalentTo(key2));
        }

        [Test]
        public void DifferingPasswordsProduceDifferingKeys()
        {
            var keyDerivationFunction = new Pkcs5S2KeyGenerator();
            var key1 = keyDerivationFunction.DeriveKey("a", new byte[0]);
            var key2 = keyDerivationFunction.DeriveKey("b", new byte[0]);
            Assert.That(key1, Is.Not.EquivalentTo(key2));
        }

        [Test]
        public void DifferingSaltsProduceDifferingKeys()
        {
            var keyDerivationFunction = new Pkcs5S2KeyGenerator();
            var key1 = keyDerivationFunction.DeriveKey("", new byte[0]);
            var key2 = keyDerivationFunction.DeriveKey("", new byte[] {1});
            Assert.That(key1, Is.Not.EquivalentTo(key2));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(256)]
        [TestCase(333)]
        public void KeyLengthIsKeyBitSizeDividedBy8(int keyBitSize)
        {
            var keyDerivationFunction = new Pkcs5S2KeyGenerator(keyBitSize);
            var key = keyDerivationFunction.DeriveKey("", new byte[0]);
            Assert.That(key.Length, Is.EqualTo(keyBitSize / 8));
        }
    }
}