namespace mRemoteNG.UI.Forms.OptionsPages
{
    partial class SecurityPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecurityPage));
            this.chkEncryptCompleteFile = new System.Windows.Forms.CheckBox();
            this.comboBoxEncryptionEngine = new System.Windows.Forms.ComboBox();
            this.labelEncryptionEngine = new System.Windows.Forms.Label();
            this.labelBlockCipher = new System.Windows.Forms.Label();
            this.comboBoxBlockCipher = new System.Windows.Forms.ComboBox();
            this.groupAdvancedSecurityOptions = new System.Windows.Forms.GroupBox();
            this.groupAdvancedSecurityOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkEncryptCompleteFile
            // 
            this.chkEncryptCompleteFile.AutoSize = true;
            this.chkEncryptCompleteFile.Location = new System.Drawing.Point(3, 3);
            this.chkEncryptCompleteFile.Name = "chkEncryptCompleteFile";
            this.chkEncryptCompleteFile.Size = new System.Drawing.Size(180, 17);
            this.chkEncryptCompleteFile.TabIndex = 19;
            this.chkEncryptCompleteFile.Text = "Encrypt complete connection file";
            this.chkEncryptCompleteFile.UseVisualStyleBackColor = true;
            // 
            // comboBoxEncryptionEngine
            // 
            this.comboBoxEncryptionEngine.FormattingEnabled = true;
            this.comboBoxEncryptionEngine.Location = new System.Drawing.Point(125, 25);
            this.comboBoxEncryptionEngine.Name = "comboBoxEncryptionEngine";
            this.comboBoxEncryptionEngine.Size = new System.Drawing.Size(124, 21);
            this.comboBoxEncryptionEngine.TabIndex = 20;
            // 
            // labelEncryptionEngine
            // 
            this.labelEncryptionEngine.AutoSize = true;
            this.labelEncryptionEngine.Location = new System.Drawing.Point(9, 28);
            this.labelEncryptionEngine.Name = "labelEncryptionEngine";
            this.labelEncryptionEngine.Size = new System.Drawing.Size(93, 13);
            this.labelEncryptionEngine.TabIndex = 21;
            this.labelEncryptionEngine.Text = "Encryption Engine";
            // 
            // labelBlockCipher
            // 
            this.labelBlockCipher.AutoSize = true;
            this.labelBlockCipher.Location = new System.Drawing.Point(9, 60);
            this.labelBlockCipher.Name = "labelBlockCipher";
            this.labelBlockCipher.Size = new System.Drawing.Size(67, 13);
            this.labelBlockCipher.TabIndex = 22;
            this.labelBlockCipher.Text = "Block Cipher";
            // 
            // comboBoxBlockCipher
            // 
            this.comboBoxBlockCipher.FormattingEnabled = true;
            this.comboBoxBlockCipher.Location = new System.Drawing.Point(125, 57);
            this.comboBoxBlockCipher.Name = "comboBoxBlockCipher";
            this.comboBoxBlockCipher.Size = new System.Drawing.Size(124, 21);
            this.comboBoxBlockCipher.TabIndex = 23;
            // 
            // groupAdvancedSecurityOptions
            // 
            this.groupAdvancedSecurityOptions.Controls.Add(this.comboBoxBlockCipher);
            this.groupAdvancedSecurityOptions.Controls.Add(this.labelBlockCipher);
            this.groupAdvancedSecurityOptions.Controls.Add(this.comboBoxEncryptionEngine);
            this.groupAdvancedSecurityOptions.Controls.Add(this.labelEncryptionEngine);
            this.groupAdvancedSecurityOptions.Location = new System.Drawing.Point(3, 96);
            this.groupAdvancedSecurityOptions.Name = "groupAdvancedSecurityOptions";
            this.groupAdvancedSecurityOptions.Size = new System.Drawing.Size(437, 189);
            this.groupAdvancedSecurityOptions.TabIndex = 24;
            this.groupAdvancedSecurityOptions.TabStop = false;
            this.groupAdvancedSecurityOptions.Text = "Advanced Security Options";
            // 
            // SecurityPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkEncryptCompleteFile);
            this.Controls.Add(this.groupAdvancedSecurityOptions);
            this.Name = "SecurityPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            this.groupAdvancedSecurityOptions.ResumeLayout(false);
            this.groupAdvancedSecurityOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox chkEncryptCompleteFile;
        private System.Windows.Forms.ComboBox comboBoxEncryptionEngine;
        private System.Windows.Forms.Label labelEncryptionEngine;
        private System.Windows.Forms.Label labelBlockCipher;
        private System.Windows.Forms.ComboBox comboBoxBlockCipher;
        private System.Windows.Forms.GroupBox groupAdvancedSecurityOptions;
    }
}
