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
            ArgumentNullException.ThrowIfNull(baseDeserializer);

            _baseDeserializer = baseDeserializer;
        }

        public IEnumerable<ICredentialRecord> Deserialize(string xml, SecureString key)
        {
            string decryptedXml = DecryptPasswords(xml, key);
            return _baseDeserializer.Deserialize(decryptedXml);
        }

        private static string DecryptPasswords(string xml, SecureString key)
        {
            if (string.IsNullOrEmpty(xml)) return xml;
            XDocument xdoc = XDocument.Parse(xml);
            XElement root = xdoc.Root
                ?? throw new InvalidOperationException("XML document has no root element.");
            ICryptographyProvider cryptoProvider = new CryptoProviderFactoryFromXml(root).Build();
            DecryptAuthHeader(root, cryptoProvider, key);
            foreach (XElement credentialElement in xdoc.Descendants())
            {
                XAttribute? passwordAttribute = credentialElement.Attribute("Password");
                if (passwordAttribute == null) continue;
                string decryptedPassword = cryptoProvider.Decrypt(passwordAttribute.Value, key);
                passwordAttribute.SetValue(decryptedPassword);
            }

            return xdoc.ToString();
        }

        private static void DecryptAuthHeader(XElement rootElement, ICryptographyProvider cryptographyProvider, SecureString key)
        {
            XAttribute? authAttribute = rootElement.Attribute("Auth");
            if (authAttribute == null)
                throw new EncryptionException("Could not find Auth header in the XML repository root element.");
            cryptographyProvider.Decrypt(authAttribute.Value, key);
        }
    }
}