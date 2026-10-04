using System;
using System.IO;
using System.Net;
using System.Runtime.Versioning;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public class Logger
    {
        public static readonly Logger Instance = new();

        internal const string SyslogAppenderName = "SyslogAppender";

        private IAppender? _fileAppender;

        public ILog Log { get; private set; } = null!; // initialized via SetLogPath() called from the constructor

        public static string DefaultLogPath => BuildLogFilePath();

        private Logger()
        {
            Initialize();
        }

        private void Initialize()
        {
            XmlConfigurator.Configure(LogManager.CreateRepository("mRemoteNG"));

            if (string.IsNullOrEmpty(Properties.OptionsNotificationsPage.Default.LogFilePath))
            {
                Properties.OptionsNotificationsPage.Default.LogFilePath = BuildLogFilePath();
            }

            SetLogPath(Properties.OptionsNotificationsPage.Default.LogToApplicationDirectory ? DefaultLogPath : Properties.OptionsNotificationsPage.Default.LogFilePath);
        }

        public void SetLogPath(string path)
        {
            ILoggerRepository repository = LogManager.GetRepository("mRemoteNG");

            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            IAppender[] appenders = repository.GetAppenders();

            foreach (IAppender appender in appenders)
            {
                if (appender is not RollingFileAppender fileAppender) continue;
                if (fileAppender is not { Name: "LogFileAppender" }) continue;
                _fileAppender = fileAppender;
            }

            if (_fileAppender is RollingFileAppender storedFileAppender)
            {
                storedFileAppender.File = path;
                storedFileAppender.ActivateOptions();
            }

            ConfigureSyslog();

            Log = LogManager.GetLogger("mRemoteNG", "Logger");
        }

        /// <summary>
        /// Adds or removes the syslog appender on the "mRemoteNG" repository based on the
        /// current notification options. When enabled, log messages are forwarded to the
        /// configured remote syslog server via UDP (RFC 3164).
        /// </summary>
        public void ConfigureSyslog()
        {
            if (LogManager.GetRepository("mRemoteNG") is not Hierarchy hierarchy)
                return;

            log4net.Repository.Hierarchy.Logger root = hierarchy.Root;

            // Remove any previously attached syslog appender so settings changes take effect.
            if (root.GetAppender(SyslogAppenderName) is IAppender existing)
            {
                root.RemoveAppender(existing);
                if (existing is IDisposable disposable)
                    disposable.Dispose();
            }

            bool enabled = Properties.OptionsNotificationsPage.Default.LogToSyslog;
            string host = Properties.OptionsNotificationsPage.Default.SyslogServerHost;
            int port = Properties.OptionsNotificationsPage.Default.SyslogServerPort;

            if (!enabled)
            {
                RestoreFileAppender(root);
                return;
            }

            try
            {
                RemoteSyslogAppender appender = BuildSyslogAppender(host, port);
                root.AddAppender(appender);
                if (root.GetAppender("LogFileAppender") is IAppender fileAppender)
                    root.RemoveAppender(fileAppender);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or System.Net.Sockets.SocketException)
            {
                Properties.OptionsNotificationsPage.Default.LogToSyslog = false;
                LogManager.GetLogger("mRemoteNG", "Logger").Warn("Syslog has been disabled because its server endpoint is invalid.", ex);
                RestoreFileAppender(root);
            }

            hierarchy.Configured = true;
        }

        private void RestoreFileAppender(log4net.Repository.Hierarchy.Logger root)
        {
            if (_fileAppender != null && root.GetAppender("LogFileAppender") == null)
                root.AddAppender(_fileAppender);
        }

        /// <summary>
        /// Builds and activates a <see cref="RemoteSyslogAppender"/> targeting the given host and port.
        /// </summary>
        internal static RemoteSyslogAppender BuildSyslogAppender(string host, int port)
        {
            if (port is < 1 or > 65535)
                throw new ArgumentOutOfRangeException(nameof(port), "Syslog port must be between 1 and 65535.");

            IPAddress[] addresses = Dns.GetHostAddresses(host);
            if (addresses.Length == 0)
                throw new InvalidOperationException("The syslog server host did not resolve to an IP address.");

            var layout = new PatternLayout("%-6level- %message");
            layout.ActivateOptions();

            var appender = new RemoteSyslogAppender
            {
                Name = SyslogAppenderName,
                RemoteAddress = addresses[0],
                RemotePort = port,
                Facility = RemoteSyslogAppender.SyslogFacility.User,
                Identity = new PatternLayout(Application.ProductName ?? "mRemoteNG"),
                Layout = layout
            };
            appender.ActivateOptions();
            return appender;
        }


        private static string BuildLogFilePath()
        {
            string logFilePath = Runtime.IsPortableEdition ? GetLogPathPortableEdition() : GetLogPathNormalEdition();

            string? logFileName = Path.ChangeExtension(Application.ProductName, ".log");

            if (logFileName == null) return "mRemoteNG.log";

            string logFile = Path.Combine(logFilePath, logFileName);

            return logFile;
        }

        private static string GetLogPathNormalEdition()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.ProductName ?? "mRemoteNG");
        }

        private static string GetLogPathPortableEdition()
        {
            return Application.StartupPath;
        }

    }
}