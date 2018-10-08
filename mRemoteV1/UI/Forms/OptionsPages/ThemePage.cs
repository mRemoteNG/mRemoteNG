using System;
using System.Windows.Forms;
using mRemoteNG.Themes;
using System.Linq;
using System.Collections.Generic;
using BrightIdeasSoftware;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class ThemePage
    {

        #region Private Fields
        private ThemeManager _themeManager;
        private ThemeInfo _oriTheme;
        private bool _oriActiveTheming;
        List<ThemeInfo> modifiedThemes = new List<ThemeInfo>();
        #endregion


        public ThemePage()
        {

            InitializeComponent();
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
            themeEnableCombo.Text = Language.strOptionsThemeEnableTheming;
        }

        private new void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;
            base.ApplyTheme(); 
        }

        public override void LoadSettings()
        {
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
            //Color cell formatter 
            listPalette.FormatCell += ListPalette_FormatCell;
            //Load theming active property and disable controls 
            if (_themeManager.ThemingActive)
            {
                themeEnableCombo.Checked = true;
            }
            else
            {
                themeEnableCombo.Checked = false;
                cboTheme.Enabled = false;
                // reset to the default theme when disabling theme support
                _themeManager.ActiveTheme = _themeManager.DefaultTheme;
            }
        }

        private void ListPalette_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.ColumnIndex != ColorCol.Index) return;
            var colorElem = (PseudoKeyColor)e.Model;
            e.SubItem.BackColor = colorElem.Value;
        }


        public override void SaveSettings()
        {
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
            if (!_themeManager.ThemingActive) return;
            _themeManager.ActiveTheme = (ThemeInfo)cboTheme.SelectedItem;
            listPalette.ClearObjects();
            if (!_themeManager.ActiveTheme.IsExtendable || !_themeManager.ThemingActive) return;
            btnThemeNew.Enabled = true;
            listPalette.ClearObjects();
            listPalette.Enabled = false;
            ColorMeList();
            if (_themeManager.ActiveTheme.IsThemeBase) return;
            listPalette.Enabled = true;
            btnThemeDelete.Enabled = true;
            listPalette.CellClick += ListPalette_CellClick;
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

        private void ColorMeList()
        {
            foreach (var colorElem in _themeManager.ActiveTheme.ExtendedPalette.ExtColorPalette)
                listPalette.AddObject(new PseudoKeyColor(colorElem.Key, colorElem.Value));
        }

        private void btnThemeNew_Click(object sender, EventArgs e)
        {
            var name = _themeManager.ActiveTheme.Name;
            var res = Input.input.InputBox(Language.strOptionsThemeNewThemeCaption, Language.strOptionsThemeNewThemeText, ref name);
            if (res != DialogResult.OK) return;
            if (_themeManager.isThemeNameOk(name))
            {
                var addedTheme = _themeManager.addTheme(_themeManager.ActiveTheme, name);
                _themeManager.ActiveTheme = addedTheme;
                LoadSettings();
            }
            else
            {
                TaskDialog.CTaskDialog.ShowTaskDialogBox(this, Language.strErrors, Language.strOptionsThemeNewThemeError, "", "", "", "", "", "", TaskDialog.ETaskDialogButtons.Ok, TaskDialog.ESysIcons.Error, TaskDialog.ESysIcons.Information, 0);
            }
        }

        private void btnThemeDelete_Click(object sender, EventArgs e)
        {

            var res = TaskDialog.CTaskDialog.ShowTaskDialogBox(this, Language.strWarnings , Language.strOptionsThemeDeleteConfirmation, "", "", "", "", "", "", TaskDialog.ETaskDialogButtons.YesNo, TaskDialog.ESysIcons.Question, TaskDialog.ESysIcons.Information, 0);

            if (res != DialogResult.Yes) return;
            if (modifiedThemes.Contains(_themeManager.ActiveTheme))
                modifiedThemes.Remove(_themeManager.ActiveTheme);
            _themeManager.deleteTheme(_themeManager.ActiveTheme);
            LoadSettings();
        }

        #endregion

        #endregion

        private void themeEnableCombo_CheckedChanged(object sender, EventArgs e)
        {
            if (themeEnableCombo.Checked)
            {
                _themeManager.ThemingActive = true;
                if(_themeManager.ThemingActive)
                {
                    themeEnableCombo.Checked = true;
                    cboTheme.Enabled = true;
                }
                else
                {
                    TaskDialog.CTaskDialog.ShowTaskDialogBox(this, Language.strErrors, Language.strOptionsThemeErrorNoThemes, "", "", "", "", "", "", TaskDialog.ETaskDialogButtons.Ok, TaskDialog.ESysIcons.Error, TaskDialog.ESysIcons.Information, 0);
                    themeEnableCombo.Checked = false;
                }
            }
            else
            {
                _themeManager.ThemingActive = false;
                themeEnableCombo.Checked = false;
                cboTheme.Enabled = false;
            }
            LoadSettings();
        }
    }
}