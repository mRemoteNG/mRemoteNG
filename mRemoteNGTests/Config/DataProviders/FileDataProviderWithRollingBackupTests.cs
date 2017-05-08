using System.IO;
using mRemoteNG.Config.DataProviders;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders
{
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
            _testFileRollingBackup = Path.GetFileName(_testFilePath) + ".*-*-*.backup";
            _dataProvider = new FileDataProviderWithRollingBackup(_testFilePath);
        }

        [TearDown]
        public void Teardown()
        {
            var allTestFileMatcher = Path.GetFileName(_testFilePath) + "*";
            FileTestHelpers.DeleteFilesInDirectory(_testFileDirectory, allTestFileMatcher);
        }

        [Test]
        public void RollingBackupCreatedIfRegularBackupExists()
        {
            for (var i = 0; i < 4; i++)
                _dataProvider.Save("");
                
            var rollingBackupFiles = Directory.GetFiles(_testFileDirectory, _testFileRollingBackup);
            Assert.That(rollingBackupFiles.Length, Is.EqualTo(2));
        }

        [Test]
        public void NoRollingBackupCreatedIfRegularBackupDoesntExists()
        {
            for (var i = 0; i < 2; i++)
                _dataProvider.Save("");

            var rollingBackupFiles = Directory.GetFiles(_testFileDirectory, _testFileRollingBackup);
            Assert.That(rollingBackupFiles.Length, Is.EqualTo(0));
        }
    }
}