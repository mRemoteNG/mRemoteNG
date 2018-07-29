using System;
using System.ComponentModel;
using mRemoteNG.Security;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class SecurityPage : OptionsPage
    {
        public SecurityPage()
        {
            InitializeComponent();
            PopulateEncryptionEngineDropDown();
            PopulateBlockCipherDropDown();
            ApplyTheme();
        }

        [Browsable(false)]
        public override string PageName
        {
            get => Language.strTabSecurity;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
            chkEncryptCompleteFile.Text = Language.strEncryptCompleteConnectionFile;
            labelBlockCipher.Text = Language.strEncryptionBlockCipherMode;
            labelEncryptionEngine.Text = Language.strEncryptionEngine;
            labelKdfIterations.Text = Language.strEncryptionKeyDerivationIterations;
            groupAdvancedSecurityOptions.Text = Language.strAdvancedSecurityOptions;
        }

        public override void LoadSettings()
        {
            base.SaveSettings();
            chkEncryptCompleteFile.Checked = Settings.Default.EncryptCompleteConnectionsFile;
            comboBoxEncryptionEngine.Text = Enum.GetName(typeof(BlockCipherEngines), Settings.Default.EncryptionEngine);
            comboBoxBlockCipher.Text = Enum.GetName(typeof(BlockCipherModes), Settings.Default.EncryptionBlockCipherMode);
            numberBoxKdfIterations.Value = Settings.Default.EncryptionKeyDerivationIterations;
        }

        public override void SaveSettings()
        {
            Settings.Default.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;
            Settings.Default.EncryptionEngine = (BlockCipherEngines) comboBoxEncryptionEngine.SelectedItem;
            Settings.Default.EncryptionBlockCipherMode = (BlockCipherModes) comboBoxBlockCipher.SelectedItem;
            Settings.Default.EncryptionKeyDerivationIterations = (int)numberBoxKdfIterations.Value;
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
    }
}