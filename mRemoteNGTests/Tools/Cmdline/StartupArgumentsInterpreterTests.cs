using System;
using System.IO;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Tools.Cmdline;
using NUnit.Framework;

namespace mRemoteNGTests.Tools.Cmdline
{
    public class StartupArgumentsInterpreterTests
    {
        private string _originalConnectionFilePath;
        private string _originalBackupLocation;
        private bool _originalLoadConsFromCustomLocation;
        private string _testDirectory;

        [SetUp]
        public void SetUp()
        {
            _originalConnectionFilePath = OptionsConnectionsPage.Default.ConnectionFilePath;
            _originalBackupLocation = OptionsBackupPage.Default.BackupLocation;
            _originalLoadConsFromCustomLocation = OptionsBackupPage.Default.LoadConsFromCustomLocation;
            _testDirectory = Path.Combine(Path.GetTempPath(), $"mRemoteNG command line {Guid.NewGuid()}");
            Directory.CreateDirectory(_testDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            OptionsConnectionsPage.Default.ConnectionFilePath = _originalConnectionFilePath;
            OptionsBackupPage.Default.BackupLocation = _originalBackupLocation;
            OptionsBackupPage.Default.LoadConsFromCustomLocation = _originalLoadConsFromCustomLocation;
            Directory.Delete(_testDirectory, true);
        }

        [Test]
        public void ParseArguments_CustomConnectionFile_OverridesPreviouslyConfiguredConnectionFile()
        {
            string customConnectionFile = Path.Combine(_testDirectory, "connections.xml");
            File.WriteAllText(customConnectionFile, string.Empty);
            OptionsConnectionsPage.Default.ConnectionFilePath = "previous-connections.xml";

            StartupArgumentsInterpreter interpreter = new(new MessageCollector());
            interpreter.ParseArguments(new[] { "mRemoteNG.exe", $"/c:\"{customConnectionFile}\"" });

            Assert.That(OptionsConnectionsPage.Default.ConnectionFilePath, Is.EqualTo(customConnectionFile));
            Assert.That(OptionsBackupPage.Default.BackupLocation, Is.EqualTo(customConnectionFile));
        }

        [Test]
        public void CmdArgumentsInterpreter_QuotedUncPathWithSpaces_IsPreserved()
        {
            const string uncPath = @"\\fileserver\Shared Folder\confCons.xml";

            CmdArgumentsInterpreter arguments = new(new[] { "mRemoteNG.exe", $"/c:\"{uncPath}\"" });

            Assert.That(arguments["c"], Is.EqualTo(uncPath));
        }
    }
}
