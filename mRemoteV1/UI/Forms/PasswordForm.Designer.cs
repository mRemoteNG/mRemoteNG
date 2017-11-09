using TextBox = mRemoteNG.UI.Forms.TextBox;

namespace mRemoteNG.UI.Forms
{
	public 
	partial class PasswordForm : System.Windows.Forms.Form
	{
			
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
			
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
			
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
            this.lblPassword = new Controls.Base.NGLabel();
            this.lblVerify = new Controls.Base.NGLabel();
            this.btnOK = new Controls.Base.NGButton();
            this.btnCancel = new Controls.Base.NGButton();
            this.lblStatus = new Controls.Base.NGLabel();
            this.pbLock = new System.Windows.Forms.PictureBox();
            this.txtVerify = new mRemoteNG.UI.Forms.TextBox();
            this.txtPassword = new mRemoteNG.UI.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLock)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(82, 12);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password:";
            // 
            // lblVerify
            // 
            this.lblVerify.AutoSize = true;
            this.lblVerify.Location = new System.Drawing.Point(82, 51);
            this.lblVerify.Name = "lblVerify";
            this.lblVerify.Size = new System.Drawing.Size(36, 13);
            this.lblVerify.TabIndex = 3;
            this.lblVerify.Text = "Verify:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(210, 119);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = global::mRemoteNG.Language.strButtonOK;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(291, 119);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = global::mRemoteNG.Language.strButtonCancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(85, 90);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(281, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblStatus.Visible = false;
            // 
            // pbLock
            // 
            this.pbLock.Image = global::mRemoteNG.Resources.Lock;
            this.pbLock.Location = new System.Drawing.Point(12, 12);
            this.pbLock.Name = "pbLock";
            this.pbLock.Size = new System.Drawing.Size(64, 64);
            this.pbLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLock.TabIndex = 7;
            this.pbLock.TabStop = false;
            // 
            // txtVerify
            // 
            this.txtVerify.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVerify.Location = new System.Drawing.Point(85, 67);
            this.txtVerify.Name = "txtVerify";
            this.txtVerify.SelectAllOnFocus = true;
            this.txtVerify.Size = new System.Drawing.Size(281, 20);
            this.txtVerify.TabIndex = 4;
            this.txtVerify.UseSystemPasswordChar = true;
            this.txtVerify.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(85, 28);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.SelectAllOnFocus = true;
            this.txtPassword.Size = new System.Drawing.Size(281, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // PasswordForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(378, 154);
            this.ControlBox = false;
            this.Controls.Add(this.pbLock);
            this.Controls.Add(this.txtVerify);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblVerify);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Password";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PasswordForm_FormClosed);
            this.Load += new System.EventHandler(this.frmPassword_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        private TextBox txtPassword;
		private TextBox txtVerify;
		private Controls.Base.NGLabel lblPassword;
		private Controls.Base.NGLabel lblVerify;
		private Controls.Base.NGButton btnOK;
		private Controls.Base.NGButton btnCancel;
		private Controls.Base.NGLabel lblStatus;
		private System.Windows.Forms.PictureBox pbLock;
	}
}
