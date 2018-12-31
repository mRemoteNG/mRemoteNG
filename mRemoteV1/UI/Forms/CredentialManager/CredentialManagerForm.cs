using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.Themes;
using mRemoteNG.Tools;

namespace mRemoteNG.UI.Forms.CredentialManager
{
    public partial class CredentialManagerForm : Form
    {
        private readonly IEnumerable<UserControl> _pageControls;
        private readonly ThemeManager _themeManager;

        public CredentialManagerForm(IEnumerable<UserControl> pageControls)
        {
            _pageControls = pageControls.ThrowIfNull(nameof(pageControls));
            InitializeComponent();
            _themeManager = ThemeManager.getInstance();
            _themeManager.ThemeChanged += ApplyTheme;
            ApplyTheme();
            ApplyLanguage();
            SetupListView();
            olvPageList.SelectedIndex = 0;
        }

        private void SetupListView()
        {
            olvColumnPage.ImageGetter = ImageGetter;
            olvPageList.SelectionChanged += (sender, args) => ShowPage(olvPageList.SelectedObject as Control);
            olvPageList.SetObjects(_pageControls);
        }

        private void ShowPage(Control page)
        {
            if (page == null)
                return;

            panelMain.Controls.Clear();
            panelMain.Controls.Add(page);
            page.Dock = DockStyle.Fill;
        }

        private object ImageGetter(object rowObject)
        {
            var page = rowObject as ICredentialManagerPage;
            return page?.PageIcon;
        }

        private void ApplyTheme()
        {
            if (!_themeManager.ThemingActive)
                return;

            BackColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = _themeManager.ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        private void ApplyLanguage()
        {
            Text = Language.strCredentialManager;
            buttonClose.Text = Language.strButtonClose;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}