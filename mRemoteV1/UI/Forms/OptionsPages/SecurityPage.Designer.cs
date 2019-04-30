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
            this.chkEncryptCompleteFile = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.comboBoxEncryptionEngine = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.labelEncryptionEngine = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.labelBlockCipher = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.comboBoxBlockCipher = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.groupAdvancedSecurityOptions = new mRemoteNG.UI.Controls.Base.NGGroupBox();
            this.numberBoxKdfIterations = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.labelKdfIterations = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupAdvancedSecurityOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberBoxKdfIterations)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkEncryptCompleteFile
            // 
            this.chkEncryptCompleteFile._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkEncryptCompleteFile.AutoSize = true;
            this.chkEncryptCompleteFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEncryptCompleteFile.Location = new System.Drawing.Point(3, 3);
            this.chkEncryptCompleteFile.Name = "chkEncryptCompleteFile";
            this.chkEncryptCompleteFile.Size = new System.Drawing.Size(194, 17);
            this.chkEncryptCompleteFile.TabIndex = 0;
            this.chkEncryptCompleteFile.Text = "Encrypt complete connection file";
            this.chkEncryptCompleteFile.UseVisualStyleBackColor = true;
            // 
            // comboBoxEncryptionEngine
            // 
            this.comboBoxEncryptionEngine._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.comboBoxEncryptionEngine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxEncryptionEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEncryptionEngine.FormattingEnabled = true;
            this.comboBoxEncryptionEngine.Location = new System.Drawing.Point(223, 3);
            this.comboBoxEncryptionEngine.Name = "comboBoxEncryptionEngine";
            this.comboBoxEncryptionEngine.Size = new System.Drawing.Size(131, 21);
            this.comboBoxEncryptionEngine.Sorted = true;
            this.comboBoxEncryptionEngine.TabIndex = 1;
            // 
            // labelEncryptionEngine
            // 
            this.labelEncryptionEngine.AutoSize = true;
            this.labelEncryptionEngine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEncryptionEngine.Location = new System.Drawing.Point(3, 0);
            this.labelEncryptionEngine.Name = "labelEncryptionEngine";
            this.labelEncryptionEngine.Size = new System.Drawing.Size(214, 26);
            this.labelEncryptionEngine.TabIndex = 0;
            this.labelEncryptionEngine.Text = "Encryption Engine";
            this.labelEncryptionEngine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelBlockCipher
            // 
            this.labelBlockCipher.AutoSize = true;
            this.labelBlockCipher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBlockCipher.Location = new System.Drawing.Point(3, 26);
            this.labelBlockCipher.Name = "labelBlockCipher";
            this.labelBlockCipher.Size = new System.Drawing.Size(214, 26);
            this.labelBlockCipher.TabIndex = 2;
            this.labelBlockCipher.Text = "Block Cipher Mode";
            this.labelBlockCipher.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxBlockCipher
            // 
            this.comboBoxBlockCipher._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.comboBoxBlockCipher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxBlockCipher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBlockCipher.FormattingEnabled = true;
            this.comboBoxBlockCipher.Location = new System.Drawing.Point(223, 29);
            this.comboBoxBlockCipher.Name = "comboBoxBlockCipher";
            this.comboBoxBlockCipher.Size = new System.Drawing.Size(131, 21);
            this.comboBoxBlockCipher.TabIndex = 3;
            // 
            // groupAdvancedSecurityOptions
            // 
            this.groupAdvancedSecurityOptions.Controls.Add(this.tableLayoutPanel1);
            this.groupAdvancedSecurityOptions.Location = new System.Drawing.Point(3, 30);
            this.groupAdvancedSecurityOptions.Name = "groupAdvancedSecurityOptions";
            this.groupAdvancedSecurityOptions.Size = new System.Drawing.Size(604, 112);
            this.groupAdvancedSecurityOptions.TabIndex = 1;
            this.groupAdvancedSecurityOptions.TabStop = false;
            this.groupAdvancedSecurityOptions.Text = "Advanced Security Options";
            // 
            // numberBoxKdfIterations
            // 
            this.numberBoxKdfIterations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numberBoxKdfIterations.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numberBoxKdfIterations.Location = new System.Drawing.Point(223, 55);
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
            this.numberBoxKdfIterations.Size = new System.Drawing.Size(131, 22);
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
            this.labelKdfIterations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelKdfIterations.Location = new System.Drawing.Point(3, 52);
            this.labelKdfIterations.Name = "labelKdfIterations";
            this.labelKdfIterations.Size = new System.Drawing.Size(214, 26);
            this.labelKdfIterations.TabIndex = 4;
            this.labelKdfIterations.Text = "Key Derivation Function Iterations";
            this.labelKdfIterations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelKdfIterations, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numberBoxKdfIterations, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelBlockCipher, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxEncryptionEngine, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelEncryptionEngine, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxBlockCipher, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 21);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(357, 81);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // SecurityPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.chkEncryptCompleteFile);
            this.Controls.Add(this.groupAdvancedSecurityOptions);
            this.Name = "SecurityPage";
            this.Size = new System.Drawing.Size(610, 490);
            this.groupAdvancedSecurityOptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numberBoxKdfIterations)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
