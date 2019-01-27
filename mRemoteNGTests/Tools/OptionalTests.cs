using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
	public class OptionalTests
	{
		[Test]
		public void OptionalReturnsEmptyListWhenGivenNullValue()
		{
			var sut = new Optional<object>(null);
			Assert.That(sut, Is.Empty);
		}

		[Test]
		public void OptionalReturnsValueIfNotNull()
		{
			var expected = new object();
			var sut = new Optional<object>(expected);
			Assert.That(sut, Has.Member(expected));
		}

	    [Test]
	    public void CallingToOptionalOnNullObjectReturnsEmptyOptional()
	    {
	        var sut = ((object) null).ToOptional();
            Assert.That(sut, Is.Empty);
	    }
	}
}
