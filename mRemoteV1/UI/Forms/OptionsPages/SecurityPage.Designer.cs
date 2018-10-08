namespace mRemoteNG.UI.Forms.OptionsPages
{
    sealed partial class SecurityPage
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
            this.chkEncryptCompleteFile = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.comboBoxEncryptionEngine = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.labelEncryptionEngine = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.labelBlockCipher = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.comboBoxBlockCipher = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.groupAdvancedSecurityOptions = new mRemoteNG.UI.Controls.Base.NGGroupBox();
            this.numberBoxKdfIterations = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.labelKdfIterations = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.groupAdvancedSecurityOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberBoxKdfIterations)).BeginInit();
            this.SuspendLayout();
            // 
            // chkEncryptCompleteFile
            // 
            this.chkEncryptCompleteFile._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkEncryptCompleteFile.AutoSize = true;
            this.chkEncryptCompleteFile.Location = new System.Drawing.Point(3, 3);
            this.chkEncryptCompleteFile.Name = "chkEncryptCompleteFile";
            this.chkEncryptCompleteFile.Size = new System.Drawing.Size(180, 17);
            this.chkEncryptCompleteFile.TabIndex = 0;
            this.chkEncryptCompleteFile.Text = "Encrypt complete connection file";
            this.chkEncryptCompleteFile.UseVisualStyleBackColor = true;
            // 
            // comboBoxEncryptionEngine
            // 
            this.comboBoxEncryptionEngine._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.comboBoxEncryptionEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEncryptionEngine.FormattingEnabled = true;
            this.comboBoxEncryptionEngine.Location = new System.Drawing.Point(191, 25);
            this.comboBoxEncryptionEngine.Name = "comboBoxEncryptionEngine";
            this.comboBoxEncryptionEngine.Size = new System.Drawing.Size(123, 21);
            this.comboBoxEncryptionEngine.Sorted = true;
            this.comboBoxEncryptionEngine.TabIndex = 1;
            // 
            // labelEncryptionEngine
            // 
            this.labelEncryptionEngine.AutoSize = true;
            this.labelEncryptionEngine.Location = new System.Drawing.Point(9, 28);
            this.labelEncryptionEngine.Name = "labelEncryptionEngine";
            this.labelEncryptionEngine.Size = new System.Drawing.Size(93, 13);
            this.labelEncryptionEngine.TabIndex = 0;
            this.labelEncryptionEngine.Text = "Encryption Engine";
            // 
            // labelBlockCipher
            // 
            this.labelBlockCipher.AutoSize = true;
            this.labelBlockCipher.Location = new System.Drawing.Point(9, 60);
            this.labelBlockCipher.Name = "labelBlockCipher";
            this.labelBlockCipher.Size = new System.Drawing.Size(97, 13);
            this.labelBlockCipher.TabIndex = 2;
            this.labelBlockCipher.Text = "Block Cipher Mode";
            // 
            // comboBoxBlockCipher
            // 
            this.comboBoxBlockCipher._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.comboBoxBlockCipher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBlockCipher.FormattingEnabled = true;
            this.comboBoxBlockCipher.Location = new System.Drawing.Point(191, 57);
            this.comboBoxBlockCipher.Name = "comboBoxBlockCipher";
            this.comboBoxBlockCipher.Size = new System.Drawing.Size(123, 21);
            this.comboBoxBlockCipher.TabIndex = 3;
            // 
            // groupAdvancedSecurityOptions
            // 
            this.groupAdvancedSecurityOptions.Controls.Add(this.numberBoxKdfIterations);
            this.groupAdvancedSecurityOptions.Controls.Add(this.labelKdfIterations);
            this.groupAdvancedSecurityOptions.Controls.Add(this.comboBoxEncryptionEngine);
            this.groupAdvancedSecurityOptions.Controls.Add(this.comboBoxBlockCipher);
            this.groupAdvancedSecurityOptions.Controls.Add(this.labelBlockCipher);
            this.groupAdvancedSecurityOptions.Controls.Add(this.labelEncryptionEngine);
            this.groupAdvancedSecurityOptions.Location = new System.Drawing.Point(3, 30);
            this.groupAdvancedSecurityOptions.Name = "groupAdvancedSecurityOptions";
            this.groupAdvancedSecurityOptions.Size = new System.Drawing.Size(604, 128);
            this.groupAdvancedSecurityOptions.TabIndex = 1;
            this.groupAdvancedSecurityOptions.TabStop = false;
            this.groupAdvancedSecurityOptions.Text = "Advanced Security Options";
            // 
            // numberBoxKdfIterations
            // 
            this.numberBoxKdfIterations.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numberBoxKdfIterations.Location = new System.Drawing.Point(191, 88);
            this.numberBoxKdfIterations.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.numberBoxKdfIterations.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numberBoxKdfIterations.Name = "numberBoxKdfIterations";
            this.numberBoxKdfIterations.Size = new System.Drawing.Size(90, 20);
            this.numberBoxKdfIterations.TabIndex = 5;
            this.numberBoxKdfIterations.ThousandsSeparator = true;
            this.numberBoxKdfIterations.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // labelKdfIterations
            // 
            this.labelKdfIterations.AutoSize = true;
            this.labelKdfIterations.Location = new System.Drawing.Point(9, 90);
            this.labelKdfIterations.Name = "labelKdfIterations";
            this.labelKdfIterations.Size = new System.Drawing.Size(166, 13);
            this.labelKdfIterations.TabIndex = 4;
            this.labelKdfIterations.Text = "Key Derivation Function Iterations";
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
            ((System.ComponentModel.ISupportInitialize)(this.numberBoxKdfIterations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal Controls.Base.NGCheckBox chkEncryptCompleteFile;
        private Controls.Base.NGComboBox comboBoxEncryptionEngine;
        private Controls.Base.NGLabel labelEncryptionEngine;
        private Controls.Base.NGLabel labelBlockCipher;
        private Controls.Base.NGComboBox comboBoxBlockCipher;
        private Controls.Base.NGGroupBox groupAdvancedSecurityOptions;
        private Controls.Base.NGNumericUpDown numberBoxKdfIterations;
        private Controls.Base.NGLabel labelKdfIterations;
    }
}
