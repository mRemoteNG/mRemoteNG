using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Connection;
using mRemoteNGTests.TestHelpers;
using NUnit.Framework;

namespace mRemoteNGTests.Config.Serializers.ConnectionSerializers.Xml
{
    [TestFixture]
    public class XmlConnectionPresetSerializerTests
    {
        [Test]
        public void SerializeThenDeserializePreservesPresetValues()
        {
            ConnectionInfo sourceOne = ConnectionInfoHelpers.GetRandomizedConnectionInfo(randomizeInheritance: true);
            ConnectionInfo sourceTwo = ConnectionInfoHelpers.GetRandomizedConnectionInfo(randomizeInheritance: true);

            ConnectionPreset presetOne = ConnectionPreset.FromConnection("Preset One", sourceOne);
            ConnectionPreset presetTwo = ConnectionPreset.FromConnection("Preset Two", sourceTwo);

            XmlConnectionPresetSerializer serializer = new();
            XmlConnectionPresetDeserializer deserializer = new();

            string xml = serializer.Serialize(new[] { presetOne, presetTwo });
            IReadOnlyList<ConnectionPreset> deserializedPresets = deserializer.Deserialize(xml);

            Assert.That(deserializedPresets.Count, Is.EqualTo(2));

            ConnectionPreset deserializedOne = deserializedPresets.Single(preset => string.Equals(preset.Name, "Preset One", StringComparison.Ordinal));
            ConnectionPreset deserializedTwo = deserializedPresets.Single(preset => string.Equals(preset.Name, "Preset Two", StringComparison.Ordinal));

            AssertPresetMatchesConnection(deserializedOne, sourceOne);
            AssertPresetMatchesConnection(deserializedTwo, sourceTwo);
        }

        [Test]
        public void DeserializeEmptyStringReturnsEmptyCollection()
        {
            XmlConnectionPresetDeserializer deserializer = new();
            IReadOnlyList<ConnectionPreset> deserializedPresets = deserializer.Deserialize(string.Empty);

            Assert.That(deserializedPresets, Is.Empty);
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
                    $"Connection property '{property.Name}' did not round-trip correctly.");
            }

            foreach (PropertyInfo property in ConnectionPreset.ConfigurableInheritanceProperties)
            {
                object? expectedValue = property.GetValue(sourceConnection.Inheritance, null);
                object? actualValue = property.GetValue(preset.Inheritance, null);
                Assert.That(
                    actualValue,
                    Is.EqualTo(expectedValue),
                    $"Inheritance property '{property.Name}' did not round-trip correctly.");
            }
        }
    }
}
