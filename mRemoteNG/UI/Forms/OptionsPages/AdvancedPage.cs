using System;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Properties;
using mRemoteNG.Tools;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class AdvancedPage
    {
        public AdvancedPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.Settings_16x);
            var display = new DisplayProperties();
            var img = display.ScaleImage(Properties.Resources.PuttyConfig);
            btnLaunchPutty.Image = img;
        }

        #region Public Methods

        public override string PageName
        {
            get => Language.Advanced;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            lblSeconds.Text = Language.Seconds;
            lblMaximumPuttyWaitTime.Text = Language.PuttyTimeout;
            chkAutomaticReconnect.Text = Language.CheckboxAutomaticReconnect;
            chkLoadBalanceInfoUseUtf8.Text = Language.LoadBalanceInfoUseUtf8;
            lblConfigurePuttySessions.Text = Language.PuttySessionsConfig;
            btnLaunchPutty.Text = Language.ButtonLaunchPutty;
            btnBrowseCustomPuttyPath.Text = Language._Browse;
            chkUseCustomPuttyPath.Text = Language.CheckboxPuttyPath;
            lblUVNCSCPort.Text = Language.UltraVNCSCListeningPort;
        }

        public override void LoadSettings()
        {
            chkAutomaticReconnect.Checked = Settings.Default.ReconnectOnDisconnect;
            chkLoadBalanceInfoUseUtf8.Checked = Settings.Default.RdpLoadBalanceInfoUseUtf8;
            numPuttyWaitTime.Value = Settings.Default.MaxPuttyWaitTime;

            chkUseCustomPuttyPath.Checked = Settings.Default.UseCustomPuttyPath;
            txtCustomPuttyPath.Text = Settings.Default.CustomPuttyPath;
            SetPuttyLaunchButtonEnabled();

            numUVNCSCPort.Value = Settings.Default.UVNCSCPort;
        }

        public override void SaveSettings()
        {
            Settings.Default.ReconnectOnDisconnect = chkAutomaticReconnect.Checked;
            Settings.Default.RdpLoadBalanceInfoUseUtf8 = chkLoadBalanceInfoUseUtf8.Checked;

            var puttyPathChanged = false;
            if (Settings.Default.CustomPuttyPath != txtCustomPuttyPath.Text)
            {
                puttyPathChanged = true;
                Settings.Default.CustomPuttyPath = txtCustomPuttyPath.Text;
            }

            if (Settings.Default.UseCustomPuttyPath != chkUseCustomPuttyPath.Checked)
            {
                puttyPathChanged = true;
                Settings.Default.UseCustomPuttyPath = chkUseCustomPuttyPath.Checked;
            }

            if (puttyPathChanged)
            {
                PuttyBase.PuttyPath = Settings.Default.UseCustomPuttyPath
                    ? Settings.Default.CustomPuttyPath
                    : GeneralAppInfo.PuttyPath;
                PuttySessionsManager.Instance.AddSessions();
            }

            Settings.Default.MaxPuttyWaitTime = (int)numPuttyWaitTime.Value;
            Settings.Default.UVNCSCPort = (int)numUVNCSCPort.Value;
        }

        #endregion

        #region Private Methods

        #region Event Handlers

        private void chkUseCustomPuttyPath_CheckedChanged(object sender, EventArgs e)
        {
            txtCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked;
            btnBrowseCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked;
            SetPuttyLaunchButtonEnabled();
        }

        private void txtCustomPuttyPath_TextChanged(object sender, EventArgs e)
        {
            SetPuttyLaunchButtonEnabled();
        }

        private void btnBrowseCustomPuttyPath_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = $@"{Language.FilterApplication}|*.exe|{Language.FilterAll}|*.*";
                openFileDialog.FileName = Path.GetFileName(GeneralAppInfo.PuttyPath);
                openFileDialog.CheckFileExists = true;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                txtCustomPuttyPath.Text = openFileDialog.FileName;
                SetPuttyLaunchButtonEnabled();
            }
        }

        private void btnLaunchPutty_Click(object sender, EventArgs e)
        {
            try
            {
                var puttyProcess = new PuttyProcessController();
                var fileName = chkUseCustomPuttyPath.Checked ? txtCustomPuttyPath.Text : GeneralAppInfo.PuttyPath;
                puttyProcess.Start(fileName);
                puttyProcess.SetControlText("Button", "&Cancel", "&Close");
                puttyProcess.SetControlVisible("Button", "&Open", false);
                puttyProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.ErrorCouldNotLaunchPutty, Application.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                Runtime.MessageCollector.AddExceptionMessage(Language.ErrorCouldNotLaunchPutty, ex);
            }
        }

        #endregion

        private void SetPuttyLaunchButtonEnabled()
        {
            var puttyPath = chkUseCustomPuttyPath.Checked ? txtCustomPuttyPath.Text : GeneralAppInfo.PuttyPath;

            var exists = false;
            try
            {
                exists = File.Exists(puttyPath);
            }
            catch
            {
                // ignored
            }

            lblConfigurePuttySessions.Enabled = exists;
            btnLaunchPutty.Enabled = exists;
        }

        #endregion
    }
}