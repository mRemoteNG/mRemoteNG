namespace mRemoteNG.UI.Controls
{
    partial class CredentialRecordListView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.objectListView1 = new Base.NGListView();
            this.olvColumnCredentialId = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnUsername = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDomain = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnRepositorySource = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnRepositoryTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnTitle);
            this.objectListView1.AllColumns.Add(this.olvColumnUsername);
            this.objectListView1.AllColumns.Add(this.olvColumnDomain);
            this.objectListView1.AllColumns.Add(this.olvColumnCredentialId);
            this.objectListView1.AllColumns.Add(this.olvColumnRepositoryTitle);
            this.objectListView1.AllColumns.Add(this.olvColumnRepositorySource);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnTitle,
            this.olvColumnUsername,
            this.olvColumnDomain,
            this.olvColumnRepositoryTitle});
            this.objectListView1.CopySelectionOnControlC = false;
            this.objectListView1.CopySelectionOnControlCUsesDragSource = false;
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(0, 0);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.SelectAllOnControlA = false;
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(367, 204);
            this.objectListView1.TabIndex = 2;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.UseNotifyPropertyChanged = true;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnCredentialId
            // 
            this.olvColumnCredentialId.AspectName = "";
            this.olvColumnCredentialId.DisplayIndex = 0;
            this.olvColumnCredentialId.IsEditable = false;
            this.olvColumnCredentialId.IsVisible = false;
            this.olvColumnCredentialId.Text = "Credential ID";
            // 
            // olvColumnTitle
            // 
            this.olvColumnTitle.AspectName = "";
            this.olvColumnTitle.Groupable = false;
            this.olvColumnTitle.Text = "Title";
            // 
            // olvColumnUsername
            // 
            this.olvColumnUsername.AspectName = "";
            this.olvColumnUsername.Text = "Username";
            // 
            // olvColumnDomain
            // 
            this.olvColumnDomain.AspectName = "";
            this.olvColumnDomain.Text = "Domain";
            // 
            // olvColumnRepositorySource
            // 
            this.olvColumnRepositorySource.DisplayIndex = 4;
            this.olvColumnRepositorySource.IsVisible = false;
            this.olvColumnRepositorySource.Text = "Source";
            // 
            // olvColumnRepositoryTitle
            // 
            this.olvColumnRepositoryTitle.Text = "Repository Title";
            // 
            // CredentialRecordListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectListView1);
            this.Name = "CredentialRecordListView";
            this.Size = new System.Drawing.Size(367, 204);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Base.NGListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnCredentialId;
        private BrightIdeasSoftware.OLVColumn olvColumnTitle;
        private BrightIdeasSoftware.OLVColumn olvColumnUsername;
        private BrightIdeasSoftware.OLVColumn olvColumnDomain;
        private BrightIdeasSoftware.OLVColumn olvColumnRepositorySource;
        private BrightIdeasSoftware.OLVColumn olvColumnRepositoryTitle;
    }
}
