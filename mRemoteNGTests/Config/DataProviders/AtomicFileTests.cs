using System;
using System.IO;
using mRemoteNG.Config.DataProviders;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders;

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
    public void ReplacesExistingFileWithLongNameAndLeavesNoTemporaryFiles()
    {
        string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        string path = Path.Combine(directory, new string('a', 219) + ".xml");
        try
        {
            File.WriteAllText(path, "original");
            AtomicFile.WriteAllText(path, "replacement");
            Assert.That(File.ReadAllText(path), Is.EqualTo("replacement"));
            Assert.That(Directory.GetFiles(directory), Has.Length.EqualTo(1));
        }
        finally { Directory.Delete(directory, recursive: true); }
    }

    [Test]
    [Platform("Win")]
    public void FailedReplacementPreservesExistingFileAndCleansTemporaryFile()
    {
        string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        string destination = Path.Combine(directory, "connections.xml");
        File.WriteAllText(destination, "original");
        try
        {
            using (var stream = new FileStream(destination, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Assert.That(() => AtomicFile.WriteAllText(destination, "replacement"), Throws.InstanceOf<IOException>());
                using (var reader = new StreamReader(stream))
                    Assert.That(reader.ReadToEnd(), Is.EqualTo("original"));
                Assert.That(Directory.GetFiles(directory), Is.EquivalentTo(new[] { destination }));
            }
            Assert.That(File.ReadAllText(destination), Is.EqualTo("original"));
        }
        finally { Directory.Delete(directory, recursive: true); }
    }

    [Test]
    public void FailedMovePreservesExistingDirectoryAndCleansTemporaryFile()
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
