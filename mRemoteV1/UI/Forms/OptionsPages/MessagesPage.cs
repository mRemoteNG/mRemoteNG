using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class MessagesPage
    {
        public MessagesPage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
            get { return "Messages"; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            // notifications panel
            lblSwitchToErrorsAndInfos.Text = Language.strSwitchToErrorsAndInfos;
            chkSwitchToMCInformation.Text = Language.strInformations;
            chkSwitchToMCWarnings.Text = Language.strWarnings;
            chkSwitchToMCErrors.Text = Language.strErrors;
        }

        public override void LoadSettings()
        {
            base.SaveSettings();
            LoadNotificationPanelSettings();
            LoadLoggingSettings();
            LoadPopupSettings();
        }

        public override void SaveSettings()
        {
            SaveNotificationPanelSettings();
            SaveLoggingSettings();
            SavePopupSettings();
            Settings.Default.Save();
            Runtime.MessageWriters.Clear();
            MessageCollectorSetup.BuildMessageWritersFromSettings(Runtime.MessageWriters);
        }

        private void LoadNotificationPanelSettings()
        {
            chkShowDebugInMC.Checked = Settings.Default.NotificationPanelWriterWriteDebugMsgs;
            chkShowInfoInMC.Checked = Settings.Default.NotificationPanelWriterWriteInfoMsgs;
            chkShowWarningInMC.Checked = Settings.Default.NotificationPanelWriterWriteWarningMsgs;
            chkShowErrorInMC.Checked = Settings.Default.NotificationPanelWriterWriteErrorMsgs;
            chkSwitchToMCInformation.Checked = Settings.Default.SwitchToMCOnInformation;
            chkSwitchToMCWarnings.Checked = Settings.Default.SwitchToMCOnWarning;
            chkSwitchToMCErrors.Checked = Settings.Default.SwitchToMCOnError;
        }

        private void LoadLoggingSettings()
        {
            textBoxLogPath.Text = Settings.Default.LogFilePath;
            chkLogDebugMsgs.Checked = Settings.Default.TextLogMessageWriterWriteDebugMsgs;
            chkLogInfoMsgs.Checked = Settings.Default.TextLogMessageWriterWriteInfoMsgs;
            chkLogWarningMsgs.Checked = Settings.Default.TextLogMessageWriterWriteWarningMsgs;
            chkLogErrorMsgs.Checked = Settings.Default.TextLogMessageWriterWriteErrorMsgs;
        }

        private void LoadPopupSettings()
        {
            
        }

        private void SaveNotificationPanelSettings()
        {
            Settings.Default.NotificationPanelWriterWriteDebugMsgs = chkShowDebugInMC.Checked;
            Settings.Default.NotificationPanelWriterWriteInfoMsgs = chkShowInfoInMC.Checked;
            Settings.Default.NotificationPanelWriterWriteWarningMsgs = chkShowWarningInMC.Checked;
            Settings.Default.NotificationPanelWriterWriteErrorMsgs = chkShowErrorInMC.Checked;
            Settings.Default.SwitchToMCOnInformation = chkSwitchToMCInformation.Checked;
            Settings.Default.SwitchToMCOnWarning = chkSwitchToMCWarnings.Checked;
            Settings.Default.SwitchToMCOnError = chkSwitchToMCErrors.Checked;
            
        }

        private void SaveLoggingSettings()
        {
            Settings.Default.LogFilePath = textBoxLogPath.Text;
            Settings.Default.TextLogMessageWriterWriteDebugMsgs = chkLogDebugMsgs.Checked;
            Settings.Default.TextLogMessageWriterWriteInfoMsgs = chkLogInfoMsgs.Checked;
            Settings.Default.TextLogMessageWriterWriteWarningMsgs = chkLogWarningMsgs.Checked;
            Settings.Default.TextLogMessageWriterWriteErrorMsgs = chkLogErrorMsgs.Checked;
        }

        private void SavePopupSettings()
        {

        }

        private void buttonSelectLogPath_Click(object sender, System.EventArgs e)
        {
            var currentFile = textBoxLogPath.Text;
            var currentDirectory = Path.GetDirectoryName(currentFile);
            saveFileDialogLogging.Title = "Choose a location to save the mRemoteNG log file";
            saveFileDialogLogging.Filter = @"Log file|*.log";
            saveFileDialogLogging.InitialDirectory = currentDirectory;
            saveFileDialogLogging.FileName = currentFile;
            var dialogResult = saveFileDialogLogging.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            textBoxLogPath.Text = saveFileDialogLogging.FileName;
        }

        private void buttonRestoreDefaultLogPath_Click(object sender, System.EventArgs e)
        {
            textBoxLogPath.Text = Logger.DefaultLogPath;
        }

        private void buttonOpenLogFile_Click(object sender, System.EventArgs e)
        {
            if (Path.GetExtension(textBoxLogPath.Text) == ".log")
                Process.Start(textBoxLogPath.Text);
        }
    }
}