using System.Security;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using NUnit.Framework;


namespace mRemoteNGTests.Security.Authentication
{
    public class PasswordAuthenticatorTests
    {
        private ICryptographyProvider _cryptographyProvider;
        private string _cipherText;
        private readonly SecureString _correctPassword = "9theCorrectPass#5".ConvertToSecureString();
        private readonly SecureString _wrongPassword = "wrongPassword".ConvertToSecureString();

        [SetUp]
        public void Setup()
        {
            _cryptographyProvider = new AeadCryptographyProvider {KeyDerivationIterations = 10000};
            _cipherText = "MPELiwk7+xeNlruIyt5uxTvVB+/RLVoLdUGnwY4CWCqwKe7T2IBwWo4oaKum5hdv7447g5m2nZsYPrfARSlotQB4r1KZQg==";
        }

        [Test]
        public void AuthenticatingWithCorrectPasswordReturnsTrue()
        {
            var authenticator = new PasswordAuthenticator(_cryptographyProvider, _cipherText, () => Optional<SecureString>.Empty);
            var authenticated = authenticator.Authenticate(_correctPassword);
            Assert.That(authenticated);
        }

        [Test]
        public void AuthenticatingWithWrongPasswordReturnsFalse()
        {
            var authenticator = new PasswordAuthenticator(_cryptographyProvider, _cipherText, () => Optional<SecureString>.Empty);
            var authenticated = authenticator.Authenticate(_wrongPassword);
            Assert.That(!authenticated);
        }

        [Test]
        public void AuthenticationRequestorIsCalledWhenInitialPasswordIsWrong()
        {
            var wasCalled = false;

            Optional<SecureString> AuthenticationRequestor()
            {
                wasCalled = true;
                return _correctPassword;
            }

            var authenticator = new PasswordAuthenticator(_cryptographyProvider, _cipherText, AuthenticationRequestor);
            authenticator.Authenticate(_wrongPassword);
            Assert.That(wasCalled);
        }

        [Test]
        public void AuthenticationRequestorNotCalledWhenInitialPasswordIsCorrect()
        {
            var wasCalled = false;
            Optional<SecureString> AuthenticationRequestor()
            {
                wasCalled = true;
                return _correctPassword;
            }

            var authenticator = new PasswordAuthenticator(_cryptographyProvider, _cipherText, AuthenticationRequestor);
            authenticator.Authenticate(_correctPassword);
            Assert.That(!wasCalled);
        }

        [Test]
        public void ProvidingCorrectPasswordToTheAuthenticationRequestorReturnsTrue()
        {
            var authenticator = new PasswordAuthenticator(_cryptographyProvider, _cipherText, () => _correctPassword);
            var authenticated = authenticator.Authenticate(_wrongPassword);
            Assert.That(authenticated);
        }

        [Test]
        public void AuthenticationFailsWhenAuthenticationRequestorGivenEmptyPassword()
        {
            var authenticator = new PasswordAuthenticator(_cryptographyProvider, _cipherText, () => new SecureString());
            var authenticated = authenticator.Authenticate(_wrongPassword);
            Assert.That(!authenticated);
        }

        [Test]
        public void AuthenticatorRespectsMaxAttempts()
        {
            var authAttempts = 0;
            Optional<SecureString>  AuthenticationRequestor()
            {
                authAttempts++;
                return _wrongPassword;
            }

            var authenticator = new PasswordAuthenticator(_cryptographyProvider, _cipherText, AuthenticationRequestor);
            authenticator.Authenticate(_wrongPassword);
            Assert.That(authAttempts == authenticator.MaxAttempts);
        }

        [Test]
        public void AuthenticatorRespectsMaxAttemptsCustomValue()
        {
            const int customMaxAttempts = 5;
            var authAttempts = 0;
            Optional<SecureString> AuthenticationRequestor()
            {
                authAttempts++;
                return _wrongPassword;
            }

            var authenticator =
                new PasswordAuthenticator(_cryptographyProvider, _cipherText, AuthenticationRequestor)
                {
                    MaxAttempts = customMaxAttempts
                };
            authenticator.Authenticate(_wrongPassword);
            Assert.That(authAttempts == customMaxAttempts);
        }
    }
}