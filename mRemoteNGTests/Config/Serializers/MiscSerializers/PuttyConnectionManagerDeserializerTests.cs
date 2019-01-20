using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNGTests.Properties;
using NUnit.Framework;
using System.Linq;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    public class PuttyConnectionManagerDeserializerTests
    {
        private PuttyConnectionManagerDeserializer _deserializer;
        private ContainerInfo _rootImportedFolder;
        private SerializationResult _deserializationResult;
        private const string ExpectedRootFolderName = "test_puttyConnectionManager_database";
        private const string ExpectedConnectionDisplayName = "my ssh connection";
        private const string ExpectedConnectionHostname = "server1.mydomain.com";
        private const string ExpectedConnectionDescription = "My Description Here";
        private const int ExpectedConnectionPort = 22;
        private const ProtocolType ExpectedProtocolType = ProtocolType.SSH2;
        private const string ExpectedPuttySession = "MyCustomPuttySession";
        private const string ExpectedConnectionUsername = "mysshusername";
        private const string ExpectedConnectionPassword = "password123";


        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            var fileContents = Resources.test_puttyConnectionManager_database;
            _deserializer = new PuttyConnectionManagerDeserializer();
            _deserializationResult = _deserializer.Deserialize(fileContents);
            _rootImportedFolder = _deserializationResult.ConnectionRecords.Cast<ContainerInfo>().First();
        }

        [Test]
        public void RootFolderImportedWithCorrectName()
        {
            Assert.That(_rootImportedFolder.Name, Is.EqualTo(ExpectedRootFolderName));
        }

        [Test]
        public void ConnectionDisplayNameImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Name, Is.EqualTo(ExpectedConnectionDisplayName));
        }

        [Test]
        public void ConnectionHostNameImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Hostname, Is.EqualTo(ExpectedConnectionHostname));
        }

        [Test]
        public void ConnectionDescriptionImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Description, Is.EqualTo(ExpectedConnectionDescription));
        }

        [Test]
        public void ConnectionPortImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Port, Is.EqualTo(ExpectedConnectionPort));
        }

        [Test]
        public void ConnectionProtocolTypeImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.Protocol, Is.EqualTo(ExpectedProtocolType));
        }

        [Test]
        public void ConnectionPuttySessionImported()
        {
            var connection = GetSshConnection();
            Assert.That(connection.PuttySession, Is.EqualTo(ExpectedPuttySession));
        }

        [Test]
        public void CredentialIdProperlySet()
        {
            var connection = GetSshConnection();
            var cred = _deserializationResult.ConnectionToCredentialMap.DistinctCredentialRecords.First();
            Assert.That(connection.CredentialRecordId.FirstOrDefault(), Is.EqualTo(cred.Id));
        }

        [Test]
        public void ConnectionUsernameImported()
        {
            var credentialMap = _deserializationResult.ConnectionToCredentialMap;
            Assert.That(credentialMap.DistinctCredentialRecords.First().Username, Is.EqualTo(ExpectedConnectionUsername));
        }

        [Test]
        public void ConnectionPasswordImported()
        {
            var credentialMap = _deserializationResult.ConnectionToCredentialMap;
            Assert.That(credentialMap.DistinctCredentialRecords.First().Password.ConvertToUnsecureString(), Is.EqualTo(ExpectedConnectionPassword));
        }

        private ConnectionInfo GetSshConnection()
        {
            var sshFolder = _rootImportedFolder.Children.OfType<ContainerInfo>().First(node => node.Name == "SSHFolder");
            return sshFolder.Children.First();
        }
    }
}