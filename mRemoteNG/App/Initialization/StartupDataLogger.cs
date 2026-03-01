using System;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.App.Initialization
{
    [SupportedOSPlatform("windows")]
    public class StartupDataLogger(MessageCollector messageCollector)
    {
        private readonly MessageCollector _messageCollector = messageCollector ?? throw new ArgumentNullException(nameof(messageCollector));

        public void LogStartupData()
        {
            LogApplicationData();
            LogSettingsData();
            LogCmdLineArgs();
            LogSystemData();
            LogClrData();
            LogCultureData();
        }

        private void LogSystemData()
        {
            string osData = GetOperatingSystemData();
            string architecture = GetArchitectureData();
            string[] nonEmptyData = Array.FindAll(new[] {osData, architecture}, s => !string.IsNullOrEmpty(s));
            string data = string.Join(" ", nonEmptyData);
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }

        private string GetOperatingSystemData()
        {
            string osVersion = string.Empty;
            string servicePack = string.Empty;

            try
            {
                foreach (ManagementBaseObject o in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE Primary=True")
                    .Get())
                {
                    ManagementObject managementObject = (ManagementObject)o;
                    osVersion = Convert.ToString(managementObject.GetPropertyValue("Caption"), CultureInfo.InvariantCulture)?.Trim() ?? string.Empty;
                    servicePack = GetOSServicePack(servicePack, managementObject);
                }
            }
            catch (Exception ex)
            {
                _messageCollector.AddExceptionMessage("Error retrieving operating system information from WMI.", ex);
            }

            string osData = string.Join(" ", osVersion, servicePack);
            return osData;
        }

        private static string GetOSServicePack(string servicePack, ManagementObject managementObject)
        {
            int servicePackNumber = Convert.ToInt32(managementObject.GetPropertyValue("ServicePackMajorVersion"), CultureInfo.InvariantCulture);
            if (servicePackNumber != 0)
            {
                servicePack = $"Service Pack {servicePackNumber}";
            }

            return servicePack;
        }

        private string GetArchitectureData()
        {
            string architecture = string.Empty;
            try
            {
                foreach (ManagementBaseObject o in new ManagementObjectSearcher("SELECT AddressWidth FROM Win32_Processor WHERE DeviceID=\'CPU0\'").Get())
                {
                    ManagementObject managementObject = (ManagementObject)o;
                    int addressWidth = Convert.ToInt32(managementObject.GetPropertyValue("AddressWidth"), CultureInfo.InvariantCulture);
                    architecture = $"{addressWidth}-bit";
                }
            }
            catch (Exception ex)
            {
                _messageCollector.AddExceptionMessage("Error retrieving operating system address width from WMI.", ex);
            }

            return architecture;
        }

        private void LogApplicationData()
        {
            string data = $"{Application.ProductName} {Application.ProductVersion}";
            if (Runtime.IsPortableEdition)
                data += $" {Language.PortableEdition}";
            data += " starting.";
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }

        private void LogSettingsData()
        {
            if (string.IsNullOrWhiteSpace(SettingsFileInfo.UserSettingsFilePath))
            {
                _messageCollector.AddMessage(MessageClass.InformationMsg,
                    "User settings file path could not be determined.", true);
                return;
            }

            string path = SettingsFileInfo.UserSettingsFilePath;
            string detail;
            try
            {
                FileInfo fi = new(path);
                detail = fi.Exists
                    ? $"User settings file: {path} (size: {fi.Length} bytes, modified: {fi.LastWriteTime:yyyy-MM-dd HH:mm:ss})"
                    : $"User settings file: {path} (file does not exist — using defaults)";
            }
            catch (Exception ex)
            {
                detail = $"User settings file: {path} (could not read file info: {ex.Message})";
            }

            _messageCollector.AddMessage(MessageClass.InformationMsg, detail, true);
        }

        private void LogCmdLineArgs()
        {
            string data = $"Command Line: {string.Join(" ", Environment.GetCommandLineArgs())}";
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }

        private void LogClrData()
        {
            string data = $"Microsoft .NET CLR {Environment.Version}";
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }

        private void LogCultureData()
        {
            string data = $"System Culture: {Thread.CurrentThread.CurrentUICulture.Name}/{Thread.CurrentThread.CurrentUICulture.NativeName}";
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }
    }
}
