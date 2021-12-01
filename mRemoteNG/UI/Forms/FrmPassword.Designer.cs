using mRemoteNG.UI.Controls;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Forms
{
	public 
	partial class FrmPassword : System.Windows.Forms.Form
	{
			
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
                    if(components != null)
					    components.Dispose();

                    if(_password != null)
                        _password.Dispose();
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
            this.lblPassword = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblVerify = new mRemoteNG.UI.Controls.MrngLabel();
            this.btnOK = new MrngButton();
            this.btnCancel = new MrngButton();
            this.lblStatus = new mRemoteNG.UI.Controls.MrngLabel();
            this.pbLock = new System.Windows.Forms.PictureBox();
            this.txtVerify = new mRemoteNG.UI.Controls.MrngTextBox();
            this.txtPassword = new mRemoteNG.UI.Controls.MrngTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pbLock)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(73, 9);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(59, 13);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password:";
            // 
            // lblVerify
            // 
            this.lblVerify.AutoSize = true;
            this.lblVerify.Location = new System.Drawing.Point(73, 50);
            this.lblVerify.Name = "lblVerify";
            this.lblVerify.Size = new System.Drawing.Size(38, 13);
            this.lblVerify.TabIndex = 3;
            this.lblVerify.Text = "Verify:";
            // 
            // btnOK
            // 
            this.btnOK._mice = MrngButton.MouseState.HOVER;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(215, 124);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = Language._Ok;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel._mice = MrngButton.MouseState.HOVER;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(296, 124);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = Language._Cancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.lblStatus, 2);
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(73, 91);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(298, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblStatus.Visible = false;
            // 
            // pbLock
            // 
            this.pbLock.Image = global::mRemoteNG.Properties.Resources.ASPWebSite_16x;
            this.pbLock.Location = new System.Drawing.Point(3, 12);
            this.pbLock.Name = "pbLock";
            this.tableLayoutPanel1.SetRowSpan(this.pbLock, 6);
            this.pbLock.Size = new System.Drawing.Size(64, 64);
            this.pbLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLock.TabIndex = 7;
            this.pbLock.TabStop = false;
            // 
            // txtVerify
            // 
            this.txtVerify.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtVerify, 2);
            this.txtVerify.Location = new System.Drawing.Point(73, 66);
            this.txtVerify.Name = "txtVerify";
            this.txtVerify.Size = new System.Drawing.Size(298, 22);
            this.txtVerify.TabIndex = 4;
            this.txtVerify.UseSystemPasswordChar = true;
            this.txtVerify.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtPassword, 2);
            this.txtPassword.Location = new System.Drawing.Point(73, 25);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(298, 22);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.pbLock, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtVerify, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblVerify, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtPassword, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblPassword, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.33334F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(374, 150);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // PasswordForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(374, 150);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Password";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PasswordForm_FormClosed);
            this.Load += new System.EventHandler(this.FrmPassword_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLock)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

		}
        private Controls.MrngTextBox txtPassword;
		private Controls.MrngTextBox txtVerify;
		private Controls.MrngLabel lblPassword;
		private Controls.MrngLabel lblVerify;
		private MrngButton btnOK;
		private MrngButton btnCancel;
		private Controls.MrngLabel lblStatus;
		private System.Windows.Forms.PictureBox pbLock;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
