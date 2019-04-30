
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
            this.lblUpdatesExplanation = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.pnlUpdateCheck = new System.Windows.Forms.Panel();
            this.btnUpdateCheckNow = new mRemoteNG.UI.Controls.Base.NGButton();
            this.chkCheckForUpdatesOnStartup = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.cboUpdateCheckFrequency = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.lblReleaseChannelExplanation = new mRemoteNG.UI.Controls.Base.NGTextBox();
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
            this.groupBoxReleaseChannel = new mRemoteNG.UI.Controls.Base.NGGroupBox();
            this.pnlUpdateCheck.SuspendLayout();
            this.pnlProxy.SuspendLayout();
            this.pnlProxyBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).BeginInit();
            this.pnlProxyAuthentication.SuspendLayout();
            this.groupBoxReleaseChannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUpdatesExplanation
            // 
            this.lblUpdatesExplanation.Location = new System.Drawing.Point(4, 5);
            this.lblUpdatesExplanation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpdatesExplanation.Name = "lblUpdatesExplanation";
            this.lblUpdatesExplanation.Size = new System.Drawing.Size(892, 48);
            this.lblUpdatesExplanation.TabIndex = 0;
            this.lblUpdatesExplanation.Text = "mRemoteNG can periodically connect to the mRemoteNG website to check for updates." +
    "";
            // 
            // pnlUpdateCheck
            // 
            this.pnlUpdateCheck.Controls.Add(this.btnUpdateCheckNow);
            this.pnlUpdateCheck.Controls.Add(this.chkCheckForUpdatesOnStartup);
            this.pnlUpdateCheck.Controls.Add(this.cboUpdateCheckFrequency);
            this.pnlUpdateCheck.Location = new System.Drawing.Point(4, 57);
            this.pnlUpdateCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlUpdateCheck.Name = "pnlUpdateCheck";
            this.pnlUpdateCheck.Size = new System.Drawing.Size(906, 128);
            this.pnlUpdateCheck.TabIndex = 1;
            // 
            // btnUpdateCheckNow
            // 
            this.btnUpdateCheckNow._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.OUT;
            this.btnUpdateCheckNow.Location = new System.Drawing.Point(4, 88);
            this.btnUpdateCheckNow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUpdateCheckNow.Name = "btnUpdateCheckNow";
            this.btnUpdateCheckNow.Size = new System.Drawing.Size(180, 34);
            this.btnUpdateCheckNow.TabIndex = 2;
            this.btnUpdateCheckNow.Text = "Check Now";
            this.btnUpdateCheckNow.UseVisualStyleBackColor = true;
            this.btnUpdateCheckNow.Click += new System.EventHandler(this.btnUpdateCheckNow_Click);
            // 
            // chkCheckForUpdatesOnStartup
            // 
            this.chkCheckForUpdatesOnStartup._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkCheckForUpdatesOnStartup.AutoSize = true;
            this.chkCheckForUpdatesOnStartup.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCheckForUpdatesOnStartup.Location = new System.Drawing.Point(4, 10);
            this.chkCheckForUpdatesOnStartup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCheckForUpdatesOnStartup.Name = "chkCheckForUpdatesOnStartup";
            this.chkCheckForUpdatesOnStartup.Size = new System.Drawing.Size(174, 27);
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
            this.cboUpdateCheckFrequency.Location = new System.Drawing.Point(4, 45);
            this.cboUpdateCheckFrequency.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboUpdateCheckFrequency.Name = "cboUpdateCheckFrequency";
            this.cboUpdateCheckFrequency.Size = new System.Drawing.Size(178, 31);
            this.cboUpdateCheckFrequency.TabIndex = 1;
            // 
            // lblReleaseChannelExplanation
            // 
            this.lblReleaseChannelExplanation.BackColor = System.Drawing.SystemColors.Control;
            this.lblReleaseChannelExplanation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblReleaseChannelExplanation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReleaseChannelExplanation.Location = new System.Drawing.Point(4, 82);
            this.lblReleaseChannelExplanation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblReleaseChannelExplanation.Multiline = true;
            this.lblReleaseChannelExplanation.Name = "lblReleaseChannelExplanation";
            this.lblReleaseChannelExplanation.ReadOnly = true;
            this.lblReleaseChannelExplanation.Size = new System.Drawing.Size(892, 66);
            this.lblReleaseChannelExplanation.TabIndex = 2;
            this.lblReleaseChannelExplanation.Text = "Stable channel includes final releases only.\r\nBeta channel includes Betas & Relea" +
    "se Candidates.\r\nDevelopment Channel includes Alphas, Betas & Release Candidates." +
    "";
            // 
            // cboReleaseChannel
            // 
            this.cboReleaseChannel._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.cboReleaseChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReleaseChannel.FormattingEnabled = true;
            this.cboReleaseChannel.Location = new System.Drawing.Point(4, 32);
            this.cboReleaseChannel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboReleaseChannel.Name = "cboReleaseChannel";
            this.cboReleaseChannel.Size = new System.Drawing.Size(178, 31);
            this.cboReleaseChannel.TabIndex = 1;
            // 
            // pnlProxy
            // 
            this.pnlProxy.Controls.Add(this.pnlProxyBasic);
            this.pnlProxy.Controls.Add(this.chkUseProxyForAutomaticUpdates);
            this.pnlProxy.Controls.Add(this.chkUseProxyAuthentication);
            this.pnlProxy.Controls.Add(this.pnlProxyAuthentication);
            this.pnlProxy.Controls.Add(this.btnTestProxy);
            this.pnlProxy.Location = new System.Drawing.Point(0, 357);
            this.pnlProxy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlProxy.Name = "pnlProxy";
            this.pnlProxy.Size = new System.Drawing.Size(915, 301);
            this.pnlProxy.TabIndex = 3;
            // 
            // pnlProxyBasic
            // 
            this.pnlProxyBasic.Controls.Add(this.lblProxyAddress);
            this.pnlProxyBasic.Controls.Add(this.txtProxyAddress);
            this.pnlProxyBasic.Controls.Add(this.lblProxyPort);
            this.pnlProxyBasic.Controls.Add(this.numProxyPort);
            this.pnlProxyBasic.Enabled = false;
            this.pnlProxyBasic.Location = new System.Drawing.Point(4, 42);
            this.pnlProxyBasic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlProxyBasic.Name = "pnlProxyBasic";
            this.pnlProxyBasic.Size = new System.Drawing.Size(906, 56);
            this.pnlProxyBasic.TabIndex = 1;
            // 
            // lblProxyAddress
            // 
            this.lblProxyAddress.Location = new System.Drawing.Point(13, 16);
            this.lblProxyAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProxyAddress.Name = "lblProxyAddress";
            this.lblProxyAddress.Size = new System.Drawing.Size(144, 29);
            this.lblProxyAddress.TabIndex = 0;
            this.lblProxyAddress.Text = "Address:";
            this.lblProxyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyAddress
            // 
            this.txtProxyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyAddress.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProxyAddress.Location = new System.Drawing.Point(165, 17);
            this.txtProxyAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProxyAddress.Name = "txtProxyAddress";
            this.txtProxyAddress.Size = new System.Drawing.Size(359, 29);
            this.txtProxyAddress.TabIndex = 1;
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.Location = new System.Drawing.Point(532, 17);
            this.lblProxyPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(96, 29);
            this.lblProxyPort.TabIndex = 2;
            this.lblProxyPort.Text = "Port:";
            this.lblProxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numProxyPort
            // 
            this.numProxyPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numProxyPort.Location = new System.Drawing.Point(636, 16);
            this.numProxyPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.numProxyPort.Size = new System.Drawing.Size(96, 29);
            this.numProxyPort.TabIndex = 3;
            this.numProxyPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // chkUseProxyForAutomaticUpdates
            // 
            this.chkUseProxyForAutomaticUpdates._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkUseProxyForAutomaticUpdates.AutoSize = true;
            this.chkUseProxyForAutomaticUpdates.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseProxyForAutomaticUpdates.Location = new System.Drawing.Point(9, 7);
            this.chkUseProxyForAutomaticUpdates.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUseProxyForAutomaticUpdates.Name = "chkUseProxyForAutomaticUpdates";
            this.chkUseProxyForAutomaticUpdates.Size = new System.Drawing.Size(262, 27);
            this.chkUseProxyForAutomaticUpdates.TabIndex = 0;
            this.chkUseProxyForAutomaticUpdates.Text = "Use a proxy server to connect";
            this.chkUseProxyForAutomaticUpdates.UseVisualStyleBackColor = true;
            this.chkUseProxyForAutomaticUpdates.CheckedChanged += new System.EventHandler(this.chkUseProxyForAutomaticUpdates_CheckedChanged);
            // 
            // chkUseProxyAuthentication
            // 
            this.chkUseProxyAuthentication._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.OUT;
            this.chkUseProxyAuthentication.AutoSize = true;
            this.chkUseProxyAuthentication.Enabled = false;
            this.chkUseProxyAuthentication.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseProxyAuthentication.Location = new System.Drawing.Point(8, 106);
            this.chkUseProxyAuthentication.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUseProxyAuthentication.Name = "chkUseProxyAuthentication";
            this.chkUseProxyAuthentication.Size = new System.Drawing.Size(345, 27);
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
            this.pnlProxyAuthentication.Location = new System.Drawing.Point(4, 141);
            this.pnlProxyAuthentication.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlProxyAuthentication.Name = "pnlProxyAuthentication";
            this.pnlProxyAuthentication.Size = new System.Drawing.Size(906, 115);
            this.pnlProxyAuthentication.TabIndex = 3;
            // 
            // lblProxyUsername
            // 
            this.lblProxyUsername.Location = new System.Drawing.Point(13, 19);
            this.lblProxyUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProxyUsername.Name = "lblProxyUsername";
            this.lblProxyUsername.Size = new System.Drawing.Size(144, 29);
            this.lblProxyUsername.TabIndex = 0;
            this.lblProxyUsername.Text = "Username:";
            this.lblProxyUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyUsername
            // 
            this.txtProxyUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProxyUsername.Location = new System.Drawing.Point(165, 19);
            this.txtProxyUsername.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProxyUsername.Name = "txtProxyUsername";
            this.txtProxyUsername.Size = new System.Drawing.Size(359, 29);
            this.txtProxyUsername.TabIndex = 1;
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.Location = new System.Drawing.Point(13, 66);
            this.lblProxyPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(144, 29);
            this.lblProxyPassword.TabIndex = 2;
            this.lblProxyPassword.Text = "Password:";
            this.lblProxyPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProxyPassword.Location = new System.Drawing.Point(165, 66);
            this.txtProxyPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(359, 29);
            this.txtProxyPassword.TabIndex = 3;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // btnTestProxy
            // 
            this.btnTestProxy._mice = mRemoteNG.UI.Controls.Base.NGButton.MouseState.OUT;
            this.btnTestProxy.Location = new System.Drawing.Point(8, 263);
            this.btnTestProxy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTestProxy.Name = "btnTestProxy";
            this.btnTestProxy.Size = new System.Drawing.Size(180, 34);
            this.btnTestProxy.TabIndex = 4;
            this.btnTestProxy.Text = "Test Proxy";
            this.btnTestProxy.UseVisualStyleBackColor = true;
            this.btnTestProxy.Click += new System.EventHandler(this.btnTestProxy_Click);
            // 
            // groupBoxReleaseChannel
            // 
            this.groupBoxReleaseChannel.Controls.Add(this.lblReleaseChannelExplanation);
            this.groupBoxReleaseChannel.Controls.Add(this.cboReleaseChannel);
            this.groupBoxReleaseChannel.Location = new System.Drawing.Point(4, 193);
            this.groupBoxReleaseChannel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxReleaseChannel.Name = "groupBoxReleaseChannel";
            this.groupBoxReleaseChannel.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxReleaseChannel.Size = new System.Drawing.Size(906, 156);
            this.groupBoxReleaseChannel.TabIndex = 3;
            this.groupBoxReleaseChannel.TabStop = false;
            this.groupBoxReleaseChannel.Text = "Release Channel";
            // 
            // UpdatesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.groupBoxReleaseChannel);
            this.Controls.Add(this.lblUpdatesExplanation);
            this.Controls.Add(this.pnlUpdateCheck);
            this.Controls.Add(this.pnlProxy);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "UpdatesPage";
            this.Size = new System.Drawing.Size(915, 735);
            this.pnlUpdateCheck.ResumeLayout(false);
            this.pnlUpdateCheck.PerformLayout();
            this.pnlProxy.ResumeLayout(false);
            this.pnlProxy.PerformLayout();
            this.pnlProxyBasic.ResumeLayout(false);
            this.pnlProxyBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).EndInit();
            this.pnlProxyAuthentication.ResumeLayout(false);
            this.pnlProxyAuthentication.PerformLayout();
            this.groupBoxReleaseChannel.ResumeLayout(false);
            this.groupBoxReleaseChannel.PerformLayout();
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
        private Controls.Base.NGComboBox cboReleaseChannel;
        private Controls.Base.NGTextBox lblReleaseChannelExplanation;
        private Controls.Base.NGGroupBox groupBoxReleaseChannel;
    }
}