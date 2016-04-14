
namespace mRemoteNG.App.Info
{
    public class SettingsFileInfo
    {
        #if !PORTABLE
		public static readonly string SettingsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\" + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.ProductName;
        #else
        public static readonly string SettingsPath = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath;
        #endif
        public static readonly string LayoutFileName = "pnlLayout.xml";
        public static readonly string ExtAppsFilesName = "extApps.xml";
        public const string ThemesFileName = "Themes.xml";
    }
}