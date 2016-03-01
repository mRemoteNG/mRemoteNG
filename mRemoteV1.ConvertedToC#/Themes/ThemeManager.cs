using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using mRemoteNG.My;

namespace mRemoteNG.Themes
{
	public class ThemeManager
	{
		#region "Public Methods"
		public static ThemeInfo LoadTheme(string themeName, bool setActive = true)
		{
			ThemeInfo loadedTheme = DefaultTheme;

			if (!string.IsNullOrEmpty(themeName)) {
				foreach (ThemeInfo theme in LoadThemes()) {
					if (theme.Name == themeName) {
						loadedTheme = theme;
						break; // TODO: might not be correct. Was : Exit For
					}
				}
			}

			if (setActive)
				ActiveTheme = loadedTheme;
			return loadedTheme;
		}

		public static List<ThemeInfo> LoadThemes()
		{
			List<ThemeInfo> themes = new List<ThemeInfo>();
			themes.Add(DefaultTheme);
			try {
				themes.AddRange(ThemeSerializer.LoadFromXmlFile(Path.Combine(mRemoteNG.App.Info.Settings.SettingsPath, mRemoteNG.App.Info.Settings.ThemesFileName)));
			} catch (FileNotFoundException ex) {
			}

			return themes;
		}

		public static void SaveThemes(List<ThemeInfo> themes)
		{
			themes.Remove(DefaultTheme);
			ThemeSerializer.SaveToXmlFile(themes, Path.Combine(mRemoteNG.App.Info.Settings.SettingsPath, mRemoteNG.App.Info.Settings.ThemesFileName));
		}

		public static void SaveThemes(ThemeInfo[] themes)
		{
			SaveThemes(new List<ThemeInfo>(themes));
		}

		public static void SaveThemes(BindingList<ThemeInfo> themes)
		{
			ThemeInfo[] themesArray = new ThemeInfo[themes.Count];
			themes.CopyTo(themesArray, 0);
			SaveThemes(themesArray);
		}
		#endregion

		#region "Events"
		public static event ThemeChangedEventHandler ThemeChanged;
		public static delegate void ThemeChangedEventHandler();
		protected static void NotifyThemeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name")
				return;
			if (ThemeChanged != null) {
				ThemeChanged();
			}
		}
		#endregion

		#region "Properties"
		// ReSharper disable InconsistentNaming
		private static readonly ThemeInfo _defaultTheme = new ThemeInfo(Language.strDefaultTheme);
		// ReSharper restore InconsistentNaming
		public static ThemeInfo DefaultTheme {
			get { return _defaultTheme; }
		}

		private static ThemeInfo _activeTheme;
		private static bool _activeThemeHandlerSet = false;
		public static ThemeInfo ActiveTheme {
			get {
				if (_activeTheme == null)
					return DefaultTheme;
				return _activeTheme;
			}
			set {
				// We need to set ActiveTheme to the new theme to make sure it references the right object.
				// However, if both themes have the same properties, we don't need to raise a notification event.
				bool needNotify = true;
				if (_activeTheme != null) {
					if (_activeTheme.Equals(value))
						needNotify = false;
				}

				if (_activeThemeHandlerSet)
					_activeTheme.PropertyChanged -= NotifyThemeChanged;

				_activeTheme = value;

				_activeTheme.PropertyChanged += NotifyThemeChanged;
				_activeThemeHandlerSet = true;

				if (needNotify)
					NotifyThemeChanged(_activeTheme, new PropertyChangedEventArgs(""));
			}
		}
		#endregion
	}
}
