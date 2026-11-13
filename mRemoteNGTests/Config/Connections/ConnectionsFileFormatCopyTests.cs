using System;
using System.IO;
using System.Linq;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Connection;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Connections;

public class ConnectionsFileFormatCopyTests
{
    // The root of a file saved by 1.76; the copy is taken before the file is read, so a node is enough.
    private const string File26 =
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
        "<mrng:Connections xmlns:mrng=\"http://mremoteng.org\" Name=\"Connections\" Export=\"false\" ConfVersion=\"2.6\">\r\n" +
        "  <Node Name=\"server\" Type=\"Connection\" Hostname=\"server.example\" />\r\n" +
        "</mrng:Connections>";

    private string _directory;
    private string _filePath;
    private string _copyPath;

    [SetUp]
    public void Setup()
    {
        _directory = Path.Combine(Path.GetTempPath(), "mrng-format-copy-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directory);
        _filePath = Path.Combine(_directory, "confCons.xml");
        _copyPath = _filePath + ".v2.6.bak";
    }

    [TearDown]
    public void Teardown()
    {
        if (Directory.Exists(_directory))
            Directory.Delete(_directory, true);
    }

    [Test]
    public void FileInAnOlderFormatIsCopiedBeforeItIsSavedInTheNewOne()
    {
        File.WriteAllText(_filePath, File26);

        Save();

        Assert.That(File.ReadAllText(_copyPath), Is.EqualTo(File26));
        Assert.That(VersionOf(_filePath), Is.GreaterThan(new Version(2, 6)));
    }

    [Test]
    public void FileAlreadyInTheCurrentFormatIsNotCopied()
    {
        Save();
        Save();

        Assert.That(Directory.GetFiles(_directory).Where(f => f.EndsWith(".bak", StringComparison.OrdinalIgnoreCase)), Is.Empty);
    }

    [Test]
    public void ExistingCopyIsNeverReplaced()
    {
        File.WriteAllText(_copyPath, "first copy");
        File.WriteAllText(_filePath, File26);

        Save();

        Assert.That(File.ReadAllText(_copyPath), Is.EqualTo("first copy"));
    }

    [Test]
    public void CopyIsKeptWhenTheRollingBackupsArePruned()
    {
        File.WriteAllText(_filePath, File26);
        Save();
        string rollingBackup = string.Format(mRemoteNG.Properties.OptionsBackupPage.Default.BackupFileNameFormat, _filePath, DateTime.Now.AddMinutes(1));
        File.WriteAllText(rollingBackup, File26);

        new FileBackupPruner().PruneBackupFiles(_filePath, 0);

        Assert.That(File.Exists(rollingBackup), Is.False, "The pruning did not run.");
        Assert.That(File.Exists(_copyPath), Is.True);
    }

    [TestCase("<Connections ConfVersion=\"2.6\" />", "2.6")]
    [TestCase("<Connections ConfVersion=\"2,6\" />", "2.6")]
    [TestCase("<?xml version=\"1.0\" encoding=\"utf-8\"?><mrng:Connections xmlns:mrng=\"http://mremoteng.org\" FullFileEncryption=\"true\" ConfVersion=\"2.9\">encrypted</mrng:Connections>", "2.9")]
    [TestCase("<Connections />", null)]
    [TestCase("encrypted whole by an old version", null)]
    [TestCase("", null)]
    public void VersionIsReadFromTheRootElement(string content, string expected)
    {
        Version version = ConnectionsFileFormatCopy.ReadVersion(new StringReader(content));

        Assert.That(version, Is.EqualTo(expected == null ? null : Version.Parse(expected)));
    }

    private void Save()
    {
        ConnectionTreeModel model = new();
        RootNodeInfo root = new(RootNodeType.Connection);
        root.AddChild(new ConnectionInfo { Name = "server", Hostname = "server.example" });
        model.AddRootNode(root);
        new XmlConnectionsSaver(_filePath, new SaveFilter()).Save(model);
    }

    private static Version VersionOf(string filePath)
    {
        using StreamReader reader = File.OpenText(filePath);
        return ConnectionsFileFormatCopy.ReadVersion(reader);
    }
}
