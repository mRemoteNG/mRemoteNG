namespace mRemoteNG.Security
{
    /// <summary>
    /// Legacy default values for the connection file format.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="LegacyEncryptionKey"/> is baked into the confCons.xml file
    /// format since the earliest versions of mRemoteNG. When a user has NOT set a
    /// custom master password, this key is used to encrypt/decrypt connection
    /// properties in the XML file.
    /// </para>
    /// <para>
    /// BACKWARD COMPATIBILITY: changing this value would make every existing
    /// connection file unreadable unless a migration path is implemented.
    /// The XML serializer writes "ThisIsNotProtected" (vs "ThisIsProtected") as a
    /// sentinel to distinguish files using this default key from those with a
    /// user-chosen password.
    /// </para>
    /// </remarks>
    public static class ConnectionFileDefaults
    {
        /// <summary>
        /// The legacy default encryption key used when no master password is set.
        /// </summary>
        public const string LegacyEncryptionKey = "mR3m";
    }
}
