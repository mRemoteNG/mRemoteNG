using mRemoteNG.Connection.Protocol.RDP;
using NUnit.Framework;

namespace mRemoteNGTests.Connection.Protocol.RDP
{
    [TestFixture]
    public class RdpProtocolParseDomainFromUsernameTests
    {
        [TestCase("AzureAD\\user@domain.com", "user@domain.com", "AzureAD")]
        [TestCase("CORP\\john", "john", "CORP")]
        [TestCase("DOMAIN\\user\\extra", "user\\extra", "DOMAIN")]
        public void ParseDomainFromUsername_WithBackslashPrefix_SplitsCorrectly(
            string input, string expectedUser, string expectedDomain)
        {
            var (user, domain) = RdpProtocol.ParseDomainFromUsername(input);

            Assert.That(user, Is.EqualTo(expectedUser));
            Assert.That(domain, Is.EqualTo(expectedDomain));
        }

        [TestCase("user@domain.com")]
        [TestCase("plainuser")]
        public void ParseDomainFromUsername_WithoutBackslash_ReturnsUnchangedAndEmptyDomain(string input)
        {
            var (user, domain) = RdpProtocol.ParseDomainFromUsername(input);

            Assert.That(user, Is.EqualTo(input));
            Assert.That(domain, Is.Empty);
        }

        [Test]
        public void ParseDomainFromUsername_WithEmptyString_ReturnsEmptyAndEmptyDomain()
        {
            var (user, domain) = RdpProtocol.ParseDomainFromUsername(string.Empty);

            Assert.That(user, Is.Empty);
            Assert.That(domain, Is.Empty);
        }
    }
}
