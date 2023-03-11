using System;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;
using mRemoteNG.Security.Factories;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers
{
    [SupportedOSPlatform("windows")]
    public class XmlConnectionsDecryptor
    {
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly RootNodeInfo _rootNodeInfo;

        public Func<Optional<SecureString>> AuthenticationRequestor { get; set; }

        public int KeyDerivationIterations
        {
            get { return _cryptographyProvider.KeyDerivationIterations; }
            set { _cryptographyProvider.KeyDerivationIterations = value; }
        }


        public XmlConnectionsDecryptor(RootNodeInfo rootNodeInfo)
        {
            _cryptographyProvider = new LegacyRijndaelCryptographyProvider();
            _rootNodeInfo = rootNodeInfo;
        }

        public XmlConnectionsDecryptor(BlockCipherEngines blockCipherEngine, BlockCipherModes blockCipherMode, RootNodeInfo rootNodeInfo)
        {
            _cryptographyProvider = new CryptoProviderFactory(blockCipherEngine, blockCipherMode).Build();
            _rootNodeInfo = rootNodeInfo;
        }

        public string Decrypt(string plainText)
        {
            return plainText == ""
                ? ""
                : _cryptographyProvider.Decrypt(plainText, _rootNodeInfo.PasswordString.ConvertToSecureString());
        }

        public string LegacyFullFileDecrypt(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return "";
            if (xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>")) return xml;

            var decryptedContent = "";
            bool notDecr;

            try
            {
                decryptedContent = _cryptographyProvider.Decrypt(xml, _rootNodeInfo.PasswordString.ConvertToSecureString());
                notDecr = decryptedContent == xml;
            }
            catch (Exception)
            {
                notDecr = true;
            }

            if (notDecr)
            {
                if (Authenticate(xml, _rootNodeInfo.PasswordString.ConvertToSecureString()))
                {
                    decryptedContent =
                        _cryptographyProvider.Decrypt(xml, _rootNodeInfo.PasswordString.ConvertToSecureString());
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

        public bool ConnectionsFileIsAuthentic(string protectedString, SecureString password)
        {
            var connectionsFileIsNotEncrypted = false;
            try
            {
                connectionsFileIsNotEncrypted = _cryptographyProvider.Decrypt(protectedString, _rootNodeInfo.PasswordString.ConvertToSecureString()) == "ThisIsNotProtected";
            }
            catch (EncryptionException)
            {
            }

            return connectionsFileIsNotEncrypted || Authenticate(protectedString, _rootNodeInfo.PasswordString.ConvertToSecureString());
        }

        private bool Authenticate(string cipherText, SecureString password)
        {
            var authenticator = new PasswordAuthenticator(_cryptographyProvider, cipherText, AuthenticationRequestor);
            var authenticated = authenticator.Authenticate(password);

            if (!authenticated)
                return false;

            _rootNodeInfo.PasswordString = authenticator.LastAuthenticatedPassword.ConvertToUnsecureString();
            return true;
        }
    }
}