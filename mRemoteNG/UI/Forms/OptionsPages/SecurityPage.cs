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

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class SecurityPage : OptionsPage
    {
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
    }
}