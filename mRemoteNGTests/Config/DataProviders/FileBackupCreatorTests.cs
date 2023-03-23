using System.IO;
using mRemoteNG.Config.DataProviders;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders;

public class FileBackupCreatorTests
{
    private FileBackupCreator _fileBackupCreator;
    private string _testFilePath;
    private string _testFilePathBackup;
    private string _testFileDirectory;
    private string _testFileRollingBackup;

    [SetUp]
    public void Setup()
    {
        _testFilePath = FileTestHelpers.NewTempFilePath();
        _testFileDirectory = Path.GetDirectoryName(_testFilePath);
        _testFileRollingBackup = Path.GetFileName(_testFilePath) + ".*-*.backup";
        _testFilePathBackup = _testFilePath + ".backup";
        _fileBackupCreator = new FileBackupCreator();
    }

    [TearDown]
    public void Teardown()
    {
        if (Directory.Exists(_testFileDirectory))
            Directory.Delete(_testFileDirectory, true);
    }

    [Test]
    public void BackupCreatedWhenFileAlreadyExists()
    {
        File.WriteAllText(_testFilePath, "");
        _fileBackupCreator.CreateBackupFile(_testFilePath);
        var rollingBackupFiles = Directory.GetFiles(_testFileDirectory, _testFileRollingBackup);
        Assert.That(rollingBackupFiles.Length, Is.EqualTo(1));
    }

    [Test]
    public void BackupNotCreatedIfFileDidntAlreadyExist()
    {
        _fileBackupCreator.CreateBackupFile(_testFilePath);
        var backupFileExists = File.Exists(_testFilePathBackup);
        Assert.That(backupFileExists, Is.False);
    }
}