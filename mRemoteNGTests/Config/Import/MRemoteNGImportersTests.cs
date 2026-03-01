using System.IO;
using mRemoteNG.Config.Import;
using mRemoteNG.Container;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Import;

[TestFixture]
public class MRemoteNGImportersTests
{
    [Test]
    public void CsvImporter_ReturnsWithoutCreatingMissingFile()
    {
        string filePath = FileTestHelpers.NewTempFilePath(".csv");
        var destination = new ContainerInfo();
        var importer = new MRemoteNGCsvImporter();

        importer.Import(filePath, destination);

        Assert.That(File.Exists(filePath), Is.False);
        Assert.That(destination.Children, Is.Empty);
    }

    [Test]
    public void XmlImporter_ReturnsWithoutCreatingMissingFile()
    {
        string filePath = FileTestHelpers.NewTempFilePath(".xml");
        var destination = new ContainerInfo();
        var importer = new MRemoteNGXmlImporter();

        importer.Import(filePath, destination);

        Assert.That(File.Exists(filePath), Is.False);
        Assert.That(destination.Children, Is.Empty);
    }
}

