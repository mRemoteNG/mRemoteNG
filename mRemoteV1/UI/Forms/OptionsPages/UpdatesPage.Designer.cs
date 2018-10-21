
namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public sealed partial class UpdatesPage : OptionsPage
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
            this.lblUpdatesExplanation = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pnlUpdateCheck = new System.Windows.Forms.Panel();
            this.btnUpdateCheckNow = new mRemoteNG.UI.Controls.Base.NGButton();
            this.chkCheckForUpdatesOnStartup = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.cboUpdateCheckFrequency = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.textBox1 = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblReleaseChannel = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.cboReleaseChannel = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.pnlProxy = new System.Windows.Forms.Panel();
            this.pnlProxyBasic = new System.Windows.Forms.Panel();
            this.lblProxyAddress = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtProxyAddress = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblProxyPort = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.numProxyPort = new mRemoteNG.UI.Controls.Base.NGNumericUpDown();
            this.chkUseProxyForAutomaticUpdates = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkUseProxyAuthentication = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.pnlProxyAuthentication = new System.Windows.Forms.Panel();
            this.lblProxyUsername = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtProxyUsername = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.lblProxyPassword = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.txtProxyPassword = new mRemoteNG.UI.Controls.Base.NGTextBox();
            this.btnTestProxy = new mRemoteNG.UI.Controls.Base.NGButton();
            this.pnlReleaseChannel = new System.Windows.Forms.Panel();
            this.pnlUpdateCheck.SuspendLayout();
            this.pnlProxy.SuspendLayout();
            this.pnlProxyBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).BeginInit();
            this.pnlProxyAuthentication.SuspendLayout();
            this.pnlReleaseChannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUpdatesExplanation
            // 
            this.lblUpdatesExplanation.Location = new System.Drawing.Point(3, 2);
            this.lblUpdatesExplanation.Name = "lblUpdatesExplanation";
            this.lblUpdatesExplanation.Size = new System.Drawing.Size(536, 32);
            this.lblUpdatesExplanation.TabIndex = 0;
            this.lblUpdatesExplanation.Text = "mRemoteNG can periodically connect to the mRemoteNG website to check for updates." +
    "";
            // 
            // pnlUpdateCheck
            // 
            this.pnlUpdateCheck.Controls.Add(this.btnUpdateCheckNow);
            this.pnlUpdateCheck.Controls.Add(this.chkCheckForUpdatesOnStartup);
            this.pnlUpdateCheck.Controls.Add(this.cboUpdateCheckFrequency);
            this.pnlUpdateCheck.Location = new System.Drawing.Point(3, 38);
            this.pnlUpdateCheck.Name = "pnlUpdateCheck";
            this.pnlUpdateCheck.Size = new System.Drawing.Size(604, 79);
            this.pnlUpdateCheck.TabIndex = 1;
            // 
            // btnUpdateCheckNow
            // 
            this.btnUpdateCheckNow._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnUpdateCheckNow.Location = new System.Drawing.Point(3, 49);
            this.btnUpdateCheckNow.Name = "btnUpdateCheckNow";
            this.btnUpdateCheckNow.Size = new System.Drawing.Size(120, 23);
            this.btnUpdateCheckNow.TabIndex = 2;
            this.btnUpdateCheckNow.Text = "Check Now";
            this.btnUpdateCheckNow.UseVisualStyleBackColor = true;
            this.btnUpdateCheckNow.Click += new System.EventHandler(this.btnUpdateCheckNow_Click);
            // 
            // chkCheckForUpdatesOnStartup
            // 
            this.chkCheckForUpdatesOnStartup._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkCheckForUpdatesOnStartup.AutoSize = true;
            this.chkCheckForUpdatesOnStartup.Location = new System.Drawing.Point(3, 4);
            this.chkCheckForUpdatesOnStartup.Name = "chkCheckForUpdatesOnStartup";
            this.chkCheckForUpdatesOnStartup.Size = new System.Drawing.Size(113, 17);
            this.chkCheckForUpdatesOnStartup.TabIndex = 0;
            this.chkCheckForUpdatesOnStartup.Text = "Check for updates";
            this.chkCheckForUpdatesOnStartup.UseVisualStyleBackColor = true;
            this.chkCheckForUpdatesOnStartup.CheckedChanged += new System.EventHandler(this.chkCheckForUpdatesOnStartup_CheckedChanged);
            // 
            // cboUpdateCheckFrequency
            // 
            this.cboUpdateCheckFrequency._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.cboUpdateCheckFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUpdateCheckFrequency.FormattingEnabled = true;
            this.cboUpdateCheckFrequency.Location = new System.Drawing.Point(3, 22);
            this.cboUpdateCheckFrequency.Name = "cboUpdateCheckFrequency";
            this.cboUpdateCheckFrequency.Size = new System.Drawing.Size(120, 21);
            this.cboUpdateCheckFrequency.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(3, 47);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(366, 44);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Stable channel includes final releases only.\r\nBeta channel includes Betas & Relea" +
    "se Candidates.\r\nDevelopment Channel includes Alphas, Betas & Release Candidates." +
    "";
            // 
            // lblReleaseChannel
            // 
            this.lblReleaseChannel.AutoSize = true;
            this.lblReleaseChannel.Location = new System.Drawing.Point(0, 3);
            this.lblReleaseChannel.Margin = new System.Windows.Forms.Padding(3);
            this.lblReleaseChannel.Name = "lblReleaseChannel";
            this.lblReleaseChannel.Size = new System.Drawing.Size(91, 13);
            this.lblReleaseChannel.TabIndex = 0;
            this.lblReleaseChannel.Text = "Release Channel:";
            // 
            // cboReleaseChannel
            // 
            this.cboReleaseChannel._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.cboReleaseChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReleaseChannel.FormattingEnabled = true;
            this.cboReleaseChannel.Location = new System.Drawing.Point(3, 20);
            this.cboReleaseChannel.Name = "cboReleaseChannel";
            this.cboReleaseChannel.Size = new System.Drawing.Size(120, 21);
            this.cboReleaseChannel.TabIndex = 1;
            // 
            // pnlProxy
            // 
            this.pnlProxy.Controls.Add(this.pnlProxyBasic);
            this.pnlProxy.Controls.Add(this.chkUseProxyForAutomaticUpdates);
            this.pnlProxy.Controls.Add(this.chkUseProxyAuthentication);
            this.pnlProxy.Controls.Add(this.pnlProxyAuthentication);
            this.pnlProxy.Controls.Add(this.btnTestProxy);
            this.pnlProxy.Location = new System.Drawing.Point(0, 226);
            this.pnlProxy.Name = "pnlProxy";
            this.pnlProxy.Size = new System.Drawing.Size(610, 203);
            this.pnlProxy.TabIndex = 3;
            // 
            // pnlProxyBasic
            // 
            this.pnlProxyBasic.Controls.Add(this.lblProxyAddress);
            this.pnlProxyBasic.Controls.Add(this.txtProxyAddress);
            this.pnlProxyBasic.Controls.Add(this.lblProxyPort);
            this.pnlProxyBasic.Controls.Add(this.numProxyPort);
            this.pnlProxyBasic.Enabled = false;
            this.pnlProxyBasic.Location = new System.Drawing.Point(3, 22);
            this.pnlProxyBasic.Name = "pnlProxyBasic";
            this.pnlProxyBasic.Size = new System.Drawing.Size(604, 40);
            this.pnlProxyBasic.TabIndex = 1;
            // 
            // lblProxyAddress
            // 
            this.lblProxyAddress.Location = new System.Drawing.Point(8, 0);
            this.lblProxyAddress.Name = "lblProxyAddress";
            this.lblProxyAddress.Size = new System.Drawing.Size(96, 24);
            this.lblProxyAddress.TabIndex = 0;
            this.lblProxyAddress.Text = "Address:";
            this.lblProxyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyAddress
            // 
            this.txtProxyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyAddress.Location = new System.Drawing.Point(110, 4);
            this.txtProxyAddress.Name = "txtProxyAddress";
            this.txtProxyAddress.Size = new System.Drawing.Size(240, 20);
            this.txtProxyAddress.TabIndex = 1;
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.Location = new System.Drawing.Point(350, 1);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(64, 23);
            this.lblProxyPort.TabIndex = 2;
            this.lblProxyPort.Text = "Port:";
            this.lblProxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numProxyPort
            // 
            this.numProxyPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numProxyPort.Location = new System.Drawing.Point(420, 4);
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
            this.chkUseProxyForAutomaticUpdates._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkUseProxyForAutomaticUpdates.AutoSize = true;
            this.chkUseProxyForAutomaticUpdates.Location = new System.Drawing.Point(6, 0);
            this.chkUseProxyForAutomaticUpdates.Name = "chkUseProxyForAutomaticUpdates";
            this.chkUseProxyForAutomaticUpdates.Size = new System.Drawing.Size(168, 17);
            this.chkUseProxyForAutomaticUpdates.TabIndex = 0;
            this.chkUseProxyForAutomaticUpdates.Text = "Use a proxy server to connect";
            this.chkUseProxyForAutomaticUpdates.UseVisualStyleBackColor = true;
            this.chkUseProxyForAutomaticUpdates.CheckedChanged += new System.EventHandler(this.chkUseProxyForAutomaticUpdates_CheckedChanged);
            // 
            // chkUseProxyAuthentication
            // 
            this.chkUseProxyAuthentication._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkUseProxyAuthentication.AutoSize = true;
            this.chkUseProxyAuthentication.Enabled = false;
            this.chkUseProxyAuthentication.Location = new System.Drawing.Point(27, 70);
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
            this.pnlProxyAuthentication.Location = new System.Drawing.Point(3, 94);
            this.pnlProxyAuthentication.Name = "pnlProxyAuthentication";
            this.pnlProxyAuthentication.Size = new System.Drawing.Size(604, 72);
            this.pnlProxyAuthentication.TabIndex = 3;
            // 
            // lblProxyUsername
            // 
            this.lblProxyUsername.Location = new System.Drawing.Point(8, 0);
            this.lblProxyUsername.Name = "lblProxyUsername";
            this.lblProxyUsername.Size = new System.Drawing.Size(96, 24);
            this.lblProxyUsername.TabIndex = 0;
            this.lblProxyUsername.Text = "Username:";
            this.lblProxyUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyUsername
            // 
            this.txtProxyUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyUsername.Location = new System.Drawing.Point(110, 4);
            this.txtProxyUsername.Name = "txtProxyUsername";
            this.txtProxyUsername.Size = new System.Drawing.Size(240, 20);
            this.txtProxyUsername.TabIndex = 1;
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.Location = new System.Drawing.Point(8, 26);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(96, 24);
            this.lblProxyPassword.TabIndex = 2;
            this.lblProxyPassword.Text = "Password:";
            this.lblProxyPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyPassword.Location = new System.Drawing.Point(110, 30);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(240, 20);
            this.txtProxyPassword.TabIndex = 3;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // btnTestProxy
            // 
            this.btnTestProxy._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.HOVER;
            this.btnTestProxy.Location = new System.Drawing.Point(6, 172);
            this.btnTestProxy.Name = "btnTestProxy";
            this.btnTestProxy.Size = new System.Drawing.Size(120, 23);
            this.btnTestProxy.TabIndex = 4;
            this.btnTestProxy.Text = "Test Proxy";
            this.btnTestProxy.UseVisualStyleBackColor = true;
            this.btnTestProxy.Click += new System.EventHandler(this.btnTestProxy_Click);
            // 
            // pnlReleaseChannel
            // 
            this.pnlReleaseChannel.Controls.Add(this.textBox1);
            this.pnlReleaseChannel.Controls.Add(this.lblReleaseChannel);
            this.pnlReleaseChannel.Controls.Add(this.cboReleaseChannel);
            this.pnlReleaseChannel.Location = new System.Drawing.Point(3, 123);
            this.pnlReleaseChannel.Name = "pnlReleaseChannel";
            this.pnlReleaseChannel.Size = new System.Drawing.Size(604, 97);
            this.pnlReleaseChannel.TabIndex = 2;
            // 
            // UpdatesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlReleaseChannel);
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
            this.pnlReleaseChannel.ResumeLayout(false);
            this.pnlReleaseChannel.PerformLayout();
            this.ResumeLayout(false);

		}
		internal Controls.Base.NGLabel lblUpdatesExplanation;
		internal System.Windows.Forms.Panel pnlUpdateCheck;
		internal Controls.Base.NGButton btnUpdateCheckNow;
		internal Controls.Base.NGCheckBox chkCheckForUpdatesOnStartup;
		internal Controls.Base.NGComboBox cboUpdateCheckFrequency;
		internal System.Windows.Forms.Panel pnlProxy;
		internal System.Windows.Forms.Panel pnlProxyBasic;
		internal Controls.Base.NGLabel lblProxyAddress;
		internal Controls.Base.NGTextBox txtProxyAddress;
		internal Controls.Base.NGLabel lblProxyPort;
		internal Controls.Base.NGNumericUpDown numProxyPort;
		internal Controls.Base.NGCheckBox chkUseProxyForAutomaticUpdates;
		internal Controls.Base.NGCheckBox chkUseProxyAuthentication;
		internal System.Windows.Forms.Panel pnlProxyAuthentication;
		internal Controls.Base.NGLabel lblProxyUsername;
		internal Controls.Base.NGTextBox txtProxyUsername;
		internal Controls.Base.NGLabel lblProxyPassword;
		internal Controls.Base.NGTextBox txtProxyPassword;
		internal Controls.Base.NGButton btnTestProxy;
        private Controls.Base.NGLabel lblReleaseChannel;
        private Controls.Base.NGComboBox cboReleaseChannel;
        private Controls.Base.NGTextBox textBox1;
        private System.Windows.Forms.Panel pnlReleaseChannel;
    }
}