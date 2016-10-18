using System.Security;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.SymmetricEncryption;
using NUnit.Framework;


namespace mRemoteNGTests.Security.Authentication
{
    public class PasswordAuthenticatorTests
    {
        private PasswordAuthenticator _authenticator;
        private readonly SecureString _correctPassword = "9theCorrectPass#5".ConvertToSecureString();
        private readonly SecureString _wrongPassword = "wrongPassword".ConvertToSecureString();

        [SetUp]
        public void Setup()
        {
            var cryptoProvider = new AeadCryptographyProvider {KeyDerivationIterations = 10000};
            const string cipherText = "MPELiwk7+xeNlruIyt5uxTvVB+/RLVoLdUGnwY4CWCqwKe7T2IBwWo4oaKum5hdv7447g5m2nZsYPrfARSlotQB4r1KZQg==";
            _authenticator = new PasswordAuthenticator(cryptoProvider, cipherText);
        }

        [TearDown]
        public void Teardown()
        {
            _authenticator = null;
        }

        [Test]
        public void AuthenticatingWithCorrectPasswordReturnsTrue()
        {
            var authenticated = _authenticator.Authenticate(_correctPassword);
            Assert.That(authenticated);
        }

        [Test]
        public void AuthenticatingWithWrongPasswordReturnsFalse()
        {
            var authenticated = _authenticator.Authenticate(_wrongPassword);
            Assert.That(!authenticated);
        }

        [Test]
        public void AuthenticationRequestorIsCalledWhenInitialPasswordIsWrong()
        {
            var wasCalled = false;
            _authenticator.AuthenticationRequestor = () =>
            {
                wasCalled = true;
                return _correctPassword;
            };
            _authenticator.Authenticate(_wrongPassword);
            Assert.That(wasCalled);
        }

        [Test]
        public void AuthenticationRequestorNotCalledWhenInitialPasswordIsCorrect()
        {
            var wasCalled = false;
            _authenticator.AuthenticationRequestor = () =>
            {
                wasCalled = true;
                return _correctPassword;
            };
            _authenticator.Authenticate(_correctPassword);
            Assert.That(!wasCalled);
        }

        [Test]
        public void ProvidingCorrectPasswordToTheAuthenticationRequestorReturnsTrue()
        {
            _authenticator.AuthenticationRequestor = () => _correctPassword;
            var authenticated = _authenticator.Authenticate(_wrongPassword);
            Assert.That(authenticated);
        }

        [Test]
        public void AuthenticationFailsWhenAuthenticationRequestorGivenEmptyPassword()
        {
            _authenticator.AuthenticationRequestor = () => new SecureString();
            var authenticated = _authenticator.Authenticate(_wrongPassword);
            Assert.That(!authenticated);
        }

        [Test]
        public void AuthenticatorRespectsMaxAttempts()
        {
            var authAttempts = 0;
            _authenticator.AuthenticationRequestor = () =>
            {
                authAttempts++;
                return _wrongPassword;
            };
            _authenticator.Authenticate(_wrongPassword);
            Assert.That(authAttempts == _authenticator.MaxAttempts);
        }

        [Test]
        public void AuthenticatorRespectsMaxAttemptsCustomValue()
        {
            const int customMaxAttempts = 5;
            _authenticator.MaxAttempts = customMaxAttempts;
            var authAttempts = 0;
            _authenticator.AuthenticationRequestor = () =>
            {
                authAttempts++;
                return _wrongPassword;
            };
            _authenticator.Authenticate(_wrongPassword);
            Assert.That(authAttempts == customMaxAttempts);
        }
    }
}