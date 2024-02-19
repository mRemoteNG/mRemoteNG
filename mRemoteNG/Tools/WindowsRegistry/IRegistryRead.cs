using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Interface for the Registry class providing methods for interacting with read actions in the Windows Registry.
    /// </summary>
    public interface IRegistryRead
    {
        #region registry reader
        string[] GetSubKeyNames(RegistryHive hive, string path);
        string GetPropertyValue(WindowsRegistryKey key);
        string GetPropertyValue(RegistryHive hive, string path, string name);

        bool GetBoolValue(RegistryHive hive, string path, string propertyName, bool defaultValue = false);
        int GetDwordValue(RegistryHive hive, string path, string propertyName, int defaultValue = 0);

        WindowsRegistryKey GetWindowsRegistryKey(RegistryHive hive, string path, string name);
        WindowsRegistryKey GetWindowsRegistryKey(WindowsRegistryKey key);

        List<WindowsRegistryKey> GetRegistryEntries(RegistryHive hive, string path);
        List<WindowsRegistryKey> GetRegistryEntryiesRecursive(RegistryHive hive, string path);
        #endregion

        #region registry tools
        RegistryHive ConvertStringToRegistryHive(string hiveString);
        RegistryValueKind ConvertStringToRegistryValueKind(string valueType);
        RegistryValueKind ConvertTypeToRegistryValueKind(Type valueType);
        Type ConvertRegistryValueKindToType(RegistryValueKind valueKind);
        #endregion
    }
}