

using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class StartupExitPage : OptionsPage
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
            chkReconnectOnStart = new MrngCheckBox();
            chkSingleInstance = new MrngCheckBox();
            chkStartMinimized = new MrngCheckBox();
            chkStartFullScreen = new MrngCheckBox();
            pnlOptions = new System.Windows.Forms.Panel();
            lblRegistrySettingsUsedInfo = new System.Windows.Forms.Label();
            pnlOptions.SuspendLayout();
            SuspendLayout();
            // 
            // chkReconnectOnStart
            // 
            chkReconnectOnStart._mice = MrngCheckBox.MouseState.OUT;
            chkReconnectOnStart.AutoSize = true;
            chkReconnectOnStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkReconnectOnStart.Location = new System.Drawing.Point(6, 4);
            chkReconnectOnStart.Name = "chkReconnectOnStart";
            chkReconnectOnStart.Size = new System.Drawing.Size(295, 17);
            chkReconnectOnStart.TabIndex = 1;
            chkReconnectOnStart.Text = "Reconnect to previously opened sessions on startup";
            chkReconnectOnStart.UseVisualStyleBackColor = true;
            // 
            // chkSingleInstance
            // 
            chkSingleInstance._mice = MrngCheckBox.MouseState.OUT;
            chkSingleInstance.AutoSize = true;
            chkSingleInstance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkSingleInstance.Location = new System.Drawing.Point(6, 27);
            chkSingleInstance.Name = "chkSingleInstance";
            chkSingleInstance.Size = new System.Drawing.Size(404, 17);
            chkSingleInstance.TabIndex = 2;
            chkSingleInstance.Text = "Allow only a single instance of the application (mRemote restart required)";
            chkSingleInstance.UseVisualStyleBackColor = true;
            // 
            // chkStartMinimized
            // 
            chkStartMinimized._mice = MrngCheckBox.MouseState.OUT;
            chkStartMinimized.AutoSize = true;
            chkStartMinimized.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkStartMinimized.Location = new System.Drawing.Point(6, 50);
            chkStartMinimized.Name = "chkStartMinimized";
            chkStartMinimized.Size = new System.Drawing.Size(105, 17);
            chkStartMinimized.TabIndex = 3;
            chkStartMinimized.Text = "Start minimized";
            chkStartMinimized.UseVisualStyleBackColor = true;
            chkStartMinimized.CheckedChanged += chkStartMinimized_CheckedChanged;
            // 
            // chkStartFullScreen
            // 
            chkStartFullScreen._mice = MrngCheckBox.MouseState.OUT;
            chkStartFullScreen.AutoSize = true;
            chkStartFullScreen.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkStartFullScreen.Location = new System.Drawing.Point(6, 73);
            chkStartFullScreen.Name = "chkStartFullScreen";
            chkStartFullScreen.Size = new System.Drawing.Size(109, 17);
            chkStartFullScreen.TabIndex = 4;
            chkStartFullScreen.Text = "Start Full Screen";
            chkStartFullScreen.UseVisualStyleBackColor = true;
            chkStartFullScreen.CheckedChanged += chkStartFullScreen_CheckedChanged;
            // 
            // pnlOptions
            // 
            pnlOptions.Controls.Add(chkStartFullScreen);
            pnlOptions.Controls.Add(chkReconnectOnStart);
            pnlOptions.Controls.Add(chkStartMinimized);
            pnlOptions.Controls.Add(chkSingleInstance);
            pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            pnlOptions.Location = new System.Drawing.Point(0, 30);
            pnlOptions.Name = "pnlOptions";
            pnlOptions.Size = new System.Drawing.Size(610, 135);
            pnlOptions.TabIndex = 0;
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
            // StartupExitPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(pnlOptions);
            Controls.Add(lblRegistrySettingsUsedInfo);
            Name = "StartupExitPage";
            Size = new System.Drawing.Size(610, 490);
            Load += StartupExitPage_Load;
            pnlOptions.ResumeLayout(false);
            pnlOptions.PerformLayout();
            ResumeLayout(false);
        }

        internal MrngCheckBox chkReconnectOnStart;
        internal MrngCheckBox chkSingleInstance;
        internal MrngCheckBox chkStartMinimized;
        internal MrngCheckBox chkStartFullScreen;
        internal System.Windows.Forms.Label lblRegistrySettingsUsedInfo;
        internal System.Windows.Forms.Panel pnlOptions;
    }
}
