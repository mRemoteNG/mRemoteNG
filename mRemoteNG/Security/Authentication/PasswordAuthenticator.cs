using System;
using System.Linq;
using System.Security;
using mRemoteNG.Tools;

namespace mRemoteNG.Security.Authentication
{
    public class PasswordAuthenticator : IAuthenticator
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly string _cipherText;
        private readonly Func<Optional<SecureString>> _authenticationRequestor;

        public PasswordAuthenticator(ICryptographyProvider cryptographyProvider,
                                     string cipherText,
                                     Func<Optional<SecureString>> authenticationRequestor)
        {
            ArgumentNullException.ThrowIfNull(cryptographyProvider);
            ArgumentNullException.ThrowIfNull(authenticationRequestor);
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Value cannot be null or empty.", nameof(cipherText));
            _cryptographyProvider = cryptographyProvider;
            _cipherText = cipherText;
            _authenticationRequestor = authenticationRequestor;
        }

        public int MaxAttempts { get; set; } = 3;
        public SecureString? LastAuthenticatedPassword { get; private set; }

        public bool Authenticate(SecureString password)
        {
            bool authenticated = false;
            int attempts = 0;
            while (!authenticated && attempts < MaxAttempts)
            {
                try
                {
                    _cryptographyProvider.Decrypt(_cipherText, password);
                    authenticated = true;
                    LastAuthenticatedPassword = password;
                }
                catch
                {
                    Optional<SecureString> providedPassword = _authenticationRequestor();
                    if (!providedPassword.Any())
                        return false;

                    password = providedPassword.First();
                    if (password == null || password.Length == 0) break;
                }

                attempts++;
            }

            return authenticated;
        }
    }
}