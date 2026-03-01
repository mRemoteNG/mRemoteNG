using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using mRemoteNG.App.Info;
using mRemoteNG.App.Initialization;
using mRemoteNG.App.Update;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Cmdline;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;


using mRemoteNG.Config.DatabaseConnectors; // Added for DatabaseProfileManager

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
    public class Startup
    {
        private RegistryLoader _RegistryLoader;
        private AppUpdater _appUpdate;
        private readonly ConnectionIconLoader _connectionIconLoader;
        public static Startup Instance { get; } = new Startup();

        public string[]? CommandLineArgs { get; set; }

        private Startup()
        {
            _RegistryLoader = RegistryLoader.Instance; //created instance
            _appUpdate = new AppUpdater(); 
            _connectionIconLoader = new ConnectionIconLoader(GeneralAppInfo.HomePath + "\\Icons\\");
        }

        public void InitializeProgram(MessageCollector messageCollector)
        {
            Debug.Print("---------------------------" + Environment.NewLine + "[START] - " + Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture));
            StartupDataLogger startupLogger = new(messageCollector);
            startupLogger.LogStartupData();
            CompatibilityChecker.CheckCompatibility(messageCollector);
            ParseCommandLineArgs(messageCollector);
            IeBrowserEmulation.Register();
            _connectionIconLoader.GetConnectionIcons();
            DefaultConnectionInfo.Instance.LoadFrom(Settings.Default, a => "ConDefault" + a);
            DefaultConnectionInheritance.LoadFrom(Settings.Default, a => "InhDefault" + a);
            PluginManager.Instance.LoadPlugins();
        }

        private void ParseCommandLineArgs(MessageCollector messageCollector)
        {
            StartupArgumentsInterpreter interpreter = new(messageCollector);
            interpreter.ParseArguments(CommandLineArgs ?? Environment.GetCommandLineArgs());
        }

        public static void CreateConnectionsProvider(MessageCollector messageCollector)
        {
            messageCollector.AddMessage(MessageClass.DebugMsg, "Determining if we need a connections syncronizer");

            if (Properties.OptionsDBsPage.Default.UseSQLServer)
            {
                // Check if profile picker should be shown
                if (Properties.OptionsDBsPage.Default.ShowDatabasePickerOnStartup)
                {
                    using (var picker = new FrmDatabasePicker())
                    {
                        if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            if (picker.SelectedProfile != null)
                            {
                                DatabaseProfileManager.ApplyProfileToSettings(picker.SelectedProfile);
                            }
                        }
                        else
                        {
                            // User cancelled, do not enable SQL sync
                            return;
                        }
                    }
                }

                messageCollector.AddMessage(MessageClass.DebugMsg, "Creating database syncronizer");
                Runtime.ConnectionsService.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(new SqlConnectionsUpdateChecker());
                Runtime.ConnectionsService.RemoteConnectionsSyncronizer.Enable();
            }
            else if (Properties.OptionsConnectionsPage.Default.WatchConnectionFile)
            {
                messageCollector.AddMessage(MessageClass.DebugMsg, "Creating file syncronizer");
                string startupFile = ConnectionsService.GetStartupConnectionFileName();
                if (!string.IsNullOrEmpty(startupFile))
                {
                    Runtime.ConnectionsService.RemoteConnectionsSyncronizer = new RemoteConnectionsSyncronizer(new FileConnectionsUpdateChecker(startupFile));
                    Runtime.ConnectionsService.RemoteConnectionsSyncronizer.Enable();
                }
            }
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

            DateTime nextUpdateCheck = Convert.ToDateTime(Properties.OptionsUpdatesPage.Default.CheckForUpdatesLastCheck.Add(TimeSpan.FromDays(Convert.ToDouble(Properties.OptionsUpdatesPage.Default.CheckForUpdatesFrequencyDays))));
            if (!Properties.OptionsUpdatesPage.Default.UpdatePending && DateTime.UtcNow < nextUpdateCheck)
            {
                return;
            }

            try
            {
                await _appUpdate.GetUpdateInfoAsync();
                // Update is available, but don't show the panel automatically at startup
                // User can check for updates manually via Help > Check for Updates menu
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionMessage("CheckForUpdate() failed.", ex);
            }
        }
    }
}