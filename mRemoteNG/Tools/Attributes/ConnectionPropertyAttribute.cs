using System;

namespace mRemoteNG.Tools.Attributes
{
    /// <summary>
    /// Marks a property in <see cref="Connection.AbstractConnectionRecord"/> as a connection property.
    /// This attribute drives reflection-based discovery of connection properties, eliminating
    /// the need to manually register new properties in multiple locations (serializers, UI, inheritance).
    ///
    /// Phase 1 (#1321): attribute + reflector + validation tests.
    /// Phase 2: serializers auto-discover properties via this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ConnectionPropertyAttribute : Attribute
    {
        /// <summary>
        /// Whether this property can be inherited from parent folders.
        /// When true, <see cref="Connection.ConnectionInfoInheritance"/> must have
        /// a matching bool property with the same name.
        /// Default: true.
        /// </summary>
        public bool Inheritable { get; set; } = true;

        /// <summary>
        /// Whether this property should be included in serialization (XML, SQL, CSV).
        /// Default: true.
        /// </summary>
        public bool Serializable { get; set; } = true;

        /// <summary>
        /// Whether this property contains a password/secret that requires encryption
        /// when serialized.
        /// Default: false.
        /// </summary>
        public bool IsEncrypted { get; set; }
    }
}
