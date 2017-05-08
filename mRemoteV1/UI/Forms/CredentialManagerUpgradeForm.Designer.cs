namespace mRemoteNG.UI.Forms
{
    partial class CredentialManagerUpgradeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredentialManagerUpgradeForm));
            this.tabControl = new mRemoteNG.UI.Controls.HeadlessTabControl();
            this.tabPageWelcome = new System.Windows.Forms.TabPage();
            this.buttonExit = new System.Windows.Forms.Button();
            this.labelDescriptionOfUpgrade = new System.Windows.Forms.Label();
            this.buttonPerformUpgrade = new System.Windows.Forms.Button();
            this.buttonNewFile = new System.Windows.Forms.Button();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.tabPageUpgradeOptions = new System.Windows.Forms.TabPage();
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelConfConsPathHeader = new System.Windows.Forms.Label();
            this.labelConfConsPath = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPageWelcome.SuspendLayout();
            this.tabPageUpgradeOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageWelcome);
            this.tabControl.Controls.Add(this.tabPageUpgradeOptions);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ItemSize = new System.Drawing.Size(60, 20);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(613, 364);
            this.tabControl.TabIndex = 5;
            // 
            // tabPageWelcome
            // 
            this.tabPageWelcome.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageWelcome.Controls.Add(this.buttonExit);
            this.tabPageWelcome.Controls.Add(this.labelDescriptionOfUpgrade);
            this.tabPageWelcome.Controls.Add(this.buttonPerformUpgrade);
            this.tabPageWelcome.Controls.Add(this.buttonNewFile);
            this.tabPageWelcome.Controls.Add(this.buttonOpenFile);
            this.tabPageWelcome.Location = new System.Drawing.Point(4, 24);
            this.tabPageWelcome.Name = "tabPageWelcome";
            this.tabPageWelcome.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWelcome.Size = new System.Drawing.Size(605, 336);
            this.tabPageWelcome.TabIndex = 0;
            this.tabPageWelcome.Text = "tabPage1";
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExit.Location = new System.Drawing.Point(221, 268);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(139, 23);
            this.buttonExit.TabIndex = 4;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // labelDescriptionOfUpgrade
            // 
            this.labelDescriptionOfUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDescriptionOfUpgrade.Location = new System.Drawing.Point(6, 3);
            this.labelDescriptionOfUpgrade.Name = "labelDescriptionOfUpgrade";
            this.labelDescriptionOfUpgrade.Size = new System.Drawing.Size(593, 120);
            this.labelDescriptionOfUpgrade.TabIndex = 0;
            this.labelDescriptionOfUpgrade.Text = resources.GetString("labelDescriptionOfUpgrade.Text");
            // 
            // buttonPerformUpgrade
            // 
            this.buttonPerformUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPerformUpgrade.Location = new System.Drawing.Point(221, 181);
            this.buttonPerformUpgrade.Name = "buttonPerformUpgrade";
            this.buttonPerformUpgrade.Size = new System.Drawing.Size(139, 23);
            this.buttonPerformUpgrade.TabIndex = 1;
            this.buttonPerformUpgrade.Text = "Upgrade";
            this.buttonPerformUpgrade.UseVisualStyleBackColor = true;
            this.buttonPerformUpgrade.Click += new System.EventHandler(this.buttonPerformUpgrade_Click);
            // 
            // buttonNewFile
            // 
            this.buttonNewFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNewFile.Location = new System.Drawing.Point(221, 239);
            this.buttonNewFile.Name = "buttonNewFile";
            this.buttonNewFile.Size = new System.Drawing.Size(139, 23);
            this.buttonNewFile.TabIndex = 3;
            this.buttonNewFile.Text = "Create and open new file";
            this.buttonNewFile.UseVisualStyleBackColor = true;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOpenFile.Location = new System.Drawing.Point(221, 210);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(139, 23);
            this.buttonOpenFile.TabIndex = 2;
            this.buttonOpenFile.Text = "Open a different file";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            // 
            // tabPageUpgradeOptions
            // 
            this.tabPageUpgradeOptions.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageUpgradeOptions.Controls.Add(this.labelConfConsPath);
            this.tabPageUpgradeOptions.Controls.Add(this.labelConfConsPathHeader);
            this.tabPageUpgradeOptions.Controls.Add(this.buttonBack);
            this.tabPageUpgradeOptions.Location = new System.Drawing.Point(4, 24);
            this.tabPageUpgradeOptions.Name = "tabPageUpgradeOptions";
            this.tabPageUpgradeOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpgradeOptions.Size = new System.Drawing.Size(605, 336);
            this.tabPageUpgradeOptions.TabIndex = 1;
            this.tabPageUpgradeOptions.Text = "tabPage2";
            // 
            // buttonBack
            // 
            this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBack.Location = new System.Drawing.Point(436, 307);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 0;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // labelConfConsPathHeader
            // 
            this.labelConfConsPathHeader.AutoSize = true;
            this.labelConfConsPathHeader.Location = new System.Drawing.Point(6, 3);
            this.labelConfConsPathHeader.Name = "labelConfConsPathHeader";
            this.labelConfConsPathHeader.Size = new System.Drawing.Size(85, 13);
            this.labelConfConsPathHeader.TabIndex = 1;
            this.labelConfConsPathHeader.Text = "Connections file:";
            // 
            // labelConfConsPath
            // 
            this.labelConfConsPath.Location = new System.Drawing.Point(97, 3);
            this.labelConfConsPath.Name = "labelConfConsPath";
            this.labelConfConsPath.Size = new System.Drawing.Size(500, 35);
            this.labelConfConsPath.TabIndex = 2;
            this.labelConfConsPath.Text = "label1";
            // 
            // CredentialManagerUpgradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 364);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.Name = "CredentialManagerUpgradeForm";
            this.Text = "Credential Manager Upgrade";
            this.tabControl.ResumeLayout(false);
            this.tabPageWelcome.ResumeLayout(false);
            this.tabPageUpgradeOptions.ResumeLayout(false);
            this.tabPageUpgradeOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelDescriptionOfUpgrade;
        private System.Windows.Forms.Button buttonPerformUpgrade;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Button buttonNewFile;
        private System.Windows.Forms.Button buttonExit;
        private mRemoteNG.UI.Controls.HeadlessTabControl tabControl;
        private System.Windows.Forms.TabPage tabPageWelcome;
        private System.Windows.Forms.TabPage tabPageUpgradeOptions;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelConfConsPath;
        private System.Windows.Forms.Label labelConfConsPathHeader;
    }
}