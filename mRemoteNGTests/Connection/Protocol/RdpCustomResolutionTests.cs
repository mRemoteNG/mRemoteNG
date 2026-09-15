using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class RdpCustomResolutionTests
    {
        [TestCase("1920x1080", 1920, 1080)]
        [TestCase("1280X720", 1280, 720)]
        [TestCase(" 2560 x 1440 ", 2560, 1440)]
        [TestCase("1600*900", 1600, 900)]
        public void TryParseCustomResolution_ValidValues_Parsed(string value, int expectedWidth, int expectedHeight)
        {
            bool ok = RdpExtensions.TryParseCustomResolution(value, out int width, out int height);

            Assert.That(ok, Is.True);
            Assert.That(width, Is.EqualTo(expectedWidth));
            Assert.That(height, Is.EqualTo(expectedHeight));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("1920")]
        [TestCase("abc")]
        [TestCase("1920x")]
        [TestCase("100x100")]     // below 200px minimum
        [TestCase("9000x1080")]   // above 8192px maximum
        [TestCase("1920x99999")]
        public void TryParseCustomResolution_InvalidValues_Rejected(string value)
        {
            bool ok = RdpExtensions.TryParseCustomResolution(value, out int width, out int height);

            Assert.That(ok, Is.False);
            Assert.That(width, Is.EqualTo(0));
            Assert.That(height, Is.EqualTo(0));
        }
    }
}
