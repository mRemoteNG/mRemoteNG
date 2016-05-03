using System;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using mRemoteNG.Config.Putty;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.My;
using mRemoteNG.Tools;
using PSTaskDialog;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class AdvancedPage
    {
        public AdvancedPage()
        {
            InitializeComponent();
        }

        #region Public Methods

        public override string PageName
        {
            get { return Language.strTabAdvanced; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            lblSeconds.Text = Language.strLabelSeconds;
            lblMaximumPuttyWaitTime.Text = Language.strLabelPuttyTimeout;
            chkAutomaticReconnect.Text = Language.strCheckboxAutomaticReconnect;
            lblConfigurePuttySessions.Text = Language.strLabelPuttySessionsConfig;
            btnLaunchPutty.Text = Language.strButtonLaunchPutty;
            btnBrowseCustomPuttyPath.Text = Language.strButtonBrowse;
            chkUseCustomPuttyPath.Text = Language.strCheckboxPuttyPath;
            chkAutomaticallyGetSessionInfo.Text = Language.strAutomaticallyGetSessionInfo;
            chkWriteLogFile.Text = Language.strWriteLogFile;
            lblUVNCSCPort.Text = Language.strUltraVNCSCListeningPort;
            lblXulRunnerPath.Text = Language.strXULrunnerPath;
            btnBrowseXulRunnerPath.Text = Language.strButtonBrowse;
            chkEncryptCompleteFile.Text = Language.strEncryptCompleteConnectionFile;
        }

        public override void LoadSettings()
        {
            base.SaveSettings();

            chkWriteLogFile.Checked = Settings.Default.WriteLogFile;

            chkEncryptCompleteFile.Checked = Settings.Default.EncryptCompleteConnectionsFile;
            chkAutomaticallyGetSessionInfo.Checked = Settings.Default.AutomaticallyGetSessionInfo;
            chkAutomaticReconnect.Checked = Settings.Default.ReconnectOnDisconnect;
            numPuttyWaitTime.Value = Settings.Default.MaxPuttyWaitTime;

            chkUseCustomPuttyPath.Checked = Settings.Default.UseCustomPuttyPath;
            txtCustomPuttyPath.Text = Settings.Default.CustomPuttyPath;
            SetPuttyLaunchButtonEnabled();

            numUVNCSCPort.Value = Settings.Default.UVNCSCPort;

            txtXULrunnerPath.Text = Settings.Default.XULRunnerPath;
        }

        public override void SaveSettings()
        {
            Settings.Default.WriteLogFile = chkWriteLogFile.Checked;
            Settings.Default.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;
            Settings.Default.AutomaticallyGetSessionInfo = chkAutomaticallyGetSessionInfo.Checked;
            Settings.Default.ReconnectOnDisconnect = chkAutomaticReconnect.Checked;

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
                PuttyBase.PuttyPath = Settings.Default.UseCustomPuttyPath ? Settings.Default.CustomPuttyPath : GeneralAppInfo.PuttyPath;
                Sessions.AddSessionsToTree();
            }

            Settings.Default.MaxPuttyWaitTime = (int) numPuttyWaitTime.Value;
            Settings.Default.UVNCSCPort = (int) numUVNCSCPort.Value;
            Settings.Default.XULRunnerPath = txtXULrunnerPath.Text;

            Settings.Default.Save();
        }

        #endregion

        #region Private Methods

        #region Event Handlers

        public void chkUseCustomPuttyPath_CheckedChanged(object sender, EventArgs e)
        {
            txtCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked;
            btnBrowseCustomPuttyPath.Enabled = chkUseCustomPuttyPath.Checked;
            SetPuttyLaunchButtonEnabled();
        }

        public void txtCustomPuttyPath_TextChanged(object sender, EventArgs e)
        {
            SetPuttyLaunchButtonEnabled();
        }

        public void btnBrowseCustomPuttyPath_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = $"{Language.strFilterApplication}|*.exe|{Language.strFilterAll}|*.*";
                openFileDialog.FileName = Path.GetFileName(GeneralAppInfo.PuttyPath);
                openFileDialog.CheckFileExists = true;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtCustomPuttyPath.Text = openFileDialog.FileName;
                    SetPuttyLaunchButtonEnabled();
                }
            }
        }

        public void btnLaunchPutty_Click(object sender, EventArgs e)
        {
            try
            {
                var puttyProcess = new PuttyProcessController();
                var fileName = "";
                fileName = chkUseCustomPuttyPath.Checked ? txtCustomPuttyPath.Text : GeneralAppInfo.PuttyPath;
                puttyProcess.Start(fileName);
                puttyProcess.SetControlText("Button", "&Cancel", "&Close");
                puttyProcess.SetControlVisible("Button", "&Open", false);
                puttyProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                cTaskDialog.MessageBox(Convert.ToString(Application.ProductName), Language.strErrorCouldNotLaunchPutty,
                    "", ex.Message, "", "", eTaskDialogButtons.OK, eSysIcons.Error, eSysIcons.Error);
            }
        }

        public void btnBrowseXulRunnerPath_Click(object sender, EventArgs e)
        {
            var oDlg = new FolderBrowserDialog {ShowNewFolderButton = false};

            if (oDlg.ShowDialog() == DialogResult.OK)
            {
                txtXULrunnerPath.Text = oDlg.SelectedPath;
            }

            oDlg.Dispose();
        }

        #endregion

        private void SetPuttyLaunchButtonEnabled()
        {
            var puttyPath = "";
            puttyPath = chkUseCustomPuttyPath.Checked ? txtCustomPuttyPath.Text : GeneralAppInfo.PuttyPath;

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