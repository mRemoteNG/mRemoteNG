

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class BackupPage : OptionsPage
	{

		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            this.tableLayoutPanelBackupFile = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxConnectionBackupPath = new System.Windows.Forms.TextBox();
            this.lblConnectionsBackupPath = new System.Windows.Forms.Label();
            this.lblConnectionsBackupFrequency = new System.Windows.Forms.Label();
            this.lblConnectionsBackupMaxCount = new System.Windows.Forms.Label();
            this.cmbConnectionBackupFrequency = new System.Windows.Forms.ComboBox();
            this.buttonBrowsePath = new System.Windows.Forms.Button();
            this.numMaxBackups = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanelBackupFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBackups)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelBackupFile
            // 
            this.tableLayoutPanelBackupFile.ColumnCount = 4;
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelBackupFile.Controls.Add(this.textBoxConnectionBackupPath, 1, 2);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblConnectionsBackupPath, 0, 2);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblConnectionsBackupFrequency, 0, 0);
            this.tableLayoutPanelBackupFile.Controls.Add(this.lblConnectionsBackupMaxCount, 0, 1);
            this.tableLayoutPanelBackupFile.Controls.Add(this.cmbConnectionBackupFrequency, 1, 0);
            this.tableLayoutPanelBackupFile.Controls.Add(this.buttonBrowsePath, 2, 2);
            this.tableLayoutPanelBackupFile.Controls.Add(this.numMaxBackups, 1, 1);
            this.tableLayoutPanelBackupFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBackupFile.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelBackupFile.Name = "tableLayoutPanelBackupFile";
            this.tableLayoutPanelBackupFile.RowCount = 4;
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBackupFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBackupFile.Size = new System.Drawing.Size(630, 412);
            this.tableLayoutPanelBackupFile.TabIndex = 15;
            // 
            // textBoxConnectionBackupPath
            // 
            this.textBoxConnectionBackupPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxConnectionBackupPath.Location = new System.Drawing.Point(195, 58);
            this.textBoxConnectionBackupPath.Name = "textBoxConnectionBackupPath";
            this.textBoxConnectionBackupPath.Size = new System.Drawing.Size(250, 22);
            this.textBoxConnectionBackupPath.TabIndex = 14;
            // 
            // lblConnectionsBackupPath
            // 
            this.lblConnectionsBackupPath.AutoSize = true;
            this.lblConnectionsBackupPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConnectionsBackupPath.Location = new System.Drawing.Point(3, 55);
            this.lblConnectionsBackupPath.Name = "lblConnectionsBackupPath";
            this.lblConnectionsBackupPath.Size = new System.Drawing.Size(186, 29);
            this.lblConnectionsBackupPath.TabIndex = 13;
            this.lblConnectionsBackupPath.Text = "Location of connection file backup";
            this.lblConnectionsBackupPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConnectionsBackupFrequency
            // 
            this.lblConnectionsBackupFrequency.AutoSize = true;
            this.lblConnectionsBackupFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConnectionsBackupFrequency.Location = new System.Drawing.Point(3, 0);
            this.lblConnectionsBackupFrequency.Name = "lblConnectionsBackupFrequency";
            this.lblConnectionsBackupFrequency.Size = new System.Drawing.Size(186, 27);
            this.lblConnectionsBackupFrequency.TabIndex = 14;
            this.lblConnectionsBackupFrequency.Text = "Connection Backup Frequency";
            this.lblConnectionsBackupFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConnectionsBackupMaxCount
            // 
            this.lblConnectionsBackupMaxCount.AutoSize = true;
            this.lblConnectionsBackupMaxCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConnectionsBackupMaxCount.Location = new System.Drawing.Point(3, 27);
            this.lblConnectionsBackupMaxCount.Name = "lblConnectionsBackupMaxCount";
            this.lblConnectionsBackupMaxCount.Size = new System.Drawing.Size(186, 28);
            this.lblConnectionsBackupMaxCount.TabIndex = 13;
            this.lblConnectionsBackupMaxCount.Text = "Maximum number of backups";
            this.lblConnectionsBackupMaxCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbConnectionBackupFrequency
            // 
            this.cmbConnectionBackupFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbConnectionBackupFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConnectionBackupFrequency.FormattingEnabled = true;
            this.cmbConnectionBackupFrequency.Location = new System.Drawing.Point(195, 3);
            this.cmbConnectionBackupFrequency.Name = "cmbConnectionBackupFrequency";
            this.cmbConnectionBackupFrequency.Size = new System.Drawing.Size(250, 21);
            this.cmbConnectionBackupFrequency.TabIndex = 13;
            // 
            // buttonBrowsePath
            // 
            this.buttonBrowsePath.Location = new System.Drawing.Point(451, 58);
            this.buttonBrowsePath.Name = "buttonBrowsePath";
            this.buttonBrowsePath.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowsePath.TabIndex = 15;
            this.buttonBrowsePath.Text = "Browse";
            this.buttonBrowsePath.UseVisualStyleBackColor = true;
            this.buttonBrowsePath.Click += new System.EventHandler(this.ButtonBrowsePath_Click);
            // 
            // numMaxBackups
            // 
            this.numMaxBackups.Location = new System.Drawing.Point(195, 30);
            this.numMaxBackups.Name = "numMaxBackups";
            this.numMaxBackups.Size = new System.Drawing.Size(60, 22);
            this.numMaxBackups.TabIndex = 16;
            // 
            // BackupPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanelBackupFile);
            this.Name = "BackupPage";
            this.Size = new System.Drawing.Size(630, 412);
            this.tableLayoutPanelBackupFile.ResumeLayout(false);
            this.tableLayoutPanelBackupFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBackups)).EndInit();
            this.ResumeLayout(false);

		}

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBackupFile;
        private System.Windows.Forms.TextBox textBoxConnectionBackupPath;
        private System.Windows.Forms.Label lblConnectionsBackupPath;
        private System.Windows.Forms.Label lblConnectionsBackupFrequency;
        private System.Windows.Forms.Label lblConnectionsBackupMaxCount;
        private System.Windows.Forms.ComboBox cmbConnectionBackupFrequency;
        private System.Windows.Forms.Button buttonBrowsePath;
        private System.Windows.Forms.NumericUpDown numMaxBackups;
    }
}