using System;
using System.Management;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.App.Initialization
{
    [SupportedOSPlatform("windows")]
    public class StartupDataLogger
    {
        private readonly MessageCollector _messageCollector;

        public StartupDataLogger(MessageCollector messageCollector)
        {
            if (messageCollector == null)
                throw new ArgumentNullException(nameof(messageCollector));

            _messageCollector = messageCollector;
        }

        public void LogStartupData()
        {
            LogApplicationData();
            LogCmdLineArgs();
            LogSystemData();
            LogClrData();
            LogCultureData();
        }

        private void LogSystemData()
        {
            var osData = GetOperatingSystemData();
            var architecture = GetArchitectureData();
            var nonEmptyData = Array.FindAll(new[] {osData, architecture}, s => !string.IsNullOrEmpty(s));
            var data = string.Join(" ", nonEmptyData);
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }

        private string GetOperatingSystemData()
        {
            var osVersion = string.Empty;
            var servicePack = string.Empty;

            try
            {
                foreach (var o in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE Primary=True")
                    .Get())
                {
                    var managementObject = (ManagementObject)o;
                    osVersion = Convert.ToString(managementObject.GetPropertyValue("Caption")).Trim();
                    servicePack = GetOSServicePack(servicePack, managementObject);
                }
            }
            catch (Exception ex)
            {
                _messageCollector.AddExceptionMessage("Error retrieving operating system information from WMI.", ex);
            }

            var osData = string.Join(" ", osVersion, servicePack);
            return osData;
        }

        private string GetOSServicePack(string servicePack, ManagementObject managementObject)
        {
            var servicePackNumber = Convert.ToInt32(managementObject.GetPropertyValue("ServicePackMajorVersion"));
            if (servicePackNumber != 0)
            {
                servicePack = $"Service Pack {servicePackNumber}";
            }

            return servicePack;
        }

        private string GetArchitectureData()
        {
            var architecture = string.Empty;
            try
            {
                foreach (var o in new ManagementObjectSearcher("SELECT AddressWidth FROM Win32_Processor WHERE DeviceID=\'CPU0\'")
                    .Get())
                {
                    var managementObject = (ManagementObject)o;
                    var addressWidth = Convert.ToInt32(managementObject.GetPropertyValue("AddressWidth"));
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
            var data = $"{Application.ProductName} {Application.ProductVersion}";
            if (Runtime.IsPortableEdition)
                data += $" {Language.PortableEdition}";
            data += " starting.";
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }

        private void LogCmdLineArgs()
        {
            var data = $"Command Line: {string.Join(" ", Environment.GetCommandLineArgs())}";
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }

        private void LogClrData()
        {
            var data = $"Microsoft .NET CLR {Environment.Version}";
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }

        private void LogCultureData()
        {
            var data =
                $"System Culture: {Thread.CurrentThread.CurrentUICulture.Name}/{Thread.CurrentThread.CurrentUICulture.NativeName}";
            _messageCollector.AddMessage(MessageClass.InformationMsg, data, true);
        }
    }
}