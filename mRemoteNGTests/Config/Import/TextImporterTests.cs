using System.Linq;
using mRemoteNG.Config.Import;
using mRemoteNG.Container;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Import
{
    [TestFixture]
    public class TextImporterTests
    {
        [Test]
        public void Import_ParsesSimpleLine_AddsConnection()
        {
            var importer = new TextImporter();
            var container = new ContainerInfo();
            var input = "192.168.1.1 user password";

            importer.Import(input, container);

            Assert.That(container.Children.Count, Is.EqualTo(1));
            var conn = container.Children[0];
            Assert.That(conn.Hostname, Is.EqualTo("192.168.1.1"));
            Assert.That(conn.Username, Is.EqualTo("user"));
            Assert.That(conn.Password, Is.EqualTo("password"));
        }

        [Test]
        public void Import_ParsesMultipleLines_AddsMultipleConnections()
        {
            var importer = new TextImporter();
            var container = new ContainerInfo();
            var input = "host1 user1 pass1\r\nhost2 user2 pass2";

            importer.Import(input, container);

            Assert.That(container.Children.Count, Is.EqualTo(2));
            Assert.That(container.Children[0].Hostname, Is.EqualTo("host1"));
            Assert.That(container.Children[1].Hostname, Is.EqualTo("host2"));
        }

        [Test]
        public void Import_ParsesCommaSeparatedValues()
        {
            var importer = new TextImporter();
            var container = new ContainerInfo();
            var input = "host1,user1,pass1,3389";

            importer.Import(input, container);

            Assert.That(container.Children.Count, Is.EqualTo(1));
            var conn = container.Children[0];
            Assert.That(conn.Hostname, Is.EqualTo("host1"));
            Assert.That(conn.Username, Is.EqualTo("user1"));
            Assert.That(conn.Password, Is.EqualTo("pass1"));
            Assert.That(conn.Port, Is.EqualTo(3389));
        }

        [Test]
        public void Import_IgnoresEmptyLines()
        {
            var importer = new TextImporter();
            var container = new ContainerInfo();
            var input = "host1\r\n\r\nhost2";

            importer.Import(input, container);

            Assert.That(container.Children.Count, Is.EqualTo(2));
        }

        [Test]
        public void Import_HandlesMissingFields()
        {
            var importer = new TextImporter();
            var container = new ContainerInfo();
            var input = "host1";

            importer.Import(input, container);

            Assert.That(container.Children.Count, Is.EqualTo(1));
            Assert.That(container.Children[0].Hostname, Is.EqualTo("host1"));
            Assert.That(container.Children[0].Username, Is.Null.Or.Empty);
        }
    }
}
