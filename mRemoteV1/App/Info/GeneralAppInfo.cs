using System.Collections.Generic;
using System;
using System.Threading;


namespace mRemoteNG.App.Info
{
	public class GeneralAppInfo
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
				details.Add(string.Format(".NET CLR {0}", System.Environment.Version));
				string detailsString = string.Join("; ", details.ToArray());
						
				return string.Format("Mozilla/4.0 ({0}) {1}/{2}", detailsString, System.Windows.Forms.Application.ProductName, System.Windows.Forms.Application.ProductVersion);
			}
		}
	}
}