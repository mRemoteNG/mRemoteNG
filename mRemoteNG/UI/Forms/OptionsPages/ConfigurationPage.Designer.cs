using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class ConfigurationPage : OptionsPage
    {
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
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
                try { base.Dispose(disposing); }
                catch (System.NullReferenceException) { }
            }
        }

        private System.ComponentModel.Container components = null;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            tableLayoutPanelPath = new System.Windows.Forms.TableLayoutPanel();
            lblConfigurationDirectory = new MrngLabel();
            txtConfigurationDirectory = new MrngTextBox();
            btnBrowseConfigurationDirectory = new MrngButton();
            lblExtAppsFile = new MrngLabel();
            txtExtAppsFilePath = new MrngTextBox();
            btnBrowseExtAppsFile = new MrngButton();
            lblPortableInfo = new MrngLabel();
            lblConfigurationRestartRequired = new MrngLabel();
            tableLayoutPanelPath.SuspendLayout();
            SuspendLayout();
            //
            // tableLayoutPanelPath
            //
            tableLayoutPanelPath.ColumnCount = 3;
            tableLayoutPanelPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            tableLayoutPanelPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            tableLayoutPanelPath.Controls.Add(lblConfigurationDirectory, 0, 0);
            tableLayoutPanelPath.Controls.Add(txtConfigurationDirectory, 1, 0);
            tableLayoutPanelPath.Controls.Add(btnBrowseConfigurationDirectory, 2, 0);
            tableLayoutPanelPath.Controls.Add(lblExtAppsFile, 0, 1);
            tableLayoutPanelPath.Controls.Add(txtExtAppsFilePath, 1, 1);
            tableLayoutPanelPath.Controls.Add(btnBrowseExtAppsFile, 2, 1);
            tableLayoutPanelPath.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelPath.Location = new System.Drawing.Point(0, 30);
            tableLayoutPanelPath.Name = "tableLayoutPanelPath";
            tableLayoutPanelPath.RowCount = 2;
            tableLayoutPanelPath.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            tableLayoutPanelPath.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            tableLayoutPanelPath.Size = new System.Drawing.Size(610, 64);
            tableLayoutPanelPath.TabIndex = 1;
            //
            // lblConfigurationDirectory
            //
            lblConfigurationDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            lblConfigurationDirectory.Location = new System.Drawing.Point(3, 0);
            lblConfigurationDirectory.Name = "lblConfigurationDirectory";
            lblConfigurationDirectory.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            lblConfigurationDirectory.Size = new System.Drawing.Size(164, 32);
            lblConfigurationDirectory.TabIndex = 0;
            lblConfigurationDirectory.Text = "Configuration directory:";
            lblConfigurationDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtConfigurationDirectory
            //
            txtConfigurationDirectory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtConfigurationDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            txtConfigurationDirectory.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtConfigurationDirectory.Location = new System.Drawing.Point(173, 5);
            txtConfigurationDirectory.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            txtConfigurationDirectory.Name = "txtConfigurationDirectory";
            txtConfigurationDirectory.Size = new System.Drawing.Size(338, 22);
            txtConfigurationDirectory.TabIndex = 1;
            //
            // btnBrowseConfigurationDirectory
            //
            btnBrowseConfigurationDirectory._mice = MrngButton.MouseState.OUT;
            btnBrowseConfigurationDirectory.Location = new System.Drawing.Point(517, 3);
            btnBrowseConfigurationDirectory.Name = "btnBrowseConfigurationDirectory";
            btnBrowseConfigurationDirectory.Size = new System.Drawing.Size(90, 25);
            btnBrowseConfigurationDirectory.TabIndex = 2;
            btnBrowseConfigurationDirectory.Text = "Browse";
            btnBrowseConfigurationDirectory.UseVisualStyleBackColor = true;
            btnBrowseConfigurationDirectory.Click += btnBrowseConfigurationDirectory_Click;
            //
            // lblExtAppsFile
            //
            lblExtAppsFile.Dock = System.Windows.Forms.DockStyle.Fill;
            lblExtAppsFile.Location = new System.Drawing.Point(3, 32);
            lblExtAppsFile.Name = "lblExtAppsFile";
            lblExtAppsFile.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            lblExtAppsFile.Size = new System.Drawing.Size(164, 32);
            lblExtAppsFile.TabIndex = 3;
            lblExtAppsFile.Text = "External apps file:";
            lblExtAppsFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtExtAppsFilePath
            //
            txtExtAppsFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtExtAppsFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            txtExtAppsFilePath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtExtAppsFilePath.Location = new System.Drawing.Point(173, 37);
            txtExtAppsFilePath.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            txtExtAppsFilePath.Name = "txtExtAppsFilePath";
            txtExtAppsFilePath.Size = new System.Drawing.Size(338, 22);
            txtExtAppsFilePath.TabIndex = 4;
            //
            // btnBrowseExtAppsFile
            //
            btnBrowseExtAppsFile._mice = MrngButton.MouseState.OUT;
            btnBrowseExtAppsFile.Location = new System.Drawing.Point(517, 35);
            btnBrowseExtAppsFile.Name = "btnBrowseExtAppsFile";
            btnBrowseExtAppsFile.Size = new System.Drawing.Size(90, 25);
            btnBrowseExtAppsFile.TabIndex = 5;
            btnBrowseExtAppsFile.Text = "Browse";
            btnBrowseExtAppsFile.UseVisualStyleBackColor = true;
            btnBrowseExtAppsFile.Click += btnBrowseExtAppsFile_Click;
            //
            // lblPortableInfo
            //
            lblPortableInfo.Dock = System.Windows.Forms.DockStyle.Top;
            lblPortableInfo.Location = new System.Drawing.Point(0, 94);
            lblPortableInfo.Name = "lblPortableInfo";
            lblPortableInfo.Padding = new System.Windows.Forms.Padding(6, 8, 6, 0);
            lblPortableInfo.Size = new System.Drawing.Size(610, 44);
            lblPortableInfo.TabIndex = 2;
            lblPortableInfo.Text = "Leave this value empty to use the default per-user configuration directory.";
            //
            // lblConfigurationRestartRequired
            //
            lblConfigurationRestartRequired.Dock = System.Windows.Forms.DockStyle.Top;
            lblConfigurationRestartRequired.Location = new System.Drawing.Point(0, 0);
            lblConfigurationRestartRequired.Name = "lblConfigurationRestartRequired";
            lblConfigurationRestartRequired.Padding = new System.Windows.Forms.Padding(6, 8, 6, 0);
            lblConfigurationRestartRequired.Size = new System.Drawing.Size(610, 30);
            lblConfigurationRestartRequired.TabIndex = 0;
            lblConfigurationRestartRequired.Text = "mRemoteNG must be restarted before configuration directory changes take effect.";
            //
            // ConfigurationPage
            //
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(lblPortableInfo);
            Controls.Add(tableLayoutPanelPath);
            Controls.Add(lblConfigurationRestartRequired);
            Name = "ConfigurationPage";
            Size = new System.Drawing.Size(610, 496);
            tableLayoutPanelPath.ResumeLayout(false);
            tableLayoutPanelPath.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPath;
        private MrngLabel lblConfigurationDirectory;
        private MrngTextBox txtConfigurationDirectory;
        private MrngButton btnBrowseConfigurationDirectory;
        private MrngLabel lblExtAppsFile;
        private MrngTextBox txtExtAppsFilePath;
        private MrngButton btnBrowseExtAppsFile;
        private MrngLabel lblPortableInfo;
        private MrngLabel lblConfigurationRestartRequired;
    }
}
