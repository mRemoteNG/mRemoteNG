using System;
using System.IO;
using mRemoteNG.Config.DataProviders;
using NUnit.Framework;

namespace mRemoteNGTests.Config.DataProviders;

public class FileDataProviderSaveFailureTests
{
    [Test]
    public void PlainProviderReportsFailedWriteWithoutThrowing()
    {
        string directory = Path.Combine(Path.GetTempPath(), "mrng-provider-failure-" + Guid.NewGuid().ToString("N"));
        string destination = Path.Combine(directory, "pnlLayout.xml");
        Directory.CreateDirectory(destination); // A directory cannot be replaced by the saved file.
        try
        {
            // Callers such as SettingsSaver continue with later steps after a failed layout save.
            Assert.DoesNotThrow(() => new FileDataProvider(destination).Save("<layout />"));
            Assert.That(Directory.Exists(destination), Is.True);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Test]
    public void RollingBackupProviderPropagatesFailedWrite()
    {
        string directory = Path.Combine(Path.GetTempPath(), "mrng-provider-failure-" + Guid.NewGuid().ToString("N"));
        string destination = Path.Combine(directory, "confCons.xml");
        Directory.CreateDirectory(destination);
        try
        {
            var error = Assert.Catch(() => new FileDataProviderWithRollingBackup(destination).Save("<connections />"));
            Assert.That(error, Is.InstanceOf<IOException>().Or.InstanceOf<UnauthorizedAccessException>());
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
}
