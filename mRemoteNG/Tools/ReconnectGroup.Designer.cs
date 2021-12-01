using mRemoteNG.UI.Controls;
using mRemoteNG.Resources.Language;

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
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.grpAutomaticReconnect = new System.Windows.Forms.GroupBox();
            this.lblAnimation = new mRemoteNG.UI.Controls.MrngLabel();
            this.btnClose = new MrngButton();
            this.lblServerStatus = new mRemoteNG.UI.Controls.MrngLabel();
            this.chkReconnectWhenReady = new MrngCheckBox();
            this.pbServerStatus = new System.Windows.Forms.PictureBox();
            this.tmrAnimation = new System.Windows.Forms.Timer(this.components);
            this.grpAutomaticReconnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbServerStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // grpAutomaticReconnect
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
            this.grpAutomaticReconnect.Text = "Automatic Reconnect";
            // 
            // lblAnimation
            // 
            this.lblAnimation.Location = new System.Drawing.Point(124, 22);
            this.lblAnimation.Name = "lblAnimation";
            this.lblAnimation.Size = new System.Drawing.Size(32, 17);
            this.lblAnimation.TabIndex = 8;
            // 
            // btnClose
            // 
            this.btnClose._mice = MrngButton.MouseState.HOVER;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(6, 67);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(159, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = Language._Close;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Location = new System.Drawing.Point(15, 24);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(76, 13);
            this.lblServerStatus.TabIndex = 3;
            this.lblServerStatus.Text = "Server Status:";
            // 
            // chkReconnectWhenReady
            // 
            this.chkReconnectWhenReady._mice = MrngCheckBox.MouseState.HOVER;
            this.chkReconnectWhenReady.AutoSize = true;
            this.chkReconnectWhenReady.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkReconnectWhenReady.Location = new System.Drawing.Point(18, 44);
            this.chkReconnectWhenReady.Name = "chkReconnectWhenReady";
            this.chkReconnectWhenReady.Size = new System.Drawing.Size(140, 17);
            this.chkReconnectWhenReady.TabIndex = 6;
            this.chkReconnectWhenReady.Text = Language.CheckboxReconnectWhenReady;
            this.chkReconnectWhenReady.UseVisualStyleBackColor = true;
            this.chkReconnectWhenReady.CheckedChanged += new System.EventHandler(this.chkReconnectWhenReady_CheckedChanged);
            // 
            // pbServerStatus
            // 
            this.pbServerStatus.Image = global::mRemoteNG.Properties.Resources.HostStatus_Check;
            this.pbServerStatus.Location = new System.Drawing.Point(99, 23);
            this.pbServerStatus.Name = "pbServerStatus";
            this.pbServerStatus.Size = new System.Drawing.Size(16, 16);
            this.pbServerStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbServerStatus.TabIndex = 5;
            this.pbServerStatus.TabStop = false;
            // 
            // tmrAnimation
            // 
            this.tmrAnimation.Enabled = true;
            this.tmrAnimation.Interval = 200;
            this.tmrAnimation.Tick += new System.EventHandler(this.tmrAnimation_Tick);
            // 
            // ReconnectGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.grpAutomaticReconnect);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ReconnectGroup";
            this.Size = new System.Drawing.Size(228, 138);
            this.grpAutomaticReconnect.ResumeLayout(false);
            this.grpAutomaticReconnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbServerStatus)).EndInit();
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.GroupBox grpAutomaticReconnect;
		internal MrngButton btnClose;
		internal UI.Controls.MrngLabel lblServerStatus;
		internal MrngCheckBox chkReconnectWhenReady;
		internal System.Windows.Forms.PictureBox pbServerStatus;
		internal System.Windows.Forms.Timer tmrAnimation;
		internal UI.Controls.MrngLabel lblAnimation;
        private System.ComponentModel.IContainer components;
    }
}