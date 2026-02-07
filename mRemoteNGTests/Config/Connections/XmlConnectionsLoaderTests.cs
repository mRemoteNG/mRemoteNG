using System;
using System.IO;
using System.Xml;
using mRemoteNG.Config.Connections;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Connections;

internal class XmlConnectionsLoaderTests
{
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
}
