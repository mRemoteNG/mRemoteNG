using System.Collections.Generic;
using System.Net;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    public class IpRangeParserTests
    {
        [TestCase("192.168.1.1")]
        [TestCase("  192.168.1.1  ")]
        [TestCase("2001:db8::1")]
        public void SingleAddressYieldsAOneAddressRange(string input)
        {
            bool parsed = IpRangeParser.TryParse(input, out IPAddress? start, out IPAddress? end, out string error);

            Assert.Multiple(() =>
            {
                Assert.That(parsed, Is.True, error);
                Assert.That(start, Is.EqualTo(end));
                Assert.That(start?.ToString(), Is.EqualTo(input.Trim()));
            });
        }

        [TestCase("192.168.1.1 - 192.168.1.254", "192.168.1.1", "192.168.1.254")]
        [TestCase("192.168.1.1-192.168.1.254", "192.168.1.1", "192.168.1.254")]
        [TestCase("2001:db8::1 - 2001:db8::ff", "2001:db8::1", "2001:db8::ff")]
        public void ExplicitRangeIsParsed(string input, string expectedStart, string expectedEnd)
        {
            bool parsed = IpRangeParser.TryParse(input, out IPAddress? start, out IPAddress? end, out string error);

            Assert.Multiple(() =>
            {
                Assert.That(parsed, Is.True, error);
                Assert.That(start?.ToString(), Is.EqualTo(expectedStart));
                Assert.That(end?.ToString(), Is.EqualTo(expectedEnd));
            });
        }

        [Test]
        public void ReversedRangeIsNormalised()
        {
            IpRangeParser.TryParse("192.168.1.254 - 192.168.1.1", out IPAddress? start, out IPAddress? end, out _);

            Assert.Multiple(() =>
            {
                Assert.That(start?.ToString(), Is.EqualTo("192.168.1.1"));
                Assert.That(end?.ToString(), Is.EqualTo("192.168.1.254"));
            });
        }

        [TestCase("192.168.1.0/24", "192.168.1.0", "192.168.1.255")]
        [TestCase("192.168.1.130/24", "192.168.1.0", "192.168.1.255")]
        [TestCase("10.1.2.3/32", "10.1.2.3", "10.1.2.3")]
        [TestCase("10.0.0.0/16", "10.0.0.0", "10.0.255.255")]
        [TestCase("172.16.4.6/30", "172.16.4.4", "172.16.4.7")]
        [TestCase("2001:db8::/120", "2001:db8::", "2001:db8::ff")]
        [TestCase("2001:db8::1/128", "2001:db8::1", "2001:db8::1")]
        public void CidrBlockIsExpandedToItsFirstAndLastAddress(string input, string expectedStart, string expectedEnd)
        {
            bool parsed = IpRangeParser.TryParse(input, out IPAddress? start, out IPAddress? end, out string error);

            Assert.Multiple(() =>
            {
                Assert.That(parsed, Is.True, error);
                Assert.That(start?.ToString(), Is.EqualTo(expectedStart));
                Assert.That(end?.ToString(), Is.EqualTo(expectedEnd));
            });
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("not an address")]
        [TestCase("192.168.1.256")]
        [TestCase("192.168.1.1 - ")]
        [TestCase("192.168.1.1 - 2001:db8::1")]
        [TestCase("192.168.1.0/33")]
        [TestCase("2001:db8::/129")]
        [TestCase("192.168.1.0/-1")]
        [TestCase("192.168.1.0/abc")]
        public void InvalidInputIsRejectedWithAReason(string input)
        {
            bool parsed = IpRangeParser.TryParse(input, out IPAddress? start, out IPAddress? end, out string error);

            Assert.Multiple(() =>
            {
                Assert.That(parsed, Is.False);
                Assert.That(start, Is.Null);
                Assert.That(end, Is.Null);
                Assert.That(error, Is.Not.Empty);
            });
        }

        [Test]
        public void ParseThrowsWithAUserReadableMessage()
        {
            System.ArgumentException? ex =
                Assert.Throws<System.ArgumentException>(() => IpRangeParser.Parse("nonsense"));

            Assert.That(ex?.Message, Does.Contain("nonsense"));
        }

        [Test]
        public void ParsedCidrBlockCanDriveAPortScannerRange()
        {
            (IPAddress start, IPAddress end) = IpRangeParser.Parse("192.168.1.0/30");

            // The scanner enumerates the range inclusively, so a /30 covers four addresses.
            List<int> ports = [80];
            Assert.DoesNotThrow(() => _ = new PortScanner(start, end, ports));
        }
    }
}
