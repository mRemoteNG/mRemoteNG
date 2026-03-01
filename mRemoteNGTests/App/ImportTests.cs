using System.IO;
using mRemoteNG.App;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNGTests.Properties;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.App;

public class ImportTests
{
    [Test]
    public void ErrorHandlerCalledWhenUnsupportedFileExtensionFound()
    {
        using (FileTestHelpers.DisposableTempFile(out var file, ".blah"))
        {
            var conService = new ConnectionsService(PuttySessionsManager.Instance);
            var container = new ContainerInfo();
            var exceptionOccurred = false;

            Import.HeadlessFileImport(new[] { file }, container, conService, s => exceptionOccurred = true);

            Assert.That(exceptionOccurred);
        }
    }

    [Test]
    public void AnErrorInOneFileDoNotPreventOtherFilesFromProcessing()
    {
        using (FileTestHelpers.DisposableTempFile(out var badFile, ".blah"))
        using (FileTestHelpers.DisposableTempFile(out var rdpFile, ".rdp"))
        {
            File.AppendAllText(rdpFile, Resources.test_remotedesktopconnection_rdp);
            var conService = new ConnectionsService(PuttySessionsManager.Instance);
            var container = new ContainerInfo();
            var exceptionCount = 0;

            Import.HeadlessFileImport(new[] { badFile, rdpFile }, container, conService, s => exceptionCount++);

            Assert.That(exceptionCount, Is.EqualTo(1));
            Assert.That(container.Children, Has.One.Items);
        }
    }

    [Test]
    public void TextListFileIsImported()
    {
        using (FileTestHelpers.DisposableTempFile(out var textFile, ".txt"))
        {
            File.AppendAllText(textFile, "server1\r\nserver2 user2 pass2 3390");
            var conService = new ConnectionsService(PuttySessionsManager.Instance);
            var container = new ContainerInfo();

            Import.HeadlessFileImport(new[] { textFile }, container, conService);

            Assert.That(container.Children.Count, Is.EqualTo(2));
            Assert.That(container.Children[0].Hostname, Is.EqualTo("server1"));
            Assert.That(container.Children[1].Hostname, Is.EqualTo("server2"));
            Assert.That(container.Children[1].Port, Is.EqualTo(3390));
        }
    }
}