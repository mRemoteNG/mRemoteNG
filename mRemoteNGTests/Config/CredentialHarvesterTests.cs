using System;
using System.Linq;
using System.Security;
using System.Xml.Linq;
using mRemoteNG.Config;
using mRemoteNG.Config.Serializers;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Security;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Config
{
#pragma warning disable 618
    public class CredentialHarvesterTests
    {

        private CredentialHarvester _credentialHarvester;
        private ICryptographyProvider _cryptographyProvider;
        private SecureString _key;

        [SetUp]
        public void Setup()
        {
            _credentialHarvester = new CredentialHarvester();
            _cryptographyProvider = new CryptographyProviderFactory().CreateAeadCryptographyProvider(BlockCipherEngines.AES, BlockCipherModes.GCM);
            _key = "testKey123".ConvertToSecureString();
        }

        [Test]
        public void HarvestsUsername()
        {
            var connection = new ConnectionInfo { Username = "myuser", Domain = "somedomain", Password = "mypass" };
            var xdoc = CreateTestData(connection);
            var credentials = _credentialHarvester.Harvest(xdoc, _key);
            Assert.That(credentials.Single().Username, Is.EqualTo(connection.Username));
        }

        [Test]
        public void HarvestsDomain()
        {
            var connection = new ConnectionInfo { Username = "myuser", Domain = "somedomain", Password = "mypass" };
            var xdoc = CreateTestData(connection);
            var credentials = _credentialHarvester.Harvest(xdoc, _key);
            Assert.That(credentials.Single().Domain, Is.EqualTo(connection.Domain));
        }

        [Test]
        public void HarvestsPassword()
        {
            var connection = new ConnectionInfo { Username = "myuser", Domain = "somedomain", Password = "mypass" };
            var xdoc = CreateTestData(connection);
            var credentials = _credentialHarvester.Harvest(xdoc, _key);
            Assert.That(credentials.Single().Password.ConvertToUnsecureString(), Is.EqualTo(connection.Password));
        }

        [Test]
        public void DoesNotHarvestEmptyCredentials()
        {
            var connection = new ConnectionInfo();
            var xdoc = CreateTestData(connection);
            var credentials = _credentialHarvester.Harvest(xdoc, _key);
            Assert.That(credentials.Count(), Is.EqualTo(0));
        }

        [Test]
        public void HarvestsAllCredentials()
        {
            var container = new ContainerInfo();
            var con1 = new ConnectionInfo {Username = "blah"};
            var con2 = new ConnectionInfo {Username = "something"};
            container.AddChildRange(new [] {con1, con2});
            var xdoc = CreateTestData(container);
            var credentials = _credentialHarvester.Harvest(xdoc, _key);
            Assert.That(credentials.Count(), Is.EqualTo(2));
        }

        [Test]
        public void OnlyReturnsUniqueCredentials()
        {
            var container = new ContainerInfo();
            var con1 = new ConnectionInfo { Username = "something" };
            var con2 = new ConnectionInfo { Username = "something" };
            container.AddChildRange(new[] { con1, con2 });
            var xdoc = CreateTestData(container);
            var credentials = _credentialHarvester.Harvest(xdoc, _key);
            Assert.That(credentials.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CredentialMapCorrectForSingleCredential()
        {
            var connection = new ConnectionInfo { Username = "myuser", Domain = "somedomain", Password = "mypass" };
            var connectionGuid = Guid.Parse(connection.ConstantID);
            var xdoc = CreateTestData(connection);
            _credentialHarvester.Harvest(xdoc, _key);
            var map = _credentialHarvester.ConnectionToCredentialMap;
            Assert.That(map[connectionGuid].Username, Is.EqualTo(connection.Username));
        }

        [Test]
        public void CredentialMapDoesntContainDuplicateCredentialObjects()
        {
            var container = new ContainerInfo();
            var con1 = new ConnectionInfo { Username = "something" };
            var con2 = new ConnectionInfo { Username = "something" };
            container.AddChildRange(new[] { con1, con2 });
            var xdoc = CreateTestData(container);
            var con1Id = Guid.Parse(con1.ConstantID);
            var con2Id = Guid.Parse(con2.ConstantID);
            _credentialHarvester.Harvest(xdoc, _key);
            var map = _credentialHarvester.ConnectionToCredentialMap;
            Assert.That(map[con1Id], Is.EqualTo(map[con2Id]));
        }


        private XDocument CreateTestData(ConnectionInfo connectionInfo)
        {
            var rootNode = new RootNodeInfo(RootNodeType.Connection) {PasswordString = _key.ConvertToUnsecureString()};
            rootNode.AddChild(connectionInfo);
            var nodeSerializer = new XmlConnectionNodeSerializer26(_cryptographyProvider, _key, new SaveFilter());
            var serializer = new XmlConnectionsSerializer(_cryptographyProvider, nodeSerializer);
            var serializedData = serializer.Serialize(rootNode);
            return XDocument.Parse(serializedData);
        }
    }
#pragma warning restore 618
}