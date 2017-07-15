namespace mRemoteNG.UI.Window
{
    partial class SSHCommandWIndow
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
            this.txtSSHCommand = new System.Windows.Forms.RichTextBox();
            this.lstCommands = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // txtSSHCommand
            // 
            this.txtSSHCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSSHCommand.Location = new System.Drawing.Point(0, 0);
            this.txtSSHCommand.MinimumSize = new System.Drawing.Size(0, 150);
            this.txtSSHCommand.Name = "txtSSHCommand";
            this.txtSSHCommand.Size = new System.Drawing.Size(821, 532);
            this.txtSSHCommand.TabIndex = 0;
            this.txtSSHCommand.Text = "";
            this.txtSSHCommand.Enter += new System.EventHandler(this.txtSSHCommand_Enter);
            this.txtSSHCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSSHCommand_KeyDown);
            this.txtSSHCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSSHCommand_KeyUp);
            // 
            // lstCommands
            // 
            this.lstCommands.Dock = System.Windows.Forms.DockStyle.Right;
            this.lstCommands.FormattingEnabled = true;
            this.lstCommands.ItemHeight = 16;
            this.lstCommands.Location = new System.Drawing.Point(701, 0);
            this.lstCommands.Name = "lstCommands";
            this.lstCommands.Size = new System.Drawing.Size(120, 532);
            this.lstCommands.TabIndex = 1;
            this.lstCommands.Visible = false;
            // 
            // SSHCommandWIndow
            // 
            this.ClientSize = new System.Drawing.Size(821, 532);
            this.Controls.Add(this.lstCommands);
            this.Controls.Add(this.txtSSHCommand);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SSHCommandWIndow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtSSHCommand;
        private System.Windows.Forms.ListBox lstCommands;
    }
}