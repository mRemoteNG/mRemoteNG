using mRemoteNG.App;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.Versioning;
using System.Drawing;
using System.Windows.Forms;

namespace mRemoteNG.Themes
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// Main class of the theming component. Centralizes creation, loading and deletion of themes
    /// Implemented as a singleton
    /// </summary>
    public class ThemeManager
    {
        #region Private Variables

        private ThemeInfo _activeTheme = null!; // set by SetActive() in the constructor
        private Hashtable themes = null!;       // set by LoadThemes() in the constructor
        private bool _themeActive;
        private static ThemeManager? themeInstance;
        private readonly string themePath = App.Info.SettingsFileInfo.ThemeFolder;

        #endregion

        #region Constructors

        private ThemeManager()
        {
            LoadThemes();
            SetActive();
            _themeActive = true;
        }

        private void SetActive()
        {
            if (themes[Properties.OptionsThemePage.Default.ThemeName] is ThemeInfo savedTheme)
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

        // Persist the dark/light state of the active theme so startup can read it
        // without loading any theme from disk (see ProgramRoot.StartApplication).
        // Uses the raw _activeTheme, not the ThemingActive-gated ActiveTheme: during
        // construction ThemingActive is still false, which would otherwise persist "light".
        private void PersistActiveThemeDarkFlag()
        {
            bool dark = IsThemeDark(_activeTheme);
            if (Properties.OptionsThemePage.Default.IsActiveThemeDark == dark) return;
            Properties.OptionsThemePage.Default.IsActiveThemeDark = dark;
            Properties.OptionsThemePage.Default.Save();
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
        public List<ThemeInfo> LoadThemes()
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
                    if (defaultTheme == null)
                        return themes.Values.OfType<ThemeInfo>().ToList();
                    themes.Add(defaultTheme.Name, defaultTheme);
                    //Then the rest
                    foreach (string themeFile in themeFiles)
                    {
                        // Skip the default theme here, since it will get loaded again without the *NG below...
                        if (themeFile.Contains("vs2015light.vstheme")) continue;
                        //filter default one
                        ThemeInfo extTheme = ThemeSerializer.LoadFromXmlFile(themeFile, defaultTheme);
                        if (extTheme.Theme == null || themes.ContainsKey(extTheme.Name)) continue;

                        if (extTheme.Name.Equals("darcula") || extTheme.Name.Equals("vs2015blue") ||
                            extTheme.Name.Equals("vs2015dark"))
                            extTheme.Name = $"{extTheme.Name}NG";

                        themes.Add(extTheme.Name, extTheme);
                    }

                    //Load the embedded themes, extended palettes are taken from the vs2015 themes, trying to match the color theme

                    // 2015
                    if (themes["vs2015lightNG"] is ThemeInfo lightBase)
                    {
                        ThemeInfo vs2015Light = new("vs2015Light", new VS2015LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015, lightBase.ExtendedPalette);
                        themes.Add(vs2015Light.Name, vs2015Light);
                    }

                    if (themes["vs2015darkNG"] is ThemeInfo darkBase)
                    {
                        ThemeInfo vs2015Dark = new("vs2015Dark", new VS2015DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015, darkBase.ExtendedPalette);
                        themes.Add(vs2015Dark.Name, vs2015Dark);
                    }

                    if (themes["vs2015blueNG"] is ThemeInfo blueBase)
                    {
                        ThemeInfo vs2015Blue = new("vs2015Blue", new VS2015BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015, blueBase.ExtendedPalette);
                        themes.Add(vs2015Blue.Name, vs2015Blue);
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
            ThemeInfo modifiedTheme = (ThemeInfo)baseTheme.Clone();
            modifiedTheme.Name = newThemeName;
            modifiedTheme.IsExtendable = true;
            modifiedTheme.IsThemeBase = false;
            ThemeSerializer.SaveToXmlFile(modifiedTheme, baseTheme);
            themes.Add(newThemeName, modifiedTheme);
            return modifiedTheme;
        }

        //Delete a theme from memory and disk
        public void deleteTheme(ThemeInfo themeToDelete)
        {
            if (!themes.Contains(themeToDelete.Name)) return;
            if (ActiveTheme == themeToDelete)
                ActiveTheme = DefaultTheme;
            themes.Remove(themeToDelete.Name);
            ThemeSerializer.DeleteFile(themeToDelete);
        }

        //Synchronize the theme XML values from memory to disk
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
            if (e.PropertyName == "Name")
            {
                return;
            }

            ThemeChangedEvent?.Invoke();
        }

        #endregion

        #region Properties

        public bool ThemingActive
        {
            get => _themeActive;
            set
            {
                if (themes.Count == 0) return;
                _themeActive = value;
                Properties.OptionsThemePage.Default.ThemingActive = value;
                PersistActiveThemeDarkFlag();
                NotifyThemeChanged(this, new PropertyChangedEventArgs(""));
            }
        }

        public ThemeInfo DefaultTheme =>
            (themes != null && ThemesCount > 0 ? themes["vs2015Light"] as ThemeInfo : null)
            ?? new ThemeInfo("vs2015Light", new VS2015LightTheme(), "",
                             VisualStudioToolStripExtender.VsVersion.Vs2015);

        public ThemeInfo ActiveTheme
        {
            // default if themes are not enabled
            get => ThemingActive == false ? DefaultTheme : _activeTheme;
            set
            {
                // You can only enable theming if there are themes loaded
                // Default accordingly...
                if (value == null)
                {
                    bool changed = !Properties.OptionsThemePage.Default.ThemeName.Equals(DefaultTheme.Name);

                    Properties.OptionsThemePage.Default.ThemeName = DefaultTheme.Name;
                    _activeTheme = DefaultTheme;
                    PersistActiveThemeDarkFlag();

                    if (changed)
                        NotifyThemeChanged(this, new PropertyChangedEventArgs("theme"));

                    Properties.OptionsThemePage.Default.Save();
                    return;
                }

                _activeTheme = value;
                Properties.OptionsThemePage.Default.ThemeName = value.Name;
                PersistActiveThemeDarkFlag();
                NotifyThemeChanged(this, new PropertyChangedEventArgs("theme"));
            }
        }

        public bool ActiveAndExtended => ThemingActive && ActiveTheme.IsExtended;

        // Below this HSL lightness (Color.GetBrightness) the "Dialog_Background" is treated as dark.
        private const float DarkThemeBrightnessThreshold = 0.5f;

        // True when the given theme has a dark background (HSL lightness of "Dialog_Background").
        public static bool IsThemeDark(ThemeInfo? theme)
        {
            Color background = theme?.ExtendedPalette?.getColor("Dialog_Background") ?? SystemColors.Control;
            return background.GetBrightness() < DarkThemeBrightnessThreshold;
        }

        /// <summary>
        /// True when the active theme has a dark background (derived from the "Dialog_Background"
        /// brightness (HSL lightness via <see cref="Color.GetBrightness"/>), since there is no
        /// explicit dark flag).
        /// </summary>
        public bool IsActiveThemeDark => IsThemeDark(ActiveTheme);

        /// <summary>
        /// Applies a dark or light native title bar to the given form based on the active theme's
        /// background brightness. Safe to call before the handle exists (no-op).
        /// </summary>
        public void ApplyThemeToTitleBar(Form form)
        {
            if (form == null || !form.IsHandleCreated)
                return;

            NativeMethods.UseImmersiveDarkMode(form.Handle, IsActiveThemeDark);
        }

        public int ThemesCount => themes.Count;

        #endregion
    }
}