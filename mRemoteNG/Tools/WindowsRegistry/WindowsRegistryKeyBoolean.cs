using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools.WindowsRegistry
{
    /// <summary>
    /// Represents a boolean Windows Registry key, extending the base class WindowsRegistryKey.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class WindowsRegistryKeyBoolean : WindowsRegistryKey
    {
        /// <summary>
        /// Gets or sets the default boolean value for a Windows Registry key.
        /// </summary>
        /// <remarks>
        /// The default value is initially set to `false`.
        /// </remarks>
        public bool DefaultValue { get; private set; } = false;

        /// <summary>
        /// Gets or sets the boolean value for a Windows Registry key.
        /// </summary>
        public new bool Value
        {
            get => BoolValue;
            private set
            {
                BoolValue = value;
                UpdateIsProvidedState();
            }
        }
        private bool BoolValue;

        /// <summary>
        /// Converts and initializes a WindowsRegistryKeyBoolean from a base WindowsRegistryKey, with an optional default value.
        /// </summary>
        /// <param name="baseKey">The base WindowsRegistryKey to convert from.</param>
        /// <param name="defaultValue">Optional default value to set for the WindowsRegistryKeyBoolean.</param>
        public void ConvertFromWindowsRegistryKey(WindowsRegistryKey baseKey, bool? defaultValue = null)
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

            ConvertToBool(baseKey.Value);
        }

        private void SetDefaultValue(bool? defaultValue)
        {
            if (defaultValue.HasValue)
                DefaultValue = (bool)defaultValue;
        }

        private void ConvertToBool(string newValue)
        {
            if (IsKeyPresent && bool.TryParse(newValue, out bool boolValue))
                BoolValue = boolValue;
            else if (IsKeyPresent && int.TryParse(newValue, out int intValue))
                BoolValue = intValue == 1;
            else
                BoolValue = DefaultValue;
        }
    }
}
