using System;
using System.IO;
using mRemoteNG.Config.DataProviders;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders;

public class FileDataProviderTests
{
    private FileDataProvider _dataProvider;
    private string _testFilePath;
    private string _testFileDirectory;

    [SetUp]
    public void Setup()
    {
        _testFilePath = FileTestHelpers.NewTempFilePath();
        _testFileDirectory = Path.GetDirectoryName(_testFilePath);
        _dataProvider = new FileDataProvider(_testFilePath);
    }

    [TearDown]
    public void Teardown()
    {
        if (Directory.Exists(_testFileDirectory))
            Directory.Delete(_testFileDirectory, true);
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
        var fileThatShouldExist = Path.Combine(_testFileDirectory, folder1, folder2, Path.GetRandomFileName());
        _dataProvider.FilePath = fileThatShouldExist;
        _dataProvider.Save("");
        Assert.That(File.Exists(fileThatShouldExist), Is.True);
    }

    [Test]
    public void Constructor_WithPathTraversal_ThrowsArgumentException()
    {
        string maliciousPath = @"C:\Users\..\..\..\Windows\System32\config.xml";
        Assert.Throws<ArgumentException>(() => new FileDataProvider(maliciousPath));
    }

    [Test]
    public void FilePath_SetWithPathTraversal_ThrowsArgumentException()
    {
        string maliciousPath = @"..\..\..\Windows\System32\config.xml";
        Assert.Throws<ArgumentException>(() => _dataProvider.FilePath = maliciousPath);
    }

    [Test]
    public void SaveProducesNonEmptyFileForNonEmptyContent()
    {
        var content = new string('X', 10000);
        _dataProvider.Save(content);
        var fileInfo = new FileInfo(_testFilePath);
        Assert.That(fileInfo.Exists, Is.True);
        Assert.That(fileInfo.Length, Is.GreaterThan(0));
        Assert.That(File.ReadAllText(_testFilePath), Is.EqualTo(content));
    }

    [Test]
    public void MoveTo_WithPathTraversal_ThrowsArgumentException()
    {
        string maliciousPath = @"..\..\..\Windows\System32\config.xml";
        // The method catches the exception internally, so we need to verify it doesn't move the file
        _dataProvider.Save("test");
        _dataProvider.MoveTo(maliciousPath);
        
        // Verify the file wasn't moved to the malicious path
        Assert.That(File.Exists(maliciousPath), Is.False);
        // Verify the original file still exists
        Assert.That(File.Exists(_testFilePath), Is.True);
    }
}