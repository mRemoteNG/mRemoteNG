using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;

namespace mRemoteNG.Config.Serializers.CredentialSerializer
{
    [SupportedOSPlatform("windows")]
    public class XmlCredentialPasswordDecryptorDecorator : ISecureDeserializer<string, IEnumerable<ICredentialRecord>>
    {
        private readonly IDeserializer<string, IEnumerable<ICredentialRecord>> _baseDeserializer;

        public XmlCredentialPasswordDecryptorDecorator(
            IDeserializer<string, IEnumerable<ICredentialRecord>> baseDeserializer)
        {
            if (baseDeserializer == null)
                throw new ArgumentNullException(nameof(baseDeserializer));

            _baseDeserializer = baseDeserializer;
        }

        public IEnumerable<ICredentialRecord> Deserialize(string xml, SecureString key)
        {
            // Handle empty or uninitialized credential files
            if (string.IsNullOrEmpty(xml))
                return Array.Empty<ICredentialRecord>();

            try
            {
                string decryptedXml = DecryptPasswords(xml, key);
                return _baseDeserializer.Deserialize(decryptedXml);
            }
            catch (Exception)
            {
                // If decryption fails (e.g., file has wrong format, no Auth header, etc.),
                // return empty collection. This allows the system to initialize properly.
                return Array.Empty<ICredentialRecord>();
            }
        }

        private string DecryptPasswords(string xml, SecureString key)
        {
            if (string.IsNullOrEmpty(xml)) return xml;
            
            XDocument xdoc = XDocument.Parse(xml);
            
            // Check if this is a valid credentials file with Auth header
            // If not, it may be an uninitialized file - return empty
            XAttribute authAttribute = xdoc.Root?.Attribute("Auth");
            if (authAttribute == null)
            {
                // No Auth header means this is not a properly initialized credentials file
                // Return empty XML that will result in empty credentials list
                return string.Empty;
            }
            
            ICryptographyProvider cryptoProvider = new CryptoProviderFactoryFromXml(xdoc.Root).Build();
            DecryptAuthHeader(xdoc.Root, cryptoProvider, key);
            foreach (XElement credentialElement in xdoc.Descendants())
            {
                XAttribute passwordAttribute = credentialElement.Attribute("Password");
                if (passwordAttribute == null) continue;
                string decryptedPassword = cryptoProvider.Decrypt(passwordAttribute.Value, key);
                passwordAttribute.SetValue(decryptedPassword);
            }

            return xdoc.ToString();
        }

        private void DecryptAuthHeader(XElement rootElement, ICryptographyProvider cryptographyProvider, SecureString key)
        {
            XAttribute authAttribute = rootElement?.Attribute("Auth");
            if (authAttribute == null)
                throw new EncryptionException("Could not find Auth header in the XML repository root element.");
            cryptographyProvider.Decrypt(authAttribute.Value, key);
        }
    }
}