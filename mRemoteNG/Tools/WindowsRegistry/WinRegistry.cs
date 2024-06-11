using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Provides functionality to interact with the Windows Registry, including reading, writing, and managing registry entries.
    /// </summary>
    /// <remarks>
    /// This class encapsulates common functionality for working with the Windows Registry. It offers methods to open registry keys, create subkeys, and read or write values.
    /// The class serves as a base for specialized registry entry classes, centralizing registry operations and validation checks.
    /// Users can create instances of this class to perform various registry operations, such as retrieving subkey names, reading values of different types, creating keys, and deleting registry entries or keys.
    /// Additionally, the class includes methods for converting between different registry value types and handling custom validation rules.
    /// 
    /// License:
    /// This class is licensed under MIT License.
    /// </remarks>
    public class WinRegistry : IRegistry, IRegistryRead
    {
        #region Public Read Method: GetSubKeyNames

        /// <summary>
        /// Retrieves the names of subkeys under a specified registry key path.
        /// </summary>
        /// <param name="Hive">The RegistryHive where the subkeys are located.</param>
        /// <param name="Path">The path to the registry key containing the subkeys.</param>
        /// <returns>An array of strings containing the names of subkeys, or an empty array if no subkeys are found.</returns>
        public string[] GetSubKeyNames(RegistryHive Hive, string Path)
        {
            ThrowIfHiveInvalid(Hive);
            ThrowIfPathInvalid(Path);

            using var key = RegistryKey.OpenBaseKey(Hive, RegistryView.Default).OpenSubKey(Path);
            if (key != null)
                return key.GetSubKeyNames();
            else
                return Array.Empty<string>();
        }

        #endregion

        #region Public Read Value Method 

        /// <summary>
        /// Retrieves the data value associated with the specified registry key and value name.
        /// </summary>
        /// <param name="Hive">The registry hive.</param>
        /// <param name="Path">The registry key path.</param>
        /// <param name="Name">The name of the value. Null or Empty to get default.</param>
        /// <returns>The value data as a string, or null if the value is not found.</returns>
        /// <exception cref = "ArgumentException" > Thrown when the specified registry hive, path or name is invalid</exception>
        public string GetValue(RegistryHive Hive, string Path, string Name)
        {
            ThrowIfHiveInvalid(Hive);
            ThrowIfPathInvalid(Path);

            using var key = RegistryKey.OpenBaseKey(Hive, RegistryView.Default).OpenSubKey(Path);
            if (key == null)
                return null;

            // Ensure name is null when null or empty to get defaults value
            if (string.IsNullOrEmpty(Name))
                Name = null;

            return key.GetValue(Name)?.ToString();
        }

        /// <summary>
        /// Retrieves the data value associated with the specified registry key and uses the default value if the value name is not specified.
        /// </summary>
        /// <param name="Hive">The registry hive.</param>
        /// <param name="Path">The registry key path.</param>
        /// <returns>The value data as a string, or null if the value is not found.</returns>
        public string GetDefaultValue(RegistryHive Hive, string Path)
        {
            return GetValue(Hive, Path, null);
        }

        /// <summary>
        /// Retrieves the string value from the specified REG_SZ registry key.
        /// </summary>
        /// <param name="Hive">The registry hive.</param>
        /// <param name="Path">The registry key path.</param>
        /// <param name="Name">The name of the value. Null or Empty to get default.</param>
        /// <param name="DefaultValue">The default value to return if the property is not found.</param>
        /// <returns>The value data as string, or the specified default value if the value is not found.</returns>
        public string GetStringValue(RegistryHive Hive, string Path, string Name, string DefaultValue = null)
        {
            string value = GetValue(Hive, Path, Name);
            return value ?? DefaultValue;
        }

        /// <summary>
        /// Retrieves the bool value from from the specified REG_SZ or REG_DWORD registry key.
        /// </summary>
        /// <param name="Hive">The registry hive.</param>
        /// <param name="Path">The registry key path.</param>
        /// <param name="Name">The name of the value.</param>
        /// <param name="DefaultValue">The default value to return if the property is not found or cannot be parsed. (Default = false)</param>
        /// <returns>The value data as bool, parsed from its REG_SZ or REG_DWORD representation if possible, or the specified default value if the value is not found or cannot be parsed.</returns>
        public bool GetBoolValue(RegistryHive Hive, string Path, string Name, bool DefaultValue = false)
        {
            string value = GetValue(Hive, Path, Name);

            if (!string.IsNullOrEmpty(value))
            {
                if (int.TryParse(value, out int intValue))
                    return intValue == 1;
                if (bool.TryParse(value, out bool boolValue))
                    return boolValue;
            }

            return DefaultValue;
        }

        /// <summary>
        /// Retrieves the integer value from from the specified REG_DWORD registry key.
        /// </summary>
        /// <param name="Hive">The registry hive.</param>
        /// <param name="Path">The registry key path.</param>
        /// <param name="Name">The name of the value.</param>
        /// <param name="DefaultValue">The default value to return if the property is not found or cannot be parsed. (Default = 0)</param>
        /// <returns>The value data as integer, parsed from its REG_DWORD representation if possible, or the specified default value if the value is not found or cannot be parsed.</returns>
        public int GetIntegerValue(RegistryHive Hive, string Path, string Name, int DefaultValue = 0)
        {
            string value = GetValue(Hive, Path, Name);

            if (int.TryParse(value, out int intValue))
                return intValue;

            return DefaultValue;
        }

        #endregion

        #region Public Read Tree Methods

        /// <summary>
        /// Retrieves a windows registry entry for a specific registry hive, path, and value name.
        /// </summary>
        /// <param name="hive">The RegistryHive of the key.</param>
        /// <param name="path">The path of the key.</param>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <returns>A WinRegistryEntry<string> object representing the specified registry key and value.</returns>
        public WinRegistryEntry<string> GetEntry(RegistryHive hive, string path, string name)
        {
            return WinRegistryEntry<string>
                    .New(hive, path, name)
                    .Read();
        }

        /// <summary>
        /// Retrieves a list of registry entries and their values under a given key path.
        /// </summary>
        /// <param name="hive">The registry hive.</param>
        /// <param name="path">The registry key path.</param>
        /// <returns>A list of WinRegistryEntry<string> objects, each representing a value within the specified registry key path.</returns>
        public List<WinRegistryEntry<string>> GetEntries(RegistryHive hive, string path)
        {
            using var key = RegistryKey.OpenBaseKey(hive, RegistryView.Default).OpenSubKey(path);
            if (key == null)
                return new List<WinRegistryEntry<string>>(); // Return an empty list when no key is found

            return key.GetValueNames()
                .Select(name => WinRegistryEntry<string>.New(hive, path, name)
                    .Read())
                .ToList();
        }

        /// <summary>
        /// Recursively retrieves registry entries under a given key path and its subkeys.
        /// </summary>
        /// <param name="hive">The registry hive.</param>
        /// <param name="path">The registry key path.</param>
        /// <returns>A list of WinRegistryEntry<string> objects, each representing a value within the specified registry key path.</returns>
        public List<WinRegistryEntry<string>> GetEntriesRecursive(RegistryHive hive, string path)
        {
            List<WinRegistryEntry<string>> list = new();
            using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
            using (var key = baseKey.OpenSubKey(path))
            {
                if (key != null)
                {
                    foreach (string subPathName in key.GetSubKeyNames())
                    {
                        string subKey = $"{path}\\{subPathName}";
                        list.AddRange(GetEntriesRecursive(hive, subKey));
                    }
                }
            }

            list.AddRange(GetEntries(hive, path));
            return list;
        }
        #endregion

        #region Public Write Methods

        /// <summary>
        /// Sets the value of a specific property within a registry key using individual parameters.
        /// </summary>
        /// <param name="Hive">The registry hive.</param>
        /// <param name="Path">The registry key path.</param>
        /// <param name="Name">The name of the value.</param>
        /// <param name="Value">The value to set for the property.</param>
        /// <param name="ValueKind">The data type of the value to set.</param>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while writing to the Windows Registry key.</exception>
        public void SetValue(RegistryHive Hive, string Path, string Name, object Value, RegistryValueKind ValueKind)
        {
            ThrowIfHiveInvalid(Hive);
            ThrowIfPathInvalid(Path);

            string name = string.IsNullOrEmpty(Name) ? null : Name;
            RegistryValueKind valueKind = string.IsNullOrEmpty(Name) ? RegistryValueKind.String : ValueKind;

            ThrowIfValueKindInvalid(valueKind);

            try
            {
                using RegistryKey baseKey = RegistryKey.OpenBaseKey(Hive, RegistryView.Default);
                using RegistryKey registryKey = baseKey.CreateSubKey(Path, true);

                registryKey.SetValue(name, Value, valueKind);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error writing to the Windows Registry.", ex);
            }
        }

        /// <summary>
        /// Sets the default value of a registry key to the specified string value.
        /// </summary>
        /// <param name="Hive">The registry hive.</param>
        /// <param name="Path">The registry key path.</param>
        /// <param name="Value">The value to set for the default property.</param>
        /// <exception cref="ArgumentException">Thrown when the specified registry hive or path is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while writing to the Windows Registry key.</exception>
        public void SetDefaultValue(RegistryHive Hive, string Path, string Value)
        {
            SetValue(Hive, Path, null, Value, RegistryValueKind.String);
        }

        /// <summary>
        /// Creates a registry key at the specified location.
        /// </summary>
        /// <param name="Hive">The registry hive to create the key under.</param>
        /// <param name="Path">The path of the registry key to create.</param>
        /// <exception cref="ArgumentException">Thrown when the specified registry hive or path is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while creating the Windows Registry key.</exception>
        public void CreateKey(RegistryHive Hive, string Path)
        {
            ThrowIfHiveInvalid(Hive);
            ThrowIfPathInvalid(Path);

            try
            {
                using RegistryKey baseKey = RegistryKey.OpenBaseKey(Hive, RegistryView.Default);
                baseKey.CreateSubKey(Path);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating the Windows Registry key.", ex);
            }
        }

        #endregion

        #region Public Delete Methods

        /// <summary>
        /// Deletes a registry value under the specified registry key.
        /// </summary>
        /// <param name="Hive">The registry hive to open.</param>
        /// <param name="Path">The path of the registry key where the value exists.</param>
        /// <param name="Name">The name of the value to delete.</param>
        /// <exception cref="ArgumentException">Thrown when the specified registry hive, path or name is invalid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user does not have the necessary permissions to delete the registry value.</exception>
        /// <exception cref="SecurityException">Thrown when the user does not have the necessary permissions to delete the registry value due to security restrictions.</exception>
        public void DeleteRegistryValue(RegistryHive Hive, string Path, string Name)
        {
            ThrowIfHiveInvalid(Hive);
            ThrowIfPathInvalid(Path);
            ThrowIfNameInvalid(Name);
            try
            {
                using RegistryKey baseKey = RegistryKey.OpenBaseKey(Hive, RegistryView.Default);
                using RegistryKey key = baseKey.OpenSubKey(Path, true);
                key?.DeleteValue(Name, true);
            }
            catch (SecurityException ex)
            {
                throw new SecurityException("Insufficient permissions to delete the registry key or value.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Insufficient permissions to delete the registry key or value due to security restrictions.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the registry key or value.", ex);
            }
        }

        /// <summary>
        /// Deletes a registry key and its subkeys.
        /// </summary>
        /// <param name="Hive">The registry hive to open.</param>
        /// <param name="Path">The path of the registry key to delete.</param>
        /// <exception cref="ArgumentException">Thrown when the specified registry hive or path is invalid</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user does not have the necessary permissions to delete the registry key.</exception>
        /// <exception cref="SecurityException">Thrown when the user does not have the necessary permissions to delete the registry key due to security restrictions.</exception>
        public void DeleteTree(RegistryHive Hive, string Path)
        {
            ThrowIfHiveInvalid(Hive);
            ThrowIfPathInvalid(Path);
            try
            {
                using RegistryKey baseKey = RegistryKey.OpenBaseKey(Hive, RegistryView.Default);
                baseKey?.DeleteSubKeyTree(Path, true);
            }
            catch (SecurityException ex)
            {
                throw new SecurityException("Insufficient permissions to delete the registry key or value.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Insufficient permissions to delete the registry key or value due to security restrictions.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the registry key or value.", ex);
            }
        }

        #endregion

        #region Public Converter Methods

        /*******************************************************
         * 
         * Converter methods
         *
         ******************************************************/
        /// <summary>
        /// Converts a string representation of a Registry Hive to the corresponding RegistryHive enum value.
        /// </summary>
        /// <param name="HiveString">A string representation of a Registry Hive, not case-sensitive.</param>
        /// <returns>The RegistryHive enum value corresponding to the provided string representation.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided string does not match a valid Registry Hive.</exception>
        public RegistryHive ConvertStringToRegistryHive(string HiveString)
        {
            if (string.IsNullOrEmpty(HiveString))
                throw new ArgumentNullException(nameof(HiveString), "The registry hive string cannot be null or empty. Please provide a valid registry hive string.");

            return HiveString.ToLower() switch
            {
                "hkcr" or "hkey_classes_root" or "classesroot" => RegistryHive.ClassesRoot,
                "hkcu" or "hkey_current_user" or "currentuser" => RegistryHive.CurrentUser,
                "hklm" or "hkey_local_machine" or "localmachine" => RegistryHive.LocalMachine,
                "hku" or "hkey_users" or "users" => RegistryHive.Users,
                "hkcc" or "hkey_current_config" or "currentconfig" => RegistryHive.CurrentConfig,
                _ => throw new ArgumentException("Invalid registry hive string.", nameof(HiveString)),
            };
        }

        /// <summary>
        /// Converts a string representation of a RegistryValueKind to the corresponding RegistryValueKind enum value.
        /// </summary>
        /// <param name="ValueType">A string representation of a RegistryValueKind, not case-sensitive.</param>
        /// <returns>The RegistryValueKind enum value corresponding to the provided string representation.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided string does not match a valid RegistryValueKind.</exception>
        public RegistryValueKind ConvertStringToRegistryValueKind(string ValueType)
        {
            if (string.IsNullOrEmpty(ValueType))
                throw new ArgumentNullException(nameof(ValueType), "The registry value type string cannot be null or empty. Please provide a valid registry value type string.");

            return ValueType.ToLower() switch
            {
                "string" or "reg_sz" => RegistryValueKind.String,
                "dword" or "reg_dword" => RegistryValueKind.DWord,
                "binary" or "reg_binary" => RegistryValueKind.Binary,
                "qword" or "reg_qword" => RegistryValueKind.QWord,
                "multistring" or "reg_multi_sz" => RegistryValueKind.MultiString,
                "expandstring" or "reg_expand_sz" => RegistryValueKind.ExpandString,
                _ => throw new ArgumentException("Invalid RegistryValueKind string representation.", nameof(ValueType)),
            };
        }


        /// <summary>
        /// Converts a .NET data type to the corresponding RegistryValueKind.
        /// </summary>
        /// <param name="ValueType">The .NET data type to convert.</param>
        /// <returns>The corresponding RegistryValueKind.</returns>
        public RegistryValueKind ConvertTypeToRegistryValueKind(Type ValueType)
        {
            if (ValueType == null)
                throw new ArgumentNullException(nameof(ValueType), "The value type argument cannot be null.");

            return Type.GetTypeCode(ValueType) switch
            {
                TypeCode.String => RegistryValueKind.String,
                TypeCode.Int32 => RegistryValueKind.DWord,
                TypeCode.Int64 => RegistryValueKind.QWord,
                TypeCode.Boolean => RegistryValueKind.DWord,
                TypeCode.Byte => RegistryValueKind.Binary,
                /*
                TypeCode.Single => RegistryValueKind.String,
                TypeCode.Double => RegistryValueKind.String;
                TypeCode.DateTime => RegistryValueKind.String;
                TypeCode.Char => RegistryValueKind.String;
                TypeCode.Decimal => RegistryValueKind.String;
                */
                _ => RegistryValueKind.String,// Default to String for unsupported types
            };
        }

        /// <summary>
        /// Converts a RegistryValueKind enumeration value to its corresponding .NET Type.
        /// </summary>
        /// <param name="ValueKind">The RegistryValueKind value to be converted.</param>
        /// <returns>The .NET Type that corresponds to the given RegistryValueKind.</returns>
        public Type ConvertRegistryValueKindToType(RegistryValueKind ValueKind)
        {
            return ValueKind switch
            {
                RegistryValueKind.String or RegistryValueKind.ExpandString => typeof(string),
                RegistryValueKind.DWord => typeof(int),
                RegistryValueKind.QWord => typeof(long),
                RegistryValueKind.Binary => typeof(byte[]),
                RegistryValueKind.MultiString => typeof(string[]),
                _ => typeof(object),
            };
        }

        #endregion

        #region throw if invalid methods

        /// <summary>
        /// Validates the specified RegistryHive value.
        /// </summary>
        /// <param name="hive">The RegistryHive value to validate.</param>
        /// <exception cref="ArgumentException">Thrown when an unknown or unsupported RegistryHive value is provided.</exception>
        private static void ThrowIfHiveInvalid(RegistryHive Hive)
        {
            if (!Enum.IsDefined(typeof(RegistryHive), Hive) || Hive == RegistryHive.CurrentConfig || Hive == 0)
                throw new ArgumentException("Invalid parameter: Unknown or unsupported RegistryHive value.", nameof(Hive));
        }

        /// <summary>
        /// Throws an exception if the specified path is null or empty.
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <returns>The validated parameter path.</returns>
        private static void ThrowIfPathInvalid(string Path)
        {
            if (string.IsNullOrWhiteSpace(Path))
                throw new ArgumentException("Invalid parameter: Path cannot be null, empty, or consist only of whitespace characters.", nameof(Path));
        }

        /// <summary>
        /// Throws an exception if the specified name is null or empty.
        /// </summary>
        /// <param name="name">The name to validate.</param>
        private static void ThrowIfNameInvalid(string Name)
        {
            if (Name == null)
                throw new ArgumentNullException(nameof(Name), "Invalid parameter: Name cannot be null.");
        }

        /// <summary>
        /// Throws an exception if the specified RegistryValueKind is unknown.
        /// </summary>
        /// <param name="valueKind">The RegistryValueKind to validate.</param>
        /// <exception cref="InvalidOperationException">Thrown when the RegistryValueKind is Unknown.</exception>
        private static void ThrowIfValueKindInvalid(RegistryValueKind ValueKind)
        {
            if (!Enum.IsDefined(typeof(RegistryValueKind), ValueKind) || ValueKind == RegistryValueKind.Unknown || ValueKind == RegistryValueKind.None)
                throw new ArgumentException("Invalid parameter: Unknown or unsupported RegistryValueKind value.", nameof(ValueKind));
        }

        #endregion
    }
}