

namespace mRemoteNG.UI.Forms.OptionsPages
{
	
    public sealed partial class AppearancePage : OptionsPage
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
            this.lblLanguageRestartRequired = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.cboLanguage = new mRemoteNG.UI.Controls.Base.NGComboBox();
            this.lblLanguage = new mRemoteNG.UI.Controls.Base.NGLabel();
            this.chkShowFullConnectionsFilePathInTitle = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkShowDescriptionTooltipsInTree = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkShowSystemTrayIcon = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.chkMinimizeToSystemTray = new mRemoteNG.UI.Controls.Base.NGCheckBox();
            this.SuspendLayout();
            // 
            // lblLanguageRestartRequired
            // 
            this.lblLanguageRestartRequired.AutoSize = true;
            this.lblLanguageRestartRequired.Location = new System.Drawing.Point(3, 56);
            this.lblLanguageRestartRequired.Name = "lblLanguageRestartRequired";
            this.lblLanguageRestartRequired.Size = new System.Drawing.Size(380, 13);
            this.lblLanguageRestartRequired.TabIndex = 2;
            this.lblLanguageRestartRequired.Text = "mRemoteNG must be restarted before changes to the language will take effect.";
            // 
            // cboLanguage
            // 
            this.cboLanguage._mice = mRemoteNG.UI.Controls.Base.NGComboBox.MouseState.HOVER;
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Location = new System.Drawing.Point(3, 24);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(304, 21);
            this.cboLanguage.Sorted = true;
            this.cboLanguage.TabIndex = 1;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(3, 3);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(55, 13);
            this.lblLanguage.TabIndex = 0;
            this.lblLanguage.Text = "Language";
            // 
            // chkShowFullConnectionsFilePathInTitle
            // 
            this.chkShowFullConnectionsFilePathInTitle._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkShowFullConnectionsFilePathInTitle.AutoSize = true;
            this.chkShowFullConnectionsFilePathInTitle.Location = new System.Drawing.Point(3, 127);
            this.chkShowFullConnectionsFilePathInTitle.Name = "chkShowFullConnectionsFilePathInTitle";
            this.chkShowFullConnectionsFilePathInTitle.Size = new System.Drawing.Size(239, 17);
            this.chkShowFullConnectionsFilePathInTitle.TabIndex = 4;
            this.chkShowFullConnectionsFilePathInTitle.Text = "Show full connections file path in window title";
            this.chkShowFullConnectionsFilePathInTitle.UseVisualStyleBackColor = true;
            // 
            // chkShowDescriptionTooltipsInTree
            // 
            this.chkShowDescriptionTooltipsInTree._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkShowDescriptionTooltipsInTree.AutoSize = true;
            this.chkShowDescriptionTooltipsInTree.Location = new System.Drawing.Point(3, 104);
            this.chkShowDescriptionTooltipsInTree.Name = "chkShowDescriptionTooltipsInTree";
            this.chkShowDescriptionTooltipsInTree.Size = new System.Drawing.Size(231, 17);
            this.chkShowDescriptionTooltipsInTree.TabIndex = 3;
            this.chkShowDescriptionTooltipsInTree.Text = "Show description tooltips in connection tree";
            this.chkShowDescriptionTooltipsInTree.UseVisualStyleBackColor = true;
            // 
            // chkShowSystemTrayIcon
            // 
            this.chkShowSystemTrayIcon._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkShowSystemTrayIcon.AutoSize = true;
            this.chkShowSystemTrayIcon.Location = new System.Drawing.Point(3, 173);
            this.chkShowSystemTrayIcon.Name = "chkShowSystemTrayIcon";
            this.chkShowSystemTrayIcon.Size = new System.Drawing.Size(172, 17);
            this.chkShowSystemTrayIcon.TabIndex = 5;
            this.chkShowSystemTrayIcon.Text = "Always show System Tray Icon";
            this.chkShowSystemTrayIcon.UseVisualStyleBackColor = true;
            // 
            // chkMinimizeToSystemTray
            // 
            this.chkMinimizeToSystemTray._mice = mRemoteNG.UI.Controls.Base.NGCheckBox.MouseState.HOVER;
            this.chkMinimizeToSystemTray.AutoSize = true;
            this.chkMinimizeToSystemTray.Location = new System.Drawing.Point(3, 196);
            this.chkMinimizeToSystemTray.Name = "chkMinimizeToSystemTray";
            this.chkMinimizeToSystemTray.Size = new System.Drawing.Size(139, 17);
            this.chkMinimizeToSystemTray.TabIndex = 6;
            this.chkMinimizeToSystemTray.Text = "Minimize to System Tray";
            this.chkMinimizeToSystemTray.UseVisualStyleBackColor = true;
            // 
            // AppearancePage
            // 
            this.Controls.Add(this.lblLanguageRestartRequired);
            this.Controls.Add(this.cboLanguage);
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.chkShowFullConnectionsFilePathInTitle);
            this.Controls.Add(this.chkShowDescriptionTooltipsInTree);
            this.Controls.Add(this.chkShowSystemTrayIcon);
            this.Controls.Add(this.chkMinimizeToSystemTray);
            this.Name = "AppearancePage";
            this.PageIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PageIcon")));
            this.Size = new System.Drawing.Size(610, 489);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal Controls.Base.NGLabel lblLanguageRestartRequired;
		internal Controls.Base.NGComboBox cboLanguage;
		internal Controls.Base.NGLabel lblLanguage;
		internal Controls.Base.NGCheckBox chkShowFullConnectionsFilePathInTitle;
		internal Controls.Base.NGCheckBox chkShowDescriptionTooltipsInTree;
		internal Controls.Base.NGCheckBox chkShowSystemTrayIcon;
		internal Controls.Base.NGCheckBox chkMinimizeToSystemTray;
			
	}
}
