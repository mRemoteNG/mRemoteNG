using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Themes
{
    //Main class to manage the themes 
    //Singleton 
    public class ThemeManager
    {
        #region Private Variables

        private  ThemeInfo _activeTheme; 
        private  Hashtable themes; 
        private static ThemeManager themeInstance = null;
        #endregion


        #region Constructors
        private ThemeManager()
        {
            LoadThemes();
            ActiveTheme = DefaultTheme;
        }

        #endregion

        #region Public Methods
        public static ThemeManager getInstance()
        {
            if(themeInstance == null)
            {
                themeInstance = new ThemeManager();
            }
            return themeInstance;
        }


        public ThemeInfo LoadTheme(string themeLocation)
        {
            return null;
        }

        //THe manager precharges all the themes at once
        public  List<ThemeInfo> LoadThemes()
        {
            if (themes == null)
            {
                themes = new Hashtable();

                //Load the embedded themes
                ThemeInfo vs2003 = new ThemeInfo("Vs2003", new VS2003Theme(), "", VisualStudioToolStripExtender.VsVersion.Vs2003);
                themes.Add("Vs2003", vs2003);
                ThemeInfo vs2005 = new ThemeInfo("Vs2005", new VS2005Theme(), "", VisualStudioToolStripExtender.VsVersion.Vs2005);
                themes.Add("Vs2005", vs2005);
                ThemeInfo vs2012Light = new ThemeInfo("vs2012Light", new VS2012LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012);
                themes.Add("vs2012Light", vs2012Light);
                ThemeInfo vs2012Dark = new ThemeInfo("vs2012Dark", new VS2012DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012);
                themes.Add("vs2012Dark", vs2012Dark);
                ThemeInfo vs2012Blue = new ThemeInfo("vs2012Blue", new VS2012BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012);
                themes.Add("vs2012Blue", vs2012Blue);
                ThemeInfo vs2013Light = new ThemeInfo("vs2013Light", new VS2013LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013);
                themes.Add("vs2013Light", vs2013Light);
                ThemeInfo vs2013Dark = new ThemeInfo("vs2013Dark", new VS2013DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013);
                themes.Add("vs2013Dark", vs2013Dark);
                ThemeInfo vs2013Blue = new ThemeInfo("vs2013Blue", new VS2013BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013);
                themes.Add("vs2013Blue", vs2013Blue);
                ThemeInfo vs2015Light = new ThemeInfo("vs2015Light", new VS2015LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015);
                themes.Add("vs2015Light", vs2015Light);
                ThemeInfo vs2015Dark = new ThemeInfo("vs2015Dark", new VS2015DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015);
                themes.Add("vs2015Dark", vs2015Dark);
                ThemeInfo vs2015Blue = new ThemeInfo("vs2005Blue", new VS2015BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015);
                themes.Add("vs2005Blue", vs2015Blue);

                //Load the files in theme folder 
                string[] themeFiles =  Directory.GetFiles(Path.Combine(App.Info.SettingsFileInfo.SettingsPath, "themes"),"*.vstheme");

                foreach (string themeFile in themeFiles)
                {
                    ThemeInfo extTheme = ThemeSerializer.LoadFromXmlFile(themeFile);
                    if (extTheme.Theme != null)
                    {
                        themes.Add(extTheme.Name, extTheme);
                    }
                }


            }




            return themes.Keys.OfType<ThemeInfo>().ToList();

        }

        private void SaveThemes(List<ThemeInfo> themes)
        {

        }

        private void SaveThemes(ThemeInfo[] themes)
        {
        }

        public void SaveThemes(BindingList<ThemeInfo> themes)
        {
        }
        #endregion

        #region Events
        public delegate void ThemeChangedEventHandler();
        private ThemeChangedEventHandler ThemeChangedEvent;

        public  event ThemeChangedEventHandler ThemeChanged
        {
            add { ThemeChangedEvent = (ThemeChangedEventHandler)System.Delegate.Combine(ThemeChangedEvent, value); }
            remove { ThemeChangedEvent = (ThemeChangedEventHandler)System.Delegate.Remove(ThemeChangedEvent, value); }
        }

        private  void NotifyThemeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                return;
            }
            ThemeChangedEvent?.Invoke();
        }
        #endregion

        #region Properties
        public ThemeInfo DefaultTheme 
        {
			get
			{
                return (ThemeInfo) themes["vs2015light"];
			} 
		}

        public ThemeInfo ActiveTheme
		{
			get
			{
                return _activeTheme;
			}
			set
			{
                _activeTheme = value;
			}
		}
        #endregion
	}
}