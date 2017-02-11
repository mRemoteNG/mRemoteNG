namespace mRemoteNG.UI.Forms.CredentialManagerPages.CredentialRepositoryEditorPages
{
    partial class XmlCredentialRepositoryEditorPage
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
            this.components = new System.ComponentModel.Container();
            this.labelPageTitle = new System.Windows.Forms.Label();
            this.buttonBrowseFiles = new System.Windows.Forms.Button();
            this.labelFilePath = new System.Windows.Forms.Label();
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.selectFilePathDialog = new System.Windows.Forms.SaveFileDialog();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.txtboxId = new System.Windows.Forms.TextBox();
            this.labelId = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.newPasswordBoxes = new mRemoteNG.UI.Controls.NewPasswordWithVerification();
            this.SuspendLayout();
            // 
            // labelPageTitle
            // 
            this.labelPageTitle.AutoSize = true;
            this.labelPageTitle.Location = new System.Drawing.Point(29, 6);
            this.labelPageTitle.Name = "labelPageTitle";
            this.labelPageTitle.Size = new System.Drawing.Size(132, 13);
            this.labelPageTitle.TabIndex = 0;
            this.labelPageTitle.Text = "XML Credential Repository";
            // 
            // buttonBrowseFiles
            // 
            this.buttonBrowseFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseFiles.Location = new System.Drawing.Point(276, 128);
            this.buttonBrowseFiles.Name = "buttonBrowseFiles";
            this.buttonBrowseFiles.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseFiles.TabIndex = 1;
            this.buttonBrowseFiles.Text = "Browse";
            this.buttonBrowseFiles.UseVisualStyleBackColor = true;
            this.buttonBrowseFiles.Click += new System.EventHandler(this.buttonBrowseFiles_Click);
            // 
            // labelFilePath
            // 
            this.labelFilePath.AutoSize = true;
            this.labelFilePath.Location = new System.Drawing.Point(29, 86);
            this.labelFilePath.Name = "labelFilePath";
            this.labelFilePath.Size = new System.Drawing.Size(48, 13);
            this.labelFilePath.TabIndex = 2;
            this.labelFilePath.Text = "File Path";
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilePath.Location = new System.Drawing.Point(32, 102);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(319, 20);
            this.textBoxFilePath.TabIndex = 3;
            // 
            // selectFilePathDialog
            // 
            this.selectFilePathDialog.DefaultExt = "xml";
            this.selectFilePathDialog.Filter = "XML|*.xml";
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfirm.Location = new System.Drawing.Point(266, 248);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(75, 23);
            this.buttonConfirm.TabIndex = 9;
            this.buttonConfirm.Text = "Confirm";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // txtboxId
            // 
            this.txtboxId.Location = new System.Drawing.Point(32, 51);
            this.txtboxId.Name = "txtboxId";
            this.txtboxId.ReadOnly = true;
            this.txtboxId.Size = new System.Drawing.Size(238, 20);
            this.txtboxId.TabIndex = 10;
            this.txtboxId.TabStop = false;
            // 
            // labelId
            // 
            this.labelId.AutoSize = true;
            this.labelId.Location = new System.Drawing.Point(29, 35);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(18, 13);
            this.labelId.TabIndex = 11;
            this.labelId.Text = "ID";
            // 
            // buttonBack
            // 
            this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBack.Location = new System.Drawing.Point(185, 248);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 12;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // toolTip
            // 
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            // 
            // newPasswordBoxes
            // 
            this.newPasswordBoxes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newPasswordBoxes.Location = new System.Drawing.Point(32, 154);
            this.newPasswordBoxes.MinimumSize = new System.Drawing.Size(0, 100);
            this.newPasswordBoxes.Name = "newPasswordBoxes";
            this.newPasswordBoxes.PasswordChar = '\0';
            this.newPasswordBoxes.Size = new System.Drawing.Size(319, 100);
            this.newPasswordBoxes.TabIndex = 13;
            this.newPasswordBoxes.UseSystemPasswordChar = true;
            // 
            // XmlCredentialRepositoryEditorPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.labelId);
            this.Controls.Add(this.txtboxId);
            this.Controls.Add(this.buttonConfirm);
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.labelFilePath);
            this.Controls.Add(this.buttonBrowseFiles);
            this.Controls.Add(this.labelPageTitle);
            this.Controls.Add(this.newPasswordBoxes);
            this.MinimumSize = new System.Drawing.Size(300, 260);
            this.Name = "XmlCredentialRepositoryEditorPage";
            this.Size = new System.Drawing.Size(354, 274);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPageTitle;
        private System.Windows.Forms.Button buttonBrowseFiles;
        private System.Windows.Forms.Label labelFilePath;
        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.SaveFileDialog selectFilePathDialog;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.TextBox txtboxId;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Button buttonBack;
        private Controls.NewPasswordWithVerification newPasswordBoxes;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
