namespace mRemoteNG.UI.Forms.OptionsPages
{
    partial class CredentialsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredentialsPage));
            this.checkBoxUnlockOnStartup = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxUnlockOnStartup
            // 
            this.checkBoxUnlockOnStartup.AutoSize = true;
            this.checkBoxUnlockOnStartup.Location = new System.Drawing.Point(3, 0);
            this.checkBoxUnlockOnStartup.Name = "checkBoxUnlockOnStartup";
            this.checkBoxUnlockOnStartup.Size = new System.Drawing.Size(261, 17);
            this.checkBoxUnlockOnStartup.TabIndex = 0;
            this.checkBoxUnlockOnStartup.Text = "Prompt to unlock credential repositories on startup";
            this.checkBoxUnlockOnStartup.UseVisualStyleBackColor = true;
            // 
            // CredentialsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxUnlockOnStartup);
            this.Name = "CredentialsPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxUnlockOnStartup;
    }
}
