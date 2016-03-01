using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace mRemoteNG
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class ReconnectGroup : System.Windows.Forms.UserControl
	{

		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.grpAutomaticReconnect = new System.Windows.Forms.GroupBox();
			this.lblAnimation = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblServerStatus = new System.Windows.Forms.Label();
			this.chkReconnectWhenReady = new System.Windows.Forms.CheckBox();
			this.pbServerStatus = new System.Windows.Forms.PictureBox();
			this.tmrAnimation = new System.Windows.Forms.Timer(this.components);
			this.grpAutomaticReconnect.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.pbServerStatus).BeginInit();
			this.SuspendLayout();
			//
			//grpAutomaticReconnect
			//
			this.grpAutomaticReconnect.BackColor = System.Drawing.Color.White;
			this.grpAutomaticReconnect.Controls.Add(this.lblAnimation);
			this.grpAutomaticReconnect.Controls.Add(this.btnClose);
			this.grpAutomaticReconnect.Controls.Add(this.lblServerStatus);
			this.grpAutomaticReconnect.Controls.Add(this.chkReconnectWhenReady);
			this.grpAutomaticReconnect.Controls.Add(this.pbServerStatus);
			this.grpAutomaticReconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grpAutomaticReconnect.Location = new System.Drawing.Point(3, 0);
			this.grpAutomaticReconnect.Name = "grpAutomaticReconnect";
			this.grpAutomaticReconnect.Size = new System.Drawing.Size(171, 98);
			this.grpAutomaticReconnect.TabIndex = 8;
			this.grpAutomaticReconnect.TabStop = false;
			this.grpAutomaticReconnect.Text = "Automatisches wiederverbinden";
			//
			//lblAnimation
			//
			this.lblAnimation.Location = new System.Drawing.Point(124, 22);
			this.lblAnimation.Name = "lblAnimation";
			this.lblAnimation.Size = new System.Drawing.Size(32, 17);
			this.lblAnimation.TabIndex = 8;
			//
			//btnClose
			//
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Location = new System.Drawing.Point(6, 67);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(159, 23);
			this.btnClose.TabIndex = 7;
			this.btnClose.Text = "&Schließen";
			this.btnClose.UseVisualStyleBackColor = true;
			//
			//lblServerStatus
			//
			this.lblServerStatus.AutoSize = true;
			this.lblServerStatus.Location = new System.Drawing.Point(15, 24);
			this.lblServerStatus.Name = "lblServerStatus";
			this.lblServerStatus.Size = new System.Drawing.Size(74, 13);
			this.lblServerStatus.TabIndex = 3;
			this.lblServerStatus.Text = "Server Status:";
			//
			//chkReconnectWhenReady
			//
			this.chkReconnectWhenReady.AutoSize = true;
			this.chkReconnectWhenReady.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.chkReconnectWhenReady.Location = new System.Drawing.Point(18, 44);
			this.chkReconnectWhenReady.Name = "chkReconnectWhenReady";
			this.chkReconnectWhenReady.Size = new System.Drawing.Size(129, 17);
			this.chkReconnectWhenReady.TabIndex = 6;
			this.chkReconnectWhenReady.Text = "Verbinden wenn bereit";
			this.chkReconnectWhenReady.UseVisualStyleBackColor = true;
			//
			//pbServerStatus
			//
			this.pbServerStatus.Image = global::mRemoteNG.My.Resources.Resources.HostStatus_Check;
			this.pbServerStatus.Location = new System.Drawing.Point(99, 23);
			this.pbServerStatus.Name = "pbServerStatus";
			this.pbServerStatus.Size = new System.Drawing.Size(16, 16);
			this.pbServerStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pbServerStatus.TabIndex = 5;
			this.pbServerStatus.TabStop = false;
			//
			//tmrAnimation
			//
			this.tmrAnimation.Enabled = true;
			this.tmrAnimation.Interval = 200;
			//
			//ReconnectGroup
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.grpAutomaticReconnect);
			this.Name = "ReconnectGroup";
			this.Size = new System.Drawing.Size(228, 138);
			this.grpAutomaticReconnect.ResumeLayout(false);
			this.grpAutomaticReconnect.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.pbServerStatus).EndInit();
			this.ResumeLayout(false);

		}
		internal System.Windows.Forms.GroupBox grpAutomaticReconnect;
		private System.Windows.Forms.Button withEventsField_btnClose;
		internal System.Windows.Forms.Button btnClose {
			get { return withEventsField_btnClose; }
			set {
				if (withEventsField_btnClose != null) {
					withEventsField_btnClose.Click -= btnClose_Click;
				}
				withEventsField_btnClose = value;
				if (withEventsField_btnClose != null) {
					withEventsField_btnClose.Click += btnClose_Click;
				}
			}
		}
		internal System.Windows.Forms.Label lblServerStatus;
		private System.Windows.Forms.CheckBox withEventsField_chkReconnectWhenReady;
		internal System.Windows.Forms.CheckBox chkReconnectWhenReady {
			get { return withEventsField_chkReconnectWhenReady; }
			set {
				if (withEventsField_chkReconnectWhenReady != null) {
					withEventsField_chkReconnectWhenReady.CheckedChanged -= chkReconnectWhenReady_CheckedChanged;
				}
				withEventsField_chkReconnectWhenReady = value;
				if (withEventsField_chkReconnectWhenReady != null) {
					withEventsField_chkReconnectWhenReady.CheckedChanged += chkReconnectWhenReady_CheckedChanged;
				}
			}
		}
		internal System.Windows.Forms.PictureBox pbServerStatus;
		private System.Windows.Forms.Timer withEventsField_tmrAnimation;
		internal System.Windows.Forms.Timer tmrAnimation {
			get { return withEventsField_tmrAnimation; }
			set {
				if (withEventsField_tmrAnimation != null) {
					withEventsField_tmrAnimation.Tick -= tmrAnimation_Tick;
				}
				withEventsField_tmrAnimation = value;
				if (withEventsField_tmrAnimation != null) {
					withEventsField_tmrAnimation.Tick += tmrAnimation_Tick;
				}
			}
		}

		internal System.Windows.Forms.Label lblAnimation;
	}
}
