

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
            this.chkReconnectOnStart = new mRemoteNG.UI.Controls.MrngCheckBox();
            this.chkSaveConsOnExit = new mRemoteNG.UI.Controls.MrngCheckBox();
            this.chkSingleInstance = new mRemoteNG.UI.Controls.MrngCheckBox();
            this.chkStartMinimized = new mRemoteNG.UI.Controls.MrngCheckBox();
            this.chkStartFullScreen = new mRemoteNG.UI.Controls.MrngCheckBox();
            this.SuspendLayout();
            // 
            // chkReconnectOnStart
            // 
            this.chkReconnectOnStart._mice = mRemoteNG.UI.Controls.MrngCheckBox.MouseState.OUT;
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
            this.chkSaveConsOnExit._mice = mRemoteNG.UI.Controls.MrngCheckBox.MouseState.OUT;
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
            this.chkSingleInstance._mice = mRemoteNG.UI.Controls.MrngCheckBox.MouseState.OUT;
            this.chkSingleInstance.AutoSize = true;
            this.chkSingleInstance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSingleInstance.Location = new System.Drawing.Point(3, 50);
            this.chkSingleInstance.Name = "chkSingleInstance";
            this.chkSingleInstance.Size = new System.Drawing.Size(404, 17);
            this.chkSingleInstance.TabIndex = 2;
            this.chkSingleInstance.Text = "Allow only a single instance of the application (mRemote restart required)";
            this.chkSingleInstance.UseVisualStyleBackColor = true;
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized._mice = mRemoteNG.UI.Controls.MrngCheckBox.MouseState.OUT;
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStartMinimized.Location = new System.Drawing.Point(3, 73);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(105, 17);
            this.chkStartMinimized.TabIndex = 3;
            this.chkStartMinimized.Text = "Start minimized";
            this.chkStartMinimized.UseVisualStyleBackColor = true;
            this.chkStartMinimized.CheckedChanged += new System.EventHandler(this.chkStartMinimized_CheckedChanged);
            // 
            // chkStartFullScreen
            // 
            this.chkStartFullScreen._mice = mRemoteNG.UI.Controls.MrngCheckBox.MouseState.OUT;
            this.chkStartFullScreen.AutoSize = true;
            this.chkStartFullScreen.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStartFullScreen.Location = new System.Drawing.Point(3, 96);
            this.chkStartFullScreen.Name = "chkStartFullScreen";
            this.chkStartFullScreen.Size = new System.Drawing.Size(109, 17);
            this.chkStartFullScreen.TabIndex = 4;
            this.chkStartFullScreen.Text = "Start Full Screen";
            this.chkStartFullScreen.UseVisualStyleBackColor = true;
            this.chkStartFullScreen.CheckedChanged += new System.EventHandler(this.chkStartFullScreen_CheckedChanged);
            // 
            // StartupExitPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.chkStartFullScreen);
            this.Controls.Add(this.chkReconnectOnStart);
            this.Controls.Add(this.chkSaveConsOnExit);
            this.Controls.Add(this.chkSingleInstance);
            this.Controls.Add(this.chkStartMinimized);
            this.Name = "StartupExitPage";
            this.Size = new System.Drawing.Size(610, 490);
            this.Load += new System.EventHandler(this.StartupExitPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal MrngCheckBox chkReconnectOnStart;
		internal MrngCheckBox chkSaveConsOnExit;
		internal MrngCheckBox chkSingleInstance;
        internal MrngCheckBox chkStartMinimized;
        internal MrngCheckBox chkStartFullScreen;
    }
}
