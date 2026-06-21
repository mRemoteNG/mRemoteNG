using System;
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
using mRemoteNG.App;
using mRemoteNG.Messages;

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

        private new void ApplyTheme()
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
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var updateChannel = Properties.OptionsUpdatesPage.Default.CurrentUpdateChannelType;
            if (version != null && updateChannel != null)
            {
                var versionString = version.ToString();
                OpenUrl("https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/v" + versionString[..^2] + "-" + updateChannel + "/COPYING.txt");
            }
            Close();
        }

        private void llChangelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var updateChannel = Properties.OptionsUpdatesPage.Default.CurrentUpdateChannelType;
            if (version != null && updateChannel != null)
            {
                var versionString = version.ToString();
                OpenUrl("https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/v" + versionString[..^2] + "-" + updateChannel + "/CHANGELOG.md");
            }
            Close();
        }

        private void llCredits_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var updateChannel = Properties.OptionsUpdatesPage.Default.CurrentUpdateChannelType;
            if (version != null && updateChannel != null)
            {
                var versionString = version.ToString();
                OpenUrl("https://raw.githubusercontent.com/mRemoteNG/mRemoteNG/v" + versionString[..^2] + "-" + updateChannel + "/CREDITS.md");
            }
            Close();
        }

        private static void OpenUrl(string url)
        {
            // Validate URL format to prevent injection
            if (string.IsNullOrWhiteSpace(url))
                return;
            
            // Basic URL validation - ensure it starts with http:// or https://
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                // Invalid URL format - don't try to open it
                return;
            }
            
            try
            {
                // Use the standard .NET approach for opening URLs securely
                // UseShellExecute=true delegates to the OS default handler
                var startInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch
            {
                // Fallback for older .NET Core versions with bug: https://github.com/dotnet/corefx/issues/10361
                // Use platform-specific URL launchers
                try
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // Use rundll32 with url.dll as fallback
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = "rundll32.exe",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        startInfo.ArgumentList.Add("url.dll,FileProtocolHandler");
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
                }
                catch
                {
                    // Unable to open URL - notify the user
                    Runtime.MessageCollector?.AddMessage(MessageClass.WarningMsg, 
                        "Unable to open URL in browser. Please open manually: " + url, true);
                }
            }
        }
    }
}