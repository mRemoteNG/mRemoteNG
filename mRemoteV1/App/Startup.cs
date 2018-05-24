using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using mRemoteNG.App.Info;
using mRemoteNG.App.Initialization;
using mRemoteNG.App.Update;
using mRemoteNG.Config.Connections;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DatabaseConnectors;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Cmdline;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;


namespace mRemoteNG.App
{
	public class Startup
    {
        private readonly AppUpdater _appUpdate;
        private readonly ConnectionIconLoader _connectionIconLoader;
        private readonly FrmMain _frmMain;
	    private readonly Windows _windows;
        private readonly IConnectionsService _connectionsService;
        private readonly DatabaseConnectorFactory _databaseConnectorFactory;
        private readonly CompatibilityChecker _compatibilityChecker;

        public Startup(FrmMain frmMain, Windows windows, IConnectionsService connectionsService,
            AppUpdater appUpdate, DatabaseConnectorFactory databaseConnectorFactory, CompatibilityChecker compatibilityChecker)
        {
            _frmMain = frmMain.ThrowIfNull(nameof(frmMain));
			_windows = windows.ThrowIfNull(nameof(windows));
            _connectionsService = connectionsService.ThrowIfNull(nameof(connectionsService));
	        _appUpdate = appUpdate.ThrowIfNull(nameof(appUpdate));
            _databaseConnectorFactory = databaseConnectorFactory.ThrowIfNull(nameof(databaseConnectorFactory));
            _compatibilityChecker = compatibilityChecker.ThrowIfNull(nameof(compatibilityChecker));
            _connectionIconLoader = new ConnectionIconLoader(GeneralAppInfo.HomePath + "\\Icons\\");
        }

        public void InitializeProgram(MessageCollector messageCollector)
        {
            Debug.Print("---------------------------" + Environment.NewLine + "[START] - " + Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture));
            var startupLogger = new StartupDataLogger(messageCollector);
            startupLogger.LogStartupData();
            _compatibilityChecker.CheckCompatibility();
            ParseCommandLineArgs(messageCollector);
            IeBrowserEmulation.Register();
            _connectionIconLoader.GetConnectionIcons();
            DefaultConnectionInfo.Instance.LoadFrom(Settings.Default, a=>"ConDefault"+a);
            DefaultConnectionInheritance.Instance.LoadFrom(Settings.Default, a=>"InhDefault"+a);
        }

        private static void ParseCommandLineArgs(MessageCollector messageCollector)
        {
            var interpreter = new StartupArgumentsInterpreter(messageCollector);
            interpreter.ParseArguments(Environment.GetCommandLineArgs());
        }

        public void CreateConnectionsProvider(MessageCollector messageCollector)
        {
            messageCollector.AddMessage(MessageClass.DebugMsg, "Determining if we need a database syncronizer");
            if (!Settings.Default.UseSQLServer) return;
            messageCollector.AddMessage(MessageClass.DebugMsg, "Creating database syncronizer");
            var sqlConnectionsUpdateChecker = new SqlConnectionsUpdateChecker(_connectionsService, _databaseConnectorFactory);
            _connectionsService.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(sqlConnectionsUpdateChecker, _connectionsService);
            _connectionsService.RemoteConnectionsSyncronizer.Enable();
        }

        public void CheckForUpdate()
        {
            if (_appUpdate.IsGetUpdateInfoRunning)
            {
                return;
            }

            var nextUpdateCheck = Convert.ToDateTime(Settings.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(Convert.ToDouble(Settings.Default.CheckForUpdatesFrequencyDays))));
            if (!Settings.Default.UpdatePending && DateTime.UtcNow < nextUpdateCheck)
            {
                return;
            }

            _appUpdate.GetUpdateInfoCompletedEvent += GetUpdateInfoCompleted;
            _appUpdate.GetUpdateInfoAsync();
        }

        private void GetUpdateInfoCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (_frmMain.InvokeRequired)
            {
                _frmMain.Invoke(new AsyncCompletedEventHandler(GetUpdateInfoCompleted), sender, e);
                return;
            }

            try
            {
                _appUpdate.GetUpdateInfoCompletedEvent -= GetUpdateInfoCompleted;

                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    throw e.Error;
                }

                if (_appUpdate.IsUpdateAvailable())
                {
                    _windows.Show(WindowType.Update);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("GetUpdateInfoCompleted() failed.", ex);
            }
        }
    }
}