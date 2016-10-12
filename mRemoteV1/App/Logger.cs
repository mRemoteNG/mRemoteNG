﻿using System.IO;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Config;
#if !PORTABLE
using System;
#endif

namespace mRemoteNG.App
{
    public class Logger
    {
        private static readonly Logger _loggerInstance = new Logger();
        private ILog _log;

        static Logger()
        {
        }

        private Logger()
        {
            Initialize();
        }

        public static ILog Instance => _loggerInstance._log;

        private void Initialize()
        {
            XmlConfigurator.Configure();
            var logFile = BuildLogFilePath();

            var repository = LogManager.GetRepository();
            var appenders = repository.GetAppenders();

            foreach (var appender in appenders)
            {
                var fileAppender = (RollingFileAppender) appender;
                if ((fileAppender == null) || (fileAppender.Name != "LogFileAppender")) continue;
                fileAppender.File = logFile;
                fileAppender.ActivateOptions();
            }
            _log = LogManager.GetLogger("Logger");
        }

        private static string BuildLogFilePath()
        {
#if !PORTABLE
			string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);
#else
            var logFilePath = Application.StartupPath;
#endif
            var logFileName = Path.ChangeExtension(Application.ProductName, ".log");
            var logFile = Path.Combine(logFilePath, logFileName);
            return logFile;
        }
    }
}