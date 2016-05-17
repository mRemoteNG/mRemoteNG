using System;
using System.Management;
using System.Collections;

namespace CustomActions
{
    public class InstalledWindowsUpdateGatherer
    {
        private ManagementScope _managementScope;
        private ManagementClass _managementClass;

        public InstalledWindowsUpdateGatherer()
        {
            _managementScope = Connect();
            _managementClass = new ManagementClass("Win32_QuickFixEngineering");
        }


        public ManagementScope Connect()
        {
            try
            {
                return new ManagementScope(@"root\cimv2");
            }
            catch (ManagementException e)
            {
                Console.WriteLine("Failed to connect", e.Message);
                throw;
            }
        }

        public ArrayList GetInstalledUpdates()
        {
            string query = "SELECT * FROM Win32_QuickFixEngineering";
            ArrayList installedUpdates = new ArrayList();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(_managementScope, new ObjectQuery(query));
            foreach(ManagementObject queryObj in searcher.Get())
            {
                installedUpdates.Add(queryObj["HotFixID"]);
            }
            return installedUpdates;
        }

        public bool IsUpdateInstalled(string KB)
        {
            bool updateIsInstalled = false;
            string query = string.Format("SELECT HotFixID FROM Win32_QuickFixEngineering WHERE HotFixID='{0}'", KB);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(_managementScope, new ObjectQuery(query));
            if (searcher.Get().Count > 0)
                updateIsInstalled = true;
            return updateIsInstalled;
        }
    }
}