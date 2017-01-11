using System.Collections.Generic;
using System.ComponentModel;
using System.IO;


namespace mRemoteNG.Themes
{
	public static class ThemeManager
    {
        #region Private Variables

        private static ThemeInfo _activeTheme;
        private static bool _activeThemeHandlerSet;
        #endregion

        #region Public Methods
        public static ThemeInfo LoadTheme(string themeName, bool setActive = true)
		{
			var loadedTheme = DefaultTheme;
			if (!string.IsNullOrEmpty(themeName))
			{
				foreach (var theme in LoadThemes())
				{
				    if (theme.Name != themeName) continue;
				    loadedTheme = theme;
				    break;
				}
			}
				
			if (setActive)
			{
				ActiveTheme = loadedTheme;
			}
			return loadedTheme;
		}
			
		public static List<ThemeInfo> LoadThemes()
		{
		    var themes = new List<ThemeInfo> {DefaultTheme};
		    try
			{
				themes.AddRange(ThemeSerializer.LoadFromXmlFile(Path.Combine(App.Info.SettingsFileInfo.SettingsPath, App.Info.SettingsFileInfo.ThemesFileName)));
			}
			catch (FileNotFoundException)
			{
			}
				
			return themes;
		}

        private static void SaveThemes(List<ThemeInfo> themes)
		{
			themes.Remove(DefaultTheme);
			ThemeSerializer.SaveToXmlFile(themes, Path.Combine(App.Info.SettingsFileInfo.SettingsPath, App.Info.SettingsFileInfo.ThemesFileName));
		}

        private static void SaveThemes(ThemeInfo[] themes)
		{
			SaveThemes(new List<ThemeInfo>(themes));
		}
			
		public static void SaveThemes(BindingList<ThemeInfo> themes)
		{
			var themesArray = new ThemeInfo[themes.Count - 1 + 1];
			themes.CopyTo(themesArray, 0);
			SaveThemes(themesArray);
		}
        #endregion
			
        #region Events
		public delegate void ThemeChangedEventHandler();
		private static ThemeChangedEventHandler ThemeChangedEvent;
			
		public static event ThemeChangedEventHandler ThemeChanged
		{
			add { ThemeChangedEvent = (ThemeChangedEventHandler) System.Delegate.Combine(ThemeChangedEvent, value); }
			remove { ThemeChangedEvent = (ThemeChangedEventHandler) System.Delegate.Remove(ThemeChangedEvent, value); }
		}

        private static void NotifyThemeChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name")
			{
				return;
			}
		    ThemeChangedEvent?.Invoke();
		}
        #endregion
			
        #region Properties
        public static ThemeInfo DefaultTheme { get; } = new ThemeInfo(Language.strDefaultTheme);

        public static ThemeInfo ActiveTheme
		{
			get
			{
			    return _activeTheme ?? DefaultTheme;
			}
			set
			{
				// We need to set ActiveTheme to the new theme to make sure it references the right object.
				// However, if both themes have the same properties, we don't need to raise a notification event.
				var needNotify = true;
				if (_activeTheme != null)
				{
					if (_activeTheme.Equals(value))
					{
						needNotify = false;
					}
				}
					
				if (_activeThemeHandlerSet)
				{
				    if (_activeTheme != null) _activeTheme.PropertyChanged -= NotifyThemeChanged;
				}
					
				_activeTheme = value;
					
				_activeTheme.PropertyChanged += NotifyThemeChanged;
				_activeThemeHandlerSet = true;
					
				if (needNotify)
				{
					NotifyThemeChanged(_activeTheme, new PropertyChangedEventArgs(""));
				}
			}
		}
        #endregion
	}
}