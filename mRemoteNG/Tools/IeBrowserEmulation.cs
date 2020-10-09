using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;
using mRemoteNG.App;

namespace mRemoteNG.Tools
{
    public class IeBrowserEmulation
    {
        // found this here:
        // http://www.neowin.net/forum/topic/1077469-vbnet-webbrowser-control-does-not-load-javascript/#comment-596755046

        private static void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                using (var key = Registry.CurrentUser.CreateSubKey(
                                                                   string
                                                                       .Concat("Software\\Wow6432Node\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\",
                                                                               feature),
                                                                   RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    key?.SetValue(appName, value, RegistryValueKind.DWord);
                }
            }


            using (var key = Registry.CurrentUser.CreateSubKey(
                                                               string
                                                                   .Concat("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\",
                                                                           feature),
                                                               RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key?.SetValue(appName, value, RegistryValueKind.DWord);
            }
        }

#if PORTABLE
        private static void DeleteBrowserFeatureControlKey(string feature, string appName)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                using (var key = Registry.CurrentUser.OpenSubKey(
                                                                 string
                                                                     .Concat("Software\\Wow6432Node\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\",
                                                                             feature),
                                                                 RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    if (key?.GetValueNames().Contains(appName) ?? false)
                        key.DeleteValue(appName);
                }
            }


            using (var key = Registry.CurrentUser.CreateSubKey(
                                                               string
                                                                   .Concat("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\",
                                                                           feature),
                                                               RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                if (key?.GetValueNames().Contains(appName) ?? false)
                    key.DeleteValue(appName);
            }
        }
#endif

        private static void SetBrowserFeatureControl()
        {
            // http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

            // FeatureControl settings are per-process
            var fileName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            // make sure the control is not running inside Visual Studio Designer
            if (string.Compare(fileName, "devenv.exe", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(fileName, "XDesProc.exe", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return;
            }

            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode());
            // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
            SetBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_DOMSTORAGE", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_GPU_RENDERING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName, 1);
        }

#if PORTABLE
        private static void DeleteBrowserFeatureControl()
        {
            // http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

            // FeatureControl settings are per-process
            var fileName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            // make sure the control is not running inside Visual Studio Designer
            if (string.Compare(fileName, "devenv.exe", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(fileName, "XDesProc.exe", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return;
            }

            DeleteBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName);
            // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
            DeleteBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_DOMSTORAGE", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_GPU_RENDERING", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS", fileName);
            DeleteBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName);
        }
#endif

        private static uint GetBrowserEmulationMode()
        {
            // https://msdn.microsoft.com/en-us/library/ee330730%28v=vs.85%29.aspx

            var browserVersion = 9;
            // default to IE9.

            using (var ieKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer",
                                                                RegistryKeyPermissionCheck.ReadSubTree,
                                                                RegistryRights.QueryValues))
            {
                if (ieKey != null)
                {
                    var version = ieKey.GetValue("svcVersion");
                    if (null == version)
                    {
                        version = ieKey.GetValue("Version");
                        if (version == null)
                        {
                            throw new ApplicationException("Microsoft Internet Explorer is required!");
                        }
                    }

                    int.TryParse(version.ToString().Split('.')[0], out browserVersion);
                }
            }

            uint mode;

            switch (browserVersion)
            {
                // https://support.microsoft.com/en-us/lifecycle#gp/Microsoft-Internet-Explorer
                // IE 7 & 8 are basically not supported any more...
#if OLD_BROWSERS
                // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. Default value for applications hosting the WebBrowser Control.
				case 7:
					mode = 7000;
					break; 

                // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. Default value for Internet Explorer 8
				case 8:
					mode = 8000;
					break; 
#endif
                // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
                case 9:
                    mode = 9000;
                    break;

                // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode. Default value for Internet Explorer 10.
                case 10:
                    mode = 10000;
                    break;

                // IE11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 edge mode. Default value for IE11.
                case 11:
                    mode = 11000;
                    break;

                default:
                    // use IE9 mode by default
                    mode = 9000;
                    break;
            }

            return mode;
        }

        public static void Register()
        {
            try
            {
                SetBrowserFeatureControl();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage("IeBrowserEmulation.Register() failed.", ex);
            }
        }


        public static void Unregister()
        {
#if PORTABLE
            try
            {
                DeleteBrowserFeatureControl();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionMessage("IeBrowserEmulation.Unregister() failed.", ex);
            }
#endif
        }

        private IeBrowserEmulation()
        {
        }
    }
}