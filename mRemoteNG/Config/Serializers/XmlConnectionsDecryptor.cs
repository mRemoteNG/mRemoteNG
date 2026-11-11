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

        /// <summary>
        /// Used for providers that aren't built from the (engine, mode) pair above — currently only
        /// <see cref="SymmetricEncryption.DpapiCryptographyProvider"/>, which has no engine/mode/KDF
        /// of its own.
        /// </summary>
        public XmlConnectionsDecryptor(ICryptographyProvider cryptographyProvider, RootNodeInfo rootNodeInfo)
        {
            _cryptographyProvider = cryptographyProvider;
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
            if (xml.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>") || xml.Contains("<?xml version='1.0' encoding='utf-8'?>")) return xml;

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
            bool connectionsFileIsNotEncrypted = false;
            try
            {
                connectionsFileIsNotEncrypted = _cryptographyProvider.Decrypt(protectedString, _rootNodeInfo.PasswordString.ConvertToSecureString()) == "ThisIsNotProtected";
            }
            catch (EncryptionException)
            {
            }

            return connectionsFileIsNotEncrypted || Authenticate(protectedString, _rootNodeInfo.PasswordString.ConvertToSecureString());
        }

        /// <summary>
        /// DPAPI equivalent of <see cref="ConnectionsFileIsAuthentic"/>. There is no password to try
        /// or re-prompt for — <see cref="SymmetricEncryption.DpapiCryptographyProvider.Decrypt"/>
        /// either unprotects the root <c>Protected</c> value (same Windows account + machine that
        /// saved it) or throws <see cref="EncryptionException"/>, which this method lets propagate
        /// so the caller fails the whole load instead of continuing with an unreadable root.
        /// </summary>
        /// <param name="protectedString">The root element's <c>Protected</c> attribute value.</param>
        /// <exception cref="EncryptionException">
        /// The file is missing a valid root protection marker, was protected under a different
        /// Windows user account or machine, or its DPAPI master key changed/was removed.
        /// </exception>
        public void VerifyDpapiProtectedRoot(string protectedString)
        {
            if (string.IsNullOrEmpty(protectedString))
                throw new EncryptionException("The DPAPI-protected connections file is missing its root protection marker.");

            string decryptedMarker = _cryptographyProvider.Decrypt(protectedString, null);
            if (decryptedMarker != "ThisIsNotProtected")
                throw new EncryptionException("The DPAPI-protected connections file has an invalid root protection marker.");
        }

        private bool Authenticate(string cipherText, SecureString password)
        {
            PasswordAuthenticator authenticator = new(_cryptographyProvider, cipherText, AuthenticationRequestor);
            bool authenticated = authenticator.Authenticate(password);

            if (!authenticated)
                return false;

            // A successful Authenticate() guarantees LastAuthenticatedPassword is set.
            _rootNodeInfo.PasswordString = authenticator.LastAuthenticatedPassword!.ConvertToUnsecureString();
            return true;
        }
    }
}