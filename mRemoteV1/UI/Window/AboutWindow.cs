using System;
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
            ApplyEditions();
            LoadDocuments();
        }

        #endregion

        #region Private Methods

        private void ApplyLanguage()
        {
            lblLicense.Text = Language.strLabelReleasedUnderGPL;
            TabText = Language.strAbout;
            Text = Language.strAbout;
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

        private void ApplyEditions()
        {
#if PORTABLE
            lblTitle.Text += $@" {Language.strLabelPortableEdition}";
#endif
        }

#if false
                private void FillLinkLabel(LinkLabel llbl, string txt, string URL)
		        {
			        llbl.Links.Clear();

			        int Open = txt.IndexOf("[");
			        while (Open != -1)
			        {
				        txt = txt.Remove(Open, 1);
				        int Close = txt.IndexOf("]", Open);
				        if (Close == -1)
				        {
					        break;
				        }
				        txt = txt.Remove(Close, 1);
				        llbl.Links.Add(Open, Close - Open, URL);
				        Open = txt.IndexOf("[", Open);
			        }

			        llbl.Text = txt;
		        }
#endif

        #endregion

        #region Form Stuff

        private void LoadDocuments()
        {
            try
            {
                lblCopyright.Text = GeneralAppInfo.Copyright;
                lblVersion.Text = $@"Version {GeneralAppInfo.ApplicationVersion}";
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                var backgroundColor = ColorTranslator.ToHtml(ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background"));

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
                            $"{Environment.NewLine}***See CHANGELOG.md for full History...***{Environment.NewLine}";
                        break;
                    }
                }
                var changelogHtml = Markdown.ToHtml(changelog, pipeline);
                changelogHtml = $"<body style=\"font-family:arial,helvetica,sans-serif;font-size:12px;\" bgcolor=\"{backgroundColor}\">{changelogHtml}</body>";

                var credits = new StreamReader(GeneralAppInfo.HomePath + @"\CREDITS.md", Encoding.UTF8, true).ReadToEnd();
                var creditsHtml = Markdown.ToHtml(credits, pipeline);
                creditsHtml = $"<body style=\"font-family:arial,helvetica,sans-serif;font-size:12px;\" bgcolor=\"{backgroundColor}\">{creditsHtml}</body>";

                gwbChangeLog.LoadHtml(changelogHtml);
                gwbCredits.LoadHtml(creditsHtml);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    "Loading About failed" + Environment.NewLine + ex.Message, true);
            }
        }

#if false
        private void llblFAMFAMFAM_LinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
		{
			Runtime.GoToURL(Language.strFAMFAMFAMAttributionURL);
		}

		private void llblMagicLibrary_LinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
		{
			Runtime.GoToURL(Language.strMagicLibraryAttributionURL);
		}

		private void llblWeifenLuo_LinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
		{
			Runtime.GoToURL(Language.strWeifenLuoAttributionURL);
		}
#endif

        #endregion
    }
}