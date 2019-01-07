using System;
using System.Windows.Forms;
using mRemoteNG.Themes;
using System.Linq;
using System.Collections.Generic;
using BrightIdeasSoftware;
using mRemoteNG.UI.Forms.Input;
using mRemoteNG.UI.TaskDialog;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class ThemePage
    {

        #region Private Fields
        private readonly ThemeManager _themeManager;
        private readonly ThemeInfo _oriTheme;
        private readonly bool _oriActiveTheming;
        private readonly List<ThemeInfo> modifiedThemes = new List<ThemeInfo>();
        #endregion

        public ThemePage()
        {
            InitializeComponent();
            PageIcon = Resources.Appearance_Icon;
            _themeManager = ThemeManager.getInstance();
            if (!_themeManager.ThemingActive) return;
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            _oriTheme = _themeManager.ActiveTheme;
            _oriActiveTheming = _themeManager.ThemingActive;
        }

        public override string PageName
        {
            get => Language.strOptionsTabTheme;
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            btnThemeDelete.Text = Language.strOptionsThemeButtonDelete;
            btnThemeNew.Text = Language.strOptionsThemeButtonNew;
            labelRestart.Text = Language.strOptionsThemeThemeChaangeWarning;
            themeEnableChk.Text = Language.strOptionsThemeEnableTheming;
        }

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;
            base.ApplyTheme(); 
        }

        public override void LoadSettings()
        {
            themeEnableChk.CheckedChanged -= ThemeEnableChkCheckedChanged;
            base.SaveSettings();
            //At first we cannot create or delete themes, depends later on the type of selected theme
            btnThemeNew.Enabled = false;
            btnThemeDelete.Enabled = false;
            //Load the list of themes
            cboTheme.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            cboTheme.Items.AddRange(_themeManager.LoadThemes().OrderBy(x => x.Name).ToArray());
            cboTheme.SelectedItem = _themeManager.ActiveTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());
            cboTheme.DisplayMember = "Name";
            
            //Load theming active property and disable controls 
            if (_themeManager.ThemingActive)
            {
                themeEnableChk.Checked = true;
                listPalette.FormatCell += ListPalette_FormatCell; //Color cell formatter 
            }
            else
            {
                themeEnableChk.Checked = false;
                cboTheme.Enabled = false;
                // reset to the default theme when disabling theme support
                _themeManager.ActiveTheme = _themeManager.DefaultTheme;
            }
            themeEnableChk.CheckedChanged += ThemeEnableChkCheckedChanged;
        }

        private void ListPalette_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.ColumnIndex != ColorCol.Index) return;
            var colorElem = (PseudoKeyColor)e.Model;
            e.SubItem.BackColor = colorElem.Value;
        }


        public override void SaveSettings()
        {
            // Save the theme settings form close so we don't run into unexpected results while modifying...
            // Prompt the user that a restart is required to apply the new theme...
            if (!Settings.Default.ThemeName.Equals(((ThemeInfo)cboTheme.SelectedItem).Name))
            {
                Settings.Default.ThemeName = ((ThemeInfo)cboTheme.SelectedItem).Name;
                Settings.Default.Save();

                CTaskDialog.MessageBox("Theme Changed", "Restart Required.", "Please restart mRemoteNG to apply the selected theme.",
                    ETaskDialogButtons.Ok, ESysIcons.Information);
            }

            base.SaveSettings();
            foreach(var updatedTheme in modifiedThemes)
            {
                _themeManager.updateTheme(updatedTheme);
            }
        }

        public override void RevertSettings()
        {
            base.RevertSettings();
            _themeManager.ActiveTheme = _oriTheme;
            _themeManager.ThemingActive = _oriActiveTheming;
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

            var selectedTheme = (ThemeInfo)cboTheme.SelectedItem;
            
            if (selectedTheme.IsExtendable)
            {
                // it's Extendable, so now we can do this more expensive operations...
                listPalette.ClearObjects();
                ColorMeList(selectedTheme);
                listPalette.Enabled = true;
                listPalette.Visible = true;
                listPalette.CellClick += ListPalette_CellClick;
            }

            if (selectedTheme.IsThemeBase) return;

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

            var colorElem = (PseudoKeyColor)e.Model;

            var colorDlg = new ColorDialog
            {
                AllowFullOpen = true,
                FullOpen = true,
                AnyColor = true,
                SolidColorOnly = false,
                Color = colorElem.Value
            };

            if (colorDlg.ShowDialog() != DialogResult.OK) return;
            modifiedThemes.Add(_themeManager.ActiveTheme);
            _themeManager.ActiveTheme.ExtendedPalette.replaceColor(colorElem.Key, colorDlg.Color);
            colorElem.Value = colorDlg.Color;
            listPalette.RefreshObject(e.Model);
            _themeManager.refreshUI();

        }

        private void ColorMeList(ThemeInfo ti)
        {
            foreach (var colorElem in ti.ExtendedPalette.ExtColorPalette)
                listPalette.AddObject(new PseudoKeyColor(colorElem.Key, colorElem.Value));
        }

        private void btnThemeNew_Click(object sender, EventArgs e)
        {
            using (var frmInputBox = new FrmInputBox(Language.strOptionsThemeNewThemeCaption, Language.strOptionsThemeNewThemeText, _themeManager.ActiveTheme.Name))
            {
                var dr = frmInputBox.ShowDialog();
                if (dr != DialogResult.OK) return;
                if (_themeManager.isThemeNameOk(frmInputBox.returnValue))
                {
                    var addedTheme = _themeManager.addTheme(_themeManager.ActiveTheme, frmInputBox.returnValue);
                    _themeManager.ActiveTheme = addedTheme;
                    LoadSettings();
                }
                else
                {
                    CTaskDialog.ShowTaskDialogBox(this, Language.strErrors, Language.strOptionsThemeNewThemeError, "", "", "", "", "", "", ETaskDialogButtons.Ok, ESysIcons.Error, ESysIcons.Information, 0);
                }
            }
        }

        private void btnThemeDelete_Click(object sender, EventArgs e)
        {

            var res = CTaskDialog.ShowTaskDialogBox(this, Language.strWarnings , Language.strOptionsThemeDeleteConfirmation, "", "", "", "", "", "", ETaskDialogButtons.YesNo, ESysIcons.Question, ESysIcons.Information, 0);

            if (res != DialogResult.Yes) return;
            if (modifiedThemes.Contains(_themeManager.ActiveTheme))
                modifiedThemes.Remove(_themeManager.ActiveTheme);
            _themeManager.deleteTheme(_themeManager.ActiveTheme);
            LoadSettings();
        }

        #endregion

        #endregion

        private void ThemeEnableChkCheckedChanged(object sender, EventArgs e)
        {
            if (themeEnableChk.Checked)
            {
                if(_themeManager.ThemesCount > 0)
                {
                    _themeManager.ThemingActive = true;
                    cboTheme.Enabled = true;
                }
                else
                {
                    CTaskDialog.ShowTaskDialogBox(this, Language.strErrors, Language.strOptionsThemeErrorNoThemes, "", "", "", "", "", "", ETaskDialogButtons.Ok, ESysIcons.Error, ESysIcons.Information, 0);
                    themeEnableChk.Checked = false;
                    _themeManager.ThemingActive = false;
                    cboTheme.Enabled = false;
                }

                listPalette.FormatCell += ListPalette_FormatCell;
            }
            else
            {
                _themeManager.ThemingActive = false;
                themeEnableChk.Checked = false;
                cboTheme.Enabled = false;
                listPalette.FormatCell -= ListPalette_FormatCell;
            }
            LoadSettings();
        }
    }
}