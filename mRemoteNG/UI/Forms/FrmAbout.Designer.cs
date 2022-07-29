namespace mRemoteNG.UI.Forms
{
    public partial class frmAbout
    {
        #region  Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.llCredits = new System.Windows.Forms.LinkLabel();
            this.llChangelog = new System.Windows.Forms.LinkLabel();
            this.llLicense = new System.Windows.Forms.LinkLabel();
            this.lblTitle = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblVersion = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblLicense = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblCopyright = new mRemoteNG.UI.Controls.MrngLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbLogo
            // 
            this.pbLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.pbLogo.BackgroundImage = global::mRemoteNG.Properties.Resources.Header_dark;
            this.pbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(584, 120);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.llCredits);
            this.pnlBottom.Controls.Add(this.llChangelog);
            this.pnlBottom.Controls.Add(this.llLicense);
            this.pnlBottom.Controls.Add(this.lblTitle);
            this.pnlBottom.Controls.Add(this.lblVersion);
            this.pnlBottom.Controls.Add(this.lblLicense);
            this.pnlBottom.Controls.Add(this.lblCopyright);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlBottom.Location = new System.Drawing.Point(0, 120);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(584, 161);
            this.pnlBottom.TabIndex = 1;
            // 
            // llCredits
            // 
            this.llCredits.AutoSize = true;
            this.llCredits.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.llCredits.Location = new System.Drawing.Point(5, 134);
            this.llCredits.Name = "llCredits";
            this.llCredits.Size = new System.Drawing.Size(49, 17);
            this.llCredits.TabIndex = 10;
            this.llCredits.TabStop = true;
            this.llCredits.Text = "Credits";
            this.llCredits.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCredits_LinkClicked);
            // 
            // llChangelog
            // 
            this.llChangelog.AutoSize = true;
            this.llChangelog.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.llChangelog.Location = new System.Drawing.Point(5, 117);
            this.llChangelog.Name = "llChangelog";
            this.llChangelog.Size = new System.Drawing.Size(71, 17);
            this.llChangelog.TabIndex = 9;
            this.llChangelog.TabStop = true;
            this.llChangelog.Text = "Changelog";
            this.llChangelog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llChangelog_LinkClicked);
            // 
            // llLicense
            // 
            this.llLicense.AutoSize = true;
            this.llLicense.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.llLicense.Location = new System.Drawing.Point(5, 100);
            this.llLicense.Name = "llLicense";
            this.llLicense.Size = new System.Drawing.Size(50, 17);
            this.llLicense.TabIndex = 8;
            this.llLicense.TabStop = true;
            this.llLicense.Text = "License";
            this.llLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llLicense_LinkClicked);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTitle.Location = new System.Drawing.Point(6, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 27);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "mRemoteNG";
            this.lblTitle.UseCompatibleTextRendering = true;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVersion.Location = new System.Drawing.Point(6, 30);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(49, 22);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version";
            this.lblVersion.UseCompatibleTextRendering = true;
            // 
            // lblLicense
            // 
            this.lblLicense.AutoSize = true;
            this.lblLicense.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicense.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLicense.Location = new System.Drawing.Point(6, 74);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.Size = new System.Drawing.Size(48, 22);
            this.lblLicense.TabIndex = 5;
            this.lblLicense.Text = "License";
            this.lblLicense.UseCompatibleTextRendering = true;
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCopyright.Location = new System.Drawing.Point(6, 52);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(63, 22);
            this.lblCopyright.TabIndex = 2;
            this.lblCopyright.Text = "Copyright";
            this.lblCopyright.UseCompatibleTextRendering = true;
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(584, 281);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pbLogo);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(20000, 10000);
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        internal Controls.MrngLabel lblCopyright;
        internal Controls.MrngLabel lblTitle;
        internal Controls.MrngLabel lblVersion;
        internal Controls.MrngLabel lblLicense;
        internal System.Windows.Forms.Panel pnlBottom;
        internal System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.LinkLabel llCredits;
        private System.Windows.Forms.LinkLabel llChangelog;
        private System.Windows.Forms.LinkLabel llLicense;
    }
}
