// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using AxWFICALib;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using AxMSTSCLib;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports


namespace mRemoteNG.Forms
{
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public 
	partial class PasswordForm : System.Windows.Forms.Form
		{
			
			//Form overrides dispose to clean up the component list.
			[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
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
			[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
			{
				this.lblPassword = new System.Windows.Forms.Label();
				this.Load += new System.EventHandler(frmPassword_Load);
				this.lblVerify = new System.Windows.Forms.Label();
				this.btnOK = new System.Windows.Forms.Button();
				this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
				this.btnCancel = new System.Windows.Forms.Button();
				this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
				this.lblStatus = new System.Windows.Forms.Label();
				this.pbLock = new System.Windows.Forms.PictureBox();
				this.txtVerify = new mRemoteNG.Controls.TextBox();
				this.txtVerify.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
				this.txtPassword = new mRemoteNG.Controls.TextBox();
				this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
				((System.ComponentModel.ISupportInitialize) this.pbLock).BeginInit();
				this.SuspendLayout();
				//
				//lblPassword
				//
				this.lblPassword.AutoSize = true;
				this.lblPassword.Location = new System.Drawing.Point(82, 12);
				this.lblPassword.Name = "lblPassword";
				this.lblPassword.Size = new System.Drawing.Size(56, 13);
				this.lblPassword.TabIndex = 1;
				this.lblPassword.Text = "Password:";
				//
				//lblVerify
				//
				this.lblVerify.AutoSize = true;
				this.lblVerify.Location = new System.Drawing.Point(82, 51);
				this.lblVerify.Name = "lblVerify";
				this.lblVerify.Size = new System.Drawing.Size(36, 13);
				this.lblVerify.TabIndex = 3;
				this.lblVerify.Text = "Verify:";
				//
				//btnOK
				//
				this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
				this.btnOK.Location = new System.Drawing.Point(291, 119);
				this.btnOK.Name = "btnOK";
				this.btnOK.Size = new System.Drawing.Size(75, 23);
				this.btnOK.TabIndex = 7;
				this.btnOK.Text = global::mRemoteNG.My.Language.strButtonOK;
				this.btnOK.UseVisualStyleBackColor = true;
				//
				//btnCancel
				//
				this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
				this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
				this.btnCancel.Location = new System.Drawing.Point(210, 119);
				this.btnCancel.Name = "btnCancel";
				this.btnCancel.Size = new System.Drawing.Size(75, 23);
				this.btnCancel.TabIndex = 6;
				this.btnCancel.Text = global::mRemoteNG.My.Language.strButtonCancel;
				this.btnCancel.UseVisualStyleBackColor = true;
				//
				//lblStatus
				//
				this.lblStatus.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
					| System.Windows.Forms.AnchorStyles.Right);
				this.lblStatus.ForeColor = System.Drawing.Color.Red;
				this.lblStatus.Location = new System.Drawing.Point(85, 90);
				this.lblStatus.Name = "lblStatus";
				this.lblStatus.Size = new System.Drawing.Size(281, 13);
				this.lblStatus.TabIndex = 5;
				this.lblStatus.Text = "Status";
				this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
				this.lblStatus.Visible = false;
				//
				//pbLock
				//
				this.pbLock.Image = global::My.Resources.Resources.Lock;
				this.pbLock.Location = new System.Drawing.Point(12, 12);
				this.pbLock.Name = "pbLock";
				this.pbLock.Size = new System.Drawing.Size(64, 64);
				this.pbLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
				this.pbLock.TabIndex = 7;
				this.pbLock.TabStop = false;
				//
				//txtVerify
				//
				this.txtVerify.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
					| System.Windows.Forms.AnchorStyles.Right);
				this.txtVerify.Location = new System.Drawing.Point(85, 67);
				this.txtVerify.Name = "txtVerify";
				this.txtVerify.SelectAllOnFocus = true;
				this.txtVerify.Size = new System.Drawing.Size(281, 20);
				this.txtVerify.TabIndex = 4;
				this.txtVerify.UseSystemPasswordChar = true;
				//
				//txtPassword
				//
				this.txtPassword.Anchor = (System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
					| System.Windows.Forms.AnchorStyles.Right);
				this.txtPassword.Location = new System.Drawing.Point(85, 28);
				this.txtPassword.Name = "txtPassword";
				this.txtPassword.SelectAllOnFocus = true;
				this.txtPassword.Size = new System.Drawing.Size(281, 20);
				this.txtPassword.TabIndex = 2;
				this.txtPassword.UseSystemPasswordChar = true;
				//
				//frmPassword
				//
				this.AcceptButton = this.btnOK;
				this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
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
				((System.ComponentModel.ISupportInitialize) this.pbLock).EndInit();
				this.ResumeLayout(false);
				this.PerformLayout();
				
			}
			private Controls.TextBox txtPassword;
			private Controls.TextBox txtVerify;
			private System.Windows.Forms.Label lblPassword;
			private System.Windows.Forms.Label lblVerify;
			private System.Windows.Forms.Button btnOK;
			private System.Windows.Forms.Button btnCancel;
			private System.Windows.Forms.Label lblStatus;
			private System.Windows.Forms.PictureBox pbLock;
		}
}
