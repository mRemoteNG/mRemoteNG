using System;
using System.IO;
using System.Linq;
using System.Reflection;
using mRemoteNG.Config;
using mRemoteNG.Connection;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config
{
    public class ConnectionPresetServiceTests
    {
        [Test]
        public void SavePresetPersistsAndCanBeReloaded()
        {
            string testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string presetFilePath = Path.Combine(testDirectory, "connectionPresets.xml");

            try
            {
                ConnectionInfo sourceConnection = ConnectionInfoHelpers.GetRandomizedConnectionInfo(randomizeInheritance: true);

                ConnectionPresetService service = new(presetFilePath);
                bool saveSucceeded = service.SavePreset("My Preset", sourceConnection);

                Assert.That(saveSucceeded, Is.True);
                Assert.That(File.Exists(presetFilePath), Is.True);

                ConnectionPresetService reloadedService = new(presetFilePath);
                ConnectionPreset reloadedPreset = reloadedService.GetPresets().Single(preset => string.Equals(preset.Name, "My Preset", StringComparison.Ordinal));

                AssertPresetMatchesConnection(reloadedPreset, sourceConnection);
            }
            finally
            {
                if (Directory.Exists(testDirectory))
                {
                    Directory.Delete(testDirectory, true);
                }
            }
        }

        [Test]
        public void ApplyPresetUpdatesTargetConnections()
        {
            string testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string presetFilePath = Path.Combine(testDirectory, "connectionPresets.xml");

            try
            {
                ConnectionInfo sourceConnection = ConnectionInfoHelpers.GetRandomizedConnectionInfo(randomizeInheritance: true);
                ConnectionInfo targetConnection = ConnectionInfoHelpers.GetRandomizedConnectionInfo(randomizeInheritance: true);

                ConnectionPresetService service = new(presetFilePath);
                service.SavePreset("My Preset", sourceConnection);

                bool applySucceeded = service.ApplyPreset("My Preset", new[] { targetConnection });

                Assert.That(applySucceeded, Is.True);
                AssertConnectionMatches(sourceConnection, targetConnection);
            }
            finally
            {
                if (Directory.Exists(testDirectory))
                {
                    Directory.Delete(testDirectory, true);
                }
            }
        }

        private static void AssertPresetMatchesConnection(ConnectionPreset preset, ConnectionInfo sourceConnection)
        {
            foreach (PropertyInfo property in ConnectionPreset.ConfigurableConnectionProperties)
            {
                object? expectedValue = property.GetValue(sourceConnection, null);
                object? actualValue = property.GetValue(preset.ConnectionInfo, null);
                Assert.That(
                    actualValue,
                    Is.EqualTo(expectedValue),
                    $"Connection property '{property.Name}' did not persist correctly.");
            }

            foreach (PropertyInfo property in ConnectionPreset.ConfigurableInheritanceProperties)
            {
                object? expectedValue = property.GetValue(sourceConnection.Inheritance, null);
                object? actualValue = property.GetValue(preset.Inheritance, null);
                Assert.That(
                    actualValue,
                    Is.EqualTo(expectedValue),
                    $"Inheritance property '{property.Name}' did not persist correctly.");
            }
        }

        private static void AssertConnectionMatches(ConnectionInfo expected, ConnectionInfo actual)
        {
            foreach (PropertyInfo property in ConnectionPreset.ConfigurableConnectionProperties)
            {
                object? expectedValue = property.GetValue(expected, null);
                object? actualValue = property.GetValue(actual, null);
                Assert.That(
                    actualValue,
                    Is.EqualTo(expectedValue),
                    $"Connection property '{property.Name}' did not match after applying preset.");
            }

            foreach (PropertyInfo property in ConnectionPreset.ConfigurableInheritanceProperties)
            {
                object? expectedValue = property.GetValue(expected.Inheritance, null);
                object? actualValue = property.GetValue(actual.Inheritance, null);
                Assert.That(
                    actualValue,
                    Is.EqualTo(expectedValue),
                    $"Inheritance property '{property.Name}' did not match after applying preset.");
            }
        }
    }
}
