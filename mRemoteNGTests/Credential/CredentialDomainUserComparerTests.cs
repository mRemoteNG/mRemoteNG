using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialDomainUserComparerTests
    {
        private CredentialDomainUserComparer _comparer;

        [SetUp]
        public void Setup()
        {
            _comparer = new CredentialDomainUserComparer();
        }

        [Test]
        public void CredentialsWithTheSameUsernameAndDomainAreEqual()
        {
            const string user = "user1";
            const string domain = "domain";
            var cred1 = Substitute.For<ICredentialRecord>();
            cred1.Username.Returns(user);
            cred1.Domain.Returns(domain);
            var cred2 = Substitute.For<ICredentialRecord>();
            cred2.Username.Returns(user);
            cred2.Domain.Returns(domain);
            Assert.That(_comparer.Equals(cred1, cred2), Is.True);
        }

        [Test]
        public void CredentialsWithDifferentUsernamesAreNotEqual()
        {
            const string domain = "domain";
            var cred1 = Substitute.For<ICredentialRecord>();
            cred1.Username.Returns("user1");
            cred1.Domain.Returns(domain);
            var cred2 = Substitute.For<ICredentialRecord>();
            cred2.Username.Returns("user2");
            cred2.Domain.Returns(domain);
            Assert.That(_comparer.Equals(cred1, cred2), Is.False);
        }

        [Test]
        public void CredentialsWithDifferentDomainsAreNotEqual()
        {
            const string user = "user1";
            var cred1 = Substitute.For<ICredentialRecord>();
            cred1.Username.Returns(user);
            cred1.Domain.Returns("domain1");
            var cred2 = Substitute.For<ICredentialRecord>();
            cred2.Username.Returns(user);
            cred2.Domain.Returns("domain2");
            Assert.That(_comparer.Equals(cred1, cred2), Is.False);
        }
    }
}