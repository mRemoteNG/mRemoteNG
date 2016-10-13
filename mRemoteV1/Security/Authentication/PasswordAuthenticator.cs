using System;
using System.Security;
using Org.BouncyCastle.Security;

namespace mRemoteNG.Security.Authentication
{
    public class PasswordAuthenticator : IAuthenticator
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly string _cipherText;

        public Func<SecureString> AuthenticationRequestor { get; set; }

        public PasswordAuthenticator(ICryptographyProvider cryptographyProvider, string cipherText)
        {
            _cryptographyProvider = cryptographyProvider;
            _cipherText = cipherText;
        }

        public bool Authenticate(SecureString password)
        {
            var authenticated = false;
            while (!authenticated)
            {
                try
                {
                    _cryptographyProvider.Decrypt(_cipherText, password);
                    authenticated = true;
                }
                catch (EncryptionException)
                {
                    password = AuthenticationRequestor?.Invoke();
                    if (password == null || password.Length == 0) break;
                }
            }
            return authenticated;
        }
    }
}