namespace mRemoteNG.UI.Window
{
    public partial class AboutWindow : BaseWindow
    {
        #region  Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblTitle = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblVersion = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblLicense = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblCopyright = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.gwbCredits = new Gecko.GeckoWebBrowser();
            this.gwbChangeLog = new Gecko.GeckoWebBrowser();
            this.tlpTop = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.tlpBottom.SuspendLayout();
            this.tlpTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.pnlTop.Controls.Add(this.pbLogo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.ForeColor = System.Drawing.Color.White;
            this.pnlTop.Location = new System.Drawing.Point(3, 3);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1111, 116);
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
            this.pnlBottom.Controls.Add(this.lblTitle);
            this.pnlBottom.Controls.Add(this.lblVersion);
            this.pnlBottom.Controls.Add(this.lblLicense);
            this.pnlBottom.Controls.Add(this.lblCopyright);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlBottom.Location = new System.Drawing.Point(3, 125);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1111, 194);
            this.pnlBottom.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 2;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBottom.Controls.Add(this.gwbCredits, 0, 0);
            this.tlpBottom.Controls.Add(this.gwbChangeLog, 1, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(0, 235);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBottom.Size = new System.Drawing.Size(1117, 470);
            this.tlpBottom.TabIndex = 13;
            // 
            // gwbCredits
            // 
            this.gwbCredits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gwbCredits.FrameEventsPropagateToMainWindow = false;
            this.gwbCredits.Location = new System.Drawing.Point(3, 3);
            this.gwbCredits.Name = "gwbCredits";
            this.gwbCredits.NoDefaultContextMenu = true;
            this.gwbCredits.Size = new System.Drawing.Size(552, 464);
            this.gwbCredits.TabIndex = 12;
            this.gwbCredits.UseHttpActivityObserver = false;
            // 
            // gwbChangeLog
            // 
            this.gwbChangeLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gwbChangeLog.FrameEventsPropagateToMainWindow = false;
            this.gwbChangeLog.Location = new System.Drawing.Point(561, 3);
            this.gwbChangeLog.Name = "gwbChangeLog";
            this.gwbChangeLog.NoDefaultContextMenu = true;
            this.gwbChangeLog.Size = new System.Drawing.Size(553, 464);
            this.gwbChangeLog.TabIndex = 13;
            this.gwbChangeLog.UseHttpActivityObserver = false;
            // 
            // tlpTop
            // 
            this.tlpTop.ColumnCount = 1;
            this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTop.Controls.Add(this.pnlTop, 0, 0);
            this.tlpTop.Controls.Add(this.pnlBottom, 0, 1);
            this.tlpTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpTop.Location = new System.Drawing.Point(0, 0);
            this.tlpTop.Name = "tlpTop";
            this.tlpTop.RowCount = 2;
            this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpTop.Size = new System.Drawing.Size(1117, 235);
            this.tlpTop.TabIndex = 14;
            // 
            // AboutWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1117, 705);
            this.Controls.Add(this.tlpBottom);
            this.Controls.Add(this.tlpTop);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.tlpBottom.ResumeLayout(false);
            this.tlpTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        internal Controls.Base.NGLabel lblCopyright;
        internal Controls.Base.NGLabel lblTitle;
        internal Controls.Base.NGLabel lblVersion;
        internal Controls.Base.NGLabel lblLicense;
        internal System.Windows.Forms.Panel pnlBottom;
        internal System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private System.Windows.Forms.TableLayoutPanel tlpTop;
        internal System.Windows.Forms.Panel pnlTop;
        private Gecko.GeckoWebBrowser gwbCredits;
        private Gecko.GeckoWebBrowser gwbChangeLog;
    }
}
