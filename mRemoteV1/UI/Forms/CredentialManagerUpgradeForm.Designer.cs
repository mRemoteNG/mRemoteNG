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
            this.newConnectionsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabControl = new mRemoteNG.UI.Controls.HeadlessTabControl();
            this.tabPageWelcome = new System.Windows.Forms.TabPage();
            this.textBoxConfConPathTab1 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.labelConfConsPathHeaderOnTab1 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.buttonExit = new mRemoteNG.UI.Controls.Base.NGButton();
            this.labelDescriptionOfUpgrade = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.buttonBeginUpgrade = new mRemoteNG.UI.Controls.Base.NGButton();
            this.buttonNewFile = new mRemoteNG.UI.Controls.Base.NGButton();
            this.buttonOpenFile = new mRemoteNG.UI.Controls.Base.NGButton();
            this.tabPageHarvestedCreds = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCredsBack = new mRemoteNG.UI.Controls.Base.NGButton();
            this.btnCredsContinue = new mRemoteNG.UI.Controls.Base.NGButton();
            this.olvFoundCredentials = new mRemoteNG.UI.Controls.Base.NGListView();
            this.colTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colUsername = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colDomain = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colPassword = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lblCredsFound = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.tabPageSaveRepo = new System.Windows.Forms.TabPage();
            this.gbSetPassword = new System.Windows.Forms.GroupBox();
            this.newRepositoryPasswordEntry = new mRemoteNG.UI.Controls.NewPasswordWithVerification();
            this.gbWhereToSaveCredFile = new System.Windows.Forms.GroupBox();
            this.buttonNewRepoPathBrowse = new mRemoteNG.UI.Controls.Base.NGButton();
            this.textBoxCredRepoPath = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.textBoxConfConPathTab2 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.buttonExecuteUpgrade = new mRemoteNG.UI.Controls.Base.NGButton();
            this.labelConfConsPathHeaderOnTab2 = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.buttonSaveRepoBack = new mRemoteNG.UI.Controls.Base.NGButton();
            this.tabControl.SuspendLayout();
            this.tabPageWelcome.SuspendLayout();
            this.tabPageHarvestedCreds.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvFoundCredentials)).BeginInit();
            this.tabPageSaveRepo.SuspendLayout();
            this.gbSetPassword.SuspendLayout();
            this.gbWhereToSaveCredFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // newCredRepoPathDialog
            // 
            this.newCredRepoPathDialog.Filter = "Xml|*.xml|All files|*.*";
            this.newCredRepoPathDialog.Title = "New credential repository path";
            // 
            // newConnectionsFileDialog
            // 
            this.newConnectionsFileDialog.Filter = "Xml|*.xml|All files|*.*";
            this.newConnectionsFileDialog.Title = "Create new connection file";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageWelcome);
            this.tabControl.Controls.Add(this.tabPageHarvestedCreds);
            this.tabControl.Controls.Add(this.tabPageSaveRepo);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ItemSize = new System.Drawing.Size(60, 20);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(602, 405);
            this.tabControl.TabIndex = 5;
            // 
            // tabPageWelcome
            // 
            this.tabPageWelcome.BackColor = System.Drawing.Color.Transparent;
            this.tabPageWelcome.Controls.Add(this.textBoxConfConPathTab1);
            this.tabPageWelcome.Controls.Add(this.labelConfConsPathHeaderOnTab1);
            this.tabPageWelcome.Controls.Add(this.buttonExit);
            this.tabPageWelcome.Controls.Add(this.labelDescriptionOfUpgrade);
            this.tabPageWelcome.Controls.Add(this.buttonBeginUpgrade);
            this.tabPageWelcome.Controls.Add(this.buttonNewFile);
            this.tabPageWelcome.Controls.Add(this.buttonOpenFile);
            this.tabPageWelcome.Location = new System.Drawing.Point(4, 24);
            this.tabPageWelcome.Name = "tabPageWelcome";
            this.tabPageWelcome.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWelcome.Size = new System.Drawing.Size(594, 377);
            this.tabPageWelcome.TabIndex = 0;
            this.tabPageWelcome.Text = "welcomePage";
            // 
            // textBoxConfConPathTab1
            // 
            this.textBoxConfConPathTab1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConfConPathTab1.Location = new System.Drawing.Point(9, 150);
            this.textBoxConfConPathTab1.Multiline = true;
            this.textBoxConfConPathTab1.Name = "textBoxConfConPathTab1";
            this.textBoxConfConPathTab1.ReadOnly = true;
            this.textBoxConfConPathTab1.Size = new System.Drawing.Size(577, 55);
            this.textBoxConfConPathTab1.TabIndex = 6;
            // 
            // labelConfConsPathHeaderOnTab1
            // 
            this.labelConfConsPathHeaderOnTab1.AutoSize = true;
            this.labelConfConsPathHeaderOnTab1.Location = new System.Drawing.Point(6, 134);
            this.labelConfConsPathHeaderOnTab1.Name = "labelConfConsPathHeaderOnTab1";
            this.labelConfConsPathHeaderOnTab1.Size = new System.Drawing.Size(104, 13);
            this.labelConfConsPathHeaderOnTab1.TabIndex = 5;
            this.labelConfConsPathHeaderOnTab1.Text = "Connection file path:";
            // 
            // buttonExit
            // 
            this.buttonExit._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExit.Location = new System.Drawing.Point(190, 339);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(212, 23);
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
            this.labelDescriptionOfUpgrade.Size = new System.Drawing.Size(597, 141);
            this.labelDescriptionOfUpgrade.TabIndex = 0;
            this.labelDescriptionOfUpgrade.Text = resources.GetString("labelDescriptionOfUpgrade.Text");
            // 
            // buttonBeginUpgrade
            // 
            this.buttonBeginUpgrade._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonBeginUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonBeginUpgrade.Location = new System.Drawing.Point(190, 252);
            this.buttonBeginUpgrade.Name = "buttonBeginUpgrade";
            this.buttonBeginUpgrade.Size = new System.Drawing.Size(212, 23);
            this.buttonBeginUpgrade.TabIndex = 1;
            this.buttonBeginUpgrade.Text = "Upgrade";
            this.buttonBeginUpgrade.UseVisualStyleBackColor = true;
            this.buttonBeginUpgrade.Click += new System.EventHandler(this.buttonBeginUpgrade_Click);
            // 
            // buttonNewFile
            // 
            this.buttonNewFile._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonNewFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNewFile.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.buttonNewFile.Location = new System.Drawing.Point(190, 310);
            this.buttonNewFile.Name = "buttonNewFile";
            this.buttonNewFile.Size = new System.Drawing.Size(212, 23);
            this.buttonNewFile.TabIndex = 3;
            this.buttonNewFile.Text = "Create and open new file";
            this.buttonNewFile.UseVisualStyleBackColor = true;
            this.buttonNewFile.Click += new System.EventHandler(this.buttonNewFile_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOpenFile.Location = new System.Drawing.Point(190, 281);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(212, 23);
            this.buttonOpenFile.TabIndex = 2;
            this.buttonOpenFile.Text = "Open a different file";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // tabPageHarvestedCreds
            // 
            this.tabPageHarvestedCreds.BackColor = System.Drawing.Color.Transparent;
            this.tabPageHarvestedCreds.Controls.Add(this.tableLayoutPanel1);
            this.tabPageHarvestedCreds.Location = new System.Drawing.Point(4, 24);
            this.tabPageHarvestedCreds.Name = "tabPageHarvestedCreds";
            this.tabPageHarvestedCreds.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHarvestedCreds.Size = new System.Drawing.Size(594, 377);
            this.tabPageHarvestedCreds.TabIndex = 2;
            this.tabPageHarvestedCreds.Text = "harvestedCreds";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.olvFoundCredentials, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCredsFound, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(588, 371);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCredsBack);
            this.panel1.Controls.Add(this.btnCredsContinue);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 339);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(582, 29);
            this.panel1.TabIndex = 0;
            // 
            // btnCredsBack
            // 
            this.btnCredsBack._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnCredsBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCredsBack.Location = new System.Drawing.Point(373, 2);
            this.btnCredsBack.Name = "btnCredsBack";
            this.btnCredsBack.Size = new System.Drawing.Size(100, 24);
            this.btnCredsBack.TabIndex = 2;
            this.btnCredsBack.Text = "Back";
            this.btnCredsBack.UseVisualStyleBackColor = true;
            this.btnCredsBack.Click += new System.EventHandler(this.btnCredsBack_Click);
            // 
            // btnCredsContinue
            // 
            this.btnCredsContinue._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnCredsContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCredsContinue.Location = new System.Drawing.Point(479, 2);
            this.btnCredsContinue.Name = "btnCredsContinue";
            this.btnCredsContinue.Size = new System.Drawing.Size(100, 24);
            this.btnCredsContinue.TabIndex = 1;
            this.btnCredsContinue.Text = "Continue";
            this.btnCredsContinue.UseVisualStyleBackColor = true;
            this.btnCredsContinue.Click += new System.EventHandler(this.btnCredsContinue_Click);
            // 
            // olvFoundCredentials
            // 
            this.olvFoundCredentials.AllColumns.Add(this.colTitle);
            this.olvFoundCredentials.AllColumns.Add(this.colUsername);
            this.olvFoundCredentials.AllColumns.Add(this.colDomain);
            this.olvFoundCredentials.AllColumns.Add(this.colPassword);
            this.olvFoundCredentials.CellEditUseWholeCell = false;
            this.olvFoundCredentials.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTitle,
            this.colUsername,
            this.colDomain,
            this.colPassword});
            this.olvFoundCredentials.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvFoundCredentials.DecorateLines = true;
            this.olvFoundCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvFoundCredentials.HasCollapsibleGroups = false;
            this.olvFoundCredentials.Location = new System.Drawing.Point(3, 23);
            this.olvFoundCredentials.Name = "olvFoundCredentials";
            this.olvFoundCredentials.ShowFilterMenuOnRightClick = false;
            this.olvFoundCredentials.Size = new System.Drawing.Size(582, 310);
            this.olvFoundCredentials.SortGroupItemsByPrimaryColumn = false;
            this.olvFoundCredentials.TabIndex = 4;
            this.olvFoundCredentials.UseCompatibleStateImageBehavior = false;
            this.olvFoundCredentials.UseNotifyPropertyChanged = true;
            this.olvFoundCredentials.UseOverlays = false;
            this.olvFoundCredentials.View = System.Windows.Forms.View.Details;
            // 
            // colTitle
            // 
            this.colTitle.AspectName = "Title";
            this.colTitle.Groupable = false;
            this.colTitle.Hideable = false;
            this.colTitle.Text = "Title";
            // 
            // colUsername
            // 
            this.colUsername.AspectName = "Username";
            this.colUsername.Groupable = false;
            this.colUsername.Hideable = false;
            this.colUsername.Text = "Username";
            // 
            // colDomain
            // 
            this.colDomain.AspectName = "Domain";
            this.colDomain.Groupable = false;
            this.colDomain.Hideable = false;
            this.colDomain.Text = "Domain";
            // 
            // colPassword
            // 
            this.colPassword.AspectName = "Password";
            this.colPassword.FillsFreeSpace = true;
            this.colPassword.Groupable = false;
            this.colPassword.Hideable = false;
            this.colPassword.Text = "Password";
            // 
            // lblCredsFound
            // 
            this.lblCredsFound.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCredsFound.AutoSize = true;
            this.lblCredsFound.Location = new System.Drawing.Point(3, 3);
            this.lblCredsFound.Name = "lblCredsFound";
            this.lblCredsFound.Size = new System.Drawing.Size(92, 13);
            this.lblCredsFound.TabIndex = 3;
            this.lblCredsFound.Text = "Credentials found:";
            // 
            // tabPageSaveRepo
            // 
            this.tabPageSaveRepo.BackColor = System.Drawing.Color.Transparent;
            this.tabPageSaveRepo.Controls.Add(this.gbSetPassword);
            this.tabPageSaveRepo.Controls.Add(this.gbWhereToSaveCredFile);
            this.tabPageSaveRepo.Controls.Add(this.textBoxConfConPathTab2);
            this.tabPageSaveRepo.Controls.Add(this.buttonExecuteUpgrade);
            this.tabPageSaveRepo.Controls.Add(this.labelConfConsPathHeaderOnTab2);
            this.tabPageSaveRepo.Controls.Add(this.buttonSaveRepoBack);
            this.tabPageSaveRepo.Location = new System.Drawing.Point(4, 24);
            this.tabPageSaveRepo.Name = "tabPageSaveRepo";
            this.tabPageSaveRepo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSaveRepo.Size = new System.Drawing.Size(594, 377);
            this.tabPageSaveRepo.TabIndex = 1;
            this.tabPageSaveRepo.Text = "saveRepoPage";
            // 
            // gbSetPassword
            // 
            this.gbSetPassword.Controls.Add(this.newRepositoryPasswordEntry);
            this.gbSetPassword.Location = new System.Drawing.Point(11, 209);
            this.gbSetPassword.Name = "gbSetPassword";
            this.gbSetPassword.Size = new System.Drawing.Size(572, 119);
            this.gbSetPassword.TabIndex = 11;
            this.gbSetPassword.TabStop = false;
            this.gbSetPassword.Text = "Set password for the credential repository";
            // 
            // newRepositoryPasswordEntry
            // 
            this.newRepositoryPasswordEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newRepositoryPasswordEntry.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newRepositoryPasswordEntry.Location = new System.Drawing.Point(3, 16);
            this.newRepositoryPasswordEntry.MinimumSize = new System.Drawing.Size(0, 100);
            this.newRepositoryPasswordEntry.Name = "newRepositoryPasswordEntry";
            this.newRepositoryPasswordEntry.PasswordChar = '\0';
            this.newRepositoryPasswordEntry.Size = new System.Drawing.Size(566, 100);
            this.newRepositoryPasswordEntry.TabIndex = 3;
            this.newRepositoryPasswordEntry.UseSystemPasswordChar = true;
            this.newRepositoryPasswordEntry.Verified += new System.EventHandler(this.newRepositoryPasswordEntry_Verified);
            this.newRepositoryPasswordEntry.NotVerified += new System.EventHandler(this.newRepositoryPasswordEntry_NotVerified);
            // 
            // gbWhereToSaveCredFile
            // 
            this.gbWhereToSaveCredFile.Controls.Add(this.buttonNewRepoPathBrowse);
            this.gbWhereToSaveCredFile.Controls.Add(this.textBoxCredRepoPath);
            this.gbWhereToSaveCredFile.Location = new System.Drawing.Point(11, 113);
            this.gbWhereToSaveCredFile.Name = "gbWhereToSaveCredFile";
            this.gbWhereToSaveCredFile.Size = new System.Drawing.Size(572, 90);
            this.gbWhereToSaveCredFile.TabIndex = 10;
            this.gbWhereToSaveCredFile.TabStop = false;
            this.gbWhereToSaveCredFile.Text = "Where should we save the new credential file?";
            // 
            // buttonNewRepoPathBrowse
            // 
            this.buttonNewRepoPathBrowse._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonNewRepoPathBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNewRepoPathBrowse.Location = new System.Drawing.Point(466, 45);
            this.buttonNewRepoPathBrowse.Name = "buttonNewRepoPathBrowse";
            this.buttonNewRepoPathBrowse.Size = new System.Drawing.Size(100, 24);
            this.buttonNewRepoPathBrowse.TabIndex = 8;
            this.buttonNewRepoPathBrowse.Text = "Browse";
            this.buttonNewRepoPathBrowse.UseVisualStyleBackColor = true;
            this.buttonNewRepoPathBrowse.Click += new System.EventHandler(this.buttonNewRepoPathBrowse_Click);
            // 
            // textBoxCredRepoPath
            // 
            this.textBoxCredRepoPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCredRepoPath.Location = new System.Drawing.Point(6, 19);
            this.textBoxCredRepoPath.Name = "textBoxCredRepoPath";
            this.textBoxCredRepoPath.Size = new System.Drawing.Size(560, 20);
            this.textBoxCredRepoPath.TabIndex = 6;
            this.textBoxCredRepoPath.TextChanged += new System.EventHandler(this.textBoxCredRepoPath_TextChanged);
            // 
            // textBoxConfConPathTab2
            // 
            this.textBoxConfConPathTab2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConfConPathTab2.Location = new System.Drawing.Point(9, 32);
            this.textBoxConfConPathTab2.Multiline = true;
            this.textBoxConfConPathTab2.Name = "textBoxConfConPathTab2";
            this.textBoxConfConPathTab2.ReadOnly = true;
            this.textBoxConfConPathTab2.Size = new System.Drawing.Size(574, 41);
            this.textBoxConfConPathTab2.TabIndex = 9;
            // 
            // buttonExecuteUpgrade
            // 
            this.buttonExecuteUpgrade._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonExecuteUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExecuteUpgrade.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonExecuteUpgrade.Enabled = false;
            this.buttonExecuteUpgrade.Location = new System.Drawing.Point(483, 345);
            this.buttonExecuteUpgrade.Name = "buttonExecuteUpgrade";
            this.buttonExecuteUpgrade.Size = new System.Drawing.Size(100, 24);
            this.buttonExecuteUpgrade.TabIndex = 5;
            this.buttonExecuteUpgrade.Text = "Upgrade";
            this.buttonExecuteUpgrade.UseVisualStyleBackColor = true;
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
            // buttonSaveRepoBack
            // 
            this.buttonSaveRepoBack._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonSaveRepoBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveRepoBack.Location = new System.Drawing.Point(377, 345);
            this.buttonSaveRepoBack.Name = "buttonSaveRepoBack";
            this.buttonSaveRepoBack.Size = new System.Drawing.Size(100, 24);
            this.buttonSaveRepoBack.TabIndex = 0;
            this.buttonSaveRepoBack.Text = "Back";
            this.buttonSaveRepoBack.UseVisualStyleBackColor = true;
            this.buttonSaveRepoBack.Click += new System.EventHandler(this.buttonSaveRepoBack_Click);
            // 
            // CredentialManagerUpgradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(602, 405);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CredentialManagerUpgradeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credential Manager Upgrade";
            this.tabControl.ResumeLayout(false);
            this.tabPageWelcome.ResumeLayout(false);
            this.tabPageWelcome.PerformLayout();
            this.tabPageHarvestedCreds.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvFoundCredentials)).EndInit();
            this.tabPageSaveRepo.ResumeLayout(false);
            this.tabPageSaveRepo.PerformLayout();
            this.gbSetPassword.ResumeLayout(false);
            this.gbWhereToSaveCredFile.ResumeLayout(false);
            this.gbWhereToSaveCredFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Base.NGLabel labelDescriptionOfUpgrade;
        private Controls.Base.NGButton buttonBeginUpgrade;
        private Controls.Base.NGButton buttonOpenFile;
        private Controls.Base.NGButton buttonNewFile;
        private Controls.Base.NGButton buttonExit;
        private mRemoteNG.UI.Controls.HeadlessTabControl tabControl;
        private System.Windows.Forms.TabPage tabPageWelcome;
        private System.Windows.Forms.TabPage tabPageSaveRepo;
        private Controls.Base.NGButton buttonSaveRepoBack;
        private Controls.Base.NGLabel labelConfConsPathHeaderOnTab2;
        private Controls.Base.NGButton buttonExecuteUpgrade;
        private Controls.NewPasswordWithVerification newRepositoryPasswordEntry;
        private Controls.Base.NGTextBox textBoxCredRepoPath;
        private System.Windows.Forms.SaveFileDialog newCredRepoPathDialog;
        private Controls.Base.NGButton buttonNewRepoPathBrowse;
        private Controls.Base.NGLabel labelConfConsPathHeaderOnTab1;
        private Controls.Base.NGTextBox textBoxConfConPathTab1;
        private Controls.Base.NGTextBox textBoxConfConPathTab2;
        private System.Windows.Forms.SaveFileDialog newConnectionsFileDialog;
        private System.Windows.Forms.TabPage tabPageHarvestedCreds;
        private Controls.Base.NGButton btnCredsBack;
        private Controls.Base.NGButton btnCredsContinue;
        private Controls.Base.NGLabel lblCredsFound;
        private Controls.Base.NGListView olvFoundCredentials;
        private BrightIdeasSoftware.OLVColumn colUsername;
        private BrightIdeasSoftware.OLVColumn colDomain;
        private BrightIdeasSoftware.OLVColumn colPassword;
        private BrightIdeasSoftware.OLVColumn colTitle;
        private System.Windows.Forms.GroupBox gbSetPassword;
        private System.Windows.Forms.GroupBox gbWhereToSaveCredFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
    }
}