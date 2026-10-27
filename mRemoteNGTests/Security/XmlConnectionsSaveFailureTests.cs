using System;
using System.IO;
using mRemoteNG.Config;
using mRemoteNG.Config.Connections;
using mRemoteNG.Security;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;
using NUnit.Framework;

namespace mRemoteNGTests.Security;

public class XmlConnectionsSaveFailureTests
{
    [Test]
    public void FailedDestinationWriteCannotBeReportedAsSuccessfulSave()
    {
        string directory = Path.Combine(Path.GetTempPath(), "mrng-save-failure-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        string destination = Path.Combine(directory, "connections.xml");
        Directory.CreateDirectory(destination); // A directory cannot be atomically replaced by the saved file.
        var model = new ConnectionTreeModel();
        model.AddRootNode(new RootNodeInfo(RootNodeType.Connection)
        {
            PasswordString = "Synthetic-Test-Master-Key-123!"
        });
        try
        {
            var saver = new XmlConnectionsSaver(destination, new SaveFilter());
            var error = Assert.Catch(() => saver.Save(model));
            Assert.That(error, Is.InstanceOf<IOException>().Or.InstanceOf<UnauthorizedAccessException>());
            Assert.That(Directory.Exists(destination), Is.True);
            Assert.That(Directory.GetFiles(directory), Is.Empty, "Failed writes must remove their temporary file.");
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
}
