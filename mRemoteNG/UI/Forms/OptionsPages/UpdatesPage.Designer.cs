
using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class UpdatesPage : OptionsPage
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
                try { base.Dispose(disposing); }
                catch (System.NullReferenceException) { /* finalizer-safe: Control.ContextMenuStrip may be null on non-STA thread */ }
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
            lblUpdatesExplanation = new MrngLabel();
            pnlUpdateCheck = new System.Windows.Forms.Panel();
            cboUpdateCheckFrequency = new MrngComboBox();
            btnUpdateCheckNow = new MrngButton();
            chkCheckForUpdatesOnStartup = new MrngCheckBox();
            lblReleaseChannelExplanation = new MrngTextBox();
            cboReleaseChannel = new MrngComboBox();
            pnlProxy = new System.Windows.Forms.Panel();
            tblProxyBasic = new System.Windows.Forms.TableLayoutPanel();
            numProxyPort = new MrngNumericUpDown();
            lblProxyAddress = new MrngLabel();
            txtProxyAddress = new MrngTextBox();
            lblProxyPort = new MrngLabel();
            tblProxyAuthentication = new System.Windows.Forms.TableLayoutPanel();
            lblProxyPassword = new MrngLabel();
            lblProxyUsername = new MrngLabel();
            txtProxyUsername = new MrngTextBox();
            txtProxyPassword = new MrngTextBox();
            chkUseProxyForAutomaticUpdates = new MrngCheckBox();
            chkUseProxyAuthentication = new MrngCheckBox();
            btnTestProxy = new MrngButton();
            groupBoxReleaseChannel = new MrngGroupBox();
            pnlDefaultUpdate = new System.Windows.Forms.Panel();
            lblRegistrySettingsUsedInfo = new System.Windows.Forms.Label();
            pnlUpdateCheck.SuspendLayout();
            pnlProxy.SuspendLayout();
            tblProxyBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numProxyPort).BeginInit();
            tblProxyAuthentication.SuspendLayout();
            groupBoxReleaseChannel.SuspendLayout();
            pnlDefaultUpdate.SuspendLayout();
            SuspendLayout();
            // 
            // lblUpdatesExplanation
            // 
            lblUpdatesExplanation.Dock = System.Windows.Forms.DockStyle.Top;
            lblUpdatesExplanation.Location = new System.Drawing.Point(0, 0);
            lblUpdatesExplanation.Name = "lblUpdatesExplanation";
            lblUpdatesExplanation.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            lblUpdatesExplanation.Size = new System.Drawing.Size(610, 38);
            lblUpdatesExplanation.TabIndex = 0;
            lblUpdatesExplanation.Text = "mRemoteNG can periodically connect to the mRemoteNG website to check for updates.";
            // 
            // pnlUpdateCheck
            // 
            pnlUpdateCheck.Controls.Add(cboUpdateCheckFrequency);
            pnlUpdateCheck.Controls.Add(btnUpdateCheckNow);
            pnlUpdateCheck.Controls.Add(chkCheckForUpdatesOnStartup);
            pnlUpdateCheck.Controls.Add(lblUpdatesExplanation);
            pnlUpdateCheck.Dock = System.Windows.Forms.DockStyle.Top;
            pnlUpdateCheck.Location = new System.Drawing.Point(0, 30);
            pnlUpdateCheck.Name = "pnlUpdateCheck";
            pnlUpdateCheck.Size = new System.Drawing.Size(610, 114);
            pnlUpdateCheck.TabIndex = 0;
            // 
            // cboUpdateCheckFrequency
            // 
            cboUpdateCheckFrequency._mice = MrngComboBox.MouseState.HOVER;
            cboUpdateCheckFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboUpdateCheckFrequency.FormattingEnabled = true;
            cboUpdateCheckFrequency.Location = new System.Drawing.Point(27, 44);
            cboUpdateCheckFrequency.Name = "cboUpdateCheckFrequency";
            cboUpdateCheckFrequency.Size = new System.Drawing.Size(120, 21);
            cboUpdateCheckFrequency.TabIndex = 1;
            cboUpdateCheckFrequency.Enabled = false;
            // 
            // btnUpdateCheckNow
            // 
            btnUpdateCheckNow._mice = MrngButton.MouseState.OUT;
            btnUpdateCheckNow.Location = new System.Drawing.Point(5, 79);
            btnUpdateCheckNow.Name = "btnUpdateCheckNow";
            btnUpdateCheckNow.Size = new System.Drawing.Size(122, 25);
            btnUpdateCheckNow.TabIndex = 2;
            btnUpdateCheckNow.Text = "Check Now";
            btnUpdateCheckNow.UseVisualStyleBackColor = true;
            btnUpdateCheckNow.Click += btnUpdateCheckNow_Click;
            // 
            // chkCheckForUpdatesOnStartup
            // 
            chkCheckForUpdatesOnStartup._mice = MrngCheckBox.MouseState.OUT;
            chkCheckForUpdatesOnStartup.AutoSize = true;
            chkCheckForUpdatesOnStartup.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkCheckForUpdatesOnStartup.Location = new System.Drawing.Point(9, 21);
            chkCheckForUpdatesOnStartup.Name = "chkCheckForUpdatesOnStartup";
            chkCheckForUpdatesOnStartup.Size = new System.Drawing.Size(120, 17);
            chkCheckForUpdatesOnStartup.TabIndex = 0;
            chkCheckForUpdatesOnStartup.Text = "Check for updates";
            chkCheckForUpdatesOnStartup.UseVisualStyleBackColor = true;
            chkCheckForUpdatesOnStartup.CheckedChanged += chkCheckForUpdatesOnStartup_CheckedChanged;
            // 
            // lblReleaseChannelExplanation
            // 
            lblReleaseChannelExplanation.BackColor = System.Drawing.SystemColors.Control;
            lblReleaseChannelExplanation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lblReleaseChannelExplanation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblReleaseChannelExplanation.Location = new System.Drawing.Point(6, 48);
            lblReleaseChannelExplanation.Multiline = true;
            lblReleaseChannelExplanation.Name = "lblReleaseChannelExplanation";
            lblReleaseChannelExplanation.ReadOnly = true;
            lblReleaseChannelExplanation.Size = new System.Drawing.Size(595, 44);
            lblReleaseChannelExplanation.TabIndex = 2;
            lblReleaseChannelExplanation.Text = "Stable channel includes final releases only.\r\nBeta channel includes Betas & Release Candidates.\r\nDevelopment Channel includes Alphas, Betas & Release Candidates.";
            // 
            // cboReleaseChannel
            // 
            cboReleaseChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboReleaseChannel.FormattingEnabled = true;
            cboReleaseChannel.Location = new System.Drawing.Point(7, 21);
            cboReleaseChannel.Name = "cboReleaseChannel";
            cboReleaseChannel.Size = new System.Drawing.Size(120, 21);
            cboReleaseChannel.TabIndex = 1;
            // 
            // pnlProxy
            // 
            pnlProxy.Controls.Add(tblProxyBasic);
            pnlProxy.Controls.Add(tblProxyAuthentication);
            pnlProxy.Controls.Add(chkUseProxyForAutomaticUpdates);
            pnlProxy.Controls.Add(chkUseProxyAuthentication);
            pnlProxy.Controls.Add(btnTestProxy);
            pnlProxy.Dock = System.Windows.Forms.DockStyle.Top;
            pnlProxy.Location = new System.Drawing.Point(0, 248);
            pnlProxy.Name = "pnlProxy";
            pnlProxy.Size = new System.Drawing.Size(610, 232);
            pnlProxy.TabIndex = 3;
            // 
            // tblProxyBasic
            // 
            tblProxyBasic.ColumnCount = 2;
            tblProxyBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            tblProxyBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tblProxyBasic.Controls.Add(numProxyPort, 1, 1);
            tblProxyBasic.Controls.Add(lblProxyAddress, 0, 0);
            tblProxyBasic.Controls.Add(txtProxyAddress, 1, 0);
            tblProxyBasic.Controls.Add(lblProxyPort, 0, 1);
            tblProxyBasic.Location = new System.Drawing.Point(6, 28);
            tblProxyBasic.Name = "tblProxyBasic";
            tblProxyBasic.RowCount = 3;
            tblProxyBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tblProxyBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tblProxyBasic.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tblProxyBasic.Size = new System.Drawing.Size(350, 57);
            tblProxyBasic.TabIndex = 6;
            // 
            // numProxyPort
            // 
            numProxyPort.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            numProxyPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numProxyPort.Location = new System.Drawing.Point(163, 29);
            numProxyPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numProxyPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numProxyPort.Name = "numProxyPort";
            numProxyPort.Size = new System.Drawing.Size(64, 22);
            numProxyPort.TabIndex = 3;
            numProxyPort.Value = new decimal(new int[] { 80, 0, 0, 0 });
            // 
            // lblProxyAddress
            // 
            lblProxyAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            lblProxyAddress.Location = new System.Drawing.Point(3, 0);
            lblProxyAddress.Name = "lblProxyAddress";
            lblProxyAddress.Size = new System.Drawing.Size(154, 26);
            lblProxyAddress.TabIndex = 0;
            lblProxyAddress.Text = "Address:";
            lblProxyAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyAddress
            // 
            txtProxyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtProxyAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            txtProxyAddress.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtProxyAddress.Location = new System.Drawing.Point(163, 3);
            txtProxyAddress.Name = "txtProxyAddress";
            txtProxyAddress.Size = new System.Drawing.Size(184, 22);
            txtProxyAddress.TabIndex = 1;
            // 
            // lblProxyPort
            // 
            lblProxyPort.Dock = System.Windows.Forms.DockStyle.Fill;
            lblProxyPort.Location = new System.Drawing.Point(3, 26);
            lblProxyPort.Name = "lblProxyPort";
            lblProxyPort.Size = new System.Drawing.Size(154, 26);
            lblProxyPort.TabIndex = 2;
            lblProxyPort.Text = "Port:";
            lblProxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tblProxyAuthentication
            // 
            tblProxyAuthentication.ColumnCount = 2;
            tblProxyAuthentication.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            tblProxyAuthentication.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tblProxyAuthentication.Controls.Add(lblProxyPassword, 0, 1);
            tblProxyAuthentication.Controls.Add(lblProxyUsername, 0, 0);
            tblProxyAuthentication.Controls.Add(txtProxyUsername, 1, 0);
            tblProxyAuthentication.Controls.Add(txtProxyPassword, 1, 1);
            tblProxyAuthentication.Location = new System.Drawing.Point(6, 124);
            tblProxyAuthentication.Name = "tblProxyAuthentication";
            tblProxyAuthentication.RowCount = 3;
            tblProxyAuthentication.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tblProxyAuthentication.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tblProxyAuthentication.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tblProxyAuthentication.Size = new System.Drawing.Size(350, 57);
            tblProxyAuthentication.TabIndex = 5;
            // 
            // lblProxyPassword
            // 
            lblProxyPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            lblProxyPassword.Location = new System.Drawing.Point(3, 26);
            lblProxyPassword.Name = "lblProxyPassword";
            lblProxyPassword.Size = new System.Drawing.Size(154, 26);
            lblProxyPassword.TabIndex = 2;
            lblProxyPassword.Text = "Password:";
            lblProxyPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProxyUsername
            // 
            lblProxyUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            lblProxyUsername.Location = new System.Drawing.Point(3, 0);
            lblProxyUsername.Name = "lblProxyUsername";
            lblProxyUsername.Size = new System.Drawing.Size(154, 26);
            lblProxyUsername.TabIndex = 0;
            lblProxyUsername.Text = "Username:";
            lblProxyUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyUsername
            // 
            txtProxyUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtProxyUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            txtProxyUsername.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtProxyUsername.Location = new System.Drawing.Point(163, 3);
            txtProxyUsername.Name = "txtProxyUsername";
            txtProxyUsername.Size = new System.Drawing.Size(184, 22);
            txtProxyUsername.TabIndex = 1;
            // 
            // txtProxyPassword
            // 
            txtProxyPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtProxyPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            txtProxyPassword.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtProxyPassword.Location = new System.Drawing.Point(163, 29);
            txtProxyPassword.Name = "txtProxyPassword";
            txtProxyPassword.PasswordChar = '*';
            txtProxyPassword.Size = new System.Drawing.Size(184, 22);
            txtProxyPassword.TabIndex = 3;
            txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // chkUseProxyForAutomaticUpdates
            // 
            chkUseProxyForAutomaticUpdates._mice = MrngCheckBox.MouseState.OUT;
            chkUseProxyForAutomaticUpdates.AutoSize = true;
            chkUseProxyForAutomaticUpdates.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkUseProxyForAutomaticUpdates.Location = new System.Drawing.Point(6, 5);
            chkUseProxyForAutomaticUpdates.Name = "chkUseProxyForAutomaticUpdates";
            chkUseProxyForAutomaticUpdates.Size = new System.Drawing.Size(176, 17);
            chkUseProxyForAutomaticUpdates.TabIndex = 0;
            chkUseProxyForAutomaticUpdates.Text = "Use a proxy server to connect";
            chkUseProxyForAutomaticUpdates.UseVisualStyleBackColor = true;
            chkUseProxyForAutomaticUpdates.CheckedChanged += chkUseProxyForAutomaticUpdates_CheckedChanged;
            // 
            // chkUseProxyAuthentication
            // 
            chkUseProxyAuthentication._mice = MrngCheckBox.MouseState.OUT;
            chkUseProxyAuthentication.AutoSize = true;
            chkUseProxyAuthentication.Enabled = false;
            chkUseProxyAuthentication.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkUseProxyAuthentication.Location = new System.Drawing.Point(6, 101);
            chkUseProxyAuthentication.Name = "chkUseProxyAuthentication";
            chkUseProxyAuthentication.Size = new System.Drawing.Size(235, 17);
            chkUseProxyAuthentication.TabIndex = 2;
            chkUseProxyAuthentication.Text = "This proxy server requires authentication";
            chkUseProxyAuthentication.UseVisualStyleBackColor = true;
            chkUseProxyAuthentication.CheckedChanged += chkUseProxyAuthentication_CheckedChanged;
            // 
            // btnTestProxy
            // 
            btnTestProxy._mice = MrngButton.MouseState.OUT;
            btnTestProxy.Location = new System.Drawing.Point(6, 187);
            btnTestProxy.Name = "btnTestProxy";
            btnTestProxy.Size = new System.Drawing.Size(120, 25);
            btnTestProxy.TabIndex = 4;
            btnTestProxy.Text = "Test Proxy";
            btnTestProxy.UseVisualStyleBackColor = true;
            btnTestProxy.Click += btnTestProxy_Click;
            // 
            // groupBoxReleaseChannel
            // 
            groupBoxReleaseChannel.Controls.Add(lblReleaseChannelExplanation);
            groupBoxReleaseChannel.Controls.Add(cboReleaseChannel);
            groupBoxReleaseChannel.Dock = System.Windows.Forms.DockStyle.Top;
            groupBoxReleaseChannel.Location = new System.Drawing.Point(0, 144);
            groupBoxReleaseChannel.Name = "groupBoxReleaseChannel";
            groupBoxReleaseChannel.Size = new System.Drawing.Size(610, 104);
            groupBoxReleaseChannel.TabIndex = 3;
            groupBoxReleaseChannel.TabStop = false;
            groupBoxReleaseChannel.Text = "Release Channel";
            // 
            // pnlDefaultUpdate
            // 
            pnlDefaultUpdate.Controls.Add(pnlProxy);
            pnlDefaultUpdate.Controls.Add(groupBoxReleaseChannel);
            pnlDefaultUpdate.Controls.Add(pnlUpdateCheck);
            pnlDefaultUpdate.Controls.Add(lblRegistrySettingsUsedInfo);
            pnlDefaultUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            pnlDefaultUpdate.Location = new System.Drawing.Point(0, 0);
            pnlDefaultUpdate.Name = "pnlDefaultUpdate";
            pnlDefaultUpdate.Size = new System.Drawing.Size(610, 483);
            pnlDefaultUpdate.TabIndex = 4;
            // 
            // lblRegistrySettingsUsedInfo
            // 
            lblRegistrySettingsUsedInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            lblRegistrySettingsUsedInfo.Dock = System.Windows.Forms.DockStyle.Top;
            lblRegistrySettingsUsedInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            lblRegistrySettingsUsedInfo.Location = new System.Drawing.Point(0, 0);
            lblRegistrySettingsUsedInfo.Name = "lblRegistrySettingsUsedInfo";
            lblRegistrySettingsUsedInfo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            lblRegistrySettingsUsedInfo.Size = new System.Drawing.Size(610, 30);
            lblRegistrySettingsUsedInfo.TabIndex = 0;
            lblRegistrySettingsUsedInfo.Text = "Some settings are configured by your Administrator. Please contact your administrator for more information.";
            lblRegistrySettingsUsedInfo.Visible = false;
            // 
            // UpdatesPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(pnlDefaultUpdate);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "UpdatesPage";
            Size = new System.Drawing.Size(610, 496);
            pnlUpdateCheck.ResumeLayout(false);
            pnlUpdateCheck.PerformLayout();
            pnlProxy.ResumeLayout(false);
            pnlProxy.PerformLayout();
            tblProxyBasic.ResumeLayout(false);
            tblProxyBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numProxyPort).EndInit();
            tblProxyAuthentication.ResumeLayout(false);
            tblProxyAuthentication.PerformLayout();
            groupBoxReleaseChannel.ResumeLayout(false);
            groupBoxReleaseChannel.PerformLayout();
            pnlDefaultUpdate.ResumeLayout(false);
            ResumeLayout(false);
        }

        internal Controls.MrngLabel lblUpdatesExplanation;
        internal System.Windows.Forms.Panel pnlUpdateCheck;
        internal MrngButton btnUpdateCheckNow;
        internal MrngCheckBox chkCheckForUpdatesOnStartup;
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
        private System.Windows.Forms.Panel pnlDefaultUpdate;
        internal System.Windows.Forms.Label lblRegistrySettingsUsedInfo;
        internal MrngComboBox cboUpdateCheckFrequency;
    }
}