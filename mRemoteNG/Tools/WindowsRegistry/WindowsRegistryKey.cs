using System;
using System.Runtime.Versioning;
using Microsoft.Win32;


namespace mRemoteNG.Tools.WindowsRegistry
{
    /// <summary>
    /// Represents a Windows Registry key with a default string value, providing a flexible abstraction for registry operations.
    /// This class can be extended by inherited classes to customize behavior for specific data types.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class WindowsRegistryKey
    {
        #region properties

        #region Property registryHive
        public RegistryHive Hive
        {
            get { return _Hive; }
            set
            {
                if (value == 0)
                    throw new ArgumentNullException("RegistryHive unknown.");

                _Hive = value;
            }
        }
        private RegistryHive _Hive { get; set; }
        #endregion

        #region Property path
        public string Path
        {
            get { return _Path; }
            set
            {
                value.ThrowIfNull(nameof(value));
                _Path = value;
            }
        }
        private string _Path  { get; set; }
        #endregion

        #region Property name
        public string Name
        {
            get { return _Name; }
            set
            {
                value.ThrowIfNull(nameof(value));
                _Name = value;
            }
        }
        private string _Name { get; set; }
        #endregion

        #region Property value
        public virtual string Value
        {
            get => _value;
            set
            {
                _value = value;
                UpdateIsProvidedState();
            }
        }
        private string _value;
        #endregion

        public RegistryValueKind ValueKind { get; set; } = RegistryValueKind.Unknown;

        public bool IsKeyPresent { get; set; } = false;
        #endregion

        #region public methods
        /// <summary>
        /// Checks if the Windows Registry key is ready for reading by ensuring that the hive,
        /// path, and name properties are set.
        /// </summary>
        /// <returns>True if the key is ready for reading, otherwise false.</returns>
        public bool IsKeyReadable() {
            return (IsHiveSet() && IsPathSet() && IsNameSet());
        }

        /// <summary>
        /// Checks if the Windows Registry key is ready for a write operation.
        /// The key is considered write-ready if none of the following conditions are met:
        /// - The hive is set
        /// - The registry value type is set
        /// - The key path is set
        /// - The value name is set
        /// </summary>
        /// <returns>Returns true if the key is write-ready, otherwise false.</returns>
        public bool IsKeyWritable() {
            return (IsHiveSet() && IsValueKindSet() && IsPathSet() && IsNameSet());
        }
        #endregion

        #region protected methods
        protected void UpdateIsProvidedState()
        {
            // Key is present when RegistryKey value is not null
            IsKeyPresent = Value != null;
        }

        protected bool IsHiveSet() => Hive != 0;

        protected bool IsValueKindSet() => ValueKind != 0;

        protected bool IsPathSet() => Path != null;

        protected bool IsNameSet() => Name != null;
        #endregion
    }
}
