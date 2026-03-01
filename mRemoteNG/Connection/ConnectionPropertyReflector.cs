using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using mRemoteNG.Tools.Attributes;

namespace mRemoteNG.Connection
{
    /// <summary>
    /// Discovers connection properties via reflection, providing a single source of truth
    /// for which properties exist, which are inheritable, serializable, etc.
    /// This eliminates the need to manually maintain property lists across serializers,
    /// UI controls, and inheritance classes.
    ///
    /// Phase 1 (#1321): auto-discovery + validation.
    /// Phase 2: serializers use this to auto-serialize properties.
    /// </summary>
    public static class ConnectionPropertyReflector
    {
        private static readonly Lazy<IReadOnlyList<ConnectionPropertyDescriptor>> _allProperties =
            new(DiscoverProperties);

        /// <summary>
        /// Properties that are intentionally non-inheritable (no matching bool in
        /// <see cref="ConnectionInfoInheritance"/>). These are identity or infrastructure
        /// properties that should never be inherited from parent folders.
        /// </summary>
        private static readonly HashSet<string> _knownNonInheritable = new(StringComparer.Ordinal)
        {
            "Name",              // identity — each node has its own name
            "ConstantID",        // identity — unique per connection
            "CredentialId",      // internal — not shown in UI
            "IsTemplate",        // display — per-node toggle
        };

        /// <summary>
        /// Properties that are not included in standard serialization
        /// (they are serialized separately or are runtime-only).
        /// </summary>
        private static readonly HashSet<string> _knownNonSerializable = new(StringComparer.Ordinal)
        {
            "ConstantID",        // serialized as a special XML/SQL attribute, not as a regular property
        };

        /// <summary>
        /// Gets all discovered connection property descriptors from
        /// <see cref="AbstractConnectionRecord"/>.
        /// Results are cached after first call.
        /// </summary>
        public static IReadOnlyList<ConnectionPropertyDescriptor> GetAllProperties() => _allProperties.Value;

        /// <summary>
        /// Gets only properties that should be included in serialization.
        /// </summary>
        public static IEnumerable<ConnectionPropertyDescriptor> GetSerializableProperties()
            => GetAllProperties().Where(p => p.Serializable);

        /// <summary>
        /// Gets only properties that support inheritance from parent folders.
        /// </summary>
        public static IEnumerable<ConnectionPropertyDescriptor> GetInheritableProperties()
            => GetAllProperties().Where(p => p.Inheritable);

        /// <summary>
        /// Validates that every inheritable connection property has a matching bool property
        /// in <see cref="ConnectionInfoInheritance"/>. Returns a list of property names
        /// that are marked inheritable but have no matching inheritance toggle.
        /// An empty list means everything is in sync.
        /// </summary>
        public static IReadOnlyList<string> ValidateInheritanceSync()
        {
            var inheritanceType = typeof(ConnectionInfoInheritance);
            var missing = new List<string>();

            foreach (var prop in GetInheritableProperties())
            {
                var inheritanceProp = inheritanceType.GetProperty(prop.Name);
                if (inheritanceProp == null || inheritanceProp.PropertyType != typeof(bool))
                {
                    missing.Add(prop.Name);
                }
            }

            return missing;
        }

        /// <summary>
        /// Validates the reverse: finds bool properties in <see cref="ConnectionInfoInheritance"/>
        /// that have no matching connection property in <see cref="AbstractConnectionRecord"/>.
        /// These are orphaned inheritance toggles.
        /// </summary>
        public static IReadOnlyList<string> ValidateOrphanedInheritance()
        {
            var connectionType = typeof(AbstractConnectionRecord);
            var inheritanceType = typeof(ConnectionInfoInheritance);
            var exclusions = new HashSet<string>(StringComparer.Ordinal) { "EverythingInherited", "Parent", "InheritanceActive", "AutoSort" };

            var orphaned = new List<string>();

            foreach (var prop in inheritanceType.GetProperties())
            {
                if (prop.PropertyType != typeof(bool)) continue;
                if (exclusions.Contains(prop.Name)) continue;

                var connectionProp = connectionType.GetProperty(prop.Name);
                if (connectionProp == null)
                {
                    orphaned.Add(prop.Name);
                }
            }

            return orphaned;
        }

        private static IReadOnlyList<ConnectionPropertyDescriptor> DiscoverProperties()
        {
            var connectionType = typeof(AbstractConnectionRecord);
            var inheritanceType = typeof(ConnectionInfoInheritance);
            var result = new List<ConnectionPropertyDescriptor>();

            foreach (var prop in connectionType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Skip non-data properties (events, indexers)
                if (!prop.CanRead) continue;

                // Check for explicit [ConnectionProperty] attribute first
                var cpAttr = prop.GetCustomAttribute<ConnectionPropertyAttribute>();

                bool isBrowsable = prop.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false;
                bool isPassword = prop.GetCustomAttribute<PasswordPropertyTextAttribute>()?.Password == true;

                // Determine inheritability: explicit attribute > known non-inheritable > has matching bool
                bool inheritable;
                if (cpAttr != null)
                {
                    inheritable = cpAttr.Inheritable;
                }
                else if (_knownNonInheritable.Contains(prop.Name))
                {
                    inheritable = false;
                }
                else
                {
                    // Check if ConnectionInfoInheritance has a matching bool property
                    var inheritanceProp = inheritanceType.GetProperty(prop.Name);
                    inheritable = inheritanceProp != null && inheritanceProp.PropertyType == typeof(bool);
                }

                // Determine serializability: explicit attribute > known non-serializable > default true
                bool serializable;
                if (cpAttr != null)
                {
                    serializable = cpAttr.Serializable;
                }
                else if (_knownNonSerializable.Contains(prop.Name))
                {
                    serializable = false;
                }
                else
                {
                    // Read-only properties are not serializable by default
                    serializable = prop.CanWrite;
                }

                // Determine encryption: explicit attribute > PasswordPropertyText > default false
                bool isEncrypted = cpAttr?.IsEncrypted ?? isPassword;

                result.Add(new ConnectionPropertyDescriptor(
                    prop.Name,
                    prop.PropertyType,
                    prop,
                    inheritable,
                    serializable,
                    isEncrypted,
                    isBrowsable
                ));
            }

            return result.AsReadOnly();
        }
    }

    /// <summary>
    /// Describes a single connection property discovered via reflection.
    /// </summary>
    public sealed class ConnectionPropertyDescriptor
    {
        public string Name { get; }
        public Type PropertyType { get; }
        public PropertyInfo PropertyInfo { get; }
        public bool Inheritable { get; }
        public bool Serializable { get; }
        public bool IsEncrypted { get; }
        public bool IsBrowsable { get; }

        public ConnectionPropertyDescriptor(
            string name,
            Type propertyType,
            PropertyInfo propertyInfo,
            bool inheritable,
            bool serializable,
            bool isEncrypted,
            bool isBrowsable)
        {
            Name = name;
            PropertyType = propertyType;
            PropertyInfo = propertyInfo;
            Inheritable = inheritable;
            Serializable = serializable;
            IsEncrypted = isEncrypted;
            IsBrowsable = isBrowsable;
        }

        public override string ToString() => $"{Name} ({PropertyType.Name}, inheritable={Inheritable}, serializable={Serializable})";
    }
}
