using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Config.Serializers.CredentialSerializer;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.CredentialSerializers
{
    public class XmlCredentialPasswordEncryptorDecoratorTests
    {
        private XmlCredentialPasswordEncryptorDecorator _sut;
        private ISerializer<IEnumerable<ICredentialRecord>, string> _baseSerializer;
        private ICryptographyProvider _cryptoProvider;

        [SetUp]
        public void Setup()
        {
            _baseSerializer = Substitute.For<ISerializer<IEnumerable<ICredentialRecord>, string>>();
            _baseSerializer.Serialize(null).ReturnsForAnyArgs(GenerateXml());
            _cryptoProvider = Substitute.For<ICryptographyProvider>();
            _cryptoProvider.Encrypt(null, null).ReturnsForAnyArgs("encrypted");
            _sut = new XmlCredentialPasswordEncryptorDecorator(_baseSerializer, _cryptoProvider, new SecureString());
        }


        [Test]
        public void CantPassNullCredentialList()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Serialize(null));
        }

        [Test]
        public void EncryptsPasswordAttributesInXml()
        {
            var credList = Substitute.For<IEnumerable<ICredentialRecord>>();
            var output = _sut.Serialize(credList);
            var outputAsXdoc = XDocument.Parse(output);
            var firstElementPassword = outputAsXdoc.Root?.Descendants().First().FirstAttribute.Value;
            Assert.That(firstElementPassword, Is.EqualTo("encrypted"));
        }

        [Test]
        public void SetsAuthField()
        {
            var credList = Substitute.For<IEnumerable<ICredentialRecord>>();
            var output = _sut.Serialize(credList);
            var outputAsXdoc = XDocument.Parse(output);
            var authField = outputAsXdoc.Root?.Attribute("Auth")?.Value;
            Assert.That(authField, Is.EqualTo("encrypted"));
        }

        private string GenerateXml()
        {
            var randomString = Guid.NewGuid().ToString();
            return 
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<Root Auth=\"\">" +
                    $"<Element1 Password=\"{randomString}\" />" +
                    $"<Element1 Password=\"{randomString}\">" +
                        $"<Element1 Password=\"{randomString}\" />" +
                    "</Element1>" +
                "</Root>";
        }
    }
}