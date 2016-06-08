namespace mRemoteNG.UI.Forms
{
    partial class frmCredentialManager
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddCredential = new System.Windows.Forms.Button();
            this.btnRemoveCredential = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(189, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(367, 309);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(481, 357);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnAddCredential
            // 
            this.btnAddCredential.Location = new System.Drawing.Point(189, 327);
            this.btnAddCredential.Name = "btnAddCredential";
            this.btnAddCredential.Size = new System.Drawing.Size(75, 23);
            this.btnAddCredential.TabIndex = 2;
            this.btnAddCredential.Text = "Add";
            this.btnAddCredential.UseVisualStyleBackColor = true;
            // 
            // btnRemoveCredential
            // 
            this.btnRemoveCredential.Location = new System.Drawing.Point(271, 326);
            this.btnRemoveCredential.Name = "btnRemoveCredential";
            this.btnRemoveCredential.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveCredential.TabIndex = 3;
            this.btnRemoveCredential.Text = "Remove";
            this.btnRemoveCredential.UseVisualStyleBackColor = true;
            // 
            // frmCredentialManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 392);
            this.Controls.Add(this.btnRemoveCredential);
            this.Controls.Add(this.btnAddCredential);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.listView1);
            this.Name = "frmCredentialManager";
            this.Text = "Credential Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddCredential;
        private System.Windows.Forms.Button btnRemoveCredential;
    }
}