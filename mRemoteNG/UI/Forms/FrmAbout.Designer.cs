namespace mRemoteNG.UI.Forms
{
    public partial class frmAbout
    {
        #region  Windows Form Designer generated code
        private void InitializeComponent()
        {
            pbLogo = new System.Windows.Forms.PictureBox();
            pnlBottom = new System.Windows.Forms.Panel();
            llCredits = new System.Windows.Forms.LinkLabel();
            llChangelog = new System.Windows.Forms.LinkLabel();
            llLicense = new System.Windows.Forms.LinkLabel();
            lblTitle = new Controls.MrngLabel();
            lblVersion = new Controls.MrngLabel();
            lblLicense = new Controls.MrngLabel();
            lblCopyright = new Controls.MrngLabel();
            lblForkHeader = new Controls.MrngLabel();
            llForkGitHub = new System.Windows.Forms.LinkLabel();
            llForkReleases = new System.Windows.Forms.LinkLabel();
            llForkChangelog = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            //
            // pbLogo
            //
            pbLogo.BackColor = System.Drawing.Color.FromArgb(52, 58, 64);
            pbLogo.BackgroundImage = Properties.Resources.Header_dark;
            pbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pbLogo.Dock = System.Windows.Forms.DockStyle.Top;
            pbLogo.Location = new System.Drawing.Point(0, 0);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new System.Drawing.Size(584, 120);
            pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pbLogo.TabIndex = 1;
            pbLogo.TabStop = false;
            //
            // pnlBottom
            //
            pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            pnlBottom.Controls.Add(llForkChangelog);
            pnlBottom.Controls.Add(llForkReleases);
            pnlBottom.Controls.Add(llForkGitHub);
            pnlBottom.Controls.Add(lblForkHeader);
            pnlBottom.Controls.Add(llCredits);
            pnlBottom.Controls.Add(llChangelog);
            pnlBottom.Controls.Add(llLicense);
            pnlBottom.Controls.Add(lblTitle);
            pnlBottom.Controls.Add(lblVersion);
            pnlBottom.Controls.Add(lblLicense);
            pnlBottom.Controls.Add(lblCopyright);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlBottom.ForeColor = System.Drawing.SystemColors.ControlText;
            pnlBottom.Location = new System.Drawing.Point(0, 120);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(584, 220);
            pnlBottom.TabIndex = 1;
            //
            // llCredits
            //
            llCredits.AutoSize = true;
            llCredits.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            llCredits.Location = new System.Drawing.Point(5, 134);
            llCredits.Name = "llCredits";
            llCredits.Size = new System.Drawing.Size(49, 17);
            llCredits.TabIndex = 10;
            llCredits.TabStop = true;
            llCredits.Text = "Credits";
            llCredits.LinkClicked += llCredits_LinkClicked;
            //
            // llChangelog
            //
            llChangelog.AutoSize = true;
            llChangelog.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            llChangelog.Location = new System.Drawing.Point(5, 117);
            llChangelog.Name = "llChangelog";
            llChangelog.Size = new System.Drawing.Size(71, 17);
            llChangelog.TabIndex = 9;
            llChangelog.TabStop = true;
            llChangelog.Text = "Changelog";
            llChangelog.LinkClicked += llChangelog_LinkClicked;
            //
            // llLicense
            //
            llLicense.AutoSize = true;
            llLicense.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            llLicense.Location = new System.Drawing.Point(5, 100);
            llLicense.Name = "llLicense";
            llLicense.Size = new System.Drawing.Size(50, 17);
            llLicense.TabIndex = 8;
            llLicense.TabStop = true;
            llLicense.Text = "License";
            llLicense.LinkClicked += llLicense_LinkClicked;
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            lblTitle.Location = new System.Drawing.Point(6, 3);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(149, 27);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Fructus temporum";
            lblTitle.UseCompatibleTextRendering = true;
            //
            // lblVersion
            //
            lblVersion.AutoSize = true;
            lblVersion.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            lblVersion.Location = new System.Drawing.Point(6, 30);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new System.Drawing.Size(49, 22);
            lblVersion.TabIndex = 1;
            lblVersion.Text = "Version";
            lblVersion.UseCompatibleTextRendering = true;
            //
            // lblLicense
            //
            lblLicense.AutoSize = true;
            lblLicense.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            lblLicense.ForeColor = System.Drawing.SystemColors.ControlText;
            lblLicense.Location = new System.Drawing.Point(6, 74);
            lblLicense.Name = "lblLicense";
            lblLicense.Size = new System.Drawing.Size(48, 22);
            lblLicense.TabIndex = 5;
            lblLicense.Text = "License";
            lblLicense.UseCompatibleTextRendering = true;
            //
            // lblCopyright
            //
            lblCopyright.AutoSize = true;
            lblCopyright.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            lblCopyright.ForeColor = System.Drawing.SystemColors.ControlText;
            lblCopyright.Location = new System.Drawing.Point(6, 52);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new System.Drawing.Size(63, 22);
            lblCopyright.TabIndex = 2;
            lblCopyright.Text = "Copyright";
            lblCopyright.UseCompatibleTextRendering = true;
            //
            // lblForkHeader
            //
            lblForkHeader.AutoSize = true;
            lblForkHeader.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            lblForkHeader.ForeColor = System.Drawing.SystemColors.ControlText;
            lblForkHeader.Location = new System.Drawing.Point(5, 160);
            lblForkHeader.Name = "lblForkHeader";
            lblForkHeader.Size = new System.Drawing.Size(100, 17);
            lblForkHeader.TabIndex = 11;
            lblForkHeader.Text = "This Fork";
            //
            // llForkGitHub
            //
            llForkGitHub.AutoSize = true;
            llForkGitHub.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            llForkGitHub.Location = new System.Drawing.Point(5, 179);
            llForkGitHub.Name = "llForkGitHub";
            llForkGitHub.Size = new System.Drawing.Size(75, 17);
            llForkGitHub.TabIndex = 12;
            llForkGitHub.TabStop = true;
            llForkGitHub.Text = "GitHub Page";
            llForkGitHub.LinkClicked += llForkGitHub_LinkClicked;
            //
            // llForkReleases
            //
            llForkReleases.AutoSize = true;
            llForkReleases.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            llForkReleases.Location = new System.Drawing.Point(85, 179);
            llForkReleases.Name = "llForkReleases";
            llForkReleases.Size = new System.Drawing.Size(56, 17);
            llForkReleases.TabIndex = 13;
            llForkReleases.TabStop = true;
            llForkReleases.Text = "Releases";
            llForkReleases.LinkClicked += llForkReleases_LinkClicked;
            //
            // llForkChangelog
            //
            llForkChangelog.AutoSize = true;
            llForkChangelog.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            llForkChangelog.Location = new System.Drawing.Point(150, 179);
            llForkChangelog.Name = "llForkChangelog";
            llForkChangelog.Size = new System.Drawing.Size(71, 17);
            llForkChangelog.TabIndex = 14;
            llForkChangelog.TabStop = true;
            llForkChangelog.Text = "Changelog";
            llForkChangelog.LinkClicked += llForkChangelog_LinkClicked;
            //
            // frmAbout
            //
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(584, 340);
            Controls.Add(pnlBottom);
            Controls.Add(pbLogo);
            Font = new System.Drawing.Font("Segoe UI", 8.25F);
            ForeColor = System.Drawing.SystemColors.ControlText;
            Name = "frmAbout";
            Text = "About";
            TabText = "About";
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private Controls.MrngLabel lblForkHeader;
        private System.Windows.Forms.LinkLabel llForkGitHub;
        private System.Windows.Forms.LinkLabel llForkReleases;
        private System.Windows.Forms.LinkLabel llForkChangelog;
    }
}
