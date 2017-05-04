using System;
using mRemoteNG.Credential;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Credential
{
    public class CredentialRecordTypeConverterTests
    {
        private CredentialRecordTypeConverter _converter;

        [SetUp]
        public void Setup()
        {
            _converter = new CredentialRecordTypeConverter();
        }

        [TestCase(typeof(Guid), true)]
        public void CanConvertFrom(Type typeToConvertFrom, bool expectedOutcome)
        {
            var actualOutcome = _converter.CanConvertFrom(typeToConvertFrom);
            Assert.That(actualOutcome, Is.EqualTo(expectedOutcome));
        }

        [TestCase(typeof(Guid), true)]
        [TestCase(typeof(ICredentialRecord), true)]
        public void CanConvertTo(Type typeToConvertFrom, bool expectedOutcome)
        {
            var actualOutcome = _converter.CanConvertTo(typeToConvertFrom);
            Assert.That(actualOutcome, Is.EqualTo(expectedOutcome));
        }

        [Test]
        public void ConvertingToGuidReturnsCorrectValue()
        {
            var credRecord = Substitute.For<ICredentialRecord>();
            credRecord.Id.Returns(Guid.NewGuid());
            var convertedValue = _converter.ConvertTo(credRecord, typeof(Guid));
            Assert.That(convertedValue, Is.EqualTo(credRecord.Id));
        }
    }
}