namespace mRemoteNG.UI.Forms
{
    partial class CompositeCredentialRepoUnlockerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompositeCredentialRepoUnlockerForm));
            this.objectListViewRepos = new BrightIdeasSoftware.ObjectListView();
            this.labelUnlocking = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.secureTextBoxPassword = new mRemoteNG.UI.Controls.SecureTextBox();
            this.buttonUnlock = new System.Windows.Forms.Button();
            this.buttonSkip = new System.Windows.Forms.Button();
            this.labelRepoTitle = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelRepoSource = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewRepos)).BeginInit();
            this.SuspendLayout();
            // 
            // objectListViewRepos
            // 
            this.objectListViewRepos.CellEditUseWholeCell = false;
            this.objectListViewRepos.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewRepos.Dock = System.Windows.Forms.DockStyle.Left;
            this.objectListViewRepos.Location = new System.Drawing.Point(0, 0);
            this.objectListViewRepos.Name = "objectListViewRepos";
            this.objectListViewRepos.Size = new System.Drawing.Size(141, 212);
            this.objectListViewRepos.TabIndex = 0;
            this.objectListViewRepos.UseCompatibleStateImageBehavior = false;
            this.objectListViewRepos.View = System.Windows.Forms.View.Details;
            // 
            // labelUnlocking
            // 
            this.labelUnlocking.AutoSize = true;
            this.labelUnlocking.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUnlocking.Location = new System.Drawing.Point(147, 9);
            this.labelUnlocking.Name = "labelUnlocking";
            this.labelUnlocking.Size = new System.Drawing.Size(81, 16);
            this.labelUnlocking.TabIndex = 1;
            this.labelUnlocking.Text = "Unlocking:";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(156, 116);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(53, 13);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "Password";
            // 
            // secureTextBoxPassword
            // 
            this.secureTextBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secureTextBoxPassword.Location = new System.Drawing.Point(159, 132);
            this.secureTextBoxPassword.Name = "secureTextBoxPassword";
            this.secureTextBoxPassword.Size = new System.Drawing.Size(233, 20);
            this.secureTextBoxPassword.TabIndex = 3;
            // 
            // buttonUnlock
            // 
            this.buttonUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUnlock.Location = new System.Drawing.Point(236, 177);
            this.buttonUnlock.Name = "buttonUnlock";
            this.buttonUnlock.Size = new System.Drawing.Size(75, 23);
            this.buttonUnlock.TabIndex = 4;
            this.buttonUnlock.Text = "Unlock";
            this.buttonUnlock.UseVisualStyleBackColor = true;
            // 
            // buttonSkip
            // 
            this.buttonSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSkip.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSkip.Location = new System.Drawing.Point(317, 177);
            this.buttonSkip.Name = "buttonSkip";
            this.buttonSkip.Size = new System.Drawing.Size(75, 23);
            this.buttonSkip.TabIndex = 5;
            this.buttonSkip.Text = "Skip";
            this.buttonSkip.UseVisualStyleBackColor = true;
            // 
            // labelRepoTitle
            // 
            this.labelRepoTitle.AutoSize = true;
            this.labelRepoTitle.Location = new System.Drawing.Point(157, 34);
            this.labelRepoTitle.Name = "labelRepoTitle";
            this.labelRepoTitle.Size = new System.Drawing.Size(27, 13);
            this.labelRepoTitle.TabIndex = 6;
            this.labelRepoTitle.Text = "Title";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(236, 67);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(156, 20);
            this.textBox2.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(236, 34);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(156, 20);
            this.textBox1.TabIndex = 8;
            // 
            // labelRepoSource
            // 
            this.labelRepoSource.AutoSize = true;
            this.labelRepoSource.Location = new System.Drawing.Point(156, 67);
            this.labelRepoSource.Name = "labelRepoSource";
            this.labelRepoSource.Size = new System.Drawing.Size(41, 13);
            this.labelRepoSource.TabIndex = 7;
            this.labelRepoSource.Text = "Source";
            // 
            // CompositeCredentialRepoUnlockerForm
            // 
            this.AcceptButton = this.buttonUnlock;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonSkip;
            this.ClientSize = new System.Drawing.Size(404, 212);
            this.Controls.Add(this.secureTextBoxPassword);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.buttonUnlock);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonSkip);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelRepoSource);
            this.Controls.Add(this.objectListViewRepos);
            this.Controls.Add(this.labelUnlocking);
            this.Controls.Add(this.labelRepoTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(420, 250);
            this.Name = "CompositeCredentialRepoUnlockerForm";
            this.Text = "Unlock Credential Repository";
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewRepos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListViewRepos;
        private System.Windows.Forms.Label labelUnlocking;
        private System.Windows.Forms.Label labelPassword;
        private Controls.SecureTextBox secureTextBoxPassword;
        private System.Windows.Forms.Button buttonUnlock;
        private System.Windows.Forms.Button buttonSkip;
        private System.Windows.Forms.Label labelRepoTitle;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelRepoSource;
    }
}