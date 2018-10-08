using mRemoteNG.App;
using mRemoteNG.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        private bool _themeActive;
        private static ThemeManager themeInstance;
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
            if (themes[Settings.Default.ThemeName] != null)
                ActiveTheme = (ThemeInfo) themes[Settings.Default.ThemeName];
            else
                ActiveTheme = DefaultTheme;
        }

        #endregion

        #region Public Methods
        public static ThemeManager getInstance()
        {
            return themeInstance ?? (themeInstance = new ThemeManager());
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
            if (themes != null) return themes.Values.OfType<ThemeInfo>().ToList();
            themes = new Hashtable();

            //Load the files in theme folder first, to incluide vstheme light as default 
            var themePath = App.Info.SettingsFileInfo.ThemeFolder;
            if (themePath == null) return themes.Values.OfType<ThemeInfo>().ToList();
            try
            {
                //In install mode first time is necesary to copy the themes folder
                if (!Directory.Exists(themePath))
                {
                    Directory.CreateDirectory(themePath);

                }
                var orig = new DirectoryInfo(App.Info.SettingsFileInfo.InstalledThemeFolder);
                var files = orig.GetFiles();
                foreach (var file in files)
                {

                    if (!File.Exists(Path.Combine(themePath, file.Name)))
                        file.CopyTo(Path.Combine(themePath, file.Name), true);
                }



                //Check that theme folder exist before trying to load themes
                if (Directory.Exists(themePath))
                {
                    var themeFiles = Directory.GetFiles(themePath, "*.vstheme");
                    var defaultThemeURL = Directory.GetFiles(themePath, "vs2015light" + ".vstheme")[0];
                    //First we load the default theme, its vs2015light 
                    var defaultTheme = ThemeSerializer.LoadFromXmlFile(defaultThemeURL);
                    themes.Add(defaultTheme.Name, defaultTheme);
                    //Then the rest
                    foreach (var themeFile in themeFiles)
                    {
                        //filter default one
                        var extTheme = ThemeSerializer.LoadFromXmlFile(themeFile, defaultTheme);
                        if (extTheme.Theme != null && !themes.ContainsKey(extTheme.Name))
                        {
                            themes.Add(extTheme.Name, extTheme);
                        }
                    }


                    //Load the embedded themes, extended palettes are taken from the vs2015 themes, trying to match the color theme
                    var vs2003 = new ThemeInfo("Vs2003", new VS2003Theme(), "", VisualStudioToolStripExtender.VsVersion.Vs2003, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                    themes.Add(vs2003.Name, vs2003);
                    var vs2005 = new ThemeInfo("Vs2005", new VS2005Theme(), "", VisualStudioToolStripExtender.VsVersion.Vs2005, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                    themes.Add(vs2005.Name, vs2005);
                    var vs2012Light = new ThemeInfo("vs2012Light", new VS2012LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                    themes.Add(vs2012Light.Name, vs2012Light);
                    var vs2012Dark = new ThemeInfo("vs2012Dark", new VS2012DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)themes["vs2015dark"]).ExtendedPalette);
                    themes.Add(vs2012Dark.Name, vs2012Dark);
                    var vs2012Blue = new ThemeInfo("vs2012Blue", new VS2012BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)themes["vs2015blue"]).ExtendedPalette);
                    themes.Add(vs2012Blue.Name, vs2012Blue);
                    var vs2013Light = new ThemeInfo("vs2013Light", new VS2013LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)themes["vs2015light"]).ExtendedPalette);
                    themes.Add(vs2013Light.Name, vs2013Light);
                    var vs2013Dark = new ThemeInfo("vs2013Dark", new VS2013DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)themes["vs2015dark"]).ExtendedPalette);
                    themes.Add(vs2013Dark.Name, vs2013Dark);
                    var vs2013Blue = new ThemeInfo("vs2013Blue", new VS2013BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)themes["vs2015blue"]).ExtendedPalette);
                    themes.Add(vs2013Blue.Name, vs2013Blue);
                }
            }
            catch(Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Error loading themes" + Environment.NewLine + ex.Message, true);
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
            if (themes.Contains(newThemeName)) return null;
            var modifiedTheme = (ThemeInfo)baseTheme.Clone();
            modifiedTheme.Name = newThemeName;
            modifiedTheme.IsExtendable = true;
            modifiedTheme.IsThemeBase = false;
            ThemeSerializer.SaveToXmlFile(modifiedTheme,baseTheme);
            themes.Add(newThemeName,modifiedTheme);
            return modifiedTheme;
        }

        //Delete a theme from memory and disk
        public void deleteTheme(ThemeInfo themeToDelete)
        {
            if (!themes.Contains(themeToDelete.Name)) return;
            if(ActiveTheme == themeToDelete)
                ActiveTheme = DefaultTheme;
            themes.Remove(themeToDelete.Name);
            ThemeSerializer.DeleteFile(themeToDelete);
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
            var badChars = Path.GetInvalidFileNameChars();
            return name.IndexOfAny(badChars) == -1;
        }
  
         
        #endregion

        #region Events
        public delegate void ThemeChangedEventHandler();
        private ThemeChangedEventHandler ThemeChangedEvent;

        public  event ThemeChangedEventHandler ThemeChanged
        {
            add => ThemeChangedEvent = (ThemeChangedEventHandler)Delegate.Combine(ThemeChangedEvent, value);
            remove => ThemeChangedEvent = (ThemeChangedEventHandler)Delegate.Remove(ThemeChangedEvent, value);
        }

        // ReSharper disable once UnusedParameter.Local
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
            get => _themeActive;
            set
            {
                if (themes.Count == 0) return;
                _themeActive = value;
                Settings.Default.ThemingActive = value;
                NotifyThemeChanged(this, new PropertyChangedEventArgs(""));
            }
        }

        public ThemeInfo DefaultTheme => (ThemeInfo) themes["vs2015light"];

        public ThemeInfo ActiveTheme
		{
            // default if themes are not enabled
            get => ThemingActive == false ? DefaultTheme : _activeTheme;
            set
			{
                //You can only enable theming if there are themes laoded
			    if (value == null) return;
			    _activeTheme = value;
			    Settings.Default.ThemeName = value.Name;
			    NotifyThemeChanged(this, new PropertyChangedEventArgs("theme"));
			}
		}
        #endregion
	}
} 