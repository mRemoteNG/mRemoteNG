using System.Collections.Generic;
using System.Security;
using mRemoteNG.Config;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.Specs.Utilities
{
    public class XmlCredentialRepoBuilder
    {
        public SecureString EncryptionKey { get; set; } = "someKey1".ConvertToSecureString();
        public ICryptographyProvider CryptographyProvider { get; set; } = new AeadCryptographyProvider();
        public string XmlFileContent { get; set; } = "";

        public ICredentialRepository BuildXmlCredentialRepo()
        {
            var dataProvider = new InMemoryStringDataProvider(XmlFileContent);
            var encryptor = new XmlCredentialPasswordEncryptorDecorator(
                CryptographyProvider,
                new XmlCredentialRecordSerializer()
            );
            var decryptor = new XmlCredentialPasswordDecryptorDecorator(
                new XmlCredentialRecordDeserializer()
            );

            return new XmlCredentialRepository(
                new CredentialRepositoryConfig(),
                new CredentialRecordSaver(
                    dataProvider,
                    new StaticSerializerKeyProviderDecorator<IEnumerable<ICredentialRecord>, string>(encryptor, encryptor) { Key = EncryptionKey }
                ), new CredentialRecordLoader(
                    dataProvider,
                    new StaticDeserializerKeyProviderDecorator<string, IEnumerable<ICredentialRecord>>(decryptor, decryptor) { Key = EncryptionKey }
                )
            );
        }
    }
}