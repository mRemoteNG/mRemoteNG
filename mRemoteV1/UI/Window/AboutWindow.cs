using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using mRemoteNG.App;
using mRemoteNG.App.Info;

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
		internal LinkLabel llblFAMFAMFAM;
		internal LinkLabel llblMagicLibrary;
		internal LinkLabel llblWeifenLuo;
		internal Panel pnlTop;
				
		private void InitializeComponent()
		{
			pnlTop = new Panel();
			Load += new EventHandler(About_Load);
			lblEdition = new Label();
			pbLogo = new PictureBox();
			pnlBottom = new Panel();
			llblWeifenLuo = new LinkLabel();
			llblWeifenLuo.LinkClicked += new LinkLabelLinkClickedEventHandler(llblWeifenLuo_LinkClicked);
			llblMagicLibrary = new LinkLabel();
			llblMagicLibrary.LinkClicked += new LinkLabelLinkClickedEventHandler(llblMagicLibrary_LinkClicked);
			llblFAMFAMFAM = new LinkLabel();
			llblFAMFAMFAM.LinkClicked += new LinkLabelLinkClickedEventHandler(llblFAMFAMFAM_LinkClicked);
			txtChangeLog = new TextBox();
			lblTitle = new Label();
			lblVersion = new Label();
			lblChangeLog = new Label();
			lblLicense = new Label();
			lblCopyright = new Label();
			pnlTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) pbLogo).BeginInit();
			pnlBottom.SuspendLayout();
			SuspendLayout();
			//
			//pnlTop
			//
			pnlTop.Anchor = (AnchorStyles.Top | AnchorStyles.Left) 
			                | AnchorStyles.Right;
			pnlTop.BackColor = System.Drawing.Color.Black;
			pnlTop.Controls.Add(lblEdition);
			pnlTop.Controls.Add(pbLogo);
			pnlTop.ForeColor = System.Drawing.Color.White;
			pnlTop.Location = new System.Drawing.Point(-1, -1);
			pnlTop.Name = "pnlTop";
			pnlTop.Size = new System.Drawing.Size(788, 145);
			pnlTop.TabIndex = 0;
			//
			//lblEdition
			//
			lblEdition.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			lblEdition.BackColor = System.Drawing.Color.Black;
			lblEdition.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			lblEdition.ForeColor = System.Drawing.Color.White;
			lblEdition.Location = new System.Drawing.Point(512, 112);
			lblEdition.Name = "lblEdition";
			lblEdition.Size = new System.Drawing.Size(264, 24);
			lblEdition.TabIndex = 0;
			lblEdition.Text = "Edition";
			lblEdition.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			lblEdition.Visible = false;
			//
			//pbLogo
			//
			pbLogo.Image = Resources.Logo;
			pbLogo.Location = new System.Drawing.Point(8, 8);
			pbLogo.Name = "pbLogo";
			pbLogo.Size = new System.Drawing.Size(492, 128);
			pbLogo.SizeMode = PictureBoxSizeMode.AutoSize;
			pbLogo.TabIndex = 1;
			pbLogo.TabStop = false;
			//
			//pnlBottom
			//
			pnlBottom.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) 
			                    | AnchorStyles.Left) 
			                   | AnchorStyles.Right;
			pnlBottom.BackColor = System.Drawing.SystemColors.Control;
			pnlBottom.Controls.Add(llblWeifenLuo);
			pnlBottom.Controls.Add(llblMagicLibrary);
			pnlBottom.Controls.Add(llblFAMFAMFAM);
			pnlBottom.Controls.Add(txtChangeLog);
			pnlBottom.Controls.Add(lblTitle);
			pnlBottom.Controls.Add(lblVersion);
			pnlBottom.Controls.Add(lblChangeLog);
			pnlBottom.Controls.Add(lblLicense);
			pnlBottom.Controls.Add(lblCopyright);
			pnlBottom.ForeColor = System.Drawing.SystemColors.ControlText;
			pnlBottom.Location = new System.Drawing.Point(-1, 144);
			pnlBottom.Name = "pnlBottom";
			pnlBottom.Size = new System.Drawing.Size(788, 418);
			pnlBottom.TabIndex = 1;
			//
			//llblWeifenLuo
			//
			llblWeifenLuo.AutoSize = true;
			llblWeifenLuo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			llblWeifenLuo.ForeColor = System.Drawing.SystemColors.ControlText;
			llblWeifenLuo.LinkColor = System.Drawing.Color.Blue;
			llblWeifenLuo.Location = new System.Drawing.Point(16, 158);
			llblWeifenLuo.Name = "llblWeifenLuo";
			llblWeifenLuo.Size = new System.Drawing.Size(78, 22);
			llblWeifenLuo.TabIndex = 9;
			llblWeifenLuo.TabStop = true;
			llblWeifenLuo.Text = "WeifenLuo";
			llblWeifenLuo.UseCompatibleTextRendering = true;
			//
			//llblMagicLibrary
			//
			llblMagicLibrary.AutoSize = true;
			llblMagicLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			llblMagicLibrary.ForeColor = System.Drawing.SystemColors.ControlText;
			llblMagicLibrary.LinkColor = System.Drawing.Color.Blue;
			llblMagicLibrary.Location = new System.Drawing.Point(16, 136);
			llblMagicLibrary.Name = "llblMagicLibrary";
			llblMagicLibrary.Size = new System.Drawing.Size(92, 22);
			llblMagicLibrary.TabIndex = 8;
			llblMagicLibrary.TabStop = true;
			llblMagicLibrary.Text = "MagicLibrary";
			llblMagicLibrary.UseCompatibleTextRendering = true;
			//
			//llblFAMFAMFAM
			//
			llblFAMFAMFAM.AutoSize = true;
			llblFAMFAMFAM.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			llblFAMFAMFAM.ForeColor = System.Drawing.SystemColors.ControlText;
			llblFAMFAMFAM.LinkColor = System.Drawing.Color.Blue;
			llblFAMFAMFAM.Location = new System.Drawing.Point(16, 116);
			llblFAMFAMFAM.Name = "llblFAMFAMFAM";
			llblFAMFAMFAM.Size = new System.Drawing.Size(101, 22);
			llblFAMFAMFAM.TabIndex = 4;
			llblFAMFAMFAM.TabStop = true;
			llblFAMFAMFAM.Text = "FAMFAMFAM";
			llblFAMFAMFAM.UseCompatibleTextRendering = true;
			//
			//txtChangeLog
			//
			txtChangeLog.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) 
			                       | AnchorStyles.Left) 
			                      | AnchorStyles.Right;
			txtChangeLog.BackColor = System.Drawing.SystemColors.Control;
			txtChangeLog.BorderStyle = BorderStyle.None;
			txtChangeLog.Cursor = Cursors.Default;
			txtChangeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			txtChangeLog.ForeColor = System.Drawing.SystemColors.ControlText;
			txtChangeLog.Location = new System.Drawing.Point(24, 224);
			txtChangeLog.Multiline = true;
			txtChangeLog.Name = "txtChangeLog";
			txtChangeLog.ReadOnly = true;
			txtChangeLog.ScrollBars = ScrollBars.Vertical;
			txtChangeLog.Size = new System.Drawing.Size(760, 192);
			txtChangeLog.TabIndex = 7;
			txtChangeLog.TabStop = false;
			//
			//lblTitle
			//
			lblTitle.AutoSize = true;
			lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
			lblTitle.Location = new System.Drawing.Point(16, 16);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new System.Drawing.Size(122, 27);
			lblTitle.TabIndex = 0;
			lblTitle.Text = "mRemoteNG";
			lblTitle.UseCompatibleTextRendering = true;
			//
			//lblVersion
			//
			lblVersion.AutoSize = true;
			lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
			lblVersion.Location = new System.Drawing.Point(16, 56);
			lblVersion.Name = "lblVersion";
			lblVersion.Size = new System.Drawing.Size(57, 22);
			lblVersion.TabIndex = 1;
			lblVersion.Text = "Version";
			lblVersion.UseCompatibleTextRendering = true;
			//
			//lblChangeLog
			//
			lblChangeLog.AutoSize = true;
			lblChangeLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			lblChangeLog.ForeColor = System.Drawing.SystemColors.ControlText;
			lblChangeLog.Location = new System.Drawing.Point(16, 199);
			lblChangeLog.Name = "lblChangeLog";
			lblChangeLog.Size = new System.Drawing.Size(92, 22);
			lblChangeLog.TabIndex = 6;
			lblChangeLog.Text = "Change Log:";
			lblChangeLog.UseCompatibleTextRendering = true;
			//
			//lblLicense
			//
			lblLicense.AutoSize = true;
			lblLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			lblLicense.ForeColor = System.Drawing.SystemColors.ControlText;
			lblLicense.Location = new System.Drawing.Point(16, 96);
			lblLicense.Name = "lblLicense";
			lblLicense.Size = new System.Drawing.Size(58, 22);
			lblLicense.TabIndex = 5;
			lblLicense.Text = "License";
			lblLicense.UseCompatibleTextRendering = true;
			//
			//lblCopyright
			//
			lblCopyright.AutoSize = true;
			lblCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			lblCopyright.ForeColor = System.Drawing.SystemColors.ControlText;
			lblCopyright.Location = new System.Drawing.Point(16, 76);
			lblCopyright.Name = "lblCopyright";
			lblCopyright.Size = new System.Drawing.Size(70, 22);
			lblCopyright.TabIndex = 2;
			lblCopyright.Text = "Copyright";
			lblCopyright.UseCompatibleTextRendering = true;
			//
			//About
			//
			BackColor = System.Drawing.SystemColors.Control;
			ClientSize = new System.Drawing.Size(784, 564);
			Controls.Add(pnlTop);
			Controls.Add(pnlBottom);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			ForeColor = System.Drawing.SystemColors.ControlText;
			Icon = Resources.mRemote_Icon;
			MaximumSize = new System.Drawing.Size(20000, 10000);
			Name = "About";
			TabText = "About";
			Text = "About";
			pnlTop.ResumeLayout(false);
			pnlTop.PerformLayout();
			((System.ComponentModel.ISupportInitialize) pbLogo).EndInit();
			pnlBottom.ResumeLayout(false);
			pnlBottom.PerformLayout();
			ResumeLayout(false);
					
		}
        #endregion
				
        #region Public Methods
		public AboutWindow(DockContent Panel)
		{
			WindowType = WindowType.About;
			DockPnl = Panel;
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
		private void About_Load(object sender, EventArgs e)
		{
			ApplyLanguage();
			ApplyEditions();
					
			try
			{
				lblCopyright.Text = (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Copyright;
						
				lblVersion.Text = "Version " + (new Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase()).Info.Version.ToString();
						
				FillLinkLabel(llblFAMFAMFAM, Language.strFAMFAMFAMAttribution, Language.strFAMFAMFAMAttributionURL);
				FillLinkLabel(llblMagicLibrary, Language.strMagicLibraryAttribution, Language.strMagicLibraryAttributionURL);
				FillLinkLabel(llblWeifenLuo, Language.strWeifenLuoAttribution, Language.strWeifenLuoAttributionURL);
						
				if (File.Exists(GeneralAppInfo.HomePath + "\\CHANGELOG.TXT"))
				{
					StreamReader sR = new StreamReader(GeneralAppInfo.HomePath + "\\CHANGELOG.TXT");
					txtChangeLog.Text = sR.ReadToEnd();
					sR.Close();
				}
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "Loading About failed" + Environment.NewLine + ex.Message, true);
			}
		}
				
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
        #endregion
	}
}
