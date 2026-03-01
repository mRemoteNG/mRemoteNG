using System;
using System.IO;
using mRemoteNG.Config.DataProviders;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders;

public class FileBackupPrunerTests
{
    private FileBackupPruner _fileBackupPruner;
    private string _testFilePath;
    private string _testFileDirectory;

    [SetUp]
    public void Setup()
    {
        _testFilePath = FileTestHelpers.NewTempFilePath();
        _testFileDirectory = Path.GetDirectoryName(_testFilePath);
        _fileBackupPruner = new FileBackupPruner();
    }

    [TearDown]
    public void Teardown()
    {
        if (Directory.Exists(_testFileDirectory))
            Directory.Delete(_testFileDirectory, true);
    }

    [Test]
    public void PruneBackupFiles_WithPathTraversal_ThrowsArgumentException()
    {
        string maliciousPath = @"..\..\..\Windows\System32\config.xml";
        Assert.Throws<ArgumentException>(() => FileBackupPruner.PruneBackupFiles(maliciousPath, 5));
    }

    [Test]
    public void PruneBackupFiles_WithForwardSlashTraversal_ThrowsArgumentException()
    {
        string maliciousPath = @"../../../etc/passwd";
        Assert.Throws<ArgumentException>(() => FileBackupPruner.PruneBackupFiles(maliciousPath, 5));
    }

    [Test]
    public void PruneBackupFiles_WithValidPath_DoesNotThrow()
    {
        // Create the test file
        File.WriteAllText(_testFilePath, "test");
        
        Assert.DoesNotThrow(() => FileBackupPruner.PruneBackupFiles(_testFilePath, 5));
    }
}
