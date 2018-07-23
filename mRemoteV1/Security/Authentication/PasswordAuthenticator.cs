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

        public int MaxAttempts { get; set; } = 3;
        public SecureString LastAuthenticatedPassword { get; private set; }

        public PasswordAuthenticator(ICryptographyProvider cryptographyProvider, string cipherText, Func<Optional<SecureString>> authenticationRequestor)
        {
            _cryptographyProvider = cryptographyProvider.ThrowIfNull(nameof(cryptographyProvider));
            _cipherText = cipherText.ThrowIfNullOrEmpty(nameof(cipherText));
            _authenticationRequestor = authenticationRequestor.ThrowIfNull(nameof(authenticationRequestor));
        }

        public bool Authenticate(SecureString password)
        {
            var authenticated = false;
            var attempts = 0;
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
                    var providedPassword = _authenticationRequestor();
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