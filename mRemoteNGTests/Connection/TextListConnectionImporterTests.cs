using System.IO;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    [TestFixture]
    public class TextListConnectionImporterTests
    {
        [Test]
        public void Import_ValidFile_AddsConnections()
        {
            using (FileTestHelpers.DisposableTempFile(out var textFile, ".txt"))
            {
                File.AppendAllText(textFile, "host1\r\nhost2 user2 pass2 3390");
                var destination = new ContainerInfo();
                var importer = new TextListConnectionImporter();

                importer.Import(textFile, destination);

                Assert.That(destination.Children.Count, Is.EqualTo(2));
                Assert.That(destination.Children[0].Hostname, Is.EqualTo("host1"));
                Assert.That(destination.Children[1].Hostname, Is.EqualTo("host2"));
                Assert.That(destination.Children[1].Username, Is.EqualTo("user2"));
                Assert.That(destination.Children[1].Password, Is.EqualTo("pass2"));
                Assert.That(destination.Children[1].Port, Is.EqualTo(3390));
            }
        }

        [Test]
        public void Import_MissingFile_DoesNothing()
        {
            var destination = new ContainerInfo();
            var importer = new TextListConnectionImporter();

            importer.Import(Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.txt"), destination);

            Assert.That(destination.Children, Is.Empty);
        }
    }
}
