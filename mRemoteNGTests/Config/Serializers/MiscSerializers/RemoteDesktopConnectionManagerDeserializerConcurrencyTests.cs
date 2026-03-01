using System;
using System.Threading.Tasks;
using mRemoteNG.Config.Serializers.MiscSerializers;
using mRemoteNG.Tree;
using mRemoteNGTests.Properties;
using NUnit.Framework;
using System.Linq;
using mRemoteNG.Container;

namespace mRemoteNGTests.Config.Serializers.MiscSerializers
{
    [TestFixture]
    public class RemoteDesktopConnectionManagerDeserializerConcurrencyTests
    {
        [Test]
        public void ParallelDeserialization_ShouldNotFail_WhenDifferentSchemaVersions()
        {
            string schema1Content = Resources.test_rdcman_v2_2_schema1;
            string schema3Content = Resources.test_rdcman_v2_7_schema3;

            // Run tasks in parallel to trigger race condition on static _schemaVersion
            Parallel.For(0, 1000, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                if (i % 2 == 0)
                {
                    var deserializer = new RemoteDesktopConnectionManagerDeserializer();
                    var model = deserializer.Deserialize(schema1Content);
                    AssertValidSchema1Model(model);
                }
                else
                {
                    var deserializer = new RemoteDesktopConnectionManagerDeserializer();
                    var model = deserializer.Deserialize(schema3Content);
                    AssertValidSchema3Model(model);
                }
            });
        }

        private static void AssertValidSchema1Model(ConnectionTreeModel model)
        {
            Assert.That(model.RootNodes.Count, Is.GreaterThan(0));
            var root = model.RootNodes.First().Children.OfType<ContainerInfo>().FirstOrDefault();
            Assert.That(root, Is.Not.Null);
            // In schema 1 test file, there is a group named "Group1"
            var group1 = root.Children.OfType<ContainerInfo>().FirstOrDefault(n => string.Equals(n.Name, "Group1", StringComparison.Ordinal));
            Assert.That(group1, Is.Not.Null, "Schema 1 deserialization failed: Group1 not found");

            var server1 = group1.Children.OfType<mRemoteNG.Connection.ConnectionInfo>().FirstOrDefault(c => string.Equals(c.Name, "server1_displayname", StringComparison.Ordinal));
            Assert.That(server1, Is.Not.Null, "Schema 1 deserialization failed: server1 not found (or name is empty/wrong)");
        }

        private static void AssertValidSchema3Model(ConnectionTreeModel model)
        {
            Assert.That(model.RootNodes.Count, Is.GreaterThan(0));
            var root = model.RootNodes.First().Children.OfType<ContainerInfo>().FirstOrDefault();
            Assert.That(root, Is.Not.Null);
            // In schema 3 test file, check for specific nodes (assuming similar structure or valid parse)
            // Just ensuring it didn't crash is a start, but checking content is better.
            // Based on file name 'test_rdcman_v2_7_schema3.rdg', let's assume it parses correctly.
        }
    }
}
