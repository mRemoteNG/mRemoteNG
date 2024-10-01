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
            lblRegistrySettingsUsedInfo = new System.Windows.Forms.Label();
            pnlOptions = new System.Windows.Forms.Panel();
            groupPasswordGenerator = new System.Windows.Forms.GroupBox();
            pnlPasswordTxtAndBtn = new System.Windows.Forms.TableLayoutPanel();
            txtPasswdGenerator = new System.Windows.Forms.TextBox();
            btnPasswdGenerator = new System.Windows.Forms.Button();
            lblPasswdGenDescription = new System.Windows.Forms.Label();
            groupAdvancedSecurityOptions.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numberBoxKdfIterations).BeginInit();
            pnlOptions.SuspendLayout();
            groupPasswordGenerator.SuspendLayout();
            pnlPasswordTxtAndBtn.SuspendLayout();
            SuspendLayout();
            // 
            // chkEncryptCompleteFile
            // 
            chkEncryptCompleteFile._mice = MrngCheckBox.MouseState.OUT;
            chkEncryptCompleteFile.AutoSize = true;
            chkEncryptCompleteFile.Dock = System.Windows.Forms.DockStyle.Top;
            chkEncryptCompleteFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkEncryptCompleteFile.Location = new System.Drawing.Point(0, 0);
            chkEncryptCompleteFile.Margin = new System.Windows.Forms.Padding(6);
            chkEncryptCompleteFile.Name = "chkEncryptCompleteFile";
            chkEncryptCompleteFile.Padding = new System.Windows.Forms.Padding(4);
            chkEncryptCompleteFile.Size = new System.Drawing.Size(610, 25);
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
            comboBoxEncryptionEngine.Location = new System.Drawing.Point(197, 8);
            comboBoxEncryptionEngine.Margin = new System.Windows.Forms.Padding(4);
            comboBoxEncryptionEngine.Name = "comboBoxEncryptionEngine";
            comboBoxEncryptionEngine.Size = new System.Drawing.Size(196, 21);
            comboBoxEncryptionEngine.Sorted = true;
            comboBoxEncryptionEngine.TabIndex = 1;
            // 
            // labelEncryptionEngine
            // 
            labelEncryptionEngine.AutoSize = true;
            labelEncryptionEngine.Dock = System.Windows.Forms.DockStyle.Fill;
            labelEncryptionEngine.Location = new System.Drawing.Point(8, 4);
            labelEncryptionEngine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelEncryptionEngine.Name = "labelEncryptionEngine";
            labelEncryptionEngine.Size = new System.Drawing.Size(181, 29);
            labelEncryptionEngine.TabIndex = 0;
            labelEncryptionEngine.Text = "Encryption Engine";
            labelEncryptionEngine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelBlockCipher
            // 
            labelBlockCipher.AutoSize = true;
            labelBlockCipher.Dock = System.Windows.Forms.DockStyle.Fill;
            labelBlockCipher.Location = new System.Drawing.Point(8, 33);
            labelBlockCipher.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelBlockCipher.Name = "labelBlockCipher";
            labelBlockCipher.Size = new System.Drawing.Size(181, 29);
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
            comboBoxBlockCipher.Location = new System.Drawing.Point(197, 37);
            comboBoxBlockCipher.Margin = new System.Windows.Forms.Padding(4);
            comboBoxBlockCipher.Name = "comboBoxBlockCipher";
            comboBoxBlockCipher.Size = new System.Drawing.Size(196, 21);
            comboBoxBlockCipher.TabIndex = 2;
            // 
            // groupAdvancedSecurityOptions
            // 
            groupAdvancedSecurityOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            groupAdvancedSecurityOptions.Controls.Add(tableLayoutPanel1);
            groupAdvancedSecurityOptions.Dock = System.Windows.Forms.DockStyle.Top;
            groupAdvancedSecurityOptions.Location = new System.Drawing.Point(0, 25);
            groupAdvancedSecurityOptions.Margin = new System.Windows.Forms.Padding(6);
            groupAdvancedSecurityOptions.Name = "groupAdvancedSecurityOptions";
            groupAdvancedSecurityOptions.Padding = new System.Windows.Forms.Padding(4);
            groupAdvancedSecurityOptions.Size = new System.Drawing.Size(610, 159);
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
            tableLayoutPanel1.Location = new System.Drawing.Point(4, 19);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.Size = new System.Drawing.Size(401, 136);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // labelKdfIterations
            // 
            labelKdfIterations.AutoSize = true;
            labelKdfIterations.Dock = System.Windows.Forms.DockStyle.Fill;
            labelKdfIterations.Location = new System.Drawing.Point(8, 62);
            labelKdfIterations.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelKdfIterations.Name = "labelKdfIterations";
            labelKdfIterations.Size = new System.Drawing.Size(181, 30);
            labelKdfIterations.TabIndex = 4;
            labelKdfIterations.Text = "Key Derivation Function Iterations";
            labelKdfIterations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numberBoxKdfIterations
            // 
            numberBoxKdfIterations.Dock = System.Windows.Forms.DockStyle.Fill;
            numberBoxKdfIterations.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numberBoxKdfIterations.Location = new System.Drawing.Point(197, 66);
            numberBoxKdfIterations.Margin = new System.Windows.Forms.Padding(4);
            numberBoxKdfIterations.Maximum = new decimal(new int[] { 50000, 0, 0, 0 });
            numberBoxKdfIterations.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            numberBoxKdfIterations.Name = "numberBoxKdfIterations";
            numberBoxKdfIterations.Size = new System.Drawing.Size(196, 22);
            numberBoxKdfIterations.TabIndex = 3;
            numberBoxKdfIterations.ThousandsSeparator = true;
            numberBoxKdfIterations.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // btnTestSettings
            // 
            btnTestSettings._mice = MrngButton.MouseState.OUT;
            btnTestSettings.AutoSize = true;
            btnTestSettings.Location = new System.Drawing.Point(197, 96);
            btnTestSettings.Margin = new System.Windows.Forms.Padding(4);
            btnTestSettings.Name = "btnTestSettings";
            btnTestSettings.Padding = new System.Windows.Forms.Padding(4);
            btnTestSettings.Size = new System.Drawing.Size(196, 33);
            btnTestSettings.TabIndex = 4;
            btnTestSettings.Text = "Test Settings";
            btnTestSettings.UseVisualStyleBackColor = true;
            btnTestSettings.Click += BtnTestSettings_Click;
            // 
            // lblRegistrySettingsUsedInfo
            // 
            lblRegistrySettingsUsedInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            lblRegistrySettingsUsedInfo.Dock = System.Windows.Forms.DockStyle.Top;
            lblRegistrySettingsUsedInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            lblRegistrySettingsUsedInfo.Location = new System.Drawing.Point(0, 0);
            lblRegistrySettingsUsedInfo.Name = "lblRegistrySettingsUsedInfo";
            lblRegistrySettingsUsedInfo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            lblRegistrySettingsUsedInfo.Size = new System.Drawing.Size(610, 30);
            lblRegistrySettingsUsedInfo.TabIndex = 3;
            lblRegistrySettingsUsedInfo.Text = "Some settings are configured by your Administrator. Please contact your administrator for more information.";
            lblRegistrySettingsUsedInfo.Visible = false;
            // 
            // pnlOptions
            // 
            pnlOptions.Controls.Add(groupAdvancedSecurityOptions);
            pnlOptions.Controls.Add(chkEncryptCompleteFile);
            pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            pnlOptions.Location = new System.Drawing.Point(0, 30);
            pnlOptions.Name = "pnlOptions";
            pnlOptions.Size = new System.Drawing.Size(610, 201);
            pnlOptions.TabIndex = 2;
            // 
            // groupPasswordGenerator
            // 
            groupPasswordGenerator.Controls.Add(pnlPasswordTxtAndBtn);
            groupPasswordGenerator.Controls.Add(lblPasswdGenDescription);
            groupPasswordGenerator.Dock = System.Windows.Forms.DockStyle.Top;
            groupPasswordGenerator.Location = new System.Drawing.Point(0, 231);
            groupPasswordGenerator.Name = "groupPasswordGenerator";
            groupPasswordGenerator.Size = new System.Drawing.Size(610, 116);
            groupPasswordGenerator.TabIndex = 4;
            groupPasswordGenerator.TabStop = false;
            groupPasswordGenerator.Text = "Secure Key Generator";
            // 
            // pnlPasswordTxtAndBtn
            // 
            pnlPasswordTxtAndBtn.ColumnCount = 2;
            pnlPasswordTxtAndBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.91437F));
            pnlPasswordTxtAndBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.08563F));
            pnlPasswordTxtAndBtn.Controls.Add(txtPasswdGenerator, 0, 0);
            pnlPasswordTxtAndBtn.Controls.Add(btnPasswdGenerator, 1, 0);
            pnlPasswordTxtAndBtn.Location = new System.Drawing.Point(4, 43);
            pnlPasswordTxtAndBtn.Name = "pnlPasswordTxtAndBtn";
            pnlPasswordTxtAndBtn.RowCount = 1;
            pnlPasswordTxtAndBtn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            pnlPasswordTxtAndBtn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            pnlPasswordTxtAndBtn.Size = new System.Drawing.Size(327, 31);
            pnlPasswordTxtAndBtn.TabIndex = 1;
            // 
            // txtPasswdGenerator
            // 
            txtPasswdGenerator.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtPasswdGenerator.Location = new System.Drawing.Point(3, 4);
            txtPasswdGenerator.Name = "txtPasswdGenerator";
            txtPasswdGenerator.PasswordChar = '*';
            txtPasswdGenerator.Size = new System.Drawing.Size(203, 22);
            txtPasswdGenerator.TabIndex = 1;
            txtPasswdGenerator.TextChanged += txtPasswdGenerator_TextChanged;
            // 
            // btnPasswdGenerator
            // 
            btnPasswdGenerator.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            btnPasswdGenerator.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnPasswdGenerator.Enabled = false;
            btnPasswdGenerator.Location = new System.Drawing.Point(212, 4);
            btnPasswdGenerator.Name = "btnPasswdGenerator";
            btnPasswdGenerator.Size = new System.Drawing.Size(112, 23);
            btnPasswdGenerator.TabIndex = 2;
            btnPasswdGenerator.Text = "Copy to Clipboard";
            btnPasswdGenerator.UseVisualStyleBackColor = true;
            btnPasswdGenerator.Click += btnPasswdGenerator_Click;
            // 
            // lblPasswdGenDescription
            // 
            lblPasswdGenDescription.AutoSize = true;
            lblPasswdGenDescription.Dock = System.Windows.Forms.DockStyle.Top;
            lblPasswdGenDescription.Location = new System.Drawing.Point(3, 18);
            lblPasswdGenDescription.Name = "lblPasswdGenDescription";
            lblPasswdGenDescription.Size = new System.Drawing.Size(327, 13);
            lblPasswdGenDescription.TabIndex = 0;
            lblPasswdGenDescription.Text = "Generate an encrypted password suitable for registry settings.";
            // 
            // SecurityPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(groupPasswordGenerator);
            Controls.Add(pnlOptions);
            Controls.Add(lblRegistrySettingsUsedInfo);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "SecurityPage";
            Size = new System.Drawing.Size(610, 490);
            groupAdvancedSecurityOptions.ResumeLayout(false);
            groupAdvancedSecurityOptions.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numberBoxKdfIterations).EndInit();
            pnlOptions.ResumeLayout(false);
            pnlOptions.PerformLayout();
            groupPasswordGenerator.ResumeLayout(false);
            groupPasswordGenerator.PerformLayout();
            pnlPasswordTxtAndBtn.ResumeLayout(false);
            pnlPasswordTxtAndBtn.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        internal MrngCheckBox chkEncryptCompleteFile;
        private MrngComboBox comboBoxEncryptionEngine;
        private Controls.MrngLabel labelEncryptionEngine;
        private Controls.MrngLabel labelBlockCipher;
        private MrngComboBox comboBoxBlockCipher;
        private MrngGroupBox groupAdvancedSecurityOptions;
        internal Controls.MrngNumericUpDown numberBoxKdfIterations;
        private Controls.MrngLabel labelKdfIterations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MrngButton btnTestSettings;
        internal System.Windows.Forms.Label lblRegistrySettingsUsedInfo;
        private System.Windows.Forms.Panel pnlOptions;
        private System.Windows.Forms.GroupBox groupPasswordGenerator;
        private System.Windows.Forms.TableLayoutPanel pnlPasswordTxtAndBtn;
        private System.Windows.Forms.TextBox txtPasswdGenerator;
        private System.Windows.Forms.Button btnPasswdGenerator;
        private System.Windows.Forms.Label lblPasswdGenDescription;
    }
}
