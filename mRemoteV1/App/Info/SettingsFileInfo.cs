
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace mRemoteNG.App.Info
{
    public class SettingsFileInfo
    {
#if !PORTABLE
		public static readonly string SettingsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\" + Application.ProductName;
#else
        public static readonly string SettingsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\" + Application.ProductName;
#endif
        public static readonly string LayoutFileName = "pnlLayout.xml";
        public static readonly string ExtAppsFilesName = "extApps.xml";
        public const string ThemesFileName = "Themes.xml";
    }
}