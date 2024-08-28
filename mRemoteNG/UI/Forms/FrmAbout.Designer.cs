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
            pnlBottom.Size = new System.Drawing.Size(584, 161);
            pnlBottom.TabIndex = 1;
            // 
            // llCredits
            // 
            llCredits.AutoSize = true;
            llCredits.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            llChangelog.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            llLicense.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            lblTitle.Location = new System.Drawing.Point(6, 3);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(103, 27);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Libro Ultimo";
            lblTitle.UseCompatibleTextRendering = true;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            lblLicense.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            lblCopyright.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblCopyright.ForeColor = System.Drawing.SystemColors.ControlText;
            lblCopyright.Location = new System.Drawing.Point(6, 52);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new System.Drawing.Size(63, 22);
            lblCopyright.TabIndex = 2;
            lblCopyright.Text = "Copyright";
            lblCopyright.UseCompatibleTextRendering = true;
            // 
            // frmAbout
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(584, 281);
            Controls.Add(pnlBottom);
            Controls.Add(pbLogo);
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ForeColor = System.Drawing.SystemColors.ControlText;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(20000, 10000);
            MinimizeBox = false;
            Name = "frmAbout";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "About";
            TopMost = true;
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
    }
}
