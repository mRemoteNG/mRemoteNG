using System;
using System.Security;
using mRemoteNG.App;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.Config.Connections
{
    public class ConnectionsDecryptor
    {
        private readonly ICryptographyProvider _cryptographyProvider;

        public ConnectionsDecryptor()
        {
            _cryptographyProvider = new LegacyRijndaelCryptographyProvider();
        }

        public ConnectionsDecryptor(BlockCipherEngines blockCipherEngine, BlockCipherModes blockCipherMode)
        {
            _cryptographyProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(blockCipherEngine, blockCipherMode);
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
                if (Authenticate(xml))
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

        public bool ConnectionsFileIsAuthentic(string protectedString, RootNodeInfo rootInfo)
        {
            var connectionsFileIsNotEncrypted = false;
            try
            {
                connectionsFileIsNotEncrypted = _cryptographyProvider.Decrypt(protectedString, Runtime.EncryptionKey) == "ThisIsNotProtected";
            }
            catch (EncryptionException)
            {
            }
            return connectionsFileIsNotEncrypted || Authenticate(protectedString, rootInfo);
        }

        private bool Authenticate(string cipherText, RootNodeInfo rootInfo = null)
        {
            var authenticator = new PasswordAuthenticator(_cryptographyProvider, cipherText)
            {
                AuthenticationRequestor = PromptForPassword
            };

            var authenticated = authenticator.Authenticate(Runtime.EncryptionKey);

            if (authenticated && rootInfo != null)
            {
                rootInfo.Password = true;
                rootInfo.PasswordString = Runtime.EncryptionKey.ConvertToUnsecureString();
            }

            return authenticated;
        }

        private SecureString PromptForPassword()
        {
            var password = MiscTools.PasswordDialog("", false);
            Runtime.EncryptionKey = password;
            return password;
        }
    }
}