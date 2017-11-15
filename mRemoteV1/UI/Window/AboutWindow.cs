using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.App.Info;
// ReSharper disable ArrangeRedundantParentheses
// ReSharper disable RedundantCast

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
		internal PictureBox pbLogo;
		internal Controls.Base.NGLabel lblEdition;
        internal Controls.Base.NGLabel lblCredits;
        private TableLayoutPanel tblLayout;
        internal Controls.Base.NGLabel lblChangeLog;
        internal Controls.Base.NGTextBox txtCredits;
				
		private void InitializeComponent()
		{
            this.lblEdition = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblCredits = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtCredits = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.txtChangeLog = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblTitle = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblVersion = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblLicense = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblCopyright = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblChangeLog = new mRemoteNG.UI.Controls.Base.NGLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.tblLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEdition
            // 
            this.lblEdition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEdition.BackColor = System.Drawing.Color.Transparent;
            this.lblEdition.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEdition.ForeColor = System.Drawing.Color.Black;
            this.lblEdition.Location = new System.Drawing.Point(571, 76);
            this.lblEdition.Name = "lblEdition";
            this.lblEdition.Size = new System.Drawing.Size(264, 24);
            this.lblEdition.TabIndex = 0;
            this.lblEdition.Text = "Edition";
            this.lblEdition.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblEdition.Visible = false;
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.pbLogo.Image = global::mRemoteNG.Resources.Logo;
            this.pbLogo.Location = new System.Drawing.Point(3, 3);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(406, 94);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // lblCredits
            // 
            this.lblCredits.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCredits.AutoSize = true;
            this.lblCredits.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCredits.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCredits.Location = new System.Drawing.Point(3, 215);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(55, 25);
            this.lblCredits.TabIndex = 11;
            this.lblCredits.Text = "Credits:";
            this.lblCredits.UseCompatibleTextRendering = true;
            // 
            // txtCredits
            // 
            this.txtCredits.BackColor = System.Drawing.SystemColors.Control;
            this.txtCredits.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCredits.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtCredits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCredits.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCredits.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtCredits.Location = new System.Drawing.Point(3, 243);
            this.txtCredits.Multiline = true;
            this.txtCredits.Name = "txtCredits";
            this.txtCredits.ReadOnly = true;
            this.txtCredits.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCredits.Size = new System.Drawing.Size(413, 186);
            this.txtCredits.TabIndex = 7;
            this.txtCredits.TabStop = false;
            // 
            // txtChangeLog
            // 
            this.txtChangeLog.BackColor = System.Drawing.SystemColors.Control;
            this.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtChangeLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChangeLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChangeLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtChangeLog.Location = new System.Drawing.Point(422, 243);
            this.txtChangeLog.Multiline = true;
            this.txtChangeLog.Name = "txtChangeLog";
            this.txtChangeLog.ReadOnly = true;
            this.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChangeLog.Size = new System.Drawing.Size(413, 186);
            this.txtChangeLog.TabIndex = 10;
            this.txtChangeLog.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTitle.Location = new System.Drawing.Point(3, 104);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(126, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "mRemoteNG";
            this.lblTitle.UseCompatibleTextRendering = true;
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVersion.Location = new System.Drawing.Point(3, 140);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(55, 25);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version";
            this.lblVersion.UseCompatibleTextRendering = true;
            // 
            // lblLicense
            // 
            this.lblLicense.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLicense.AutoSize = true;
            this.lblLicense.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblLicense.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLicense.Location = new System.Drawing.Point(3, 190);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.Size = new System.Drawing.Size(54, 25);
            this.lblLicense.TabIndex = 5;
            this.lblLicense.Text = "License";
            this.lblLicense.UseCompatibleTextRendering = true;
            // 
            // lblCopyright
            // 
            this.lblCopyright.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCopyright.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCopyright.Location = new System.Drawing.Point(3, 165);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(71, 25);
            this.lblCopyright.TabIndex = 2;
            this.lblCopyright.Text = "Copyright";
            this.lblCopyright.UseCompatibleTextRendering = true;
            // 
            // tblLayout
            // 
            this.tblLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.tblLayout.ColumnCount = 2;
            this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblLayout.Controls.Add(this.txtChangeLog, 1, 6);
            this.tblLayout.Controls.Add(this.txtCredits, 0, 6);
            this.tblLayout.Controls.Add(this.lblEdition, 1, 0);
            this.tblLayout.Controls.Add(this.lblCredits, 0, 5);
            this.tblLayout.Controls.Add(this.lblVersion, 0, 2);
            this.tblLayout.Controls.Add(this.pbLogo, 0, 0);
            this.tblLayout.Controls.Add(this.lblTitle, 0, 1);
            this.tblLayout.Controls.Add(this.lblCopyright, 0, 3);
            this.tblLayout.Controls.Add(this.lblLicense, 0, 4);
            this.tblLayout.Controls.Add(this.lblChangeLog, 1, 5);
            this.tblLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayout.Location = new System.Drawing.Point(0, 0);
            this.tblLayout.Name = "tblLayout";
            this.tblLayout.RowCount = 7;
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayout.Size = new System.Drawing.Size(838, 383);
            this.tblLayout.TabIndex = 2;
            // 
            // lblChangeLog
            // 
            this.lblChangeLog.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblChangeLog.AutoSize = true;
            this.lblChangeLog.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblChangeLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblChangeLog.Location = new System.Drawing.Point(422, 215);
            this.lblChangeLog.Name = "lblChangeLog";
            this.lblChangeLog.Size = new System.Drawing.Size(89, 25);
            this.lblChangeLog.TabIndex = 11;
            this.lblChangeLog.Text = "Change Log:";
            this.lblChangeLog.UseCompatibleTextRendering = true;
            // 
            // AboutWindow
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(838, 383);
            this.Controls.Add(this.tblLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = global::mRemoteNG.Resources.mRemote_Icon;
            this.Name = "AboutWindow";
            this.TabText = "About";
            this.Text = "About";
            this.Load += new System.EventHandler(this.About_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.tblLayout.ResumeLayout(false);
            this.tblLayout.PerformLayout();
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
            Themes.ThemeManager.getInstance().ThemeChanged += ApplyTheme;
        }
        #endregion
				
        #region Private Methods
		private void ApplyLanguage()
		{
			lblLicense.Text = Language.strLabelReleasedUnderGPL;
			lblChangeLog.Text = Language.strLabelChangeLog;
			TabText = Language.strAbout;
			Text = Language.strAbout;
		}

        private new void ApplyTheme()
        {
            if (Themes.ThemeManager.getInstance().ThemingActive)
            {
                base.ApplyTheme(); 
                //pnlBottom.BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                //pnlBottom.ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
                //pnlTop.BackColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Background");
                //pnlTop.ForeColor = Themes.ThemeManager.getInstance().ActiveTheme.ExtendedPalette.getColor("Dialog_Foreground");
            }
        }

        private void ApplyEditions()
		{
            #if PORTABLE
			lblEdition.Text = Language.strLabelPortableEdition;
			lblEdition.Visible = true;
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
	        ApplyLanguage();
            ApplyTheme();
	        ApplyEditions();

	        try
	        {
	            lblCopyright.Text = GeneralAppInfo.Copyright;

	            lblVersion.Text = @"Version " + GeneralAppInfo.ApplicationVersion;

	            if (File.Exists(GeneralAppInfo.HomePath + "\\CHANGELOG.TXT"))
	            {
	                using (var sR = new StreamReader(GeneralAppInfo.HomePath + "\\CHANGELOG.TXT"))
	                    txtChangeLog.Text = sR.ReadToEnd();
	            }

	            if (File.Exists(GeneralAppInfo.HomePath + "\\CREDITS.TXT"))
	            {
	                using (var sR = new StreamReader(GeneralAppInfo.HomePath + "\\CREDITS.TXT", Encoding.Default, true))
	                    txtCredits.Text = sR.ReadToEnd();
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
