using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using mRemoteNG.App.Info;
using mRemoteNG.App.Initialization;
using mRemoteNG.App.Update;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Cmdline;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public class Startup
    {
        private AppUpdater _appUpdate;
        private readonly ConnectionIconLoader _connectionIconLoader;
        private readonly FrmMain _frmMain = FrmMain.Default;

        public static Startup Instance { get; } = new Startup();

        private Startup()
        {
            _appUpdate = new AppUpdater();
            _connectionIconLoader = new ConnectionIconLoader(GeneralAppInfo.HomePath + "\\Icons\\");
        }

        static Startup()
        {
        }

        public void InitializeProgram(MessageCollector messageCollector)
        {
            Debug.Print("---------------------------" + Environment.NewLine + "[START] - " + Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture));
            var startupLogger = new StartupDataLogger(messageCollector);
            startupLogger.LogStartupData();
            CompatibilityChecker.CheckCompatibility(messageCollector);
            ParseCommandLineArgs(messageCollector);
            IeBrowserEmulation.Register();
            _connectionIconLoader.GetConnectionIcons();
            DefaultConnectionInfo.Instance.LoadFrom(Settings.Default, a => "ConDefault" + a);
            DefaultConnectionInheritance.Instance.LoadFrom(Settings.Default, a => "InhDefault" + a);
        }

        private static void ParseCommandLineArgs(MessageCollector messageCollector)
        {
            var interpreter = new StartupArgumentsInterpreter(messageCollector);
            interpreter.ParseArguments(Environment.GetCommandLineArgs());
        }

        public void CreateConnectionsProvider(MessageCollector messageCollector)
        {
            messageCollector.AddMessage(MessageClass.DebugMsg, "Determining if we need a database syncronizer");
            if (!Properties.OptionsDBsPage.Default.UseSQLServer) return;
            messageCollector.AddMessage(MessageClass.DebugMsg, "Creating database syncronizer");
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(new SqlConnectionsUpdateChecker());
            Runtime.ConnectionsService.RemoteConnectionsSyncronizer.Enable();
        }

        public async Task CheckForUpdate()
        {
            if (_appUpdate == null)
            {
                _appUpdate = new AppUpdater();
            }
            else if (_appUpdate.IsGetUpdateInfoRunning)
            {
                return;
            }

            var nextUpdateCheck =
                Convert.ToDateTime(Properties.OptionsUpdatesPage.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(Convert.ToDouble(Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays))));
            if (!Properties.OptionsUpdatesPage.Default.UpdatePending && DateTime.UtcNow < nextUpdateCheck)
            {
                return;
            }

            try
            {
                await _appUpdate.GetUpdateInfoAsync();
                if (_appUpdate.IsUpdateAvailable())
                {
                    Windows.Show(WindowType.Update);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("CheckForUpdate() failed.", ex);
            }
        }
    }
}