

namespace mRemoteNG.UI.Window
{
	public partial class UpdateWindow
	{
        #region  Windows Form Designer generated code
		internal System.Windows.Forms.Label lblStatus;
		internal System.Windows.Forms.TextBox txtChangeLog;
		internal System.Windows.Forms.ProgressBar prgbDownload;
		internal System.Windows.Forms.Button btnDownload;
		internal System.Windows.Forms.Label lblChangeLogLabel;
		internal System.Windows.Forms.Panel pnlUpdate;
		internal System.Windows.Forms.Label lblLatestVersionLabel;
		internal System.Windows.Forms.Label lblInstalledVersionLabel;
		internal System.Windows.Forms.Label lblLatestVersion;
		internal System.Windows.Forms.Label lblInstalledVersion;
		internal System.Windows.Forms.PictureBox pbUpdateImage;
		internal System.Windows.Forms.Button btnCheckForUpdate;
				
		private void InitializeComponent()
		{
            this.btnCheckForUpdate = new System.Windows.Forms.Button();
            this.pnlUpdate = new System.Windows.Forms.Panel();
            this.lblChangeLogLabel = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.prgbDownload = new System.Windows.Forms.ProgressBar();
            this.txtChangeLog = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLatestVersionLabel = new System.Windows.Forms.Label();
            this.lblInstalledVersionLabel = new System.Windows.Forms.Label();
            this.lblLatestVersion = new System.Windows.Forms.Label();
            this.lblInstalledVersion = new System.Windows.Forms.Label();
            this.pbUpdateImage = new System.Windows.Forms.PictureBox();
            this.pnlUpdate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUpdateImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCheckForUpdate
            // 
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
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Location = new System.Drawing.Point(0, 216);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(144, 32);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download and Install";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // prgbDownload
            // 
            this.prgbDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgbDownload.Location = new System.Drawing.Point(160, 224);
            this.prgbDownload.Name = "prgbDownload";
            this.prgbDownload.Size = new System.Drawing.Size(542, 23);
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
