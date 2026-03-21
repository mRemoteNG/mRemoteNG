using mRemoteNG.App.Info;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection;
using mRemoteNG.Credential;
using mRemoteNG.Credential.Repositories;
using System.Linq;
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

        private static SecureString? _masterPasswordKey;
        public static SecureString EncryptionKey { get; private set; } = CreateDefaultEncryptionKey();
        public static bool HasActiveMasterPasswordSession => _masterPasswordKey != null;

        public static ICredentialRepositoryList CredentialProviderCatalog { get; } = new CredentialRepositoryList();

        /// <summary>
        /// The global credential service facade for accessing credential repositories.
        /// Must be initialized at startup by calling <see cref="InitializeCredentialService"/>.
        /// </summary>
        public static CredentialServiceFacade? CredentialService { get; private set; }

        /// <summary>
        /// Initializes the credential service and loads the credential repository list.
        /// Should be called during application startup.
        /// </summary>
        public static void InitializeCredentialService()
        {
            if (CredentialService == null)
            {
                CredentialServiceFactory factory = new();
                CredentialService = factory.Build();
                CredentialService.LoadRepositoryList();
            }
            
            // Always ensure a default credential repository exists
            EnsureDefaultCredentialRepository();
        }

        /// <summary>
        /// Creates a default credential repository if none exists, and ensures it's loaded with an encryption key.
        /// This method can be called multiple times and will only create a repository if needed.
        /// </summary>
        public static void EnsureDefaultCredentialRepository()
        {
            // Check if any repository exists and is loaded
            ICredentialRepository? loadedRepo = CredentialProviderCatalog.CredentialProviders.FirstOrDefault(r => r.IsLoaded);
            
            if (loadedRepo != null)
                return; // Already have a loaded repository

            // Try to get an existing unloaded repository
            ICredentialRepository? existingRepo = CredentialProviderCatalog.CredentialProviders.FirstOrDefault();

            if (existingRepo == null)
            {
                // Create a brand new default repository
                existingRepo = CreateDefaultCredentialRepository();
                CredentialService?.AddRepository(existingRepo);
            }

            SecureString repositoryKey = ResolveCredentialRepositoryKey(existingRepo);

            // Load the repository with the encryption key
            try
            {
                existingRepo.LoadCredentials(repositoryKey);
                MigrateCredentialRepositoryKeyIfNeeded(existingRepo, repositoryKey);
            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionMessage("Failed to load credential repository", ex);
            }
        }

        public static void SetEncryptionKey(SecureString key)
        {
            UpdateEncryptionKey(key.Copy(), syncLoadedRepositories: true);
        }

        public static void SetEncryptionKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                ResetEncryptionKey();
                return;
            }

            SetEncryptionKey(key.ConvertToSecureString());
        }

        public static void ResetEncryptionKey()
        {
            UpdateEncryptionKey(_masterPasswordKey?.Copy() ?? CreateCurrentRootOrDefaultEncryptionKey(), syncLoadedRepositories: true);
        }

        public static void SetMasterPasswordSession(SecureString key)
        {
            _masterPasswordKey?.Dispose();
            _masterPasswordKey = key.Copy();
            UpdateEncryptionKey(_masterPasswordKey.Copy(), syncLoadedRepositories: true);
        }

        public static void ClearMasterPasswordSession()
        {
            _masterPasswordKey?.Dispose();
            _masterPasswordKey = null;
            UpdateEncryptionKey(CreateCurrentRootOrDefaultEncryptionKey(), syncLoadedRepositories: true);
        }

        /// <summary>
        /// Creates a new default XML credential repository.
        /// </summary>
        private static ICredentialRepository CreateDefaultCredentialRepository()
        {
            string credentialsPath = Path.Combine(Info.SettingsFileInfo.SettingsPath, "credentials.xml");
            
            CredentialRepositoryConfig config = new()
            {
                Title = "Default Credentials",
                TypeName = "XmlCredentialRepository",
                Source = credentialsPath
            };
            
            Security.Factories.CryptoProviderFactoryFromSettings cryptoFromSettings = new();
            Config.Serializers.CredentialSerializer.XmlCredentialPasswordEncryptorDecorator serializer = 
                new(cryptoFromSettings.Build(), new Config.Serializers.CredentialSerializer.XmlCredentialRecordSerializer());
            Config.Serializers.CredentialSerializer.XmlCredentialPasswordDecryptorDecorator deserializer = 
                new(new Config.Serializers.CredentialSerializer.XmlCredentialRecordDeserializer());
            
            Credential.Repositories.XmlCredentialRepositoryFactory repoFactory = new(serializer, deserializer);
            return repoFactory.Build(config);
        }

        private static SecureString CreateDefaultEncryptionKey()
        {
            return new RootNodeInfo(RootNodeType.Connection).PasswordString.ConvertToSecureString();
        }

        private static SecureString CreateCurrentRootOrDefaultEncryptionKey()
        {
            RootNodeInfo? rootNode = ConnectionsService.ConnectionTreeModel?.RootNodes.OfType<RootNodeInfo>().FirstOrDefault();
            return rootNode is { Password: true }
                ? rootNode.PasswordString.ConvertToSecureString()
                : CreateDefaultEncryptionKey();
        }

        private static void UpdateEncryptionKey(SecureString newKey, bool syncLoadedRepositories)
        {
            bool keyChanged = EncryptionKey.ConvertToUnsecureString() != newKey.ConvertToUnsecureString();

            EncryptionKey?.Dispose();
            EncryptionKey = newKey;

            if (syncLoadedRepositories && keyChanged)
                SyncLoadedCredentialRepositoriesToEncryptionKey();
        }

        private static SecureString ResolveCredentialRepositoryKey(ICredentialRepository repository)
        {
            string source = repository.Config.Source;
            if (string.IsNullOrWhiteSpace(source) || !File.Exists(source))
                return EncryptionKey.Copy();

            if (XmlKeyValidator.CredentialsFileUsesKey(source, EncryptionKey))
                return EncryptionKey.Copy();

            SecureString defaultKey = CreateDefaultEncryptionKey();
            return XmlKeyValidator.CredentialsFileUsesKey(source, defaultKey)
                ? defaultKey
                : EncryptionKey.Copy();
        }

        private static void MigrateCredentialRepositoryKeyIfNeeded(ICredentialRepository repository, SecureString loadedKey)
        {
            if (loadedKey.ConvertToUnsecureString() == EncryptionKey.ConvertToUnsecureString())
            {
                repository.Config.Key = EncryptionKey.Copy();
                return;
            }

            try
            {
                repository.SaveCredentials(EncryptionKey);
                repository.Config.Key = EncryptionKey.Copy();
            }
            catch (Exception ex)
            {
                MessageCollector.AddExceptionMessage("Failed to migrate credential repository encryption key", ex);
            }
        }

        private static void SyncLoadedCredentialRepositoriesToEncryptionKey()
        {
            foreach (ICredentialRepository repository in CredentialProviderCatalog.CredentialProviders.Where(r => r.IsLoaded))
            {
                try
                {
                    repository.SaveCredentials(EncryptionKey);
                    repository.Config.Key = EncryptionKey.Copy();
                }
                catch (Exception ex)
                {
                    MessageCollector.AddExceptionMessage("Failed to synchronize credential repository encryption key", ex);
                }
            }
        }

        public static ConnectionInitiator ConnectionInitiator { get; set; } = new ConnectionInitiator();

        public static ConnectionsService ConnectionsService { get; } = new ConnectionsService(PuttySessionsManager.Instance);

        #region Connections Loading/Saving

        public static void LoadConnectionsAsync()
        {
            Thread t = new(LoadConnectionsBGd);
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
            string connectionFileName = "";

            try
            {
                // disable sql update checking while we are loading updates
                ConnectionsService.RemoteConnectionsSyncronizer?.Disable();

                if (withDialog)
                {
                    OpenFileDialog loadDialog = DialogFactory.BuildLoadConnectionsDialog();
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
                    ConnectionsService.LastSqlUpdate = DateTime.Now.ToUniversalTime();
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
                    string commandButtons = string.Join("|", Language._TryAgain, Language.CommandOpenConnectionFile, string.Format(Language.CommandExitProgram, Application.ProductName));
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

                    bool answered = false;
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
