using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Security.Authentication;


namespace mRemoteNG.Config.Serializers
{
    public class XmlCredentialDeserializer
    {
        public string SchemaVersion { get; } = "1.0";
        public IAuthenticator Authenticator { get; set; }

        public IEnumerable<ICredentialRecord> Deserialize(string xml, SecureString decryptionKey)
        {
            var xdoc = XDocument.Parse(xml);
            var rootElement = xdoc.Root;
            ValidateSchemaVersion(rootElement);
            var cryptographyProvider = BuildCryptoProvider(rootElement);
            Authenticate(rootElement, cryptographyProvider, decryptionKey);

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
            return credentials.ToArray();
        }

        private void ValidateSchemaVersion(XElement rootElement)
        {
            var docSchemaVersion = rootElement?.Attribute("SchemaVersion")?.Value;
            if (docSchemaVersion != SchemaVersion)
                throw new Exception($"The schema version of this document is not supported by this class. Document Version: {docSchemaVersion} Supported Version: {SchemaVersion}");
        }

        private void Authenticate(XElement rootElement, ICryptographyProvider cryptographyProvider, SecureString key)
        {
            var authString = rootElement.Attribute("Auth")?.Value;
            cryptographyProvider.Decrypt(authString, key);
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