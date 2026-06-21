using System.Linq;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.IntegrationTests
{
    /// <summary>
    /// Security-focused serialization tests for the SSH_DotNet private-key properties. The reflection
    /// round-trip in <see cref="XmlSerializationLifeCycleTests"/> proves values survive a round trip,
    /// but it does NOT prove the passphrase is encrypted at rest (plaintext would round-trip too) —
    /// these tests are the guard against a plaintext-leak bug.
    /// </summary>
    [TestFixture]
    [Category("Unit")]
    public class SshDotNetCertSerializationTests
    {
        private const string Passphrase = "s3cr3t-passphrase-XYZ-123";
        private readonly ICryptoProviderFactory _cryptoFactory = new CryptoProviderFactory(BlockCipherEngines.AES, BlockCipherModes.GCM);

        private string SerializeSingleConnection(ConnectionInfo con, SaveFilter saveFilter)
        {
            var cryptoProvider = _cryptoFactory.Build();
            var model = new ConnectionTreeModel();
            var rootNode = new RootNodeInfo(RootNodeType.Connection);
            rootNode.AddChild(con);
            model.AddRootNode(rootNode);

            var nodeSerializer = new XmlConnectionNodeSerializer28(
                cryptoProvider,
                rootNode.PasswordString.ConvertToSecureString(),
                saveFilter);
            var serializer = new XmlConnectionsSerializer(cryptoProvider, nodeSerializer);
            return serializer.Serialize(model);
        }

        [Test]
        public void Passphrase_IsEncryptedAtRest_AndRoundTrips()
        {
            var con = new ConnectionInfo { Name = "con", SshDotNetPrivateKeyPassphrase = Passphrase };

            var xml = SerializeSingleConnection(con, new SaveFilter { SavePassword = true });

            // Ciphertext at rest: the plaintext passphrase must NOT appear in the serialized XML.
            Assert.That(xml, Does.Not.Contain(Passphrase), "Passphrase must be encrypted at rest, not stored as plaintext");
            Assert.That(xml, Does.Contain("SshDotNetPrivateKeyPassphrase="), "Passphrase attribute should be present");

            // ...and it decrypts back to the original value.
            var deserialized = new XmlConnectionsDeserializer().Deserialize(xml);
            var roundTripped = deserialized.GetRecursiveChildList().First(c => c.Name == "con");
            Assert.That(roundTripped.SshDotNetPrivateKeyPassphrase, Is.EqualTo(Passphrase));
        }

        [Test]
        public void Passphrase_IsBlank_WhenSaveFilterDisablesPasswords()
        {
            var con = new ConnectionInfo { Name = "con", SshDotNetPrivateKeyPassphrase = Passphrase };

            var xml = SerializeSingleConnection(con, new SaveFilter { SavePassword = false });

            Assert.That(xml, Does.Not.Contain(Passphrase));
            Assert.That(xml, Does.Contain("SshDotNetPrivateKeyPassphrase=\"\""),
                "Passphrase must be blank when the save filter disallows saving passwords");
        }

        [Test]
        public void KeyFile_IsStoredPlaintext_AndRoundTrips()
        {
            const string keyPath = @"C:\keys\id_ed25519";
            var con = new ConnectionInfo { Name = "con", SshDotNetPrivateKeyFile = keyPath };

            var xml = SerializeSingleConnection(con, new SaveFilter { SavePassword = true });

            // The key file path is not a secret -> stored as plaintext.
            Assert.That(xml, Does.Contain(keyPath));

            var deserialized = new XmlConnectionsDeserializer().Deserialize(xml);
            var roundTripped = deserialized.GetRecursiveChildList().First(c => c.Name == "con");
            Assert.That(roundTripped.SshDotNetPrivateKeyFile, Is.EqualTo(keyPath));
        }
    }
}
