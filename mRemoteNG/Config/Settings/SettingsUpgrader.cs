using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Versioning;
using mRemoteNG.App;
using mRemoteNG.Messages;

namespace mRemoteNG.Config.Settings
{
    /// <summary>
    /// Carries the options over to a new version on its first start. Each version keeps its
    /// user.config in a folder of its own, so a new version starts from the defaults until
    /// the values of the previous one are copied across.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class SettingsUpgrader
    {
        /// <summary>
        /// Every settings class with options of the user. Each keeps its own section in
        /// user.config and has to be upgraded on its own.
        /// </summary>
        internal static IReadOnlyList<ApplicationSettingsBase> AllSettings =>
        [
            Properties.App.Default,
            Properties.AppUI.Default,
            Properties.OptionsAdvancedPage.Default,
            Properties.OptionsAppearancePage.Default,
            Properties.OptionsBackupPage.Default,
            Properties.OptionsConnectionsPage.Default,
            Properties.OptionsCredentialsPage.Default,
            Properties.OptionsDBsPage.Default,
            Properties.OptionsNotificationsPage.Default,
            Properties.OptionsRbac.Default,
            Properties.OptionsSecurityPage.Default,
            Properties.OptionsStartupExitPage.Default,
            Properties.OptionsTabsPanelsPage.Default,
            Properties.OptionsThemePage.Default,
            Properties.OptionsUpdatesPage.Default,
            Properties.Settings.Default,
        ];

        /// <summary>
        /// Runs before anything reads an option, so the first start of a new version already
        /// uses the values carried over.
        /// </summary>
        public static void UpgradeIfNeeded(MessageCollector messageCollector)
        {
            if (!Properties.App.Default.DoUpgrade) return;

            bool earlierVersionFound = HasSettingsFromEarlierVersion();

            foreach (ApplicationSettingsBase settings in AllSettings)
            {
                try
                {
                    settings.Upgrade();
                }
                catch (Exception ex)
                {
                    messageCollector.AddExceptionMessage($"{settings.GetType().Name}.Upgrade() failed", ex);
                }
            }

            // Nothing from an earlier 1.77/1.78 version: the options may still be where
            // mRemoteNG 1.76 left them, which no later version looks at.
            if (!earlierVersionFound && !Runtime.IsPortableEdition)
                ImportFromMRemoteNG176(messageCollector);

            // App's own Upgrade() just copied the previous version's "false" in; set it last.
            Properties.App.Default.DoUpgrade = false;

            // Clear pending update flag
            // This is used for automatic updates, not for settings migration, but it
            // needs to be cleared here because we know that we just updated.
            Properties.OptionsUpdatesPage.Default.UpdatePending = false;

            // Saved now rather than on exit: a session that does not end normally would
            // otherwise upgrade again on the next start, over whatever was changed since.
            foreach (ApplicationSettingsBase settings in AllSettings)
            {
                try
                {
                    settings.Save();
                }
                catch (Exception ex)
                {
                    messageCollector.AddExceptionMessage($"Saving {settings.GetType().Name} after the upgrade failed", ex);
                }
            }
        }

        private static bool HasSettingsFromEarlierVersion()
        {
            try
            {
                string current = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
                return HasEarlierVersion(current);
            }
            catch (ConfigurationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Whether a version below the one <paramref name="userConfigPath"/> belongs to left a
        /// user.config beside it: <c>...\mRemoteNG_Url_&lt;hash&gt;\&lt;version&gt;\user.config</c>.
        /// </summary>
        internal static bool HasEarlierVersion(string userConfigPath)
        {
            DirectoryInfo? versionFolder = Directory.GetParent(userConfigPath);
            DirectoryInfo? appFolder = versionFolder?.Parent;
            if (versionFolder == null || appFolder is not { Exists: true }
                || !Version.TryParse(versionFolder.Name, out Version? current))
                return false;

            foreach (DirectoryInfo folder in appFolder.EnumerateDirectories())
            {
                if (Version.TryParse(folder.Name, out Version? version) && version < current
                    && File.Exists(Path.Combine(folder.FullName, "user.config")))
                    return true;
            }

            return false;
        }

        private static void ImportFromMRemoteNG176(MessageCollector messageCollector)
        {
            try
            {
                string? legacyFile = Settings176Importer.FindNewestUserConfig(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                if (legacyFile == null) return;

                int imported = Settings176Importer.Import(legacyFile, AllSettings,
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                messageCollector.AddMessage(MessageClass.InformationMsg,
                    $"Imported {imported} options from mRemoteNG 1.76 ({legacyFile})", true);
            }
            catch (Exception ex)
            {
                messageCollector.AddExceptionMessage("Importing the options of mRemoteNG 1.76 failed", ex);
            }
        }
    }
}
