using System;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using NSubstitute;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers
{
    public class CsvConnectionsSerializerMremotengFormatTests
    {
        private const string ConnectionName = "myconnection";
        private const string Username = "myuser";
        private const string Domain = "mydomain";
        private const string Password = "mypass123";

        [TestCase(Username)]
        [TestCase(Domain)]
        [TestCase(Password)]
        [TestCase("InheritColors")]
        public void CreatesCsv(string valueThatShouldExist)
        {
            var serializer = new CsvConnectionsSerializerMremotengFormat(new SaveFilter());
            var connectionInfo = BuildConnectionInfo();
            var csv = serializer.Serialize(connectionInfo);
            Assert.That(csv, Does.Match(valueThatShouldExist));
        }

        [TestCase(Username)]
        [TestCase(Domain)]
        [TestCase(Password)]
        [TestCase("InheritColors")]
        public void SerializerRespectsSaveFilterSettings(string valueThatShouldntExist)
        {
            var saveFilter = new SaveFilter(true);
            var serializer = new CsvConnectionsSerializerMremotengFormat(saveFilter);
            var connectionInfo = BuildConnectionInfo();
            var csv = serializer.Serialize(connectionInfo);
            Assert.That(csv, Does.Not.Match(valueThatShouldntExist));
        }

        [Test]
        public void CanSerializeEmptyConnectionInfo()
        {
            var serializer = new CsvConnectionsSerializerMremotengFormat(new SaveFilter());
            var connectionInfo = new ConnectionInfo();
            var csv = serializer.Serialize(connectionInfo);
            Assert.That(csv, Is.Not.Empty);
        }

        [Test]
        public void CantPassNullToConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new CsvConnectionsSerializerMremotengFormat(null));
        }

        [Test]
        public void CantPassNullToSerializeConnectionInfo()
        {
            var serializer = new CsvConnectionsSerializerMremotengFormat(new SaveFilter());
            Assert.Throws<ArgumentNullException>(() => serializer.Serialize((ConnectionInfo)null));
        }

        [Test]
        public void CantPassNullToSerializeConnectionTreeModel()
        {
            var serializer = new CsvConnectionsSerializerMremotengFormat(new SaveFilter());
            Assert.Throws<ArgumentNullException>(() => serializer.Serialize((ConnectionTreeModel)null));
        }

        private ConnectionInfo BuildConnectionInfo()
        {
            var credRecord = Substitute.For<ICredentialRecord>();
            credRecord.Username.Returns(Username);
            credRecord.Domain.Returns(Domain);
            credRecord.Password.Returns(Password.ConvertToSecureString());
            return new ConnectionInfo
            {
                Name = ConnectionName,
                CredentialRecord = credRecord,
                Inheritance = {Colors = true}
            };
        }
    }
}