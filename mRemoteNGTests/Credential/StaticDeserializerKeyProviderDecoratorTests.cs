using System.Security;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Credential
{
    public class StaticDeserializerKeyProviderDecoratorTests
    {
        private StaticDeserializerKeyProviderDecorator<object, string> _deserializerKeyProvider;
        private IDeserializer<object, string> _baseDeserializer;
        private IHasKey _objThatNeedsKey;
        private readonly SecureString _key = "someKey1".ConvertToSecureString();

        [SetUp]
        public void Setup()
        {
            _objThatNeedsKey = Substitute.For<IHasKey>();
            _baseDeserializer = Substitute.For<IDeserializer<object, string>>();
            _deserializerKeyProvider = new StaticDeserializerKeyProviderDecorator<object, string>(_objThatNeedsKey, _baseDeserializer) { Key = _key };
        }

        [Test]
        public void CallsBaseSerializer()
        {
            var creds = new[] { Substitute.For<ICredentialRecord>() };
            _deserializerKeyProvider.Deserialize(creds);
            _baseDeserializer.Received().Deserialize(creds);
        }

        [Test]
        public void PassesOnItsKey()
        {
            var creds = new[] { Substitute.For<ICredentialRecord>() };
            _deserializerKeyProvider.Deserialize(creds);
            Assert.That(_objThatNeedsKey.Key, Is.EqualTo(_deserializerKeyProvider.Key));
        }
    }
}
