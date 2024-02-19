using Microsoft.Win32;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools.WindowsRegistry
{
    /// <summary>
    /// Represents a integer Windows Registry key, extending the base class WindowsRegistryKey.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class WindowsRegistryKeyInteger : WindowsRegistryKey
    {
        /// <summary>
        /// Gets or sets the default integer value for a Windows Registry key.
        /// </summary>
        /// <remarks>
        /// The default value is initially set to `-1`.
        /// </remarks>
        public int DefaultValue { get; private set; } = -1;


        /// <summary>
        /// Gets or sets the integer value for a Windows Registry key.
        /// </summary>
        public new int Value
        {
            get => IntegerValue;
            private set
            {
                IntegerValue = value;
                UpdateIsProvidedState();
            }
        }
        private int IntegerValue;

        /// <summary>
        /// Converts and initializes a WindowsRegistryKeyInteger from a base WindowsRegistryKey, with an optional default value.
        /// </summary>
        /// <param name="baseKey">The base WindowsRegistryKey to convert from.</param>
        /// <param name="defaultValue">Optional default value to set for the WindowsRegistryKeyBoolean.</param>
        public void ConvertFromWindowsRegistryKey(WindowsRegistryKey baseKey, int? defaultValue = null)
        {
            SetDefaultValue(defaultValue);
            FromBaseKey(baseKey);
        }

        private void FromBaseKey(WindowsRegistryKey baseKey)
        {
            if (baseKey == null)
                throw new ArgumentNullException(nameof(baseKey));

            Hive = baseKey.Hive;
            Path = baseKey.Path;
            Name = baseKey.Name;
            ValueKind = baseKey.ValueKind;
            IsKeyPresent = baseKey.IsKeyPresent;

            ConvertToInteger(baseKey.Value);
        }

        private void SetDefaultValue (int? defaultValue)
        {
            if (defaultValue.HasValue)
                DefaultValue = (int)defaultValue;
        }

        private void ConvertToInteger(string newValue)
        {
            if (ValueKind != RegistryValueKind.DWord)
                IsKeyPresent = false;

            if (IsKeyPresent && int.TryParse(newValue.ToString(), out int intValue))
                IntegerValue = intValue;
            else
                IntegerValue = DefaultValue;
        }
    }
}
