using System;
using System.IO;
using mRemoteNG.Config.DataProviders;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders
{
    public class FileDataProviderTests
    {
        private FileDataProvider _dataProvider;
        private string _testFilePath;

        [SetUp]
        public void Setup()
        {
            _testFilePath = FileTestHelpers.NewTempFilePath();
            FileTestHelpers.DeleteTestFile(_testFilePath);
            _dataProvider = new FileDataProvider(_testFilePath);
        }

        [TearDown]
        public void Teardown()
        {
            var tempFilePath = FileTestHelpers.GetAppSpecificTempDirectory();
            Directory.Delete(tempFilePath, true);
        }

        [Test]
        public void SetsFileContent()
        {
            Assert.That(File.Exists(_testFilePath), Is.False);
            var expectedFileContent = Guid.NewGuid().ToString();
            _dataProvider.Save(expectedFileContent);
            var fileContent = File.ReadAllText(_testFilePath);
            Assert.That(fileContent, Is.EqualTo(expectedFileContent));
        }

        [Test]
        public void LoadingFileThatDoesntExistProvidesEmptyString()
        {
            var fileThatShouldntExist = Guid.NewGuid().ToString();
            var dataProvider = new FileDataProvider(fileThatShouldntExist);
            var loadedData = dataProvider.Load();
            Assert.That(loadedData, Is.Empty);
        }

        [Test]
        public void SaveCreatesDirectoriesThatDontExist()
        {
            var folder1 = Guid.NewGuid().ToString();
            var folder2 = Guid.NewGuid().ToString();
            var fileThatShouldExist = Path.Combine(FileTestHelpers.GetAppSpecificTempDirectory(), folder1, folder2, Path.GetRandomFileName());
            _dataProvider.FilePath = fileThatShouldExist;
            _dataProvider.Save("");
            Assert.That(File.Exists(fileThatShouldExist), Is.True);
        }
    }
}