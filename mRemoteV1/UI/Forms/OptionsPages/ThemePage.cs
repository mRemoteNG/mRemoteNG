using System;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class ThemePage
    {
        private ThemeManager _themeManager;

        public ThemePage()
        {
            _themeManager = ThemeManager.getInstance();
            InitializeComponent();
            base.ApplyTheme();

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

            //some work in theming here
            _themeList = new BindingList<ThemeInfo>(_themeManager.LoadThemes());
            cboTheme.DataSource = _themeList;
            cboTheme.SelectedItem = _themeManager.ActiveTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());

            ThemePropertyGrid.PropertySort = PropertySort.Categorized;

            _originalTheme = _themeManager.ActiveTheme;
        }

        public override void SaveSettings()
        {
            base.SaveSettings();

            //_themeManager.SaveThemes(_themeList);
            //Settings.Default.ThemeName = _themeManager.ActiveTheme.Name;

            Settings.Default.Save();
        }

        public override void RevertSettings()
        {
            _themeManager.ActiveTheme = _originalTheme;
        }

        #region Private Fields

        private BindingList<ThemeInfo> _themeList;
        private ThemeInfo _originalTheme;

        #endregion

        #region Private Methods

        #region Event Handlers

        private void cboTheme_DropDown(object sender, EventArgs e)
        {
            if (Equals(_themeManager.ActiveTheme, _themeManager.DefaultTheme))
            {
                return;
            }
            _themeManager.ActiveTheme.Name = cboTheme.Text;
        }

        private void cboTheme_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboTheme.SelectedItem == null)
            {
                cboTheme.SelectedItem = _themeManager.DefaultTheme;
            }

            if (Equals(cboTheme.SelectedItem, _themeManager.DefaultTheme))
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

            _themeManager.ActiveTheme = (ThemeInfo) cboTheme.SelectedItem;
            ThemePropertyGrid.SelectedObject = _themeManager.ActiveTheme;
            ThemePropertyGrid.Refresh();
        }

        private void btnThemeNew_Click(object sender, EventArgs e)
        {
            var newTheme = (ThemeInfo)_themeManager.ActiveTheme.Clone();
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

            cboTheme.SelectedItem = _themeManager.DefaultTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());
        }

        #endregion

        #endregion
    }
}