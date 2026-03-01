using System;
using System.IO;
using System.Linq;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Connection;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    public class RemoteDesktopConnectionManagerNewVersionTests
    {
        private RemoteDesktopConnectionManagerDeserializer _deserializer;
        private string _connectionFileContents292;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _deserializer = new RemoteDesktopConnectionManagerDeserializer();
            _connectionFileContents292 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<RDCMan programVersion=""2.92"" schemaVersion=""3"">
  <file>
    <properties>
      <name>test_RDCMan_connections</name>
    </properties>
    <group>
      <properties>
        <name>Group1</name>
      </properties>
      <server>
        <properties>
          <name>server1</name>
        </properties>
      </server>
    </group>
  </file>
</RDCMan>";
        }

        [Test]
        public void CanDeserializeVersion292()
        {
            var connectionTreeModel = _deserializer.Deserialize(_connectionFileContents292);
            Assert.That(connectionTreeModel.RootNodes.Count, Is.GreaterThan(0));
            var rootNode = connectionTreeModel.RootNodes.First();
            Assert.That(rootNode.Children.Count, Is.GreaterThan(0));
        }

        [Test]
        public void CanDeserializeVersion290()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<RDCMan programVersion=""2.90"" schemaVersion=""3"">
  <file>
    <properties>
      <name>test_RDCMan_connections</name>
    </properties>
    <group>
      <properties>
        <name>Group1</name>
      </properties>
      <server>
        <properties>
          <name>server1</name>
        </properties>
      </server>
    </group>
  </file>
</RDCMan>";
            var connectionTreeModel = _deserializer.Deserialize(xml);
            var rootNode = connectionTreeModel.RootNodes.First();
            Assert.That(rootNode.Children.Count, Is.GreaterThan(0));
        }

        [Test]
        public void CanDeserializeVersion293()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<RDCMan programVersion=""2.93"" schemaVersion=""3"">
  <file>
    <properties>
      <name>test_RDCMan_connections</name>
    </properties>
    <group>
      <properties>
        <name>Group1</name>
      </properties>
      <server>
        <properties>
          <name>server1</name>
        </properties>
      </server>
    </group>
  </file>
</RDCMan>";
            var connectionTreeModel = _deserializer.Deserialize(xml);
            var rootNode = connectionTreeModel.RootNodes.First();
            Assert.That(rootNode.Children.Count, Is.GreaterThan(0));
        }

        [Test]
        public void CanDeserializeFutureSchemaVersion()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<RDCMan programVersion=""3.0"" schemaVersion=""5"">
  <file>
    <properties>
      <name>test_RDCMan_connections</name>
    </properties>
    <group>
      <properties>
        <name>Group1</name>
      </properties>
      <server>
        <properties>
          <name>server1</name>
        </properties>
      </server>
    </group>
  </file>
</RDCMan>";
            var connectionTreeModel = _deserializer.Deserialize(xml);
            var rootNode = connectionTreeModel.RootNodes.First();
            Assert.That(rootNode.Children.Count, Is.GreaterThan(0));
        }

        [Test]
        public void ExceptionThrownOnUnsupportedVersionTooLow()
        {
             string badVersion = @"<?xml version=""1.0"" encoding=""utf-8""?>
<RDCMan programVersion=""2.0"" schemaVersion=""3"">
  <file>
  </file>
</RDCMan>";
            Assert.That(() => _deserializer.Deserialize(badVersion), Throws.TypeOf<FileFormatException>());
        }

        [Test]
        public void ExceptionThrownOnSchemaVersion2()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<RDCMan programVersion=""2.7"" schemaVersion=""2"">
  <file>
  </file>
</RDCMan>";
            Assert.That(() => _deserializer.Deserialize(xml), Throws.TypeOf<FileFormatException>());
        }
    }
}
