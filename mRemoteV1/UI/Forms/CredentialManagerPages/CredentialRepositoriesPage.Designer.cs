using mRemoteNG.Credential.Repositories;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
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
            CredentialRepositoryList credentialRepositoryList1 = new CredentialRepositoryList();
            this.buttonAdd = new Controls.Base.NGButton();
            this.buttonRemove = new Controls.Base.NGButton();
            this.buttonEdit = new Controls.Base.NGButton();
            this.credentialRepositoryListView = new mRemoteNG.UI.Controls.CredentialRepositoryListView();
            this.buttonToggleLoad = new Controls.Base.NGButton();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Image = global::mRemoteNG.Resources.key_add;
            this.buttonAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAdd.Location = new System.Drawing.Point(105, 237);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(99, 32);
            this.buttonAdd.TabIndex = 5;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Image = global::mRemoteNG.Resources.key_delete;
            this.buttonRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRemove.Location = new System.Drawing.Point(315, 237);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(99, 32);
            this.buttonRemove.TabIndex = 6;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEdit.Enabled = false;
            this.buttonEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEdit.Location = new System.Drawing.Point(210, 237);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(99, 32);
            this.buttonEdit.TabIndex = 8;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // credentialRepositoryListView
            // 
            this.credentialRepositoryListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.credentialRepositoryListView.CredentialRepositoryList = credentialRepositoryList1;
            this.credentialRepositoryListView.DoubleClickHandler = null;
            this.credentialRepositoryListView.Location = new System.Drawing.Point(0, 0);
            this.credentialRepositoryListView.Name = "credentialRepositoryListView";
            this.credentialRepositoryListView.RepositoryFilter = null;
            this.credentialRepositoryListView.Size = new System.Drawing.Size(417, 231);
            this.credentialRepositoryListView.TabIndex = 9;
            // 
            // buttonToggleLoad
            // 
            this.buttonToggleLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonToggleLoad.Enabled = false;
            this.buttonToggleLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonToggleLoad.Location = new System.Drawing.Point(3, 237);
            this.buttonToggleLoad.Name = "buttonToggleLoad";
            this.buttonToggleLoad.Size = new System.Drawing.Size(99, 32);
            this.buttonToggleLoad.TabIndex = 10;
            this.buttonToggleLoad.Text = "Load";
            this.buttonToggleLoad.UseVisualStyleBackColor = true;
            this.buttonToggleLoad.Click += new System.EventHandler(this.buttonToggleLoad_Click);
            // 
            // CredentialRepositoriesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonToggleLoad);
            this.Controls.Add(this.credentialRepositoryListView);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonRemove);
            this.Name = "CredentialRepositoriesPage";
            this.Size = new System.Drawing.Size(417, 272);
            this.ResumeLayout(false);

        }

        #endregion

       private Controls.Base.NGButton buttonAdd;
       private Controls.Base.NGButton buttonRemove;
       private Controls.Base.NGButton buttonEdit;
        private Controls.CredentialRepositoryListView credentialRepositoryListView;
       private Controls.Base.NGButton buttonToggleLoad;
    }
}
