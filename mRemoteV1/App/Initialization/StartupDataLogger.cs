using System;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using log4net;


namespace mRemoteNG.App.Initialization
{
    public class StartupDataLogger
    {
        private readonly ILog _logger;

        public StartupDataLogger(ILog logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public void Execute()
        {
            if (!Settings.Default.WriteLogFile) return;
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
            _logger.InfoFormat(string.Join(" ", Array.FindAll(new[] { osData, architecture }, s => !string.IsNullOrEmpty(Convert.ToString(s)))));
        }

        private string GetOperatingSystemData()
        {
            var osVersion = string.Empty;
            var servicePack = string.Empty;

            try
            {
                foreach (var o in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE Primary=True").Get())
                {
                    var managementObject = (ManagementObject)o;
                    osVersion = Convert.ToString(managementObject.GetPropertyValue("Caption")).Trim();
                    servicePack = GetOSServicePack(servicePack, managementObject);
                }
            }
            catch (Exception ex)
            {
                _logger.WarnFormat($"Error retrieving operating system information from WMI. {ex.Message}");
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
                foreach (var o in new ManagementObjectSearcher("SELECT * FROM Win32_Processor WHERE DeviceID=\'CPU0\'").Get())
                {
                    var managementObject = (ManagementObject)o;
                    var addressWidth = Convert.ToInt32(managementObject.GetPropertyValue("AddressWidth"));
                    architecture = $"{addressWidth}-bit";
                }
            }
            catch (Exception ex)
            {
                _logger.WarnFormat($"Error retrieving operating system address width from WMI. {ex.Message}");
            }
            return architecture;
        }

        private void LogApplicationData()
        {
#if !PORTABLE
            _logger.InfoFormat($"{Application.ProductName} {Application.ProductVersion} starting.");
#else
            _logger.InfoFormat($"{Application.ProductName} {Application.ProductVersion} {Language.strLabelPortableEdition} starting.");
#endif
        }

        private void LogCmdLineArgs()
        {
            _logger.InfoFormat($"Command Line: {Environment.GetCommandLineArgs()}");
        }

        private void LogClrData()
        {
            _logger.InfoFormat($"Microsoft .NET CLR {Environment.Version}");
        }

        private void LogCultureData()
        {
            _logger.InfoFormat($"System Culture: {Thread.CurrentThread.CurrentUICulture.Name}/{Thread.CurrentThread.CurrentUICulture.NativeName}");
        }
    }
}