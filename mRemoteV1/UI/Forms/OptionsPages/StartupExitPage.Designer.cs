

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public partial class StartupExitPage : OptionsPage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupExitPage));
			this.chkReconnectOnStart = new System.Windows.Forms.CheckBox();
			base.Load += new System.EventHandler(StartupExitPage_Load);
			this.chkSaveConsOnExit = new System.Windows.Forms.CheckBox();
			this.chkSingleInstance = new System.Windows.Forms.CheckBox();
			this.chkProperInstallationOfComponentsAtStartup = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			//
			//chkReconnectOnStart
			//
			this.chkReconnectOnStart.AutoSize = true;
			this.chkReconnectOnStart.Location = new System.Drawing.Point(3, 24);
			this.chkReconnectOnStart.Name = "chkReconnectOnStart";
			this.chkReconnectOnStart.Size = new System.Drawing.Size(273, 17);
			this.chkReconnectOnStart.TabIndex = 7;
			this.chkReconnectOnStart.Text = "Reconnect to previously opened sessions on startup";
			this.chkReconnectOnStart.UseVisualStyleBackColor = true;
			//
			//chkSaveConsOnExit
			//
			this.chkSaveConsOnExit.AutoSize = true;
			this.chkSaveConsOnExit.Location = new System.Drawing.Point(3, 0);
			this.chkSaveConsOnExit.Name = "chkSaveConsOnExit";
			this.chkSaveConsOnExit.Size = new System.Drawing.Size(146, 17);
			this.chkSaveConsOnExit.TabIndex = 6;
			this.chkSaveConsOnExit.Text = "Save connections on exit";
			this.chkSaveConsOnExit.UseVisualStyleBackColor = true;
			//
			//chkSingleInstance
			//
			this.chkSingleInstance.AutoSize = true;
			this.chkSingleInstance.Location = new System.Drawing.Point(3, 48);
			this.chkSingleInstance.Name = "chkSingleInstance";
			this.chkSingleInstance.Size = new System.Drawing.Size(366, 17);
			this.chkSingleInstance.TabIndex = 8;
			this.chkSingleInstance.Text = "Allow only a single instance of the application (mRemote restart required)";
			this.chkSingleInstance.UseVisualStyleBackColor = true;
			//
			//chkProperInstallationOfComponentsAtStartup
			//
			this.chkProperInstallationOfComponentsAtStartup.AutoSize = true;
			this.chkProperInstallationOfComponentsAtStartup.Location = new System.Drawing.Point(3, 72);
			this.chkProperInstallationOfComponentsAtStartup.Name = "chkProperInstallationOfComponentsAtStartup";
			this.chkProperInstallationOfComponentsAtStartup.Size = new System.Drawing.Size(262, 17);
			this.chkProperInstallationOfComponentsAtStartup.TabIndex = 9;
			this.chkProperInstallationOfComponentsAtStartup.Text = "Check proper installation of components at startup";
			this.chkProperInstallationOfComponentsAtStartup.UseVisualStyleBackColor = true;
			//
			//StartupExitPage
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF((float) (6.0F), (float) (13.0F));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkReconnectOnStart);
			this.Controls.Add(this.chkSaveConsOnExit);
			this.Controls.Add(this.chkSingleInstance);
			this.Controls.Add(this.chkProperInstallationOfComponentsAtStartup);
			this.Name = "StartupExitPage";
			this.PageIcon = (System.Drawing.Icon) (resources.GetObject("$this.PageIcon"));
			this.Size = new System.Drawing.Size(610, 489);
			this.ResumeLayout(false);
			this.PerformLayout();
				
		}
		internal System.Windows.Forms.CheckBox chkReconnectOnStart;
		internal System.Windows.Forms.CheckBox chkSaveConsOnExit;
		internal System.Windows.Forms.CheckBox chkSingleInstance;
		internal System.Windows.Forms.CheckBox chkProperInstallationOfComponentsAtStartup;
			
	}
}
