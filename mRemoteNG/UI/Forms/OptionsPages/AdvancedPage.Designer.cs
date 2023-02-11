

using mRemoteNG.UI.Controls;

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
            this.lblMaximumPuttyWaitTime = new mRemoteNG.UI.Controls.MrngLabel();
            this.chkAutomaticReconnect = new mRemoteNG.UI.Controls.MrngCheckBox();
            this.numPuttyWaitTime = new mRemoteNG.UI.Controls.MrngNumericUpDown();
            this.chkUseCustomPuttyPath = new mRemoteNG.UI.Controls.MrngCheckBox();
            this.lblConfigurePuttySessions = new mRemoteNG.UI.Controls.MrngLabel();
            this.numUVNCSCPort = new mRemoteNG.UI.Controls.MrngNumericUpDown();
            this.txtCustomPuttyPath = new mRemoteNG.UI.Controls.MrngTextBox();
            this.btnLaunchPutty = new mRemoteNG.UI.Controls.MrngButton();
            this.lblUVNCSCPort = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblSeconds = new mRemoteNG.UI.Controls.MrngLabel();
            this.btnBrowseCustomPuttyPath = new mRemoteNG.UI.Controls.MrngButton();
            this.chkLoadBalanceInfoUseUtf8 = new mRemoteNG.UI.Controls.MrngCheckBox();
            this.chkNoReconnect = new mRemoteNG.UI.Controls.MrngCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPuttyWaitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUVNCSCPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMaximumPuttyWaitTime
            // 
            this.lblMaximumPuttyWaitTime.Location = new System.Drawing.Point(4, 253);
            this.lblMaximumPuttyWaitTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaximumPuttyWaitTime.Name = "lblMaximumPuttyWaitTime";
            this.lblMaximumPuttyWaitTime.Size = new System.Drawing.Size(546, 30);
            this.lblMaximumPuttyWaitTime.TabIndex = 7;
            this.lblMaximumPuttyWaitTime.Text = "Maximum PuTTY wait time:";
            this.lblMaximumPuttyWaitTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkAutomaticReconnect
            // 
            this.chkAutomaticReconnect._mice = mRemoteNG.UI.Controls.MrngCheckBox.MouseState.OUT;
            this.chkAutomaticReconnect.AutoSize = true;
            this.chkAutomaticReconnect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkAutomaticReconnect.Location = new System.Drawing.Point(4, 4);
            this.chkAutomaticReconnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAutomaticReconnect.Name = "chkAutomaticReconnect";
            this.chkAutomaticReconnect.Size = new System.Drawing.Size(625, 27);
            this.chkAutomaticReconnect.TabIndex = 1;
            this.chkAutomaticReconnect.Text = "Display reconnection dialog when disconnected from server (RDP && ICA only)";
            this.chkAutomaticReconnect.UseVisualStyleBackColor = true;
            this.chkAutomaticReconnect.CheckedChanged += new System.EventHandler(this.chkAutomaticReconnect_CheckedChanged);
            // 
            // chkNoReconnect
            // 
            this.chkNoReconnect._mice = mRemoteNG.UI.Controls.MrngCheckBox.MouseState.OUT;
            this.chkNoReconnect.AutoSize = true;
            this.chkNoReconnect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkNoReconnect.Location = new System.Drawing.Point(32, 39);
            this.chkNoReconnect.Margin = new System.Windows.Forms.Padding(4);
            this.chkNoReconnect.Name = "chkNoReconnect";
            this.chkNoReconnect.Size = new System.Drawing.Size(645, 27);
            this.chkNoReconnect.TabIndex = 11;
            this.chkNoReconnect.Text = "Automatically try to reconnect when disconnected from server (RDP && ICA only)";
            this.chkNoReconnect.UseVisualStyleBackColor = true;
            this.chkNoReconnect.CheckedChanged += new System.EventHandler(this.chkNoReconnect_CheckedChanged);
            // 
            // numPuttyWaitTime
            // 
            this.numPuttyWaitTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPuttyWaitTime.Location = new System.Drawing.Point(1260, 482);
            this.numPuttyWaitTime.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.numPuttyWaitTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numPuttyWaitTime.Name = "numPuttyWaitTime";
            this.numPuttyWaitTime.Size = new System.Drawing.Size(202, 29);
            this.numPuttyWaitTime.TabIndex = 7;
            this.numPuttyWaitTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chkUseCustomPuttyPath
            // 
            this.chkUseCustomPuttyPath._mice = mRemoteNG.UI.Controls.MrngCheckBox.MouseState.OUT;
            this.chkUseCustomPuttyPath.AutoSize = true;
            this.chkUseCustomPuttyPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkUseCustomPuttyPath.Location = new System.Drawing.Point(4, 109);
            this.chkUseCustomPuttyPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUseCustomPuttyPath.Name = "chkUseCustomPuttyPath";
            this.chkUseCustomPuttyPath.Size = new System.Drawing.Size(221, 27);
            this.chkUseCustomPuttyPath.TabIndex = 3;
            this.chkUseCustomPuttyPath.Text = "Use custom PuTTY path:";
            this.chkUseCustomPuttyPath.UseVisualStyleBackColor = true;
            this.chkUseCustomPuttyPath.CheckedChanged += new System.EventHandler(this.chkUseCustomPuttyPath_CheckedChanged);
            // 
            // lblConfigurePuttySessions
            // 
            this.lblConfigurePuttySessions.Location = new System.Drawing.Point(4, 199);
            this.lblConfigurePuttySessions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigurePuttySessions.Name = "lblConfigurePuttySessions";
            this.lblConfigurePuttySessions.Size = new System.Drawing.Size(546, 38);
            this.lblConfigurePuttySessions.TabIndex = 5;
            this.lblConfigurePuttySessions.Text = "To configure PuTTY sessions click this button:";
            this.lblConfigurePuttySessions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numUVNCSCPort
            // 
            this.numUVNCSCPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUVNCSCPort.Location = new System.Drawing.Point(1260, 576);
            this.numUVNCSCPort.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.numUVNCSCPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUVNCSCPort.Name = "numUVNCSCPort";
            this.numUVNCSCPort.Size = new System.Drawing.Size(202, 29);
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
            this.txtCustomPuttyPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtCustomPuttyPath.Location = new System.Drawing.Point(32, 143);
            this.txtCustomPuttyPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCustomPuttyPath.Name = "txtCustomPuttyPath";
            this.txtCustomPuttyPath.Size = new System.Drawing.Size(518, 29);
            this.txtCustomPuttyPath.TabIndex = 4;
            this.txtCustomPuttyPath.TextChanged += new System.EventHandler(this.txtCustomPuttyPath_TextChanged);
            // 
            // btnLaunchPutty
            // 
            this.btnLaunchPutty._mice = mRemoteNG.UI.Controls.MrngButton.MouseState.OUT;
            this.btnLaunchPutty.Image = global::mRemoteNG.Properties.Resources.PuttyConfig;
            this.btnLaunchPutty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLaunchPutty.Location = new System.Drawing.Point(560, 199);
            this.btnLaunchPutty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLaunchPutty.Name = "btnLaunchPutty";
            this.btnLaunchPutty.Size = new System.Drawing.Size(183, 38);
            this.btnLaunchPutty.TabIndex = 6;
            this.btnLaunchPutty.Text = "Launch PuTTY";
            this.btnLaunchPutty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLaunchPutty.UseVisualStyleBackColor = true;
            this.btnLaunchPutty.Click += new System.EventHandler(this.btnLaunchPutty_Click);
            // 
            // lblUVNCSCPort
            // 
            this.lblUVNCSCPort.Location = new System.Drawing.Point(4, 296);
            this.lblUVNCSCPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUVNCSCPort.Name = "lblUVNCSCPort";
            this.lblUVNCSCPort.Size = new System.Drawing.Size(546, 28);
            this.lblUVNCSCPort.TabIndex = 10;
            this.lblUVNCSCPort.Text = "UltraVNC SingleClick Listening Port:";
            this.lblUVNCSCPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUVNCSCPort.Visible = false;
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(658, 259);
            this.lblSeconds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(71, 23);
            this.lblSeconds.TabIndex = 9;
            this.lblSeconds.Text = "seconds";
            // 
            // btnBrowseCustomPuttyPath
            // 
            this.btnBrowseCustomPuttyPath._mice = mRemoteNG.UI.Controls.MrngButton.MouseState.OUT;
            this.btnBrowseCustomPuttyPath.Enabled = false;
            this.btnBrowseCustomPuttyPath.Location = new System.Drawing.Point(560, 141);
            this.btnBrowseCustomPuttyPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseCustomPuttyPath.Name = "btnBrowseCustomPuttyPath";
            this.btnBrowseCustomPuttyPath.Size = new System.Drawing.Size(183, 38);
            this.btnBrowseCustomPuttyPath.TabIndex = 5;
            this.btnBrowseCustomPuttyPath.Text = "Browse...";
            this.btnBrowseCustomPuttyPath.UseVisualStyleBackColor = true;
            this.btnBrowseCustomPuttyPath.Click += new System.EventHandler(this.btnBrowseCustomPuttyPath_Click);
            // 
            // chkLoadBalanceInfoUseUtf8
            // 
            this.chkLoadBalanceInfoUseUtf8._mice = mRemoteNG.UI.Controls.MrngCheckBox.MouseState.OUT;
            this.chkLoadBalanceInfoUseUtf8.AutoSize = true;
            this.chkLoadBalanceInfoUseUtf8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkLoadBalanceInfoUseUtf8.Location = new System.Drawing.Point(4, 74);
            this.chkLoadBalanceInfoUseUtf8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkLoadBalanceInfoUseUtf8.Name = "chkLoadBalanceInfoUseUtf8";
            this.chkLoadBalanceInfoUseUtf8.Size = new System.Drawing.Size(471, 27);
            this.chkLoadBalanceInfoUseUtf8.TabIndex = 2;
            this.chkLoadBalanceInfoUseUtf8.Text = "Use UTF8 encoding for RDP \"Load Balance Info\" property";
            this.chkLoadBalanceInfoUseUtf8.UseVisualStyleBackColor = true;
            // 
            // AdvancedPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.chkNoReconnect);
            this.Controls.Add(this.chkLoadBalanceInfoUseUtf8);
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
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "AdvancedPage";
            this.Size = new System.Drawing.Size(915, 735);
            ((System.ComponentModel.ISupportInitialize)(this.numPuttyWaitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUVNCSCPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal Controls.MrngLabel lblMaximumPuttyWaitTime;
		internal MrngCheckBox chkAutomaticReconnect;
		internal Controls.MrngNumericUpDown numPuttyWaitTime;
		internal MrngCheckBox chkUseCustomPuttyPath;
		internal Controls.MrngLabel lblConfigurePuttySessions;
		internal Controls.MrngNumericUpDown numUVNCSCPort;
		internal Controls.MrngTextBox txtCustomPuttyPath;
		internal MrngButton btnLaunchPutty;
		internal Controls.MrngLabel lblUVNCSCPort;
		internal Controls.MrngLabel lblSeconds;
		internal MrngButton btnBrowseCustomPuttyPath;
        private MrngCheckBox chkLoadBalanceInfoUseUtf8;
        internal MrngCheckBox chkNoReconnect;
    }
}
