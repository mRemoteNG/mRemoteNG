using System.Linq;
using System.Security;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.IntegrationTests
{
    public class XmlCredentialSerializerLifeCycleTests
    {
        private XmlCredentialRecordSerializer _serializer;
        private XmlCredentialRecordDeserializer _deserializer;

        [SetUp]
        public void Setup()
        {
            var cryptoProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.AES, BlockCipherModes.CCM);
            _serializer = new XmlCredentialRecordSerializer();
            _deserializer = new XmlCredentialRecordDeserializer();
        }

        [Test]
        public void WeCanSerializeAndDeserializeXmlCredentials()
        {
            var credentials = CreateCredentials();
            var serializedCredentials = _serializer.Serialize(credentials);
            var deserializedCredentials = _deserializer.Deserialize(serializedCredentials);
            Assert.That(deserializedCredentials.Count(), Is.EqualTo(2));
        }


        private CredentialRecord[] CreateCredentials()
        {
            return new[]
            {
                new CredentialRecord {Title = "mycred1", Username = "user1", Domain = "dom1", Password = "somepass1".ConvertToSecureString()},
                new CredentialRecord {Title = "mycred2", Username = "user2", Domain = "dom2", Password = "somepass2".ConvertToSecureString()}
            };
        }
    }
}