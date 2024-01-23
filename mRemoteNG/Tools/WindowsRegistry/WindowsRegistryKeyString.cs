using Microsoft.Win32;
using System;
using System.Linq;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools.WindowsRegistry
{
    /// <summary>
    /// Represents a string Windows Registry key, extending the base class WindowsRegistryKey, can be evaluated with a value set.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class WindowsRegistryKeyString : WindowsRegistryKey
    {
        /// <summary>
        /// Gets or sets the default integer value for a Windows Registry key.
        /// </summary>
        public string DefaultValue { get; private set; }

        /// <summary>
        /// Gets or sets an array of allowed values for validation.
        /// </summary>
        public string[] AllowedValues { get; set; }

        /// <summary>
        /// Gets or sets a boolean flag indicating whether validation is case-sensitive.
        /// </summary>
        public bool IsCaseSensitiveValidation { get; set; } = false;

        /// <summary>
        /// Gets a boolean indicating whether the value is valid based on the validation rules.
        /// </summary>
        public bool IsValid { get; private set; } = false;


        public new string Value
        {
            get => StringValue;
            private set
            {
                StringValue = value;
                UpdateIsProvidedState();
                Validate();
            }
        }
        private string StringValue;

        /// <summary>
        /// Converts and initializes a WindowsRegistryKeyString from a base WindowsRegistryKey, with an optional default value.
        /// </summary>
        /// <param name="baseKey">The base WindowsRegistryKey to convert from.</param>
        /// <param name="defaultValue">Optional default value to set for the WindowsRegistryKeyBoolean.</param>
        public void ConvertFromWindowsRegistryKey(WindowsRegistryKey baseKey, string defaultValue = null)
        {
            SetDefaultValue(defaultValue);
            FromBaseKey(baseKey);
            Validate();
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

            ConvertToString(baseKey.Value);
        }

        private void SetDefaultValue (string defaultValue)
        {
            DefaultValue = defaultValue;
        }
        private void ConvertToString(string newValue)
        {
            if (IsKeyPresent && newValue != null)
                StringValue = newValue;
            else
                StringValue = DefaultValue;
        }

        /// <summary>
        /// Validates a Windows Registry key value against a set of allowed values, considering case sensitivity.
        /// </summary>
        /// <param name="allowedValues">Array of allowed values.</param>
        /// <param name="caseSensitive">Optional parameter to specify case sensitivity in validation.</param>
        public void Validate(string[] allowedValues = null, bool? caseSensitive = null)
        {
            // Key must be present to evaluate
            if (!IsKeyPresent)
                return;

            if (caseSensitive.HasValue)
                IsCaseSensitiveValidation = caseSensitive.Value;

            if (allowedValues != null && allowedValues.Length >= 1)
                AllowedValues = allowedValues;

            // AllowedValues array cannot be null or empty.
            if (AllowedValues == null || AllowedValues.Length == 0 || !IsKeyPresent)
                return;

            if (IsKeyPresent && AllowedValues.Any(v =>
               IsCaseSensitiveValidation ? v == Value : v.Equals(Value, StringComparison.OrdinalIgnoreCase)))
            {
                // Set to true when the value is found in the valid values
                IsValid = true; 
            }
            else
            {
                // Set to false when the value is not found in the valid values
                IsValid = false;
                StringValue = DefaultValue;
            }
        }

    }
}
