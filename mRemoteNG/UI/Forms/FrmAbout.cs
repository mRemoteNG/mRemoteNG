using System.Diagnostics;
using System.Windows.Forms;
using mRemoteNG.App.Info;
using mRemoteNG.Themes;
using mRemoteNG.Resources.Language;
using System.Reflection;
using mRemoteNG.Properties;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using mRemoteNG.UI.Window;

namespace mRemoteNG.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class frmAbout : BaseWindow
    {
        public static frmAbout Instance { get; set; } = new frmAbout();

        public frmAbout()
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
            base.Text = Language.MenuItem_About;
            TabText = Language.MenuItem_About;
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
            
            // Don't cancel close when shown in DockPanel
            // This allows the tab to close properly without showing connection close dialog
        }

        private void llLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/v" + Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, Assembly.GetExecutingAssembly().GetName().Version.ToString().Length - 2) + "-" + Properties.OptionsUpdatesPage.Default.CurrentUpdateChannelType + "/COPYING.txt");
            Close();
        }

        private void llChangelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/v" + Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, Assembly.GetExecutingAssembly().GetName().Version.ToString().Length - 2) + "-" + Properties.OptionsUpdatesPage.Default.CurrentUpdateChannelType + "/CHANGELOG.md");
            Close();
        }

        private void llCredits_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl("https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/v" + Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, Assembly.GetExecutingAssembly().GetName().Version.ToString().Length - 2) + "-" + Properties.OptionsUpdatesPage.Default.CurrentUpdateChannelType + "/CREDITS.md");
            Close();
        }

        private void OpenUrl(string url)
        {
            try
            {
                // Try to open URL with UseShellExecute
                var startInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Use ArgumentList for better security instead of string concatenation
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    startInfo.ArgumentList.Add("/c");
                    startInfo.ArgumentList.Add("start");
                    startInfo.ArgumentList.Add(url);
                    Process.Start(startInfo);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "xdg-open",
                        UseShellExecute = false
                    };
                    startInfo.ArgumentList.Add(url);
                    Process.Start(startInfo);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "open",
                        UseShellExecute = false
                    };
                    startInfo.ArgumentList.Add(url);
                    Process.Start(startInfo);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}