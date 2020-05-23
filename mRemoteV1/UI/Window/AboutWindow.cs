using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using mRemoteNG.Themes;
using Markdig;
using WeifenLuo.WinFormsUI.Docking;
using CefSharp;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using mRemoteNG.Connection.Protocol.Http;

namespace mRemoteNG.UI.Window
{
    public partial class AboutWindow : BaseWindow
    {
        public AboutWindow()
        {
            WindowType = WindowType.About;
            DockPnl = new DockContent();
            InitializeComponent();
            FontOverrider.FontOverride(this);
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
            ApplyTheme();
            LoadDocuments();
        }

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

        private void LoadDocuments()
        {
            try
            {
                var creditsPath = GeneralAppInfo.HomePath + @"\CREDITS.md";
                var changelogPath = GeneralAppInfo.HomePath + @"\CHANGELOG.md";
                if (!File.Exists(creditsPath) && !File.Exists(changelogPath)) return;

                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                var backgroundColor = ColorTranslator.ToHtml(ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background"));
                var foregroundColor = ColorTranslator.ToHtml(ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground"));
                var css = $@"<head><style>body{{font-family:arial,helvetica,sans-serif;font-size:12px;color:" +
                    $@"{foregroundColor};}}a:link,a:visited,a:hover,a:active{{text-decoration:none;" +
                    $@"background-color:{foregroundColor};color:{backgroundColor};}}</style></head>";

                // The Changelog is a bit long anyways... Limit the number of lines to something reasonable
                var changelog = "";
                using (var sR = new StreamReader(changelogPath, Encoding.UTF8, true))
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
                changelogHtml = $"<html>{css}<body bgcolor=\"{backgroundColor}\">{changelogHtml}</body></html>";

                var credits = new StreamReader(creditsPath, Encoding.UTF8, true).ReadToEnd();
                var creditsHtml = Markdown.ToHtml(credits, pipeline);
                creditsHtml = $"<html>{css}<body bgcolor=\"{backgroundColor}\">{creditsHtml}</body></html>";

                cwbChangeLog.RequestHandler = new RequestHandler();
                cwbCredits.RequestHandler = new RequestHandler();

                cwbChangeLog.LoadHtml(changelogHtml);
                cwbCredits.Load(creditsHtml);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "Loading About failed" + Environment.NewLine + ex.Message, true);
            }
        }
    }
}