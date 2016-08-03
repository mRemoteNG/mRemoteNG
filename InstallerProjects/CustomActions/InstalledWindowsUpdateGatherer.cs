using System;
using System.Management;
using System.Collections;

namespace CustomActions
{
    public class InstalledWindowsUpdateGatherer
    {
        private readonly ManagementScope _managementScope;

        public InstalledWindowsUpdateGatherer()
        {
            _managementScope = Connect();
        }


        public ManagementScope Connect()
        {
            try
            {
                return new ManagementScope(@"root\cimv2");
            }
            catch (ManagementException e)
            {
                Console.WriteLine($"Failed to connect: {e.Message}");
                throw;
            }
        }

        public ArrayList GetInstalledUpdates()
        {
            const string query = "SELECT * FROM Win32_QuickFixEngineering";
            var installedUpdates = new ArrayList();
            var searcher = new ManagementObjectSearcher(_managementScope, new ObjectQuery(query));
            foreach(var o in searcher.Get())
            {
                var queryObj = (ManagementObject) o;
                installedUpdates.Add(queryObj["HotFixID"]);
            }
            return installedUpdates;
        }

        public bool IsUpdateInstalled(string kb)
        {
            var updateIsInstalled = false;
            var query = $"SELECT HotFixID FROM Win32_QuickFixEngineering WHERE HotFixID='{kb}'";
            var searcher = new ManagementObjectSearcher(_managementScope, new ObjectQuery(query));
            if (searcher.Get().Count > 0)
                updateIsInstalled = true;
            return updateIsInstalled;
        }
    }
}