using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using mRemoteNG.App.Runtime;

namespace mRemoteNG.Tools
{
	public class IeBrowserEmulation
	{
		private const string BrowserEmulationKey = "Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";
		private static int _previousIeBrowserEmulationValue = 0;
		public static void Register()
		{
			try {
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
				if (registryKey == null) {
					Registry.CurrentUser.CreateSubKey(BrowserEmulationKey);
					registryKey = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
					if (registryKey == null)
						return;
				}
				string exeName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
				_previousIeBrowserEmulationValue = registryKey.GetValue(exeName, 0);
				registryKey.SetValue(exeName, 11000, RegistryValueKind.DWord);
			} catch (Exception ex) {
				mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("IeBrowserEmulation.Register() failed.", ex, , true);
			}
		}

		#if PORTABLE
		public static void Unregister()
		{
			try {
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
				if (registryKey == null)
					return;
				string exeName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
				if (_previousIeBrowserEmulationValue == 0) {
					registryKey.DeleteValue(exeName);
				} else {
					registryKey.SetValue(exeName, _previousIeBrowserEmulationValue, RegistryValueKind.DWord);
				}
			} catch (Exception ex) {
				mRemoteNG.App.Runtime.MessageCollector.AddExceptionMessage("IeBrowserEmulation.Unregister() failed.", ex, , true);
			}
			#endif
		}
	}
}

