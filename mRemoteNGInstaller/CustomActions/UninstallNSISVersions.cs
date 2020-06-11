using System;
using Microsoft.Win32;
using System.Diagnostics;


namespace CustomActions
{
    public class UninstallNsisVersions
    {
        private const string RegistryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\mRemoteNG";

        private const string RegistryPathWow6432 =
            "Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\mRemoteNG";

        private RegistryKey _activeRegistryPath;


        public UninstallNsisVersions()
        {
            GetLegacymRemoteNgRegistryKeyPath();
        }

        public void UninstallLegacyVersion(bool silent = false)
        {
            if (!IsLegacymRemoteNgInstalled())
                return;
            var uninstallString = GetLegacyUninstallString();
            var forceNonTempUninstaller = $"_?={uninstallString.Replace("Uninstall.exe", "").Replace(@"""", "")}";
            var silentUninstall = "";
            if (silent) silentUninstall = "/S";
            var processStartInfo = new ProcessStartInfo(uninstallString)
            {
                UseShellExecute = true,
                Arguments = $"{forceNonTempUninstaller} {silentUninstall}"
            };
            var uninstallProcess = Process.Start(processStartInfo);
            while (uninstallProcess != null && uninstallProcess.HasExited == false)
                Debug.WriteLine("Waiting for uninstaller to exit");
        }

        public bool IsLegacymRemoteNgInstalled()
        {
            return _activeRegistryPath != null;
        }

        public string GetLegacyUninstallString()
        {
            return IsLegacymRemoteNgInstalled() ? _activeRegistryPath.GetValue("UninstallString").ToString() : "";
        }

        private void GetLegacymRemoteNgRegistryKeyPath()
        {
            GetUninstallKeyPath();
            GetUninstallKeyPath6432();
        }

        private void GetUninstallKeyPath()
        {
            try
            {
                _activeRegistryPath = Registry.LocalMachine.OpenSubKey(RegistryPath);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void GetUninstallKeyPath6432()
        {
            try
            {
                _activeRegistryPath = Registry.LocalMachine.OpenSubKey(RegistryPathWow6432);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}