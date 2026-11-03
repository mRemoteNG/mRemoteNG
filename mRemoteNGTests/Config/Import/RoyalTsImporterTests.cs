using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using mRemoteNG.Config.Import;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNGTests.Config.Serializers.MiscSerializers;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Import;

public class RoyalTsImporterTests
{
    private const string MinimalDocument =
        """
        <?xml version="1.0" encoding="utf-8"?>
        <RTSZDocument>
          <RoyalSSHConnection>
            <ID>b0000000-0000-4000-8000-000000000001</ID>
            <ParentID>b0000000-0000-4000-8000-000000000000</ParentID>
            <Name>router</Name>
            <URI>10.0.0.1</URI>
            <PositionNr>2</PositionNr>
          </RoyalSSHConnection>
        </RTSZDocument>
        """;

    private static readonly Encoding Utf8WithBom = new UTF8Encoding(true);

    [Test]
    public void PlainXmlDocumentIsImportedIntoAFolderNamedAfterTheDocument()
    {
        using (FileTestHelpers.DisposableTempFile(out var file, ".rtsz"))
        {
            File.WriteAllText(file, RoyalTsDocumentDeserializerTests.Document, Utf8WithBom);
            var destination = new ContainerInfo();

            new RoyalTsImporter().Import(file, destination);

            var imported = (ContainerInfo)destination.Children.Single();
            Assert.That(imported.Name, Is.EqualTo("Lab"));
            Assert.That(imported.Children.Select(node => node.Name), Is.EqualTo(new[] { "Web", "Servers" }));
        }
    }

    [Test]
    public void ZippedDocumentIsReadFromTheArchive()
    {
        using (FileTestHelpers.DisposableTempFile(out var file, ".rtsz"))
        {
            using (var stream = File.Create(file))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
            using (var writer = new StreamWriter(archive.CreateEntry("Document.rtsz").Open(), Utf8WithBom))
                writer.Write(RoyalTsDocumentDeserializerTests.Document);
            var destination = new ContainerInfo();

            new RoyalTsImporter().Import(file, destination);

            var imported = (ContainerInfo)destination.Children.Single();
            Assert.That(imported.Name, Is.EqualTo("Lab"));
            Assert.That(imported.GetRecursiveChildList().Any(node => node.Name == "dc01"), Is.True);
        }
    }

    [Test]
    public void DocumentWithoutANameIsImportedUnderTheFileName()
    {
        using (FileTestHelpers.DisposableTempFile(out var file, ".rtsz"))
        {
            File.WriteAllText(file, MinimalDocument, Utf8WithBom);
            var destination = new ContainerInfo();

            new RoyalTsImporter().Import(file, destination);

            var imported = (ContainerInfo)destination.Children.Single();
            Assert.That(imported.Name, Is.EqualTo(Path.GetFileNameWithoutExtension(file)));
            Assert.That(imported.Children.Single().Name, Is.EqualTo("router"));
        }
    }

    [Test]
    public void ImportFromFileRecognisesTheRtszExtension()
    {
        using (FileTestHelpers.DisposableTempFile(out var file, ".rtsz"))
        {
            File.WriteAllText(file, MinimalDocument, Utf8WithBom);
            var conService = new ConnectionsService(PuttySessionsManager.Instance);
            var destination = new ContainerInfo();
            var failed = false;

            // Import alone would name this test namespace, not mRemoteNG.App.Import.
            mRemoteNG.App.Import.HeadlessFileImport(new[] { file }, destination, conService, _ => failed = true);

            Assert.That(failed, Is.False);
            Assert.That(destination.Children, Has.One.Items);
        }
    }
}
