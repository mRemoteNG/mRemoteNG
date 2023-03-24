

using mRemoteNG.UI.Controls;

namespace mRemoteNG.UI.Forms.OptionsPages
{

    public sealed partial class AppearancePage : OptionsPage
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
            lblLanguageRestartRequired = new MrngLabel();
            cboLanguage = new MrngComboBox();
            lblLanguage = new MrngLabel();
            chkShowFullConnectionsFilePathInTitle = new MrngCheckBox();
            chkShowDescriptionTooltipsInTree = new MrngCheckBox();
            chkShowSystemTrayIcon = new MrngCheckBox();
            chkMinimizeToSystemTray = new MrngCheckBox();
            chkCloseToSystemTray = new MrngCheckBox();
            SuspendLayout();
            // 
            // lblLanguageRestartRequired
            // 
            lblLanguageRestartRequired.AutoSize = true;
            lblLanguageRestartRequired.Location = new System.Drawing.Point(8, 61);
            lblLanguageRestartRequired.Name = "lblLanguageRestartRequired";
            lblLanguageRestartRequired.Size = new System.Drawing.Size(414, 13);
            lblLanguageRestartRequired.TabIndex = 2;
            lblLanguageRestartRequired.Text = "mRemoteNG must be restarted before changes to the language will take effect.";
            // 
            // cboLanguage
            // 
            cboLanguage._mice = MrngComboBox.MouseState.HOVER;
            cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLanguage.FormattingEnabled = true;
            cboLanguage.Location = new System.Drawing.Point(8, 29);
            cboLanguage.Name = "cboLanguage";
            cboLanguage.Size = new System.Drawing.Size(304, 21);
            cboLanguage.Sorted = true;
            cboLanguage.TabIndex = 1;
            // 
            // lblLanguage
            // 
            lblLanguage.AutoSize = true;
            lblLanguage.Location = new System.Drawing.Point(8, 8);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new System.Drawing.Size(58, 13);
            lblLanguage.TabIndex = 0;
            lblLanguage.Text = "Language";
            // 
            // chkShowFullConnectionsFilePathInTitle
            // 
            chkShowFullConnectionsFilePathInTitle._mice = MrngCheckBox.MouseState.OUT;
            chkShowFullConnectionsFilePathInTitle.AutoSize = true;
            chkShowFullConnectionsFilePathInTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowFullConnectionsFilePathInTitle.Location = new System.Drawing.Point(8, 132);
            chkShowFullConnectionsFilePathInTitle.Name = "chkShowFullConnectionsFilePathInTitle";
            chkShowFullConnectionsFilePathInTitle.Size = new System.Drawing.Size(268, 17);
            chkShowFullConnectionsFilePathInTitle.TabIndex = 4;
            chkShowFullConnectionsFilePathInTitle.Text = "Show full connections file path in window title";
            chkShowFullConnectionsFilePathInTitle.UseVisualStyleBackColor = true;
            // 
            // chkShowDescriptionTooltipsInTree
            // 
            chkShowDescriptionTooltipsInTree._mice = MrngCheckBox.MouseState.OUT;
            chkShowDescriptionTooltipsInTree.AutoSize = true;
            chkShowDescriptionTooltipsInTree.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowDescriptionTooltipsInTree.Location = new System.Drawing.Point(8, 109);
            chkShowDescriptionTooltipsInTree.Name = "chkShowDescriptionTooltipsInTree";
            chkShowDescriptionTooltipsInTree.Size = new System.Drawing.Size(256, 17);
            chkShowDescriptionTooltipsInTree.TabIndex = 3;
            chkShowDescriptionTooltipsInTree.Text = "Show description tooltips in connection tree";
            chkShowDescriptionTooltipsInTree.UseVisualStyleBackColor = true;
            // 
            // chkShowSystemTrayIcon
            // 
            chkShowSystemTrayIcon._mice = MrngCheckBox.MouseState.OUT;
            chkShowSystemTrayIcon.AutoSize = true;
            chkShowSystemTrayIcon.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkShowSystemTrayIcon.Location = new System.Drawing.Point(8, 178);
            chkShowSystemTrayIcon.Name = "chkShowSystemTrayIcon";
            chkShowSystemTrayIcon.Size = new System.Drawing.Size(178, 17);
            chkShowSystemTrayIcon.TabIndex = 5;
            chkShowSystemTrayIcon.Text = "Always show System Tray Icon";
            chkShowSystemTrayIcon.UseVisualStyleBackColor = true;
            // 
            // chkMinimizeToSystemTray
            // 
            chkMinimizeToSystemTray._mice = MrngCheckBox.MouseState.OUT;
            chkMinimizeToSystemTray.AutoSize = true;
            chkMinimizeToSystemTray.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkMinimizeToSystemTray.Location = new System.Drawing.Point(8, 201);
            chkMinimizeToSystemTray.Name = "chkMinimizeToSystemTray";
            chkMinimizeToSystemTray.Size = new System.Drawing.Size(147, 17);
            chkMinimizeToSystemTray.TabIndex = 6;
            chkMinimizeToSystemTray.Text = "Minimize to System Tray";
            chkMinimizeToSystemTray.UseVisualStyleBackColor = true;
            // 
            // chkCloseToSystemTray
            // 
            chkCloseToSystemTray._mice = MrngCheckBox.MouseState.OUT;
            chkCloseToSystemTray.AutoSize = true;
            chkCloseToSystemTray.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            chkCloseToSystemTray.Location = new System.Drawing.Point(8, 224);
            chkCloseToSystemTray.Name = "chkCloseToSystemTray";
            chkCloseToSystemTray.Size = new System.Drawing.Size(129, 17);
            chkCloseToSystemTray.TabIndex = 7;
            chkCloseToSystemTray.Text = "Close to System Tray";
            chkCloseToSystemTray.UseVisualStyleBackColor = true;
            // 
            // AppearancePage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            Controls.Add(chkCloseToSystemTray);
            Controls.Add(lblLanguageRestartRequired);
            Controls.Add(cboLanguage);
            Controls.Add(lblLanguage);
            Controls.Add(chkShowFullConnectionsFilePathInTitle);
            Controls.Add(chkShowDescriptionTooltipsInTree);
            Controls.Add(chkShowSystemTrayIcon);
            Controls.Add(chkMinimizeToSystemTray);
            Name = "AppearancePage";
            Size = new System.Drawing.Size(610, 490);
            ResumeLayout(false);
            PerformLayout();
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
