

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
            lblMaximumPuttyWaitTime = new MrngLabel();
            chkAutomaticReconnect = new MrngCheckBox();
            numPuttyWaitTime = new MrngNumericUpDown();
            chkUseCustomPuttyPath = new MrngCheckBox();
            lblConfigurePuttySessions = new MrngLabel();
            numUVNCSCPort = new MrngNumericUpDown();
            txtCustomPuttyPath = new MrngTextBox();
            btnLaunchPutty = new MrngButton();
            lblUVNCSCPort = new MrngLabel();
            lblSeconds = new MrngLabel();
            btnBrowseCustomPuttyPath = new MrngButton();
            chkLoadBalanceInfoUseUtf8 = new MrngCheckBox();
            chkNoReconnect = new MrngCheckBox();
            ((System.ComponentModel.ISupportInitialize)numPuttyWaitTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUVNCSCPort).BeginInit();
            SuspendLayout();
            // 
            // lblMaximumPuttyWaitTime
            // 
            lblMaximumPuttyWaitTime.Location = new System.Drawing.Point(9, 174);
            lblMaximumPuttyWaitTime.Name = "lblMaximumPuttyWaitTime";
            lblMaximumPuttyWaitTime.Size = new System.Drawing.Size(364, 20);
            lblMaximumPuttyWaitTime.TabIndex = 7;
            lblMaximumPuttyWaitTime.Text = "Maximum PuTTY wait time:";
            lblMaximumPuttyWaitTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkAutomaticReconnect
            // 
            chkAutomaticReconnect._mice = MrngCheckBox.MouseState.OUT;
            chkAutomaticReconnect.AutoSize = true;
            chkAutomaticReconnect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkAutomaticReconnect.Location = new System.Drawing.Point(9, 8);
            chkAutomaticReconnect.Name = "chkAutomaticReconnect";
            chkAutomaticReconnect.Size = new System.Drawing.Size(421, 17);
            chkAutomaticReconnect.TabIndex = 1;
            chkAutomaticReconnect.Text = "Display reconnection dialog when disconnected from server (RDP && ICA only)";
            chkAutomaticReconnect.UseVisualStyleBackColor = true;
            chkAutomaticReconnect.CheckedChanged += chkAutomaticReconnect_CheckedChanged;
            // 
            // numPuttyWaitTime
            // 
            numPuttyWaitTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numPuttyWaitTime.Location = new System.Drawing.Point(840, 321);
            numPuttyWaitTime.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            numPuttyWaitTime.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            numPuttyWaitTime.Name = "numPuttyWaitTime";
            numPuttyWaitTime.Size = new System.Drawing.Size(135, 22);
            numPuttyWaitTime.TabIndex = 7;
            numPuttyWaitTime.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // chkUseCustomPuttyPath
            // 
            chkUseCustomPuttyPath._mice = MrngCheckBox.MouseState.OUT;
            chkUseCustomPuttyPath.AutoSize = true;
            chkUseCustomPuttyPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkUseCustomPuttyPath.Location = new System.Drawing.Point(9, 78);
            chkUseCustomPuttyPath.Name = "chkUseCustomPuttyPath";
            chkUseCustomPuttyPath.Size = new System.Drawing.Size(148, 17);
            chkUseCustomPuttyPath.TabIndex = 3;
            chkUseCustomPuttyPath.Text = "Use custom PuTTY path:";
            chkUseCustomPuttyPath.UseVisualStyleBackColor = true;
            chkUseCustomPuttyPath.CheckedChanged += chkUseCustomPuttyPath_CheckedChanged;
            // 
            // lblConfigurePuttySessions
            // 
            lblConfigurePuttySessions.Location = new System.Drawing.Point(9, 138);
            lblConfigurePuttySessions.Name = "lblConfigurePuttySessions";
            lblConfigurePuttySessions.Size = new System.Drawing.Size(364, 25);
            lblConfigurePuttySessions.TabIndex = 5;
            lblConfigurePuttySessions.Text = "To configure PuTTY sessions click this button:";
            lblConfigurePuttySessions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numUVNCSCPort
            // 
            numUVNCSCPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numUVNCSCPort.Location = new System.Drawing.Point(840, 384);
            numUVNCSCPort.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            numUVNCSCPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numUVNCSCPort.Name = "numUVNCSCPort";
            numUVNCSCPort.Size = new System.Drawing.Size(135, 22);
            numUVNCSCPort.TabIndex = 8;
            numUVNCSCPort.Value = new decimal(new int[] { 5500, 0, 0, 0 });
            numUVNCSCPort.Visible = false;
            // 
            // txtCustomPuttyPath
            // 
            txtCustomPuttyPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCustomPuttyPath.Enabled = false;
            txtCustomPuttyPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtCustomPuttyPath.Location = new System.Drawing.Point(27, 100);
            txtCustomPuttyPath.Name = "txtCustomPuttyPath";
            txtCustomPuttyPath.Size = new System.Drawing.Size(346, 22);
            txtCustomPuttyPath.TabIndex = 4;
            txtCustomPuttyPath.TextChanged += txtCustomPuttyPath_TextChanged;
            // 
            // btnLaunchPutty
            // 
            btnLaunchPutty._mice = MrngButton.MouseState.OUT;
            btnLaunchPutty.Image = Properties.Resources.PuttyConfig;
            btnLaunchPutty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnLaunchPutty.Location = new System.Drawing.Point(379, 138);
            btnLaunchPutty.Name = "btnLaunchPutty";
            btnLaunchPutty.Size = new System.Drawing.Size(122, 25);
            btnLaunchPutty.TabIndex = 6;
            btnLaunchPutty.Text = "Launch PuTTY";
            btnLaunchPutty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            btnLaunchPutty.UseVisualStyleBackColor = true;
            btnLaunchPutty.Click += btnLaunchPutty_Click;
            // 
            // lblUVNCSCPort
            // 
            lblUVNCSCPort.Location = new System.Drawing.Point(9, 202);
            lblUVNCSCPort.Name = "lblUVNCSCPort";
            lblUVNCSCPort.Size = new System.Drawing.Size(364, 19);
            lblUVNCSCPort.TabIndex = 10;
            lblUVNCSCPort.Text = "UltraVNC SingleClick Listening Port:";
            lblUVNCSCPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lblUVNCSCPort.Visible = false;
            // 
            // lblSeconds
            // 
            lblSeconds.AutoSize = true;
            lblSeconds.Location = new System.Drawing.Point(445, 178);
            lblSeconds.Name = "lblSeconds";
            lblSeconds.Size = new System.Drawing.Size(49, 13);
            lblSeconds.TabIndex = 9;
            lblSeconds.Text = "seconds";
            // 
            // btnBrowseCustomPuttyPath
            // 
            btnBrowseCustomPuttyPath._mice = MrngButton.MouseState.OUT;
            btnBrowseCustomPuttyPath.Enabled = false;
            btnBrowseCustomPuttyPath.Location = new System.Drawing.Point(379, 99);
            btnBrowseCustomPuttyPath.Name = "btnBrowseCustomPuttyPath";
            btnBrowseCustomPuttyPath.Size = new System.Drawing.Size(122, 25);
            btnBrowseCustomPuttyPath.TabIndex = 5;
            btnBrowseCustomPuttyPath.Text = "Browse...";
            btnBrowseCustomPuttyPath.UseVisualStyleBackColor = true;
            btnBrowseCustomPuttyPath.Click += btnBrowseCustomPuttyPath_Click;
            // 
            // chkLoadBalanceInfoUseUtf8
            // 
            chkLoadBalanceInfoUseUtf8._mice = MrngCheckBox.MouseState.OUT;
            chkLoadBalanceInfoUseUtf8.AutoSize = true;
            chkLoadBalanceInfoUseUtf8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkLoadBalanceInfoUseUtf8.Location = new System.Drawing.Point(9, 54);
            chkLoadBalanceInfoUseUtf8.Name = "chkLoadBalanceInfoUseUtf8";
            chkLoadBalanceInfoUseUtf8.Size = new System.Drawing.Size(317, 17);
            chkLoadBalanceInfoUseUtf8.TabIndex = 2;
            chkLoadBalanceInfoUseUtf8.Text = "Use UTF8 encoding for RDP \"Load Balance Info\" property";
            chkLoadBalanceInfoUseUtf8.UseVisualStyleBackColor = true;
            // 
            // chkNoReconnect
            // 
            chkNoReconnect._mice = MrngCheckBox.MouseState.OUT;
            chkNoReconnect.AutoSize = true;
            chkNoReconnect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkNoReconnect.Location = new System.Drawing.Point(27, 31);
            chkNoReconnect.Name = "chkNoReconnect";
            chkNoReconnect.Size = new System.Drawing.Size(430, 17);
            chkNoReconnect.TabIndex = 11;
            chkNoReconnect.Text = "Automatically try to reconnect when disconnected from server (RDP && ICA only)";
            chkNoReconnect.UseVisualStyleBackColor = true;
            chkNoReconnect.CheckedChanged += chkNoReconnect_CheckedChanged;
            // 
            // AdvancedPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(chkNoReconnect);
            Controls.Add(chkLoadBalanceInfoUseUtf8);
            Controls.Add(lblMaximumPuttyWaitTime);
            Controls.Add(chkAutomaticReconnect);
            Controls.Add(numPuttyWaitTime);
            Controls.Add(chkUseCustomPuttyPath);
            Controls.Add(lblConfigurePuttySessions);
            Controls.Add(numUVNCSCPort);
            Controls.Add(txtCustomPuttyPath);
            Controls.Add(btnLaunchPutty);
            Controls.Add(lblUVNCSCPort);
            Controls.Add(lblSeconds);
            Controls.Add(btnBrowseCustomPuttyPath);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "AdvancedPage";
            Size = new System.Drawing.Size(610, 490);
            ((System.ComponentModel.ISupportInitialize)numPuttyWaitTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUVNCSCPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
