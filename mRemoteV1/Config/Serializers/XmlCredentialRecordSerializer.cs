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

        public string SchemaVersion { get; } = "1.0";

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
                    new XAttribute("EncryptionEngine", _cryptographyProvider.CipherEngine),
                    new XAttribute("BlockCipherMode", _cryptographyProvider.CipherMode),
                    new XAttribute("KdfIterations", _cryptographyProvider.KeyDerivationIterations),
                    new XAttribute("SchemaVersion", SchemaVersion),
                    from r in credentialRecords
                    select new XElement("Credential",
                        new XAttribute("Id", r.Id),
                        new XAttribute("Title", r.Title),
                        new XAttribute("Username", r.Username),
                        new XAttribute("Domain", r.Domain),
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