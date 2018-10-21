

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public sealed partial class AdvancedPage : OptionsPage
	{
		//UserControl overrides dispose to clean up the component list.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedPage));
            this.chkAutomaticallyGetSessionInfo = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.lblMaximumPuttyWaitTime = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.chkAutomaticReconnect = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.numPuttyWaitTime = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.chkUseCustomPuttyPath = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.lblConfigurePuttySessions = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.numUVNCSCPort = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.txtCustomPuttyPath = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.btnLaunchPutty = new mRemoteNG.UI.Controls.Base.NGButton();
            this.lblUVNCSCPort = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.lblSeconds = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.btnBrowseCustomPuttyPath = new mRemoteNG.UI.Controls.Base.NGButton();
            this.chkLoadBalanceInfoUseUtf8 = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPuttyWaitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUVNCSCPort)).BeginInit();
            this.SuspendLayout();
            // 
            // chkAutomaticallyGetSessionInfo
            // 
            this.chkAutomaticallyGetSessionInfo._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkAutomaticallyGetSessionInfo.AutoSize = true;
            this.chkAutomaticallyGetSessionInfo.Location = new System.Drawing.Point(3, 3);
            this.chkAutomaticallyGetSessionInfo.Name = "chkAutomaticallyGetSessionInfo";
            this.chkAutomaticallyGetSessionInfo.Size = new System.Drawing.Size(198, 17);
            this.chkAutomaticallyGetSessionInfo.TabIndex = 0;
            this.chkAutomaticallyGetSessionInfo.Text = "Automatically get session information";
            this.chkAutomaticallyGetSessionInfo.UseVisualStyleBackColor = true;
            // 
            // lblMaximumPuttyWaitTime
            // 
            this.lblMaximumPuttyWaitTime.Location = new System.Drawing.Point(3, 175);
            this.lblMaximumPuttyWaitTime.Name = "lblMaximumPuttyWaitTime";
            this.lblMaximumPuttyWaitTime.Size = new System.Drawing.Size(364, 13);
            this.lblMaximumPuttyWaitTime.TabIndex = 7;
            this.lblMaximumPuttyWaitTime.Text = "Maximum PuTTY wait time:";
            this.lblMaximumPuttyWaitTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkAutomaticReconnect
            // 
            this.chkAutomaticReconnect._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkAutomaticReconnect.AutoSize = true;
            this.chkAutomaticReconnect.Location = new System.Drawing.Point(3, 26);
            this.chkAutomaticReconnect.Name = "chkAutomaticReconnect";
            this.chkAutomaticReconnect.Size = new System.Drawing.Size(399, 17);
            this.chkAutomaticReconnect.TabIndex = 1;
            this.chkAutomaticReconnect.Text = "Automatically try to reconnect when disconnected from server (RDP && ICA only)";
            this.chkAutomaticReconnect.UseVisualStyleBackColor = true;
            // 
            // numPuttyWaitTime
            // 
            this.numPuttyWaitTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPuttyWaitTime.Location = new System.Drawing.Point(373, 173);
            this.numPuttyWaitTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numPuttyWaitTime.Name = "numPuttyWaitTime";
            this.numPuttyWaitTime.Size = new System.Drawing.Size(49, 20);
            this.numPuttyWaitTime.TabIndex = 7;
            this.numPuttyWaitTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chkUseCustomPuttyPath
            // 
            this.chkUseCustomPuttyPath._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkUseCustomPuttyPath.AutoSize = true;
            this.chkUseCustomPuttyPath.Location = new System.Drawing.Point(3, 72);
            this.chkUseCustomPuttyPath.Name = "chkUseCustomPuttyPath";
            this.chkUseCustomPuttyPath.Size = new System.Drawing.Size(146, 17);
            this.chkUseCustomPuttyPath.TabIndex = 3;
            this.chkUseCustomPuttyPath.Text = "Use custom PuTTY path:";
            this.chkUseCustomPuttyPath.UseVisualStyleBackColor = true;
            this.chkUseCustomPuttyPath.CheckedChanged += new System.EventHandler(this.chkUseCustomPuttyPath_CheckedChanged);
            // 
            // lblConfigurePuttySessions
            // 
            this.lblConfigurePuttySessions.Location = new System.Drawing.Point(3, 144);
            this.lblConfigurePuttySessions.Name = "lblConfigurePuttySessions";
            this.lblConfigurePuttySessions.Size = new System.Drawing.Size(364, 13);
            this.lblConfigurePuttySessions.TabIndex = 5;
            this.lblConfigurePuttySessions.Text = "To configure PuTTY sessions click this button:";
            this.lblConfigurePuttySessions.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numUVNCSCPort
            // 
            this.numUVNCSCPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUVNCSCPort.Location = new System.Drawing.Point(373, 218);
            this.numUVNCSCPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUVNCSCPort.Name = "numUVNCSCPort";
            this.numUVNCSCPort.Size = new System.Drawing.Size(72, 20);
            this.numUVNCSCPort.TabIndex = 8;
            this.numUVNCSCPort.Value = new decimal(new int[] {
            5500,
            0,
            0,
            0});
            this.numUVNCSCPort.Visible = false;
            // 
            // txtCustomPuttyPath
            // 
            this.txtCustomPuttyPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomPuttyPath.Enabled = false;
            this.txtCustomPuttyPath.Location = new System.Drawing.Point(21, 95);
            this.txtCustomPuttyPath.Name = "txtCustomPuttyPath";
            this.txtCustomPuttyPath.Size = new System.Drawing.Size(346, 20);
            this.txtCustomPuttyPath.TabIndex = 4;
            this.txtCustomPuttyPath.TextChanged += new System.EventHandler(this.txtCustomPuttyPath_TextChanged);
            // 
            // btnLaunchPutty
            // 
            this.btnLaunchPutty._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnLaunchPutty.Image = global::mRemoteNG.Resources.PuttyConfig;
            this.btnLaunchPutty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLaunchPutty.Location = new System.Drawing.Point(373, 138);
            this.btnLaunchPutty.Name = "btnLaunchPutty";
            this.btnLaunchPutty.Size = new System.Drawing.Size(110, 25);
            this.btnLaunchPutty.TabIndex = 6;
            this.btnLaunchPutty.Text = "Launch PuTTY";
            this.btnLaunchPutty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLaunchPutty.UseVisualStyleBackColor = true;
            this.btnLaunchPutty.Click += new System.EventHandler(this.btnLaunchPutty_Click);
            // 
            // lblUVNCSCPort
            // 
            this.lblUVNCSCPort.Location = new System.Drawing.Point(3, 220);
            this.lblUVNCSCPort.Name = "lblUVNCSCPort";
            this.lblUVNCSCPort.Size = new System.Drawing.Size(364, 13);
            this.lblUVNCSCPort.TabIndex = 10;
            this.lblUVNCSCPort.Text = "UltraVNC SingleClick Listening Port:";
            this.lblUVNCSCPort.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblUVNCSCPort.Visible = false;
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(428, 175);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(47, 13);
            this.lblSeconds.TabIndex = 9;
            this.lblSeconds.Text = "seconds";
            // 
            // btnBrowseCustomPuttyPath
            // 
            this.btnBrowseCustomPuttyPath._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnBrowseCustomPuttyPath.Enabled = false;
            this.btnBrowseCustomPuttyPath.Location = new System.Drawing.Point(373, 93);
            this.btnBrowseCustomPuttyPath.Name = "btnBrowseCustomPuttyPath";
            this.btnBrowseCustomPuttyPath.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseCustomPuttyPath.TabIndex = 5;
            this.btnBrowseCustomPuttyPath.Text = "Browse...";
            this.btnBrowseCustomPuttyPath.UseVisualStyleBackColor = true;
            this.btnBrowseCustomPuttyPath.Click += new System.EventHandler(this.btnBrowseCustomPuttyPath_Click);
            // 
            // chkLoadBalanceInfoUseUtf8
            // 
            this.chkLoadBalanceInfoUseUtf8._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkLoadBalanceInfoUseUtf8.AutoSize = true;
            this.chkLoadBalanceInfoUseUtf8.Location = new System.Drawing.Point(3, 49);
            this.chkLoadBalanceInfoUseUtf8.Name = "chkLoadBalanceInfoUseUtf8";
            this.chkLoadBalanceInfoUseUtf8.Size = new System.Drawing.Size(304, 17);
            this.chkLoadBalanceInfoUseUtf8.TabIndex = 2;
            this.chkLoadBalanceInfoUseUtf8.Text = "Use UTF8 encoding for RDP \"Load Balance Info\" property";
            this.chkLoadBalanceInfoUseUtf8.UseVisualStyleBackColor = true;
            // 
            // AdvancedPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkLoadBalanceInfoUseUtf8);
            this.Controls.Add(this.chkAutomaticallyGetSessionInfo);
            this.Controls.Add(this.lblMaximumPuttyWaitTime);
            this.Controls.Add(this.chkAutomaticReconnect);
            this.Controls.Add(this.numPuttyWaitTime);
            this.Controls.Add(this.chkUseCustomPuttyPath);
            this.Controls.Add(this.lblConfigurePuttySessions);
            this.Controls.Add(this.numUVNCSCPort);
            this.Controls.Add(this.txtCustomPuttyPath);
            this.Controls.Add(this.btnLaunchPutty);
            this.Controls.Add(this.lblUVNCSCPort);
            this.Controls.Add(this.lblSeconds);
            this.Controls.Add(this.btnBrowseCustomPuttyPath);
            this.Name = "AdvancedPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            ((System.ComponentModel.ISupportInitialize)(this.numPuttyWaitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUVNCSCPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal Controls.Base.NGCheckBox chkAutomaticallyGetSessionInfo;
		internal Controls.Base.NGLabel lblMaximumPuttyWaitTime;
		internal Controls.Base.NGCheckBox chkAutomaticReconnect;
		internal Controls.Base.NGNumericUpDown numPuttyWaitTime;
		internal Controls.Base.NGCheckBox chkUseCustomPuttyPath;
		internal Controls.Base.NGLabel lblConfigurePuttySessions;
		internal Controls.Base.NGNumericUpDown numUVNCSCPort;
		internal Controls.Base.NGTextBox txtCustomPuttyPath;
		internal Controls.Base.NGButton btnLaunchPutty;
		internal Controls.Base.NGLabel lblUVNCSCPort;
		internal Controls.Base.NGLabel lblSeconds;
		internal Controls.Base.NGButton btnBrowseCustomPuttyPath;
        private Controls.Base.NGCheckBox chkLoadBalanceInfoUseUtf8;
    }
}
