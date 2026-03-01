using System;
using System.IO;
using System.Xml;
using mRemoteNG.Config.Connections;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Connections;

internal class XmlConnectionsLoaderTests
{
    private static readonly Func<string, Optional<System.Security.SecureString>> NoPasswordRequestor =
        _ => Optional<System.Security.SecureString>.Empty;

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
        const string xxePayload = @"<?xml version=""1.0"" encoding=""utf-8""?>
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
            File.WriteAllText(filePath, @"<?xml version=""1.0"" encoding=""utf-8""?><LocalConnections>");

            string olderBrokenBackup = $"{filePath}.20250207-1100000000.backup";
            File.WriteAllText(olderBrokenBackup, @"<?xml version=""1.0"" encoding=""utf-8""?><Connections");
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

    [Test]
    public void ThrowsWhenNoBackupsExistAndPrimaryIsCorrupt()
    {
        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            File.WriteAllText(filePath, "this is not xml at all");

            XmlConnectionsLoader loader = new(filePath, new MessageCollector(), NoPasswordRequestor);

            Assert.Throws<XmlException>(() => loader.Load());
        }
    }

    [Test]
    public void ThrowsWhenAllBackupsAreAlsoCorrupt()
    {
        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            File.WriteAllText(filePath, "corrupt primary");

            string backup1 = $"{filePath}.20250207-1100000000.backup";
            File.WriteAllText(backup1, "corrupt backup 1");

            string backup2 = $"{filePath}.20250207-1200000000.backup";
            File.WriteAllText(backup2, "corrupt backup 2");

            XmlConnectionsLoader loader = new(filePath, new MessageCollector(), NoPasswordRequestor);

            Assert.Throws<XmlException>(() => loader.Load());
        }
    }

    [Test]
    public void LoadsSuccessfullyWhenPrimaryFileIsValid()
    {
        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            File.WriteAllText(filePath, ValidConnectionsXml);

            XmlConnectionsLoader loader = new(filePath);
            var loadedTree = loader.Load();

            Assert.That(loadedTree, Is.Not.Null);
        }
    }

    [Test]
    public void FallsBackToValidBackupWhenPrimaryFileIsEmpty()
    {
        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            // Simulate crash-corrupted (empty) primary file
            File.WriteAllText(filePath, "");

            string validBackup = $"{filePath}.20250207-1200000000.backup";
            File.WriteAllText(validBackup, ValidConnectionsXml);

            XmlConnectionsLoader loader = new(filePath, new MessageCollector(), NoPasswordRequestor);
            var loadedTree = loader.Load();

            Assert.That(loadedTree, Is.Not.Null);
            Assert.That(File.ReadAllText(filePath), Is.EqualTo(ValidConnectionsXml));
        }
    }

    [Test]
    public void ThrowsXmlExceptionWhenFileIsEmptyAndNoBackupsExist()
    {
        using (FileTestHelpers.DisposableTempFile(out var filePath, ".xml"))
        {
            File.WriteAllText(filePath, "");

            XmlConnectionsLoader loader = new(filePath, new MessageCollector(), NoPasswordRequestor);

            Assert.Throws<XmlException>(() => loader.Load());
        }
    }
}
