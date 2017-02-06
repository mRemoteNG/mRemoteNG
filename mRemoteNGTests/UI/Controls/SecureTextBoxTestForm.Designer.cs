using mRemoteNG.UI.Controls;

namespace mRemoteNGTests.UI.Controls
{
    partial class SecureTextBoxTestForm
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
            this.secureTextBox1 = new mRemoteNG.UI.Controls.SecureTextBox();
            this.SuspendLayout();
            // 
            // secureTextBox1
            // 
            this.secureTextBox1.Location = new System.Drawing.Point(12, 12);
            this.secureTextBox1.Name = "secureTextBox1";
            this.secureTextBox1.ShortcutsEnabled = false;
            this.secureTextBox1.Size = new System.Drawing.Size(232, 20);
            this.secureTextBox1.TabIndex = 0;
            // 
            // SecureTextBoxTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 45);
            this.Controls.Add(this.secureTextBox1);
            this.Name = "SecureTextBoxTestForm";
            this.Text = "SecureTextBoxTestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal SecureTextBox secureTextBox1;
    }
}