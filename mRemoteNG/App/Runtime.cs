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
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.App
{
    [SupportedOSPlatform("windows")]
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

        /// <summary>
        /// Feature flag to enable the credential manager feature
        /// </summary>
        public static bool UseCredentialManager => false;

        public static WindowList WindowList { get; set; }
        public static MessageCollector MessageCollector { get; } = new MessageCollector();
        public static NotificationAreaIcon NotificationAreaIcon { get; set; }
        public static ExternalToolsService ExternalToolsService { get; } = new ExternalToolsService();

        public static SecureString EncryptionKey { get; set; } =
            new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString();

        public static ICredentialRepositoryList CredentialProviderCatalog { get; } = new CredentialRepositoryList();

        public static ConnectionInitiator ConnectionInitiator { get; set; } = new ConnectionInitiator();

        public static ConnectionsService ConnectionsService { get; } =
            new ConnectionsService(PuttySessionsManager.Instance);

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
                    Properties.OptionsDBsPage.Default.UseSQLServer = false;
                    Properties.OptionsDBsPage.Default.Save();
                }
                else if (!Properties.OptionsDBsPage.Default.UseSQLServer)
                {
                    connectionFileName = ConnectionsService.GetStartupConnectionFileName();
                }

                ConnectionsService.LoadConnections(Properties.OptionsDBsPage.Default.UseSQLServer, false, connectionFileName);

                if (Properties.OptionsDBsPage.Default.UseSQLServer)
                {
                    ConnectionsService.LastSqlUpdate = DateTime.Now;
                } 
				else
                {
                    ConnectionsService.LastFileUpdate =  System.IO.File.GetLastWriteTime(connectionFileName);
                }

                // re-enable sql update checking after updates are loaded
                ConnectionsService.RemoteConnectionsSyncronizer?.Enable();
            }
            catch (Exception ex)
            {
                FrmSplashScreenNew.GetInstance().Close();

                if (Properties.OptionsDBsPage.Default.UseSQLServer)
                {
                    MessageCollector.AddExceptionMessage(Language.LoadFromSqlFailed, ex);
                    var commandButtons = string.Join("|", Language._TryAgain, Language.CommandOpenConnectionFile, string.Format(Language.CommandExitProgram, Application.ProductName));
                    CTaskDialog.ShowCommandBox(Application.ProductName, Language.LoadFromSqlFailed, Language.LoadFromSqlFailedContent, MiscTools.GetExceptionMessageRecursive(ex), "", "", commandButtons, false, ESysIcons.Error, ESysIcons.Error);
                    switch (CTaskDialog.CommandButtonResult)
                    {
                        case 0:
                            LoadConnections(withDialog);
                            return;
                        case 1:
                            Properties.OptionsDBsPage.Default.UseSQLServer = false;
                            LoadConnections(true);
                            return;
                        default:
                            Application.Exit();
                            return;
                    }
                }

                if (ex is FileNotFoundException && !withDialog)
                {
                    MessageCollector.AddExceptionMessage(
                                                         string.Format(Language.ConnectionsFileCouldNotBeLoadedNew,
                                                                       connectionFileName), ex,
                                                         MessageClass.InformationMsg);

                    string[] commandButtons =
                    {
                        Language.ConfigurationCreateNew,
                        Language.ConfigurationCustomPath,
                        Language.ConfigurationImportFile,
                        Language.Exit
                    };

                    var answered = false;
                    while (!answered)
                    {
                        try
                        {
                            CTaskDialog.ShowTaskDialogBox(GeneralAppInfo.ProductName, Language.ConnectionFileNotFound, "", "", "", "", "", string.Join(" | ", commandButtons), ETaskDialogButtons.None, ESysIcons.Question, ESysIcons.Question);

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
                            MessageCollector.AddExceptionMessage(string.Format(Language.ConnectionsFileCouldNotBeLoadedNew, connectionFileName), exc, MessageClass.InformationMsg);
                        }
                    }

                    return;
                }

                MessageCollector.AddExceptionStackTrace(
                                                        string.Format(Language.ConnectionsFileCouldNotBeLoaded,
                                                                      connectionFileName), ex);
                if (connectionFileName != ConnectionsService.GetStartupConnectionFileName())
                {
                    LoadConnections(withDialog);
                }
                else
                {
                    MessageBox.Show(FrmMain.Default, string.Format(Language.ErrorStartupConnectionFileLoad, Environment.NewLine, Application.ProductName, ConnectionsService.GetStartupConnectionFileName(), MiscTools.GetExceptionMessageRecursive(ex)), @"Could not load startup file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
        }

        #endregion
    }
}