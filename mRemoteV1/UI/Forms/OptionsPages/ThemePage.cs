using System;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class ThemePage
    {
        public ThemePage()
        {
            InitializeComponent();
        }

        public override string PageName
        {
            get { return Language.strOptionsTabTheme; }
            set { }
        }

        public override void ApplyLanguage()
        {
            base.ApplyLanguage();

            btnThemeDelete.Text = Language.strOptionsThemeButtonDelete;
            btnThemeNew.Text = Language.strOptionsThemeButtonNew;
        }

        public override void LoadSettings()
        {
            base.SaveSettings();

            _themeList = new BindingList<ThemeInfo>(ThemeManager.LoadThemes());
            cboTheme.DataSource = _themeList;
            cboTheme.SelectedItem = ThemeManager.ActiveTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());

            ThemePropertyGrid.PropertySort = PropertySort.Categorized;

            _originalTheme = ThemeManager.ActiveTheme;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            ThemeManager.SaveThemes(_themeList);
            Settings.Default.ThemeName = ThemeManager.ActiveTheme.Name;

            Settings.Default.Save();
        }

        public override void RevertSettings()
        {
            ThemeManager.ActiveTheme = _originalTheme;
        }

        #region Private Fields

        private BindingList<ThemeInfo> _themeList;
        private ThemeInfo _originalTheme;

        #endregion

        #region Private Methods

        #region Event Handlers

        private void cboTheme_DropDown(object sender, EventArgs e)
        {
            if (Equals(ThemeManager.ActiveTheme, ThemeManager.DefaultTheme))
            {
                return;
            }
            ThemeManager.ActiveTheme.Name = cboTheme.Text;
        }

        private void cboTheme_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboTheme.SelectedItem == null)
            {
                cboTheme.SelectedItem = ThemeManager.DefaultTheme;
            }

            if (Equals(cboTheme.SelectedItem, ThemeManager.DefaultTheme))
            {
                cboTheme.DropDownStyle = ComboBoxStyle.DropDownList;
                btnThemeDelete.Enabled = false;
                ThemePropertyGrid.Enabled = false;
            }
            else
            {
                cboTheme.DropDownStyle = ComboBoxStyle.DropDown;
                btnThemeDelete.Enabled = true;
                ThemePropertyGrid.Enabled = true;
            }

            ThemeManager.ActiveTheme = (ThemeInfo) cboTheme.SelectedItem;
            ThemePropertyGrid.SelectedObject = ThemeManager.ActiveTheme;
            ThemePropertyGrid.Refresh();
        }

        private void btnThemeNew_Click(object sender, EventArgs e)
        {
            var newTheme = (ThemeInfo) ThemeManager.ActiveTheme.Clone();
            newTheme.Name = Language.strUnnamedTheme;

            _themeList.Add(newTheme);

            cboTheme.SelectedItem = newTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());

            cboTheme.Focus();
        }

        private void btnThemeDelete_Click(object sender, EventArgs e)
        {
            var theme = (ThemeInfo) cboTheme.SelectedItem;
            if (theme == null)
            {
                return;
            }

            _themeList.Remove(theme);

            cboTheme.SelectedItem = ThemeManager.DefaultTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());
        }

        #endregion

        #endregion
    }
}