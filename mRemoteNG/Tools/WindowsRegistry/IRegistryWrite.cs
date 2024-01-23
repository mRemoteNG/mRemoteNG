using Microsoft.Win32;
using System;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Interface for the Registry class providing methods for interacting with write actions in the Windows Registry.
    /// </summary>
    public interface IRegistryWrite
    {
        #region registry writer
        void SetRegistryValue(WindowsRegistryKey key);
        void SetRegistryValue(RegistryHive hive, string path, string name, object value, RegistryValueKind valueKind);

        #endregion

        #region registry tools
        RegistryHive ConvertStringToRegistryHive(string hiveString);
        RegistryValueKind ConvertStringToRegistryValueKind(string valueType);
        RegistryValueKind ConvertTypeToRegistryValueKind(Type valueType);
        Type ConvertRegistryValueKindToType(RegistryValueKind valueKind);
        #endregion
    }
}