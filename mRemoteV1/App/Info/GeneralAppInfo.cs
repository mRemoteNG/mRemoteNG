using System.Collections.Generic;
using System;
using System.Threading;
using static System.Environment;


namespace mRemoteNG.App.Info
{
	public class GeneralAppInfo
	{
		public static readonly string UrlHome = "http://www.mremoteng.org/";
		public static readonly string UrlDonate = "http://donate.mremoteng.org/";
		public static readonly string UrlForum = "http://forum.mremoteng.org/";
		public static readonly string UrlBugs = "http://bugs.mremoteng.org/";
		public static readonly string HomePath = new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase().Info.DirectoryPath;
		public static string EncryptionKey = "mR3m";
		public static string ReportingFilePath = "";
		public static readonly string PuttyPath = new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase().Info.DirectoryPath + "\\PuTTYNG.exe";
        public static string UserAgent
		{
			get
			{
			    var details = new List<string>();
			    details.Add("compatible");
			    details.Add(OSVersion.Platform == PlatformID.Win32NT ? $"Windows NT {OSVersion.Version.Major}.{OSVersion.Version.Minor}": OSVersion.VersionString);
			    if (Tools.EnvironmentInfo.IsWow64)
				{
					details.Add("WOW64");
				}
				details.Add(Thread.CurrentThread.CurrentUICulture.Name);
				details.Add($".NET CLR {Environment.Version}");
				string detailsString = string.Join("; ", details.ToArray());
						
				return $"Mozilla/4.0 ({detailsString}) {System.Windows.Forms.Application.ProductName}/{System.Windows.Forms.Application.ProductVersion}";
			}
		}
	}
}