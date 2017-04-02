using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.CredentialSerializers
{
    public class XmlCredentialSerializerTests
    {
        private XmlCredentialRecordSerializer _serializer;
        private ICredentialRecord _cred1;

        [SetUp]
        public void Setup()
        {
            _serializer = new XmlCredentialRecordSerializer();
            _cred1 = new CredentialRecord { Title = "testcred", Username = "davids", Domain = "mydomain", Password = "mypass".ConvertToSecureString() };
        }

        [Test]
        public void ProducesValidXml()
        {
            var serialized = _serializer.Serialize(new[] { _cred1 });
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.DoesNotThrow(() => XDocument.Parse(serialized));
        }

        [Test]
        public void AllCredentialsSerialized()
        {
            var cred2 = new CredentialRecord { Title = "testcred2", Username = "admin", Domain = "otherdomain", Password = "somepass".ConvertToSecureString() };
            var serialized = _serializer.Serialize(new[] { _cred1, cred2 });
            var serializedCount = XDocument.Parse(serialized).Descendants("Credential").Count();
            Assert.That(serializedCount, Is.EqualTo(2));
        }

        [Test]
        public void IncludesSchemaVersionParameter()
        {
            var serialized = _serializer.Serialize(new[] { _cred1 });
            var xdoc = XDocument.Parse(serialized);
            Assert.That(xdoc.Root?.Attribute("SchemaVersion")?.Value, Is.EqualTo(_serializer.SchemaVersion));
        }
    }
}