

using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Window
{
	public partial class UpdateWindow
	{
        #region  Windows Form Designer generated code
		internal Controls.MrngLabel lblStatus;
		internal Controls.MrngTextBox txtChangeLog;
		internal Controls.MrngLabel lblChangeLogLabel;
		internal Controls.MrngLabel lblLatestVersionLabel;
		internal Controls.MrngLabel lblInstalledVersionLabel;
		internal Controls.MrngLabel lblLatestVersion;
		internal Controls.MrngLabel lblInstalledVersion;
		internal System.Windows.Forms.PictureBox pbUpdateImage;
		internal MrngButton btnCheckForUpdate;
				
		private void InitializeComponent()
		{
            this.btnCheckForUpdate = new MrngButton();
            this.lblChangeLogLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.txtChangeLog = new mRemoteNG.UI.Controls.MrngTextBox();
            this.lblStatus = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblLatestVersionLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblInstalledVersionLabel = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblLatestVersion = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblInstalledVersion = new mRemoteNG.UI.Controls.MrngLabel();
            this.pbUpdateImage = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.prgbDownload = new mRemoteNG.UI.Controls.MrngProgressBar();
            this.btnDownload = new MrngButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbUpdateImage)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCheckForUpdate
            // 
            this.btnCheckForUpdate._mice = MrngButton.MouseState.HOVER;
            this.btnCheckForUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckForUpdate.Location = new System.Drawing.Point(3, 94);
            this.btnCheckForUpdate.Name = "btnCheckForUpdate";
            this.btnCheckForUpdate.Size = new System.Drawing.Size(104, 32);
            this.btnCheckForUpdate.TabIndex = 5;
            this.btnCheckForUpdate.Text = "Check Again";
            this.btnCheckForUpdate.UseVisualStyleBackColor = true;
            this.btnCheckForUpdate.Click += new System.EventHandler(this.btnCheckForUpdate_Click);
            // 
            // lblChangeLogLabel
            // 
            this.lblChangeLogLabel.AutoSize = true;
            this.lblChangeLogLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangeLogLabel.Location = new System.Drawing.Point(3, 129);
            this.lblChangeLogLabel.Name = "lblChangeLogLabel";
            this.lblChangeLogLabel.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblChangeLogLabel.Size = new System.Drawing.Size(73, 23);
            this.lblChangeLogLabel.TabIndex = 0;
            this.lblChangeLogLabel.Text = "Change Log:";
            // 
            // txtChangeLog
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtChangeLog, 3);
            this.txtChangeLog.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtChangeLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChangeLog.Location = new System.Drawing.Point(3, 155);
            this.txtChangeLog.Multiline = true;
            this.txtChangeLog.Name = "txtChangeLog";
            this.txtChangeLog.ReadOnly = true;
            this.txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChangeLog.Size = new System.Drawing.Size(728, 225);
            this.txtChangeLog.TabIndex = 1;
            this.txtChangeLog.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatus, 3);
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblStatus.Location = new System.Drawing.Point(3, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblStatus.Size = new System.Drawing.Size(57, 31);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLatestVersionLabel
            // 
            this.lblLatestVersionLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblLatestVersionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLatestVersionLabel.Location = new System.Drawing.Point(3, 61);
            this.lblLatestVersionLabel.Name = "lblLatestVersionLabel";
            this.lblLatestVersionLabel.Size = new System.Drawing.Size(120, 30);
            this.lblLatestVersionLabel.TabIndex = 3;
            this.lblLatestVersionLabel.Text = "Current version:";
            this.lblLatestVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInstalledVersionLabel
            // 
            this.lblInstalledVersionLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblInstalledVersionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstalledVersionLabel.Location = new System.Drawing.Point(3, 31);
            this.lblInstalledVersionLabel.Name = "lblInstalledVersionLabel";
            this.lblInstalledVersionLabel.Size = new System.Drawing.Size(120, 30);
            this.lblInstalledVersionLabel.TabIndex = 1;
            this.lblInstalledVersionLabel.Text = "Installed version:";
            this.lblInstalledVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLatestVersion
            // 
            this.lblLatestVersion.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblLatestVersion.Location = new System.Drawing.Point(129, 61);
            this.lblLatestVersion.Name = "lblLatestVersion";
            this.lblLatestVersion.Size = new System.Drawing.Size(104, 30);
            this.lblLatestVersion.TabIndex = 4;
            this.lblLatestVersion.Text = "Version";
            this.lblLatestVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInstalledVersion
            // 
            this.lblInstalledVersion.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInstalledVersion.Location = new System.Drawing.Point(129, 31);
            this.lblInstalledVersion.Name = "lblInstalledVersion";
            this.lblInstalledVersion.Size = new System.Drawing.Size(104, 30);
            this.lblInstalledVersion.TabIndex = 2;
            this.lblInstalledVersion.Text = "Version";
            this.lblInstalledVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbUpdateImage
            // 
            this.pbUpdateImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbUpdateImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbUpdateImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbUpdateImage.Location = new System.Drawing.Point(239, 34);
            this.pbUpdateImage.Name = "pbUpdateImage";
            this.tableLayoutPanel1.SetRowSpan(this.pbUpdateImage, 4);
            this.pbUpdateImage.Size = new System.Drawing.Size(492, 115);
            this.pbUpdateImage.TabIndex = 45;
            this.pbUpdateImage.TabStop = false;
            this.pbUpdateImage.Visible = false;
            this.pbUpdateImage.Click += new System.EventHandler(this.pbUpdateImage_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblChangeLogLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCheckForUpdate, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblInstalledVersionLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblLatestVersionLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblInstalledVersion, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblLatestVersion, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtChangeLog, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnDownload, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.pbUpdateImage, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.prgbDownload, 2, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 418);
            this.tableLayoutPanel1.TabIndex = 46;
            // 
            // prgbDownload
            // 
            this.prgbDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgbDownload.Location = new System.Drawing.Point(239, 386);
            this.prgbDownload.Name = "prgbDownload";
            this.prgbDownload.Size = new System.Drawing.Size(492, 29);
            this.prgbDownload.TabIndex = 3;
            this.prgbDownload.Visible = false;
            // 
            // btnDownload
            // 
            this.btnDownload._mice = MrngButton.MouseState.HOVER;
            this.tableLayoutPanel1.SetColumnSpan(this.btnDownload, 2);
            this.btnDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Location = new System.Drawing.Point(3, 386);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(230, 29);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download and Install";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // UpdateWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(734, 418);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpdateWindow";
            this.TabText = "Update";
            this.Text = "Update";
            this.Load += new System.EventHandler(this.Update_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbUpdateImage)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

		}
        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal MrngButton btnDownload;
        internal Controls.MrngProgressBar prgbDownload;
    }
}
