using System;
using System.IO;
using mRemoteNG.Config.Settings;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Settings
{
    [TestFixture]
    public class SettingsUpgraderTests
    {
        private string _appFolder;

        [SetUp]
        public void Setup()
        {
            _appFolder = Path.Combine(Path.GetTempPath(), $"mrngupgrade_{Guid.NewGuid():N}", "mRemoteNG_Url_abcd");
            Directory.CreateDirectory(_appFolder);
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                Directory.Delete(Path.GetDirectoryName(_appFolder)!, true);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        [Test]
        public void AnEarlierVersionBesideTheCurrentOneIsFound()
        {
            WriteVersion("1.78.2.3702");

            Assert.That(SettingsUpgrader.HasEarlierVersion(CurrentFile("1.78.2.3735")), Is.True);
        }

        [Test]
        public void ALaterVersionIsNotAnEarlierOne()
        {
            WriteVersion("1.78.2.3800");

            Assert.That(SettingsUpgrader.HasEarlierVersion(CurrentFile("1.78.2.3735")), Is.False);
        }

        [Test]
        public void AVersionFolderWithoutUserConfigDoesNotCount()
        {
            Directory.CreateDirectory(Path.Combine(_appFolder, "1.78.2.3702"));
            Directory.CreateDirectory(Path.Combine(_appFolder, "logs"));

            Assert.That(SettingsUpgrader.HasEarlierVersion(CurrentFile("1.78.2.3735")), Is.False);
        }

        [Test]
        public void AFirstInstallHasNoEarlierVersion()
        {
            Assert.That(SettingsUpgrader.HasEarlierVersion(CurrentFile("1.78.2.3735")), Is.False);
        }

        private string CurrentFile(string version) => Path.Combine(_appFolder, version, "user.config");

        private void WriteVersion(string version)
        {
            string folder = Path.Combine(_appFolder, version);
            Directory.CreateDirectory(folder);
            File.WriteAllText(Path.Combine(folder, "user.config"), "<configuration />");
        }
    }
}
