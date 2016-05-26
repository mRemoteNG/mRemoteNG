using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace CustomActions
{
    public class UninstallNSISVersions
    {
        private const string REGISTRY_PATH = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\mRemoteNG";
        private const string REGISTRY_PATH_Wow6432 = "Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\mRemoteNG";
        private RegistryKey _activeRegistryPath;


        public UninstallNSISVersions()
        {
            GetLegacymRemoteNGRegistryKeyPath();
        }

        public void UninstallLegacyVersion(bool Silent = false)
        {
            if (!IsLegacymRemoteNGInstalled())
                return;
            string uninstallString = GetLegacyUninstallString();
            string forceNonTempUninstaller = string.Format("_?={0}", uninstallString.Replace("Uninstall.exe", "").Replace(@"""", ""));
            string silentUninstall = "";
            if (Silent)
            {
                silentUninstall = "/S";
            }
            ProcessStartInfo processStartInfo = new ProcessStartInfo(uninstallString);
            processStartInfo.UseShellExecute = true;
            processStartInfo.Arguments = string.Format("{0} {1}", forceNonTempUninstaller, silentUninstall);
            Process uninstallProcess = Process.Start(processStartInfo);
            while (uninstallProcess.HasExited == false)
            {
                Debug.WriteLine("Waiting for uninstaller to exit");
            }
        }

        public bool IsLegacymRemoteNGInstalled()
        {
            return (_activeRegistryPath != null);
        }

        public string GetLegacyUninstallString()
        {
            if (IsLegacymRemoteNGInstalled())
                return _activeRegistryPath.GetValue("UninstallString").ToString();
            return "";
        }

        private void GetLegacymRemoteNGRegistryKeyPath()
        {
            GetUninstallKeyPath();
            GetUninstallKeyPath6432();
        }

        private void GetUninstallKeyPath()
        {
            try
            {
                _activeRegistryPath = Registry.LocalMachine.OpenSubKey(REGISTRY_PATH);
            }
            catch (Exception ex)
            { }
        }

        private void GetUninstallKeyPath6432()
        {
            try
            {
                _activeRegistryPath = Registry.LocalMachine.OpenSubKey(REGISTRY_PATH_Wow6432);
            }
            catch (Exception ex)
            { }
        }
    }
}