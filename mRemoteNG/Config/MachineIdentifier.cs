using System;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace mRemoteNG.Config.MachineIdentifier
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Provides functionality to generate a consistent and unique machine identifier
    /// based on hardware properties (disk serial number, MAC address, BIOS UUID, and machine name).
    /// This class is supported only on Windows.
    /// </summary>
    public static class MachineIdentifierGenerator
    {
        /// <summary>
        /// Generates a consistent machine identifier by combining hardware-based identifiers
        /// (disk serial number, MAC address, BIOS UUID, and machine name) and hashing the result.
        /// </summary>
        /// <returns>A consistent and unique identifier for the machine.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if the method is called on a non-Windows platform.</exception>
        public static string GenerateMachineIdentifier()
        {
            // Ensure the code runs only on Windows
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("This method is supported only on Windows.");
            }

            // Retrieve hardware-based identifiers (with fallbacks)
            string diskId = GetDiskSerialNumber() ?? "NO_DISK_ID";
            string macAddress = GetMacAddress() ?? "NO_MAC_ADDRESS";
            string biosUuid = GetBiosUuid() ?? "NO_BIOS_UUID";
            string machineName = Environment.MachineName;

            // Combine them into a single string
            string combined = $"{diskId}_{macAddress}_{biosUuid}_{machineName}";

            // Hash the combined string to ensure a fixed length and improve security
            byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(combined));
            return Convert.ToHexStringLower(hashBytes);
        }

        /// <summary>
        /// Retrieves the serial number of the first physical disk using WMI.
        /// </summary>
        /// <returns>The disk serial number, or null if the serial number cannot be retrieved.</returns>
        private static string? GetDiskSerialNumber()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new("SELECT SerialNumber FROM Win32_DiskDrive"))
                {
                    foreach (ManagementObject wmi_HD in searcher.Get())
                    {
                        if (wmi_HD["SerialNumber"] != null)
                        {
                            string? serialNumber = wmi_HD["SerialNumber"].ToString()?.Trim();
                            if (!string.IsNullOrEmpty(serialNumber))
                            {
                                return serialNumber;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving disk serial number: {ex.Message}");
            }

            return null; // Return null if the disk serial number cannot be retrieved
        }

        /// <summary>
        /// Retrieves the MAC address of the first active network adapter.
        /// </summary>
        /// <returns>The MAC address, or null if the MAC address cannot be retrieved.</returns>
        private static string? GetMacAddress()
        {
            try
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in interfaces)
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        string macAddress = adapter.GetPhysicalAddress().ToString();
                        if (!string.IsNullOrEmpty(macAddress))
                        {
                            return macAddress;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving MAC address: {ex.Message}");
            }

            return null; // Return null if the MAC address cannot be retrieved
        }

        /// <summary>
        /// Retrieves the BIOS UUID of the machine using WMI.
        /// </summary>
        /// <returns>The BIOS UUID, or null if the UUID cannot be retrieved.</returns>
        private static string? GetBiosUuid()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystemProduct"))
                {
                    foreach (ManagementObject wmi_HD in searcher.Get())
                    {
                        if (wmi_HD["UUID"] != null)
                        {
                            string? uuid = wmi_HD["UUID"].ToString()?.Trim();
                            if (!string.IsNullOrEmpty(uuid))
                            {
                                return uuid;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving BIOS UUID: {ex.Message}");
            }

            return null; // Return null if the BIOS UUID cannot be retrieved
        }
    }
}