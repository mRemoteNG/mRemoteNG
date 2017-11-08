using System;
using System.IO;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Config;
// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App
{
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
            XmlConfigurator.Configure();
            if (string.IsNullOrEmpty(Settings.Default.LogFilePath))
                Settings.Default.LogFilePath = BuildLogFilePath();

            SetLogPath(Settings.Default.LogToApplicationDirectory ? DefaultLogPath : Settings.Default.LogFilePath);
        }

        public void SetLogPath(string path)
        {
            var repository = LogManager.GetRepository();
            var appenders = repository.GetAppenders();

            foreach (var appender in appenders)
            {
                var fileAppender = (RollingFileAppender)appender;
                if (fileAppender == null || fileAppender.Name != "LogFileAppender") continue;
                fileAppender.File = path;
                fileAppender.ActivateOptions();
            }
            Log = LogManager.GetLogger("Logger");
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
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);
        }

        private static string GetLogPathPortableEdition()
        {
            return Application.StartupPath;
        }
    }
}