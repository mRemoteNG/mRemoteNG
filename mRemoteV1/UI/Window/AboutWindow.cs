using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Window
{
    public class AboutWindow : BaseWindow
    {
        #region Form Init

		internal Controls.Base.NGLabel lblCopyright;
		internal Controls.Base.NGLabel lblTitle;
		internal Controls.Base.NGLabel lblVersion;
		internal Controls.Base.NGLabel lblLicense;
		internal Controls.Base.NGTextBox txtChangeLog;
		internal Panel pnlBottom;
		internal PictureBox pbLogo;
        internal Controls.Base.NGTextBox txtCredits;
        internal Panel pnlTop;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.txtCredits = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.txtChangeLog = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblTitle = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblVersion = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblLicense = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblCopyright = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor =
                System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.pnlTop.Controls.Add(this.pbLogo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.ForeColor = System.Drawing.Color.White;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1117, 122);
            this.pnlTop.TabIndex = 0;
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::mRemoteNG.Resources.Header_dark;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(450, 120);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.txtCredits);
            this.pnlBottom.Controls.Add(this.txtChangeLog);
            this.pnlBottom.Controls.Add(this.lblTitle);
            this.pnlBottom.Controls.Add(this.lblVersion);
            this.pnlBottom.Controls.Add(this.lblLicense);
            this.pnlBottom.Controls.Add(this.lblCopyright);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlBottom.Location = new System.Drawing.Point(0, 122);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1117, 583);
            this.pnlBottom.TabIndex = 1;
            // 
            // txtCredits
            // 
            this.txtCredits.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Bottom)
                                                    | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCredits.BackColor = System.Drawing.SystemColors.Control;
            this.txtCredits.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCredits.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtCredits.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular,
                                                           System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCredits.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtCredits.Location = new System.Drawing.Point(8, 131);
            this.txtCredits.MinimumSize = new System.Drawing.Size(370, 260);
            this.txtCredits.Multiline = true;
            this.txtCredits.Name = "txtCredits";
            this.txtCredits.ReadOnly = true;
            this.txtCredits.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCredits.Size = new System.Drawing.Size(400, 449);
            this.txtCredits.TabIndex = 7;
            this.txtCredits.TabStop = false;
            // 
            // txtChangeLog
            // 
            this.txtChangeLog.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom)
                                                     | System.Windows.Forms.AnchorStyles.Left)
                                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChangeLog.BackColor = System.Drawing.SystemColors.Control;
            this.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtChangeLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular,
                                                             System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChangeLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtChangeLog.Location = new System.Drawing.Point(414, 131);
            this.txtChangeLog.MinimumSize = new System.Drawing.Size(370, 260);
            this.txtChangeLog.Multiline = true;
            this.txtChangeLog.Name = "txtChangeLog";
            this.txtChangeLog.ReadOnly = true;
            this.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChangeLog.Size = new System.Drawing.Size(696, 449);
            this.txtChangeLog.TabIndex = 10;
            this.txtChangeLog.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold,
                                                         System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTitle.Location = new System.Drawing.Point(3, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(126, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "mRemoteNG";
            this.lblTitle.UseCompatibleTextRendering = true;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVersion.Location = new System.Drawing.Point(3, 34);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(55, 25);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version";
            this.lblVersion.UseCompatibleTextRendering = true;
            // 
            // lblLicense
            // 
            this.lblLicense.AutoSize = true;
            this.lblLicense.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblLicense.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLicense.Location = new System.Drawing.Point(3, 84);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.Size = new System.Drawing.Size(54, 25);
            this.lblLicense.TabIndex = 5;
            this.lblLicense.Text = "License";
            this.lblLicense.UseCompatibleTextRendering = true;
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCopyright.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCopyright.Location = new System.Drawing.Point(3, 59);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(71, 25);
            this.lblCopyright.TabIndex = 2;
            this.lblCopyright.Text = "Copyright";
            this.lblCopyright.UseCompatibleTextRendering = true;
            // 
            // AboutWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1117, 705);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(20000, 10000);
            this.Name = "AboutWindow";
            this.TabText = "About";
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        #region Public Methods

        public AboutWindow()
        {
            WindowType = WindowType.About;
            DockPnl = new DockContent();
            InitializeComponent();
            FontOverrider.FontOverride(this);
            ThemeManager.getInstance().ThemeChanged += ApplyTheme;
            ApplyLanguage();
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
            lblTitle.Text += " " + Language.strLabelPortableEdition;
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

        private void About_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            ApplyEditions();

            try
            {
                lblCopyright.Text = GeneralAppInfo.Copyright;

                lblVersion.Text = $@"Version {GeneralAppInfo.ApplicationVersion}";

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

                if (File.Exists(GeneralAppInfo.HomePath + "\\CHANGELOG.md"))
	            {
                    using (var sR = new StreamReader(GeneralAppInfo.HomePath + "\\CHANGELOG.md", Encoding.UTF8, true))
                    {
                        string line;
                        var i = 0;
                        while ((line = sR.ReadLine()) != null && i < 128)
                        {
                            txtChangeLog.Text += line + Environment.NewLine;
                            i++;
                        }

                        if (i == 128)
                        {
                            txtChangeLog.Text +=
                                $"{Environment.NewLine}****************************************{Environment.NewLine}See CHANGELOG.md for full History...{Environment.NewLine}****************************************{Environment.NewLine}";
                        }
                    }
                }

	            if (File.Exists(GeneralAppInfo.HomePath + "\\CREDITS.md"))
	            {
                    using (var sR = new StreamReader(GeneralAppInfo.HomePath + "\\CREDITS.md", Encoding.UTF8, true))
                    {
                        string line;
                        while ((line = sR.ReadLine()) != null)
                            txtCredits.Text += line + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
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