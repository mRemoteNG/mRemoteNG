using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;


namespace mRemoteNG.Config.Serializers
{
    public class XmlCredentialDeserializer
    {
        private readonly ICryptographyProvider _cryptographyProvider;

        public XmlCredentialDeserializer(ICryptographyProvider cryptographyProvider)
        {
            if (cryptographyProvider == null)
                throw new ArgumentNullException(nameof(cryptographyProvider));
            _cryptographyProvider = cryptographyProvider;
        }

        public IEnumerable<ICredentialRecord> Deserialize(string xml, SecureString decryptionKey)
        {
            var xdoc = XDocument.Parse(xml);
            var credentials = from element in xdoc.Descendants("Credential")
                select new CredentialRecord(Guid.Parse(element.Attribute("Id")?.Value))
                {
                    Name = element.Attribute("Name")?.Value,
                    Username = element.Attribute("Username")?.Value,
                    Password = _cryptographyProvider.Decrypt(element.Attribute("Password")?.Value, decryptionKey).ConvertToSecureString(),
                    Domain = element.Attribute("Domain")?.Value
                };
            return credentials;
        }
    }
}