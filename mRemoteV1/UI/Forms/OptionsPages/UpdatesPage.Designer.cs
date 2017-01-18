
namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public partial class UpdatesPage : OptionsPage
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
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdatesPage));
            this.lblUpdatesExplanation = new System.Windows.Forms.Label();
            this.pnlUpdateCheck = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblReleaseChannel = new System.Windows.Forms.Label();
            this.cboReleaseChannel = new System.Windows.Forms.ComboBox();
            this.btnUpdateCheckNow = new System.Windows.Forms.Button();
            this.chkCheckForUpdatesOnStartup = new System.Windows.Forms.CheckBox();
            this.cboUpdateCheckFrequency = new System.Windows.Forms.ComboBox();
            this.pnlProxy = new System.Windows.Forms.Panel();
            this.pnlProxyBasic = new System.Windows.Forms.Panel();
            this.lblProxyAddress = new System.Windows.Forms.Label();
            this.txtProxyAddress = new System.Windows.Forms.TextBox();
            this.lblProxyPort = new System.Windows.Forms.Label();
            this.numProxyPort = new System.Windows.Forms.NumericUpDown();
            this.chkUseProxyForAutomaticUpdates = new System.Windows.Forms.CheckBox();
            this.chkUseProxyAuthentication = new System.Windows.Forms.CheckBox();
            this.pnlProxyAuthentication = new System.Windows.Forms.Panel();
            this.lblProxyUsername = new System.Windows.Forms.Label();
            this.txtProxyUsername = new System.Windows.Forms.TextBox();
            this.lblProxyPassword = new System.Windows.Forms.Label();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.btnTestProxy = new System.Windows.Forms.Button();
            this.pnlUpdateCheck.SuspendLayout();
            this.pnlProxy.SuspendLayout();
            this.pnlProxyBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).BeginInit();
            this.pnlProxyAuthentication.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUpdatesExplanation
            // 
            this.lblUpdatesExplanation.Location = new System.Drawing.Point(3, 0);
            this.lblUpdatesExplanation.Name = "lblUpdatesExplanation";
            this.lblUpdatesExplanation.Size = new System.Drawing.Size(536, 40);
            this.lblUpdatesExplanation.TabIndex = 3;
            this.lblUpdatesExplanation.Text = "mRemoteNG can periodically connect to the mRemoteNG website to check for updates." +
    "";
            // 
            // pnlUpdateCheck
            // 
            this.pnlUpdateCheck.Controls.Add(this.textBox1);
            this.pnlUpdateCheck.Controls.Add(this.lblReleaseChannel);
            this.pnlUpdateCheck.Controls.Add(this.cboReleaseChannel);
            this.pnlUpdateCheck.Controls.Add(this.btnUpdateCheckNow);
            this.pnlUpdateCheck.Controls.Add(this.chkCheckForUpdatesOnStartup);
            this.pnlUpdateCheck.Controls.Add(this.cboUpdateCheckFrequency);
            this.pnlUpdateCheck.Location = new System.Drawing.Point(0, 48);
            this.pnlUpdateCheck.Name = "pnlUpdateCheck";
            this.pnlUpdateCheck.Size = new System.Drawing.Size(610, 120);
            this.pnlUpdateCheck.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(226, 68);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(366, 44);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "Stable channel includes final releases only.\r\nBeta channel includes Betas & Relea" +
    "se Candidates.\r\nDevelopment Channel includes Alphas, Betas & Release Candidates." +
    "";
            // 
            // lblReleaseChannel
            // 
            this.lblReleaseChannel.AutoSize = true;
            this.lblReleaseChannel.Location = new System.Drawing.Point(223, 12);
            this.lblReleaseChannel.Name = "lblReleaseChannel";
            this.lblReleaseChannel.Size = new System.Drawing.Size(91, 13);
            this.lblReleaseChannel.TabIndex = 4;
            this.lblReleaseChannel.Text = "Release Channel:";
            // 
            // cboReleaseChannel
            // 
            this.cboReleaseChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReleaseChannel.FormattingEnabled = true;
            this.cboReleaseChannel.Location = new System.Drawing.Point(226, 41);
            this.cboReleaseChannel.Name = "cboReleaseChannel";
            this.cboReleaseChannel.Size = new System.Drawing.Size(121, 21);
            this.cboReleaseChannel.TabIndex = 3;
            // 
            // btnUpdateCheckNow
            // 
            this.btnUpdateCheckNow.Location = new System.Drawing.Point(3, 80);
            this.btnUpdateCheckNow.Name = "btnUpdateCheckNow";
            this.btnUpdateCheckNow.Size = new System.Drawing.Size(120, 32);
            this.btnUpdateCheckNow.TabIndex = 2;
            this.btnUpdateCheckNow.Text = "Check Now";
            this.btnUpdateCheckNow.UseVisualStyleBackColor = true;
            this.btnUpdateCheckNow.Click += new System.EventHandler(this.btnUpdateCheckNow_Click);
            // 
            // chkCheckForUpdatesOnStartup
            // 
            this.chkCheckForUpdatesOnStartup.AutoSize = true;
            this.chkCheckForUpdatesOnStartup.Location = new System.Drawing.Point(3, 8);
            this.chkCheckForUpdatesOnStartup.Name = "chkCheckForUpdatesOnStartup";
            this.chkCheckForUpdatesOnStartup.Size = new System.Drawing.Size(113, 17);
            this.chkCheckForUpdatesOnStartup.TabIndex = 0;
            this.chkCheckForUpdatesOnStartup.Text = "Check for updates";
            this.chkCheckForUpdatesOnStartup.UseVisualStyleBackColor = true;
            this.chkCheckForUpdatesOnStartup.CheckedChanged += new System.EventHandler(this.chkCheckForUpdatesOnStartup_CheckedChanged);
            // 
            // cboUpdateCheckFrequency
            // 
            this.cboUpdateCheckFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUpdateCheckFrequency.FormattingEnabled = true;
            this.cboUpdateCheckFrequency.Location = new System.Drawing.Point(3, 41);
            this.cboUpdateCheckFrequency.Name = "cboUpdateCheckFrequency";
            this.cboUpdateCheckFrequency.Size = new System.Drawing.Size(128, 21);
            this.cboUpdateCheckFrequency.TabIndex = 1;
            // 
            // pnlProxy
            // 
            this.pnlProxy.Controls.Add(this.pnlProxyBasic);
            this.pnlProxy.Controls.Add(this.chkUseProxyForAutomaticUpdates);
            this.pnlProxy.Controls.Add(this.chkUseProxyAuthentication);
            this.pnlProxy.Controls.Add(this.pnlProxyAuthentication);
            this.pnlProxy.Controls.Add(this.btnTestProxy);
            this.pnlProxy.Location = new System.Drawing.Point(0, 200);
            this.pnlProxy.Name = "pnlProxy";
            this.pnlProxy.Size = new System.Drawing.Size(610, 224);
            this.pnlProxy.TabIndex = 5;
            // 
            // pnlProxyBasic
            // 
            this.pnlProxyBasic.Controls.Add(this.lblProxyAddress);
            this.pnlProxyBasic.Controls.Add(this.txtProxyAddress);
            this.pnlProxyBasic.Controls.Add(this.lblProxyPort);
            this.pnlProxyBasic.Controls.Add(this.numProxyPort);
            this.pnlProxyBasic.Enabled = false;
            this.pnlProxyBasic.Location = new System.Drawing.Point(3, 32);
            this.pnlProxyBasic.Name = "pnlProxyBasic";
            this.pnlProxyBasic.Size = new System.Drawing.Size(604, 40);
            this.pnlProxyBasic.TabIndex = 1;
            // 
            // lblProxyAddress
            // 
            this.lblProxyAddress.Location = new System.Drawing.Point(8, 4);
            this.lblProxyAddress.Name = "lblProxyAddress";
            this.lblProxyAddress.Size = new System.Drawing.Size(96, 24);
            this.lblProxyAddress.TabIndex = 0;
            this.lblProxyAddress.Text = "Address:";
            this.lblProxyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyAddress
            // 
            this.txtProxyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyAddress.Location = new System.Drawing.Point(104, 8);
            this.txtProxyAddress.Name = "txtProxyAddress";
            this.txtProxyAddress.Size = new System.Drawing.Size(240, 20);
            this.txtProxyAddress.TabIndex = 1;
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.Location = new System.Drawing.Point(350, 5);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(64, 23);
            this.lblProxyPort.TabIndex = 2;
            this.lblProxyPort.Text = "Port:";
            this.lblProxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numProxyPort
            // 
            this.numProxyPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numProxyPort.Location = new System.Drawing.Point(420, 8);
            this.numProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numProxyPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numProxyPort.Name = "numProxyPort";
            this.numProxyPort.Size = new System.Drawing.Size(64, 20);
            this.numProxyPort.TabIndex = 3;
            this.numProxyPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // chkUseProxyForAutomaticUpdates
            // 
            this.chkUseProxyForAutomaticUpdates.AutoSize = true;
            this.chkUseProxyForAutomaticUpdates.Location = new System.Drawing.Point(3, 8);
            this.chkUseProxyForAutomaticUpdates.Name = "chkUseProxyForAutomaticUpdates";
            this.chkUseProxyForAutomaticUpdates.Size = new System.Drawing.Size(168, 17);
            this.chkUseProxyForAutomaticUpdates.TabIndex = 0;
            this.chkUseProxyForAutomaticUpdates.Text = "Use a proxy server to connect";
            this.chkUseProxyForAutomaticUpdates.UseVisualStyleBackColor = true;
            this.chkUseProxyForAutomaticUpdates.CheckedChanged += new System.EventHandler(this.chkUseProxyForAutomaticUpdates_CheckedChanged);
            // 
            // chkUseProxyAuthentication
            // 
            this.chkUseProxyAuthentication.AutoSize = true;
            this.chkUseProxyAuthentication.Enabled = false;
            this.chkUseProxyAuthentication.Location = new System.Drawing.Point(27, 80);
            this.chkUseProxyAuthentication.Name = "chkUseProxyAuthentication";
            this.chkUseProxyAuthentication.Size = new System.Drawing.Size(216, 17);
            this.chkUseProxyAuthentication.TabIndex = 2;
            this.chkUseProxyAuthentication.Text = "This proxy server requires authentication";
            this.chkUseProxyAuthentication.UseVisualStyleBackColor = true;
            this.chkUseProxyAuthentication.CheckedChanged += new System.EventHandler(this.chkUseProxyAuthentication_CheckedChanged);
            // 
            // pnlProxyAuthentication
            // 
            this.pnlProxyAuthentication.Controls.Add(this.lblProxyUsername);
            this.pnlProxyAuthentication.Controls.Add(this.txtProxyUsername);
            this.pnlProxyAuthentication.Controls.Add(this.lblProxyPassword);
            this.pnlProxyAuthentication.Controls.Add(this.txtProxyPassword);
            this.pnlProxyAuthentication.Enabled = false;
            this.pnlProxyAuthentication.Location = new System.Drawing.Point(3, 104);
            this.pnlProxyAuthentication.Name = "pnlProxyAuthentication";
            this.pnlProxyAuthentication.Size = new System.Drawing.Size(604, 72);
            this.pnlProxyAuthentication.TabIndex = 3;
            // 
            // lblProxyUsername
            // 
            this.lblProxyUsername.Location = new System.Drawing.Point(8, 4);
            this.lblProxyUsername.Name = "lblProxyUsername";
            this.lblProxyUsername.Size = new System.Drawing.Size(96, 24);
            this.lblProxyUsername.TabIndex = 0;
            this.lblProxyUsername.Text = "Username:";
            this.lblProxyUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyUsername
            // 
            this.txtProxyUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyUsername.Location = new System.Drawing.Point(104, 8);
            this.txtProxyUsername.Name = "txtProxyUsername";
            this.txtProxyUsername.Size = new System.Drawing.Size(240, 20);
            this.txtProxyUsername.TabIndex = 1;
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.Location = new System.Drawing.Point(8, 36);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(96, 24);
            this.lblProxyPassword.TabIndex = 2;
            this.lblProxyPassword.Text = "Password:";
            this.lblProxyPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyPassword.Location = new System.Drawing.Point(104, 40);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(240, 20);
            this.txtProxyPassword.TabIndex = 3;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // btnTestProxy
            // 
            this.btnTestProxy.Location = new System.Drawing.Point(3, 184);
            this.btnTestProxy.Name = "btnTestProxy";
            this.btnTestProxy.Size = new System.Drawing.Size(120, 32);
            this.btnTestProxy.TabIndex = 4;
            this.btnTestProxy.Text = "Test Proxy";
            this.btnTestProxy.UseVisualStyleBackColor = true;
            this.btnTestProxy.Click += new System.EventHandler(this.btnTestProxy_Click);
            // 
            // UpdatesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblUpdatesExplanation);
            this.Controls.Add(this.pnlUpdateCheck);
            this.Controls.Add(this.pnlProxy);
            this.Name = "UpdatesPage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            this.pnlUpdateCheck.ResumeLayout(false);
            this.pnlUpdateCheck.PerformLayout();
            this.pnlProxy.ResumeLayout(false);
            this.pnlProxy.PerformLayout();
            this.pnlProxyBasic.ResumeLayout(false);
            this.pnlProxyBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).EndInit();
            this.pnlProxyAuthentication.ResumeLayout(false);
            this.pnlProxyAuthentication.PerformLayout();
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.Label lblUpdatesExplanation;
		internal System.Windows.Forms.Panel pnlUpdateCheck;
		internal System.Windows.Forms.Button btnUpdateCheckNow;
		internal System.Windows.Forms.CheckBox chkCheckForUpdatesOnStartup;
		internal System.Windows.Forms.ComboBox cboUpdateCheckFrequency;
		internal System.Windows.Forms.Panel pnlProxy;
		internal System.Windows.Forms.Panel pnlProxyBasic;
		internal System.Windows.Forms.Label lblProxyAddress;
		internal System.Windows.Forms.TextBox txtProxyAddress;
		internal System.Windows.Forms.Label lblProxyPort;
		internal System.Windows.Forms.NumericUpDown numProxyPort;
		internal System.Windows.Forms.CheckBox chkUseProxyForAutomaticUpdates;
		internal System.Windows.Forms.CheckBox chkUseProxyAuthentication;
		internal System.Windows.Forms.Panel pnlProxyAuthentication;
		internal System.Windows.Forms.Label lblProxyUsername;
		internal System.Windows.Forms.TextBox txtProxyUsername;
		internal System.Windows.Forms.Label lblProxyPassword;
		internal System.Windows.Forms.TextBox txtProxyPassword;
		internal System.Windows.Forms.Button btnTestProxy;
        private System.Windows.Forms.Label lblReleaseChannel;
        private System.Windows.Forms.ComboBox cboReleaseChannel;
        private System.Windows.Forms.TextBox textBox1;
    }
}