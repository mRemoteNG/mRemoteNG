// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

//using System.Environment;
using System.Threading;


namespace mRemoteNG.App.Info
{
	public class General
	{
		public static readonly string URLHome = "http://www.mremoteng.org/";
		public static readonly string URLDonate = "http://donate.mremoteng.org/";
		public static readonly string URLForum = "http://forum.mremoteng.org/";
		public static readonly string URLBugs = "http://bugs.mremoteng.org/";
		public static readonly string HomePath = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath;
		public static string EncryptionKey = "mR3m";
		public static string ReportingFilePath = "";
		public static readonly string PuttyPath = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\PuTTYNG.exe";
public static string UserAgent
		{
			get
			{
				List<string> details = new List<string>();
				details.Add("compatible");
				if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					details.Add(string.Format("Windows NT {0}.{1}", System.Environment.OSVersion.Version.Major, System.Environment.OSVersion.Version.Minor));
				}
				else
				{
					details.Add(System.Environment.OSVersion.VersionString);
				}
				if (Tools.EnvironmentInfo.IsWow64)
				{
					details.Add("WOW64");
				}
				details.Add(Thread.CurrentThread.CurrentUICulture.Name);
				details.Add(string.Format(".NET CLR {0}", Version.ToString()));
				string detailsString = string.Join("; ", details.ToArray());
						
				return string.Format("Mozilla/4.0 ({0}) {1}/{2}", detailsString, System.Windows.Forms.Application.ProductName, System.Windows.Forms.Application.ProductVersion);
			}
		}
	}
			
	public class Settings
	{
#if !PORTABLE
		public static readonly string SettingsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\" + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName;
#else
		public static readonly string SettingsPath = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath;
#endif
		public static readonly string LayoutFileName = "pnlLayout.xml";
		public static readonly string ExtAppsFilesName = "extApps.xml";
		public const string ThemesFileName = "Themes.xml";
	}
			
	public class Update
	{
public static string FileName
		{
			get
			{
#if DEBUG
				return "update-debug.txt";
#endif
//						
//						if ((string) (My.Settings.Default.UpdateChannel.ToLowerInvariant()) == "beta")
//						{
//							return "update-beta.txt";
//							}
//							else if ((string) (My.Settings.Default.UpdateChannel.ToLowerInvariant()) == "debug")
//							{
//								return "update-debug.txt";
//								}
//								else
//								{
//									return "update.txt";
//									}
						}
					}
				}
						
	public class Connections
	{
		public static readonly string DefaultConnectionsPath = App.Info.Settings.SettingsPath;
		public static readonly string DefaultConnectionsFile = "confCons.xml";
		public static readonly string DefaultConnectionsFileNew = "confConsNew.xml";
		public static readonly double ConnectionFileVersion = 2.5;
	}
						
	public class Credentials
	{
		public static readonly string CredentialsPath = App.Info.Settings.SettingsPath;
		public static readonly string CredentialsFile = "confCreds.xml";
		public static readonly string CredentialsFileNew = "confCredsNew.xml";
		public static readonly double CredentialsFileVersion = 1.0;
	}			
}
