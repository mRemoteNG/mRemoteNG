using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Tools.Attributes;
using NUnit.Framework;

namespace mRemoteNGTests.Connection
{
    /// <summary>
    /// Validates the ConnectionPropertyReflector and ensures that the connection
    /// property system stays in sync across AbstractConnectionRecord and
    /// ConnectionInfoInheritance. These tests catch the #1 source of bugs when
    /// adding new connection properties: forgetting to add the matching
    /// inheritance toggle (issue #1321).
    /// </summary>
    public class ConnectionPropertyReflectorTests
    {
        [Test]
        public void DiscoversSomeProperties()
        {
            var properties = ConnectionPropertyReflector.GetAllProperties();
            Assert.That(properties.Count, Is.GreaterThan(50),
                "Reflector should discover 50+ connection properties from AbstractConnectionRecord");
        }

        [Test]
        public void InheritablePropertiesHaveMatchingInheritanceBooleans()
        {
            var missing = ConnectionPropertyReflector.ValidateInheritanceSync();
            Assert.That(missing, Is.Empty,
                $"These inheritable properties have no matching bool in ConnectionInfoInheritance: " +
                $"{string.Join(", ", missing)}. " +
                "Either add the bool to ConnectionInfoInheritance, or mark the property " +
                "[ConnectionProperty(Inheritable = false)] in AbstractConnectionRecord.");
        }

        [Test]
        public void NoOrphanedInheritanceBooleans()
        {
            var orphaned = ConnectionPropertyReflector.ValidateOrphanedInheritance();
            Assert.That(orphaned, Is.Empty,
                $"These inheritance booleans have no matching property in AbstractConnectionRecord: " +
                $"{string.Join(", ", orphaned)}. " +
                "Remove the orphaned bool from ConnectionInfoInheritance or add the matching property.");
        }

        [Test]
        public void PasswordPropertiesAreMarkedEncrypted()
        {
            var encrypted = ConnectionPropertyReflector.GetAllProperties()
                .Where(p => p.IsEncrypted)
                .Select(p => p.Name)
                .ToList();

            Assert.That(encrypted, Does.Contain("Password"));
            Assert.That(encrypted, Does.Contain("RDGatewayPassword"));
            Assert.That(encrypted, Does.Contain("VNCProxyPassword"));
        }

        [Test]
        public void ConstantIDIsNotSerializable()
        {
            var constantId = ConnectionPropertyReflector.GetAllProperties()
                .FirstOrDefault(p => string.Equals(p.Name, "ConstantID", StringComparison.Ordinal));

            Assert.That(constantId, Is.Not.Null);
            Assert.That(constantId!.Serializable, Is.False);
        }

        [Test]
        public void NameIsNotInheritable()
        {
            var name = ConnectionPropertyReflector.GetAllProperties()
                .FirstOrDefault(p => string.Equals(p.Name, "Name", StringComparison.Ordinal));

            Assert.That(name, Is.Not.Null);
            Assert.That(name!.Inheritable, Is.False);
        }

        [Test]
        public void HostnameIsInheritable()
        {
            var hostname = ConnectionPropertyReflector.GetAllProperties()
                .FirstOrDefault(p => string.Equals(p.Name, "Hostname", StringComparison.Ordinal));

            Assert.That(hostname, Is.Not.Null);
            Assert.That(hostname!.Inheritable, Is.True);
        }

        [Test]
        public void BrowsableFalsePropertiesAreDetected()
        {
            var nonBrowsable = ConnectionPropertyReflector.GetAllProperties()
                .Where(p => !p.IsBrowsable)
                .Select(p => p.Name)
                .ToList();

            Assert.That(nonBrowsable, Does.Contain("ConstantID"));
            Assert.That(nonBrowsable, Does.Contain("CredentialId"));
        }

        [Test]
        public void GetSerializablePropertiesExcludesNonSerializable()
        {
            var serializable = ConnectionPropertyReflector.GetSerializableProperties()
                .Select(p => p.Name)
                .ToList();

            Assert.That(serializable, Does.Not.Contain("ConstantID"));
        }

        [Test]
        public void GetInheritablePropertiesExcludesNonInheritable()
        {
            var inheritable = ConnectionPropertyReflector.GetInheritableProperties()
                .Select(p => p.Name)
                .ToList();

            Assert.That(inheritable, Does.Not.Contain("Name"));
            Assert.That(inheritable, Does.Not.Contain("ConstantID"));
        }

        [Test]
        public void AllInheritablePropertiesAreInBothClasses()
        {
            // For every property the reflector says is inheritable,
            // verify it exists in both AbstractConnectionRecord AND
            // ConnectionInfoInheritance.
            var inheritanceType = typeof(ConnectionInfoInheritance);
            var connectionType = typeof(AbstractConnectionRecord);

            foreach (var prop in ConnectionPropertyReflector.GetInheritableProperties())
            {
                Assert.That(connectionType.GetProperty(prop.Name), Is.Not.Null,
                    $"Inheritable property '{prop.Name}' missing from AbstractConnectionRecord");

                var inheritanceProp = inheritanceType.GetProperty(prop.Name);
                Assert.That(inheritanceProp, Is.Not.Null,
                    $"Inheritable property '{prop.Name}' has no matching bool in ConnectionInfoInheritance");
                Assert.That(inheritanceProp!.PropertyType, Is.EqualTo(typeof(bool)),
                    $"Inheritance property '{prop.Name}' should be bool but is {inheritanceProp.PropertyType.Name}");
            }
        }

        [Test]
        public void DescriptorToStringIsReadable()
        {
            var first = ConnectionPropertyReflector.GetAllProperties().First();
            var str = first.ToString();
            Assert.That(str, Does.Contain(first.Name));
        }
    }
}
