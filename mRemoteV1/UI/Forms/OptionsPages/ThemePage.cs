using System;
using System.ComponentModel;
using System.Windows.Forms;
using mRemoteNG.Themes;
using System.Linq;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class ThemePage
    {

        #region Private Fields
        private ThemeManager _themeManager;
        private ThemeInfo _oriTheme;
        private bool      _oriActiveTheming;
        #endregion


        public ThemePage()
        {
          
            InitializeComponent();
            if (!Tools.DesignModeTest.IsInDesignMode(this))
            {
                _themeManager = ThemeManager.getInstance();
                _themeManager.ThemeChanged += ApplyTheme;
                 _oriTheme = _themeManager.ActiveTheme;
                _oriActiveTheming = _themeManager.ThemingActive;
            }
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
            //Apply language to active theme checkbox
            //Apply languate to warning label
        }

        private new void ApplyTheme()
        {
            if (Tools.DesignModeTest.IsInDesignMode(this))
                return;
            if (Themes.ThemeManager.getInstance().ThemingActive)
            {
                base.ApplyTheme(); 
            }
        }

        public override void LoadSettings()
        {
            base.SaveSettings();

            //Load the list of themes
            cboTheme.Items.AddRange(_themeManager.LoadThemes().OrderBy(x => x.Name).ToArray());
            cboTheme.SelectedItem = _themeManager.ActiveTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());
            cboTheme.DisplayMember = "Name";
            //Load colors form theme

            //Load theming active property and disable controls 
            if(_themeManager.ThemingActive)
            {
                themeEnableCombo.Checked = true;
            }else
            {
                themeEnableCombo.Checked = false;
                cboTheme.Enabled = false;
            }
        }

        public override void SaveSettings()
        {
            base.SaveSettings();
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
            _themeManager.ActiveTheme = (ThemeInfo)cboTheme.SelectedItem; 
        }

        private void btnThemeNew_Click(object sender, EventArgs e)
        {
           /* var newTheme = (ThemeInfo)_themeManager.ActiveTheme.Clone();
            newTheme.Name = Language.strUnnamedTheme;

            _themeList.Add(newTheme);

            cboTheme.SelectedItem = newTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());

            cboTheme.Focus();*/
        }

        private void btnThemeDelete_Click(object sender, EventArgs e)
        {
           /* var theme = (ThemeInfo) cboTheme.SelectedItem;
            if (theme == null)
            {
                return;
            }

            _themeList.Remove(theme);

            cboTheme.SelectedItem = _themeManager.DefaultTheme;
            cboTheme_SelectionChangeCommitted(this, new EventArgs());*/
        }

        #endregion

        #endregion

        private void themeEnableCombo_CheckedChanged(object sender, EventArgs e)
        {
            if(themeEnableCombo.Checked)
            {
                _themeManager.ThemingActive = true;
                themeEnableCombo.Checked = true;
                cboTheme.Enabled = true;
            }
            else
            {
                _themeManager.ThemingActive = false;
                themeEnableCombo.Checked = false;
                cboTheme.Enabled = false;
            }
        }
    }
}