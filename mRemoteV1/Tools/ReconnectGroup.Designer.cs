namespace mRemoteNG.Tools
{
    public partial class ReconnectGroup : System.Windows.Forms.UserControl
	{
		//UserControl overrides dispose to clean up the component list.
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
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.grpAutomaticReconnect = new System.Windows.Forms.GroupBox();
			this.lblAnimation = new UI.Controls.Base.NGLabel();
			this.btnClose = new UI.Controls.Base.NGButton();
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			this.lblServerStatus = new UI.Controls.Base.NGLabel();
			this.chkReconnectWhenReady = new UI.Controls.Base.NGCheckBox();
			this.chkReconnectWhenReady.CheckedChanged += new System.EventHandler(this.chkReconnectWhenReady_CheckedChanged);
			this.pbServerStatus = new System.Windows.Forms.PictureBox();
			this.tmrAnimation = new System.Windows.Forms.Timer(this.components);
			this.tmrAnimation.Tick += new System.EventHandler(this.tmrAnimation_Tick);
			this.grpAutomaticReconnect.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.pbServerStatus).BeginInit();
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
			this.grpAutomaticReconnect.Text = Language.strGroupboxAutomaticReconnect;
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
			this.btnClose.Text = Language.strButtonClose;
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
			this.chkReconnectWhenReady.Text = Language.strCheckboxReconnectWhenReady;
			this.chkReconnectWhenReady.UseVisualStyleBackColor = true;
			//
			//pbServerStatus
			//
			this.pbServerStatus.Image = Resources.HostStatus_Check;
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
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.grpAutomaticReconnect);
			this.Name = "ReconnectGroup";
			this.Size = new System.Drawing.Size(228, 138);
			this.grpAutomaticReconnect.ResumeLayout(false);
			this.grpAutomaticReconnect.PerformLayout();
			((System.ComponentModel.ISupportInitialize) this.pbServerStatus).EndInit();
			this.ResumeLayout(false);
		}
		internal System.Windows.Forms.GroupBox grpAutomaticReconnect;
		internal UI.Controls.Base.NGButton btnClose;
		internal UI.Controls.Base.NGLabel lblServerStatus;
		internal UI.Controls.Base.NGCheckBox chkReconnectWhenReady;
		internal System.Windows.Forms.PictureBox pbServerStatus;
		internal System.Windows.Forms.Timer tmrAnimation;
		internal UI.Controls.Base.NGLabel lblAnimation;
	}
}