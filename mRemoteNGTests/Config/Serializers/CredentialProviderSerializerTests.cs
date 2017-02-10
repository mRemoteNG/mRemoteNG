using System;
using mRemoteNG.Config.Serializers.CredentialProviderSerializer;
using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;


namespace mRemoteNGTests.Config.Serializers
{
    public class CredentialProviderSerializerTests
    {
        private CredentialProviderSerializer _credentialProviderSerializer;

        [SetUp]
        public void Setup()
        {
            _credentialProviderSerializer = new CredentialProviderSerializer();
        }

        private ICredentialRepository InitializeMockProvider()
        {
            var provider = Substitute.For<ICredentialRepository>();
            provider.Config.Name.Returns("ProviderName");
            provider.Config.Id.Returns(Guid.NewGuid());
            return provider;
        }

        [Test]
        public void SerializeExists()
        {
            var mockProvider = InitializeMockProvider();
            var providers = new[] { mockProvider };
            var serializedContent = _credentialProviderSerializer.Serialize(providers);
            Assert.That(serializedContent, Is.Not.Null);
        }
    }
}