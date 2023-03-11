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
            var decryptedXml = DecryptPasswords(xml, key);
            return _baseDeserializer.Deserialize(decryptedXml);
        }

        private string DecryptPasswords(string xml, SecureString key)
        {
            if (string.IsNullOrEmpty(xml)) return xml;
            var xdoc = XDocument.Parse(xml);
            var cryptoProvider = new CryptoProviderFactoryFromXml(xdoc.Root).Build();
            DecryptAuthHeader(xdoc.Root, cryptoProvider, key);
            foreach (var credentialElement in xdoc.Descendants())
            {
                var passwordAttribute = credentialElement.Attribute("Password");
                if (passwordAttribute == null) continue;
                var decryptedPassword = cryptoProvider.Decrypt(passwordAttribute.Value, key);
                passwordAttribute.SetValue(decryptedPassword);
            }

            return xdoc.ToString();
        }

        private void DecryptAuthHeader(XElement rootElement, ICryptographyProvider cryptographyProvider, SecureString key)
        {
            var authAttribute = rootElement.Attribute("Auth");
            if (authAttribute == null)
                throw new EncryptionException("Could not find Auth header in the XML repository root element.");
            cryptographyProvider.Decrypt(authAttribute.Value, key);
        }
    }
}