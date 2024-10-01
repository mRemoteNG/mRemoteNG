using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.ConnectionSerializers.Xml;
using mRemoteNG.Properties;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.Config.Settings.Registry;
using mRemoteNG.Security.SymmetricEncryption;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class SecurityPage : OptionsPage
    {
        #region Private Fields
        private OptRegistrySecurityPage pageRegSettingsInstance;

        private readonly Timer clipboardClearTimer = new() { Interval = 1000 };
        private const int clipboardClearSeconds = 30;
        private int countdownSeconds = clipboardClearSeconds;
        #endregion

        public SecurityPage()
        {
            InitializeComponent();
            PopulateEncryptionEngineDropDown();
            PopulateBlockCipherDropDown();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Lock_16x);
        }

        [Browsable(false)]
        public override string PageName
        {
            get => Language.TabSecurity;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
            chkEncryptCompleteFile.Text = Language.EncryptCompleteConnectionFile;
            labelBlockCipher.Text = Language.EncryptionBlockCipherMode;
            labelEncryptionEngine.Text = Language.EncryptionEngine;
            labelKdfIterations.Text = Language.EncryptionKeyDerivationIterations;
            groupAdvancedSecurityOptions.Text = Language.AdvancedSecurityOptions;
            btnTestSettings.Text = Language.TestSettings;
            lblRegistrySettingsUsedInfo.Text = Language.OptionsCompanyPolicyMessage;
        }

        public override void LoadSettings()
        {
            chkEncryptCompleteFile.Checked = Properties.OptionsSecurityPage.Default.EncryptCompleteConnectionsFile;
            comboBoxEncryptionEngine.Text = Enum.GetName(typeof(BlockCipherEngines), Properties.OptionsSecurityPage.Default.EncryptionEngine);
            comboBoxBlockCipher.Text =
                Enum.GetName(typeof(BlockCipherModes), Properties.OptionsSecurityPage.Default.EncryptionBlockCipherMode);
            numberBoxKdfIterations.Value = Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations;
        }

        public override void SaveSettings()
        {
            Properties.OptionsSecurityPage.Default.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;
            Properties.OptionsSecurityPage.Default.EncryptionEngine = (BlockCipherEngines)comboBoxEncryptionEngine.SelectedItem;
            Properties.OptionsSecurityPage.Default.EncryptionBlockCipherMode = (BlockCipherModes)comboBoxBlockCipher.SelectedItem;
            Properties.OptionsSecurityPage.Default.EncryptionKeyDerivationIterations = (int)numberBoxKdfIterations.Value;
        }

        public override void LoadRegistrySettings()
        {
            Type settingsType = typeof(OptRegistrySecurityPage);
            RegistryLoader.RegistrySettings.TryGetValue(settingsType, out var settings);
            pageRegSettingsInstance = settings as OptRegistrySecurityPage;

            RegistryLoader.Cleanup(settingsType);

            // ***
            // Disable controls based on the registry settings.
            //
            if (pageRegSettingsInstance.EncryptCompleteConnectionsFile.IsSet)
                DisableControl(chkEncryptCompleteFile);

            if (pageRegSettingsInstance.EncryptionEngine.IsSet)
                DisableControl(comboBoxEncryptionEngine);

            if (pageRegSettingsInstance.EncryptionBlockCipherMode.IsSet)
                DisableControl(comboBoxBlockCipher);

            if (pageRegSettingsInstance.EncryptionKeyDerivationIterations.IsSet)
                DisableControl(numberBoxKdfIterations);

            // Updates the visibility of the information label indicating whether registry settings are used.
            lblRegistrySettingsUsedInfo.Visible = ShowRegistrySettingsUsedInfo();
        }

        private bool ShowRegistrySettingsUsedInfo()
        {
            return pageRegSettingsInstance.EncryptCompleteConnectionsFile.IsSet
                || pageRegSettingsInstance.EncryptionEngine.IsSet
                || pageRegSettingsInstance.EncryptionBlockCipherMode.IsSet
                || pageRegSettingsInstance.EncryptionKeyDerivationIterations.IsSet;
        }

        public override void RevertSettings()
        {
        }

        private void PopulateEncryptionEngineDropDown()
        {
            comboBoxEncryptionEngine.DataSource = Enum.GetValues(typeof(BlockCipherEngines));
        }

        private void PopulateBlockCipherDropDown()
        {
            comboBoxBlockCipher.DataSource = Enum.GetValues(typeof(BlockCipherModes));
        }

        private void BtnTestSettings_Click(object sender, EventArgs e)
        {
            Tree.ConnectionTreeModel connectionTree = Runtime.ConnectionsService.ConnectionTreeModel;
            if (!connectionTree.RootNodes.Any())
                return;

            BlockCipherEngines engine = (BlockCipherEngines)comboBoxEncryptionEngine.SelectedItem;
            BlockCipherModes mode = (BlockCipherModes)comboBoxBlockCipher.SelectedItem;
            ICryptographyProvider cryptographyProvider = new CryptoProviderFactory(engine, mode).Build();
            cryptographyProvider.KeyDerivationIterations = (int)numberBoxKdfIterations.Value;

            XmlConnectionSerializerFactory serializerFactory = new();
            Config.Serializers.ISerializer<Connection.ConnectionInfo, string> serializer = serializerFactory.Build(cryptographyProvider, connectionTree, useFullEncryption: chkEncryptCompleteFile.Checked);
            int nodeCount = connectionTree.GetRecursiveChildList().Count;

            Stopwatch timer = Stopwatch.StartNew();
            Container.ContainerInfo rootNode = connectionTree.RootNodes.First();
            serializer.Serialize(rootNode);
            timer.Stop();

            MessageBox.Show(this,
                string.Format(Language.EncryptionTestResultMessage,
                nodeCount, engine, mode, numberBoxKdfIterations.Value, timer.Elapsed.TotalSeconds),
                Language.EncryptionTest,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }


        /// <summary>
        /// Encrypts the entered password, copies it to the clipboard, and starts a 30-second timer to clear the clipboard.
        /// Also updates a countdown display in the UI.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event data.</param>
        private void btnPasswdGenerator_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPasswdGenerator.Text)) return;

            var cryptographyProvider = new LegacyRijndaelCryptographyProvider();

            try
            {
                // Encrypt and set the clipboard content
                string encryptedText = cryptographyProvider.Encrypt(txtPasswdGenerator.Text, Runtime.EncryptionKey);
                System.Windows.Clipboard.SetText(encryptedText);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Failed to set clipboard content. Please try again.");
                return;
            }

            // Reset the countdown and start the timer
            ResetAndStartTimer();
        }

        /// <summary>
        /// Resets the countdown timer and starts it.
        /// </summary>
        private void ResetAndStartTimer()
        {
            // Stop the timer if it's running
            if (clipboardClearTimer.Enabled)
            {
                clipboardClearTimer.Stop();
            }

            countdownSeconds = clipboardClearSeconds; // Reset countdown
            clipboardClearTimer.Tick -= ClipboardClearTimer_Tick; // Detach in case it was previously attached
            clipboardClearTimer.Tick += ClipboardClearTimer_Tick; // Re-attach event handler
            clipboardClearTimer.Start();
        }

        /// <summary>
        /// Handles the timer tick event for clearing the clipboard.
        /// Decrements the countdown, updates the UI label, and clears the clipboard when the countdown reaches zero.
        /// Stops the timer and handles any errors that occur during clipboard clearing.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event data.</param>
        private void ClipboardClearTimer_Tick(object sender, EventArgs e)
        {
            if (countdownSeconds > 0)
            {
                countdownSeconds--;
            }
            else
            {
                StopClipboardClearTimer();
                ClearClipboard();
            }
        }

        /// <summary>
        /// Stops the clipboard clear timer and detaches the event handler.
        /// </summary>
        private void StopClipboardClearTimer()
        {
            clipboardClearTimer.Stop();
            clipboardClearTimer.Tick -= ClipboardClearTimer_Tick;
        }

        /// <summary>
        /// Clears the clipboard and handles any exceptions that occur.
        /// </summary>
        private void ClearClipboard()
        {
            try
            {
                System.Windows.Clipboard.Clear();
                txtPasswdGenerator.Clear();
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Failed to clear clipboard.");
            }
        }

        /// <summary>
        /// Handles the TextChanged event for the password generator TextBox.
        /// Enables or disables the button depending on whether the TextBox has content.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event data.</param>
        private void txtPasswdGenerator_TextChanged(object sender, EventArgs e)
        {
            // Enable the button if there's text in the TextBox, disable it otherwise.
            btnPasswdGenerator.Enabled = !string.IsNullOrWhiteSpace(txtPasswdGenerator.Text);
        }
    }
}