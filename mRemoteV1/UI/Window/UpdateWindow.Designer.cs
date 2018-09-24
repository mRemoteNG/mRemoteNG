

namespace mRemoteNG.UI.Window
{
	public partial class UpdateWindow
	{
        #region  Windows Form Designer generated code
		internal Controls.Base.NGLabel lblStatus;
		internal Controls.Base.NGTextBox txtChangeLog;
		internal Controls.Base.NGProgressBar prgbDownload;
		internal Controls.Base.NGButton btnDownload;
		internal Controls.Base.NGLabel lblChangeLogLabel;
		internal System.Windows.Forms.Panel pnlUpdate;
		internal Controls.Base.NGLabel lblLatestVersionLabel;
		internal Controls.Base.NGLabel lblInstalledVersionLabel;
		internal Controls.Base.NGLabel lblLatestVersion;
		internal Controls.Base.NGLabel lblInstalledVersion;
		internal System.Windows.Forms.PictureBox pbUpdateImage;
		internal Controls.Base.NGButton btnCheckForUpdate;
				
		private void InitializeComponent()
		{
            this.btnCheckForUpdate = new mRemoteNG.UI.Controls.Base.NGButton();
            this.pnlUpdate = new System.Windows.Forms.Panel();
            this.lblChangeLogLabel = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.btnDownload = new mRemoteNG.UI.Controls.Base.NGButton();
            this.prgbDownload = new mRemoteNG.UI.Controls.Base.NGProgressBar();
            this.txtChangeLog = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblStatus = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblLatestVersionLabel = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblInstalledVersionLabel = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblLatestVersion = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblInstalledVersion = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pbUpdateImage = new System.Windows.Forms.PictureBox();
            this.pnlUpdate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUpdateImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCheckForUpdate
            // 
            this.btnCheckForUpdate._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnCheckForUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckForUpdate.Location = new System.Drawing.Point(16, 104);
            this.btnCheckForUpdate.Name = "btnCheckForUpdate";
            this.btnCheckForUpdate.Size = new System.Drawing.Size(104, 32);
            this.btnCheckForUpdate.TabIndex = 5;
            this.btnCheckForUpdate.Text = "Check Again";
            this.btnCheckForUpdate.UseVisualStyleBackColor = true;
            this.btnCheckForUpdate.Click += new System.EventHandler(this.btnCheckForUpdate_Click);
            // 
            // pnlUpdate
            // 
            this.pnlUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlUpdate.Controls.Add(this.lblChangeLogLabel);
            this.pnlUpdate.Controls.Add(this.btnDownload);
            this.pnlUpdate.Controls.Add(this.prgbDownload);
            this.pnlUpdate.Controls.Add(this.txtChangeLog);
            this.pnlUpdate.Location = new System.Drawing.Point(16, 152);
            this.pnlUpdate.Name = "pnlUpdate";
            this.pnlUpdate.Size = new System.Drawing.Size(718, 248);
            this.pnlUpdate.TabIndex = 6;
            this.pnlUpdate.Visible = false;
            // 
            // lblChangeLogLabel
            // 
            this.lblChangeLogLabel.AutoSize = true;
            this.lblChangeLogLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangeLogLabel.Location = new System.Drawing.Point(0, 0);
            this.lblChangeLogLabel.Name = "lblChangeLogLabel";
            this.lblChangeLogLabel.Size = new System.Drawing.Size(73, 13);
            this.lblChangeLogLabel.TabIndex = 0;
            this.lblChangeLogLabel.Text = "Change Log:";
            // 
            // btnDownload
            // 
            this.btnDownload._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Location = new System.Drawing.Point(0, 216);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(224, 32);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download and Install";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // prgbDownload
            // 
            this.prgbDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgbDownload.Location = new System.Drawing.Point(230, 224);
            this.prgbDownload.Name = "prgbDownload";
            this.prgbDownload.Size = new System.Drawing.Size(472, 23);
            this.prgbDownload.TabIndex = 3;
            this.prgbDownload.Visible = false;
            // 
            // txtChangeLog
            // 
            this.txtChangeLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChangeLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtChangeLog.Location = new System.Drawing.Point(16, 24);
            this.txtChangeLog.Multiline = true;
            this.txtChangeLog.Name = "txtChangeLog";
            this.txtChangeLog.ReadOnly = true;
            this.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChangeLog.Size = new System.Drawing.Size(699, 181);
            this.txtChangeLog.TabIndex = 1;
            this.txtChangeLog.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblStatus.Location = new System.Drawing.Point(12, 16);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(660, 23);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLatestVersionLabel
            // 
            this.lblLatestVersionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLatestVersionLabel.Location = new System.Drawing.Point(16, 72);
            this.lblLatestVersionLabel.Name = "lblLatestVersionLabel";
            this.lblLatestVersionLabel.Size = new System.Drawing.Size(120, 16);
            this.lblLatestVersionLabel.TabIndex = 3;
            this.lblLatestVersionLabel.Text = "Current version:";
            this.lblLatestVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInstalledVersionLabel
            // 
            this.lblInstalledVersionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstalledVersionLabel.Location = new System.Drawing.Point(16, 48);
            this.lblInstalledVersionLabel.Name = "lblInstalledVersionLabel";
            this.lblInstalledVersionLabel.Size = new System.Drawing.Size(120, 16);
            this.lblInstalledVersionLabel.TabIndex = 1;
            this.lblInstalledVersionLabel.Text = "Installed version:";
            this.lblInstalledVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLatestVersion
            // 
            this.lblLatestVersion.Location = new System.Drawing.Point(136, 72);
            this.lblLatestVersion.Name = "lblLatestVersion";
            this.lblLatestVersion.Size = new System.Drawing.Size(104, 16);
            this.lblLatestVersion.TabIndex = 4;
            this.lblLatestVersion.Text = "Version";
            this.lblLatestVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInstalledVersion
            // 
            this.lblInstalledVersion.Location = new System.Drawing.Point(136, 48);
            this.lblInstalledVersion.Name = "lblInstalledVersion";
            this.lblInstalledVersion.Size = new System.Drawing.Size(104, 16);
            this.lblInstalledVersion.TabIndex = 2;
            this.lblInstalledVersion.Text = "Version";
            this.lblInstalledVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbUpdateImage
            // 
            this.pbUpdateImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbUpdateImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbUpdateImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbUpdateImage.Location = new System.Drawing.Point(246, 48);
            this.pbUpdateImage.Name = "pbUpdateImage";
            this.pbUpdateImage.Size = new System.Drawing.Size(468, 60);
            this.pbUpdateImage.TabIndex = 45;
            this.pbUpdateImage.TabStop = false;
            this.pbUpdateImage.Visible = false;
            this.pbUpdateImage.Click += new System.EventHandler(this.pbUpdateImage_Click);
            // 
            // UpdateWindow
            // 
            this.ClientSize = new System.Drawing.Size(734, 418);
            this.Controls.Add(this.pbUpdateImage);
            this.Controls.Add(this.lblLatestVersionLabel);
            this.Controls.Add(this.lblInstalledVersionLabel);
            this.Controls.Add(this.lblLatestVersion);
            this.Controls.Add(this.btnCheckForUpdate);
            this.Controls.Add(this.lblInstalledVersion);
            this.Controls.Add(this.pnlUpdate);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::mRemoteNG.Resources.Update_Icon;
            this.Name = "UpdateWindow";
            this.TabText = "Update";
            this.Text = "Update";
            this.Load += new System.EventHandler(this.Update_Load);
            this.pnlUpdate.ResumeLayout(false);
            this.pnlUpdate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUpdateImage)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion
	}
}
