using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Security;

namespace mRemoteNG.Tools.WindowsRegistry
{
    /// <summary>
    /// This class interacting with the Windows Registry.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class WindowsRegistry : IRegistry, IRegistryRead, IRegistryWrite
    {
        #region public read
        /// <summary>
        /// Retrieves the names of subkeys under a specified registry key path.
        /// </summary>
        /// <param name="hive">The RegistryHive where the subkeys are located.</param>
        /// <param name="path">The path to the registry key containing the subkeys.</param>
        /// <returns>An array of strings containing the names of subkeys, or an empty array if no subkeys are found.</returns>
        public string[] GetSubKeyNames(RegistryHive hive, string path)
        {
            if (hive == 0)
                throw new ArgumentException("Unknown or unsupported RegistryHive value.", nameof(hive));
            path.ThrowIfNull(nameof(path));

            using (var key = OpenSubKey(hive, path))
            {
                return key.Any()
                    ? key.First().GetSubKeyNames()
                    : new string[0];
            }
         }

        /// <summary>
        /// Retrieves the value of a specific property within a Windows Registry key and returns it as an Optional<string>.
        /// </summary>
        /// <param name="key">The WindowsRegistryKey containing information about the registry property.</param>
        /// <returns>An Optional<string> containing the property value, or Optional<string>.Empty if the value is not found.</returns>
        public string GetPropertyValue(WindowsRegistryKey key)
        {
            if (!key.IsKeyReadable())
                throw new InvalidOperationException("The Windows Registry key is not ready for reading.");

            return GetPropertyValue(key.Hive, key.Path, key.Name);
        }

        /// <summary>
        /// Retrieves the value of a specific property within the Windows Registry and returns it as an Optional<string>.
        /// </summary>
        /// <param name="hive">The RegistryHive where the property is located.</param>
        /// <param name="path">The path to the registry key containing the property.</param>
        /// <param name="name">The name of the property to retrieve.</param>
        /// <returns>An Optional<string> containing the property value, or Optional<string>.Empty if the value is not found.</returns>
        public string GetPropertyValue(RegistryHive hive, string path, string name)
        {
            if (hive == 0)
                throw new ArgumentException("Unknown or unsupported RegistryHive value.", nameof(hive));
            path.ThrowIfNull(nameof(path));
            name.ThrowIfNull(nameof(name));

            using (var key = OpenSubKey(hive, path))
            {
                if (!key.Any())
                    return null;

                var keyValue = key.First().GetValue(name);

                if (keyValue == null)
                    return null;

                return keyValue.ToString();
            }
        }

        /// <summary>
        /// Gets a boolean value from the Windows Registry based on the specified registry path and property name.
        /// If the value is not found or cannot be parsed, it returns a specified default value.
        /// </summary>
        /// <param name="hive">The Registry hive where the value is located.</param>
        /// <param name="path">The registry path to the key containing the property.</param>
        /// <param name="propertyName">The name of the property to retrieve.</param>
        /// <param name="defaultValue">The default value to return if the property is not found or cannot be parsed. Default is false.</param>
        /// <returns>The boolean value of the specified property or the default value if not found or cannot be parsed.</returns>
        public bool GetBoolValue(RegistryHive hive, string path, string propertyName, bool defaultValue = false)
        {
            var value = GetPropertyValue(hive, path, propertyName);

            if (!string.IsNullOrEmpty(value))
            {
                if (int.TryParse(value, out int intValue))
                    return intValue == 1;
                if (bool.TryParse(value, out bool boolValue))
                    return boolValue;
            }

            return defaultValue;
        }

        /// <summary>
        /// Retrieves a WindowsRegistryKey object for a specific registry hive, path, and value name.
        /// </summary>
        /// <param name="hive">The RegistryHive of the key.</param>
        /// <param name="path">The path of the key.</param>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <returns>A WindowsRegistryKey object representing the specified registry key and value.</returns>
        public WindowsRegistryKey GetWindowsRegistryKey(RegistryHive hive, string path, string name)
        {
            WindowsRegistryKey key = new()
            {
                Hive = hive,
                Path = path,
                Name = name
            };

            return GetWindowsRegistryKey(key);
        }

        /// <summary>
        /// Retrieves a WindowsRegistryKey object for a specific WindowsRegistryKey, populating its value and value kind.
        /// </summary>
        /// <param name="key">A WindowsRegistryKey object containing the hive, path, value name and more.</param>
        /// <returns>A WindowsRegistryKey object representing the specified registry key and value, with value and value kind populated.</returns>
        public WindowsRegistryKey GetWindowsRegistryKey(WindowsRegistryKey key)
        {
            if (!key.IsKeyReadable())
                throw new InvalidOperationException("The Windows Registry key is not ready for reading.");

            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(key.Hive, RegistryView.Default), subKey = baseKey.OpenSubKey(key.Path))
            {
                if (subKey != null)
                {
                    var value = subKey.GetValue(key.Name);
                    if (value != null)
                        key.Value = value.ToString();

                    if (TestValueKindExists(subKey, key.Name, out RegistryValueKind ValueKind))
                        key.ValueKind = ValueKind;
                }
            }

            return key;
        }

        /// <summary>
        /// Retrieves a list of registry entries (properties) and their values under a given key path.
        /// </summary>
        /// <param name="hive">The RegistryHive of the key path.</param>
        /// <param name="path">The path of the key from which to retrieve values.</param>
        /// <returns>A list of WindowsRegistryKey objects, each representing a value within the specified registry key path.</returns>
        public List<WindowsRegistryKey> GetRegistryEntries(RegistryHive hive, string path)
        {
            if (hive == 0)
                throw new ArgumentException("Unknown or unsupported RegistryHive value.", nameof(hive));
            path.ThrowIfNull(nameof(path));

            List<WindowsRegistryKey> list = new List<WindowsRegistryKey>();
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default), key = baseKey.OpenSubKey(path))
            {
                if (key != null)
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        list.Add(new WindowsRegistryKey
                            {
                                Hive = hive,
                                Path = path,
                                Name = valueName,
                                Value = key.GetValue(valueName).ToString(),
                                ValueKind = key.GetValueKind(valueName)
                            }
                        );
                    }
                }
                
            }
            return list;
        }

        /// <summary>
        /// Recursively retrieves registry entries under a given key path and its subkeys.
        /// </summary>
        /// <param name="hive">The RegistryHive of the key path.</param>
        /// <param name="path">The path of the key from which to retrieve values.</param>
        /// <returns>A list of WindowsRegistryKey objects, each representing a value within the specified registry key path.</returns>
        public List<WindowsRegistryKey> GetRegistryEntryiesRecursive(RegistryHive hive, string path)
        {
            if (hive == 0)
                throw new ArgumentException("Unknown or unsupported RegistryHive value.", nameof(hive));
            path.ThrowIfNull(nameof(path));

            List<WindowsRegistryKey> list = GetRegistryEntries(hive, path);
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default), key = baseKey.OpenSubKey(path))
            {
                if (key != null)
                {
                    foreach (string subPathName in key.GetSubKeyNames())
                    {
                        string subKey = $"{path}\\{subPathName}";
                        list.AddRange(GetRegistryEntryiesRecursive(hive, subKey));
                    }
                }
            }

            return list;
        }
        #endregion

        #region public write methods
        /// <summary>
        /// Sets the value of a specific property within a registry key using individual parameters.
        /// </summary>
        /// <param name="hive">The registry hive.</param>
        /// <param name="path">The path to the registry key containing the property.</param>
        /// <param name="name">The name of the property to set.</param>
        /// <param name="value">The value to set for the property.</param>
        /// <param name="valueKind">The data type of the value to set.</param>
        public void SetRegistryValue(RegistryHive hive, string path, string name, object value, RegistryValueKind valueKind)
        {
            WindowsRegistryKey key = (new WindowsRegistryKey
            {
                Hive      = hive,
                Path      = path,
                Name      = name,
                Value     = value.ToString(),
                ValueKind = valueKind
            });

            CreateOrSetRegistryValue(key);
        }

        /// <summary>
        /// Sets the value of a specific property within a registry key using the provided WindowsRegistryKey object.
        /// </summary>
        /// <param name="key">The WindowsRegistryKey object containing information about the registry property.</param>
        public void SetRegistryValue(WindowsRegistryKey key)
        {
            CreateOrSetRegistryValue(key);
        }

        /// <summary>
        /// Deletes a registry key and its subkeys.
        /// </summary>
        /// <param name="hive">The registry hive to open.</param>
        /// <param name="path">The path of the registry key to delete.</param>
        /// <param name="ignoreNotFound">Set to true to ignore if the key is not found.</param>
        public void DeleteRegistryKey(RegistryHive hive, string path, bool ignoreNotFound = false)
        {
            try
            {
                using (RegistryKey key = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
                {
                    if (key != null)
                    {
                        key.DeleteSubKeyTree(path, ignoreNotFound);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions according to your requirements
                Console.WriteLine($"Error deleting registry key: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Converts a string representation of a Registry Hive to the corresponding RegistryHive enum value.
        /// </summary>
        /// <param name="hiveString">A string representation of a Registry Hive, not case-sensitive.</param>
        /// <returns>The RegistryHive enum value corresponding to the provided string representation.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided string does not match a valid Registry Hive.</exception>
        public RegistryHive ConvertStringToRegistryHive(string hiveString)
        {
            switch (hiveString.ToLower())
            {
                // ClassesRoot
                case "hkcr":
                case "hkey_classes_root":
                case "classesroot":
                    return RegistryHive.ClassesRoot;

                // CurrentUser
                case "hkcu":
                case "hkey_current_user":
                case "currentuser":
                    return RegistryHive.CurrentUser;

                // LocalMachine
                case "hklm":
                case "hkey_local_machine":
                case "localmachine":
                    return RegistryHive.LocalMachine;

                // Users
                case "hku":
                case "hkey_users":
                case "users":
                    return RegistryHive.Users;

                // CurrentConfig
                case "hkcc":
                case "hkey_current_config":
                case "currentconfig":
                    return RegistryHive.CurrentConfig;

                default:
                    throw new ArgumentException("Invalid registry hive string.", nameof(hiveString));
            }
        }

        /// <summary>
        /// Converts a string representation of a RegistryValueKind to the corresponding RegistryValueKind enum value.
        /// </summary>
        /// <param name="valueType">A string representation of a RegistryValueKind, not case-sensitive.</param>
        /// <returns>The RegistryValueKind enum value corresponding to the provided string representation.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided string does not match a valid RegistryValueKind.</exception>
        public RegistryValueKind ConvertStringToRegistryValueKind(string valueType)
        {
            switch (valueType.ToLower())
            {
                // REG_SZ
                case "string":
                case "reg_sz":
                    return RegistryValueKind.String;

                // REG_DWORD
                case "dword":
                case "reg_dword":
                    return RegistryValueKind.DWord;

                // REG_BINARY
                case "binary":
                case "reg_binary":
                    return RegistryValueKind.Binary;

                // REG_QWORD
                case "qword":
                case "reg_qword":
                    return RegistryValueKind.QWord;

                //  REG_MULTI_SZ
                case "multistring":
                case "reg_multi_sz":
                    return RegistryValueKind.MultiString;

                // REG_EXPAND_SZ
                case "expandstring":
                case "reg_expand_sz":
                    return RegistryValueKind.ExpandString;

                default:
                    throw new ArgumentException("Invalid RegistryValueKind string representation.", nameof(valueType));
            }
        }


        /// <summary>
        /// Converts a .NET data type to the corresponding RegistryValueKind.
        /// </summary>
        /// <param name="valueType">The .NET data type to convert.</param>
        /// <returns>The corresponding RegistryValueKind.</returns>
        public RegistryValueKind ConvertTypeToRegistryValueKind(Type valueType)
        {
            switch (Type.GetTypeCode(valueType))
            {
                case TypeCode.String:
                    return RegistryValueKind.String;
                case TypeCode.Int32:
                    return RegistryValueKind.DWord;
                case TypeCode.Int64:
                    return RegistryValueKind.QWord;
                case TypeCode.Boolean:
                    return RegistryValueKind.DWord;
                case TypeCode.Byte:
                    return RegistryValueKind.Binary;
                /*
                case TypeCode.Single:
                    return RegistryValueKind;
                case TypeCode.Double:
                    return RegistryValueKind.String;
                case TypeCode.DateTime:
                    return RegistryValueKind.String; // DateTime can be stored as a string or other types
                case TypeCode.Char:
                    return RegistryValueKind.String; // Char can be stored as a string or other types
                case TypeCode.Decimal:
                     return RegistryValueKind.String; // Decimal can be stored as a string or other types
                */
                default:
                    return RegistryValueKind.String; // Default to String for unsupported types
            }
        }

        /// <summary>
        /// Converts a RegistryValueKind enumeration value to its corresponding .NET Type.
        /// </summary>
        /// <param name="valueKind">The RegistryValueKind value to be converted.</param>
        /// <returns>The .NET Type that corresponds to the given RegistryValueKind.</returns>
        public Type ConvertRegistryValueKindToType(RegistryValueKind valueKind)
        {
            switch (valueKind)
            {
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    return typeof(string);
                case RegistryValueKind.DWord:
                    return typeof(int);
                case RegistryValueKind.QWord:
                    return typeof(long);
                case RegistryValueKind.Binary:
                    return typeof(byte[]);
                case RegistryValueKind.MultiString:
                    return typeof(string[]);
                case RegistryValueKind.Unknown:
                default:
                    return typeof(object);
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Opens a subkey within the Windows Registry under the specified hive and key path.
        /// </summary>
        /// <param name="hive">The Windows Registry hive where the subkey is located.</param>
        /// <param name="keyPath">The path to the subkey to be opened.</param>
        /// <returns>
        /// A disposable optional object containing the opened registry subkey if successful;
        /// otherwise, it returns an empty optional object.
        /// </returns>
        private DisposableOptional<RegistryKey> OpenSubKey(RegistryHive hive, string keyPath)
        {
            switch (hive)
            {
                case RegistryHive.ClassesRoot:
                    return new DisposableOptional<RegistryKey>(Registry.ClassesRoot.OpenSubKey(keyPath));
                case RegistryHive.CurrentConfig:
                    return new DisposableOptional<RegistryKey>(Registry.CurrentConfig.OpenSubKey(keyPath));
                case RegistryHive.CurrentUser:
                    return new DisposableOptional<RegistryKey>(Registry.CurrentUser.OpenSubKey(keyPath));
                case RegistryHive.Users:
                    return new DisposableOptional<RegistryKey>(Registry.Users.OpenSubKey(keyPath));
                case RegistryHive.LocalMachine:
                    return new DisposableOptional<RegistryKey>(Registry.LocalMachine.OpenSubKey(keyPath));
                default:
                    throw new ArgumentOutOfRangeException(nameof(hive), hive, null);
            }
        }

        /// <summary>
        /// Attempts to retrieve the value kind of a specific property within a registry subkey.
        /// </summary>
        /// <param name="subKey">The registry subkey from which to retrieve the value kind.</param>
        /// <param name="valueName">The name of the property for which to retrieve the value kind.</param>
        /// <param name="valueKind">An output parameter that will contain the retrieved value kind, or RegistryValueKind.Unknown if the property or value kind is not found.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        private bool TestValueKindExists(RegistryKey subKey, string valueName, out RegistryValueKind valueKind)
        {
            // Set a default value
            valueKind = RegistryValueKind.Unknown;
            try
            {
                valueKind = subKey.GetValueKind(valueName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates or sets the value of a specific property within a registry key.
        /// </summary>
        /// <param name="key">The WindowsRegistryKey object containing information about the registry property.</param>
        /// <exception cref="InvalidOperationException">Thrown when the Windows Registry key is not ready for writing.</exception>
        /// <exception cref="SecurityException">Thrown when a security-related error occurs while accessing the registry.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while accessing the registry.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when access to the registry is unauthorized.</exception>
        /// <exception cref="Exception">Thrown for all other exceptions.</exception>
        private void CreateOrSetRegistryValue(WindowsRegistryKey key)
        {
            try
            {
                if (!key.IsKeyWritable())
                    throw new InvalidOperationException("The Windows Registry key is not ready for writing.");

                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(key.Hive, RegistryView.Default), registryKey = baseKey.OpenSubKey(key.Path, true))
                {
                    if (registryKey == null)
                    {
                        // The specified subkey doesn't exist, so create it.
                        using (RegistryKey newKey = baseKey.CreateSubKey(key.Path))
                        {
                            newKey.SetValue(key.Name, key.Value, key.ValueKind);
                        }
                    }
                    else
                    {
                        registryKey.SetValue(key.Name, key.Value, key.ValueKind);
                    }
                }
            }
            catch (SecurityException ex)
            {
                throw ex;
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}