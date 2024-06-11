using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    public interface IRegistry
    {
        #region Registry Reader

        /// <summary>
        /// Gets the names of subkeys under the specified registry hive and path.
        /// </summary>
        string[] GetSubKeyNames(RegistryHive hive, string path);

        /// <summary>
        /// Gets the value of a registry entry specified by its name.
        /// </summary>
        string GetValue(RegistryHive hive, string path, string name);

        /// <summary>
        /// Gets the string value of a registry entry specified by its name.
        /// </summary>
        string GetStringValue(RegistryHive hive, string path, string name, string defaultValue = null);

        /// <summary>
        /// Gets the boolean value of a registry entry specified by its name.
        /// </summary>
        bool GetBoolValue(RegistryHive hive, string path, string propertyName, bool defaultValue = false);

        /// <summary>
        /// Gets the DWORD value of a registry entry specified by its name.
        /// </summary>
        int GetIntegerValue(RegistryHive hive, string path, string propertyName, int defaultValue = -1);

        /// <summary>
        /// Retrieves a specific registry entry of type string from the Windows Registry.
        /// </summary>
        WinRegistryEntry<string> GetEntry(RegistryHive hive, string path, string name);

        /// <summary>
        /// Retrieves a list of string-type registry entries from the Windows Registry under the specified path.
        /// </summary>
        List<WinRegistryEntry<string>> GetEntries(RegistryHive hive, string path);

        /// <summary>
        /// Retrieves a list of string-type registry entries from the Windows Registry under the specified path and its subkeys recursively.
        /// </summary>
        List<WinRegistryEntry<string>> GetEntriesRecursive(RegistryHive hive, string path);

        #endregion

        #region Registry Writer

        /// <summary>
        /// Sets the value of a registry entry.
        /// </summary>
        void SetValue(RegistryHive hive, string path, string name, object value, RegistryValueKind valueKind);

        /// <summary>
        /// Creates a new registry key.
        /// </summary>
        void CreateKey(RegistryHive hive, string path);

        /// <summary>
        /// Deletes a registry value.
        /// </summary>
        void DeleteRegistryValue(RegistryHive hive, string path, string name);

        /// <summary>
        /// Deletes a registry key and all its subkeys and values.
        /// </summary>
        void DeleteTree(RegistryHive hive, string path);

        #endregion

        #region Registry Tools

        /// <summary>
        /// Converts a string representation of a registry hive to the corresponding RegistryHive enum value.
        /// </summary>
        RegistryHive ConvertStringToRegistryHive(string hiveString);

        /// <summary>
        /// Converts a string representation of a registry value kind to the corresponding RegistryValueKind enum value.
        /// </summary>
        RegistryValueKind ConvertStringToRegistryValueKind(string valueType);

        /// <summary>
        /// Converts a .NET type to the corresponding RegistryValueKind enum value.
        /// </summary>
        RegistryValueKind ConvertTypeToRegistryValueKind(Type valueType);

        /// <summary>
        /// Converts a RegistryValueKind enum value to the corresponding .NET type.
        /// </summary>
        Type ConvertRegistryValueKindToType(RegistryValueKind valueKind);

        #endregion
    }
}
