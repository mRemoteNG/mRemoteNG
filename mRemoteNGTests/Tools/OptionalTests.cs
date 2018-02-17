using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
	public class OptionalTests
	{
		[Test]
		public void MaybeReturnsEmptyListWhenGivenNullValue()
		{
			var sut = new Optional<object>(null);
			Assert.That(sut, Is.Empty);
		}

		[Test]
		public void MaybeReturnsValueIfNotNull()
		{
			var expected = new object();
			var sut = new Optional<object>(expected);
			Assert.That(sut, Has.Member(expected));
		}

	    [Test]
	    public void MaybeExtensionOfNullObjectIsntNull()
	    {
	        var sut = ((object) null).Maybe();
            Assert.That(sut, Is.Not.Null);
	    }
	}
}
