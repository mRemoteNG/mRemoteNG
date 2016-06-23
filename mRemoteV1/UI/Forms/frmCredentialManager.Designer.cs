namespace mRemoteNG.UI.Forms
{
    partial class FrmCredentialManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCredentialManager));
            this.lvCredentialList = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddCredential = new System.Windows.Forms.Button();
            this.btnRemoveCredential = new System.Windows.Forms.Button();
            this.btnEditCredential = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvCredentialList
            // 
            this.lvCredentialList.Location = new System.Drawing.Point(12, 11);
            this.lvCredentialList.Name = "lvCredentialList";
            this.lvCredentialList.Size = new System.Drawing.Size(430, 309);
            this.lvCredentialList.TabIndex = 0;
            this.lvCredentialList.UseCompatibleStateImageBehavior = false;
            this.lvCredentialList.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(367, 326);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddCredential
            // 
            this.btnAddCredential.Image = global::mRemoteNG.Resources.key_add;
            this.btnAddCredential.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddCredential.Location = new System.Drawing.Point(12, 326);
            this.btnAddCredential.Name = "btnAddCredential";
            this.btnAddCredential.Size = new System.Drawing.Size(86, 23);
            this.btnAddCredential.TabIndex = 1;
            this.btnAddCredential.Text = "Add";
            this.btnAddCredential.UseVisualStyleBackColor = true;
            this.btnAddCredential.Click += new System.EventHandler(this.btnAddCredential_Click);
            // 
            // btnRemoveCredential
            // 
            this.btnRemoveCredential.Image = global::mRemoteNG.Resources.key_delete;
            this.btnRemoveCredential.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveCredential.Location = new System.Drawing.Point(199, 326);
            this.btnRemoveCredential.Name = "btnRemoveCredential";
            this.btnRemoveCredential.Size = new System.Drawing.Size(86, 23);
            this.btnRemoveCredential.TabIndex = 3;
            this.btnRemoveCredential.Text = "Remove";
            this.btnRemoveCredential.UseVisualStyleBackColor = true;
            // 
            // btnEditCredential
            // 
            this.btnEditCredential.Location = new System.Drawing.Point(104, 326);
            this.btnEditCredential.Name = "btnEditCredential";
            this.btnEditCredential.Size = new System.Drawing.Size(89, 23);
            this.btnEditCredential.TabIndex = 2;
            this.btnEditCredential.Text = "Edit";
            this.btnEditCredential.UseVisualStyleBackColor = true;
            this.btnEditCredential.Click += new System.EventHandler(this.btnEditCredential_Click);
            // 
            // FrmCredentialManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(454, 365);
            this.Controls.Add(this.btnEditCredential);
            this.Controls.Add(this.btnRemoveCredential);
            this.Controls.Add(this.btnAddCredential);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvCredentialList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmCredentialManager";
            this.Text = "Credential Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvCredentialList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddCredential;
        private System.Windows.Forms.Button btnRemoveCredential;
        private System.Windows.Forms.Button btnEditCredential;
    }
}