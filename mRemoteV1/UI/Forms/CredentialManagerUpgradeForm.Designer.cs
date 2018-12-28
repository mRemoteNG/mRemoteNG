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
            this.newCredRepoPathDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabControl = new mRemoteNG.UI.Controls.HeadlessTabControl();
            this.tabPageWelcome = new System.Windows.Forms.TabPage();
            this.textBoxConfConPathTab1 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.labelConfConsPathHeaderOnTab1 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.buttonExit = new mRemoteNG.UI.Controls.Base.NGButton();
            this.labelDescriptionOfUpgrade = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.buttonPerformUpgrade = new mRemoteNG.UI.Controls.Base.NGButton();
            this.buttonNewFile = new mRemoteNG.UI.Controls.Base.NGButton();
            this.buttonOpenFile = new mRemoteNG.UI.Controls.Base.NGButton();
            this.tabPageUpgradeOptions = new System.Windows.Forms.TabPage();
            this.textBoxConfConPathTab2 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.buttonNewRepoPathBrowse = new mRemoteNG.UI.Controls.Base.NGButton();
            this.labelWhereToSaveCredFile = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.textBoxCredRepoPath = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.buttonExecuteUpgrade = new mRemoteNG.UI.Controls.Base.NGButton();
            this.labelSetPassword = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.newRepositoryPasswordEntry = new mRemoteNG.UI.Controls.NewPasswordWithVerification();
            this.labelConfConsPathHeaderOnTab2 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.buttonBack = new mRemoteNG.UI.Controls.Base.NGButton();
            this.newConnectionsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabControl.SuspendLayout();
            this.tabPageWelcome.SuspendLayout();
            this.tabPageUpgradeOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // newCredRepoPathDialog
            // 
            this.newCredRepoPathDialog.Filter = "Xml|*.xml|All files|*.*";
            this.newCredRepoPathDialog.Title = "New credential repository path";
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
            this.tabControl.Size = new System.Drawing.Size(420, 402);
            this.tabControl.TabIndex = 5;
            // 
            // tabPageWelcome
            // 
            this.tabPageWelcome.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageWelcome.Controls.Add(this.textBoxConfConPathTab1);
            this.tabPageWelcome.Controls.Add(this.labelConfConsPathHeaderOnTab1);
            this.tabPageWelcome.Controls.Add(this.buttonExit);
            this.tabPageWelcome.Controls.Add(this.labelDescriptionOfUpgrade);
            this.tabPageWelcome.Controls.Add(this.buttonPerformUpgrade);
            this.tabPageWelcome.Controls.Add(this.buttonNewFile);
            this.tabPageWelcome.Controls.Add(this.buttonOpenFile);
            this.tabPageWelcome.Location = new System.Drawing.Point(4, 24);
            this.tabPageWelcome.Name = "tabPageWelcome";
            this.tabPageWelcome.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWelcome.Size = new System.Drawing.Size(412, 374);
            this.tabPageWelcome.TabIndex = 0;
            this.tabPageWelcome.Text = "welcomePage";
            // 
            // textBoxConfConPathTab1
            // 
            this.textBoxConfConPathTab1.Location = new System.Drawing.Point(30, 177);
            this.textBoxConfConPathTab1.Multiline = true;
            this.textBoxConfConPathTab1.Name = "textBoxConfConPathTab1";
            this.textBoxConfConPathTab1.ReadOnly = true;
            this.textBoxConfConPathTab1.Size = new System.Drawing.Size(376, 55);
            this.textBoxConfConPathTab1.TabIndex = 6;
            // 
            // labelConfConsPathHeaderOnTab1
            // 
            this.labelConfConsPathHeaderOnTab1.AutoSize = true;
            this.labelConfConsPathHeaderOnTab1.Location = new System.Drawing.Point(10, 161);
            this.labelConfConsPathHeaderOnTab1.Name = "labelConfConsPathHeaderOnTab1";
            this.labelConfConsPathHeaderOnTab1.Size = new System.Drawing.Size(104, 13);
            this.labelConfConsPathHeaderOnTab1.TabIndex = 5;
            this.labelConfConsPathHeaderOnTab1.Text = "Connection file path:";
            // 
            // buttonExit
            // 
            this.buttonExit._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExit.Location = new System.Drawing.Point(142, 343);
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
            this.labelDescriptionOfUpgrade.Location = new System.Drawing.Point(6, 20);
            this.labelDescriptionOfUpgrade.Name = "labelDescriptionOfUpgrade";
            this.labelDescriptionOfUpgrade.Size = new System.Drawing.Size(400, 141);
            this.labelDescriptionOfUpgrade.TabIndex = 0;
            this.labelDescriptionOfUpgrade.Text = resources.GetString("labelDescriptionOfUpgrade.Text");
            // 
            // buttonPerformUpgrade
            // 
            this.buttonPerformUpgrade._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonPerformUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPerformUpgrade.Location = new System.Drawing.Point(142, 256);
            this.buttonPerformUpgrade.Name = "buttonPerformUpgrade";
            this.buttonPerformUpgrade.Size = new System.Drawing.Size(139, 23);
            this.buttonPerformUpgrade.TabIndex = 1;
            this.buttonPerformUpgrade.Text = "Upgrade";
            this.buttonPerformUpgrade.UseVisualStyleBackColor = true;
            this.buttonPerformUpgrade.Click += new System.EventHandler(this.buttonPerformUpgrade_Click);
            // 
            // buttonNewFile
            // 
            this.buttonNewFile._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonNewFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNewFile.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.buttonNewFile.Location = new System.Drawing.Point(142, 314);
            this.buttonNewFile.Name = "buttonNewFile";
            this.buttonNewFile.Size = new System.Drawing.Size(139, 23);
            this.buttonNewFile.TabIndex = 3;
            this.buttonNewFile.Text = "Create and open new file";
            this.buttonNewFile.UseVisualStyleBackColor = true;
            this.buttonNewFile.Click += new System.EventHandler(this.buttonNewFile_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOpenFile.Location = new System.Drawing.Point(142, 285);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(139, 23);
            this.buttonOpenFile.TabIndex = 2;
            this.buttonOpenFile.Text = "Open a different file";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // tabPageUpgradeOptions
            // 
            this.tabPageUpgradeOptions.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageUpgradeOptions.Controls.Add(this.textBoxConfConPathTab2);
            this.tabPageUpgradeOptions.Controls.Add(this.buttonNewRepoPathBrowse);
            this.tabPageUpgradeOptions.Controls.Add(this.labelWhereToSaveCredFile);
            this.tabPageUpgradeOptions.Controls.Add(this.textBoxCredRepoPath);
            this.tabPageUpgradeOptions.Controls.Add(this.buttonExecuteUpgrade);
            this.tabPageUpgradeOptions.Controls.Add(this.labelSetPassword);
            this.tabPageUpgradeOptions.Controls.Add(this.newRepositoryPasswordEntry);
            this.tabPageUpgradeOptions.Controls.Add(this.labelConfConsPathHeaderOnTab2);
            this.tabPageUpgradeOptions.Controls.Add(this.buttonBack);
            this.tabPageUpgradeOptions.Location = new System.Drawing.Point(4, 24);
            this.tabPageUpgradeOptions.Name = "tabPageUpgradeOptions";
            this.tabPageUpgradeOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpgradeOptions.Size = new System.Drawing.Size(412, 374);
            this.tabPageUpgradeOptions.TabIndex = 1;
            this.tabPageUpgradeOptions.Text = "upgradePage";
            // 
            // textBoxConfConPathTab2
            // 
            this.textBoxConfConPathTab2.Location = new System.Drawing.Point(27, 32);
            this.textBoxConfConPathTab2.Multiline = true;
            this.textBoxConfConPathTab2.Name = "textBoxConfConPathTab2";
            this.textBoxConfConPathTab2.ReadOnly = true;
            this.textBoxConfConPathTab2.Size = new System.Drawing.Size(377, 41);
            this.textBoxConfConPathTab2.TabIndex = 9;
            // 
            // buttonNewRepoPathBrowse
            // 
            this.buttonNewRepoPathBrowse._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonNewRepoPathBrowse.Location = new System.Drawing.Point(329, 143);
            this.buttonNewRepoPathBrowse.Name = "buttonNewRepoPathBrowse";
            this.buttonNewRepoPathBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonNewRepoPathBrowse.TabIndex = 8;
            this.buttonNewRepoPathBrowse.Text = "Browse";
            this.buttonNewRepoPathBrowse.UseVisualStyleBackColor = true;
            this.buttonNewRepoPathBrowse.Click += new System.EventHandler(this.buttonNewRepoPathBrowse_Click);
            // 
            // labelWhereToSaveCredFile
            // 
            this.labelWhereToSaveCredFile.AutoSize = true;
            this.labelWhereToSaveCredFile.Location = new System.Drawing.Point(14, 98);
            this.labelWhereToSaveCredFile.Name = "labelWhereToSaveCredFile";
            this.labelWhereToSaveCredFile.Size = new System.Drawing.Size(228, 13);
            this.labelWhereToSaveCredFile.TabIndex = 7;
            this.labelWhereToSaveCredFile.Text = "Where should we save the new credential file?";
            // 
            // textBoxCredRepoPath
            // 
            this.textBoxCredRepoPath.Location = new System.Drawing.Point(27, 117);
            this.textBoxCredRepoPath.Name = "textBoxCredRepoPath";
            this.textBoxCredRepoPath.Size = new System.Drawing.Size(377, 20);
            this.textBoxCredRepoPath.TabIndex = 6;
            // 
            // buttonExecuteUpgrade
            // 
            this.buttonExecuteUpgrade._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonExecuteUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExecuteUpgrade.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonExecuteUpgrade.Location = new System.Drawing.Point(329, 343);
            this.buttonExecuteUpgrade.Name = "buttonExecuteUpgrade";
            this.buttonExecuteUpgrade.Size = new System.Drawing.Size(75, 23);
            this.buttonExecuteUpgrade.TabIndex = 5;
            this.buttonExecuteUpgrade.Text = "Upgrade";
            this.buttonExecuteUpgrade.UseVisualStyleBackColor = true;
            // 
            // labelSetPassword
            // 
            this.labelSetPassword.AutoSize = true;
            this.labelSetPassword.Location = new System.Drawing.Point(13, 194);
            this.labelSetPassword.Name = "labelSetPassword";
            this.labelSetPassword.Size = new System.Drawing.Size(201, 13);
            this.labelSetPassword.TabIndex = 4;
            this.labelSetPassword.Text = "Set password for the credential repository";
            // 
            // newRepositoryPasswordEntry
            // 
            this.newRepositoryPasswordEntry.Location = new System.Drawing.Point(27, 220);
            this.newRepositoryPasswordEntry.MinimumSize = new System.Drawing.Size(0, 100);
            this.newRepositoryPasswordEntry.Name = "newRepositoryPasswordEntry";
            this.newRepositoryPasswordEntry.PasswordChar = '\0';
            this.newRepositoryPasswordEntry.Size = new System.Drawing.Size(377, 100);
            this.newRepositoryPasswordEntry.TabIndex = 3;
            this.newRepositoryPasswordEntry.UseSystemPasswordChar = false;
            // 
            // labelConfConsPathHeaderOnTab2
            // 
            this.labelConfConsPathHeaderOnTab2.AutoSize = true;
            this.labelConfConsPathHeaderOnTab2.Location = new System.Drawing.Point(6, 16);
            this.labelConfConsPathHeaderOnTab2.Name = "labelConfConsPathHeaderOnTab2";
            this.labelConfConsPathHeaderOnTab2.Size = new System.Drawing.Size(85, 13);
            this.labelConfConsPathHeaderOnTab2.TabIndex = 1;
            this.labelConfConsPathHeaderOnTab2.Text = "Connections file:";
            // 
            // buttonBack
            // 
            this.buttonBack._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBack.Location = new System.Drawing.Point(248, 343);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 0;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // newConnectionsFileDialog
            // 
            this.newConnectionsFileDialog.Filter = "Xml|*.xml|All files|*.*";
            this.newConnectionsFileDialog.Title = "Create new connection file";
            // 
            // CredentialManagerUpgradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 402);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CredentialManagerUpgradeForm";
            this.ShowInTaskbar = false;
            this.Text = "Credential Manager Upgrade";
            this.tabControl.ResumeLayout(false);
            this.tabPageWelcome.ResumeLayout(false);
            this.tabPageWelcome.PerformLayout();
            this.tabPageUpgradeOptions.ResumeLayout(false);
            this.tabPageUpgradeOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Base.NGLabel labelDescriptionOfUpgrade;
        private Controls.Base.NGButton buttonPerformUpgrade;
        private Controls.Base.NGButton buttonOpenFile;
        private Controls.Base.NGButton buttonNewFile;
        private Controls.Base.NGButton buttonExit;
        private mRemoteNG.UI.Controls.HeadlessTabControl tabControl;
        private System.Windows.Forms.TabPage tabPageWelcome;
        private System.Windows.Forms.TabPage tabPageUpgradeOptions;
        private Controls.Base.NGButton buttonBack;
        private Controls.Base.NGLabel labelConfConsPathHeaderOnTab2;
        private Controls.Base.NGButton buttonExecuteUpgrade;
        private Controls.Base.NGLabel labelSetPassword;
        private Controls.NewPasswordWithVerification newRepositoryPasswordEntry;
        private Controls.Base.NGLabel labelWhereToSaveCredFile;
        private Controls.Base.NGTextBox textBoxCredRepoPath;
        private System.Windows.Forms.SaveFileDialog newCredRepoPathDialog;
        private Controls.Base.NGButton buttonNewRepoPathBrowse;
        private Controls.Base.NGLabel labelConfConsPathHeaderOnTab1;
        private Controls.Base.NGTextBox textBoxConfConPathTab1;
        private Controls.Base.NGTextBox textBoxConfConPathTab2;
        private System.Windows.Forms.SaveFileDialog newConnectionsFileDialog;
    }
}