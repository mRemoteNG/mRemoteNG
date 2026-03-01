using System;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    public class ConnectionPresetTests
    {
        [Test]
        public void CaptureAndApplyCopiesAllPresetProperties()
        {
            ConnectionInfo sourceConnection = ConnectionInfoHelpers.GetRandomizedConnectionInfo(randomizeInheritance: true);
            ConnectionInfo targetConnection = ConnectionInfoHelpers.GetRandomizedConnectionInfo(randomizeInheritance: true);

            ConnectionPreset preset = ConnectionPreset.FromConnection("Preset-A", sourceConnection);
            preset.ApplyTo(targetConnection);

            AssertConnectionPropertiesEqual(sourceConnection, targetConnection);
            AssertInheritancePropertiesEqual(sourceConnection.Inheritance, targetConnection.Inheritance);
        }

        [Test]
        public void CloneCreatesIndependentCopy()
        {
            ConnectionInfo sourceConnection = ConnectionInfoHelpers.GetRandomizedConnectionInfo(randomizeInheritance: true);
            ConnectionPreset preset = ConnectionPreset.FromConnection("Preset-A", sourceConnection);

            ConnectionPreset clone = preset.Clone();
            clone.ConnectionInfo.Username = Guid.NewGuid().ToString("N");
            clone.Inheritance.Domain = !clone.Inheritance.Domain;

            Assert.That(clone.ConnectionInfo.Username, Is.Not.EqualTo(preset.ConnectionInfo.Username));
            Assert.That(clone.Inheritance.Domain, Is.Not.EqualTo(preset.Inheritance.Domain));
        }

        private static void AssertConnectionPropertiesEqual(ConnectionInfo expected, ConnectionInfo actual)
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
        }

        private static void AssertInheritancePropertiesEqual(ConnectionInfoInheritance expected, ConnectionInfoInheritance actual)
        {
            foreach (PropertyInfo property in ConnectionPreset.ConfigurableInheritanceProperties)
            {
                object? expectedValue = property.GetValue(expected, null);
                object? actualValue = property.GetValue(actual, null);
                Assert.That(
                    actualValue,
                    Is.EqualTo(expectedValue),
                    $"Inheritance property '{property.Name}' did not match after applying preset.");
            }
        }
    }
}
