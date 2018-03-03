using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.App
{
	public class Runtime
    {
        public static bool IsPortableEdition
        {
            get
            {
#if PORTABLE
                return true;
#else
                return false;
#endif
            }
        }

        public static MessageCollector MessageCollector { get; } = new MessageCollector();
        public static NotificationAreaIcon NotificationAreaIcon { get; set; }
        public static ConnectionsService ConnectionsService { get; } = new ConnectionsService(PuttySessionsManager.Instance);

        #region Connections Loading/Saving
        public static void LoadConnectionsAsync()
        {
            _withDialog = false;

            var t = new Thread(LoadConnectionsBGd);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private static bool _withDialog;
        private static void LoadConnectionsBGd()
        {
            LoadConnections(_withDialog);
        }

        public static void LoadConnections(bool withDialog = false)
        {
            var connectionFileName = "";

            try
            {
                // disable sql update checking while we are loading updates
                ConnectionsService.RemoteConnectionsSyncronizer?.Disable();

                if (!Settings.Default.UseSQLServer)
                {
                    if (withDialog)
                    {
                        var loadDialog = DialogFactory.BuildLoadConnectionsDialog();
                        if (loadDialog.ShowDialog() != DialogResult.OK) return;
                        connectionFileName = loadDialog.FileName;
                    }
                    else
                    {
                        connectionFileName = ConnectionsService.GetStartupConnectionFileName();
                    }

                    var backupFileCreator = new FileBackupCreator();
                    backupFileCreator.CreateBackupFile(connectionFileName);

                    var backupPruner = new FileBackupPruner();
                    backupPruner.PruneBackupFiles(connectionFileName);
                }

                ConnectionsService.LoadConnections(Settings.Default.UseSQLServer, false, connectionFileName);

                if (Settings.Default.UseSQLServer)
                {
                    ConnectionsService.LastSqlUpdate = DateTime.Now;
                }
                else
                {
                    if (connectionFileName == ConnectionsService.GetDefaultStartupConnectionFileName())
                    {
                        Settings.Default.LoadConsFromCustomLocation = false;
                    }
                    else
                    {
                        Settings.Default.LoadConsFromCustomLocation = true;
                        Settings.Default.CustomConsPath = connectionFileName;
                    }
                }

                // re-enable sql update checking after updates are loaded
                ConnectionsService.RemoteConnectionsSyncronizer?.Enable();
            }
            catch (Exception ex)
            {
                if (Settings.Default.UseSQLServer)
                {
                    MessageCollector.AddExceptionMessage(Language.strLoadFromSqlFailed, ex);
                    var commandButtons = string.Join("|", Language.strCommandTryAgain, Language.strCommandOpenConnectionFile, string.Format(Language.strCommandExitProgram, Application.ProductName));
                    CTaskDialog.ShowCommandBox(Application.ProductName, Language.strLoadFromSqlFailed, Language.strLoadFromSqlFailedContent, MiscTools.GetExceptionMessageRecursive(ex), "", "", commandButtons, false, ESysIcons.Error, ESysIcons.Error);
                    switch (CTaskDialog.CommandButtonResult)
                    {
                        case 0:
                            LoadConnections(withDialog);
                            return;
                        case 1:
                            Settings.Default.UseSQLServer = false;
                            LoadConnections(true);
                            return;
                        default:
                            Application.Exit();
                            return;
                    }
                }
                if (ex is FileNotFoundException && !withDialog)
                {
                    MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotBeLoadedNew, connectionFileName), ex, MessageClass.InformationMsg);

                    string[] commandButtons =
                    {
                        Language.ConfigurationCreateNew,
                        Language.ConfigurationCustomPath,
                        Language.ConfigurationImportFile,
                        Language.strMenuExit
                    };

                    var answered = false;
                    while (!answered)
                    {
                        try
                        {
                            CTaskDialog.ShowTaskDialogBox(
                                GeneralAppInfo.ProductName, 
                                Language.ConnectionFileNotFound, 
                                "", "", "", "", "", 
                                string.Join(" | ", commandButtons), 
                                ETaskDialogButtons.None, 
                                ESysIcons.Question, 
                                ESysIcons.Question);

                            switch (CTaskDialog.CommandButtonResult)
                            {
                                case 0:
                                    ConnectionsService.NewConnectionsFile(connectionFileName);
                                    answered = true;
                                    break;
                                case 1:
                                    LoadConnections(true);
                                    answered = true;
                                    break;
                                case 2:
                                    ConnectionsService.NewConnectionsFile(connectionFileName);
                                    Import.ImportFromFile(ConnectionsService.ConnectionTreeModel.RootNodes[0]);
                                    answered = true;
                                    break;
                                case 3:
                                    Application.Exit();
                                    answered = true;
                                    break;
                            }                               
                        }
                        catch (Exception exc)
                        {
                            MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotBeLoadedNew, connectionFileName), exc, MessageClass.InformationMsg);
                        }
                    }
                    return;
                }

                MessageCollector.AddExceptionStackTrace(string.Format(Language.strConnectionsFileCouldNotBeLoaded, connectionFileName), ex);
                if (connectionFileName != ConnectionsService.GetStartupConnectionFileName())
                {
                    LoadConnections(withDialog);
                }
                else
                {
                    MessageBox.Show(FrmMain.Default,
                        string.Format(Language.strErrorStartupConnectionFileLoad, Environment.NewLine, Application.ProductName, ConnectionsService.GetStartupConnectionFileName(), MiscTools.GetExceptionMessageRecursive(ex)),
                        @"Could not load startup file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }
        #endregion
    }
}