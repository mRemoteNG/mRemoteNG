using System;
using System.IO;
using mRemoteNG.Config.DataProviders;
using NUnit.Framework;

namespace mRemoteNGTests.Config;

[TestFixture]
public class AtomicFileTests
{
    [Test]
    public void ReplacesExistingFileAndLeavesNoTemporaryFiles()
    {
        string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        try
        {
            string path = Path.Combine(directory, "connections.xml");
            AtomicFile.WriteAllText(path, "first");
            AtomicFile.WriteAllText(path, "second");
            Assert.That(File.ReadAllText(path), Is.EqualTo("second"));
            Assert.That(Directory.GetFiles(directory), Has.Length.EqualTo(1));
        }
        finally { Directory.Delete(directory, recursive: true); }
    }

    [Test]
    public void FailedReplacementPreservesExistingDestinationAndCleansTemporaryFile()
    {
        string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        string destination = Path.Combine(directory, "occupied");
        Directory.CreateDirectory(destination);
        File.WriteAllText(Path.Combine(destination, "original"), "keep");
        try
        {
            Assert.That(() => AtomicFile.WriteAllText(destination, "replacement"), Throws.InstanceOf<IOException>());
            Assert.That(File.ReadAllText(Path.Combine(destination, "original")), Is.EqualTo("keep"));
            Assert.That(Directory.GetFiles(directory), Is.Empty);
        }
        finally { Directory.Delete(directory, recursive: true); }
    }
}
