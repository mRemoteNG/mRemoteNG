using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{
    public partial class FrmAbout : Form
    {
        public static FrmAbout Instance { get; set; } = new FrmAbout();

        private FrmAbout()
        {
            InitializeComponent();
            Icon = Resources.ImageConverter.GetImageAsIcon(Properties.Resources.UIAboutBox_16x);
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
            ApplyTheme();
        }

        private void ApplyLanguage()
        {
            lblLicense.Text = Language.ReleasedUnderGPL;
            base.Text = Language.About;
            llChangelog.Text = Language.Changelog;
            llCredits.Text = Language.Credits;
            llLicense.Text = Language.License;
            lblCopyright.Text = GeneralAppInfo.Copyright;
            lblVersion.Text = $@"Version {GeneralAppInfo.ApplicationVersion}";
            AddPortableString();
        }

        [Conditional("PORTABLE")]
        private void AddPortableString() => lblTitle.Text += $@" {Language.PortableEdition}";

        private void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ThemingActive) return;
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            pnlBottom.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlBottom.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            e.Cancel = true;
            Hide();
        }

        private void llLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/mRemoteNG/mRemoteNG/blob/develop/COPYING.TXT");
            Close();
        }

        private void llChangelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/mRemoteNG/mRemoteNG/blob/develop/CHANGELOG.md");
            Close();
        }

        private void llCredits_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/mRemoteNG/mRemoteNG/blob/develop/CREDITS.md");
            Close();
        }
    }
}