namespace mRemoteNG.UI.Controls
{
    partial class NewPasswordWithVerification
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
            this.labelFirstPasswordBox = new Controls.Base.NGLabel();
            this.labelSecondPasswordBox = new Controls.Base.NGLabel();
            this.labelPasswordsDontMatch = new Controls.Base.NGLabel();
            this.imgError = new System.Windows.Forms.PictureBox();
            this.secureTextBox2 = new mRemoteNG.UI.Controls.SecureTextBox();
            this.secureTextBox1 = new mRemoteNG.UI.Controls.SecureTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgError)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFirstPasswordBox
            // 
            this.labelFirstPasswordBox.AutoSize = true;
            this.labelFirstPasswordBox.Location = new System.Drawing.Point(-3, 0);
            this.labelFirstPasswordBox.Name = "labelFirstPasswordBox";
            this.labelFirstPasswordBox.Size = new System.Drawing.Size(78, 13);
            this.labelFirstPasswordBox.TabIndex = 2;
            this.labelFirstPasswordBox.Text = "New Password";
            // 
            // labelSecondPasswordBox
            // 
            this.labelSecondPasswordBox.AutoSize = true;
            this.labelSecondPasswordBox.Location = new System.Drawing.Point(-3, 42);
            this.labelSecondPasswordBox.Name = "labelSecondPasswordBox";
            this.labelSecondPasswordBox.Size = new System.Drawing.Size(79, 13);
            this.labelSecondPasswordBox.TabIndex = 3;
            this.labelSecondPasswordBox.Text = "VerifyPassword";
            // 
            // labelPasswordsDontMatch
            // 
            this.labelPasswordsDontMatch.AutoSize = true;
            this.labelPasswordsDontMatch.Location = new System.Drawing.Point(23, 83);
            this.labelPasswordsDontMatch.Name = "labelPasswordsDontMatch";
            this.labelPasswordsDontMatch.Size = new System.Drawing.Size(123, 13);
            this.labelPasswordsDontMatch.TabIndex = 4;
            this.labelPasswordsDontMatch.Text = "Passwords do not match";
            this.labelPasswordsDontMatch.Visible = false;
            // 
            // imgError
            // 
            this.imgError.Image = global::mRemoteNG.Resources.ErrorSmall;
            this.imgError.Location = new System.Drawing.Point(3, 81);
            this.imgError.Name = "imgError";
            this.imgError.Size = new System.Drawing.Size(16, 16);
            this.imgError.TabIndex = 5;
            this.imgError.TabStop = false;
            this.imgError.Visible = false;
            // 
            // secureTextBox2
            // 
            this.secureTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secureTextBox2.Location = new System.Drawing.Point(0, 58);
            this.secureTextBox2.Name = "secureTextBox2";
            this.secureTextBox2.Size = new System.Drawing.Size(193, 20);
            this.secureTextBox2.TabIndex = 1;
            // 
            // secureTextBox1
            // 
            this.secureTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secureTextBox1.Location = new System.Drawing.Point(0, 16);
            this.secureTextBox1.Name = "secureTextBox1";
            this.secureTextBox1.Size = new System.Drawing.Size(193, 20);
            this.secureTextBox1.TabIndex = 0;
            // 
            // NewPasswordWithVerification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imgError);
            this.Controls.Add(this.labelPasswordsDontMatch);
            this.Controls.Add(this.labelSecondPasswordBox);
            this.Controls.Add(this.labelFirstPasswordBox);
            this.Controls.Add(this.secureTextBox2);
            this.Controls.Add(this.secureTextBox1);
            this.MinimumSize = new System.Drawing.Size(0, 100);
            this.Name = "NewPasswordWithVerification";
            this.Size = new System.Drawing.Size(193, 100);
            ((System.ComponentModel.ISupportInitialize)(this.imgError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SecureTextBox secureTextBox1;
        private SecureTextBox secureTextBox2;
        private Controls.Base.NGLabel labelFirstPasswordBox;
        private Controls.Base.NGLabel labelSecondPasswordBox;
        private Controls.Base.NGLabel labelPasswordsDontMatch;
        private System.Windows.Forms.PictureBox imgError;
    }
}
