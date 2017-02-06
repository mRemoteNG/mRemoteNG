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
		internal Label lblCopyright;
		internal Label lblTitle;
		internal Label lblVersion;
		internal Label lblLicense;
		internal TextBox txtChangeLog;
		internal Label lblChangeLog;
		internal Panel pnlBottom;
		internal PictureBox pbLogo;
		internal Label lblEdition;
        internal Label lblCredits;
        internal TextBox txtCredits;
        private TextBox verText;
        internal Panel pnlTop;
				
		private void InitializeComponent()
		{
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblEdition = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblCredits = new System.Windows.Forms.Label();
            this.txtCredits = new System.Windows.Forms.TextBox();
            this.txtChangeLog = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblChangeLog = new System.Windows.Forms.Label();
            this.lblLicense = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.verText = new System.Windows.Forms.TextBox();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTop.BackColor = System.Drawing.Color.Black;
            this.pnlTop.Controls.Add(this.lblEdition);
            this.pnlTop.Controls.Add(this.pbLogo);
            this.pnlTop.ForeColor = System.Drawing.Color.White;
            this.pnlTop.Location = new System.Drawing.Point(-1, -1);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1121, 145);
            this.pnlTop.TabIndex = 0;
            // 
            // lblEdition
            // 
            this.lblEdition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEdition.BackColor = System.Drawing.Color.Black;
            this.lblEdition.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEdition.ForeColor = System.Drawing.Color.White;
            this.lblEdition.Location = new System.Drawing.Point(845, 112);
            this.lblEdition.Name = "lblEdition";
            this.lblEdition.Size = new System.Drawing.Size(264, 24);
            this.lblEdition.TabIndex = 0;
            this.lblEdition.Text = "Edition";
            this.lblEdition.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblEdition.Visible = false;
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::mRemoteNG.Resources.Logo;
            this.pbLogo.Location = new System.Drawing.Point(8, 8);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(492, 128);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.verText);
            this.pnlBottom.Controls.Add(this.lblCredits);
            this.pnlBottom.Controls.Add(this.txtCredits);
            this.pnlBottom.Controls.Add(this.txtChangeLog);
            this.pnlBottom.Controls.Add(this.lblTitle);
            this.pnlBottom.Controls.Add(this.lblVersion);
            this.pnlBottom.Controls.Add(this.lblChangeLog);
            this.pnlBottom.Controls.Add(this.lblLicense);
            this.pnlBottom.Controls.Add(this.lblCopyright);
            this.pnlBottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlBottom.Location = new System.Drawing.Point(-1, 144);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1121, 559);
            this.pnlBottom.TabIndex = 1;
            // 
            // lblCredits
            // 
            this.lblCredits.AutoSize = true;
            this.lblCredits.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblCredits.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCredits.Location = new System.Drawing.Point(8, 131);
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
            this.txtCredits.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCredits.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtCredits.Location = new System.Drawing.Point(8, 156);
            this.txtCredits.MinimumSize = new System.Drawing.Size(370, 260);
            this.txtCredits.Multiline = true;
            this.txtCredits.Name = "txtCredits";
            this.txtCredits.ReadOnly = true;
            this.txtCredits.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCredits.Size = new System.Drawing.Size(400, 400);
            this.txtCredits.TabIndex = 7;
            this.txtCredits.TabStop = false;
            // 
            // txtChangeLog
            // 
            this.txtChangeLog.BackColor = System.Drawing.SystemColors.Control;
            this.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtChangeLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChangeLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtChangeLog.Location = new System.Drawing.Point(414, 156);
            this.txtChangeLog.MinimumSize = new System.Drawing.Size(370, 260);
            this.txtChangeLog.Multiline = true;
            this.txtChangeLog.Name = "txtChangeLog";
            this.txtChangeLog.ReadOnly = true;
            this.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChangeLog.Size = new System.Drawing.Size(700, 400);
            this.txtChangeLog.TabIndex = 10;
            this.txtChangeLog.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTitle.Location = new System.Drawing.Point(8, 20);
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
            this.lblVersion.Location = new System.Drawing.Point(8, 51);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(55, 25);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version";
            this.lblVersion.UseCompatibleTextRendering = true;
            // 
            // lblChangeLog
            // 
            this.lblChangeLog.AutoSize = true;
            this.lblChangeLog.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblChangeLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblChangeLog.Location = new System.Drawing.Point(414, 131);
            this.lblChangeLog.Name = "lblChangeLog";
            this.lblChangeLog.Size = new System.Drawing.Size(89, 25);
            this.lblChangeLog.TabIndex = 6;
            this.lblChangeLog.Text = "Change Log:";
            this.lblChangeLog.UseCompatibleTextRendering = true;
            // 
            // lblLicense
            // 
            this.lblLicense.AutoSize = true;
            this.lblLicense.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblLicense.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLicense.Location = new System.Drawing.Point(8, 101);
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
            this.lblCopyright.Location = new System.Drawing.Point(8, 76);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(71, 25);
            this.lblCopyright.TabIndex = 2;
            this.lblCopyright.Text = "Copyright";
            this.lblCopyright.UseCompatibleTextRendering = true;
            // 
            // verText
            // 
            this.verText.BackColor = System.Drawing.SystemColors.Control;
            this.verText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.verText.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verText.Location = new System.Drawing.Point(69, 51);
            this.verText.Name = "verText";
            this.verText.Size = new System.Drawing.Size(147, 20);
            this.verText.TabIndex = 12;
            this.verText.TabStop = false;
            this.verText.Text = "w.x.y.z";
            // 
            // AboutWindow
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1117, 705);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = global::mRemoteNG.Resources.mRemote_Icon;
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
			Runtime.FontOverride(this);
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
	        ApplyEditions();

	        try
	        {
	            lblCopyright.Text = GeneralAppInfo.Copyright;

	            lblVersion.Text = @"Version ";
	            verText.Text = GeneralAppInfo.ApplicationVersion;

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
