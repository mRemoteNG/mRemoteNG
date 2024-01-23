using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using static mRemoteNG.Config.Settings.Registry.RegistryController;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Interface for the RegistryWorker class providing functionality to interact with the Windows Registry to retrieve registry settings.
    /// </summary>
    public interface IRegistryAdvancedRead
    {
        #region WindowsRegistry reader
        string[] GetSubKeyNames(RegistryHive hive, string path);
        string GetPropertyValue(WindowsRegistryKey key);
        string GetPropertyValue(RegistryHive hive, string path, string name);

        WindowsRegistryKey GetWindowsRegistryKey(RegistryHive hive, string path, string name);
        WindowsRegistryKey GetWindowsRegistryKey(WindowsRegistryKey key);

        List<WindowsRegistryKey> GetRegistryEntries(RegistryHive hive, string path);
        List<WindowsRegistryKey> GetRegistryEntryiesRecursive(RegistryHive hive, string path);
        #endregion

        #region WindowsRegistry tools
        RegistryHive ConvertStringToRegistryHive(string hiveString);
        RegistryValueKind ConvertStringToRegistryValueKind(string valueType);
        RegistryValueKind ConvertTypeToRegistryValueKind(Type valueType);
        Type ConvertRegistryValueKindToType(RegistryValueKind valueKind);
        #endregion

        #region WindowsRegistryAdvanced GetInteger
        WindowsRegistryKeyInteger GetInteger(RegistryHive hive, string path, string propertyName, int? defaultValue = null);
        #endregion

        #region WindowsRegistryAdvanced GetString
        WindowsRegistryKeyString GetString(RegistryHive hive, string path, string propertyName, string defaultValue = null);
        WindowsRegistryKeyString GetStringValidated(RegistryHive hive, string path, string propertyName, string[] allowedValues, bool caseSensitive = false, string defaultValue = null);
        #endregion

        #region WindowsRegistryAdvanced GetBoolean
        WindowsRegistryKeyBoolean GetBoolean(RegistryHive hive, string path, string propertyName, bool? defaultValue = null);
        #endregion
    }
}