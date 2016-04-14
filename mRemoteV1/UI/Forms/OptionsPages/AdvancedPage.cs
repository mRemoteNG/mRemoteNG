using mRemoteNG.Connection.Protocol;
using mRemoteNG.My;
using mRemoteNG.Tools;
using PSTaskDialog;
using System;
using System.IO;
using System.Windows.Forms;


namespace mRemoteNG.Forms.OptionsPages
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
			get
			{
				return Language.strTabAdvanced;
			}
			set
			{
			}
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
				
			chkWriteLogFile.Checked = System.Convert.ToBoolean(My.Settings.Default.WriteLogFile);
			chkEncryptCompleteFile.Checked = System.Convert.ToBoolean(My.Settings.Default.EncryptCompleteConnectionsFile);
			chkAutomaticallyGetSessionInfo.Checked = System.Convert.ToBoolean(My.Settings.Default.AutomaticallyGetSessionInfo);
			chkAutomaticReconnect.Checked = System.Convert.ToBoolean(My.Settings.Default.ReconnectOnDisconnect);
			numPuttyWaitTime.Value = System.Convert.ToDecimal(My.Settings.Default.MaxPuttyWaitTime);
				
			chkUseCustomPuttyPath.Checked = My.Settings.Default.UseCustomPuttyPath;
            txtCustomPuttyPath.Text = My.Settings.Default.CustomPuttyPath;
			SetPuttyLaunchButtonEnabled();
				
			numUVNCSCPort.Value = System.Convert.ToDecimal(My.Settings.Default.UVNCSCPort);
				
			txtXULrunnerPath.Text = System.Convert.ToString(My.Settings.Default.XULRunnerPath);
		}
			
		public override void SaveSettings()
		{
			base.SaveSettings();
				
			My.Settings.Default.WriteLogFile = chkWriteLogFile.Checked;
			My.Settings.Default.EncryptCompleteConnectionsFile = chkEncryptCompleteFile.Checked;
			My.Settings.Default.AutomaticallyGetSessionInfo = chkAutomaticallyGetSessionInfo.Checked;
			My.Settings.Default.ReconnectOnDisconnect = chkAutomaticReconnect.Checked;
				
			bool puttyPathChanged = false;
            if (!(My.Settings.Default.CustomPuttyPath == txtCustomPuttyPath.Text))
			{
				puttyPathChanged = true;
                My.Settings.Default.CustomPuttyPath = txtCustomPuttyPath.Text;
			}
            if (!(My.Settings.Default.UseCustomPuttyPath == chkUseCustomPuttyPath.Checked))
			{
				puttyPathChanged = true;
                My.Settings.Default.UseCustomPuttyPath = chkUseCustomPuttyPath.Checked;
			}
			if (puttyPathChanged)
			{
                if (My.Settings.Default.UseCustomPuttyPath)
				{
                    PuttyBase.PuttyPath = My.Settings.Default.CustomPuttyPath;
				}
				else
				{
					PuttyBase.PuttyPath = App.Info.General.PuttyPath;
				}
				Config.Putty.Sessions.AddSessionsToTree();
			}
				
			My.Settings.Default.MaxPuttyWaitTime = (int)numPuttyWaitTime.Value;
				
			My.Settings.Default.UVNCSCPort = (int)numUVNCSCPort.Value;
				
			My.Settings.Default.XULRunnerPath = txtXULrunnerPath.Text;
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
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = string.Format("{0}|*.exe|{1}|*.*", Language.strFilterApplication, Language.strFilterAll);
				openFileDialog.FileName = Path.GetFileName(App.Info.General.PuttyPath);
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
				PuttyProcessController puttyProcess = new PuttyProcessController();
				string fileName = "";
				if (chkUseCustomPuttyPath.Checked)
				{
					fileName = txtCustomPuttyPath.Text;
				}
				else
				{
					fileName = App.Info.General.PuttyPath;
				}
				puttyProcess.Start(fileName);
				puttyProcess.SetControlText("Button", "&Cancel", "&Close");
				puttyProcess.SetControlVisible("Button", "&Open", false);
				puttyProcess.WaitForExit();
			}
			catch (Exception ex)
			{
				cTaskDialog.MessageBox(System.Convert.ToString(Application.ProductName), Language.strErrorCouldNotLaunchPutty, "", ex.Message, "", "", eTaskDialogButtons.OK, eSysIcons.Error, eSysIcons.Error);
			}
		}
			
		public void btnBrowseXulRunnerPath_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog oDlg = new FolderBrowserDialog();
			oDlg.ShowNewFolderButton = false;
				
			if (oDlg.ShowDialog() == DialogResult.OK)
			{
				txtXULrunnerPath.Text = oDlg.SelectedPath;
			}
				
			oDlg.Dispose();
		}
        #endregion
			
		private void SetPuttyLaunchButtonEnabled()
		{
			string puttyPath = "";
			if (chkUseCustomPuttyPath.Checked)
			{
				puttyPath = txtCustomPuttyPath.Text;
			}
			else
			{
				puttyPath = App.Info.General.PuttyPath;
			}
				
			bool exists = false;
			try
			{
				exists = File.Exists(puttyPath);
			}
			catch
			{
			}
				
			if (exists)
			{
				lblConfigurePuttySessions.Enabled = true;
				btnLaunchPutty.Enabled = true;
			}
			else
			{
				lblConfigurePuttySessions.Enabled = false;
				btnLaunchPutty.Enabled = false;
			}
		}
        #endregion
	}
}
