using System;
using System.IO;
using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Messages;
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
        Runtime.MessageCollector.ClearMessages();
    }

    [TearDown]
    public void Teardown()
    {
        if (Directory.Exists(_testFileDirectory))
            Directory.Delete(_testFileDirectory, true);
        Runtime.MessageCollector.ClearMessages();
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

    /// <summary>
    /// Simulates a disk-full/permission-denied style write failure (a real "disk is full" is not
    /// practically reproducible in a unit test) by pointing <see cref="FileDataProvider.FilePath"/>
    /// at an existing directory instead of a file — <see cref="File.WriteAllText(string, string)"/>
    /// throws <see cref="UnauthorizedAccessException"/> for that, the same way it would for a genuine
    /// out-of-space write. Confirms <see cref="FileDataProvider.Save"/> does not let that exception
    /// escape (it would crash the caller — e.g. the connections-save flow in
    /// <see cref="mRemoteNG.Config.Connections.XmlConnectionsSaver"/>) and that it is reported via
    /// <see cref="Runtime.MessageCollector"/> instead of silently disappearing.
    /// </summary>
    [Test]
    public void Save_WhenWriteFails_DoesNotThrowAndReportsExceptionToMessageCollector()
    {
        string directoryAsFilePath = Path.Combine(_testFileDirectory, "a-directory-not-a-file");
        Directory.CreateDirectory(directoryAsFilePath);
        _dataProvider.FilePath = directoryAsFilePath;

        Assert.DoesNotThrow(() => _dataProvider.Save("content that cannot be written"));

        Assert.That(Runtime.MessageCollector.Messages.Any(m =>
            m.Class == MessageClass.ErrorMsg && m.Text.Contains("Failed to save file")), Is.True);
    }
}