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
            // SecurityPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkEncryptCompleteFile);
            this.Name = "SecurityPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox chkEncryptCompleteFile;
    }
}
