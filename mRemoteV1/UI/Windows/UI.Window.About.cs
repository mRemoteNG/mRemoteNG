using System;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using mRemoteNG.App;


namespace mRemoteNG.UI.Window
{
	public class About : Base
	{
        #region Form Init
		internal System.Windows.Forms.Label lblCopyright;
		internal System.Windows.Forms.Label lblTitle;
		internal System.Windows.Forms.Label lblVersion;
		internal System.Windows.Forms.Label lblLicense;
		internal System.Windows.Forms.TextBox txtChangeLog;
		internal System.Windows.Forms.Label lblChangeLog;
		internal System.Windows.Forms.Panel pnlBottom;
		internal System.Windows.Forms.PictureBox pbLogo;
		internal System.Windows.Forms.Label lblEdition;
		internal System.Windows.Forms.LinkLabel llblFAMFAMFAM;
		internal System.Windows.Forms.LinkLabel llblMagicLibrary;
		internal System.Windows.Forms.LinkLabel llblWeifenLuo;
		internal System.Windows.Forms.Panel pnlTop;
				
		private void InitializeComponent()
		{
			this.pnlTop = new System.Windows.Forms.Panel();
			this.Load += new System.EventHandler(About_Load);
			this.lblEdition = new System.Windows.Forms.Label();
			this.pbLogo = new System.Windows.Forms.PictureBox();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.llblWeifenLuo = new System.Windows.Forms.LinkLabel();
			this.llblWeifenLuo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblWeifenLuo_LinkClicked);
			this.llblMagicLibrary = new System.Windows.Forms.LinkLabel();
			this.llblMagicLibrary.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblMagicLibrary_LinkClicked);
			this.llblFAMFAMFAM = new System.Windows.Forms.LinkLabel();
			this.llblFAMFAMFAM.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblFAMFAMFAM_LinkClicked);
			this.txtChangeLog = new System.Windows.Forms.TextBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblChangeLog = new System.Windows.Forms.Label();
			this.lblLicense = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.pnlTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.pbLogo).BeginInit();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			//
			//pnlTop
			//
			this.pnlTop.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.pnlTop.BackColor = System.Drawing.Color.Black;
			this.pnlTop.Controls.Add(this.lblEdition);
			this.pnlTop.Controls.Add(this.pbLogo);
			this.pnlTop.ForeColor = System.Drawing.Color.White;
			this.pnlTop.Location = new System.Drawing.Point(-1, -1);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(788, 145);
			this.pnlTop.TabIndex = 0;
			//
			//lblEdition
			//
			this.lblEdition.Anchor = (System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.lblEdition.BackColor = System.Drawing.Color.Black;
			this.lblEdition.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (14.25F), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.lblEdition.ForeColor = System.Drawing.Color.White;
			this.lblEdition.Location = new System.Drawing.Point(512, 112);
			this.lblEdition.Name = "lblEdition";
			this.lblEdition.Size = new System.Drawing.Size(264, 24);
			this.lblEdition.TabIndex = 0;
			this.lblEdition.Text = "Edition";
			this.lblEdition.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.lblEdition.Visible = false;
			//
			//pbLogo
			//
			this.pbLogo.Image = My.Resources.Logo;
			this.pbLogo.Location = new System.Drawing.Point(8, 8);
			this.pbLogo.Name = "pbLogo";
			this.pbLogo.Size = new System.Drawing.Size(492, 128);
			this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pbLogo.TabIndex = 1;
			this.pbLogo.TabStop = false;
			//
			//pnlBottom
			//
			this.pnlBottom.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
			this.pnlBottom.Controls.Add(this.llblWeifenLuo);
			this.pnlBottom.Controls.Add(this.llblMagicLibrary);
			this.pnlBottom.Controls.Add(this.llblFAMFAMFAM);
			this.pnlBottom.Controls.Add(this.txtChangeLog);
			this.pnlBottom.Controls.Add(this.lblTitle);
			this.pnlBottom.Controls.Add(this.lblVersion);
			this.pnlBottom.Controls.Add(this.lblChangeLog);
			this.pnlBottom.Controls.Add(this.lblLicense);
			this.pnlBottom.Controls.Add(this.lblCopyright);
			this.pnlBottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this.pnlBottom.Location = new System.Drawing.Point(-1, 144);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(788, 418);
			this.pnlBottom.TabIndex = 1;
			//
			//llblWeifenLuo
			//
			this.llblWeifenLuo.AutoSize = true;
			this.llblWeifenLuo.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (11.0F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.llblWeifenLuo.ForeColor = System.Drawing.SystemColors.ControlText;
			this.llblWeifenLuo.LinkColor = System.Drawing.Color.Blue;
			this.llblWeifenLuo.Location = new System.Drawing.Point(16, 158);
			this.llblWeifenLuo.Name = "llblWeifenLuo";
			this.llblWeifenLuo.Size = new System.Drawing.Size(78, 22);
			this.llblWeifenLuo.TabIndex = 9;
			this.llblWeifenLuo.TabStop = true;
			this.llblWeifenLuo.Text = "WeifenLuo";
			this.llblWeifenLuo.UseCompatibleTextRendering = true;
			//
			//llblMagicLibrary
			//
			this.llblMagicLibrary.AutoSize = true;
			this.llblMagicLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (11.0F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.llblMagicLibrary.ForeColor = System.Drawing.SystemColors.ControlText;
			this.llblMagicLibrary.LinkColor = System.Drawing.Color.Blue;
			this.llblMagicLibrary.Location = new System.Drawing.Point(16, 136);
			this.llblMagicLibrary.Name = "llblMagicLibrary";
			this.llblMagicLibrary.Size = new System.Drawing.Size(92, 22);
			this.llblMagicLibrary.TabIndex = 8;
			this.llblMagicLibrary.TabStop = true;
			this.llblMagicLibrary.Text = "MagicLibrary";
			this.llblMagicLibrary.UseCompatibleTextRendering = true;
			//
			//llblFAMFAMFAM
			//
			this.llblFAMFAMFAM.AutoSize = true;
			this.llblFAMFAMFAM.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (11.0F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.llblFAMFAMFAM.ForeColor = System.Drawing.SystemColors.ControlText;
			this.llblFAMFAMFAM.LinkColor = System.Drawing.Color.Blue;
			this.llblFAMFAMFAM.Location = new System.Drawing.Point(16, 116);
			this.llblFAMFAMFAM.Name = "llblFAMFAMFAM";
			this.llblFAMFAMFAM.Size = new System.Drawing.Size(101, 22);
			this.llblFAMFAMFAM.TabIndex = 4;
			this.llblFAMFAMFAM.TabStop = true;
			this.llblFAMFAMFAM.Text = "FAMFAMFAM";
			this.llblFAMFAMFAM.UseCompatibleTextRendering = true;
			//
			//txtChangeLog
			//
			this.txtChangeLog.Anchor = (System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtChangeLog.BackColor = System.Drawing.SystemColors.Control;
			this.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Default;
			this.txtChangeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (9.0F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.txtChangeLog.ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtChangeLog.Location = new System.Drawing.Point(24, 224);
			this.txtChangeLog.Multiline = true;
			this.txtChangeLog.Name = "txtChangeLog";
			this.txtChangeLog.ReadOnly = true;
			this.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtChangeLog.Size = new System.Drawing.Size(760, 192);
			this.txtChangeLog.TabIndex = 7;
			this.txtChangeLog.TabStop = false;
			//
			//lblTitle
			//
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (14.0F), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblTitle.Location = new System.Drawing.Point(16, 16);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(122, 27);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "mRemoteNG";
			this.lblTitle.UseCompatibleTextRendering = true;
			//
			//lblVersion
			//
			this.lblVersion.AutoSize = true;
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (11.0F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblVersion.Location = new System.Drawing.Point(16, 56);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(57, 22);
			this.lblVersion.TabIndex = 1;
			this.lblVersion.Text = "Version";
			this.lblVersion.UseCompatibleTextRendering = true;
			//
			//lblChangeLog
			//
			this.lblChangeLog.AutoSize = true;
			this.lblChangeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (11.0F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.lblChangeLog.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblChangeLog.Location = new System.Drawing.Point(16, 199);
			this.lblChangeLog.Name = "lblChangeLog";
			this.lblChangeLog.Size = new System.Drawing.Size(92, 22);
			this.lblChangeLog.TabIndex = 6;
			this.lblChangeLog.Text = "Change Log:";
			this.lblChangeLog.UseCompatibleTextRendering = true;
			//
			//lblLicense
			//
			this.lblLicense.AutoSize = true;
			this.lblLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (11.0F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.lblLicense.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblLicense.Location = new System.Drawing.Point(16, 96);
			this.lblLicense.Name = "lblLicense";
			this.lblLicense.Size = new System.Drawing.Size(58, 22);
			this.lblLicense.TabIndex = 5;
			this.lblLicense.Text = "License";
			this.lblLicense.UseCompatibleTextRendering = true;
			//
			//lblCopyright
			//
			this.lblCopyright.AutoSize = true;
			this.lblCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (11.0F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.lblCopyright.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCopyright.Location = new System.Drawing.Point(16, 76);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new System.Drawing.Size(70, 22);
			this.lblCopyright.TabIndex = 2;
			this.lblCopyright.Text = "Copyright";
			this.lblCopyright.UseCompatibleTextRendering = true;
			//
			//About
			//
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(784, 564);
			this.Controls.Add(this.pnlTop);
			this.Controls.Add(this.pnlBottom);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", (float) (8.25F), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Icon = My.Resources.mRemote_Icon;
			this.MaximumSize = new System.Drawing.Size(20000, 10000);
			this.Name = "About";
			this.TabText = "About";
			this.Text = "About";
			this.pnlTop.ResumeLayout(false);
			this.pnlTop.PerformLayout();
			((System.ComponentModel.ISupportInitialize) this.pbLogo).EndInit();
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);
					
		}
        #endregion
				
        #region Public Methods
		public About(DockContent Panel)
		{
			this.WindowType = Type.About;
			this.DockPnl = Panel;
			this.InitializeComponent();
			App.Runtime.FontOverride(this);
		}
        #endregion
				
        #region Private Methods
		private void ApplyLanguage()
		{
			lblLicense.Text = My.Language.strLabelReleasedUnderGPL;
			lblChangeLog.Text = My.Language.strLabelChangeLog;
			TabText = My.Language.strAbout;
			Text = My.Language.strAbout;
		}
				
		private void ApplyEditions()
		{
            #if PORTABLE
			lblEdition.Text = My.Language.strLabelPortableEdition;
			lblEdition.Visible = true;
            #endif
		}
				
		private void FillLinkLabel(LinkLabel llbl, string Text, string URL)
		{
			llbl.Links.Clear();
					
			int Open = Text.IndexOf("[");
			int Close = 0;
			while (Open != -1)
			{
				Text = Text.Remove(Open, 1);
				Close = Text.IndexOf("]", Open);
				if (Close == -1)
				{
					break;
				}
				Text = Text.Remove(Close, 1);
				llbl.Links.Add(Open, Close - Open, URL);
				Open = Text.IndexOf("[", Open);
			}
					
			llbl.Text = Text;
		}
        #endregion
				
        #region Form Stuff
		private void About_Load(object sender, System.EventArgs e)
		{
			ApplyLanguage();
			ApplyEditions();
					
			try
			{
				lblCopyright.Text = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Copyright;
						
				this.lblVersion.Text = "Version " + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Version.ToString();
						
				FillLinkLabel(llblFAMFAMFAM, My.Language.strFAMFAMFAMAttribution, My.Language.strFAMFAMFAMAttributionURL);
				FillLinkLabel(llblMagicLibrary, My.Language.strMagicLibraryAttribution, My.Language.strMagicLibraryAttributionURL);
				FillLinkLabel(llblWeifenLuo, My.Language.strWeifenLuoAttribution, My.Language.strWeifenLuoAttributionURL);
						
				if (File.Exists((new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\CHANGELOG.TXT"))
				{
					StreamReader sR = new StreamReader((new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.DirectoryPath + "\\CHANGELOG.TXT");
					this.txtChangeLog.Text = sR.ReadToEnd();
					sR.Close();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Loading About failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
		private void llblFAMFAMFAM_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			App.Runtime.GoToURL(My.Language.strFAMFAMFAMAttributionURL);
		}
				
		private void llblMagicLibrary_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			App.Runtime.GoToURL(My.Language.strMagicLibraryAttributionURL);
		}
				
		private void llblWeifenLuo_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			App.Runtime.GoToURL(My.Language.strWeifenLuoAttributionURL);
		}
        #endregion
	}
}
