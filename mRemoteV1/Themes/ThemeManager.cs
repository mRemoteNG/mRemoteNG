using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using mRemoteNG.My;


namespace mRemoteNG.Themes
{
	public class ThemeManager
	{
        #region Public Methods
		public static ThemeInfo LoadTheme(string themeName, bool setActive = true)
		{
			ThemeInfo loadedTheme = DefaultTheme;
				
			if (!string.IsNullOrEmpty(themeName))
			{
				foreach (ThemeInfo theme in LoadThemes())
				{
					if (theme.Name == themeName)
					{
						loadedTheme = theme;
						break;
					}
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
			List<ThemeInfo> themes = new List<ThemeInfo>();
			themes.Add(DefaultTheme);
			try
			{
				themes.AddRange(ThemeSerializer.LoadFromXmlFile(Path.Combine(App.Info.Settings.SettingsPath, App.Info.Settings.ThemesFileName)));
			}
			catch (FileNotFoundException)
			{
			}
				
			return themes;
		}
			
		public static void SaveThemes(List<ThemeInfo> themes)
		{
			themes.Remove(DefaultTheme);
			ThemeSerializer.SaveToXmlFile(themes, Path.Combine(App.Info.Settings.SettingsPath, App.Info.Settings.ThemesFileName));
		}
			
		public static void SaveThemes(ThemeInfo[] themes)
		{
			SaveThemes(new List<ThemeInfo>(themes));
		}
			
		public static void SaveThemes(BindingList<ThemeInfo> themes)
		{
			ThemeInfo[] themesArray = new ThemeInfo[themes.Count - 1 + 1];
			themes.CopyTo(themesArray, 0);
			SaveThemes(themesArray);
		}
        #endregion
			
        #region Events
		public delegate void ThemeChangedEventHandler();
		private static ThemeChangedEventHandler ThemeChangedEvent;
			
		public static event ThemeChangedEventHandler ThemeChanged
		{
			add
			{
				ThemeChangedEvent = (ThemeChangedEventHandler) System.Delegate.Combine(ThemeChangedEvent, value);
			}
			remove
			{
				ThemeChangedEvent = (ThemeChangedEventHandler) System.Delegate.Remove(ThemeChangedEvent, value);
			}
		}
			
		protected static void NotifyThemeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name")
			{
				return ;
			}
			if (ThemeChangedEvent != null)
				ThemeChangedEvent();
		}
        #endregion
			
        #region Properties
		// ReSharper disable InconsistentNaming
		private static readonly ThemeInfo _defaultTheme = new ThemeInfo(Language.strDefaultTheme);
		// ReSharper restore InconsistentNaming
        public static ThemeInfo DefaultTheme
		{
			get
			{
				return _defaultTheme;
			}
		}
			
		private static ThemeInfo _activeTheme;
		private static bool _activeThemeHandlerSet = false;
        public static ThemeInfo ActiveTheme
		{
			get
			{
				if (_activeTheme == null)
				{
					return DefaultTheme;
				}
				return _activeTheme;
			}
			set
			{
				// We need to set ActiveTheme to the new theme to make sure it references the right object.
				// However, if both themes have the same properties, we don't need to raise a notification event.
				bool needNotify = true;
				if (_activeTheme != null)
				{
					if (_activeTheme.Equals(value))
					{
						needNotify = false;
					}
				}
					
				if (_activeThemeHandlerSet)
				{
					_activeTheme.PropertyChanged -= NotifyThemeChanged;
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
