using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;
using static System.Environment;


namespace mRemoteNG.App.Info
{
    [SupportedOSPlatform("windows")]
    public static class GeneralAppInfo
    {
        public const string UrlHome = "https://www.mremoteng.org";
        public const string UrlDonate = "https://mremoteng.org/contribute";
        public const string UrlForum = "https://www.reddit.com/r/mRemoteNG";
        public const string UrlBugs = "https://bugs.mremoteng.org";
        public const string UrlDocumentation = "https://mremoteng.readthedocs.io/en/latest/";
        public static string ApplicationVersion = Application.ProductVersion;
        public static readonly string ProductName = Application.ProductName;
        public static readonly string Copyright = ((AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false)).Copyright;
        public static readonly string HomePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        //public static string ReportingFilePath = "";
        public static readonly string PuttyPath = HomePath + "\\PuTTYNG.exe";

        public static string UserAgent
        {
            get
            {
                var details = new List<string>
                {
                    "compatible",
                    OSVersion.Platform == PlatformID.Win32NT
                        ? $"Windows NT {OSVersion.Version.Major}.{OSVersion.Version.Minor}"
                        : OSVersion.VersionString
                };
                if (Is64BitProcess)
                {
                    details.Add("WOW64");
                }

                details.Add(Thread.CurrentThread.CurrentUICulture.Name);
                details.Add($".NET CLR {Environment.Version}");
                var detailsString = string.Join("; ", details.ToArray());

                return $"Mozilla/5.0 ({detailsString}) {ProductName}/{ApplicationVersion}";
            }
        }

        public static Version GetApplicationVersion()
        {
            System.Version.TryParse(ApplicationVersion, out var v);
            return v;
        }
    }
}