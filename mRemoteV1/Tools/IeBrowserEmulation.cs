using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using mRemoteNG.App;


namespace mRemoteNG.Tools
{
	public class IeBrowserEmulation
	{
		private const string BrowserEmulationKey = "Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";
		private static int _previousIeBrowserEmulationValue = 0;
		public static void Register()
		{
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
				if (registryKey == null)
				{
					Registry.CurrentUser.CreateSubKey(BrowserEmulationKey);
					registryKey = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
					if (registryKey == null)
					{
						return ;
					}
				}
				string exeName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
				_previousIeBrowserEmulationValue = Convert.ToInt32(registryKey.GetValue(exeName, 0));
				registryKey.SetValue(exeName, 11000, RegistryValueKind.DWord);
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "IeBrowserEmulation.Register() failed.", ex: ex, logOnly: true);
			}
		}
			
		public static void Unregister()
		{
            #if PORTABLE
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
				if (registryKey == null)
				{
					return ;
				}
				string exeName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
				if (_previousIeBrowserEmulationValue == 0)
				{
					registryKey.DeleteValue(exeName);
				}
				else
				{
					registryKey.SetValue(exeName, _previousIeBrowserEmulationValue, RegistryValueKind.DWord);
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddExceptionMessage(message: "IeBrowserEmulation.Unregister() failed.", ex: ex, logOnly: true);
			}
            #endif
		}
	}
}
