using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.Security
{
    public class RandomGeneratorTests
    {
        [Test]
        public void ProducesStringOfCorrectLength()
        {
            var text = RandomGenerator.RandomString(8);
            Assert.That(text.Length, Is.EqualTo(8));
        }

        [Test]
        public void StringsDiffer()
        {
            var text1 = RandomGenerator.RandomString(8);
            var text2 = RandomGenerator.RandomString(8);
            Assert.That(text1, Is.Not.EqualTo(text2));
        }
    }
}