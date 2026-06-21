using System;
using System.Management;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CustomActions
{
    public class InstalledWindowsUpdateChecker
    {
        private readonly ManagementScope _managementScope;
        private static readonly Regex KbPattern = new Regex(@"^(KB)?\d+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
                var sanitizedKb = SanitizeKbId(kb);
                if (string.IsNullOrEmpty(sanitizedKb))
                    continue; // Skip invalid KB IDs
                
                if (counter > 0)
                    whereClause += " OR ";
                whereClause += $"HotFixID='{sanitizedKb}'";
                counter++;
            }
            return whereClause;
        }

        /// <summary>
        /// Sanitizes a KB ID to prevent WQL injection attacks.
        /// KB IDs must match the pattern: optional "KB" prefix followed by digits,
        /// or just digits. Any other characters are rejected.
        /// </summary>
        /// <param name="kbId">The KB ID to sanitize</param>
        /// <returns>The sanitized KB ID, or empty string if invalid</returns>
        private string SanitizeKbId(string kbId)
        {
            if (string.IsNullOrWhiteSpace(kbId))
                return string.Empty;

            // KB IDs should match the pattern: optional KB prefix followed by digits
            // (e.g., KB1234567 or 1234567)
            // Trim whitespace and check if it matches the expected pattern
            var trimmedKb = kbId.Trim();
            if (!KbPattern.IsMatch(trimmedKb))
                return string.Empty;

            // Normalize to uppercase
            var normalizedKb = trimmedKb.ToUpperInvariant();
            
            // Ensure KB prefix is present (Win32_QuickFixEngineering always uses the KB prefix)
            if (!normalizedKb.StartsWith("KB"))
                normalizedKb = "KB" + normalizedKb;

            return normalizedKb;
        }
    }
}