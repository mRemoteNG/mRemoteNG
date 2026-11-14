using System.Linq;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Properties;
using NUnit.Framework;

namespace mRemoteNGTests.Connection;

/// <summary>
/// The connection defaults as mRemoteNG loads and saves them at startup and exit, through the
/// ConDefault* settings rather than another connection.
/// </summary>
public class DefaultConnectionSettingsTests
{
    [Test]
    public void DefaultsLoadAndSaveThroughTheSettingsWithoutErrors()
    {
        int before = Runtime.MessageCollector.Messages.Count();
        DefaultConnectionInfo.Instance.LoadFrom(Settings.Default, a => "ConDefault" + a);
        DefaultConnectionInfo.Instance.SaveTo(new Settings(), a => "ConDefault" + a);

        string[] errors = Runtime.MessageCollector.Messages.Skip(before)
            .Select(message => message.Text.Replace("\r", "").Split('\n'))
            .Where(lines => lines[0].StartsWith("Error loading default") || lines[0].StartsWith("Error saving default"))
            .Select(lines => lines[0] + ": " + (lines.Length > 1 ? lines[1] : ""))
            .ToArray();

        Assert.That(errors, Is.Empty);
    }

    [TestCase(RDPDiskDrives.None)]
    [TestCase(RDPDiskDrives.Local)]
    [TestCase(RDPDiskDrives.All)]
    [TestCase(RDPDiskDrives.Custom)]
    public void DiskDrivesChosenAsTheDefaultSurviveARestart(RDPDiskDrives drives)
    {
        string stored = Settings.Default.ConDefaultRedirectDiskDrives;
        RDPDiskDrives current = DefaultConnectionInfo.Instance.RedirectDiskDrives;
        try
        {
            DefaultConnectionInfo.Instance.RedirectDiskDrives = drives;
            Settings saved = new();
            DefaultConnectionInfo.Instance.SaveTo(saved, a => "ConDefault" + a);
            Settings.Default.ConDefaultRedirectDiskDrives = saved.ConDefaultRedirectDiskDrives;

            // after a restart, new connections and the defaults are built from the settings again
            ConnectionInfo afterRestart = new();
            DefaultConnectionInfo.Instance.RedirectDiskDrives = RDPDiskDrives.None;
            DefaultConnectionInfo.Instance.LoadFrom(Settings.Default, a => "ConDefault" + a);

            Assert.That(afterRestart.RedirectDiskDrives, Is.EqualTo(drives));
            Assert.That(DefaultConnectionInfo.Instance.RedirectDiskDrives, Is.EqualTo(drives));
        }
        finally
        {
            DefaultConnectionInfo.Instance.RedirectDiskDrives = current;
            Settings.Default.ConDefaultRedirectDiskDrives = stored;
        }
    }

    // values saved while the setting was a bool, by 1.76 and by earlier 1.78 builds
    [TestCase("True", RDPDiskDrives.All)]
    [TestCase("False", RDPDiskDrives.None)]
    [TestCase("Local", RDPDiskDrives.Local)]
    [TestCase("custom", RDPDiskDrives.Custom)]
    [TestCase("", RDPDiskDrives.None)]
    [TestCase(null, RDPDiskDrives.None)]
    [TestCase("7", RDPDiskDrives.None)]
    public void StoredDiskDrivesValueIsRead(string stored, RDPDiskDrives expected)
    {
        Assert.That(RdpDiskDrivesSetting.Parse(stored), Is.EqualTo(expected));
    }
}
