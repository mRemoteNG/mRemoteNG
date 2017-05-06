using mRemoteNG.Credential.Repositories;

namespace mRemoteNG.UI.Forms.CredentialManagerPages
{
    partial class CredentialRepositorySelectionPage
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
            this.credentialRepositoryListView = new mRemoteNG.UI.Controls.CredentialRepositoryListView();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.credentialRepositoryListView.Size = new System.Drawing.Size(368, 137);
            this.credentialRepositoryListView.TabIndex = 0;
            // 
            // buttonContinue
            // 
            this.buttonContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonContinue.Location = new System.Drawing.Point(278, 143);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(75, 23);
            this.buttonContinue.TabIndex = 1;
            this.buttonContinue.Text = "Continue";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBack.Location = new System.Drawing.Point(197, 143);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 2;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // CredentialRepositorySelectionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonContinue);
            this.Controls.Add(this.credentialRepositoryListView);
            this.Name = "CredentialRepositorySelectionPage";
            this.Size = new System.Drawing.Size(368, 169);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CredentialRepositoryListView credentialRepositoryListView;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.Button buttonBack;
    }
}
