

using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public sealed partial class StartupExitPage : OptionsPage
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
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
            this.chkReconnectOnStart = new MrngCheckBox();
            this.chkSaveConsOnExit = new MrngCheckBox();
            this.chkSingleInstance = new MrngCheckBox();
            this.chkProperInstallationOfComponentsAtStartup = new MrngCheckBox();
            this.chkStartMinimized = new MrngCheckBox();
            this.SuspendLayout();
            // 
            // chkReconnectOnStart
            // 
            this.chkReconnectOnStart._mice = MrngCheckBox.MouseState.OUT;
            this.chkReconnectOnStart.AutoSize = true;
            this.chkReconnectOnStart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkReconnectOnStart.Location = new System.Drawing.Point(3, 26);
            this.chkReconnectOnStart.Name = "chkReconnectOnStart";
            this.chkReconnectOnStart.Size = new System.Drawing.Size(295, 17);
            this.chkReconnectOnStart.TabIndex = 1;
            this.chkReconnectOnStart.Text = "Reconnect to previously opened sessions on startup";
            this.chkReconnectOnStart.UseVisualStyleBackColor = true;
            // 
            // chkSaveConsOnExit
            // 
            this.chkSaveConsOnExit._mice = MrngCheckBox.MouseState.OUT;
            this.chkSaveConsOnExit.AutoSize = true;
            this.chkSaveConsOnExit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSaveConsOnExit.Location = new System.Drawing.Point(3, 2);
            this.chkSaveConsOnExit.Name = "chkSaveConsOnExit";
            this.chkSaveConsOnExit.Size = new System.Drawing.Size(153, 17);
            this.chkSaveConsOnExit.TabIndex = 0;
            this.chkSaveConsOnExit.Text = "Save connections on exit";
            this.chkSaveConsOnExit.UseVisualStyleBackColor = true;
            // 
            // chkSingleInstance
            // 
            this.chkSingleInstance._mice = MrngCheckBox.MouseState.OUT;
            this.chkSingleInstance.AutoSize = true;
            this.chkSingleInstance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSingleInstance.Location = new System.Drawing.Point(3, 50);
            this.chkSingleInstance.Name = "chkSingleInstance";
            this.chkSingleInstance.Size = new System.Drawing.Size(404, 17);
            this.chkSingleInstance.TabIndex = 2;
            this.chkSingleInstance.Text = "Allow only a single instance of the application (mRemote restart required)";
            this.chkSingleInstance.UseVisualStyleBackColor = true;
            // 
            // chkProperInstallationOfComponentsAtStartup
            // 
            this.chkProperInstallationOfComponentsAtStartup._mice = MrngCheckBox.MouseState.OUT;
            this.chkProperInstallationOfComponentsAtStartup.AutoSize = true;
            this.chkProperInstallationOfComponentsAtStartup.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkProperInstallationOfComponentsAtStartup.Location = new System.Drawing.Point(3, 74);
            this.chkProperInstallationOfComponentsAtStartup.Name = "chkProperInstallationOfComponentsAtStartup";
            this.chkProperInstallationOfComponentsAtStartup.Size = new System.Drawing.Size(290, 17);
            this.chkProperInstallationOfComponentsAtStartup.TabIndex = 3;
            this.chkProperInstallationOfComponentsAtStartup.Text = "Check proper installation of components at startup";
            this.chkProperInstallationOfComponentsAtStartup.UseVisualStyleBackColor = true;
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized._mice = MrngCheckBox.MouseState.OUT;
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStartMinimized.Location = new System.Drawing.Point(3, 97);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(105, 17);
            this.chkStartMinimized.TabIndex = 3;
            this.chkStartMinimized.Text = "Start minimized";
            this.chkStartMinimized.UseVisualStyleBackColor = true;
            // 
            // StartupExitPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.chkReconnectOnStart);
            this.Controls.Add(this.chkSaveConsOnExit);
            this.Controls.Add(this.chkSingleInstance);
            this.Controls.Add(this.chkStartMinimized);
            this.Controls.Add(this.chkProperInstallationOfComponentsAtStartup);
            this.Name = "StartupExitPage";
            this.Size = new System.Drawing.Size(610, 490);
            this.Load += new System.EventHandler(this.StartupExitPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal MrngCheckBox chkReconnectOnStart;
		internal MrngCheckBox chkSaveConsOnExit;
		internal MrngCheckBox chkSingleInstance;
		internal MrngCheckBox chkProperInstallationOfComponentsAtStartup;
        internal MrngCheckBox chkStartMinimized;
    }
}
