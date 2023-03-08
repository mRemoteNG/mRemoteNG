

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
        internal MrngButton btnCheckForUpdate;

        private void InitializeComponent()
        {
            btnCheckForUpdate = new MrngButton();
            lblChangeLogLabel = new MrngLabel();
            txtChangeLog = new MrngTextBox();
            lblStatus = new MrngLabel();
            lblLatestVersionLabel = new MrngLabel();
            lblInstalledVersionLabel = new MrngLabel();
            lblLatestVersion = new MrngLabel();
            lblInstalledVersion = new MrngLabel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            btnDownload = new MrngButton();
            prgbDownload = new MrngProgressBar();
            pbUpdateImage = new System.Windows.Forms.PictureBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbUpdateImage).BeginInit();
            SuspendLayout();
            // 
            // btnCheckForUpdate
            // 
            btnCheckForUpdate._mice = MrngButton.MouseState.OUT;
            btnCheckForUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCheckForUpdate.Location = new System.Drawing.Point(3, 94);
            btnCheckForUpdate.Name = "btnCheckForUpdate";
            btnCheckForUpdate.Size = new System.Drawing.Size(104, 32);
            btnCheckForUpdate.TabIndex = 5;
            btnCheckForUpdate.Text = "Check Again";
            btnCheckForUpdate.UseVisualStyleBackColor = true;
            btnCheckForUpdate.Click += btnCheckForUpdate_Click;
            // 
            // lblChangeLogLabel
            // 
            lblChangeLogLabel.AutoSize = true;
            lblChangeLogLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblChangeLogLabel.Location = new System.Drawing.Point(3, 129);
            lblChangeLogLabel.Name = "lblChangeLogLabel";
            lblChangeLogLabel.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            lblChangeLogLabel.Size = new System.Drawing.Size(73, 23);
            lblChangeLogLabel.TabIndex = 0;
            lblChangeLogLabel.Text = "Change Log:";
            // 
            // txtChangeLog
            // 
            tableLayoutPanel1.SetColumnSpan(txtChangeLog, 3);
            txtChangeLog.Dock = System.Windows.Forms.DockStyle.Fill;
            txtChangeLog.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtChangeLog.Location = new System.Drawing.Point(3, 381);
            txtChangeLog.Multiline = true;
            txtChangeLog.Name = "txtChangeLog";
            txtChangeLog.ReadOnly = true;
            txtChangeLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtChangeLog.Size = new System.Drawing.Size(728, 14);
            txtChangeLog.TabIndex = 1;
            txtChangeLog.TabStop = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(lblStatus, 3);
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            lblStatus.Location = new System.Drawing.Point(3, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            lblStatus.Size = new System.Drawing.Size(57, 31);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Status";
            lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLatestVersionLabel
            // 
            lblLatestVersionLabel.Dock = System.Windows.Forms.DockStyle.Right;
            lblLatestVersionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblLatestVersionLabel.Location = new System.Drawing.Point(3, 61);
            lblLatestVersionLabel.Name = "lblLatestVersionLabel";
            lblLatestVersionLabel.Size = new System.Drawing.Size(120, 30);
            lblLatestVersionLabel.TabIndex = 3;
            lblLatestVersionLabel.Text = "Current version:";
            lblLatestVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInstalledVersionLabel
            // 
            lblInstalledVersionLabel.Dock = System.Windows.Forms.DockStyle.Right;
            lblInstalledVersionLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblInstalledVersionLabel.Location = new System.Drawing.Point(3, 31);
            lblInstalledVersionLabel.Name = "lblInstalledVersionLabel";
            lblInstalledVersionLabel.Size = new System.Drawing.Size(120, 30);
            lblInstalledVersionLabel.TabIndex = 1;
            lblInstalledVersionLabel.Text = "Installed version:";
            lblInstalledVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLatestVersion
            // 
            lblLatestVersion.Dock = System.Windows.Forms.DockStyle.Left;
            lblLatestVersion.Location = new System.Drawing.Point(129, 61);
            lblLatestVersion.Name = "lblLatestVersion";
            lblLatestVersion.Size = new System.Drawing.Size(166, 30);
            lblLatestVersion.TabIndex = 4;
            lblLatestVersion.Text = "Version";
            lblLatestVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInstalledVersion
            // 
            lblInstalledVersion.Dock = System.Windows.Forms.DockStyle.Left;
            lblInstalledVersion.Location = new System.Drawing.Point(129, 31);
            lblInstalledVersion.Name = "lblInstalledVersion";
            lblInstalledVersion.Size = new System.Drawing.Size(166, 30);
            lblInstalledVersion.TabIndex = 2;
            lblInstalledVersion.Text = "Version";
            lblInstalledVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblChangeLogLabel, 0, 4);
            tableLayoutPanel1.Controls.Add(lblStatus, 0, 0);
            tableLayoutPanel1.Controls.Add(btnCheckForUpdate, 0, 3);
            tableLayoutPanel1.Controls.Add(lblInstalledVersionLabel, 0, 1);
            tableLayoutPanel1.Controls.Add(lblLatestVersionLabel, 0, 2);
            tableLayoutPanel1.Controls.Add(lblInstalledVersion, 1, 1);
            tableLayoutPanel1.Controls.Add(lblLatestVersion, 1, 2);
            tableLayoutPanel1.Controls.Add(txtChangeLog, 0, 5);
            tableLayoutPanel1.Controls.Add(btnDownload, 0, 6);
            tableLayoutPanel1.Controls.Add(prgbDownload, 2, 6);
            tableLayoutPanel1.Controls.Add(pbUpdateImage, 2, 3);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 9;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new System.Drawing.Size(734, 418);
            tableLayoutPanel1.TabIndex = 46;
            // 
            // btnDownload
            // 
            btnDownload._mice = MrngButton.MouseState.OUT;
            tableLayoutPanel1.SetColumnSpan(btnDownload, 2);
            btnDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDownload.Location = new System.Drawing.Point(3, 401);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new System.Drawing.Size(292, 14);
            btnDownload.TabIndex = 2;
            btnDownload.Text = "Download and Install";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // prgbDownload
            // 
            prgbDownload.Dock = System.Windows.Forms.DockStyle.Fill;
            prgbDownload.Location = new System.Drawing.Point(301, 401);
            prgbDownload.Name = "prgbDownload";
            prgbDownload.Size = new System.Drawing.Size(430, 14);
            prgbDownload.TabIndex = 3;
            prgbDownload.Visible = false;
            // 
            // pbUpdateImage
            // 
            pbUpdateImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pbUpdateImage.Cursor = System.Windows.Forms.Cursors.Hand;
            pbUpdateImage.Location = new System.Drawing.Point(301, 94);
            pbUpdateImage.Name = "pbUpdateImage";
            tableLayoutPanel1.SetRowSpan(pbUpdateImage, 4);
            pbUpdateImage.Size = new System.Drawing.Size(33, 32);
            pbUpdateImage.TabIndex = 45;
            pbUpdateImage.TabStop = false;
            pbUpdateImage.Visible = false;
            pbUpdateImage.Click += pbUpdateImage_Click;
            // 
            // UpdateWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(734, 418);
            Controls.Add(tableLayoutPanel1);
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Name = "UpdateWindow";
            TabText = "Update";
            Text = "Update";
            Load += Update_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbUpdateImage).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        internal MrngButton btnDownload;
        internal Controls.MrngProgressBar prgbDownload;
        internal System.Windows.Forms.PictureBox pbUpdateImage;
    }
}
