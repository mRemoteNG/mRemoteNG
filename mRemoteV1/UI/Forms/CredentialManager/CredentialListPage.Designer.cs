namespace mRemoteNG.UI.Forms.CredentialManager
{
    sealed partial class CredentialListPage
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
            mRemoteNG.Credential.Repositories.CredentialRepositoryList credentialRepositoryList1 = new mRemoteNG.Credential.Repositories.CredentialRepositoryList();
            this.credentialRecordListView = new mRemoteNG.UI.Controls.CredentialRecordListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.msRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.msEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // credentialRecordListView
            // 
            this.credentialRecordListView.CredentialRepositoryList = credentialRepositoryList1;
            this.credentialRecordListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.credentialRecordListView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.credentialRecordListView.Location = new System.Drawing.Point(0, 24);
            this.credentialRecordListView.Name = "credentialRecordListView";
            this.credentialRecordListView.Size = new System.Drawing.Size(383, 252);
            this.credentialRecordListView.TabIndex = 6;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msAdd,
            this.msRemove,
            this.msEdit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.ShowItemToolTips = true;
            this.menuStrip1.Size = new System.Drawing.Size(383, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msAdd
            // 
            this.msAdd.AutoToolTip = true;
            this.msAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.msAdd.Image = global::mRemoteNG.Resources.key_add;
            this.msAdd.Name = "msAdd";
            this.msAdd.Size = new System.Drawing.Size(28, 20);
            this.msAdd.Text = "Add Record";
            this.msAdd.Click += new System.EventHandler(this.msAdd_Click);
            // 
            // msRemove
            // 
            this.msRemove.AutoToolTip = true;
            this.msRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.msRemove.Image = global::mRemoteNG.Resources.key_delete;
            this.msRemove.Name = "msRemove";
            this.msRemove.Size = new System.Drawing.Size(28, 20);
            this.msRemove.Text = "Remove Record";
            this.msRemove.Click += new System.EventHandler(this.msRemove_Click);
            // 
            // msEdit
            // 
            this.msEdit.AutoToolTip = true;
            this.msEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.msEdit.Image = global::mRemoteNG.Resources.cog;
            this.msEdit.Name = "msEdit";
            this.msEdit.Size = new System.Drawing.Size(28, 20);
            this.msEdit.Text = "Edit Record";
            this.msEdit.Click += new System.EventHandler(this.msEdit_Click);
            // 
            // CredentialListPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.credentialRecordListView);
            this.Controls.Add(this.menuStrip1);
            this.Name = "CredentialListPage";
            this.Size = new System.Drawing.Size(383, 276);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Controls.CredentialRecordListView credentialRecordListView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msAdd;
        private System.Windows.Forms.ToolStripMenuItem msEdit;
        private System.Windows.Forms.ToolStripMenuItem msRemove;
    }
}
