using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Microsoft.Win32;
using mRemoteNG.App.Info;
using mRemoteNG.Tools.WindowsRegistry;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public sealed partial class OptRegistryNotificationsPage
    {

        #region Notification Panel Settings

        /// <summary>
        /// Specifies whether debug messages are written to the notification panel.
        /// </summary>
        public WinRegistryEntry<bool> NfpWriteDebugMsgs { get; private set; }

        /// <summary>
        /// Specifies whether information messages are written to the notification panel.
        /// </summary>
        public WinRegistryEntry<bool> NfpWriteInfoMsgs { get; private set; }

        /// <summary>
        /// Specifies whether warning messages are written to the notification panel.
        /// </summary>
        public WinRegistryEntry<bool> NfpWriteWarningMsgs { get; private set; }

        /// <summary>
        /// Specifies whether error messages are written to the notification panel.
        /// </summary>
        public WinRegistryEntry<bool> NfpWriteErrorMsgs { get; private set;  }

        /// <summary>
        /// Specifies whether to switch to notification panel when information messages are received.
        /// </summary>
        public WinRegistryEntry<bool> SwitchToMCOnInformation { get; private set; }

        /// <summary>
        /// Specifies whether to switch to notification panel when warning messages are received.
        /// </summary>
        public WinRegistryEntry<bool> SwitchToMCOnWarning { get; private set; }

        /// <summary>
        /// Specifies whether to switch to notification panel when error messages are received.
        /// </summary>
        public WinRegistryEntry<bool> SwitchToMCOnError { get; private set; }

        #endregion

        #region Logging Panel Settings

        /// <summary>
        /// Specifies whether logs should be written to the application directory.
        /// </summary>
        public WinRegistryEntry<bool> LogToApplicationDirectory { get; private set; }

        /// <summary>
        /// Specifies the file path for logging.
        /// </summary>
        public WinRegistryEntry<string> LogFilePath { get; private set; }

        /// <summary>
        /// Specifies whether debug messages should be written to the text log.
        /// </summary>
        public WinRegistryEntry<bool> LfWriteDebugMsgs { get; private set; }

        /// <summary>
        /// Specifies whether information messages should be written to the text log.
        /// </summary>
        public WinRegistryEntry<bool> LfWriteInfoMsgs { get; private set; }

        /// <summary>
        /// Specifies whether warning messages should be written to the text log.
        /// </summary>
        public WinRegistryEntry<bool> LfWriteWarningMsgs { get; private set; }

        /// <summary>
        /// Specifies whether error messages should be written to the text log.
        /// </summary>
        public WinRegistryEntry<bool> LfWriteErrorMsgs { get; private set; }

        #endregion

        #region Popup Panel Settings

        /// <summary>
        /// Specifies whether debug messages should be displayed as popups.
        /// </summary>
        public WinRegistryEntry<bool> PuWriteDebugMsgs { get; private set; }

        /// <summary>
        /// Specifies whether information messages should be displayed as popups.
        /// </summary>
        public WinRegistryEntry<bool> PuWriteInfoMsgs { get; private set; }

        /// <summary>
        /// Specifies whether warning messages should be displayed as popups.
        /// </summary>
        public WinRegistryEntry<bool> PuWriteWarningMsgs { get; private set; }

        /// <summary>
        /// Specifies whether error messages should be displayed as popups.
        /// </summary>
        public WinRegistryEntry<bool> PuWriteErrorMsgs { get; private set; }

        #endregion

        public OptRegistryNotificationsPage()
        {
            IRegistryRead regValueUtility = new WinRegistry();
            RegistryHive hive = WindowsRegistryInfo.Hive;
            string subKey = WindowsRegistryInfo.NotificationOptions;

            // Notification Panel Settings
            NfpWriteDebugMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(NfpWriteDebugMsgs)).Read();
            NfpWriteInfoMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(NfpWriteInfoMsgs)).Read();
            NfpWriteWarningMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(NfpWriteWarningMsgs)).Read();
            NfpWriteErrorMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(NfpWriteErrorMsgs)).Read();
            SwitchToMCOnInformation = new WinRegistryEntry<bool>(hive, subKey, nameof(SwitchToMCOnInformation)).Read();
            SwitchToMCOnWarning = new WinRegistryEntry<bool>(hive, subKey, nameof(SwitchToMCOnWarning)).Read();
            SwitchToMCOnError = new WinRegistryEntry<bool>(hive, subKey, nameof(SwitchToMCOnError)).Read();

            // Logging Panel Settings
            LogToApplicationDirectory = new WinRegistryEntry<bool>(hive, subKey, nameof(LogToApplicationDirectory)).Read();
            LogFilePath = new WinRegistryEntry<string>(hive, subKey, nameof(LogFilePath)).Read();
            LfWriteDebugMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(LfWriteDebugMsgs)).Read();
            LfWriteInfoMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(LfWriteInfoMsgs)).Read();
            LfWriteWarningMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(LfWriteWarningMsgs)).Read();
            LfWriteErrorMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(LfWriteErrorMsgs)).Read();

            // Popup Panel Settings
            PuWriteDebugMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(PuWriteDebugMsgs)).Read();
            PuWriteInfoMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(PuWriteInfoMsgs)).Read();
            PuWriteWarningMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(PuWriteWarningMsgs)).Read();
            PuWriteErrorMsgs = new WinRegistryEntry<bool>(hive, subKey, nameof(PuWriteErrorMsgs)).Read();

            SetupValidation();
            Apply();
        }

        /// <summary>
        /// Configures validation settings for various parameters
        /// </summary>
        private void SetupValidation()
        {

        }

        /// <summary>
        /// Applies registry settings and overrides various properties.
        /// </summary>
        private void Apply()
        {
            LoadNotificationPanelSettings();
            LoadLoggingPanelSettings();
            LoadPopupPanelSettings();
        }

        private void LoadNotificationPanelSettings()
        {
            // AllowNotifications reg setting: if false disable
            if (!CommonRegistrySettings.AllowNotifications)
            {
                Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs = false;
                Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs = false;
                Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs = false;
                Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs = false;

                Properties.OptionsNotificationsPage.Default.SwitchToMCOnInformation = false;
                Properties.OptionsNotificationsPage.Default.SwitchToMCOnWarning = false;
                Properties.OptionsNotificationsPage.Default.SwitchToMCOnError = false;
                return;
            }

            // NfpWriteDebugMsgs reg setting: set NotificationPanelWriterWriteDebugMsgs option based on value
            if (NfpWriteDebugMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteDebugMsgs = NfpWriteDebugMsgs.Value;

            // NfpWriteInfoMsgs reg setting: set NotificationPanelWriterWriteInfoMsgs option based on value
            if (NfpWriteInfoMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteInfoMsgs = NfpWriteInfoMsgs.Value;

            // NfpWriteWarningMsgs reg setting: set NotificationPanelWriterWriteWarningMsgs option based on value
            if (NfpWriteWarningMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteWarningMsgs = NfpWriteWarningMsgs.Value;

            // NfpWriteErrorMsgs reg setting: set NotificationPanelWriterWriteErrorMsgs option based on value
            if (NfpWriteErrorMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.NotificationPanelWriterWriteErrorMsgs = NfpWriteErrorMsgs.Value;

            // SwitchToMCOnInformation reg setting: set SwitchToMCOnInformation option based on value
            if (SwitchToMCOnInformation.IsSet)
                Properties.OptionsNotificationsPage.Default.SwitchToMCOnInformation = SwitchToMCOnInformation.Value;

            // SwitchToMCOnWarning reg setting: set SwitchToMCOnWarning option based on value
            if (SwitchToMCOnWarning.IsSet)
                Properties.OptionsNotificationsPage.Default.SwitchToMCOnWarning = SwitchToMCOnWarning.Value;

            // SwitchToMCOnError reg setting: set SwitchToMCOnError option based on value
            if (SwitchToMCOnError.IsSet)
                Properties.OptionsNotificationsPage.Default.SwitchToMCOnError = SwitchToMCOnError.Value;
        }

        private void LoadLoggingPanelSettings()
        {
            // AllowLogging reg setting: if false disable
            if (!CommonRegistrySettings.AllowLogging)
            {
                Properties.OptionsNotificationsPage.Default.LogToApplicationDirectory = true;
                Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs = false;
                Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs = false;
                Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs = false;
                Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs = false;
                return;
            }

            // LogToApplicationDirectory reg setting: set LogToApplicationDirectory option based on value
            if (LogToApplicationDirectory.IsSet)
                Properties.OptionsNotificationsPage.Default.LogToApplicationDirectory = LogToApplicationDirectory.Value;

            // LogFilePath reg setting:
            // 1. Check that configured path is valid
            // 2. Set LogFilePath value
            // 3. Ensure LogToApplicationDirectory is false
            // 4. Disable all controls
            if (LogFilePath.IsSet && PathIsValid(LogFilePath.Value))
            {
                Properties.OptionsNotificationsPage.Default.LogFilePath = LogFilePath.Value;
                Properties.OptionsNotificationsPage.Default.LogToApplicationDirectory = false;
            }

            // LfWriteDebugMsgs reg setting: set TextLogMessageWriterWriteDebugMsgs option based on value
            if (LfWriteDebugMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteDebugMsgs = LfWriteDebugMsgs.Value;

            // LfWriteInfoMsgs reg setting: set TextLogMessageWriterWriteInfoMsgs option based on value
            if (LfWriteInfoMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteInfoMsgs = LfWriteInfoMsgs.Value;

            // LfWriteWarningMsgs reg setting: set TextLogMessageWriterWriteWarningMsgs option based on value
            if (LfWriteWarningMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteWarningMsgs = LfWriteWarningMsgs.Value;

            // LfWriteErrorMsgs reg setting: set TextLogMessageWriterWriteErrorMsgs option based on value
            if (LfWriteErrorMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.TextLogMessageWriterWriteErrorMsgs = LfWriteErrorMsgs.Value;
        }

        private void LoadPopupPanelSettings()
        {
            // AllowPopups reg setting: if false disable
            if (!CommonRegistrySettings.AllowPopups)
            {
                Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs = false;
                Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs = false;
                Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs = false;
                Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs = false;
                return;
            }

            // PuWriteDebugMsgs reg setting: set PopupMessageWriterWriteDebugMsgs option based on value
            if (PuWriteDebugMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteDebugMsgs = PuWriteDebugMsgs.Value;

            // PuWriteInfoMsgs reg setting: set PopupMessageWriterWriteInfoMsgs option based on value
            if (PuWriteInfoMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteInfoMsgs = PuWriteInfoMsgs.Value;

            // PuWriteWarningMsgs reg setting: set PopupMessageWriterWriteWarningMsgs option based on value
            if (PuWriteWarningMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteWarningMsgs = PuWriteWarningMsgs.Value;

            // PuWriteErrorMsgs reg setting: set PopupMessageWriterWriteErrorMsgs option based on value
            if (PuWriteErrorMsgs.IsSet)
                Properties.OptionsNotificationsPage.Default.PopupMessageWriterWriteErrorMsgs = PuWriteErrorMsgs.Value;
        }

        /// <summary>
        /// Validates if a path is a valid absolute path to a file from the root of a drive.
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <returns>True if the path is valid; otherwise, false.</returns>
        private static bool PathIsValid(string path)
        {
            // Check if the path is rooted in a drive
            if (string.IsNullOrEmpty(path) || !Path.IsPathRooted(path) || path.Length < 3)
                return false;

            // Convert the path to uppercase for case-insensitive comparison
            path = path.ToUpper();

            // Check if the drive letter is valid
            char driveLetter = path[0];
            if (!char.IsLetter(driveLetter) || path[1] != ':' || path[2] != '\\')
                return false;

            // Check if such driver exists
            if (!DriveInfo.GetDrives().Any(drive => drive.Name[0] == driveLetter))
                return false;

            // Check if the rest of the path is valid
            string invalidFileNameChars = new string(Path.GetInvalidPathChars()) + @":/?*""<>|";
            if (path.Substring(3).Any(ch => invalidFileNameChars.Contains(ch)))
                return false;
            if (path.EndsWith("."))
                return false;

            return true;
        }
    }
}