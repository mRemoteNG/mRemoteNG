using System;
using System.Text;
using NUnit.Framework;
using mRemoteNG.Connection.Protocol.RDP;

namespace mRemoteNGTests.Connection.Protocol.RDP
{
    [TestFixture]
    public class AzureLoadBalanceInfoEncoderTests
    {
        [Test]
        public void Encode_BasicString_ReturnsEncoded()
        {
            string input = "test";
            string output = AzureLoadBalanceInfoEncoder.Encode(input);
            Assert.That(output.Length, Is.EqualTo(3));
        }

        [Test]
        public void Encode_OddLengthString_ReturnsEncodedWithPadding()
        {
            string input = "test1";
            string output = AzureLoadBalanceInfoEncoder.Encode(input);
            Assert.That(output.Length, Is.EqualTo(4));
        }

        [Test]
        public void Encode_EmptyString_ReturnsEncoded()
        {
            string input = "";
            string output = AzureLoadBalanceInfoEncoder.Encode(input);
            Assert.That(output.Length, Is.EqualTo(1));
            Assert.That((int)output[0], Is.EqualTo(0x0A0D));
        }

        [Test]
        public void Encode_ComplexString_ReturnsEncoded()
        {
            string input = "Cookie: msts=3640205228.20480.0000";
            string output = AzureLoadBalanceInfoEncoder.Encode(input);
            Assert.That(output, Is.Not.Null);
            Assert.That(output, Is.Not.Empty);
        }
    }
}
