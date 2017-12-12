using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace mRemoteNG.App.Info
{
    public static class SettingsFileInfo
    {
        private static readonly string ExePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        public static string SettingsPath { get; } = Runtime.IsPortableEdition ? ExePath : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Application.ProductName;
        public static string LayoutFileName { get; } = "pnlLayout.xml";
        public static string ExtAppsFilesName { get; } = "extApps.xml";
        public static string ThemesFileName { get; } = "Themes.xml";
        public static string ThemeFolder { get; } = SettingsPath != null ?  Path.Combine(SettingsPath, "Themes") : String.Empty;
        public static string InstalledThemeFolder { get; } = ExePath != null ?  Path.Combine(ExePath, "Themes") : String.Empty;
    }
}