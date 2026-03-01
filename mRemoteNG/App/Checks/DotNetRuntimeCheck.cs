using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace mRemoteNG.DotNet.Update
{
    [SupportedOSPlatform("windows")]
    public static class DotNetRuntimeCheck
    {
        public const string RequiredDotnetVersion = "10.0";
        private const string ReleaseFeedUrl = "https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/releases-index.json";

        #region Installed Version Check
        /// <summary>
        /// Gets the installed .NET 10 runtime version if present
        /// </summary>
        /// <returns>The version string (e.g., "v10.0.0") or null if not found</returns>
        [SupportedOSPlatform("windows")]
        public static string? GetLatestDotNetRuntimeVersion()
        {
            string[] registryPaths = new[]
            {
                @"SOFTWARE\dotnet\Setup\InstalledVersions\x86",
                @"SOFTWARE\dotnet\Setup\InstalledVersions\x64",
                @"SOFTWARE\dotnet\Setup\InstalledVersions\arm64"
            };

            foreach (string path in registryPaths)
            {
                try
                {
                    using RegistryKey? key = Registry.LocalMachine.OpenSubKey(path);
                    if (key == null)
                    {
                        continue;
                    }

                    // Check for the "sharedhost" subkey
                    using (RegistryKey? sharedHostKey = key.OpenSubKey("sharedhost"))
                    {
                        if (sharedHostKey == null) {
                            continue;
                        }

                        // Look for the "Version" value in sharedhost
                        object? versionValue = sharedHostKey.GetValue("Version");
                        if (versionValue != null)
                        {
                            string? version = versionValue.ToString();
                            if (!string.IsNullOrWhiteSpace(version))
                            {
                                return version;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking registry fallback: {ex.Message}");
                }
            }

            return null;
        }
        #endregion Installed Version Check
        #region Latest Online Version Check
        public static async Task<(string latestRuntimeVersion, string downloadUrl)> GetLatestAvailableDotNetVersionAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "DotNetRuntimeChecker");

                string jsonContent = await httpClient.GetStringAsync(ReleaseFeedUrl);
                JObject releasesIndex = JObject.Parse(jsonContent);

                // Find the entry for .NET matching RequiredDotnetVersion
                JToken? dotnetEntry = releasesIndex["releases-index"]?.FirstOrDefault(entry => entry["channel-version"]?.ToString() == RequiredDotnetVersion);

                if (dotnetEntry != null && dotnetEntry["latest-runtime"] != null)
                {
                    string? latestRuntimeVersion = dotnetEntry["latest-runtime"]?.ToString();
                    string arch;
                    switch (RuntimeInformation.OSArchitecture)
                    {
                        case Architecture.Arm64:
                            arch = "arm64";
                            break;
                        case Architecture.X86:
                            arch = "x86";
                            break;
                       case Architecture.X64:
                           arch = "x64";
                           break;
                       default:
                           throw new NotSupportedException($"Unsupported architecture: {RuntimeInformation.OSArchitecture}");
                   }
                    if (!string.IsNullOrEmpty(latestRuntimeVersion))
                    {
                        // Construct the download URL using the latest version
                        string downloadUrl = $"https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-{latestRuntimeVersion}-windows-{arch}-installer";
                        return (latestRuntimeVersion, downloadUrl);
                    }
                }

                return ("Unknown", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching latest version: {ex.Message}");
                return ("Unknown", "");
            }
        }
        #endregion Latest Online Version Check
    }
}
