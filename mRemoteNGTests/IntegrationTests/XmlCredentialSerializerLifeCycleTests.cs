using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.IntegrationTests
{
    public class XmlCredentialSerializerLifeCycleTests
    {
        private ISecureSerializer<IEnumerable<ICredentialRecord>, string> _serializer;
        private ISecureDeserializer<string, IEnumerable<ICredentialRecord>> _deserializer;
        private readonly Guid _id = Guid.NewGuid();
        private const string Title = "mycredential1";
        private const string Username = "user1";
        private const string Domain = "domain1";
        private readonly SecureString _key = "myPassword1!".ConvertToSecureString();

        [SetUp]
        public void Setup()
        {
            var keyProvider = Substitute.For<IKeyProvider>();
            keyProvider.GetKey().Returns(_key);
            var cryptoProvider = new CryptoProviderFactory(BlockCipherEngines.AES, BlockCipherModes.CCM).Build();
            _serializer = new XmlCredentialPasswordEncryptorDecorator(cryptoProvider, new XmlCredentialRecordSerializer());
            _deserializer = new XmlCredentialPasswordDecryptorDecorator(new XmlCredentialRecordDeserializer());
        }

        [Test]
        public void WeCanSerializeAndDeserializeXmlCredentials()
        {
            var credentials = new[] { new CredentialRecord(), new CredentialRecord() };
            var serializedCredentials = _serializer.Serialize(credentials, _key);
            var deserializedCredentials = _deserializer.Deserialize(serializedCredentials, _key);
            Assert.That(deserializedCredentials.Count(), Is.EqualTo(2));
        }

        [Test]
        public void IdConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Id, Is.EqualTo(_id));
        }

        [Test]
        public void TitleConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Title, Is.EqualTo(Title));
        }

        [Test]
        public void UsernameConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Username, Is.EqualTo(Username));
        }

        [Test]
        public void DomainConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Domain, Is.EqualTo(Domain));
        }

        [Test]
        public void PasswordConsistentAfterSerialization()
        {
            var sut = SerializeThenDeserializeCredential();
            Assert.That(sut.Password.ConvertToUnsecureString(), Is.EqualTo(_key.ConvertToUnsecureString()));
        }

        private ICredentialRecord SerializeThenDeserializeCredential()
        {
            var credentials = new[]
            {
                new CredentialRecord(_id) {Title = Title, Username = Username, Domain = Domain, Password = _key}
            };
            var serializedCredentials = _serializer.Serialize(credentials, _key);
            var deserializedCredentials = _deserializer.Deserialize(serializedCredentials, _key);
            return deserializedCredentials.First();
        }
    }
}