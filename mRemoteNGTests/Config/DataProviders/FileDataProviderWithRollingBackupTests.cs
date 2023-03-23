using System.IO;
using System.Threading;
using mRemoteNG.Config.DataProviders;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders;

public class FileDataProviderWithRollingBackupTests
{
    private FileDataProviderWithRollingBackup _dataProvider;
    private string _testFilePath;
    private string _testFileDirectory;
    private string _testFileRollingBackup;

    [SetUp]
    public void Setup()
    {
        _testFilePath = FileTestHelpers.NewTempFilePath();
        _testFileDirectory = Path.GetDirectoryName(_testFilePath);
        _testFileRollingBackup = Path.GetFileName(_testFilePath) + ".*-*.backup";
        _dataProvider = new FileDataProviderWithRollingBackup(_testFilePath);
    }

    [TearDown]
    public void Teardown()
    {
        if (Directory.Exists(_testFileDirectory))
            Directory.Delete(_testFileDirectory, true);
    }

    [Test]
    public void RollingBackupCreatedIfRegularBackupExists()
    {
        for (var i = 0; i < 3; i++)
        {
            _dataProvider.Save("");
            Thread.Sleep(100);
        }

        var rollingBackupFiles = Directory.GetFiles(_testFileDirectory, _testFileRollingBackup);
        Assert.That(rollingBackupFiles.Length, Is.EqualTo(2));
    }

    [Test]
    public void NoRollingBackupCreatedIfRegularFileDoesntExists()
    {
        _dataProvider.Save("");
        var rollingBackupFiles = Directory.GetFiles(_testFileDirectory, _testFileRollingBackup);
        Assert.That(rollingBackupFiles.Length, Is.EqualTo(0));
    }
}