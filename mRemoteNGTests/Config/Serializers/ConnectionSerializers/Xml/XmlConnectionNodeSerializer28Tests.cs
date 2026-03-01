using System;
using System.Linq;
using System.Xml.Linq;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml
{
    [TestFixture]
    public class XmlConnectionNodeSerializer28Tests
    {
        private ICryptographyProvider _cryptographyProvider;
        private XmlConnectionNodeSerializer28 _serializer;

        [SetUp]
        public void Setup()
        {
            _cryptographyProvider = new AeadCryptographyProvider();
            var connectionTreeModel = new ConnectionTreeModel();
            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            connectionTreeModel.AddRootNode(rootNode);

            _serializer = new XmlConnectionNodeSerializer28(
                _cryptographyProvider,
                rootNode.PasswordString.ConvertToSecureString(),
                new SaveFilter());
        }

        [Test]
        public void Serialize_NullStringProperties_ShouldNotThrow()
        {
            // Arrange
            var connectionInfo = new ConnectionInfo
            {
                Name = "TestConnection",
                PuttySession = null, // This triggers the bug
                Description = null,
                Icon = null,
                Panel = null,
                Color = null,
                TabColor = null,
                Hostname = null,
                AlternativeAddress = null,
                UserViaAPI = null,
                Username = null,
                Password = null,
                VaultOpenbaoMount = null,
                VaultOpenbaoRole = null,
                Domain = null,
                EC2InstanceId = null,
                EC2Region = null,
                VmId = null,
                SSHTunnelConnectionName = null,
                OpeningCommand = null,
                ExtApp = null,
                SSHOptions = null,
                PrivateKeyPath = null,
                LoadBalanceInfo = null,
                RDGatewayHostname = null,
                RDGatewayUsername = null,
                RDGatewayPassword = null,
                RDGatewayAccessToken = null,
                RDGatewayDomain = null,
                RDGatewayUserViaAPI = null,
                RedirectDiskDrivesCustom = null,
                PreExtApp = null,
                PostExtApp = null,
                MacAddress = null,
                UserField = null,
                EnvironmentTags = null,
                RDPStartProgram = null,
                RDPStartProgramWorkDir = null,
                VNCProxyIP = null,
                VNCProxyUsername = null,
                VNCProxyPassword = null,
                UserField1 = null,
                UserField2 = null,
                UserField3 = null,
                UserField4 = null,
                UserField5 = null,
                UserField6 = null,
                UserField7 = null,
                UserField8 = null,
                UserField9 = null,
                UserField10 = null
            };

            // Act & Assert
            // This is expected to NOT throw ArgumentNullException after the fix
            Assert.DoesNotThrow(() => _serializer.Serialize(connectionInfo));
        }
    }
}
