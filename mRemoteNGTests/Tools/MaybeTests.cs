using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
	public class MaybeTests
	{
		[Test]
		public void MaybeReturnsEmptyListWhenGivenNullValue()
		{
			var sut = new Maybe<object>(null);
			Assert.That(sut, Is.Empty);
		}

		[Test]
		public void MaybeReturnsValueIfNotNull()
		{
			var expected = new object();
			var sut = new Maybe<object>(expected);
			Assert.That(sut, Has.Member(expected));
		}
	}
}
