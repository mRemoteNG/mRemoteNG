

namespace mRemoteNG.UI.Forms.OptionsPages
{
    public partial class AdvancedPage : OptionsPage
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
            this.chkWriteLogFile = new System.Windows.Forms.CheckBox();
            this.chkAutomaticallyGetSessionInfo = new System.Windows.Forms.CheckBox();
            this.lblXulRunnerPath = new System.Windows.Forms.Label();
            this.lblMaximumPuttyWaitTime = new System.Windows.Forms.Label();
            this.chkEncryptCompleteFile = new System.Windows.Forms.CheckBox();
            this.chkAutomaticReconnect = new System.Windows.Forms.CheckBox();
            this.btnBrowseXulRunnerPath = new System.Windows.Forms.Button();
            this.numPuttyWaitTime = new System.Windows.Forms.NumericUpDown();
            this.chkUseCustomPuttyPath = new System.Windows.Forms.CheckBox();
            this.lblConfigurePuttySessions = new System.Windows.Forms.Label();
            this.txtXULrunnerPath = new System.Windows.Forms.TextBox();
            this.numUVNCSCPort = new System.Windows.Forms.NumericUpDown();
            this.txtCustomPuttyPath = new System.Windows.Forms.TextBox();
            this.btnLaunchPutty = new System.Windows.Forms.Button();
            this.lblUVNCSCPort = new System.Windows.Forms.Label();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.btnBrowseCustomPuttyPath = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPuttyWaitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUVNCSCPort)).BeginInit();
            this.SuspendLayout();
            // 
            // chkWriteLogFile
            // 
            this.chkWriteLogFile.AutoSize = true;
            this.chkWriteLogFile.Location = new System.Drawing.Point(3, 0);
            this.chkWriteLogFile.Name = "chkWriteLogFile";
            this.chkWriteLogFile.Size = new System.Drawing.Size(171, 17);
            this.chkWriteLogFile.TabIndex = 17;
            this.chkWriteLogFile.Text = "Write log file (mRemoteNG.log)";
            this.chkWriteLogFile.UseVisualStyleBackColor = true;
            // 
            // chkAutomaticallyGetSessionInfo
            // 
            this.chkAutomaticallyGetSessionInfo.AutoSize = true;
            this.chkAutomaticallyGetSessionInfo.Location = new System.Drawing.Point(3, 46);
            this.chkAutomaticallyGetSessionInfo.Name = "chkAutomaticallyGetSessionInfo";
            this.chkAutomaticallyGetSessionInfo.Size = new System.Drawing.Size(198, 17);
            this.chkAutomaticallyGetSessionInfo.TabIndex = 19;
            this.chkAutomaticallyGetSessionInfo.Text = "Automatically get session information";
            this.chkAutomaticallyGetSessionInfo.UseVisualStyleBackColor = true;
            // 
            // lblXulRunnerPath
            // 
            this.lblXulRunnerPath.AutoSize = true;
            this.lblXulRunnerPath.Location = new System.Drawing.Point(3, 240);
            this.lblXulRunnerPath.Name = "lblXulRunnerPath";
            this.lblXulRunnerPath.Size = new System.Drawing.Size(90, 13);
            this.lblXulRunnerPath.TabIndex = 29;
            this.lblXulRunnerPath.Text = "XULRunner path:";
            // 
            // lblMaximumPuttyWaitTime
            // 
            this.lblMaximumPuttyWaitTime.Location = new System.Drawing.Point(3, 195);
            this.lblMaximumPuttyWaitTime.Name = "lblMaximumPuttyWaitTime";
            this.lblMaximumPuttyWaitTime.Size = new System.Drawing.Size(364, 13);
            this.lblMaximumPuttyWaitTime.TabIndex = 26;
            this.lblMaximumPuttyWaitTime.Text = "Maximum PuTTY wait time:";
            this.lblMaximumPuttyWaitTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkEncryptCompleteFile
            // 
            this.chkEncryptCompleteFile.AutoSize = true;
            this.chkEncryptCompleteFile.Location = new System.Drawing.Point(3, 23);
            this.chkEncryptCompleteFile.Name = "chkEncryptCompleteFile";
            this.chkEncryptCompleteFile.Size = new System.Drawing.Size(180, 17);
            this.chkEncryptCompleteFile.TabIndex = 18;
            this.chkEncryptCompleteFile.Text = "Encrypt complete connection file";
            this.chkEncryptCompleteFile.UseVisualStyleBackColor = true;
            // 
            // chkAutomaticReconnect
            // 
            this.chkAutomaticReconnect.AutoSize = true;
            this.chkAutomaticReconnect.Location = new System.Drawing.Point(3, 69);
            this.chkAutomaticReconnect.Name = "chkAutomaticReconnect";
            this.chkAutomaticReconnect.Size = new System.Drawing.Size(399, 17);
            this.chkAutomaticReconnect.TabIndex = 20;
            this.chkAutomaticReconnect.Text = "Automatically try to reconnect when disconnected from server (RDP && ICA only)";
            this.chkAutomaticReconnect.UseVisualStyleBackColor = true;
            // 
            // btnBrowseXulRunnerPath
            // 
            this.btnBrowseXulRunnerPath.Location = new System.Drawing.Point(373, 262);
            this.btnBrowseXulRunnerPath.Name = "btnBrowseXulRunnerPath";
            this.btnBrowseXulRunnerPath.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseXulRunnerPath.TabIndex = 31;
            this.btnBrowseXulRunnerPath.Text = "Browse...";
            this.btnBrowseXulRunnerPath.UseVisualStyleBackColor = true;
            this.btnBrowseXulRunnerPath.Click += new System.EventHandler(this.btnBrowseXulRunnerPath_Click);
            // 
            // numPuttyWaitTime
            // 
            this.numPuttyWaitTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPuttyWaitTime.Location = new System.Drawing.Point(373, 193);
            this.numPuttyWaitTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numPuttyWaitTime.Name = "numPuttyWaitTime";
            this.numPuttyWaitTime.Size = new System.Drawing.Size(49, 20);
            this.numPuttyWaitTime.TabIndex = 27;
            this.numPuttyWaitTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chkUseCustomPuttyPath
            // 
            this.chkUseCustomPuttyPath.AutoSize = true;
            this.chkUseCustomPuttyPath.Location = new System.Drawing.Point(3, 92);
            this.chkUseCustomPuttyPath.Name = "chkUseCustomPuttyPath";
            this.chkUseCustomPuttyPath.Size = new System.Drawing.Size(146, 17);
            this.chkUseCustomPuttyPath.TabIndex = 21;
            this.chkUseCustomPuttyPath.Text = "Use custom PuTTY path:";
            this.chkUseCustomPuttyPath.UseVisualStyleBackColor = true;
            this.chkUseCustomPuttyPath.CheckedChanged += new System.EventHandler(this.chkUseCustomPuttyPath_CheckedChanged);
            // 
            // lblConfigurePuttySessions
            // 
            this.lblConfigurePuttySessions.Location = new System.Drawing.Point(3, 164);
            this.lblConfigurePuttySessions.Name = "lblConfigurePuttySessions";
            this.lblConfigurePuttySessions.Size = new System.Drawing.Size(364, 13);
            this.lblConfigurePuttySessions.TabIndex = 24;
            this.lblConfigurePuttySessions.Text = "To configure PuTTY sessions click this button:";
            this.lblConfigurePuttySessions.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtXULrunnerPath
            // 
            this.txtXULrunnerPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXULrunnerPath.Location = new System.Drawing.Point(21, 264);
            this.txtXULrunnerPath.Name = "txtXULrunnerPath";
            this.txtXULrunnerPath.Size = new System.Drawing.Size(346, 20);
            this.txtXULrunnerPath.TabIndex = 30;
            // 
            // numUVNCSCPort
            // 
            this.numUVNCSCPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numUVNCSCPort.Location = new System.Drawing.Point(373, 305);
            this.numUVNCSCPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUVNCSCPort.Name = "numUVNCSCPort";
            this.numUVNCSCPort.Size = new System.Drawing.Size(72, 20);
            this.numUVNCSCPort.TabIndex = 33;
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
            this.txtCustomPuttyPath.Location = new System.Drawing.Point(21, 115);
            this.txtCustomPuttyPath.Name = "txtCustomPuttyPath";
            this.txtCustomPuttyPath.Size = new System.Drawing.Size(346, 20);
            this.txtCustomPuttyPath.TabIndex = 22;
            this.txtCustomPuttyPath.TextChanged += new System.EventHandler(this.txtCustomPuttyPath_TextChanged);
            // 
            // btnLaunchPutty
            // 
            this.btnLaunchPutty.Image = global::mRemoteNG.My.Resources.PuttyConfig;
            this.btnLaunchPutty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLaunchPutty.Location = new System.Drawing.Point(373, 159);
            this.btnLaunchPutty.Name = "btnLaunchPutty";
            this.btnLaunchPutty.Size = new System.Drawing.Size(110, 23);
            this.btnLaunchPutty.TabIndex = 25;
            this.btnLaunchPutty.Text = "Launch PuTTY";
            this.btnLaunchPutty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLaunchPutty.UseVisualStyleBackColor = true;
            this.btnLaunchPutty.Click += new System.EventHandler(this.btnLaunchPutty_Click);
            // 
            // lblUVNCSCPort
            // 
            this.lblUVNCSCPort.Location = new System.Drawing.Point(3, 307);
            this.lblUVNCSCPort.Name = "lblUVNCSCPort";
            this.lblUVNCSCPort.Size = new System.Drawing.Size(364, 13);
            this.lblUVNCSCPort.TabIndex = 32;
            this.lblUVNCSCPort.Text = "UltraVNC SingleClick Listening Port:";
            this.lblUVNCSCPort.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblUVNCSCPort.Visible = false;
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(428, 195);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(47, 13);
            this.lblSeconds.TabIndex = 28;
            this.lblSeconds.Text = "seconds";
            // 
            // btnBrowseCustomPuttyPath
            // 
            this.btnBrowseCustomPuttyPath.Enabled = false;
            this.btnBrowseCustomPuttyPath.Location = new System.Drawing.Point(373, 113);
            this.btnBrowseCustomPuttyPath.Name = "btnBrowseCustomPuttyPath";
            this.btnBrowseCustomPuttyPath.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseCustomPuttyPath.TabIndex = 23;
            this.btnBrowseCustomPuttyPath.Text = "Browse...";
            this.btnBrowseCustomPuttyPath.UseVisualStyleBackColor = true;
            this.btnBrowseCustomPuttyPath.Click += new System.EventHandler(this.btnBrowseCustomPuttyPath_Click);
            // 
            // AdvancedPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkWriteLogFile);
            this.Controls.Add(this.chkAutomaticallyGetSessionInfo);
            this.Controls.Add(this.lblXulRunnerPath);
            this.Controls.Add(this.lblMaximumPuttyWaitTime);
            this.Controls.Add(this.chkEncryptCompleteFile);
            this.Controls.Add(this.chkAutomaticReconnect);
            this.Controls.Add(this.btnBrowseXulRunnerPath);
            this.Controls.Add(this.numPuttyWaitTime);
            this.Controls.Add(this.chkUseCustomPuttyPath);
            this.Controls.Add(this.lblConfigurePuttySessions);
            this.Controls.Add(this.txtXULrunnerPath);
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
		internal System.Windows.Forms.CheckBox chkWriteLogFile;
		internal System.Windows.Forms.CheckBox chkAutomaticallyGetSessionInfo;
		internal System.Windows.Forms.Label lblXulRunnerPath;
		internal System.Windows.Forms.Label lblMaximumPuttyWaitTime;
		internal System.Windows.Forms.CheckBox chkEncryptCompleteFile;
		internal System.Windows.Forms.CheckBox chkAutomaticReconnect;
		internal System.Windows.Forms.Button btnBrowseXulRunnerPath;
		internal System.Windows.Forms.NumericUpDown numPuttyWaitTime;
		internal System.Windows.Forms.CheckBox chkUseCustomPuttyPath;
		internal System.Windows.Forms.Label lblConfigurePuttySessions;
		internal System.Windows.Forms.TextBox txtXULrunnerPath;
		internal System.Windows.Forms.NumericUpDown numUVNCSCPort;
		internal System.Windows.Forms.TextBox txtCustomPuttyPath;
		internal System.Windows.Forms.Button btnLaunchPutty;
		internal System.Windows.Forms.Label lblUVNCSCPort;
		internal System.Windows.Forms.Label lblSeconds;
		internal System.Windows.Forms.Button btnBrowseCustomPuttyPath;
			
	}
}
