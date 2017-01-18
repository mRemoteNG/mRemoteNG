using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;


namespace mRemoteNG.Config.Serializers
{
    public class XmlCredentialRecordSerializer
    {
        private readonly ICryptographyProvider _cryptographyProvider;

        public XmlCredentialRecordSerializer(ICryptographyProvider cryptographyProvider)
        {
            if (cryptographyProvider == null)
                throw new ArgumentNullException(nameof(cryptographyProvider));
            _cryptographyProvider = cryptographyProvider;
        }

        public string Serialize(IEnumerable<ICredentialRecord> credentialRecords, SecureString encryptionKey)
        {
            var xdoc = new XDocument(
                new XElement("Credentials",
                    from r in credentialRecords
                    select new XElement("Credential",
                        new XAttribute("Id", r.Id),
                        new XAttribute("Name", r.Name),
                        new XAttribute("Username", r.Username),
                        new XAttribute("Password", _cryptographyProvider.Encrypt(r.Password.ConvertToUnsecureString(), encryptionKey))
                    )
                )
            )
            {
                Declaration = new XDeclaration("1.0", "utf-8", null)
            };
            return xdoc.Declaration + Environment.NewLine + xdoc;
        }
    }
}