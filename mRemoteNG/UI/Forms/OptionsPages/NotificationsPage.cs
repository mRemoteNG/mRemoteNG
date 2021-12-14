using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class NotificationsPage
    {
        public NotificationsPage()
        {
            InitializeComponent();
            ApplyTheme();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.LogError_16x);
        }

        public override string PageName
        {
            get => Language.Notifications;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            // notifications panel
            groupBoxNotifications.Text = Language.Notifications;
            labelNotificationsShowTypes.Text = Language.ShowTheseMessageTypes;
            chkShowDebugInMC.Text = Language.Debug;
            chkShowInfoInMC.Text = Language.Informations;
            chkShowWarningInMC.Text = Language.Warnings;
            chkShowErrorInMC.Text = Language.Errors;
            labelSwitchToErrorsAndInfos.Text = Language.SwitchToErrorsAndInfos;
            chkSwitchToMCInformation.Text = Language.Informations;
            chkSwitchToMCWarnings.Text = Language.Warnings;
            chkSwitchToMCErrors.Text = Language.Errors;

            // logging
            groupBoxLogging.Text = Language.Logging;
            chkLogDebugMsgs.Text = Language.Debug;
            chkLogInfoMsgs.Text = Language.Informations;
            chkLogWarningMsgs.Text = Language.Warnings;
            chkLogErrorMsgs.Text = Language.Errors;
            chkLogToCurrentDir.Text = Language.LogToAppDir;
            labelLogFilePath.Text = Language.LogFilePath;
            labelLogTheseMsgTypes.Text = Language.LogTheseMessageTypes;
            buttonOpenLogFile.Text = Language.OpenFile;
            buttonSelectLogPath.Text = Language.ChoosePath;
            buttonRestoreDefaultLogPath.Text = Language.UseDefault;

            // popups
            groupBoxPopups.Text = Language.Popups;
            labelPopupShowTypes.Text = Language.ShowTheseMessageTypes;
            chkPopupDebug.Text = Language.Debug;
            chkPopupInfo.Text = Language.Informations;
            chkPopupWarning.Text = Language.Warnings;
            chkPopupError.Text = Language.Errors;
        }

        public override void LoadSettings()
        {
            LoadNotificationPanelSettings();
            LoadLoggingSettings();
            LoadPopupSettings();
        }

        public override void SaveSettings()
        {
            SaveNotificationPanelSettings();
            SaveLoggingSettings();
            SavePopupSettings();
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
            saveFileDialogLogging.Title = Language.ChooseLogPath;
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