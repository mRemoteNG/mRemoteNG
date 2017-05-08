using System.IO;
using mRemoteNG.Config.DataProviders;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders
{
    public class FileDataProviderWithBackupTests
    {
        private FileDataProviderWithBackup _dataProvider;
        private string _testFilePath;
        private string _testFilePathBackup;

        [SetUp]
        public void Setup()
        {
            _testFilePath = FileTestHelpers.NewTempFilePath();
            _testFilePathBackup = _testFilePath + ".backup";
            _dataProvider = new FileDataProviderWithBackup(_testFilePath);
        }

        [TearDown]
        public void Teardown()
        {
            FileTestHelpers.DeleteTestFile(_testFilePath);
            FileTestHelpers.DeleteTestFile(_testFilePathBackup);
        }

        [Test]
        public void BackupCreatedWhenFileAlreadyExists()
        {
            _dataProvider.Save("");
            _dataProvider.Save("");
            var backupFileExists = File.Exists(_testFilePathBackup);
            Assert.That(backupFileExists, Is.True);
        }

        [Test]
        public void BackupNotCreatedIfFileDidntAlreadyExist()
        {
            _dataProvider.Save("");
            var backupFileExists = File.Exists(_testFilePathBackup);
            Assert.That(backupFileExists, Is.False);
        }
    }
}