using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using mRemoteNG.App;
using mRemoteNG.Config.Connections;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Connections;

[NonParallelizable]
public class ConnectionRecoveryFailureTests
{
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("<?xml version=\"1.0\" encoding=\"utf-8\"?><Connections><Node>")]
    public void MalformedFile_LoadException_AllowsStartupRecoveryWithoutChangingOriginal(string malformed)
    {
        string path = Path.Combine(Path.GetTempPath(), "mRemoteNG-load-" + Guid.NewGuid().ToString("N") + ".xml");
        File.WriteAllText(path, malformed);
        try
        {
            var error = Assert.Throws<XmlException>(() => new XmlConnectionsLoader(path).Load());
            Assert.That(Runtime.CanRecoverConnectionsFile(error, false, false), Is.True);
            Assert.That(File.ReadAllText(path), Is.EqualTo(malformed));
        }
        finally { File.Delete(path); }
    }

    [Test]
    public void MissingFile_LoadException_AllowsStartupRecovery()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".xml");
        var error = Assert.Throws<FileNotFoundException>(() => new XmlConnectionsLoader(path));
        Assert.That(Runtime.CanRecoverConnectionsFile(error, false, false), Is.True);
    }

    [Test]
    public void OtherFailuresOrExplicitLoads_DoNotOfferCorruptionRecovery()
    {
        Assert.That(Runtime.CanRecoverConnectionsFile(new CryptographicException(), false, false), Is.False);
        Assert.That(Runtime.CanRecoverConnectionsFile(new InvalidDataException("Unsupported KDF"), false, false), Is.False);
        Assert.That(Runtime.CanRecoverConnectionsFile(new Exception("Unsupported format"), false, false), Is.False);
        Assert.That(Runtime.CanRecoverConnectionsFile(new XmlException(), true, false), Is.False);
        Assert.That(Runtime.CanRecoverConnectionsFile(new XmlException(), false, true), Is.False);
    }
}
