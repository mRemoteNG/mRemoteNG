using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.App.Info;
using mRemoteNG.Messages;
using WeifenLuo.WinFormsUI.Docking;

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
        internal Panel pnlTop;

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblEdition = new Label();
            pbLogo = new PictureBox();
            pnlBottom = new Panel();
            lblCredits = new Label();
            txtCredits = new TextBox();
            txtChangeLog = new TextBox();
            lblTitle = new Label();
            lblVersion = new Label();
            lblChangeLog = new Label();
            lblLicense = new Label();
            lblCopyright = new Label();
            pnlTop.SuspendLayout();
            ((ISupportInitialize) pbLogo).BeginInit();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Anchor = AnchorStyles.Top | AnchorStyles.Left
                            | AnchorStyles.Right;
            pnlTop.BackColor = Color.Black;
            pnlTop.Controls.Add(lblEdition);
            pnlTop.Controls.Add(pbLogo);
            pnlTop.ForeColor = Color.White;
            pnlTop.Location = new Point(-1, -1);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(988, 145);
            pnlTop.TabIndex = 0;
            // 
            // lblEdition
            // 
            lblEdition.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblEdition.BackColor = Color.Black;
            lblEdition.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEdition.ForeColor = Color.White;
            lblEdition.Location = new Point(712, 112);
            lblEdition.Name = "lblEdition";
            lblEdition.Size = new Size(264, 24);
            lblEdition.TabIndex = 0;
            lblEdition.Text = "Edition";
            lblEdition.TextAlign = ContentAlignment.BottomRight;
            lblEdition.Visible = false;
            // 
            // pbLogo
            // 
            pbLogo.Image = Resources.Logo;
            pbLogo.Location = new Point(8, 8);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(492, 128);
            pbLogo.SizeMode = PictureBoxSizeMode.AutoSize;
            pbLogo.TabIndex = 1;
            pbLogo.TabStop = false;
            // 
            // pnlBottom
            // 
            pnlBottom.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                               | AnchorStyles.Left
                               | AnchorStyles.Right;
            pnlBottom.BackColor = SystemColors.Control;
            pnlBottom.Controls.Add(lblCredits);
            pnlBottom.Controls.Add(txtCredits);
            pnlBottom.Controls.Add(txtChangeLog);
            pnlBottom.Controls.Add(lblTitle);
            pnlBottom.Controls.Add(lblVersion);
            pnlBottom.Controls.Add(lblChangeLog);
            pnlBottom.Controls.Add(lblLicense);
            pnlBottom.Controls.Add(lblCopyright);
            pnlBottom.ForeColor = SystemColors.ControlText;
            pnlBottom.Location = new Point(-1, 144);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(988, 559);
            pnlBottom.TabIndex = 1;
            // 
            // lblCredits
            // 
            lblCredits.AutoSize = true;
            lblCredits.Font = new Font("Segoe UI", 11F);
            lblCredits.ForeColor = SystemColors.ControlText;
            lblCredits.Location = new Point(16, 131);
            lblCredits.Name = "lblCredits";
            lblCredits.Size = new Size(55, 25);
            lblCredits.TabIndex = 11;
            lblCredits.Text = "Credits:";
            lblCredits.UseCompatibleTextRendering = true;
            // 
            // txtCredits
            // 
            txtCredits.BackColor = SystemColors.Control;
            txtCredits.BorderStyle = BorderStyle.None;
            txtCredits.Cursor = Cursors.Default;
            txtCredits.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCredits.ForeColor = SystemColors.ControlText;
            txtCredits.Location = new Point(8, 156);
            txtCredits.MinimumSize = new Size(370, 260);
            txtCredits.Multiline = true;
            txtCredits.Name = "txtCredits";
            txtCredits.ReadOnly = true;
            txtCredits.ScrollBars = ScrollBars.Vertical;
            txtCredits.Size = new Size(485, 400);
            txtCredits.TabIndex = 7;
            txtCredits.TabStop = false;
            // 
            // txtChangeLog
            // 
            txtChangeLog.BackColor = SystemColors.Control;
            txtChangeLog.BorderStyle = BorderStyle.None;
            txtChangeLog.Cursor = Cursors.Default;
            txtChangeLog.Font = new Font("Segoe UI", 9F);
            txtChangeLog.ForeColor = SystemColors.ControlText;
            txtChangeLog.Location = new Point(501, 156);
            txtChangeLog.MinimumSize = new Size(370, 260);
            txtChangeLog.Multiline = true;
            txtChangeLog.Name = "txtChangeLog";
            txtChangeLog.ReadOnly = true;
            txtChangeLog.ScrollBars = ScrollBars.Vertical;
            txtChangeLog.Size = new Size(485, 400);
            txtChangeLog.TabIndex = 10;
            txtChangeLog.TabStop = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = SystemColors.ControlText;
            lblTitle.Location = new Point(16, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(126, 31);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "mRemoteNG";
            lblTitle.UseCompatibleTextRendering = true;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 11F);
            lblVersion.ForeColor = SystemColors.ControlText;
            lblVersion.Location = new Point(16, 56);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(55, 25);
            lblVersion.TabIndex = 1;
            lblVersion.Text = "Version";
            lblVersion.UseCompatibleTextRendering = true;
            // 
            // lblChangeLog
            // 
            lblChangeLog.AutoSize = true;
            lblChangeLog.Font = new Font("Segoe UI", 11F);
            lblChangeLog.ForeColor = SystemColors.ControlText;
            lblChangeLog.Location = new Point(501, 131);
            lblChangeLog.Name = "lblChangeLog";
            lblChangeLog.Size = new Size(89, 25);
            lblChangeLog.TabIndex = 6;
            lblChangeLog.Text = "Change Log:";
            lblChangeLog.UseCompatibleTextRendering = true;
            // 
            // lblLicense
            // 
            lblLicense.AutoSize = true;
            lblLicense.Font = new Font("Segoe UI", 11F);
            lblLicense.ForeColor = SystemColors.ControlText;
            lblLicense.Location = new Point(16, 96);
            lblLicense.Name = "lblLicense";
            lblLicense.Size = new Size(54, 25);
            lblLicense.TabIndex = 5;
            lblLicense.Text = "License";
            lblLicense.UseCompatibleTextRendering = true;
            // 
            // lblCopyright
            // 
            lblCopyright.AutoSize = true;
            lblCopyright.Font = new Font("Segoe UI", 11F);
            lblCopyright.ForeColor = SystemColors.ControlText;
            lblCopyright.Location = new Point(16, 76);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new Size(71, 25);
            lblCopyright.TabIndex = 2;
            lblCopyright.Text = "Copyright";
            lblCopyright.UseCompatibleTextRendering = true;
            // 
            // AboutWindow
            // 
            BackColor = SystemColors.Control;
            ClientSize = new Size(984, 705);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.ControlText;
            Icon = Resources.mRemote_Icon;
            MaximumSize = new Size(20000, 10000);
            Name = "AboutWindow";
            TabText = "About";
            Text = "About";
            Load += About_Load;
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ((ISupportInitialize) pbLogo).EndInit();
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

        private void FillLinkLabel(LinkLabel llbl, string txt, string URL)
        {
            llbl.Links.Clear();

            var Open = txt.IndexOf("[");
            while (Open != -1)
            {
                txt = txt.Remove(Open, 1);
                var Close = txt.IndexOf("]", Open);
                if (Close == -1)
                    break;
                txt = txt.Remove(Close, 1);
                llbl.Links.Add(Open, Close - Open, URL);
                Open = txt.IndexOf("[", Open);
            }

            llbl.Text = txt;
        }

        #endregion

        #region Form Stuff

        private void About_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            ApplyEditions();

            try
            {
                lblCopyright.Text = GeneralAppInfo.Copyright;

                lblVersion.Text = @"Version " + GeneralAppInfo.ApplicationVersion;

                if (File.Exists(GeneralAppInfo.HomePath + "\\CHANGELOG.TXT"))
                {
                    var sR = new StreamReader(GeneralAppInfo.HomePath + "\\CHANGELOG.TXT");
                    txtChangeLog.Text = sR.ReadToEnd();
                    sR.Close();
                }

                if (File.Exists(GeneralAppInfo.HomePath + "\\CREDITS.TXT"))
                {
                    var sR = new StreamReader(GeneralAppInfo.HomePath + "\\CREDITS.TXT", Encoding.Default, true);
                    txtCredits.Text = sR.ReadToEnd();
                    sR.Close();
                }
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