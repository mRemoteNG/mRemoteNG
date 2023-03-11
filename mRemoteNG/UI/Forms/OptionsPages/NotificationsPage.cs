using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
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
            chkShowDebugInMC.Checked = Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs;
            chkShowInfoInMC.Checked = Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs;
            chkShowWarningInMC.Checked = Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs;
            chkShowErrorInMC.Checked = Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs;
            chkSwitchToMCInformation.Checked = Properties.OptionsNotificationsPage.Default.SwitchToMCOnInformation;
            chkSwitchToMCWarnings.Checked = Properties.OptionsNotificationsPage.Default.SwitchToMCOnWarning;
            chkSwitchToMCErrors.Checked = Properties.OptionsNotificationsPage.Default.SwitchToMCOnError;
        }

        private void LoadLoggingSettings()
        {
            chkLogToCurrentDir.Checked = Properties.OptionsNotificationsPage.Default.LogToApplicationDirectory;
            textBoxLogPath.Text = Properties.OptionsNotificationsPage.Default.LogFilePath;
            chkLogDebugMsgs.Checked = Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs;
            chkLogInfoMsgs.Checked = Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs;
            chkLogWarningMsgs.Checked = Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs;
            chkLogErrorMsgs.Checked = Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs;
        }

        private void LoadPopupSettings()
        {
            chkPopupDebug.Checked = Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs;
            chkPopupInfo.Checked = Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs;
            chkPopupWarning.Checked = Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs;
            chkPopupError.Checked = Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs;
        }

        private void SaveNotificationPanelSettings()
        {
            Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs = chkShowDebugInMC.Checked;
            Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs = chkShowInfoInMC.Checked;
            Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs = chkShowWarningInMC.Checked;
            Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs = chkShowErrorInMC.Checked;
            Properties.OptionsNotificationsPage.Default.SwitchToMCOnInformation = chkSwitchToMCInformation.Checked;
            Properties.OptionsNotificationsPage.Default.SwitchToMCOnWarning = chkSwitchToMCWarnings.Checked;
            Properties.OptionsNotificationsPage.Default.SwitchToMCOnError = chkSwitchToMCErrors.Checked;
        }

        private void SaveLoggingSettings()
        {
            Properties.OptionsNotificationsPage.Default.LogToApplicationDirectory = chkLogToCurrentDir.Checked;
            Properties.OptionsNotificationsPage.Default.LogFilePath = textBoxLogPath.Text;
            Logger.Instance.SetLogPath(Properties.OptionsNotificationsPage.Default.LogFilePath);
            Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs = chkLogDebugMsgs.Checked;
            Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs = chkLogInfoMsgs.Checked;
            Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs = chkLogWarningMsgs.Checked;
            Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs = chkLogErrorMsgs.Checked;
        }

        private void SavePopupSettings()
        {
            Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs = chkPopupDebug.Checked;
            Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs = chkPopupInfo.Checked;
            Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs = chkPopupWarning.Checked;
            Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs = chkPopupError.Checked;
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