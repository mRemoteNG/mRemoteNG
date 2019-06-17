using System;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class AdvancedPage
    {
        public AdvancedPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.Config_Icon;
            var display = new DisplayProperties();
            var img = display.ScaleImage(Resources.PuttyConfig);
            btnLaunchPutty.Image = img;
        }

        #region Public Methods

        public override string PageName
        {
            get => Language.strTabAdvanced;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            lblSeconds.Text = Language.strLabelSeconds;
            lblMaximumPuttyWaitTime.Text = Language.strLabelPuttyTimeout;
            chkAutomaticReconnect.Text = Language.strCheckboxAutomaticReconnect;
            chkLoadBalanceInfoUseUtf8.Text = Language.strLoadBalanceInfoUseUtf8;
            lblConfigurePuttySessions.Text = Language.strLabelPuttySessionsConfig;
            btnLaunchPutty.Text = Language.strButtonLaunchPutty;
            btnBrowseCustomPuttyPath.Text = Language.strButtonBrowse;
            chkUseCustomPuttyPath.Text = Language.strCheckboxPuttyPath;
            chkAutomaticallyGetSessionInfo.Text = Language.strAutomaticallyGetSessionInfo;
            lblUVNCSCPort.Text = Language.strUltraVNCSCListeningPort;
        }

        public override void LoadSettings()
        {
            chkAutomaticallyGetSessionInfo.Checked = Settings.Default.AutomaticallyGetSessionInfo;
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
            Settings.Default.AutomaticallyGetSessionInfo = chkAutomaticallyGetSessionInfo.Checked;
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
                openFileDialog.Filter = $@"{Language.strFilterApplication}|*.exe|{Language.strFilterAll}|*.*";
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
                MessageBox.Show(Language.strErrorCouldNotLaunchPutty, Application.ProductName,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                Runtime.MessageCollector.AddExceptionMessage(Language.strErrorCouldNotLaunchPutty, ex);
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