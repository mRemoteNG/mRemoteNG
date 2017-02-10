namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    partial class CredentialRepositoriesPage
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
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnProvider = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSource = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Image = global::mRemoteNG.Resources.key_add;
            this.buttonAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAdd.Location = new System.Drawing.Point(210, 269);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(99, 32);
            this.buttonAdd.TabIndex = 5;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Image = global::mRemoteNG.Resources.key_delete;
            this.buttonRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRemove.Location = new System.Drawing.Point(315, 269);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(99, 32);
            this.buttonRemove.TabIndex = 6;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnName);
            this.objectListView1.AllColumns.Add(this.olvColumnProvider);
            this.objectListView1.AllColumns.Add(this.olvColumnSource);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnName,
            this.olvColumnProvider,
            this.olvColumnSource});
            this.objectListView1.CopySelectionOnControlC = false;
            this.objectListView1.CopySelectionOnControlCUsesDragSource = false;
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.HideSelection = false;
            this.objectListView1.Location = new System.Drawing.Point(0, 0);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.SelectAllOnControlA = false;
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(417, 263);
            this.objectListView1.TabIndex = 7;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.UseNotifyPropertyChanged = true;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnName
            // 
            this.olvColumnName.AspectName = "Name";
            this.olvColumnName.FillsFreeSpace = true;
            this.olvColumnName.Groupable = false;
            this.olvColumnName.Text = "Name";
            // 
            // olvColumnProvider
            // 
            this.olvColumnProvider.FillsFreeSpace = true;
            this.olvColumnProvider.Groupable = false;
            this.olvColumnProvider.Hideable = false;
            this.olvColumnProvider.IsEditable = false;
            this.olvColumnProvider.Searchable = false;
            this.olvColumnProvider.Text = "Provider";
            // 
            // olvColumnSource
            // 
            this.olvColumnSource.FillsFreeSpace = true;
            this.olvColumnSource.Groupable = false;
            this.olvColumnSource.Text = "Source";
            // 
            // CredentialSourcesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectListView1);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonRemove);
            this.Name = "CredentialSourcesPage";
            this.Size = new System.Drawing.Size(417, 304);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnName;
        private BrightIdeasSoftware.OLVColumn olvColumnProvider;
        private BrightIdeasSoftware.OLVColumn olvColumnSource;
    }
}
