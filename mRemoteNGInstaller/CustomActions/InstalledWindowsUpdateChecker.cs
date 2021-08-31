using System;
using System.Management;
using System.Collections;
using System.Collections.Generic;

namespace CustomActions
{
    public class InstalledWindowsUpdateChecker
    {
        private readonly ManagementScope _managementScope;

        public InstalledWindowsUpdateChecker()
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

        public bool IsUpdateInstalled(string kb) => IsUpdateInstalled(new[] {kb});

        public bool IsUpdateInstalled(IEnumerable<string> kbList)
        {
            var updateIsInstalled = false;
            var whereClause = BuildWhereClauseFromKbList(kbList);
            if (whereClause == "") return false;
            var query = $"SELECT HotFixID FROM Win32_QuickFixEngineering WHERE {whereClause}";
            var searcher = new ManagementObjectSearcher(_managementScope, new ObjectQuery(query));
            if (searcher.Get().Count > 0)
                updateIsInstalled = true;
            return updateIsInstalled;
        }

        private string BuildWhereClauseFromKbList(IEnumerable<string> kbList)
        {
            var whereClause = "";
            var counter = 0;
            foreach (var kb in kbList)
            {
                if (counter > 0)
                    whereClause += " OR ";
                whereClause += $"HotFixID='{kb}'";
                counter++;
            }
            return whereClause;
        }
    }
}