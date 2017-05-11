using mRemoteNG.App.Info;
using mRemoteNG.Config.Connections;
using mRemoteNG.Connection;
using mRemoteNG.Messages;
using mRemoteNG.Tools;
using mRemoteNG.Tree.Root;
using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.Config.Connections.Multiuser;
using mRemoteNG.Config.DataProviders;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using mRemoteNG.UI;
using mRemoteNG.UI.Forms;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.App
{
    public static class Runtime
    {
        #region Public Properties

        #if PORTABLE
        public static bool IsPortableEdition { get; } = true;
        #else
        public static bool IsPortableEdition { get; } = false;
        #endif

        public static WindowList WindowList { get; set; }
        public static MessageCollector MessageCollector { get; } = new MessageCollector();
        public static NotificationAreaIcon NotificationAreaIcon { get; set; }
        public static RemoteConnectionsSyncronizer RemoteConnectionsSyncronizer { get; set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private static DateTime LastSqlUpdate { get; set; }
        public static ExternalToolsService ExternalToolsService { get; } = new ExternalToolsService();
        public static SecureString EncryptionKey { get; set; } = new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString();
        public static ICredentialRepositoryList CredentialProviderCatalog { get; } = new CredentialRepositoryList();
        public static ConnectionsService ConnectionsService { get; } = new ConnectionsService();
        #endregion

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
            var connectionsLoader = new ConnectionsLoader();
            try
            {
                // disable sql update checking while we are loading updates
                RemoteConnectionsSyncronizer?.Disable();

                if (!Settings.Default.UseSQLServer)
                {
                    if (withDialog)
                    {
                        var loadDialog = ConnectionsLoadDialog();
                        if (loadDialog.ShowDialog() != DialogResult.OK) return;
                        connectionsLoader.ConnectionFileName = loadDialog.FileName;
                    }
                    else
                    {
                        connectionsLoader.ConnectionFileName = ConnectionsService.GetStartupConnectionFileName();
                    }

                    var backupFileCreator = new FileBackupCreator();
                    backupFileCreator.CreateBackupFile(connectionsLoader.ConnectionFileName);

                    var backupPruner = new FileBackupPruner();
                    backupPruner.PruneBackupFiles(connectionsLoader.ConnectionFileName);
                }

                connectionsLoader.UseDatabase = Settings.Default.UseSQLServer;
                ConnectionsService.ConnectionTreeModel = connectionsLoader.LoadConnections(CredentialProviderCatalog.GetCredentialRecords(), false);
                Windows.TreeForm.ConnectionTree.ConnectionTreeModel = ConnectionsService.ConnectionTreeModel;

                if (Settings.Default.UseSQLServer)
                {
                    LastSqlUpdate = DateTime.Now;
                }
                else
                {
                    if (connectionsLoader.ConnectionFileName == ConnectionsService.GetDefaultStartupConnectionFileName())
                    {
                        Settings.Default.LoadConsFromCustomLocation = false;
                    }
                    else
                    {
                        Settings.Default.LoadConsFromCustomLocation = true;
                        Settings.Default.CustomConsPath = connectionsLoader.ConnectionFileName;
                    }
                }

                // re-enable sql update checking after updates are loaded
                RemoteConnectionsSyncronizer?.Enable();
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
                    MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotBeLoadedNew, connectionsLoader.ConnectionFileName), ex, MessageClass.InformationMsg);
                    ConnectionsService.NewConnections(connectionsLoader.ConnectionFileName);
                    return;
                }

                MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotBeLoaded, connectionsLoader.ConnectionFileName), ex);
                if (connectionsLoader.ConnectionFileName != ConnectionsService.GetStartupConnectionFileName())
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

        private static OpenFileDialog ConnectionsLoadDialog()
        {
            return new OpenFileDialog
            {
                CheckFileExists = true,
                InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath,
                Filter = Language.strFiltermRemoteXML + @"|*.xml|" + Language.strFilterAll + @"|*.*"
            };
        }

        public static void SaveConnectionsAsync()
        {
            var t = new Thread(SaveConnectionsBGd);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private static readonly object SaveLock = new object();
        private static void SaveConnectionsBGd()
        {
            Monitor.Enter(SaveLock);
            SaveConnections();
            Monitor.Exit(SaveLock);
        }

        public static void SaveConnections()
        {
            if (ConnectionsService.ConnectionTreeModel == null) return;
            if (!ConnectionsService.IsConnectionsFileLoaded) return;

            try
            {
                RemoteConnectionsSyncronizer?.Disable();

                var connectionsSaver = new ConnectionsSaver();

                if (!Settings.Default.UseSQLServer)
                    connectionsSaver.ConnectionFileName = ConnectionsService.GetStartupConnectionFileName();

                connectionsSaver.SaveFilter = new SaveFilter();
                connectionsSaver.ConnectionTreeModel = ConnectionsService.ConnectionTreeModel;

                if (Settings.Default.UseSQLServer)
                {
                    connectionsSaver.SaveFormat = ConnectionsSaver.Format.SQL;
                    connectionsSaver.SQLHost = Settings.Default.SQLHost;
                    connectionsSaver.SQLDatabaseName = Settings.Default.SQLDatabaseName;
                    connectionsSaver.SQLUsername = Settings.Default.SQLUser;
                    var cryptographyProvider = new LegacyRijndaelCryptographyProvider();
                    connectionsSaver.SQLPassword = cryptographyProvider.Decrypt(Settings.Default.SQLPass, EncryptionKey);
                }

                connectionsSaver.SaveConnections();

                if (Settings.Default.UseSQLServer)
                    LastSqlUpdate = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageCollector.AddMessage(MessageClass.ErrorMsg, Language.strConnectionsFileCouldNotBeSaved + Environment.NewLine + ex.Message);
            }
            finally
            {
                RemoteConnectionsSyncronizer?.Enable();
            }
        }

        public static void SaveConnectionsAs()
        {
            var connectionsSave = new ConnectionsSaver();

            try
            {
                RemoteConnectionsSyncronizer?.Disable();

                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.InitialDirectory = ConnectionsFileInfo.DefaultConnectionsPath;
                    saveFileDialog.FileName = ConnectionsFileInfo.DefaultConnectionsFile;
                    saveFileDialog.OverwritePrompt = true;
                    saveFileDialog.Filter = $@"{Language.strFiltermRemoteXML}|*.xml|{Language.strFilterAll}|*.*";

                    if (saveFileDialog.ShowDialog(FrmMain.Default) != DialogResult.OK) return;

                    connectionsSave.SaveFormat = ConnectionsSaver.Format.mRXML;
                    connectionsSave.ConnectionFileName = saveFileDialog.FileName;
                    connectionsSave.SaveFilter = new SaveFilter();
                    connectionsSave.ConnectionTreeModel = ConnectionsService.ConnectionTreeModel;

                    connectionsSave.SaveConnections();

                    if (saveFileDialog.FileName == ConnectionsService.GetDefaultStartupConnectionFileName())
                    {
                        Settings.Default.LoadConsFromCustomLocation = false;
                    }
                    else
                    {
                        Settings.Default.LoadConsFromCustomLocation = true;
                        Settings.Default.CustomConsPath = saveFileDialog.FileName;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionMessage(string.Format(Language.strConnectionsFileCouldNotSaveAs, connectionsSave.ConnectionFileName), ex);
            }
            finally
            {
                RemoteConnectionsSyncronizer?.Enable();
            }
        }
        #endregion
    }
}