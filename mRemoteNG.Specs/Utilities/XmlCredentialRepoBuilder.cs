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

        public ICredentialRepository BuildXmlCredentialRepo()
        {
            var xmlFileBuilder = new CredRepoXmlFileBuilder();
            var xmlFileContent = xmlFileBuilder.Build(CryptographyProvider.Encrypt("someheaderdata", EncryptionKey));
            var dataProvider = new InMemoryStringDataProvider(xmlFileContent);
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
                    encryptor
                ), new CredentialRecordLoader(
                    dataProvider,
                    decryptor
                )
            );
        }
    }
}