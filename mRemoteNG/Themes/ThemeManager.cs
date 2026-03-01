using mRemoteNG.App;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;

namespace mRemoteNG.Themes
{
    /// <summary>
    /// Singleton manager for the theming subsystem. Centralizes loading, creation,
    /// deletion, and live switching of visual themes. Themes are stored as
    /// <c>.vstheme</c> XML files and can be either built-in (VS2015 Light/Dark/Blue)
    /// or user-created extensions. Fires <see cref="ThemeChanged"/> events to notify
    /// all subscribed UI components to refresh their appearance.
    /// As of v1.80.0, theme changes are applied live without requiring a restart.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class ThemeManager
    {
        #region Private Variables

        private ThemeInfo _activeTheme = null!;
        private ThemeInfo? _highContrastTheme;
        private bool _highContrastActive;
        private Hashtable themes = null!;
        private bool _themeActive;
        private static ThemeManager? themeInstance;
        private readonly string themePath = App.Info.SettingsFileInfo.ThemeFolder;

        #endregion

        #region Constructors

        private ThemeManager()
        {
            _highContrastActive = SystemInformation.HighContrast;
            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;

            LoadThemes();
            SetActive();
            _themeActive = true;
        }

        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.Accessibility || e.Category == UserPreferenceCategory.Color)
            {
                bool newState = SystemInformation.HighContrast;
                if (_highContrastActive != newState)
                {
                    _highContrastActive = newState;
                    NotifyThemeChanged(this, new PropertyChangedEventArgs("HighContrast"));
                }
            }
        }

        private void SetActive()
        {
            var themeName = Properties.OptionsThemePage.Default.ThemeName;
            if (themeName != null && themes[themeName] is ThemeInfo savedTheme)
                ActiveTheme = savedTheme;
            else
            {
                ActiveTheme = DefaultTheme;
                if (string.IsNullOrEmpty(Properties.OptionsThemePage.Default.ThemeName)) return;

                //too early for logging to be enabled...
                Debug.WriteLine("Detected invalid Theme in settings file. Resetting to default.");
                // if we got here, then there's an invalid theme name in use, so just empty it out...
                Properties.OptionsThemePage.Default.ThemeName = "";
                Properties.OptionsThemePage.Default.Save();
            }
        }

        #endregion

        #region Public Methods

        public static ThemeManager getInstance()
        {
            return themeInstance ?? (themeInstance = new ThemeManager());
        }


        public ThemeInfo? getTheme(string themeName)
        {
            return themes[themeName] as ThemeInfo;
        }

        private bool ThemeDirExists()
        {
            //Load the files in theme folder first, to include vstheme light as default
            if (themePath == null) return false;
            try
            {
                //In install mode first time is necessary to copy the themes folder
                if (!Directory.Exists(themePath))
                {
                    Directory.CreateDirectory(themePath);
                }

                DirectoryInfo orig = new(App.Info.SettingsFileInfo.InstalledThemeFolder);
                FileInfo[] files = orig.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (!File.Exists(Path.Combine(themePath, file.Name)))
                        file.CopyTo(Path.Combine(themePath, file.Name), true);
                }

                return Directory.Exists(themePath);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error loading theme directory", ex);
            }

            return false;
        }

        private ThemeInfo? LoadDefaultTheme()
        {
            try
            {
                if (ThemeDirExists())
                {
                    string defaultThemeURL = $"{themePath}\\vs2015light.vstheme";

                    if (!File.Exists($"{themePath}\\vs2015light.vstheme"))
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Could not find default theme file.",
                                                            true);
                        return null;
                    }

                    //First we load the default base theme, its vs2015lightNG
                    //the true "default" in DockPanelSuite built-in VS2015LightTheme named "vs2015Light"
                    //hence the *NG suffix for this one...
                    ThemeInfo defaultTheme = ThemeSerializer.LoadFromXmlFile(defaultThemeURL);
                    defaultTheme.Name = $"{defaultTheme.Name}NG";
                    return defaultTheme;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error loading default theme", ex);
            }

            return null;
        }

        //The manager precharges all the themes at once
        public IList<ThemeInfo> LoadThemes()
        {
            if (themes != null) return themes.Values.OfType<ThemeInfo>().ToList();
            themes = [];

            if (themePath == null) return themes.Values.OfType<ThemeInfo>().ToList();
            try
            {
                //Check that theme folder exist before trying to load themes
                if (ThemeDirExists())
                {
                    string[] themeFiles = Directory.GetFiles(themePath, "*.vstheme");

                    //First we load the default base theme, its vs2015lightNG
                    ThemeInfo? defaultTheme = LoadDefaultTheme();
                    if (defaultTheme?.Name == null) return themes.Values.OfType<ThemeInfo>().ToList();
                    themes.Add(defaultTheme.Name, defaultTheme);
                    //Then the rest
                    foreach (string themeFile in themeFiles)
                    {
                        // Skip the default theme here, since it will get loaded again without the *NG below...
                        if (themeFile.Contains("vs2015light.vstheme", StringComparison.Ordinal)) continue;
                        //filter default one
                        ThemeInfo extTheme = ThemeSerializer.LoadFromXmlFile(themeFile, defaultTheme);
                        if (extTheme.Theme == null || extTheme.Name == null || themes.ContainsKey(extTheme.Name)) continue;

                        if (extTheme.Name.Equals("darcula", StringComparison.Ordinal) || extTheme.Name.Equals("vs2015blue", StringComparison.Ordinal) ||
                            extTheme.Name.Equals("vs2015dark", StringComparison.Ordinal))
                            extTheme.Name = $"{extTheme.Name}NG";

                        themes.Add(extTheme.Name, extTheme);
                    }

                    //Load the embedded themes, extended palettes are taken from the vs2015 themes, trying to match the color theme

                    // 2015
                    var lightNG = themes["vs2015lightNG"] as ThemeInfo;
                    var darkNG = themes["vs2015darkNG"] as ThemeInfo;
                    var blueNG = themes["vs2015blueNG"] as ThemeInfo;

                    if (lightNG != null)
                    {
                        ThemeInfo vs2015Light = new("vs2015Light", lightNG.Theme, "", VisualStudioToolStripExtender.VsVersion.Vs2015, lightNG.ExtendedPalette);
                        themes.Add(vs2015Light.Name!, vs2015Light);
                    }
                    if (darkNG != null)
                    {
                        ThemeInfo vs2015Dark = new("vs2015Dark", darkNG.Theme, "", VisualStudioToolStripExtender.VsVersion.Vs2015, darkNG.ExtendedPalette);
                        themes.Add(vs2015Dark.Name!, vs2015Dark);
                    }
                    if (blueNG != null)
                    {
                        ThemeInfo vs2015Blue = new("vs2015Blue", blueNG.Theme, "", VisualStudioToolStripExtender.VsVersion.Vs2015, blueNG.ExtendedPalette);
                        themes.Add(vs2015Blue.Name!, vs2015Blue);
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error loading themes", ex);
            }

            return themes.Values.OfType<ThemeInfo>().ToList();
        }

        /// <summary>
        /// Add a new theme based on an existing one by cloning and renaming, the theme is saved to disk
        /// </summary>
        /// <param name="baseTheme"></param>
        /// <param name="newThemeName"></param>
        /// <returns></returns>
        public ThemeInfo? addTheme(ThemeInfo baseTheme, string newThemeName)
        {
            if (themes.Contains(newThemeName)) return null;

            // Embedded themes (vs2015Light/Dark/Blue) have empty URIs because they are built-in.
            // Resolve the corresponding NG file-based variant to use as the copy template.
            ThemeInfo fileBase = baseTheme;
            if (string.IsNullOrEmpty(fileBase.URI) && baseTheme.Name != null)
            {
                string ngName = baseTheme.Name.ToLowerInvariant() + "NG";
                if (themes[ngName] is ThemeInfo ngTheme && !string.IsNullOrEmpty(ngTheme.URI))
                    fileBase = ngTheme;
            }

            ThemeInfo modifiedTheme = (ThemeInfo)baseTheme.Clone();
            modifiedTheme.Name = newThemeName;
            modifiedTheme.IsExtendable = true;
            modifiedTheme.IsThemeBase = false;
            ThemeSerializer.SaveToXmlFile(modifiedTheme, fileBase);
            themes.Add(newThemeName, modifiedTheme);
            return modifiedTheme;
        }

        //Delete a theme from memory and disk
        public void deleteTheme(ThemeInfo themeToDelete)
        {
            if (themeToDelete.Name == null || !themes.Contains(themeToDelete.Name)) return;
            if (ActiveTheme == themeToDelete)
                ActiveTheme = DefaultTheme;
            themes.Remove(themeToDelete.Name);
            ThemeSerializer.DeleteFile(themeToDelete);
        }

        //Synchronize the theme XML values from memory to disk
        public static void updateTheme(ThemeInfo themeToUpdate)
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
            return name.IndexOfAny(badChars) == -1;
        }

        #endregion

        #region Events

        public delegate void ThemeChangedEventHandler();

        private ThemeChangedEventHandler? ThemeChangedEvent;

        public event ThemeChangedEventHandler ThemeChanged
        {
            add => ThemeChangedEvent = (ThemeChangedEventHandler?)Delegate.Combine(ThemeChangedEvent, value);
            remove => ThemeChangedEvent = (ThemeChangedEventHandler?)Delegate.Remove(ThemeChangedEvent, value);
        }

        // ReSharper disable once UnusedParameter.Local
        private void NotifyThemeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "Name", StringComparison.Ordinal))
            {
                return;
            }

            try
            {
                ThemeChangedEvent?.Invoke();
            }
            catch (Exception ex)
            {
                // Ensure we don't crash the whole app if one listener fails
                Debug.WriteLine($"Error in ThemeChangedEvent: {ex.Message}");
            }
        }

        #endregion

        #region Properties

        public bool ThemingActive
        {
            get => _themeActive;
            set
            {
                if (themes == null || themes.Count == 0) return;
                _themeActive = value;
                Properties.OptionsThemePage.Default.ThemingActive = value;
                NotifyThemeChanged(this, new PropertyChangedEventArgs(""));
            }
        }

        public ThemeInfo DefaultTheme =>
            themes != null && themes.ContainsKey("vs2015Light") && themes["vs2015Light"] is ThemeInfo cached
                ? cached
                : new ThemeInfo("vs2015Light", new VS2015LightTheme(), "",
                                VisualStudioToolStripExtender.VsVersion.Vs2015);

        public ThemeInfo HighContrastTheme =>
             _highContrastTheme ??= new ThemeInfo("HighContrast", new VS2005Theme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015, false);

        public ThemeInfo ActiveTheme
        {
            // default if themes are not enabled
            get
            {
                if (_highContrastActive) return HighContrastTheme;

                if (!ThemingActive) return DefaultTheme;
                return _activeTheme ?? DefaultTheme;
            }
            set
            {
                // You can only enable theming if there are themes loaded
                // Default accordingly...
                if (value == null)
                {
                    if (_activeTheme != null && _activeTheme.Name == DefaultTheme.Name) return;

                    Properties.OptionsThemePage.Default.ThemeName = DefaultTheme.Name;
                    _activeTheme = DefaultTheme;

                    NotifyThemeChanged(this, new PropertyChangedEventArgs("theme"));

                    Properties.OptionsThemePage.Default.Save();
                    return;
                }

                if (_activeTheme != null && string.Equals(_activeTheme.Name, value.Name, StringComparison.Ordinal)) return;

                _activeTheme = value;
                Properties.OptionsThemePage.Default.ThemeName = value.Name;
                NotifyThemeChanged(this, new PropertyChangedEventArgs("theme"));
            }
        }

        public bool ActiveAndExtended => ThemingActive && ActiveTheme.IsExtended;

        public int ThemesCount => themes.Count;

        #endregion
    }
}