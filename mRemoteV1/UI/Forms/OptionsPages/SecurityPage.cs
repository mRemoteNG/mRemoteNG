﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Config.Serializers.Xml;
using mRemoteNG.Security;
using mRemoteNG.Security.Factories;

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
            PageIcon = Resources.Shield_Icon;
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
            btnTestSettings.Text = Language.TestSettings;
        }

        public override void LoadSettings()
        {
            chkEncryptCompleteFile.Checked = Settings.Default.EncryptCompleteConnectionsFile;
            comboBoxEncryptionEngine.Text = Enum.GetName(typeof(BlockCipherEngines), Settings.Default.EncryptionEngine);
            comboBoxBlockCipher.Text =
                Enum.GetName(typeof(BlockCipherModes), Settings.Default.EncryptionBlockCipherMode);
            numberBoxKdfIterations.Value = Settings.Default.EncryptionKeyDerivationIterations;
        }

        public override void SaveSettings()
        {
            Settings.Default.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;
            Settings.Default.EncryptionEngine = (BlockCipherEngines)comboBoxEncryptionEngine.SelectedItem;
            Settings.Default.EncryptionBlockCipherMode = (BlockCipherModes)comboBoxBlockCipher.SelectedItem;
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

        private void BtnTestSettings_Click(object sender, EventArgs e)
        {
            var connectionTree = Runtime.ConnectionsService.ConnectionTreeModel;
            if (!connectionTree.RootNodes.Any())
                return;

            var engine = (BlockCipherEngines) comboBoxEncryptionEngine.SelectedItem;
            var mode = (BlockCipherModes) comboBoxBlockCipher.SelectedItem;
            var cryptographyProvider = new CryptoProviderFactory(engine, mode).Build();
            cryptographyProvider.KeyDerivationIterations = (int) numberBoxKdfIterations.Value;

            var serializerFactory = new XmlConnectionSerializerFactory();
            var serializer = serializerFactory.Build(cryptographyProvider, connectionTree, useFullEncryption: chkEncryptCompleteFile.Checked);
            var nodeCount = connectionTree.GetRecursiveChildList().Count;

            var timer = Stopwatch.StartNew();
            var rootNode = connectionTree.RootNodes.First();
            serializer.Serialize(rootNode);
            timer.Stop();

            MessageBox.Show(this,
                string.Format(Language.EncryptionTestResultMessage,
                nodeCount, engine, mode, numberBoxKdfIterations.Value, timer.Elapsed.TotalSeconds),
                Language.EncryptionTest,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}