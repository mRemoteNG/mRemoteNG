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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecurityPage));
            this.chkEncryptCompleteFile = new System.Windows.Forms.CheckBox();
            this.cryptoTypeComboBox = new System.Windows.Forms.ComboBox();
            this.cryptoProviderLabel = new System.Windows.Forms.Label();
            this.cryptoGroupBox = new System.Windows.Forms.GroupBox();
            this.blockCipherModeGroup = new System.Windows.Forms.GroupBox();
            this.bcmRadioEax = new System.Windows.Forms.RadioButton();
            this.bcmRadioCcm = new System.Windows.Forms.RadioButton();
            this.bcmRadioGcm = new System.Windows.Forms.RadioButton();
            this.blockCipherEngineGroup = new System.Windows.Forms.GroupBox();
            this.bceRadioSerpent = new System.Windows.Forms.RadioButton();
            this.bceRadioTwofish = new System.Windows.Forms.RadioButton();
            this.bceRadioAES = new System.Windows.Forms.RadioButton();
            this.securityPageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cryptoGroupBox.SuspendLayout();
            this.blockCipherModeGroup.SuspendLayout();
            this.blockCipherEngineGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.securityPageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // chkEncryptCompleteFile
            // 
            this.chkEncryptCompleteFile.AutoSize = true;
            this.chkEncryptCompleteFile.Location = new System.Drawing.Point(4, 4);
            this.chkEncryptCompleteFile.Margin = new System.Windows.Forms.Padding(4);
            this.chkEncryptCompleteFile.Name = "chkEncryptCompleteFile";
            this.chkEncryptCompleteFile.Size = new System.Drawing.Size(234, 21);
            this.chkEncryptCompleteFile.TabIndex = 19;
            this.chkEncryptCompleteFile.Text = "Encrypt complete connection file";
            this.chkEncryptCompleteFile.UseVisualStyleBackColor = true;
            // 
            // cryptoTypeComboBox
            // 
            this.cryptoTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cryptoTypeComboBox.FormattingEnabled = true;
            this.cryptoTypeComboBox.Location = new System.Drawing.Point(173, 28);
            this.cryptoTypeComboBox.Name = "cryptoTypeComboBox";
            this.cryptoTypeComboBox.Size = new System.Drawing.Size(364, 24);
            this.cryptoTypeComboBox.TabIndex = 20;
            this.cryptoTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.cryptoTypeComboBox_SelectedIndexChanged);
            // 
            // cryptoProviderLabel
            // 
            this.cryptoProviderLabel.AutoSize = true;
            this.cryptoProviderLabel.Location = new System.Drawing.Point(13, 31);
            this.cryptoProviderLabel.Name = "cryptoProviderLabel";
            this.cryptoProviderLabel.Size = new System.Drawing.Size(154, 17);
            this.cryptoProviderLabel.TabIndex = 21;
            this.cryptoProviderLabel.Text = "Cryptography Provider:";
            // 
            // cryptoGroupBox
            // 
            this.cryptoGroupBox.Controls.Add(this.blockCipherModeGroup);
            this.cryptoGroupBox.Controls.Add(this.blockCipherEngineGroup);
            this.cryptoGroupBox.Controls.Add(this.cryptoProviderLabel);
            this.cryptoGroupBox.Controls.Add(this.cryptoTypeComboBox);
            this.cryptoGroupBox.Location = new System.Drawing.Point(4, 32);
            this.cryptoGroupBox.Name = "cryptoGroupBox";
            this.cryptoGroupBox.Size = new System.Drawing.Size(543, 298);
            this.cryptoGroupBox.TabIndex = 22;
            this.cryptoGroupBox.TabStop = false;
            this.cryptoGroupBox.Text = "Cryptography";
            // 
            // blockCipherModeGroup
            // 
            this.blockCipherModeGroup.Controls.Add(this.bcmRadioEax);
            this.blockCipherModeGroup.Controls.Add(this.bcmRadioCcm);
            this.blockCipherModeGroup.Controls.Add(this.bcmRadioGcm);
            this.blockCipherModeGroup.Enabled = false;
            this.blockCipherModeGroup.Location = new System.Drawing.Point(240, 68);
            this.blockCipherModeGroup.Name = "blockCipherModeGroup";
            this.blockCipherModeGroup.Size = new System.Drawing.Size(200, 108);
            this.blockCipherModeGroup.TabIndex = 24;
            this.blockCipherModeGroup.TabStop = false;
            this.blockCipherModeGroup.Text = "Block Cipher Mode";
            // 
            // bcmRadioEax
            // 
            this.bcmRadioEax.AutoSize = true;
            this.bcmRadioEax.Cursor = System.Windows.Forms.Cursors.Default;
            this.bcmRadioEax.Location = new System.Drawing.Point(23, 75);
            this.bcmRadioEax.Name = "bcmRadioEax";
            this.bcmRadioEax.Size = new System.Drawing.Size(56, 21);
            this.bcmRadioEax.TabIndex = 2;
            this.bcmRadioEax.Text = "EAX";
            this.bcmRadioEax.UseVisualStyleBackColor = true;
            // 
            // bcmRadioCcm
            // 
            this.bcmRadioCcm.AutoSize = true;
            this.bcmRadioCcm.Location = new System.Drawing.Point(23, 48);
            this.bcmRadioCcm.Name = "bcmRadioCcm";
            this.bcmRadioCcm.Size = new System.Drawing.Size(58, 21);
            this.bcmRadioCcm.TabIndex = 1;
            this.bcmRadioCcm.Text = "CCM";
            this.bcmRadioCcm.UseVisualStyleBackColor = true;
            // 
            // bcmRadioGcm
            // 
            this.bcmRadioGcm.AutoSize = true;
            this.bcmRadioGcm.Checked = true;
            this.bcmRadioGcm.Location = new System.Drawing.Point(23, 21);
            this.bcmRadioGcm.Name = "bcmRadioGcm";
            this.bcmRadioGcm.Size = new System.Drawing.Size(60, 21);
            this.bcmRadioGcm.TabIndex = 0;
            this.bcmRadioGcm.TabStop = true;
            this.bcmRadioGcm.Text = "GCM";
            this.bcmRadioGcm.UseVisualStyleBackColor = true;
            // 
            // blockCipherEngineGroup
            // 
            this.blockCipherEngineGroup.Controls.Add(this.bceRadioSerpent);
            this.blockCipherEngineGroup.Controls.Add(this.bceRadioTwofish);
            this.blockCipherEngineGroup.Controls.Add(this.bceRadioAES);
            this.blockCipherEngineGroup.Enabled = false;
            this.blockCipherEngineGroup.Location = new System.Drawing.Point(34, 68);
            this.blockCipherEngineGroup.Name = "blockCipherEngineGroup";
            this.blockCipherEngineGroup.Size = new System.Drawing.Size(200, 108);
            this.blockCipherEngineGroup.TabIndex = 23;
            this.blockCipherEngineGroup.TabStop = false;
            this.blockCipherEngineGroup.Text = "Block Cipher Engine";
            // 
            // bceRadioSerpent
            // 
            this.bceRadioSerpent.AutoSize = true;
            this.bceRadioSerpent.Location = new System.Drawing.Point(23, 75);
            this.bceRadioSerpent.Name = "bceRadioSerpent";
            this.bceRadioSerpent.Size = new System.Drawing.Size(79, 21);
            this.bceRadioSerpent.TabIndex = 2;
            this.bceRadioSerpent.Text = "Serpent";
            this.bceRadioSerpent.UseVisualStyleBackColor = true;
            // 
            // bceRadioTwofish
            // 
            this.bceRadioTwofish.AutoSize = true;
            this.bceRadioTwofish.Location = new System.Drawing.Point(23, 48);
            this.bceRadioTwofish.Name = "bceRadioTwofish";
            this.bceRadioTwofish.Size = new System.Drawing.Size(77, 21);
            this.bceRadioTwofish.TabIndex = 1;
            this.bceRadioTwofish.Text = "Twofish";
            this.bceRadioTwofish.UseVisualStyleBackColor = true;
            // 
            // bceRadioAES
            // 
            this.bceRadioAES.AutoSize = true;
            this.bceRadioAES.Checked = true;
            this.bceRadioAES.Location = new System.Drawing.Point(23, 21);
            this.bceRadioAES.Name = "bceRadioAES";
            this.bceRadioAES.Size = new System.Drawing.Size(56, 21);
            this.bceRadioAES.TabIndex = 0;
            this.bceRadioAES.TabStop = true;
            this.bceRadioAES.Text = "AES";
            this.bceRadioAES.UseVisualStyleBackColor = true;
            // 
            // securityPageBindingSource
            // 
            this.securityPageBindingSource.DataSource = typeof(mRemoteNG.UI.Forms.OptionsPages.SecurityPage);
            // 
            // SecurityPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cryptoGroupBox);
            this.Controls.Add(this.chkEncryptCompleteFile);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SecurityPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(813, 602);
            this.cryptoGroupBox.ResumeLayout(false);
            this.cryptoGroupBox.PerformLayout();
            this.blockCipherModeGroup.ResumeLayout(false);
            this.blockCipherModeGroup.PerformLayout();
            this.blockCipherEngineGroup.ResumeLayout(false);
            this.blockCipherEngineGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.securityPageBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox chkEncryptCompleteFile;
        private System.Windows.Forms.ComboBox cryptoTypeComboBox;
        private System.Windows.Forms.Label cryptoProviderLabel;
        private System.Windows.Forms.GroupBox cryptoGroupBox;
        private System.Windows.Forms.GroupBox blockCipherEngineGroup;
        private System.Windows.Forms.RadioButton bceRadioAES;
        private System.Windows.Forms.GroupBox blockCipherModeGroup;
        private System.Windows.Forms.RadioButton bcmRadioEax;
        private System.Windows.Forms.RadioButton bcmRadioCcm;
        private System.Windows.Forms.RadioButton bcmRadioGcm;
        private System.Windows.Forms.RadioButton bceRadioSerpent;
        private System.Windows.Forms.RadioButton bceRadioTwofish;
        private System.Windows.Forms.BindingSource securityPageBindingSource;
    }
}
