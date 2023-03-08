using System;
using System.Linq;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace mRemoteNG.Tools.WindowsRegistry
{
    [SupportedOSPlatform("windows")]
    public class WindowsRegistry : IRegistry
    {
        public string[] GetSubKeyNames(RegistryHive hive, string keyPath)
        {
            keyPath.ThrowIfNull(nameof(keyPath));

            using (var key = OpenSubKey(hive, keyPath))
            {
                return key.Any()
                    ? key.First().GetSubKeyNames()
                    : new string[0];
            }
        }

        public Optional<string> GetKeyValue(RegistryHive hive, string keyPath, string propertyName)
        {
            keyPath.ThrowIfNull(nameof(keyPath));
            propertyName.ThrowIfNull(nameof(propertyName));

            using (var key = OpenSubKey(hive, keyPath))
            {
                if (!key.Any())
                    return Optional<string>.Empty;

                return key.First().GetValue(propertyName) as string;
            }
        }

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
    }
}