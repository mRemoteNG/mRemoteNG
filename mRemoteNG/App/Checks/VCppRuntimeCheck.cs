using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning; // Add for SupportedOSPlatform

namespace mRemoteNG.App.Update
{
    public static class VCppRuntimeCheck
    {
        [SupportedOSPlatform("windows")]
        public static IList<string> GetInstalledVcRedistVersions()
        {
            var installedVersions = new List<string>();
            var baseKeys = new[]
            {
                @"SOFTWARE\Microsoft\VisualStudio",
                @"SOFTWARE\WOW6432Node\Microsoft\VisualStudio"
            };

            for (int major = 14; major <= 17; major++) // Covers 2015–2022+
            {
                for (int minor = 0; minor <= 3; minor++)
                {
                    string version = $"{major}.{minor}";
                    foreach (var baseKey in baseKeys)
                    {
                        string path = $@"{baseKey}\{version}\VC\Runtimes\x64";
                        using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(path))
                        {
                            if (key?.GetValue("Installed") is int installed && installed == 1)
                            {
                                installedVersions.Add(version);
                            }
                        }
                    }
                }
            }

            return installedVersions;
        }
    }
}
