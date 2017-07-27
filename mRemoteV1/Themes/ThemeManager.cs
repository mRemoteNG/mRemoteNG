using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        private bool _themeActive = false;
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

                //Load the files in theme folder first, to incluide vstheme light as default
                string[] themeFiles = Directory.GetFiles(Path.Combine(App.Info.SettingsFileInfo.SettingsPath, "themes"), "*.vstheme");
                string defaultThemeURL = Directory.GetFiles(Path.Combine(App.Info.SettingsFileInfo.SettingsPath, "themes"), "vs2015light"+".vstheme")[0];
                //First we load the default theme, its vs2015light 
                ThemeInfo defaultTheme = ThemeSerializer.LoadFromXmlFile(defaultThemeURL);
                themes.Add(defaultTheme.Name, defaultTheme);
                //Then the rest
                foreach (string themeFile in themeFiles)
                {
                    //filter default one
                    ThemeInfo extTheme = ThemeSerializer.LoadFromXmlFile(themeFile, defaultTheme);
                    if (extTheme.Theme != null && !themes.ContainsKey(extTheme.Name) )
                    {
                        themes.Add(extTheme.Name, extTheme);
                    }
                }


                //Load the embedded themes, extended palettes are taken from the vs2015 themes, trying to match the color theme
                ThemeInfo vs2003 = new ThemeInfo("Vs2003", new VS2003Theme(), "", VisualStudioToolStripExtender.VsVersion.Vs2003, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                themes.Add("Vs2003", vs2003);
                ThemeInfo vs2005 = new ThemeInfo("Vs2005", new VS2005Theme(), "", VisualStudioToolStripExtender.VsVersion.Vs2005, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                themes.Add("Vs2005", vs2005);
                ThemeInfo vs2012Light = new ThemeInfo("vs2012Light", new VS2012LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012,((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                themes.Add("vs2012Light", vs2012Light);
                ThemeInfo vs2012Dark = new ThemeInfo("vs2012Dark", new VS2012DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)themes["vs2015dark"]).ExtendedPalette);
                themes.Add("vs2012Dark", vs2012Dark);
                ThemeInfo vs2012Blue = new ThemeInfo("vs2012Blue", new VS2012BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)themes["vs2015blue"]).ExtendedPalette);
                themes.Add("vs2012Blue", vs2012Blue);
                ThemeInfo vs2013Light = new ThemeInfo("vs2013Light", new VS2013LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                themes.Add("vs2013Light", vs2013Light);
                ThemeInfo vs2013Dark = new ThemeInfo("vs2013Dark", new VS2013DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)themes["vs2015dark"]).ExtendedPalette);
                themes.Add("vs2013Dark", vs2013Dark);
                ThemeInfo vs2013Blue = new ThemeInfo("vs2013Blue", new VS2013BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)themes["vs2015blue"]).ExtendedPalette);
                themes.Add("vs2013Blue", vs2013Blue);
 
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
        public  bool ThemingActive
        {
            get
            {
                return _themeActive;
            }
            set
            {
               _themeActive = value;
            }
        }

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
                return (ThemeInfo)themes["vs2015light"];//_activeTheme;
			}
			set
			{
                if(value != null)
                    _activeTheme = value;
			}
		}
        #endregion
	}
} 