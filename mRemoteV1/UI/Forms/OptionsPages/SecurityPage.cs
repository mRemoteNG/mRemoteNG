using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;
using mRemoteNG.Security;
using mRemoteNG.Security.SymmetricEncryption;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class SecurityPage : OptionsPage
    {
        public SecurityPage()
        {
            InitializeComponent();

            // Set available values
            cryptoTypeComboBox.DataSource = Enum.GetValues(typeof(CryptoProviders));
            bceRadioAES.Tag = BlockCipherEngines.AES;
            bceRadioSerpent.Tag = BlockCipherEngines.Serpent;
            bceRadioTwofish.Tag = BlockCipherEngines.Twofish;
            bcmRadioCcm.Tag = BlockCipherModes.CCM;
            bcmRadioEax.Tag = BlockCipherModes.EAX;
            bcmRadioGcm.Tag = BlockCipherModes.GCM;

        }

        public override string PageName
        {
            get { return Language.strSecurity; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();
            chkEncryptCompleteFile.Text = Language.strEncryptCompleteConnectionFile;
            cryptoProviderLabel.Text = Language.strCryptoProvider;
            cryptoGroupBox.Text = Language.strCryptography;
            blockCipherEngineGroup.Text = Language.strBlockCipherEngine;
            blockCipherModeGroup.Text = Language.strBlockCipherMode;
            bceRadioAES.Text = Language.strBCEAES;
            bceRadioTwofish.Text = Language.strBCETwofish;
            bceRadioSerpent.Text = Language.strBCESerpent;
            bcmRadioCcm.Text = Language.strBCMCcm;
            bcmRadioEax.Text = Language.strBCMEax;
            bcmRadioGcm.Text = Language.strBCMGcm;
        }

        public override void LoadSettings()
        {
            base.SaveSettings();

            chkEncryptCompleteFile.Checked = Settings.Default.EncryptCompleteConnectionsFile;

            // Determine the set crypto provider
            switch((CryptoProviders)Settings.Default.CryptoProvider)
            {
                case CryptoProviders.Rijndael:
                    cryptoTypeComboBox.SelectedItem = CryptoProviders.Rijndael;
                    break;
                case CryptoProviders.AEAD:
                    cryptoTypeComboBox.SelectedItem = CryptoProviders.AEAD;
                    break;
                default:
                    break;
            }

            // Determine the set Crypto Engine and Mode
            switch((BlockCipherEngines)Settings.Default.CryptoBlockCipherEngine)
            {
                case BlockCipherEngines.AES:
                    bceRadioAES.Checked = true;
                    break;
                case BlockCipherEngines.Serpent:
                    bceRadioSerpent.Checked = true;
                    break;
                case BlockCipherEngines.Twofish:
                    bceRadioTwofish.Checked = true;
                    break;
            }

            switch ((BlockCipherModes)Settings.Default.CryptoBlockCipherMode)
            {
                case BlockCipherModes.CCM:
                    bcmRadioCcm.Checked = true;
                    break;
                case BlockCipherModes.EAX:
                    bcmRadioEax.Checked = true;
                    break;
                case BlockCipherModes.GCM:
                    bcmRadioGcm.Checked = true;
                    break;
            }
        }

        public override void SaveSettings()
        {
            var cryptoCipherEngineChoice = blockCipherEngineGroup.Controls.OfType<RadioButton>().FirstOrDefault(x => x.Checked);
            var cryptoCipherEngineModeChoice = blockCipherModeGroup.Controls.OfType<RadioButton>().FirstOrDefault(x => x.Checked);

            Settings.Default.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;
            Settings.Default.CryptoProvider = (int)cryptoTypeComboBox.SelectedItem;
            Settings.Default.CryptoBlockCipherEngine = (int)cryptoCipherEngineChoice.Tag;
            Settings.Default.CryptoBlockCipherMode = (int)cryptoCipherEngineModeChoice.Tag;
            Settings.Default.Save();
        }

        private void cryptoTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If the AEAD provider has been selected, then we can enable the additional group boxes
            if((CryptoProviders)cryptoTypeComboBox.SelectedItem == CryptoProviders.AEAD)
            {
                blockCipherEngineGroup.Enabled = true;
                blockCipherModeGroup.Enabled = true;
            }
            else
            {
                blockCipherEngineGroup.Enabled = false;
                blockCipherModeGroup.Enabled = false;
            }
        }
    }
}
