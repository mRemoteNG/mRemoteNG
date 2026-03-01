using mRemoteNG.App;
using mRemoteNG.Properties;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace mRemoteNGTests.App
{
    [NonParallelizable]
    public class CommandLineParserTests
    {
        private const string TestEnvironmentVariableName = "MREMOTE_CMDLINE_PARSER_TEST_PATH";

        private string _originalConnectionFilePath = string.Empty;
        private bool _originalLoadConsFromCustomLocation;
        private string _originalBackupLocation = string.Empty;
        private string _originalCustomConfigurationPath = string.Empty;
        private bool _originalLogToApplicationDirectory;
        private string _originalLogFilePath = string.Empty;
        private string? _originalEnvironmentVariableValue;
        private readonly List<string> _temporaryDirectories = [];

        [SetUp]
        public void SetUp()
        {
            _originalConnectionFilePath = OptionsConnectionsPage.Default.ConnectionFilePath;
            _originalLoadConsFromCustomLocation = OptionsBackupPage.Default.LoadConsFromCustomLocation;
            _originalBackupLocation = OptionsBackupPage.Default.BackupLocation;
            _originalCustomConfigurationPath = Settings.Default.CustomConfigurationPath;
            _originalLogToApplicationDirectory = OptionsNotificationsPage.Default.LogToApplicationDirectory;
            _originalLogFilePath = OptionsNotificationsPage.Default.LogFilePath;
            _originalEnvironmentVariableValue = Environment.GetEnvironmentVariable(TestEnvironmentVariableName);
        }

        [TearDown]
        public void TearDown()
        {
            OptionsConnectionsPage.Default.ConnectionFilePath = _originalConnectionFilePath;
            OptionsBackupPage.Default.LoadConsFromCustomLocation = _originalLoadConsFromCustomLocation;
            OptionsBackupPage.Default.BackupLocation = _originalBackupLocation;
            Settings.Default.CustomConfigurationPath = _originalCustomConfigurationPath;
            OptionsNotificationsPage.Default.LogToApplicationDirectory = _originalLogToApplicationDirectory;
            OptionsNotificationsPage.Default.LogFilePath = _originalLogFilePath;
            Environment.SetEnvironmentVariable(TestEnvironmentVariableName, _originalEnvironmentVariableValue);

            foreach (string tempDirectory in _temporaryDirectories)
            {
                try
                {
                    if (Directory.Exists(tempDirectory))
                        Directory.Delete(tempDirectory, true);
                }
                catch
                {
                    // Cleanup is best effort only.
                }
            }
        }

        [Test]
        public void ApplySwitches_SetsConnectionFilePath_WhenConsContainsEnvironmentVariable()
        {
            string tempDirectory = CreateTempDirectory();
            string connectionsFilePath = Path.Combine(tempDirectory, "confCons.xml");
            File.WriteAllText(connectionsFilePath, "<connections />");
            Environment.SetEnvironmentVariable(TestEnvironmentVariableName, tempDirectory);

            CommandLineParser parser = new(["--cons", $"%{TestEnvironmentVariableName}%\\confCons.xml"]);

            parser.ApplySwitches();

            Assert.That(OptionsConnectionsPage.Default.ConnectionFilePath, Is.EqualTo(Path.GetFullPath(connectionsFilePath)));
            Assert.That(OptionsBackupPage.Default.LoadConsFromCustomLocation, Is.True);
            Assert.That(OptionsBackupPage.Default.BackupLocation, Is.EqualTo(Path.GetFullPath(connectionsFilePath)));
        }

        [Test]
        public void ApplySwitches_SetsCustomConfigurationPath_WhenSettingsSwitchIsProvided()
        {
            string tempDirectory = CreateTempDirectory();
            Environment.SetEnvironmentVariable(TestEnvironmentVariableName, tempDirectory);

            CommandLineParser parser = new(["/settings", $"%{TestEnvironmentVariableName}%"]);

            parser.ApplySwitches();

            Assert.That(Settings.Default.CustomConfigurationPath, Is.EqualTo(Path.GetFullPath(tempDirectory)));
        }

        [Test]
        public void ApplySwitches_SetsLogFilePath_WhenLogSwitchIsProvided()
        {
            string tempDirectory = CreateTempDirectory();
            Environment.SetEnvironmentVariable(TestEnvironmentVariableName, tempDirectory);
            string expectedLogFilePath = Path.GetFullPath(Path.Combine(tempDirectory, "mRemoteNG.log"));

            CommandLineParser parser = new(["--log", $"%{TestEnvironmentVariableName}%\\mRemoteNG.log"]);

            parser.ApplySwitches();

            Assert.That(OptionsNotificationsPage.Default.LogToApplicationDirectory, Is.False);
            Assert.That(OptionsNotificationsPage.Default.LogFilePath, Is.EqualTo(expectedLogFilePath));
        }

        [Test]
        public void GetNormalizedArguments_ExpandsEnvironmentVariablesForConfiguredPathSwitches()
        {
            string tempDirectory = CreateTempDirectory();
            Environment.SetEnvironmentVariable(TestEnvironmentVariableName, tempDirectory);

            string[] inputArgs =
            [
                "--connect", "ConnA",
                "--cons", $"%{TestEnvironmentVariableName}%\\confCons.xml",
                "/settings:%" + TestEnvironmentVariableName + "%",
                "--log=%" + TestEnvironmentVariableName + "%\\mRemoteNG.log"
            ];

            CommandLineParser parser = new(inputArgs);

            string[] normalizedArgs = parser.GetNormalizedArguments();

            Assert.That(normalizedArgs[0], Is.EqualTo("--connect"));
            Assert.That(normalizedArgs[1], Is.EqualTo("ConnA"));
            Assert.That(normalizedArgs[2], Is.EqualTo("--cons"));
            Assert.That(normalizedArgs[3], Is.EqualTo(Path.Combine(tempDirectory, "confCons.xml")));
            Assert.That(normalizedArgs[4], Is.EqualTo($"/settings:{tempDirectory}"));
            Assert.That(normalizedArgs[5], Is.EqualTo($"--log={Path.Combine(tempDirectory, "mRemoteNG.log")}"));
        }

        private string CreateTempDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), "mRemoteNG_CommandLineParserTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);
            _temporaryDirectories.Add(tempDirectory);
            return tempDirectory;
        }
    }
}
