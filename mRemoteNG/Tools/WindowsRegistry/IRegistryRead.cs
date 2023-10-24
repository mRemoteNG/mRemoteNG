using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    public interface IRegistryRead
    {
        #region registry reader
        string[] GetSubKeyNames(RegistryHive hive, string path);

        Optional<string> GetPropertyValue(WindowsRegistryKey key);
        Optional<string> GetPropertyValue(RegistryHive hive, string path, string name);

        WindowsRegistryKey GetWindowsRegistryKey(RegistryHive hive, string path, string name);
        WindowsRegistryKey GetWindowsRegistryKey(WindowsRegistryKey key);

        List<WindowsRegistryKey> GetRegistryEntries(RegistryHive hive, string path);
        List<WindowsRegistryKey> GetRegistryEntryiesRecursive(RegistryHive hive, string path);
        #endregion
        
        #region converter
        RegistryHive ConvertStringToRegistryHive(string hiveString);
        RegistryValueKind ConvertStringToRegistryValueKind(string valueType);
        #endregion
    }
}