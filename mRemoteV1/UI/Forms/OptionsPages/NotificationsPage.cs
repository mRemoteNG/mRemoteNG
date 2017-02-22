using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class NotificationsPage
    {
        public NotificationsPage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
            get { return Language.strMenuNotifications; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            // notifications panel
            groupBoxNotifications.Text = Language.strMenuNotifications;
            labelNotificationsShowTypes.Text = Language.strShowTheseMessageTypes;
            chkShowDebugInMC.Text = Language.strDebug;
            chkShowInfoInMC.Text = Language.strInformations;
            chkShowWarningInMC.Text = Language.strWarnings;
            chkShowErrorInMC.Text = Language.strErrors;
            labelSwitchToErrorsAndInfos.Text = Language.strSwitchToErrorsAndInfos;
            chkSwitchToMCInformation.Text = Language.strInformations;
            chkSwitchToMCWarnings.Text = Language.strWarnings;
            chkSwitchToMCErrors.Text = Language.strErrors;

            // logging
            groupBoxLogging.Text = Language.strLogging;
            chkLogDebugMsgs.Text = Language.strDebug;
            chkLogInfoMsgs.Text = Language.strInformations;
            chkLogWarningMsgs.Text = Language.strWarnings;
            chkLogErrorMsgs.Text = Language.strErrors;
            chkLogToCurrentDir.Text = Language.strLogToAppDir;
            labelLogFilePath.Text = Language.strLogFilePath;
            labelLogTheseMsgTypes.Text = Language.strLogTheseMessageTypes;
            buttonOpenLogFile.Text = Language.strOpenFile;
            buttonSelectLogPath.Text = Language.strChoosePath;
            buttonRestoreDefaultLogPath.Text = Language.strUseDefault;

            // popups
            groupBoxPopups.Text = Language.strPopups;
            labelPopupShowTypes.Text = Language.strShowTheseMessageTypes;
            chkPopupDebug.Text = Language.strDebug;
            chkPopupInfo.Text = Language.strInformations;
            chkPopupWarning.Text = Language.strWarnings;
            chkPopupError.Text = Language.strErrors;
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
            chkLogToCurrentDir.Checked = Settings.Default.LogToApplicationDirectory;
            textBoxLogPath.Text = Settings.Default.LogFilePath;
            chkLogDebugMsgs.Checked = Settings.Default.TextLogMessageWriterWriteDebugMsgs;
            chkLogInfoMsgs.Checked = Settings.Default.TextLogMessageWriterWriteInfoMsgs;
            chkLogWarningMsgs.Checked = Settings.Default.TextLogMessageWriterWriteWarningMsgs;
            chkLogErrorMsgs.Checked = Settings.Default.TextLogMessageWriterWriteErrorMsgs;
        }

        private void LoadPopupSettings()
        {
            chkPopupDebug.Checked = Settings.Default.PopupMessageWriterWriteDebugMsgs;
            chkPopupInfo.Checked = Settings.Default.PopupMessageWriterWriteInfoMsgs;
            chkPopupWarning.Checked = Settings.Default.PopupMessageWriterWriteWarningMsgs;
            chkPopupError.Checked = Settings.Default.PopupMessageWriterWriteErrorMsgs;
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
            Settings.Default.LogToApplicationDirectory = chkLogToCurrentDir.Checked;
            Settings.Default.LogFilePath = textBoxLogPath.Text;
            Logger.Instance.SetLogPath(Settings.Default.LogFilePath);
            Settings.Default.TextLogMessageWriterWriteDebugMsgs = chkLogDebugMsgs.Checked;
            Settings.Default.TextLogMessageWriterWriteInfoMsgs = chkLogInfoMsgs.Checked;
            Settings.Default.TextLogMessageWriterWriteWarningMsgs = chkLogWarningMsgs.Checked;
            Settings.Default.TextLogMessageWriterWriteErrorMsgs = chkLogErrorMsgs.Checked;
        }

        private void SavePopupSettings()
        {
            Settings.Default.PopupMessageWriterWriteDebugMsgs = chkPopupDebug.Checked;
            Settings.Default.PopupMessageWriterWriteInfoMsgs = chkPopupInfo.Checked;
            Settings.Default.PopupMessageWriterWriteWarningMsgs = chkPopupWarning.Checked;
            Settings.Default.PopupMessageWriterWriteErrorMsgs = chkPopupError.Checked;
        }

        private void buttonSelectLogPath_Click(object sender, System.EventArgs e)
        {
            var currentFile = textBoxLogPath.Text;
            var currentDirectory = Path.GetDirectoryName(currentFile);
            saveFileDialogLogging.Title = Language.strChooseLogPath;
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

        private void chkLogToCurrentDir_CheckedChanged(object sender, System.EventArgs e)
        {
            buttonSelectLogPath.Enabled = !chkLogToCurrentDir.Checked;
            buttonRestoreDefaultLogPath.Enabled = !chkLogToCurrentDir.Checked;
            textBoxLogPath.Text = Logger.DefaultLogPath;
        }
    }
}