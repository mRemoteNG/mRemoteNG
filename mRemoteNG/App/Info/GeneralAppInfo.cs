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
        public const string UrlHome = "https://mremoteng.org";
        public const string UrlDonate = "https://mremoteng.org/contribute";
        public const string UrlForum = "https://github.com/orgs/mRemoteNG/discussions";
        public const string UrlChat = "https://app.element.io/#/room/#mremoteng:matrix.org";
        public const string UrlCommunity = "https://www.reddit.com/r/mRemoteNG";
        public const string UrlBugs = "https://github.com/mRemoteNG/mRemoteNG/issues/new";
        public const string UrlDocumentation = "https://mremoteng.readthedocs.io/en/latest/";
        public static readonly string ApplicationVersion = Application.ProductVersion;
        public static readonly string? ProductName = Application.ProductName;
        public static readonly string? Copyright = (Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false) as AssemblyCopyrightAttribute)?.Copyright;
        public static readonly string? HomePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        //public static string ReportingFilePath = "";
        private static readonly string puttyPath = HomePath + "\\PuTTYNG.exe";

        public static string UserAgent
        {
            get
            {
                List<string> details =
                [
                    "compatible",
                    OSVersion.Platform == PlatformID.Win32NT
                        ? $"Windows NT {OSVersion.Version.Major}.{OSVersion.Version.Minor}"
                        : OSVersion.VersionString
                ];
                if (Is64BitProcess)
                {
                    details.Add("WOW64");
                }

                details.Add(Thread.CurrentThread.CurrentUICulture.Name);
                details.Add($".NET CLR {Environment.Version}");
                string detailsString = string.Join("; ", [.. details]);

                return $"Mozilla/5.0 ({detailsString}) {ProductName}/{ApplicationVersion}";
            }
        }

        public static string PuttyPath => puttyPath;

        public static Version? GetApplicationVersion()
        {
            string cleanedVersion = ApplicationVersion.Split(' ')[0].Replace("(", "").Replace(")", "").Replace("Build", "");
            cleanedVersion = cleanedVersion + "." + ApplicationVersion.Split(' ')[^1].Replace(")", "");

            _ = System.Version.TryParse(cleanedVersion, out Version? parsedVersion);
            return parsedVersion;
        }
    }
}