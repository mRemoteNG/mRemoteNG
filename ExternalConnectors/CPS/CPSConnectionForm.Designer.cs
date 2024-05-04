namespace ExternalConnectors.CPS
{
    partial class CPSConnectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CPSConnectionForm));
            tbServerURL = new TextBox();
            label3 = new Label();
            tbAPIKey = new TextBox();
            btnOK = new Button();
            btnCancel = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            label6 = new Label();
            tbOTP = new TextBox();
            cbUseSSO = new CheckBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            label4 = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tbServerURL
            // 
            tbServerURL.Dock = DockStyle.Fill;
            tbServerURL.Location = new Point(298, 5);
            tbServerURL.Margin = new Padding(5);
            tbServerURL.Name = "tbServerURL";
            tbServerURL.Size = new Size(611, 27);
            tbServerURL.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(5, 84);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(283, 42);
            label3.TabIndex = 5;
            label3.Text = "API Key";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tbAPIKey
            // 
            tbAPIKey.Dock = DockStyle.Fill;
            tbAPIKey.Location = new Point(298, 89);
            tbAPIKey.Margin = new Padding(5);
            tbAPIKey.Name = "tbAPIKey";
            tbAPIKey.Size = new Size(611, 27);
            tbAPIKey.TabIndex = 4;
            tbAPIKey.UseSystemPasswordChar = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Right;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new Point(337, 16);
            btnOK.Margin = new Padding(5);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(101, 35);
            btnOK.TabIndex = 6;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Left;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(474, 16);
            btnCancel.Margin = new Padding(5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(101, 35);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.06997F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67.93003F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label3, 0, 2);
            tableLayoutPanel1.Controls.Add(tbServerURL, 1, 0);
            tableLayoutPanel1.Controls.Add(tbAPIKey, 1, 2);
            tableLayoutPanel1.Controls.Add(label6, 0, 3);
            tableLayoutPanel1.Controls.Add(tbOTP, 1, 3);
            tableLayoutPanel1.Controls.Add(cbUseSSO, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Size = new Size(914, 212);
            tableLayoutPanel1.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(5, 0);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(283, 42);
            label1.TabIndex = 2;
            label1.Text = "Passwordstate URL";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(3, 126);
            label6.Name = "label6";
            label6.Size = new Size(287, 42);
            label6.TabIndex = 15;
            label6.Text = "2FA OTP (Optional)";
            // 
            // tbOTP
            // 
            tbOTP.Dock = DockStyle.Fill;
            tbOTP.Location = new Point(298, 131);
            tbOTP.Margin = new Padding(5);
            tbOTP.Name = "tbOTP";
            tbOTP.Size = new Size(611, 27);
            tbOTP.TabIndex = 5;
            // 
            // cbUseSSO
            // 
            cbUseSSO.Anchor = AnchorStyles.Left;
            cbUseSSO.AutoSize = true;
            cbUseSSO.Location = new Point(5, 53);
            cbUseSSO.Margin = new Padding(5, 5, 5, 0);
            cbUseSSO.Name = "cbUseSSO";
            cbUseSSO.Size = new Size(157, 24);
            cbUseSSO.TabIndex = 14;
            cbUseSSO.Text = "Use SSO / WinAuth";
            cbUseSSO.UseVisualStyleBackColor = true;
            cbUseSSO.CheckedChanged += cbUseSSO_CheckedChanged;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 106F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 26F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 107F));
            tableLayoutPanel2.Controls.Add(btnOK, 1, 0);
            tableLayoutPanel2.Controls.Add(btnCancel, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Location = new Point(0, 300);
            tableLayoutPanel2.Margin = new Padding(5);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(914, 67);
            tableLayoutPanel2.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(0, 212);
            label4.Margin = new Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new Size(345, 20);
            label4.TabIndex = 14;
            label4.Text = "URL is the base URL, like https://pass.domain.local/";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CPSConnectionForm
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 367);
            Controls.Add(label4);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(5);
            Name = "CPSConnectionForm";
            Text = "Passwordstate API Login Data";
            Activated += CPSConnectionForm_Activated;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label3;

        public System.Windows.Forms.TextBox tbServerURL;
        //public System.Windows.Forms.TextBox tbUsername;
        public System.Windows.Forms.TextBox tbAPIKey;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.CheckBox cbUseSSO;
        private System.Windows.Forms.Label label4;
        private Label label6;
        public TextBox tbOTP;
    }
}