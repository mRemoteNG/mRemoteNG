using System;
using System.ComponentModel;
using mRemoteNG.Security;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class SecurityPage : OptionsPage
    {
        public SecurityPage()
        {
            InitializeComponent();
            PopulateEncryptionEngineDropDown();
            PopulateBlockCipherDropDown();
        }

        [Browsable(false)]
        public override string PageName
        {
            get { return Language.strTabSecurity; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
            chkEncryptCompleteFile.Text = Language.strEncryptCompleteConnectionFile;
            labelBlockCipher.Text = Language.strBlockCipher;
            labelEncryptionEngine.Text = Language.strEncryptionEngine;
        }

        public override void LoadSettings()
        {
            chkEncryptCompleteFile.Checked = Settings.Default.EncryptCompleteConnectionsFile;
            comboBoxEncryptionEngine.Text = Enum.GetName(typeof(BlockCipherEngines), Settings.Default.EncryptionEngine);
            comboBoxBlockCipher.Text = Enum.GetName(typeof(BlockCipherModes), Settings.Default.EncryptionBlockCipherMode);
        }

        public override void SaveSettings()
        {
            Settings.Default.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;
            Settings.Default.EncryptionEngine = (BlockCipherEngines) Enum.Parse(typeof(BlockCipherEngines), comboBoxEncryptionEngine.Text);
            Settings.Default.EncryptionBlockCipherMode = (BlockCipherModes) Enum.Parse(typeof(BlockCipherModes), comboBoxBlockCipher.Text);
            Settings.Default.Save();
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