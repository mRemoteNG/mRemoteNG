using mRemoteNG.Credential;
using mRemoteNG.Security;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Credential
{
    public class CredentialDomainUserPasswordComparerTests
    {
        const string User = "user1";
        const string Domain = "domain";
        const string Password = "password1";

        private CredentialDomainUserPasswordComparer _comparer;

        [SetUp]
        public void Setup()
        {
            _comparer = new CredentialDomainUserPasswordComparer();
        }

        [Test]
        public void CredentialsWithTheSameUsernameDomainAndPasswordAreEqual()
        {
            var cred1 = Substitute.For<ICredentialRecord>();
            cred1.Username.Returns(User);
            cred1.Domain.Returns(Domain);
            cred1.Password.Returns(Password.ConvertToSecureString());

            var cred2 = Substitute.For<ICredentialRecord>();
            cred2.Username.Returns(User);
            cred2.Domain.Returns(Domain);
            cred2.Password.Returns(Password.ConvertToSecureString());

            Assert.That(_comparer.Equals(cred1, cred2), Is.True);
        }

        [Test]
        public void CredentialsWithDifferentUsernamesAreNotEqual()
        {
            var cred1 = Substitute.For<ICredentialRecord>();
            cred1.Username.Returns("user1");
            cred1.Domain.Returns(Domain);
            cred1.Password.Returns(Password.ConvertToSecureString());

            var cred2 = Substitute.For<ICredentialRecord>();
            cred2.Username.Returns("user2");
            cred2.Domain.Returns(Domain);
            cred2.Password.Returns(Password.ConvertToSecureString());

            Assert.That(_comparer.Equals(cred1, cred2), Is.False);
        }

        [Test]
        public void CredentialsWithDifferentDomainsAreNotEqual()
        {
            var cred1 = Substitute.For<ICredentialRecord>();
            cred1.Username.Returns(User);
            cred1.Domain.Returns("domain1");
            cred1.Password.Returns(Password.ConvertToSecureString());

            var cred2 = Substitute.For<ICredentialRecord>();
            cred2.Username.Returns(User);
            cred2.Domain.Returns("domain2");
            cred2.Password.Returns(Password.ConvertToSecureString());

            Assert.That(_comparer.Equals(cred1, cred2), Is.False);
        }

        [Test]
        public void CredentialsWithDifferentPasswordsAreNotEqual()
        {
            var cred1 = Substitute.For<ICredentialRecord>();
            cred1.Username.Returns(User);
            cred1.Domain.Returns(Domain);
            cred1.Password.Returns("p1".ConvertToSecureString());

            var cred2 = Substitute.For<ICredentialRecord>();
            cred2.Username.Returns(User);
            cred2.Domain.Returns(Domain);
            cred2.Password.Returns("p2".ConvertToSecureString());

            Assert.That(_comparer.Equals(cred1, cred2), Is.False);
        }
    }
}