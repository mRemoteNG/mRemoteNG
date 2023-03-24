using mRemoteNG.UI.Controls;

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
            chkEncryptCompleteFile = new MrngCheckBox();
            comboBoxEncryptionEngine = new MrngComboBox();
            labelEncryptionEngine = new MrngLabel();
            labelBlockCipher = new MrngLabel();
            comboBoxBlockCipher = new MrngComboBox();
            groupAdvancedSecurityOptions = new MrngGroupBox();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            labelKdfIterations = new MrngLabel();
            numberBoxKdfIterations = new MrngNumericUpDown();
            btnTestSettings = new MrngButton();
            groupAdvancedSecurityOptions.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numberBoxKdfIterations).BeginInit();
            SuspendLayout();
            // 
            // chkEncryptCompleteFile
            // 
            chkEncryptCompleteFile._mice = MrngCheckBox.MouseState.OUT;
            chkEncryptCompleteFile.AutoSize = true;
            chkEncryptCompleteFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkEncryptCompleteFile.Location = new System.Drawing.Point(12, 10);
            chkEncryptCompleteFile.Margin = new System.Windows.Forms.Padding(6);
            chkEncryptCompleteFile.Name = "chkEncryptCompleteFile";
            chkEncryptCompleteFile.Size = new System.Drawing.Size(286, 27);
            chkEncryptCompleteFile.TabIndex = 0;
            chkEncryptCompleteFile.Text = "Encrypt complete connection file";
            chkEncryptCompleteFile.UseVisualStyleBackColor = true;
            // 
            // comboBoxEncryptionEngine
            // 
            comboBoxEncryptionEngine._mice = MrngComboBox.MouseState.HOVER;
            comboBoxEncryptionEngine.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxEncryptionEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxEncryptionEngine.FormattingEnabled = true;
            comboBoxEncryptionEngine.Location = new System.Drawing.Point(280, 4);
            comboBoxEncryptionEngine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            comboBoxEncryptionEngine.Name = "comboBoxEncryptionEngine";
            comboBoxEncryptionEngine.Size = new System.Drawing.Size(196, 31);
            comboBoxEncryptionEngine.Sorted = true;
            comboBoxEncryptionEngine.TabIndex = 1;
            // 
            // labelEncryptionEngine
            // 
            labelEncryptionEngine.AutoSize = true;
            labelEncryptionEngine.Dock = System.Windows.Forms.DockStyle.Fill;
            labelEncryptionEngine.Location = new System.Drawing.Point(4, 0);
            labelEncryptionEngine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelEncryptionEngine.Name = "labelEncryptionEngine";
            labelEncryptionEngine.Size = new System.Drawing.Size(268, 39);
            labelEncryptionEngine.TabIndex = 0;
            labelEncryptionEngine.Text = "Encryption Engine";
            labelEncryptionEngine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelBlockCipher
            // 
            labelBlockCipher.AutoSize = true;
            labelBlockCipher.Dock = System.Windows.Forms.DockStyle.Fill;
            labelBlockCipher.Location = new System.Drawing.Point(4, 39);
            labelBlockCipher.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelBlockCipher.Name = "labelBlockCipher";
            labelBlockCipher.Size = new System.Drawing.Size(268, 39);
            labelBlockCipher.TabIndex = 2;
            labelBlockCipher.Text = "Block Cipher Mode";
            labelBlockCipher.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxBlockCipher
            // 
            comboBoxBlockCipher._mice = MrngComboBox.MouseState.HOVER;
            comboBoxBlockCipher.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxBlockCipher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxBlockCipher.FormattingEnabled = true;
            comboBoxBlockCipher.Location = new System.Drawing.Point(280, 43);
            comboBoxBlockCipher.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            comboBoxBlockCipher.Name = "comboBoxBlockCipher";
            comboBoxBlockCipher.Size = new System.Drawing.Size(196, 31);
            comboBoxBlockCipher.TabIndex = 2;
            // 
            // groupAdvancedSecurityOptions
            // 
            groupAdvancedSecurityOptions.Controls.Add(tableLayoutPanel1);
            groupAdvancedSecurityOptions.Location = new System.Drawing.Point(12, 51);
            groupAdvancedSecurityOptions.Margin = new System.Windows.Forms.Padding(6);
            groupAdvancedSecurityOptions.Name = "groupAdvancedSecurityOptions";
            groupAdvancedSecurityOptions.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupAdvancedSecurityOptions.Size = new System.Drawing.Size(906, 201);
            groupAdvancedSecurityOptions.TabIndex = 1;
            groupAdvancedSecurityOptions.TabStop = false;
            groupAdvancedSecurityOptions.Text = "Advanced Security Options";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(labelKdfIterations, 0, 2);
            tableLayoutPanel1.Controls.Add(numberBoxKdfIterations, 1, 2);
            tableLayoutPanel1.Controls.Add(labelBlockCipher, 0, 1);
            tableLayoutPanel1.Controls.Add(comboBoxEncryptionEngine, 1, 0);
            tableLayoutPanel1.Controls.Add(labelEncryptionEngine, 0, 0);
            tableLayoutPanel1.Controls.Add(comboBoxBlockCipher, 1, 1);
            tableLayoutPanel1.Controls.Add(btnTestSettings, 1, 3);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            tableLayoutPanel1.Location = new System.Drawing.Point(4, 26);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(480, 171);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // labelKdfIterations
            // 
            labelKdfIterations.AutoSize = true;
            labelKdfIterations.Dock = System.Windows.Forms.DockStyle.Fill;
            labelKdfIterations.Location = new System.Drawing.Point(4, 78);
            labelKdfIterations.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelKdfIterations.Name = "labelKdfIterations";
            labelKdfIterations.Size = new System.Drawing.Size(268, 37);
            labelKdfIterations.TabIndex = 4;
            labelKdfIterations.Text = "Key Derivation Function Iterations";
            labelKdfIterations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numberBoxKdfIterations
            // 
            numberBoxKdfIterations.Dock = System.Windows.Forms.DockStyle.Fill;
            numberBoxKdfIterations.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numberBoxKdfIterations.Location = new System.Drawing.Point(280, 82);
            numberBoxKdfIterations.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            numberBoxKdfIterations.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            numberBoxKdfIterations.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            numberBoxKdfIterations.Name = "numberBoxKdfIterations";
            numberBoxKdfIterations.Size = new System.Drawing.Size(196, 29);
            numberBoxKdfIterations.TabIndex = 3;
            numberBoxKdfIterations.ThousandsSeparator = true;
            numberBoxKdfIterations.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // btnTestSettings
            // 
            btnTestSettings._mice = MrngButton.MouseState.OUT;
            btnTestSettings.AutoSize = true;
            btnTestSettings.Dock = System.Windows.Forms.DockStyle.Right;
            btnTestSettings.Location = new System.Drawing.Point(294, 119);
            btnTestSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnTestSettings.Name = "btnTestSettings";
            btnTestSettings.Size = new System.Drawing.Size(182, 52);
            btnTestSettings.TabIndex = 4;
            btnTestSettings.Text = "Test Settings";
            btnTestSettings.UseVisualStyleBackColor = true;
            btnTestSettings.Click += BtnTestSettings_Click;
            // 
            // SecurityPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(chkEncryptCompleteFile);
            Controls.Add(groupAdvancedSecurityOptions);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "SecurityPage";
            Size = new System.Drawing.Size(915, 735);
            groupAdvancedSecurityOptions.ResumeLayout(false);
            groupAdvancedSecurityOptions.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numberBoxKdfIterations).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        internal MrngCheckBox chkEncryptCompleteFile;
        private MrngComboBox comboBoxEncryptionEngine;
        private Controls.MrngLabel labelEncryptionEngine;
        private Controls.MrngLabel labelBlockCipher;
        private MrngComboBox comboBoxBlockCipher;
        private MrngGroupBox groupAdvancedSecurityOptions;
        private Controls.MrngNumericUpDown numberBoxKdfIterations;
        private Controls.MrngLabel labelKdfIterations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MrngButton btnTestSettings;
    }
}
