

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
            this.chkUseCustomWinboxPath = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.txtCustomWinboxPath = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.btnBrowseCustomWinboxPath = new mRemoteNG.UI.Controls.Base.NGButton();
            this.btnLaunchWinbox = new mRemoteNG.UI.Controls.Base.NGButton();
            ((System.ComponentModel.ISupportInitialize)(this.numPuttyWaitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUVNCSCPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMaximumPuttyWaitTime
            // 
            this.lblMaximumPuttyWaitTime.Location = new System.Drawing.Point(3, 145);
            this.lblMaximumPuttyWaitTime.Name = "lblMaximumPuttyWaitTime";
            this.lblMaximumPuttyWaitTime.Size = new System.Drawing.Size(364, 20);
            this.lblMaximumPuttyWaitTime.TabIndex = 7;
            this.lblMaximumPuttyWaitTime.Text = "Maximum PuTTY wait time:";
            this.lblMaximumPuttyWaitTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkAutomaticReconnect
            // 
            this.chkAutomaticReconnect._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkAutomaticReconnect.AutoSize = true;
            this.chkAutomaticReconnect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutomaticReconnect.Location = new System.Drawing.Point(3, 3);
            this.chkAutomaticReconnect.Name = "chkAutomaticReconnect";
            this.chkAutomaticReconnect.Size = new System.Drawing.Size(430, 17);
            this.chkAutomaticReconnect.TabIndex = 1;
            this.chkAutomaticReconnect.Text = "Automatically try to reconnect when disconnected from server (RDP && ICA only)";
            this.chkAutomaticReconnect.UseVisualStyleBackColor = true;
            // 
            // numPuttyWaitTime
            // 
            this.numPuttyWaitTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPuttyWaitTime.Location = new System.Drawing.Point(373, 143);
            this.numPuttyWaitTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numPuttyWaitTime.Name = "numPuttyWaitTime";
            this.numPuttyWaitTime.Size = new System.Drawing.Size(60, 22);
            this.numPuttyWaitTime.TabIndex = 7;
            this.numPuttyWaitTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chkUseCustomPuttyPath
            // 
            this.chkUseCustomPuttyPath._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkUseCustomPuttyPath.AutoSize = true;
            this.chkUseCustomPuttyPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseCustomPuttyPath.Location = new System.Drawing.Point(3, 49);
            this.chkUseCustomPuttyPath.Name = "chkUseCustomPuttyPath";
            this.chkUseCustomPuttyPath.Size = new System.Drawing.Size(148, 17);
            this.chkUseCustomPuttyPath.TabIndex = 3;
            this.chkUseCustomPuttyPath.Text = "Use custom PuTTY path:";
            this.chkUseCustomPuttyPath.UseVisualStyleBackColor = true;
            this.chkUseCustomPuttyPath.CheckedChanged += new System.EventHandler(this.chkUseCustomPuttyPath_CheckedChanged);
            // 
            // lblConfigurePuttySessions
            // 
            this.lblConfigurePuttySessions.Location = new System.Drawing.Point(3, 109);
            this.lblConfigurePuttySessions.Name = "lblConfigurePuttySessions";
            this.lblConfigurePuttySessions.Size = new System.Drawing.Size(364, 25);
            this.lblConfigurePuttySessions.TabIndex = 5;
            this.lblConfigurePuttySessions.Text = "To configure PuTTY sessions click this button:";
            this.lblConfigurePuttySessions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numUVNCSCPort
            // 
            this.numUVNCSCPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUVNCSCPort.Location = new System.Drawing.Point(373, 171);
            this.numUVNCSCPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUVNCSCPort.Name = "numUVNCSCPort";
            this.numUVNCSCPort.Size = new System.Drawing.Size(60, 22);
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
            this.txtCustomPuttyPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomPuttyPath.Location = new System.Drawing.Point(21, 72);
            this.txtCustomPuttyPath.Name = "txtCustomPuttyPath";
            this.txtCustomPuttyPath.Size = new System.Drawing.Size(346, 22);
            this.txtCustomPuttyPath.TabIndex = 4;
            this.txtCustomPuttyPath.TextChanged += new System.EventHandler(this.txtCustomPuttyPath_TextChanged);
            // 
            // btnLaunchPutty
            // 
            this.btnLaunchPutty._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.OUT;
            this.btnLaunchPutty.Image = global::mRemoteNG.Resources.PuttyConfig;
            this.btnLaunchPutty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLaunchPutty.Location = new System.Drawing.Point(373, 109);
            this.btnLaunchPutty.Name = "btnLaunchPutty";
            this.btnLaunchPutty.Size = new System.Drawing.Size(122, 25);
            this.btnLaunchPutty.TabIndex = 6;
            this.btnLaunchPutty.Text = "Launch PuTTY";
            this.btnLaunchPutty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLaunchPutty.UseVisualStyleBackColor = true;
            this.btnLaunchPutty.Click += new System.EventHandler(this.btnLaunchPutty_Click);
            // 
            // lblUVNCSCPort
            // 
            this.lblUVNCSCPort.Location = new System.Drawing.Point(3, 174);
            this.lblUVNCSCPort.Name = "lblUVNCSCPort";
            this.lblUVNCSCPort.Size = new System.Drawing.Size(364, 19);
            this.lblUVNCSCPort.TabIndex = 10;
            this.lblUVNCSCPort.Text = "UltraVNC SingleClick Listening Port:";
            this.lblUVNCSCPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUVNCSCPort.Visible = false;
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(439, 149);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(49, 13);
            this.lblSeconds.TabIndex = 9;
            this.lblSeconds.Text = "seconds";
            // 
            // btnBrowseCustomPuttyPath
            // 
            this.btnBrowseCustomPuttyPath._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.OUT;
            this.btnBrowseCustomPuttyPath.Enabled = false;
            this.btnBrowseCustomPuttyPath.Location = new System.Drawing.Point(373, 71);
            this.btnBrowseCustomPuttyPath.Name = "btnBrowseCustomPuttyPath";
            this.btnBrowseCustomPuttyPath.Size = new System.Drawing.Size(122, 25);
            this.btnBrowseCustomPuttyPath.TabIndex = 5;
            this.btnBrowseCustomPuttyPath.Text = "Browse...";
            this.btnBrowseCustomPuttyPath.UseVisualStyleBackColor = true;
            this.btnBrowseCustomPuttyPath.Click += new System.EventHandler(this.btnBrowseCustomPuttyPath_Click);
            // 
            // chkLoadBalanceInfoUseUtf8
            // 
            this.chkLoadBalanceInfoUseUtf8._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkLoadBalanceInfoUseUtf8.AutoSize = true;
            this.chkLoadBalanceInfoUseUtf8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLoadBalanceInfoUseUtf8.Location = new System.Drawing.Point(3, 26);
            this.chkLoadBalanceInfoUseUtf8.Name = "chkLoadBalanceInfoUseUtf8";
            this.chkLoadBalanceInfoUseUtf8.Size = new System.Drawing.Size(317, 17);
            this.chkLoadBalanceInfoUseUtf8.TabIndex = 2;
            this.chkLoadBalanceInfoUseUtf8.Text = "Use UTF8 encoding for RDP \"Load Balance Info\" property";
            this.chkLoadBalanceInfoUseUtf8.UseVisualStyleBackColor = true;
            // 
            // chkUseCustomWinboxPath
            // 
            this.chkUseCustomWinboxPath._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkUseCustomWinboxPath.AutoSize = true;
            this.chkUseCustomWinboxPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseCustomWinboxPath.Location = new System.Drawing.Point(5, 217);
            this.chkUseCustomWinboxPath.Name = "chkUseCustomWinboxPath";
            this.chkUseCustomWinboxPath.Size = new System.Drawing.Size(158, 17);
            this.chkUseCustomWinboxPath.TabIndex = 11;
            this.chkUseCustomWinboxPath.Text = "Use custom Winbox path:";
            this.chkUseCustomWinboxPath.UseVisualStyleBackColor = true;
            this.chkUseCustomWinboxPath.CheckedChanged += new System.EventHandler(this.chkUseCustomWinboxPath_CheckedChanged);
            // 
            // txtCustomWinboxPath
            // 
            this.txtCustomWinboxPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomWinboxPath.Enabled = false;
            this.txtCustomWinboxPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomWinboxPath.Location = new System.Drawing.Point(21, 241);
            this.txtCustomWinboxPath.Name = "txtCustomWinboxPath";
            this.txtCustomWinboxPath.Size = new System.Drawing.Size(346, 22);
            this.txtCustomWinboxPath.TabIndex = 12;
            this.txtCustomWinboxPath.TextChanged += new System.EventHandler(this.txtCustomWinboxPath_TextChanged);
            // 
            // btnBrowseCustomWinboxPath
            // 
            this.btnBrowseCustomWinboxPath._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.OUT;
            this.btnBrowseCustomWinboxPath.Enabled = false;
            this.btnBrowseCustomWinboxPath.Location = new System.Drawing.Point(373, 237);
            this.btnBrowseCustomWinboxPath.Name = "btnBrowseCustomWinboxPath";
            this.btnBrowseCustomWinboxPath.Size = new System.Drawing.Size(122, 25);
            this.btnBrowseCustomWinboxPath.TabIndex = 13;
            this.btnBrowseCustomWinboxPath.Text = "Browse...";
            this.btnBrowseCustomWinboxPath.UseVisualStyleBackColor = true;
            this.btnBrowseCustomWinboxPath.Click += new System.EventHandler(this.btnBrowseCustomWinboxPath_Click);
            // 
            // btnLaunchWinbox
            // 
            this.btnLaunchWinbox._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.OUT;
            this.btnLaunchWinbox.Image = global::mRemoteNG.Resources.winbox_icon;
            this.btnLaunchWinbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLaunchWinbox.Location = new System.Drawing.Point(373, 276);
            this.btnLaunchWinbox.Name = "btnLaunchWinbox";
            this.btnLaunchWinbox.Size = new System.Drawing.Size(122, 25);
            this.btnLaunchWinbox.TabIndex = 14;
            this.btnLaunchWinbox.Text = "Test Winbox";
            this.btnLaunchWinbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLaunchWinbox.UseVisualStyleBackColor = true;
            this.btnLaunchWinbox.Click += new System.EventHandler(this.btnTestWinbox_Click);
            // 
            // AdvancedPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.btnLaunchWinbox);
            this.Controls.Add(this.txtCustomWinboxPath);
            this.Controls.Add(this.chkUseCustomWinboxPath);
            this.Controls.Add(this.btnBrowseCustomWinboxPath);
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
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AdvancedPage";
            this.Size = new System.Drawing.Size(610, 490);
            ((System.ComponentModel.ISupportInitialize)(this.numPuttyWaitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUVNCSCPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
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
        internal Controls.Base.NGCheckBox chkUseCustomWinboxPath;
        internal Controls.Base.NGTextBox txtCustomWinboxPath;
        internal Controls.Base.NGButton btnBrowseCustomWinboxPath;
        internal Controls.Base.NGButton btnLaunchWinbox;
    }
}
