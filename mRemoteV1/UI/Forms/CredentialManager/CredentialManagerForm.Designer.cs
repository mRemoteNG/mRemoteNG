namespace mRemoteNG.UI.Forms.CredentialManager
{
    partial class CredentialManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredentialManagerForm));
            this.panelMain = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnEditRepo = new mRemoteNG.UI.Controls.Base.NGButton();
            this.btnToggleUnlock = new mRemoteNG.UI.Controls.Base.NGButton();
            this.btnRemoveRepo = new mRemoteNG.UI.Controls.Base.NGButton();
            this.btnAddRepo = new mRemoteNG.UI.Controls.Base.NGButton();
            this.buttonClose = new mRemoteNG.UI.Controls.Base.NGButton();
            this.olvCredRepos = new mRemoteNG.UI.Controls.Base.NGListView();
            this.colCredRepoTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvCredRepos)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Location = new System.Drawing.Point(152, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(555, 375);
            this.panelMain.TabIndex = 4;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnEditRepo);
            this.pnlBottom.Controls.Add(this.btnToggleUnlock);
            this.pnlBottom.Controls.Add(this.btnRemoveRepo);
            this.pnlBottom.Controls.Add(this.btnAddRepo);
            this.pnlBottom.Controls.Add(this.buttonClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 374);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(707, 50);
            this.pnlBottom.TabIndex = 6;
            // 
            // btnEditRepo
            // 
            this.btnEditRepo._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnEditRepo.Location = new System.Drawing.Point(71, 24);
            this.btnEditRepo.Name = "btnEditRepo";
            this.btnEditRepo.Size = new System.Drawing.Size(75, 23);
            this.btnEditRepo.TabIndex = 5;
            this.btnEditRepo.Text = "Edit";
            this.btnEditRepo.UseVisualStyleBackColor = true;
            this.btnEditRepo.Click += new System.EventHandler(this.btnEditRepo_Click);
            // 
            // btnToggleUnlock
            // 
            this.btnToggleUnlock._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnToggleUnlock.Location = new System.Drawing.Point(0, 24);
            this.btnToggleUnlock.Name = "btnToggleUnlock";
            this.btnToggleUnlock.Size = new System.Drawing.Size(75, 23);
            this.btnToggleUnlock.TabIndex = 4;
            this.btnToggleUnlock.Text = "Unlock";
            this.btnToggleUnlock.UseVisualStyleBackColor = true;
            this.btnToggleUnlock.Click += new System.EventHandler(this.btnToggleUnlock_Click);
            // 
            // btnRemoveRepo
            // 
            this.btnRemoveRepo._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnRemoveRepo.Location = new System.Drawing.Point(71, 0);
            this.btnRemoveRepo.Name = "btnRemoveRepo";
            this.btnRemoveRepo.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveRepo.TabIndex = 3;
            this.btnRemoveRepo.Text = "Remove";
            this.btnRemoveRepo.UseVisualStyleBackColor = true;
            this.btnRemoveRepo.Click += new System.EventHandler(this.btnRemoveRepo_Click);
            // 
            // btnAddRepo
            // 
            this.btnAddRepo._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnAddRepo.Location = new System.Drawing.Point(0, 0);
            this.btnAddRepo.Name = "btnAddRepo";
            this.btnAddRepo.Size = new System.Drawing.Size(75, 23);
            this.btnAddRepo.TabIndex = 2;
            this.btnAddRepo.Text = "Add";
            this.btnAddRepo.UseVisualStyleBackColor = true;
            this.btnAddRepo.Click += new System.EventHandler(this.btnAddRepo_Click);
            // 
            // buttonClose
            // 
            this.buttonClose._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(620, 15);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // olvCredRepos
            // 
            this.olvCredRepos.AllColumns.Add(this.colCredRepoTitle);
            this.olvCredRepos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.olvCredRepos.CellEditUseWholeCell = false;
            this.olvCredRepos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCredRepoTitle});
            this.olvCredRepos.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvCredRepos.DecorateLines = true;
            this.olvCredRepos.FullRowSelect = true;
            this.olvCredRepos.HasCollapsibleGroups = false;
            this.olvCredRepos.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.olvCredRepos.HideSelection = false;
            this.olvCredRepos.IsSearchOnSortColumn = false;
            this.olvCredRepos.Location = new System.Drawing.Point(0, 0);
            this.olvCredRepos.MultiSelect = false;
            this.olvCredRepos.Name = "olvCredRepos";
            this.olvCredRepos.RowHeight = 30;
            this.olvCredRepos.SelectAllOnControlA = false;
            this.olvCredRepos.ShowFilterMenuOnRightClick = false;
            this.olvCredRepos.ShowGroups = false;
            this.olvCredRepos.ShowHeaderInAllViews = false;
            this.olvCredRepos.Size = new System.Drawing.Size(154, 375);
            this.olvCredRepos.TabIndex = 5;
            this.olvCredRepos.UseCompatibleStateImageBehavior = false;
            this.olvCredRepos.View = System.Windows.Forms.View.Details;
            this.olvCredRepos.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.olvCredRepos_FormatRow);
            // 
            // colCredRepoTitle
            // 
            this.colCredRepoTitle.AspectName = "";
            this.colCredRepoTitle.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
            this.colCredRepoTitle.FillsFreeSpace = true;
            this.colCredRepoTitle.Groupable = false;
            this.colCredRepoTitle.Hideable = false;
            this.colCredRepoTitle.IsEditable = false;
            this.colCredRepoTitle.Searchable = false;
            this.colCredRepoTitle.Sortable = false;
            this.colCredRepoTitle.Text = "Repositories";
            this.colCredRepoTitle.Width = 120;
            // 
            // CredentialManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 424);
            this.Controls.Add(this.olvCredRepos);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.pnlBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CredentialManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Credential Manager";
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvCredRepos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelMain;
        private Controls.Base.NGListView olvCredRepos;
        private System.Windows.Forms.Panel pnlBottom;
        private Controls.Base.NGButton buttonClose;
        private BrightIdeasSoftware.OLVColumn colCredRepoTitle;
        private Controls.Base.NGButton btnEditRepo;
        private Controls.Base.NGButton btnToggleUnlock;
        private Controls.Base.NGButton btnRemoveRepo;
        private Controls.Base.NGButton btnAddRepo;
    }
}