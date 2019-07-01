using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using Gecko;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.Themes;
using Markdig;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Window
{
    public partial class AboutWindow : BaseWindow
    {
        #region Public Methods

        public AboutWindow()
        {
            WindowType = WindowType.About;
            DockPnl = new DockContent();
            if (!Xpcom.IsInitialized)
                Xpcom.Initialize("Firefox");
            InitializeComponent();
            FontOverrider.FontOverride(this);
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
            ApplyTheme();
            LoadDocuments();
        }

        #endregion Public Methods

        #region Private Methods

        private void ApplyLanguage()
        {
            lblLicense.Text = Language.strLabelReleasedUnderGPL;
            TabText = Language.strAbout;
            Text = Language.strAbout;
            lblCopyright.Text = GeneralAppInfo.Copyright;
            lblVersion.Text = $@"Version {GeneralAppInfo.ApplicationVersion}";
#if PORTABLE
            lblTitle.Text += $@" {Language.strLabelPortableEdition}";
#endif
        }

        private new void ApplyTheme()
        {
            if (!ThemeManager.getInstance().ThemingActive) return;
            base.ApplyTheme();
            if (!ThemeManager.getInstance().ActiveAndExtended) return;
            BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlBottom.BackColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlBottom.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            pnlTop.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
            pnlTop.ForeColor = ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
        }

        #endregion Private Methods

        #region Form Stuff

        private void LoadDocuments()
        {
            try
            {
                // AppVeyor seems to pull text files in UNIX format... This messes up the display on the about screen...
                //
                // This would be MUCH faster:
                //var UnxEndRx = new Regex(@"(?<!\r)\n$"); // Look for UNIX line endings and still Windows line endings.
                //if (UnxEndRx.IsMatch(txtChangeLog.Text))
                //        txtChangeLog.Text = txtChangeLog.Text.Replace("\n", Environment.NewLine);
                //
                // But for some reason that I couldn't figure out, the RegEx.IsMatch on CREDITS.md/txtCredits.Text
                // did not work at all despite it CLEARLY ending with \n when pulled from AppVeyor...
                // The Changelog is a bit long anyways... Limit the number of lines to something reasonable.

                if (!File.Exists(GeneralAppInfo.HomePath + @"\CREDITS.md") && !File.Exists(GeneralAppInfo.HomePath + @"\CHANGELOG.md")) return;

                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                var backgroundColor = ColorTranslator.ToHtml(ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background"));
                var foregroundColor = ColorTranslator.ToHtml(ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground"));
                var css =
                    $@"<head><style>body{{font-family:arial,helvetica,sans-serif;font-size:12px;color:{foregroundColor};}}a:link,a:visited,a:hover,a:active{{text-decoration:none;background-color:{foregroundColor};color:{backgroundColor};}}</style></head>";

                var changelog = "";
                using (var sR = new StreamReader(GeneralAppInfo.HomePath + @"\CHANGELOG.md", Encoding.UTF8, true))
                {
                    string line;
                    var i = 0;
                    while ((line = sR.ReadLine()) != null)
                    {
                        changelog += line + Environment.NewLine;
                        i++;
                        if (i <= 128 || line != string.Empty) continue;
                        changelog +=
                            $"{Environment.NewLine}***See [CHANGELOG.md](https://github.com/mRemoteNG/mRemoteNG/blob/develop/CHANGELOG.md) for full History...***{Environment.NewLine}";
                        break;
                    }
                }
                var changelogHtml = Markdown.ToHtml(changelog, pipeline);
                changelogHtml = css + $"<body bgcolor=\"{backgroundColor}\">{changelogHtml}</body>";

                var credits = new StreamReader(GeneralAppInfo.HomePath + @"\CREDITS.md", Encoding.UTF8, true).ReadToEnd();
                var creditsHtml = Markdown.ToHtml(credits, pipeline);
                creditsHtml = css + $"<body bgcolor=\"{backgroundColor}\">{creditsHtml}</body>";

                gwbChangeLog.LoadHtml(changelogHtml.Replace("©", "&copy;"));
                gwbCredits.LoadHtml(creditsHtml.Replace("©", "&copy;"));
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "Loading About failed" + Environment.NewLine + ex.Message, true);
            }
        }

        private void LinkClicked(object sender, DomMouseEventArgs e)
        {
            Process.Start(((GeckoWebBrowser)sender).StatusText);
            e.Handled = true;
        }

        #endregion Form Stuff
    }
}