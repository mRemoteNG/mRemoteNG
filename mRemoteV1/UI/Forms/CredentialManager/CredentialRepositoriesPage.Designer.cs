using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.UI.Forms.CredentialManager
{
    sealed partial class CredentialRepositoriesPage
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
            this.credentialRepositoryListView = new mRemoteNG.UI.Controls.CredentialRepositoryListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msToggleLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.msAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.msEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.msRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // credentialRepositoryListView
            // 
            this.credentialRepositoryListView.CredentialRepositoryList = credentialRepositoryList1;
            this.credentialRepositoryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.credentialRepositoryListView.DoubleClickHandler = null;
            this.credentialRepositoryListView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.credentialRepositoryListView.Location = new System.Drawing.Point(0, 24);
            this.credentialRepositoryListView.Name = "credentialRepositoryListView";
            this.credentialRepositoryListView.RepositoryFilter = null;
            this.credentialRepositoryListView.Size = new System.Drawing.Size(417, 248);
            this.credentialRepositoryListView.TabIndex = 9;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msToggleLoad,
            this.msAdd,
            this.msEdit,
            this.msRemove});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(417, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msToggleLoad
            // 
            this.msToggleLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.msToggleLoad.Image = global::mRemoteNG.Resources.Connection_Add;
            this.msToggleLoad.Name = "msToggleLoad";
            this.msToggleLoad.Size = new System.Drawing.Size(97, 20);
            this.msToggleLoad.Text = "msToggleLoad";
            this.msToggleLoad.Click += new System.EventHandler(this.buttonToggleLoad_Click);
            // 
            // msAdd
            // 
            this.msAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.msAdd.Image = global::mRemoteNG.Resources.Delete;
            this.msAdd.Name = "msAdd";
            this.msAdd.Size = new System.Drawing.Size(57, 20);
            this.msAdd.Text = "msAdd";
            this.msAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // msEdit
            // 
            this.msEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.msEdit.Image = global::mRemoteNG.Resources.cog;
            this.msEdit.Name = "msEdit";
            this.msEdit.Size = new System.Drawing.Size(28, 20);
            this.msEdit.Text = "msEdit";
            this.msEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // msRemove
            // 
            this.msRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.msRemove.Image = global::mRemoteNG.Resources.Delete;
            this.msRemove.Name = "msRemove";
            this.msRemove.Size = new System.Drawing.Size(28, 20);
            this.msRemove.Text = "msRemove";
            this.msRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // CredentialRepositoriesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.credentialRepositoryListView);
            this.Controls.Add(this.menuStrip1);
            this.Name = "CredentialRepositoriesPage";
            this.Size = new System.Drawing.Size(417, 272);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Controls.CredentialRepositoryListView credentialRepositoryListView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msToggleLoad;
        private System.Windows.Forms.ToolStripMenuItem msAdd;
        private System.Windows.Forms.ToolStripMenuItem msEdit;
        private System.Windows.Forms.ToolStripMenuItem msRemove;
    }
}
