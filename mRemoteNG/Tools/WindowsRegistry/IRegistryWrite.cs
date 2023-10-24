using Microsoft.Win32;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    public interface IRegistryWrite
    {
        #region registry writer
        void SetRegistryValue(WindowsRegistryKey key);
        void SetRegistryValue(RegistryHive hive, string path, string name, object value, RegistryValueKind valueKind);

        #endregion

        #region converter
        RegistryHive ConvertStringToRegistryHive(string hiveString);
        RegistryValueKind ConvertStringToRegistryValueKind(string valueType);
        #endregion}
    }
}