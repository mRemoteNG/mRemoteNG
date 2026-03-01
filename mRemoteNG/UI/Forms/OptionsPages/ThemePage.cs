using System;
using System.Windows.Forms;
using mRemoteNG.Themes;
using System.Linq;
using System.Collections.Generic;
using BrightIdeasSoftware;
using mRemoteNG.Properties;
using mRemoteNG.UI.TaskDialog;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    [SupportedOSPlatform("windows")]
    public sealed partial class ThemePage
    {
        #region Private Fields

        private readonly ThemeManager _themeManager;
        private readonly bool _oriActiveTheming;
        private ThemeInfo? _oriActiveTheme;
        private readonly List<ThemeInfo> modifiedThemes = [];

        #endregion

        public ThemePage()
        {
            InitializeComponent();
            PageIcon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.AppearanceEditor_16x);
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
            _themeManager.ThemeChanged += ApplyTheme;
            _oriActiveTheming = _themeManager.ThemingActive;
        }

        public override string PageName
        {
            get => Language.Theme;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            btnThemeDelete.Text = Language._Delete;
            btnThemeNew.Text = Language._New;
            labelRestart.Text = "Theme changes are applied live.";
        }

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;
            base.ApplyTheme();
        }

        public override void LoadSettings()
        {
            //At first we cannot create or delete themes, depends later on the type of selected theme
            btnThemeNew.Enabled = false;
            btnThemeDelete.Enabled = false;
            //Load the list of themes
            cboTheme.Items.Clear();
            cboTheme.DisplayMember = "Name";
            // ReSharper disable once CoVariantArrayConversion
            cboTheme.Items.AddRange(_themeManager.LoadThemes().OrderBy(x => x.Name, StringComparer.Ordinal).ToArray());
            cboTheme.SelectedItem = _themeManager.ActiveTheme;
            // Store the original active theme for reverting
            _oriActiveTheme = _themeManager.ActiveTheme;
            cboTheme_SelectionChangeCommitted(this, EventArgs.Empty);

            listPalette.FormatCell += ListPalette_FormatCell; //Color cell formatter

            // Apply the current theme to the panel on load
            ApplyTheme();
        }

        private void ListPalette_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.ColumnIndex != ColorCol.Index) return;
            PseudoKeyColor colorElem = (PseudoKeyColor)e.Model;
            e.SubItem.BackColor = colorElem.Value;
        }


        public override void SaveSettings()
        {
            base.SaveSettings();

            Properties.OptionsThemePage.Default.ThemingActive = true;

            // Apply the selected theme live without requiring a restart
            if (cboTheme.SelectedItem != null)
            {
                ThemeInfo selectedTheme = (ThemeInfo)cboTheme.SelectedItem;
                if (!Properties.OptionsThemePage.Default.ThemeName.Equals(selectedTheme.Name, StringComparison.Ordinal))
                {
                    Properties.OptionsThemePage.Default.ThemeName = selectedTheme.Name;
                    _themeManager.ActiveTheme = selectedTheme;
                }
            }

            foreach (ThemeInfo updatedTheme in modifiedThemes)
            {
                ThemeManager.updateTheme(updatedTheme);
            }

            Properties.OptionsThemePage.Default.Save();
        }

        public override void RevertSettings()
        {
            base.RevertSettings();
            _themeManager.ThemingActive = _oriActiveTheming;

            // Clear the modified themes list without saving
            modifiedThemes.Clear();

            // Restore the original theme selection
            if (_oriActiveTheme != null)
            {
                _themeManager.ActiveTheme = _oriActiveTheme;
                // Reload the theme list to reflect the original state
                cboTheme.Items.Clear();
                cboTheme.Items.AddRange(_themeManager.LoadThemes().OrderBy(x => x.Name, StringComparer.Ordinal).ToArray());
                cboTheme.SelectedItem = _oriActiveTheme;
                cboTheme_SelectionChangeCommitted(this, EventArgs.Empty);
            }
        }

        #region Private Methods

        #region Event Handlers

        private void cboTheme_SelectionChangeCommitted(object sender, EventArgs e)
        {
            btnThemeNew.Enabled = false;
            btnThemeDelete.Enabled = false;

            // don't display listPalette if it's not an Extendable theme...
            listPalette.CellClick -= ListPalette_CellClick;
            listPalette.Enabled = false;
            listPalette.Visible = false;

            if (!_themeManager.ThemingActive) return;

            btnThemeNew.Enabled = true;

            ThemeInfo? selectedTheme = cboTheme.SelectedItem as ThemeInfo;

            if (selectedTheme != null && selectedTheme.IsExtendable)
            {
                // it's Extendable, so now we can do this more expensive operations...
                listPalette.ClearObjects();
                ColorMeList(selectedTheme);
                listPalette.Enabled = true;
                listPalette.Visible = true;
                listPalette.CellClick += ListPalette_CellClick;
            }

            // Apply selected theme as live preview
            if (selectedTheme != null)
                _themeManager.ActiveTheme = selectedTheme;

            if (selectedTheme != null && selectedTheme.IsThemeBase) return;

            btnThemeDelete.Enabled = true;
        }

        /// <summary>
        /// Edit an object, since KeyValuePair value cannot be set without creating a new object, a parallel object model exist in the list
        /// besides the one in the active theme, so any modification must be done to the two models
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListPalette_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Model == null) return;
            PseudoKeyColor colorElem = (PseudoKeyColor)e.Model;

            ColorDialog colorDlg = new()
            {
                AllowFullOpen = true,
                FullOpen = true,
                AnyColor = true,
                SolidColorOnly = false,
                Color = colorElem.Value
            };

            if (colorDlg.ShowDialog() != DialogResult.OK) return;
            modifiedThemes.Add(_themeManager.ActiveTheme);
            _themeManager.ActiveTheme.ExtendedPalette?.replaceColor(colorElem.Key, colorDlg.Color);
            colorElem.Value = colorDlg.Color;
            listPalette.RefreshObject(e.Model);
            _themeManager.refreshUI();
        }

        private void ColorMeList(ThemeInfo ti)
        {
            if (ti.ExtendedPalette == null) return;
            foreach (KeyValuePair<string, System.Drawing.Color> colorElem in ti.ExtendedPalette.ExtColorPalette)
                listPalette.AddObject(new PseudoKeyColor(colorElem.Key, colorElem.Value));
        }

        private void btnThemeNew_Click(object sender, EventArgs e)
        {
            using (FrmInputBox frmInputBox = new(Language.OptionsThemeNewThemeCaption, Language.OptionsThemeNewThemeText, _themeManager.ActiveTheme.Name ?? string.Empty))
            {
                DialogResult dr = frmInputBox.ShowDialog();
                if (dr != DialogResult.OK) return;
                if (frmInputBox.returnValue != null && _themeManager.isThemeNameOk(frmInputBox.returnValue))
                {
                    ThemeInfo? addedTheme = _themeManager.addTheme(_themeManager.ActiveTheme, frmInputBox.returnValue);
                    if (addedTheme != null)
                        _themeManager.ActiveTheme = addedTheme;
                    LoadSettings();
                }
                else
                {
                    CTaskDialog.ShowTaskDialogBox(this, Language.Errors, Language.OptionsThemeNewThemeError, "", "", "", "", "", "", ETaskDialogButtons.Ok, ESysIcons.Error, ESysIcons.Information, 0);
                }
            }
        }

        private void btnThemeDelete_Click(object sender, EventArgs e)
        {
            DialogResult res = CTaskDialog.ShowTaskDialogBox(this, Language.Warnings,
                                                    Language.OptionsThemeDeleteConfirmation, "", "", "", "", "", "",
                                                    ETaskDialogButtons.YesNo,
                                                    ESysIcons.Question, ESysIcons.Information, 0);

            if (res != DialogResult.Yes) return;
            if (modifiedThemes.Contains(_themeManager.ActiveTheme))
                modifiedThemes.Remove(_themeManager.ActiveTheme);
            _themeManager.deleteTheme(_themeManager.ActiveTheme);
            LoadSettings();
        }

        #endregion

        #endregion
    }
}