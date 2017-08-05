using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Themes
{
    /// <summary>
    /// Main class of the theming component. Centralices creation, loading and deletion of themes
    /// Implmeented as a singleton
    /// </summary>
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
            SetActive();
            _themeActive = Settings.Default.ThemingActive;
        }

        private void SetActive()
        {
            if (themes[mRemoteNG.Settings.Default.ThemeName] != null)
                ActiveTheme = (ThemeInfo) themes[mRemoteNG.Settings.Default.ThemeName];
            else
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


        public ThemeInfo getTheme(string themeName)
        {
            if(themes[themeName] != null)
                return (ThemeInfo)themes[themeName];
            return null;
        }

        //THe manager precharges all the themes at once
        public  List<ThemeInfo> LoadThemes()
        {
            if (themes == null)
            {
                themes = new Hashtable();

                //Load the files in theme folder first, to incluide vstheme light as default

                string execPath = App.Info.SettingsFileInfo.SettingsPath;
                if(execPath != null)
                {
                    string[] themeFiles = Directory.GetFiles(Path.Combine(execPath, "themes"), "*.vstheme");
                    string defaultThemeURL = Directory.GetFiles(Path.Combine(execPath, "themes"), "vs2015light" + ".vstheme")[0];
                    //First we load the default theme, its vs2015light 
                    ThemeInfo defaultTheme = ThemeSerializer.LoadFromXmlFile(defaultThemeURL);
                    themes.Add(defaultTheme.Name, defaultTheme);
                    //Then the rest
                    foreach (string themeFile in themeFiles)
                    {
                        //filter default one
                        ThemeInfo extTheme = ThemeSerializer.LoadFromXmlFile(themeFile, defaultTheme);
                        if (extTheme.Theme != null && !themes.ContainsKey(extTheme.Name))
                        {
                            themes.Add(extTheme.Name, extTheme);
                        }
                    }


                    //Load the embedded themes, extended palettes are taken from the vs2015 themes, trying to match the color theme
                    ThemeInfo vs2003 = new ThemeInfo("Vs2003", new VS2003Theme(), "", VisualStudioToolStripExtender.VsVersion.Vs2003, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                    themes.Add(vs2003.Name, vs2003);
                    ThemeInfo vs2005 = new ThemeInfo("Vs2005", new VS2005Theme(), "", VisualStudioToolStripExtender.VsVersion.Vs2005, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                    themes.Add(vs2005.Name, vs2005);
                    ThemeInfo vs2012Light = new ThemeInfo("vs2012Light", new VS2012LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                    themes.Add(vs2012Light.Name, vs2012Light);
                    ThemeInfo vs2012Dark = new ThemeInfo("vs2012Dark", new VS2012DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)themes["vs2015dark"]).ExtendedPalette);
                    themes.Add(vs2012Dark.Name, vs2012Dark);
                    ThemeInfo vs2012Blue = new ThemeInfo("vs2012Blue", new VS2012BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)themes["vs2015blue"]).ExtendedPalette);
                    themes.Add(vs2012Blue.Name, vs2012Blue);
                    ThemeInfo vs2013Light = new ThemeInfo("vs2013Light", new VS2013LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                    themes.Add(vs2013Light.Name, vs2013Light);
                    ThemeInfo vs2013Dark = new ThemeInfo("vs2013Dark", new VS2013DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)themes["vs2015dark"]).ExtendedPalette);
                    themes.Add(vs2013Dark.Name, vs2013Dark);
                    ThemeInfo vs2013Blue = new ThemeInfo("vs2013Blue", new VS2013BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)themes["vs2015blue"]).ExtendedPalette);
                    themes.Add(vs2013Blue.Name, vs2013Blue);

                }

            } 
            return themes.Values.OfType<ThemeInfo>().ToList(); 
        }

        /// <summary>
        /// Add a new theme based on an existing one by cloning and renaming, the theme is saved to disk
        /// </summary>
        /// <param name="baseTheme"></param>
        /// <param name="newThemeName"></param>
        /// <returns></returns>
        public ThemeInfo addTheme(ThemeInfo baseTheme, string newThemeName)
        {
            if (!themes.Contains(newThemeName))
            {
                ThemeInfo modifiedTheme = (ThemeInfo)baseTheme.Clone();
                modifiedTheme.Name = newThemeName;
                modifiedTheme.IsExtendable = true;
                modifiedTheme.IsThemeBase = false;
                ThemeSerializer.SaveToXmlFile(modifiedTheme,baseTheme);
                themes.Add(newThemeName,modifiedTheme);
                return modifiedTheme;
            }
            return null;
        }

        //Delete a theme from memory and disk
        public void deleteTheme(ThemeInfo themeToDelete)
        {
            if (themes.Contains(themeToDelete.Name))
            {
                if(ActiveTheme == themeToDelete)
                    ActiveTheme = DefaultTheme;
                themes.Remove(themeToDelete.Name);
                ThemeSerializer.DeleteFile(themeToDelete);
            } 
        }

        //Sincronize the theme XML values from memory to disk
        public void updateTheme(ThemeInfo themeToUpdate)
        {
            ThemeSerializer.UpdateThemeXMLValues(themeToUpdate); 
        }

        //refresh the ui controls to reflect a theme change
        public void refreshUI()
        {
            NotifyThemeChanged(this, new PropertyChangedEventArgs(""));
        }

        //Verify if theme name is repeated or if the name is a valid file  name
        public bool isThemeNameOk(string name)
        {
            if (themes.Contains(name))
                return false;
            char[] badChars = Path.GetInvalidFileNameChars();
            if (name.IndexOfAny(badChars) != -1)
                return false; 
            return true;
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
                Settings.Default.ThemingActive = value;
                NotifyThemeChanged(this, new PropertyChangedEventArgs(""));
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
                return _activeTheme;
			}
			set
			{
                if(value != null)
                {
                    _activeTheme = value;
                    Settings.Default.ThemeName = value.Name;
                    NotifyThemeChanged(this, new PropertyChangedEventArgs("theme"));
                }
			}
		}
        #endregion
	}
} 