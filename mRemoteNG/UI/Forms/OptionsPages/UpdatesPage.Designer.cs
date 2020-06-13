
using mRemoteNG.UI.Controls;

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
            this.lblUpdatesExplanation = new mRemoteNG.UI.Controls.MrngLabel();
            this.pnlUpdateCheck = new System.Windows.Forms.Panel();
            this.btnUpdateCheckNow = new MrngButton();
            this.chkCheckForUpdatesOnStartup = new MrngCheckBox();
            this.cboUpdateCheckFrequency = new MrngComboBox();
            this.lblReleaseChannelExplanation = new mRemoteNG.UI.Controls.MrngTextBox();
            this.cboReleaseChannel = new MrngComboBox();
            this.pnlProxy = new System.Windows.Forms.Panel();
            this.tblProxyBasic = new System.Windows.Forms.TableLayoutPanel();
            this.numProxyPort = new mRemoteNG.UI.Controls.MrngNumericUpDown();
            this.lblProxyAddress = new mRemoteNG.UI.Controls.MrngLabel();
            this.txtProxyAddress = new mRemoteNG.UI.Controls.MrngTextBox();
            this.lblProxyPort = new mRemoteNG.UI.Controls.MrngLabel();
            this.tblProxyAuthentication = new System.Windows.Forms.TableLayoutPanel();
            this.lblProxyPassword = new mRemoteNG.UI.Controls.MrngLabel();
            this.lblProxyUsername = new mRemoteNG.UI.Controls.MrngLabel();
            this.txtProxyUsername = new mRemoteNG.UI.Controls.MrngTextBox();
            this.txtProxyPassword = new mRemoteNG.UI.Controls.MrngTextBox();
            this.chkUseProxyForAutomaticUpdates = new MrngCheckBox();
            this.chkUseProxyAuthentication = new MrngCheckBox();
            this.btnTestProxy = new MrngButton();
            this.groupBoxReleaseChannel = new MrngGroupBox();
            this.pnlUpdateCheck.SuspendLayout();
            this.pnlProxy.SuspendLayout();
            this.tblProxyBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).BeginInit();
            this.tblProxyAuthentication.SuspendLayout();
            this.groupBoxReleaseChannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUpdatesExplanation
            // 
            this.lblUpdatesExplanation.Location = new System.Drawing.Point(3, 3);
            this.lblUpdatesExplanation.Name = "lblUpdatesExplanation";
            this.lblUpdatesExplanation.Size = new System.Drawing.Size(595, 32);
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
            this.pnlUpdateCheck.Size = new System.Drawing.Size(604, 99);
            this.pnlUpdateCheck.TabIndex = 1;
            // 
            // btnUpdateCheckNow
            // 
            this.btnUpdateCheckNow._mice = MrngButton.MouseState.OUT;
            this.btnUpdateCheckNow.Location = new System.Drawing.Point(5, 63);
            this.btnUpdateCheckNow.Name = "btnUpdateCheckNow";
            this.btnUpdateCheckNow.Size = new System.Drawing.Size(122, 25);
            this.btnUpdateCheckNow.TabIndex = 2;
            this.btnUpdateCheckNow.Text = "Check Now";
            this.btnUpdateCheckNow.UseVisualStyleBackColor = true;
            this.btnUpdateCheckNow.Click += new System.EventHandler(this.btnUpdateCheckNow_Click);
            // 
            // chkCheckForUpdatesOnStartup
            // 
            this.chkCheckForUpdatesOnStartup._mice = MrngCheckBox.MouseState.OUT;
            this.chkCheckForUpdatesOnStartup.AutoSize = true;
            this.chkCheckForUpdatesOnStartup.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCheckForUpdatesOnStartup.Location = new System.Drawing.Point(6, 11);
            this.chkCheckForUpdatesOnStartup.Name = "chkCheckForUpdatesOnStartup";
            this.chkCheckForUpdatesOnStartup.Size = new System.Drawing.Size(120, 17);
            this.chkCheckForUpdatesOnStartup.TabIndex = 0;
            this.chkCheckForUpdatesOnStartup.Text = "Check for updates";
            this.chkCheckForUpdatesOnStartup.UseVisualStyleBackColor = true;
            this.chkCheckForUpdatesOnStartup.CheckedChanged += new System.EventHandler(this.chkCheckForUpdatesOnStartup_CheckedChanged);
            // 
            // cboUpdateCheckFrequency
            // 
            this.cboUpdateCheckFrequency._mice = MrngComboBox.MouseState.HOVER;
            this.cboUpdateCheckFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUpdateCheckFrequency.FormattingEnabled = true;
            this.cboUpdateCheckFrequency.Location = new System.Drawing.Point(6, 34);
            this.cboUpdateCheckFrequency.Name = "cboUpdateCheckFrequency";
            this.cboUpdateCheckFrequency.Size = new System.Drawing.Size(120, 21);
            this.cboUpdateCheckFrequency.TabIndex = 1;
            // 
            // lblReleaseChannelExplanation
            // 
            this.lblReleaseChannelExplanation.BackColor = System.Drawing.SystemColors.Control;
            this.lblReleaseChannelExplanation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblReleaseChannelExplanation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReleaseChannelExplanation.Location = new System.Drawing.Point(6, 48);
            this.lblReleaseChannelExplanation.Multiline = true;
            this.lblReleaseChannelExplanation.Name = "lblReleaseChannelExplanation";
            this.lblReleaseChannelExplanation.ReadOnly = true;
            this.lblReleaseChannelExplanation.Size = new System.Drawing.Size(595, 44);
            this.lblReleaseChannelExplanation.TabIndex = 2;
            this.lblReleaseChannelExplanation.Text = "Stable channel includes final releases only.\r\nBeta channel includes Betas & Relea" +
    "se Candidates.\r\nDevelopment Channel includes Alphas, Betas & Release Candidates." +
    "";
            // 
            // cboReleaseChannel
            // 
            this.cboReleaseChannel._mice = MrngComboBox.MouseState.HOVER;
            this.cboReleaseChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReleaseChannel.FormattingEnabled = true;
            this.cboReleaseChannel.Location = new System.Drawing.Point(7, 21);
            this.cboReleaseChannel.Name = "cboReleaseChannel";
            this.cboReleaseChannel.Size = new System.Drawing.Size(120, 21);
            this.cboReleaseChannel.TabIndex = 1;
            // 
            // pnlProxy
            // 
            this.pnlProxy.Controls.Add(this.tblProxyBasic);
            this.pnlProxy.Controls.Add(this.tblProxyAuthentication);
            this.pnlProxy.Controls.Add(this.chkUseProxyForAutomaticUpdates);
            this.pnlProxy.Controls.Add(this.chkUseProxyAuthentication);
            this.pnlProxy.Controls.Add(this.btnTestProxy);
            this.pnlProxy.Location = new System.Drawing.Point(3, 253);
            this.pnlProxy.Name = "pnlProxy";
            this.pnlProxy.Size = new System.Drawing.Size(604, 223);
            this.pnlProxy.TabIndex = 3;
            // 
            // tblProxyBasic
            // 
            this.tblProxyBasic.ColumnCount = 2;
            this.tblProxyBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tblProxyBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProxyBasic.Controls.Add(this.numProxyPort, 1, 1);
            this.tblProxyBasic.Controls.Add(this.lblProxyAddress, 0, 0);
            this.tblProxyBasic.Controls.Add(this.txtProxyAddress, 1, 0);
            this.tblProxyBasic.Controls.Add(this.lblProxyPort, 0, 1);
            this.tblProxyBasic.Location = new System.Drawing.Point(6, 28);
            this.tblProxyBasic.Name = "tblProxyBasic";
            this.tblProxyBasic.RowCount = 3;
            this.tblProxyBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tblProxyBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tblProxyBasic.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProxyBasic.Size = new System.Drawing.Size(350, 57);
            this.tblProxyBasic.TabIndex = 6;
            // 
            // numProxyPort
            // 
            this.numProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numProxyPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numProxyPort.Location = new System.Drawing.Point(163, 29);
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
            this.numProxyPort.Size = new System.Drawing.Size(64, 22);
            this.numProxyPort.TabIndex = 3;
            this.numProxyPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // lblProxyAddress
            // 
            this.lblProxyAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProxyAddress.Location = new System.Drawing.Point(3, 0);
            this.lblProxyAddress.Name = "lblProxyAddress";
            this.lblProxyAddress.Size = new System.Drawing.Size(154, 26);
            this.lblProxyAddress.TabIndex = 0;
            this.lblProxyAddress.Text = "Address:";
            this.lblProxyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyAddress
            // 
            this.txtProxyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyAddress.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProxyAddress.Location = new System.Drawing.Point(163, 3);
            this.txtProxyAddress.Name = "txtProxyAddress";
            this.txtProxyAddress.Size = new System.Drawing.Size(184, 22);
            this.txtProxyAddress.TabIndex = 1;
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProxyPort.Location = new System.Drawing.Point(3, 26);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(154, 26);
            this.lblProxyPort.TabIndex = 2;
            this.lblProxyPort.Text = "Port:";
            this.lblProxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tblProxyAuthentication
            // 
            this.tblProxyAuthentication.ColumnCount = 2;
            this.tblProxyAuthentication.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tblProxyAuthentication.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProxyAuthentication.Controls.Add(this.lblProxyPassword, 0, 1);
            this.tblProxyAuthentication.Controls.Add(this.lblProxyUsername, 0, 0);
            this.tblProxyAuthentication.Controls.Add(this.txtProxyUsername, 1, 0);
            this.tblProxyAuthentication.Controls.Add(this.txtProxyPassword, 1, 1);
            this.tblProxyAuthentication.Location = new System.Drawing.Point(6, 124);
            this.tblProxyAuthentication.Name = "tblProxyAuthentication";
            this.tblProxyAuthentication.RowCount = 3;
            this.tblProxyAuthentication.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tblProxyAuthentication.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tblProxyAuthentication.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblProxyAuthentication.Size = new System.Drawing.Size(350, 57);
            this.tblProxyAuthentication.TabIndex = 5;
            // 
            // lblProxyPassword
            // 
            this.lblProxyPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProxyPassword.Location = new System.Drawing.Point(3, 26);
            this.lblProxyPassword.Name = "lblProxyPassword";
            this.lblProxyPassword.Size = new System.Drawing.Size(154, 26);
            this.lblProxyPassword.TabIndex = 2;
            this.lblProxyPassword.Text = "Password:";
            this.lblProxyPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProxyUsername
            // 
            this.lblProxyUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProxyUsername.Location = new System.Drawing.Point(3, 0);
            this.lblProxyUsername.Name = "lblProxyUsername";
            this.lblProxyUsername.Size = new System.Drawing.Size(154, 26);
            this.lblProxyUsername.TabIndex = 0;
            this.lblProxyUsername.Text = "Username:";
            this.lblProxyUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyUsername
            // 
            this.txtProxyUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProxyUsername.Location = new System.Drawing.Point(163, 3);
            this.txtProxyUsername.Name = "txtProxyUsername";
            this.txtProxyUsername.Size = new System.Drawing.Size(184, 22);
            this.txtProxyUsername.TabIndex = 1;
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProxyPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProxyPassword.Location = new System.Drawing.Point(163, 29);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(184, 22);
            this.txtProxyPassword.TabIndex = 3;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // chkUseProxyForAutomaticUpdates
            // 
            this.chkUseProxyForAutomaticUpdates._mice = MrngCheckBox.MouseState.OUT;
            this.chkUseProxyForAutomaticUpdates.AutoSize = true;
            this.chkUseProxyForAutomaticUpdates.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseProxyForAutomaticUpdates.Location = new System.Drawing.Point(6, 5);
            this.chkUseProxyForAutomaticUpdates.Name = "chkUseProxyForAutomaticUpdates";
            this.chkUseProxyForAutomaticUpdates.Size = new System.Drawing.Size(176, 17);
            this.chkUseProxyForAutomaticUpdates.TabIndex = 0;
            this.chkUseProxyForAutomaticUpdates.Text = "Use a proxy server to connect";
            this.chkUseProxyForAutomaticUpdates.UseVisualStyleBackColor = true;
            this.chkUseProxyForAutomaticUpdates.CheckedChanged += new System.EventHandler(this.chkUseProxyForAutomaticUpdates_CheckedChanged);
            // 
            // chkUseProxyAuthentication
            // 
            this.chkUseProxyAuthentication._mice = MrngCheckBox.MouseState.OUT;
            this.chkUseProxyAuthentication.AutoSize = true;
            this.chkUseProxyAuthentication.Enabled = false;
            this.chkUseProxyAuthentication.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseProxyAuthentication.Location = new System.Drawing.Point(6, 101);
            this.chkUseProxyAuthentication.Name = "chkUseProxyAuthentication";
            this.chkUseProxyAuthentication.Size = new System.Drawing.Size(235, 17);
            this.chkUseProxyAuthentication.TabIndex = 2;
            this.chkUseProxyAuthentication.Text = "This proxy server requires authentication";
            this.chkUseProxyAuthentication.UseVisualStyleBackColor = true;
            this.chkUseProxyAuthentication.CheckedChanged += new System.EventHandler(this.chkUseProxyAuthentication_CheckedChanged);
            // 
            // btnTestProxy
            // 
            this.btnTestProxy._mice = MrngButton.MouseState.OUT;
            this.btnTestProxy.Location = new System.Drawing.Point(6, 187);
            this.btnTestProxy.Name = "btnTestProxy";
            this.btnTestProxy.Size = new System.Drawing.Size(120, 25);
            this.btnTestProxy.TabIndex = 4;
            this.btnTestProxy.Text = "Test Proxy";
            this.btnTestProxy.UseVisualStyleBackColor = true;
            this.btnTestProxy.Click += new System.EventHandler(this.btnTestProxy_Click);
            // 
            // groupBoxReleaseChannel
            // 
            this.groupBoxReleaseChannel.Controls.Add(this.lblReleaseChannelExplanation);
            this.groupBoxReleaseChannel.Controls.Add(this.cboReleaseChannel);
            this.groupBoxReleaseChannel.Location = new System.Drawing.Point(3, 143);
            this.groupBoxReleaseChannel.Name = "groupBoxReleaseChannel";
            this.groupBoxReleaseChannel.Size = new System.Drawing.Size(604, 104);
            this.groupBoxReleaseChannel.TabIndex = 3;
            this.groupBoxReleaseChannel.TabStop = false;
            this.groupBoxReleaseChannel.Text = "Release Channel";
            // 
            // UpdatesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.groupBoxReleaseChannel);
            this.Controls.Add(this.lblUpdatesExplanation);
            this.Controls.Add(this.pnlUpdateCheck);
            this.Controls.Add(this.pnlProxy);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UpdatesPage";
            this.Size = new System.Drawing.Size(610, 490);
            this.pnlUpdateCheck.ResumeLayout(false);
            this.pnlUpdateCheck.PerformLayout();
            this.pnlProxy.ResumeLayout(false);
            this.pnlProxy.PerformLayout();
            this.tblProxyBasic.ResumeLayout(false);
            this.tblProxyBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).EndInit();
            this.tblProxyAuthentication.ResumeLayout(false);
            this.tblProxyAuthentication.PerformLayout();
            this.groupBoxReleaseChannel.ResumeLayout(false);
            this.groupBoxReleaseChannel.PerformLayout();
            this.ResumeLayout(false);

		}
		internal Controls.MrngLabel lblUpdatesExplanation;
		internal System.Windows.Forms.Panel pnlUpdateCheck;
		internal MrngButton btnUpdateCheckNow;
		internal MrngCheckBox chkCheckForUpdatesOnStartup;
		internal MrngComboBox cboUpdateCheckFrequency;
		internal System.Windows.Forms.Panel pnlProxy;
		internal Controls.MrngLabel lblProxyAddress;
		internal Controls.MrngTextBox txtProxyAddress;
		internal Controls.MrngLabel lblProxyPort;
		internal Controls.MrngNumericUpDown numProxyPort;
		internal MrngCheckBox chkUseProxyForAutomaticUpdates;
		internal MrngCheckBox chkUseProxyAuthentication;
		internal Controls.MrngLabel lblProxyUsername;
		internal Controls.MrngTextBox txtProxyUsername;
		internal Controls.MrngLabel lblProxyPassword;
		internal Controls.MrngTextBox txtProxyPassword;
		internal MrngButton btnTestProxy;
        private MrngComboBox cboReleaseChannel;
        private Controls.MrngTextBox lblReleaseChannelExplanation;
        private MrngGroupBox groupBoxReleaseChannel;
        private System.Windows.Forms.TableLayoutPanel tblProxyBasic;
        private System.Windows.Forms.TableLayoutPanel tblProxyAuthentication;
    }
}