using System.Collections.Generic;
using mRemoteNG.Tools;
using NUnit.Framework;

namespace mRemoteNGTests.Tools
{
    public class PortListParserTests
    {
        [Test]
        public void ListOfPortsIsParsed()
        {
            bool parsed = PortListParser.TryParse("22, 80, 443, 3389", out List<int> ports, out string error);

            Assert.Multiple(() =>
            {
                Assert.That(parsed, Is.True, error);
                Assert.That(ports, Is.EqualTo(new[] { 22, 80, 443, 3389 }));
            });
        }

        [TestCase("22;80 443")]
        [TestCase("  443 ,22,  80 ")]
        public void SemicolonsSpacesAndPaddingAreAccepted(string input)
        {
            PortListParser.TryParse(input, out List<int> ports, out string error);

            Assert.That(ports, Is.SupersetOf(new[] { 22, 80 }), error);
        }

        [Test]
        public void RangeIsExpanded()
        {
            PortListParser.TryParse("8000-8003", out List<int> ports, out _);

            Assert.That(ports, Is.EqualTo(new[] { 8000, 8001, 8002, 8003 }));
        }

        [Test]
        public void MixedListAndRangeIsSortedAndDeduplicated()
        {
            PortListParser.TryParse("443, 22, 80, 8000-8002, 80, 8001", out List<int> ports, out _);

            Assert.That(ports, Is.EqualTo(new[] { 22, 80, 443, 8000, 8001, 8002 }));
        }

        [Test]
        public void ReversedRangeIsNormalised()
        {
            PortListParser.TryParse("8003-8000", out List<int> ports, out _);

            Assert.That(ports, Is.EqualTo(new[] { 8000, 8001, 8002, 8003 }));
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("0")]
        [TestCase("65536")]
        [TestCase("-1")]
        [TestCase("http")]
        [TestCase("22, abc")]
        [TestCase("8000-")]
        [TestCase("8000-99999")]
        public void InvalidInputIsRejectedWithAReason(string input)
        {
            bool parsed = PortListParser.TryParse(input, out List<int> ports, out string error);

            Assert.Multiple(() =>
            {
                Assert.That(parsed, Is.False);
                Assert.That(ports, Is.Empty);
                Assert.That(error, Is.Not.Empty);
            });
        }

        [Test]
        public void AllPortsCoversTheWholeRange()
        {
            List<int> ports = PortListParser.AllPorts();

            Assert.Multiple(() =>
            {
                Assert.That(ports, Has.Count.EqualTo(65535));
                Assert.That(ports[0], Is.EqualTo(PortListParser.MinPort));
                Assert.That(ports[^1], Is.EqualTo(PortListParser.MaxPort));
            });
        }
    }
}
