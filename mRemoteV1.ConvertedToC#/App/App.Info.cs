using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Environment;
using System.Threading;

namespace mRemoteNG.App
{
	namespace Info
	{
		public class General
		{
			public static readonly string URLHome = "http://www.mremoteng.org/";
			public static readonly string URLDonate = "http://donate.mremoteng.org/";
			public static readonly string URLForum = "http://forum.mremoteng.org/";
			public static readonly string URLBugs = "http://bugs.mremoteng.org/";
			public static readonly string HomePath = mRemoteNG.My.MyProject.Application.Info.DirectoryPath;
			public static string EncryptionKey = "mR3m";
			public static string ReportingFilePath = "";
			public static readonly string PuttyPath = mRemoteNG.My.MyProject.Application.Info.DirectoryPath + "\\PuTTYNG.exe";
			public static string UserAgent {
				get {
					List<string> details = new List<string>();
					details.Add("compatible");
					if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
						details.Add(string.Format("Windows NT {0}.{1}", Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor));
					} else {
						details.Add(Environment.OSVersion.VersionString);
					}
					if (mRemoteNG.Tools.EnvironmentInfo.IsWow64)
						details.Add("WOW64");
					details.Add(Thread.CurrentThread.CurrentUICulture.Name);
					details.Add(string.Format(".NET CLR {0}", System.Environment.Version.ToString()));
					string detailsString = string.Join("; ", details.ToArray());
                    
					return string.Format("Mozilla/4.0 ({0}) {1}/{2}", detailsString, Application.ProductName, Application.ProductVersion);
				}
			}
            public static readonly bool IsPortable =
#if PORTABLE
 true;
#else 
                false; 
#endif
		}

		public class Settings
		{
            public static readonly string SettingsPath =
                General.IsPortable ?
                General.HomePath :
                System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), Application.ProductName);
			
			public static readonly string LayoutFileName = "pnlLayout.xml";
			public static readonly string ExtAppsFilesName = "extApps.xml";
			public const string ThemesFileName = "Themes.xml";
		}

		public class Update
		{
			public static string FileName {
				
				get {
                    #if DEBUG
					return "update-debug.txt";
					#endif
					switch (mRemoteNG.My.MySettingsProperty.Settings.UpdateChannel.ToLowerInvariant()) {
						case "beta":
							return "update-beta.txt";
						case "debug":
							return "update-debug.txt";
						default:
							return "update.txt";
					}
				}
			}
		}

		public class Connections
		{
			public static readonly string DefaultConnectionsPath = mRemoteNG.App.Info.Settings.SettingsPath;
			public static readonly string DefaultConnectionsFile = "confCons.xml";
			public static readonly string DefaultConnectionsFileNew = "confConsNew.xml";
			public static readonly double ConnectionFileVersion = 2.5;
		}

		public class Credentials
		{
			public static readonly string CredentialsPath = mRemoteNG.App.Info.Settings.SettingsPath;
			public static readonly string CredentialsFile = "confCreds.xml";
			public static readonly string CredentialsFileNew = "confCredsNew.xml";
			public static readonly double CredentialsFileVersion = 1.0;
		}
	}
}
