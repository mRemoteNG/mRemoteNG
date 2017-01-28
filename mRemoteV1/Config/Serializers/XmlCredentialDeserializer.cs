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
        public string SchemaVersion { get; } = "1.0";

        public IEnumerable<ICredentialRecord> Deserialize(string xml, SecureString decryptionKey)
        {
            try
            {
                var xdoc = XDocument.Parse(xml);
                ValidateSchemaVersion(xdoc);
                var cryptographyProvider = BuildCryptoProvider(xdoc.Root);
                var credentials = from element in xdoc.Descendants("Credential")
                    select new CredentialRecord(Guid.Parse(element.Attribute("Id")?.Value))
                    {
                        Title = element.Attribute("Title")?.Value ?? "",
                        Username = element.Attribute("Username")?.Value ?? "",
                        Password =
                            cryptographyProvider.Decrypt(element.Attribute("Password")?.Value, decryptionKey)
                                .ConvertToSecureString(),
                        Domain = element.Attribute("Domain")?.Value ?? ""
                    };
                return credentials;
            }
            catch
            {
                return new ICredentialRecord[0];
            }
        }

        private void ValidateSchemaVersion(XDocument xdoc)
        {
            var docSchemaVersion = xdoc.Root?.Attribute("SchemaVersion")?.Value;
            if (docSchemaVersion != SchemaVersion)
                throw new Exception($"The schema version of this document is not supported by this class. Document Version: {docSchemaVersion} Supported Version: {SchemaVersion}");
        }

        private ICryptographyProvider BuildCryptoProvider(XElement rootElement)
        {
            if (rootElement == null)
                throw new ArgumentNullException(nameof(rootElement));

            BlockCipherEngines engine;
            Enum.TryParse(rootElement.Attribute("EncryptionEngine")?.Value, true, out engine);

            BlockCipherModes mode;
            Enum.TryParse(rootElement.Attribute("BlockCipherMode")?.Value, true, out mode);

            int kdfIterations;
            int.TryParse(rootElement.Attribute("KdfIterations")?.Value, out kdfIterations);

            var cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(engine, mode);
            cryptoProvider.KeyDerivationIterations = kdfIterations;
            
            return cryptoProvider;
        }
    }
}