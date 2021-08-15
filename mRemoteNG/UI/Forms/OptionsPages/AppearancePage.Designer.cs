

using mRemoteNG.UI.Controls;

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
            this.lblLanguageRestartRequired = new mRemoteNG.UI.Controls.MrngLabel();
            this.cboLanguage = new MrngComboBox();
            this.lblLanguage = new mRemoteNG.UI.Controls.MrngLabel();
            this.chkShowFullConnectionsFilePathInTitle = new MrngCheckBox();
            this.chkShowDescriptionTooltipsInTree = new MrngCheckBox();
            this.chkShowSystemTrayIcon = new MrngCheckBox();
            this.chkMinimizeToSystemTray = new MrngCheckBox();
            this.chkCloseToSystemTray = new MrngCheckBox();
            this.SuspendLayout();
            // 
            // lblLanguageRestartRequired
            // 
            this.lblLanguageRestartRequired.AutoSize = true;
            this.lblLanguageRestartRequired.Location = new System.Drawing.Point(3, 56);
            this.lblLanguageRestartRequired.Name = "lblLanguageRestartRequired";
            this.lblLanguageRestartRequired.Size = new System.Drawing.Size(414, 13);
            this.lblLanguageRestartRequired.TabIndex = 2;
            this.lblLanguageRestartRequired.Text = "mRemoteNG must be restarted before changes to the language will take effect.";
            // 
            // cboLanguage
            // 
            this.cboLanguage._mice = MrngComboBox.MouseState.HOVER;
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
            this.lblLanguage.Size = new System.Drawing.Size(58, 13);
            this.lblLanguage.TabIndex = 0;
            this.lblLanguage.Text = "Language";
            // 
            // chkShowFullConnectionsFilePathInTitle
            // 
            this.chkShowFullConnectionsFilePathInTitle._mice = MrngCheckBox.MouseState.OUT;
            this.chkShowFullConnectionsFilePathInTitle.AutoSize = true;
            this.chkShowFullConnectionsFilePathInTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowFullConnectionsFilePathInTitle.Location = new System.Drawing.Point(3, 127);
            this.chkShowFullConnectionsFilePathInTitle.Name = "chkShowFullConnectionsFilePathInTitle";
            this.chkShowFullConnectionsFilePathInTitle.Size = new System.Drawing.Size(268, 17);
            this.chkShowFullConnectionsFilePathInTitle.TabIndex = 4;
            this.chkShowFullConnectionsFilePathInTitle.Text = "Show full connections file path in window title";
            this.chkShowFullConnectionsFilePathInTitle.UseVisualStyleBackColor = true;
            // 
            // chkShowDescriptionTooltipsInTree
            // 
            this.chkShowDescriptionTooltipsInTree._mice = MrngCheckBox.MouseState.OUT;
            this.chkShowDescriptionTooltipsInTree.AutoSize = true;
            this.chkShowDescriptionTooltipsInTree.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowDescriptionTooltipsInTree.Location = new System.Drawing.Point(3, 104);
            this.chkShowDescriptionTooltipsInTree.Name = "chkShowDescriptionTooltipsInTree";
            this.chkShowDescriptionTooltipsInTree.Size = new System.Drawing.Size(256, 17);
            this.chkShowDescriptionTooltipsInTree.TabIndex = 3;
            this.chkShowDescriptionTooltipsInTree.Text = "Show description tooltips in connection tree";
            this.chkShowDescriptionTooltipsInTree.UseVisualStyleBackColor = true;
            // 
            // chkShowSystemTrayIcon
            // 
            this.chkShowSystemTrayIcon._mice = MrngCheckBox.MouseState.OUT;
            this.chkShowSystemTrayIcon.AutoSize = true;
            this.chkShowSystemTrayIcon.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowSystemTrayIcon.Location = new System.Drawing.Point(3, 173);
            this.chkShowSystemTrayIcon.Name = "chkShowSystemTrayIcon";
            this.chkShowSystemTrayIcon.Size = new System.Drawing.Size(178, 17);
            this.chkShowSystemTrayIcon.TabIndex = 5;
            this.chkShowSystemTrayIcon.Text = "Always show System Tray Icon";
            this.chkShowSystemTrayIcon.UseVisualStyleBackColor = true;
            // 
            // chkMinimizeToSystemTray
            // 
            this.chkMinimizeToSystemTray._mice = MrngCheckBox.MouseState.OUT;
            this.chkMinimizeToSystemTray.AutoSize = true;
            this.chkMinimizeToSystemTray.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMinimizeToSystemTray.Location = new System.Drawing.Point(3, 196);
            this.chkMinimizeToSystemTray.Name = "chkMinimizeToSystemTray";
            this.chkMinimizeToSystemTray.Size = new System.Drawing.Size(147, 17);
            this.chkMinimizeToSystemTray.TabIndex = 6;
            this.chkMinimizeToSystemTray.Text = "Minimize to System Tray";
            this.chkMinimizeToSystemTray.UseVisualStyleBackColor = true;
            // 
            // chkCloseToSystemTray
            // 
            this.chkCloseToSystemTray._mice = MrngCheckBox.MouseState.OUT;
            this.chkCloseToSystemTray.AutoSize = true;
            this.chkCloseToSystemTray.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCloseToSystemTray.Location = new System.Drawing.Point(3, 219);
            this.chkCloseToSystemTray.Name = "chkCloseToSystemTray";
            this.chkCloseToSystemTray.Size = new System.Drawing.Size(129, 17);
            this.chkCloseToSystemTray.TabIndex = 7;
            this.chkCloseToSystemTray.Text = "Close to System Tray";
            this.chkCloseToSystemTray.UseVisualStyleBackColor = true;
            // 
            // AppearancePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.chkCloseToSystemTray);
            this.Controls.Add(this.lblLanguageRestartRequired);
            this.Controls.Add(this.cboLanguage);
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.chkShowFullConnectionsFilePathInTitle);
            this.Controls.Add(this.chkShowDescriptionTooltipsInTree);
            this.Controls.Add(this.chkShowSystemTrayIcon);
            this.Controls.Add(this.chkMinimizeToSystemTray);
            this.Name = "AppearancePage";
            this.Size = new System.Drawing.Size(610, 490);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		internal Controls.MrngLabel lblLanguageRestartRequired;
		internal MrngComboBox cboLanguage;
		internal Controls.MrngLabel lblLanguage;
		internal MrngCheckBox chkShowFullConnectionsFilePathInTitle;
		internal MrngCheckBox chkShowDescriptionTooltipsInTree;
		internal MrngCheckBox chkShowSystemTrayIcon;
		internal MrngCheckBox chkMinimizeToSystemTray;
        internal MrngCheckBox chkCloseToSystemTray;
    }
}
