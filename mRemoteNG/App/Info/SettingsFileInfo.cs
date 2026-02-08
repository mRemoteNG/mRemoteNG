using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Forms;
using mRemoteNG.Connection;

namespace mRemoteNG.App.Info
{
    [SupportedOSPlatform("windows")]
    public static class SettingsFileInfo
    {
        private static readonly string ExePath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConnectionInfo))?.Location) ?? string.Empty;

        public static string SettingsPath =>
            Runtime.IsPortableEdition
                ? ExePath
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);

        public static string UserSettingsFilePath =>
            Runtime.IsPortableEdition
                ? Path.Combine(ExePath, $"{Path.GetFileNameWithoutExtension(Application.ExecutablePath)}.settings")
                : GetInstalledUserSettingsFilePath();

        public static string UserSettingsFolderPath =>
            string.IsNullOrWhiteSpace(UserSettingsFilePath)
                ? string.Empty
                : Path.GetDirectoryName(UserSettingsFilePath) ?? string.Empty;

        public static string LayoutFileName { get; } = "pnlLayout.xml";
        public static string ExtAppsFilesName { get; } = "extApps.xml";
        public static string ThemesFileName { get; } = "Themes.xml";
        public static string LocalConnectionProperties { get; } = "LocalConnectionProperties.xml";

        public static string ThemeFolder { get; } =
            SettingsPath != null ? Path.Combine(SettingsPath, "Themes") : String.Empty;

        public static string InstalledThemeFolder { get; } =
            ExePath != null ? Path.Combine(ExePath, "Themes") : String.Empty;

        private static string GetInstalledUserSettingsFilePath()
        {
            try
            {
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            }
            catch (ConfigurationErrorsException)
            {
                return string.Empty;
            }
        }
    }
}
