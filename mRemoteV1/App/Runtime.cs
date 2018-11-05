using mRemoteNG.App.Info;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Messages;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;
using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace mRemoteNG.App
{
    public static class Runtime
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

        public static WindowList WindowList { get; set; }
        public static MessageCollector MessageCollector { get; } = new MessageCollector();
        public static NotificationAreaIcon NotificationAreaIcon { get; set; }
        public static ExternalToolsService ExternalToolsService { get; } = new ExternalToolsService();
        public static SecureString EncryptionKey { get; set; } = new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString();
        public static ICredentialRepositoryList CredentialProviderCatalog { get; } = new CredentialRepositoryList();
        public static ConnectionsService ConnectionsService { get; } = new ConnectionsService(PuttySessionsManager.Instance);

        #region Connections Loading/Saving
        public static void LoadConnectionsAsync()
        {
            var t = new Thread(LoadConnectionsBGd);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private static void LoadConnectionsBGd()
        {
            LoadConnections();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="withDialog">
        /// Should we show the file selection dialog to allow the user to select
        /// a connection file
        /// </param>
        public static void LoadConnections(bool withDialog = false)
        {
            var connectionFileName = "";

            try
            {
                // disable sql update checking while we are loading updates
                ConnectionsService.RemoteConnectionsSyncronizer?.Disable();

                if (withDialog)
                {
                    var loadDialog = DialogFactory.BuildLoadConnectionsDialog();
                    if (loadDialog.ShowDialog() != DialogResult.OK)
                        return;

                    connectionFileName = loadDialog.FileName;
                    Settings.Default.UseSQLServer = false;
                    Settings.Default.Save();
                }
                else if (!Settings.Default.UseSQLServer)
                {
                    connectionFileName = ConnectionsService.GetStartupConnectionFileName();
                }

                ConnectionsService.LoadConnections(Settings.Default.UseSQLServer, false, connectionFileName);

                if (Settings.Default.UseSQLServer)
                {
                    ConnectionsService.LastSqlUpdate = DateTime.Now;
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