using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol
{
    [TestFixture]
    public class RdpProtocolCredentialDelegationTests
    {
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("   ", true)]
        [TestCase("user", false)]
        [TestCase("DOMAIN\\user", false)]
        public void ShouldDisableCredentialsDelegation_ReturnsExpectedValue(string username, bool expected)
        {
            bool actual = RdpProtocol.ShouldDisableCredentialsDelegation(username);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
