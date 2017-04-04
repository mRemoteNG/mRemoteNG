using System;
using System.Collections.Generic;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;

namespace mRemoteNG.Config.Serializers.CredentialSerializer
{
    public class XmlCredentialPasswordDecryptorDecorator : IDeserializer<string, IEnumerable<ICredentialRecord>>, IHasKey
    {
        private readonly IDeserializer<string, IEnumerable<ICredentialRecord>> _baseDeserializer;
        private SecureString _decryptionKey = new SecureString();

        public SecureString Key
        {
            get { return _decryptionKey; }
            set
            {
                if (value == null) return;
                _decryptionKey = value;
            }
        }

        public XmlCredentialPasswordDecryptorDecorator(IDeserializer<string, IEnumerable<ICredentialRecord>> baseDeserializer)
        {
            if (baseDeserializer == null)
                throw new ArgumentNullException(nameof(baseDeserializer));

            _baseDeserializer = baseDeserializer;
        }

        public IEnumerable<ICredentialRecord> Deserialize(string xml)
        {
            var decryptedXml = DecryptPasswords(xml);
            return _baseDeserializer.Deserialize(decryptedXml);
        }

        private string DecryptPasswords(string xml)
        {
            var xdoc = XDocument.Parse(xml);
            var cryptoProvider = new CryptoProviderFactoryFromXml(xdoc.Root).Build();
            foreach (var credentialElement in xdoc.Descendants())
            {
                var passwordAttribute = credentialElement.Attribute("Password");
                if (passwordAttribute == null) continue;
                var decryptedPassword = cryptoProvider.Decrypt(passwordAttribute.Value, _decryptionKey);
                passwordAttribute.SetValue(decryptedPassword);
            }
            return xdoc.ToString();
        }
    }
}