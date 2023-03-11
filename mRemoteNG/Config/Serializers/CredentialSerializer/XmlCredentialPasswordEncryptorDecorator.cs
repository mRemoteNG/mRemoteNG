using System;
using System.Collections.Generic;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Credential;
using mRemoteNG.Security;

namespace mRemoteNG.Config.Serializers.CredentialSerializer
{
    public class XmlCredentialPasswordEncryptorDecorator : ISecureSerializer<IEnumerable<ICredentialRecord>, string>
    {
        private readonly ISerializer<IEnumerable<ICredentialRecord>, string> _baseSerializer;
        private readonly ICryptographyProvider _cryptographyProvider;

        public XmlCredentialPasswordEncryptorDecorator(ICryptographyProvider cryptographyProvider, ISerializer<IEnumerable<ICredentialRecord>, string> baseSerializer)
        {
            if (baseSerializer == null)
                throw new ArgumentNullException(nameof(baseSerializer));
            if (cryptographyProvider == null)
                throw new ArgumentNullException(nameof(cryptographyProvider));

            _baseSerializer = baseSerializer;
            _cryptographyProvider = cryptographyProvider;
        }


        public string Serialize(IEnumerable<ICredentialRecord> credentialRecords, SecureString key)
        {
            if (credentialRecords == null)
                throw new ArgumentNullException(nameof(credentialRecords));

            var baseReturn = _baseSerializer.Serialize(credentialRecords);
            var encryptedReturn = EncryptPasswordAttributes(baseReturn, key);
            return encryptedReturn;
        }

        private string EncryptPasswordAttributes(string xml, SecureString encryptionKey)
        {
            var xdoc = XDocument.Parse(xml);
            SetEncryptionAttributes(xdoc, encryptionKey);
            foreach (var element in xdoc.Descendants())
            {
                var passwordAttribute = element.Attribute("Password");
                if (passwordAttribute == null) continue;
                var encryptedPassword = _cryptographyProvider.Encrypt(passwordAttribute.Value, encryptionKey);
                passwordAttribute.Value = encryptedPassword;
            }

            return xdoc.Declaration + Environment.NewLine + xdoc;
        }

        private void SetEncryptionAttributes(XDocument xdoc, SecureString encryptionKey)
        {
            xdoc.Root?.SetAttributeValue("EncryptionEngine", _cryptographyProvider.CipherEngine);
            xdoc.Root?.SetAttributeValue("BlockCipherMode", _cryptographyProvider.CipherMode);
            xdoc.Root?.SetAttributeValue("KdfIterations", _cryptographyProvider.KeyDerivationIterations);
            xdoc.Root?.SetAttributeValue("Auth", _cryptographyProvider.Encrypt(RandomGenerator.RandomString(20), encryptionKey));
        }
    }
}