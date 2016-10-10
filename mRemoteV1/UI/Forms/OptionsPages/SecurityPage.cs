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
        }

        public override void SaveSettings()
        {
            Settings.Default.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;

            Settings.Default.Save();
        }

        public override void RevertSettings()
        {

        }

        private void PopulateEncryptionEngineDropDown()
        {
            var possibleEngines = Enum.GetNames(typeof(BlockCipherEngines));
            comboBoxEncryptionEngine.Items.AddRange(possibleEngines);
            comboBoxEncryptionEngine.Text = Enum.GetName(typeof(BlockCipherEngines), Settings.Default.EncryptionEngine);
        }

        private void PopulateBlockCipherDropDown()
        {
            var possibleCipherModes = Enum.GetNames(typeof(BlockCipherModes));
            comboBoxBlockCipher.Items.AddRange(possibleCipherModes);
            comboBoxBlockCipher.Text = Enum.GetName(typeof(BlockCipherModes), Settings.Default.EncryptionBlockCipherMode);
        }
    }
}