namespace mRemoteNG.UI.Forms.CredentialManager
{
    partial class CredentialManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredentialManagerForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msAddRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.msRemoveRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.msToggleUnlock = new System.Windows.Forms.ToolStripMenuItem();
            this.msEditRepo = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.olvCredRepos = new mRemoteNG.UI.Controls.Base.NGListView();
            this.colCredRepoTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.buttonClose = new mRemoteNG.UI.Controls.Base.NGButton();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvCredRepos)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msAddRepo,
            this.msRemoveRepo,
            this.msToggleUnlock,
            this.msEditRepo});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.ShowItemToolTips = true;
            this.menuStrip1.Size = new System.Drawing.Size(307, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msAddRepo
            // 
            this.msAddRepo.AutoToolTip = true;
            this.msAddRepo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.msAddRepo.Image = global::mRemoteNG.Resources.Connection_Add;
            this.msAddRepo.Name = "msAddRepo";
            this.msAddRepo.Size = new System.Drawing.Size(28, 20);
            this.msAddRepo.Text = "Add Repository";
            this.msAddRepo.Click += new System.EventHandler(this.msAddRepo_Click);
            // 
            // msRemoveRepo
            // 
            this.msRemoveRepo.AutoToolTip = true;
            this.msRemoveRepo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.msRemoveRepo.Image = global::mRemoteNG.Resources.Delete;
            this.msRemoveRepo.Name = "msRemoveRepo";
            this.msRemoveRepo.Size = new System.Drawing.Size(28, 20);
            this.msRemoveRepo.Text = "Delete Repository";
            this.msRemoveRepo.Click += new System.EventHandler(this.msRemoveRepo_Click);
            // 
            // msToggleUnlock
            // 
            this.msToggleUnlock.AutoToolTip = true;
            this.msToggleUnlock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.msToggleUnlock.Image = global::mRemoteNG.Resources.folder_key;
            this.msToggleUnlock.Name = "msToggleUnlock";
            this.msToggleUnlock.Size = new System.Drawing.Size(28, 20);
            this.msToggleUnlock.Text = "Lock Repository";
            this.msToggleUnlock.Click += new System.EventHandler(this.msToggleUnlock_Click);
            // 
            // msEditRepo
            // 
            this.msEditRepo.AutoToolTip = true;
            this.msEditRepo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.msEditRepo.Image = global::mRemoteNG.Resources.Properties;
            this.msEditRepo.Name = "msEditRepo";
            this.msEditRepo.Size = new System.Drawing.Size(28, 20);
            this.msEditRepo.Text = "Edit Repository";
            this.msEditRepo.Click += new System.EventHandler(this.msEditRepo_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.olvCredRepos);
            this.splitContainer.Panel1.Controls.Add(this.menuStrip1);
            this.splitContainer.Size = new System.Drawing.Size(925, 442);
            this.splitContainer.SplitterDistance = 307;
            this.splitContainer.TabIndex = 8;
            // 
            // olvCredRepos
            // 
            this.olvCredRepos.AllColumns.Add(this.colCredRepoTitle);
            this.olvCredRepos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.olvCredRepos.CellEditUseWholeCell = false;
            this.olvCredRepos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCredRepoTitle});
            this.olvCredRepos.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvCredRepos.DecorateLines = true;
            this.olvCredRepos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvCredRepos.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvCredRepos.FullRowSelect = true;
            this.olvCredRepos.HasCollapsibleGroups = false;
            this.olvCredRepos.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.olvCredRepos.HideSelection = false;
            this.olvCredRepos.IsSearchOnSortColumn = false;
            this.olvCredRepos.Location = new System.Drawing.Point(0, 24);
            this.olvCredRepos.MultiSelect = false;
            this.olvCredRepos.Name = "olvCredRepos";
            this.olvCredRepos.RowHeight = 30;
            this.olvCredRepos.SelectAllOnControlA = false;
            this.olvCredRepos.ShowFilterMenuOnRightClick = false;
            this.olvCredRepos.ShowGroups = false;
            this.olvCredRepos.ShowHeaderInAllViews = false;
            this.olvCredRepos.Size = new System.Drawing.Size(307, 418);
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
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.buttonClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 442);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(925, 35);
            this.pnlBottom.TabIndex = 9;
            // 
            // buttonClose
            // 
            this.buttonClose._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(816, 6);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(100, 24);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // CredentialManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 477);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "CredentialManagerForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Credential Manager";
            this.TopMost = true;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvCredRepos)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.Base.NGListView olvCredRepos;
        private Controls.Base.NGButton buttonClose;
        private BrightIdeasSoftware.OLVColumn colCredRepoTitle;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripMenuItem msAddRepo;
        private System.Windows.Forms.ToolStripMenuItem msRemoveRepo;
        private System.Windows.Forms.ToolStripMenuItem msToggleUnlock;
        private System.Windows.Forms.ToolStripMenuItem msEditRepo;
        private System.Windows.Forms.Panel pnlBottom;
    }
}