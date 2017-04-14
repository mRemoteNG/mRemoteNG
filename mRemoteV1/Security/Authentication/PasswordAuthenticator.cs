using System;
using System.Security;

namespace mRemoteNG.Security.Authentication
{
    public class PasswordAuthenticator : IAuthenticator
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly string _cipherText;

        public Func<SecureString> AuthenticationRequestor { get; set; }
        public int MaxAttempts { get; set; } = 3;
        public SecureString LastAuthenticatedPassword { get; private set; }

        public PasswordAuthenticator(ICryptographyProvider cryptographyProvider, string cipherText)
        {
            _cryptographyProvider = cryptographyProvider;
            _cipherText = cipherText;
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
                    password = AuthenticationRequestor?.Invoke();
                    if (password == null || password.Length == 0) break;
                }
                attempts++;
            }
            return authenticated;
        }
    }
}