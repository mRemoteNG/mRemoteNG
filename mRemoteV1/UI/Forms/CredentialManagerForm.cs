using System;
using System.Collections.Generic;
using System.Windows.Forms;
using mRemoteNG.UI.Forms.CredentialManagerPages;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms
{
    public partial class CredentialManagerForm : Form
    {
        private readonly IEnumerable<UserControl> _pageControls;

        public CredentialManagerForm(IEnumerable<UserControl> pageControls)
        {
            if (pageControls == null)
                throw new ArgumentNullException(nameof(pageControls));

            _pageControls = pageControls;
            InitializeComponent();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
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
            if (page == null) return;
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
            if (Themes.ThemeManager.getInstance().ThemingActive)
            {
                BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            }
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