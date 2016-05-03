
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace mRemoteNG.App.Info
{
    public class SettingsFileInfo
    {
        public static readonly string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
#if !PORTABLE
		public static readonly string SettingsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\" + Application.ProductName;
#else
        public static readonly string SettingsPath = exePath;
#endif
        
        public static readonly string LayoutFileName = "pnlLayout.xml";
        public static readonly string ExtAppsFilesName = "extApps.xml";
        public const string ThemesFileName = "Themes.xml";
    }
}