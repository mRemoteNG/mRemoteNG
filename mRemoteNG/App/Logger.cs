using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Config;
using mRemoteNG.Properties;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public class Logger
    {
        public static readonly Logger Instance = new Logger();

        public ILog Log { get; private set; }

        public static string DefaultLogPath => BuildLogFilePath();

        private Logger()
        {
            Initialize();
        }

        private void Initialize()
        {
            XmlConfigurator.Configure(LogManager.CreateRepository("mRemoteNG"));
            if (string.IsNullOrEmpty(Properties.OptionsNotificationsPage.Default.LogFilePath))
                Properties.OptionsNotificationsPage.Default.LogFilePath = BuildLogFilePath();

            SetLogPath(Properties.OptionsNotificationsPage.Default.LogToApplicationDirectory ? DefaultLogPath : Properties.OptionsNotificationsPage.Default.LogFilePath);
        }

        public void SetLogPath(string path)
        {
            var repository = LogManager.GetRepository("mRemoteNG");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            var appenders = repository.GetAppenders();

            foreach (var appender in appenders)
            {
                var fileAppender = (RollingFileAppender)appender;
                if (fileAppender == null || fileAppender.Name != "LogFileAppender") continue;
                fileAppender.File = path;
                fileAppender.ActivateOptions();
            }

            Log = LogManager.GetLogger("mRemoteNG", "Logger");
        }

        private static string BuildLogFilePath()
        {
            var logFilePath = Runtime.IsPortableEdition ? GetLogPathPortableEdition() : GetLogPathNormalEdition();
            var logFileName = Path.ChangeExtension(Application.ProductName, ".log");
            if (logFileName == null) return "mRemoteNG.log";
            var logFile = Path.Combine(logFilePath, logFileName);
            return logFile;
        }

        private static string GetLogPathNormalEdition()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                Application.ProductName);
        }

        private static string GetLogPathPortableEdition()
        {
            return Application.StartupPath;
        }
    }
}