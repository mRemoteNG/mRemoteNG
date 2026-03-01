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
        private SecureString? _cachedDecryptionKey;

        public Func<Optional<SecureString>>? AuthenticationRequestor { get; set; }

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

        private SecureString GetDecryptionKey()
        {
            return _cachedDecryptionKey ??= _rootNodeInfo.PasswordString.ConvertToSecureString();
        }

        private void InvalidateKeyCache()
        {
            _cachedDecryptionKey = null;
        }

        public string Decrypt(string plainText)
        {
            return plainText == ""
                ? ""
                : _cryptographyProvider.Decrypt(plainText, GetDecryptionKey());
        }

        public string LegacyFullFileDecrypt(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return "";
            if (xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>")) return xml;

            string decryptedContent = "";
            bool notDecr;

            try
            {
                decryptedContent = _cryptographyProvider.Decrypt(xml, GetDecryptionKey());
                notDecr = decryptedContent == xml;
            }
            catch (Exception)
            {
                notDecr = true;
            }

            if (notDecr)
            {
                if (Authenticate(xml, GetDecryptionKey()))
                {
                    decryptedContent =
                        _cryptographyProvider.Decrypt(xml, GetDecryptionKey());
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
            bool connectionsFileIsNotEncrypted = false;
            try
            {
                connectionsFileIsNotEncrypted = string.Equals(_cryptographyProvider.Decrypt(protectedString, GetDecryptionKey()), "ThisIsNotProtected", StringComparison.Ordinal);
            }
            catch (EncryptionException)
            {
                _ = 0; // Intentionally empty — file is not encrypted
            }

            return connectionsFileIsNotEncrypted || Authenticate(protectedString, GetDecryptionKey());
        }

        private bool Authenticate(string cipherText, SecureString password)
        {
            if (AuthenticationRequestor is null)
                return false;

            PasswordAuthenticator authenticator = new(_cryptographyProvider, cipherText, AuthenticationRequestor);
            bool authenticated = authenticator.Authenticate(password);

            if (!authenticated || authenticator.LastAuthenticatedPassword is null)
                return false;

            _rootNodeInfo.PasswordString = authenticator.LastAuthenticatedPassword.ConvertToUnsecureString();
            InvalidateKeyCache();
            return true;
        }
    }
}
