using System;
using System.Runtime.Versioning;
using System.Security;
using mRemoteNG.App;
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

            string decryptedContent = "";
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
            if (TryDecryptProtectionMarker(protectedString, new RootNodeInfo(RootNodeType.Connection).DefaultPassword.ConvertToSecureString(), out string? defaultMarker) &&
                defaultMarker == "ThisIsNotProtected")
            {
                _rootNodeInfo.PasswordString = "";
                if (!Runtime.HasActiveMasterPasswordSession)
                    Runtime.ResetEncryptionKey();
                return true;
            }

            if (TryDecryptProtectionMarker(protectedString, _rootNodeInfo.PasswordString.ConvertToSecureString(), out string? currentMarker))
            {
                if (currentMarker == "ThisIsProtected")
                {
                    if (!Runtime.HasActiveMasterPasswordSession)
                        Runtime.SetEncryptionKey(_rootNodeInfo.PasswordString);
                    return true;
                }

                if (currentMarker == "ThisIsNotProtected")
                {
                    _rootNodeInfo.PasswordString = "";
                    if (!Runtime.HasActiveMasterPasswordSession)
                        Runtime.ResetEncryptionKey();
                    return true;
                }
            }

            return Authenticate(protectedString, _rootNodeInfo.PasswordString.ConvertToSecureString());
        }

        private bool Authenticate(string cipherText, SecureString password)
        {
            PasswordAuthenticator authenticator = new(_cryptographyProvider, cipherText, AuthenticationRequestor);
            bool authenticated = authenticator.Authenticate(password);

            if (!authenticated)
                return false;

            _rootNodeInfo.PasswordString = authenticator.LastAuthenticatedPassword.ConvertToUnsecureString();
            if (!Runtime.HasActiveMasterPasswordSession)
                Runtime.SetEncryptionKey(authenticator.LastAuthenticatedPassword);
            return true;
        }

        private bool TryDecryptProtectionMarker(string protectedString, SecureString key, out string marker)
        {
            marker = string.Empty;

            try
            {
                marker = _cryptographyProvider.Decrypt(protectedString, key);
                return true;
            }
            catch (EncryptionException)
            {
                return false;
            }
        }
    }
}
