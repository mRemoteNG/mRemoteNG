using mRemoteNG.Config.Connections;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;
using System;
using System.IO;
using System.Xml;

namespace mRemoteNGTests.Config.Connections;

internal class XmlConnectionsLoaderTests
{
    private const string ValidConnectionsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Connections Name=""Connections"" ConfVersion=""1.2""></Connections>";

    [Test]
    public void ThrowsFileNotFound()
    {
        Assert.Throws<FileNotFoundException>(() => new XmlConnectionsLoader(FileTestHelpers.NewTempFilePath()).Load());
    }

    [Test]
    public void ThrowsArgumentExceptionWhenFilePathIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => new XmlConnectionsLoader(""));
    }

    [Test]
    public void ThrowsXmlExceptionForXxePayload()
    {
        const string xxePayload = @"<?xml version='1.0'?>
<!DOCTYPE foo [
<!ELEMENT foo ANY >
<!ENTITY xxe SYSTEM 'file:///etc/passwd' >]>
<root><item>&xxe;</item></root>";

        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            File.WriteAllText(filePath, xxePayload);
            var loader = new XmlConnectionsLoader(filePath);

            Assert.Throws<XmlException>(() => loader.Load());
        }
    }

    [Test]
    public void FallsBackToNewestValidBackupWhenPrimaryFileHasInvalidXml()
    {
        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            File.WriteAllText(filePath, "<LocalConnections>");

            string olderBrokenBackup = $"{filePath}.20250207-1100000000.backup";
            File.WriteAllText(olderBrokenBackup, "<Connections");
            File.SetLastWriteTimeUtc(olderBrokenBackup, DateTime.UtcNow.AddMinutes(-10));

            string newerValidBackup = $"{filePath}.20250207-1200000000.backup";
            File.WriteAllText(newerValidBackup, ValidConnectionsXml);
            File.SetLastWriteTimeUtc(newerValidBackup, DateTime.UtcNow.AddMinutes(-1));

            XmlConnectionsLoader loader = new(filePath);
            var loadedTree = loader.Load();

            Assert.That(loadedTree, Is.Not.Null);
            Assert.That(File.ReadAllText(filePath), Is.EqualTo(ValidConnectionsXml));
        }
    }
}
