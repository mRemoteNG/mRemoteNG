namespace mRemoteNG.UI.Controls
{
    partial class CredentialRepositoryListView
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
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnTitle = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnProvider = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSource = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnId = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnIsLoaded = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnTitle);
            this.objectListView1.AllColumns.Add(this.olvColumnProvider);
            this.objectListView1.AllColumns.Add(this.olvColumnSource);
            this.objectListView1.AllColumns.Add(this.olvColumnId);
            this.objectListView1.AllColumns.Add(this.olvColumnIsLoaded);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnTitle,
            this.olvColumnProvider,
            this.olvColumnSource,
            this.olvColumnId,
            this.olvColumnIsLoaded});
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
            this.objectListView1.Size = new System.Drawing.Size(308, 171);
            this.objectListView1.TabIndex = 8;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.UseNotifyPropertyChanged = true;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnTitle
            // 
            this.olvColumnTitle.Groupable = false;
            this.olvColumnTitle.Text = "Title";
            // 
            // olvColumnProvider
            // 
            this.olvColumnProvider.AspectName = "";
            this.olvColumnProvider.Groupable = false;
            this.olvColumnProvider.Hideable = false;
            this.olvColumnProvider.IsEditable = false;
            this.olvColumnProvider.Searchable = false;
            this.olvColumnProvider.Text = "Provider";
            // 
            // olvColumnSource
            // 
            this.olvColumnSource.AspectName = "";
            this.olvColumnSource.Groupable = false;
            this.olvColumnSource.Text = "Source";
            // 
            // olvColumnId
            // 
            this.olvColumnId.Text = "ID";
            // 
            // olvColumnIsLoaded
            // 
            this.olvColumnIsLoaded.IsEditable = false;
            this.olvColumnIsLoaded.Text = "Loaded";
            // 
            // CredentialRepositoryListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectListView1);
            this.Name = "CredentialRepositoryListView";
            this.Size = new System.Drawing.Size(308, 171);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnProvider;
        private BrightIdeasSoftware.OLVColumn olvColumnSource;
        private BrightIdeasSoftware.OLVColumn olvColumnTitle;
        private BrightIdeasSoftware.OLVColumn olvColumnId;
        private BrightIdeasSoftware.OLVColumn olvColumnIsLoaded;
    }
}
