using System.Security;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Credential
{
    public class StaticSerializerKeyProviderDecoratorTests
    {
        private StaticSerializerKeyProviderDecorator<object, string> _serializerKeyProvider;
        private ISerializer<object, string> _baseSerializer;
        private IHasKey _objThatNeedsKey;
        private readonly SecureString _key = "someKey1".ConvertToSecureString();

        [SetUp]
        public void Setup()
        {
            _objThatNeedsKey = Substitute.For<IHasKey>();
            _baseSerializer = Substitute.For<ISerializer<object, string>>();
            _serializerKeyProvider = new StaticSerializerKeyProviderDecorator<object, string>(_objThatNeedsKey, _baseSerializer) { Key = _key };
        }

        [Test]
        public void CallsBaseSerializer()
        {
            var creds = new[] { Substitute.For<ICredentialRecord>() };
            _serializerKeyProvider.Serialize(creds);
            _baseSerializer.Received().Serialize(creds);
        }

        [Test]
        public void PassesOnItsKey()
        {
            var creds = new[] { Substitute.For<ICredentialRecord>() };
            _serializerKeyProvider.Serialize(creds);
            Assert.That(_objThatNeedsKey.Key, Is.EqualTo(_serializerKeyProvider.Key));
        }
    }
}