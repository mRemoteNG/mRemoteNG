using System;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Connections
{
    public class ConnectionsDecryptor
    {
        private readonly ICryptographyProvider _cryptographyProvider;

        public Func<SecureString> AuthenticationRequestor { get; set; }
        public int KeyDerivationIterations { get; set; } = 1000;

        public ConnectionsDecryptor()
        {
            _cryptographyProvider = new LegacyRijndaelCryptographyProvider();
        }

        public ConnectionsDecryptor(BlockCipherEngines blockCipherEngine, BlockCipherModes blockCipherMode)
        {
            _cryptographyProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(blockCipherEngine, blockCipherMode);
            ((AeadCryptographyProvider) _cryptographyProvider).KeyDerivationIterations = KeyDerivationIterations;
        }

        public string Decrypt(string plainText)
        {
            return plainText == "" ? "" : _cryptographyProvider.Decrypt(plainText, Runtime.EncryptionKey);
        }

        public string LegacyFullFileDecrypt(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return "";
            if (xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>")) return xml;

            var decryptedContent = "";
            bool notDecr;

            try
            {
                decryptedContent = _cryptographyProvider.Decrypt(xml, Runtime.EncryptionKey);
                notDecr = decryptedContent == xml;
            }
            catch (Exception)
            {
                notDecr = true;
            }

            if (notDecr)
            {
                if (Authenticate(xml, Runtime.EncryptionKey))
                {
                    decryptedContent = _cryptographyProvider.Decrypt(xml, Runtime.EncryptionKey);
                    notDecr = false;
                }

                if (notDecr == false)
                    return decryptedContent;
            }
            else
            {
                return decryptedContent;
            }

            return "";
        }

        public bool ConnectionsFileIsAuthentic(string protectedString, SecureString password, RootNodeInfo rootInfo)
        {
            var connectionsFileIsNotEncrypted = false;
            try
            {
                connectionsFileIsNotEncrypted = _cryptographyProvider.Decrypt(protectedString, password) == "ThisIsNotProtected";
            }
            catch (EncryptionException)
            {
            }
            return connectionsFileIsNotEncrypted || Authenticate(protectedString, password, rootInfo);
        }

        private bool Authenticate(string cipherText, SecureString password, RootNodeInfo rootInfo = null)
        {
            var authenticator = new PasswordAuthenticator(_cryptographyProvider, cipherText)
            {
                AuthenticationRequestor = AuthenticationRequestor
            };

            var authenticated = authenticator.Authenticate(password);

            if (!authenticated) return authenticated;
            Runtime.EncryptionKey = authenticator.LastAuthenticatedPassword;
            if (rootInfo == null) return authenticated;
            rootInfo.Password = true;
            rootInfo.PasswordString = Runtime.EncryptionKey.ConvertToUnsecureString();
            return authenticated;
        }
    }
}