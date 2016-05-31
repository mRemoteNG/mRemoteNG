

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public partial class AppearancePage : OptionsPage
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppearancePage));
			this.lblLanguageRestartRequired = new System.Windows.Forms.Label();
			this.cboLanguage = new System.Windows.Forms.ComboBox();
			this.lblLanguage = new System.Windows.Forms.Label();
			this.chkShowFullConnectionsFilePathInTitle = new System.Windows.Forms.CheckBox();
			this.chkShowDescriptionTooltipsInTree = new System.Windows.Forms.CheckBox();
			this.chkShowSystemTrayIcon = new System.Windows.Forms.CheckBox();
			this.chkMinimizeToSystemTray = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			//
			//lblLanguageRestartRequired
			//
			this.lblLanguageRestartRequired.AutoSize = true;
			this.lblLanguageRestartRequired.Location = new System.Drawing.Point(3, 56);
			this.lblLanguageRestartRequired.Name = "lblLanguageRestartRequired";
			this.lblLanguageRestartRequired.Size = new System.Drawing.Size(380, 13);
			this.lblLanguageRestartRequired.TabIndex = 9;
			this.lblLanguageRestartRequired.Text = "mRemoteNG must be restarted before changes to the language will take effect.";
			//
			//cboLanguage
			//
			this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboLanguage.FormattingEnabled = true;
			this.cboLanguage.Location = new System.Drawing.Point(3, 24);
			this.cboLanguage.Name = "cboLanguage";
			this.cboLanguage.Size = new System.Drawing.Size(304, 21);
			this.cboLanguage.Sorted = true;
			this.cboLanguage.TabIndex = 8;
			//
			//lblLanguage
			//
			this.lblLanguage.AutoSize = true;
			this.lblLanguage.Location = new System.Drawing.Point(3, 0);
			this.lblLanguage.Name = "lblLanguage";
			this.lblLanguage.Size = new System.Drawing.Size(55, 13);
			this.lblLanguage.TabIndex = 7;
			this.lblLanguage.Text = "Language";
			//
			//chkShowFullConnectionsFilePathInTitle
			//
			this.chkShowFullConnectionsFilePathInTitle.AutoSize = true;
			this.chkShowFullConnectionsFilePathInTitle.Location = new System.Drawing.Point(3, 141);
			this.chkShowFullConnectionsFilePathInTitle.Name = "chkShowFullConnectionsFilePathInTitle";
			this.chkShowFullConnectionsFilePathInTitle.Size = new System.Drawing.Size(239, 17);
			this.chkShowFullConnectionsFilePathInTitle.TabIndex = 11;
			this.chkShowFullConnectionsFilePathInTitle.Text = "Show full connections file path in window title";
			this.chkShowFullConnectionsFilePathInTitle.UseVisualStyleBackColor = true;
			//
			//chkShowDescriptionTooltipsInTree
			//
			this.chkShowDescriptionTooltipsInTree.AutoSize = true;
			this.chkShowDescriptionTooltipsInTree.Location = new System.Drawing.Point(3, 118);
			this.chkShowDescriptionTooltipsInTree.Name = "chkShowDescriptionTooltipsInTree";
			this.chkShowDescriptionTooltipsInTree.Size = new System.Drawing.Size(231, 17);
			this.chkShowDescriptionTooltipsInTree.TabIndex = 10;
			this.chkShowDescriptionTooltipsInTree.Text = "Show description tooltips in connection tree";
			this.chkShowDescriptionTooltipsInTree.UseVisualStyleBackColor = true;
			//
			//chkShowSystemTrayIcon
			//
			this.chkShowSystemTrayIcon.AutoSize = true;
			this.chkShowSystemTrayIcon.Location = new System.Drawing.Point(3, 187);
			this.chkShowSystemTrayIcon.Name = "chkShowSystemTrayIcon";
			this.chkShowSystemTrayIcon.Size = new System.Drawing.Size(172, 17);
			this.chkShowSystemTrayIcon.TabIndex = 12;
			this.chkShowSystemTrayIcon.Text = "Always show System Tray Icon";
			this.chkShowSystemTrayIcon.UseVisualStyleBackColor = true;
			//
			//chkMinimizeToSystemTray
			//
			this.chkMinimizeToSystemTray.AutoSize = true;
			this.chkMinimizeToSystemTray.Location = new System.Drawing.Point(3, 210);
			this.chkMinimizeToSystemTray.Name = "chkMinimizeToSystemTray";
			this.chkMinimizeToSystemTray.Size = new System.Drawing.Size(139, 17);
			this.chkMinimizeToSystemTray.TabIndex = 13;
			this.chkMinimizeToSystemTray.Text = "Minimize to System Tray";
			this.chkMinimizeToSystemTray.UseVisualStyleBackColor = true;
			//
			//AppearancePage
			//
			this.Controls.Add(this.lblLanguageRestartRequired);
			this.Controls.Add(this.cboLanguage);
			this.Controls.Add(this.lblLanguage);
			this.Controls.Add(this.chkShowFullConnectionsFilePathInTitle);
			this.Controls.Add(this.chkShowDescriptionTooltipsInTree);
			this.Controls.Add(this.chkShowSystemTrayIcon);
			this.Controls.Add(this.chkMinimizeToSystemTray);
			this.Name = "AppearancePage";
			this.PageIcon = (System.Drawing.Icon) (resources.GetObject("$this.PageIcon"));
			this.Size = new System.Drawing.Size(610, 489);
			this.ResumeLayout(false);
			this.PerformLayout();
				
		}
		internal System.Windows.Forms.Label lblLanguageRestartRequired;
		internal System.Windows.Forms.ComboBox cboLanguage;
		internal System.Windows.Forms.Label lblLanguage;
		internal System.Windows.Forms.CheckBox chkShowFullConnectionsFilePathInTitle;
		internal System.Windows.Forms.CheckBox chkShowDescriptionTooltipsInTree;
		internal System.Windows.Forms.CheckBox chkShowSystemTrayIcon;
		internal System.Windows.Forms.CheckBox chkMinimizeToSystemTray;
			
	}
}
