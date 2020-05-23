using mRemoteNG.App.Info;
using mRemoteNG.Themes;
using System.Windows.Forms;
using System.Diagnostics;

namespace mRemoteNG.UI.Window
{
    public partial class FrmAbout : Form
    {
        public static FrmAbout Instance { get; set; } = new FrmAbout();

        private FrmAbout()
        {
            InitializeComponent();
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
            ApplyTheme();
        }

        private void ApplyLanguage()
        {
            lblLicense.Text = Language.strLabelReleasedUnderGPL;
            Text = Language.strAbout;
            llChangelog.Text = Language.strChangelog;
            llCredits.Text = Language.strCredits;
            llLicense.Text = Language.strLicense;
            lblCopyright.Text = GeneralAppInfo.Copyright;
            lblVersion.Text = $@"Version {GeneralAppInfo.ApplicationVersion}";
            AddPortableString();
        }

        [Conditional("PORTABLE")]
        private void AddPortableString() => lblTitle.Text += $@" {Language.strLabelPortableEdition}";

        private void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ThemingActive) return;
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            pnlBottom.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlBottom.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
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