using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Represents an entry in the Windows Registry.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the functionality needed to interact with Windows Registry entries, 
    /// including reading and writing values. It provides a comprehensive set of methods to handle 
    /// different value types, ensuring flexibility and ease of use.
    /// 
    /// Key features include:
    /// - Reading values from a specified registry path and name.
    /// - Writing values to a specified registry path and name.
    /// - Support for multiple data types such as strings, integers, and binary data.
    /// - Ability to specify the registry hive (e.g., HKEY_LOCAL_MACHINE, HKEY_CURRENT_USER).
    /// 
    /// This class is designed to simplify the manipulation of registry entries by providing a 
    /// straightforward interface for common registry operations.
    /// 
    /// Example usage:
    /// <code>
    /// var registryEntry = new RegistryEntry<string>(RegistryHive.LocalMachine, @"Software\MyApp", "Settings");
    /// var value = registryEntry.Read();
    /// if (value != **)
    ///     registryEntry.Write("newVal");
    /// </code>
    /// 
    /// <code>
    /// var registryEntry = new RegistryEntry<int>(RegistryHive.LocalMachine, @"Software\MyApp", "Settings").Read();
    /// </code>
    /// 
    /// <code>
    /// var registryEntry = new RegistryEntry<long>(RegistryHive.LocalMachine, @"Software\MyApp", "Settings").SetValidation(min, max).Read();
    /// if (registryEntry.IsValid())
    ///     Do Something
    /// </code>
    /// 
    /// </remarks>
    public class WinRegistryEntry<T>
    {
        #region Registry Fileds & Properties

        /// <summary>
        /// Represents the registry hive associated with the registry key.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="RegistryHive.CurrentUser"/>.
        /// </remarks>
        public RegistryHive Hive
        {
            get { return privateHive; }
            set
            {
                privateHive =
                    !Enum.IsDefined(typeof(RegistryHive), value) || value == RegistryHive.CurrentConfig || value == RegistryHive.ClassesRoot
                    ? throw new ArgumentException("Invalid parameter: Unknown or unsupported RegistryHive value.", nameof(Hive))
                    : value;
            }
        }
        private RegistryHive privateHive = RegistryHive.CurrentUser;

        /// <summary>
        /// Represents the path of the registry entry.
        /// </summary>
        public string Path
        {
            get { return privatePath; }
            set
            {
                privatePath =
                    !string.IsNullOrWhiteSpace(value)
                    ? value
                    : throw new ArgumentNullException(nameof(Path), "Invalid parameter: Path cannot be null, empty, or consist only of whitespace characters.");
            }
        }
        private string privatePath;

        /// <summary>
        /// Represents the name of the registry entry.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the kind of data stored in the registry value.
        /// </summary>
        public RegistryValueKind ValueKind { get; private set; } = InitialRegistryValueKind();

        /// <summary>
        /// Represents the value of the registry entry.
        /// </summary>
        public T Value
        {
            get { return privateValue; }
            set
            {
                privateValue = ValueValidationRules(value);
            }
        }
        private T privateValue;

        #endregion

        #region Aditional Fileds & Properties

        private T[] AllowedValues;
        private int? MinInt32Value;
        private int? MaxInt32Value;
        private long? MinInt64Value;
        private long? MaxInt64Value;
        private Type EnumType;

        /// <summary>
        /// Represents the raw value retrieved directly from the registry.
        /// </summary>
        private string RawValue;

        /// <summary>
        /// Represents the type of the generic parameter T.
        /// </summary>
        private readonly Type ElementType = typeof(T);

        /// <summary>
        /// Indicates whether the reading operation for the registry value was successful.
        /// </summary>
        private bool ReadOperationSucceeded;

        /// <summary>
        /// Indicates whether a lock operation should be performed after a successful read operation.
        /// </summary>
        private bool DoLock;

        /// <summary>
        /// Indicates whether the WinRegistryEntry is currently locked, preventing further read operations.
        /// </summary>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the entry has been explicitly set in the registry.
        /// This check is faster as it only verifies if a value was readed (set in registry).
        /// </summary>
        public bool IsSet => IsEntrySet();

        /// <summary>
        /// Gets a value indicating whether the entry's value is valid according to custom validation rules.
        /// This check includes whether the value has been set and whether it adheres to the defined validation criteria.
        /// </summary>
        public bool IsValid => CheckIsValid();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WinRegistryEntry{T}"/> class with default values.
        /// </summary>
        public WinRegistryEntry() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinRegistryEntry{T}"/> class for reading a default value from the specified registry hive and path.
        /// </summary>
        /// <param name="hive">The registry hive of the entry.</param>
        /// <param name="path">The path of the registry entry.</param>
        public WinRegistryEntry(RegistryHive hive, string path)
        {
            Hive = hive;
            Path = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinRegistryEntry{T}"/> class for writing a default value to the specified registry hive and path.
        /// </summary>
        /// <param name="hive">The registry hive of the entry.</param>
        /// <param name="path">The path of the registry entry.</param>
        /// <param name="value">The value of the registry entry.</param>
        public WinRegistryEntry(RegistryHive hive, string path, T value)
        {
            Hive = hive;
            Path = path;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinRegistryEntry{T}"/> class for reading a specific value from the specified registry hive, path, and name.
        /// </summary>
        /// <param name="hive">The registry hive of the entry.</param>
        /// <param name="path">The path of the registry entry.</param>
        /// <param name="name">The name of the registry entry.</param>
        public WinRegistryEntry(RegistryHive hive, string path, string name)
        {
            Hive = hive;
            Path = path;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinRegistryEntry{T}"/> class for writing a specific value to the specified registry hive, path, and name.
        /// </summary>
        /// <param name="hive">The registry hive of the entry.</param>
        /// <param name="path">The path of the registry entry.</param>
        /// <param name="name">The name of the registry entry.</param>
        /// <param name="value">The value of the registry entry.</param>
        public WinRegistryEntry(RegistryHive hive, string path, string name, T value)
        {
            Hive = hive;
            Path = path;
            Name = name;
            Value = value;
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Creates a new instance of the <see cref="WinRegistryEntry{T}"/> class for reading a default value from the specified registry hive and path.
        /// </summary>
        /// <param name="hive">The registry hive of the entry.</param>
        /// <param name="path">The path of the registry entry.</param>
        /// <returns>A new instance of the <see cref="WinRegistryEntry{T}"/> class.</returns>
        public static WinRegistryEntry<T> New(RegistryHive hive, string path)
        {
            return new WinRegistryEntry<T>(hive, path);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WinRegistryEntry{T}"/> class for writing a value to the specified registry hive and path.
        /// </summary>
        /// <param name="hive">The registry hive of the entry.</param>
        /// <param name="path">The path of the registry entry.</param>
        /// <param name="value">The value of the registry entry.</param>
        /// <returns>A new instance of the <see cref="WinRegistryEntry{T}"/> class.</returns>
        public static WinRegistryEntry<T> New(RegistryHive hive, string path, T value)
        {
            return new WinRegistryEntry<T>(hive, path, value);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WinRegistryEntry{T}"/> class for reading a specific value from the specified registry hive, path, and name.
        /// </summary>
        /// <param name="hive">The registry hive of the entry.</param>
        /// <param name="path">The path of the registry entry.</param>
        /// <param name="name">The name of the registry entry.</param>
        /// <returns>A new instance of the <see cref="WinRegistryEntry{T}"/> class.</returns>
        public static WinRegistryEntry<T> New(RegistryHive hive, string path, string name)
        {
            return new WinRegistryEntry<T>(hive, path, name);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WinRegistryEntry{T}"/> class for writing a specific value to the specified registry hive, path, and name.
        /// </summary>
        /// <param name="hive">The registry hive of the entry.</param>
        /// <param name="path">The path of the registry entry.</param>
        /// <param name="name">The name of the registry entry.</param>
        /// <param name="value">The value of the registry entry.</param>
        /// <returns>A new instance of the <see cref="WinRegistryEntry{T}"/> class.</returns>
        public static WinRegistryEntry<T> New(RegistryHive hive, string path, string name, T value)
        {
            return new WinRegistryEntry<T>(hive, path, name, value);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the kind of the registry value, ensuring it is a valid and defined <see cref="RegistryValueKind"/>.
        /// </summary>
        /// <param name="valueKind">The registry value kind to set.</param>
        /// <returns>The current instance of <see cref="WinRegistryEntry{T}"/> to allow for method chaining.</returns>
        public WinRegistryEntry<T> SetValueKind(RegistryValueKind valueKind)
        {
            if (ValueKindValidationRule(valueKind))
                ValueKind = valueKind;
            else
                // Validation rule returned false, so the initial value will be used for the specified system type.
                // Nothing will be changed
                LogError("ValueKind is not valid and cannot be set."); 

            return this;
        }

        /// <summary>
        /// Sets the allowed values for validation, with an option for case sensitivity.
        /// </summary>
        /// <param name="allowedValues">The array of allowed values.</param>
        public WinRegistryEntry<T> SetValidation(T[] allowedValues)
        {
            ResetValidation();

            if (allowedValues != null && allowedValues.Length > 0)
            {
                AllowedValues = allowedValues;
            }

            if (ElementType == typeof(string) && privateValue != null)
                privateValue = EnforceStringInputValidity(privateValue);

            return this;
        }

        /// <summary>
        /// Sets up validation using an array of allowed integer values.
        /// </summary>
        /// <param name="allowedValues">The array of allowed integer values.</param>
        public WinRegistryEntry<T> SetValidation(int[] allowedValues)
        {
            T[] mappedValues = allowedValues?.Select(value => (T)(object)value).ToArray();
            return SetValidation(mappedValues);
        }

        /// <summary>
        /// Sets up validation using an array of allowed integer values.
        /// </summary>
        /// <param name="allowedValues">The array of allowed integer values.</param>
        public WinRegistryEntry<T> SetValidation(long[] allowedValues)
        {
            T[] mappedValues = allowedValues?.Select(value => (T)(object)value).ToArray();
            return SetValidation(mappedValues);
        }

        /// <summary>
        /// Sets up validation for a range of integer values.
        /// </summary>
        /// <param name="minValue">The minimum value of the range.</param>
        /// <param name="maxValue">The maximum value of the range.</param>
        public WinRegistryEntry<T> SetValidation(int minValue, int maxValue)
        {
            ValidateRange(minValue, maxValue);
            ResetValidation();

            MinInt32Value = minValue;
            MaxInt32Value = maxValue;

            return this;
        }

        /// <summary>
        /// Sets up validation for a range of integer values.
        /// </summary>
        /// <param name="minValue">The minimum value of the range.</param>
        /// <param name="maxValue">The maximum value of the range.</param>
        public WinRegistryEntry<T> SetValidation(long minValue, long maxValue)
        {
            ValidateRange(minValue, maxValue);
            ResetValidation();

            MinInt64Value = minValue;
            MaxInt64Value = maxValue;

            return this;
        }

        /// <summary>
        /// Sets up validation rules for a range of Int32, Int64 values.
        /// </summary>
        /// <param name="minValue">The minimum value of the range. "*" can be provided to indicate no minimum value.</param>
        /// <param name="maxValue">The maximum value of the range. "*" can be provided to indicate no maximum value.</param>
        /// <returns>The current instance of the WinRegistryEntry<T> class.</returns>
        /// <exception cref="ArgumentException">Thrown when the registry entry type is not a valid Int32 or Int64.</exception>
        /// <exception cref="ArgumentException">Thrown when an invalid minimum value is provided for Int32 or Int64.</exception>
        /// <exception cref="ArgumentException">Thrown when an invalid maximum value is provided for Int32 or Int64.</exception>

        public WinRegistryEntry<T> SetValidation(string minValue, string maxValue)
        {
            if (string.IsNullOrEmpty(minValue) || minValue == "*")
                minValue = "0";
            if ((string.IsNullOrEmpty(maxValue) || maxValue == "*"))
                maxValue = (ElementType == typeof(int))
                    ? Int32.MaxValue.ToString()
                    : Int64.MaxValue.ToString();

            if (ElementType == typeof(int))
            {
                if (!int.TryParse(minValue, out int minIntValue))
                    throw new ArgumentException("Invalid minimum value for Int32.");
                if (!int.TryParse(maxValue, out int maxIntValue))
                    throw new ArgumentException("Invalid maximum value for Int32.");

                return SetValidation(minIntValue, maxIntValue);
            }
            else if (ElementType == typeof(long))
            {
                if (!long.TryParse(minValue, out long minLongValue))
                    throw new ArgumentException("Invalid minimum value for Int64.");
                if (!long.TryParse(maxValue, out long maxLongValue))
                    throw new ArgumentException("Invalid maximum value for Int64.");

                return SetValidation(minLongValue, maxLongValue);
            }
            else
            {
                throw new ArgumentException("Registry entry type must be either a valid Int32 or Int64 to use this validation.");
            }
        }

        /// <summary>
        /// Sets the validation to use an enumeration type
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        public WinRegistryEntry<T> SetValidation<TEnum>() where TEnum : Enum
        {
            ResetValidation();

            Type enumType = typeof(TEnum);
            if (enumType != null)
                EnumType = enumType;

            return this;
        }

        /// <summary>
        /// Checks if the Windows Registry key is ready for reading by ensuring that the hive,
        /// path, and name properties are set.
        /// </summary>
        /// <returns>True if the key is ready for reading, otherwise false.</returns>
        public bool IsReadable()
        {
            return IsHiveSet() && IsPathSet();
        }

        /// <summary>
        /// Checks if the Windows Registry key is ready for a write operation.
        /// The key is considered write-ready if none of the following conditions are met:
        /// - The hive is set
        /// - The registry value type is set
        /// - The key path is set
        /// </summary>
        /// <returns>Returns true if the key is write-ready, otherwise false.</returns>
        public bool IsWritable()
        {
            return IsHiveSet() && IsValueKindSet() && IsPathSet();
        }

        /// <summary>
        /// Reads the value of the registry entry from the specified registry path and assigns it to the Value property.
        /// </summary>
        /// <returns>The current instance of <see cref="WinRegistryEntry{T}"/> to allow for method chaining.</returns>
        public WinRegistryEntry<T> Read()
        {
            if (IsLocked)
                throw new InvalidOperationException("Operation denied: Cannot read registry entry again. Lock is enabled.");

            if (!IsReadable())
                throw new InvalidOperationException("Unable to read registry key. Hive, path, and name are required.");

            string rawValue = null;
            string name = string.IsNullOrEmpty(Name) ? null : Name;

            try
            {
                using var key = RegistryKey.OpenBaseKey(Hive, RegistryView.Default).OpenSubKey(Path);
                if (key != null)
                    RawValue = rawValue = key.GetValue(name)?.ToString();

                if (rawValue != null)
                    ValueKind = key.GetValueKind(name);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error reading the Windows Registry.", ex);
            }

            if (string.IsNullOrEmpty(rawValue))
            {
                // Issue in Windows registry: Value was null or empty.
                string logName = string.IsNullOrEmpty(Name) ? "Default Value" : Name;
                LogInfo($"Value for {logName} is Null or Empty");
            }
            else if (!ValueKindValidationRule(ValueKind))
            {
                // Issue in Windows registry: Value kind of the value cannot be parsed to type T.
                LogError($"Cannot parse a Value of type {ValueKind} to the specified type {typeof(T).FullName}.");
            }
            else
            {
                Value = ConvertValueBasedOnType(rawValue);
                ReadOperationSucceeded = true;

                if (DoLock)
                    IsLocked = true;
            }

            return this;
        }

        /// <summary>
        /// Writes the value of the registry entry to the specified registry path.
        /// </summary>
        /// <returns>The current instance of <see cref="WinRegistryEntry{T}"/> to allow for method chaining.</returns>
        public WinRegistryEntry<T> Write()
        {
            if (!IsWritable())
                throw new InvalidOperationException("Unable to write registry key. Hive, path, name, value kind, and value are required.");

            string name = string.IsNullOrEmpty(Name) ? null : Name;
            RegistryValueKind valueKind = string.IsNullOrEmpty(Name) ? RegistryValueKind.String : ValueKind;

            string value;
            if (typeof(T) == typeof(bool))
            {
                value = (bool)(object)Value
                    ? ValueKind == RegistryValueKind.DWord ? "1" : "True"
                    : ValueKind == RegistryValueKind.DWord ? "0" : "False";
            }
            else
            {
                value = Value.ToString();
            }

            try
            {
                using RegistryKey baseKey = RegistryKey.OpenBaseKey(Hive, RegistryView.Default);
                using RegistryKey registryKey = baseKey.CreateSubKey(Path, true);

                registryKey.SetValue(name, value, valueKind);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error writing to the Windows Registry.", ex);
            }

            return this;
        }

        /// <summary>
        /// Writes a new value to the registry entry.
        /// </summary>
        /// <param name="newValue">The new value to be written to the registry entry.</param>
        /// <returns>The current instance of <see cref="WinRegistryEntry{T}"/> to allow for method chaining.</returns>
        public WinRegistryEntry<T> Write(T newValue)
        {
            Value = newValue;
            return Write();
        }

        /// <summary>
        /// Clears the current values of the instance.
        /// </summary>
        /// <remarks>
        /// This method resets the values to their default states.
        /// After invoking this method, the "Validations" properties <c>IsSet</c> and <c>IsValid</c> will return <c>false</c>. 
        /// This is useful in scenarios where the value needs to be validated through alternative mechanisms.
        /// </remarks>
        public void Clear()
        {
            RawValue = null;
            Value = default;
            ReadOperationSucceeded = false;
        }

        /// <summary>
        /// Locks the WinRegistryEntry to prevent further read operations.
        /// If a read operation has already succeeded, the entry is immediately locked.
        /// If no read operation has been performed yet, a flag indicating the intention to lock after a successful read operation is set.
        /// </summary>
        /// <returns>The current instance of <see cref="WinRegistryEntry{T}"/> to allow for method chaining.</returns>
        public WinRegistryEntry<T> Lock()
        {
            if (ReadOperationSucceeded)
                IsLocked = true;
            else
                DoLock = true;

            return this;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts a string value to the specified .NET data type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target .NET data type to which the value is converted.</typeparam>
        /// <param name="originalValue">The string value to be converted.</param>
        /// <returns>The converted value of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when Conversion for <typeparamref name="T"/> failed..</exception>
        /// <exception cref="InvalidOperationException">Thrown when Conversion not supported for type <typeparamref name="T"/>.</exception>
        private T ConvertValueBasedOnType(string originalValue)
        {
            try
            {
                if (ElementType == typeof(string))
                {
                    return (T)(object)originalValue;
                }
                else if (ElementType == typeof(int))
                {
                    if (int.TryParse(originalValue, out int intValue))
                        return (T)(object)intValue;
                }
                else if (ElementType == typeof(long))
                {
                    if (long.TryParse(originalValue, out long longValue))
                        return (T)(object)longValue;
                }
                else if (ElementType == typeof(bool))
                {
                    if (bool.TryParse(originalValue, out bool boolValue))
                        return (T)(object)boolValue;
                    else if (int.TryParse(originalValue, out int intBool))
                        return (T)(object)(intBool == 1);
                }
            }
            catch
            {
                throw new InvalidOperationException($"Conversion for '{ElementType}' failed.");
            }
            throw new InvalidOperationException($"Conversion not supported for type '{ElementType}'.");
        }

        /// <summary>
        /// Logs an error message to the standard error stream.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        private static void LogError(string message)
        {
            Console.Error.WriteLine($"Error: {message}");
        }

        /// <summary>
        /// Logs an info message to the standard error stream.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        private static void LogInfo(string message)
        {
            Console.WriteLine($"Info: {message}");
        }

        /// <summary>
        /// Validates the provided value based on its type-specific rules.
        /// </summary>
        /// <param name="value">The value to be validated.</param>
        /// <exception cref="ArgumentException">Thrown when <typeparamref name="T"/> is bool and value is not True, False, 0 or 1.</exception>
        /// <exception cref="ArgumentException">Thrown when <typeparamref name="T"/> is int/long and value is negative.</exception>
        /// <exception cref="ArgumentException">Thrown when Value type <typeparamref name="T"/> is not supported.</exception>
        private T ValueValidationRules(T value)
        {
            // Boolean values are either string or DWORD. Mapping is needed to update ValueKind.
            var booleanRegistryValueKindMap = new Dictionary<string, RegistryValueKind>
            {
                { "true", RegistryValueKind.String },
                { "false", RegistryValueKind.String },
                { "0", RegistryValueKind.DWord },
                { "1", RegistryValueKind.DWord }
            };

            // For string elements, directly enforce validity and correct the input.
            if (ElementType == typeof(string))
            {
                return EnforceStringInputValidity(value);
            }
            // For boolean elements, check if the value is valid and convert it to the appropriate value kind.
            else if (ElementType == typeof(bool))
            {
                if (!booleanRegistryValueKindMap.TryGetValue(value.ToString().ToLower(), out var valueKind))
                    throw new ArgumentException("Invalid value. Supported values are ci strings 'True'/'False' or numbers '0'/'1'.", nameof(value));

                ValueKind = valueKind;
                return value;
            }
            // For integer or long elements, ensure the value is not negative.
            else if (ElementType == typeof(int) || ElementType == typeof(long))
            {
                if (Convert.ToInt64(value) < 0)
                    throw new ArgumentException("Value cannot be negative.", nameof(value));
                return value;
            }
            // For byte elements, ensure the value is not null or empty.
            else if (ElementType == typeof(byte))
            {
                if (value == null || ((byte[])(object)value).Length == 0)
                    throw new ArgumentException("Value cannot be null or empty.", nameof(value));
                return value;
            }

            // For unsupported element types, throw an ArgumentException.
            throw new ArgumentException($"Value type '{ElementType.FullName}' is not supported.", nameof(value));
        }

        /// <summary>
        /// Validates and corrects a string value based on a set of allowed values or enumeration values.
        /// </summary>
        /// <param name="value">The input value to be validated and potentially corrected.</param>
        /// <returns>The validated and potentially corrected value.</returns>
        private T EnforceStringInputValidity(T value)
        {
            if (AllowedValues != null)
            {
                T matchedValue = AllowedValues.FirstOrDefault(v => v.ToString().Equals(value.ToString(), StringComparison.OrdinalIgnoreCase));

                if (matchedValue != null)
                    // Correct the Value to ensure the correct spelling and avoid user typing mistakes
                    return matchedValue;
            }
            else if (EnumType != null && EnumType.IsEnum)
            {
                var matchedEnumValue = Enum.GetValues(EnumType)
                          .Cast<Enum>()
                          .FirstOrDefault(e => e.ToString().Equals(value.ToString(), StringComparison.OrdinalIgnoreCase));

                if (matchedEnumValue != null)
                    // Correct the Value to ensure the correct spelling and avoid user typing mistakes
                    return ConvertValueBasedOnType(matchedEnumValue.ToString());
            }

            return value;
        }

        /// <summary>
        /// Private method to validate the range values.
        /// </summary>
        /// <typeparam name="U">The type of the values being validated.</typeparam>
        /// <param name="minValue">The minimum value of the range.</param>
        /// <param name="maxValue">The maximum value of the range.</param>
        /// <param name="type">The type of registry entry (used for error messages).</param>
        private static void ValidateRange<U>(U minValue, U maxValue) where U : IComparable<U>
        {
            Type typeCode = typeof(U);

            string type =
                typeCode == typeof(int) ? "dword" :
                typeCode == typeof(long) ? "qword"
                : throw new ArgumentException("Registry entry type must be either Int32 or Int64 to use this validation.");

            if (minValue.CompareTo(default(U)) < 0)
                throw new ArgumentException($"Negative value not allowed for {type} parameter.", nameof(minValue));
            if (maxValue.CompareTo(default(U)) < 0)
                throw new ArgumentException($"Negative value not allowed for {type} parameter.", nameof(maxValue));
            if (minValue.CompareTo(maxValue) > 0)
                throw new ArgumentException("MinValue must be less than or equal to MaxValue.");
        }

        /// <summary>
        /// Validates the specified registry value kind.
        /// </summary>
        /// <param name="valueKind">The registry value kind to validate.</param>
        /// <returns>The validated <see cref="RegistryValueKind"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when Invalid parameter: Unknown or unsupported <typeparamref name="valueKind" value.</exception>
        /// <exception cref="ArgumentException">Thrown when Value type <typeparamref name="T"/> is not supported.</exception>
        private bool ValueKindValidationRule(RegistryValueKind valueKind)
        {
            if (!Enum.IsDefined(typeof(RegistryValueKind), valueKind) || valueKind == RegistryValueKind.Unknown || valueKind == RegistryValueKind.None || valueKind == 0)
                throw new ArgumentException("Invalid parameter: Unknown or unsupported RegistryValueKind value.", nameof(valueKind));

            return Type.GetTypeCode(ElementType) switch
            {
                TypeCode.Boolean => valueKind == RegistryValueKind.String || valueKind == RegistryValueKind.DWord,
                TypeCode.Int32 => valueKind == RegistryValueKind.DWord,
                TypeCode.Int64 => valueKind == RegistryValueKind.QWord,
                TypeCode.Byte => valueKind == RegistryValueKind.Binary,
                TypeCode.String => valueKind == RegistryValueKind.String || valueKind == RegistryValueKind.DWord || valueKind == RegistryValueKind.QWord, // Strings are compatible with most data types.
                _ => throw new ArgumentException($"Value type '{ElementType.FullName}' is not supported.")
            };
        }

        /// <summary>
        /// Determines the initial RegistryValueKind based on the type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>The initial RegistryValueKind determined by the type <typeparamref name="T"/>.</returns>
        private static RegistryValueKind InitialRegistryValueKind()
        {
            return Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.Int32 => RegistryValueKind.DWord,
                TypeCode.Int64 => RegistryValueKind.QWord,
                TypeCode.Boolean => RegistryValueKind.String,
                TypeCode.Byte => RegistryValueKind.Binary,
                _ => RegistryValueKind.String, // Default to String for unsupported types
            };
        }

        /// <summary>
        /// Determines whether the registry entry has been set.
        /// </summary>
        private bool IsEntrySet() => RawValue != null && ReadOperationSucceeded;

        /// <summary>
        /// Determines whether the registry hive has been explicitly set.
        /// </summary>
        /// <returns><c>true</c> if the hive is set; otherwise, <c>false</c>.</returns>
        private bool IsHiveSet() => Hive != 0;

        /// <summary>
        /// Determines whether the value kind of the registry entry has been explicitly set.
        /// </summary>
        /// <returns><c>true</c> if the value kind is set; otherwise, <c>false</c>.</returns>
        private bool IsValueKindSet() => ValueKind != 0;

        /// <summary>
        /// Determines whether the path of the registry entry has been explicitly set.
        /// </summary>
        /// <returns><c>true</c> if the path is set; otherwise, <c>false</c>.</returns>
        private bool IsPathSet() => Path != null;

        /// <summary>
        /// Checks if the current value is valid according to its type-specific rules and constraints.
        /// </summary>
        /// <returns>True if the value is valid; otherwise, false.</returns>
        private bool CheckIsValid()
        {
            if (!IsEntrySet()) return false;

            return Type.GetTypeCode(ElementType) switch
            {
                TypeCode.String => ValidateString(),
                TypeCode.Boolean => true,
                TypeCode.Int32 => ValidateInt32(),
                TypeCode.Int64 => ValidateInt64(),
                _ => throw new ArgumentException($"Value type '{ElementType.FullName}' is not supported."),
            };
        }

        /// <summary>
        /// Resets all validation setup for the entry to their default values.
        /// This includes clearing allowed values, resetting case sensitivity, setting numeric ranges and enum types to null.
        /// </summary>
        private void ResetValidation()
        {
            AllowedValues = null;

            MinInt32Value = null;
            MaxInt32Value = null;
            MinInt64Value = null;
            MaxInt64Value = null;

            EnumType = null;
        }

        /// <summary>
        /// Validates a string value based on allowed values or enumeration values.
        /// </summary>
        /// <returns>True if the string value is valid; otherwise, false.</returns>
        private bool ValidateString()
        {
            if (AllowedValues != null)
            {
                //return AllowedValues.FirstOrDefault(v => v.ToString().Equals(Value.ToString(), StringComparison.OrdinalIgnoreCase)) != null;
                return AllowedValues.Contains(Value);
            }
            else if (EnumType != null && EnumType.IsEnum)
            {
                return Enum.GetValues(EnumType)
                           .Cast<Enum>()
                           .Any(e => e.ToString().Equals(Value.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            return true;
        }

        /// <summary>
        /// Validates an integer value based on allowed values, minimum and maximum values, or enumeration values.
        /// </summary>
        /// <returns>True if the integer value is valid; otherwise, false.</returns>
        private bool ValidateInt32()
        {
            int value = (int)(object)Value;

            if (AllowedValues != null)
                return AllowedValues.Contains(Value);

            else if (MinInt32Value != null && MaxInt32Value != null)
                return value >= MinInt32Value && value <= MaxInt32Value;

            else if (EnumType != null && EnumType.IsEnum)
            {
                foreach (object enumValue in Enum.GetValues(EnumType))
                {
                    if (Convert.ToInt32(enumValue) == value)
                    {
                        return true;
                    }
                }

                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates a long integer value based on allowed values, minimum and maximum values, or enumeration values.
        /// </summary>
        /// <returns>True if the long integer value is valid; otherwise, false.</returns>
        private bool ValidateInt64()
        {
            long value = (long)(object)Value;

            if (AllowedValues != null)
                return AllowedValues.Contains(Value);

            else if (MinInt64Value != null && MaxInt64Value != null)
                return value >= MinInt64Value && value <= MaxInt64Value;

            return true;
        }

        #endregion
    }
}
