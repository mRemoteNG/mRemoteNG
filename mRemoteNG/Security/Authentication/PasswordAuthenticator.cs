using System;
using System.Linq;
using System.Security;
using mRemoteNG.Tools;

namespace mRemoteNG.Security.Authentication
{
    public class PasswordAuthenticator(ICryptographyProvider cryptographyProvider,
                                 string cipherText,
                                 Func<Optional<SecureString>> authenticationRequestor) : IAuthenticator
    {
        private readonly ICryptographyProvider _cryptographyProvider = cryptographyProvider.ThrowIfNull(nameof(cryptographyProvider));
        private readonly string _cipherText = cipherText.ThrowIfNullOrEmpty(nameof(cipherText));
        private readonly Func<Optional<SecureString>> _authenticationRequestor = authenticationRequestor.ThrowIfNull(nameof(authenticationRequestor));

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